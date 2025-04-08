using InsuranceBackend.Enum;
using InsuranceBackend.Models;
using Microsoft.Data.SqlClient;
using Microsoft.EntityFrameworkCore;
using InsuranceBackend.Services.Contracts;
using InsuranceBackend.Services.DTO;
using System.Threading.Tasks;

namespace InsuranceBackend.Services
{
    public class AgentService(InsuranceDbContext dbContext, ILogger<AgentService> logger, IConfiguration configuration) : IAgentService
    {
        private readonly InsuranceDbContext _context = dbContext;
        private readonly ILogger<AgentService> _logger = logger;
        private readonly IConfiguration _configuration = configuration;

        public async Task<AgentModel> AddAgent(AgentModel agent)
        {
            try
            {
                var con = new SqlConnection(_configuration.GetConnectionString("DefaultConnection"));
                con.Open();
                var cmd = new SqlCommand(
                    "INSERT INTO Agents(userID,agentName,gender,phoneNum,dob,email,address,grade,profilePic,status) VALUES('"
                        + agent.UserId
                        + "','"
                        + agent.AgentName
                        + "','"
                        + agent.Gender
                        + "','"
                        + agent.PhoneNum
                        + "','dob','"
                        + agent.Email
                        + "','Address','1','"
                        + agent.ProfilePic
                        + "',0)",
                    con
                );
                await cmd.ExecuteNonQueryAsync();
                await con.CloseAsync();
                return await GetAgentByName(agent.AgentName);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error adding agent");
                throw;
            }
        }

        public async Task<AgentModel> GetAgent(int agentID)
        {
            try
            {
                var res = await _context.Agents.FirstOrDefaultAsync(a => a.AgentId == agentID);
                if (res != null)
                    return new AgentModel
                    {
                        AgentId = res.AgentId,
                        Address = res.Address,
                        AgentName = res.AgentName,
                        Dob = res.Dob,
                        Email = res.Email,
                        Gender = res.Gender,
                        Grade = res.Grade,
                        PhoneNum = res.PhoneNum,
                        ProfilePic = res.ProfilePic,
                        Status = res.Status,
                        UserId = res.UserId
                    };
                else throw new Exception("Agent not found");
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error fetching agent by ID");
                throw;
            }
        }

        public async Task<AgentModel> GetAgentByName(string agentName)
        {
            try
            {
                var res = await _context.Agents.FirstOrDefaultAsync(a => a.AgentName == agentName);
                if (res != null)
                    return new AgentModel
                    {
                        AgentId = res.AgentId,
                        Address = res.Address,
                        AgentName = agentName,
                        Dob = res.Dob,
                        Email = res.Email,
                        Gender = res.Gender,
                        Grade = res.Grade,
                        PhoneNum = res.PhoneNum,
                        ProfilePic = res.ProfilePic,
                        Status = res.Status,
                        UserId = res.UserId
                    };
                else
                    throw new Exception("Agent not found");
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error fetching agent by name");
                throw;
            }
        }

        public async Task DeleteAgent(int agentID)
        {
            try
            {
                var dbagent = await GetAgent(agentID);
                _context.Agents.Remove(new Agent
                {
                    AgentId = dbagent.AgentId,
                    Address = dbagent.Address,
                    AgentName = dbagent.AgentName,
                    Dob = dbagent.Dob,
                    Email = dbagent.Email,
                    Gender = dbagent.Gender,
                    Grade = dbagent.Grade,
                    PhoneNum = dbagent.PhoneNum,
                    ProfilePic = dbagent.ProfilePic,
                    Status = dbagent.Status,
                    UserId = dbagent.UserId
                });
                await _context.SaveChangesAsync();
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error deleting agent");
                throw;
            }
        }

        public async Task<AgentModel> UpdateAgent(int agentID, AgentModel agent)
        {
            try
            {
                if (GetAgent(agentID) == null)
                    throw new Exception("Agent not found");
                _context.Agents.Update(new Agent
                {
                    AgentId = agent.AgentId,
                    Address = agent.Address,
                    AgentName = agent.AgentName,
                    Dob = agent.Dob,
                    Email = agent.Email,
                    Gender = agent.Gender,
                    Grade = agent.Grade,
                    PhoneNum = agent.PhoneNum,
                    ProfilePic = agent.ProfilePic,
                    Status = agent.Status,
                    UserId = agent.UserId
                });
                await _context.SaveChangesAsync();
                return await GetAgent(agentID);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error updating agent");
                throw;
            }
        }

        public async Task ChangeAgentStatus(int _agentID, ActorStatusEnum e)
        {
            try
            {
                var dbagent = await GetAgent(_agentID);
                if (!System.Enum.IsDefined(typeof(StatusEnum), e))
                    throw new Exception("Invalid status enum");
                dbagent.Status = e;
                await UpdateAgent(_agentID, dbagent);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error changing agent status");
                throw;
            }
        }

