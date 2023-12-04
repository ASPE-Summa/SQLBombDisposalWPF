using System;
using System.Collections.Generic;

namespace SQLBombDisposal.Models;

public partial class Student
{
    public int Id { get; set; }

    public string Name { get; set; } = null!;

    public string Grade { get; set; } = null!;

    public int MathScore { get; set; }

    public int EnglishScore { get; set; }

    public int HistoryScore { get; set; }

    public int GeographyScore { get; set; }

    public int ScienceScore { get; set; }

    public int ArtScore { get; set; }
}
