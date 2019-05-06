using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.IO;
namespace EmptyProject.Core.Net
{
    public interface INetHelper
    {
        /// <summary>
        /// 参数列表
        /// </summary>
        IHttpParameterCollection Parameters { get; set; }
        /// <summary>
        /// 访问地址
        /// </summary>
        string Url { get; set; }
        /// <summary>
        /// 类型
        /// POST,GET
        /// </summary>
        string Type { get; set; }
        /// <summary>
        /// /// 发送数据
        /// </summary>
        /// <returns>返回回传数据</returns>
        string Send();
        /// <summary>
        /// 向远程服务器发送文件
        /// </summary>
        /// <param name="TransferStream">要传送的文件</param>
        /// <returns></returns>
        string Send(Stream TransferStream);
        /// <summary>
        /// 编码
        /// </summary>
        Encoding Encoder { get; set; }
    }
}
