namespace game
{
	public struct Pair<U, V>
	{
		public U first;
		public V second;

		public Pair(U first, V second)
		{
			this.first = first;
			this.second = second;
		}

		public override string ToString()
		{
			return string.Concat("(", first, ", ", second, ")");
		}
	}
}
