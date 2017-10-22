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

		// Compare this card to another, using a default comparison based only on Kind.
		public int CompareTo(Card other) {
			// All enums implement IComparable and compare based on integer values.
			return Kind.CompareTo(other.Kind);
		}

		public static bool operator <(Card lhs, Card rhs) {
			return lhs.CompareTo(rhs) < 0;
		}

		public static bool operator >(Card lhs, Card rhs) {
			return lhs.CompareTo(rhs) > 0;
		}
	}
}
