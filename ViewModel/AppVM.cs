using PC_GAMING_BAZE.CustomCommands;
using PC_GAMING_BAZE.Models;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Data.SQLite;
using System.IO;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Reflection;
using System.Runtime.CompilerServices;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;
using System.Windows.Controls;
using System.Windows.Input;

namespace PC_GAMING_BAZE.ViewModel
{

    public class AppVM : INotifyPropertyChanged
    {

        public ComputerHostElement _selectedItem;

       /* public SetTimeItemClick setTimeItemClick { get; set; }*/

        public SQLiteConnection Connect;

        private ObservableCollection<ComputerHostElement> HostsCollection;

        public ObservableCollection<ComputerHostElement> HostsCollectionD 
        {

            get
            {
                return HostsCollection;
            }
            set
            {
                HostsCollection = value;
                NotifyPropertyChanged();
            }

        }

       // public MainWindow mainWindow;
        private BackgroundWorker worker;

        public event PropertyChangedEventHandler PropertyChanged;
        public event EventHandler CanExecuteChanged;

        public AppVM()
        {

          /*  setTimeItemClick = new SetTimeItemClick(SelectedCommandHandler, CanExecuteSelectedCommand);*/

            if (!File.Exists(Path.GetDirectoryName(Assembly.GetEntryAssembly().Location) + "PCGB.db"))
            {
                SQLiteConnection.CreateFile(Path.GetDirectoryName(Assembly.GetEntryAssembly().Location) + "PCGB.db");

                Connect = new SQLiteConnection("Data Source= " + Path.GetDirectoryName(Assembly.GetEntryAssembly().Location) + "PCGB.db"); // в строке указывается к какой базе подключаемся

                string commandText = "CREATE TABLE IF NOT EXISTS [hosts] ( [id] INTEGER PRIMARY KEY AUTOINCREMENT NOT NULL, [host_id] BIGINT, [host_name] VARCHAR(255), " +
                    "[status] BOOLEAN, [guest_acc_id] BIGINT, [tech_work] BOOLEAN, [time_av] BIGINT, UNIQUE(host_id, host_name))";
                SQLiteCommand Command = new SQLiteCommand(commandText, Connect);
                Connect.Open();
                Command.ExecuteNonQuery();
                Connect.Close();

            }
            else
            {
                Connect = new SQLiteConnection("Data Source= " + Path.GetDirectoryName(Assembly.GetEntryAssembly().Location) + "PCGB.db"); // в строке указывается к какой базе подключаемся
            }

            Connect.Open();

            HostsCollection = new ObservableCollection<ComputerHostElement>();
           // this.mainWindow = mainWindow;
            worker = new BackgroundWorker();
            worker.WorkerReportsProgress = true;
            worker.WorkerSupportsCancellation = true;

            worker.DoWork += new DoWorkEventHandler(UpdateDBData);
            worker.RunWorkerAsync();

        }

