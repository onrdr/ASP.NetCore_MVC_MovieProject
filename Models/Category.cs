using System.ComponentModel;
using System.ComponentModel.DataAnnotations;

namespace Models
{
    public class Category
    {
        public int Id { get; set; }
        public string Name { get; set; }

        [DisplayName("Display Order")]
        [Range(1, 15, ErrorMessage = "Must be between 1 and 5")]
        public int DisplayOrder { get; set; }
        public DateTime CreatedDateTime { get; set; } = DateTime.Now;
    }
}
