using DemOffice.GenericCrud.Helpers;
using Microsoft.AspNetCore.Http;

namespace DemOffice.GenericCrud.Services
{
    public interface IAuthContext
    {
        long GetUserId();

        string GetUsername();

        string GetRole();
    }

    public class AuthContext : IAuthContext
    {
        private readonly IHttpContextAccessor _context;

        public AuthContext(IHttpContextAccessor context)
        {
            _context = context;
        }

        public long GetUserId()
        {
            return _context.HttpContext.User.Claims.GetId();
        }

        public string GetUsername()
        {
            return _context.HttpContext.User.Claims.GetUsername();
        }

        public string GetRole()
        {
            return _context.HttpContext.User.Claims.GetRole();
        }
    }
}
