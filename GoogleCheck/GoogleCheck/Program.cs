using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading;
using System.Threading.Tasks;

namespace GoogleCheck
{
    class Program
    {
        static void Main(string[] args)
        {
            while (true) { 
                var randomNumber = new Random();
                var privateNewsDb = new PrivateNewsDb();
                Console.WriteLine("Bat dau kiem tra ket noi mang...");
                var listModel = privateNewsDb.GetValuesForGoogle();
                Console.WriteLine("Bat dau lay thong tin tu google...");
                foreach (var item in listModel)
                {
                    Console.ResetColor();
                    var numberNext = randomNumber.Next(40000, 120000);
                    var number = GetCountOfResultFromGoogle(item.PhoneNumer);
                    Thread.Sleep(numberNext);
                    Console.WriteLine("Bat dau doc tin....");
                    if (number > 1000)
                    {
                        privateNewsDb.Update(item.Id, 3);
                        Console.ForegroundColor = ConsoleColor.Green;
                        Console.WriteLine("So dien thoai :" + item.PhoneNumer + " la so moi gioi. Id:" + item.Id);

                    }
                    else {
                        if (number == 0)
                        {
                            Console.WriteLine("Khong the ket noi den google.com do qua so lan yeu cau! Thu lai trong thoi gian toi!");
                        }
                        else {
                            privateNewsDb.Update(item.Id, 1);
                            Console.ForegroundColor = ConsoleColor.Red;
                            Console.WriteLine("So dien thoai :" + item.PhoneNumer + " la so chinh chu. Id:" + item.Id);
                        }
                    }
                }
                Thread.Sleep(60000);
            }
        }

        /// <summary>
        /// Get count of result from Google
        /// </summary>
        /// <param name="keyword"></param>
        /// <returns></returns>
        private static int GetCountOfResultFromGoogle(string keyword)
        {
            //Get content of google result
            var buildQuery = "https://www.google.com/search?q=" + keyword;
            var resultHtml = GetContent(buildQuery);
            var resultFormat = GetBetween(resultHtml, "About", "results<nobr>").Replace(",", "");
            int resultNumber;
            if (!Int32.TryParse(resultFormat, out resultNumber))
            {
                resultNumber = 0;
            }
            return resultNumber;
        }

        /// <summary>
        /// Get content of web site
        /// </summary>
        /// <param name="url"></param>
        /// <returns></returns>
        private static string GetContent(string url)
        {
            var content = String.Empty;
            try
            {
                var requestUrl = (HttpWebRequest)WebRequest.Create(url);

                //--Start add proxy - Remove lines code and config from App.config - If you don't need
                //IWebProxy proxy = new WebProxy("fsoft-proxy", 8080); 
                //proxy.Credentials = new NetworkCredential("hault1", "Ginta@123");
                //request.Proxy = proxy;
                //--End add proxy
                var request = TryAddCookie(requestUrl, new List<Cookie> ());
                request.UseDefaultCredentials = true;
                request.Timeout = 15 * 1000;
                request.Method = "GET";
                request.UserAgent = "Mozilla/5.0 (Windows NT 6.1; WOW64) AppleWebKit/537.36 (KHTML, like Gecko) Chrome/47.0.2526.106 Safari/537.36";
                HttpStatusCode statusCode;
                using (var response = request.GetResponse())
                using (var stream = response.GetResponseStream())
                {
                    var contentType = response.ContentType;
                    Encoding encoding = null;
                    if (contentType != null)
                    {
                        var match = Regex.Match(contentType, @"(?<=charset\=).*");
                        if (match.Success)
                            encoding = Encoding.GetEncoding(match.ToString());
                    }

                    encoding = encoding ?? Encoding.UTF8;

                    statusCode = ((HttpWebResponse)response).StatusCode;
                    using (var reader = new StreamReader(stream, encoding))
                        content = reader.ReadToEnd();
                }
            }
            catch (Exception)
            {

                Console.WriteLine("Khong the ket noi den google.com do qua so lan yeu cau! Thu lai trong thoi gian toi!");
            }
            return content;
        }

        private static HttpWebRequest TryAddCookie(WebRequest webRequest, List<Cookie> cookie)
        {
            HttpWebRequest httpRequest = webRequest as HttpWebRequest;
            if (httpRequest == null)
            {
                return httpRequest;
            }

            if (httpRequest.CookieContainer == null)
            {
                httpRequest.CookieContainer = new CookieContainer();
            }
            foreach (var item in cookie)
            {
                httpRequest.CookieContainer.Add(item);
            }
            return httpRequest;
        }

        /// <summary>
        /// Get result from data
        /// </summary>
        /// <param name="strSource"></param>
        /// <param name="strStart"></param>
        /// <param name="strEnd"></param>
        /// <returns></returns>
        public static string GetBetween(string strSource, string strStart, string strEnd)
        {
            int Start, End;
            if (strSource.Contains(strStart) && strSource.Contains(strEnd))
            {
                Start = strSource.IndexOf(strStart, 0) + strStart.Length;
                End = strSource.IndexOf(strEnd, Start);
                return strSource.Substring(Start, End - Start);
            }
            else
            {
                return "";
            }
        }

    }
}
