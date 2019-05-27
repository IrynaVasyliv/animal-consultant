using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;

namespace DemOffice.GenericCrud.Helpers
{
    public static class ClaimsExtensions
    {
        public static readonly string ClaimId = "Id";

        public static long GetId(this IEnumerable<Claim> claims)
        {
            return Convert.ToInt64(claims.First(c => c.Type == ClaimId).Value);
        }

        public static string GetUsername(this IEnumerable<Claim> claims)
        {
            return claims.First(c => c.Type == ClaimsIdentity.DefaultNameClaimType).Value;
        }

        public static string GetRole(this IEnumerable<Claim> claims)
        {
            return claims.First(c => c.Type == ClaimsIdentity.DefaultRoleClaimType).Value;
        }
    }
}
