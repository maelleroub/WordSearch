using System;

namespace WordSearch
{
	public class Grid
	{
		public enum Direction { N, NE, E, SE, S, SW, W, NW };
		public int width { get; private set;}
		public int height { get; private set;}
		private Cell[,] arr;
		private string[] words;
		private Letter[] histo = new Letter[26];
		public Grid (Cell[,] arr, string[] words)
		{
			this.arr = arr;
			this.words = words;
			for (int i = 0; i < words.Length; i++)
				this.words [i] = this.words [i].ToUpper ();
			height = arr.GetLength (0);
			width = arr.GetLength (1);
		}
		public void FillGrid(string s)
		{
			for (int i = 0; i < height; i++)
				for (int j = 0; j < width; j++)
					arr [i,j] = new Cell (s [i * width + j]);
		}
		public void PrintGrid()
		{
			for (int i = 0; i < height; i++) 
			{
				for (int j = 0; j < width; j++)
					Console.Write (arr [i,j].letter + " ");
				Console.WriteLine ();
			}
		}
		public void PrintCheck()
		{
			for (int i = 0; i < height; i++) 
			{
				for (int j = 0; j < width; j++)
				{
					if (arr [i, j].found)
						Console.ForegroundColor = ConsoleColor.Red;
					Console.Write (arr [i,j].letter + " ");
					Console.ForegroundColor = ConsoleColor.White;
				}
				Console.WriteLine ();
			}
		}
		public void FillHisto()
		{
			for(int i = 0; i < 26; i++)
				histo[i] = new Letter((char)(65 + i));
			for (int i = 0; i < height; i++)
				for (int j = 0; j < width; j++)
					histo [arr [i, j].letter - 65].AddOccur(i,j);
		}
		public void PrintHisto()
		{
			for (int i = 0; i < 26; i++) 
			{
				Console.WriteLine (histo [i].c + ": " + histo [i].occur);
			}
		}
		private int PartitionHisto(int low, int high)
		{
			int pivot = histo [high].occur;
			Letter tmp;
			for (int j = low; j < high; j++) 
			{
				if (histo [j].occur <= pivot) 
				{
					tmp = histo [low];
					histo [low] = histo [j];
					histo [j] = tmp;
					low++;
				}
			}
			tmp = histo[low];
			histo [low] = histo [high];
			histo[high] = tmp;
			return low;
		}
		public void QuickSortHisto(int low, int high)
		{
			if (low < high) 
			{
				int pivot = PartitionHisto (low, high);
				QuickSortHisto (low, pivot - 1);
				QuickSortHisto (pivot + 1, high);
			}
		}
		public Tuple<int, int> LeastOccurences(string word)
		{
			//Returns index of least frequent character in histo and in word
			for (int i = 0; i < 26; i++) 
				for (int j = 0; j < word.Length; j++) 
					if (histo [i].c == word [j])
						return new Tuple<int, int>(i, j);
			return new Tuple<int, int>(0,0);
		}
		public bool FindWord(string word)
		{
			Tuple<int, int> i = LeastOccurences (word);
			//For each occurence of the Letter, search in the 8 directions
			foreach (Tuple<int, int> t in histo[i.Item1].coord) 
				for (int j = 0; j < 8; j++) 
					if (FindFromLeastOccur (word, (Direction)j, t.Item1, t.Item2, i.Item2))
						return true;
			return false;
		}
		private bool EnoughRoom(int size, Direction dir, int i, int j, int cur)
		{
			bool b = true;
			if((int)dir <= 1 || (int)dir == 7) //Northern direction
				b = b && ((height - i - 1) >= cur && i >= size - cur - 1);
			else if((int)dir >= 3 && (int)dir <= 5) //Southern direction
				b = b && ((height - i) >= size - cur && i >= cur);
			if((int)dir >= 1 && (int)dir <= 3) //Eastern direction
				b = b && (j >= cur && (width - j) >= size - cur);
			else if ((int)dir >= 5) //Western direction
				b = b && (width - j - 1 >= cur && j >= size - cur - 1);
			return b;
		}
		public bool FindFromLeastOccur(string word, Direction dir, int i, int j, int cur)
		{
			int i2 = i;
			int j2 = j;
			if((int)dir <= 1 || (int)dir == 7)
				i2 += cur;
			else if((int)dir >= 3 && (int)dir <= 5)
				i2 -= cur;
			if((int)dir >= 1 && (int)dir <= 3)
				j2 -= cur;
			else if ((int)dir >= 5)
				j2 += cur;
			return (EnoughRoom (word.Length, dir, i, j, cur) && FindFromBeginning (word, dir, i2, j2, 0));
		}
		private bool FindFromBeginning(string word, Direction dir, int i, int j, int cur)
		{
			if (arr [i, j].letter != word [cur])
				return false;
			if (cur == word.Length - 1)
			{
				arr [i, j].Find ();
				return true;
			}
			int i2 = i;
			int j2 = j;
			if ((int)dir >= 1 && (int)dir <= 3)
				j2++;
			else if ((int)dir >= 5)
				j2--;
			if ((int)dir == 7 || (int)dir <= 1)
				i2--;
			else if ((int)dir >= 3 && (int)dir <= 5)
				i2++;
			if (FindFromBeginning (word, dir, i2, j2, cur + 1))
			{
				arr [i, j].Find ();
				return true;
			}
			return false;
		}
		public string FindList()
		{
			foreach (string w in words)
			{
				if (!FindWord (w))
					Console.Error.WriteLine (w + ": Not in the grid");
			}
			string s = "";
			for (int i = 0; i < height; i++)
				for (int j = 0; j < width; j++)
				{
					if (!arr [i, j].found)
						s += arr [i, j].letter;
				}
			return s;
		}
	}
}