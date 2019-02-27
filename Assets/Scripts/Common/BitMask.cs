namespace game
{
	public class BitMask
	{
		private int m_bits;

		public int bits
		{
			get { return m_bits; }
			set { m_bits = value; }
		}

		public BitMask(int initialBits = 0)
		{
			m_bits = initialBits;
		}

		public void Set(int bit)
		{
			m_bits |= (1 << bit);
		}

		public void Clear(int bit)
		{
			m_bits &= ~(1 << bit);
		}

		public void Toggle(int bit)
		{
			m_bits ^= (1 << bit);
		}

		public void ShiftLeft(int count)
		{
			m_bits <<= count;
		}

		public void ShiftRight(int count)
		{
			m_bits >>= count;
		}

		public void Complement()
		{
			m_bits = ~m_bits;
		}

		public bool Test(int bit)
		{
			return ((m_bits >> bit) & 1) == 1;
		}
	}
}
