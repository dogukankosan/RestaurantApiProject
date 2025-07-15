using FluentValidation;
using RestaurantAPI.Dtos.EmailDtos;

namespace RestaurantAPI.ValidationRules
{
    public class UpdateEMailValidator : AbstractValidator<UpdateEmailDto>
    {
        public UpdateEMailValidator()
        {
            RuleFor(x => x.EmailBox)
                .NotEmpty().WithMessage("E-posta adresi boş olamaz.")
                .MaximumLength(100).WithMessage("E-posta adresi en fazla 100 karakter olabilir.")
                .EmailAddress().WithMessage("Geçerli bir e-posta adresi giriniz.");

            RuleFor(x => x.EmailPassword)
                .NotEmpty().WithMessage("Şifre boş olamaz.")
                .MaximumLength(100).WithMessage("Şifre en fazla 100 karakter olabilir.");

            RuleFor(x => x.EmailServer)
            .NotEmpty().WithMessage("Sunucu boş olamaz.")
            .MaximumLength(100).WithMessage("Sunucu adı en fazla 100 karakter olabilir.")
            .Matches(@"^[a-zA-Z0-9.-]+\.[a-zA-Z]{2,}$")
            .WithMessage("Geçerli bir sunucu adresi giriniz (örn: smtp.domain.com).");


            RuleFor(x => x.EmailPort)
                .InclusiveBetween(0, 65535).WithMessage("Geçerli bir SSL port numarası giriniz.");

            RuleFor(x => x.EmailCompanyName)
                .MaximumLength(100).WithMessage("Şirket adı en fazla 100 karakter olabilir.");

            RuleFor(x => x.EmailAddress)
                .MaximumLength(200).WithMessage("Adres en fazla 200 karakter olabilir.");

            RuleFor(x => x.EmailImage)
                .Must(img => img == null || img.Length <= 2 * 1024 * 1024)
                .WithMessage("Görsel en fazla 2MB olabilir.");

        }
    }
}