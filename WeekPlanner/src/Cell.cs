namespace WeekPlanner
{
	
	public class Cell
	{
		public readonly int Row, Column;
		public readonly double Width, Height;

		public Cell (int row, int column, double width, double height)
		{
			this.Row = row;
			this.Column = column;
			this.Width = width;
			this.Height = height;
		}

	}
}
