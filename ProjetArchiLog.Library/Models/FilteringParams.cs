using Microsoft.AspNetCore.Http;
using ProjetArchiLog.Library.Extensions;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ProjetArchiLog.Library.Models
{
    public class FilteringParams<TModel>
    {
        public Dictionary<string, string> Filters { get; set; }

        public FilteringParams(IQueryCollection QueryParams)
        {
            SetQueryParams(QueryParams);
        }

        public void SetQueryParams(IQueryCollection QueryParams)
        {
            this.Filters = new Dictionary<string, string>();
            foreach (var Param in QueryParams.ExtractModelProperties<TModel>())
            {
                this.Filters.Add(Param.Key, Param.Value.ToString());
            }
        }
    }
}
