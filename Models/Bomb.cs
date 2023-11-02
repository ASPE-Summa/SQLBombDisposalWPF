using System;
using System.Collections.Generic;

namespace SQLBombDisposal.Models;

public partial class Bomb
{
    public int BombId { get; set; }

    public string Name { get; set; } = null!;

    public int Complexity { get; set; }
}
