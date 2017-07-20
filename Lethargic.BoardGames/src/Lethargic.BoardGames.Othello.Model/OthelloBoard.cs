using System;
using System.Collections.Generic;
using System.Linq;
using Lethargic.BoardGames.Model;

namespace Lethargic.BoardGames.Othello.Model {
	/// <summary>
	/// Implements the board model for a game of Othello. Tracks which squares of the 8x8 grid are occupied
	/// by which player, as well as state for the current player and move history.
	/// </summary>
	public class OthelloBoard : IGameBoard {
		private struct FlipSet {
			public BoardDirection Direction { get; set; }
			public sbyte Count { get; set; }
		} 

		public const int BOARD_SIZE = 8;

		// The board is represented by an 8x8 matrix of signed bytes. Each entry represents one square on the board.
		private sbyte[,] mBoard = {
			{9, 9, 9, 9, 9, 9, 9, 9, 9, 9},
			{9, 0, 0, 0, 0, 0, 0, 0, 0, 0},
			{9, 0, 0, 0, 0, 0, 0, 0, 0, 0},
			{9, 0, 0, 0, 0, 0, 0, 0, 0, 0},
			{9, 0, 0, 0, -1, 1, 0, 0, 0, 0},
			{9, 0, 0, 0, 1, -1, 0, 0, 0, 0},
			{9, 0, 0, 0, 0, 0, 0, 0, 0, 0},
			{9, 0, 0, 0, 0, 0, 0, 0, 0, 0},
			{9, 0, 0, 0, 0, 0, 0, 0, 0, 0},
			{9, 9, 9, 9, 9, 9, 9, 9, 9, 9},
		};

		// Internally, we will represent pieces for each player as 1 or -1 (for player 2), which makes certain game 
		// operations easier to code. Those values don't make sense to the public, however, so we will expose them in a 
		// public property by mapping -1 to a value of 2. This will reduce coupling between other components and the 
		// private model logic.
		private int mCurrentPlayer;

		/// <summary>
		/// Constructs an othello board in the starting game state.
		/// </summary>
		public OthelloBoard() {
			mCurrentPlayer = 1;
		}

		/// <summary>
		/// The player whose move it is.
		/// </summary>
		public int CurrentPlayer {
			get {
				return mCurrentPlayer == 1 ? 1 : 2;
			}
		}

		/// <summary>
		/// How many "pass" moves have been applied in a row.
		/// </summary>
		public int PassCount { get; private set; }

		// Move history state and properties
		private List<OthelloMove> mMoveHistory = new List<OthelloMove>();
		private List<List<FlipSet>> mFlipSets = new List<List<FlipSet>>();

		IReadOnlyList<IGameMove> IGameBoard.MoveHistory => mMoveHistory;
		public IReadOnlyList<OthelloMove> MoveHistory => mMoveHistory;


		public bool IsFinished => PassCount == 2;


		// Game advantage state and properties
		private int mAdvantageValue;
		private GameAdvantage mAdvantage;
		public GameAdvantage CurrentAdvantage => mAdvantage;

		// This is how we will expose the state of the gameboard in a way that reduces coupling.
		// No one needs to know HOW the data is represented; they simply need to know which player is
		// at which position.
		/// <summary>
		/// Returns an integer representing which player has a piece at the given position, or 0 if the position
		/// is empty.
		/// </summary>
		public int GetPlayerAtPosition(BoardPosition boardPosition) {
			sbyte pos = mBoard[boardPosition.Row + 1, boardPosition.Col + 1];
			if (pos == -1) { // -1 maps to player 2.
				return 2;
			}
			if (pos == 9) { // out of bounds
				return -1;
			}
			return pos; // otherwise the value is correct
		}

		private void SetAdvantage() {
			mAdvantage = new GameAdvantage(mAdvantageValue > 0 ? 1 : mAdvantageValue < 0 ? 2 : 0,
				Math.Abs(mAdvantageValue));
		}

		private void SetPlayerAtPosition(BoardPosition position, int player) {
			mBoard[position.Row + 1, position.Col + 1] = (sbyte)(player <= 1 ? player : -1);
		}

		/// <summary>
		/// Returns true if the given in-bounds position is an enemy of the given player.
		/// </summary>
		/// <param name="pos">assumed to be in bounds</param>
		private bool PositionIsEnemy(BoardPosition pos, int player) => GetPlayerAtPosition(pos) + player == 3;

		private bool PositionIsEmpty(BoardPosition position) => GetPlayerAtPosition(position) == 0;

		/// <summary>
		/// Applies the given move to the board state.
		/// </summary>
		/// <param name="m">a move that is assumed to be valid</param>
		void IGameBoard.ApplyMove(IGameMove move) {
			ApplyMove(move as OthelloMove);
		}

