using Microsoft.AspNetCore.Mvc;
using FameMatchServer.Models;
using FameMatchServer.DTO;
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
    [HttpPost("login")]
    public IActionResult Login([FromBody] DTO.Loginfo loginDto)
    {
        try
        {
            if (loginDto is DTO.Casted)
            {
                HttpContext.Session.Clear(); //Logout any previous login attempt

                //Get model user class from DB with matching email. 
                Models.Casted? modelsCasted = context.GetCasted(loginDto.UserEmail);

                //Check if user exist for this email and if password match, if not return Access Denied (Error 403) 
                if (modelsCasted == null || modelsCasted.User.UserPassword != loginDto.UserPassword)
                {
                    return Unauthorized();
                }

                //Login suceed! now mark login in session memory!
                HttpContext.Session.SetString("loggedInCasted", modelsCasted.User.UserEmail);

                DTO.Casted dtoCasted = new DTO.Casted(modelsCasted);
                // dtoUser.ProfileImagePath = GetProfileImageVirtualPath(dtoUser.UserId);
                return Ok(dtoCasted);
            }
            else if(loginDto is  DTO.Castor) 
            {
                HttpContext.Session.Clear(); //Logout any previous login attempt

                //Get model user class from DB with matching email. 
                Models.Castor? modelsCastor = context.GetCastor(loginDto.UserEmail);

                //Check if user exist for this email and if password match, if not return Access Denied (Error 403) 
                if (modelsCastor == null || modelsCastor.User.UserPassword != loginDto.UserPassword)
                {
                    return Unauthorized();
                }

                //Login suceed! now mark login in session memory!
                HttpContext.Session.SetString("loggedInCastor", modelsCastor.User.UserEmail);

                DTO.Castor dtoCastor = new DTO.Castor(modelsCastor);
                // dtoUser.ProfileImagePath = GetProfileImageVirtualPath(dtoUser.UserId);
                return Ok(dtoCastor);
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
    [HttpPost("register")]
    public IActionResult Register([FromBody] DTO.User userDto)
    {
        try
        {
            HttpContext.Session.Clear(); //Logout any previous login attempt

            //Create model user class
            Models.User modelsUser = userDto.GetModel();

            context.Users.Add(modelsUser);
            context.SaveChanges();

            //User was added!
            DTO.User dtoUser = new DTO.User(modelsUser);
            //////dtoUser.ProfileImagePath = GetProfileImageVirtualPath(dtoUser.Id);
            return Ok(dtoUser);
        }
        catch (Exception ex)
        {
            return BadRequest(ex.Message);
        }

    }




}

