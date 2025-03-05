using Microsoft.AspNetCore.Mvc;
using FameMatchServer.Models;
using FameMatchServer.DTO;
using Microsoft.EntityFrameworkCore;
using Microsoft.Data.SqlClient;
using System.Data;
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

    private DTO.User? GetUser(string email)
    {
        DTO.User? theUser = null;
        Models.User? user = context.Users.
            Where(u => u.UserEmail == email).FirstOrDefault();
        if (user == null)
        {
            return null;
        }

        int id = user.UserId;

        Models.Casted? userCasted = context.Casteds.Where(c => c.UserId == id).Include(c => c.User).FirstOrDefault();
        if (userCasted != null)
        { 
            theUser = new DTO.Casted(userCasted);
        }
        else 
        {
            Models.Castor? userCastor = context.Castors.Where(c => c.UserId == id).FirstOrDefault();
            if (userCastor != null) 
                theUser = new DTO.Castor(userCastor);
        }

        return theUser;
    }
    [HttpPost("login")]
    public IActionResult Login([FromBody] DTO.Loginfo loginDto)
    {
        try
        {
            HttpContext.Session.Clear(); //Logout any previous login attempt

            //Get model user class from DB with matching email. 
            DTO.User? dtoUser = GetUser(loginDto.UserEmail);

            if (dtoUser is DTO.Casted)
            {
                //HttpContext.Session.Clear(); //Logout any previous login attempt

                //Get model user class from DB with matching email. 
               

                //Check if user exist for this email and if password match, if not return Access Denied (Error 403) 
                if (dtoUser == null || dtoUser.UserPassword != loginDto.UserPassword)
                {
                    return Unauthorized();
                }

                //Login suceed! now mark login in session memory!
                HttpContext.Session.SetString("loggedInCasted", dtoUser.UserEmail);

                //DTO.Casted dtoCasted = new DTO.Casted(dtoCasted);
                // dtoUser.ProfileImagePath = GetProfileImageVirtualPath(dtoUser.UserId);
                return Ok(dtoUser);
            }

            //Check if user exist for this email and if password match, if not return Access Denied (Error 403) 
          
            else if(dtoUser is  DTO.Castor) 
            {
                //HttpContext.Session.Clear(); //Logout any previous login attempt

                //Get model user class from DB with matching email. 
                //Models.Castor? modelsCastor = context.GetCastor(loginDto.UserEmail);

                //Check if user exist for this email and if password match, if not return Access Denied (Error 403) 
                if (dtoUser == null || dtoUser.UserPassword != loginDto.UserPassword)
                {
                    return Unauthorized();
                }

                //Login suceed! now mark login in session memory!
                HttpContext.Session.SetString("loggedInCastor", dtoUser.UserEmail);

                //DTO.Castor dtoCastor = new DTO.Castor(modelsCastor);
                // dtoUser.ProfileImagePath = GetProfileImageVirtualPath(dtoUser.UserId);
                return Ok(dtoUser);
            }
            else//לשאול את עופר
            {
                return Unauthorized();
            }
        }
        catch (Exception ex)
        {
            return BadRequest(ex.Message);
        }

    }
    [HttpPost("registerCasted")]
    public IActionResult RegisterCasted([FromBody] DTO.Casted castedDto)
    {
        try
        {
            HttpContext.Session.Clear(); //Logout any previous login attempt

            //Create model parent class to be written in the DB
            Models.Casted modelsCasted = castedDto.GetModel();

            context.Casteds.Add(modelsCasted);
            context.SaveChanges();

            //User was added!
            DTO.Casted dtoCasted = new DTO.Casted(modelsCasted);
            return Ok(dtoCasted);
        }
        catch (Exception ex)
        {
            return BadRequest(ex.Message);
        }


    }
    [HttpPost("registerCastor")]
    public IActionResult RegisterCastor([FromBody] DTO.Castor castordto)
    {
        try
        {
            HttpContext.Session.Clear(); //Logout any previous login attempt

            //Create model babysiter class to be written in the DB
            Models.Castor modelsCastor = castordto.GetModel();

            context.Castors.Add(modelsCastor);
            context.SaveChanges();

            //User was added!
            DTO.Castor dtoCastor = new DTO.Castor(modelsCastor);
            return Ok(dtoCastor);
        }
        catch (Exception ex)
        {
            return BadRequest(ex.Message);
        }


    }

    [HttpPost("UpdateCastor")]
    public IActionResult UpdateCastor([FromBody] DTO.Castor userDto)
    {
        try
        {
            //Check if who is logged in
            string? userEmail = HttpContext.Session.GetString("loggedInCastor");
            if (string.IsNullOrEmpty(userEmail))
            {
                return Unauthorized("User is not logged in");
            }

            //Get model user class from DB with matching email. 
            Models.Castor? theUser = context.GetCastor(userEmail);
            //Clear the tracking of all objects to avoid double tracking
            context.ChangeTracker.Clear();

            //Check if the user that is logged in is the same user of the task
            //this situation is ok only if the user is a manager
            if (theUser == null || (userDto.UserId != theUser.UserId))
            {
                return Unauthorized("Failed to update user");
            }

            Models.Castor appUser = userDto.GetModel();

            context.Entry(appUser).State = EntityState.Modified;

            context.SaveChanges();

            //Profile was updated!
            return Ok();
        }
        catch (Exception ex)
        {
            return BadRequest(ex.Message);
        }

    }
    [HttpPost("UpdateCasted")]
    public IActionResult UpdateCasted([FromBody] DTO.Casted userDto)
    {
        try
        {
            //Check if who is logged in
            string? userEmail = HttpContext.Session.GetString("loggedInCasted");
            if (string.IsNullOrEmpty(userEmail))
            {
                return Unauthorized("User is not logged in");
            }

            //Get model user class from DB with matching email. 
            Models.Casted? theUser = context.GetCasted(userEmail);
            //Clear the tracking of all objects to avoid double tracking
            context.ChangeTracker.Clear();

            //Check if the user that is logged in is the same user of the task
            //this situation is ok only if the user is a manager
            if (theUser == null || (userDto.UserId != theUser.UserId))
            {
                return Unauthorized("Failed to update user");
            }

            Models.Casted appUser = userDto.GetModel();

            context.Entry(appUser).State = EntityState.Modified;

            context.SaveChanges();

            //Task was updated!
            return Ok();
        }
        catch (Exception ex)
        {
            return BadRequest(ex.Message);
        }

    }
    [HttpGet("GetAllCasteds")]
    public IActionResult GetAllCasteds()
    {
        try
        {
            List<DTO.Casted> castedList = new List<DTO.Casted>();
            List<Models.Casted> modelCasteds = context.Casteds.Include(c => c.User).ToList();
            foreach (Models.Casted casted in modelCasteds)
            {
                DTO.Casted dtoCasted = new DTO.Casted(casted);
                
                castedList.Add(dtoCasted);
            }
            return Ok(castedList);
        }
        catch (Exception ex)
        {
            return BadRequest(ex.Message);
        }
    }

    [HttpGet("GetAllCastors")]
    public IActionResult GetAllCastors()
    {
        try
        {
            List<DTO.Castor> castorList = new List<DTO.Castor>();
            List<Models.Castor> modelCastors = context.Castors.Include(c => c.User).ToList();
            foreach (Models.Castor castor in modelCastors)
            {
                DTO.Castor dtoCastor = new DTO.Castor(castor);

                castorList.Add(dtoCastor);
            }
            return Ok(castorList);
        }
        catch (Exception ex)
        {
            return BadRequest(ex.Message);
        }
    }
    [HttpGet("GetUsers")]
    public IActionResult GetUsers()
    {
        try
        {
            ////Check if who is logged in
            //string? userEmail = HttpContext.Session.GetString("loggedInUser");
            //if (string.IsNullOrEmpty(userEmail))
            //{
            //    return Unauthorized("User is not logged in");
            //}

            //Read all users

            List<Models.User> list = context.GetUsers();

            List<DTO.User> users = new List<DTO.User>();

            foreach (Models.User u in list)
            {
                DTO.User user = new DTO.User(u);

                users.Add(user);
            }
            return Ok(users);
        }
        catch (Exception ex)
        {
            return BadRequest(ex.Message);
        }

    }
    [HttpPost("Block")]
    public IActionResult Block([FromBody] DTO.User u)
    {
        try
        {
            //Create model user class
            Models.User user = context.GetUser1(u.UserId);
            user.IsBlocked = u.IsBlocked;
            context.Entry(user).State = EntityState.Modified;

            context.SaveChanges();
            return Ok();
        }
        catch (Exception ex)
        {
            return BadRequest(ex.Message);
        }

    }
    [HttpPost("AddAudition")]
    public IActionResult AddAudition([FromBody] DTO.Audition a)
    {
        try
        {
            //Create model user class
            Models.Audition audition = a.GetModel();
            
            context.Auditions.Add(audition);
            context.SaveChanges();
            DTO.Audition newAudition = new DTO.Audition(audition);
            return Ok(newAudition);
        }
        catch (Exception ex)
        {
            return BadRequest(ex.Message);
        }

    }
    [HttpGet("GetAllAuditions")]
    public IActionResult GetAllAuditions()
    {
        try
        {
            List<DTO.Audition> auditionList = new List<DTO.Audition>();
            List<Models.Audition> modelAuditions = context.Auditions.ToList();
            foreach (Models.Audition audition in modelAuditions)
            {
                DTO.Audition dtoAudition = new DTO.Audition(audition);

                auditionList.Add(dtoAudition);
            }
            return Ok(auditionList);
        }
        catch (Exception ex)
        {
            return BadRequest(ex.Message);
        }
    }
    [HttpGet("GetAllEmails")]
    public IActionResult GetAllEmails()
    {
        try
        {
            //Check if who is logged in
            //string? userEmail = HttpContext.Session.GetString("loggedInUser");
            //if (string.IsNullOrEmpty(userEmail))
            //{
            //    return Unauthorized("User is not logged in");
            //}

            //Read all emails

            List<string> list = context.GetAllEmails();

            List<string> allEmails = new List<string>();

            foreach (string p in list)
            {
                string post = new string(p);

                allEmails.Add(post);
            }
            return Ok(allEmails);
        }
        catch (Exception ex)
        {
            return BadRequest(ex.Message);
        }

    }

    [HttpPost("UpdateUserPassword")]
    public IActionResult UpdateUserPassword([FromBody] DTO.User userDto)
    {
        try
        {
            //Check if who is logged in
            //string? userEmail = HttpContext.Session.GetString("loggedInUser");
            //if (string.IsNullOrEmpty(userEmail))
            //{
            //    return Unauthorized("User is not logged in");
            //}

            //Get model user class from DB with matching email. 
            Models.User? theUser = context.GetUser(userDto.UserEmail);
            //Clear the tracking of all objects to avoid double tracking
            context.ChangeTracker.Clear();

            //Check if the user that is logged in is the same user of the task
            //this situation is ok only if the user is a manager
            if (theUser == null || (userDto.UserId != theUser.UserId))
            {
                return Unauthorized("Failed to update user");
            }

            Models.User appUser = userDto.GetModel();

            context.Entry(appUser).State = EntityState.Modified;

            context.SaveChanges();

            //Task was updated!
            return Ok();
        }
        catch (Exception ex)
        {
            return BadRequest(ex.Message);
        }

    }
    [HttpGet("GetUserByEmail")]
    public IActionResult GetUserByEmail([FromQuery] string email)
    {
        try
        {
            Models.User u = context.GetUserEmail(email);
            DTO.User user = new DTO.User(u);
            return Ok(user);
        }
        catch (Exception ex)
        {
            return BadRequest(ex.Message);
        }

    }
    [HttpPost("Accept")]
    public IActionResult Accept([FromBody] DTO.Castor c)
    {
        try
        {
            //Create model user class
            Models.Castor castor = c.GetModel();
            //castor.IsAprooved = c.IsAprooved;
            context.Entry(castor).State = EntityState.Modified;
            context.Entry(castor.User).State = EntityState.Modified;
            context.SaveChanges();
            return Ok();
        }
        catch (Exception ex)
        {
            return BadRequest(ex.Message);
        }

    }
    [HttpPost("Declaine")]
    public IActionResult Declaine([FromBody] DTO.Castor c)
    {
        try
        {
            //Create model user class
            Models.Castor castor = c.GetModel();
            context.Entry(castor).State = EntityState.Modified;
            context.Entry(castor.User).State = EntityState.Modified;

            context.SaveChanges();
            return Ok();
        }
        catch (Exception ex)
        {
            return BadRequest(ex.Message);
        }

    }
    [HttpGet("GetUserByAudition")]
    public IActionResult GetUserByAudition([FromQuery] int id)
    {
        try
        {
            //returns user by user id
            Models.Castor u = context.GetUserByAudition(id);
            DTO.Castor user = new DTO.Castor(u);
            return Ok(user);
        }
        catch (Exception ex)
        {
            return BadRequest(ex.Message);
        }
    }
}



