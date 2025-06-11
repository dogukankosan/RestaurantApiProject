using FluentValidation;
using RestaurantAPI.Dtos.GalleryImageDtos;
using RestaurantAPI.Helpers;

namespace RestaurantAPI.ValidationRules
{
    public class CreateGalleryImageValidator : AbstractValidator<CreateGalleryImageDto>
    {
        public CreateGalleryImageValidator()
        {
            RuleFor(x => x.ImageTitle)
                .Must(title => string.IsNullOrWhiteSpace(title)
                               || (!string.IsNullOrWhiteSpace(title)
                                   && title.Trim().Length >= 3))
                .WithMessage("Görsel başlığı en az 3 karakter olmalı")
                .MaximumLength(100)
                .WithMessage("Görsel başlığı en fazla 100 karakter olabilir");

            RuleFor(x => x.ImageByte)
                .NotNull().WithMessage("Görsel verisi boş olamaz")
                .Must(ValidatorMethod.BeValidImage)
                .WithMessage("Geçerli bir görsel dosyası (.jpg/.png/.gif, max 5 MB) yükleyiniz");
        }
    }
}