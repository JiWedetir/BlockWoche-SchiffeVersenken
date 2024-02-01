namespace SchiffeVersenken.Data.Controller
{
    public interface IOpponent
    {
        public bool _YourTurn { get; set; }
        public Task SetShipAsync();
        public Task<bool> SetShipAsync(int[,] board);
        public Task ShootAsync();
    }
}
