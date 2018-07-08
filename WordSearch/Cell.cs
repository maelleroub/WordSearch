using System;

namespace WordSearch
{
	public class Cell
	{
		public char letter { get; private set;}
		private bool found;
		public Cell (char letter)
		{
			this.letter = letter;
		}
	}
}