namespace WeekPlanner {
	using System;

	public class TaskArea
	{
		public readonly double X1, Y1, X2, Y2;
		private int rows;
		private int cols;
		private double cellWidth;
		private double cellHeight;
		private Cell [,] area;
		
		public TaskArea (double x1, double y1, double x2, double y2, int rows, int cols)
		{
			this.X1 = x1;
			this.X2 = x2;
			this.Y1 = y1;
			this.Y2 = y2;
			this.cols = cols;
			this.rows = rows;
			cellWidth = (x2 - x1) / cols;
			cellHeight = (y2 - y1) / rows;
			area = new Cell [rows,cols];
			//FillArea ();
		}

		public Cell GetCellAt (double x, double y)
		{
			Console.WriteLine ("X2:{0} X1:{1}", X2,X1);
			Console.WriteLine ("Getting Cell at {0},{1}", x,y);
			// Column the point belongs to
			double l = X2 - X1;
			double d = cols;
			double col = x / (l / d);

			// Row the point belongs to
			l = Y2 - Y1;
			d = rows;
			double row = y / (l / d);
			Console.WriteLine ("Col {0}, Row {1}", col, row);
			//return area[(int)row, (int)col];
			return new Cell ((int)row, (int)col, cellWidth, cellHeight);
		}

	}
}
