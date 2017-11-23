using System;
using System.Linq;

namespace LinqExercises {
	class Program {
		static void Main(string[] args) {
			Film[] films = {
				new Film("Wall-E", "Pixar Animation Studios", 180000000, 521300000, 98, 2008),
				new Film("Finding Nemo", "Pixar Animation Studios", 94000000, 936700000, 100, 2003),
				new Film("The Lion King", "Walt Disney Feature Animation", 45000000, 987500000, 88, 1994),
				new Film("Finding Dory", "Pixar Animation Studios", 200000000, 1028000000, 97, 2016),
				new Film("The Empire Strikes Back", "Lucasfilm Ltd.", 33000000, 538400000, 124, 1980),
				new Film("Star Wars", "Lucasfilm Ltd.", 11000000, 775400000, 121, 1977),
				new Film("Guardians of the Galaxy", "Marvel Studios", 232300000, 773300000, 122, 2014)	
			};

			// 1.Create a sequence of titles for all films that were produced by Pixar Animation Studios.
			var q1 = films.Where(f => f.ProductionCompany == "Pixar Animation Studios").Select(f => f.Title);

			// 2.Create a sequence of the 5 highest - earning films.
			// Answer: Finding Dory (2016), The Lion King (1994), Finding Nemo (2003), Star Wars (1977), Guardians of the Galaxy (2014)

			// 3. Find the average budget of films produced by Lucasfilm Ltd. (Multiple statements may be required.)
			// Answer: 22000000

			// 4. Determine the title of the film that has the highest profit-per-minute-of-length ratio. 
			// (Profit: the difference between earnings and budget.)
			// Answer: The Lion King

			// 5. Reorder the films by the year of release in ascending order. Calculate the total earnings of only the films including 
			// and following the first film in the reordered sequence that is less than 120 minutes in length.
			// Answer: 4246800000
			
			
		}
	}
}
