using System;
using System.Collections.Generic;
using Lethargic.BoardGames.Model;

namespace Lethargic.BoardGames.Othello.Model {
	/// <summary>
	/// Represents a single move that can be or has been applied to an OthelloBoard object.
	/// </summary>
	public class OthelloMove : IGameMove, IEquatable<OthelloMove> {
		public int Player { get; set; }

		/// <summary>
		/// True if the move represents a "pass".
		/// </summary>
		public bool IsPass =>
			Position.Row == -1 && Position.Col == -1; 

		/// <summary>
		/// The position of the move.
		/// </summary>
		public BoardPosition Position { get; }

		/// <summary>
		/// Initializes a new OthelloMove instance representing the given board position.
		/// </summary>
		public OthelloMove(BoardPosition pos) {
			Position = pos;
		}
		
		public override bool Equals(object obj) {
			return Equals(obj as OthelloMove);
		}

		public override int GetHashCode() =>
			Position.GetHashCode();

		/// <summary>
		/// Returns true if the two objects have the same position.
		/// </summary>
		public bool Equals(IGameMove obj) {
			return Equals(obj as OthelloMove);
		}

		public bool Equals(OthelloMove other) {
			return other != null && Position.Equals(other.Position);
		}
		
		// For debugging.
		public override string ToString() {
			return Position.ToString();
		}
	}
}
