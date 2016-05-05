using CefSharp;
using CefSharp.WinForms;
using GetNewsTools.Business;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Diagnostics;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace GetNewsTools
{
    public partial class frmGetNews : Form
    {
        const string DEFAULT_URL = "www.bdschinhchu.com.vn";
        const string BDS_COM_VN = "http://batdongsan.com.vn";
        const string RONG_BAY = "http://rongbay.com";
        const string VAT_GIA = "http://vatgia.com";
        const string EN_BAC = "http://enbac.com";
        const string RAO_VAT_VNEXPRESS = "http://raovat.vnexpress.net";
        private ArrayList arrUrls = new ArrayList();
        private ChromiumWebBrowser broswer = null;
        private string m_SourceUrl = string.Empty;
        private CefSettings m_Settings = new CefSettings();
        private List<string> lstLinks = new List<string>();

        public frmGetNews()
        {
            InitializeComponent();            
            broswer = new ChromiumWebBrowser(DEFAULT_URL);
            broswer.Dock = DockStyle.Fill;
            pnlBrowser.Controls.Add(broswer);          
            SetCboDataSource();            
        }

        private void GetContentPage()
        {
            try
            {
                //Get the underlying browser host wrapper
                var browserHost = broswer.GetBrowser().GetHost();
                var requestContext = browserHost.RequestContext;
                string errorMessage;
                var success = requestContext.SetPreference("enable_do_not_track", true, out errorMessage);
                if (!success)
                {
                    MessageBox.Show("Unable to set preference enable_do_not_track errorMessage: " + errorMessage);
                }
                //Example of disable spellchecking
                //success = requestContext.SetPreference("browser.enable_spellchecking", false, out errorMessage);

                var preferences = requestContext.GetAllPreferences(true);
                var doNotTrack = (bool)preferences["enable_do_not_track"];
                
            }
            catch (Exception)
            {

                throw;
            }
        }


        private void btnGoTo_Click(object sender, EventArgs e)
        {
            try
            {
                lstLinks.Clear();
                GetHtmlContent();
            }
            catch (Exception)
            {
                throw;
            }
        }
        private void GetHtmlContent()
        {
            var task = broswer.GetSourceAsync().ContinueWith(t =>
            {
                if (!t.IsFaulted)
                {
                    lstLinks.Add(t.Result.ToString());
                    MessageBox.Show(t.Result.ToString());
                }
            }, TaskScheduler.Current);
            if (!task.IsCompleted)
                task.Wait();

        }
        private void RegistJsToGetLinks(string html)
        {
            
            broswer.LoadHtml(html, "http://customrendering/");            
            StringBuilder sb = new StringBuilder();
            sb.AppendLine(" function getLinksTag() {");
            sb.AppendLine("     var links = $('.link_redirect');");
            sb.AppendLine("     $(links).each(function(){");
            sb.AppendLine("     return $(this).attr('href');");
            sb.AppendLine(" });");
            sb.AppendLine(" getLinksTag();");

            var task = broswer.EvaluateScriptAsync(sb.ToString());          

            task.ContinueWith(t =>
            {
                if (!t.IsFaulted)
                {
                    var response = t.Result;

                    if (response.Success == true)
                    {
                        lstLinks.Add(response.Result.ToString());
                        MessageBox.Show(response.Result.ToString());
                    }
                }
            }, TaskScheduler.FromCurrentSynchronizationContext());

        }

        private void SetCboDataSource()
        {
            try
            {                
                arrUrls.Add(BDS_COM_VN);
                arrUrls.Add(RONG_BAY);
                arrUrls.Add(VAT_GIA);
                arrUrls.Add(EN_BAC);
                arrUrls.Add(RAO_VAT_VNEXPRESS);
                cboUrls.Items.AddRange(arrUrls.ToArray());
            }
            catch (Exception)
            {
                throw;
            }
        }

        private void cboUrls_SelectedIndexChanged(object sender, EventArgs e)
        {
            try
            {
                if (cboUrls.SelectedIndex == -1) return;
                switch (cboUrls.SelectedIndex)
                {
                    case 0:
                        m_SourceUrl = "http://batdongsan.com.vn/nha-dat-ban-tp-hcm";
                        break;
                    case 1:
                        m_SourceUrl = "http://rongbay.com/TP-HCM/Mua-Ban-nha-dat-c15.html";
                        break;
                    case 2:
                        m_SourceUrl = "http://rongbay.com/TP-HCM/Mua-Ban-nha-dat-c15.html";
                        break;
                    case 3:
                        m_SourceUrl = "http://rongbay.com/TP-HCM/Mua-Ban-nha-dat-c15.html";
                        break;
                    case 4:
                        m_SourceUrl = "http://rongbay.com/TP-HCM/Mua-Ban-nha-dat-c15.html";
                        break;
                    default:
                        break;
                }
                broswer.Load(m_SourceUrl);
            }
            catch (Exception)
            {
                throw;
            }
        }

        //private void GetSettings()
        //{
        //    // Set Google API keys, used for Geolocation requests sans GPS.  See http://www.chromium.org/developers/how-tos/api-keys
        //    // Environment.SetEnvironmentVariable("GOOGLE_API_KEY", "");
        //    // Environment.SetEnvironmentVariable("GOOGLE_DEFAULT_CLIENT_ID", "");
        //    // Environment.SetEnvironmentVariable("GOOGLE_DEFAULT_CLIENT_SECRET", "");
        //    //Chromium Command Line args
        //    //http://peter.sh/experiments/chromium-command-line-switches/
        //    //NOTE: Not all relevant in relation to `CefSharp`, use for reference purposes only.
        //    var settings = new CefSettings();
        //    settings.RemoteDebuggingPort = 8088;
        //    //The location where cache data will be stored on disk. If empty an in-memory cache will be used for some features and a temporary disk cache for others.
        //    //HTML5 databases such as localStorage will only persist across sessions if a cache path is specified. 
        //    settings.CachePath = "cache";
        //    //settings.UserAgent = "Mozilla/5.0 (Windows NT 6.1; WOW64) AppleWebKit/537.36 (KHTML, like Gecko) Chrome/47.0.2526.106 Safari/537.36";
        //    settings.UserAgent = "CefSharp Browser" + Cef.CefSharpVersion; // Example User Agent
        //    //settings.UserAgent = "CefSharp Browser" + Cef.CefSharpVersion; // Example User Agent
        //    //settings.CefCommandLineArgs.Add("renderer-process-limit", "1");
        //    //settings.CefCommandLineArgs.Add("renderer-startup-dialog", "1");
        //    //settings.CefCommandLineArgs.Add("enable-media-stream", "1"); //Enable WebRTC
        //    //settings.CefCommandLineArgs.Add("no-proxy-server", "1"); //Don't use a proxy server, always make direct connections. Overrides any other proxy server flags that are passed.
        //    //settings.CefCommandLineArgs.Add("debug-plugin-loading", "1"); //Dumps extra logging about plugin loading to the log file.
        //    //settings.CefCommandLineArgs.Add("disable-plugins-discovery", "1"); //Disable discovering third-party plugins. Effectively loading only ones shipped with the browser plus third-party ones as specified by --extra-plugin-dir and --load-plugin switches
        //    //settings.CefCommandLineArgs.Add("enable-system-flash", "1"); //Automatically discovered and load a system-wide installation of Pepper Flash.
        //    settings.CefCommandLineArgs.Add("allow-running-insecure-content", "1"); //By default, an https page cannot run JavaScript, CSS or plugins from http URLs. This provides an override to get the old insecure behavior. Only available in 47 and above.

        //    //settings.CefCommandLineArgs.Add("enable-logging", "1"); //Enable Logging for the Renderer process (will open with a cmd prompt and output debug messages - use in conjunction with setting LogSeverity = LogSeverity.Verbose;)
        //    //settings.LogSeverity = LogSeverity.Verbose; // Needed for enable-logging to output messages
        //    //settings.CefCommandLineArgs.Add("disable-extensions", "1"); //Extension support can be disabled
        //    //settings.CefCommandLineArgs.Add("disable-pdf-extension", "1"); //The PDF extension specifically can be disabled
        //    #region DisableSettings

        //    //Load the pepper flash player that comes with Google Chrome - may be possible to load these values from the registry and query the dll for it's version info (Step 2 not strictly required it seems)
        //    //settings.CefCommandLineArgs.Add("ppapi-flash-path", @"C:\Program Files (x86)\Google\Chrome\Application\47.0.2526.106\PepperFlash\pepflashplayer.dll"); //Load a specific pepper flash version (Step 1 of 2)
        //    //settings.CefCommandLineArgs.Add("ppapi-flash-version", "20.0.0.228"); //Load a specific pepper flash version (Step 2 of 2)

        //    //NOTE: For OSR best performance you should run with GPU disabled:
        //    // `--disable-gpu --disable-gpu-compositing --enable-begin-frame-scheduling`
        //    // (you'll loose WebGL support but gain increased FPS and reduced CPU usage).
        //    // http://magpcss.org/ceforum/viewtopic.php?f=6&t=13271#p27075
        //    //https://bitbucket.org/chromiumembedded/cef/commits/e3c1d8632eb43c1c2793d71639f3f5695696a5e8

        //    //NOTE: The following function will set all three params
        //    //settings.SetOffScreenRenderingBestPerformanceArgs();
        //    //settings.CefCommandLineArgs.Add("disable-gpu", "1");
        //    //settings.CefCommandLineArgs.Add("disable-gpu-compositing", "1");
        //    //settings.Cefhttp://test/resource/loadCommandLineArgs.Add("enable-begin-frame-scheduling", "1");

        //    //settings.CefCommandLineArgs.Add("disable-gpu-vsync", "1"); //Disable Vsync

        //    //Disables the DirectWrite font rendering system on windows.
        //    //Possibly useful when experiencing blury fonts.
        //    //settings.CefCommandLineArgs.Add("disable-direct-write", "1");

        //    //settings.MultiThreadedMessageLoop = multiThreadedMessageLoop;

        //    //// Off Screen rendering (WPF/Offscreen)
        //    //if (osr)
        //    //{
        //    //    settings.WindowlessRenderingEnabled = true;
        //    //    // Disable Surfaces so internal PDF viewer works for OSR
        //    //    // https://bitbucket.org/chromiumembedded/cef/issues/1689
        //    //    //settings.CefCommandLineArgs.Add("disable-surfaces", "1");
        //    //    settings.EnableInternalPdfViewerOffScreen();

        //    //    // DevTools doesn't seem to be working when this is enabled
        //    //    // http://magpcss.org/ceforum/viewtopic.php?f=6&t=14095
        //    //    //settings.CefCommandLineArgs.Add("enable-begin-frame-scheduling", "1");

        //    //    // Disable GPU in WPF and Offscreen examples until #1634 has been resolved
        //    //    settings.CefCommandLineArgs.Add("disable-gpu", "1");
        //    //}

        //    //var proxy = ProxyConfig.GetProxyInformation();
        //    //switch (proxy.AccessType)
        //    //{
        //    //    case InternetOpenType.Direct:
        //    //        {
        //    //            //Don't use a proxy server, always make direct connections.
        //    //            settings.CefCommandLineArgs.Add("no-proxy-server", "1");
        //    //            break;
        //    //        }
        //    //    case InternetOpenType.Proxy:
        //    //        {
        //    //            settings.CefCommandLineArgs.Add("proxy-server", proxy.ProxyAddress);
        //    //            break;
        //    //        }
        //    //    case InternetOpenType.PreConfig:
        //    //        {
        //    //            settings.CefCommandLineArgs.Add("proxy-auto-detect", "1");
        //    //            break;
        //    //        }
        //    //}

        //    ////settings.LogSeverity = LogSeverity.Verbose;

        //    //if (DebuggingSubProcess)
        //    //{
        //    //    var architecture = Environment.Is64BitProcess ? "x64" : "x86";
        //    //    settings.BrowserSubprocessPath = "..\\..\\..\\..\\CefSharp.BrowserSubprocess\\bin\\" + architecture + "\\Debug\\CefSharp.BrowserSubprocess.exe";
        //    //}

        //    //settings.RegisterScheme(new CefCustomScheme
        //    //{
        //    //    SchemeName = CefSharpSchemeHandlerFactory.SchemeName,
        //    //    SchemeHandlerFactory = new CefSharpSchemeHandlerFactory()
        //    //    //SchemeHandlerFactory = new InMemorySchemeAndResourceHandlerFactory()
        //    //});

        //    //settings.RegisterScheme(new CefCustomScheme
        //    //{
        //    //    SchemeName = CefSharpSchemeHandlerFactory.SchemeNameTest,
        //    //    SchemeHandlerFactory = new CefSharpSchemeHandlerFactory()
        //    //});

        //    //settings.RegisterExtension(new CefExtension("cefsharp/example", Resources.extension));

        //    //settings.FocusedNodeChangedEnabled = true;

        //    ////The Request Context has been initialized, you can now set preferences, like proxy server settings
        //    Cef.OnContextInitialized = delegate
        //    {
        //        var cookieManager = Cef.GetGlobalCookieManager();
        //        cookieManager.SetStoragePath("cookies", true);
        //        cookieManager.SetSupportedSchemes("custom");

        //        //Dispose of context when finished - preferable not to keep a reference if possible.
        //        using (var context = Cef.GetGlobalRequestContext())
        //        {
        //            string errorMessage;
        //            //You can set most preferences using a `.` notation rather than having to create a complex set of dictionaries.
        //            //The default is true, you can change to false to disable
        //            context.SetPreference("webkit.webprefs.plugins_enabled", true, out errorMessage);
        //        }
        //    };

        //    if (!Cef.Initialize(settings, shutdownOnProcessExit: true, performDependencyCheck: false))
        //    {
        //        throw new Exception("Unable to Initialize Cef");
        //    }
        //    #endregion
        //}
    }
}
