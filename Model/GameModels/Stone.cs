﻿using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Model.GameModels
{
    public class Stone
    {
        public string Sign { get; set; } = string.Empty;
        public Point Location { get; set; }
        public string State { get; set; } = string.Empty;
    }
}
