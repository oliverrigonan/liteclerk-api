using liteclerk_api.DTO;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Options;
using Microsoft.IdentityModel.Tokens;
using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;

namespace liteclerk_api.Modules
{
    public interface ISysUserAuthenticationModule
    {
        public Task<SysUserAuthenticationResponseDTO> Authenticate(SysUserAuthenticationRequestDTO sysUserAuthenticationRequestDTO);
    }

    public class SysUserAuthenticationModule : ISysUserAuthenticationModule
    {
        private readonly DBContext.LiteclerkDBContext _dbContext;
        private readonly SysUserAuthenticationSecretKeyDTO _sysUserAuthenticationSecretKeyDTO;
        public DateTime? _tokenExpirationDate;

        public SysUserAuthenticationModule(DBContext.LiteclerkDBContext dbContext, IOptions<SysUserAuthenticationSecretKeyDTO> sysUserAuthenticationSecretKeyDTO)
        {
            _dbContext = dbContext;
            _sysUserAuthenticationSecretKeyDTO = sysUserAuthenticationSecretKeyDTO.Value;
        }

        public async Task<SysUserAuthenticationResponseDTO> Authenticate(SysUserAuthenticationRequestDTO sysUserAuthenticationRequestDTO)
        {
            try
            {
                DBSets.MstUserDBSet user = await (
                    from d in _dbContext.MstUsers
                    where d.Username == sysUserAuthenticationRequestDTO.Username
                    && d.Password == sysUserAuthenticationRequestDTO.Password
                    select d
                ).FirstOrDefaultAsync();

                if (user == null)
                {
                    return null;
                }

                String token = GenerateJwtToken(user);

                return new SysUserAuthenticationResponseDTO(user, token, _tokenExpirationDate.ToString());
            }
            catch(Exception e)
            {
                throw e;
            }
        }

        private String GenerateJwtToken(DBSets.MstUserDBSet mstUserDBSet)
        {
            JwtSecurityTokenHandler tokenHandler = new JwtSecurityTokenHandler();
            byte[] key = Encoding.ASCII.GetBytes(_sysUserAuthenticationSecretKeyDTO.SecretKey);

            SecurityTokenDescriptor tokenDescriptor = new SecurityTokenDescriptor
            {
                Subject = new ClaimsIdentity(new Claim[]
                {
                    new Claim(ClaimTypes.Name, mstUserDBSet.Id.ToString())
                }),
                Expires = DateTime.UtcNow.AddDays(7),
                SigningCredentials = new SigningCredentials(new SymmetricSecurityKey(key), SecurityAlgorithms.HmacSha256Signature)
            };

            _tokenExpirationDate = tokenDescriptor.Expires;
            SecurityToken token = tokenHandler.CreateToken(tokenDescriptor);

            return tokenHandler.WriteToken(token);
        }
    }
}
