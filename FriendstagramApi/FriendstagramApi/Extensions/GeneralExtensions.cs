using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;

namespace FriendstagramApi.Extensions
{
    public static class GeneralExtensions
    {

        public static int ReadUserId(this ClaimsPrincipal claimsPrincipal)
        {
            List<Claim> claims = claimsPrincipal.Claims.ToList();
            string claimOfIdValue = claims.FirstOrDefault(x => x.Type == "id").Value;

            int userId;
            int.TryParse(claimOfIdValue, out userId);

            if (userId > 0)
            {
                return userId;
            }
            else
            {
                throw new Exception("Can not read token");
            }
        }




    }
}
