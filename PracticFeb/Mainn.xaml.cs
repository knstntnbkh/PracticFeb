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
    /// Логика взаимодействия для Mainn.xaml
    /// </summary>
    public partial class Mainn : Window
    {
        AppDbContext context;
        User user { get; set; }
        public Mainn(User u)
        {
            InitializeComponent();
            context = new AppDbContext();
            user = context.Users.Include(x => x.CartItems).Where(x => x.Id == u.Id).FirstOrDefault();
            Cart();

        }

        private void Cart()
        {
            bool e = false;
            wp_menu.Children.Clear();
            foreach (Product product in context.Products)
            {
                Grid grid = new Grid();
                grid.ColumnDefinitions.Add(new ColumnDefinition() { Width = new GridLength(100) });
                grid.RowDefinitions.Add(new RowDefinition() { Height = new GridLength(100) });
                grid.RowDefinitions.Add(new RowDefinition() { Height = new GridLength(40) });
                grid.RowDefinitions.Add(new RowDefinition() { Height = new GridLength(40) });
                grid.Margin = new Thickness(2);

                Image image = new Image();
                try
                {
                    image.Source = new BitmapImage(new Uri(Path.Combine(Environment.CurrentDirectory, "Pictures", product.Photo)));
                }
                catch
                {
                    e = true;
                }
                image.SetValue(Grid.RowProperty, 0);

                TextBlock textBlock = new TextBlock();
                textBlock.Text = product.Name + product.Price;
                textBlock.FontSize = 13;
                textBlock.TextWrapping = TextWrapping.Wrap;
                textBlock.SetValue(Grid.RowProperty, 1);

                Button button = new Button();
                button.Content = "Добавить";
                button.Foreground = Brushes.Black;
                var converter = new BrushConverter();
                button.FontWeight = FontWeights.Bold;
                button.FontSize = 17;
                button.Name = $"p_{product.Id}";
                button.Click += new RoutedEventHandler(AddItem);
                button.SetValue(Grid.RowProperty, 2);

                grid.Children.Add(image);
                grid.Children.Add(textBlock);
                grid.Children.Add(button);

                wp_menu.Children.Add(grid);
            }
            if (e) MessageBox.Show("Возникли ошибки при загрузке изображений, проверьте имена файлов");
        }

        private void AddItem(object sender, RoutedEventArgs e)
        {
            int id = Convert.ToInt32(((FrameworkElement)sender).Name.Split("_")[1]);
            if (context.CartItems.Where(x => x.Product.Id == id && user.CartItems.Contains(x)).FirstOrDefault() == null)
                context.Users.Include(x => x.CartItems).Where(x => x.Id == user.Id).FirstOrDefault()
                    .CartItems.Add(new CartItem() { Product = context.Products.Where(x => x.Id == id).FirstOrDefault(), Amount = 1 });
            context.SaveChanges();
        }

        private void Button_Click(object sender, RoutedEventArgs e)
        {
            this.Hide();
            lk lk = new lk(user);
            lk.Show();
        }
    }
}
