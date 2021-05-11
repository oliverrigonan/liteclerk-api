using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Cors;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Azure.Storage;
using Microsoft.Azure.Storage.Blob;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;

namespace liteclerk_api.APIControllers
{
    [Authorize]
    [EnableCors("AppCorsPolicy")]
    [Route("api/[controller]")]
    [ApiController]
    public class MstCompanyAPIController : ControllerBase
    {
        private readonly DBContext.LiteclerkDBContext _dbContext;
        private IConfiguration _configuration { get; }

        public MstCompanyAPIController(DBContext.LiteclerkDBContext dbContext, IConfiguration configuration)
        {
            _dbContext = dbContext;
            _configuration = configuration;
        }

        [NonAction]
        public String PadZeroes(Int32 number, Int32 length)
        {
            var result = number.ToString();
            var pad = length - result.Length;
            while (pad > 0)
            {
                result = '0' + result;
                pad--;
            }

            return result;
        }

        [HttpGet("list")]
        public async Task<ActionResult> GetCompanyList()
        {
            try
            {
                var companies = await (
                    from d in _dbContext.MstCompanies
                    select new DTO.MstCompanyDTO
                    {
                        Id = d.Id,
                        CompanyCode = d.CompanyCode,
                        ManualCode = d.ManualCode,
                        Company = d.Company,
                        Address = d.Address,
                        TIN = d.TIN,
                        ImageURL = d.ImageURL,
                        CurrencyId = d.CurrencyId,
                        Currency = new DTO.MstCurrencyDTO
                        {
                            ManualCode = d.MstCurrency_CurrencyId.ManualCode,
                            Currency = d.MstCurrency_CurrencyId.Currency
                        },
                        CostMethod = d.CostMethod,
                        IncomeAccountId = d.IncomeAccountId,
                        IncomeAccount = new DTO.MstAccountDTO
                        {
                            ManualCode = d.MstAccount_IncomeAccountId.ManualCode,
                            Account = d.MstAccount_IncomeAccountId.Account
                        },
                        SalesInvoiceCheckedByUserId = d.SalesInvoiceCheckedByUserId,
                        SalesInvoiceCheckedByUser = new DTO.MstUserDTO
                        {
                            Username = d.MstUser_SalesInvoiceCheckedByUserId.Username,
                            Fullname = d.MstUser_SalesInvoiceCheckedByUserId.Fullname
                        },
                        SalesInvoiceApprovedByUserId = d.SalesInvoiceApprovedByUserId,
                        SalesInvoiceApprovedByUser = new DTO.MstUserDTO
                        {
                            Username = d.MstUser_SalesInvoiceApprovedByUserId.Username,
                            Fullname = d.MstUser_SalesInvoiceApprovedByUserId.Fullname
                        },
                        ForexGainAccountId = d.ForexGainAccountId,
                        ForexGainAccount = new DTO.MstAccountDTO
                        {
                            ManualCode = d.MstAccount_ForexGainAccount.ManualCode,
                            Account = d.MstAccount_ForexGainAccount.Account
                        },
                        ForexLossAccountId = d.ForexLossAccountId,
                        ForexLossAccount = new DTO.MstAccountDTO
                        {
                            ManualCode = d.MstAccount_ForexLossAccount.ManualCode,
                            Account = d.MstAccount_ForexLossAccount.Account
                        },
                        IsLocked = d.IsLocked,
                        CreatedByUser = new DTO.MstUserDTO
                        {
                            Username = d.MstUser_CreatedByUserId.Username,
                            Fullname = d.MstUser_CreatedByUserId.Fullname
                        },
                        CreatedDateTime = d.CreatedDateTime.ToString("MMMM dd, yyyy hh:mm tt"),
                        UpdatedByUser = new DTO.MstUserDTO
                        {
                            Username = d.MstUser_UpdatedByUserId.Username,
                            Fullname = d.MstUser_UpdatedByUserId.Fullname
                        },
                        UpdatedDateTime = d.UpdatedDateTime.ToString("MMMM dd, yyyy hh:mm tt")
                    }
                ).ToListAsync();

                return StatusCode(200, companies);
            }
            catch (Exception e)
            {
                return StatusCode(500, e.InnerException.Message);
            }
        }

