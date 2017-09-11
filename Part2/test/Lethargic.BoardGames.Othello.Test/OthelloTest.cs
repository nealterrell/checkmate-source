using System;
using System.Collections.Generic;
using System.Text;
using Lethargic.BoardGames.Model;
using Lethargic.BoardGames.Othello.Model;
using Lethargic.BoardGames.Othello.View;
using Lethargic.BoardGames.Test;

namespace Lethargic.BoardGames.Othello.Test {
	public class OthelloTest : BoardGameTest<OthelloBoard, OthelloMove, OthelloConsoleView> {

		protected OthelloMove Move(BoardPosition pos) {
			return new OthelloMove(pos);
		}

		protected OthelloMove Move(int row, int col) {
			return Move(Pos(row, col));
		}
	}
}
