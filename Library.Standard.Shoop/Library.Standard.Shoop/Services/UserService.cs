using Library.Shoop.Models;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Library.Shoop.Services
{
    public class UserService
    {

        private string persistPath = $"{Environment.GetFolderPath(Environment.SpecialFolder.LocalApplicationData)}";

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
        private static UserService current;

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

            //if (!Directory.Exists())
            //{
            //    Directory.CreateDirectory(cartPersistPath);
            //}
        }


        // public method to add a product to the cart
        public void Add(Product product, int amount)
        {
            // only add the product if it is in stock
            if (product is ProductByQuantity)
            {
                var quantityProduct = product as ProductByQuantity;

                if (quantityProduct != null)
                {
                    if (quantityProduct.typeOfProduct > 0)
                    {
                        product.Id = NextId;
                        cartList.Add(product);
                        quantityProduct.productAmount = amount;

                        // if the product in inventory is less than the quantity in the cart, then the product is removed from the cart if not the quantity is reduced
                        if (quantityProduct.typeOfProduct > 1)
                        {
                            quantityProduct.typeOfProduct -= amount;
                        }
                        else
                        {
                            adminService.Remove(product.Id);
                        }
                    }
                }
            }
            else if (product is ProductByWeight)
            {
                var weightProduct = product as ProductByWeight;

                if (weightProduct != null)
                {
                    if (weightProduct.typeOfProduct > 0)
                    {
                        product.Id = NextId;
                        cartList.Add(product);
                        weightProduct.productAmount = amount;

                        // if the product in inventory is less than the quantity in the cart, then the product is removed from the cart if not the quantity is reduced
                        if (weightProduct.typeOfProduct > 1)
                        {
                            weightProduct.typeOfProduct -= amount;
                        }
                        else
                        {
                            adminService.Remove(product.Id);
                        }
                    }
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

            if (productToDelete is ProductByQuantity)
            {
                var quantityProduct = productToDelete as ProductByQuantity;

                if (quantityProduct != null)
                {
                    if (quantityProduct.typeOfProduct == 1)
                    {
                        adminService.AddProduct(productToDelete);
                    }
                    else
                    {
                        quantityProduct.typeOfProduct++;
                    }

                    cartList.Remove(productToDelete);
                }
            }
            else if (productToDelete is ProductByWeight)
            {
                var weightProduct = productToDelete as ProductByWeight;

                if (weightProduct != null)
                {
                    if (weightProduct.typeOfProduct == 1)
                    {
                        adminService.AddProduct(productToDelete);
                    }
                    else
                    {
                        weightProduct.typeOfProduct++;
                    }

                    cartList.Remove(productToDelete);
                }
            }
        }

        // print the cart list
        public void ListCart()
        {
            foreach (var product in cartList)
            {
                Console.WriteLine($"{product.Id} - {product.Name} - ${product.Price}");
            }
        }

        public void DeleteCart()
        {
            foreach (var product in cartList.ToList())
            {
               cartList.Remove(product);
            }
        }

        // Old Method
        // public method to search the cart
        //public void Search(string seachString)
        //{
        //    // find the product in the cart using the name or description

        //    var productToFind = cartList.FirstOrDefault(t => t.Name.ToLower().Contains(seachString.ToLower()) || t.Description.ToLower().Contains(seachString.ToLower()));

        //    if (productToFind == null)
        //    {
        //        Console.WriteLine("No product found");
        //    }

        //    // if the product is found then print the product information
        //    Console.WriteLine(productToFind);
        //}

        // public method to load the cart from a file
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
            cartList = JsonConvert.DeserializeObject<List<Product>>
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

            var productJson = JsonConvert.SerializeObject(cartList, new JsonSerializerSettings
            { TypeNameHandling = TypeNameHandling.All });
            File.WriteAllText(fileName, productJson);
        }

        // public method to checkout the items in the cart
        public void Checkout()
        {
            double subTotal = 0;
            double taxAmount;
            double total;


            // calculate the sub total and tax amount accounting bogo
            foreach (Product product in cartList)
            {
                if (product.IsBogo == true)
                {
                    if (product is ProductByQuantity)
                    {
                        var quantityProduct = product as ProductByQuantity;

                        if (quantityProduct != null)
                        {
                            var totalBogoQuan = (quantityProduct.productAmount / 2) + (quantityProduct.productAmount % 2);

                            subTotal += quantityProduct.Price * totalBogoQuan;
                        }

                    }
                    else if (product is ProductByWeight)
                    {
                        var weightProduct = product as ProductByWeight;

                        if (weightProduct != null)
                        {
                            var totalBogoWeight = (weightProduct.productAmount / 2) + (weightProduct.productAmount % 2);

                            subTotal += weightProduct.Price * totalBogoWeight;
                        }   
                    }
                }
                else
                {
                    subTotal += product.Price;
                }
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
                    return cartList;
                }

                if (_sort == 1)
                {
                    return cartList
                        .Where(i => string.IsNullOrEmpty(_query) || (i.Description.Contains(_query)
                            || i.Name.Contains(_query)))
                        .OrderBy(i => i.Name);
                }
                else if (_sort == 2)
                {
                    return cartList
                        .Where(i => string.IsNullOrEmpty(_query) || (i.Description.Contains(_query)
                            || i.Name.Contains(_query)))
                        .OrderBy(i => i.TotalPrice);
                }
                else if (_sort == 3)
                {
                    return cartList
                        .Where(i => string.IsNullOrEmpty(_query) || (i.Description.Contains(_query)
                            || i.Name.Contains(_query)))
                        .OrderBy(i => i.Price);
                }
                else
                {
                    return cartList;
                }
            }
        }
    }
}
