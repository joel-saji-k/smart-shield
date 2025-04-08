using InsuranceBackend.Enum;
using InsuranceBackend.Models;
using InsuranceBackend.Services.DTO;

namespace InsuranceBackend.Services.Contracts
{
    public interface ICompanyService
    {
        CompanyModel AddCompany(CompanyModel company);
        CompanyModel? GetCompany(int userID);
        CompanyModel GetCompanyByName(string companyName);
        IEnumerable<CompanyModel> GetAllCompanies();
        void DeleteCompany(int companyID);
        CompanyModel UpdateCompany(int companyID, CompanyModel company);
        PolicyModel UpdatePolicy(PolicyModel policy);
        void AddPolicy(PolicyModel policy);
        void AddPolicyTerm(PolicyTermModel policyTerm);
        PolicyModel GetPolicy(int policyId);
        void SetCompanyStatus(int _companyID, ActorStatusEnum e);
        void ChangeAgentRequest(int agentID, StatusEnum e);
        IEnumerable<AgentCompany> ViewAgents(int companyId);
        IEnumerable<Policy> ViewPolicies(int companyID);
        AgentCompanyModel CreateReferral(AgentCompanyModel _agentCompany);
    }
}