        [HttpGet("list/locked")]
        public async Task<ActionResult> GetLockedCompanyList()
        {
            try
            {
                var companies = await (
                    from d in _dbContext.MstCompanies
                    where d.IsLocked == true
                    select new DTO.MstCompanyDTO
                    {
                        Id = d.Id,
                        CompanyCode = d.CompanyCode,
                        ManualCode = d.ManualCode,
                        Company = d.Company,
                        Address = d.Address,
                        TIN = d.TIN,
                        ImageURL = d.ImageURL,
                        CurrencyId = d.CurrencyId,
                        Currency = new DTO.MstCurrencyDTO
                        {
                            ManualCode = d.MstCurrency_CurrencyId.ManualCode,
                            Currency = d.MstCurrency_CurrencyId.Currency
                        },
                        CostMethod = d.CostMethod,
                        IncomeAccountId = d.IncomeAccountId,
                        IncomeAccount = new DTO.MstAccountDTO
                        {
                            ManualCode = d.MstAccount_IncomeAccountId.ManualCode,
                            Account = d.MstAccount_IncomeAccountId.Account
                        },
                        SalesInvoiceCheckedByUserId = d.SalesInvoiceCheckedByUserId,
                        SalesInvoiceCheckedByUser = new DTO.MstUserDTO
                        {
                            Username = d.MstUser_SalesInvoiceCheckedByUserId.Username,
                            Fullname = d.MstUser_SalesInvoiceCheckedByUserId.Fullname
                        },
                        SalesInvoiceApprovedByUserId = d.SalesInvoiceApprovedByUserId,
                        SalesInvoiceApprovedByUser = new DTO.MstUserDTO
                        {
                            Username = d.MstUser_SalesInvoiceApprovedByUserId.Username,
                            Fullname = d.MstUser_SalesInvoiceApprovedByUserId.Fullname
                        },
                        ForexGainAccountId = d.ForexGainAccountId,
                        ForexGainAccount = new DTO.MstAccountDTO
                        {
                            ManualCode = d.MstAccount_ForexGainAccount.ManualCode,
                            Account = d.MstAccount_ForexGainAccount.Account
                        },
                        ForexLossAccountId = d.ForexLossAccountId,
                        ForexLossAccount = new DTO.MstAccountDTO
                        {
                            ManualCode = d.MstAccount_ForexLossAccount.ManualCode,
                            Account = d.MstAccount_ForexLossAccount.Account
                        },
                        IsLocked = d.IsLocked,
                        CreatedByUser = new DTO.MstUserDTO
                        {
                            Username = d.MstUser_CreatedByUserId.Username,
                            Fullname = d.MstUser_CreatedByUserId.Fullname
                        },
                        CreatedDateTime = d.CreatedDateTime.ToString("MMMM dd, yyyy hh:mm tt"),
                        UpdatedByUser = new DTO.MstUserDTO
                        {
                            Username = d.MstUser_UpdatedByUserId.Username,
                            Fullname = d.MstUser_UpdatedByUserId.Fullname
                        },
                        UpdatedDateTime = d.UpdatedDateTime.ToString("MMMM dd, yyyy hh:mm tt")
                    }
                ).ToListAsync();

                return StatusCode(200, companies);
            }
            catch (Exception e)
            {
                return StatusCode(500, e.InnerException.Message);
            }
        }

