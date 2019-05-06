using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Security.Cryptography;
namespace EmptyProject.Core.Security
{
    public class SignatureHelper
    {
        /// <summary>
        /// SHA1签名
        /// </summary>
        /// <param name="InputStr">待签名的字符串</param>
        /// <returns></returns>
        public static string SHA1(string InputStr)
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
    }
}
