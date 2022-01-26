using FluentValidation;
using GodOx.Sys.API.Models.Dtos.Input;

namespace GodOx.Sys.API.Models.Dtos.Validators
{
    public class SetUserRoleInputValidator : AbstractValidator<SetUserRoleInput>
    {
        public SetUserRoleInputValidator()
        {
            RuleFor(x => x.UserId).NotEmpty().WithMessage("用户id必须填写");
            RuleFor(x => x.RoleId).NotEmpty().WithMessage("角色id必须填写一个");

        }
    }
}
