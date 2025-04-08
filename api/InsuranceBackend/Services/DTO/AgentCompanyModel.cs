using InsuranceBackend.Enum;
using System;
using System.Collections.Generic;

namespace InsuranceBackend.Services.DTO;

public class AgentCompanyModel
{
    public int Id { get; set; }

    public int AgentId { get; set; }

    public int CompanyId { get; set; }

    public string Referral { get; set; } = null!;

    public StatusEnum Status { get; set; }    
}