        [HttpGet("detail/{id}")]
        public async Task<ActionResult> GetCompanyDetail(int id)
        {
            try
            {
                var companyDetail = await (
                     from d in _dbContext.MstCompanies
                     where d.Id == id
                     select new DTO.MstCompanyDTO
                     {
                         Id = d.Id,
                         CompanyCode = d.CompanyCode,
                         ManualCode = d.ManualCode,
                         Company = d.Company,
                         Address = d.Address,
                         TIN = d.TIN,
                         ImageURL = d.ImageURL,
                         CurrencyId = d.CurrencyId,
                         Currency = new DTO.MstCurrencyDTO
                         {
                             ManualCode = d.MstCurrency_CurrencyId.ManualCode,
                             Currency = d.MstCurrency_CurrencyId.Currency
                         },
                         CostMethod = d.CostMethod,
                         IncomeAccountId = d.IncomeAccountId,
                         IncomeAccount = new DTO.MstAccountDTO
                         {
                             ManualCode = d.MstAccount_IncomeAccountId.ManualCode,
                             Account = d.MstAccount_IncomeAccountId.Account
                         },
                         SalesInvoiceCheckedByUserId = d.SalesInvoiceCheckedByUserId,
                         SalesInvoiceCheckedByUser = new DTO.MstUserDTO
                         {
                             Username = d.MstUser_SalesInvoiceCheckedByUserId.Username,
                             Fullname = d.MstUser_SalesInvoiceCheckedByUserId.Fullname
                         },
                         SalesInvoiceApprovedByUserId = d.SalesInvoiceApprovedByUserId,
                         SalesInvoiceApprovedByUser = new DTO.MstUserDTO
                         {
                             Username = d.MstUser_SalesInvoiceApprovedByUserId.Username,
                             Fullname = d.MstUser_SalesInvoiceApprovedByUserId.Fullname
                         },
                         ForexGainAccountId = d.ForexGainAccountId,
                         ForexGainAccount = new DTO.MstAccountDTO
                         {
                             ManualCode = d.MstAccount_ForexGainAccount.ManualCode,
                             Account = d.MstAccount_ForexGainAccount.Account
                         },
                         ForexLossAccountId = d.ForexLossAccountId,
                         ForexLossAccount = new DTO.MstAccountDTO
                         {
                             ManualCode = d.MstAccount_ForexLossAccount.ManualCode,
                             Account = d.MstAccount_ForexLossAccount.Account
                         },
                         IsLocked = d.IsLocked,
                         CreatedByUser = new DTO.MstUserDTO
                         {
                             Username = d.MstUser_CreatedByUserId.Username,
                             Fullname = d.MstUser_CreatedByUserId.Fullname
                         },
                         CreatedDateTime = d.CreatedDateTime.ToString("MMMM dd, yyyy hh:mm tt"),
                         UpdatedByUser = new DTO.MstUserDTO
                         {
                             Username = d.MstUser_UpdatedByUserId.Username,
                             Fullname = d.MstUser_UpdatedByUserId.Fullname
                         },
                         UpdatedDateTime = d.UpdatedDateTime.ToString("MMMM dd, yyyy hh:mm tt")
                     }
                ).FirstOrDefaultAsync();

                return StatusCode(200, companyDetail);
            }
            catch (Exception e)
            {
                return StatusCode(500, e.InnerException.Message);
            }
        }

        [HttpPost("add")]
        public async Task<ActionResult> AddCompany()
        {
            try
            {
                Int32 loginUserId = Convert.ToInt32(User.FindFirst(ClaimTypes.Name)?.Value);

                var loginUser = await (
                    from d in _dbContext.MstUsers
                    where d.Id == loginUserId
                    select d
                ).FirstOrDefaultAsync();

                if (loginUser == null)
                {
                    return StatusCode(404, "Login user not found.");
                }

                var currency = await (
                    from d in _dbContext.MstCurrencies
                    select d
                ).FirstOrDefaultAsync();

                if (currency == null)
                {
                    return StatusCode(404, "Currency not found.");
                }

                String companyCode = "0000000001";
                var lastCompany = await (
                    from d in _dbContext.MstCompanies
                    orderby d.Id descending
                    select d
                ).FirstOrDefaultAsync();

                if (lastCompany != null)
                {
                    Int32 lastCompanyCode = Convert.ToInt32(lastCompany.CompanyCode) + 0000000001;
                    companyCode = PadZeroes(lastCompanyCode, 10);
                }

                var newCompany = new DBSets.MstCompanyDBSet()
                {
                    CompanyCode = companyCode,
                    ManualCode = companyCode,
                    Company = "",
                    Address = "",
                    TIN = "",
                    ImageURL = "",
                    CurrencyId = currency.Id,
                    CostMethod = "Last Purchase Cost",
                    IncomeAccountId = null,
                    SalesInvoiceCheckedByUserId = null,
                    SalesInvoiceApprovedByUserId = null,
                    ForexGainAccountId = null,
                    ForexLossAccountId = null,
                    IsLocked = false,
                    CreatedByUserId = loginUserId,
                    CreatedDateTime = DateTime.Now,
                    UpdatedByUserId = loginUserId,
                    UpdatedDateTime = DateTime.Now
                };

                _dbContext.MstCompanies.Add(newCompany);
                await _dbContext.SaveChangesAsync();

                return StatusCode(200, newCompany.Id);
            }
            catch (Exception e)
            {
                return StatusCode(500, e.InnerException.Message);
            }
        }

