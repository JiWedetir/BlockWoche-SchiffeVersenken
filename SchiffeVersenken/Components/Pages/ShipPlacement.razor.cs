using Microsoft.AspNetCore.Components;
using SchiffeVersenken.Data;

namespace SchiffeVersenken.Components.Pages
{
	public partial class ShipPlacement
    {
		private int _fieldcnt;
		private List<ShipDetails> _ships;
		private List<int> _shipSizes = new List<int> { 5, 4, 4, 3, 3, 3, 2, 2, 2, 2};
		private ShipDetails _lastClickedShip = null;

		protected override void OnInitialized()
		{
			_fieldcnt = 10;
			shipsTemplate shipsTemplate = new shipsTemplate(_shipSizes);
			_ships = shipsTemplate._Ships;
		}

		private void ShipClicked(ShipDetails ship)
		{
			if (_lastClickedShip == null || _lastClickedShip == ship)
			{
				ship.IsClicked = !ship.IsClicked;
			}
			else
			{
				_lastClickedShip.IsClicked = false;
				ship.IsClicked = true;
			}
			_lastClickedShip = ship;
		}

		RenderFragment BuildSvg(ShipDetails ship) => builder =>
		{
			int size = ship.Size;
			int squareSize = 30 / _fieldcnt;
			builder.OpenElement(0, "svg");
			builder.AddAttribute(1, "width", $"{size * squareSize}vw");
			builder.AddAttribute(2, "height", $"{squareSize}vw");

			// Rahmen für das gesamte Rechteck
			builder.OpenElement(9, "rect");
			builder.AddAttribute(12, "width", $"{size * squareSize}vw");
			builder.AddAttribute(13, "height", $"{squareSize}vw");
			builder.AddAttribute(15, "onclick", EventCallback.Factory.Create(this, () => ShipClicked(ship)));
			builder.AddAttribute(14, "class", $"rectangle {(ship.IsClicked ? "clicked" : "")}");
			builder.CloseElement();

			// Quadrate erstellen und dem SVG hinzufügen
			for (int i = 0; i < size; i++)
			{
				builder.OpenElement(3, "rect");
				builder.AddAttribute(4, "x", $"{i * squareSize}vw");
				builder.AddAttribute(5, "y", "0");
				builder.AddAttribute(6, "width", $"calc({squareSize - (2 / size)}vw");
				builder.AddAttribute(7, "height", $"calc({squareSize - (2 / size)})vw");
				builder.AddAttribute(8, "class", "quadrat");
				builder.CloseElement();
			}

			builder.CloseElement();
		};




		private void GoToPreviousPage()
		{
			NavigationManager.NavigateTo("/PregameVsComputer", true);
		}

		private void GoToNextPage()
		{
			NavigationManager.NavigateTo("/Game", true);
		}
	}
}
