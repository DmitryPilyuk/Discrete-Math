using System;
using System.Collections.Generic;

namespace LZ77
{
	class Program
	{
		static void Main(string[] args)
		{
			string message = "abcbbbbbacabbacddacdbbaccbbadadaddd abcccccbacabbacbbaddbdaccbbddadadcc bcabbcdabacbbacbbddcbbaccbbdbdadaac.";
			List<Node> code = LZ77.Encode(message);
			foreach (var node in code)
			{
				node.PrintNode();
			}

			string decoded = LZ77.Decode(code);
			Console.WriteLine($"\n{decoded}");
			Console.WriteLine($"Is decoded equal message: {decoded == message}");

		}
	}

	public class Node
	{
		public int offset;
		public int length;
		public char next;

		public Node(int offset, int length, char next)
		{
			this.offset = offset;
			this.length = length;
			this.next = next;
		}

		public void PrintNode()
		{
			Console.Write($"<{offset}, {length}, {next}> ");
		}
	}

	public class LZ77
	{
		public static List<Node> Encode(string message)
		{
			int pos = 0;
			List<Node> res = new();
			int startBuffer = 0;
			while (pos < message.Length)
			{
				Node newNode = FindMatch(startBuffer, pos, message);
				pos += newNode.length + 1;
				startBuffer = pos - 10 > 0? pos - 10 : 0;
				res.Add(newNode);
			}
			return res;
		}

		public static Node FindMatch(int start, int pos, string message)
		{
			int offset = 0;
			int len = 0;
			bool flag = true;
			int i = 1;
			string buffer = message.Substring(start, pos - start);
			while (flag)
			{
				string newString = message.Substring(pos, i);
				int index = (buffer + newString).IndexOf(newString);
				if (index != -1 && index < pos - start)
				{
					offset = pos - start - index;
					len = i;
					i++;
				}
				else
				{
					flag = false;
				}
			}
			return new Node(offset, len, message[pos + len]);
		}

		public static string Decode(List<Node> code)
		{
			string result = "";
			foreach (var node in code)
			{
				if (node.length > 0)
				{
					int start = result.Length - node.offset;
					for (int i = 0; i < node.length; i++)
					{
						result += result[start + i];
					}
				}
				result += node.next;
			}
			return result;
		}
	}
}
