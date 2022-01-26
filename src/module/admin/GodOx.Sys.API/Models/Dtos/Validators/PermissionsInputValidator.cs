using FluentValidation;
using GodOx.Sys.API.Models.Dtos.Input;

namespace GodOx.Sys.API.Models.Dtos.Validators
{
    public class PermissionsInputValidator : AbstractValidator<PermissionsInput>
    {
        public PermissionsInputValidator()
        {
            RuleFor(x => x.RoleId).NotEmpty().WithMessage("角色Id必须传递");
            RuleFor(x => x.MenuId).NotEmpty().WithMessage("菜单Id必须传递");
        }
    }
}
