using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Connect4
{
    public class Cell
    {
        public int Row { get; set; }
        public int Column { get; set; }
        public CellState State { get; set; }
    }
}
