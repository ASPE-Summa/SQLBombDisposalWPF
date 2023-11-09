using System;
using System.Collections.Generic;

namespace SQLBombDisposal.Models;

public partial class Button
{
    public int Id { get; set; }

    public string Emoji { get; set; } = null!;

    public string Name { get; set; } = null!;

    public virtual ButtonVoltage? ButtonVoltage { get; set; }
}
