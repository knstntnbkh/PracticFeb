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
using System.Windows.Shapes;

namespace PracticFeb
{
    /// <summary>
    /// Логика взаимодействия для Window1.xaml
    /// </summary>
    public partial class Window1 : Window
    {
        public Window1()
        {
            InitializeComponent();
        }

        private void windowavto_Click(object sender, RoutedEventArgs e)
        {
            this.Hide();
            MainWindow mainWindow = new MainWindow();
            mainWindow.Show();
        }

        private void zareg_Click(object sender, RoutedEventArgs e)
        {
            var loginn = login.Text;

            var passs = pass.Text;

            var maill = mail.Text;

            var context = new AppDbContext();

            var user_exists = context.Users.FirstOrDefault(x => x.Login == loginn);
            if(user_exists is not null)
            {
                MessageBox.Show("Такой пользователь уже зарегистрирован");
                return;
            }    
            var user = new User { Login = loginn, Password = passs, Email= maill };
            context.Users.Add(user);
            context.SaveChanges();
        }
    }
}
