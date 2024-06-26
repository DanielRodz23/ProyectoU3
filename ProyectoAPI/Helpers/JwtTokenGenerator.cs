﻿using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using Microsoft.IdentityModel.Tokens;
using ProyectoAPI.Models.Entities;
using ProyectoAPI.Models.LoginModels;

namespace ProyectoAPI.Helpers
{
    public class JwtTokenGenerator
    {
        private readonly IConfigurationRoot jwtconfig;
        private JwtModel jwtModel;

        public JwtTokenGenerator(IConfigurationRoot jwtconfig)
        {
            this.jwtconfig = jwtconfig;
            jwtModel = jwtconfig.GetSection("Jwt").Get<JwtModel>() ?? new();
        }
        public string GetToken(Departamentos depa)
        {
            List<Claim> claims =
            [
                new Claim(ClaimTypes.Name, depa.Nombre),
                new Claim(JwtRegisteredClaimNames.Sub, jwtModel.Subject),
                new Claim(JwtRegisteredClaimNames.Aud, jwtModel.Audience),
                new Claim(JwtRegisteredClaimNames.Iat, DateTime.Now.ToString()),
                // claims.Add(new Claim(JwtRegisteredClaimNames.Exp, DateTime.Now.AddMinutes(20).ToString()));
                new Claim("id", depa.Id.ToString()),
            ];

            JwtSecurityTokenHandler handler = new();

            var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(jwtModel.Key));
            var signin = new SigningCredentials(key, SecurityAlgorithms.HmacSha256);

            var token = new SecurityTokenDescriptor()
            {
                Subject = new ClaimsIdentity(claims),
                Issuer = jwtModel.Issuer,
                Audience = jwtModel.Audience,
                Expires = DateTime.UtcNow.AddHours(24),
                SigningCredentials = signin
            };
            var data =handler.CreateEncodedJwt(token); 
            return data;
        }
    }
}
