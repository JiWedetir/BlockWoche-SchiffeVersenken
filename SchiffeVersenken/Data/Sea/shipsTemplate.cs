namespace SchiffeVersenken.Data
{
	public enum Orientation
	{
		Vertical,
		Horizontal
	}
	public class ShipDetails
	{
		public string Name { get; set; }
		public int PositionX { get; set; }
		public int PositionY { get; set; }
		public int Size { get; set; }
		public bool IsClicked { get; set; }
		public bool IsPlaced { get; set; }
		public Orientation Orientation { get; set; } = Orientation.Horizontal;
	}
	public static class shipsTemplate
	{
		public static List<int> _ShipSizes { get; } = new List<int> { 5, 4, 4, 3, 3, 3, 2, 2, 2, 2 };
		public static List<ShipDetails> _Ships { get; set; } = _ShipSizes.Select(size => CreateShip(size)).ToList();

		/// <summary>
		/// Creates a new ship with the given size.
		/// </summary>
		private static ShipDetails CreateShip(int size)
		{
			var ship = new ShipDetails();
			switch (size)
			{
				case 5:
					ship.Name = "Carrier";
					ship.Size = 5;
					break;
				case 4:
					ship.Name = "Battleship";
					ship.Size = 4;
					break;
				case 3:
					ship.Name = "Cruiser";
					ship.Size = 3;
					break;
				default:
					ship.Name = "Destroyer";
					ship.Size = 2;
					break;
			}
			return ship;
		}
	}
}