		public void ApplyMove(OthelloMove m) {
			if (m == null) {
				throw new ArgumentNullException(nameof(m));
			}

			List<FlipSet> currentFlips = new List<FlipSet>();
			// If the move is a pass, then we do very little.
			if (m.IsPass) {
				PassCount++;
			}
			else {
				PassCount = 0;
				// Otherwise update the board at the move's position with the current player.
				SetPlayerAtPosition(m.Position, CurrentPlayer);
				mAdvantageValue += mCurrentPlayer;

				// Iterate through all 8 directions radially from the move's position.
				foreach (BoardDirection dir in BoardDirection.CardinalDirections) {
					// Repeatedly move in the selected direction, as long as we find "enemy" squares.
					BoardPosition newPos = m.Position;
					int steps = 0;
					do {
						newPos = newPos.Translate(dir);
						steps++;
					} while (PositionIsEnemy(newPos, CurrentPlayer));

					// This is a valid direction of flips if we moved at least 2 squares, and ended in bounds and on a
					// "friendly" square.
					if (steps > 1 && GetPlayerAtPosition(newPos) == CurrentPlayer) {
						// Record a FlipSet for this direction
						currentFlips.Add(new FlipSet() {
							Direction = dir,
							Count = (sbyte)(steps - 1)
						});

						var reverse = -dir;
						// Repeatedly walk back the way we came, updating the board with the current player's piece.
						do {
							newPos = newPos.Translate(reverse);
							SetPlayerAtPosition(newPos, CurrentPlayer);
							mAdvantageValue += 2 * mCurrentPlayer;
							
							steps--;
						}
						while (steps > 1);
					}
				}
			}

			// Update the rest of the board state.
			mCurrentPlayer = -mCurrentPlayer;
			SetAdvantage();
			mMoveHistory.Add(m);
			mFlipSets.Add(currentFlips);
		}

		/// <summary>
		/// Returns an enumeration of moves that would be valid to apply to the current board state.
		/// </summary>
		/// <returns></returns>
		public IEnumerable<OthelloMove> GetPossibleMoves() {
			var moves = new List<OthelloMove>();

			foreach (BoardPosition position in BoardPosition.GetRectangularPositions(BOARD_SIZE, BOARD_SIZE)) {
				if (!PositionIsEmpty(position)) {
					continue;
				}

				// Iterate through all 8 cardinal directions from the current position.
				foreach (BoardDirection dir in BoardDirection.CardinalDirections) {
					// Repeatedly move in the selected direction, as long as we find "enemy" squares.
					BoardPosition newPos = position;
					int steps = 0;
					do {
						newPos = newPos.Translate(dir);
						steps++;
					} while (PositionIsEnemy(newPos, CurrentPlayer));

					// This is a valid direction of flips if we moved at least 2 squares, and ended in bounds and on a
					// "friendly" square.
					if (steps > 1 && GetPlayerAtPosition(newPos) == CurrentPlayer) {
						moves.Add(new OthelloMove(position));
						break;
					}
				}
				// If the current position is valid, yield a move at the position.
			}

			// If no positions were valid, return a "pass" move.
			if (moves.Count == 0) {
				moves.Add(new OthelloMove(new BoardPosition(-1, -1)));
			}

			return moves;
		}


		/// <summary>
		/// Undoes the last move, restoring the game to its state before the move was applied.
		/// </summary>
		public void UndoLastMove() {
			OthelloMove m = mMoveHistory.Last();

			// Note: there is a bug in this code.
			if (!m.IsPass) {
				// Reset the board at the move's position.
				SetPlayerAtPosition(m.Position, 0);

				// Iterate through the move's recorded flipsets.
				foreach (var flipSet in mFlipSets.Last()) {
					BoardPosition pos = m.Position;
					// For each flipset, walk along the flipset's direction resetting pieces.
					for (int i = 1; i <= flipSet.Count; i++) {
						pos = pos.Translate(flipSet.Direction);
						// At this moment, CurrentPlayer is actually the enemy of the move that
						// we are undoing, whose pieces we must restore.
						SetPlayerAtPosition(pos, CurrentPlayer);
					}
				}

				// Check to see if the second-to-last move was a pass; if so, set PassCount.
				if (mMoveHistory.Count > 1 && mMoveHistory[mMoveHistory.Count - 2].IsPass) {
					PassCount = 1;
				}
			}
			else {
				PassCount--;
			}
			// Reset the remaining game state.
			SetAdvantage();
			mCurrentPlayer = -mCurrentPlayer;
			mMoveHistory.RemoveAt(mMoveHistory.Count - 1);
			mFlipSets.RemoveAt(mFlipSets.Count - 1);
		}

		IEnumerable<IGameMove> IGameBoard.GetPossibleMoves() {
			return GetPossibleMoves();
		}

		public static void Main() {
			OthelloBoard b = new OthelloBoard();
			IGameBoard gb = b;
		}
	}
}