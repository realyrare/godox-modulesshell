using FluentValidation;
using GodOx.Cms.API.Models.Dtos.Input;

/*************************************
* 类 名： ContentHrefInputValidator
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
    public class ContentHrefInputValidator : AbstractValidator<ContentHrefInput>
    {
        public ContentHrefInputValidator()
        {
            RuleFor(x => x.Content).NotEmpty().WithMessage("标题必须填写");
        }
    }
}