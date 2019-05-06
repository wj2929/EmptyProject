using System;
using EmptyProject.Core.Config;

namespace EmptyProject.Core.Net
{
    public interface IHttpParameterCollection
    {
        #region 加入参数
        /// <summary>
        /// 加入参数
        /// </summary>
        /// <param name="Key">键</param>
        /// <param name="Value">值</param>
        void Add(string Key, string Value);
        /// <summary>
        /// 加入参数
        /// </summary>
        /// <param name="Key">键</param>
        /// <param name="Value">值</param>
        void Add(string Key, Guid Value);
        /// <summary>
        /// 加入参数
        /// </summary>
        /// <param name="Key">键</param>
        /// <param name="Value">值</param>
        void Add(string Key, DateTime Value);
        /// <summary>
        /// 加入参数
        /// </summary>
        /// <param name="Key">键</param>
        /// <param name="Value">值</param>
        void Add(string Key, decimal Value);
        /// <summary>
        /// 加入参数
        /// </summary>
        /// <param name="Key">键</param>
        /// <param name="Value">值</param>
        void Add(string Key, int Value);
        /// <summary>
        /// 加入参数
        /// </summary>
        /// <param name="Key">键</param>
        /// <param name="Value">值</param>
        void Add(string Key, byte Value);
        /// <summary>
        /// 加入参数
        /// </summary>
        /// <param name="Key">键</param>
        /// <param name="Value">值</param>
        void Add<T>(string Key, IConfigBase<T> Value);
        /// <summary>
        /// 加入参数
        /// </summary>
        /// <param name="Key">键</param>
        /// <param name="Value">值</param>
        void Add(string Key, bool Value);
        #endregion
        /// <summary>
        /// 移除一个参数
        /// </summary>
        /// <param name="Key">键</param>
        void Remove(string Key);
        /// <summary>
        /// 将所有参数清空
        /// </summary>
        void Clear();
        /// <summary>
        /// 转换为字符串
        /// </summary>
        /// <returns></returns>
        string ToString();
    }
}
