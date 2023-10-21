namespace MagicVilla_CoupounAPI.Models.DTO
{
    public class CouponEditedDTO
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public int Percentage { get; set; }
        public bool IsActive { get; set; }
    }
}
