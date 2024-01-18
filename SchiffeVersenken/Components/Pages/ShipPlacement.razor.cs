using Microsoft.AspNetCore.Components;
using SchiffeVersenken.Data;
using SchiffeVersenken.Data.Model;
using Orientation = SchiffeVersenken.Data.Orientation;


namespace SchiffeVersenken.Components.Pages
{
	public partial class ShipPlacement
    {
		[CascadingParameter]
		public GameLogic Game { get; set; }

		private int _fieldcnt;
		private List<ShipDetails> _shipsPlaced = new List<ShipDetails>();
		private List<ShipDetails> _ships;
		private List<int> _shipSizes;
		private ShipDetails _lastClickedShip;

		private bool[,] _fieldBoolArray;
		
		protected override void OnInitialized()
		{
			StateHasChanged();
			_fieldcnt = Game._Size;
			setDefaultValues();
		}

		private void setDefaultValues()
		{
			_ships = shipsTemplate._Ships;
			_shipSizes = shipsTemplate._ShipSizes;
			_fieldBoolArray = new bool[_fieldcnt, _fieldcnt];
			_lastClickedShip = null;
			foreach (var ship in _ships)
			{
				ship.IsClicked = false;
				ship.IsPlaced = false;
				ship.PositionX = 0;
				ship.PositionY = 0;
				ship.Orientation = Orientation.Horizontal;
			}
			_shipsPlaced.Clear();
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
			_shipsPlaced.Add(_lastClickedShip);
			_lastClickedShip.PositionX = x;
			_lastClickedShip.PositionY = y;

			if (!Game._Player.CheckShips(_shipsPlaced))
			{
				_shipsPlaced.Remove(_lastClickedShip);
				DialogService.ShowPopup("Ship can't be placed there");
				return;
			}


			int shipLength = _lastClickedShip.Size;
			Orientation orientation = _lastClickedShip.Orientation;
			_lastClickedShip.IsPlaced = true;
			_lastClickedShip = null;

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
			int shipIndex = _ships.IndexOf(ship);
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
			builder.AddAttribute(7, "class", $"rectangle {(ship.IsClicked ? "clicked" : "")} {(ship.IsPlaced ? "placed" : "notPlaced")}");
			builder.CloseElement();


			builder.CloseElement();
		};


		private void OnClickResetAll()
		{
			setDefaultValues();
		}

		private void OnClickResetLastShip()
		{
			if(_shipsPlaced.Count > 0)
			{
				_lastClickedShip = null;
				ShipDetails ship = _shipsPlaced[_shipsPlaced.Count - 1];
				ship.IsClicked = false;
				ship.IsPlaced = false;
				

				Orientation orientation = ship.Orientation;
				int shipLength = ship.Size;
				int x = ship.PositionX;
				int y = ship.PositionY;

				if (orientation == Orientation.Horizontal)
				{
					for (int i = 0; i < shipLength; i++)
					{
						//Change Squares Horizontal
						_fieldBoolArray[(x + i), y] = false;
					}
				}
				else
				{
					for (int i = 0; i < shipLength; i++)
					{
						//Change Squares Vertical
						_fieldBoolArray[x, (y + i)] = false;
					}
				}

				_shipsPlaced.Remove(ship);
			}
		}

		private void GoToPreviousPage()
		{
			NavigationManager.NavigateTo("/PregameVsComputer", true);
		}

		private void GoToNextPage()
		{
			if (!Game._Player.SetShips(_shipsPlaced))
			{
				DialogService.ShowPopup("Error in ship Placement");
				return;
			}

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
