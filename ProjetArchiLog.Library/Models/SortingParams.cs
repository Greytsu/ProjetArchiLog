﻿namespace ProjetArchiLog.Library.Models
{
    public class SortingParams
    {
        public String? Sort { get; set; }

        public bool HasSort()
        {
            return !string.IsNullOrWhiteSpace(Sort);
        }

        public String[]? GetParams()
        {
            return Sort?.Split(",");
        }
    }
}
