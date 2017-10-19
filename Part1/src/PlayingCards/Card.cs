using System;

namespace Lethargic.PlayingCards {
	public enum CardSuit : byte {
		Clubs,
		Diamonds,
		Hearts,
		Spades
	}

	public enum CardKind : byte {
		Two = 2, // a value can be supplied explicitly, and other values will count up from there.
		Three,
		Four,
		Five,
		Six,
		Seven,
		Eight,
		Nine,
		Ten,
		Jack,
		Queen,
		King,
		Ace // == 14
	}


	public struct Card : IComparable<Card> {
		public CardSuit Suit { get; }
		public CardKind Kind { get; }

		public Card(CardKind kind, CardSuit suit) {
			Kind = kind;
			Suit = suit;
		}

		public override string ToString() {
			int kindValue = (int)Kind;
			string r;
			if (kindValue >= 2 && kindValue <= 10) {
				r = kindValue.ToString();
			}
			else {
				r = Kind.ToString(); // ToString on an enum returns the name given in code, e.g., "Jack", "Two", etc.
			}
			return r + " of " + Suit.ToString();
		}

		// Compare this card to another, to decide which wins the War game. This is inherited from the IComparable 
		// interface.
		public int CompareTo(Card other) {
			// compare the cards based on the integer value of their Kind.
			return Kind.CompareTo(other.Kind);
		}
	}
}
