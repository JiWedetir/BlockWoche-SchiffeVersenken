namespace SchiffeVersenken.Data.Controller
{
    public interface IOpponent
    {
        public Task SetShipAsync();
        public Task<bool> SetShipAsync(int[,] board);
        public Task ShootAsync();
    }
}
