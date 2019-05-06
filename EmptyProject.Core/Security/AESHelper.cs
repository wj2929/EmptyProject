using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Security;
using System.Security.Cryptography;
using EmptyProject.Core.Validation;

namespace EmptyProject.Core.Security
{
    /// <summary>
    /// AES加密助手
    /// </summary>
    public class AESHelper
    {
        /// <summary>
        /// 公钥
        /// </summary>
        private static string PublicKey
        {
            get
            {
                return "3f9asd9bngadf902kfsad932kfhfsadf";
            }
        }

        /// <summary>
        /// AES解密函数
        /// </summary>
        /// <param name="pToDencrypt">要解密的字符串</param>
        /// <param name="skey">私钥(最高32字节,对应256位AES)，为空则使用公钥</param>
        /// <returns>解密后的结果</returns>
        public static string AESDecrypt(string pToDencrypt, string skey = "")
        {
            try
            {
                byte[] keyArray = UTF8Encoding.UTF8.GetBytes(skey.IsEmpty() ? PublicKey : skey);
                byte[] toEncryptArray = Convert.FromBase64String(pToDencrypt);

                RijndaelManaged rDel = new RijndaelManaged();
                rDel.Key = keyArray;
                rDel.Mode = CipherMode.ECB;
                rDel.Padding = PaddingMode.PKCS7;

                ICryptoTransform cTransform = rDel.CreateDecryptor();
                byte[] resultArray = cTransform.TransformFinalBlock(toEncryptArray, 0, toEncryptArray.Length);

                return UTF8Encoding.UTF8.GetString(resultArray);
            }
            catch
            {
                return "err";
            }
        }

        /// <summary>
        /// AES加密函数
        /// </summary>
        /// <param name="pToEncrypt">要加密的字符串</param>
        /// <param name="skey">私钥(最高32字节,对应256位AES)，为空则使用公钥</param>
        /// <returns>加密结果</returns>
        public static string AESEncrypt(string pToEncrypt, string skey = "")
        {
            byte[] keyArray = UTF8Encoding.UTF8.GetBytes(skey.IsEmpty() ? PublicKey : skey);
            byte[] toEncryptArray = UTF8Encoding.UTF8.GetBytes(pToEncrypt);

            RijndaelManaged rDel = new RijndaelManaged();
            rDel.Key = keyArray;
            rDel.Mode = CipherMode.ECB;
            rDel.Padding = PaddingMode.PKCS7;

            ICryptoTransform cTransform = rDel.CreateEncryptor();
            byte[] resultArray = cTransform.TransformFinalBlock(toEncryptArray, 0, toEncryptArray.Length);

            return Convert.ToBase64String(resultArray, 0, resultArray.Length);
        }
    }
}
