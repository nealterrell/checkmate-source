using System;
using System.Collections.Generic;
using System.Text;

namespace Lethargic.BoardGames.Model {
	public struct GameAdvantage  : IEquatable<GameAdvantage> {
		public int Player { get; private set; }
		public int Advantage { get; private set; }

		public GameAdvantage(int player, int advantage) {
			Player = player;
			Advantage = advantage;
		}

		public bool Equals(GameAdvantage other) {
			return Player == other.Player && Advantage == other.Advantage;
		}
	}
}
