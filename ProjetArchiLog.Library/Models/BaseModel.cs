using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ProjetArchiLog.Library.Models
{
    public abstract class BaseModel
    {
        [Key]
        public Guid Id { get; set; }
        public bool Active { get; set; }
        public DateTime? DeletedAt { get; set; }
    }
}
