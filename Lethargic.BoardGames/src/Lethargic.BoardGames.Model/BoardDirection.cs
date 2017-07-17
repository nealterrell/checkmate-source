using System;
using System.Collections.Generic;

namespace Lethargic.BoardGames.Model {
	public struct BoardDirection : IEquatable<BoardDirection> {
		public sbyte RowDelta { get; private set; }
		public sbyte ColDelta { get; private set; }

		public BoardDirection(sbyte rowDelta, sbyte colDelta) {
			RowDelta = rowDelta;
			ColDelta = colDelta;
		}

		public bool Equals(BoardDirection other) {
			return RowDelta == other.RowDelta && ColDelta == other.ColDelta;
		}

		public static BoardDirection operator-(BoardDirection rhs) {
			return new BoardDirection((sbyte)-rhs.RowDelta, (sbyte)-rhs.ColDelta);
		}

		public static IEnumerable<BoardDirection> CardinalDirections { get; }
			= new BoardDirection[] {
				new BoardDirection(-1, -1),
				new BoardDirection(-1, 0),
				new BoardDirection(-1, 1),
				new BoardDirection(0, -1),
				new BoardDirection(0, 1),
				new BoardDirection(1, -1),
				new BoardDirection(1, 0),
				new BoardDirection(1, 1),
			};
	}
}
