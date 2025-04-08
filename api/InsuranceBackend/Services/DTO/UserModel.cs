using InsuranceBackend.Enum;
using System;
using System.Collections.Generic;

namespace InsuranceBackend.Services.DTO;

public class UserModel
{
    public int UserId { get; set; }

    public string UserName { get; set; } = null!;

    public string Password { get; set; } = null!;

    public UserTypeEnum Type { get; set; }

    public StatusEnum Status { get; set; }
    
}
