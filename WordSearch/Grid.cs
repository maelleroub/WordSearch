﻿using System;

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
				if (histo [j].occur >= pivot) 
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
		public bool FindWord(string word, int dir, int i, int j, int cur)
		{
			//8 directions, 0: N, 1: NE, 2: E, 3: SE, 4: S, 5: SW, 6: W, 7: NW
			Console.WriteLine(arr[i,j].letter);
			if (arr [i, j].letter != word [cur])
				return false;
			if (cur == word.Length - 1)
				return true;
			if (dir >= 1 && dir <= 3)
				j++;
			else if (dir >= 5)
				j--;
			if (dir == 7 || dir <= 1)
				i--;
			else if (dir >= 4 && dir <= 6)
				i++;
			return FindWord (word, dir, i, j, cur + 1);
		}
	}
}