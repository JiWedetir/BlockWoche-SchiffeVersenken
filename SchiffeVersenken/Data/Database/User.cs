using SQLite;

namespace SchiffeVersenken.Data.Database
{
    public class User
    {
        [PrimaryKey, AutoIncrement]
        public int Id { get; set; }

        [NotNull, Unique]
        public string Name { get; set; } = string.Empty;

        [NotNull]
        public string PasswordHash { get; set; } = string.Empty;

        [NotNull]
        public string Salt { get; set; } = string.Empty;
    }
}
