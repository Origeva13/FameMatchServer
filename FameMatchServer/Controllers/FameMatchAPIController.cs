using Microsoft.AspNetCore.Mvc;
using FameMatchServer.Models;
using FameMatchServer.DTO;
using Microsoft.EntityFrameworkCore;
using Microsoft.Data.SqlClient;
using System.Data;
using Microsoft.EntityFrameworkCore.Metadata.Internal;
namespace FameMatchServer.Controllers;

[Route("api")]
[ApiController]
public class FameMatchAPIController : ControllerBase
{
    //a variable to hold a reference to the db context!
    private FameMatchDbContext context;
    //a variable that hold a reference to web hosting interface (that provide information like the folder on which the server runs etc...)
    private IWebHostEnvironment webHostEnvironment;
    //Use dependency injection to get the db context and web host into the constructor
    public FameMatchAPIController(FameMatchDbContext context, IWebHostEnvironment env)
    {
        this.context = context;
        this.webHostEnvironment = env;

    }

    [HttpGet]
    [Route("TestServer")]
    public ActionResult<string> TestServer()
    {
        return Ok("Server Responded Successfully");
    }

    // This helper method retrieves a user by their email address.
    // It checks the Users table for a match, and then determines whether the user is a Casted or a Castor.
    // Returns the appropriate DTO object (Casted or Castor) or null if the user is not found.
    private DTO.User? GetUser(string email)
    {
        DTO.User? theUser = null;

        // Attempt to find the user in the database by email
        Models.User? user = context.Users
            .Where(u => u.UserEmail == email)
            .FirstOrDefault();

        if (user == null)
        {
            return null; // User with given email not found
        }

        int id = user.UserId;

        // Check if the user is a Casted
        Models.Casted? userCasted = context.Casteds
            .Where(c => c.UserId == id)
            .Include(c => c.User)
            .FirstOrDefault();

        if (userCasted != null)
        {
            theUser = new DTO.Casted(userCasted);
        }
        else
        {
            // If not Casted, check if user is a Castor
            Models.Castor? userCastor = context.Castors
                .Where(c => c.UserId == id)
                .FirstOrDefault();

            if (userCastor != null)
            {
                theUser = new DTO.Castor(userCastor);
            }
        }

        return theUser;
    }

    // This endpoint handles user login for both Casted and Castor users.
    // It verifies the email and password provided in the request body,
    // determines the type of the user (Casted or Castor),
    // and sets a session variable to indicate a successful login.
    // Returns:
    // - 200 OK with the user data if login is successful
    // - 401 Unauthorized if credentials are incorrect or user type is invalid
    // - 400 BadRequest if an exception occurs
    [HttpPost("login")]
    public IActionResult Login([FromBody] DTO.Loginfo loginDto)
    {
        try
        {
            HttpContext.Session.Clear(); // Logout any previous login attempt

            // Get DTO user (either Casted or Castor) by email
            DTO.User? dtoUser = GetUser(loginDto.UserEmail);

            if (dtoUser is DTO.Casted)
            {
                // Check credentials for Casted user
                if (dtoUser == null || dtoUser.UserPassword != loginDto.UserPassword)
                {
                    return Unauthorized(); // Email or password mismatch
                }

                // Login succeeded, store user email in session
                HttpContext.Session.SetString("loggedInCasted", dtoUser.UserEmail);

                return Ok(dtoUser);
            }
            else if (dtoUser is DTO.Castor)
            {
                // Check credentials for Castor user
                if (dtoUser == null || dtoUser.UserPassword != loginDto.UserPassword)
                {
                    return Unauthorized(); // Email or password mismatch
                }

                // Login succeeded, store user email in session
                HttpContext.Session.SetString("loggedInCastor", dtoUser.UserEmail);

                return Ok(dtoUser);
            }
            else
            {
                // User is neither Casted nor Castor (unexpected case)
                return Unauthorized();
            }
        }
        catch (Exception ex)
        {
            return BadRequest(ex.Message);
        }
    }

