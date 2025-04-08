using InsuranceBackend.Enum;
using InsuranceBackend.Services.DTO;

namespace InsuranceBackend.Services.Contracts
{
    public interface IAgentService
    {
        Task<AgentModel> AddAgent(AgentModel agent);
        Task<AgentModel> GetAgent(int agentID);
        Task<AgentModel> GetAgentByName(string agentName);
        Task DeleteAgent(int agentID);
        Task<AgentModel> UpdateAgent(int agentID, AgentModel agent);
        Task ChangeAgentStatus(int _agentID, ActorStatusEnum e);
        Task ChangeClientPolicyStatus(int clientpolicyID, StatusEnum e);
        Task<ClientDeathModel> AddClientDeath(ClientDeathModel clientDeath);
        Task<MaturityModel> AddMaturity(MaturityModel maturity);
        Task<PremiumModel> AddPenalty(PremiumModel premium);
        Task<IEnumerable<PolicyModel>> ViewPolicies(int agentID);
        Task RequestCompany(int companyID, int agentID);
        Task<List<int>> GetClientsbyAgent(int agentID);
        Task<PolicyTermModel> GetPolicyTerm(int policyTermId);
        Task<List<PolicyModel>> GetPolicies(int companyId);
    }
}
