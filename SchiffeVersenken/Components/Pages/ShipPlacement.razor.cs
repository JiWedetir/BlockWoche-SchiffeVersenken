using Microsoft.AspNetCore.Components;
using MudBlazor;
using SchiffeVersenken.Components.Shared;
using SchiffeVersenken.Data;
using SchiffeVersenken.Data.Sea;
using Orientation = SchiffeVersenken.Data.Orientation;


namespace SchiffeVersenken.Components.Pages
{
	public partial class ShipPlacement
    {
		// Gets the game logic service instance, cascaded from a parent component
		[CascadingParameter]
		public GameLogicService GameService { get; set; }

		// Background image URL for the ship placement page
		private string bgUrl = "url('../images/backgroundshipplacement.png')";

		// List of ship sizes used for generating ship placement options
		private List<int> _shipSizes = new List<int>();
		// List of ship details including size to be placed
		private List<ShipDetails> _ships = new List<ShipDetails>();

		// List of placed ships
		private List<ShipDetails> _shipsPlaced = new List<ShipDetails>();
		// The last ship selected by the user
		private ShipDetails _lastClickedShip;
		// The size of the game field
		private int _fieldcnt;

		// Instance of board for BattleFieldComponent
		private Square[,] _board = null;

		/// <summary>
		/// Initializes the component by setting up the game field and default ship placements
		/// </summary>
		protected override void OnInitialized()
		{
			StateHasChanged();
			_fieldcnt = GameService.Game._Size;
			setDefaultValues();
			CreateField();
		}

		/// <summary>
		/// Resets the ships to their default values and prepares the game board
		/// </summary>
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

		/// <summary>
		/// Generates the game board based on the specified field count
		/// </summary>
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

		/// <summary>
		/// Toggles the selection status of a ship when clicked.
		/// </summary>
		/// <param name="ship">The ship that was clicked.</param>
		private void ShipClicked(ShipDetails ship)
		{
			if (_lastClickedShip == null)
			{
				ship.IsClicked = !ship.IsClicked;
			}
			else if (_lastClickedShip == ship)
			{
				ChangeOrientation();
			}
			else
			{
				_lastClickedShip.IsClicked = false;
				ship.IsClicked = true;
			}
			_lastClickedShip = ship;
		}

		/// <summary>
		/// Handles the placement of a ship on the game board.
		/// </summary>
		/// <param name="coords">The coordinates where the ship is to be placed.</param>
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
                OpenDialogPlacement("Ship can't be placed there");
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

		/// <summary>
		/// Changes the orientation of the currently selected ship.
		/// </summary>
		private void ChangeOrientation()
		{
			if (_lastClickedShip == null)
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

		/// <summary>
		/// Resets all ships to their default placement values.
		/// </summary>
		private void OnClickResetAll()
		{
			setDefaultValues();
		}

		/// <summary>
		/// Removes the last placed ship from the board.
		/// </summary>
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

		/// <summary>
		/// Finalizes the ship placement and navigates to the game page.
		/// </summary>
		private void GoToNextPage()
		{
			if (!GameService.Game._Player.SetShips(_shipsPlaced))
			{
                OpenDialogPlacement("Error in ship Placement");
				return;
			}

			NavigationManager.NavigateTo("/Game", true);
		}

		/// <summary>
		/// Opens a PopUp with a specific message.
		/// </summary>
		/// <param name="message">The message to display in the PopUp.</param>
		private void OpenDialogPlacement(string message)
		{
			DialogParameters parameters = new DialogParameters { { "ContentText", message } };
			DialogService.Show<ShipPlacementDialog>("", parameters);
		}

		/// <summary>
		/// Opens a Info PopUp with the given text
		/// </summary>
		private void OpenDialogInfo()
		{
			string text =	$"<b>Ship Placement Instructions:</b>\n" +
							$"&#8226; You can select a ship you want to place by simply clicking it, if a ship is selected it is red, and when placed gray.\n" +
							$"&#8226; If a one or more ships are placed, you can reset one or all ships by clicking the accordingly named ships.\n" +
							$"&#8226; A ship can be rotates by clicking the selected ship again or clicking the rotate button.\n" +
							$"&#8226; When all ships are clicked, you can proceed to the game by the now clickable 'Start the Battle' Button.";
            DialogParameters parameters = new DialogParameters { { "ContentText", text } };
            DialogService.Show<InfoDialog>("", parameters);
        }

		/// <summary>
		/// Generates a RenderFragment for displaying an SVG representation of a ship.
		/// </summary>
		/// <param name="ship">The ship details.</param>
		/// <returns>A RenderFragment for the ship's SVG.</returns>
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
	}
}
