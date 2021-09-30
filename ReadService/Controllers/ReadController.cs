using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using ReadService.Models;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.ModelBinding;
using Microsoft.Extensions.Configuration;
using UpdateService.Repository;

// For more information on enabling MVC for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace UpdateService.Controllers
{

    [ApiController]
    [Route("api/[controller]/[action]")]
    public class ReadController : ControllerBase
    {
        private readonly IUserRepo _userRepo;

        public ReadController(IUserRepo userRepo)
        {
            _userRepo = userRepo;
        }

        [HttpGet]
        public async Task<ActionResult> GetAllStudent()
        {
            try
            {
                var students = await _userRepo.AllStudent();

                return StatusCode(StatusCodes.Status200OK,  students );
            }
            catch (Exception ex)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, ex.Message);
            }
        }

        [HttpGet("{email}")]
        public async Task<ActionResult> GetStudent([FromRoute] string email)
        {
            try
            {
                if(String.IsNullOrWhiteSpace(email))
                {
                    return StatusCode(StatusCodes.Status400BadRequest, new { message = "You didn't pass a student ID" });
                }
                var student = await _userRepo.GetStudentByEmail(email);

                return StatusCode(StatusCodes.Status200OK, student);
            }
            catch (Exception ex)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, ex.Message);
            }
        }
    }
}
