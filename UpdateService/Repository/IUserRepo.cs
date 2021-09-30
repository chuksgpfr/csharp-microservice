using System;
using System.Threading.Tasks;
using UpdateService.Data;
using UpdateService.Models;
using Microsoft.AspNetCore.Identity;

namespace UpdateService.Repository
{
    public interface IUserRepo
    {
        Task<ApplicationUser> GetUserByEmail(string email);
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

        public async Task<ApplicationUser> GetUserByEmail(string email)
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
