using GodOx.Sys.API.Models.Dtos.Output;
using Microsoft.Extensions.Options;
using Microsoft.IdentityModel.Tokens;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;

namespace GodOx.Sys.API.Jwt
{
    public class JwtHelper
    {
        private readonly IOptions<JwtSetting> _jwtSetting;

        public JwtHelper(IOptions<JwtSetting> jwtSetting)
        {
            _jwtSetting = jwtSetting;
        }
        public string GetJwtToken(LoginOutput loginOutput)
        {
            //如果是基于用户的授权策略，这里要添加用户;如果是基于角色的授权策略，这里要添加角色
            var claims = new List<Claim>
            {
                    new Claim(ClaimTypes.Name, loginOutput.LoginName),
                    new Claim(JwtRegisteredClaimNames.Sid, loginOutput.Id.ToString()),
                    new Claim(ClaimTypes.Expiration, DateTime.Now.AddSeconds(_jwtSetting.Value.ExpireSeconds).ToString(CultureInfo.InvariantCulture)),
                    new Claim(ClaimTypes.Role,"Type"),
                    new Claim("mobile",loginOutput.Mobile)
            };
            var token = BuildJwtToken(claims.ToArray());
            return token;
        }
        /// <summary>   
        /// 获取基于JWT的Token
        /// </summary>
        /// <param name="claims">需要在登陆的时候配置</param>
        /// <returns></returns>

        public string BuildJwtToken(Claim[] claims)
        {
            var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_jwtSetting.Value.SecurityKey));
            var creds = new SigningCredentials(key, SecurityAlgorithms.HmacSha256);
            // 实例化JwtSecurityToken
            var jwtToken = new JwtSecurityToken(
                issuer: _jwtSetting.Value.Issuer,
                audience: _jwtSetting.Value.Audience,
                claims: claims,
                notBefore: DateTime.Now,
                expires: DateTime.Now.AddDays(1),
                signingCredentials: creds
            );
            // 生成 Token
            return new JwtSecurityTokenHandler().WriteToken(jwtToken);
        }
    }
}
