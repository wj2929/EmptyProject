using System;
using System.Collections.Generic;
using System.Linq;
using EmptyProject.Core.Config;
using EmptyProject.Core.Validation;

namespace EmptyProject.Core.Net
{
    public class HttpParameterCollection : IHttpParameterCollection
    {
        public HttpParameterCollection()
        {
            this._Dic = new Dictionary<string, string>();
        }
        private IDictionary<string, string> _Dic = null;
        /// <summary>
        /// 参数字典
        /// </summary>
        private IDictionary<string, string> Dic
        {
            get
            {
                if (this._Dic == null)
                    this._Dic = new Dictionary<string, string>();

                return this._Dic;
            }
        }

        #region IHttpParameterCollection 成员

        /// <summary>
        /// 加入参数
        /// </summary>
        /// <param name="Key">键</param>
        /// <param name="Value">值</param>
        public void Add(string Key, string Value)
        {
            this.Dic[Key] = Value;
        }

        /// <summary>
        /// 加入参数
        /// </summary>
        /// <param name="Key">键</param>
        /// <param name="Value">值</param>
        public void Add(string Key, Guid Value)
        {
            this.Add(Key, Value.ToString());
        }

        /// <summary>
        /// 加入参数
        /// </summary>
        /// <param name="Key">键</param>
        /// <param name="Value">值</param>
        public void Add(string Key, DateTime Value)
        {
            this.Add(Key, Value.ToString());
        }

        /// <summary>
        /// 加入参数
        /// </summary>
        /// <param name="Key">键</param>
        /// <param name="Value">值</param>
        public void Add(string Key, decimal Value)
        {
            this.Add(Key, Value.ToString());
        }

        /// <summary>
        /// 加入参数
        /// </summary>
        /// <param name="Key">键</param>
        /// <param name="Value">值</param>
        public void Add(string Key, int Value)
        {
            this.Add(Key, Value.ToString());
        }

        /// <summary>
        /// 加入参数
        /// </summary>
        /// <param name="Key">键</param>
        /// <param name="Value">值</param>
        public void Add(string Key, byte Value)
        {
            this.Add(Key, Value.ToString());
        }

        /// <summary>
        /// 加入参数
        /// </summary>
        /// <param name="Key">键</param>
        /// <param name="Value">值</param>
        public void Add<T>(string Key, IConfigBase<T> Value)
        {
            this.Add(Key, Value.ToConfig());
        }
        /// <summary>
        /// 加入参数
        /// </summary>
        /// <param name="Key">键</param>
        /// <param name="Value">值</param>
        public void Add(string Key, bool Value)
        {
            this.Add(Key, Value.ToString());
        }

        /// <summary>
        /// 移除一个参数
        /// </summary>
        /// <param name="Key">键</param>
        public void Remove(string Key)
        {
            this.Dic.Remove(Key);
        }

        /// <summary>
        /// 将所有参数清空
        /// </summary>
        public void Clear()
        {
            this.Dic.Clear();
        }

        /// <summary>
        /// 拼接参数
        /// </summary>
        /// <returns></returns>
        public override string ToString()
        {
            if (this.Dic.Count == 0)
                return "";

            System.Text.StringBuilder sb = new System.Text.StringBuilder(this.Dic.Count * 10);
            string[] tArr = this.Dic.Where(t => !t.Key.IsEmpty() && !t.Value.IsEmpty()).Select(t => string.Format("{0}={1}", t.Key, t.Value)).ToArray();
            if (tArr.Length == 0)
                return "";

            return string.Join("&", tArr);
        }
        #endregion
    }
}