        [HttpPut("save/{id}")]
        public async Task<ActionResult> SaveCompany(int id, [FromBody] DTO.MstCompanyDTO mstCompanyDTO)
        {
            try
            {
                Int32 loginUserId = Convert.ToInt32(User.FindFirst(ClaimTypes.Name)?.Value);

                var loginUser = await (
                    from d in _dbContext.MstUsers
                    where d.Id == loginUserId
                    select d
                ).FirstOrDefaultAsync();

                if (loginUser == null)
                {
                    return StatusCode(404, "Login user not found.");
                }

                var company = await (
                    from d in _dbContext.MstCompanies
                    where d.Id == id
                    select d
                ).FirstOrDefaultAsync();

                if (company == null)
                {
                    return StatusCode(404, "Company not found.");
                }

                if (company.IsLocked == true)
                {
                    return StatusCode(400, "Cannot save or make any changes to a company that is locked.");
                }

                var currency = await (
                    from d in _dbContext.MstCurrencies
                    where d.Id == mstCompanyDTO.CurrencyId
                    select d
                ).FirstOrDefaultAsync();

                if (currency == null)
                {
                    return StatusCode(404, "Currency not found.");
                }

                if (mstCompanyDTO.IncomeAccountId != null)
                {
                    var incomeAccount = await (
                        from d in _dbContext.MstAccounts
                        where d.Id == mstCompanyDTO.IncomeAccountId
                        select d
                    ).FirstOrDefaultAsync();

                    if (incomeAccount == null)
                    {
                        return StatusCode(404, "Income account not found.");
                    }
                }

                if (mstCompanyDTO.SalesInvoiceCheckedByUserId != null)
                {
                    var salesInvoiceCheckedByUser = await (
                        from d in _dbContext.MstUsers
                        where d.Id == mstCompanyDTO.SalesInvoiceCheckedByUserId
                        select d
                    ).FirstOrDefaultAsync();

                    if (salesInvoiceCheckedByUser == null)
                    {
                        return StatusCode(404, "Sales invoice checked by user not found.");
                    }
                }

                if (mstCompanyDTO.SalesInvoiceApprovedByUserId != null)
                {
                    var salesInvoiceCheckedByUser = await (
                        from d in _dbContext.MstUsers
                        where d.Id == mstCompanyDTO.SalesInvoiceApprovedByUserId
                        select d
                    ).FirstOrDefaultAsync();

                    if (salesInvoiceCheckedByUser == null)
                    {
                        return StatusCode(404, "Sales invoice approved by user not found.");
                    }
                }

                if (mstCompanyDTO.ForexGainAccountId != null)
                {
                    var forexGainAccount = await (
                        from d in _dbContext.MstAccounts
                        where d.Id == mstCompanyDTO.ForexGainAccountId
                        select d
                    ).FirstOrDefaultAsync();

                    if (forexGainAccount == null)
                    {
                        return StatusCode(404, "Forex gain account not found.");
                    }
                }

                if (mstCompanyDTO.ForexLossAccount != null)
                {
                    var forexLossAccount = await (
                        from d in _dbContext.MstAccounts
                        where d.Id == mstCompanyDTO.IncomeAccountId
                        select d
                    ).FirstOrDefaultAsync();

                    if (forexLossAccount == null)
                    {
                        return StatusCode(404, "Forex loss account not found.");
                    }
                }

                var saveCompany = company;
                saveCompany.ManualCode = mstCompanyDTO.ManualCode;
                saveCompany.Company = mstCompanyDTO.Company;
                saveCompany.Address = mstCompanyDTO.Address;
                saveCompany.TIN = mstCompanyDTO.TIN;
                saveCompany.ImageURL = mstCompanyDTO.ImageURL;
                saveCompany.CurrencyId = mstCompanyDTO.CurrencyId;
                saveCompany.CostMethod = mstCompanyDTO.CostMethod;
                saveCompany.IncomeAccountId = mstCompanyDTO.IncomeAccountId;
                saveCompany.SalesInvoiceCheckedByUserId = mstCompanyDTO.SalesInvoiceCheckedByUserId;
                saveCompany.SalesInvoiceApprovedByUserId = mstCompanyDTO.SalesInvoiceApprovedByUserId;
                saveCompany.ForexGainAccountId = mstCompanyDTO.ForexGainAccountId;
                saveCompany.ForexLossAccountId = mstCompanyDTO.ForexLossAccountId;
                saveCompany.UpdatedByUserId = loginUserId;
                saveCompany.UpdatedDateTime = DateTime.Now;

                await _dbContext.SaveChangesAsync();

                return StatusCode(200);
            }
            catch (Exception e)
            {
                return StatusCode(500, e.InnerException.Message);
            }
        }

