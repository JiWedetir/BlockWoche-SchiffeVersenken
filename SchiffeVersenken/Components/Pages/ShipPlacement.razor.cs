using Microsoft.AspNetCore.Components;
using SchiffeVersenken.Data;
using SchiffeVersenken.Data.Model;


namespace SchiffeVersenken.Components.Pages
{
	public partial class ShipPlacement
    {
		[CascadingParameter]
		public GameLogic Game { get; set; }

		private int _fieldcnt;
		private List<ShipDetails> _shipsPlaced = new List<ShipDetails>();
		private List<ShipDetails> _shipsToSet;
		private List<int> _shipSizes;
		private ShipDetails _lastClickedShip;

		private bool[,] _fieldBoolArray;
		
		protected override void OnInitialized()
		{
			_fieldcnt = 10;
			_shipsToSet = shipsTemplate._Ships;
			_shipSizes = shipsTemplate._ShipSizes;
			_fieldBoolArray = new bool[_fieldcnt, _fieldcnt];
			_lastClickedShip = null;
			foreach(var ship in _shipsToSet)
			{
				ship.IsClicked = false;
				ship.IsPlaced = false;
			}
			Game._Player.SetBoardSize(_fieldcnt);
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

		private void OnSquareClick(int x, int y)
		{
			if (_lastClickedShip == null)
			{
				return;
			}
			_lastClickedShip.PositionX = x + 1;
			_lastClickedShip.PositionY = y + 1;
			_shipsPlaced.Add(_lastClickedShip);

			//Fehlende Einstellungen im Game
			//if (!Game._Player.SetShips(_shipsPlaced))
			//{
			//	_shipsPlaced.Remove(_lastClickedShip);
			//	DialogService.ShowPopup("Ship can't be placed there");
			//	return;
			//}


			int shipLength = _lastClickedShip.Size;
			Orientation orientation = _lastClickedShip.Orientation;
			_lastClickedShip.IsPlaced = true;
			_shipsToSet.Remove(_lastClickedShip);

			if (orientation == Orientation.Horizontal)
			{
				for (int i = 0; i < shipLength; i++)
				{
					//Change Squares Horizontal
					_fieldBoolArray[(x + i), y] = true;
				}
			}
			else
			{
				for (int i = 0; i < shipLength; i++)
				{
					//Change Squares Vertical
					_fieldBoolArray[x, (y + i)] = true;
				}
			}
		}

		private void ChangeOrientation()
		{
			if(_lastClickedShip == null)
			{
				return;
			}

			if (_lastClickedShip.Orientation == Orientation.Horizontal)
			{
				_lastClickedShip.Orientation = Orientation.Vertical;
			}
			else
			{
				_lastClickedShip.Orientation = Orientation.Horizontal;
			}
		}


		RenderFragment BuildSvg(ShipDetails ship) => builder =>
		{
			int size = ship.Size;
			int squareSize = 30 / _fieldcnt;
			int shipIndex = _shipsToSet.IndexOf(ship);
			int width, height;

			if(ship.Orientation == Orientation.Horizontal)
			{
				width = size * squareSize;
				height = squareSize;
			}
			else
			{
				width = squareSize;
				height = size * squareSize;
			}	
	

			builder.OpenElement(0, "svg");
			builder.AddAttribute(1, "width", $"{width}vw");
			builder.AddAttribute(2, "height", $"{height}vw");

			builder.OpenElement(3, "rect");
			builder.AddAttribute(4, "width", $"{width}vw");
			builder.AddAttribute(5, "height", $"{height}vw");
			builder.AddAttribute(6, "onclick", EventCallback.Factory.Create(this, () => ShipClicked(ship)));
			builder.AddAttribute(7, "class", $"rectangle {(ship.IsClicked ? "clicked" : "")} {(ship.IsPlaced ? "placed" : "")}");
			builder.CloseElement();


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

	public interface IDialogService
	{
		Task ShowPopup(string message);
	}

	public class DialogService : IDialogService
	{
		public async Task ShowPopup(string message)
		{
			await Application.Current.MainPage.DisplayAlert("Alert", message, "OK");
		}
	}
}
