using System;

namespace WordSearch
{
	public class Cell
	{
		public char letter { get; private set;}
		public bool found { get; private set; }
		public Cell (char letter)
		{
			this.letter = letter;
		}
		public void Find()
		{
			found = true;
		}
	}
}