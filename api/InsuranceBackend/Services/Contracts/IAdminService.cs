using InsuranceBackend.Enum;
using InsuranceBackend.Services.DTO;

namespace InsuranceBackend.Services.Contracts
{
    public interface IAdminService
    {
        Task<List<AgentModel>> GetAllAgent();
        Task<List<CompanyModel>> GetAllCompanies();
        Task<List<ClientModel>> GetAllClient();
        Task<PolicyTypeModel> AddPolicytype(PolicyTypeModel policyType);
        Task ChangeUserStatus(UserModel user);
        Task ChangeActorStatus(int userId, UserTypeEnum type, StatusEnum status);
        Task<List<PolicyModel>> GetAllPolicies(int companyId);
        Task<List<MaturityModel>> GetAllMaturities();
        Task<List<PolicyTypeModel>> GetAllPoliciesTypes();
        Task<List<FeedbackModel>> GetAllFeedbacks();
        Task<List<PolicyTermModel>> GetAllPolicyTerms(int policyId);
        Task ChangePolicyStatus(PolicyModel policy);
        Task<PolicyModel> GetPolicy(int policyId);
    }
}
