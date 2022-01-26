using FluentValidation;
using GodOx.Cms.API.Models.Dtos.Input;

/*************************************
* 类 名： ColumnInputValidator
* 作 者： realyrare
* 邮 箱： mahonggang8888@126.com
* 时 间： 2021/3/15 19:28:24
* .netV： 3.1
*┌───────────────────────────────────┐　    
*│　     版权所有：神牛软件　　　　	 │
*└───────────────────────────────────┘
**************************************/

namespace GodOx.Cms.API.Models.Dtos.Validators
{
    public class ColumnInputValidator : AbstractValidator<ColumnInput>
    {
        public ColumnInputValidator()
        {
            CascadeMode = CascadeMode.StopOnFirstFailure;
            RuleFor(x => x.Title).NotEmpty().WithMessage("标题必须填写");
            RuleFor(x => x.Keyword).NotEmpty().WithMessage("站点关键字必须填写");
            RuleFor(x => x.Summary).NotEmpty().WithMessage("站点描述必须填写");
        }
    }
}