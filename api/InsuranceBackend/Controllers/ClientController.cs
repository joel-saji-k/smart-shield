using InsuranceBackend.Models;
using InsuranceBackend.Services;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace InsuranceBackend.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ClientController : ControllerBase
    {
        ClientService _clientService;
        UserService _userService;

        public ClientController()
        {
            _clientService = new ClientService();
            _userService = new UserService();
        }

        [HttpGet]
        [Route("GetClient")]
        public IActionResult GetClientById(int clientId)
        {
            var dbclient = _clientService.GetClient(clientId);
            return Ok(dbclient);
        }

        [HttpGet]
        [Route("GetClientById")]
        public IActionResult GetClient(int userId)
        {
            var dbclient = _clientService.GetClientById(userId);
            return Ok(dbclient);
        }

        [HttpPost]
        [Route("AddNominee")]
        public IActionResult AddNominee()
        {
            NomineeModel nominee = new NomineeModel()
            {
                NomineeName = Request.Form["nomineeName"],
                PhoneNum = Request.Form["phoneNum"],
                Relation = Request.Form["Relation"],
                ClientId = int.Parse(Request.Form["clientId"]),
                Address = Request.Form["address"],
            };

            _clientService.AddNominee(nominee);
            return Ok(nominee);
        }

        [HttpGet]
        [Route("ViewNominee")]
        public IEnumerable<NomineeModel> GetNominees(int clientId)
        {
            var nominees = _clientService.ViewClientNominees(clientId);
            return nominees;
        }

        [HttpGet]
        [Route("ViewPolicies")]
        public IEnumerable<PolicyModel> GetPolicies(int policytypeId = 0, int agentId = 0, int order = 0)
        {
            if (policytypeId != 0 && order != 0)
            {
                return _clientService.GetPolicies(typeId: policytypeId, order: 1);
            }
            else if (agentId != 0 && order != 0)
            {
                return _clientService.GetPolicies(agentId: agentId, order: 1);
            }
            else if (order != 0)
            {
                return _clientService.GetPolicies(order: order);
            }
            else
                return _clientService.GetPolicies();
        }

        [HttpGet]
        [Route("GetTypes")]
        public IEnumerable<PolicyTypeModel> GetTypes()
        {
            return _clientService.GetTypes();
        }

        [HttpGet]
        [Route("GetCompanies")]
        public IEnumerable<CompanyModel> GetCompanies()
        {
            return _clientService.GetCompanies();
        }

        [HttpGet]
        [Route("GetCPolicy")]
        public IActionResult GetCPolicy(int clientpolicyId)
        {
            return Ok(_clientService.GetClientPolicy(clientpolicyId));
        }

        [HttpGet]
        [Route("GetPTerm")]
        public IActionResult GetPterm(int policytermId)
        {
            return Ok(_clientService.GetPolicyTerm(policytermId));
        }

        //ClientPolicies
        [HttpPost]
        [Route("AddClientPolicy")]
        public IActionResult AddClientPolicy()
        {
            ClientPolicyModel clientPolicy =
                new()
                {
                    ClientId = int.Parse(Request.Form["clientId"]),
                    PolicyTermId = int.Parse(Request.Form["policyTermId"]),
                    NomineeId = int.Parse(Request.Form["nomineeId"]),
                    StartDate = Request.Form["startDate"],
                    ExpDate = Request.Form["expDate"],
                    Counter = int.Parse(Request.Form["counter"]),
                    Status = Enum.ClientPolicyStatusEnum.Inactive,
                    Referral = Request.Form["referral"],
                    AgentId = int.Parse(Request.Form["agentId"])
                };
            return Ok(_clientService.AddClientPolicy(clientPolicy));
        }

        [HttpPost]
        [Route("makePayment")]
        public IActionResult MakePayment()
        {
            PaymentModel payment =
                new()
                {
                    ClientPolicyId = int.Parse(Request.Form["clientPolicyId"]),
                    TransactionId = Request.Form["transactionId"],
                    Time = Request.Form["time"],
                    Amount = int.Parse(Request.Form["amount"]),                        
                    Status = Enum.PaymentStatusEnum.Processing
                };
            if (int.Parse(Request.Form["penalty"]) == 0 )
                return Ok(_clientService.MakePayment(payment, 0));
            else
                return Ok(_clientService.MakePayment(payment, 1));
        }

        [HttpGet]
        [Route("ViewMaturity")]
        public IEnumerable<MaturityModel> ViewMaturities(int clientId)
        {
            return _clientService.ViewMaturities(clientId);
        }

        [HttpGet]
        [Route("ValidateReferral")]
        public IActionResult ValidateRef(string referral)
        {
            var res = _clientService.ValidateReferral(referral);
            return Ok(res);
        }

        [HttpGet]
        [Route("GetTerms")]
        public IEnumerable<PolicyTermModel> GetTerms(int policyId)
        {
            var res = _clientService.GetPterms(policyId);
            return res;
        }

        [HttpGet]
        [Route("GetClientPolicies")]
        public IEnumerable<ClientPolicyModel> GetClientPolicies(int clientId)
        {
            return _clientService.GetCPolicies(clientId);
        }

        [HttpGet]
        [Route("GetMaturities")]
        public IEnumerable<MaturityModel> GetMaturities(int clientId)
        {
            return _clientService.GetMaturities(clientId);
        }

        [HttpGet]
        [Route("GetPolicyterm")]
        public IEnumerable<PolicyTermModel> GetPterms(int policytermId)
        {
            return _clientService.GetPterms(policytermId);
        }

        [HttpGet]
        [Route("GetPenalties")]
        public IEnumerable<PremiumModel> GetPenalties(int clientPolicyId)
        {
            return _clientService.ViewPenalties(clientPolicyId);
        }
    }
}
