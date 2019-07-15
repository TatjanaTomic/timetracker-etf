﻿using Microsoft.IdentityModel.Tokens;
using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;

namespace TimeTrackerEtf.Extensions
{
    public class JwtTokenGenerator
    {
        public static string Generate(string name, bool isAdmin, string issuer, string key)
        {
            var claims = new List<Claim>
            {
                new Claim(JwtRegisteredClaimNames.Sub, name),
                new Claim(JwtRegisteredClaimNames.Jti, Guid.NewGuid().ToString())

            };

            if(isAdmin)
            {
                claims.Add(new Claim(ClaimTypes.Role, "admin"));
            }

            var securityKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(key));
            var credentials = new SigningCredentials(securityKey, SecurityAlgorithms.HmacSha256);

            var token = new JwtSecurityToken(
                issuer,
                issuer,
                claims,
                // WARNING: Use shorter period
                expires: DateTime.Now.AddDays(365),
                signingCredentials: credentials);

            return new JwtSecurityTokenHandler().WriteToken(token);
        }
    }
}
