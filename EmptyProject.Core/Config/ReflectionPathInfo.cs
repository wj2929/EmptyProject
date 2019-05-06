using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using EmptyProject.Core.Validation;
using EmptyProject.Core.Collection;

namespace EmptyProject.Core.Config
{
    public class ReflectionPathInfo : IConfigBase<ReflectionPathInfo>
    {
        /// <summary>
        /// 键
        /// </summary>
        public string Key { get; set; }
        /// <summary>
        /// 名称
        /// </summary>
        public string Name { get; set; }
        /// <summary>
        /// 完整路径
        /// </summary>
        public string FullPath { get; set; }
        /// <summary>
        /// 反射信息
        /// </summary>
        public string AssemblyPath { get; set; }

        #region IConfigBase
        /// <summary>
        /// 转换为配置文件
        /// </summary>
        /// <returns></returns>
        public string ToConfig()
        {
            IDictionary<string, string> Parameters = new Dictionary<string, string>(4);
            Parameters.Add("Key", this.Key);
            Parameters.Add("Name", this.Name);
            Parameters.Add("FullPath", this.FullPath);
            Parameters.Add("AssemblyPath", this.AssemblyPath);
            return Parameters.ToConfig("ReflectionPathInfo");
        }

        /// <summary>
        /// 从配置文件构造对象
        /// </summary>
        /// <param name="Config"></param>
        /// <returns></returns>
        public ReflectionPathInfo FromConfig(string Config)
        {
            if (Config.IsEmpty())
                return null;

            return new ReflectionPathInfo()
            {
                Key = Config.GetTag("Key"),
                Name = Config.GetTag("Name"),
                FullPath = Config.GetTag("FullPath"),
                AssemblyPath = Config.GetTag("AssemblyPath")
            };
        } 
        #endregion
    }
}
