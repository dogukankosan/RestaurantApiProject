using FluentValidation;
using RestaurantAPI.Dtos.ChefDtos;
namespace RestaurantAPI.ValidationRules
{
    public class CreateChefValidator : AbstractValidator<CreateChefDto>
    {
        public CreateChefValidator()
        {
            RuleFor(x => x.ChefNameSurname)
                .NotEmpty().WithMessage("Şef adı soyadı boş geçilemez")
                .MaximumLength(100).WithMessage("Şef adı soyadı en fazla 100 karakter olabilir");

            RuleFor(x => x.ChefTitle)
                .NotEmpty().WithMessage("Şef unvanı boş geçilemez")
                .MaximumLength(200).WithMessage("Şef unvanı en fazla 200 karakter olabilir");
        }
    }
}