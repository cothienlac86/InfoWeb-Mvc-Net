using HtmlAgilityPack;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Configuration;
using System.Data;
using System.Data.SqlClient;
using System.IO;
using System.Linq;
using System.Net;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading;
using System.Windows.Forms;

namespace GetInfoWeb
{
    public partial class MainForm : Form
    {
        private List<string> urlSource = null;
        private string m_CopyUrl = string.Empty;
        private List<PrivateNews> copyData = new List<PrivateNews>();

        public MainForm()
        {
            InitializeComponent();
            // Application logic
            InitUrlsSource();
            // Background Worker
            //copyWorkers.WorkerReportsProgress = true;
            //copyWorkers.WorkerSupportsCancellation = true;
            //copyWorkers.DoWork += new DoWorkEventHandler(copyWorkers_DoWork);
            //copyWorkers.ProgressChanged += new ProgressChangedEventHandler(copyWorkers_ProgressChanged);
            //copyWorkers.RunWorkerCompleted += new RunWorkerCompletedEventHandler(copyWorkers_RunWorkerCompleted);
        }

        private void InitUrlsSource()
        {
            if (urlSource == null)
            {
                //muaban.net, chotot.vn, batdongsan.com.vn, rongbay.com, vatgia.com, enbac.com, vnexpress.net
                urlSource = new List<string>();
                urlSource.Add("muaban.net");
                urlSource.Add("chotot.vn");
                urlSource.Add("batdongsan.com.vn");
                urlSource.Add("rongbay.com");
                urlSource.Add("vatgia.com");
                urlSource.Add("enbac.com");
                urlSource.Add("vnexpress.net");
            }
            comboBox1.DataSource = urlSource.ToArray();
            comboBox1.DisplayMember = "Value";
        }
        private void comboBox1_SelectedIndexChanged(object sender, EventArgs e)
        {
            switch (comboBox1.SelectedIndex)
            {
                case 0:
                    m_CopyUrl = "http://muaban.net/mua-ban-nha-dat-cho-thue-ho-chi-minh-l59-c3?cp={0}";
                    break;

                case 1:
                    m_CopyUrl = "https://www.chotot.com/tp-ho-chi-minh/mua-ban-bat-dong-san?o={0}#";
                    break;
                case 2:
                    m_CopyUrl = "http://batdongsan.com.vn/nha-dat-ban-tp-hcm/p{0}";
                    break;
                case 3:
                    m_CopyUrl = "http://rongbay.com/TP-HCM/Mua-Ban-nha-dat-c15-trang{0}.html";
                    break;
                case 4:
                    m_CopyUrl = "http://vatgia.com";
                    break;
                case 5:
                    m_CopyUrl = "http://enbac.com";
                    break;
                case 6:
                    m_CopyUrl = "http://vnexpress.net";
                    break;
                default:
                    m_CopyUrl = string.Empty;
                    break;
            }
        }

        private void ExecuteCopyData()
        {
            if (string.IsNullOrEmpty(txtCopyNo.Text)) return;
            if (string.IsNullOrEmpty(m_CopyUrl)) return;
            //Console.WriteLine(GetCountOfResultFromGoogle("0982982690"));
            Console.ForegroundColor = ConsoleColor.Green;
            Console.WriteLine(string.Format("== Lay thong tin trang: {0} ==", comboBox1.SelectedText));
            Console.ResetColor();
            Console.ForegroundColor = ConsoleColor.Yellow;
            Console.WriteLine("Bat dau kiem tra ket noi mang...");
            Thread.Sleep(1000);
            try
            {
                int page2Copy = int.Parse(txtCopyNo.Text);
                for (int i = 0; i < page2Copy; i++)
                {
                    var link = string.Format(m_CopyUrl, i + 1);                    
                    Console.WriteLine(string.Format("== Trang: {0} ==", link));
                    //GetBdsLinks(link, (i + 1));
                    //GetRongBayLinks(link, i + 1);
                    GetMuaBanLinks(link, i + 1);
                    Thread.Sleep(2000);
                }
            }
            catch (Exception ex)
            {
                Console.ResetColor();
                Console.ForegroundColor = ConsoleColor.Red;
                Console.WriteLine(string.Format("Error:{0}", ex.Message));
                //Console.WriteLine("Khong the ket noi voi internet. \nVui long kiem tra ket noi mang va cau hinh may tinh!\nNhan Enter de thoat...");
                Console.ReadLine();
            }
            finally
            {
                if (copyData.Count > 0)
                {
                    dataGridView1.DataSource = copyData.ToArray<PrivateNews>();
                    //dataGridView1.DataMember = "Value";
                    copyData.Clear();
                }
            }
        }

