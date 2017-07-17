using System;
using System.Collections.Generic;
using Lethargic.BoardGames.Model;

namespace Lethargic.BoardGames.Othello.Model {
	/// <summary>
	/// Represents a single move that can be or has been applied to an OthelloBoard object.
	/// </summary>
	public class OthelloMove : IGameMove, IEquatable<OthelloMove> {
		/// <summary>
		/// True if the move represents a "pass".
		/// </summary>
		public bool IsPass {
			get { return Position.Row == -1 && Position.Col == -1; }
		}

		/// <summary>
		/// The position of the move.
		/// </summary>
		public BoardPosition Position { get; private set; }

		/// <summary>
		/// Initializes a new OthelloMove instance representing the given board position.
		/// </summary>
		public OthelloMove(BoardPosition pos) {
			Position = pos;
		}

		/// <summary>
		/// Returns true if the two objects have the same position.
		/// </summary>
		public bool Equals(IGameMove obj) {
			OthelloMove other = obj as OthelloMove;
			return other.Position.Row == this.Position.Row && other.Position.Col == this.Position.Col;
		}

		public bool Equals(OthelloMove other) {
			return other.Position.Row == this.Position.Row && other.Position.Col == this.Position.Col;
		}

		// Any time you override Equals you should also override GetHashCode, which is used in hashing data structures
		// to find hash buckets for an object.
		public override int GetHashCode() {
			return Position.GetHashCode();
		}

		public override string ToString() {
			return Position.ToString();
		}
	}
}
