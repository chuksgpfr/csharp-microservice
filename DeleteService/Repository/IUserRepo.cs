using System;
using System.Threading.Tasks;
using DeleteService.Data;
using DeleteService.Models;
using Microsoft.AspNetCore.Identity;

namespace DeleteService.Repository
{
    public interface IUserRepo
    {
        Task<IdentityResult> DeleteUser(ApplicationUser user);
        Task<ApplicationUser> FindUserByEmail(string email);
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

        public async Task<IdentityResult> DeleteUser(ApplicationUser user)
        {
            try
            {
                return await _userManager.DeleteAsync(user);
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public async Task<ApplicationUser> FindUserByEmail(string email)
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
    }
}