        [HttpPut("lock/{id}")]
        public async Task<ActionResult> LockCompany(int id, [FromBody] DTO.MstCompanyDTO mstCompanyDTO)
        {
            try
            {
                Int32 loginUserId = Convert.ToInt32(User.FindFirst(ClaimTypes.Name)?.Value);

                var loginUser = await (
                    from d in _dbContext.MstUsers
                    where d.Id == loginUserId
                    select d
                ).FirstOrDefaultAsync();

                if (loginUser == null)
                {
                    return StatusCode(404, "Login user not found.");
                }

                var company = await (
                    from d in _dbContext.MstCompanies
                    where d.Id == id
                    select d
                ).FirstOrDefaultAsync();

                if (company == null)
                {
                    return StatusCode(404, "Company not found.");
                }

                if (company.IsLocked == true)
                {
                    return StatusCode(400, "Cannot lock a company that is already locked.");
                }

                var currency = await (
                    from d in _dbContext.MstCurrencies
                    where d.Id == mstCompanyDTO.CurrencyId
                    select d
                ).FirstOrDefaultAsync();

                if (currency == null)
                {
                    return StatusCode(404, "Currency not found.");
                }

                if (mstCompanyDTO.IncomeAccountId != null)
                {
                    var incomeAccount = await (
                        from d in _dbContext.MstAccounts
                        where d.Id == mstCompanyDTO.IncomeAccountId
                        select d
                    ).FirstOrDefaultAsync();

                    if (incomeAccount == null)
                    {
                        return StatusCode(404, "Income account not found.");
                    }
                }

                if (mstCompanyDTO.SalesInvoiceCheckedByUserId != null)
                {
                    var salesInvoiceCheckedByUser = await (
                        from d in _dbContext.MstUsers
                        where d.Id == mstCompanyDTO.SalesInvoiceCheckedByUserId
                        select d
                    ).FirstOrDefaultAsync();

                    if (salesInvoiceCheckedByUser == null)
                    {
                        return StatusCode(404, "Sales invoice checked by user not found.");
                    }
                }

                if (mstCompanyDTO.SalesInvoiceApprovedByUserId != null)
                {
                    var salesInvoiceCheckedByUser = await (
                        from d in _dbContext.MstUsers
                        where d.Id == mstCompanyDTO.SalesInvoiceApprovedByUserId
                        select d
                    ).FirstOrDefaultAsync();

                    if (salesInvoiceCheckedByUser == null)
                    {
                        return StatusCode(404, "Sales invoice approved by user not found.");
                    }
                }

                if (mstCompanyDTO.ForexGainAccountId != null)
                {
                    var forexGainAccount = await (
                        from d in _dbContext.MstAccounts
                        where d.Id == mstCompanyDTO.ForexGainAccountId
                        select d
                    ).FirstOrDefaultAsync();

                    if (forexGainAccount == null)
                    {
                        return StatusCode(404, "Forex gain account not found.");
                    }
                }

                if (mstCompanyDTO.ForexLossAccount != null)
                {
                    var forexLossAccount = await (
                        from d in _dbContext.MstAccounts
                        where d.Id == mstCompanyDTO.IncomeAccountId
                        select d
                    ).FirstOrDefaultAsync();

                    if (forexLossAccount == null)
                    {
                        return StatusCode(404, "Forex loss account not found.");
                    }
                }

                var lockCompany = company;
                lockCompany.ManualCode = mstCompanyDTO.ManualCode;
                lockCompany.Company = mstCompanyDTO.Company;
                lockCompany.Address = mstCompanyDTO.Address;
                lockCompany.TIN = mstCompanyDTO.TIN;
                lockCompany.ImageURL = mstCompanyDTO.ImageURL;
                lockCompany.CurrencyId = mstCompanyDTO.CurrencyId;
                lockCompany.CostMethod = mstCompanyDTO.CostMethod;
                lockCompany.IncomeAccountId = mstCompanyDTO.IncomeAccountId;
                lockCompany.SalesInvoiceCheckedByUserId = mstCompanyDTO.SalesInvoiceCheckedByUserId;
                lockCompany.SalesInvoiceApprovedByUserId = mstCompanyDTO.SalesInvoiceApprovedByUserId;
                lockCompany.ForexGainAccountId = mstCompanyDTO.ForexGainAccountId;
                lockCompany.ForexLossAccountId = mstCompanyDTO.ForexLossAccountId;
                lockCompany.IsLocked = true;
                lockCompany.UpdatedByUserId = loginUserId;
                lockCompany.UpdatedDateTime = DateTime.Now;

                await _dbContext.SaveChangesAsync();

                return StatusCode(200);
            }
            catch (Exception e)
            {
                return StatusCode(500, e.InnerException.Message);
            }
        }

