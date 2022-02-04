using System;
using System.Collections;
using System.Collections.Generic;
using System.Net;
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

            for (int i = 2; i <= 10; i++)
            {
                driver.FindElement(By.XPath("/html/body/table/tbody/tr[2]/td/div/div/div/div[3]/table/tbody/tr[1]/td[3]/table/tbody/tr[1]/td[" + i + "]/span/a")).Click();
                listBooks(ref driver);
            }

            foreach (KeyValuePair<int, string> item in booksURL)
            {
                Console.WriteLine(item.Key + " - " + item.Value);
            }
            driver.Quit();
            Console.WriteLine("\nEnter the numer of the book you want to download...");
            int bookNumber = int.Parse(Console.ReadLine());
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
            WebClient myWebClient = new WebClient();
            myWebClient.DownloadFile(downloadURL, bookName);
            driver.Quit();
        }
    }
}