using GodOx.Auth.API.Models.Dtos.Output;

namespace GodOx.Auth.API
{
    /// <summary>
    /// 当前用户上下文
    /// </summary>
    public class CurrentAppContext
    {
        public CurrentAppContext(HttpWxUserOutput wxUserOutput)
        {
            WxUser = wxUserOutput;
        }
        public static HttpWxUserOutput WxUser { get; set; }

    }
}
