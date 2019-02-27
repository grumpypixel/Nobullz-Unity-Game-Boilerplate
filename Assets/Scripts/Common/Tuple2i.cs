namespace game
{
	public struct Tuple2i
	{
		public int x;
		public int y;

		public Tuple2i(int x, int y)
		{
			this.x = x;
			this.y = y;
		}

		public override string ToString()
		{
			return string.Concat("(", x, ", ", y, ")");
		}
	}
}
