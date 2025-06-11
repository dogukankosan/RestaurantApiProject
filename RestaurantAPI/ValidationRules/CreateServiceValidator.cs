using System.Text.RegularExpressions;
using FluentValidation;
using RestaurantAPI.Dtos.ServiceDtos;

namespace RestaurantAPI.ValidationRules
{
    public class CreateServiceValidator : AbstractValidator<CreateServiceDto>
    {
        private static readonly Regex BootstrapIconPattern =
            new Regex(@"^bi-[a-z0-9]+(?:-[a-z0-9]+)*$", RegexOptions.Compiled);
        public CreateServiceValidator()
        {
            RuleFor(x => x.ServiceTitle)
                .NotEmpty().WithMessage("Servis başlığı boş geçilemez")
                .Must(title => !string.IsNullOrWhiteSpace(title))
                .WithMessage("Servis başlığı boşluklardan oluşamaz")
                .MinimumLength(3).WithMessage("Servis başlığı en az 3 karakter olmalı")
                .MaximumLength(100).WithMessage("Servis başlığı en fazla 100 karakter olabilir");

            RuleFor(x => x.ServiceDescription)
                .MaximumLength(1000).WithMessage("Servis açıklaması en fazla 1000 karakter olabilir")
                .When(desc => !string.IsNullOrWhiteSpace(desc.ServiceDescription));

            RuleFor(x => x.ServiceIconUrl)
                .NotEmpty().WithMessage("İkon sınıfı boş geçilemez")
                .MaximumLength(200).WithMessage("İkon sınıfı en fazla 200 karakter olabilir")
                .Must(icon => BootstrapIconPattern.IsMatch(icon))
                .WithMessage("Geçerli bir Bootstrap icon sınıfı giriniz (örn. bi-alarm-fill)");
        }
    }
}