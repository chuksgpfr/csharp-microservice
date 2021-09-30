using System;
using System.Threading.Tasks;
using CreateService.Data;
using CreateService.Models;
using Microsoft.AspNetCore.Identity;

namespace CreateService.Repository
{
    public interface IUserRepo
    {
        Task<ApplicationUser> GetUserById(string userid);
        Task<IdentityResult> UpdateUser(ApplicationUser user);
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

        public async Task<ApplicationUser> GetUserById(string userid)
        {
            try
            {
                return await _userManager.FindByIdAsync(userid);
            }
            catch (Exception ex)
            {

                throw ex;
            }
        }


        public async Task<IdentityResult> UpdateUser(ApplicationUser user)
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
