using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Net;
using System.IO;
using System.Security.Cryptography;
using EmptyProject.Core.Security;
using EmptyProject.Core.Validation;
using EmptyProject.Core.Time;

namespace EmptyProject.Core.Net
{
    /// <summary>
    /// 提供安全通信的相关方法
    /// （此类不能被继承）
    /// </summary>
    public sealed class SecurityCommunication
    {
        /// <summary>
        /// 构造函数
        /// </summary>
        /// <param name="PrivateKey">私钥</param>
        /// <param name="SecretKeyMode">密钥模式</param>
        /// <param name="SecretKeyLifeCycle">密钥生命周期</param>
        public SecurityCommunication(string PrivateKey = "", SecretKeyModes SecretKeyMode = SecretKeyModes.私钥加时间戳, SecretKeyLifeCycles SecretKeyLifeCycle = SecretKeyLifeCycles.本日内)
        {
            this.PrivateKey = PrivateKey;
            this.SecretKeyMode = SecretKeyMode;
            this.SecretKeyLifeCycle = SecretKeyLifeCycle;
        }

        private INetHelper _NetHelper;
        /// <summary>
        /// 网络访问助手
        /// </summary>
        private INetHelper NetHelper
        {
            get
            {
                if (this._NetHelper == null)
                    this._NetHelper = new NetHelper();

                return this._NetHelper;
            }
        }
        /// <summary>
        /// 私钥
        /// </summary>
        private string PrivateKey { get; set; }
        /// <summary>
        /// 密钥
        /// </summary>
        private string SecretKey
        {
            get
            {
                switch (this.SecretKeyMode)
                {
                    case SecretKeyModes.公钥:
                        return "";
                    case SecretKeyModes.私钥:
                        return this.PrivateKey;
                    case SecretKeyModes.私钥加时间戳:
                        {
                            int TimeStampLength = this.TimeStamp.Length;
                            int PrivateKeyLength = this.PrivateKey.Length;
                            if (PrivateKeyLength + TimeStampLength > 32)
                                return this.PrivateKey.Substring(0, PrivateKeyLength - (PrivateKeyLength + TimeStampLength - 32)) + this.TimeStamp;

                            return this.PrivateKey + this.TimeStamp;
                        }
                }

                return "";
            }
        }
        /// <summary>
        /// 时间戳
        /// </summary>
        private string TimeStamp
        {
            get
            {
                return this.SecretKeyLifeCycle == SecretKeyLifeCycles.本日内 ? DateTimeHelper.GetDateString() : string.Format("{0:yyyyMMddhh}", DateTime.Now);
            }
        }
        /// <summary>
        /// 签名
        /// </summary>
        public string Signature { get; private set; }
        /// <summary>
        /// 密文
        /// </summary>
        public string CipherText { get; private set; }
        /// <summary>
        /// 明文
        /// </summary>
        public string PlainText { get; private set; }
        /// <summary>
        /// 加密模式
        /// </summary>
        public SecretKeyModes SecretKeyMode { get; private set; }
        /// <summary>
        /// 密钥生命周期
        /// </summary>
        public SecretKeyLifeCycles SecretKeyLifeCycle { get; private set; }
        /// <summary>
        /// 加密
        /// </summary>
        public void Encrypt(string PlainText)
        {
            this.PlainText = PlainText;
            this.CipherText = AESHelper.AESEncrypt(this.PlainText, this.SecretKey).Replace("+", "{{PLUS}}");
            this.Signature = SignatureHelper.SHA1(this.SecretKey + this.PlainText);
        }
        /// <summary>
        /// 数据校验
        /// </summary>
        /// <param name="CipherText">密文</param>
        /// <param name="Signature">签名</param>
        public bool Check(string CipherText, string Signature = "")
        {
            this.CipherText = CipherText.Replace("{{PLUS}}", "+");
            this.Signature = Signature;

            this.PlainText = AESHelper.AESDecrypt(this.CipherText, this.SecretKey);

            if (PlainText == "err")
                return false;

            if (Signature.IsEmpty())
                return true;

            return SignatureHelper.SHA1(this.SecretKey + this.PlainText) == this.Signature;
        }

        /// <summary>
        /// 安全通信
        /// </summary>
        /// <param name="PlainText">明文</param>
        /// <param name="TargetUrl">目标地址</param>
        /// <param name="Method">
        /// [模式]；
        /// 可选值：POST、GET；
        /// 默认为：POST。
        /// </param>
        public string Communication(string PlainText, string TargetUrl, string Method = "POST")
        {
            if (PlainText.IsEmpty())
                throw new ArgumentNullException("PlainText");

            if (TargetUrl.IsEmpty())
                throw new ArgumentNullException("TargetUrl");

            this.Encrypt(PlainText);

            this.NetHelper.Type = Method;
            this.NetHelper.Url = TargetUrl;
            this.NetHelper.Parameters.Clear();
            this.NetHelper.Parameters.Add("Signature", this.Signature);
            this.NetHelper.Parameters.Add("CipherText", this.CipherText);
            return this.NetHelper.Send();
        }

        /// <summary>
        /// 安全文件传送
        /// </summary>
        /// <param name="PlainText">明文参数</param>
        /// <param name="TargetUrl">目标地址</param>
        /// <param name="InputStream">要传送的文件流</param>
        /// <param name="Method">
        /// [模式]；
        /// 可选值：POST、GET；
        /// 默认为：POST。
        /// </param>
        /// <returns></returns>
        public string Communication(string PlainText, string TargetUrl, Stream InputStream, string Method = "POST")
        {
            if (PlainText.IsEmpty())
                throw new ArgumentNullException("PlainText");

            if (TargetUrl.IsEmpty())
                throw new ArgumentNullException("TargetUrl");

            if (InputStream == null || InputStream.Length == 0)
                throw new ArgumentNullException("InputStream");

            this.Encrypt(PlainText);

            this.NetHelper.Type = Method;
            this.NetHelper.Url = TargetUrl;
            this.NetHelper.Parameters.Clear();
            this.NetHelper.Parameters.Add("Signature", this.Signature);
            this.NetHelper.Parameters.Add("CipherText", this.CipherText);

            return this.NetHelper.Send(InputStream);
        }
    }

    /// <summary>
    /// 加密模式
    /// </summary>
    [Flags]
    public enum SecretKeyModes
    {
        公钥 = 1,
        私钥 = 2,
        私钥加时间戳 = 4
    }

    /// <summary>
    /// 密钥生命周期
    /// </summary>
    [Flags]
    public enum SecretKeyLifeCycles
    {
        本日内 = 1,
        本小时内 = 2
    }

}
