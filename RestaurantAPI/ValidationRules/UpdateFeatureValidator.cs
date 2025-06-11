using FluentValidation;
using RestaurantAPI.Dtos.FeatureDtos;
using RestaurantAPI.Helpers;

namespace RestaurantAPI.ValidationRules
{
    public class UpdateFeatureValidator : AbstractValidator<UpdateFeatureDto>
    {
        public UpdateFeatureValidator()
        {
            RuleFor(x => x.FeatureTitle)
                .NotEmpty().WithMessage("Başlık boş geçilemez")
                .Must(title => !string.IsNullOrWhiteSpace(title)).WithMessage("Başlık boşluklardan oluşamaz")
                .MinimumLength(3).WithMessage("Başlık en az 3 karakter olmalı")
                .MaximumLength(150).WithMessage("Başlık en fazla 150 karakter olabilir");

            RuleFor(x => x.FeatureSubTitle)
                .NotEmpty().WithMessage("Alt başlık boş geçilemez")
                .Must(sub => !string.IsNullOrWhiteSpace(sub)).WithMessage("Alt başlık boşluklardan oluşamaz")
                .MinimumLength(3).WithMessage("Alt başlık en az 3 karakter olmalı")
                .MaximumLength(150).WithMessage("Alt başlık en fazla 150 karakter olabilir");

            RuleFor(x => x.FeatureDescription)
                .NotEmpty().WithMessage("Açıklama boş geçilemez")
                .MaximumLength(1000).WithMessage("Açıklama en fazla 1000 karakter olabilir");

            RuleFor(x => x.FeatureVideoUrl)
                .NotEmpty().WithMessage("Video URL boş geçilemez")
                .MaximumLength(300).WithMessage("Video URL en fazla 300 karakter olabilir")
                .Must(ValidatorMethod.BeValidHttpUrl)
                .WithMessage("Geçerli bir video URL (http/https) giriniz");

            RuleFor(x => x.FeatureImageData)
                .NotNull().WithMessage("Görsel verisi boş olamaz")
                .Must(ValidatorMethod.BeValidImage).WithMessage("Geçerli bir görsel dosyası (.jpg/.png/.gif, max 5 MB) yükleyiniz");
        }
    }
}