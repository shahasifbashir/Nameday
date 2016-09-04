using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NameDay
{
    /// <summary>
    /// This class is the model for how the data will look like 
    /// </summary>
    public class NamedarModel
    {
        public int Month { get; set; }
        public int Day { get; set; }
        public IEnumerable<string> Names { get; set; }
        public NamedarModel(int month,int day,IEnumerable<string> name)
        {
            Month = month;
            Day = day;
            Names = name;
        }
        public string nameAsString => string.Join(",", Names);
        public NamedarModel() { }
    }
}
