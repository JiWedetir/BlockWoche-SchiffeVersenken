using Microsoft.AspNetCore.Components;
using SchiffeVersenken.Data;
using SchiffeVersenken.Data.Model;
using SchiffeVersenken.Data.Sea;
using Orientation = SchiffeVersenken.Data.Orientation;
using SchiffeVersenken.Components.Shared;
using MudBlazor;


namespace SchiffeVersenken.Components.Pages
{
    public partial class ShipPlacement
    {
		[CascadingParameter]
		public GameLogicService GameService { get; set; }

		private int _fieldcnt;
		private List<ShipDetails> _shipsPlaced = new List<ShipDetails>();
		private List<ShipDetails> _ships;
		private List<int> _shipSizes;
		private ShipDetails _lastClickedShip;

		private Square[,] _board = null;
		private string _bgUrl = "url('../images/backgroundshipplacement.png')";

		protected override void OnInitialized()
		{
			StateHasChanged();
			_fieldcnt = GameService.Game._Size;
			setDefaultValues();
			CreateField();

		}


		public void CreateField()
		{
			_board = new Square[_fieldcnt, _fieldcnt];
			for (int i = 0; i < _fieldcnt; i++)
			{
				for (int j = 0; j < _fieldcnt; j++)
				{
					_board[i, j] = new Square();
					_board[i, j].SetToEmptySquare();
				}
			}
		}

		private void setDefaultValues()
		{
			_ships = shipsTemplate._Ships;
			_shipSizes = shipsTemplate._ShipSizes;
			_lastClickedShip = null;
			foreach (var ship in _ships)
			{
				ship.IsClicked = false;
				ship.IsPlaced = false;
				ship.PositionX = 0;
				ship.PositionY = 0;
				ship.Orientation = Orientation.Vertical;
			}
			_shipsPlaced.Clear();
			CreateField();
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

		private void OnSquareClick(int[] coords)
		{
			int x = coords[0];
			int y = coords[1];

			if (_lastClickedShip == null)
			{
				return;
			}

			_shipsPlaced.Add(_lastClickedShip);
			_lastClickedShip.PositionX = x;
			_lastClickedShip.PositionY = y;

			if (!GameService.Game._Player.CheckShips(_shipsPlaced))
			{
				_shipsPlaced.Remove(_lastClickedShip);
				OpenDialog("Ship can't be placed there");
				return;
			}

			
			int shipLength = _lastClickedShip.Size;
			Orientation orientation = _lastClickedShip.Orientation;
			_lastClickedShip.IsPlaced = true;
			_lastClickedShip.IsClicked = false;

			if (orientation == Orientation.Horizontal)
			{
				for (int i = 0; i < shipLength; i++)
				{
					//Change Squares Horizontal
					_board[(x + i), y]._State = SquareState.Ship;

				}
			}
			else
			{
				for (int i = 0; i < shipLength; i++)
				{
					//Change Squares Vertical
					_board[x, (y + i)]._State = SquareState.Ship;
				}
			}

			_lastClickedShip = null;
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


		RenderFragment PlaceShipSVG(ShipDetails ship) => builder =>
		{
			int size = ship.Size;
			int shipIndex = _ships.IndexOf(ship);
			Orientation orientation = ship.Orientation;

			string href = string.Empty;
			string height = string.Empty;

			switch (size)
			{
				case 5:
					href = "./images/ships/carrier.svg#carrier";
					height = "100%";
					break;
				case 4:
					href = "./images/ships/battleship.svg#battleship";
					height = "90%";
					break;
				case 3:
					href = "./images/ships/cruiser.svg#cruiser";
					height = "80%";
					break;
				case 2:
					href = "./images/ships/destroyer.svg#destroyer";
					height = "70%";
					break;
			}

			string transformStyle = orientation == Orientation.Horizontal ? "transform: rotate(90deg);" : string.Empty;

			string style = $"flex: 1 1 auto; height:{height}; {transformStyle}";

			builder.OpenElement(0, "svg");
			builder.AddAttribute(1, "style", style);
			builder.AddAttribute(2, "onclick", EventCallback.Factory.Create(this, () => ShipClicked(ship)));
			builder.AddAttribute(3, "class", $"ship {(ship.IsClicked ? "clicked" : "")} {(ship.IsPlaced ? "placed" : "notPlaced")}");
			builder.OpenElement(4, "use");
			builder.AddAttribute(5, "href", href);
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
						_board[(x + i), y]._State = SquareState.Empty;
					}
				}
				else
				{
					for (int i = 0; i < shipLength; i++)
					{
						//Change Squares Vertical
						_board[x, (y + i)]._State = SquareState.Empty;
					}
				}

				_shipsPlaced.Remove(ship);
			}
		}


		private void GoToNextPage()
		{
			if (!GameService.Game._Player.SetShips(_shipsPlaced))
			{
				OpenDialog("Error in ship Placement");
				return;
			}

			NavigationManager.NavigateTo("/Game", true);
		}

		private void OpenDialog(string message)
		{
			DialogParameters parameters = new DialogParameters { { "ContentText", message } };
			DialogService.Show<ShipPlacementDialog>("", parameters);
		}
	}
}
