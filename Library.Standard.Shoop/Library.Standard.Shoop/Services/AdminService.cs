using Library.Shoop.Models;
using Library.Shoop.Utility;
using Library.Standard.Shoop.Utility;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Library.Shoop.Services
{
    public class AdminService
    {

        private string persistPath = $"{Environment.GetFolderPath(Environment.SpecialFolder.LocalApplicationData)}"; 

        private ListNavigator<Product> listNavigator;
        
        // create a list of products for inventory
        private List<Product> inventoryList;

        // public method to return the list of products
        public List<Product> Inventory
        {
            get
            {
                var productJson = new WebRequestHandler().Get("http://localhost:5260/Product");
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
        private static AdminService current;

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
        private AdminService()
        {

            var productJson = new WebRequestHandler().Get("http://localhost:5260/Product").Result;
            inventoryList = JsonConvert.DeserializeObject<List<Product>>(productJson);

           // listNavigator = new ListNavigator<Product>(inventoryList);
        }

        // public method to add a new product to the inventory
        public void AddOrUpdate(Product product)
        {

            if (product is ProductByQuantity)
            {
                var response = new WebRequestHandler().Post("http://localhost:5260/Quantity/AddOrUpdateQuantity", product).Result;
                var newQuanProd = JsonConvert.DeserializeObject<ProductByQuantity>(response);

                var oldVersion = inventoryList.FirstOrDefault(i => i.Id == newQuanProd.Id);
                if (oldVersion != null)
                {
                   var index = inventoryList.IndexOf(oldVersion);
                   inventoryList.RemoveAt(index);
                   inventoryList.Insert(index, newQuanProd);
                }
                else
                {
                    inventoryList.Add(newQuanProd);
                }
            }
            else if (product is ProductByWeight)
            {
                var response = new WebRequestHandler().Post("http://localhost:5260/Weight/AddOrUpdateWeight", product).Result;
                var newWeightProd = JsonConvert.DeserializeObject<ProductByWeight>(response);


                var oldVersion = inventoryList.FirstOrDefault(i => i.Id == newWeightProd.Id);
                if (oldVersion != null)
                {
                    var index = inventoryList.IndexOf(oldVersion);
                    inventoryList.RemoveAt(index);
                    inventoryList.Insert(index, newWeightProd);
                }
                else
                {
                    inventoryList.Add(newWeightProd);
                }
            }
        }

        // public method to remove a product from the inventory
        public void Remove(int id)
        {
            var response = new WebRequestHandler().Get($"http://localhost:5260/Product/Delete/{id}");
            var productToDelete = inventoryList.FirstOrDefault(t => t.Id == id);

            if (productToDelete == null)
            {
                return;
            }

            inventoryList.Remove(productToDelete);
        }
        
        // public method to update a product in the inventory -- *NOT USED in UWP Version*
        public void Update(Product product)
        {

            if (product == null)
            {
                return;
            }
            
            var oldProduct = inventoryList.FirstOrDefault(t => t.Id == product.Id);

            if (oldProduct != null)
            {
                oldProduct.Name = product.Name;
                oldProduct.Price = product.Price;

                if (oldProduct is ProductByQuantity)
                {
                    var oldProductByQuantity = oldProduct as ProductByQuantity;

                    if (oldProductByQuantity != null)
                    {
                        Console.WriteLine("What is the quantity of the product?");
                        oldProductByQuantity.typeOfProduct = int.Parse(Console.ReadLine() ?? "0");
                    }
                }
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

        public void Load(string fileName = null)
        {

            if (string.IsNullOrEmpty(fileName))
            {
                fileName = $"{persistPath}\\SaveData.json";
            }
            else
            {
                fileName = $"{persistPath}\\{fileName}.json";
            }

            var productJson = File.ReadAllText(fileName);
            inventoryList = JsonConvert.DeserializeObject<List<Product>>
                (productJson, new JsonSerializerSettings
                { TypeNameHandling = TypeNameHandling.All }) ?? new List<Product>();
        }

        // public method to save the cart to a file with the name of the file
        public void Save(string fileName = null)
        {

            if (string.IsNullOrEmpty(fileName))
            {
                fileName = $"{persistPath}\\SaveData.json";
            }
            else
            {
                fileName = $"{persistPath}\\{fileName}.json";
            }

            var productJson = JsonConvert.SerializeObject(inventoryList, new JsonSerializerSettings
            { TypeNameHandling = TypeNameHandling.All });
            File.WriteAllText(fileName, productJson);
        }


        // stateless method - old
        // search the inventory by name or description
        //public void Search(string seachString)
        //{
        //    var productToFind = inventoryList.FirstOrDefault(t => t.Name.ToLower().Contains(seachString.ToLower()) || t.Description.ToLower().Contains(seachString.ToLower()));

        //    if (productToFind == null)
        //    {
        //        Console.WriteLine("No product found");
        //    }

        //    Console.WriteLine(productToFind);
        //}

        // Admin Menu
        public static void AdminMenu()
        {
            Console.WriteLine("1. Add to Inventory");
            Console.WriteLine("2. Remove from Inventory");
            Console.WriteLine("3. Update Product in Inventory");
            Console.WriteLine("4. List Inventory");
            Console.WriteLine("5. Exit");
        }

        // search the inventory by name or description and filter it by name, desc, or price
        // Statefull method

        private string _query;
        private int _sort;

        public IEnumerable<Product> Search(string query, int sort)
        {
            _query = query;
            _sort = sort;
            return ProcessedList;
        }
        public IEnumerable<Product> ProcessedList
        {
            get
            {
                if (string.IsNullOrEmpty(_query))
                {
                    return inventoryList;
                }
                
                if (_sort == 1)
                {
                    return inventoryList
                        .Where(i => string.IsNullOrEmpty(_query) || (i.Description.Contains(_query)
                            || i.Name.Contains(_query)))
                        .OrderBy(i => i.Name);
                }
                else if (_sort == 2)
                {
                    return inventoryList
                        .Where(i => string.IsNullOrEmpty(_query) || (i.Description.Contains(_query)
                            || i.Name.Contains(_query)))
                        .OrderBy(i => i.TotalPrice);
                }
                else if (_sort == 3)
                {
                    return inventoryList
                        .Where(i => string.IsNullOrEmpty(_query) || (i.Description.Contains(_query)
                            || i.Name.Contains(_query)))
                        .OrderBy(i => i.Price);
                }
                else
                {
                    return inventoryList;
                }
            }
        }

    }
}
