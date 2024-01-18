using SQLite;

namespace SchiffeVersenken.Data.Database
{
    public class User
    {
        [PrimaryKey, AutoIncrement]
        public int Id { get; set; }

        [NotNull]
        public string Name { get; set; }

        [NotNull]
        public string PasswordHash { get; set; }

        [NotNull, Unique]
        public int[] Salt { get; set; }
    }
}
