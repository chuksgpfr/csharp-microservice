using System;
using System.Threading.Tasks;
using ReadService.Data;
using ReadService.Models;
using Microsoft.AspNetCore.Identity;
using System.Collections.Generic;

namespace UpdateService.Repository
{
    public interface IUserRepo
    {
        Task<ApplicationUser> GetStudentByEmail(string email);
        Task<IdentityResult> UpdateStudent(ApplicationUser user);
        Task<IList<ApplicationUser>> AllStudent();
    }

    class UserRepo : IUserRepo
    {
        private readonly UserManager<ApplicationUser> _userManager;
        private readonly ApplicationDbContext _context;

        public UserRepo(UserManager<ApplicationUser> userManager, ApplicationDbContext context)
        {
            _userManager = userManager;
            _context = context;
        }

        public async Task<IList<ApplicationUser>> AllStudent()
        {
            try
            {
                return await _userManager.GetUsersInRoleAsync("Student");
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public async Task<ApplicationUser> GetStudentByEmail(string email)
        {
            try
            {
                return await _userManager.FindByEmailAsync(email);
            }
            catch (Exception ex)
            {

                throw ex;
            }
        }


        public async Task<IdentityResult> UpdateStudent(ApplicationUser user)
        {
            try
            {
                return await _userManager.UpdateAsync(user);

            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
    }
}
