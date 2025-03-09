using FluentValidation;
using SmartGym.Core.Application.Abstraction.Models.Auth;

namespace SmartGym.Core.Application.Abstraction.Validators.Auth
{
    public class SignUpRequestValidator : AbstractValidator<SignUpRequest>
    {
        public SignUpRequestValidator()
        {
            RuleFor(x => x.Email)
                .NotEmpty()
                .EmailAddress();

            RuleFor(x => x.Password)
               .NotEmpty();

            RuleFor(x => x.FirstName)
                .NotEmpty()
                .Length(3, 20);

            RuleFor(x => x.LastName)
               .NotEmpty()
               .Length(3, 20);

            RuleFor(x => x.Gender)
               .NotEmpty()
               .Must(g => g.Equals("Male", StringComparison.OrdinalIgnoreCase) ||
                          g.Equals("Female", StringComparison.OrdinalIgnoreCase))
               .WithMessage("Gender must be either 'Male' or 'Female'.");

            RuleFor(x => x.BirthDay)
                .NotEmpty();

            RuleFor(x => x.Height)
                .GreaterThan(50);

            RuleFor(x => x.Weight)
                .GreaterThan(50);
        }
    }
}
