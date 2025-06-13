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
                .NotEmpty().WithMessage("Ad soyad boş geçilemez")
                .Must(name => !string.IsNullOrWhiteSpace(name))
                    .WithMessage("Ad soyad yalnızca boşluklardan oluşamaz")
                .Must(ValidatorMethod.BeValidName)
                    .WithMessage("Ad ve soyad arasında en az iki parça olmalı (ör. \"Ahmet Yılmaz\")")
                .MaximumLength(100).WithMessage("Ad soyad en fazla 100 karakter olabilir");

            RuleFor(x => x.ReservationEmail)
                .NotEmpty().WithMessage("E-posta boş geçilemez")
                .MaximumLength(100).WithMessage("E-posta en fazla 100 karakter olabilir")
                .EmailAddress().WithMessage("Geçerli bir e-posta adresi giriniz");

            RuleFor(x => x.ReservationPhone)
                .NotEmpty().WithMessage("Telefon numarası boş geçilemez")
                .MaximumLength(20).WithMessage("Telefon numarası en fazla 20 karakter olabilir")
                .Matches(@"^\+?\d{7,20}$")
                    .WithMessage("Telefon numarası yalnızca rakamlardan oluşmalı, isteğe bağlı başında + olabilir");

            RuleFor(x => x.ReservationDate)
                .NotNull().WithMessage("Rezervasyon tarihi zorunlu")
                .Must(date => date.ToUniversalTime() >= DateTime.UtcNow)
                .WithMessage("Geçmiş tarihe rezervasyon yapılamaz");

            RuleFor(x => x.ReservationCountOfPeople)
                .GreaterThan(0).WithMessage("Kişi sayısı 0'dan büyük olmalıdır")
                .LessThanOrEqualTo(100).WithMessage("En fazla 100 kişilik rezervasyon yapılabilir");

            RuleFor(x => x.ReservationMessage)
                .MaximumLength(500).WithMessage("Mesaj en fazla 500 karakter olabilir")
                .When(x => !string.IsNullOrWhiteSpace(x.ReservationMessage));
        }
    }
}