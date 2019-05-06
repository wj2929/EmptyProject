using System;
using System.IO;
using EmptyProject.Core.Validation;
using EmptyProject.Core.IO;

namespace EmptyProject.Core.Config
{
    public class LocalConfigurationAccessHelper<TValue> : IConfigAccessHelper<TValue>
                where TValue : class,IConfigBase<TValue>, new()
    {
        public LocalConfigurationAccessHelper(string ConfigName)
        {
            this.ConfigName = ConfigName;
        }

        private DateTime? _ConfigFileLastWriteTime;

        /// <summary>
        /// 配置文件路径
        /// </summary>
        private string ConfigPath { get; set; }

        private string _ConfigName;
        /// <summary>
        /// 配置文件名称
        /// </summary>
        public string ConfigName
        {
            get
            {
                return this._ConfigName;
            }
            private set
            {
                this._ConfigName = value;
                this.ConfigPath = IOHelper.LocateServerPath(String.Format("Config/{0}.config", value));
            }
        }

        #region IConfigAccessHelper
        /// <summary>
        /// 保存配置文件
        /// </summary>
        public void Save()
        {
            //File.WriteAllText(this.ConfigPath, this.ConfigEntity.ToConfig());
            Save(this.ConfigEntity.ToConfig());
        }

        /// <summary>
        /// 保存配置文件
        /// </summary>
        /// <param name="Config"></param>
        private void Save(string Config)
        {
            File.WriteAllText(this.ConfigPath, Config);
            _ConfigFileLastWriteTime = File.GetLastWriteTime(this.ConfigPath);
        }

        /// <summary>
        /// 替换配置文件
        /// </summary>
        public void Replace(string Config)
        {
            if (Config.IsEmpty())
                return;

            Save(Config);
            this.Load(Config);
        }

        private TValue _ConfigEntity;
        /// <summary>
        /// 配置对象实体
        /// </summary>
        public TValue ConfigEntity
        {
            get
            {
                if (this._ConfigEntity == null)
                    this.Load();

                return this._ConfigEntity;
            }
            protected set
            {
                this._ConfigEntity = value;
            }
        }

        /// <summary>
        /// 加载配置文件
        /// </summary>
        /// <returns></returns>
        public void Load()
        {
            if (_ConfigFileLastWriteTime == null || _ConfigFileLastWriteTime.Value != File.GetLastWriteTime(this.ConfigPath))
            {
                string ConfigStr = File.ReadAllText(this.ConfigPath);
                this.Load(ConfigStr);
                _ConfigFileLastWriteTime = File.GetLastWriteTime(this.ConfigPath);
            }
        }

        /// <summary>
        /// 加载配置文件
        /// </summary>
        /// <param name="Config"></param>
        private void Load(string Config)
        {
            this.ConfigEntity = new TValue().FromConfig(Config);
        }
        #endregion
    }
}
