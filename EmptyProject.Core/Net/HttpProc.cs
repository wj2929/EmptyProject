using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Net;
using System.IO;
using System.Net.Cache;

namespace EmptyProject.Core.Net
{
    public enum HttpMethod
    {
        GET,
        POST
    }

    public class HttpProc
    {
        //Settings settings = new Settings();
        //GlobalConfigInfo globalConfigInfo = GlobalConfig.GetGlobalConfigInfo();

        // Fields
        private CookieCollection _cookieGet;
        private CookieCollection _cookiePost;
        private Encoding _encoding;
        private IWebProxy _Proxy;
        private string _ResHtml;
        private string _strCode;
        private string _strErr;
        private string _strPostdata;
        private string _strRefUrl;
        private string _strUrl;
        private bool _succeed;
        private string _redirectUrl;

        public bool UseWap = false;
        public HttpMethod httpMethod = HttpMethod.GET;
        public bool NoCacheCookie = false;
        private bool allowAutoRedirect = true;
        //const int Max_Retry_Num = 10;

        //private static ConfigData configData = null;//ConfigHelper.ReadGlobalConfigData(string.Empty);
        // Methods

                //    return CreateRequest(timeout, ProxyEnabled,
                //ProxyServer,
                //ProxyPort,
                //ProxyUser,
                //ProxyPass);



        public HttpProc()
            : this(null, null, null)
        {

        }

        public HttpProc(string 地址)
            : this(地址, null, null)
        {

        }

        public HttpProc(string 地址, CookieCollection 要发送的cookie)
            : this(地址, null, 要发送的cookie)
        {

        }

        public HttpProc(string 地址, string 发送数据)
            : this(地址, 发送数据, null)
        {

        }

        public HttpProc(string 地址, string 发送数据, CookieCollection 要发送的cookie)
        {
            //if (configData == null)
            //{
            //    if(Common.ExistConfigFile())
            //        configData = ConfigHelper.ReadGlobalConfigData(Common.GetConfigFile());
            //}
            this._encoding = Encoding.UTF8;
            this._strUrl = 地址;
            this._strPostdata = 发送数据;
            this._cookiePost = 要发送的cookie;
            _RetryNum = 1;
            _TimeOut = 10;
        }

        public void SetEncoding(Encoding encoding)
        {
            this._encoding = encoding;
        }

        public HttpWebRequest CreateRequest(int timeout, bool proxyEnabled, string proxyServer, int proxyPort, string proxyUser, string proxyPass)
        {
            HttpWebRequest 请求 = null;
            请求 = (HttpWebRequest)WebRequest.Create(this._strUrl);
            请求.CachePolicy = new RequestCachePolicy(RequestCacheLevel.NoCacheNoStore);
            请求.AllowAutoRedirect = allowAutoRedirect;
            请求.Accept = "*/*";
            请求.UserAgent = "Mozilla/4.0 (compatible; MSIE 6.0; Windows NT 5.2; .NET CLR 1.1.4322)";
            //请求.UserAgent = "Mozilla/4.0 (compatible; MSIE 7.0; Windows NT 5.1; .NET CLR 2.0.50727; .NET CLR 3.0.4506.2152; .NET CLR 3.5.30729)";
            if (UseWap)
            {
                请求.Accept = "text/html,application/xhtml+xml,application/xml;q=0.9,*/*;q=0.8";
                请求.UserAgent = "Mozilla/5.0 (Windows; U; Windows NT 5.2; zh-CN; rv:1.9.1.5) Gecko/20091102 Firefox/3.5.5 GTB6";
            }
            请求.CookieContainer = new CookieContainer();
            请求.Referer = this.Referer;
            请求.Timeout = timeout;// 1000 * 60;       //超时时间
            WebProxy myProxy = new WebProxy();
            if (proxyEnabled)
            {
                if (!string.IsNullOrEmpty(proxyServer) && proxyPort != 0)
                {
                    myProxy = new WebProxy(proxyServer, proxyPort);

                    if (!string.IsNullOrEmpty(proxyUser) && !string.IsNullOrEmpty(proxyPass))
                        myProxy.Credentials = new NetworkCredential(proxyUser, proxyPass);

                    请求.Proxy = myProxy;
                }
            }
            if (this._cookiePost != null)
            {
                Uri u = new Uri(this._strUrl);
                foreach (Cookie c in this._cookiePost)
                {
                    c.Domain = u.Host;
                }
                请求.CookieContainer.Add(this._cookiePost);
            }
            if (!string.IsNullOrEmpty(this._strPostdata) || httpMethod == HttpMethod.POST)
            {
                请求.ContentType = "application/x-www-form-urlencoded";
                请求.Method = "POST";
                if (!string.IsNullOrEmpty(this._strPostdata))
                {
                    byte[] b = this._encoding.GetBytes(this._strPostdata);
                    请求.ContentLength = b.Length;
                    Stream sw = null;
                    try
                    {
                        sw = 请求.GetRequestStream();
                        sw.Write(b, 0, b.Length);
                    }
                    catch (Exception ex)
                    {
                        this._strErr = ex.Message;
                    }
                    finally
                    {
                        if (sw != null)
                        {
                            try
                            {
                                sw.Close();
                            }
                            catch { }
                            finally { }
                        }
                    }
                }
            }
            return 请求;
        }

