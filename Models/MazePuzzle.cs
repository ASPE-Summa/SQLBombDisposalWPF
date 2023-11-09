using System;
using System.Collections.Generic;

namespace SQLBombDisposal.Models;

public partial class MazePuzzle
{
    public int Id { get; set; }

    public string Contents { get; set; } = null!;

    public int Pattern { get; set; }

    public int Sequence { get; set; }
}
