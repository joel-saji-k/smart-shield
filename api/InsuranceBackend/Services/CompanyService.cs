using InsuranceBackend.Enum;
using System;
using Microsoft.EntityFrameworkCore;
using InsuranceBackend.Models;
using Microsoft.Data.SqlClient;
using InsuranceBackend.Services.Contracts;
using InsuranceBackend.Services.DTO;

namespace InsuranceBackend.Services
{
    public class CompanyService : ICompanyService
    {
        InsuranceDbContext _context;
        IConfiguration _configuration;

        public CompanyService(IConfiguration configuration, InsuranceDbContext dbContext)
        {
            _context = dbContext;
            _configuration = configuration;
        }

        public CompanyModel AddCompany(CompanyModel company)
        {
            try
            {
                _context.Companies.Add(new Company
                {
                    Status = company.Status,
                    Address = company.Address,
                    Email = company.Email,
                    PhoneNum = company.PhoneNum,
                    CompanyId = company.CompanyId,
                    CompanyName = company.CompanyName,
                    ProfilePic = company.ProfilePic,
                    UserId = company.UserId,
                });
                _context.SaveChanges();
            }
            catch (Exception)
            {
                throw new Exception(null);
            }
            return _context.Companies.OrderBy(c => c.CompanyId).Select(x => new CompanyModel
            {
                UserId = x.UserId,
                Address = x.Address,
                ProfilePic = x.ProfilePic,
                CompanyName = x.CompanyName,
                CompanyId = x.CompanyId,
                PhoneNum = x.PhoneNum,
                Email = x.Email,
                Status = x.Status
            }).Last();
        }

        public CompanyModel? GetCompany(int userID)
        {
            var result = _context.Companies.FirstOrDefault(c => c.UserId == userID);
            return new CompanyModel
            {
                Address = result.Address,
                Status = result.Status,
                Email = result.Email,
                PhoneNum = result.PhoneNum,
                CompanyId = result.CompanyId,
                CompanyName = result.CompanyName,
                ProfilePic = result.ProfilePic,
                UserId = result.UserId,
            };
        }

        public CompanyModel GetCompanyByName(string companyName)
        {
            var res = _context.Companies.FirstOrDefault(c => c.CompanyName == companyName);
            return new CompanyModel
            {
                Address = res.Address,
                Status = res.Status,
                Email = res.Email,
                PhoneNum = res.PhoneNum,
                CompanyId = res.CompanyId,
                CompanyName = res.CompanyName,
                ProfilePic = res.ProfilePic,
                UserId = res.UserId,
            } ?? throw new Exception();
        }

        public IEnumerable<CompanyModel> GetAllCompanies()
        {
            return _context.Companies.ToList().Select(x => new CompanyModel
            {
                UserId = x.UserId,
                Address = x.Address,
                CompanyName = x.CompanyName,
                ProfilePic = x.ProfilePic,
                PhoneNum = x.PhoneNum,
                CompanyId = x.CompanyId,
                Email = x.Email,
                Status = x.Status

            });
        }

        public void DeleteCompany(int companyID)
        {
            var dbcompany = GetCompany(companyID);
            _context.Companies.Remove(new Company
            {
                CompanyId = companyID,
                CompanyName = dbcompany.CompanyName,
                ProfilePic = dbcompany.ProfilePic,
                Status = dbcompany.Status,
                Email = dbcompany.Email,
                PhoneNum = dbcompany.PhoneNum,
                Address = dbcompany.Address,
                UserId = dbcompany.UserId,

            });
        }

        public CompanyModel UpdateCompany(int companyID, CompanyModel company)
        {
            if (GetCompany(companyID) == null)
            {
                throw new Exception();
            }
            _context.Companies.Update(new Company
            {
                CompanyId = companyID,
                UserId = company.UserId,
                Address = company.Address,
                PhoneNum = company.PhoneNum,
                Email = company.Email,
                Status = company.Status,
                CompanyName = company.CompanyName,
                ProfilePic = company.ProfilePic,
            });
            _context.SaveChanges();
            return GetCompany(companyID);
        }

        public PolicyModel UpdatePolicy(PolicyModel policy)
        {
            _context.Policies.Update(new Policy
            {
                Status = policy.Status,
                PolicyAmount = policy.PolicyAmount,
                CompanyId = policy.CompanyId,
                PolicyId = policy.PolicyId,
                PolicyName = policy.PolicyName,
                PolicytypeId = policy.PolicytypeId,
                TimePeriod = policy.TimePeriod,

            });
            var pts = _context.PolicyTerms.Where(pt => pt.PolicyId == policy.PolicyId).ToList();
            foreach (var pt in pts)
            {
                var cp = _context.ClientPolicies
                    .Where(cp => cp.PolicyTermId == pt.PolicyTermId)
                    .ToList();
                foreach (var c in cp)
                {
                    var dcp = _context.ClientPolicies.First(
                        c => c.ClientPolicyId == c.ClientPolicyId
                    );
                    dcp.Status = ClientPolicyStatusEnum.Deprecated;
                    _context.ClientPolicies.Update(dcp);
                }
            }
            _context.SaveChanges();
            return _context.Policies.OrderBy(p => p.PolicyId).Select(x => new PolicyModel
            {
                TimePeriod = x.TimePeriod,
                CompanyId = x.CompanyId,
                PolicyAmount = x.PolicyAmount,
                PolicyId = x.PolicyId,
                PolicyName = x.PolicyName,
                Status = x.Status,
                PolicytypeId = x.PolicytypeId,
                PolicyTerms = x.PolicyTerms.Select(y => new PolicyTermModel
                {
                    PolicyId = y.PolicyId,
                    Period = y.Period,
                    PolicyTermId = y.PolicyTermId,
                    PremiumAmount = y.PremiumAmount,
                    Terms = y.Terms
                })
            }).Last();
        }

