using InsuranceBackend.Enum;
using System;
using System.Collections.Generic;

namespace InsuranceBackend.Services.DTO;

public class ClientPolicyModel
{
    public int ClientPolicyId { get; set; }

    public int ClientId { get; set; }

    public int PolicyTermId { get; set; }

    public int NomineeId { get; set; }

    public string StartDate { get; set; } = null!;

    public string ExpDate { get; set; } = null!;

    public int? Counter { get; set; }

    public ClientPolicyStatusEnum Status { get; set; }

    public string Referral { get; set; } = null!;

    public int AgentId { get; set; }    
}
