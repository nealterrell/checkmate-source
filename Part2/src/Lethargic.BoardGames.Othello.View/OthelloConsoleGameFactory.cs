using System;
using System.Collections.Generic;
using System.Text;
using Lethargic.BoardGames.Model;
using Lethargic.BoardGames.Othello.Model;
using Lethargic.BoardGames.View;

namespace Lethargic.BoardGames.Othello.View {
	/// <summary>
	/// A factory for the game of Othello.
	/// </summary>
	public class OthelloConsoleGameFactory : IConsoleGameFactory {
		public string GameName => "Othello";
		public IGameBoard CreateBoard() => new OthelloBoard();
		public IConsoleView CreateConsoleView() => new OthelloConsoleView();
	}
}
