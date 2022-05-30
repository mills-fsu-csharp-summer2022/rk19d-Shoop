using Library.Shoop.Models;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Library.Shoop.Services
{
    public class AdminService
    {

        // create a list of products for inventory
        private List<Product> inventoryList;

        // public method to return the list of products
        public List<Product> Inventory
        {
            get
            {
                return inventoryList;
            }
        }

        // sets the ID of products in the inventory
        public int NextId
        {
            get
            {
                if (!Inventory.Any())
                {
                    return 1;
                }

                return Inventory.Select(t => t.Id).Max() + 1;
            }
        }

        // private instance of admin
        private static AdminService? current;

        // public static method to create a new instance of admin
        public static AdminService Current
        {
            get
            {
                if (current == null)
                {
                    current = new AdminService();
                }
                return current;
            }
        }

        // public constructor that creates a new list of products for inventory
        public AdminService()
        {
            inventoryList = new List<Product>();
        }

        // public method to add a new product to the inventory
        public void AddProduct(Product product)
        {
            product.Id = NextId;
            inventoryList.Add(product);
        }

        // public method to remove a product from the inventory
        public void Remove(int id)
        {
            var productToRemove = inventoryList.FirstOrDefault(t => t.Id == id);

            if (productToRemove != null)
            {
                inventoryList.Remove(productToRemove);
            }
            else
            {
                return;
            }
        }

        // public method to update a product in the inventory
        public void Update(Product product)
        {
            var oldProduct = inventoryList.FirstOrDefault(t => t.Id == product.Id);

            if (oldProduct != null)
            {
                oldProduct.Name = product.Name;
                oldProduct.Price = product.Price;
                oldProduct.Quantity = product.Quantity;
            }
            else
            {
                return;
            }
        }

        // print the inventory
        public void ListInventory()
        {
            foreach (var product in inventoryList)
            {
                Console.WriteLine(product);
            }
        }

        // search the inventory by name or description
        public void Search(string seachString)
        {
            var productToFind = inventoryList.FirstOrDefault(t => t.Name.ToLower().Contains(seachString.ToLower()) || t.Description.ToLower().Contains(seachString.ToLower()));

            if (productToFind == null)
            {
                Console.WriteLine("No product found");
            }

            Console.WriteLine(productToFind);
        }
        
        // Admin Menu
        public static void AdminMenu()
        {
            Console.WriteLine("1. Add to Inventory");
            Console.WriteLine("2. Remove from Inventory");
            Console.WriteLine("3. Update Product in Inventory");
            Console.WriteLine("4. List Inventory");
            Console.WriteLine("5. Exit");
        }
    }
}
