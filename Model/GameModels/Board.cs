using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Model.GameModels
{
    public class Board
    {
        public int RowCount { get; set; }
        public int ColumnCount { get; set; }
        public int BoardSize => RowCount * ColumnCount;
    }
}
