using System;
using System.Collections.Generic;

namespace AdaptiveArithmetic
{
	class Program
	{
		static void Main(string[] args)
		{
			string message = "abcbbbbbacabbacddacdbbaccbbadadaddd abcccccbacabbacbbaddbdaccbbddadadcc bcabbcdabacbbacbbddcbbaccbbdbdadaac.";
			char[] alphabet = new char[6] {'a', 'b', 'c', 'd', ' ', '.'};
			double code = AdaptiveArithmetic.Encode(message, alphabet);
			Console.WriteLine(code);
			string decoded = AdaptiveArithmetic.Decode(code, alphabet, 108);
			Console.WriteLine(decoded);
			Console.WriteLine(message);
		}
	}

	class Segment
	{
		public double high;
		public double low;
		public int weight;

		public Segment(double high, double low, int weight)
		{
			this.high = high;
			this.low = low;
			this.weight = weight;
		}

		public void PrintSegment()
		{
			Console.WriteLine($"{low} - {high}");
		}
	}

	class AdaptiveArithmetic
	{
		public static Dictionary<char, Segment> DefineSegments(char[] alphabet)
		{
			Dictionary<char, Segment> segments = new Dictionary<char, Segment>();
			double p = (double)1 / alphabet.Length;
			double low = 0;
			double high = p;

			for (int i = 0; i < alphabet.Length; i++)
			{
				segments.Add(alphabet[i], new Segment(high, low, 1));
				low = high;
				high += p;
			}

			return segments;
		}

		public static void ResizeSegments(char[] alphabet, Dictionary<char, Segment> segments)
		{
			double tmp = 0;
			int sum = 0;

			foreach (var c in alphabet)
			{
				sum += segments[c].weight;
			}

			foreach (var c in alphabet)
			{
				segments[c].low = tmp;
				segments[c].high = tmp + ((double)segments[c].weight / sum);
				tmp = segments[c].high;
			}

		}

		public static double Encode(string message, char[] alphabet)
		{
			Dictionary<char, Segment> segments = DefineSegments(alphabet);

			double low = 0;
			double high = 1;

			foreach (var c in message)
			{
				segments[c].weight++;
				double newHigh = low + (high - low) * segments[c].high;
				double newLow = low + (high - low) * segments[c].low;
				high = newHigh;
				low = newLow;
				ResizeSegments(alphabet, segments);
			}
			return (low + high) / 2;
		}

		public static string Decode(double code, char[] alphabet, int len)
		{
			Dictionary<char, Segment> segments = DefineSegments(alphabet);
			string result = "";

			for (int i = 0; i < len; i++)
			{
				foreach (var c in alphabet)
				{
					if (code >= segments[c].low && code <= segments[c].high)
					{
						result += c;
						segments[c].weight++;
						code = (code - segments[c].low) / (segments[c].high - segments[c].low);
						ResizeSegments(alphabet, segments);
						break;
					}
				}
			}
			return result;
		}
	}
}