        private async void UpdateDBData(object sender, DoWorkEventArgs e)
        {
            while (true)
            {

                HttpClient client = new HttpClient();
                HttpRequestMessage request = new HttpRequestMessage();
                request.RequestUri = new Uri("http://176.112.164.50/api/hosts");
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

                    for (int i = 0; i < root.GetProperty("result").GetArrayLength(); i++)
                    {

                        if (root.GetProperty("result")[i].GetProperty("state").ToString() == "0")
                        {

                            string commandText = "INSERT OR IGNORE INTO hosts(host_id,host_name) VALUES (" + root.GetProperty("result")[i].GetProperty("number") + ", '" + root.GetProperty("result")[i].GetProperty("name").ToString() + "');";

                            SQLiteCommand Command = new SQLiteCommand(commandText, Connect);
                            ////Connect.Open();
                            Command.ExecuteNonQuery();
                            ////Connect.Close();

                            HttpClient client_add_guest_user_for_host = new HttpClient();
                            HttpRequestMessage request_add_guest_user_for_host = new HttpRequestMessage();
                            request_add_guest_user_for_host.RequestUri = new Uri("http://176.112.164.50/api/users?Username=TerminalGuest" + root.GetProperty("result")[i].GetProperty("number") + "&UserGroupId=1");
                            request_add_guest_user_for_host.Method = HttpMethod.Put;
                            request_add_guest_user_for_host.Headers.Add("Accept", "application/json");
                            request_add_guest_user_for_host.Headers.Add("Authorization", "Basic " + System.Convert.ToBase64String(System.Text.Encoding.UTF8.GetBytes("admin:admin")));

                            HttpResponseMessage response_add_guest_user_for_host = await client_add_guest_user_for_host.SendAsync(request_add_guest_user_for_host);
                            if (response_add_guest_user_for_host.StatusCode == HttpStatusCode.OK)
                            {
                                HttpContent responseContent_add_guest_user_for_host = response_add_guest_user_for_host.Content;
                                var json_add_guest_user_for_host = await responseContent_add_guest_user_for_host.ReadAsStringAsync();

                                JsonDocument doc_add_guest_user_for_host = JsonDocument.Parse(json_add_guest_user_for_host);
                                JsonElement root_add_guest_user_for_host = doc_add_guest_user_for_host.RootElement;

                                string commandText_add_guest_user_for_host = "UPDATE hosts SET guest_acc_id=" + root_add_guest_user_for_host.GetProperty("result").ToString() + " WHERE host_id=" + root.GetProperty("result")[i].GetProperty("number").ToString() + ";";

                                SQLiteCommand Command_add_guest_user_for_host = new SQLiteCommand(commandText_add_guest_user_for_host, Connect);
                                //Connect.Open();
                                Command_add_guest_user_for_host.ExecuteNonQuery();
                                //Connect.Close();



                            }

                        }
                    }
                }

                HttpClient client_active_sessions = new HttpClient();
                HttpRequestMessage request_active_sessions = new HttpRequestMessage();
                request_active_sessions.RequestUri = new Uri("http://176.112.164.50/api/usersessions/active");
                request_active_sessions.Method = HttpMethod.Get;
                request_active_sessions.Headers.Add("Accept", "application/json");
                request_active_sessions.Headers.Add("Authorization", "Basic " + System.Convert.ToBase64String(System.Text.Encoding.UTF8.GetBytes("admin:admin")));

                HttpResponseMessage response_active_sessions = await client_active_sessions.SendAsync(request_active_sessions);
                if (response_active_sessions.StatusCode == HttpStatusCode.OK)
                {
                    HttpContent responseContent_active_sessions = response_active_sessions.Content;
                    var json_active_sessions = await responseContent_active_sessions.ReadAsStringAsync();

                    JsonDocument doc_active_sessions = JsonDocument.Parse(json_active_sessions);
                    JsonElement root_active_sessions = doc_active_sessions.RootElement;

                    Dictionary<int, int> sessions_times = new Dictionary<int, int>();

                    for (int i = 0; i < root_active_sessions.GetProperty("result").GetArrayLength(); i++)
                    {

                        HttpClient client_usr = new HttpClient();
                        HttpRequestMessage request_usr = new HttpRequestMessage();
                        request_usr.RequestUri = new Uri("http://176.112.164.50/api/users/" + root_active_sessions.GetProperty("result")[i].GetProperty("userId") + "/balance");
                        request_usr.Method = HttpMethod.Get;
                        request_usr.Headers.Add("Accept", "application/json");
                        request_usr.Headers.Add("Authorization", "Basic " + System.Convert.ToBase64String(System.Text.Encoding.UTF8.GetBytes("admin:admin")));

                        HttpResponseMessage response_usr = await client_usr.SendAsync(request_usr);
                        if (response_usr.StatusCode == HttpStatusCode.OK)
                        {
                            HttpContent resp_usr = response_usr.Content;
                            var json_usr = await resp_usr.ReadAsStringAsync();

                            JsonDocument doc_usr = JsonDocument.Parse(json_usr);
                            JsonElement root_usr = doc_usr.RootElement;

                            sessions_times.Add(Int32.Parse(root_active_sessions.GetProperty("result")[i].GetProperty("hostId").ToString()), Int32.Parse(root_usr.GetProperty("result").GetProperty("availableCreditedTime").ToString()));

                        }

                    }

                    string commandText = "UPDATE hosts SET time_av=0";

                    SQLiteCommand Command = new SQLiteCommand(commandText, Connect);
                    //Connect.Open();
                    Command.ExecuteNonQuery();
                    //Connect.Close();

                    SQLiteCommand Command_select_all_hosts = new SQLiteCommand("SELECT * FROM hosts", Connect);
                    //Connect.Open();
                    SQLiteDataReader myReader_select_all_hosts = Command_select_all_hosts.ExecuteReader();

                    while (myReader_select_all_hosts.Read())
                    {
                        if (sessions_times.ContainsKey(Int32.Parse(myReader_select_all_hosts["host_id"].ToString())))
                        {
                            string commandText_up = "UPDATE hosts SET time_av=" + sessions_times[Int32.Parse(myReader_select_all_hosts["host_id"].ToString())] + " WHERE host_id=" + Int32.Parse(myReader_select_all_hosts["host_id"].ToString()) + ";";

                            SQLiteCommand Command_up = new SQLiteCommand(commandText_up, Connect);

                            try
                            {
                                Command_up.ExecuteNonQuery();
                            }
                            catch
                            {

                            }

                            var computerHost = HostsCollection.Where(x => x.hostId == Int32.Parse(myReader_select_all_hosts["host_id"].ToString())).FirstOrDefault();

                            if (computerHost != null)
                            {
                                App.Current.Dispatcher.Invoke((Action)delegate
                                {
                                    HostsCollection[HostsCollection.IndexOf(computerHost)] = new ComputerHostElement() { guestAccId = computerHost.guestAccId, hostId = computerHost.hostId, hostName = computerHost.hostName, tinme_av = sessions_times[Int32.Parse(myReader_select_all_hosts["host_id"].ToString())] };
                                    //HostsCollection.Where(x => x.hostId == Int32.Parse(myReader_select_all_hosts["host_id"].ToString())).FirstOrDefault().tinme_av = sessions_times[Int32.Parse(myReader_select_all_hosts["host_id"].ToString())];
                                });
                                NotifyPropertyChanged();
                                Console.WriteLine("Host: " + computerHost.hostId + "  Time: " + computerHost.tinme_av);
                            }
                            else
                            {

                                App.Current.Dispatcher.Invoke((Action)delegate
                                {
                                    HostsCollection.Add(new ComputerHostElement() { guestAccId = Int32.Parse(myReader_select_all_hosts["guest_acc_id"].ToString()), hostId = Int32.Parse(myReader_select_all_hosts["host_id"].ToString()), hostName = myReader_select_all_hosts["host_name"].ToString(), tinme_av = Int32.Parse(myReader_select_all_hosts["time_av"].ToString()) });
                                });
                                NotifyPropertyChanged();
                            }

                        }
                        else
                        {
                            string commandText_up = "UPDATE hosts SET time_av=0 WHERE host_id=" + Int32.Parse(myReader_select_all_hosts["host_id"].ToString()) + ";";

                            SQLiteCommand Command_up = new SQLiteCommand(commandText_up, Connect);

                            try
                            {
                                Command_up.ExecuteNonQuery();
                            }
                            catch
                            {

                            }

                            var computerHost = HostsCollection.Where(x => x.hostId == Int32.Parse(myReader_select_all_hosts["host_id"].ToString())).FirstOrDefault();

                            if (computerHost != null)
                            {
                                App.Current.Dispatcher.Invoke((Action)delegate
                                {
                                    //HostsCollection.Where(x => x.hostId == Int32.Parse(myReader_select_all_hosts["host_id"].ToString())).FirstOrDefault().tinme_av = 0;
                                    HostsCollection[HostsCollection.IndexOf(computerHost)] = new ComputerHostElement() { guestAccId = computerHost.guestAccId, hostId = computerHost.hostId, hostName = computerHost.hostName, tinme_av = 0 };
                                });
                                NotifyPropertyChanged();
                                Console.WriteLine("Host: " + computerHost.hostId + "  Time: " + computerHost.tinme_av);
                            }
                            else
                            {
                                App.Current.Dispatcher.Invoke((Action)delegate 
                                {
                                    HostsCollection.Add(new ComputerHostElement() { guestAccId = Int32.Parse(myReader_select_all_hosts["guest_acc_id"].ToString()), hostId = Int32.Parse(myReader_select_all_hosts["host_id"].ToString()), hostName = myReader_select_all_hosts["host_name"].ToString(), tinme_av = Int32.Parse(myReader_select_all_hosts["time_av"].ToString()) });
                                });
                                NotifyPropertyChanged();

                            }
                        }
                    }
                    //Connect.Close();

                }
                System.Threading.Thread.Sleep(3000);
            }
        }

        private void NotifyPropertyChanged([CallerMemberName] String propertyName = "")
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }

        public ComputerHostElement SelectedItem
        {
            set
            {
                if (value != null)
                {
                    Console.WriteLine(value.hostName);
                }

            }
        }

        private bool CanExecuteSelectedCommand(object data) => true;

    }
}
