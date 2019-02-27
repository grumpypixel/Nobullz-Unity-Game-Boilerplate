using System;
using System.Collections.Generic;

public static class ListExtensions
{
	// Author and source unknown, snippet is probably from Stack Overflow though
	public static void Shuffle<T>(this IList<T> list)
	{
		Random rand = new Random();
		int n = list.Count;
		while (n > 1)
		{
			n--;
			int k = rand.Next(n + 1);
			T value = list[k];
			list[k] = list[n];
			list[n] = value;
		}
	}

	public static IList<T> Swap<T>(this IList<T> list, int indexA, int indexB)
	{
		T tmp = list[indexA];
		list[indexA] = list[indexB];
		list[indexB] = tmp;
		return list;
	}
}
