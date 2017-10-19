using System;
using System.Linq;

namespace Lethargic.PlayingCards {
	public class Deck {
		private static Random mRandom = new Random();

		public const int NEW_DECK_SIZE = 52;

		private Card[] mCards = new Card[NEW_DECK_SIZE];
		
		public int CurrentSize { get; private set; }

		public Deck() {
			mCards = new Card[NEW_DECK_SIZE];
			CurrentSize = NEW_DECK_SIZE; // this sets the hidden "Count" member to 52, using the private set property.

			int i = 0;
			// For simplicity, we will abuse the fact that we know that CardSuit and CardKind are really just integers.
			for (int suit = 0; suit < 4; suit++) {
				for (int kind = 2; kind <= 14; kind++) {
					mCards[i] = new Card((CardKind)kind, (CardSuit)suit); // the cast satisfies the type system.
					i++;
				}
			}
		}

		/// <summary>
		/// Performs a randomized shuffle of whichever cards are still in the deck.
		/// </summary>
		public void Shuffle() {
			// We use a static Random generator because the Random constructor uses a time-based seed, and if two decks are
			// shuffled within some small time frame, they will end up the same if they both construct a new Random 
			// instance. Using a shared static instance means this won't happen.

			// Perform a Fisher-Yates shuffle.
			for (int i = CurrentSize - 1; i > 0; i--) {
				int j = mRandom.Next(i + 1);
				Card temp = mCards[j];
				mCards[j] = mCards[i];
				mCards[i] = temp;
			}
		}

		/// <summary>
		/// Deals one card, removing it from the top of the deck.
		/// </summary>
		/// <returns>the card that used to be on top of the deck</returns>
		public Card DealOne() {
			--CurrentSize;
			return mCards[CurrentSize];
		}

		// Return a string of all the cards in the deck, from top to bottom.
		public override string ToString() {
			// String.Join: creates a string by inserting the given delimiter between every element of a given collection.
			// Reverse(): reverses a sequence.
			// Take(n): returns only the first n elements of a sequence.
			return string.Join(", ", mCards.Take(CurrentSize).Reverse());
		}
	}
}
