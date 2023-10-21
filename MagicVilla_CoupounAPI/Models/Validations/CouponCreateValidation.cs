using FluentValidation;
using MagicVilla_CoupounAPI.Models.DTO;

namespace MagicVilla_CoupounAPI.Models.Validations
{
    public class CouponCreateValidation : AbstractValidator<CouponCreateDTO>
    {
        public CouponCreateValidation()
        {
            RuleFor(model => model.Name).NotEmpty();
            RuleFor(model => model.Percentage).InclusiveBetween(1, 100);
        }
    }
}
