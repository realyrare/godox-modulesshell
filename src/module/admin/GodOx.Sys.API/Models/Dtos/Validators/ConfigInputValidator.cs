using FluentValidation;
using GodOx.Sys.API.Models.Dtos.Input;

namespace GodOx.Sys.API.Models.Dtos.Validators
{
    public class ConfigInputValidator : AbstractValidator<ConfigInput>
    {
        public ConfigInputValidator()
        {
            RuleFor(x => x.Name).NotEmpty().WithMessage("名称必须填写");
            RuleFor(x => x.Type).NotEmpty().WithMessage("类型必须填写");
        }
    }
}
