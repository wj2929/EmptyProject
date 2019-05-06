using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.IO;
using System.IO.Compression;
using System.Runtime.InteropServices;
using EmptyProject.Core.Validation;

namespace EmptyProject.Core.IO
{
    public class IOHelper
    {
        [DllImport("kernel32.dll", CharSet = CharSet.Auto)]
        public static extern int GetShortPathName([MarshalAs(UnmanagedType.LPTStr)]  string path, [MarshalAs(UnmanagedType.LPTStr)]  StringBuilder shortPath, int shortPathLength);

        /// <summary>
        /// 获取短路径
        /// </summary>
        /// <param name="longName"></param>
        /// <returns></returns>
        public static string GetShortPathName(string longName)
        {
            StringBuilder shortNameBuffer = new StringBuilder(256);
            int bufferSize = shortNameBuffer.Capacity;

            int result = GetShortPathName(longName, shortNameBuffer, bufferSize);

            return shortNameBuffer.ToString();
        }

        /// <summary>
        /// Copy文件夹
        /// </summary>
        /// <param name="Src"></param>
        /// <param name="Dst"></param>
        public static void CopyDirectory(string Src, string Dst)
        {
            String[] Files;

            if (Dst[Dst.Length - 1] != Path.DirectorySeparatorChar)
                Dst += Path.DirectorySeparatorChar;
            if (!Directory.Exists(Dst)) Directory.CreateDirectory(Dst);
            Files = Directory.GetFileSystemEntries(Src);
            foreach (string Element in Files)
            {
                // Sub directories
                if (Directory.Exists(Element))
                    CopyDirectory(Element, Dst + Path.GetFileName(Element));
                // Files in directory
                else
                    File.Copy(Element, Dst + Path.GetFileName(Element), true);
            }
        }


        /// <summary>
        /// 转换为本地服务器路径
        /// </summary>
        /// <param name="path"></param>
        /// <returns></returns>
        public static string LocateServerPath(string path)
        {
            if (System.IO.Path.IsPathRooted(path) == false)
                path = System.IO.Path.Combine(AppDomain.CurrentDomain.SetupInformation.ApplicationBase, path);

            return path;
        }

        /// <summary>
        /// 取得指定路径的扩展名
        /// </summary>
        /// <param name="FilePath">文件路径</param>
        /// <returns></returns>
        public static string GetExtensionName(string FilePath)
        {
            if (FilePath.IsEmpty())
                return string.Empty;

            return Path.GetExtension(FilePath);
        }
        /// <summary>
        /// 写文件
        /// </summary>
        /// <param name="FilePath"></param>
        /// <param name="Contents"></param>
        public static void FileWrite(string FileFullPath, string Contents)
        {
            string FilePath = FileFullPath.Substring(0, FileFullPath.LastIndexOf('\\'));
            if (!Directory.Exists(FilePath))
                Directory.CreateDirectory(FilePath);
            File.WriteAllText(FileFullPath, Contents);
        }
        /// <summary>
        /// 读文件
        /// </summary>
        /// <param name="FilePath"></param>
        /// <param name="Contents"></param>
        /// <returns></returns>
        public static string FileRead(string FileFullPath)
        {
            if (File.Exists(FileFullPath))
                return File.ReadAllText(FileFullPath);
            else
                return string.Empty;
        }
        /// <summary>
        /// 删除文件
        /// </summary>
        /// <param name="FileFullPath"></param>
        public static void DelFile(string FileFullPath)
        {
            if (File.Exists(FileFullPath))
            {
                File.Delete(FileFullPath);
            }
        }
        /// <summary>
        /// 创建目录
        /// </summary>
        /// <param name="FileFullPath"></param>
        public static void CreateDirectory(string FileFullPath)
        {
            if (!Directory.Exists(FileFullPath))
            {
                Directory.CreateDirectory(FileFullPath);
            }
        }
        /// <summary>
        /// 删除目录
        /// </summary>
        /// <param name="FolderFullPath"></param>
        public static void DelDirectory(string FolderFullPath)
        {
            if (Directory.Exists(FolderFullPath))
            {
                Directory.Delete(FolderFullPath, true);
            }
        }
        /// <summary>
        /// 文件是否存在
        /// </summary>
        /// <param name="FileFullPath"></param>
        /// <returns></returns>
        public static bool FileExist(string FileFullPath)
        {
            return File.Exists(FileFullPath);
        }

        /// <summary>
        /// 取得文件流
        /// </summary>
        /// <param name="FileName"></param>
        /// <returns></returns>
        public static Stream GetFileStream(string FileName)
        {
            if (!FileExist(FileName))
                return null;
            try
            {
                return new FileStream(FileName, FileMode.Open, FileAccess.Read, FileShare.Read);
            }
            catch
            {
                return null;
            }
        }

        /// <summary>
        /// 读filename到byte[]
        /// </summary>
        /// <param name="fileName"></param>
        /// <returns></returns>
        public static byte[] ReadFile(string fileName)
        {
            FileStream pFileStream = null;
            byte[] pReadByte = new byte[0];
            try
            {
                pFileStream = new FileStream(fileName, FileMode.Open, FileAccess.Read);
                BinaryReader r = new BinaryReader(pFileStream);
                r.BaseStream.Seek(0, SeekOrigin.Begin);    //将文件指针设置到文件开
                pReadByte = r.ReadBytes((int)r.BaseStream.Length);
                return pReadByte;
            }
            catch
            {
                return pReadByte;
            }
            finally
            {
                if (pFileStream != null)
                    pFileStream.Close();
            }
        }


        /// <summary>
        /// 写byte[]到fileName
        /// </summary>
        /// <param name="pReadByte"></param>
        /// <param name="fileName"></param>
        /// <returns></returns>
        public static bool WriteFile(byte[] pReadByte, string fileName)
        {
            FileStream pFileStream = null;
            try
            {
                pFileStream = new FileStream(fileName, FileMode.OpenOrCreate);
                pFileStream.Write(pReadByte, 0, pReadByte.Length);
            }
            catch
            {
                return false;
            }
            finally
            {
                if (pFileStream != null)
                    pFileStream.Close();
            }
            return true;
        }
    }
}
