namespace SchiffeVersenken.Data.Database
{
    public class UserScore
    {
        public string Name { get; set; } = string.Empty;
        public int Score { get; set; }
        public string Opponent { get; set; } = string.Empty;
        public bool Won { get; set; }
    }
}
