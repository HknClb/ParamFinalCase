using Core.Security.Dtos;
using FluentValidation;

namespace Identity.Api.Validators
{
    public class SignUpValidator : AbstractValidator<UserSignUpDto>
    {
        public SignUpValidator()
        {
            RuleFor(x => x.Email)
                .NotEmpty()
                .WithMessage("The email is required");

            RuleFor(x => x.Email)
                .EmailAddress()
                .WithMessage("The email is not valid");

            RuleFor(x => x.Password)
                .NotEmpty()
                .WithMessage("The password is required");

            RuleFor(x => x.Password)
                .Matches(x => x.ConfirmPassword)
                .WithMessage("Password and Confirm Password are not matching");
        }
    }
}
