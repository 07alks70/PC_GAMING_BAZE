﻿using PC_GAMING_BAZE.Models;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Windows.Controls;

namespace PC_GAMING_BAZE
{

    public partial class SetTimePage : Page
    {

        public ObservableCollection<ComputerHostElement> Computers;
        public ObservableCollection<ComputerHostElement> ComputersList
        {
            get { return Computers; }
        }

        public SetTimePage()
        {

            Computers = new ObservableCollection<ComputerHostElement>();

            SetPC();

            InitializeComponent();

           // pc_l.ItemsSource = ComputersList;

        }

        private void SetPC()
        {

            for( int i = 0; i < 15; i++)
            {

                ComputerHostElement OBJ = new ComputerHostElement();

                OBJ.name = "Объект №" + i;
                OBJ._pushedSumm = 0;

                Computers.Add(OBJ);

            }

        }

    }
}
