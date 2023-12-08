using Microsoft.EntityFrameworkCore.Metadata.Internal;
using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;
using System.Xml.Linq;

namespace Assignment2.Models
{
    public class SportClub
    {
        [DatabaseGenerated(DatabaseGeneratedOption.None)]
        [Display(Name = "Registration Number")]
        [Required(ErrorMessage = "Registration Number is required.")]
        public string Id { get; set; }

        [Required(ErrorMessage = "Title is required.")]
        [StringLength(50, MinimumLength = 3)]
        public string Title { get; set; }

        [DataType(DataType.Currency)]
        [Column(TypeName = "money")]
        public decimal Fee { get; set; }

        public ICollection<Subscription> Subscriptions { get; set; }

    }
}
