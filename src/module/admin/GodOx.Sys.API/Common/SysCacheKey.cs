namespace GodOx.Sys.API.Common
{
    public class SysCacheKey
    {
        public const string CurrentTenant = "currentTenant";
        /// <summary>
        /// 用户登录非对称加密
        /// </summary>
        public const string loginRSACrypt = "loginRSACrypt";
        /// <summary>
        /// 当前用户拥有的所有权限去做校验
        /// </summary>
        public const string AuthMenu = "authMenu";

        public const string LoginKey = "loginKey";
        /// <summary>
        ///  密码密钥
        /// </summary>
        public const string EncryptKey = "fenfenlg_salt_SmTRx";
    }
}
