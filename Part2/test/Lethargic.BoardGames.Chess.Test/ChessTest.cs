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
		protected ChessConsoleView ConsoleView{ get; } = new ChessConsoleView();

		protected ChessMove Move(BoardPosition start, BoardPosition end, ChessMoveType type = ChessMoveType.Normal) =>
			new ChessMove(start, end, type);

		protected ChessMove Move(string moveString) =>
			ConsoleView.ParseMove(moveString);

		protected override string ToString(ChessBoard board) =>
			ConsoleView.BoardToString(board);

		protected BoardPosition Pos(string algebraicPosition) =>
			ChessConsoleView.ParsePosition(algebraicPosition);

		protected ChessBoard CreateBoardFromMoves(params string[] moveStrings) {
			var board = new ChessBoard();
			Apply(board, moveStrings);
			return board;
		}

		protected void Apply(ChessBoard board, params string[] moveStrings) {
			Apply(board, moveStrings.Select(ConsoleView.ParseMove).ToArray());
		}

		/// <summary>
		/// Filters the given list of moves to only those that start at the given position.
		/// </summary>
		protected IEnumerable<ChessMove> GetMovesAtPosition(IEnumerable<ChessMove> moves, BoardPosition start) =>
			moves.Where(m => m.StartPosition == start);



		/// <summary>
		/// Returns all chess piece positions controlled by the given player
		/// </summary>
		protected IEnumerable<ChessPiece> GetAllPiecesForPlayer(ChessBoard b, int player) =>
			BoardPosition.GetRectangularPositions(8, 8)
				.Select(b.GetPieceAtPosition)
				.Where(piece => piece.Player == player);

		protected IEnumerable<BoardPosition> GetPositionsInRank(int rank) {
			BoardPosition start = new BoardPosition(8 - rank, 0); // The leftmost position in the given rank.
			return Enumerable.Range(0, 8)
				.Select(c => start.Translate(0, c));
		}
	}
}
