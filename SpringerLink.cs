using System;
using System.Collections;
using System.Collections.Generic;
using System.Net;
using System.Threading;
using OpenQA.Selenium;
using OpenQA.Selenium.Chrome;

namespace Scraper
{
    public class SpringerLink
    {
        public static IWebDriver driver = new ChromeDriver();
        public static Dictionary<int, string> booksInfo = new Dictionary<int, string>();
        public static Dictionary<int, string> booksURL = new Dictionary<int, string>();
        public static ArrayList bookList = new ArrayList();


        public static void startServer(string searchKeyword)
        {
            driver.Navigate().GoToUrl("https://link.springer.com/search?facet-content-type=%22Book%22&facet-discipline=%22Computer+Science%22");
            driver.FindElement(By.Id("query")).SendKeys(searchKeyword + Keys.Enter);
            listBooks(ref driver);

            int pages = Convert.ToInt32(driver.FindElement(By.ClassName("number-of-pages")).Text);

            driver.FindElement(By.CssSelector("#kb-nav--main > div.functions-bar.functions-bar-top > form > a.next > img")).Click();
            listBooks(ref driver);

            for (int i = 2; i < 3; i++)
            {

                driver.FindElement(By.CssSelector("#kb-nav--main > div.functions-bar.functions-bar-top > form > a.next > img")).Click();
                listBooks(ref driver);
            }

            foreach (KeyValuePair<int, string> item in booksInfo)
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
            var books = driver.FindElements(By.CssSelector("#results-list > li:nth-child(n)"));
            for (int i = 1; i <= books.Count; i++)
            {
                try
                {
                    String bookName = driver.FindElement(By.CssSelector("#results-list > li:nth-child(" + i + ") > div.text > h2 > a")).Text;
                    string bookURL = driver.FindElement(By.CssSelector("#results-list > li:nth-child(" + i + ") > div.text > h2 > a")).GetAttribute("href");
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
            string bookName = driver.FindElement(By.XPath("//*[@id=\"main-content\"]/div/article[1]/div/div/div[1]/div/div/div[1]/div[2]/h1")).Text;
            IJavaScriptExecutor js = (IJavaScriptExecutor)driver;
            js.ExecuteScript("window.scrollBy(0,400)");
            String downloadURL = driver.FindElement(By.XPath("//*[@id=\"main-content\"]/div/article[1]/div/div/div[2]/div[1]/a")).GetAttribute("href"); ;
            Console.WriteLine("\n" + bookName + " -- " + downloadURL + "\n");
            driver.Quit();
            
        }
    }
}
