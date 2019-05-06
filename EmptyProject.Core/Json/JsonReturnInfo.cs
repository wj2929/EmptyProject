using System.Collections.Generic;
using System.Text;

namespace EmptyProject.Core.Json
{
    public class JsonReturnInfo
    {
        private IDictionary<string, string> _Values;
        /// <summary>
        /// 值列表
        /// </summary>
        public IDictionary<string, string> Values
        {
            get
            {
                if (this._Values == null)
                    this._Values = new Dictionary<string, string>();

                return this._Values;
            }
            set
            {
                this._Values = value;
            }
        }

        private IList<JsonReturnInfo> _Items;
        /// <summary>
        /// 子对象
        /// </summary>
        public IList<JsonReturnInfo> Items
        {
            get
            {
                if (this._Items == null)
                    this._Items = new List<JsonReturnInfo>();

                return this._Items;
            }
            set
            {
                this._Items = value;
            }
        }

        /// <summary>
        /// 取得Json数据
        /// </summary>
        /// <returns></returns>
        public string ToJson()
        {
            int Count = 0;
            StringBuilder sb = new StringBuilder();
            sb.Append("{");
            foreach (KeyValuePair<string, string> Item in Values)
            {
                if (sb.Length > 1)
                {
                    sb.Append(",");
                }
                sb.Append("'");
                sb.Append(Item.Key);
                sb.Append("':'");
                sb.Append(Item.Value);
                sb.Append("'");
            }
            if (Items.Count > 0)
            {
                if (Values.Count > 0)
                {
                    sb.Append(",");
                }
                sb.Append("'Items':[");
                foreach (JsonReturnInfo JItem in this.Items)
                {
                    if (Count > 0)
                    {
                        sb.Append(",");
                    }
                    sb.Append(JItem.ToJson());
                    Count++;
                }
                sb.Append("]");
            }
            sb.Append("}");
            return sb.ToString().Replace("{}", "");
        }
    }
}
