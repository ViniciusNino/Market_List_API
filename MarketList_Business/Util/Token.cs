using System;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using MarketList_API.Data;
using MarketList_Model;
using Microsoft.IdentityModel.Tokens;
using BC = BCrypt.Net.BCrypt;

namespace MarketList_API.Util
{
    public static class Token
    {
        public static string GetSenhaTemporaria(int quantidadeCaracter)
        {
            string chars = "abcdefghjkmnpqrstuvwxyz023456789ABCDEFGHIJLMNOPQRSTUVXZWYK!@#$";
            string pass = "";
            Random random = new Random();
            for (int f = 0; f < quantidadeCaracter; f++)
            {
                pass = pass + chars.Substring(random.Next(0, chars.Length - 1), 1);
            }

            return pass;
        }

        public static JwtSecurityToken GerarJWTToken(UsuarioAutenticadoDTO usuario)
        {
            var claims = new[]
            {
                new Claim(JwtRegisteredClaimNames.Sub, Common.GetJwtSettings("Subject")),
                new Claim(JwtRegisteredClaimNames.Jti, Guid.NewGuid().ToString()),
                new Claim(JwtRegisteredClaimNames.Iat, DateTime.Now.ToString()),
                new Claim("Id", usuario.Id.ToString()),
                new Claim("Nome", usuario.Nome ?? string.Empty),
                new Claim("Email", usuario.Email),
            };

            var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(Common.GetJwtSettings("Key")));

            var signIn = new SigningCredentials(key, SecurityAlgorithms.HmacSha256);

            var token = new JwtSecurityToken(Common.GetJwtSettings("Issuer"), Common.GetJwtSettings("Audience"), claims, expires: DateTime.Now.AddHours(5), signingCredentials: signIn);
            return token;
        }

        public static bool ValidarSenha(string senhaUsuario, string senhaLogin)
        {
            if (senhaLogin != null)
            {
                var igual = BC.Verify(senhaUsuario, senhaLogin);
                return igual;
            }

            return false;
        }
    }
}