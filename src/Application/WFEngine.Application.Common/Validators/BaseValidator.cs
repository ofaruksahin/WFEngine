using FluentValidation;
using Microsoft.Extensions.Localization;
using WFEngine.Application.Common.Constants;

namespace WFEngine.Application.Common.Validators
{
    public abstract class BaseValidator<TModel> : AbstractValidator<TModel>
        where TModel : class
    {
        protected IStringLocalizer<ValidationMessageConstants> _l;

        protected BaseValidator(IStringLocalizer<ValidationMessageConstants> l)
        {
            _l = l;
        }
    }
}
