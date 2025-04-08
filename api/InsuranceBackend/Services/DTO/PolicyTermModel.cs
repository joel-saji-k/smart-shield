using System;
using System.Collections.Generic;

namespace InsuranceBackend.Services.DTO;

public class PolicyTermModel
{
    public int PolicyTermId { get; set; }

    public int PolicyId { get; set; }

    public int Period { get; set; }

    public int Terms { get; set; }

    public decimal PremiumAmount { get; set; }
    
}
