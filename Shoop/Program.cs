using System;
using Library.Shoop.Models;
using Library.Shoop.Services;
using Microsoft.Azure.KeyVault.Models;

namespace Shoop // Note: actual namespace depends on the project name.
{
    internal class Program
    {
        static void Main(string[] args)
        {

            int response; // Variable to ask if the person using the program is a user or an admin.

            do
            {
                // Small menu to ask if the person using the program is a user or an admin.
                Console.WriteLine("\nWelcome to Shoop!");
                Console.WriteLine("Are you a user or admin?");

                Console.WriteLine("1: User");
                Console.WriteLine("2: Admin");
                Console.WriteLine("0: Exit");

                response = int.Parse(Console.ReadLine() ?? "1");
                
                // USER SECTION 
                if (response == 1)
                {
                    Console.WriteLine("You Chose User");
                    var user = UserService.Current; // create a instance of an user
                    int input;
                    
                    Console.WriteLine("\nWelcome to Shoop!");

                    do
                    {
                        // show the user menu
                        UserService.UserMenu();
                        input = int.Parse(Console.ReadLine() ?? "1"); // get the user input defaulted to 1 (add to cart)

                        if (input == 1)
                        {
                            Console.WriteLine("You chose to Add to cart");
                            AdminService.Current.ListInventory(); // List the inventory from admin instance to the user

                            Console.WriteLine("Enter the ID of the item you would like to add to your cart");
                            int itemId = int.Parse(Console.ReadLine() ?? "1"); // get the product id from user to add to cart defaulted to 1

                            var productToAdd = AdminService.Current.Inventory.FirstOrDefault(t => t.Id == itemId); // look for the product in the inventory

                            if (productToAdd != null) // if the product is found add it to the cart
                            {
                                user.Add(productToAdd);
                            }
                            else // if the product is not found, show a message
                            {
                                Console.WriteLine("That item does not exist");
                            }
                        }
                        else if (input == 2) // Remove from the cart
                        {
                            Console.WriteLine("You chose to Remove from Cart");

                            user.ListCart(); // show the products in the cart

                            Console.WriteLine("Enter the ID of the item you would like to remove from your cart");
                            int itemId = int.Parse(Console.ReadLine() ?? "1"); // get the product id from user to remove from cart defaulted to 1

                            user.Delete(itemId); // call the delete method from the user service
                        }
                        else if (input == 3)
                        {
                            Console.WriteLine("You chose to View Cart"); // show the products in the cart

                            user.ListCart();

                        }
                        else if (input == 4)
                        {
                            Console.WriteLine("You chose to Search the Cart or Inventory");

                            // ask the user if they want to search the cart or inventory
                            Console.WriteLine("1: Search the Cart"); 
                            Console.WriteLine("2: Search the Inventory");

                            int search = int.Parse(Console.ReadLine() ?? "1");

                            // if the user chose to search the cart, ask the user for name or description
                            if (search == 1)
                            {
                                Console.WriteLine("You chose to Search the Cart");

                                Console.WriteLine("Enter the name or description of the item you would like to search for");
                                string searchItem = Console.ReadLine() ?? "1";

                                // call the search method from the user service and pass in the search string
                                user.Search(searchItem);
                            }
                            else if (search == 2)
                            {

                                // if the user chose to search the inventory, ask the user for name or description
                                Console.WriteLine("You chose to Search the Inventory");

                                Console.WriteLine("Enter the name or description of the item you would like to search for");
                                string searchItem = Console.ReadLine() ?? "1";


                                // call the admin search method from the admin service and pass in the search string
                                AdminService.Current.Search(searchItem);
                            }
                        }
                        else if (input == 5)
                        {
                            // call the save method from the user service and send in a file name defaulted to saveData.json
                            Console.WriteLine("You chose to Save Cart");
                            user.Save("SaveData.json");

                        }
                        else if (input == 6)
                        {
                            // call the load method from the user service
                            Console.WriteLine("You chose to Load Cart");
                            user.Load("SaveData.json");
                        }
                        else if (input == 7)
                        {
                            Console.WriteLine("You chose to Checkout");

                            if (user.Cart.Count == 0)
                            {
                                // if the cart is empty, show a message
                                Console.WriteLine("Your cart is empty");
                            }
                            else
                            {
                                // if the cart is not empty, show the total, tax, and subtotal and ask the user if they want to checkout
                                user.Checkout();

                                Console.WriteLine("\nDo you want to checkout?");
                                Console.WriteLine("1: Yes");
                                Console.WriteLine("2: No");

                                int checkout = int.Parse(Console.ReadLine() ?? "1");

                                // if the user chose to checkout exit program.
                                if (checkout == 1)
                                {
                                    Console.WriteLine("Thank you for shopping with us!");
                                    Environment.Exit(0);
                                }
                                else if (checkout == 2)
                                {
                                    // if the user chose to not checkout, show the user menu
                                    Console.WriteLine("You chose to cancel checkout");
                                }

                            }
                        }
                        else if (input == 8)
                        {
                            Console.WriteLine("Going back!");
                        }
                        else
                        {
                            Console.WriteLine("Incorrect Option, Try Again");
                        }
                    } while (input != 8);
                }
                // ADMIN SECTION
                else if (response == 2)
                {
                    // create an instance of an admin
                    var admin = AdminService.Current;
                    int adminInput;

                    Console.WriteLine("\nWelcome to Admin Shoop!");
                    do
                    {
                        // show the admin menu
                        Console.WriteLine("\n");
                        AdminService.AdminMenu();

                        adminInput = int.Parse(Console.ReadLine() ?? "0"); // ask the admin for input

                        if (adminInput == 1)
                        {
                            // add a new item to inventory
                            Console.WriteLine("You chose to Add to Inventory");

                            // create a product and send it to the fillProduct function and then send it to the AddProduct method in the admin service
                            var newProduct = new Product();
                            fillProduct(newProduct);
                            admin.AddProduct(newProduct);
                        }
                        else if (adminInput == 2)
                        {
                            // remove an item from inventory
                            Console.WriteLine("You chose to Remove from Inventory");
                            Console.WriteLine("Enter ID of product to delete");
                            
                            admin.ListInventory(); // list the inventory

                            // get the product id from the user and send it to the Remove method in the admin service
                            var id = int.Parse(Console.ReadLine() ?? "0");
                            admin.Remove(id);
                        }
                        else if (adminInput == 3)
                        {
                            // Update an item in the inventory
                            Console.WriteLine("You chose to Update Inventory");
                            admin.ListInventory();

                            // get the product id from the user
                            Console.WriteLine("Enter ID of product to update");
                            var idUpdate = int.Parse(Console.ReadLine() ?? "0");

                            // find the product in the inventory using the ID
                            var productToUpdate = admin.Inventory.FirstOrDefault(t => t.Id == idUpdate);

                            // if the product is found, update the information and send it to the admin service to update the inventory
                            if (productToUpdate != null)
                            {
                                fillProduct(productToUpdate);
                                admin.Update(productToUpdate);
                            }
                            else
                            {
                                // if the product is not found, show a message
                                Console.WriteLine("Product not found");
                            }
                        }
                        else if (adminInput == 4)
                        {
                            // show the inventory
                            Console.WriteLine("You chose to List Inventory");
                            admin.ListInventory();
                        }
                        else if (adminInput == 5)
                        {
                            Console.WriteLine("Going back!");
                        }
                        else
                        {
                            Console.WriteLine("Incorrect Option, Try Again");
                        }
                    } while (adminInput != 5);
                }
                else if (response == 0)
                {
                    // exit the program
                    Console.WriteLine("Goodbye!");
                    Environment.Exit(0);
                }
                else
                {
                    Console.WriteLine("Incorrect Option, Try Again");
                }
            } while (response != 0);
            // END OF PROGRAM

            // Function to fill product objects
            static void fillProduct(Product? product)
            {
                if (product == null)
                {
                    return;
                }

                Console.WriteLine("What is the name of the product?");
                product.Name = Console.ReadLine() ?? string.Empty;

                Console.WriteLine("What is the description of the product?");
                product.Description = Console.ReadLine() ?? string.Empty;

                Console.WriteLine("What is the price of the product?");
                product.Price = double.Parse(Console.ReadLine() ?? "0");

                Console.WriteLine("What is the quantity of the product?");
                product.Quantity = int.Parse(Console.ReadLine() ?? "0");
            }
        }
    }
}
// END OF CODE