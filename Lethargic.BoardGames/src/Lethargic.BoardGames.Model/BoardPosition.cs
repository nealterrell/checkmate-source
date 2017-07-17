using System;
using System.Collections.Generic;
using System.Linq;

namespace Lethargic.BoardGames.Model {
	/// <summary>
	/// Represents a row/column position on a 2D grid board.
	/// </summary>
	public struct BoardPosition : IEquatable<BoardPosition> {
		/// <summary>
		/// The row of the position.
		/// </summary>
		public int Row { get; private set; }
		/// <summary>
		/// The column of the position.
		/// </summary>
		public int Col { get; private set; }

		public BoardPosition(int row, int col) {
			Row = row;
			Col = col;
		}

		/// <summary>
		/// Translates the BoardPosition by the given amount in the row and column directions, returning a new
		/// position.
		/// </summary>
		/// <param name="rDelta">the amount to change the new position's row by</param>
		/// <param name="cDelta">the amount to change the new position's column by</param>
		/// <returns>a new BoardPosition object that has been translated from the source</returns>
		public BoardPosition Translate(int rDelta, int cDelta) {
			return new BoardPosition(Row + rDelta, Col + cDelta);
		}

		/// <summary>
		/// Translates the BoardPosition by the given amount in the row and column directions, returning a new
		/// position.
		/// </summary>
		/// <param name="direction">a BoardDirection object giving the amount to change the new position's row and column by</param>
		/// <returns>a new BoardPosition object that has been translated from the source</returns>
		public BoardPosition Translate(BoardDirection direction) {
			return Translate(direction.RowDelta, direction.ColDelta);
		}

		// An overridden ToString makes debugging easier.
		public override string ToString() {
			return "(" + Row + ", " + Col + ")";
		}

		/// <summary>
		/// Two board positions are equal if they have the same row and column.
		/// </summary>
		/// <param name="other"></param>
		public bool Equals(BoardPosition other) {
			return Row == other.Row && Col == other.Col;
		}

		public static IEnumerable<BoardPosition> GetRectangularPositions(int rows, int cols) {
			return 
				from r in Enumerable.Range(0, 8)
				from c in Enumerable.Range(0, 8)
				select new BoardPosition(r, c);
		}
	}
}
