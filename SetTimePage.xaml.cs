using PC_GAMING_BAZE.Models;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;

namespace PC_GAMING_BAZE
{

    public partial class SetTimePage : Page
    {

        public ObservableCollection<ComputerHostElement> Computers;
        public ObservableCollection<ComputerHostElement> ComputersList
        {
            get { return Computers; }
        }

        private Image GoPay_button;

        private int summ_pushed = 0;

        private string current_pc = "";

        public SetTimePage()
        {

           
            Computers = new ObservableCollection<ComputerHostElement>();

            //SetPC();

            InitializeComponent();

            GoPay_button = GoPay;

            EventManager.RegisterClassHandler(typeof(Image), Image.MouseLeftButtonDownEvent, new RoutedEventHandler(this.OnClickButtonGPMD));
            EventManager.RegisterClassHandler(typeof(Image), Image.MouseLeftButtonUpEvent, new RoutedEventHandler(this.OnClickButtonGPMU));


            // pc_l.ItemsSource = ComputersList;

        }

        private void SetPC()
        {

            for( int i = 0; i < 15; i++)
            {

                ComputerHostElement OBJ = new ComputerHostElement();

                OBJ.name = "Объект №" + i;
                OBJ._pushedSumm = 0;

                //Computers.Add(OBJ);

            }

        }

        private void SelectPC(object sender, System.Windows.RoutedEventArgs e)
        {

            push_money_block.Visibility = System.Windows.Visibility.Visible;

        }

        private void OnClickButtonGPMD(object sender, RoutedEventArgs e)
        {

            if (e.Source.Equals(GoPay_button)) GoPay_button.Opacity = 0.7;

        }

        private void OnClickButtonGPMU(object sender, RoutedEventArgs e)
        {

            if (e.Source.Equals(GoPay_button))
            {

                if (GoPayAcc(summ_pushed, current_pc))
                {

                    push_money_block.Visibility = Visibility.Collapsed;
                    summ_pushed = 0;

                }

            }

        }

        private bool GoPayAcc(int summ, string tag_pc)
        {

            return true;

        }

    }
}
