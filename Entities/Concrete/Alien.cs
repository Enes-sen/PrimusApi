using Entities.Abstract;
using Microsoft.AspNetCore.Http;
using System.ComponentModel.DataAnnotations.Schema;

namespace Entities.Concrete
{
    [Table("Aliens", Schema = "public")]
    public class Alien : IEntity
    {
        public int id { get; set; }
        public string? homeworld { get; set; }
        public string? dnasample { get; set; }
        public string? givenname { get; set; }
        public int userid { get; set; }
        public string? alienshadow { get; set; }

        [NotMapped]
        public IFormFile? FormFile { get; set; }
    }

}
