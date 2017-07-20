using System;
using System.Collections.Generic;
using System.Linq;
using Lethargic.BoardGames.Model;
using Lethargic.BoardGames.Othello.Model;
using Lethargic.BoardGames.Othello.View;
using Lethargic.BoardGames.View;

namespace Lethargic.BoardGames.ConsoleApp {
	class Program {
		public static void Main(string[] args) {
			IGameBoard board = new OthelloBoard();
			IConsoleView view = new OthelloConsoleView();

			while (!board.IsFinished) {
				Console.WriteLine(view.BoardToString(board));

				Console.WriteLine();
				Console.WriteLine("Possible moves:");
				IEnumerable<IGameMove> possMoves = board.GetPossibleMoves();
				Console.WriteLine(string.Join(",",
					possMoves.Select(view.MoveToString)));

				Console.WriteLine("It is {0}'s turn.",
					view.PlayerToString(board.CurrentPlayer));
				Console.WriteLine("Enter a move: ");
				string input = Console.ReadLine();
				IGameMove toApply = view.ParseMove(input);
				IGameMove foundMove = possMoves.FirstOrDefault(m => m.Equals(toApply));
				if (foundMove == null) {
					Console.WriteLine("Sorry, that move is invalid.");
				}
				else {
					board.ApplyMove(foundMove);
				}
			}
		}
	}
}