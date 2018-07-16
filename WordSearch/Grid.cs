using System;

namespace WordSearch
{
	public class Grid
	{
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
					Console.Write ((arr [i,j].found ? "T" : "F") + " ");
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
					if (FindFromLeastOccur (word, j, t.Item1, t.Item2, i.Item2))
						return true;
			return false;
		}
		public bool FindFromLeastOccur(string word, int dir, int i, int j, int cur)
		{
			switch (dir) 
			{
			case 0:
				if ((height - i - 1) >= cur && i >= word.Length - cur - 1)
				if (FindFromBeginning (word, dir, i + cur, j, 0))
					return true;
				break;
			case 1:
				if ((height - i - 1) >= cur && i >= word.Length - cur - 1 && j >= cur && (width - j) >= word.Length - cur)
				if (FindFromBeginning (word, dir, i + cur, j - cur, 0))
					return true;
				break;
			case 2:
				if (j >= cur && (width - j) >= word.Length - cur)
				if (FindFromBeginning (word, dir, i, j - cur, 0))
					return true;
				break;
			case 3:
				if ((height - i) >= word.Length - cur && i >= cur && j >= cur && (width - j) >= word.Length - cur)
				if (FindFromBeginning (word, dir, i - cur, j - cur, 0))
					return true;
				break;
			case 4:
				if ((height - i) >= word.Length - cur && i >= cur)
				if (FindFromBeginning (word, dir, i - cur, j, 0))
					return true;
				break;
			case 5:
				if ((height - i) >= word.Length - cur && i >= cur && width - j - 1 >= cur && j >= word.Length - cur - 1)
				if (FindFromBeginning (word, dir, i - cur, j + cur, 0))
					return true;
				break;
			case 6:
				if (width - j - 1 >= cur && j >= word.Length - cur - 1)
				if (FindFromBeginning (word, dir, i, j + cur, 0))
					return true;
				break;
			case 7:
				if (width - j - 1 >= cur && j >= word.Length - cur - 1 && (height - i - 1) >= cur && i >= word.Length - cur - 1)
				if (FindFromBeginning (word, dir, i + cur, j + cur, 0))
					return true;
				break;
			default:
				return false;
			}
			return false;
		}
		private bool FindFromBeginning(string word, int dir, int i, int j, int cur)
		{
			//8 directions, 0: N, 1: NE, 2: E, 3: SE, 4: S, 5: SW, 6: W, 7: NW
			if (arr [i, j].letter != word [cur])
				return false;
			if (cur == word.Length - 1)
			{
				arr [i, j].Find ();
				return true;
			}
			int i2 = i;
			int j2 = j;
			if (dir >= 1 && dir <= 3)
				j2++;
			else if (dir >= 5)
				j2--;
			if (dir == 7 || dir <= 1)
				i2--;
			else if (dir >= 3 && dir <= 5)
				i2++;
			if (FindFromBeginning (word, dir, i2, j2, cur + 1))
			{
				arr [i, j].Find ();
				return true;
			}
			return false;
		}
		public void FindList()
		{
			foreach (string w in words)
			{
				if (!FindWord (w))
					Console.Error.WriteLine (w + ": Not in the grid");
			}
		}
	}
}