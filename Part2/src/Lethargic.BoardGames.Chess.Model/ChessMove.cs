using System;
using Lethargic.BoardGames.Model;

namespace Lethargic.BoardGames.Chess.Model {
	/// <summary>
	/// Represents a single move to be applied to a chess board.
	/// </summary>
	public class ChessMove : IGameMove, IEquatable<ChessMove> {
		/// <summary>
		/// The starting position of the move.
		/// </summary>
		public BoardPosition StartPosition { get; }

		/// <summary>
		/// The ending position of the move.
		/// </summary>
		public BoardPosition EndPosition { get; }

		/// <summary>
		/// The type of move being applied.
		/// </summary>
		public ChessMoveType MoveType { get; }

		/// <inheritdoc />
		public int Player { get; set; }

		/// <summary>
		/// Constructs a ChessMove that moves a piece from one position to another
		/// </summary>
		/// <param name="start">the starting position of the piece to move</param>
		/// <param name="end">the position where the piece will end up</param>
		/// <param name="moveType">the type of move represented</param>
		public ChessMove(BoardPosition start, BoardPosition end, ChessMoveType moveType = ChessMoveType.Normal) {
			StartPosition = start;
			EndPosition = end;
			MoveType = moveType;
		}

		bool IEquatable<IGameMove>.Equals(IGameMove other) {
			ChessMove m = other as ChessMove;
			return this.Equals(m);
		}

		public bool Equals(ChessMove other) {
			return StartPosition.Equals(other.StartPosition) 
				&& EndPosition.Equals(other.EndPosition)
				&& MoveType.Equals(other.MoveType);
		}
	}
}