    // This endpoint handles the registration of a new Casted user.
    // It receives a DTO object from the client, converts it into a model object,
    // saves it to the database, and returns the newly created user data.
    // Also clears any previous session to ensure a clean registration flow.
    // Returns:
    // - 200 OK with the registered Casted user
    // - 400 BadRequest with error message if registration fails
    [HttpPost("registerCasted")]
    public IActionResult RegisterCasted([FromBody] DTO.Casted castedDto)
    {
        try
        {
            HttpContext.Session.Clear(); // Logout any previous login attempt

            // Convert DTO to model to prepare for DB insertion
            Models.Casted modelsCasted = castedDto.GetModel();

            context.Casteds.Add(modelsCasted); // Add the new user to the DB
            context.SaveChanges(); // Save changes to the DB

            // User was added successfully
            DTO.Casted dtoCasted = new DTO.Casted(modelsCasted);
            return Ok(dtoCasted); // Return the registered user
        }
        catch (Exception ex)
        {
            return BadRequest(ex.Message); // Return error message if something goes wrong
        }
    }

    // This endpoint registers a new Castor user (e.g., someone who posts auditions).
    // It accepts a DTO.Castor object, converts it to a database model,
    // saves the new user to the database, and returns the created user data.
    // Clears any existing session to ensure a clean state before registration.
    // Returns:
    // - 200 OK with the newly registered Castor user
    // - 400 BadRequest with an error message if registration fails
    [HttpPost("registerCastor")]
    public IActionResult RegisterCastor([FromBody] DTO.Castor castordto)
    {
        try
        {
            HttpContext.Session.Clear(); // Logout any previous login attempt

            // Convert DTO to model object for database insertion
            Models.Castor modelsCastor = castordto.GetModel();

            context.Castors.Add(modelsCastor); // Add the Castor user to the DB
            context.SaveChanges(); // Commit the changes

            // User was successfully added to the database
            DTO.Castor dtoCastor = new DTO.Castor(modelsCastor);
            return Ok(dtoCastor); // Return the registered Castor user
        }
        catch (Exception ex)
        {
            return BadRequest(ex.Message); // Return error message if something goes wrong
        }
    }


    // This endpoint allows an authenticated Castor user to update their profile information.
    // It ensures the user is logged in and that the update request matches the currently logged-in user.
    // The method updates both the Castor entity and its associated User entity in the database.
    // Returns:
    // - 200 OK if the update is successful
    // - 401 Unauthorized if the user is not logged in or attempting to update another user
    // - 400 BadRequest with an error message if an exception occurs
    [HttpPost("UpdateCastor")]
    public IActionResult UpdateCastor([FromBody] DTO.Castor userDto)
    {
        try
        {
            // Check if a Castor user is currently logged in
            string? userEmail = HttpContext.Session.GetString("loggedInCastor");
            if (string.IsNullOrEmpty(userEmail))
            {
                return Unauthorized("User is not logged in");
            }

            // Retrieve the logged-in user's model from the database
            Models.Castor? theUser = context.GetCastor(userEmail);

            // Clear tracking to avoid Entity Framework conflicts during update
            context.ChangeTracker.Clear();

            // Ensure the user being updated is the same as the one logged in
            if (theUser == null || (userDto.UserId != theUser.UserId))
            {
                return Unauthorized("Failed to update user");
            }

            // Convert incoming DTO to model and mark as modified
            Models.Castor appUser = userDto.GetModel();
            Models.User user = appUser.User;

            context.Entry(appUser).State = EntityState.Modified;
            context.Entry(user).State = EntityState.Modified;

            // Commit the changes to the database
            context.SaveChanges();

            // Profile update successful
            return Ok();
        }
        catch (Exception ex)
        {
            return BadRequest(ex.Message); // Return error if something goes wrong
        }
    }
    // This endpoint allows a logged-in Casted user to update their profile information.
    // It validates the session to ensure the user is authenticated,
    // confirms the user making the request is the same as the one logged in,
    // and updates both the Casted and related User entities in the database.
    // Returns:
    // - 200 OK if the update is successful
    // - 401 Unauthorized if not logged in or trying to update another user's profile
    // - 400 BadRequest with an error message if an exception occurs
    [HttpPost("UpdateCasted")]
    public IActionResult UpdateCasted([FromBody] DTO.Casted userDto)
    {
        try
        {
            // Check if a Casted user is currently logged in
            string? userEmail = HttpContext.Session.GetString("loggedInCasted");
            if (string.IsNullOrEmpty(userEmail))
            {
                return Unauthorized("User is not logged in");
            }

            // Retrieve the Casted model from the database using the logged-in user's email
            Models.Casted? theUser = context.GetCasted(userEmail);

            // Clear tracking to avoid Entity Framework conflicts during update
            context.ChangeTracker.Clear();

            // Ensure the user being updated is the one logged in
            if (theUser == null || (userDto.UserId != theUser.UserId))
            {
                return Unauthorized("Failed to update user");
            }

            // Convert DTO to model and mark both Casted and User entities as modified
            Models.Casted appUser = userDto.GetModel();
            Models.User user = appUser.User;

            context.Entry(appUser).State = EntityState.Modified;
            context.Entry(user).State = EntityState.Modified;

            // Save changes to the database
            context.SaveChanges();

            // Update successful
            return Ok();
        }
        catch (Exception ex)
        {
            return BadRequest(ex.Message); // Return error details if something goes wrong
        }
    }

