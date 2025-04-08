using InsuranceBackend.Enum;
using System;
using System.Collections.Generic;

namespace InsuranceBackend.Services.DTO;

public class ClientModel
{
    public int ClientId { get; set; }

    public int UserId { get; set; }

    public string ClientName { get; set; } = null!;

    public string Gender { get; set; } = null!;

    public string Dob { get; set; } = null!;

    public string Address { get; set; } = null!;

    public string ProfilePic { get; set; } = null!;

    public string PhoneNum { get; set; } = null!;

    public string Email { get; set; } = null!;

    public ActorStatusEnum Status { get; set; }
    
}
