using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RetroFinder.Models
{
    public record Position
    {
        public int Score { get; set; }
        public (int row, int col) Indeces { get; set; }
        public Position Ancestor { get; set; }
    }
}
