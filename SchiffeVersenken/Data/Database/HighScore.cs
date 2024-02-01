using SQLite;
using System.ComponentModel.DataAnnotations.Schema;

namespace SchiffeVersenken.Data.Database
{
    public class Highscore
    {
        [PrimaryKey, AutoIncrement]
        public int Id { get; set; }

        [NotNull]
        public int Score { get; set; }

        [NotNull]
        public string Opponent { get; set; }

        [NotNull]
        public bool Won { get; set; }

        [ForeignKey("User")]
        public int User_Id { get; set; }
    }
}
