using FluentValidation;
using RestaurantAPI.Dtos.IconDtos;

namespace RestaurantAPI.ValidationRules
{
    public class CreateIconValidator_ : AbstractValidator<CreateIconDto>
    {
        public class CreateIconValidator : AbstractValidator<CreateIconDto>
        {
            public CreateIconValidator()
            {
                RuleFor(x => x.IconURL)
                    .NotEmpty().WithMessage("İkon alanı boş geçilemez.")
                    .Must(url => !string.IsNullOrWhiteSpace(url)).WithMessage("İkon yalnızca boşluklardan oluşamaz.")
                    .MinimumLength(3).WithMessage("İkon adı en az 3 karakter olmalıdır.")
                    .MaximumLength(100).WithMessage("İkon adı en fazla 100 karakter olabilir.")
                    .Must(url => url.StartsWith("bi ")).WithMessage("İkon 'bi ' ile başlamalıdır. Örn: bi bi-chat-dots");
            }
        }
    }
}