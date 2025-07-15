using FluentValidation;
using RestaurantAPI.Dtos.EventDtos;
using RestaurantAPI.Helpers;
using System;
using System.Linq;

namespace RestaurantAPI.ValidationRules
{
    public class UpdateEventValidator: AbstractValidator<UpdateEventDto>
    {
        public UpdateEventValidator()
        {
            RuleFor(x => x.EventName)
                .NotEmpty().WithMessage("Etkinlik adı boş bırakılamaz.")
                .Length(3, 100).WithMessage("Etkinlik adı en az 3, en fazla 100 karakter olmalıdır.");

            RuleFor(x => x.EventPrice)
                .GreaterThan(0).WithMessage("Fiyat 0'dan büyük olmalıdır.")
                .LessThanOrEqualTo(999_999).WithMessage("Fiyat 999999'dan büyük olamaz.");

            RuleFor(x => x.EventDetails)
                .MaximumLength(500).WithMessage("Detaylar en fazla 500 karakter olabilir.")
                .When(x => !string.IsNullOrWhiteSpace(x.EventDetails));

            RuleFor(x => x.EventImage)
                .NotNull().WithMessage("Resim dosyası zorunlu.")
                .Must(ValidatorMethod.BeValidImage).WithMessage("Geçerli bir görsel dosyası (.jpg/.png/.gif, max 5MB) yükleyiniz.");
            RuleFor(x => x.EventPriceSembol)
                .NotEmpty().WithMessage("Fiyat para birimi sembolü seçilmelidir.")
                .Must((symbol) =>
                {
                    string[] allowedSymbols = new string[]
                    {
            "₺", "$", "€", "₽", "₼", "£", "₹", "¥", "₩", "฿", "₦", "₪", "₫", "₱",
            "د.إ", "ر.س", "د.ك", "د.ب", "د.ع", "ل.ل", "د.م.", "د.ت", "ج.م", "د.ج", "س.و", "د.ي", "ش.ج"
                    };
                    return allowedSymbols.Contains(symbol);
                })
                .WithMessage("Geçersiz bir para birimi sembolü seçtiniz.");
        }
    }
}