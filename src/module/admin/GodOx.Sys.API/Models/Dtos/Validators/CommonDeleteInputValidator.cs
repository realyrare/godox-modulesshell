using FluentValidation;
using GodOx.Sys.API.Models.Dtos.Common;

namespace GodOx.Sys.API.Models.Dtos.Validators
{
    public class CommonDeleteInputValidator : AbstractValidator<DeletesInput>
    {
        public CommonDeleteInputValidator()
        {
            RuleFor(x => x.Ids.Count).NotEmpty().WithMessage("Id必须传递");
        }
    }
}
