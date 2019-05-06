using System;
using System.Data;
using System.Configuration;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Web;
using System.Web.Security;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Web.UI.WebControls.WebParts;
using System.Web.UI.HtmlControls;

using EmptyProject.DomainService.Interface;
using EmptyProject.Manage.Code;
using EmptyProject.Domain;
using System.Linq;
using EmptyProject.Core.Json;
using EmptyProject.Domain.Values.Configs;

namespace EmptyProject.Manage
{
    public partial class Upload_Ajax : System.Web.UI.Page
    {
        public List<string[]> AuList = new List<string[]>();
        private string AttachmentType;
        private UploadItemConfig UploadItemConfigInfo;


        protected void Page_Load(object sender, EventArgs e)
        {
            if (Request.Files.Count > 0)
            {
                AttachmentType = Request["AttachmentType"];
                string msg = "";
                string error = "";
                if (!AttachmentType.IsEmpty() && GlobalController.UploadConfig.ConfigEntity.UploadItems.ContainsKey(AttachmentType))
                {
                    UploadItemConfigInfo = GlobalController.UploadConfig.ConfigEntity.UploadItems[AttachmentType];
                    HttpPostedFile file = Request.Files[0];
                    if (file.ContentLength == 0)
                        error = "文件长度为0";
                    else
                    {
                        string size = string.Empty;
                        string filename = string.Empty;
                        string type = string.Empty;
                        BC.Core.BaseReturnInfo ReturnInfo = Upload_File(string.Format("{0}/{1}/{2}/{3}/", UploadItemConfigInfo.UpDirectory, DateTime.Now.Year, DateTime.Now.Month, DateTime.Now.Day), file);
                        if (!ReturnInfo.State)
                            error = ReturnInfo.Message;
                        else
                        {
                            Attachment AttachmentInfo = ReturnInfo.DataObject as Attachment;
                            JsonReturnInfo jsonInfo = new JsonReturnInfo();
                            jsonInfo.Values.Add("Id", AttachmentInfo.Id.ToString());
                            jsonInfo.Values.Add("Url", AttachmentInfo.Url);
                            msg = jsonInfo.ToJson();
                        }
                    }
                }
                else
                {
                    error = "上传类型不能为空或未设置该上传类型！";
                    msg = "''";
                }
                string result = "{ error:'" + error + "', msg:" + msg + "}";
                Response.Write(result);
                Response.End();
            }

        }
        public BC.Core.BaseReturnInfo Upload_File(string savePath, HttpPostedFile PostedFile)
        {

            string filepath = string.Empty;//文件保存路径

            string name = PostedFile.FileName.Substring(PostedFile.FileName.LastIndexOf('\\') + 1);//获取文件名

            string fileName = Path.GetFileName(PostedFile.FileName);//记录文件的名字
            string contentType = PostedFile.ContentType;//上传文件类型
            string fileSize = PostedFile.ContentLength.ToString();//上传文件大小
            string fileExt = name.Substring(name.LastIndexOf(".") + 1);//上传文件后缀名
            string tpname = fileName.Split('.')[0];

            string saveFileName = string.Format("{0}.{1}", DateTime.Now.Ticks.ToString(), fileExt);
            string localPath = HttpContext.Current.Server.MapPath(savePath);
            if (!Directory.Exists(localPath))
                Directory.CreateDirectory(localPath);
            string saveFilePath = string.Format("{0}/{1}", localPath, saveFileName);

            if (PostedFile.ContentLength < UploadItemConfigInfo.MaxSize)
            {
                if (UploadItemConfigInfo.AllowedExt.ToLower().Contains(fileExt.ToLower())) //根据后缀名来限制上传类型
                {
                    PostedFile.SaveAs(saveFilePath);//上传文件到ipath这个路径里
                    IList<string> ChildFiles = new List<string>();
                    if (UploadItemConfigInfo.ImageZoomConfig.ImageZoomItems.Count > 0)
                    {
                        foreach (var ImageZoomItem in UploadItemConfigInfo.ImageZoomConfig.ImageZoomItems)
                        {
                            string ImageZoomFileName = string.Format("{0}_{1}.{2}", Path.GetFileNameWithoutExtension(saveFileName), ImageZoomItem.Value.Name, fileExt);

                            string imageZoomFilePath = string.Format("{0}/{1}", localPath, ImageZoomFileName);

                            ChildFiles.Add(savePath + ImageZoomFileName);

                            //ImageHelper.SmallPic(saveFilePath, imageZoomFilePath, ImageZoomItem.Value.Width, ImageZoomItem.Value.Height);
                        }
                    }

                    filepath = savePath + saveFileName;

                    IAttachmentDomainService AttachmentService = GlobalController.IoC.Resolve<IAttachmentDomainService>();
                    Attachment AttachmentInfo = AttachmentService.AddAttachment(new EmptyProject.Domain.Attachment
                    {
                        Default = false,
                        Directory = savePath,
                        Ext = fileExt,
                        FileName = tpname,
                        Size = int.Parse(fileSize),
                        Type = Request["AttachmentType"],
                        Url = filepath,
                        //ChildFiles = string.Join(",", ChildFiles.ToArray())
                    });
                    return new BC.Core.BaseReturnInfo
                    {
                        State = true,
                        DataObject = AttachmentInfo
                    };
                }
                else
                {
                    return new BC.Core.BaseReturnInfo
                    {
                        State = false,
                        Message = "不支持该格式"
                    };
                }
            }
            else
                return new BC.Core.BaseReturnInfo
                {
                    State = false,
                    Message = string.Format("最大上传文件大小：{0}", formatSize(UploadItemConfigInfo.MaxSize))
                };
        }


        public string formatSize(long size)
        {
            if (size == 0) return "0";
            string[] sizetext = new string[] { " B", " KB", " MB", " GB", " TB", " PB" };
            int i = (int)Math.Floor(Math.Log(size, 1024));
            return Math.Round(size / Math.Pow(1024, i), 2).ToString() + sizetext[i];
        }
    }
}