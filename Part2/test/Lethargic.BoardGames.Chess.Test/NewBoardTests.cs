using System;
using System.Collections.Generic;
using System.Text;
using FluentAssertions;
using Lethargic.BoardGames.Chess.Model;
using Xunit;

namespace Lethargic.BoardGames.Chess.Test {
	public class NewBoardTests : ChessTest {
		/// <summary>
		/// Simple facts about "new" boards.
		/// </summary>
		[Fact]
		public void NewChessBoard() {
			ChessBoard b = new ChessBoard();
			b.GetPieceAtPosition(Pos(7, 0)).Player.Should().Be(1, "Player 1 should be in lower left of board");
			b.GetPieceAtPosition(Pos(0, 0)).Player.Should().Be(2, "Player 2 should be in upper left of board");
			b.GetPieceAtPosition(Pos(4, 0)).Player.Should().Be(0, "Middle left of board should be empty");
			// Test a few select piece locations.
			b.GetPieceAtPosition(Pos(7, 4)).PieceType.Should().Be(ChessPieceType.King, "White's king at position (7,4)");
			b.GetPieceAtPosition(Pos(0, 4)).PieceType.Should().Be(ChessPieceType.King, "Black's king at position (0,4)");
			// Test other properties
			b.CurrentPlayer.Should().Be(1, "Player 1 starts the game");
			b.CurrentAdvantage.Should().Be(Advantage(0, 0), "no operations have changed the advantage");
		}


	}
}
