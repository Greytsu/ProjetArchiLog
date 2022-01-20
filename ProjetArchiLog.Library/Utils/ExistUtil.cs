namespace ProjetArchiLog.Library.Utils
{
    internal class ExistUtil
    {
        public static bool ExistProperty<Tmodel>(String PropertyName)
        {
            try
            {
                typeof(Tmodel).GetProperty(PropertyName);
            }
            catch (Exception)
            {
                return false;
            }

            return true;
        }
    }
}
