using FluentValidation;
using RestaurantAPI.Dtos.ContactDtos;

namespace RestaurantAPI.ValidationRules
{
    public class UpdateContactValidator : AbstractValidator<UpdateContactDto>
    {
        public UpdateContactValidator()
        {
            RuleFor(x => x.ContactMapLocation)
                .NotEmpty().WithMessage("Harita konumu boş geçilemez");

            RuleFor(x => x.ContactAddress)
                .NotEmpty().WithMessage("Adres boş geçilemez")
                .MaximumLength(500).WithMessage("Adres en fazla 500 karakter olabilir");

            RuleFor(x => x.ContactPhone)
               .NotEmpty().WithMessage("Telefon numarası boş geçilemez")
               .MaximumLength(20).WithMessage("Telefon numarası en fazla 20 karakter olabilir")
               .Matches(@"^\d+$").WithMessage("Telefon numarası sadece rakamlardan oluşmalıdır");

            RuleFor(x => x.ContactEmail)
                .NotEmpty().WithMessage("E-posta adresi boş geçilemez")
                .MaximumLength(100).WithMessage("E-posta adresi en fazla 100 karakter olabilir")
                .EmailAddress().WithMessage("Geçerli bir e-posta adresi giriniz");

            RuleFor(x => x.ContactOpenHours)
                .NotEmpty().WithMessage("Çalışma saatleri boş geçilemez")
                .MaximumLength(50).WithMessage("Çalışma saatleri en fazla 50 karakter olabilir");
        }
    }
}