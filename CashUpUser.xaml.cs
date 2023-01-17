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
        private User _user;

        public List<string> data = new List<string>();
        public List<User> users_v = new List<User>();

        public ObservableCollection<User> Users;

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

            input_username.Text = "Введите имя пользователя";

            _user = new User();

            Users = new ObservableCollection<User>();

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

        private async void ShowUsers(object sender, KeyEventArgs e)
        {

            //return;

            var found = false;
           // var border = (resultStack.Parent as ScrollViewer).Parent as Border;

            string query = input_username.Text;

            //data.Clear();

            Debug.WriteLine("Query username:" + query);

            /*await foreach (User usr in User.GetUsers())
            {



            }*/

           /* if (query.Length == 0)
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
            }*/

            // Clear the list
           // resultStack.Children.Clear();

            // Add the result
            /*foreach (var obj in data)
            {
                if (obj.ToLower().StartsWith(query.ToLower()))
                {
                    Debug.WriteLine("Добавить запись в предложения: " + obj);
                    // The word starts with this... Autocomplete must work
                    addItem(obj);
                    found = true;
                }
            }*/

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

                //var border = (resultStack.Parent as ScrollViewer).Parent as Border;
                //border.Visibility = System.Windows.Visibility.Collapsed;
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
            //resultStack.Children.Add(block);
        }

        private void input_username_GotFocus(object sender, RoutedEventArgs e)
        {

            if (input_username.Text == "Введите имя пользователя")
            {
                input_username.Text = "";
            }

        }

        private void input_username_LostFocus(object sender, RoutedEventArgs e)
        {

            if (input_username.Text.Length <= 0)
            {
                input_username.Text = "Введите имя пользователя";
            }

        }
    }
}
