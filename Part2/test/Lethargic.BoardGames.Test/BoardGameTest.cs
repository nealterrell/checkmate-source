using System;
using Lethargic.BoardGames.Model;
using FluentAssertions;
using System.Linq;
using System.Collections.Generic;

namespace Lethargic.BoardGames.Test {
	public abstract class BoardGameTest<TBoard, TMove>
		where TBoard : IGameBoard, new()
		where TMove : IGameMove {
		protected abstract string ToString(TBoard board);

		protected BoardPosition Pos(int row, int col) {
			return new BoardPosition(row, col);
		}

		protected GameAdvantage Advantage(int player, int advantage) {
			return new GameAdvantage(player, advantage);
		}

		private void ApplyToBoard(TBoard board, TMove move) {
			var currentAdvantage = board.CurrentAdvantage;
			var possMoves = board.GetPossibleMoves();
			board.CurrentAdvantage.Should().Be(currentAdvantage,
				"the board's value should not change after calling GetPossibleMoves");

			var toApply = possMoves.FirstOrDefault(move.Equals);
			if (toApply == null) {
				throw new InvalidOperationException("Could not apply the move " + move + " to the board\n" +
					ToString(board));
			}
			else {
				board.ApplyMove(toApply);
			}
		}

		protected void Apply(TBoard board, params TMove[] moves) {
			foreach (var move in moves) {
				ApplyToBoard(board, move);
			}
		}

		protected TBoard CreateBoardFromMoves(params TMove[] moves) {
			TBoard board = new TBoard();
			Apply(board, moves);
			return board;
		}
	}
}
