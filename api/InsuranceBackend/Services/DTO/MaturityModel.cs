using System;
using System.Collections.Generic;

namespace InsuranceBackend.Services.DTO;

public class MaturityModel
{
    public int MaturityId { get; set; }

    public int ClientPolicyId { get; set; }

    public string MaturityDate { get; set; } = null!;

    public decimal ClaimAmount { get; set; }

    public string StartDate { get; set; } = null!;    
}
