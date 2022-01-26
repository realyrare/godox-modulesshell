/*************************************
* 类名：AppUserLoginInput
* 作者：realyrare
* 邮箱：mahonggang8888@126.com
* 时间：2021/8/27 16:40:57
*┌───────────────────────────────────┐　    
*│　   版权所有：神牛软件　　　　	     │
*└───────────────────────────────────┘
**************************************/

namespace GodOx.Auth.API.Models.Dtos.Input
{
    public class AppUserLoginInput
    {
        public int TenantId { get; set; }
        public string Token { get; set; }
        public string Code { get; set; }

        public string UserInfo { get; set; }

        public string EncryptedData { get; set; }

        public string Iv { get; set; }

        public string Signature { get; set; }
    }
}