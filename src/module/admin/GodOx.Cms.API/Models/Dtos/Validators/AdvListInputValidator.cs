using FluentValidation;
using GodOx.Cms.API.Models.Dtos.Input;

/*************************************
* 类 名： AdvListInputValidator
* 作 者： realyrare
* 邮 箱： mahonggang8888@126.com
* 时 间： 2021/3/16 17:35:06
* .netV： 3.1
*┌───────────────────────────────────┐　    
*│　     版权所有：神牛软件　　　　	 │
*└───────────────────────────────────┘
**************************************/

namespace GodOx.Cms.API.Models.Dtos.Validators
{
    public class AdvListInputValidator : AbstractValidator<AdvListInput>
    {
        public AdvListInputValidator()
        {
            RuleFor(x => x.Title).NotEmpty().WithMessage("标题必须填写");
            RuleFor(x => x.Target).NotEmpty().WithMessage("跳转方式必须填写");
            //RuleFor(x => x.Type).NotEmpty().GreaterThan(0).WithMessage("类型必须填写");
            RuleFor(x => x.ImgUrl).NotEmpty().WithMessage("图片地址必须填写");
        }
    }
}