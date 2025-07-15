using FluentValidation;
using RestaurantAPI.Dtos.ReservationDtos;
using RestaurantAPI.Helpers;

namespace RestaurantAPI.ValidationRules
{
    public class CreateReservationValidator : AbstractValidator<CreateReservationDto>
    {
        public CreateReservationValidator()
        {
            RuleFor(x => x.ReservationNameSurname)
                .Cascade(CascadeMode.Stop)
                .NotEmpty().WithMessage("Ad soyad boş geçilemez.")
                .Must(name => !string.IsNullOrWhiteSpace(name)).WithMessage("Ad soyad yalnızca boşluklardan oluşamaz.")
                .Must(ValidatorMethod.BeValidName)
                    .WithMessage("Ad ve soyad arasında en az iki kelime olmalıdır. Örn: 'Ahmet Yılmaz'")
                .MaximumLength(100).WithMessage("Ad soyad en fazla 100 karakter olabilir.");

            RuleFor(x => x.ReservationEmail)
                .Cascade(CascadeMode.Stop)
                .NotEmpty().WithMessage("E-posta adresi boş geçilemez.")
                .MaximumLength(100).WithMessage("E-posta en fazla 100 karakter olabilir.")
                .EmailAddress().WithMessage("Geçerli bir e-posta adresi giriniz.");

            RuleFor(x => x.ReservationPhone)
                .Cascade(CascadeMode.Stop)
                .NotEmpty().WithMessage("Telefon numarası boş geçilemez.")
                .MaximumLength(25).WithMessage("Telefon numarası en fazla 25 karakter olabilir.")
                .Matches(@"^\+?[0-9 ]{7,25}$")
                    .WithMessage("Telefon yalnızca rakam ve boşluk içermeli, isteğe bağlı '+' ile başlayabilir.");

            RuleFor(x => x.ReservationDate)
            .Cascade(CascadeMode.Stop)
            .NotEmpty().WithMessage("Rezervasyon tarihi zorunludur.")
            .Must(date => date.ToUniversalTime() >= DateTime.UtcNow)
                .WithMessage("Geçmiş tarihli rezervasyon yapılamaz.")
            .Must(date =>
                date >= new DateTime(1900, 1, 1) &&
                date <= new DateTime(2079, 6, 6))
                .WithMessage("Tarih, sistemin desteklediği aralığın dışında (1900 - 2079).");

            RuleFor(x => x.ReservationCountOfPeople)
                .InclusiveBetween(1, 100)
                .WithMessage("Kişi sayısı 1 ile 100 arasında olmalıdır.");

            RuleFor(x => x.ReservationMessage)
                .MaximumLength(500)
                .WithMessage("Mesaj alanı en fazla 500 karakter olabilir.")
                .When(x => !string.IsNullOrWhiteSpace(x.ReservationMessage));
        }
    }
}