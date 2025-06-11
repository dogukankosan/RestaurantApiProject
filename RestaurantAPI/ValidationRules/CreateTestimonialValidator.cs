using FluentValidation;
using RestaurantAPI.Dtos.TestimonialDtos;
using RestaurantAPI.Helpers;

namespace RestaurantAPI.ValidationRules
{
    public class CreateTestimonialValidator : AbstractValidator<CreateTestimonialDto>
    {
        public CreateTestimonialValidator()
        {
            RuleFor(x => x.TestimonialNameSurname)
                .NotEmpty().WithMessage("Ad soyad boş geçilemez")
                .Must(name => !string.IsNullOrWhiteSpace(name))
                .WithMessage("Ad soyad yalnızca boşluklardan oluşamaz")
                .Must(ValidatorMethod.BeValidName)
                .WithMessage("Ad ve soyad arasında en az iki parça olmalı")
                .MaximumLength(100).WithMessage("Ad soyad en fazla 100 karakter olabilir");

            RuleFor(x => x.TestimonialTitle)
                .MaximumLength(100).WithMessage("Unvan en fazla 100 karakter olabilir")
                .When(x => !string.IsNullOrWhiteSpace(x.TestimonialTitle));

            RuleFor(x => x.TestimonialComment)
                .NotEmpty().WithMessage("Yorum boş geçilemez")
                .Must(comment => !string.IsNullOrWhiteSpace(comment))
                .WithMessage("Yorum yalnızca boşluklardan oluşamaz")
                .MaximumLength(1000).WithMessage("Yorum en fazla 1000 karakter olabilir");

            RuleFor(x => x.TestimonialImage)
                .NotNull().WithMessage("Görsel boş olamaz")
                .Must(ValidatorMethod.BeValidImage)
                .WithMessage("Geçerli bir görsel (.jpg/.png/.gif, max 5 MB) yükleyiniz");
        }
    }
}