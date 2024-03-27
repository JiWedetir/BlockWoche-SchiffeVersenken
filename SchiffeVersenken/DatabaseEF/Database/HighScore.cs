namespace SchiffeVersenken.DatabaseEF.Database
{
    public class Highscore
    {
        public int Id { get; set; }

        public int Score { get; set; }

        public string Opponent { get; set; }

        public bool Won { get; set; }

        public int User_Id { get; set; }

        public int TurnsPlayed { get; set; }

        public DateTime StartTime { get; set; }

        public DateTime EndTime { get; set; }
    }
}
