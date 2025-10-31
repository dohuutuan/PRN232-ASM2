using System;
using System.Collections.Generic;

namespace FUNewsManagement_CoreAPI.Models;

public partial class RefreshToken
{
    public int Id { get; set; }

    public short UserId { get; set; }

    public string Token { get; set; } = null!;

    public DateTime ExpireAt { get; set; }

    public bool IsRevoke { get; set; }

    public virtual SystemAccount User { get; set; } = null!;
}
