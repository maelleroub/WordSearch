using System;

namespace WordSearch
{
	class MainClass
	{
		public static void Main (string[] args)
		{
			Cell[,] arr = new Cell[8,7];
			Grid g = new Grid(arr, new string[0]);
			g.FillGrid ("GDNHETCMOITSNIDTCWTSGEHNDRTSCXDSTEGNEHDGRACIETANYCZNEGRS");
			g.PrintGrid ();
			g.FillHisto ();
			g.QuickSortHisto (0, 25);
			string word = Console.ReadLine();
			g.FindWord (word);
			g.PrintCheck ();
		}
	}
}