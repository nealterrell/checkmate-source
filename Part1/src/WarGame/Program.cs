using System;
using Lethargic.PlayingCards;
namespace Lethargic.WarGame {
	class Program {
		public static void Main(string[] args) {
			Deck playerOne = new Deck();
			Deck playerTwo = new Deck();
			playerOne.Shuffle();
			playerTwo.Shuffle();

			int playerOneWins = 0, playerTwoWins = 0;

			while (playerOne.CurrentSize > 0) {
				Card cardOne = playerOne.DealOne();
				Card cardTwo = playerTwo.DealOne();

				Console.Write($"{cardOne} vs. {cardTwo}... ");
				int comparison = cardOne.CompareTo(cardTwo);
				if (comparison == 0) {
					Console.WriteLine("It's a tie!");
				}
				else if (comparison < 0) {
					Console.WriteLine("Player 2 wins!");
					++playerTwoWins;
				}
				else {
					Console.WriteLine("Player 1 wins!");
					++playerOneWins;
				}

				Console.WriteLine("Next round? y/n");
				string yesNo = Console.ReadLine();
				if (yesNo.ToLower() == "n")
					break;
			}
			Console.WriteLine($"Game over... Player 1 has {playerOneWins} wins, "
				+ $"and Player 2 has {playerTwoWins}. GG!");
		}
	}
}
