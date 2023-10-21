namespace MagicVilla_CoupounAPI.Models.DTO
{
    public class CouponDTO
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public int Percentage { get; set; }
        public bool IsActive { get; set; }
        public DateTime? Created { get; set; }
    }
}
