using FluentValidation;
using RestaurantAPI.Dtos.TestimonialDtos;

namespace RestaurantAPI.ValidationRules
{
    public class UpdateTestimonialValidator : AbstractValidator<UpdateTestimonialDto>
    {
        public UpdateTestimonialValidator()
        {
            RuleFor(x => x.TestimonialNameSurname)
                .NotEmpty().WithMessage("Ad soyad boş geçilemez")
                .MaximumLength(100).WithMessage("Ad soyad en fazla 100 karakter olabilir");

            RuleFor(x => x.TestimonialTitle)
                .MaximumLength(100).WithMessage("Unvan en fazla 100 karakter olabilir");

            RuleFor(x => x.TestimonialComment)
                .NotEmpty().WithMessage("Yorum boş geçilemez")
                .MaximumLength(1000).WithMessage("Yorum en fazla 1000 karakter olabilir");

            RuleFor(x => x.TestimonialImage)
                .NotNull().WithMessage("Görsel boş olamaz")
                .Must(img => img.Length > 0).WithMessage("Görsel içeriği boş olamaz");
        }
    }
}