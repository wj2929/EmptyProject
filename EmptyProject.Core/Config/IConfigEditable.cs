using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace EmptyProject.Core.Config
{
    public interface IConfigEditable<TVal> : IConfigBase<TVal>
    {
        /// <summary>
        /// 保存
        /// </summary>
        void Save();
    }
}
