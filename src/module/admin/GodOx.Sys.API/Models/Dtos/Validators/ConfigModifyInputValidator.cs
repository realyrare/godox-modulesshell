using FluentValidation;
using GodOx.Sys.API.Models.Dtos.Input;

namespace GodOx.Sys.API.Models.Dtos.Validators
{
    public class ConfigModifyInputValidator : AbstractValidator<ConfigModifyInput>
    {
        public ConfigModifyInputValidator()
        {
            RuleFor(x => x.Id).NotEmpty().WithMessage("Id必须填写");
            RuleFor(x => x.Name).NotEmpty().WithMessage("名称必须填写");
            RuleFor(x => x.Type).NotEmpty().WithMessage("类型必须填写");
        }
    }
}
