using System;
using System.Collections.Generic;
using System.Text;

namespace LinqExercises
{
	class Film {
		public string Title { get; }
		public string ProductionCompany { get; }
		public double Budget { get; }
		public double Earnings { get; }
		public int LengthInMinutes { get; }
		public int YearOfRelease { get; }

		public Film(string title, string production, double budget, double earnings, int length, int year) {
			Title = title;
			ProductionCompany = production;
			Budget = budget;
			Earnings = earnings;
			LengthInMinutes = length;
			YearOfRelease = year;
		}

		public override string ToString() {
			return $"{Title} ({YearOfRelease})";
		}
	}
}
