using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PracticFeb
{
    public class User
    {
        public int Id { get; set; }
        public string Login { get; set; }
        public string Password { get; set; }

        public string Email { get; set; }

        public List<CartItem> CartItems { get; set; }
    }

    public class Product
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public decimal Price { get; set; }
        public string Photo { get; set; }
        public string Description { get; set; }
    }

    public class CartItem
    {
        public int Id { get; set; }
        public Product Product { get; set; }
        public int Amount { get; set; }
    }
}
