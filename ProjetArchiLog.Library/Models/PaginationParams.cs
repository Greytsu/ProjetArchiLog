using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ProjetArchiLog.Library.Models
{
    public class PaginationParams
    {
        public int page { get; set; }
        public int size { get; set; }

        public PaginationParams()
        {
            this.page = 1;
            this.size = 50;
        }

        public PaginationParams(PaginationParams param)
        {
            this.page = param.page > 0 ? param.page : 1;
            this.size = param.size < 50 ? param.size : 50;
        }
    }
}
