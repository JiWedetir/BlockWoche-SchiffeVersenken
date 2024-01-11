using Microsoft.AspNetCore.Components;
using SchiffeVersenken.Data;

namespace SchiffeVersenken.Components.Pages
{
	public partial class ShipPlacement
    {
		private int _fieldcnt;
		private List<ShipDetails> _ships;
		private List<int> _shipSizes = new List<int> { 5, 4, 4, 3, 3, 3, 2, 2, 2};

		protected override void OnInitialized()
		{
			_fieldcnt = 15;
			shipsTemplate shipsTemplate = new shipsTemplate(_shipSizes);
			_ships = shipsTemplate._Ships;
		}

		private void ShipClicked(ShipDetails ship)
		{
			ship.IsClicked = !ship.IsClicked;
		}

		// Methode zum Erstellen des SVG-Elements
		//< svg  width = "@shipWidth" height = "@shipHeight" >
		//	< rect @onclick = "() => ShipClicked(ship)" class="@(ship.IsClicked ? "clicked" : "")" width="@shipWidth" height="@shipHeight"/>
		//</svg> 

		//
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
			builder.AddAttribute(14, "class", $"rectangle {(ship.IsClicked ? "clicked" : "")}");
			builder.CloseElement();

			// Quadrate erstellen und dem SVG hinzufügen
			for (int i = 0; i < size; i++)
			{
				builder.OpenElement(3, "rect");
				builder.AddAttribute(4, "x", $"calc{i * squareSize - (1/size)}vw");
				builder.AddAttribute(5, "y", "0");
				builder.AddAttribute(6, "width", $"{squareSize}vw");
				builder.AddAttribute(7, "height", $"{squareSize}vw");
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
