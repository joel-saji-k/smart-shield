using System;
using System.Collections.Generic;

namespace InsuranceBackend.Services.DTO;

public class ClientDeathModel
{
    public int ClientDeathId { get; set; }

    public int ClientPolicyId { get; set; }

    public string Dod { get; set; } = null!;

    public string StartDate { get; set; } = null!;

    public decimal ClaimAmount { get; set; }    
}
