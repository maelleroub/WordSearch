using System;

namespace WordSearch
{
	class MainClass
	{
		public static void Main (string[] args)
		{
			Grid g = CreateGrid (args);
			g.PrintGrid ();
			g.FindList ();
			Console.WriteLine ("\n");
			g.PrintCheck ();
		}
		public static Grid CreateGrid(string[] args)
		{
			if (args.Length < 3)
				throw new ArgumentException ("WordSearch: too few arguments");
			int width, height = 0;
			if(!Int32.TryParse(args[0], out width))
				throw new ArgumentException("Incorrect parameter width");
			if(!Int32.TryParse(args[1], out height))
				throw new ArgumentException("Incorrect parameter height");
			Cell[,] arr = new Cell[height, width];
			string[] words = new string[args.Length - 3];
			for (int i = 0; i < args.Length - 3; i++)
				words [i] = args [i + 3];
			Grid g = new Grid (arr, words);
			g.FillGrid (args [2]);
			g.FillHisto ();
			g.QuickSortHisto (0, 25);
			return g;
		}
	}
}