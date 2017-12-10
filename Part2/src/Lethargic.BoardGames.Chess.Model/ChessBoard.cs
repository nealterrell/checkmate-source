using System;
using System.Collections.Generic;
using System.Text;
using Lethargic.BoardGames.Model;

namespace Lethargic.BoardGames.Chess.Model {
	/// <summary>
	/// Represents the board state of a game of chess. Tracks which squares of the 8x8 board are occupied
	/// by which player's pieces.
	/// </summary>
	public class ChessBoard : IGameBoard {
		#region Member variables.
		// The history of moves applied to the board.
		private List<ChessMove> mMoveHistory = new List<ChessMove>();

		// TODO: Decide on a board representation, and implement an appropriate member variable.

		// TODO: Add a means of tracking miscellaneous board state, like captured pieces and the 50-move rule.
		#endregion

		#region Auto properties.
		public int CurrentPlayer { get; private set; }

		public GameAdvantage CurrentAdvantage { get; private set; }
		#endregion

		#region Computed properties
		public bool IsFinished {
			get { throw new NotImplementedException(); }
		}

		public IReadOnlyList<ChessMove> MoveHistory => mMoveHistory;

		public bool IsCheck {
			get { throw new NotImplementedException(); }
		}

		public bool IsCheckmate {
			get { throw new NotImplementedException(); }
		}

		public bool IsStalemate {
			get { throw new NotImplementedException(); }
		}
		#endregion


		#region Public methods.
		public IEnumerable<ChessMove> GetPossibleMoves() {
			throw new NotImplementedException();
		}

		public void ApplyMove(ChessMove m) {
			throw new NotImplementedException();
		}

		public void UndoLastMove() {
			throw new NotImplementedException();
		}

		/// <summary>
		/// Returns whatever chess piece is occupying the given position.
		/// </summary>
		public ChessPiece GetPieceAtPosition(BoardPosition pos) {
			throw new NotImplementedException();
		}

		/// <summary>
		/// Returns all board positions where the given piece can be found.
		/// </summary>
		public IEnumerable<BoardPosition> GetPositionsOfPiece(ChessPiece piece) {
			throw new NotImplementedException();
		}
		#endregion

		#region Private methods.
		/// <summary>
		/// Mutates the board state so that the given piece is at the given position.
		/// </summary>
		private void SetPieceAtPosition(BoardPosition pos, ChessPiece piece) {
			throw new NotImplementedException();
		}
		#endregion

		#region Explicit IGameBoard implementations.
		IEnumerable<IGameMove> IGameBoard.GetPossibleMoves() {
			return GetPossibleMoves();
		}
		void IGameBoard.ApplyMove(IGameMove m) {
			ApplyMove(m as ChessMove);
		}
		IReadOnlyList<IGameMove> IGameBoard.MoveHistory => mMoveHistory;
		#endregion

	}
}
