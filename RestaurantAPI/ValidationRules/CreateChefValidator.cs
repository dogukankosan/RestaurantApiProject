using FluentValidation;
using RestaurantAPI.Dtos.ChefDtos;
using RestaurantAPI.Helpers;

namespace RestaurantAPI.ValidationRules
{
    public class CreateChefValidator : AbstractValidator<CreateChefDto>
    {
        public CreateChefValidator()
        {
            RuleFor(x => x.ChefNameSurname)
                .NotEmpty().WithMessage("Şef ad-soyad alanı boş geçilemez")
                .Must(name => !string.IsNullOrWhiteSpace(name)).WithMessage("Ad-soyad yalnızca boşluk olamaz")
                .MinimumLength(5).WithMessage("Şef adı en az 5 karakter olmalı")
                .MaximumLength(100).WithMessage("Şef ad-soyad en fazla 100 karakter olabilir")
                .Must(ValidatorMethod.BeValidName).WithMessage("Ad ve soyad arasında en az bir boşluk olmalı");

            RuleFor(x => x.ChefTitle)
                .NotEmpty().WithMessage("Şef unvanı boş geçilemez")
                .MinimumLength(3).WithMessage("Şef unvanı en az 3 karakter olmalı")
                .MaximumLength(200).WithMessage("Şef unvanı en fazla 200 karakter olabilir");

            RuleFor(x => x.ChefDescription)
                .MaximumLength(1000).WithMessage("Açıklama en fazla 1000 karakter olabilir")
                .When(x => !string.IsNullOrEmpty(x.ChefDescription));

            RuleFor(x => x.ChefTwitterLink)
                .MaximumLength(200).WithMessage("ChefTwitterLink en fazla 200 karakter olabilir")
                .Must(ValidatorMethod.BeValidUrl).WithMessage("ChefTwitterLink geçerli bir URL olmalı")
                .When(x => !string.IsNullOrEmpty(x.ChefTwitterLink));
            RuleFor(x => x.ChefFacebookLink)
                .MaximumLength(200).WithMessage("ChefFacebookLink en fazla 200 karakter olabilir")
                .Must(ValidatorMethod.BeValidUrl).WithMessage("ChefFacebookLink geçerli bir URL olmalı")
                .When(x => !string.IsNullOrEmpty(x.ChefFacebookLink));

            RuleFor(x => x.ChefInstagramLink)
                .MaximumLength(200).WithMessage("ChefInstagramLink en fazla 200 karakter olabilir")
                .Must(ValidatorMethod.BeValidUrl).WithMessage("ChefInstagramLink geçerli bir URL olmalı")
                .When(x => !string.IsNullOrEmpty(x.ChefInstagramLink));

            RuleFor(x => x.ChefLinkedinLink)
                .MaximumLength(200).WithMessage("ChefLinkedinLink en fazla 200 karakter olabilir")
                .Must(ValidatorMethod.BeValidUrl).WithMessage("ChefLinkedinLink geçerli bir URL olmalı")
                .When(x => !string.IsNullOrEmpty(x.ChefLinkedinLink));

            RuleFor(x => x.ChefImage)
                .NotNull().WithMessage("Şef görseli mutlaka yüklenmeli")
                .Must(ValidatorMethod.BeValidImage).WithMessage("Geçerli bir görsel dosyası (.jpg/.png/.gif, max 5 MB) yükleyiniz");

            RuleFor(x => x.ChefStatus)
                .NotNull().WithMessage("Durum belirtilmelidir");
        }
    }
}