        public async Task ChangeClientPolicyStatus(int clientpolicyID, StatusEnum e)
        {
            try
            {
                var dbclientpolicy = _context.ClientPolicies.Find(clientpolicyID);
                if (dbclientpolicy == null)
                    throw new NullReferenceException("Client policy not found");
                if (!System.Enum.IsDefined(typeof(StatusEnum), e))
                    throw new Exception("Invalid status enum");
                dbclientpolicy.Status = (ClientPolicyStatusEnum)e;
                _context.ClientPolicies.Update(dbclientpolicy);
                await _context.SaveChangesAsync();
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error changing client policy status");
                throw;
            }
        }

        public async Task<ClientDeathModel> AddClientDeath(ClientDeathModel clientDeath)
        {
            try
            {
                var dbcd = _context.ClientDeaths.FirstOrDefault(cd => cd.ClientPolicyId == clientDeath.ClientPolicyId);
                if (dbcd != null) return new ClientDeathModel
                {
                    ClaimAmount = dbcd.ClaimAmount,
                    ClientDeathId = dbcd.ClientDeathId,
                    ClientPolicyId = dbcd.ClientPolicyId,
                    Dod = dbcd.Dod,
                    StartDate = dbcd.StartDate
                };
                if (ValidateClientPolicy(clientDeath.ClientPolicyId))
                {
                    _context.ClientDeaths.Add(new ClientDeath
                    {
                        ClientPolicyId = clientDeath.ClientPolicyId,
                        ClaimAmount = clientDeath.ClaimAmount,
                        ClientDeathId = clientDeath.ClientDeathId,
                        Dod = clientDeath.Dod,
                        StartDate = clientDeath.StartDate
                    });

                    var clientPolicy = _context.ClientPolicies.First(p => p.ClientPolicyId == clientDeath.ClientPolicyId);
                    if (clientPolicy != null)
                    {
                        clientPolicy.Status = ClientPolicyStatusEnum.Mature;
                        _context.ClientPolicies.Update(clientPolicy);
                    }

                    await _context.SaveChangesAsync();
                    var result = _context.ClientDeaths.OrderBy(d => d.ClientDeathId).LastOrDefault();
                    if (result != null)
                        return new ClientDeathModel
                        {
                            StartDate = result.StartDate,
                            ClientDeathId = result.ClientDeathId,
                            ClaimAmount = result.ClaimAmount,
                            ClientPolicyId = result.ClientPolicyId,
                            Dod = result.Dod
                        };
                    else
                        throw new Exception("Client Death Failed");
                }
                else
                    throw new Exception("Client Death Ineligible");
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error adding client death");
                throw;
            }
        }

        public async Task<MaturityModel> AddMaturity(MaturityModel maturity)
        {
            try
            {
                var dbm = _context.Maturities.FirstOrDefault(m => m.ClientPolicyId == maturity.ClientPolicyId);
                if (dbm != null)
                {
                    return new MaturityModel
                    {
                        MaturityId = dbm.MaturityId,
                        MaturityDate = dbm.MaturityDate,
                        ClaimAmount = dbm.ClaimAmount,
                        ClientPolicyId = dbm.ClientPolicyId,
                        StartDate = dbm.StartDate
                    };
                }

                ValidateClientPolicy(maturity.ClientPolicyId);
                _context.Maturities.Add(new Maturity
                {
                    StartDate = maturity.StartDate,
                    ClaimAmount = maturity.ClaimAmount,
                    ClientPolicyId = maturity.ClientPolicyId,
                    MaturityDate = maturity.MaturityDate,
                    MaturityId = maturity.MaturityId,
                });

                var clientPolicy = _context.ClientPolicies.FirstOrDefault(p => p.ClientPolicyId == maturity.ClientPolicyId);
                if (clientPolicy != null)
                {
                    clientPolicy.Status = ClientPolicyStatusEnum.Mature;
                    _context.ClientPolicies.Update(clientPolicy);
                }

                _context.SaveChanges();

                var result = await _context.Maturities.OrderBy(m => m.MaturityId).LastOrDefaultAsync();
                return result != null
                    ? new MaturityModel
                    {
                        MaturityId = result.MaturityId,
                        ClaimAmount = result.ClaimAmount,
                        ClientPolicyId = result.ClientPolicyId,
                        MaturityDate = result.MaturityDate,
                        StartDate = result.StartDate
                    }
                    : new MaturityModel();
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error adding maturity");
                throw;
            }
        }

