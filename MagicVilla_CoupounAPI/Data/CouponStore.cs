using MagicVilla_CoupounAPI.Models;

namespace MagicVilla_CoupounAPI.Data
{
    public static class CouponStore
    {
        public static List<Coupon> CouponList = new List<Coupon>
        {
            new Coupon{Id=1,Name="10OFF",Percentage=10,IsActive=true},
            new Coupon{Id=2,Name="20OFF",Percentage=20,IsActive=true},
        };
    }
}
