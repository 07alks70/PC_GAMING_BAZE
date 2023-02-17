using PC_GAMING_BAZE.Models;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Diagnostics;
using System.Linq;
using System.Net.Http;
using System.Net;
using System.Text;
using System.Text.Json;
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
using System.Collections;
using PC_GAMING_BAZE.UI;
using CCNET;
using System.Reflection;

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
        private int SelectedId;

        protected Iccnet device = new Iccnet();
        protected CCNETEventArgs dataAnsw;
        private bool _isStartSession = false;
        public Task rr;

        public List<string> data = new List<string>();
        public List<User> users_v = new List<User>();

        protected ObservableCollection<User> Users = new ObservableCollection<User>();

        public ObservableCollection<User> UsersList
        {
            get { return Users; }
            set { Users = value; }
        }

        private string user_name;

        private int summ_pushed = 0;

        public ObservableCollection<ComputerHostElement> Computers;


        private void SetPC()
        {

            for (int i = 0; i < 15; i++)
            {

                ComputerHostElement OBJ = new ComputerHostElement();

                OBJ.name = "Объект №" + i;
                OBJ._pushedSumm = 0;

                Computers.Add(OBJ);

                Users.Add(new User() { id = 6, username = "ff" });
                UsersList = Users;

                this.DataContext = UsersList;

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

            new Task(() =>
            {

                OpenConnectionBillToBill();

            }).Start();
            

        }

        private void OnClickButtonGPMD(object sender, RoutedEventArgs e)
        {
            if(e.Source.Equals(goPushMoney_button)) goPushMoney_button.Opacity = 0.7;
           
            if(e.Source.Equals(GoPay_button)) GoPay_button.Opacity = 0.7;

        }

        private async void OnClickButtonGPMU(object sender, RoutedEventArgs e)
        {
            if (e.Source.Equals(goPushMoney_button))
            {

                if (SelectedId == -1) return;

                new Task(() =>
                {

                    StartPool();

                }).Start();

                GoPushMoney.Visibility = Visibility.Collapsed;

                goPushMoney_button.Opacity = 1.0;
                push_money_block.Visibility = Visibility.Visible;
                balance_push_info.Content = "Внесено: " + summ_pushed;
            }

            if (e.Source.Equals(GoPay_button))
            {

                Debug.WriteLine(SelectedId.ToString() + " Пополнили на: " + summ_pushed.ToString());

                HttpClient client = new HttpClient();
                HttpRequestMessage request = new HttpRequestMessage();
                request.RequestUri = new Uri("http://176.112.164.61/api/users/" + SelectedId + "/deposit/" + summ_pushed + "/-1");
                request.Method = HttpMethod.Put;
                request.Headers.Add("Accept", "application/json");
                request.Headers.Add("Authorization", "Basic " + System.Convert.ToBase64String(System.Text.Encoding.UTF8.GetBytes("admin:admin")));

                HttpResponseMessage response = await client.SendAsync(request);
                if (response.StatusCode == HttpStatusCode.OK)
                {
                    Debug.WriteLine("Успешно пополнено");

                   
                    new Task(() => {

                        device.RunCommand(CCNETCommand.RESET);
                        

                    }).Start();

                    _isStartSession = false;
                    summ_pushed = 0;
                    push_money_block.Visibility = Visibility.Collapsed;
                    input_username.Text = "";
                    balance_push_info.Content = "Внесено: " + summ_pushed;
                    push_money_block.Visibility = Visibility.Collapsed;
                    GoPushMoney.Visibility = Visibility.Collapsed;
                    SelectedId = -1;
                    input_username.Text = "";


                }
                if (response.StatusCode == HttpStatusCode.BadRequest)
                {
                    Debug.WriteLine("Ошибка пополнения");
                }
            }
        }

        private bool CheckExistAccount(string account_name)
        {

            return true;

        }

        private void ShowUsers(object sender, KeyEventArgs e)
        {

            GetUsers(input_username.Text);

        }

        public async void GetUsers(string query)
        {

            if(query.Length == 0)
            {

                resultStack.Children.Clear();
                return;

            }

            push_money_block.Visibility = Visibility.Collapsed;

            Debug.WriteLine(query);

            List<User> users = new List<User>();

            HttpClient client = new HttpClient();
            HttpRequestMessage request = new HttpRequestMessage();
            request.RequestUri = new Uri("http://176.112.164.61/api/users");
            request.Method = HttpMethod.Get;
            request.Headers.Add("Accept", "application/json");
            request.Headers.Add("Authorization", "Basic " + System.Convert.ToBase64String(System.Text.Encoding.UTF8.GetBytes("admin:admin")));

            HttpResponseMessage response = await client.SendAsync(request);
            if (response.StatusCode == HttpStatusCode.OK)
            {
                HttpContent responseContent = response.Content;
                var json = await responseContent.ReadAsStringAsync();

                JsonDocument doc = JsonDocument.Parse(json);
                JsonElement root = doc.RootElement;

                resultStack.Children.Clear();

                for (int i = 0; i < root.GetProperty("result").GetArrayLength(); i++)
                {

                    if (root.GetProperty("result")[i].GetProperty("userGroupId").ToString() == "1" && root.GetProperty("result")[i].GetProperty("username").ToString() == query)
                    {

                        GoPushMoney.Visibility = Visibility.Visible;
                        SelectedId = (int)Int64.Parse(root.GetProperty("result")[i].GetProperty("id").ToString());
                        addItem(root.GetProperty("result")[i].GetProperty("username").ToString(), (int)Int64.Parse(root.GetProperty("result")[i].GetProperty("id").ToString()));

                    }
                    else if (root.GetProperty("result")[i].GetProperty("userGroupId").ToString() == "1" && root.GetProperty("result")[i].GetProperty("username").ToString().ToLower().StartsWith(query.ToLower()))
                    {

                        GoPushMoney.Visibility = Visibility.Collapsed;
                        SelectedId = -1;
                        Debug.WriteLine("UserName: " + root.GetProperty("result")[i].GetProperty("username") + " ID: " + root.GetProperty("result")[i].GetProperty("id"));

                        //Users.Add(new User() { id = (int)Int64.Parse(root.GetProperty("result")[i].GetProperty("id").ToString()), username = root.GetProperty("result")[i].GetProperty("username").ToString() });

                        addItem(root.GetProperty("result")[i].GetProperty("username").ToString(), (int)Int64.Parse(root.GetProperty("result")[i].GetProperty("id").ToString()));

                        //UserItemSelect

                    }

                }

               // UsersList = Users;
                //resultStack.Visibility = Visibility.Visible;
            }

        }

         private void addItem(string text, int id)
         {
            UserItemSelect block = new UserItemSelect();

             // Add the text
             block.Text = text;
            block.Id = id;

             // A little style...
             block.Margin = new Thickness(10, 15, 10, 0);
             block.FontSize = 20.0;
             block.Foreground = new SolidColorBrush(Colors.White);
             block.Cursor = Cursors.Hand;

            // Mouse events
            block.MouseLeftButtonUp += (sender, e) =>
            {

                input_username.Text = (sender as UserItemSelect).Text;

                user_name = input_username.Text;
                SelectedId = (sender as UserItemSelect).Id;
                GoPushMoney.Visibility = Visibility.Visible;

                Debug.WriteLine("Вы выбрали ID: " + (sender as UserItemSelect).Id + " Никнейм: " + (sender as UserItemSelect).Text);

                resultStack.Children.Clear();

            };

             block.MouseEnter += (sender, e) =>
             {
                 TextBlock b = sender as UserItemSelect;
                 b.Background = Brushes.PeachPuff;
             };

             block.MouseLeave += (sender, e) =>
             {
                 TextBlock b = sender as UserItemSelect;
                 b.Background = Brushes.Transparent;
             };

             // Add to the panel
             resultStack.Children.Add(block);
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

        private void OpenConnectionBillToBill()
        {

            device = new Iccnet("COM3", Device.Bill_Validator);

            device.OnAnswerReceived += new Iccnet.AnswerReceivedEvent(Device_OnAnswerReceived);

        }

        private void Device_OnAnswerReceived(object sender, CCNETEventArgs fe)
        {

            dataAnsw = fe;

            Answer MyAnswer = fe.ReceivedAnswer;

            string mgg = "";

            if(MyAnswer.Data != null)
            {

                foreach(byte b in MyAnswer.Data)
                {

                    mgg += " " + b.ToString();

                }

            }

            if (MyAnswer.Error == null)
            {

                Dispatcher.Invoke((Action)(() => { 


                    if(MyAnswer.Message == "ACCEPTING")
                    {

                        //DebugWindowApp.Items.Add("ACCEPTING Данные: " + System.Text.Encoding.Default.GetString(MyAnswer.Data) + " " + mgg); 

                    }

                    if (MyAnswer.Message == "ESCROW POSITION")
                    {

                      
                        //DebugWindowApp.Items.Add("ESCROW POSITION Данные: " + System.Text.Encoding.Default.GetString(MyAnswer.Data) + " " + mgg);

                    }

                    if (MyAnswer.Message == "STACKING")
                    {

                        //DebugWindowApp.Items.Add("STACKING Данные: " + System.Text.Encoding.Default.GetString(MyAnswer.Data) + " " + mgg);

                    }

                    if (MyAnswer.Message == "BILL STACKED")
                    {

                        string val = "";

                        if(MyAnswer.ReceivedData[1] == 2)
                        {

                            val = MyAnswer.ReceivedData[4].ToString();

                        }
                        if(MyAnswer.ReceivedData[1] == 3)
                        {

                            val = GetCurr(MyAnswer.ReceivedData[4]).ToString();
                            summ_pushed += Int32.Parse(val);
                            Dispatcher.Invoke((Action)(() => { balance_push_info.Content = "Внесено: " + summ_pushed; }));


                        }

                        //DebugWindowApp.Items.Add("Принял сумму: " + val + ' ' + "руб");

                    }


                }));

               
            }
            else
            {
                Dispatcher.Invoke((Action)(() => {  }));

            }

            Dispatcher.Invoke((Action)(() => {  }));

        } 

        private int GetCurr(byte byte_v)
        {

            int currc = 0;

            switch (byte_v)
            {

                case 2: currc = 10;
                    break;
                case 3:
                    currc = 50;
                    break;
                case 4:
                    currc = 100;
                    break;
                case 12:
                    currc = 200;
                    break;
                case 5:
                    currc = 500;
                    break;
                case 6:
                    currc = 1000;
                    break;
                case 13:
                    currc = 2000;
                    break;
                case 7:
                    currc = 5000;
                    break;
                case 11:
                    currc = 10;
                    break;
                case 10:
                    currc = 5;
                    break;
                case 8:
                    currc = 1;
                    break;
                case 9:
                    currc = 2;
                    break;

            }

            return currc;

        }

        private async void StartPool()
        {

            bool _isRestart = false;

            device.RunCommand(CCNETCommand.RESET);

            if (dataAnsw.ReceivedAnswer.Data == null)
            {

                //DebugWindowApp.Items.Add("Ошибка подключения");

            }

            device.RunCommand(CCNETCommand.Poll);

            while (dataAnsw.ReceivedAnswer.Message == "INITIALIZE")
            {

                device.RunCommand(CCNETCommand.Poll);

            }

            device.RunCommand(CCNETCommand.Poll);

            Byte[] enable = new Byte[] { 255, 255, 255, 255, 255, 255 };
            device.RunCommand(CCNETCommand.ENABLE_BILL_TYPES, enable);

            if (dataAnsw.ReceivedAnswer.Data[0] == 0)
            {

                _isStartSession = true;

                rr = new Task(() => {

                    bool _iSession = true;

                    while (_iSession)
                    {

                        _iSession = _isStartSession;
                        device.RunCommand(CCNETCommand.Poll);

                        if (dataAnsw.ReceivedAnswer.Message == "ESCROW POSITION") device.RunCommand(CCNETCommand.STACK);

                    }


                });
                rr.Start();

            }

        }

    }
}
