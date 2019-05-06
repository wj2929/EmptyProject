using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Net;
using System.IO;
using EmptyProject.Core.Validation;

namespace EmptyProject.Core.Net
{
    public class NetHelper : INetHelper
    {
        public NetHelper()
        {
            this.Type = "POST";
            this.Encoder = Encoding.GetEncoding("UTF-8"); 
        }

        /// <summary>
        /// 编码
        /// </summary>
        public Encoding Encoder { get; set; } 

        /// <summary>
        /// 传送数据
        /// </summary>
        private string PostData { get; set; }

        /// <summary>
        /// 已经传输的数据
        /// </summary>
        private long DataSent { get; set; }

        #region INetHelper
        private string _Url = "";
        /// <summary>
        /// 访问地址
        /// </summary>
        public string Url
        {
            get
            {
                if (this._Url.IsEmpty())
                    return "";

                return this.Type == "POST" ? this._Url : String.Format("{0}?{1}", this._Url, this.Parameters);
            }
            set
            {
                _Url = value;
            }
        }

        private string _Type;
        /// <summary>
        /// 类型
        /// POST,GET
        /// </summary>
        public string Type
        {
            get
            {
                if (this._Type.IsEmpty())
                    this.Type = "POST";

                return _Type;
            }
            set
            {
                _Type = value.ToUpper();
            }
        }

        private IHttpParameterCollection _Parameters = null;
        /// <summary>
        /// 参数
        /// </summary>
        public IHttpParameterCollection Parameters
        {
            get
            {
                if (this._Parameters == null)
                    this._Parameters = new HttpParameterCollection();

                return _Parameters;
            }
            set
            {
                this._Parameters = value;
            }
        }

        /// <summary>
        /// 发送数据
        /// </summary>
        /// <param name="TransferStream">向远程服务器传送文件流（默认不传送）</param>
        /// <returns></returns>
        public string Send()
        {
            this.PostData = this.Parameters.ToString();

            WebRequest request = WebRequest.Create(this.Url);
            request.ContentType = "application/x-www-form-urlencoded";
            request.Method = this.Type;
            request.ContentLength = this.PostData.Length;

            string CallBackData;

            using (Stream requestStream = request.GetRequestStream())
            {
                using (StreamWriter writer = new StreamWriter(requestStream))
                {
                    writer.Write(this.PostData);
                    writer.Flush();

                    using (HttpWebResponse response = (HttpWebResponse)request.GetResponse())
                    {
                        using (Stream responseStream = response.GetResponseStream())
                        {
                            using (StreamReader reader = new StreamReader(responseStream, this.Encoder))
                            {
                                CallBackData = reader.ReadToEnd();
                            }
                        }
                    }
                }
            }
            return CallBackData;
        }

        /// <summary>
        /// 传送文件
        /// </summary>
        /// <param name="TransferStream">要传送的文件</param>
        /// <returns></returns>
        public string Send(Stream TransferStream)
        {
            if (TransferStream == null || TransferStream.Length == 0)
                throw new ArgumentNullException("TransferStream");


            UriBuilder TransferUri = new UriBuilder(this.Url);
            TransferUri.Query = this.Parameters.ToString();
            HttpWebRequest request = (HttpWebRequest)WebRequest.Create(TransferUri.Uri);
            request.Method = this.Type;

            string CallBackData;

            using (Stream requestStream = request.GetRequestStream())
            {
                byte[] buffer = new byte[4096];
                int readcount = 0;
                while ((readcount = TransferStream.Read(buffer, 0, buffer.Length)) > 0)
                {
                    requestStream.Write(buffer, 0, readcount);
                    requestStream.Flush();
                    this.DataSent += readcount;
                }
                using (HttpWebResponse response = (HttpWebResponse)request.GetResponse())
                {
                    using (Stream responseStream = response.GetResponseStream())
                    {
                        using (StreamReader reader = new StreamReader(responseStream, this.Encoder))
                        {
                            CallBackData = reader.ReadToEnd();
                        }
                    }
                }
            }
            return CallBackData;
        }
        #endregion
    }
}
