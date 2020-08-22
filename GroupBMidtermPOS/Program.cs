﻿using System;
using System.Collections.Generic;
using System.Xml.Schema;

namespace GroupBMidtermPOS
{
    static class Program
    {
        static void Main(string[] args)
        {
            var shoppingCart = new List<KeyValuePair<Product, int>>();
            Register register = new Register(); //open a new Register
            var clearConsole = false;

            DisplayHeader();

            do
            {
                Console.WriteLine($"Menu: Choose an Item # or [: + search term] to search");
                Menu.DisplayMainMenu(register, clearConsole);
                //call the Validator.cs -- this is a new class that will hold all the user input validation before anything else happens
                var userItem = 0;
                do
                {
                    var userItemAsString = Console.ReadLine();
                    if (userItemAsString.StartsWith(":"))
                    {
                        SearchForProduct(userItemAsString.Substring(1));

                    }
                    else
                    {
                        if (ValidateInput.GetIsInteger(userItemAsString))
                        {
                            userItem = int.Parse(userItemAsString) - 1;
                            break;
                        }
                        else
                        {
                            Console.WriteLine("Please input a valid integer.");
                        }
                    }

                } while (true);

                var userItemQuantity = GetUserItemQuantity();
                var kvpUserSelection =
                    new KeyValuePair<Product, int>(GetProduct(register.listOfProducts, userItem), userItemQuantity);
                shoppingCart.Add(kvpUserSelection);
                Console.ForegroundColor = ConsoleColor.DarkYellow;
                Console.WriteLine("-------------------------------------------------------------");
                Console.ForegroundColor = ConsoleColor.Green;
                Console.WriteLine($"Transaction total: {register.GetSubtotal(kvpUserSelection)}");
                Console.WriteLine($"Subtotal: {register.GetSubtotal(shoppingCart)}");
                clearConsole = true;
                Console.ForegroundColor = ConsoleColor.Gray;
            } while (AskToContinueToShop());

            Menu.DisplayOrderSummary(shoppingCart, register);
            var payment = Menu.AskForPaymentMethodMenu();
            TakePayment(payment);

            static bool AskToContinueToShop()
            {
                Console.WriteLine("Would you like to continue to shop? (Y/N)");
                var continueYesNo = Console.ReadLine().ToLower();
                if (ValidateInput.CheckYesNo(continueYesNo))//todo 
                {
                    if (continueYesNo == "y")
                    {
                        return true;
                    }
                }
                else
                {
                    Console.WriteLine("Please make a valid input");
                    AskToContinueToShop();
                }

                return false;
            }
            static bool AskToCheckOut()
            {
                Console.WriteLine("Are you ready to check out? (Y/N) ");
                var checkOutYesNo = Console.ReadLine().ToLower();
                return false;
            }

            static Product GetProduct(List<Product> productList, int userChoice)
            {
                Product choice = productList[userChoice];
                return choice;
            }

            void TakePayment(PaymentTypeEnum paymentType)
            {

                switch (paymentType)
                {
                    case PaymentTypeEnum.Cash:
                        register.TakePaymentCash(20.00, 15.00);
                        break;
                    case PaymentTypeEnum.Check:
                        register.TakePaymentCheck();
                        break;
                    case PaymentTypeEnum.Credit_Card:
                        register.TakePaymentCreditCard();
                        break;
                    default:
                        break;
                }
            }

            void SearchForProduct(string descriptor)
            {
                var results = register.ProductSearch(descriptor, register.listOfProducts);
                if (results.Count < 1)
                {
                    Console.WriteLine("No products were found that match the search string.");
                }
                else
                {
                    Menu.DisplayAllProducts(results);
                }

            }

        }

        private static void DisplayHeader()
        {
            Console.ForegroundColor = ConsoleColor.DarkRed;
            Console.WriteLine("WELCOME TO CHUCKY'S TOY KINGDOM!!!");
            Console.ForegroundColor = ConsoleColor.DarkYellow;
            Console.WriteLine("**********************************");
            Console.WriteLine();
        }

        public static int GetUserItemQuantity()
        {
            Console.ForegroundColor = ConsoleColor.DarkYellow;
            Console.WriteLine("Enter Quantity:");
            //maybe move this to Validator.cs class (new)
            var takeUserQuantity =
                int.TryParse(Console.ReadLine(), out int userItemQuantity); // take user user's quantity
            if (!takeUserQuantity)
            {
                Console.WriteLine("Something went wrong");
            }

            return userItemQuantity;
        }
    }
}