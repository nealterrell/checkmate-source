using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Lethargic.BoardGames.Chess.Model;
using Lethargic.BoardGames.Chess.View;
using Lethargic.BoardGames.Model;
using Lethargic.BoardGames.Test;

namespace Lethargic.BoardGames.Chess.Test {
	public class ChessTest : BoardGameTest<ChessBoard, ChessMove> {
		private ChessConsoleView mView = new ChessConsoleView();

		protected ChessMove Move(BoardPosition start, BoardPosition end, ChessMoveType type = ChessMoveType.Normal) {
			return new ChessMove(start, end, type);
		}

		protected ChessMove Move(string moveString) {
			return mView.ParseMove(moveString);
		}

		protected override string ToString(ChessBoard board) {
			return mView.BoardToString(board);
		}

		protected IEnumerable<ChessMove> GetMovesAtPosition(IEnumerable<ChessMove> moves, BoardPosition start) {
			return moves.Where(m => m.StartPosition == start);
		}

		protected BoardPosition Pos(string algebraicPosition) {
			return ChessConsoleView.ParsePosition(algebraicPosition);
		}

		protected ChessBoard CreateBoardFromMoves(params string[] moveStrings) {
			var board = new ChessBoard();
			Apply(board, moveStrings);
			return board;
		}

		protected void Apply(ChessBoard board, params string[] moveStrings) {
			Apply(board, moveStrings.Select(mView.ParseMove).ToArray());
		}
	}
}
