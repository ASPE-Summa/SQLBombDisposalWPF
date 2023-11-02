using System;
using System.Collections.Generic;

namespace SQLBombDisposal.Models;

public partial class MazePuzzle
{
    public string? Contents { get; set; }

    public int Pattern { get; set; }

    public int Sequence { get; set; }
}