        public HttpWebRequest CreateRequest(int timeout)
        {
            return CreateRequest(timeout, ProxyEnabled,
                ProxyServer,
                ProxyPort,
                ProxyUser,
                ProxyPass);
        }

        public HttpWebRequest CreateRequest()
        {
            return CreateRequest(TimeOut * 1000);
        }

        public bool VerifyCookieAvailable()
        {
            HttpWebRequest 请求 = this.CreateRequest(500);
            HttpWebResponse 响应 = null;
            StreamReader sr = null;
            try
            {
                响应 = (HttpWebResponse)请求.GetResponse();
                sr = new StreamReader(响应.GetResponseStream(), this.Encoding);
                this._ResHtml = sr.ReadToEnd();
            }
            catch (Exception ex)
            {
                this._strErr = ex.Message;
                return false;
            }
            finally
            {
                if (sr != null)
                {
                    sr.Close();
                }
            }
            this._strCode = 响应.StatusCode.ToString();
            if (this._strCode == "302")
            {
                this._ResHtml = 响应.Headers["location"];
            }
            if (响应.Cookies.Count > 0)
            {
                this._cookieGet = 响应.Cookies;

                //CookieCollection cc = 请求.CookieContainer.GetCookies(请求.RequestUri);
                //if (cc["_invisible"] != null)
                //    this._cookieGet.Add(cc["_invisible"]);
                //if (cc["_email"] != null)
                //    this._cookieGet.Add(cc["_email"]);
                //if (cc["_uid"] != null)
                //    this._cookieGet.Add(cc["_uid"]);
                //if (cc["_kx"] != null)
                //    this._cookieGet.Add(cc["_kx"]);

            }
            return true;
        }

        public string Proc(int timeout, int custom_max_retry_num, bool proxyEnabled, string proxyServer, int proxyPort)
        {
            return Proc(timeout, custom_max_retry_num, proxyEnabled, proxyServer, proxyPort, string.Empty, string.Empty);
        }

