using System;
using System.Collections.Generic;
using System.Text;
using System.Security.Cryptography;
using System.IO;
/// <summary>
/// Summary description for Des
/// </summary>
namespace EmptyProject.Core.Security
{
    public class Des
    {
        public Des()
        {
            //
            // TODO: 在此处添加构造函数逻辑
            //
        }
        private static byte[] desKey = new byte[] { 0x16, 0x09, 0x14, 0x15, 0x07, 0x01, 0x05, 0x08 };
        private static byte[] desIV = new byte[] { 0x16, 0x09, 0x14, 0x15, 0x07, 0x01, 0x05, 0x08 };

        /// <summary>
        /// 加密字符串
        /// </summary>
        /// <param name="pToEncrypt">待加密字符串</param>
        /// <returns>加密字符串</returns>
        public static string Encrypt(string pToEncrypt)
        {

            DESCryptoServiceProvider des = new DESCryptoServiceProvider();
            try
            {
                //把字符串放到byte数组中
                byte[] inputByteArray = Encoding.Default.GetBytes(pToEncrypt);

                //建立加密对象的密钥和偏移量
                //原文使用ASCIIEncoding.ASCII方法的GetBytes方法
                //使得输入密码必须输入英文文本
                des.Key = desKey;		// ASCIIEncoding.ASCII.GetBytes(sKey);
                des.IV = desIV;			//ASCIIEncoding.ASCII.GetBytes(sKey);
                MemoryStream ms = new MemoryStream();
                CryptoStream cs = new CryptoStream(ms, des.CreateEncryptor(),
                    CryptoStreamMode.Write);
                //Write the byte array into the crypto stream
                //(It will end up in the memory stream)
                cs.Write(inputByteArray, 0, inputByteArray.Length);
                cs.FlushFinalBlock();

                //Get the data back from the memory stream, and into a string
                StringBuilder ret = new StringBuilder();
                foreach (byte b in ms.ToArray())
                {
                    //Format as hex
                    ret.AppendFormat("{0:X2}", b);
                }
                ret.ToString();
                return ret.ToString();
            }
            catch
            {
                return pToEncrypt;
            }
            finally
            {
                des = null;
            }
        }
        /// <summary>
        /// 解密字符串
        /// </summary>
        /// <param name="pToDecrypt">待解密字符串</param>
        /// <returns>解密字符串</returns>
        public static string Decrypt(string pToDecrypt)
        {

            DESCryptoServiceProvider des = new DESCryptoServiceProvider();
            try
            {
                //Put the input string into the byte array
                byte[] inputByteArray = new byte[pToDecrypt.Length / 2];
                for (int x = 0; x < pToDecrypt.Length / 2; x++)
                {
                    int i = (Convert.ToInt32(pToDecrypt.Substring(x * 2, 2), 16));
                    inputByteArray[x] = (byte)i;
                }

                //建立加密对象的密钥和偏移量，此值重要，不能修改
                des.Key = desKey;			//ASCIIEncoding.ASCII.GetBytes(sKey);
                des.IV = desIV;				//ASCIIEncoding.ASCII.GetBytes(sKey);
                MemoryStream ms = new MemoryStream();
                CryptoStream cs = new CryptoStream(ms, des.CreateDecryptor(), CryptoStreamMode.Write);
                //Flush the data through the crypto stream into the memory stream
                cs.Write(inputByteArray, 0, inputByteArray.Length);
                cs.FlushFinalBlock();

                //Get the decrypted data back from the memory stream
                //建立StringBuild对象，CreateDecrypt使用的是流对象，必须把解密后的文本变成流对象
                StringBuilder ret = new StringBuilder();

                return Encoding.Default.GetString(ms.ToArray());
            }
            catch
            {
                return pToDecrypt;
            }
            finally
            {
                des = null;
            }
        }
    }
}