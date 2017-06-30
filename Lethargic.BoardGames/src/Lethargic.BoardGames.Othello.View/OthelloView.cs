using System;
using Lethargic.BoardGames.Model;
using Lethargic.BoardGames.View;
using Lethargic.BoardGames.Othello.Model;
using System.Text;

namespace Lethargic.BoardGames.Othello.View {

	public class OthelloView : IConsoleView {
		public string BoardToString(OthelloBoard board) {
			StringBuilder str = new StringBuilder();
			str.AppendLine("- 0 1 2 3 4 5 6 7");
			for (int i = 0; i < OthelloBoard.BOARD_SIZE; i++) {
				str.Append(i);
				str.Append(" ");
				for (int j = 0; j < OthelloBoard.BOARD_SIZE; j++) {
					int space = board.GetPieceAtPosition(new BoardPosition(i, j));
					str.AppendFormat("{0} ", space == 0 ? '.' : space == 1 ? 'B' : 'W');
				}
				str.AppendLine();
			}
			return str.ToString();
		}
		
		public string MoveToString(OthelloMove move) {
			return "(" + move.Position.Row + ", " + move.Position.Col + ")";
		}

		public OthelloMove ParseMove(string moveText) {
			string[] split = moveText.Trim(new char[] { '(', ')' }).Split(',');
			return new OthelloMove(new BoardPosition(Convert.ToInt32(split[0]), Convert.ToInt32(split[1])));
		}

		public string PlayerToString(int player) {
			return player == 1 ? "Black" : "White";
		}

		string IConsoleView.BoardToString(IGameBoard board) {
			return BoardToString(board as OthelloBoard);
		}

		string IConsoleView.MoveToString(IGameMove move) {
			return MoveToString(move as OthelloMove);
		}

		IGameMove IConsoleView.ParseMove(string moveText) {
			return ParseMove(moveText);
		}
	}
}
