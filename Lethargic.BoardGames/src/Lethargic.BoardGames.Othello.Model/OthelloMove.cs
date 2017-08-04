using System;
using System.Collections.Generic;
using Lethargic.BoardGames.Model;

namespace Lethargic.BoardGames.Othello.Model {
	/// <summary>
	/// Represents a single move that can be or has been applied to an OthelloBoard object.
	/// </summary>
	public class OthelloMove : IGameMove, IEquatable<OthelloMove> {
		public int Player { get; private set; }

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
		public OthelloMove(int player, BoardPosition pos) {
			Player = player;
			Position = pos;
		}

		public override bool Equals(object obj) {
			return Equals(obj as OthelloMove);
		}

		/// <summary>
		/// Returns true if the two objects have the same position.
		/// </summary>
		public bool Equals(IGameMove obj) {
			return Equals(obj as OthelloMove);
		}

		public bool Equals(OthelloMove other) {
			if (other != null)
				return Position.Equals(other.Position);
			return false;
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
