using FluentValidation;
using RestaurantAPI.Dtos.CompanyInfoDtos;
using RestaurantAPI.Helpers;
using System.Linq.Expressions;
using System.Text.RegularExpressions;

namespace RestaurantAPI.ValidationRules
{
    public class UpdateCompanyInfoValidator : AbstractValidator<UpdateCompanyInfoDto>
    {
        private static readonly Regex GoogleMapsIframePattern = new(
            @"^(https:\/\/www\.google\.com\/maps\/embed\?.+|https:\/\/maps\.google\.com\/maps\?.+)$",
            RegexOptions.IgnoreCase | RegexOptions.Compiled);
        public UpdateCompanyInfoValidator()
        {
            RuleFor(x => x.CompanyInfoAddress)
                .NotEmpty().WithMessage("Adres bilgisi boş olamaz")
                .Must(a => !string.IsNullOrWhiteSpace(a)).WithMessage("Adres yalnızca boşluklardan oluşamaz")
                .MaximumLength(200).WithMessage("Adres en fazla 200 karakter olabilir");

            RuleFor(x => x.CompanyInfoPhone)
                .NotEmpty().WithMessage("Telefon numarası boş olamaz")
                .Must(p => Regex.IsMatch(p, @"^\+?\d{10,20}$"))
                    .WithMessage("Telefon numarası sadece rakam ve isteğe bağlı + içermeli (10–20 hane)")
                .MaximumLength(20).WithMessage("Telefon numarası en fazla 20 karakter olabilir");

            RuleFor(x => x.CompanyInfoOpenClosed)
                .NotEmpty().WithMessage("Çalışma saatleri boş olamaz")
                .Must(s => !string.IsNullOrWhiteSpace(s)).WithMessage("Çalışma saatleri boşluklardan oluşamaz")
                .MaximumLength(100).WithMessage("Çalışma saatleri en fazla 100 karakter olabilir");

            RuleFor(x => x.CompanyInfoMail)
                .NotEmpty().WithMessage("E-posta adresi boş olamaz")
                .EmailAddress().WithMessage("Geçersiz e-posta formatı")
                .MaximumLength(75).WithMessage("E-posta en fazla 75 karakter olabilir");

            ApplyUrlRule(x => x.CompanyInfoGithubLink, "Github linki");
            ApplyUrlRule(x => x.CompanyInfoWebSiteLink, "Web site linki");
            ApplyUrlRule(x => x.CompanyInfoInstagramLink, "Instagram linki");
            ApplyUrlRule(x => x.CompanyInfoLinkedinLink, "LinkedIn linki");

            RuleFor(x => x.CompanyInfoIFrame)
                .Must(i => string.IsNullOrWhiteSpace(i)
                           || GoogleMapsIframePattern.IsMatch(i))
                    .WithMessage("Geçerli bir Google Maps embed iframe kodu giriniz")
                .MaximumLength(1000).WithMessage("Iframe kodu 1000 karakteri geçemez");
        }
        private void ApplyUrlRule(
            Expression<Func<UpdateCompanyInfoDto, string>> selector,
            string fieldName)
        {
            RuleFor(selector)
                .Must(url => string.IsNullOrWhiteSpace(url)
                              || ValidatorMethod.BeValidUrl(url))
                    .WithMessage($"{fieldName} geçerli bir URL olmalı")
                .MaximumLength(200).WithMessage($"{fieldName} en fazla 200 karakter olabilir");
        }
    }
}