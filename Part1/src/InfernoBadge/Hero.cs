using System;

namespace Lethargic.InfernoBadge.Core {
	/// <summary>
	/// Represents a named hero unit in an Inferno Badge game. 
	/// Assumes the entire Chapter 4 has been completed.
	/// </summary>
	public class Hero {
		public string Name { get; }
		public int Level { get; private set; } = 1;
		public int ExperiencePoints { get; private set; }
		
		public Hero(string name) {
			Name = name;
		}

		public Hero(string name, int level) : this(name) {
			Level = level;
		}

		public void GainExperiencePoints(int amountToGain) {
			ExperiencePoints += amountToGain;
			// Suppose every 100 points gains the hero 1 level.
			if (ExperiencePoints >= 100) {
				Level += ExperiencePoints / 100; // gain 1 level for each 100 points.
				ExperiencePoints %= 100;         // reset experience to between 0 and 99.
			}
		}
	}
}