    // This endpoint retrieves a list of all registered Casted users from the database.
    // Each Casted is converted from the model to a DTO and returned to the client.
    // Returns:
    // - 200 OK with a list of all Casted users
    // - 400 BadRequest with an error message if an exception occurs
    [HttpGet("GetAllCasteds")]
    public IActionResult GetAllCasteds()
    {
        try
        {
            // Create a list to hold the DTO representation of each Casted user
            List<DTO.Casted> castedList = new List<DTO.Casted>();

            // Get all Casted entities from the DB and include their associated User details
            List<Models.Casted> modelCasteds = context.Casteds.Include(c => c.User).ToList();

            // Convert each model to its corresponding DTO
            foreach (Models.Casted casted in modelCasteds)
            {
                DTO.Casted dtoCasted = new DTO.Casted(casted);
                castedList.Add(dtoCasted);
            }

            // Return the list of Casted DTOs
            return Ok(castedList);
        }
        catch (Exception ex)
        {
            return BadRequest(ex.Message); // Return error message if something goes wrong
        }
    }

    // This endpoint retrieves a list of all registered Castor users from the database.
    // Each Castor is loaded along with its associated User data and converted into a DTO.
    // Returns:
    // - 200 OK with a list of all Castor users
    // - 400 BadRequest with an error message if an exception occurs
    [HttpGet("GetAllCastors")]
    public IActionResult GetAllCastors()
    {
        try
        {
            // Create a list to store the Castor DTOs
            List<DTO.Castor> castorList = new List<DTO.Castor>();

            // Retrieve all Castor entities from the database with their associated User info
            List<Models.Castor> modelCastors = context.Castors.Include(c => c.User).ToList();

            // Convert each Castor model to a DTO and add to the list
            foreach (Models.Castor castor in modelCastors)
            {
                DTO.Castor dtoCastor = new DTO.Castor(castor);
                castorList.Add(dtoCastor);
            }

            // Return the list of Castor DTOs
            return Ok(castorList);
        }
        catch (Exception ex)
        {
            return BadRequest(ex.Message); // Return error message if something goes wrong
        }
    }
    // This endpoint retrieves a list of all users from the database (both Castors and Casteds).
    // It converts each User entity to a User DTO and returns the full list.
    // Returns:
    // - 200 OK with the list of users
    // - 400 BadRequest with an error message if an exception occurs
    [HttpGet("GetUsers")]
    public IActionResult GetUsers()
    {
        try
        {
            // Retrieve all User entities from the database
            List<Models.User> list = context.GetUsers();

            // Convert each User model to a DTO
            List<DTO.User> users = new List<DTO.User>();
            foreach (Models.User u in list)
            {
                DTO.User user = new DTO.User(u);
                users.Add(user);
            }

            // Return the list of User DTOs
            return Ok(users);
        }
        catch (Exception ex)
        {
            return BadRequest(ex.Message); // Return error message if something goes wrong
        }
    }

    // This endpoint blocks or unblocks a user based on the provided User DTO.
    // The `IsBlocked` property of the user is updated in the database.
    // Returns:
    // - 200 OK if the operation succeeds
    // - 400 BadRequest with an error message if an exception occurs
    [HttpPost("Block")]
    public IActionResult Block([FromBody] DTO.User u)
    {
        try
        {
            // Retrieve the user model from the database based on UserId
            Models.User user = context.GetUser1(u.UserId);

            // Update the IsBlocked status based on the provided DTO
            user.IsBlocked = u.IsBlocked;

            // Mark the user entity as modified so the changes will be saved
            context.Entry(user).State = EntityState.Modified;

            // Save the changes to the database
            context.SaveChanges();

            // Return a successful response
            return Ok();
        }
        catch (Exception ex)
        {
            // Return error if something goes wrong
            return BadRequest(ex.Message);
        }
    }

