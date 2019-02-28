using System;
using System.ComponentModel;

public static class EnumExtensions
{
	// Author and source unknown, snippet is probably from Stack Overflow though
	public static bool TryParse<T>(this Enum theEnum, string valueToParse, out T retval)
	{
		retval = default(T);
		if (Enum.IsDefined(typeof(T), valueToParse))
		{
			TypeConverter converter = TypeDescriptor.GetConverter(typeof(T));
			retval = (T)converter.ConvertFromString(valueToParse);
			return true;
		}
		return false;
	}

	// Author and source unknown, snippet is probably from Stack Overflow though
	public static T Next<T>(this T src) where T : struct
	{
		if (!typeof(T).IsEnum)
		{
			throw new ArgumentException(string.Format("Argument {0} is not an Enum", typeof(T).FullName));
		}
		T[] arr = (T[]) Enum.GetValues(src.GetType());
		int j = Array.IndexOf<T>(arr, src) + 1;
		return (arr.Length == j) ? arr[0] : arr[j];
	}
}
