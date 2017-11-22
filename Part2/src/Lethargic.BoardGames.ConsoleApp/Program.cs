#define GAME_CHAPTER_7

using System;
using System.Collections.Generic;
using System.Linq;
using Lethargic.BoardGames.Chess.Model;
using Lethargic.BoardGames.Chess.View;
using Lethargic.BoardGames.Model;
using Lethargic.BoardGames.Othello.Model;
using Lethargic.BoardGames.Othello.View;
using Lethargic.BoardGames.View;

namespace Lethargic.BoardGames.ConsoleApp {
	class Program {
		public static void Main(string[] args) {
			// Since Part II presents three different Main methods, we'll separate the 
			// three and let you select them by changing the #define at the top of this file.
#if GAME_CHAPTER_7
			Chapter7Game();
#elif GAME_CHAPTER_8
			Chapter8Game();
#elif GAME_CHAPTER_10
			Chapter10Game();
#endif
		}

		// The Main presented in the Othello implementation chapter.
		private static void Chapter7Game() {
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
				IGameMove foundMove = possMoves.SingleOrDefault(toApply.Equals);
				if (foundMove == null) {
					Console.WriteLine("Sorry, that move is invalid.");
				}
				else {
					board.ApplyMove(foundMove);
				}
				Console.WriteLine();
				Console.WriteLine();
			}
		}

		// The Main expanded at the end of the Othello chapter, with commands
		// for controlling the game.
		public static void Chapter8Game() {
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
				Console.WriteLine("Enter a command: ");
				string input = Console.ReadLine();

				if (input.StartsWith("move")) {
					IGameMove toApply = view.ParseMove(input.Substring(5));
					IGameMove foundMove = possMoves.SingleOrDefault(toApply.Equals);
					if (foundMove == null) {
						Console.WriteLine("Sorry, that move is invalid.");
					}
					else {
						board.ApplyMove(foundMove);
					}
				}
				else if (input.StartsWith("undo")) {
					if (!int.TryParse(input.Split(' ')[1], out int undoCount)) {
						undoCount = 1;
					}
					for (int i = 0; i < undoCount && board.MoveHistory.Count > 0; i++) {
						board.UndoLastMove();
					}
				}
				else if (input.StartsWith("history")) {
					Console.WriteLine("Move history:");
					Console.WriteLine(string.Join(Environment.NewLine,
						board.MoveHistory.Reverse().Select(
							m => view.PlayerToString(m.Player) + ":" + view.MoveToString(m))));
				}
				else if (input.StartsWith("advantage")) {
					var adv = board.CurrentAdvantage;
					if (adv.Player == 0) {
						Console.WriteLine("No player has an advantage.");
					}
					else {
						Console.WriteLine("{0} has an advantage of {1}.",
							view.PlayerToString(adv.Player), adv.Advantage);
					}
				}

				Console.WriteLine();
				Console.WriteLine();
			}
		}

		// The main for the end of the chess chapter, using abstract factories to select a game.
		public static void Chapter10Game() {
			var gameFactories = new IConsoleGameFactory[] {
				new ChessConsoleGameFactory(),
				new OthelloConsoleGameFactory(),
			};

			var gameMenuOptions = gameFactories.Zip(
				Enumerable.Range(1, gameFactories.Length),
				(game, i) => i + ". " + game.GameName);
			Console.WriteLine("Choose a game to play:");
			Console.WriteLine(string.Join(Environment.NewLine, gameMenuOptions));
			int menuChoice = Convert.ToInt32(Console.ReadLine());
			Console.WriteLine();

			var factory = gameFactories[menuChoice - 1];
			IGameBoard board = factory.CreateBoard();
			IConsoleView view = factory.CreateConsoleView();

			while (!board.IsFinished) {
				Console.WriteLine(view.BoardToString(board));

				Console.WriteLine();
				Console.WriteLine("Possible moves:");
				IEnumerable<IGameMove> possMoves = board.GetPossibleMoves();
				Console.WriteLine(string.Join(",",
					possMoves.Select(view.MoveToString)));

				Console.WriteLine("It is {0}'s turn.",
					view.PlayerToString(board.CurrentPlayer));
				Console.WriteLine("Enter a command: ");
				string input = Console.ReadLine();

				if (input.StartsWith("move")) {
					IGameMove toApply = view.ParseMove(input.Substring(5));
					IGameMove foundMove = possMoves.SingleOrDefault(toApply.Equals);
					if (foundMove == null) {
						Console.WriteLine("Sorry, that move is invalid.");
					}
					else {
						board.ApplyMove(foundMove);
					}
				}
				else if (input.StartsWith("undo")) {
					if (!int.TryParse(input.Split(' ')[1], out int undoCount)) {
						undoCount = 1;
					}
					for (int i = 0; i < undoCount && board.MoveHistory.Count > 0; i++) {
						board.UndoLastMove();
					}
				}
				else if (input.StartsWith("history")) {
					Console.WriteLine("Move history:");
					Console.WriteLine(string.Join(Environment.NewLine,
						board.MoveHistory.Reverse().Select(
							m => view.PlayerToString(m.Player) + ":" + view.MoveToString(m))));
				}
				else if (input.StartsWith("advantage")) {
					var adv = board.CurrentAdvantage;
					if (adv.Player == 0) {
						Console.WriteLine("No player has an advantage.");
					}
					else {
						Console.WriteLine("{0} has an advantage of {1}.",
							view.PlayerToString(adv.Player), adv.Advantage);
					}
				}

				Console.WriteLine();
				Console.WriteLine();
			}
		}
	}
}