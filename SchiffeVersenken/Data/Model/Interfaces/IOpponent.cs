namespace SchiffeVersenken.Data.Model.Interfaces
{
    public interface IOpponent : IPlayerBehaviour
    {
        public Task SetShipAsync();
        public Task SetShipAsync(int[,] board);
        public Task ShootAsync();
    }
}
