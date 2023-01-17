using PC_GAMING_BAZE.Models;
using PC_GAMING_BAZE.ViewModel;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
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
    /// Логика взаимодействия для ReleasedPage.xaml
    /// </summary>
    public partial class ReleasedPage : Page
    {
        public MainWindow MainWindow;
        //public ObservableCollection<ComputerHostElement> HostsCollection { get; set; }
        public ReleasedPage(MainWindow mainWindow)
        {
            MainWindow = mainWindow;
            //this.DataContext = new AppVM(MainWindow);
            //this.DataContext = MainWindow;
            InitializeComponent();
        }
    }
}
