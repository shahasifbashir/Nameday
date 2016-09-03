using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NameDay
{
    public class NamedarModel
    {
        public int month { get; set; }
        public int day { get; set; }
        public IEnumerable<string> names { get; set; }
        public NamedarModel(int month,int day,IEnumerable<string> name)
        {
            this.month = month;
            this.day = day;
            this.names = name;
        }
        public string nameAsString => string.Join(",", names);
    }
}
