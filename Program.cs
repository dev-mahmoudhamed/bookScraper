using System;

namespace test
{
    class Program
    {


        static void Main(string[] args)
        {
            Console.WriteLine("What are you looking for ?");
            string searchKeyword = Console.ReadLine();

            
            eg1lib eg1lib = new eg1lib();
            eg1lib.startServer(searchKeyword);
        }
    }
}