using Lethargic.BoardGames.Chess.Model;
using Lethargic.BoardGames.Model;
using Lethargic.BoardGames.View;

namespace Lethargic.BoardGames.Chess.View {
	/// <summary>
	/// A factory for the game of chess.
	/// </summary>
	public class ChessConsoleGameFactory : IConsoleGameFactory {
		public string GameName => "Chess";
		public IGameBoard CreateBoard() => new ChessBoard();
		public IConsoleView CreateConsoleView() => new ChessConsoleView();
	}
}
