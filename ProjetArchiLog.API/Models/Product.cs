using ProjetArchiLog.Library.Models;

namespace ProjetArchiLog.API.Models
{
    public class Product:BaseModel
    {
        public string Name { get; set; }
        public string? Description { get; set; }
        public double Price { get; set; }
        public string Type { get; set; }

    }
}
