
using System;
using System.Collections.Generic;
using System.Security.Cryptography;
using System.Text;

namespace EmptyProject.Core.WebOnly
{
    public class EncryptHelper
    {
        private string _PublicKey = "reofudslgpcvuibowekfbysaudifsgfkdbvfuisxtvwkebvfjsdtvduierkvgklautadsbjfsghofdydiahbfksdgfiusadhfsakfh";
        private string _PrivateKey = "";
        private KeyMethod _EncryptKeyMethod = KeyMethod.公钥;
        private EncryptMethod _EncryptEncryptMethod = EncryptMethod.DES;
        private string _FinalKey = "";

        /// <summary>
        /// 私钥
        /// </summary>
        public string PrivateKey
        {
            set { _PrivateKey = value; }
        }

        /// <summary>
        /// 公钥
        /// </summary>
        public string PublicKey
        {
            set { _PublicKey = value; }
        }

        /// <summary>
        /// 密钥模式
        /// </summary>
        public KeyMethod EncryptKeyMethod
        {
            get { return _EncryptKeyMethod; }
            set { _EncryptKeyMethod = value; }
        }

        /// <summary>
        /// 加密模式
        /// </summary>
        public EncryptMethod EncryptEncryptMethod
        {
            get { return _EncryptEncryptMethod; }
            set { _EncryptEncryptMethod = value; }
        }

        /// <summary>
        /// 加密
        /// </summary>
        /// <param name="InputStr"></param>
        /// <returns></returns>
        public string Encrypt(string InputStr)
        {
            StringBuilder sb = new StringBuilder();
            sb.Append(_PublicKey);
            if (_EncryptKeyMethod == KeyMethod.公钥和日期)
            {
                sb.Append(DateTime.Now.Year);
                sb.Append(DateTime.Now.Month);
                sb.Append(DateTime.Now.Day);
            }
            else if (_EncryptKeyMethod == KeyMethod.公钥和日期和私钥)
            {
                sb.Append(DateTime.Now.Year);
                sb.Append(DateTime.Now.Month);
                sb.Append(DateTime.Now.Day);
                sb.Append(_PrivateKey);
            }
            _FinalKey = sb.ToString();
            switch (_EncryptEncryptMethod)
            {
                case EncryptMethod.DES:
                    return DESEncrypt(InputStr, _FinalKey);
                case EncryptMethod.Hash:
                    return HashEncoding(InputStr);
                case EncryptMethod.MD5:
                    return MD5Encrypt(InputStr);
                case EncryptMethod.SHA1:
                    return SHA1Encrypt(InputStr);
                default:
                    return MD5Encrypt(InputStr);
            }
        }

        /// <summary>
        /// 解密
        /// </summary>
        /// <param name="InputStr"></param>
        /// <returns></returns>
        public string Decrypt(string InputStr)
        {
            StringBuilder sb = new StringBuilder();
            sb.Append(_PublicKey);
            if (_EncryptKeyMethod == KeyMethod.公钥和日期)
            {
                sb.Append(DateTime.Now.Year);
                sb.Append(DateTime.Now.Month);
                sb.Append(DateTime.Now.Day);
            }
            else if (_EncryptKeyMethod == KeyMethod.公钥和日期和私钥)
            {
                sb.Append(DateTime.Now.Year);
                sb.Append(DateTime.Now.Month);
                sb.Append(DateTime.Now.Day);
                sb.Append(_PrivateKey);
            }
            _FinalKey = sb.ToString();
            return DESDecrypt(InputStr, _FinalKey);
        }

        /// <summary>
        /// 默认加密的静态方法
        /// </summary>
        /// <param name="InputStr">要加密的字符串</param>
        /// <param name="inputPrivateKey">私钥</param>
        /// <param name="inputKeyMethod">密钥模式</param>
        /// <returns></returns>
        public static string StaticDESEncrypt(string InputStr, string inputPrivateKey, KeyMethod inputKeyMethod)
        {
            EncryptHelper t = new EncryptHelper();
            t.EncryptKeyMethod = inputKeyMethod;
            t.PrivateKey = inputPrivateKey;
            return t.Encrypt(InputStr);
        }

        /// <summary>
        /// 默认解密的静态方法
        /// </summary>
        /// <param name="InputStr">要加密的字符串</param>
        /// <param name="inputPrivateKey">私钥</param>
        /// <param name="inputKeyMethod">密钥模式</param>
        /// <returns></returns>
        public static string StaticDESDecrypt(string InputStr, string inputPrivateKey, KeyMethod inputKeyMethod)
        {
            EncryptHelper t = new EncryptHelper();
            t.EncryptKeyMethod = inputKeyMethod;
            t.PrivateKey = inputPrivateKey;
            return t.Decrypt(InputStr);
        }

        /// <summary>
        /// 生成MD5签名的静态方法
        /// </summary>
        /// <param name="InputStr"></param>
        /// <returns></returns>
        public static string StaticMD5(string InputStr)
        {
            EncryptHelper t = new EncryptHelper();
            t.EncryptEncryptMethod = EncryptMethod.MD5;
            return t.Encrypt(InputStr);
        }

