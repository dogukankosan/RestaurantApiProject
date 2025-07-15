using FluentValidation;
using RestaurantAPI.Dtos.MessageDtos;
using System.Text.RegularExpressions;

namespace RestaurantAPI.ValidationRules
{
    public class CreateMessageValidator : AbstractValidator<CreateMessageDto>
    {
        private static readonly TimeSpan SendDateTolerance = TimeSpan.FromMinutes(5);

        public CreateMessageValidator()
        {
            RuleFor(x => x.MessageNameSurname)
                .NotEmpty().WithMessage("Ad soyad boş geçilemez.")
                .Must(name => !string.IsNullOrWhiteSpace(name))
                    .WithMessage("Ad soyad yalnızca boşluklardan oluşamaz.")
                .MinimumLength(3).WithMessage("Ad soyad en az 3 karakter olmalıdır.")
                .MaximumLength(100).WithMessage("Ad soyad en fazla 100 karakter olabilir.")
                .Matches(@"^[a-zA-ZçÇğĞıİöÖşŞüÜ\s'-]+$")
                    .WithMessage("Ad soyad yalnızca harf, boşluk ve (-,') karakterlerini içerebilir.");

            RuleFor(x => x.MessageEmail)
                .NotEmpty().WithMessage("E-posta adresi boş geçilemez.")
                .EmailAddress().WithMessage("Geçerli bir e-posta adresi giriniz.")
                .MaximumLength(100).WithMessage("E-posta en fazla 100 karakter olabilir.");

            RuleFor(x => x.MessagePhone)
            .NotEmpty().WithMessage("Telefon numarası boş geçilemez.")
            .MinimumLength(7).WithMessage("Telefon numarası en az 7 karakter olmalıdır.")
            .MaximumLength(25).WithMessage("Telefon numarası en fazla 25 karakter olabilir.")
            .Matches(@"^\+?[0-9\s\-\(\)]{7,25}$")
                .WithMessage("Geçerli bir telefon numarası giriniz. (+90 532 123 4567 gibi)");

            RuleFor(x => x.MessageSubject)
                .NotEmpty().WithMessage("Konu alanı boş geçilemez.")
                .Must(sub => !string.IsNullOrWhiteSpace(sub))
                    .WithMessage("Konu yalnızca boşluklardan oluşamaz.")
                .MinimumLength(3).WithMessage("Konu en az 3 karakter olmalıdır.")
                .MaximumLength(300).WithMessage("Konu en fazla 300 karakter olabilir.");

            RuleFor(x => x.MessageDetails)
                .NotEmpty().WithMessage("Mesaj içeriği boş geçilemez.")
                .Must(details => !string.IsNullOrWhiteSpace(details))
                    .WithMessage("Mesaj içeriği yalnızca boşluklardan oluşamaz.")
                .MaximumLength(2000).WithMessage("Mesaj içeriği en fazla 2000 karakter olabilir.");

            RuleFor(x => x.MessageSendDate)
                .LessThanOrEqualTo(DateTime.Now + SendDateTolerance)
                .WithMessage($"Gelecek tarihli mesaj gönderilemez. (en fazla {SendDateTolerance.TotalMinutes} dk tolerans)");
        }
    }
}