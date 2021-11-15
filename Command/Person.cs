using DoroonNet.ViewModel;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DoroonNet.Command
{
    public class Person : NotifyImplementClass
    {
        private string _Name { get; set; }
        public string Name { 
            get 
            {
                return _Name;
            } 
            set 
            {
                _Name = value;
                OnPropertyChanged("Name");
            } 
        }

        private int _Age { get; set; }
        public int Age {
            get
            {
                return _Age;
            }
            set
            {
                _Age = value;
                OnPropertyChanged("Age");
            }
        }
    }
}
