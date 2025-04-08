using System;
using System.Collections.Generic;

namespace InsuranceBackend.Services.DTO;

public class NomineeModel
{
    public int NomineeId { get; set; }

    public int ClientId { get; set; }

    public string NomineeName { get; set; } = null!;

    public string Relation { get; set; } = null!;

    public string Address { get; set; } = null!;

    public string PhoneNum { get; set; } = null!;    
}
