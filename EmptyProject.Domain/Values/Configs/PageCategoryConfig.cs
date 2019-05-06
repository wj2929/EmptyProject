//using BC.Core;
//using System;
//using System.Collections.Generic;
//using System.Linq;
//using System.Text;
//using System.Threading.Tasks;

//namespace EmptyProject.Domain.Values.Configs
//{
//    public class PageCategoryConfig : IConfigBase<PageCategoryConfig>
//    {
//        private IList<PageCategoryItemConfig> _PageCategoryItems;
//        /// <summary>
//        /// 
//        /// </summary>
//        public IList<PageCategoryItemConfig> PageCategoryItems
//        {
//            get
//            {
//                if (this._PageCategoryItems == null)
//                    this._PageCategoryItems = new List<PageCategoryItemConfig>();

//                return this._PageCategoryItems;
//            }
//        }

//        /// <summary>
//        /// 转换为配置文件
//        /// </summary>
//        /// <returns></returns>
//        public string ToConfig()
//        {
//            StringBuilder sb = new StringBuilder();
//            sb.Append("<TestQuestionConfig>");
//            sb.Append("<Items>");
//            foreach (var Item in this.TestQuestionItems)
//            {
//                sb.Append(Item.ToConfig());
//            }
//            sb.Append("</Items>");
//            sb.Append("</TestQuestionConfig>");

//            return sb.ToString();
//        }

//        /// <summary>
//        /// 从配置文件构件对象
//        /// </summary>
//        /// <param name="Config"></param>
//        /// <returns></returns>
//        public TestQuestionConfig FromConfig(string Config)
//        {
//            TestQuestionConfig rInfo = new TestQuestionConfig();

//            if (Config.IsEmpty())
//                return rInfo;

//            IList<string> _Tags = Config.GetTag("Items").GetTags("Item");

//            foreach (string Item in _Tags)
//                rInfo.TestQuestionItems.Add(new TestQuestionItemConfig().FromConfig(Item.GetTag("Item")));

//            return rInfo;
//        }

//    }

//    public class PageCategoryItemConfig : IConfigBase<PageCategoryItemConfig>
//    {
//        /// <summary>
//        /// Name
//        /// </summary>
//        public string Title { get; set; }

//        public Guid Id { get; set; }

//        public string ToConfig()
//        {
//            StringBuilder sb = new StringBuilder();

//            sb.Append("<Item>");
//            sb.Append("<Title>");
//            sb.Append(Title);
//            sb.Append("</Title>");
//            sb.Append("<Content>");
//            sb.Append(Content);
//            sb.Append("</Content>");
//            sb.Append("<AnswerSecond>");
//            sb.Append(AnswerSecond);
//            sb.Append("</AnswerSecond>");
//            sb.Append("<SpeechSoundUrl>");
//            sb.Append(SpeechSoundUrl);
//            sb.Append("</SpeechSoundUrl>");
//            sb.Append("<BackgroundMusicUrl>");
//            sb.Append(BackgroundMusicUrl);
//            sb.Append("</BackgroundMusicUrl>");
//            sb.Append("<Score>");
//            sb.Append(Score);
//            sb.Append("</Score>");
//            sb.Append("<Type>");
//            sb.Append(Type);
//            sb.Append("</Type>");
//            sb.Append("<CategoryName>");
//            sb.Append(CategoryName);
//            sb.Append("</CategoryName>");
//            sb.Append("<Anwser>");
//            sb.Append(Anwser);
//            sb.Append("</Anwser>");
//            sb.Append("<QuestionParse>");
//            sb.Append(QuestionParse);
//            sb.Append("</QuestionParse>");
//            sb.Append("</Item>");
//            return sb.ToString();
//        }

//        public PageCategoryItemConfig FromConfig(string Config)
//        {
//            PageCategoryItemConfig rInfo = new PageCategoryItemConfig();
//            if (Config.IsEmpty())
//                return rInfo;

//            rInfo.Title = Config.GetTag("Title").Trim();
//            rInfo.Content = Config.GetTag("Content").Trim();
//            rInfo.AnswerSecond = int.Parse(Config.GetTag("AnswerSecond").Trim());
//            rInfo.SpeechSoundUrl = Config.GetTag("SpeechSoundUrl").Trim();
//            rInfo.BackgroundMusicUrl = Config.GetTag("BackgroundMusicUrl").Trim();
//            rInfo.Score = int.Parse(Config.GetTag("Score").Trim());
//            rInfo.Type = int.Parse(Config.GetTag("Type").Trim());
//            rInfo.CategoryName = Config.GetTag("CategoryName").Trim();
//            rInfo.Anwser = Config.GetTag("Anwser").Trim();
//            rInfo.QuestionParse = Config.GetTag("QuestionParse").Trim();
//            return rInfo;

//        }

//    }
//}
