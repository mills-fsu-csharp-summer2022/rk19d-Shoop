using Library.Shoop.Models;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Library.Shoop.Services
{
    public class UserService
    {
        // create a list of products called cartList
        private List<Product> cartList;

        // create an admin service
        private readonly AdminService adminService;

        // public method to get the cartList
        public List<Product> Cart
        {
            get
            {
                return cartList;
            }
        }

        // sets the ID of products in the cart
        public int NextId
        {
            get
            {
                if (!Cart.Any())
                {
                    return 1;
                }

                return Cart.Select(t => t.Id).Max() + 1;
            }
        }

        // private instance of the user service
        private static UserService? current;

        // public static method to get the current instance of the user service
        public static UserService Current
        {
            get
            {
                if (current == null)
                {
                    current = new UserService();
                }

                return current;
            }
        }

        // private constructor for the user service that creates a new list of products and creates an instance of the admin service
        private UserService()
        {
            cartList = new List<Product>();
            adminService = AdminService.Current;
        }

        // public method to add a product to the cart
        public void Add(Product product)
        {
            // only add the product if it is in stock

            if (product.Quantity > 0)
            {
                product.Id = NextId;
                cartList.Add(product);

                // if the product in inventory is less than the quantity in the cart, then the product is removed from the cart if not the quantity is reduced
                if (product.Quantity > 1)
                {
                    product.Quantity--;
                }
                else
                {
                    adminService.Remove(product.Id);
                }
            }
            else
            {
                // if the product is not in stock, then show an error message
                Console.WriteLine("Sorry, we don't have this item in stock");
                return;
            }
        }

        // public method to remove a product from the cart
        public void Delete(int id)
        {
            // find the product in the cart using the ID
            var productToDelete = cartList.FirstOrDefault(t => t.Id == id);
            if (productToDelete == null)
            {
                return;
            }

            // if the product is found then add it back to the inventory and remove it from the cart
            if (productToDelete.Quantity == 1)
            {
                adminService.AddProduct(productToDelete);
            }
            else
            {
                productToDelete.Quantity++;
            }
            
            cartList.Remove(productToDelete);

        }

        // print the cart list
        public void ListCart()
        {
            foreach (var product in cartList)
            {
                Console.WriteLine($"{product.Id} - {product.Name} - ${product.Price}");
            }
        }

        // public method to search the cart
        public void Search(string seachString)
        {
            // find the product in the cart using the name or description

            var productToFind = cartList.FirstOrDefault(t => t.Name.ToLower().Contains(seachString.ToLower()) || t.Description.ToLower().Contains(seachString.ToLower()));

            if (productToFind == null)
            {
                Console.WriteLine("No product found");
            }

            // if the product is found then print the product information
            Console.WriteLine(productToFind);
        }

        // public method to load the cart from a file
        public void Load(string fileName)
        {
            var productJson = File.ReadAllText(fileName);
            cartList = JsonConvert.DeserializeObject<List<Product>>(productJson) ?? new List<Product>();
        }

        // public method to save the cart to a file with the name of the file
        public void Save(string fileName)
        {
            var productJson = JsonConvert.SerializeObject(cartList);
            File.WriteAllText(fileName, productJson);
        }

        // public method to checkout the items in the cart
        public void Checkout()
        {
            double subTotal = 0;
            double taxAmount = 0;
            double total = 0;

            // calculate the sub total and tax amount
            foreach (var product in cartList)
            {
                subTotal += product.Price;
            }

            taxAmount = subTotal * 0.07;
            total = subTotal + taxAmount;

            Console.WriteLine("Subtotal: " + subTotal);
            Console.WriteLine("Tax: " + taxAmount);
            Console.WriteLine("Total: " + total);

        }

        // User menu 
        public static void UserMenu()
        {
            Console.WriteLine("\n1. Add to Cart");
            Console.WriteLine("2. Remove from Cart");
            Console.WriteLine("3. View Cart");
            Console.WriteLine("4. Search Cart or Inventory");
            Console.WriteLine("5. Save Cart");
            Console.WriteLine("6. Load Cart");
            Console.WriteLine("7. Checkout");
            Console.WriteLine("8. Exit");
        }
    }
}
