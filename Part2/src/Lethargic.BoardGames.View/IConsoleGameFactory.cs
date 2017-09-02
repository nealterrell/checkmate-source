using Lethargic.BoardGames.Model;

namespace Lethargic.BoardGames.View {
	/// <summary>
	/// A factory for building model and console views for a particular board game type.
	/// </summary>
	public interface IConsoleGameFactory {
		/// <summary>
		/// A human-appropriate name for the game.
		/// </summary>
		string GameName { get; }

		/// <summary>
		/// Constructs an IGameBoard-derived object for this type of game.
		/// </summary>
		IGameBoard CreateBoard();

		/// <summary>
		/// Constructs an IConsoleView-derived object for this type of game.
		/// </summary>
		IConsoleView CreateConsoleView();
	}
}
