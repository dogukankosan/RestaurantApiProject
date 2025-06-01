using FluentValidation;
using RestaurantAPI.Dtos.ServiceDtos;

namespace RestaurantAPI.ValidationRules
{
    public class CreateServiceValidator : AbstractValidator<CreateServiceDto>
    {
        public CreateServiceValidator()
        {
            RuleFor(x => x.ServiceTitle)
                .NotEmpty().WithMessage("Servis başlığı boş geçilemez")
                .MaximumLength(100).WithMessage("Servis başlığı en fazla 100 karakter olabilir");

            RuleFor(x => x.ServiceDescription)
                .MaximumLength(1000).WithMessage("Servis açıklaması en fazla 1000 karakter olabilir");

            RuleFor(x => x.ServiceIconUrl)
                .NotEmpty().WithMessage("İkon URL boş geçilemez")
                .MaximumLength(200).WithMessage("İkon URL en fazla 200 karakter olabilir");
        }
    }
}