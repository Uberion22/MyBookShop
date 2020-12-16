using BookShop.Models;
using HtmlAgilityPack;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Text;
using System.Web;

namespace BookShop.parser
{
    /// <summary>
    /// Класс предназначен для извлечения информации о книгах с сайта book24.ru, в целях быстрого наполнения базы данных книг
    /// работоспособен по состоянию на 16.12.2020
    /// </summary>
    public static class Parser
    {
        private static readonly string basicURL = "https://book24.ru";
        public static string GetDataByTag(HtmlNode node, string tag, string _class)
        {
            var data = HttpUtility.HtmlDecode(node.SelectNodes(node.XPath + tag)
                  .Where(el => el.Attributes["class"].Value.Contains(_class))
                  .Select(el => el.InnerText)
                  .Aggregate("", (acc, val) => acc += val?.ToString())
                  );

            return data;
        }
        /// <summary>
        /// Метод предназначен для выборки информации о книге по типу тэга html и классу css
        /// </summary>
        public static Book GetAllBookData(HtmlNode item)
        {
            var name = GetDataByTag(item, "//a", "book-preview__title-link");
            var temp_price = (GetDataByTag(item, "//div", "book-preview__price-current")).ToLower().Split('р');
            var price = Decimal.Parse(temp_price[0].Trim(' '));
            var author = GetDataByTag(item, "//a", "book-preview__author-link");
            if (String.IsNullOrEmpty(author))
                author = GetDataByTag(item, "//span", "book-preview__author-link");
            var imgURL = HttpUtility.HtmlDecode(item.SelectNodes(item.XPath + "//img")
              .Select(el => el.Attributes["data-src"].Value)
              .Aggregate("", (acc, val) => acc += val?.ToString())
              );
            var originalURL = basicURL + HttpUtility.HtmlDecode(item.SelectNodes(item.XPath + "//a")
                              .Where(el => el.Attributes["class"].Value.Contains("book-preview__title-link"))
                              .Select(el => el.Attributes["href"].Value)
                              .Aggregate("", (acc, val) => acc += val?.ToString())
                              );
            Book book = new Book() 
            {
                Name = name,
                Author = author,
                Price = price,
                ImageURL = imgURL,
                OriginalPageURL = originalURL,
                PublicationDate = DateTime.Now
            };
            
            return book;
        }

        public static IEnumerable<Book> GetBooks(int pageCount)
        {
            var books = new List<Book>();
            var doc = new HtmlDocument();
            var http = basicURL+ "/catalog/";
            for (int i = 1; i < pageCount + 1; i++)
            {
                if (i > 1)
                {
                    http = basicURL + "/catalog/page-" + i + "/";
                }
                var txtHTML = GetPage(@http);
                doc.LoadHtml(txtHTML);
                var Nodes = doc.DocumentNode
                                      .SelectNodes("//div")
                                      .Where(el => el.Attributes["class"].Value.Contains("book-preview _d js-catalog-element-card"));
                
                foreach (var item in Nodes)
                {
                    var book = GetAllBookData(item);
                    books.Add(book);
                }
            }
            
            return books;
        }

        public static string GetPage(string url)
        {
            var result = String.Empty;
            var request = (HttpWebRequest)WebRequest.Create(url);
            var response = (HttpWebResponse)request.GetResponse();
            
            if (response.StatusCode == HttpStatusCode.OK)
            {
                var responseStream = response.GetResponseStream();
                
                if (responseStream != null)
                {
                    StreamReader streamReader;

                    if (response.CharacterSet != null)
                    {
                        streamReader = new StreamReader(responseStream, Encoding.GetEncoding(response.CharacterSet));
                    }
                    else
                    {
                        streamReader = new StreamReader(responseStream);
                    }
                    result = streamReader.ReadToEnd();
                    streamReader.Close();
                }
                response.Close();
            }
            
            return result;
        }

    }
}
