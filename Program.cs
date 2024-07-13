using EGBuyBLL;
using EGBuyModel;
using EGBuyUtilities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace EGBuy
{
    public class Program
    {
        static void Main(string[] args)
        {
            LoadHome();
            Console.ReadKey();
        }
        public static void LoadHome()
        {
            Console.Clear();
            LoadDashboard();
            LoadDeals();
            LoadMenu();
        }
        public static void LoadDashboard()
        {
            Console.ForegroundColor = ConsoleColor.Gray;
            Console.WriteLine("-----------------------------------------------------------------------------------------");
            Console.ForegroundColor = ConsoleColor.DarkRed;
            Console.BackgroundColor = ConsoleColor.White;
            Console.WriteLine("                                         EGBUY                                           ");
            Console.ResetColor();
            Console.ForegroundColor = ConsoleColor.Gray;
            Console.WriteLine("-----------------------------------------------------------------------------------------");
        }
        public static void LoadDeals()
        {
            try
            {
                List<Product> ProductList = ProductManagement.GetProductListByDiscount(20);
                if (ProductList.Count > 0)
                {
                    Console.ForegroundColor = ConsoleColor.Yellow;
                    Console.Write("*** DEALS FOR TODAY ***  ");
                    Console.ResetColor();
                    Console.WriteLine("Get flat 20% discount on below products");
                    Console.WriteLine(Environment.NewLine);
                    foreach (Product product in ProductList)
                    {
                        Console.Write("\t"+product.Name+"\t");
                    }
                    Console.WriteLine(Environment.NewLine);
                }
                else
                {
                    //Top deals bool  variable on and off
                }
            }
            catch(Exception ex)
            {
                //Do nothing. No Product will be loading
            }
        }

        public static void LoadMenu()
        {
            Console.ForegroundColor = ConsoleColor.Gray;
            Console.WriteLine("-----------------------------------------------------------------------------------------");
            Console.ForegroundColor = ConsoleColor.Red;
            //Console.BackgroundColor = ConsoleColor.White;
            Console.WriteLine("MENU                                           ");
            Console.ResetColor();
            Console.ForegroundColor = ConsoleColor.Gray;
            Console.WriteLine("-----------------------------------------------------------------------------------------");
            List<Menu> MenuList = MenuManagement.GetMenuItems();
            if (MenuList.Count > 0)
            {
                foreach (Menu menu in MenuList)
                {
                    Console.WriteLine("{0} - {1}",menu.MenuId,menu.MenuName);
                }
                Console.WriteLine("Select your choice and hit enter");
                int Choice = 0;
                int.TryParse(Console.ReadLine(),out Choice);
                if(Choice > 0 && Choice <= MenuList.Count)
                {
                    Console.Clear();
                    LoadDashboard();
                    switch (Choice)
                    {
                        case 1:
                            DashboardMenu.LoadCategories();
                            break;
                        case 2:
                            DashboardMenu.LoadProducts();
                            break;
                        case 3:
                            LoginMenu.LoadLoginPage();
                            break;
                        case 4:
                            LoginMenu.LoadRegisterPage();
                            break;
                        case 5:
                            var user = Utilities.Session();
                            if (user == null)
                            {
                                Console.WriteLine("Please login to go to cart. Press any key to login page");
                                Console.ReadKey();
                                LoginMenu.LoadLoginPage();
                            }
                            else
                                DashboardMenu.LoadCart(user.UserId);
                            break;
                        case 6:
                            Environment.Exit(0);
                            break;
                        default:
                            Console.ForegroundColor = ConsoleColor.Red;
                            Console.WriteLine("Invalid selection. Please try again.");
                            LoadMenu();
                            break;
                    }       
                }
                else
                {
                    Console.ForegroundColor = ConsoleColor.Red;
                    Console.WriteLine("Invalid selection. Please try again.");
                    LoadMenu();
                }
            }
        }
    }
}
