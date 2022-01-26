using FluentValidation;
using GodOx.Sys.API.Models.Dtos.Input;

namespace GodOx.Sys.API.Models.Dtos.Validators
{
    public class MenuModifyInputValidator : AbstractValidator<MenuModifyInput>
    {
        public MenuModifyInputValidator()
        {
            RuleFor(x => x.Id).NotEmpty().WithMessage("菜单名称必须填写");
            RuleFor(x => x.Name).NotEmpty().WithMessage("菜单名称必须填写");
            // RuleFor(x => x.Url).NotEmpty().WithMessage("Url必须填写");
            RuleFor(x => x.HttpMethod).NotEmpty().WithMessage("HttpMethod必须填写");
            RuleFor(x => x.Icon).NotEmpty().WithMessage("菜单图标必须填写");
            RuleFor(x => x.NameCode).NotEmpty().WithMessage("菜单唯一码必须填写");
        }
    }
}
