namespace MagicVilla_CoupounAPI.Models.DTO
{
    public class CouponCreateDTO
    {
        public string Name { get; set; }
        public int Percentage { get; set; }
        public bool IsActive { get; set; }
    }
}
