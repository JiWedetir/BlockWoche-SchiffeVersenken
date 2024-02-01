using SQLite;

namespace SchiffeVersenken.Data.Database
{
    public class User
    {
        [PrimaryKey, AutoIncrement]
        public int Id { get; set; }

        [NotNull, Unique]
        public string Name { get; set; }

        [NotNull]
        public string PasswordHash { get; set; }

        [NotNull]
        public string Salt { get; set; }
    }
}
