using FluentValidation;
using RestaurantAPI.Dtos.MessageDtos;

namespace RestaurantAPI.ValidationRules
{
    public class CreateMessageValidator : AbstractValidator<CreateMessageDto>
    {
        private static readonly TimeSpan SendDateTolerance = TimeSpan.FromMinutes(5);
        public CreateMessageValidator()
        {
            RuleFor(x => x.MessageNameSurname)
                .NotEmpty().WithMessage("Ad soyad boş geçilemez")
                .Must(name => !string.IsNullOrWhiteSpace(name))
                    .WithMessage("Ad soyad yalnızca boşluklardan oluşamaz")
                .MinimumLength(3).WithMessage("Ad soyad en az 3 karakter olmalı")
                .MaximumLength(100).WithMessage("Ad soyad en fazla 100 karakter olabilir");

            RuleFor(x => x.MessageEmail)
                .NotEmpty().WithMessage("E-posta boş geçilemez")
                .MaximumLength(100).WithMessage("E-posta en fazla 100 karakter olabilir")
                .EmailAddress().WithMessage("Geçerli bir e-posta adresi giriniz");

            RuleFor(x => x.MessageSubject)
                .NotEmpty().WithMessage("Konu boş geçilemez")
                .Must(sub => !string.IsNullOrWhiteSpace(sub))
                    .WithMessage("Konu yalnızca boşluklardan oluşamaz")
                .MinimumLength(3).WithMessage("Konu en az 3 karakter olmalı")
                .MaximumLength(300).WithMessage("Konu en fazla 300 karakter olabilir");

            RuleFor(x => x.MessageDetails)
                .NotEmpty().WithMessage("Mesaj içeriği boş geçilemez")
                .Must(details => !string.IsNullOrWhiteSpace(details))
                    .WithMessage("Mesaj içeriği yalnızca boşluklardan oluşamaz")
                .MaximumLength(2000).WithMessage("Mesaj içeriği en fazla 2000 karakter olabilir");

            RuleFor(x => x.MessageSendDate)
                .LessThanOrEqualTo(DateTime.Now + SendDateTolerance)
                    .WithMessage($"Gelecek tarihli mesaj gönderilemez (en fazla {SendDateTolerance.TotalMinutes} dakikalık tolerans).");
        }
    }
}