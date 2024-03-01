using System;
using System.Collections.Generic;
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

namespace PracticFeb
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        
        public MainWindow()
        {
            InitializeComponent();
        }
        public static class SharedData
        {
            public static int ID { get; set; }
        }
        private void windowreg_Click(object sender, RoutedEventArgs e)
        {
            this.Hide();
            Window1 window1 = new Window1();
            window1.Show();
        }

        private void avtoriz_Click(object sender, RoutedEventArgs e)
        {
            var login = loginnn.Text;
            var pass = passsss.Text;

            var context = new AppDbContext();

            var user = context.Users.SingleOrDefault(x => x.Login == login && x.Password == pass);
            if (user is null)
            {
                MessageBox.Show("Неправильный логин или пароль");
                return;
            }
            else
            {
                SharedData.ID = user.Id;
                this.Hide();
                Mainn mainn = new Mainn(user);
                mainn.Show();
            }
        }
    }
}
