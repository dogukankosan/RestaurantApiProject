using FluentValidation;
using RestaurantAPI.Dtos.AboutDtos;
using RestaurantAPI.Helpers;

namespace RestaurantAPI.ValidationRules
{
    public class UpdateAboutValidator : AbstractValidator<UpdateAboutDto>
    {
        public UpdateAboutValidator()
        {
            RuleFor(x => x.AboutCompanyName)
                .NotEmpty().WithMessage("Şirket adı boş olamaz.")
                .Must(name => !string.IsNullOrWhiteSpace(name))
                    .WithMessage("Şirket adı yalnızca boşluklardan oluşamaz")
                .MinimumLength(3).WithMessage("Şirket adı en az 3 karakter olmalı")
                .MaximumLength(100).WithMessage("Şirket adı en fazla 100 karakter olabilir");

            RuleFor(x => x.AboutDesc)
                .NotEmpty().WithMessage("Açıklama alanı boş olamaz.")
                .Must(desc => !string.IsNullOrWhiteSpace(desc))
                    .WithMessage("Açıklama yalnızca boşluklardan oluşamaz")
                .MaximumLength(2000).WithMessage("Açıklama en fazla 2000 karakter olabilir");

            RuleFor(x => x.AboutWhyChoose)
                .NotEmpty().WithMessage("Neden biz alanı boş olamaz.")
                .Must(text => !string.IsNullOrWhiteSpace(text))
                    .WithMessage("Neden biz yalnızca boşluklardan oluşamaz")
                .MaximumLength(500).WithMessage("Neden biz açıklaması en fazla 500 karakter olabilir");

            RuleFor(x => x.AboutImage1)
                .NotNull().WithMessage("1. görsel seçilmelidir.")
                .Must(ValidatorMethod.BeValidImage)
                    .WithMessage("1. görsel dosyası geçersiz (.jpg/.png/.gif, max 5 MB)");

            RuleFor(x => x.AboutImage2)
                .NotNull().WithMessage("2. görsel seçilmelidir.")
                .Must(ValidatorMethod.BeValidImage)
                    .WithMessage("2. görsel dosyası geçersiz (.jpg/.png/.gif, max 5 MB)");

            RuleFor(x => x.AboutReportImage)
                .NotNull().WithMessage("Rapor görseli seçilmelidir.")
                .Must(ValidatorMethod.BeValidImage)
                    .WithMessage("Rapor görseli geçersiz (.jpg/.png/.gif, max 5 MB)");

            RuleFor(x => x.AboutRezervationImage)
                .NotNull().WithMessage("Rezervasyon görseli seçilmelidir.")
                .Must(ValidatorMethod.BeValidImage)
                    .WithMessage("Rezervasyon görseli geçersiz (.jpg/.png/.gif, max 5 MB)");
        }
    }
}