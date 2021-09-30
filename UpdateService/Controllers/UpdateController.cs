using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using UpdateService.Models;
using UpdateService.ViewModel;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.ModelBinding;
using Microsoft.Extensions.Configuration;
using UpdateService.Repository;

// For more information on enabling MVC for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace CreateService.Controllers
{
    [ApiController]
    [Route("api/[controller]/[action]")]
    public class UpdateController : ControllerBase
    {
        private readonly IUserRepo _userRepo;

        public UpdateController(IUserRepo userRepo)
        {
            _userRepo = userRepo;
        }

        [HttpPost]
        public async Task<ActionResult> UpdateStudent([FromBody] UpdateViewModel model)
        {
            try
            {
                if (ModelState.IsValid)
                {
                    var user = await _userRepo.GetUserByEmail(model.Email);

                    
                    user.Firstname = model.Firstname;
                    user.Lastname = model.Lastname;
                    user.Email = model.Email;
                    user.PhoneNumber = model.PhoneNumber;
                    user.Country = model.Country;
                    user.State = model.State;

                    IdentityResult result = await _userRepo.UpdateUser(user);
                    if (result.Succeeded)
                    {
                        return StatusCode(StatusCodes.Status200OK, new { success = true });
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
    }
}
