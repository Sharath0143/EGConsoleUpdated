using EGBuyBLL;
using EGBuyModel;
using EGBuyUtilities;
using System;
using System.Collections.Generic;
using System.ComponentModel.Design;
using System.Diagnostics;
using System.Linq;
using System.Reflection.Emit;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Xml.Serialization;

namespace EGBuy
{
    public class DashboardMenu
    {
        public static void LoadCategories()
        {
           
            Console.Clear();
            Program.LoadDashboard();
            var Categories = CategoryManagement.GetCategories();
            if (Categories.Count > 0)
            {
                for (int i = 0; i < Categories.Count; i++)
                {
                    var category = Categories[i];
                    Console.WriteLine(category.CategoryId + " - " + category.Name);
                }
                if (Utilities.Session()!=null)
                {
                    if (Utilities.Session().IsAdmin)
                    {
                        Console.ForegroundColor = ConsoleColor.Yellow;
                        Console.WriteLine("1.Add\n2.Update\n3.Delete");
                        Console.ResetColor();
                    }
                }
                Console.WriteLine("Select your choice and hit enter. Any other key to Home");
                int Choice = 0;
                int.TryParse(Console.ReadLine(), out Choice);
                if (Choice > 0 && Choice <= Categories.Count)
                {
                    if (Utilities.Session() != null)
                    {
                        if (Utilities.Session().IsAdmin)
                        {
                            switch (Choice)
                            {
                                case 1:
                                    CategoryManagement.AddCategory();
                                    break;
                                case 2:
                                    CategoryManagement.UpdateCategory();
                                    break;
                                case 3:
                                    Console.WriteLine("Enter category number");
                                    var categoryNum = 0;
                                    int.TryParse(Console.ReadLine(), out categoryNum);
                                    CategoryManagement.RemoveCategory(categoryNum);
                                    break;
                                default:
                                    LoadCategories();
                                    break;
                            }
                            LoadCategories();
                        }
                        else
                        {
                            LoadProducts(Choice);
                        }
                    }
                    else
                    {
                        LoadProducts(Choice);
                    }
                }
                else
                {
                    Console.Clear();
                    Program.LoadHome();
                }
            }
        }
        public static void LoadProducts(int categoryId)
        {
            var products = ProductManagement.GetProducts(categoryId);
            for (int i = 0; i < products.Count; i++)
            {
                Console.ForegroundColor = ConsoleColor.Yellow;
                Console.WriteLine("ProductNumber {0}{1}ProductName {2}{3}Description{4}{5}Price {6}", products[i].ProductId, Environment.NewLine, products[i].Name, Environment.NewLine, products[i].Description, Environment.NewLine, products[i].Price);
                Console.ForegroundColor = ConsoleColor.Red;
                Console.WriteLine("-----------------------------------------------------------------------------------------");
                Console.ResetColor();
            }
            Console.WriteLine("Select your choice to add to cart. Any other key to Home");
            int Choice = 0;
            int.TryParse(Console.ReadLine(), out Choice);
            if (Choice > 0 && Choice <= products.Count)
            {
                Choice -= 1;

                if (Utilities.Session() == null)
                {
                    Console.ForegroundColor = ConsoleColor.Red;
                    Console.WriteLine("Please login first. Redirecting to login page in 3 seconds");
                    Thread.Sleep(3000);
                    Console.ResetColor();
                    LoginMenu.LoadLoginPage();
                }
                else
                {
                    Console.WriteLine("Enter Quantity");
                    int quantity = 0;
                    int.TryParse(Console.ReadLine(), out quantity);
                    if (quantity > 0 && products[Choice].Quantity >= quantity)
                    {
                        //bool result=ProductManagement.UpdateProduct(products[Choice].ProductId, quantity);
                        //if (result)
                        //{
                        Cart cart = new Cart { ProductId = products[Choice].ProductId, Quantity = quantity, UserId = Utilities.Session().UserId };
                        var result = CartManagement.AddCart(cart);
                        Console.WriteLine(result);
                        Console.WriteLine("Any key to return categories");
                        Console.ReadKey();
                        LoadCategories();
                        //}
                    }
                    else
                    {
                        Console.WriteLine("Not enough quantity available");
                        Console.WriteLine("Any key to return categories");
                        Console.ReadKey();
                        LoadCategories();
                    }
                }
                
            }
            else
            {
                Console.Clear();
                Program.LoadHome();
            }
        }
        public static void LoadProducts()
        {
            Console.Clear();
            Program.LoadDashboard();
            var products = ProductManagement.GetProducts();
            for (int i = 0; i < products.Count; i++)
            {
                Console.ForegroundColor = ConsoleColor.Yellow;
                Console.WriteLine("ProductNumber {0}{1}ProductName {2}{3}Description {4}{5}Price {6}", products[i].ProductId, Environment.NewLine, products[i].Name, Environment.NewLine, products[i].Description, Environment.NewLine, products[i].Price);
                Console.ForegroundColor = ConsoleColor.Red;
                Console.WriteLine("-----------------------------------------------------------------------------------------");
                Console.ResetColor();
            }
            if (Utilities.Session() != null)
            {
                if (Utilities.Session().IsAdmin)
                {
                    Console.ForegroundColor = ConsoleColor.Yellow;
                    Console.WriteLine("1.Add\n2.Update\n3.Delete");
                    Console.ResetColor();
                }
            }
            Console.WriteLine("Select your choice to add to cart. Any other key to Home");
            int Choice = 0;
            int.TryParse(Console.ReadLine(), out Choice);
            if (Choice > 0 && Choice <= products.Count)
            {
                if (Utilities.Session() != null)
                {
                    if (Utilities.Session().IsAdmin)
                    {
                        switch (Choice)
                        {
                            case 1:
                                ProductManagement.AddProduct();
                                break;
                            case 2:
                                ProductManagement.UpdateProduct();
                                break;
                            case 3:
                                Console.WriteLine("Enter product number");
                                var productNum = 0;
                                int.TryParse(Console.ReadLine(), out productNum);
                                ProductManagement.RemoveProduct(productNum);
                                break;
                            default:
                                LoadProducts();
                                break;
                        }
                        LoadProducts();
                    }
                    else
                    {
                        Choice -= 1;
                        if (Utilities.Session() == null)
                        {
                            Console.ForegroundColor = ConsoleColor.Red;
                            Console.WriteLine("Please login first. Any key to Login page");
                            Console.ReadKey();
                            LoginMenu.LoadLoginPage();
                        }
                        else
                        {
                            Console.WriteLine("Enter Quantity");
                            int quantity = 0;
                            int.TryParse(Console.ReadLine(), out quantity);
                            if (quantity > 0 && products[Choice].Quantity >= quantity)
                            {
                                //bool result=ProductManagement.UpdateProduct(products[Choice].ProductId, quantity);
                                //if (result)
                                //{
                                Cart cart = new Cart { ProductId = products[Choice].ProductId, Quantity = quantity, UserId = Utilities.Session().UserId };
                                var result = CartManagement.AddCart(cart);
                                Console.WriteLine(result);
                                Console.WriteLine("Any key to return Home");
                                Console.ReadKey();
                                Program.LoadHome();
                                //}
                            }
                            else
                            {
                                Console.WriteLine("Not enough quantity available");
                                Console.WriteLine("Any key to return Home");
                                Console.ReadKey();
                                Program.LoadHome();
                            }
                        }
                    }
                }
                else
                {
                    Console.ForegroundColor = ConsoleColor.Red;
                    Console.WriteLine("Please login first. Any key to Login page");
                    Console.ReadKey();
                    LoginMenu.LoadLoginPage();
                }
            }
            else
            {
                Program.LoadHome();
            }
        }
        public static void LoadCart(int userId)
        {
            Console.Clear();
            Program.LoadDashboard();
            var result = from P in ProductManagement.GetProducts() join C in CartManagement.GetCarts() on P.ProductId equals C.ProductId where C.UserId == userId select new { CartId=C.CartId,Name=P.Name,Description=P.Description,Price=P.Price,Quantity=C.Quantity };
            var cartList = result.ToList();
            if (cartList.Count > 0)
            {
                for (int i = 0; i < cartList.Count; i++)
                {
                    Console.ForegroundColor = ConsoleColor.Yellow;
                    Console.WriteLine("CartNumber {0}{1}ProductName {2}{3}Description{4}{5}Price {6}{7}Quantity {8}", cartList[i].CartId, Environment.NewLine, cartList[i].Name, Environment.NewLine, cartList[i].Description, Environment.NewLine, cartList[i].Price, Environment.NewLine, cartList[i].Quantity);
                    Console.ForegroundColor = ConsoleColor.Red;
                    Console.WriteLine("-----------------------------------------------------------------------------------------");
                    Console.ResetColor();

                    Console.WriteLine("1. Update cart");
                    Console.WriteLine("2. Remove cart");
                    Console.WriteLine("3. Checkout");
                    Console.WriteLine("Press any key to Home");
                    int Choice = 0;
                    int.TryParse(Console.ReadLine(), out Choice);
                    switch(Choice) { 
                        case 1:
                            ValidateCart:
                            Console.WriteLine("Enter the cart number to update");
                            var cartId = 0;
                            int.TryParse(Console.ReadLine(), out cartId);
                            var cart=CartManagement.GetCarts().Where(c => c.CartId == cartId).FirstOrDefault();
                            if(cart == null)
                            {
                                Console.WriteLine("Invalid cart number");
                                goto ValidateCart;
                            }
                            ValidateQuantity:
                            Console.WriteLine("Enter the Quantity");
                            var quantity = 0;
                            int.TryParse(Console.ReadLine(), out quantity);
                            if (quantity <= 0)
                            {
                                Console.WriteLine("Invalid quantity");
                                goto ValidateQuantity;
                            }
                            else
                            {
                                cart.Quantity = quantity;
                                var updateResult = CartManagement.UpdateCart(cart);
                                if (updateResult == 0)
                                {
                                    Console.WriteLine("Insufficient Quantity. Updated with all the available Quantity");
                                    Console.WriteLine("Press any key");
                                    Console.ReadKey();
                                    DashboardMenu.LoadCart(userId);
                                }
                                else
                                {
                                    DashboardMenu.LoadCart(userId);
                                }
                                
                            }
                            break;
                        case 2:
                        ValidateCart2:
                            Console.WriteLine("Enter the cart number to remove");
                            var Id = 0;
                            int.TryParse(Console.ReadLine(), out Id);
                            var cartInfo = CartManagement.GetCarts().Where(c => c.CartId == Id).FirstOrDefault();
                            if (cartInfo == null)
                            {
                                Console.WriteLine("Invalid cart number");
                                goto ValidateCart2;
                            }
                            else
                            {
                                var product=ProductManagement.GetProducts().Where(c => c.ProductId == Id).FirstOrDefault();
                                Console.WriteLine("Do you want to remove Cart Product {0}",product.Name);
                                Console.WriteLine("1. Yes");
                                Console.WriteLine("Any other key to cancel");
                                var RemoveId = 0;
                                int.TryParse(Console.ReadLine(), out RemoveId);
                                if (RemoveId == 1) {
                                    CartManagement.RemoveCart(Id);
                                    DashboardMenu.LoadCart(userId);
                                }
                                else
                                {
                                    DashboardMenu.LoadCart(userId);
                                }
                            }
                            break;
                        case 3:
                            Checkout(userId);
                            break;
                        default: Program.LoadHome();
                            break;

                        }
                }
            }
            else
            {
                if (Utilities.Session() == null)
                {
                    Console.ForegroundColor = ConsoleColor.Red;
                    Console.WriteLine("Please login first. Press any key to Login");
                    Console.ReadKey();
                    Console.ResetColor();
                    LoginMenu.LoadLoginPage();
                }
                else
                {
                    Console.ForegroundColor = ConsoleColor.Yellow;
                    Console.WriteLine("Add items to the cart. Press any key to Categories");
                    Console.ReadKey();
                    Console.ResetColor();
                    DashboardMenu.LoadCategories();
                }
               
            }
        }
        public static void Checkout(int userId)
        {
            Console.Clear();
            Program.LoadDashboard();
            PrintInvoice(userId);

        }
        private static void PrintInvoice(int userId)
        {

            Console.Clear();
            Console.WriteLine("***************************INVOICE*************************");
            var User = UserManagement.GetUsers().Where(u => u.UserId == userId).FirstOrDefault();
            Console.WriteLine("Name : {0}\nAddress : {1}\nMob : {2}",User.Name,User.Address,User.ContactNumber);
            PrintLine();
            PrintRow("ProductId","Product", "Quantity", "Price", "Discount(%)","Amount");
            PrintLine();
            var results = from P in ProductManagement.GetProducts() join C in CartManagement.GetCarts() on P.ProductId equals C.ProductId join D in ProductManagement.GetDiscountList() on P.DiscountId equals D.DiscountId where C.UserId == userId where P.Quantity>=C.Quantity select new { CartId = C.CartId, Name = P.Name, Description = P.Description, Price = P.Price, Quantity = C.Quantity,ProductId=P.ProductId,Discount=D.DiscountPercentage };
            //PrintRow("", "", "", "","");
            //PrintRow("", "", "", "","");
            var Total = 0;
            if (results.Count() > 0)
            {
                foreach (var result in results) {
                    var Amount = 0;
                    int.TryParse(((result.Quantity * result.Price)-((result.Quantity * result.Price)*result.Discount/100)).ToString(),out Amount);
                    PrintRow(result.ProductId.ToString(), result.Name, result.Quantity.ToString(),result.Price.ToString() , result.Discount.ToString(),(Amount).ToString());
                    Total += Amount;
                }
                PrintLine();
                Console.WriteLine("GRAND TOTAL " + Total);
                Console.WriteLine("* Unavailabe quantity items excluded in the list");
                Console.WriteLine("1. Home\nAny other key to pay");
                int choice = 0;
                int.TryParse(Console.ReadLine(),out choice);
                if (choice == 1)
                {
                    Program.LoadHome();
                }
                else {
                    Console.WriteLine("Paid! Redirecting to Home in 3 Seconds");
                    Thread.Sleep(3000);
                    foreach (var result in results)
                    {
                        var product =ProductManagement.GetProducts().Where(p=>p.ProductId== result.ProductId).FirstOrDefault();
                        product.Quantity= product.Quantity-result.Quantity;
                        ProductManagement.UpdateProduct(product);
                    }
                }
                Program.LoadHome();
            }
        }
        static int tableWidth = 73;
        static void PrintLine()
        {
            Console.WriteLine(new string('-', tableWidth));
        }

        static void PrintRow(params string[] columns)
        {
            int width = (tableWidth - columns.Length) / columns.Length;
            string row = "|";

            foreach (string column in columns)
            {
                row += AlignCentre(column, width) + "|";
            }

            Console.WriteLine(row);
        }

        static string AlignCentre(string text, int width)
        {
            text = text.Length > width ? text.Substring(0, width - 3) + "..." : text;

            if (string.IsNullOrEmpty(text))
            {
                return new string(' ', width);
            }
            else
            {
                return text.PadRight(width - (width - text.Length) / 2).PadLeft(width);
            }
        }
    }
}
