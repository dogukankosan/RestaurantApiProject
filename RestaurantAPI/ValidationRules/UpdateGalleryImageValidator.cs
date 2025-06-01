using FluentValidation;
using RestaurantAPI.Dtos.GalleryImageDtos;
namespace RestaurantAPI.ValidationRules
{
    public class UpdateGalleryImageValidator : AbstractValidator<UpdateGalleryImageDto>
    {
        public UpdateGalleryImageValidator()
        {
            RuleFor(x => x.ImageTitle)
                .MaximumLength(100).WithMessage("Görsel başlığı en fazla 100 karakter olabilir");

            RuleFor(x => x.ImageByte)
                .NotNull().WithMessage("Görsel verisi boş olamaz")
                .Must(img => img.Length > 0).WithMessage("Görsel içeriği boş olamaz");
        }
    }
}