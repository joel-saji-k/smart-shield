using InsuranceBackend.Enum;
using System;
using System.Collections.Generic;

namespace InsuranceBackend.Services.DTO;

public class CompanyModel
{
    public int CompanyId { get; set; }

    public int UserId { get; set; }

    public string CompanyName { get; set; } = null!;

    public string Address { get; set; } = null!;

    public string Email { get; set; } = null!;

    public string PhoneNum { get; set; } = null!;

    public string ProfilePic { get; set; } = null!;

    public ActorStatusEnum Status { get; set; }    
}
