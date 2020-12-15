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
    public static class Parser
    {
        public static string GetDataByTeg(HtmlNode node, string teg, string _class)
        {
            var data = HttpUtility.HtmlDecode(node.SelectNodes(node.XPath + teg)
                  .Where(el => el.Attributes["class"].Value.Contains(_class))
                  .Select(el => el.InnerText)
                  .Aggregate("", (acc, val) => acc += val?.ToString())
                  );
            return data;
        }
        public static Book GetAllBookData(HtmlNode item)
        {
            var name = GetDataByTeg(item, "//a", "book-preview__title-link");

            string[] _price = (GetDataByTeg(item, "//div", "book-preview__price-current")).ToLower().Split('р');
            var price = Decimal.Parse(_price[0].Trim(' '));
            string author = "";
            author = GetDataByTeg(item, "//a", "book-preview__author-link");
            if (author == "")
                author = GetDataByTeg(item, "//span", "book-preview__author-link");

            var imgURL = HttpUtility.HtmlDecode(item.SelectNodes(item.XPath + "//img")
              .Select(el => el.Attributes["data-src"].Value)
              .Aggregate("", (acc, val) => acc += val?.ToString())
              );

            var originalURL = "https://book24.ru" + HttpUtility.HtmlDecode(item.SelectNodes(item.XPath + "//a")
                              .Where(el => el.Attributes["class"].Value.Contains("book-preview__title-link"))
                              .Select(el => el.Attributes["href"].Value)
                              .Aggregate("", (acc, val) => acc += val?.ToString())
                              );
            Book _book = new Book() { Name = name, Author = author, Price = price, ImageURL = imgURL, OriginalPageURL = originalURL, PublicationDate = DateTime.Now };
            return _book;
        }
        public static IEnumerable<Book> GetBooks(int pageCount)
        {
            List<Book> books = new List<Book>();
            var doc = new HtmlDocument();// Создание документа
            var http = @"https://book24.ru/catalog/";
            for (int i = 1; i < pageCount + 1; i++)
            {
                if (i > 1)
                    http = @"https://book24.ru/catalog/" + "page-" + i + "/";
                var txtHTML = GetPage(http);
                doc.LoadHtml(txtHTML);
                IEnumerable<HtmlNode> _Nodes = doc.DocumentNode
                                      .SelectNodes("//div")
                                      .Where(el => el.Attributes["class"].Value.Contains("book-preview _d js-catalog-element-card"));

                foreach (var item in _Nodes)
                {
                    Book book = GetAllBookData(item);
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
                        streamReader = new StreamReader(responseStream, Encoding.GetEncoding(response.CharacterSet));
                    else
                        streamReader = new StreamReader(responseStream);
                    result = streamReader.ReadToEnd();
                    streamReader.Close();
                }
                response.Close();
            }
            return result;
        }
        public static void DownlodImage(string _imgURL, string _imageFolder, string _imageName)
        {
            _ = new Uri(_imgURL);
            WebClient web = new WebClient(); ;
            web.DownloadFile(_imgURL, Path.Combine(_imageFolder, _imageName));
        }
    }
}
