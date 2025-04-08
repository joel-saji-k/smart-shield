using InsuranceBackend.Enum;
using System;
using System.Collections.Generic;
using System.Text.Json.Serialization;

namespace InsuranceBackend.Services.DTO;

public class PolicyModel
{
    public int PolicyId { get; set; }

    public int CompanyId { get; set; }

    public int PolicytypeId { get; set; }

    public string PolicyName { get; set; } = null!;

    public int TimePeriod { get; set; }

    public decimal PolicyAmount { get; set; }

    public StatusEnum Status { get; set; }    
    public IEnumerable<PolicyTermModel> PolicyTerms { get; set; }
}