        /// <summary>
        /// 生成SHA1签名的静态方法
        /// </summary>
        /// <param name="InputStr"></param>
        /// <returns></returns>
        public static string StaticSHA1(string InputStr)
        {
            EncryptHelper t = new EncryptHelper();
            t.EncryptEncryptMethod = EncryptMethod.SHA1;
            return t.Encrypt(InputStr);
        }

        #region DES

        /// <summary> 
        /// 加密数据 
        /// </summary> 
        /// <param name="Text"></param> 
        /// <param name="sKey"></param> 
        /// <returns></returns> 
        private string DESEncrypt(string Text, string sKey)
        {
            DESCryptoServiceProvider des = new DESCryptoServiceProvider();
            byte[] inputByteArray;
            inputByteArray = Encoding.Default.GetBytes(Text);
            des.Key = ASCIIEncoding.ASCII.GetBytes(System.Web.Security.FormsAuthentication.HashPasswordForStoringInConfigFile(sKey, "md5").Substring(0, 8));
            des.IV = ASCIIEncoding.ASCII.GetBytes(System.Web.Security.FormsAuthentication.HashPasswordForStoringInConfigFile(sKey, "md5").Substring(0, 8));
            System.IO.MemoryStream ms = new System.IO.MemoryStream();
            CryptoStream cs = new CryptoStream(ms, des.CreateEncryptor(), CryptoStreamMode.Write);
            cs.Write(inputByteArray, 0, inputByteArray.Length);
            cs.FlushFinalBlock();
            StringBuilder ret = new StringBuilder();
            foreach (byte b in ms.ToArray())
            {
                ret.AppendFormat("{0:X2}", b);
            }
            return ret.ToString();
        }

        /// <summary> 
        /// 解密数据 
        /// </summary> 
        /// <param name="Text"></param> 
        /// <param name="sKey"></param> 
        /// <returns></returns> 
        private string DESDecrypt(string Text, string sKey)
        {
            try
            {
                DESCryptoServiceProvider des = new DESCryptoServiceProvider();
                int len;
                len = Text.Length / 2;
                byte[] inputByteArray = new byte[len];
                int x, i;
                for (x = 0; x < len; x++)
                {
                    i = Convert.ToInt32(Text.Substring(x * 2, 2), 16);
                    inputByteArray[x] = (byte)i;
                }
                des.Key = ASCIIEncoding.ASCII.GetBytes(System.Web.Security.FormsAuthentication.HashPasswordForStoringInConfigFile(sKey, "md5").Substring(0, 8));
                des.IV = ASCIIEncoding.ASCII.GetBytes(System.Web.Security.FormsAuthentication.HashPasswordForStoringInConfigFile(sKey, "md5").Substring(0, 8));
                System.IO.MemoryStream ms = new System.IO.MemoryStream();
                CryptoStream cs = new CryptoStream(ms, des.CreateDecryptor(), CryptoStreamMode.Write);
                cs.Write(inputByteArray, 0, inputByteArray.Length);
                cs.FlushFinalBlock();
                return Encoding.Default.GetString(ms.ToArray());
            }
            catch
            {
                return "err";
            }
        }

        #endregion

        #region Hash
        /// <summary>
        /// 得到随机哈希加密字符串
        /// </summary>
        /// <returns></returns>
        private string GetSecurity()
        {
            string Security = HashEncoding(GetRandomValue());
            return Security;
        }
        /// <summary>
        /// 得到一个随机数值
        /// </summary>
        /// <returns></returns>
        private string GetRandomValue()
        {
            Random Seed = new Random();
            string RandomVaule = Seed.Next(1, int.MaxValue).ToString();
            return RandomVaule;
        }
        /// <summary>
        /// 哈希加密一个字符串
        /// </summary>
        /// <param name="Security"></param>
        /// <returns></returns>
        private string HashEncoding(string Security)
        {
            byte[] Value;
            UnicodeEncoding Code = new UnicodeEncoding();
            byte[] Message = Code.GetBytes(Security);
            SHA512Managed Arithmetic = new SHA512Managed();
            Value = Arithmetic.ComputeHash(Message);
            Security = "";
            foreach (byte o in Value)
            {
                Security += (int)o + "O";
            }
            return Security;
        }
        #endregion

        #region MD5
        private string MD5Encrypt(string InputStr)
        {
            return System.Web.Security.FormsAuthentication.HashPasswordForStoringInConfigFile(InputStr, "MD5");
        }
        #endregion

        #region SHA1
        private string SHA1Encrypt(string InputStr)
        {
            byte[] StrRes = Encoding.Default.GetBytes(InputStr);
            HashAlgorithm iSHA = new SHA1CryptoServiceProvider();
            StrRes = iSHA.ComputeHash(StrRes);
            StringBuilder EnText = new StringBuilder();
            foreach (byte iByte in StrRes)
            {
                EnText.AppendFormat("{0:x2}", iByte);
            }
            return EnText.ToString();
        }
        #endregion
    }

    public enum EncryptMethod
    {
        MD5, SHA1, DES, Hash, RSA
    }

    public enum KeyMethod
    {
        公钥, 公钥和日期, 公钥和日期和私钥
    }
}
