using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Threading;
using OpenQA.Selenium;
using OpenQA.Selenium.Chrome;

namespace test
{
    public class Zlibrary
    {
        public static IWebDriver driver = new ChromeDriver();
        public static Dictionary<int, string> booksInfo = new Dictionary<int, string>();
        public static Dictionary<int, string> booksURL = new Dictionary<int, string>();
        public static ArrayList bookList = new ArrayList();

        public static void startServer(string searchKeyword)
        {
            driver.Navigate().GoToUrl("https://eg1lib.org/");
            driver.FindElement(By.Id("searchFieldx")).SendKeys(searchKeyword + Keys.Enter);
            listBooks(ref driver);

            for (int i = 2; i <= 2; i++)
            {
                driver.FindElement(By.XPath("/html/body/table/tbody/tr[2]/td/div/div/div/div[3]/table/tbody/tr[1]/td[3]/table/tbody/tr[1]/td[" + i + "]/span/a")).Click();
                listBooks(ref driver);
            }

            foreach (KeyValuePair<int, string> item in booksInfo)
            {
                Console.WriteLine(item.Key + " - " + item.Value);
            }
            driver.Quit();
            Console.WriteLine("\nEnter the numer of the book you want to download...");
            string bookNumerAsTxt = Console.ReadLine();
            int bookNumber = Convert.ToInt32(bookNumerAsTxt);

            string BookURL = booksURL[bookNumber];
            Console.WriteLine("\n * " + BookURL);
            downloadBook(BookURL);
        }
        public static void listBooks(ref IWebDriver driver)
        {
            var books = driver.FindElements(By.CssSelector("#searchResultBox > div:nth-child(n)"));
            for (int i = 2; i <= books.Count; i += 2)
            {
                try
                {
                    String bookName = driver.FindElement(By.XPath("//*[@id=\"searchResultBox\"]/div[" + i + "]/div/table/tbody/tr/td[2]/table/tbody/tr[1]/td/h3/a")).Text;
                    string bookURL = driver.FindElement(By.XPath("//*[@id=\"searchResultBox\"]/div[" + i + "]/div/table/tbody/tr/td[2]/table/tbody/tr[1]/td/h3/a")).GetAttribute("href");
                    bookList.Add(bookName);
                    booksInfo.Add(bookList.IndexOf(bookName) + 1, bookName);
                    booksURL.Add(bookList.IndexOf(bookName) + 1, bookURL);
                }
                catch (System.Exception e)
                {
                    Console.WriteLine(e.Message);
                }
            }
        }
        public static void downloadBook(String url)
        {
            var driver = new ChromeDriver();
            driver.Navigate().GoToUrl(url);
            string bookName = driver.FindElement(By.XPath("/html/body/table/tbody/tr[2]/td/div/div/div/div[2]/div[1]/div[2]/h1")).Text;
            string downloadURL = driver.FindElement(By.XPath("/html/body/table/tbody/tr[2]/td/div/div/div/div[2]/div[2]/div[1]/div[1]/div/a")).GetAttribute("href");
            Console.WriteLine(downloadURL);
            driver.FindElement(By.XPath("/html/body/table/tbody/tr[2]/td/div/div/div/div[2]/div[2]/div[1]/div[1]/div/a")).Click();
           

           /* This function is copied from stackoverflow >> https://stackoverflow.com/a/29644840*/
            var downloadsPath = Environment.GetEnvironmentVariable("USERPROFILE") + @"\Downloads\" + bookName;
            for (var i = 0; i < 30; i++)
            {
                if (File.Exists(downloadsPath)) { break; }
                Thread.Sleep(1000);
            }
            var length = new FileInfo(downloadsPath).Length;
            for (var i = 0; i < 30; i++)
            {
                Thread.Sleep(1000);
                var newLength = new FileInfo(downloadsPath).Length;
                if (newLength == length && length != 0) { break; }
                length = newLength;
            }
            driver.Quit();
        }
    }
}