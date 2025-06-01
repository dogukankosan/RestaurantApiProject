using FluentValidation;
using RestaurantAPI.Dtos.ReservationDtos;

namespace RestaurantAPI.ValidationRules
{
    public class UpdateReservationValidator : AbstractValidator<UpdateReservationDto>
    {
        public UpdateReservationValidator()
        {
            RuleFor(x => x.ReservationNameSurname)
                .NotEmpty().WithMessage("Ad soyad boş geçilemez")
                .MaximumLength(100).WithMessage("Ad soyad en fazla 100 karakter olabilir");

            RuleFor(x => x.ReservationEmail)
                .NotEmpty().WithMessage("E-posta boş geçilemez")
                .MaximumLength(100).WithMessage("E-posta en fazla 100 karakter olabilir")
                .EmailAddress().WithMessage("Geçerli bir e-posta adresi giriniz");

            RuleFor(x => x.ReservationPhone)
                .NotEmpty().WithMessage("Telefon numarası boş geçilemez")
                .MaximumLength(20).WithMessage("Telefon numarası en fazla 20 karakter olabilir")
                .Matches(@"^\d+$").WithMessage("Telefon numarası sadece rakamlardan oluşmalıdır");

            RuleFor(x => x.ReservationDate)
                .GreaterThanOrEqualTo(DateTime.Now).WithMessage("Geçmiş tarihe rezervasyon yapılamaz");

            RuleFor(x => x.ReservationCountOfPeople)
                .GreaterThan(0).WithMessage("Kişi sayısı 0'dan büyük olmalıdır");
        }
    }
}