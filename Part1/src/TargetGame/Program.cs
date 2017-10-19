using System;

namespace Lethargic.TargetGame {
	class TargetPractice {
		public static void Main(string[] args) {
			int targetDistance;
			Random rngesus = new Random();
			const int MAX_DISTANCE = 1000;

			Console.WriteLine("Welcome to target practice!");

			// Choose a random target distance as an integer from 0 to 1000, inclusive.
			targetDistance = rngesus.Next(MAX_DISTANCE + 1);

			Console.WriteLine("You are trying to hit a target that is " + targetDistance + "m away.");
			bool hitTarget = false;
			while (!hitTarget) {
				double angle = GetAngle();
				double gunpowder = GetGunpowder();

				double distanceTraveled = DistanceTraveled(angle, gunpowder);
				double missedBy = Math.Abs(distanceTraveled - targetDistance);
				if (missedBy <= 1.0) {
					Console.WriteLine("You hit the target!");
					hitTarget = true;
				}
				else if (distanceTraveled < targetDistance) {
					Console.WriteLine("You were short by {0}m. Try again!", missedBy);
				}
				else {
					Console.WriteLine($"You were long by {missedBy}m. Try again!");
				}
			}
		}

		public static double GetAngle() {
			double angle = -1;
			while (angle < 0 || angle > 90) {
				Console.Write("Enter an angle of elevation for the cannon, from 0 to 90 degrees: ");
				string input = Console.ReadLine();
				angle = double.Parse(input);
			}
			return angle;
		}

		public static double GetGunpowder() {
			double gunpowder = -1;
			while (gunpowder < 0) {
				Console.Write("Enter an amount of gunpowder to use, in kilograms: ");
				string input = Console.ReadLine();
				gunpowder = double.Parse(input);
			}
			return gunpowder;
		}

		public static double DistanceTraveled(double angle, double gunpowder) {
			// Each kg of gunpowder creates 30 meters per second of velocity.
			const double GUNPOWDER_MPS = 30;
			const double GRAVITY = 9.81;

			double velocity = gunpowder * GUNPOWDER_MPS;
			double radians = angle * Math.PI / 180;

			// From physics: distance = v^2 * sin(2*theta) / G.
			return velocity * velocity * Math.Sin(2 * radians) / GRAVITY;
		}
	}
}