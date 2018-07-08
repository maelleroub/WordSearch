using System;

namespace WordSearch
{
	class MainClass
	{
		public static void Main (string[] args)
		{
			Cell[,] arr = new Cell[5,3];
			Grid g = new Grid(arr, new string[0]);
			g.FillGrid ("ANERPQCONSMPQTE");
			g.PrintGrid ();
			Console.WriteLine(g.FindWord("SMP", 2, 3, 0, 0));
		}
	}
}