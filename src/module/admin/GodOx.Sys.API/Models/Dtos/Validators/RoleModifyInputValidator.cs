using FluentValidation;
using GodOx.Sys.API.Models.Dtos.Input;

namespace GodOx.Sys.API.Models.Dtos.Validators
{
    public class RoleModifyInputValidator : AbstractValidator<RoleModifyInput>
    {
        public RoleModifyInputValidator()
        {
            RuleFor(x => x.Id).NotEmpty().WithMessage("角色Id必须传递");
            RuleFor(x => x.Name).NotEmpty().WithMessage("请填写角色名称");
            RuleFor(x => x.Description).NotEmpty().WithMessage("请填写角色备注");
        }
    }
}
