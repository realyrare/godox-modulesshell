using FluentValidation;
using GodOx.Shop.API.Models.Dtos.Input;

/*************************************
* 类名：CategoryModifyInputValidator
* 作者：realyrare
* 邮箱：mahonggang8888@126.com
* 时间：2021/8/10 10:00:41
*┌───────────────────────────────────┐　    
*│　   版权所有：神牛软件　　　　	     │
*└───────────────────────────────────┘
**************************************/

namespace GodOx.Shop.API.Models.Dtos.Validators
{

    public class CategoryModifyInputValidator : AbstractValidator<CategoryModifyInput>
    {
        public CategoryModifyInputValidator()
        {
            CascadeMode = CascadeMode.StopOnFirstFailure;
            RuleFor(x => x.Id).NotNull().GreaterThan(0).WithMessage("分类id必须大于0");
            RuleFor(x => x.Name).NotEmpty().WithMessage("标题必须填写");
            RuleFor(x => x.ModifyTime).NotNull().WithMessage("创建时间必须填写");

        }
    }
}