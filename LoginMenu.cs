using EGBuyBLL;
using EGBuyModel;
using EGBuyUtilities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading;
using System.Threading.Tasks;
using System.Xml;

namespace EGBuy
{
    public class LoginMenu
    {
        static LoginMenu() 
        {
            Console.Clear();
            Program.LoadDashboard();
        }
        public static void LoadLoginPage()
        {
            if (Utilities.Session() == null)
            {
                Console.Clear();
                Program.LoadDashboard();
                Console.WriteLine("Enter Email");
                var email = Console.ReadLine();
                Console.WriteLine("Enter Password");
                var password = Console.ReadLine();
                var user = UserManagement.GetUsers().Where(u => u.Email.Equals(email) && u.Password.Equals(password)).FirstOrDefault();
                if (user == null)
                {
                    Console.WriteLine("User not found with the username and password");
                    Console.WriteLine("1.Try again");
                    Console.WriteLine("2.Register");
                    Console.WriteLine("Select choice and hit enter. Any other choice to back Home");
                    int Choice = 0;
                    int.TryParse(Console.ReadLine(), out Choice);
                    if (Choice == 1)
                    {
                        LoadLoginPage();
                    }
                    else if(Choice==2)
                    {
                        LoadRegisterPage();
                    }
                    else
                    {
                        Program.LoadHome();
                    }
                }
                else
                {
                    Console.WriteLine("Logged in successfully");
                    Console.WriteLine("Press any key to Home");
                    Console.ReadKey();
                    Utilities.AddSession(user);
                    Program.LoadHome();
                }
            }
            else
            {
                Console.WriteLine("You are already logged in");
                Console.WriteLine(Environment.NewLine);
                Console.WriteLine("1. Logout");
                Console.WriteLine("Select choice and hit enter. Any other choice to back Home");
                int Choice = 0;
                int.TryParse(Console.ReadLine(), out Choice);
                if (Choice == 1)
                {
                    Utilities.RemoveSession();
                    Console.WriteLine("Logged out");
                    Console.WriteLine("Press any key to Home");
                    Console.ReadKey();
                    Program.LoadHome();
                }
                else
                {
                    Program.LoadHome();
                }
            }
        }
        public static void LoadRegisterPage()
        {
            Console.Clear();
            Program.LoadDashboard();
            User user = new User();
            Console.WriteLine("Enter Name");
            user.Name = Console.ReadLine();
            ValidateEmail:
            Console.WriteLine("Enter Email");
            user.Email = Console.ReadLine();
            bool isEmail = Regex.IsMatch(user.Email, @"\A(?:[a-z0-9!#$%&'*+/=?^_`{|}~-]+(?:\.[a-z0-9!#$%&'*+/=?^_`{|}~-]+)*@(?:[a-z0-9](?:[a-z0-9-]*[a-z0-9])?\.)+[a-z0-9](?:[a-z0-9-]*[a-z0-9])?)\Z", RegexOptions.IgnoreCase);
            if (!isEmail)
            {
                Console.ForegroundColor = ConsoleColor.Red;
                Console.WriteLine("Please enter a valid email");
                Console.ResetColor();
                goto ValidateEmail;
            }
            if (UserManagement.GetUser(user.Email) != null)
            {
                Console.ForegroundColor = ConsoleColor.Red;
                Console.WriteLine("Email already registered. Please go to login page");
                Console.ResetColor();
                goto ValidateEmail;
            }
            ValidateContact:
            Console.WriteLine("Enter Contact");
            user.ContactNumber = Console.ReadLine();
            bool isPhone = Regex.IsMatch(user.ContactNumber, @"^[0-9]{10}$", RegexOptions.IgnoreCase);
            if (!isPhone)
            {
                Console.ForegroundColor = ConsoleColor.Red;
                Console.WriteLine("Please enter a valid contact");
                Console.ResetColor();
                goto ValidateContact;
            }
            Console.WriteLine("Enter Address");
            user.Address = Console.ReadLine();
            ValidatePassword:
            Console.WriteLine("Enter Password");
            user.Password= Console.ReadLine();
            //if(!Regex.IsMatch(user.Password, @"^{8-15}$"))
            //{
            //    Console.ForegroundColor = ConsoleColor.Red;
            //    Console.WriteLine("Password should be 8-15 characters");
            //    Console.ResetColor();
            //    goto ValidatePassword;
            //}
            Console.WriteLine(UserManagement.AddUser(user));
            Console.WriteLine("Press any key to go to home");
            Console.ReadKey();
            Program.LoadHome();
        }
    }
}
