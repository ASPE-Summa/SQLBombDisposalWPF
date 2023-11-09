using System;
using System.Collections.Generic;

namespace SQLBombDisposal.Models;

public partial class ButtonVoltage
{
    public int ButtonId { get; set; }

    public int Voltage { get; set; }

    public virtual Button Button { get; set; } = null!;
}
