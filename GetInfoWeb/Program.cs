using HtmlAgilityPack;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Data.SqlClient;
using System.IO;
using System.Linq;
using System.Net;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading;
using System.Threading.Tasks;

namespace GetInfoWeb
{
    class Program
    {
        static void Main(string[] args)
        {
            //Console.WriteLine(GetCountOfResultFromGoogle("0982982690"));
            Console.ForegroundColor = ConsoleColor.Green;
            Console.WriteLine("== Lay thong tin trang: batdongsan.com.vn ==");
            Console.ResetColor();
            Thread.Sleep(1000);
            Console.ForegroundColor = ConsoleColor.Yellow;
            Console.WriteLine("Bat dau kiem tra ket noi mang...");
            Thread.Sleep(1000);
            try
            {
                for (int i = 0; i < 100; i++)
                {
                    var link = "http://batdongsan.com.vn/ban-nha-dat-tp-hcm/p" + i;
                    GetLinksFromWebsite(link, (i+1));
                }
            }
            catch (Exception)
            {
                Console.ResetColor();
                Console.ForegroundColor = ConsoleColor.Red;
                Console.WriteLine("Khong the ket noi voi internet. \nVui long kiem tra ket noi mang va cau hinh may tinh!\nNhan Enter de thoat...");
                Console.ReadLine();
            }
            Console.ReadLine();
        }

