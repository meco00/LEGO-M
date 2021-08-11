namespace LegoM.Data.Models
{
    using System.ComponentModel.DataAnnotations;

    public class Favourite
    {
        public int Id { get; set; }

        [Required]
        public string ProductId { get; set; }

        public virtual Product Product { get; set; }

        [Required]
        public string UserId { get; set; }

        public User User { get; set; }




    }
}
