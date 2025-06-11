using FluentValidation;
using RestaurantAPI.Dtos.CategoryDtos;

namespace RestaurantAPI.ValidationRules
{
    public class UpdateCategoryValidator : AbstractValidator<UpdateCategoryDto>
    {
        public UpdateCategoryValidator()
        {
            RuleFor(x => x.CategoryName)
                .NotEmpty().WithMessage("Kategori adı boş geçilemez")
                .Must(name => !string.IsNullOrWhiteSpace(name))
                .WithMessage("Kategori adı boşluklardan oluşamaz")
                .MinimumLength(3).WithMessage("Kategori adı en az 3 karakter olmalı")
                .MaximumLength(100).WithMessage("Kategori adı en fazla 100 karakter olabilir");
        }
    }
}