namespace SchiffeVersenken.Data.Model.Interfaces
{
    public interface IOpponent : IPlayerBehaviour
    {
        public Task SetShipAsync();
        public Task ShootAsync();
    }
}
