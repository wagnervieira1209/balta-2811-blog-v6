using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;
using BlogV6.Models;

namespace BlogV6.Extensions
{
    public static class RoleClaimsExtensions
    {
        public static IEnumerable<Claim> GetClaims(this User user)
        {
            var result = new List<Claim>()
            {
                new (ClaimTypes.Name, user.Email)
            };

            result.AddRange(user.Roles.Select(x => new Claim(ClaimTypes.Role, x.Slug)));

            return result;
        }
    }
}