    // This endpoint adds a new audition to the database.
    // It takes an Audition DTO as input, maps it to the corresponding model, 
    // and saves it to the database.
    // Returns:
    // - 200 OK with the newly created Audition DTO if the operation succeeds
    // - 400 BadRequest with an error message if an exception occurs
    [HttpPost("AddAudition")]
    public IActionResult AddAudition([FromBody] DTO.Audition a)
    {
        try
        {
            // Convert the incoming DTO to a model instance
            Models.Audition audition = a.GetModel();

            // Add the new audition to the database
            context.Auditions.Add(audition);

            // Save changes to persist the new audition
            context.SaveChanges();

            // Return the newly created audition wrapped in a DTO
            DTO.Audition newAudition = new DTO.Audition(audition);
            return Ok(newAudition);
        }
        catch (Exception ex)
        {
            // Return an error message if something goes wrong
            return BadRequest(ex.Message);
        }
    }

    // This endpoint retrieves all auditions from the database.
    // It fetches all audition records from the database, maps them to their respective DTOs, 
    // and returns the list of auditions as a response.
    // Returns:
    // - 200 OK with the list of all Audition DTOs if the operation succeeds
    // - 400 BadRequest with an error message if an exception occurs
    [HttpGet("GetAllAuditions")]
    public IActionResult GetAllAuditions()
    {
        try
        {
            // Initialize a list to hold the DTO representations of auditions
            List<DTO.Audition> auditionList = new List<DTO.Audition>();

            // Fetch all auditions from the database
            List<Models.Audition> modelAuditions = context.Auditions.ToList();

            // Convert each model audition to a DTO and add it to the list
            foreach (Models.Audition audition in modelAuditions)
            {
                DTO.Audition dtoAudition = new DTO.Audition(audition);
                auditionList.Add(dtoAudition);
            }

            // Return the list of DTO auditions
            return Ok(auditionList);
        }
        catch (Exception ex)
        {
            // Return a BadRequest response if an error occurs
            return BadRequest(ex.Message);
        }
    }

    // This endpoint retrieves all email addresses from the database.
    // It fetches all email addresses from the database, processes them as needed, 
    // and returns them as a list of strings.
    // Returns:
    // - 200 OK with the list of all email addresses if the operation succeeds
    // - 400 BadRequest with an error message if an exception occurs
    [HttpGet("GetAllEmails")]
    public IActionResult GetAllEmails()
    {
        try
        {
            // Get all email addresses from the database
            List<string> list = context.GetAllEmails();

            // Initialize a list to hold the emails
            List<string> allEmails = new List<string>();

            // Loop through each email in the list and add it to the allEmails list
            foreach (string p in list)
            {
                // Create a new string object (optional, as 'p' is already a string)
                string post = new string(p);

                // Add the email to the list
                allEmails.Add(post);
            }

            // Return the list of email addresses as a response
            return Ok(allEmails);
        }
        catch (Exception ex)
        {
            // Return a BadRequest response in case of an exception
            return BadRequest(ex.Message);
        }
    }