        /// <summary>
        /// Get count of result from Google
        /// </summary>
        /// <param name="keyword"></param>
        /// <returns></returns>
        private static int GetCountOfResultFromGoogle(string keyword) { 
            //Get content of google result
            var buildQuery = "https://www.google.com/search?q="+keyword;
            var resultHtml = GetContent(buildQuery);
            var resultFormat = GetBetween(resultHtml, "About", "results<nobr>").Replace(",", "");
            int resultNumber;
            if(!Int32.TryParse(resultFormat, out resultNumber)){
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
            var requestUrl = (HttpWebRequest)WebRequest.Create(url);

            //--Start add proxy - Remove lines code and config from App.config - If you don't need
            //IWebProxy proxy = new WebProxy("fsoft-proxy", 8080); 
            //proxy.Credentials = new NetworkCredential("hault1", "Ginta@123");
            //request.Proxy = proxy;
            //--End add proxy
            var request = TryAddCookie(requestUrl, new List<Cookie> { 
                new Cookie("__asc", "c99bb1d41538995d02618de55b8") { Domain = "batdongsan.com.vn" },
                new Cookie("_gat", "1") { Domain = "batdongsan.com.vn" },
                new Cookie("_ga", "GA1.3.1787916081.1458302207") { Domain = "batdongsan.com.vn" },
                new Cookie("__auc", "c99bb1d41538995d02618de55b8") { Domain = "batdongsan.com.vn" },
                new Cookie("psortfilter", "1%24all%24VOE%2FWO8MpO1adIX%2BwMGNUA%3D%3D") { Domain = "batdongsan.com.vn" }
                
            });
            request.Timeout = 15 * 1000;
            request.Method = "GET";
            request.UserAgent = "Mozilla/5.0 (Windows NT 6.1; WOW64) AppleWebKit/537.36 (KHTML, like Gecko) Chrome/47.0.2526.106 Safari/537.36";
            var content = String.Empty;
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

        /// <summary>
        /// 
        /// </summary>
        /// <param name="url"></param>
        /// <returns></returns>
        private static string GetLinksFromWebsite(string url, int number)
        {
            var result = String.Empty;
            var resultHtml = GetContent(url);
            //using Html Agility Pack
            var doc = new HtmlDocument();
            doc.LoadHtml(resultHtml);
            Console.ResetColor();
            Console.ForegroundColor = ConsoleColor.Cyan;
            Console.WriteLine("Bat dau doc du lieu tu trang " + number);
            Console.ResetColor();
            Console.ForegroundColor = ConsoleColor.Magenta;
            Thread.Sleep(1000);
            foreach (HtmlNode link in doc.DocumentNode.SelectNodes("//div[@class='p-title']//a"))
            {
                var news = new PrivateNews();
                Console.WriteLine("Tieu de: " + link.InnerText.Replace("\r", "").Replace("\n", "").Replace("&nbsp;", "").TrimStart().TrimEnd());
                news.Title = link.InnerText.Replace("\r", "").Replace("\n", "").Replace("&nbsp;", "").TrimStart().TrimEnd();
                Console.WriteLine("Link:" + link.Attributes["href"].Value.Replace("\r", "").Replace("\n", "").Replace("&nbsp;", "").TrimStart().TrimEnd());
                var subUrl = "http://batdongsan.com.vn" + link.Attributes["href"].Value;
                var subResultHtml = GetContent(subUrl);
                var docSub = new HtmlDocument();
                docSub.LoadHtml(subResultHtml);
                var subNodes = docSub.DocumentNode.SelectNodes("//span[@class='gia-title mar-right-15']//strong");
                foreach (var node in subNodes)
                {
                    var titleGia = node.InnerText.Replace("\r", "").Replace("\n", "").Replace("&nbsp;","").TrimStart().TrimEnd();
                    news.Price = titleGia;
                    Console.WriteLine("Gia:" + titleGia);
                }

                var subContent = docSub.DocumentNode.SelectNodes("//div[@class='pm-content stat']");
                foreach (var node in subContent)
                {
                    var titleContent = node.InnerText;
                    news.NewsContent = titleContent;
                    Console.WriteLine("Noidung:" + titleContent);
                }

                var subDientich = docSub.DocumentNode.SelectNodes("//span[@class='gia-title']//strong");
                foreach (var node in subDientich)
                {
                    var titleDientich = node.InnerText.Replace("\r", "").Replace("\n", "").Replace("&nbsp;", "").TrimStart().TrimEnd();
                    news.Dientich = titleDientich;
                    Console.WriteLine("Dientich:" + titleDientich);
                }

                var subPhone = docSub.DocumentNode.SelectNodes("//div[@id='LeftMainContent__productDetail_contactPhone']//div[@class='right']");
                if (subPhone != null) { 
                    foreach (var node in subPhone)
                    {
                        var titlePhone = node.InnerText.Replace("\r", "").Replace("\n", "").Replace("&nbsp;", "").TrimStart().TrimEnd();
                        news.PhoneNumber = titlePhone;
                        Console.WriteLine("Phone:" + titlePhone);
                    }
                }

                var subMobile = docSub.DocumentNode.SelectNodes("//div[@id='LeftMainContent__productDetail_contactMobile']//div[@class='right']");
                if (subMobile != null) {
                    foreach (var node in subMobile)
                    {
                        var titleMobile = node.InnerText.Replace("\r", "").Replace("\n", "").Replace("&nbsp;", "").TrimStart().TrimEnd();
                        news.PhoneNumber = titleMobile;
                        Console.WriteLine("Mobile:" + titleMobile);
                    }
                }

                var subAddress = docSub.DocumentNode.SelectNodes("//div[@class='left-detail']//div[@class='right']");
                if (subAddress != null) {
                    foreach (var node in subAddress) {
                        var titleAddress = node.InnerText.Replace("\r", "").Replace("\n", "").Replace("&nbsp;", "").TrimStart().TrimEnd();
                        news.Address = titleAddress;
                        Console.WriteLine("Address:" + titleAddress);
                        break;
                    }
                }

                Add(news);
                Console.WriteLine("=====================");
                Console.WriteLine("=====================");
                Thread.Sleep(1000);
            }
            return result;
        }

        public static void Add(PrivateNews model)
        {
            var connection = ConfigurationManager.ConnectionStrings["InfoWeb"].ConnectionString.ToString();
            var _conn = new SqlConnection(connection);
            if (_conn.State == ConnectionState.Closed)
                _conn.Open();
            //Create command store procedure
            var command = new SqlCommand("Add_tblPrivateNews");
            command.Connection = _conn;
            command.CommandType = CommandType.StoredProcedure;
            try
            {
                var TitleParam = new SqlParameter("@Title", model.Title);
                TitleParam.Direction = ParameterDirection.Input;
                var AddressParam = new SqlParameter("@Address", model.Address);
                AddressParam.Direction = ParameterDirection.Input;
                var DientichParam = new SqlParameter("@Dientich", model.Dientich);
                DientichParam.Direction = ParameterDirection.Input;
                var PriceParam = new SqlParameter("@Price", model.Price);
                PriceParam.Direction = ParameterDirection.Input;
                var PhoneNumberParam = new SqlParameter("@PhoneNumber", model.PhoneNumber);
                PhoneNumberParam.Direction = ParameterDirection.Input;
                var NewsContentParam = new SqlParameter("@NewsContent", model.NewsContent);
                NewsContentParam.Direction = ParameterDirection.Input;
                var StatusParam = new SqlParameter("@Status", model.Status);
                StatusParam.Direction = ParameterDirection.Input;

                command.Parameters.Add(TitleParam);
                command.Parameters.Add(AddressParam);
                command.Parameters.Add(DientichParam);
                command.Parameters.Add(PriceParam);
                command.Parameters.Add(PhoneNumberParam);
                command.Parameters.Add(NewsContentParam);
                command.Parameters.Add(StatusParam);
                command.ExecuteScalar();
            }
            catch (Exception ex) {
                Console.WriteLine("Error Message:" + ex.Message);
            }
            finally
            {
                command.Connection.Close();
                command.Connection.Dispose();
            }
            Thread.Sleep(500);
        }
    
    }


}
