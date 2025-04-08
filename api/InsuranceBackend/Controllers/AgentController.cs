using InsuranceBackend.Enum;
using InsuranceBackend.Models;
using InsuranceBackend.Services;
using InsuranceBackend.Services.Contracts;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using InsuranceBackend.Services.DTO;
using System.Threading.Tasks;

namespace Insurance.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AgentController(IAgentService agentService, IUserService userService, InsuranceDbContext dbContext) : ControllerBase
    {
        private readonly IAgentService _agentServices = agentService;
        private readonly IUserService _userService = userService;
        private readonly InsuranceDbContext _dbContext = dbContext;

        [HttpGet]
        [Route("GetPolicyTerms")]
        public async Task<IActionResult> GetTerms(int policytermId)
        {
            return Ok( await _agentServices.GetPolicyTerm(policytermId));
        }

        [HttpGet]
        [Route("GetPolicies")]
        public async Task<IActionResult> Getpolicies(int companyId)
        {            
            var res = await _agentServices.GetPolicies(companyId);
            return Ok(res);
        }

        [HttpGet]
        [Route("GetClientPolicies")]
        public IEnumerable<ClientPolicyModel> GetClientPolicies(int agentId)
        {
            var res = _dbContext.ClientPolicies.Where(cp => cp.AgentId == agentId).ToList();
            return res;
        }

        [HttpGet]
        [Route("GetAgent")]
        public IActionResult GetAgent(int userId)
        {
            var res = _dbContext.Agents.FirstOrDefault(a => a.UserId == userId);
            return Ok(res);
        }

        [HttpGet]
        [Route("GetAgentforClient")]
        public IActionResult GetAgentforClient(int agentId)
        {
            return Ok(_dbContext.Agents.First(a => a.AgentId == agentId));
        }

        [HttpGet]
        [Route("GetPolicy")]
        public IActionResult GetPolicy(int policyId)
        {
            var res = _dbContext.Policies.FirstOrDefault(p => p.PolicyId == policyId);
            return Ok(res);
        }

        [HttpGet]
        [Route("GetAgentsById")]
        public IEnumerable<Agent> GetAgentsByCompany(int companyId)
        {
            var agents = _dbContext.AgentCompanies
                .Where(ac => ac.CompanyId == companyId)
                .Select(a => a.AgentId)
                .ToList();
            List<Agent> result = new List<Agent>();
            foreach (var agentid in agents)
            {
                result.Add(_dbContext.Agents.First(a => a.AgentId == agentid));
            }
            return result;
        }

        [HttpGet]
        [Route("GetRefs")]
        public IActionResult GetRefs(int agentId, int companyId)
        {
            var a = _dbContext.AgentCompanies.First(
                ac => ac.CompanyId == companyId && ac.AgentId == agentId
            );
            return Ok(a);
        }

        [HttpGet]
        [Route("GetCompanies")]
        public IEnumerable<CompanyModel> GetCompaniesby(int agentId)
        {
            var companies = _dbContext.AgentCompanies
                .Where(ac => ac.AgentId == agentId && ac.Status == StatusEnum.Active)
                .Select(a => a.CompanyId)
                .ToList();
            List<CompanyModel> result = new();
            foreach (var companyid in companies)
            {
                var res = _dbContext.Companies.First(c => c.CompanyId == companyid);
                result.Add(res);
            }
            return result;
        }

        [HttpGet]
        [Route("GetClients")]
        public IEnumerable<ClientModel> GetClients(int agentId)
        {
            var clients = _dbContext.ClientPolicies
                .Where(cp => cp.AgentId == agentId)
                .Select(cp => cp.ClientId)
                .ToList();
            List<Client> result = new();
            foreach (var clientid in clients)
            {
                result.Add(
                    _dbContext.Clients.First(
                        c => c.ClientId == clientid && c.Status == ActorStatusEnum.Approved
                    )
                );
            }
            return result.Select(x => new ClientModel
            {
                ClientId = x.ClientId,
                Status = x.Status,
                Address = x.Address,
                ClientName = x.ClientName,
                Dob = x.Dob,
                Email = x.Email,
                Gender = x.Gender,
                PhoneNum = x.PhoneNum,
                ProfilePic = x.ProfilePic,
                UserId = x.UserId
            });
        }

        [HttpPost]
        [Route("ApplyCompany")]
        public IActionResult Apply()
        {
            AgentCompanyModel agentCompany = new();
            agentCompany.CompanyId = int.Parse(Request.Form["companyId"]);
            agentCompany.AgentId = int.Parse(Request.Form["agentId"]);
            var logagentCompany = _dbContext.AgentCompanies.FirstOrDefault(
                a => a.AgentId == agentCompany.AgentId
            );
            if (logagentCompany != null)
            {
                if (logagentCompany.CompanyId == agentCompany.CompanyId)
                {
                    return BadRequest();
                }
            }
            else
            {
                agentCompany.Status = StatusEnum.Inactive;
                agentCompany.Referral = "REFERRAL";
                _dbContext.AgentCompanies.Add(agentCompany);
            }
            _dbContext.SaveChanges();
            return Ok();
        }

        [HttpPut]
        [Route("ChangeCpolicyStatus")]
        public IActionResult ChangeStatus()
        {
            int status = int.Parse(Request.Form["status"]);
            int clientpolicyId = int.Parse(Request.Form["clientpolicyId"]);
            ClientPolicyModel dbcp = _dbContext.ClientPolicies.FirstOrDefault(
                cp => cp.ClientPolicyId == clientpolicyId
            );
            if (dbcp != null)
            {
                dbcp.Status = (ClientPolicyStatusEnum)status;
                _dbContext.ClientPolicies.Update(dbcp);
            }
            _dbContext.SaveChanges();
            return Ok(dbcp);
        }

        [HttpPost]
        [Route("AddClientDeath")]
        public IActionResult AddClientDeath()
        {
            ClientDeath clientDeath =
                new()
                {
                    ClientDeathId = 0,
                    ClientPolicyId = int.Parse(Request.Form["clientPolicyId"]),
                    Dod = Request.Form["dod"],
                    StartDate = Request.Form["startDate"],
                    ClaimAmount = int.Parse(Request.Form["claimAmount"])
                };
            _agentServices.AddClientDeath(clientDeath);
            return Ok(clientDeath);
        }

        [HttpPost]
        [Route("AddMaturity")]
        public IActionResult AddMaturity()
        {
            MaturityModel maturity =
                new()
                {
                    ClientPolicyId = int.Parse(Request.Form["clientPolicyId"]),
                    MaturityDate = Request.Form["maturityDate"],
                    ClaimAmount = int.Parse(Request.Form["claimAmount"]),
                    StartDate = Request.Form["startDate"]
                };
            return Ok(_agentServices.AddMaturity(maturity));
        }

        [HttpPost]
        [Route("AddPenalty")]
        public IActionResult AddPenalty()
        {
            PremiumModel premium =
                new()
                {
                    ClientPolicyId = int.Parse(Request.Form["clientPolicyId"]),
                    DateOfCollection = Request.Form["dateOfCollection"],
                    Penalty = int.Parse(Request.Form["penalty"]),
                    Status = PenaltyStatusEnum.Pending
                };
            return Ok(_agentServices.AddPenalty(premium));
        }
    }
}
