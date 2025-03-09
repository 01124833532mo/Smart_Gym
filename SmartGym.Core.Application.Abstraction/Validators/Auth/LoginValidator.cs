using FluentValidation;
using SmartGym.Core.Application.Abstraction.Models.Auth;

namespace SmartGym.Core.Application.Abstraction.Validators.Auth
{
    public class LoginValidator : AbstractValidator<LoginDto>
    {
        public LoginValidator()
        {
            RuleFor(x => x.Email)
                .NotEmpty().WithMessage("Email Must Be Requered")
                .EmailAddress().WithMessage("Email Must Be As Email Address ");

            RuleFor(x => x.Password)
               .NotEmpty().WithMessage("Password Must Be Requered");
        }

    }
}
