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
		public Orientation Orientation { get; set; } = Orientation.Horizontal;
	}
	public class shipsTemplate
	{
		public List<int> _ShipSizes { get; } = new List<int> { 5, 4, 4, 3, 3, 3, 2, 2, 2, 2 };
		public List<ShipDetails> _Ships { get; set; } = new List<ShipDetails>();
		public shipsTemplate(List<int> shipSizes)
		{
			foreach (int size in shipSizes)
			{
				_Ships.Add(CreateShip(size));
			}
		}

		private ShipDetails CreateShip(int size)
		{
			switch (size)
			{
				case 5:
					return new ShipDetails
					{
						Name = "Carrier",
						Size = 5
					};
					break;
				case 4:
					return new ShipDetails
					{
						Name = "Battleship",
						Size = 4
					};
					break;
				case 3:
					return new ShipDetails
					{
						Name = "Cruiser",
						Size = 3
					};
					break;
				default:
					return new ShipDetails
					{
						Name = "Destroyer",
						Size = 2
					};
					break;
			}
		}
	}
}
