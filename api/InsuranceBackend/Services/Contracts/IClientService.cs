using InsuranceBackend.Enum;
using InsuranceBackend.Models;
using InsuranceBackend.Services.DTO;

namespace InsuranceBackend.Services.Contracts
{
    public interface IClientService
    {
        Client GetClient(int clientID);
        Client? GetClientById(int userId);
        Client GetClientByName(string userName);
        IEnumerable<Client> GetAllClient();
        void DeleteClient(int clientID);
        Client UpdateClient(int clientID, Client client);
        Client AddClient(Client client);

        IEnumerable<ClientPolicyModel> GetCPolicies(int clientID);
        IEnumerable<Maturity> GetMaturities(int clientID);
        void ChangeClientStatus(int _clientID, ActorStatusEnum e);

        ClientPolicy AddClientPolicy(ClientPolicy clientPolicy);
        void AddClientDeath(ClientDeath clientDeath);
        void AddPolicyMaturity(Maturity maturity);
        void AddPolicyPremium(Premium premium);

        IEnumerable<PolicyTerm> GetPterms(int policyId);
        Payment MakePayment(Payment payment, int penalty);
        AgentCompany ValidateReferral(string referral);

        IEnumerable<PolicyModel> GetPolicies(int typeId = 0, int order = 0, int agentId = 0);
        ClientPolicy GetClientPolicy(int clientpolicyId);
        PolicyTerm GetPolicyTerm(int policytermId);

        IEnumerable<PolicyType> GetTypes();
        IEnumerable<CompanyModel> GetCompanies();

        void AddNominee(Nominee nominee);

        IEnumerable<Maturity> ViewMaturities(int clientID);
        IEnumerable<Premium> ViewPenalties(int clientpolicyId);
        List<ClientDeath> ViewClientDeath(int clientID);
        IEnumerable<Nominee> ViewClientNominees(int clientID);
    }
}
