using InsuranceBackend.Enum;
using InsuranceBackend.Models;
using InsuranceBackend.Services;
using InsuranceBackend.Services.Contracts;
using InsuranceBackend.Services.DTO;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace InsuranceBackend.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class CompanyController(ICompanyService companyService, IUserService userService, InsuranceDbContext dbContext) : ControllerBase
    {
        readonly InsuranceDbContext _dbContext = dbContext;

        [HttpGet]
        [Route("GetPolicy")]
        public IActionResult GetPolicy(int policyId)
        {
            PolicyModel policy = companyService.GetPolicy(policyId);
            return Ok(policy);
        }

        [HttpPost]
        [Route("AddPolicy")]
        public IActionResult AddPolicy()
        {
            PolicyModel policy = new();

            policy.PolicyId = int.Parse(Request.Form["policyId"]);
            policy.CompanyId = int.Parse(Request.Form["companyId"]);
            policy.PolicyName = Request.Form["policyName"];
            policy.PolicytypeId = int.Parse(Request.Form["policytypeId"]);
            policy.PolicyAmount = int.Parse(Request.Form["policyAmount"]);
            policy.TimePeriod = int.Parse(Request.Form["timePeriod"]);
            policy.Status = (int)StatusEnum.Inactive;

            if (policy == null)
                throw new ArgumentNullException(nameof(policy));

            companyService.AddPolicy(policy);
            return Ok(policy);
        }

        [HttpPost]
        [Route("AddPolicyTerm")]
        public IActionResult AddPolicyTerm()
        {
            PolicyTermModel policyterm =
                new()
                {
                    PolicyId = int.Parse(Request.Form["policyId"]),
                    Terms = int.Parse(Request.Form["terms"]),
                    PremiumAmount = int.Parse(Request.Form["premiumAmount"]),
                    Period = int.Parse(Request.Form["period"])
                };

            if (policyterm == null)
                throw new ArgumentNullException(nameof(policyterm));
            companyService.AddPolicyTerm(policyterm);
            return Ok(policyterm);
        }

        [HttpGet]
        [Route("ViewPolicies")]
        public IEnumerable<Policy> ViewPolicies(int companyID)
        {
            return companyService.ViewPolicies(companyID);
        }

        [HttpGet]
        [Route("ViewAgents")]
        public IEnumerable<AgentCompany> ViewAgents(int companyId)
        {
            return companyService.ViewAgents(companyId);
        }

        [HttpGet]
        [Route("GetCompany")]
        public CompanyModel GetCompany(int userID)
        {
            return companyService.GetCompany(userID);
        }

        [HttpGet]
        [Route("GetAllCompany")]
        public IEnumerable<CompanyModel> GetAll()
        {
            return companyService.GetAllCompanies();
        }

        [HttpPost]
        [Route("ChangeAgentCompanyStatus")]
        public IActionResult Change()
        {
            int id = int.Parse(Request.Form["id"]);
            int status = int.Parse(Request.Form["status"]);
            AgentCompany agentCompany = new();
            agentCompany = _dbContext.AgentCompanies.First(ac => ac.Id == id);
            agentCompany.Status = (StatusEnum)status;
            if (status == 0)
            {
                _dbContext.AgentCompanies.Update(agentCompany);
                _dbContext.SaveChanges();
                return Ok(agentCompany);
            }
            else
            {
                var a = companyService.CreateReferral(new AgentCompanyModel
                {
                    AgentId = agentCompany.AgentId,
                    Id = id,
                    CompanyId = agentCompany.CompanyId,
                    Referral = agentCompany.Referral,
                    Status = agentCompany.Status
                });
                var local = _dbContext.AgentCompanies.Local.FirstOrDefault(e => e.Id == a.Id);
                if (local != null)
                {
                    _dbContext.Entry(local).State = EntityState.Detached;
                }
                _dbContext.AgentCompanies.Update(new AgentCompany
                {
                    AgentId= a.AgentId,
                    Id= a.Id,
                    Status = a.Status,
                    Referral= a.Referral,
                    CompanyId= a.CompanyId,
                });
                _dbContext.SaveChanges();
                return Ok(agentCompany);
            }
        }

        [HttpPut]
        [Route("ChangePolicyStatus")]
        public IActionResult PStatusChange()
        {
            int cpid = int.Parse(Request.Form["policyId"]);
            StatusEnum status = (StatusEnum)int.Parse(Request.Form["status"]);
            var dbpolicy = companyService.GetPolicy(cpid);
            dbpolicy.Status = status;
            return Ok(companyService.UpdatePolicy(dbpolicy));
        }

        [HttpGet]
        [Route("GetAgentCompany")]
        public IActionResult GetAgentCompany(int id)
        {
            return Ok(_dbContext.AgentCompanies.FirstOrDefault(ac => ac.Id == id));
        }
    }
}
