using FluentValidation;
using RestaurantAPI.Dtos.ProductDtos;
using RestaurantAPI.Helpers;

namespace RestaurantAPI.ValidationRules
{
    public class CreateProductValidator : AbstractValidator<CreateProductDto>
    {
        public CreateProductValidator()
        {
            RuleFor(x => x.ProductName)
                .NotEmpty().WithMessage("Ürün adı boş geçilemez")
                .Must(name => !string.IsNullOrWhiteSpace(name))
                    .WithMessage("Ürün adı yalnızca boşluklardan oluşamaz")
                .MinimumLength(3).WithMessage("Ürün adı en az 3 karakter olmalı")
                .MaximumLength(100).WithMessage("Ürün adı en fazla 100 karakter olabilir");

            RuleFor(x => x.ProductDescription)
                .MaximumLength(1000).WithMessage("Ürün açıklaması en fazla 1000 karakter olabilir")
                .When(desc => !string.IsNullOrWhiteSpace(desc.ProductDescription));

            RuleFor(x => x.ProductPrice)
                .GreaterThan(0).WithMessage("Fiyat 0'dan büyük olmalıdır")
                .LessThanOrEqualTo(999_999).WithMessage("Fiyat 999999'dan büyük olamaz");

            RuleFor(x => x.ProductImageData)
                .NotNull().WithMessage("Ürün görseli boş olamaz")
                .Must(ValidatorMethod.BeValidImage).WithMessage("Geçerli bir görsel dosyası (.jpg/.png/.gif, max 5 MB) yükleyiniz");

            RuleFor(x => x.CategoryID)
                .GreaterThan(0).WithMessage("Geçerli bir kategori seçmelisiniz");
        }
    }
}