using FluentValidation;
using RestaurantAPI.Dtos.MessageDtos;

namespace RestaurantAPI.ValidationRules
{
    public class UpdateMessageValidator : AbstractValidator<UpdateMessageDto>
    {
        public UpdateMessageValidator()
        {
            RuleFor(x => x.MessageNameSurname)
                .NotEmpty().WithMessage("Ad soyad boş geçilemez")
                .MaximumLength(100).WithMessage("Ad soyad en fazla 100 karakter olabilir");

            RuleFor(x => x.MessageEmail)
                .NotEmpty().WithMessage("E-posta boş geçilemez")
                .MaximumLength(100).WithMessage("E-posta en fazla 100 karakter olabilir")
                .EmailAddress().WithMessage("Geçerli bir e-posta adresi giriniz");

            RuleFor(x => x.MessageSubject)
                .NotEmpty().WithMessage("Konu boş geçilemez")
                .MaximumLength(300).WithMessage("Konu en fazla 300 karakter olabilir");

            RuleFor(x => x.MessageDetails)
                .NotEmpty().WithMessage("Mesaj içeriği boş geçilemez");

            RuleFor(x => x.MessageSendDate)
                .LessThanOrEqualTo(DateTime.Now).WithMessage("Gelecek tarihli mesaj gönderilemez");
        }
    }
}