        public async Task<PremiumModel> AddPenalty(PremiumModel premium)
        {
            try
            {
                ValidateClientPolicy(premium.ClientPolicyId);
                _context.Premia.Add(new Premium
                {
                    ClientPolicyId = premium.ClientPolicyId,
                    DateOfCollection = DateTime.Now.ToShortDateString(),
                    PremiumId = premium.PremiumId,
                    Penalty = premium.Penalty,
                    Status = premium.Status
                });

                var clientPolicy = _context.ClientPolicies.FirstOrDefault(p => p.ClientPolicyId == premium.ClientPolicyId);
                if (clientPolicy != null)
                    premium.Status = PenaltyStatusEnum.Pending;

                _context.SaveChanges();

                var result = await _context.Premia.OrderBy(p => p.PremiumId).LastOrDefaultAsync();
                return result != null
                    ? new PremiumModel
                    {
                        ClientPolicyId = result.ClientPolicyId,
                        DateOfCollection = result.DateOfCollection,
                        Penalty = result.Penalty,
                        PremiumId = result.PremiumId,
                        Status = result.Status
                    }
                    : new PremiumModel();
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error adding penalty");
                throw;
            }
        }

        public async Task<IEnumerable<PolicyModel>> ViewPolicies(int agentID)
        {
            try
            {
                ValidateAgent(agentID);
                var dbagentcompany = await _context.AgentCompanies
                    .Where(e => e.AgentId == agentID)
                    .ToListAsync();
                var companiesIDs = dbagentcompany.Select(e => e.CompanyId).ToList();
                List<PolicyModel> policies = [];
                foreach (var compID in companiesIDs)
                {
                    var policy = _context.Policies.FirstOrDefault(p => p.CompanyId == compID && p.Status == StatusEnum.Active);
                    if (policy != null)
                        policies.Add(new PolicyModel
                        {
                            PolicyId = policy.PolicyId,
                            CompanyId = policy.CompanyId,
                            PolicyName = policy.PolicyName,
                            TimePeriod = policy.TimePeriod,
                            PolicyTerms = policy.PolicyTerms.Select(x => new PolicyTermModel
                            {
                                Period = x.Period,
                                PolicyId = x.PolicyId,
                                PolicyTermId = x.PolicyTermId,
                                PremiumAmount = x.PremiumAmount,
                                Terms = x.Terms
                            }),
                            PolicyAmount = policy.PolicyAmount,
                            PolicytypeId = policy.PolicytypeId,
                            Status = StatusEnum.Active,
                        });
                }
                return policies;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error viewing policies");
                throw;
            }
        }

        public async Task RequestCompany(int companyID, int agentID)
        {
            try
            {
                _context.AgentCompanies.Add(
                    new AgentCompany { AgentId = agentID, CompanyId = companyID }
                );
                await _context.SaveChangesAsync();
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error requesting company");
                throw;
            }
        }

        public async Task<List<int>> GetClientsbyAgent(int agentID)
        {
            try
            {
                return await _context.ClientPolicies
                    .Where(c => c.AgentId == agentID)
                    .Select(c => c.Client.ClientId)
                    .ToListAsync();
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error getting clients by agent");
                throw;
            }
        }

        public async Task<PolicyTermModel> GetPolicyTerm(int policyTermId)
        {
            try
            {
                var result = await _context.PolicyTerms.FirstOrDefaultAsync(pt => pt.PolicyTermId == policyTermId);
                if (result != null)
                    return new PolicyTermModel
                    {
                        Period = result.Period,
                        PolicyId = result.PolicyId,
                        PolicyTermId = result.PolicyTermId,
                        PremiumAmount = result.PremiumAmount,
                        Terms = result.Terms
                    };
                else
                    throw new Exception("No Policy Term Found");
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, ex.Message);
                return new PolicyTermModel();
            }
        }

        public async Task<List<PolicyModel>> GetPolicies(int companyId)
        {
            try
            {
                var result = await _context.Policies.Where(a => a.CompanyId == companyId && a.Status == StatusEnum.Active).ToListAsync();
                return [.. result.Select(x => new PolicyModel
                {
                    CompanyId = x.CompanyId,
                    PolicyAmount = x.PolicyAmount,
                    PolicyId= x.PolicyId,
                    PolicyName = x.PolicyName,
                    PolicytypeId = x.PolicytypeId,
                    Status = x.Status,
                    TimePeriod = x.TimePeriod,
                })];
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, ex.Message);
                return [];
            }
        }

        //Validators

        private bool ValidateAgent(int agentID)
        {
            return _context.Agents.FirstOrDefault(a => a.AgentId == agentID) != null;
        }

        private bool ValidateClientPolicy(int? clientPolicyId)
        {

            return (_context.ClientPolicies.FirstOrDefault(cp => cp.ClientPolicyId == clientPolicyId)
                != null && _context.Payments.Where(x => x.ClientPolicyId == clientPolicyId).Any());
        }
    }
}
