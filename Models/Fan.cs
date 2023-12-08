using System.ComponentModel.DataAnnotations;
using System.Xml.Linq;

namespace Assignment2.Models
{
    public class Fan
    {
        public int Id { get; set; }

        [Required(ErrorMessage = "Last Name is required.")]
        [StringLength(50)]
        [Display(Name = "Last Name")]
        public string LastName { get; set; }

        [Required(ErrorMessage = "First Name is required.")]
        [StringLength(50)]
        [Display(Name = "First Name")]
        public string FirstName { get; set; }

        [DataType(DataType.Date)]
        [DisplayFormat(DataFormatString = "{0:yyyy-MM-dd}", ApplyFormatInEditMode = true)]
        [Display(Name = "Birth Date")]
        public DateTime BirthDate { get; set; }

        [Display(Name = "Full Name")]
        public string FullName => LastName + ", " + FirstName;


        public ICollection<Subscription> Subscriptions { get; set; }


    }
}
