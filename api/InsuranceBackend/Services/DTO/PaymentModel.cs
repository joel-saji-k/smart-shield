using InsuranceBackend.Enum;
using System;
using System.Collections.Generic;

namespace InsuranceBackend.Services.DTO;

public class PaymentModel
{
    public int PaymentId { get; set; }

    public int ClientPolicyId { get; set; }

    public string? TransactionId { get; set; }

    public string Time { get; set; } = null!;

    public decimal Amount { get; set; }
    
}
