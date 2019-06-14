using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using AnimalConsultant.Services.Models;
using AutoMapper;
using DemOffice.GenericCrud.Repositories;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using D = AnimalConsultant.DAL.Models;
using S = AnimalConsultant.Services.Models;

namespace AnimalConsultant.Services
{
    public interface IUserService
    {
        Task<IdentityResult> CreateUserAsync(S.Users user, string userRole = "user");
        Task<bool> CheckUserForRole(string email, string password, string role);
        Task SignInUserAsync(S.Users user, bool isLongTime, string schemeName = "default");
        Task<SignInResult> PasswordSignInUserAsync(string email, string password, bool rememberMe, bool isLongTime);
        Task SignOutUserAsync();
        Task<S.Users> GetCurrentUserAsync();
        Task<S.Users> GetUserById(long id);
        Task<S.Users> GetUserByNickName(long id);
        Task<IdentityResult> UpdateUserAsync(S.Users user);
        Task<IdentityResult> ChangePasswordAsync(S.Users user, string newPassword);
        Task<IEnumerable<string>> GetAllRolesAsync();
        Task<string> GetRoleByUserAsync(long userId);
        Task SetUsersRole(long id, string role);
    }

    public class UserService : IUserService
    {
        private readonly UserManager<D.User> _userManager;
        private readonly IGenericRepository<D.User> _userRepository;
        private readonly SignInManager<D.User> _signInManager;
        private readonly RoleManager<D.Identity.Role> _roleManager;
        private readonly IHttpContextAccessor _context;
        private readonly IMapper Mapper;

        public UserService(UserManager<D.User> userManager, SignInManager<D.User> signInManager, RoleManager<D.Identity.Role> roleManager, IHttpContextAccessor context, IGenericRepository<D.User> userRepository, IMapper mapper)
        {
            _context = context;
            _userRepository = userRepository;
            Mapper = mapper;
            _userManager = userManager;
            _signInManager = signInManager;
            _roleManager = roleManager;
        }

        private async Task<IdentityResult> CreateRoleAsync(string role)
        {
            if (!_roleManager.RoleExistsAsync(role).Result)
            {
                await _roleManager.CreateAsync(new D.Identity.Role(role));
            }
            return IdentityResult.Success;

        }

        public async Task<IdentityResult> UpdateUserAsync(Users user)
        {
            var userEntity = Mapper.Map<S.Users, D.User>(user, await _userRepository.Read(user.Id));
            var userUpdateResult = await _userManager.UpdateAsync(userEntity);
            if (userUpdateResult.Succeeded)
            {
                return IdentityResult.Success;
            }
            return IdentityResult.Failed();
        }

        public async Task<IdentityResult> ChangePasswordAsync(Users user, string newPassword)
        {
            var userEntity = await _userRepository.Read(user.Id);
            var result = await _userManager.ChangePasswordAsync(userEntity, user.Password, newPassword);
            if (result.Succeeded)
            {
                return IdentityResult.Success;
            }
            return IdentityResult.Failed();
        }

        public async Task<IdentityResult> CreateUserAsync(Users user, string userRole = "user")
        {
            var userEntity = Mapper.Map<S.Users, D.User>(user);
            var userRegisterResult = await _userManager.CreateAsync(userEntity, user.Password);

            if (userRegisterResult.Succeeded && CreateRoleAsync(userRole).Result.Succeeded)
            {
                await _userManager.AddToRoleAsync(userEntity, userRole);
                return IdentityResult.Success;
            }
            return IdentityResult.Failed();
        }



        public async Task SignInUserAsync(Users user, bool isLongTime, string schemeName = "default")
        {
            var userEntity = Mapper.Map<S.Users, D.User>(user);
            var schemes = (await _signInManager.GetExternalAuthenticationSchemesAsync()).Where(c => c.Name == schemeName);
            await _signInManager.SignInAsync(userEntity, isLongTime);
        }

        public async Task<SignInResult> PasswordSignInUserAsync(string email, string password, bool rememberMe, bool isLongTime)
        {
            var user = await _userManager.FindByEmailAsync(email);
            if (user != null)
            {
                return await _signInManager.PasswordSignInAsync(user, password, rememberMe, isLongTime);
            }
            return SignInResult.Failed;
        }

        public async Task<bool> CheckUserForRole(string email, string password, string role)
        {
            D.User user = _userManager.Users.FirstOrDefault(c => c.Email == email);
            if (user != null)
            {
                return await _userManager.IsInRoleAsync(user, role);
            }
            return false;
        }

        public async Task SignOutUserAsync()
        {
            await _signInManager.SignOutAsync();
        }

        public async Task<Users> GetCurrentUserAsync()
        {
            var user = await _userManager.GetUserAsync(_context.HttpContext.User);
            var mapped = Mapper.Map<D.User, S.Users>(user);
            mapped.Role = await this.GetRoleByUserAsync(user.Id);
            return mapped;
        }

        public async Task<Users> GetUserById(long id)
        {
            var user = await _userRepository.Read(id);
            var mapped = Mapper.Map<D.User, S.Users>(user);
            mapped.Role = await this.GetRoleByUserAsync(user.Id);
            return mapped;
        }
        public async Task<Users> GetUserByNickName(long nickName)
        {
            return Mapper.Map<D.User, S.Users>(await _userRepository.Read(nickName));
        }

        public async Task<IEnumerable<string>> GetAllRolesAsync()
        {
            return await _roleManager.Roles.Select(c => c.Name).ToListAsync();
        }

        public async Task<string> GetRoleByUserAsync(long userId)
        {
            string result = (await _userManager.GetRolesAsync(await _userRepository.Read(userId))).ToList()[0];
            return result;
        }

        public async Task SetUsersRole(long id, string role)
        {
            D.User user = await _userRepository.Read(id);
            string userRole = (await _userManager.GetRolesAsync(user)).ToList()[0];
            if (!userRole.Equals(role))
            {
                await _userManager.RemoveFromRoleAsync(user, userRole);
                await _userManager.AddToRoleAsync(user, role);
            }
        }
    }
}
