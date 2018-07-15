using System;

namespace WordSearch
{
	class MainClass
	{
		public static void Main (string[] args)
		{
			Cell[,] arr = new Cell[5,7];
			Grid g = new Grid(arr, new string[0]);
			g.FillGrid ("GDNHETCMOITSNIDTCWTSGEHNDRTSCXDSTEG");
			g.PrintGrid ();
			g.FillHisto ();
			//g.PrintHisto ();
			g.QuickSortHisto (0, 25);
			//g.PrintHisto ();
			string word = Console.ReadLine();
			g.FindWord (word);
			g.PrintCheck ();
		}
	}
}