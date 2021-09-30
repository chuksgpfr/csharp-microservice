using System;
using System.Collections.Generic;
using System.Globalization;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Security.Claims;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using CreateService.Models;
using CreateService.ViewModel;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.ModelBinding;
using Microsoft.Extensions.Configuration;
using Microsoft.IdentityModel.Tokens;

// For more information on enabling MVC for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace CreateService.Controllers
{
    [ApiController]
    [Route("api/[controller]/[action]")]
    public class CreateController : ControllerBase
    {
        private readonly UserManager<ApplicationUser> _userManager;

        public IConfiguration _configuration { get; }

        public CreateController(UserManager<ApplicationUser> userManager, IConfiguration configuration)
        {
            _userManager = userManager;
            _configuration = configuration;
        }

        [HttpGet]
        public ActionResult Test()
        {
            return StatusCode(StatusCodes.Status200OK, new { message = "Good" });
        }

        [HttpPost]
        public async Task<ActionResult> CreateStudent([FromBody] SignupViewModel model)
        {
            try
            {
                if (ModelState.IsValid)
                {

                    ApplicationUser newUser = new ApplicationUser()
                    {
                        Id = Guid.NewGuid().ToString(),
                        Firstname = model.Firstname,
                        Lastname = model.Lastname,
                        Email = model.Email,
                        PhoneNumber = model.PhoneNumber,
                        Country = model.Country,
                        State = model.State,
                        UserName = model.Email,
                    };

                    IdentityResult result = await _userManager.CreateAsync(newUser, model.Password);
                    if (result.Succeeded)
                    {
                        await _userManager.AddToRoleAsync(newUser, "Student");
                        return StatusCode(StatusCodes.Status201Created, new { success = true });
                    }
                    else
                        return StatusCode(StatusCodes.Status400BadRequest, new { success = false, errors = result.Errors });
                }
                else
                {
                    IEnumerable<ModelError> allErrors = ModelState.Values.SelectMany(v => v.Errors);
                    return StatusCode(StatusCodes.Status400BadRequest, new { success = false, errors = allErrors });
                }
            }
            catch (Exception ex)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, ex.Message);
            }
        }

        [HttpPost]
        public async Task<IActionResult> Login([FromBody] LoginViewModel model)
        {
            try
            {
                //checking if the model requirement specified in SignUpViewModel is met
                if (ModelState.IsValid)
                {
                    ApplicationUser user = null;

                    //validating if this is an email
                    bool isEmailValid = IsValidEmail(model.Email);

                    if (isEmailValid)
                    {
                        //fetching user from DB
                        user = await _userManager.FindByEmailAsync(model.Email);
                    }
                    else
                    {
                        return StatusCode(StatusCodes.Status400BadRequest, new { success = false, message = "Wrong email format" });
                    }

                    //checking if user is in DB and password matches
                    if (user != null && await _userManager.CheckPasswordAsync(user, model.Password))
                    {

                        //get user role
                        var roles = await _userManager.GetRolesAsync(user);

                        //creating a new claim for jwt token
                        var claims = new[]
                        {
                            new Claim(JwtRegisteredClaimNames.Jti, Guid.NewGuid().ToString()),
                            new Claim(JwtRegisteredClaimNames.UniqueName, user.UserName),
                            new Claim("UserID", user.Id.ToString()),
                            new Claim("Role", roles[0]),
                            new Claim(JwtRegisteredClaimNames.Email, user.Email)
                        };

                        //creating a new key for jwt token
                        var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_configuration["Jwt:SecretKey"]));
                        var credentials = new SigningCredentials(key, SecurityAlgorithms.HmacSha256);

                        //creating the jwt token
                        var token = new JwtSecurityToken(
                            issuer: _configuration["Jwt:Issuer"],
                            audience: _configuration["Jwt:audience"],
                            claims: claims,
                            expires: DateTime.Now.AddDays(50),
                            signingCredentials: credentials
                        );

                        return StatusCode(StatusCodes.Status202Accepted, new { success = true, token = new JwtSecurityTokenHandler().WriteToken(token), user = user });

                    }
                    else
                    {
                        return StatusCode(StatusCodes.Status400BadRequest, new { success = false, message = "Wrong email & password combination" });
                    }
                }
                else
                {
                    return StatusCode(StatusCodes.Status400BadRequest, new { success = false, message = "Server Error" });
                }
            }
            catch (Exception ex)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, new { success = false, message = ex });
            }
        }


        public static bool IsValidEmail(string email)
        {
            if (string.IsNullOrWhiteSpace(email))
                return false;

            try
            {
                // Normalize the domain
                email = Regex.Replace(email, @"(@)(.+)$", DomainMapper,
                                      RegexOptions.None, TimeSpan.FromMilliseconds(200));

                // Examines the domain part of the email and normalizes it.
                string DomainMapper(Match match)
                {
                    // Use IdnMapping class to convert Unicode domain names.
                    var idn = new IdnMapping();

                    // Pull out and process domain name (throws ArgumentException on invalid)
                    string domainName = idn.GetAscii(match.Groups[2].Value);

                    return match.Groups[1].Value + domainName;
                }
            }
            catch (RegexMatchTimeoutException e)
            {
                return false;
            }
            catch (ArgumentException e)
            {
                return false;
            }

            try
            {
                return Regex.IsMatch(email,
                    @"^[^@\s]+@[^@\s]+\.[^@\s]+$",
                    RegexOptions.IgnoreCase, TimeSpan.FromMilliseconds(250));
            }
            catch (RegexMatchTimeoutException)
            {
                return false;
            }
        }

    }
}
