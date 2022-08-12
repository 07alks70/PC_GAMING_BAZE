using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO.Ports;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;
using System.Windows.Threading;
using CCNET;

namespace PC_GAMING_BAZE
{

    public partial class MainWindow : Window
    {

        private Grid CashUpUser_Button;
        private Grid SetTime_Button;
        private Grid Released_Button;

        private Border CashUpUser_Border;
        private Border SetTime_Border;
        private Border Released_Border;

        private Image CashUpUser_Image;
        private Image SetTime_Image;
        private Image Released_Image;

        private Label CashUpUser_Label;
        private Label SetTime_Label;
        private Label Released_Label;

        private CashUpUser CashUpUser_Page;
        private SetTimePage SetTime_Page;
        private ReleasedPage Released_Page;

        private bool _isCashUserPage = false;
        private bool _isSetTimePage = false;
        private bool _isReleasedPage = false;

        private Frame PagesHost;

        public SolidColorBrush color_select_menu = new SolidColorBrush(Color.FromRgb(71, 71, 71));
        public SolidColorBrush color_no_select_menu = new SolidColorBrush(Color.FromRgb(50, 50, 50));

        /*Iccnet device;
        private bool _isStartSession = false;
        public static MainWindow MW { get; private set; }

        public static CCNETEventArgs dataAnsw;
        public Task rr;

        public ListView lv;
        public int _summ = 0;*/

        public MainWindow()
        {

            InitializeComponent();

            CashUpUser_Button = CashUpUser;
            SetTime_Button = SetTime;
            Released_Button = Released;

            CashUpUser_Border = CashUpUserBorder;
            SetTime_Border = SetTimeBorder;
            Released_Border = ReleasedBorder;

            CashUpUser_Image = CashUpUserIcon;
            SetTime_Image = SetTimeIcon;
            Released_Image = ReleasedIcon;

            CashUpUser_Label = CashUpUserLabel;
            SetTime_Label = SetTimeLabel;
            Released_Label = ReleasedLabel;

            PagesHost = ContentView;
            

            CashUpUser_Page = new CashUpUser();
            SetTime_Page = new SetTimePage();
            Released_Page = new ReleasedPage();

            ShowCashUserPage();

            EventManager.RegisterClassHandler(typeof(Grid), Grid.MouseLeftButtonDownEvent, new RoutedEventHandler(this.OnClickButtonNavPanel));

        }

        private void OnClickButtonNavPanel(object sender, RoutedEventArgs e)
        {

            if (e.Source.Equals(CashUpUser_Button) || e.Source.Equals(CashUpUser_Image) || e.Source.Equals(CashUpUser_Label)) if(!_isCashUserPage) ShowCashUserPage();
            if (e.Source.Equals(SetTime_Button) || e.Source.Equals(SetTime_Image) || e.Source.Equals(SetTime_Label)) if (!_isSetTimePage) ShowSetTimePage();
            if (e.Source.Equals(Released_Button) || e.Source.Equals(Released_Image) || e.Source.Equals(Released_Label)) if (!_isReleasedPage) ShowReleasedPage();

        }

        private void ShowCashUserPage()
        {

            _isCashUserPage = true;
            _isSetTimePage = false;
            _isReleasedPage = false;

            CashUpUser_Button.Background = color_select_menu;
            SetTime_Button.Background = color_no_select_menu;
            Released_Button.Background = color_no_select_menu;

            CashUpUser_Border.BorderThickness = new Thickness(0, 0, 0, 1);
            SetTime_Border.BorderThickness = new Thickness(0, 0, 0, 0);
            Released_Border.BorderThickness = new Thickness(0, 0, 0, 0);

            PagesHost.Content = CashUpUser_Page;

        }

        private void ShowSetTimePage()
        {

            _isCashUserPage = false;
            _isSetTimePage = true;
            _isReleasedPage = false;

            CashUpUser_Button.Background = color_no_select_menu;
            SetTime_Button.Background = color_select_menu;
            Released_Button.Background = color_no_select_menu;

            CashUpUser_Border.BorderThickness = new Thickness(0, 0, 0, 0);
            SetTime_Border.BorderThickness = new Thickness(0, 0, 0, 1);
            Released_Border.BorderThickness = new Thickness(0, 0, 0, 0);

            PagesHost.Content = SetTime_Page;

        }

