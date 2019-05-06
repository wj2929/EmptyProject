using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Text;
using BC.Core;

namespace EmptyProject.Domain.Values.Configs
{
    public class UploadConfig : IConfigBase<UploadConfig>
    {
        private IDictionary<string, UploadItemConfig> _UploadItems;
        /// <summary>
        /// 上传项配置
        /// </summary>
        public IDictionary<string, UploadItemConfig> UploadItems
        {
            get
            {
                if (this._UploadItems == null)
                    this._UploadItems = new Dictionary<string, UploadItemConfig>();

                return this._UploadItems;
            }
        }

        #region IConfigBase<UploadConfig> 成员
        /// <summary>
        /// 转换为配置文件
        /// </summary>
        /// <returns></returns>
        public string ToConfig()
        {
            StringBuilder sb = new StringBuilder();
            sb.Append("<UploadConfig>");
            sb.Append("<Items>");
            foreach (KeyValuePair<string, UploadItemConfig> Item in this.UploadItems)
            {
                sb.Append(Item.Value.ToConfig());
            }
            sb.Append("</Items>");
            sb.Append("</UploadConfig>");

            return sb.ToString();
        }

        /// <summary>
        /// 从配置文件构件对象
        /// </summary>
        /// <param name="Config"></param>
        /// <returns></returns>
        public UploadConfig FromConfig(string Config)
        {
            UploadConfig rInfo = new UploadConfig();

            if (Config.IsEmpty())
                return rInfo;

            IList<string> _Tags = Config.GetTag("Items").GetTags("Item");

            foreach (string Item in _Tags)
                rInfo.UploadItems.Add(Item.GetTag("Key"),new UploadItemConfig().FromConfig(Item.GetTag("Item")));

            return rInfo;
        }

        #endregion
    }

    public class UploadItemConfig : IConfigBase<UploadItemConfig>
    {
        public string Key { get; set; }
        public long MaxSize { get; set; }
        public string AllowedExt { get; set; }
        public string UpDirectory { get; set; }
        public ImageZoomConfig ImageZoomConfig { get; set; }

        public string ToConfig()
        {
            StringBuilder sb = new StringBuilder();

            sb.Append("<Item>");
            sb.Append("<Key>");
            sb.Append(Key);
            sb.Append("</Key>");
            sb.Append("<MaxSize>");
            sb.Append(MaxSize.ToString());
            sb.Append("</MaxSize>");
            sb.Append("<AllowedExt>");
            sb.Append(AllowedExt);
            sb.Append("</AllowedExt>");
            sb.Append("<UpDirectory>");
            sb.Append(UpDirectory);
            sb.Append("</UpDirectory>");
            sb.Append("<ImageZoomConfig>");
            sb.Append(ImageZoomConfig.ToConfig());
            sb.Append("</ImageZoomConfig>");
            
            sb.Append("</Item>");
            return sb.ToString();
        }

        public UploadItemConfig FromConfig(string Config)
        {
            UploadItemConfig rInfo = new UploadItemConfig();
            if (Config.IsEmpty())
                return rInfo;

            rInfo.Key = Config.GetTag("Key");
            rInfo.MaxSize = Config.GetTag("MaxSize").LongByString();
            rInfo.AllowedExt = Config.GetTag("AllowedExt");
            rInfo.UpDirectory = Config.GetTag("UpDirectory");
            rInfo.ImageZoomConfig = new ImageZoomConfig().FromConfig(Config.GetTag("ImageZoomConfig"));
            return rInfo;

        }
    }

    public class ImageZoomConfig : IConfigBase<ImageZoomConfig>
    {
        private IDictionary<string, ImageZoomItemConfig> _ImageZoomItems;
        /// <summary>
        /// 图片缩放项配置
        /// </summary>
        public IDictionary<string, ImageZoomItemConfig> ImageZoomItems
        {
            get
            {
                if (this._ImageZoomItems == null)
                    this._ImageZoomItems = new Dictionary<string, ImageZoomItemConfig>();

                return this._ImageZoomItems;
            }
        }

        #region IConfigBase<UploadConfig> 成员
        /// <summary>
        /// 转换为配置文件
        /// </summary>
        /// <returns></returns>
        public string ToConfig()
        {
            StringBuilder sb = new StringBuilder();
            sb.Append("<ImageZoomConfig>");
            foreach (KeyValuePair<string, ImageZoomItemConfig> Item in this.ImageZoomItems)
            {
                sb.Append(Item.Value.ToConfig());
            }
            sb.Append("</ImageZoomConfig>");

            return sb.ToString();
        }

        /// <summary>
        /// 从配置文件构件对象
        /// </summary>
        /// <param name="Config"></param>
        /// <returns></returns>
        public ImageZoomConfig FromConfig(string Config)
        {
            ImageZoomConfig rInfo = new ImageZoomConfig();

            if (Config.IsEmpty())
                return rInfo;

            IList<string> _Tags = Config.GetTags("ImageZoomItem");

            foreach (string Item in _Tags)
                rInfo.ImageZoomItems.Add(Item.GetTag("Name"), new ImageZoomItemConfig().FromConfig(Item.GetTag("ImageZoomItem")));

            return rInfo;
        }

        #endregion
    }

    public class ImageZoomItemConfig : IConfigBase<ImageZoomItemConfig>
    {
        public string Name { get; set; }
        public int Width { get; set; }
        public int Height { get; set; }

        public string ToConfig()
        {
            StringBuilder sb = new StringBuilder();

            sb.Append("<ImageZoomItem>");
            sb.Append("<Name>");
            sb.Append(Name);
            sb.Append("</Name>");
            sb.Append("<Width>");
            sb.Append(Width.ToString());
            sb.Append("</Width>");
            sb.Append("<Height>");
            sb.Append(Height.ToString());
            sb.Append("</Height>");
            sb.Append("</ImageZoomItem>");

            return sb.ToString();
        }

        public ImageZoomItemConfig FromConfig(string Config)
        {
            ImageZoomItemConfig rInfo = new ImageZoomItemConfig();

            if (Config.IsEmpty())
                return rInfo;

            rInfo.Name = Config.GetTag("Name");
            rInfo.Width = Config.GetTag("Width").IntByString();
            rInfo.Height = Config.GetTag("Height").IntByString();

            return rInfo;

        }
    }
}