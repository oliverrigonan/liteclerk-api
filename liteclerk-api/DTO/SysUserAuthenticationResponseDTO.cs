using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace liteclerk_api.DTO
{
    public class SysUserAuthenticationResponseDTO
    {
        public Int32 Id { get; set; }
        public String AccessToken { get; set; }
        public String ExpiresIn { get; set; }
        public String UserName { get; set; }
        public String FullName { get; set; }

        public SysUserAuthenticationResponseDTO(DBSets.MstUserDBSet mstUserDBSet, String accessToken, String expiresIn)
        {
            Id = mstUserDBSet.Id;
            AccessToken = accessToken;
            ExpiresIn = expiresIn;
            UserName = mstUserDBSet.Username;
            FullName = mstUserDBSet.Fullname;
        }
    }
}