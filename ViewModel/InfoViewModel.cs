using DoroonNet.Command;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DoroonNet.ViewModel
{
    public class InfoViewModel : NotifyImplementClass
    {
        public static ObservableCollection<FlightData> _CollectionListPartial;

        public ObservableCollection<FlightData> CollectionListPartial
        {
            get { return _CollectionListPartial; }
            set
            {
                _CollectionListPartial = value;
                RaisePropertyChanged("CollectionListPartial");
            } 

        }

        public InfoViewModel()
        {
            CollectionListPartial = new ObservableCollection<FlightData>
            {
                new FlightData
                {
                    ID = 1,
                    FlightID= "#",
                    FlightHDG = 0,
                    FlightSPD = 0.01f,
                    FlightALT = 0.01f
                }
            };
        }

    }
}