        /// <summary>
        /// Get count of result from Google
        /// </summary>
        /// <param name="keyword"></param>
        /// <returns></returns>
        private int GetCountOfResultFromGoogle(string keyword)
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
        private string GetContent(string url)
        {
            var requestUrl = (HttpWebRequest)WebRequest.Create(url);
            var domainValue = comboBox1.Text;
            //--Start add proxy - Remove lines code and config from App.config - If you don't need
            //IWebProxy proxy = new WebProxy("fsoft-proxy", 8080);
            //proxy.Credentials = new NetworkCredential("hault1", "Ginta@123");
            //request.Proxy = proxy;
            //--End add proxy
            var request = TryAddCookie(requestUrl, new List<Cookie> {
                new Cookie("__asc", "c99bb1d41538995d02618de55b8") { Domain = domainValue },
                new Cookie("_gat", "1") { Domain = domainValue },
                new Cookie("_ga", "GA1.3.1787916081.1458302207") { Domain = domainValue },
                new Cookie("__auc", "c99bb1d41538995d02618de55b8") { Domain = domainValue },
                new Cookie("psortfilter", "1%24all%24VOE%2FWO8MpO1adIX%2BwMGNUA%3D%3D") { Domain = domainValue }
            });
            request.Timeout = 15 * 1000;
            request.Method = "GET";
            request.UserAgent = "Mozilla/5.0 (Windows NT 6.1; WOW64) AppleWebKit/537.36 (KHTML, like Gecko) Chrome/50.0.2661.94 Safari/537.36";
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
        private void GetBdsLinks(string url, int number)
        {
            //var result = String.Empty;
            try
            {
                var resultHtml = GetContent(url);
                //using Html Agility Pack
                var doc = new HtmlAgilityPack.HtmlDocument();
                doc.LoadHtml(resultHtml);
                Console.ResetColor();
                Console.ForegroundColor = ConsoleColor.Cyan;
                Console.WriteLine("Bat dau doc du lieu tu trang " + number);
                Console.ResetColor();
                Console.ForegroundColor = ConsoleColor.Magenta;
                Thread.Sleep(1000);
                int countIdx = 1;
                foreach (HtmlNode link in doc.DocumentNode.SelectNodes("//div[@class='p-title']//a"))
                {
                    var news = new PrivateNews();
                    Console.WriteLine("Tieu de: " + link.InnerText.Replace("\r", "").Replace("\n", "").Replace("&nbsp;", "").TrimStart().TrimEnd());
                    news.Title = link.InnerText.Replace("\r", "").Replace("\n", "").Replace("&nbsp;", "").TrimStart().TrimEnd();
                    Console.WriteLine("Link:" + link.Attributes["href"].Value.Replace("\r", "").Replace("\n", "").Replace("&nbsp;", "").TrimStart().TrimEnd());
                    var subUrl = "http://batdongsan.com.vn" + link.Attributes["href"].Value;
                    var subResultHtml = GetContent(subUrl);
                    var docSub = new HtmlAgilityPack.HtmlDocument();
                    docSub.LoadHtml(subResultHtml);
                    var subNodes = docSub.DocumentNode.SelectNodes("//span[@class='gia-title mar-right-15']//strong");
                    if (subNodes != null)
                    {
                        foreach (var node in subNodes)
                        {
                            var titleGia = node.InnerText.Replace("\r", "").Replace("\n", "").Replace("&nbsp;", "").TrimStart().TrimEnd();
                            news.Price = titleGia;
                            Console.WriteLine("Gia:" + titleGia);
                        }
                    }
                    var subContent = docSub.DocumentNode.SelectNodes("//div[@class='pm-content stat']");
                    if (subContent != null)
                    {
                        foreach (var node in subContent)
                        {
                            var titleContent = node.InnerText;
                            news.NewsContent = titleContent;
                            Console.WriteLine("Noidung:" + titleContent);
                        }
                    }

                    var subDientich = docSub.DocumentNode.SelectNodes("//span[@class='gia-title']//strong");
                    if (subDientich != null)
                    {
                        foreach (var node in subDientich)
                        {
                            var titleDientich = node.InnerText.Replace("\r", "").Replace("\n", "").Replace("&nbsp;", "").TrimStart().TrimEnd();
                            news.Dientich = titleDientich;
                            Console.WriteLine("Dientich:" + titleDientich);
                        }
                    }
                    

                    var subPhone = docSub.DocumentNode.SelectNodes("//div[@id='LeftMainContent__productDetail_contactPhone']//div[@class='right']");
                    if (subPhone != null)
                    {
                        if (subPhone != null)
                        {
                            foreach (var node in subPhone)
                            {
                                var titlePhone = node.InnerText.Replace("\r", "").Replace("\n", "").Replace("&nbsp;", "").TrimStart().TrimEnd();
                                news.PhoneNumber = titlePhone;
                                Console.WriteLine("Phone:" + titlePhone);
                            }
                        }
                    }
                    

                    var subMobile = docSub.DocumentNode.SelectNodes("//div[@id='LeftMainContent__productDetail_contactMobile']//div[@class='right']");
                    if (subMobile != null)
                    {
                        if (subMobile != null)
                        {
                            foreach (var node in subMobile)
                            {
                                var titleMobile = node.InnerText.Replace("\r", "").Replace("\n", "").Replace("&nbsp;", "").TrimStart().TrimEnd();
                                news.PhoneNumber = titleMobile;
                                Console.WriteLine("Mobile:" + titleMobile);
                            }
                        }
                    }


                    var subAddress = docSub.DocumentNode.SelectNodes("//div[@class='left-detail']//div[@class='right']");                   
                    if (subAddress != null)
                    {
                        foreach (var node in subAddress)
                        {
                            var titleAddress = node.InnerText.Replace("\r", "").Replace("\n", "").Replace("&nbsp;", "").TrimStart().TrimEnd();
                            news.Address = titleAddress;
                            Console.WriteLine("Address:" + titleAddress);
                            break;
                        }
                    }
                    news.Id = countIdx;
                    news.Datetime = DateTime.Today;
                    //Add(news);
                    countIdx++;
                    Console.WriteLine("=====================");
                    Console.WriteLine("=====================");
                    Thread.Sleep(3000);
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine("=====================");
                Console.WriteLine("Error:{0}", ex.Message);
                Console.WriteLine("=====================");

            }
            finally
            {

            }
            //return result;
        }

        /// <summary>
        ///
        /// </summary>
        /// <param name="url"></param>
        /// <returns></returns>
        private void GetRongBayLinks(string url, int number)
        {
            //var result = String.Empty;
            try
            {
                var resultHtml = GetContent(url);
                //using Html Agility Pack
                var doc = new HtmlAgilityPack.HtmlDocument();
                doc.LoadHtml(resultHtml);
                Console.ResetColor();
                Console.ForegroundColor = ConsoleColor.Cyan;
                Console.WriteLine("Bat dau doc du lieu tu trang " + number);
                Console.ResetColor();
                Console.ForegroundColor = ConsoleColor.Magenta;
                Thread.Sleep(3000);
                int countNews = 1;
                foreach (HtmlNode link in doc.DocumentNode.SelectNodes("//a[@class='link_direct']"))
                {
                    var news = new PrivateNews();
                    Console.WriteLine("Tieu de: " + link.InnerText.Replace("\r", "").Replace("\n", "").Replace("&nbsp;", "").TrimStart().TrimEnd());
                    news.Title = link.InnerText.Replace("\r", "").Replace("\n", "").Replace("&nbsp;", "").TrimStart().TrimEnd();
                    Console.WriteLine("Link:" + link.Attributes["href"].Value.Replace("\r", "").Replace("\n", "").Replace("&nbsp;", "").TrimStart().TrimEnd());
                    //var subUrl = "http://batdongsan.com.vn" + link.Attributes["href"].Value;
                    var subUrl = link.Attributes["href"].Value;
                    var subResultHtml = GetContent(subUrl);
                    var docSub = new HtmlAgilityPack.HtmlDocument();
                    docSub.LoadHtml(subResultHtml);
                    var titleNodes = docSub.DocumentNode.SelectNodes("//div[@class='detail_title color333 font_26']//h1");
                    if (titleNodes != null)
                    {
                        foreach (var node in titleNodes)
                        {
                            var title = node.InnerText.Replace("\r", "").Replace("\n", "").Replace("&nbsp;", "").TrimStart().TrimEnd();
                            news.Title = title;
                            Console.WriteLine("title:" + title);
                        }
                    }
                    
                    // Address
                    var addNodes = docSub.DocumentNode.SelectNodes("//p[@class='color444']//a[@class='color444']");
                    if (addNodes != null)
                    {
                        foreach (var node in addNodes)
                        {
                            var address = node.InnerText.Replace("\r", "").Replace("\n", "").Replace("&nbsp;", "").TrimStart().TrimEnd();
                            news.Address = address;
                            Console.WriteLine("Address:" + address);
                        }
                    }
                    
                    // Dien tich
                    //------------ Holder here---------------
                    // Price
                    //------------ Holder here---------------
                    // Phone
                    var phoneNodes = docSub.DocumentNode.SelectNodes("//div[@class='cl_333 user_phone font_14 font_700']//p");
                    if (phoneNodes != null)
                    {
                        foreach (var node in phoneNodes)
                        {
                            var phone = node.InnerText.Trim();
                            phone = phone.Replace("&#x3", "").Replace(";", "");
                            news.PhoneNumber = phone;
                            Console.WriteLine("PhoneNumber:" + phone);
                        }
                    }
                    
                    // Content
                    var contentNodes = docSub.DocumentNode.SelectNodes("//div[@class='content_input_editior']");
                    string contents = string.Empty;
                    if (contentNodes != null)
                    {
                        foreach (var node in contentNodes)
                        {
                            if (node.HasChildNodes)
                            {
                                var childNodes = node.ChildNodes.ToList<HtmlNode>();
                                childNodes.ForEach(t =>
                                {
                                    if (t.HasChildNodes)
                                    {
                                        //if (t.HasAttributes)
                                        //{
                                        //    List<HtmlAttribute> attrs = t.ChildAttributes("image").ToList<HtmlAttribute>();
                                        //    if (attrs != null)
                                        //    {
                                        //        foreach (var att in attrs)
                                        //        {

                                        //        }
                                        //    }
                                        //}

                                        contents += t.InnerText;
                                    }
                                    else
                                    {
                                        contents += t.InnerHtml;
                                    }
                                });
                            }
                            else
                            {
                                contents += node.OuterHtml; ;
                            }
                        }
                        if (!string.IsNullOrEmpty(contents))
                        {
                            news.NewsContent = contents;
                            //contents = string.Join("", contents.Split(';'));
                            Console.WriteLine("NewsContent:" + contents.Replace("\r", "").Replace("\n", "").Replace("&nbsp;", "").TrimStart().TrimEnd());
                        }
                    }                    
                    // Status
                    news.Status = 2;
                    news.Id = countNews;
                    countNews++;
                    // Set Datetiem Value
                    news.Datetime = DateTime.Today;
                    //Add(news);
                    Console.WriteLine("=====================");
                    Console.WriteLine("=====================");
                    Thread.Sleep(3000);
                    copyData.Add(news);
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine("=====================");
                Console.WriteLine("    Error:{0}", ex.Message);
                Console.WriteLine("=====================");
            }
            //return result;
        }

        private void GetRaoVatLinks(string url, int number)
        {           
            try
            {
                var resultHtml = GetContent(url);
                //using Html Agility Pack
                var doc = new HtmlAgilityPack.HtmlDocument();
                doc.LoadHtml(resultHtml);
                Console.ResetColor();
                Console.ForegroundColor = ConsoleColor.Cyan;
                Console.WriteLine("Bat dau doc du lieu tu trang " + number);
                Console.ResetColor();
                Console.ForegroundColor = ConsoleColor.Magenta;
                Thread.Sleep(3000);
                int countNews = 1;
                foreach (HtmlNode link in doc.DocumentNode.SelectNodes("//a[@class='link_direct']"))
                {
                    var news = new PrivateNews();
                    Console.WriteLine("Tieu de: " + link.InnerText.Replace("\r", "").Replace("\n", "").Replace("&nbsp;", "").TrimStart().TrimEnd());
                    news.Title = link.InnerText.Replace("\r", "").Replace("\n", "").Replace("&nbsp;", "").TrimStart().TrimEnd();
                    Console.WriteLine("Link:" + link.Attributes["href"].Value.Replace("\r", "").Replace("\n", "").Replace("&nbsp;", "").TrimStart().TrimEnd());
                    //var subUrl = "http://batdongsan.com.vn" + link.Attributes["href"].Value;
                    var subUrl = link.Attributes["href"].Value;
                    var subResultHtml = GetContent(subUrl);
                    var docSub = new HtmlAgilityPack.HtmlDocument();
                    docSub.LoadHtml(subResultHtml);
                    var titleNodes = docSub.DocumentNode.SelectNodes("//div[@class='detail_title color333 font_26']//h1");
                    foreach (var node in titleNodes)
                    {
                        var title = node.InnerText.Replace("\r", "").Replace("\n", "").Replace("&nbsp;", "").TrimStart().TrimEnd();
                        news.Title = title;
                        Console.WriteLine("title:" + title);
                    }
                    // Address
                    var addNodes = docSub.DocumentNode.SelectNodes("//p[@class='color444']//a[@class='color444']");
                    foreach (var node in addNodes)
                    {
                        var address = node.InnerText.Replace("\r", "").Replace("\n", "").Replace("&nbsp;", "").TrimStart().TrimEnd();
                        news.Address = address;
                        Console.WriteLine("Address:" + address);
                    }
                    // Dien tich
                    //------------ Holder here---------------
                    // Price
                    //------------ Holder here---------------
                    // Phone
                    var phoneNodes = docSub.DocumentNode.SelectNodes("//div[@class='cl_333 user_phone font_14 font_700']//p");
                    foreach (var node in phoneNodes)
                    {
                        var phone = node.InnerText.Trim();
                        phone = phone.Replace("&#x3", "").Replace(";", "");
                        news.PhoneNumber = phone;
                        Console.WriteLine("PhoneNumber:" + phone);
                    }
                    // Content
                    var contentNodes = docSub.DocumentNode.SelectNodes("//div[@class='content_input_editior']");
                    string contents = string.Empty;
                    foreach (var node in contentNodes)
                    {
                        if (node.HasChildNodes)
                        {
                            var childNodes = node.ChildNodes.ToList<HtmlNode>();
                            childNodes.ForEach(t =>
                            {
                                if (t.HasChildNodes)
                                {
                                    //if (t.HasAttributes)
                                    //{
                                    //    List<HtmlAttribute> attrs = t.ChildAttributes("image").ToList<HtmlAttribute>();
                                    //    if (attrs != null)
                                    //    {
                                    //        foreach (var att in attrs)
                                    //        {

                                    //        }
                                    //    }
                                    //}

                                    contents += t.InnerText;
                                }
                                else
                                {
                                    contents += t.InnerHtml;
                                }
                            });
                        }
                        else
                        {
                            contents += node.OuterHtml; ;
                        }
                    }
                    if (!string.IsNullOrEmpty(contents))
                    {
                        news.NewsContent = contents;
                        //contents = string.Join("", contents.Split(';'));
                        Console.WriteLine("NewsContent:" + ConverHexToUnicode(contents));
                    }
                    news.Id = countNews;
                    // Status
                    news.Status = 2;
                    // Set Datetiem Value
                    news.Datetime = DateTime.Today;
                    countNews++;
                    //Add(news);
                    Console.WriteLine("=====================");
                    Console.WriteLine("=====================");
                    Thread.Sleep(3000);
                    copyData.Add(news);
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine("=====================");
                Console.WriteLine("    Error:{0}", ex.Message);
                Console.WriteLine("=====================");
            }            
        }

        private void GetMuaBanLinks(string url, int number)
        {
            try
            {
                var resultHtml = GetContent(url);
                //using Html Agility Pack
                var doc = new HtmlAgilityPack.HtmlDocument();
                doc.LoadHtml(resultHtml);
                Console.ResetColor();
                Console.ForegroundColor = ConsoleColor.Cyan;
                Console.WriteLine("Bat dau doc du lieu tu trang " + number);
                Console.ResetColor();
                Console.ForegroundColor = ConsoleColor.Magenta;
                Thread.Sleep(3000);
                int countNews = 1;
                foreach (HtmlNode link in doc.DocumentNode.SelectNodes("//div[@class='mbn-content ']//a[@class='mbn-title']"))
                {
                    var news = new PrivateNews();
                    //Console.WriteLine("Tieu de: " + link.InnerText.Replace("\r", "").Replace("\n", "").Replace("&nbsp;", "").TrimStart().TrimEnd());
                    //news.Title = link.InnerText.Replace("\r", "").Replace("\n", "").Replace("&nbsp;", "").TrimStart().TrimEnd();
                    Console.WriteLine("Link:" + link.Attributes["href"].Value.Replace("\r", "").Replace("\n", "").Replace("&nbsp;", "").TrimStart().TrimEnd());
                    //var subUrl = "http://batdongsan.com.vn" + link.Attributes["href"].Value;
                    var subUrl = link.Attributes["href"].Value;
                    var subResultHtml = GetContent(subUrl);
                    var docSub = new HtmlAgilityPack.HtmlDocument();
                    docSub.LoadHtml(subResultHtml);
                    var titleNodes = docSub.DocumentNode.SelectNodes("//div[@class='cl-title clearfix']//h1");
                    if (titleNodes != null)
                    {
                        foreach (var node in titleNodes)
                        {
                            var title = node.InnerText.Replace("\r", "").Replace("\n", "").Replace("&nbsp;", "").TrimStart().TrimEnd();
                            news.Title = title;
                            Console.WriteLine("title:" + title);
                        }
                    }
                    
                    // Address
                    var addNodes = docSub.DocumentNode.SelectNodes("//div[@class='cl-price-sm clearfix']//span");
                    int nodeIdx = 1;
                    if (addNodes != null)
                    {
                        foreach (var node in addNodes)
                        {
                            if (nodeIdx == 2)
                            {
                                var address = node.InnerText.Replace("\r", "").Replace("\n", "").Replace("&nbsp;", "").TrimStart().TrimEnd();
                                news.Address = address;
                                Console.WriteLine("Address:" + address);
                            }
                            if (nodeIdx == 1)
                            {
                                var price = node.InnerText.Replace("\r", "").Replace("\n", "").Replace("&nbsp;", "").TrimStart().TrimEnd();
                                news.Price = price;
                                Console.WriteLine("price:" + price);
                            }
                            nodeIdx++;
                        }
                        nodeIdx = 1;
                    }
                    
                    // Dien tich
                    var dtichNodes = docSub.DocumentNode.SelectNodes("//*[@id='dvContent']/div/ul[1]/li[2]/div/div[2]");
                    if (dtichNodes != null)
                    {
                        foreach (var node in dtichNodes)
                        {
                            var dientich = node.InnerText.Replace("\r", "").Replace("\n", "").Replace("&nbsp;", "").TrimStart().TrimEnd();

                            news.Dientich = dientich;
                            Console.WriteLine("Dien tich:" + dientich);
                        }    
                    }
                                   
                    //// Phone
                    // Place holder to using Cefsharp
                    //var phoneNodes = docSub.DocumentNode.SelectNodes("//div[@class='cl_333 user_phone font_14 font_700']//p");
                    //foreach (var node in phoneNodes)
                    //{
                    //    var phone = node.InnerText.Trim();
                    //    phone = phone.Replace("&#x3", "").Replace(";", "");
                    //    news.PhoneNumber = phone;
                    //    Console.WriteLine("PhoneNumber:" + phone);
                    //}
                    // Content
                    var contentNodes = docSub.DocumentNode.SelectNodes("//*[@id='dvContent']/div[@class='ct-body overflow clearfix']");
                    //string contents = string.Empty;
                    if (contentNodes != null)
                    {
                        foreach (var node in contentNodes)
                        {
                            var content = node.InnerText.Replace("\r", "").Replace("\n", "").Replace("&nbsp;", "").TrimStart().TrimEnd();

                            news.NewsContent = content;
                            Console.WriteLine("Content:" + content);
                        }    
                    }                                   
                    news.Id = countNews;
                    // Status
                    news.Status = 2;
                    // Set Datetiem Value
                    news.Datetime = DateTime.Today;
                    countNews++;
                    //Add(news);
                    Console.WriteLine("=====================");
                    Console.WriteLine("=====================");
                    Thread.Sleep(3000);
                    copyData.Add(news);
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine("=====================");
                Console.WriteLine("    Error:{0}", ex.Message);
                Console.WriteLine("=====================");
            }
        }

        private StringBuilder recursionNode = new StringBuilder();

        private void RecursionNode(HtmlNode node)
        {
            if (node.HasChildNodes)
            {
                recursionNode.AppendLine(node.FirstChild.InnerText);
                RecursionNode(node.Element(node.FirstChild.Name));
            }
        }

        private string ConverHexToUnicode(string hexValue)
        {
            return hexValue.Replace("\r", "").Replace("\n", "").Replace("&nbsp;", "").TrimStart().TrimEnd();
        }

        private string HexStringToString(string hexString)
        {
            return string.Join("", Regex.Split(hexString, "(?<=\\G..)(?!$)").Select(x => (char)Convert.ToByte(x, 16)));
        }

        private byte[] HexToBytes(string hexValue)
        {
            if (hexValue == null)
                throw new ArgumentNullException("hexString");
            if (hexValue.Length % 2 != 0)
                throw new ArgumentException("hexString must have an even length", "hexString");
            var bytes = new byte[hexValue.Length / 25];
            for (int i = 0; i < bytes.Length; i += 2)
            {
                var index = i * 2;
                string currentHex = hexValue.Substring(index, 5);
                bytes[i / 2] = Convert.ToByte(currentHex);
                index++;
            }
            return bytes;
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
            catch (Exception ex)
            {
                Console.WriteLine("Error Message:" + ex.Message);
            }
            finally
            {
                command.Connection.Close();
                command.Connection.Dispose();
            }
            Thread.Sleep(500);
        }

        

        private void button1_Click(object sender, EventArgs e)
        {
            ExecuteCopyData();
        }
    }
}