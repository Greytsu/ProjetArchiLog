namespace ProjetArchiLog.Library.Models
{
    public class SortingParams
    {
        public String? sort { get; set; }

        public bool HasSort()
        {
            return !string.IsNullOrWhiteSpace(sort);
        }

        public String[]? GetParams()
        {
            return sort?.Split(",");
        }
    }
}
