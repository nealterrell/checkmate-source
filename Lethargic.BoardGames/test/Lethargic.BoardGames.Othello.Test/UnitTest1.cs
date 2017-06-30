using System;
using FluentAssertions;
using Lethargic.BoardGames.Othello.Model;
using Xunit;

namespace Lethargic.BoardGames.Othello.Test {
	public class UnitTest1 {
		[Fact]
		public void Test1() {
			OthelloBoard b = new OthelloBoard();
			b.GetPieceAtPosition(new BoardGames.Model.BoardPosition(3, 3)).Should().Be(2);
		}
	}
}
