using FluentValidation;
using GodOx.Sys.API.Models.Dtos.Input;

namespace GodOx.Sys.API.Models.Dtos.Validators
{
    public class LoginInputValidator : AbstractValidator<LoginInput>
    {
        public LoginInputValidator()
        {
            // CascadeMode = CascadeMode.StopOnFirstFailure;
            RuleFor(x => x.LoginName).NotEmpty().WithMessage("请填写用户名称");
            RuleFor(x => x.Password).NotEmpty().WithMessage("请填写用户密码");
            RuleFor(x => x.NumberGuid).NotEmpty().WithMessage("用户编号必须传递");
        }
    }
}
