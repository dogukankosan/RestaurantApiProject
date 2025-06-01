using FluentValidation;
using RestaurantAPI.Dtos.FeatureDtos;

namespace RestaurantAPI.ValidationRules
{
    public class UpdateFeatureValidator : AbstractValidator<UpdateFeatureDto>
    {
        public UpdateFeatureValidator()
        {
            RuleFor(x => x.FeatureTitle)
                .NotEmpty().WithMessage("Başlık boş geçilemez")
                .MaximumLength(150).WithMessage("Başlık en fazla 150 karakter olabilir");

            RuleFor(x => x.FeatureSubTitle)
                .NotEmpty().WithMessage("Alt başlık boş geçilemez")
                .MaximumLength(150).WithMessage("Alt başlık en fazla 150 karakter olabilir");

            RuleFor(x => x.FeatureDescription)
                .NotEmpty().WithMessage("Açıklama boş geçilemez")
                .MaximumLength(1000).WithMessage("Açıklama en fazla 1000 karakter olabilir");

            RuleFor(x => x.FeatureVideoUrl)
                .NotEmpty().WithMessage("Video URL boş geçilemez")
                .MaximumLength(300).WithMessage("Video URL en fazla 300 karakter olabilir")
                .Must(url => Uri.IsWellFormedUriString(url, UriKind.Absolute))
                .WithMessage("Geçerli bir video URL giriniz");

            RuleFor(x => x.FeatureImageData)
                .NotNull().WithMessage("Görsel verisi boş olamaz")
                .Must(img => img.Length > 0).WithMessage("Görsel içeriği boş olamaz");
        }
    }
}