    // This endpoint updates the password for a user based on the provided UserDto.
    // It ensures that the user is authenticated and that the logged-in user is the one making the request. 
    // It validates that the correct user is attempting the password change and updates the password in the database.
    // Returns:
    // - 200 OK if the password is successfully updated
    // - 400 BadRequest with an error message if an exception occurs
    // - 401 Unauthorized if the user is not logged in or if the user ID doesn't match the logged-in user
    [HttpPost("UpdateUserPassword")]
    public IActionResult UpdateUserPassword([FromBody] DTO.User userDto)
    {
        try
        {
            // Get the user model from the database using the provided user email
            Models.User? theUser = context.GetUser(userDto.UserEmail);

            // Clear any tracking of entities to avoid double tracking issues
            context.ChangeTracker.Clear();

            // Check if the user exists and if the user IDs match (for validation purposes)
            if (theUser == null || (userDto.UserId != theUser.UserId))
            {
                return Unauthorized("Failed to update user");
            }

            // Convert the DTO user to the model user to update the password
            Models.User appUser = userDto.GetModel();

            // Mark the user entity as modified so that the changes are saved
            context.Entry(appUser).State = EntityState.Modified;

            // Save the changes in the database
            context.SaveChanges();

            // Return success response
            return Ok();
        }
        catch (Exception ex)
        {
            // Return an error response in case of any exception
            return BadRequest(ex.Message);
        }
    }
    // This endpoint updates an existing audition based on the provided AuditionDto.
    // It ensures that the correct audition is being updated by validating that the Audition ID matches
    // the one in the database, and it prevents unauthorized access or updates.
    // Returns:
    // - 200 OK if the audition is successfully updated
    // - 400 BadRequest with an error message if an exception occurs
    // - 401 Unauthorized if the audition is not found or the provided Audition ID doesn't match the existing one
    [HttpPost("UpdateAudition")]
    public IActionResult UpdateAudition([FromBody] DTO.Audition audDto)
    {
        try
        {
            // Retrieve the audition from the database using the provided Audition ID
            Models.Audition? theAud = context.GetAudition(audDto.AudId);

            // Clear any tracking of entities to avoid issues with double tracking
            context.ChangeTracker.Clear();

            // Ensure that the Audition exists and that the provided ID matches the one in the database
            if (theAud == null || (audDto.AudId != theAud.AudId))
            {
                return Unauthorized("Failed to update audition");
            }

            // Convert the DTO audition to the model audition for saving to the database
            Models.Audition appAud = audDto.GetModel();

            // Mark the audition entity as modified so that changes are saved to the database
            context.Entry(appAud).State = EntityState.Modified;

            // Save the changes to the database
            context.SaveChanges();

            // Return success response
            return Ok();
        }
        catch (Exception ex)
        {
            // Return error response if something goes wrong
            return BadRequest(ex.Message);
        }
    }

    // This endpoint retrieves a user by their email address.
    // It checks if the email exists in the database and returns the user information in a DTO format.
    // Returns:
    // - 200 OK with the user details if found
    // - 400 BadRequest with an error message if an exception occurs
    // - 404 NotFound if the user with the provided email does not exist
    [HttpGet("GetUserByEmail")]
    public IActionResult GetUserByEmail([FromQuery] string email)
    {
        try
        {
            // Retrieve the user by email from the database
            Models.User u = context.GetUserEmail(email);

            // If the user is found, convert it to a DTO object
            DTO.User user = new DTO.User(u);

            // Return the user details as a successful response
            return Ok(user);
        }
        catch (Exception ex)
        {
            // Return an error response if something goes wrong
            return BadRequest(ex.Message);
        }
    }

    // This endpoint is used to accept or approve a Castor.
    // It updates the state of the Castor (and their associated User) in the database.
    // Returns:
    // - 200 OK if the Castor is successfully updated
    // - 400 BadRequest if an error occurs while saving the changes
    [HttpPost("Accept")]
    public IActionResult Accept([FromBody] DTO.Castor c)
    {
        try
        {
            // Create model user class from the DTO object
            Models.Castor castor = c.GetModel();

            // Mark the castor as modified in the context
            context.Entry(castor).State = EntityState.Modified;
            context.Entry(castor.User).State = EntityState.Modified;

            // Save changes to the database
            context.SaveChanges();

            // Return success response
            return Ok();
        }
        catch (Exception ex)
        {
            // Return an error response if something goes wrong
            return BadRequest(ex.Message);
        }
    }

    // This endpoint is used to decline or reject a Castor.
    // It updates the state of the Castor (and their associated User) in the database.
    // Returns:
    // - 200 OK if the Castor is successfully updated
    // - 400 BadRequest if an error occurs while saving the changes
    [HttpPost("Declaine")]
    public IActionResult Declaine([FromBody] DTO.Castor c)
    {
        try
        {
            // Create model user class from the DTO object
            Models.Castor castor = c.GetModel();

            // Mark the castor as modified in the context
            context.Entry(castor).State = EntityState.Modified;
            context.Entry(castor.User).State = EntityState.Modified;

            // Save changes to the database
            context.SaveChanges();

            // Return success response
            return Ok();
        }
        catch (Exception ex)
        {
            // Return an error response if something goes wrong
            return BadRequest(ex.Message);
        }
    }
    // This endpoint retrieves a Castor by the Audition ID.
    // It looks up the Castor associated with the given audition ID and returns the user data.
    // Returns:
    // - 200 OK with the Castor DTO if found
    // - 400 BadRequest if an error occurs while retrieving the user
    [HttpGet("GetUserByAudition")]
    public IActionResult GetUserByAudition([FromQuery] int id)
    {
        try
        {
            // Retrieve the Castor associated with the audition ID
            Models.Castor u = context.GetUserByAudition(id);

            // Convert the Castor model to a DTO
            DTO.Castor user = new DTO.Castor(u);

            // Return the Castor DTO
            return Ok(user);
        }
        catch (Exception ex)
        {
            // Return an error response if something goes wrong
            return BadRequest(ex.Message);
        }
    }