        private void ShowReleasedPage()
        {

            _isCashUserPage = false;
            _isSetTimePage = false;
            _isReleasedPage = true;

            CashUpUser_Button.Background = color_no_select_menu;
            SetTime_Button.Background = color_no_select_menu;
            Released_Button.Background = color_select_menu;

            CashUpUser_Border.BorderThickness = new Thickness(0, 0, 0, 0);
            SetTime_Border.BorderThickness = new Thickness(0, 0, 0, 0);
            Released_Border.BorderThickness = new Thickness(0, 0, 0, 1);

            PagesHost.Content = Released_Page;

        }



        //Ниже логика для работы купюроприемника и монетоприемника
        /*private void BtnOpenConnectionBillToBill(object sender, RoutedEventArgs er)
        {

            device = new Iccnet("COM3", Device.Bill_Validator);

            DebugWindowApp.Items.Add(device.GetPortName());
            device.OnAnswerReceived += new Iccnet.AnswerReceivedEvent(Device_OnAnswerReceived);
           


        }

        private async void PoolBtn(object sender, RoutedEventArgs er)
        {

            bool _isRestart = false;

            device.RunCommand(CCNETCommand.RESET);

            if (dataAnsw.ReceivedAnswer.Data == null)
            {

                DebugWindowApp.Items.Add("Ошибка подключения");

            }

            device.RunCommand(CCNETCommand.Poll);

            while(dataAnsw.ReceivedAnswer.Message == "INITIALIZE")
            {        

                device.RunCommand(CCNETCommand.Poll);

            }

            device.RunCommand(CCNETCommand.Poll);

            Byte[] enable = new Byte[] { 255, 255, 255, 255, 255, 255 };
            device.RunCommand(CCNETCommand.ENABLE_BILL_TYPES, enable);

            if(dataAnsw.ReceivedAnswer.Data[0] == 0)
            {

                _isStartSession = true;

                rr = new Task(() => {

                    bool _iSession = true;

                    while (_iSession) {

                        _iSession = _isStartSession;
                        device.RunCommand(CCNETCommand.Poll);

                        if (dataAnsw.ReceivedAnswer.Message == "ESCROW POSITION") device.RunCommand(CCNETCommand.STACK);

                    }
                   

                });
                rr.Start();

            }

        }

        private void StopBtn(object sender, RoutedEventArgs er)
        {

            new Task(() => {

                _isStartSession = false;

                device.RunCommand(CCNETCommand.RESET);

            }).Start();

            _summ = 0;
            Dispatcher.Invoke((Action)(() => { summ.Content = _summ.ToString() + " руб"; DebugWindowApp.Items.Clear(); }));

        }

        private void ReturnBtn(object sender, RoutedEventArgs er)
        {

            device.RunCommand(CCNETCommand.RETURN);

        }

        private void BillTableBtn(object sender, RoutedEventArgs er)
        {

            device.RunCommand(CCNETCommand.GET_BILL_TABLE);

        }

        private void ResetBtn(object sender, RoutedEventArgs er)
        {

            device.RunCommand(CCNETCommand.RESET);

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
                    DebugWindowApp.Items.Add(MyAnswer.Message);

                    DebugWindowApp.Focus();

                    if(MyAnswer.Message == "ACCEPTING")
                    {

                        //DebugWindowApp.Items.Add("ACCEPTING Данные: " + System.Text.Encoding.Default.GetString(MyAnswer.Data) + " " + mgg); 

                    }

                    if (MyAnswer.Message == "ESCROW POSITION")
                    {

                      
                        DebugWindowApp.Items.Add("ESCROW POSITION Данные: " + System.Text.Encoding.Default.GetString(MyAnswer.Data) + " " + mgg);

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
                            _summ += Int32.Parse(val);
                            Dispatcher.Invoke((Action)(() => { summ.Content = _summ.ToString() + " руб"; }));


                        }

                        DebugWindowApp.Items.Add("Принял сумму: " + val + ' ' + "руб");

                    }


                }));

               
            }
            else
            {
                Dispatcher.Invoke((Action)(() => { DebugWindowApp.Items.Add(MyAnswer.Error.Message); }));

            }

            Dispatcher.Invoke((Action)(() => { DebugWindowApp.ScrollIntoView(DebugWindowApp.Items[DebugWindowApp.Items.Count - 1]);  }));

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

        }*/

    }
}