        public void AddPolicy(PolicyModel policy)
        {
            ValidatePolicy(policy);
            policy.PolicyId = 0;
            policy.Status = (int)StatusEnum.Inactive;
            var con = new SqlConnection(_configuration.GetConnectionString("DefaultConnection"));
            con.Open();
            var cmd = new SqlCommand(
                "INSERT INTO Policies(companyID,policytypeID,policyName,timePeriod,policyAmount,status) VALUES('"
                    + policy.CompanyId
                    + "','"
                    + policy.PolicytypeId
                    + "','"
                    + policy.PolicyName
                    + "','"
                    + policy.TimePeriod
                    + "','"
                    + policy.PolicyAmount
                    + "','"
                    + (int)policy.Status
                    + "')",
                con
            );
            cmd.ExecuteNonQuery();
            var PolicyId = _context.Policies
                .OrderByDescending(p => p.PolicyId)
                .FirstOrDefault()
                .PolicyId;
            var cmd2 = new SqlCommand(
                "INSERT INTO PolicyTerms(policyID,period,terms,premiumAmount) VALUES('"
                    + PolicyId
                    + "','"
                    + policy.TimePeriod
                    + "',1,'"
                    + (float)policy.PolicyAmount * 0.8
                    + "')",
                con
            );
            cmd2.ExecuteNonQuery();
            con.Close();
        }

        public void AddPolicyTerm(PolicyTermModel policyTerm)
        {
            var con = new SqlConnection(_configuration.GetConnectionString("DefaultConnection"));
            con.Open();
            var cmd = new SqlCommand(
                "INSERT INTO PolicyTerms(policyID,period,terms,premiumAmount) VALUES('"
                    + policyTerm.PolicyId
                    + "','"
                    + policyTerm.Period
                    + "','"
                    + policyTerm.Terms
                    + "','"
                    + policyTerm.PremiumAmount
                    + "')",
                con
            );
            cmd.ExecuteNonQuery();
            con.Close();
        }

        public PolicyModel GetPolicy(int policyId)
        {
            Policy? policy = _context.Policies.FirstOrDefault(p => p.PolicyId == policyId);
            return new PolicyModel
            {
                CompanyId = policy.CompanyId,
                PolicyAmount = policy.PolicyAmount,
                PolicyId = policyId,
                PolicyName = policy.PolicyName,
                Status = policy.Status,
                TimePeriod = policy.TimePeriod,
                PolicytypeId = policy.PolicytypeId,
                PolicyTerms = policy.PolicyTerms.Select(x => new PolicyTermModel
                {
                    Period = x.Period,
                    PolicyId = x.PolicyId,
                    PolicyTermId = x.PolicyTermId,
                    PremiumAmount = x.PremiumAmount,
                    Terms = x.Terms
                })
            } ?? throw new NullReferenceException();
        }

        //Status
        public void SetCompanyStatus(int _companyID, ActorStatusEnum e)
        {
            var dbcompany = GetCompany(_companyID);
            if (!ActorStatusEnum.IsDefined(typeof(ActorStatusEnum), e))
            {
                throw new Exception();
            }
            dbcompany.Status = e;
            UpdateCompany(_companyID, dbcompany);
        }

        public void ChangeAgentRequest(int agentID, StatusEnum e)
        {
            ValidateAgentRequest(agentID);
            AgentCompany dbreq =
                _context.AgentCompanies.FirstOrDefault(a => a.AgentId == agentID)
                ?? throw new ArgumentNullException();
            if (!StatusEnum.IsDefined(typeof(StatusEnum), e))
            {
                throw new Exception();
            }
            dbreq.Status = e;
            _context.AgentCompanies.Update(dbreq);
            _context.SaveChanges();
        }

        //Views
        public IEnumerable<AgentCompany> ViewAgents(int companyId)
        {
            return _context.AgentCompanies.Where(a => a.CompanyId == companyId).ToList();
        }

        public IEnumerable<Policy> ViewPolicies(int companyID)
        {
            return _context.Policies.Include(p => p.CompanyId == companyID).ToList();
        }

        public AgentCompanyModel CreateReferral(AgentCompanyModel _agentCompany)
        {
            Random random = new();
            var dbcompany = _context.Companies.First(c => c.CompanyId == _agentCompany.CompanyId);
            var dbagent = _context.Agents.First(a => a.AgentId == _agentCompany.AgentId);
        Retry:
            _agentCompany.Referral =
                dbcompany.CompanyName.Replace(" ", "") + dbagent.AgentName.Replace(" ", "") + random.Next(1, 1000);
            var dbac = _context.AgentCompanies.FirstOrDefault(
                ac => ac.Referral == _agentCompany.Referral
            );
            if (dbac != null)
                goto Retry;
            return _agentCompany;
        }

        //Validations
        private void ValidatePolicy(PolicyModel policy)
        {
            var policytypeID =
                _context.PolicyTypes.Select(p => p.PolicytypeId == policy.PolicytypeId)
                ?? throw new ArgumentNullException(null, nameof(policy));
        }

        private void ValidateAgentRequest(int agentID)
        {
            var dbreq =
                _context.AgentCompanies.FirstOrDefault(s => s.AgentId == agentID)
                ?? throw new NullReferenceException();
        }
    }
}
