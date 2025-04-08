using InsuranceBackend.Enum;
using InsuranceBackend.Models;
using InsuranceBackend.Services.Contracts;
using InsuranceBackend.Services.DTO;
using Microsoft.EntityFrameworkCore;

namespace InsuranceBackend.Services
{
    public class AdminService(InsuranceDbContext insuranceDb, ILogger<AdminService> logger) : IAdminService
    {
        private readonly InsuranceDbContext _context = insuranceDb;
        private readonly ILogger<AdminService> _logger = logger;

        public async Task<List<AgentModel>> GetAllAgent()
        {
            try
            {
                var agents = await _context.Agents.ToListAsync();
                return [.. agents.Select(x => new AgentModel
                {
                    AgentId = x.AgentId,
                    Address = x.Address,
                    AgentName = x.AgentName,
                    Dob = x.Dob,
                    Email = x.Email,
                    Gender = x.Gender,
                    Grade = x.Grade,
                    PhoneNum = x.PhoneNum,
                    ProfilePic = x.ProfilePic,
                    Status = x.Status,
                    UserId = x.UserId
                })];
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, ex.Message);
                return [];
            }
        }

        public async Task<List<CompanyModel>> GetAllCompanies()
        {
            try
            {
                var companies = await _context.Companies.ToListAsync();
                return [..companies.Select(x => new CompanyModel
            {
                UserId = x.UserId,
                Status = x.Status,
                ProfilePic = x.ProfilePic,
                Address = x.Address,
                CompanyId = x.CompanyId,
                CompanyName = x.CompanyName,
                Email = x.Email,
                PhoneNum = x.PhoneNum,
            })];
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, ex.Message);
                return [];
            }
        }

        public async Task<List<ClientModel>> GetAllClient()
        {
            try
            {
                var clients = await _context.Clients.ToListAsync();
                return [..clients.Select(x => new ClientModel
                {
                    ClientId = x.ClientId,
                    ClientName = x.ClientName,
                    Address = x.Address,
                    Dob = x.Dob,
                    Email = x.Email,
                    Gender = x.Gender,
                    PhoneNum = x.PhoneNum,
                    ProfilePic = x.ProfilePic,
                    Status = x.Status,
                    UserId = x.UserId
                })];
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, ex.Message);
                return [];
            }
        }

        public async Task<PolicyTypeModel> AddPolicytype(PolicyTypeModel policyType)
        {
            try
            {
                var result = await _context.PolicyTypes.AddAsync(new PolicyType
                {
                    PolicytypeId = 0,
                    PolicytypeName = policyType.PolicytypeName,
                });

                await _context.SaveChangesAsync();
                return new PolicyTypeModel
                {
                    PolicytypeId = result.Entity.PolicytypeId,
                    PolicytypeName = result.Entity.PolicytypeName,
                };
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, ex.Message);
                return new PolicyTypeModel();
            }
        }

        //Approval
        public async Task ChangeUserStatus(UserModel user)
        {
            try
            {
                _context.Users.Update(new User
                {
                    UserId = user.UserId,
                    Password = user.Password,
                    Status = user.Status,
                    Type = user.Type,
                    UserName = user.UserName
                });

                await ChangeActorStatus(user.UserId, user.Type, user.Status);
                await _context.SaveChangesAsync();
            }
            catch (Exception ex)
            {
                _logger?.LogError(ex, ex.Message);
            }
        }

        public async Task ChangeActorStatus(int userId, UserTypeEnum type, StatusEnum status)
        {
            try
            {
                switch (type)
                {
                    case UserTypeEnum.Company:
                        {
                            var dbcompany = _context.Companies.First(c => c.UserId == userId);
                            if (dbcompany != null)
                            {
                                dbcompany.Status = (ActorStatusEnum)status;
                                _context.Companies.Update(dbcompany);
                                await _context.SaveChangesAsync();
                            }
                            break;
                        }
                    case UserTypeEnum.Agent:
                        {
                            var dbagent = _context.Agents.First(c => c.UserId == userId);
                            if (dbagent != null)
                            {
                                dbagent.Status = (ActorStatusEnum)status;
                                _context.Agents.Update(dbagent);
                                await _context.SaveChangesAsync();
                            }
                            break;
                        }
                    case UserTypeEnum.Client:
                        {
                            var dbclient = _context.Clients.First(c => c.UserId == userId);
                            if (dbclient != null)
                            {
                                dbclient.Status = (ActorStatusEnum)status;
                                _context.Clients.Update(dbclient);
                                await _context.SaveChangesAsync();
                            }
                            break;
                        }
                    default:
                        break;
                }
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, ex.Message);
            }
        }

        public async Task<List<PolicyModel>> GetAllPolicies()
        {
            try
            {
                var policies = await _context.Policies.ToListAsync();
                return [..policies.Select(x => new PolicyModel
            {
                PolicyId = x.PolicyId,
                Status = x.Status,
                CompanyId = x.CompanyId,
                PolicyAmount = x.PolicyAmount,
                PolicyName = x.PolicyName,
                PolicytypeId = x.PolicytypeId,
                TimePeriod = x.TimePeriod
            })];
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, ex.Message);
                return [];
            }
        }

        public async Task<List<PolicyTypeModel>> GetAllPoliciesTypes()
        {
            try
            {
                var policyTypes = await _context.PolicyTypes.ToListAsync();
                return [..policyTypes.Select(x => new PolicyTypeModel
                {
                    PolicytypeId = x.PolicytypeId,
                    PolicytypeName = x.PolicytypeName,
                })];
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, ex.Message);
                return [];
            }
        }

        public async Task<List<MaturityModel>> GetAllMaturities()
        {
            try
            {
                var maturities = await _context.Maturities.ToListAsync();
                return [..maturities.Select(x => new MaturityModel
                {
                    ClaimAmount = x.ClaimAmount,
                    ClientPolicyId = x.ClientPolicyId,
                    MaturityDate = x.MaturityDate,
                    MaturityId = x.MaturityId,
                    StartDate = x.StartDate
                })];
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, ex.Message);
                return [];
            }
        }

        public async Task<List<FeedbackModel>> GetAllFeedbacks()
        {
            try
            {
                var feedBacks = await _context.Feedbacks.ToListAsync();
                return [..feedBacks.Select(x => new FeedbackModel
                {
                     Feed = x.Feed,
                     Fid = x.Fid,
                })];
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, ex.Message);
                return [];
            }
        }

        public async Task<List<PolicyTermModel>> GetAllPolicyTerms(int policyId)
        {
            try
            {
                var policyTerms = await _context.PolicyTerms.Where(pt => pt.PolicyId == policyId).ToListAsync();
                return [..policyTerms.Select(x => new PolicyTermModel
                {
                   Period = x.Period,
                   PolicyId = x.PolicyId,
                   PolicyTermId = x.PolicyTermId,
                   PremiumAmount = x.PremiumAmount,
                   Terms = x.Terms,
                })];
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, ex.Message);
                return [];
            }
        }

        public async Task ChangePolicyStatus(PolicyModel policy)
        {
            try
            {
                _context.Policies.Update(new Policy
                {
                    CompanyId = policy.CompanyId,
                    PolicyId = policy.PolicyId,
                    PolicyAmount = policy.PolicyAmount,
                    TimePeriod = policy.TimePeriod,
                    PolicytypeId = policy.PolicytypeId,
                    PolicyName = policy.PolicyName,
                    Status = policy.Status
                });
                await _context.SaveChangesAsync();
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, ex.Message);
            }
        }

        public async Task<PolicyModel> GetPolicy(int policyId)
        {
            try
            {
                var policy = await _context.Policies.FirstOrDefaultAsync(x => x.PolicyId == policyId);
                if (policy != null)
                    return new PolicyModel
                    {
                        PolicyId = policy.PolicyId,
                        CompanyId = policy.CompanyId,
                        PolicyAmount = policy.PolicyAmount,
                        PolicyName = policy.PolicyName,
                        PolicytypeId = policy.PolicytypeId,
                        Status = policy.Status,
                        TimePeriod = policy.TimePeriod,
                    };
                else
                    throw new NullReferenceException();
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, ex.Message);
                return new PolicyModel();
            }
        }
    }
}
