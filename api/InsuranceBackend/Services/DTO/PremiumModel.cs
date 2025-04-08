using InsuranceBackend.Enum;
using System;
using System.Collections.Generic;

namespace InsuranceBackend.Services.DTO;

public class PremiumModel
{
    public int PremiumId { get; set; }

    public int ClientPolicyId { get; set; }

    public string DateOfCollection { get; set; }

    public decimal Penalty { get; set; }

    public PenaltyStatusEnum Status { get; set; }    
}
