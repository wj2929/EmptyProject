using AutoMapper;
using EmptyProject.Domain;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Web;

namespace EmptyProject.Service.Code
{
    public class GlobalController
    {
        public static string UploadXlsFile()
        {
            if (System.Web.HttpContext.Current.Request.Files.Count != 0)
            {
                var file = System.Web.HttpContext.Current.Request.Files[0];
                if (file.ContentLength == 0)
                    throw new Exception("文件不能为空！");
                else if (!new string[]{".xls",".xlsx"}.Contains(Path.GetExtension(file.FileName).ToLower()))
                    throw new Exception(string.Format("文件扩展名要求{0}", ".xls"));
                else
                {
                    string UploadPath = Path.Combine(Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "upfile"), DateTime.Now.ToString("yyyy/MM/dd").Replace("/", "\\"));
                    if (!Directory.Exists(UploadPath))
                        Directory.CreateDirectory(UploadPath);
                    string FileNameWithOutExt = DateTime.Now.Ticks.ToString();
                    string FileExt = Path.GetExtension(file.FileName).ToLower();
                    string SourFileName = Path.GetFileName(file.FileName).Split('.')[0];
                    string UploadFile = Path.Combine(UploadPath, string.Concat(FileNameWithOutExt, FileExt));
                    file.SaveAs(UploadFile);

                    return UploadFile;
                }
            }
            else
                throw new Exception("请上传文件！");
        }

        public static void InjectCustomMap()
        {
            Mapper.Initialize(cfg =>
            {
                cfg.CreateMissingTypeMaps = true;
            });
        }
    }
}