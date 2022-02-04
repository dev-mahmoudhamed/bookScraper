using System;
using Scraper;

namespace test
{
    class Program
    {


        static void Main(string[] args)
        {
            Console.ForegroundColor = ConsoleColor.Green;
            Console.WriteLine("What are you looking for ?");
            string searchKeyword = Console.ReadLine();

            Console.WriteLine("Please select server number...");
            int ServerNumber = Convert.ToInt32(Console.ReadLine());
            Console.ForegroundColor = ConsoleColor.Gray;

            switch (ServerNumber)
            {
                case 1:
                    Zlibrary.startServer(searchKeyword);
                    break;
                case 2:
                    SpringerLink.startServer(searchKeyword);
                    break;
            }
        }
    }
}