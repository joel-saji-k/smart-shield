using InsuranceBackend.Enum;
using InsuranceBackend.Models;
using InsuranceBackend.Services.Contracts;
using InsuranceBackend.Services.DTO;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace InsuranceBackend.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AdminController(IAdminService adminService) : ControllerBase
    {        
        private readonly IAdminService _adminService = adminService;

        [HttpPut]
        [Route("ChangeUserStatus")]
        public async Task<IActionResult> ChangeStatus(UserModel user)
        {
            try
            {
                await _adminService.ChangeUserStatus(user);
                return Ok(user);
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }

        [HttpPost]
        [Route("AddPolicyType")]
        public async Task<IActionResult> AddType(PolicyTypeModel policyType)
        {
            try
            {
                var result = await _adminService.AddPolicytype(policyType);
                return Ok(result);
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }

        [HttpGet]
        [Route("GetAllTypes")]
        public async Task<IActionResult> GetAllTypes()
        {
            try
            {
                var result = await _adminService.GetAllPoliciesTypes();
                return Ok(result);
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }

        [HttpGet]
        [Route("GetAllAgents")]
        public async Task<IActionResult> GetAllAgent()
        {
            try
            {
                var result = await _adminService.GetAllAgent();
                return Ok(result);
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }

        [HttpGet]
        [Route("GetAllPolicies")]
        public async Task<IActionResult> GetAll(int companyId)
        {
            try
            {
                var result = await _adminService.GetAllPolicies(companyId);
                return Ok(result);
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }

        [HttpGet]
        [Route("GetAllMaturities")]
        public async Task<IActionResult> GetAllMaturities()
        {
            try
            {
                var result = await _adminService.GetAllMaturities();
                return Ok(result);
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }

        [HttpGet]
        [Route("GetFeedbacks")]
        public async Task<IActionResult> GetFeeds()
        {
            try
            {
                var result = await _adminService.GetAllFeedbacks();
                return Ok(result);
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }

        [HttpGet]
        [Route("GetPolicyTerms")]
        public async Task<IActionResult> GetTerms(int policyId)
        {
            try
            {
                var result = await _adminService.GetAllPolicyTerms(policyId);
                return Ok(result);
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }

        [HttpPut]
        [Route("ChangePolicyStatus")]
        public async Task<IActionResult> UpdatePolicy([FromForm] PolicyModel model)
        {
            try
            {
                await _adminService.ChangePolicyStatus(new PolicyModel
                {
                    PolicyId = model.PolicyId,
                    CompanyId = model.CompanyId,
                    PolicyAmount = model.PolicyAmount,
                    PolicyName = model.PolicyName,
                    PolicytypeId = model.PolicytypeId,
                    Status = model.Status,
                    TimePeriod = model.TimePeriod
                });
                return Ok(model);
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }

        [HttpGet]
        [Route("GetPolicy")]
        public async Task<IActionResult> GetPolicy(int policyId)
        {
            try
            {
                var result = await _adminService.GetPolicy(policyId);
                return Ok(result);
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }
    }
}
