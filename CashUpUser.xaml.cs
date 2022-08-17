using PC_GAMING_BAZE.Models;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace PC_GAMING_BAZE
{
    /// <summary>
    /// Логика взаимодействия для CashUpUser.xaml
    /// </summary>
    public partial class CashUpUser : Page
    {

        private Image goPushMoney_button;
        private Image GoPay_button;

        public List<string> data = new List<string>();

        private string user_name;

        private int summ_pushed = 0;

        public ObservableCollection<ComputerHostElement> Computers;
        /*public ObservableCollection<ComputerHostElement> ComputersList
        {
            get { return Computers; }
        }*/

        private void SetPC()
        {

            for (int i = 0; i < 15; i++)
            {

                ComputerHostElement OBJ = new ComputerHostElement();

                OBJ.name = "Объект №" + i;
                OBJ._pushedSumm = 0;

                Computers.Add(OBJ);

            }

        }

        public CashUpUser()
        {

            InitializeComponent();

            goPushMoney_button = GoPushMoney;
            GoPay_button = GoPay;

            EventManager.RegisterClassHandler(typeof(Image), Image.MouseLeftButtonDownEvent, new RoutedEventHandler(this.OnClickButtonGPMD));
            EventManager.RegisterClassHandler(typeof(Image), Image.MouseLeftButtonUpEvent, new RoutedEventHandler(this.OnClickButtonGPMU));

            push_money_block.Visibility = Visibility.Collapsed;

            if (Keyboard.GetKeyStates(Key.F1) == KeyStates.Down) summ_pushed += 50; 
            if (Keyboard.GetKeyStates(Key.F2) == KeyStates.Down) summ_pushed += 100;
            if (Keyboard.GetKeyStates(Key.F3) == KeyStates.Down) summ_pushed += 200;
            if (Keyboard.GetKeyStates(Key.F4) == KeyStates.Down) summ_pushed += 500; 
            if (Keyboard.GetKeyStates(Key.F5) == KeyStates.Down) summ_pushed += 1000; 


        }

        private void OnClickButtonGPMD(object sender, RoutedEventArgs e)
        {
            if(e.Source.Equals(goPushMoney_button)) goPushMoney_button.Opacity = 0.7;
           
            if(e.Source.Equals(GoPay_button)) GoPay_button.Opacity = 0.7;

        }

        private void OnClickButtonGPMU(object sender, RoutedEventArgs e)
        {
            if (e.Source.Equals(goPushMoney_button))
            {

                goPushMoney_button.Opacity = 1.0;

                if (CheckExistAccount(user_name))
                {
                    push_money_block.Visibility = Visibility.Visible;
                    balance_push_info.Content = "Внесено: " + summ_pushed;

                }

            }

            if (e.Source.Equals(GoPay_button))
            {

                if(GoPayAcc(summ_pushed, user_name))
                {

                    push_money_block.Visibility = Visibility.Collapsed;
                    summ_pushed = 0;
                    input_username.Text = "";

                }

            }

        }

        private bool GoPayAcc(int summ, string user_name)
        {

            return true;

        }

        private bool CheckExistAccount(string account_name)
        {

            return true;

        }

        private void GetUsers()
        {

            data.Add("alks8");
            data.Add("alexkors8");
            data.Add("alexkors8");
            data.Add("alexkors9");
            data.Add("aletxkors8");
            data.Add("alexykors8");
            data.Add("alrexkyors8");
            data.Add("alexktuors8");
            data.Add("aleejkors8");
            data.Add("alexkurors8");
            data.Add("robotator400");
            data.Add("robotator500");

        }

        private void ShowUsers(object sender, KeyEventArgs e)
        {

            //return;

            var found = false;
            var border = (resultStack.Parent as ScrollViewer).Parent as Border;

            string query = input_username.Text;

            data.Clear();

            GetUsers();

            Debug.WriteLine("Query username:" + query);

            if (query.Length == 0)
            {

                Debug.WriteLine("Закрыть варианы никнеймов");

                // Clear
                resultStack.Children.Clear();
                border.Visibility = System.Windows.Visibility.Collapsed;
            }
            else
            {
                Debug.WriteLine("Открыть варианы никнеймов");
                resultStack.Children.Clear();
                border.Visibility = System.Windows.Visibility.Visible;
            }

            // Clear the list
            resultStack.Children.Clear();

            // Add the result
            foreach (var obj in data)
            {
                if (obj.ToLower().StartsWith(query.ToLower()))
                {
                    Debug.WriteLine("Добавить запись в предложения: " + obj);
                    // The word starts with this... Autocomplete must work
                    addItem(obj);
                    found = true;
                }
            }

        }

        private void addItem(string text)
        {
            TextBlock block = new TextBlock();

            // Add the text
            block.Text = text;

            // A little style...
            block.Margin = new Thickness(10, 15, 10, 0);
            block.FontSize = 20.0;
            block.Foreground = new SolidColorBrush(Colors.White);
            block.Cursor = Cursors.Hand;

            // Mouse events
            block.MouseLeftButtonUp += (sender, e) =>
            {

                input_username.Text = (sender as TextBlock).Text;

                user_name = input_username.Text;

                var border = (resultStack.Parent as ScrollViewer).Parent as Border;
                border.Visibility = System.Windows.Visibility.Collapsed;
            };

            block.MouseEnter += (sender, e) =>
            {
                TextBlock b = sender as TextBlock;
                b.Background = Brushes.PeachPuff;
            };

            block.MouseLeave += (sender, e) =>
            {
                TextBlock b = sender as TextBlock;
                b.Background = Brushes.Transparent;
            };

            // Add to the panel
            resultStack.Children.Add(block);
        }

    }
}
