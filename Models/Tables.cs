﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace DataGenerator.Models
{
    public class Tables
    {
        public string Name { get; set; }
        public string Type { get; set; }
        public Dictionary<string, int> Options { get; set; }

    }
}
