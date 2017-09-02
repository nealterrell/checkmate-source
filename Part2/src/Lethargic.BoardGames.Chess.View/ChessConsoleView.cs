using System;
using Lethargic.BoardGames.Chess.Model;
using Lethargic.BoardGames.Model;
using Lethargic.BoardGames.View;

namespace Lethargic.BoardGames.Chess.View {
	/// <summary>
	/// A chess game view for string-based console input and output.
	/// </summary>
	public class ChessConsoleView : IConsoleView {
		// Explicit method implementations.
		string IConsoleView.BoardToString(IGameBoard board) {
			return BoardToString(board as ChessBoard);
		}

		string IConsoleView.MoveToString(IGameMove move) {
			return MoveToString(move as ChessMove);
		}

		IGameMove IConsoleView.ParseMove(string moveText) {
			return ParseMove(moveText);
		}

		// Public methods.
		public string BoardToString(ChessBoard board) {
			throw new NotImplementedException();
		}

		public string MoveToString(ChessMove move) {
			throw new NotImplementedException();
		}

		public string PlayerToString(int player) {
			throw new NotImplementedException();
		}

		public ChessMove ParseMove(string moveText) {
			throw new NotImplementedException();
		}

		// Static methods.
		public static BoardPosition ParsePosition(string pos) {
			return new BoardPosition(8 - (pos[1] - '0'), pos[0] - 'a');
		}
	}
}
