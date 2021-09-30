using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using DeleteService.Models;
using DeleteService.ViewModel;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.ModelBinding;
using DeleteService.Repository;

// For more information on enabling MVC for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace DeleteService.Controllers
{

    [ApiController]
    [Route("api/[controller]/[action]")]
    public class DeleteController : ControllerBase
    {
        private readonly IUserRepo _userRepo;

        public DeleteController(IUserRepo userRepo)
        {
            _userRepo = userRepo;
        }

        [HttpPost]
        public async Task<ActionResult> DeleteStudent([FromBody] DeleteViewModel deleteViewModel)
        {
            try
            {
                if (!String.IsNullOrWhiteSpace(deleteViewModel.email))
                {
                    ApplicationUser result = await _userRepo.FindUserByEmail(deleteViewModel.email);
                    if (result != null)
                    {
                        await _userRepo.DeleteUser(result);
                        return StatusCode(StatusCodes.Status200OK, new { success = true });
                    }
                    else
                        return StatusCode(StatusCodes.Status400BadRequest, new { success = false, message = "This user does not exist" });
                }
                else
                {
                    return StatusCode(StatusCodes.Status400BadRequest, new { success = false, message = "User Id was not passed in request body" });
                }
            }
            catch (Exception ex)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, ex.Message);
            }
        }
    }
}