        public string Proc(int timeout, int custom_max_retry_num, bool proxyEnabled, string proxyServer, int proxyPort, string proxyUser, string proxyPass)
        {
            int retrynum = 0;
        retry: HttpWebRequest 请求 = this.CreateRequest(timeout, proxyEnabled, proxyServer, proxyPort, proxyUser, proxyPass);
            HttpWebResponse 响应 = null;
            StreamReader sr = null;
            try
            {
                响应 = (HttpWebResponse)请求.GetResponse();
                sr = new StreamReader(响应.GetResponseStream(), this.Encoding);
                this._ResHtml = sr.ReadToEnd();
                _redirectUrl = 请求.Address.ToString();
            }
            catch (Exception ex)
            {
                this._strErr = ex.Message;
                if (this._strErr.Contains("正在中止线程"))
                {
                    return "";
                }
                if (retrynum++ < custom_max_retry_num)
                {
                    goto retry;
                }
                return "";
            }
            finally
            {
                if (sr != null)
                {
                    sr.Close();
                }
            }
            this._strCode = ((int)响应.StatusCode).ToString();
            if (this._strCode == "302")
            {
                string location = 响应.Headers["location"];
                this._strUrl = location;
            }
            if (响应.Cookies.Count > 0 || 请求.CookieContainer.Count > 0)
            {
                CookieCollection cc = 请求.CookieContainer.GetCookies(请求.RequestUri);

                if (cc.Count > 0)
                {
                    this._cookiePost = cc;
                    this._cookieGet = cc;
                }
            }
            if (this.ResHtml == string.Empty)
            {
                if (++retrynum < custom_max_retry_num)
                {
                    goto retry;
                }
            }
            return this.ResHtml;
        }

        public string Proc(int custom_max_retry_num)
        {
            return Proc(TimeOut * 1000, custom_max_retry_num, ProxyEnabled,
                ProxyServer,
                ProxyPort,
                ProxyUser,
                ProxyPass);
        }

        public string Proc(string proxyServer, int proxyPort, string proxyUser, string proxyPass)
        {
            return Proc(TimeOut * 1000, RetryNum, true,
                proxyServer,
                proxyPort,
                proxyUser,
                proxyPass);
        }

        public string Proc()
        {
            return Proc(RetryNum);
        }

        // Properties
        public CookieCollection cookieGet
        {
            get
            {
                return this._cookieGet;
            }
        }

        public CookieCollection cookiePost
        {
            get
            {
                return this._cookiePost;
            }
            set
            {
                this._cookiePost = value;
            }
        }

        public Encoding Encoding
        {
            get
            {
                return this._encoding;
            }
            set
            {
                this._encoding = value;
            }
        }

        public IWebProxy Proxy
        {
            get
            {
                return this._Proxy;
            }
            set
            {
                this._Proxy = value;
            }
        }

        public string ResHtml
        {
            get
            {
                return this._ResHtml;
            }
        }

        public string strCode
        {
            get
            {
                return this._strCode;
            }
            set
            {
                this._strCode = value;
            }
        }

        public string strErr
        {
            get
            {
                return this._strErr;
            }
            set
            {
                this._strErr = value;
            }
        }

        public string strPostdata
        {
            get
            {
                return this._strPostdata;
            }
            set
            {
                this._strPostdata = value;
            }
        }

        public string Referer
        {
            get
            {
                return this._strRefUrl;
            }
            set
            {
                this._strRefUrl = value;
            }
        }

        public string RedirectUrl
        {
            get { return this._redirectUrl; }
        }

        public string strUrl
        {
            get
            {
                return this._strUrl;
            }
            set
            {
                this._strUrl = value;
            }
        }

        public bool succeed
        {
            get
            {
                return this._succeed;
            }
            set
            {
                this._succeed = value;
            }
        }

        /// <summary>
        /// 启用代理
        /// </summary>
        public bool ProxyEnabled { get; set; }
        /// <summary>
        /// 代理服务器地址
        /// </summary>
        public string ProxyServer { get; set; }
        /// <summary>
        /// 代理服务器端口
        /// </summary>
        public int ProxyPort { get; set; }
        /// <summary>
        /// 代理用户名
        /// </summary>
        public string ProxyUser { get; set; }
        /// <summary>
        /// 代理用户密码
        /// </summary>
        public string ProxyPass { get; set; }
        private int _TimeOut;
        /// <summary>
        /// 超时时间（秒）
        /// </summary>
        public int TimeOut
        {
            get
            {
                return _TimeOut;
            }
            set
            {
                _TimeOut = value;
            }
        }

        private int _RetryNum;
        /// <summary>
        /// 重试次数
        /// </summary>
        public int RetryNum
        {
            get
            {
                return _RetryNum;
            }
            set
            {
                _RetryNum = value;
            }
        }
    }

}