    // This endpoint retrieves all auditions for a specific user based on their user ID.
    // It filters the list of auditions to only include those associated with the given user ID.
    // Returns:
    // - 200 OK with a list of Audition DTOs for the specified user
    // - 400 BadRequest if an error occurs while retrieving the auditions
    [HttpGet("GetUserAuditions")]
    public IActionResult GetUserAuditions([FromQuery] int id)
    {
        try
        {
            // Retrieve all auditions from the database
            List<Models.Audition> list = context.GetAllAuditions();

            // Initialize a list to store the user's auditions
            List<DTO.Audition> UserAudition = new List<DTO.Audition>();

            // Loop through all auditions and filter by user ID
            foreach (Models.Audition a in list)
            {
                if (a.UserId == id)
                {
                    // Convert the Audition model to a DTO and add it to the list
                    DTO.Audition audition = new DTO.Audition(a);
                    UserAudition.Add(audition);
                }
            }

            // Return the list of auditions associated with the user
            return Ok(UserAudition);
        }
        catch (Exception ex)
        {
            // Return an error response if something goes wrong
            return BadRequest(ex.Message);
        }
    }


    #region Backup / Restore
    [HttpGet("Backup")]
    public async Task<IActionResult> Backup()
    {
        string path = $"{this.webHostEnvironment.WebRootPath}\\..\\DBScripts\\backup.bak";
        try
        {
            System.IO.File.Delete(path);
        }
        catch 
        {
            
        }
        bool success = await BackupDatabaseAsync(path);
        if (success)
        {
            return Ok("Backup was successful");
        }
        else
        {
            return BadRequest("Backup failed");
        }
    }

    [HttpGet("Restore")]
    public async Task<IActionResult> Restore()
    {
        string path = $"{this.webHostEnvironment.WebRootPath}\\..\\DBScripts\\backup.bak";

        bool success = await RestoreDatabaseAsync(path);
        if (success)
        {
            return Ok("Restore was successful");
        }
        else
        {
            return BadRequest("Restore failed");
        }
    }
    //this function backup the database to a specified path
    private async Task<bool> BackupDatabaseAsync(string path)
    {
        try
        {

            //Get the connection string
            string? connectionString = context.Database.GetConnectionString();
            //Get the database name
            string databaseName = context.Database.GetDbConnection().Database;
            //Build the backup command
            string command = $"BACKUP DATABASE {databaseName} TO DISK = '{path}'";
            //Create a connection to the database
            using (SqlConnection connection = new SqlConnection(connectionString))
            {
                //Open the connection
                await connection.OpenAsync();
                //Create a command
                using (SqlCommand sqlCommand = new SqlCommand(command, connection))
                {
                    //Execute the command
                    await sqlCommand.ExecuteNonQueryAsync();
                }
            }
            return true;
        }
        catch (Exception ex)
        {
            return false;
        }

    }

    //THis function restore the database from a backup in a certain path
    private async Task<bool> RestoreDatabaseAsync(string path)
    {
        try
        {
            //Get the connection string
            string? connectionString = context.Database.GetConnectionString();
            //Get the database name
            string databaseName = context.Database.GetDbConnection().Database;
            //Build the restore command
            string command = $@"
                USE master;
                ALTER DATABASE {databaseName} SET SINGLE_USER WITH ROLLBACK IMMEDIATE;
                RESTORE DATABASE {databaseName} FROM DISK = '{path}' WITH REPLACE;
                ALTER DATABASE {databaseName} SET MULTI_USER;";

            //Create a connection to the database
            using (SqlConnection connection = new SqlConnection(connectionString))
            {
                //Open the connection
                await connection.OpenAsync();
                //Create a command
                using (SqlCommand sqlCommand = new SqlCommand(command, connection))
                {
                    //Execute the command
                    await sqlCommand.ExecuteNonQueryAsync();
                }
            }
            return true;
        }
        catch (Exception ex)
        {
            return false;
        }

    }
    #endregion

}



