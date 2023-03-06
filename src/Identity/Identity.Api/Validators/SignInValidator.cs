using Core.Security.Dtos;
using FluentValidation;

namespace Identity.Api.Validators
{
    public class SignInValidator : AbstractValidator<UserSignInDto>
    {
        public SignInValidator()
        {
            RuleFor(x => x.UserNameOrEmail)
                .NotEmpty()
                .WithMessage("The user name or email is required");

            RuleFor(x => x.Password)
                .NotEmpty()
                .WithMessage("The password is required");
        }
    }
}
