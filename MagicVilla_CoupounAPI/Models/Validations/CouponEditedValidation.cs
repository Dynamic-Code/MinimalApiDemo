using FluentValidation;
using MagicVilla_CoupounAPI.Models.DTO;

namespace MagicVilla_CoupounAPI.Models.Validations
{
    public class CouponEditedValidation : AbstractValidator<CouponEditedDTO>
    {
        public CouponEditedValidation()
        {
            RuleFor(model => model.Id).NotEmpty().GreaterThan(0);
            RuleFor(model => model.Name).NotEmpty();
            RuleFor(model => model.Percentage).InclusiveBetween(1, 100);
        }
    }
}
