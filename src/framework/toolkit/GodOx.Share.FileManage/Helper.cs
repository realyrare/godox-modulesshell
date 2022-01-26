using Microsoft.AspNetCore.Http;
using System;
using System.IO;
using System.Security.Cryptography;

namespace GodOx.Share.FileManage
{
    public class Helper
    {
        /// <summary>
        /// 图片后缀是否存在
        /// </summary>
        /// <param name="file"></param>
        /// <returns></returns>
        public static string ImgSuffixIsExists(IFormFile file)
        {
            string fileExt = file.FileName.Split('.')[file.FileName.Split('.').Length - 1];
            if (fileExt == null)
            {
                throw new ArgumentNullException("上传的文件没有后缀");
            }
            //判断文件大小    
            long length = file.Length;
            if (length > 1024 * 1024 * 2) //2M
            {
                throw new ArgumentNullException("上传的文件不能大于2M");
            }
            string imgTypes = ".gif|.jpg|.php|.jsp|.jpeg|.png|......";
            if (imgTypes.IndexOf(fileExt.ToLower(), StringComparison.Ordinal) <= -1)
            {
                throw new ArgumentNullException("上传的文件不是图片");
            }
            string filename = GetStreamMd5(file.OpenReadStream()) + "." + fileExt;
            return filename;
        }
        /// <summary>
        /// 计算文件的MD5值
        /// </summary>
        /// <param name="filepath"></param>
        /// <returns></returns>
        public static string GetStreamMd5(Stream stream)
        {
            var oMd5Hasher = new MD5CryptoServiceProvider();
            byte[] arrbytHashValue = oMd5Hasher.ComputeHash(stream);
            //由以连字符分隔的十六进制对构成的String，其中每一对表示value 中对应的元素；例如“F-2C-4A”
            string strHashData = BitConverter.ToString(arrbytHashValue);
            //替换-
            strHashData = strHashData.Replace("-", "");
            return strHashData;
        }
    }
}
