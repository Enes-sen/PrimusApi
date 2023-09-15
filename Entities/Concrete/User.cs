using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;
using Entities.Abstract;

namespace Entities.Concrete
{
    [Table("Users", Schema = "public")]
    public class User : IEntity
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int id { get; set; }

        [MaxLength(800)] // Kullanıcı adının maksimum uzunluğunu ayarlayabilirsiniz
        public string? username { get; set; }

        [Required]
        public byte[]? passwordhash { get; set; }

        [Required]
        public byte[]? passwordsalt { get; set; }
    }

}
