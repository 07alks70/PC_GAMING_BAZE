using PC_GAMING_BAZE.Models;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PC_GAMING_BAZE.ViewModel
{
    class SetTimeVM
    {

        public ObservableCollection<ComputerHostElement> Computers;
        public ObservableCollection<ComputerHostElement> ComputersList
        {
            get { return Computers; }
        }

        public SetTimeVM()
        {

            SetPC();

            Computers = new ObservableCollection<ComputerHostElement>();

        }

        private void SetPC()
        {

            for (int i = 0; i < 15; i++)
            {

                ComputerHostElement OBJ = new ComputerHostElement();

               

                Computers.Add(OBJ);

            }

        }

    }
}
