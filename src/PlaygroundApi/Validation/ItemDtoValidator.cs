using FluentValidation;
using Playground.Dto;

namespace PlaygroundApi.Validation
{
    public class ItemDtoValidator : AbstractValidator<ItemDto>
    {
        public ItemDtoValidator()
        {
            RuleFor(x => x.Name)
                .NotEmpty();
        }
    }
}
