using System;

namespace Lethargic.BoardGames.Model {
	public interface IGameMove : IEquatable<IGameMove> {
		/// <summary>
		/// The player that applied this move, if the move has been applied to a board. If it has not,
		/// this property is undefined.
		/// </summary>
		int Player { get; }
	}
}
