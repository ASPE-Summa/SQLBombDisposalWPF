using System;
using System.Collections.Generic;

namespace SQLBombDisposal.Models;

public partial class Battleship
{
    public int Id { get; set; }

    public string Coordinates { get; set; } = null!;

    public string Description { get; set; } = null!;
}
