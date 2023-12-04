using System;
using System.Collections.Generic;

namespace SQLBombDisposal.Models;

public partial class Adventurer
{
    public int Id { get; set; }

    public string Name { get; set; } = null!;

    public int Level { get; set; }

    public string Class { get; set; } = null!;

    public int Strength { get; set; }

    public int Dexterity { get; set; }

    public int Constitution { get; set; }

    public int Intelligence { get; set; }

    public int Wisdom { get; set; }

    public int Charisma { get; set; }
}
