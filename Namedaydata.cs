using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NameDay
{
    public class Namedaydata:INotifyPropertyChanged
    {
        private string _greeting ="Hello World!"; 
        public string greeting
        {
            get
            {
                return _greeting;
            }
            set
            {
                if (value == _greeting)
                    return;
                else
                    _greeting = value;
                if (PropertyChanged != null)
                    PropertyChanged(this, new PropertyChangedEventArgs("greeting"));
            }
        }
        private List<NamedarModel> _Namedays = new List<NamedarModel>();
        public ObservableCollection<NamedarModel> Namedays { get; set; }
        public Namedaydata()
        {
            Namedays = new ObservableCollection<NamedarModel>();
            for(int month=1;month<=12;month++)
            {
                _Namedays.Add(new NamedarModel(month, 1, new string[] { "Asif" }));
                _Namedays.Add(new NamedarModel(month, 24, new string[] { "bashir","Shah" }));
            }
            performFiltering();
        }
        private NamedarModel  _selectedNameday;

        public event PropertyChangedEventHandler PropertyChanged;

        public NamedarModel  selectedNameday
        {
            get { return _selectedNameday; }
            set
            {
                _selectedNameday = value;
                if (value == null)
                    greeting = "Hello World!";
                else
                    greeting = "Hello " + value.nameAsString;

            }
        }
        private string _filter;

        public string filter
        {
            get { return _filter; }
            set
            {
                if (value == _filter)
                    return;
                _filter = value;
                PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(nameof(filter)));
                performFiltering();
            }
        }

        private void performFiltering()
        {
            if (_filter == null)
                _filter = "";
            var newfilter = filter.ToLower().Trim();
            var result = _Namedays.Where(d => d.nameAsString.ToLowerInvariant().Contains(newfilter)).ToList();
            var toremove = _Namedays.Except(result).ToList();
            foreach (var x in result)
                Namedays.Remove(x);
            var resultcount = result.Count();
            for(int i =0;i<resultcount;i++)
            {
                var resultItem = result[i];
                if (i + 1 > Namedays.Count || !Namedays[i].Equals(resultItem))
                    Namedays.Insert(i, resultItem);
            }
        }
    }
}
