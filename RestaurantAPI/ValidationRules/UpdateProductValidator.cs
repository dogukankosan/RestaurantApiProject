using FluentValidation;
using RestaurantAPI.Dtos.ProductDtos;

namespace RestaurantAPI.ValidationRules
{
    public class UpdateProductValidator : AbstractValidator<UpdateProductDto>
    {
        public UpdateProductValidator()
        {
            RuleFor(x => x.ProductName)
                .NotEmpty().WithMessage("Ürün Adı boş geçilemez")
                .MaximumLength(100).WithMessage("Ürün Adı en fazla 100 karakter olabilir");
            RuleFor(x => x.ProductDescription)
                .MaximumLength(1000).WithMessage("Ürün açıklaması en fazla 1000 karakter olabilir");
            RuleFor(x => x.Price)
                .NotEmpty().WithMessage("Fiyat boş olamaz")
                .GreaterThan(0).WithMessage("Fiyat 0'dan büyük olmalıdır")
                .LessThanOrEqualTo(999999).WithMessage("Fiyat 999999'dan büyük olamaz");
        }
    }
}