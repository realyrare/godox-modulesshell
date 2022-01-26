using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.IO;

/*************************************
* 类 名： LocalFile
* 作 者： realyrare
* 邮 箱： mhg215@yeah.net
* 时 间： 2021/3/18 14:19:38
* .netV： 3.1
*┌───────────────────────────────────┐　    
*│　     版权所有：神牛软件　　　　	 │
*└───────────────────────────────────┘
**************************************/

namespace GodOx.Share.FileManage
{
    /// <summary>
    ///本地图片上传 使用的时候单独把类注入
    /// </summary>
    public class LocalFile : IUploadFile
    {

        public bool Delete(string filename)
        {
            if (string.IsNullOrEmpty(filename))
            {
                throw new ArgumentNullException(filename);
            }
            throw new NotImplementedException();
        }

        public List<string> List(string prefix, string marker)
        {
            throw new NotImplementedException();
        }

        public string Upload(IFormFile file, string prefix)
        {
            var path = CreateDirectory(prefix);
            //var file = Request.Form.Files[0];
            var fileName = Helper.ImgSuffixIsExists(file);
            string fileFullName = path + "\\" + fileName;
            using (FileStream fs = File.Create(fileFullName))
            {
                file.CopyTo(fs);
                fs.Flush();
            }
            return Path.Combine($"/Files/{prefix}/", fileName);
        }

        public List<string> Upload(IFormFileCollection files, string prefix)
        {
            var path = CreateDirectory(prefix);
            List<string> list = new List<string>();
            foreach (var file in files)
            {
                var fileName = Helper.ImgSuffixIsExists(file);
                string fileFullName = Path.Combine(path, fileName);
                using (FileStream fs = File.Create(fileFullName))
                {
                    file.CopyTo(fs);
                    fs.Flush();
                }
                list.Add(Path.Combine($"/Files/{prefix}", fileName));

            }
            return list;
        }

        private string CreateDirectory(string prefix)
        {
            /*在windos平台中，path结尾可以包含“\”字符，
           但在linux中则会出问题，会将‘\’字符作为文件夹名称的一部分。*/
            string path;
            if (!string.IsNullOrEmpty(prefix))
            {
                path = string.Concat("wwwroot", $"/Files/{prefix}");
            }
            else
            {
                path = string.Concat("wwwroot", "/Files");
            }
            if (!Directory.Exists(path))
            {
                Directory.CreateDirectory(path);
            }
            return path;
        }
    }
}