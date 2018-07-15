using System;
using System.Collections.Generic;

namespace WordSearch
{
	public class Letter
	{
		public char c { get; private set; }
		public int occur { get; private set; }
		public List<Tuple<int, int>> coord = new List<Tuple<int, int>> ();
		public Letter (char c)
		{
			this.c = c;
		}
		public void AddOccur(int x, int y)
		{
			occur++;
			coord.Add (new Tuple<int, int> (x, y));
		}
	}
}