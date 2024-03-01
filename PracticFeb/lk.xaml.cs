using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.IO;
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
using System.IO;

namespace PracticFeb
{
    /// <summary>
    /// Логика взаимодействия для lk.xaml
    /// </summary>
    public partial class lk : Window
    {
        public lk()
        {
            InitializeComponent();
        }
        AppDbContext context;
        User user { get; set; }
        double c = 0;
        public lk(User u)
        {
            InitializeComponent();
            context = new AppDbContext();
            user = context.Users.Include(x => x.CartItems).Where(x => x.Id == u.Id).FirstOrDefault();
            c = 0;
            Spisok();
        }

        private void Spisok()
        {
            Style buttonStyle = (Style)this.FindResource("Style");

            List<CartItem> cartItems = context.CartItems.Include(x => x.Product).Where(x => user.CartItems.Contains(x)).ToList();
            bool e = false;
            sp_cart.Children.Clear();
            c = 0;
            foreach (CartItem cartItem in cartItems)
            {
                c += Convert.ToDouble(cartItem.Product.Price * cartItem.Amount);

                Grid grid = new Grid();
                grid.ColumnDefinitions.Add(new ColumnDefinition() { Width = new GridLength(100) });
                grid.ColumnDefinitions.Add(new ColumnDefinition() { });
                grid.ColumnDefinitions.Add(new ColumnDefinition() { Width = new GridLength(30) });
                grid.ColumnDefinitions.Add(new ColumnDefinition() { Width = new GridLength(30) });
                grid.ColumnDefinitions.Add(new ColumnDefinition() { Width = new GridLength(30) });
                grid.RowDefinitions.Add(new RowDefinition() { Height = new GridLength(50) });
                grid.Margin = new Thickness(2.5);

                Image image = new Image();
                try
                {
                    image.Source = new BitmapImage(new Uri(Path.Combine(Environment.CurrentDirectory, "Pictures", cartItem.Product.Photo)));
                }
                catch
                {
                    e = true;
                }
                image.SetValue(Grid.ColumnProperty, 0);
                image.Margin = new Thickness(5);

                TextBlock textBlock = new TextBlock();
                textBlock.Text = cartItem.Product.Name + "    " + Convert.ToString(cartItem.Product.Price*cartItem.Amount)+" рублей";
                textBlock.HorizontalAlignment = HorizontalAlignment.Left;
                textBlock.VerticalAlignment = VerticalAlignment.Top;
                textBlock.FontSize = 20;
                textBlock.TextWrapping = TextWrapping.Wrap;
                textBlock.SetValue(Grid.ColumnProperty, 1);

                var converter = new BrushConverter();

                Button decButton = new Button();
                decButton.Background = new SolidColorBrush(Color.FromArgb(0, 0, 0, 0));
                decButton.BorderBrush = new SolidColorBrush(Color.FromArgb(0, 0, 0, 0));
                decButton.Style = buttonStyle;
                decButton.Content = "-";
                decButton.Foreground = Brushes.Black;
                decButton.FontWeight = FontWeights.Bold;
                decButton.FontSize = 20;
                decButton.Name = $"dec_{cartItem.Id}";
                decButton.Click += new RoutedEventHandler(Minus);
                decButton.Margin = new Thickness(5);
                decButton.SetValue(Grid.ColumnProperty, 2);

                TextBlock counter = new TextBlock();
                counter.Text = cartItem.Amount.ToString();
                counter.HorizontalAlignment = HorizontalAlignment.Center;
                counter.VerticalAlignment = VerticalAlignment.Center;
                counter.FontSize = 20;
                counter.SetValue(Grid.ColumnProperty, 3);

                Button incButton = new Button();
                incButton.Background = new SolidColorBrush(Color.FromArgb(0, 0, 0, 0));
                incButton.BorderBrush = new SolidColorBrush(Color.FromArgb(0, 0, 0, 0));
                incButton.Style = buttonStyle;
                incButton.Content = "+";
                incButton.Foreground = Brushes.Black;
                incButton.FontWeight = FontWeights.Bold;
                incButton.FontSize = 20;
                incButton.Name = $"inc_{cartItem.Id}";
                incButton.Click += new RoutedEventHandler(Plus);
                incButton.Margin = new Thickness(5);
                incButton.SetValue(Grid.ColumnProperty, 4);

                grid.Children.Add(image);
                grid.Children.Add(textBlock);
                grid.Children.Add(decButton);
                grid.Children.Add(counter);
                grid.Children.Add(incButton);

                sp_cart.Children.Add(grid);

                itog.Text = $"К оплате: {c} рублей";
            }
        }

        private void Plus(object sender, RoutedEventArgs e)
        {
            int id = Convert.ToInt32(((FrameworkElement)sender).Name.Split("_")[1]);
            context.CartItems.Where(x => x.Id == id).FirstOrDefault().Amount++;
            context.SaveChanges();
            Spisok();
        }

        private void Minus(object sender, RoutedEventArgs e)
        {
            int id = Convert.ToInt32(((FrameworkElement)sender).Name.Split("_")[1]);
            context.CartItems.Where(x => x.Id == id).FirstOrDefault().Amount--;
            if (context.CartItems.Where(x => x.Id == id).FirstOrDefault().Amount == 0) context.CartItems.Remove(context.CartItems.Where(x => x.Id == id).FirstOrDefault());
            context.SaveChanges();
            Spisok();
        }

        private void b_order_Click(object sender, RoutedEventArgs e)
        {
            MessageBox.Show($"Заказ оформлен");
            itog.Text = "";
            context.Users.Include(x => x.CartItems).Where(x => x.Id == user.Id).FirstOrDefault().CartItems.Clear();
            context.SaveChanges();
            Spisok();
        }

        private void b_catalog_Click(object sender, RoutedEventArgs e)
        {
            this.Hide();
            Mainn mainn = new Mainn(user);
            mainn.Show();
        }
    }
}
