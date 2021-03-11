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
        public Int32? CompanyId { get; set; }
        public String Company { get; set; }
        public String CompanyImageURL { get; set; }
        public Int32? BranchId { get; set; }
        public String Branch { get; set; }

        public SysUserAuthenticationResponseDTO(DBSets.MstUserDBSet mstUserDBSet, String accessToken, String expiresIn)
        {
            Id = mstUserDBSet.Id;
            AccessToken = accessToken;
            ExpiresIn = expiresIn;
            UserName = mstUserDBSet.Username;
            FullName = mstUserDBSet.Fullname;
            CompanyId = mstUserDBSet.CompanyId != null ? mstUserDBSet.CompanyId : 0;
            Company = mstUserDBSet.CompanyId != null ? mstUserDBSet.MstCompany_CompanyId.Company : "";
            CompanyImageURL = mstUserDBSet.CompanyId != null ? mstUserDBSet.MstCompany_CompanyId.ImageURL : "";
            BranchId = mstUserDBSet.BranchId != null ? mstUserDBSet.BranchId : 0;
            Branch = mstUserDBSet.BranchId != null ? mstUserDBSet.MstCompanyBranch_BranchId.Branch : "";
        }
    }
}