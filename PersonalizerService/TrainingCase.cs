using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AAI {
    public class TrainingCase
    {
        public string Name { get; set; }
        public object[] Features { get; set; }
        public string[] Exclude { get; set; }
        public string Expected { get; set; }
    }
}