        [HttpPut("unlock/{id}")]
        public async Task<ActionResult> UnlockCompany(int id)
        {
            try
            {
                Int32 loginUserId = Convert.ToInt32(User.FindFirst(ClaimTypes.Name)?.Value);

                var loginUser = await (
                    from d in _dbContext.MstUsers
                    where d.Id == loginUserId
                    select d
                ).FirstOrDefaultAsync();

                if (loginUser == null)
                {
                    return StatusCode(404, "Login user not found.");
                }

                var company = await (
                    from d in _dbContext.MstCompanies
                    where d.Id == id
                    select d
                ).FirstOrDefaultAsync();

                if (company == null)
                {
                    return StatusCode(404, "Company not found.");
                }

                if (company.IsLocked == false)
                {
                    return StatusCode(400, "Cannot unlock a company that is already unlocked.");
                }

                var unlockCompany = company;
                unlockCompany.IsLocked = false;
                unlockCompany.UpdatedByUserId = loginUserId;
                unlockCompany.UpdatedDateTime = DateTime.Now;

                await _dbContext.SaveChangesAsync();

                return StatusCode(200);
            }
            catch (Exception e)
            {
                return StatusCode(500, e.InnerException.Message);
            }
        }

        [HttpDelete("delete/{id}")]
        public async Task<ActionResult> DeleteCompany(int id)
        {
            try
            {
                Int32 loginUserId = Convert.ToInt32(User.FindFirst(ClaimTypes.Name)?.Value);

                var loginUser = await (
                    from d in _dbContext.MstUsers
                    where d.Id == loginUserId
                    select d
                ).FirstOrDefaultAsync();

                if (loginUser == null)
                {
                    return StatusCode(404, "Login user not found.");
                }

                var company = await (
                    from d in _dbContext.MstCompanies
                    where d.Id == id
                    select d
                ).FirstOrDefaultAsync();

                if (company == null)
                {
                    return StatusCode(404, "Company not found.");
                }

                if (company.IsLocked == true)
                {
                    return StatusCode(400, "Cannot delete a company that is locked.");
                }

                _dbContext.MstCompanies.Remove(company);
                await _dbContext.SaveChangesAsync();

                return StatusCode(200);
            }
            catch (Exception e)
            {
                return StatusCode(500, e.InnerException.Message);
            }
        }

        [HttpPost("upload/image")]
        public async Task<ActionResult> UploadCompanyImage(IFormFile file)
        {
            try
            {
                String cloudStorageConnectionString = _configuration["CloudStorage:ConnectionString"];
                String cloudStorageContainerName = _configuration["CloudStorage:ContainerName"];

                if (CloudStorageAccount.TryParse(cloudStorageConnectionString, out CloudStorageAccount storageAccount))
                {
                    CloudStorageAccount cloudStorageAccount = CloudStorageAccount.Parse(cloudStorageConnectionString);
                    CloudBlobClient cloudBlobClient = cloudStorageAccount.CreateCloudBlobClient();
                    CloudBlobContainer cloudBlobContainer = cloudBlobClient.GetContainerReference(cloudStorageContainerName);

                    await cloudBlobContainer.CreateIfNotExistsAsync();

                    var picBlob = cloudBlobContainer.GetBlockBlobReference(file.FileName);

                    await picBlob.UploadFromStreamAsync(file.OpenReadStream());

                    return Ok(picBlob.Uri);
                }

                return StatusCode(StatusCodes.Status500InternalServerError);
            }
            catch (Exception e)
            {
                return StatusCode(500, e.InnerException.Message);
            }
        }
    }
}
