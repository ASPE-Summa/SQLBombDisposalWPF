using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SQLBombDisposal.Pages.Puzzles
{
    internal interface IPuzzle
    {
        event EventHandler<EventArgs> PuzzleCompleted;
        event EventHandler<EventArgs> TimePenalty;
    }
}
