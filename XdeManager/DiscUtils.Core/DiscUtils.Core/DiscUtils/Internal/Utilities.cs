using System;
using System.Collections.Generic;
using System.IO;
using System.Text;
using System.Text.RegularExpressions;

namespace DiscUtils.Internal
{
	// Token: 0x02000073 RID: 115
	internal static class Utilities
	{
		// Token: 0x06000425 RID: 1061 RVA: 0x0000C758 File Offset: 0x0000A958
		public static U[] Map<T, U>(ICollection<T> source, Func<T, U> func)
		{
			U[] array = new U[source.Count];
			int num = 0;
			foreach (T arg in source)
			{
				array[num++] = func(arg);
			}
			return array;
		}

		// Token: 0x06000426 RID: 1062 RVA: 0x0000C7BC File Offset: 0x0000A9BC
		public static U[] Map<T, U>(IEnumerable<T> source, Func<T, U> func)
		{
			List<U> list = new List<U>();
			foreach (T arg in source)
			{
				list.Add(func(arg));
			}
			return list.ToArray();
		}

		// Token: 0x06000427 RID: 1063 RVA: 0x0000C818 File Offset: 0x0000AA18
		public static C Filter<C, T>(ICollection<T> source, Func<T, bool> predicate) where C : ICollection<T>, new()
		{
			C result = Activator.CreateInstance<C>();
			foreach (T t in source)
			{
				if (predicate(t))
				{
					result.Add(t);
				}
			}
			return result;
		}

		// Token: 0x06000428 RID: 1064 RVA: 0x0000C878 File Offset: 0x0000AA78
		public static bool RangesOverlap<T>(T xFirst, T xLast, T yFirst, T yLast) where T : IComparable<T>
		{
			return xLast.CompareTo(yFirst) > 0 && xFirst.CompareTo(yLast) < 0;
		}

		// Token: 0x06000429 RID: 1065 RVA: 0x0000C8A0 File Offset: 0x0000AAA0
		public static bool IsAllZeros(byte[] buffer, int offset, int count)
		{
			int num = offset + count;
			for (int i = offset; i < num; i++)
			{
				if (buffer[i] != 0)
				{
					return false;
				}
			}
			return true;
		}

		// Token: 0x0600042A RID: 1066 RVA: 0x0000C8C5 File Offset: 0x0000AAC5
		public static bool IsPowerOfTwo(uint val)
		{
			if (val == 0U)
			{
				return false;
			}
			while ((val & 1U) != 1U)
			{
				val >>= 1;
			}
			return val == 1U;
		}

		// Token: 0x0600042B RID: 1067 RVA: 0x0000C8DB File Offset: 0x0000AADB
		public static bool IsPowerOfTwo(long val)
		{
			if (val == 0L)
			{
				return false;
			}
			while ((val & 1L) != 1L)
			{
				val >>= 1;
			}
			return val == 1L;
		}

		// Token: 0x0600042C RID: 1068 RVA: 0x0000C8F4 File Offset: 0x0000AAF4
		public static bool AreEqual(byte[] a, byte[] b)
		{
			if (a.Length != b.Length)
			{
				return false;
			}
			for (int i = 0; i < a.Length; i++)
			{
				if (a[i] != b[i])
				{
					return false;
				}
			}
			return true;
		}

		// Token: 0x0600042D RID: 1069 RVA: 0x0000C924 File Offset: 0x0000AB24
		public static ushort BitSwap(ushort value)
		{
			return (ushort)((int)(value & 255) << 8 | (value & 65280) >> 8);
		}

		// Token: 0x0600042E RID: 1070 RVA: 0x0000C93A File Offset: 0x0000AB3A
		public static uint BitSwap(uint value)
		{
			return (value & 255U) << 24 | (value & 65280U) << 8 | (value & 16711680U) >> 8 | (value & 4278190080U) >> 24;
		}

		// Token: 0x0600042F RID: 1071 RVA: 0x0000C965 File Offset: 0x0000AB65
		public static ulong BitSwap(ulong value)
		{
			return (ulong)Utilities.BitSwap((uint)(value & (ulong)-1)) << 32 | (ulong)Utilities.BitSwap((uint)(value >> 32));
		}

		// Token: 0x06000430 RID: 1072 RVA: 0x0000C981 File Offset: 0x0000AB81
		public static short BitSwap(short value)
		{
			return (short)Utilities.BitSwap((ushort)value);
		}

		// Token: 0x06000431 RID: 1073 RVA: 0x0000C98B File Offset: 0x0000AB8B
		public static int BitSwap(int value)
		{
			return (int)Utilities.BitSwap((uint)value);
		}

		// Token: 0x06000432 RID: 1074 RVA: 0x0000C993 File Offset: 0x0000AB93
		public static long BitSwap(long value)
		{
			return (long)Utilities.BitSwap((ulong)value);
		}

		// Token: 0x06000433 RID: 1075 RVA: 0x0000C99C File Offset: 0x0000AB9C
		public static string GetDirectoryFromPath(string path)
		{
			string text = path.TrimEnd(new char[]
			{
				'\\'
			});
			int num = text.LastIndexOf('\\');
			if (num < 0)
			{
				return string.Empty;
			}
			return text.Substring(0, num);
		}

		// Token: 0x06000434 RID: 1076 RVA: 0x0000C9D8 File Offset: 0x0000ABD8
		public static string GetFileFromPath(string path)
		{
			string text = path.Trim(new char[]
			{
				'\\'
			});
			int num = text.LastIndexOf('\\');
			if (num < 0)
			{
				return text;
			}
			return text.Substring(num + 1);
		}

		// Token: 0x06000435 RID: 1077 RVA: 0x0000CA10 File Offset: 0x0000AC10
		public static string CombinePaths(string a, string b)
		{
			if (string.IsNullOrEmpty(a) || (b.Length > 0 && b[0] == '\\'))
			{
				return b;
			}
			if (string.IsNullOrEmpty(b))
			{
				return a;
			}
			return a.TrimEnd(new char[]
			{
				'\\'
			}) + "\\" + b.TrimStart(new char[]
			{
				'\\'
			});
		}

		// Token: 0x06000436 RID: 1078 RVA: 0x0000CA74 File Offset: 0x0000AC74
		public static string ResolveRelativePath(string basePath, string relativePath)
		{
			if (string.IsNullOrEmpty(basePath))
			{
				return relativePath;
			}
			if (!basePath.EndsWith("\\"))
			{
				basePath = Path.GetDirectoryName(basePath);
			}
			string fullPath = Path.GetFullPath(Path.Combine(basePath, relativePath));
			if (basePath.StartsWith("\\") && fullPath.Length > 2 && fullPath[1].Equals(':'))
			{
				return fullPath.Substring(2);
			}
			return fullPath;
		}

		// Token: 0x06000437 RID: 1079 RVA: 0x0000CADF File Offset: 0x0000ACDF
		public static string ResolvePath(string basePath, string path)
		{
			if (!path.StartsWith("\\", StringComparison.OrdinalIgnoreCase))
			{
				return Utilities.ResolveRelativePath(basePath, path);
			}
			return path;
		}

		// Token: 0x06000438 RID: 1080 RVA: 0x0000CAF8 File Offset: 0x0000ACF8
		public static string MakeRelativePath(string path, string basePath)
		{
			List<string> list = new List<string>(path.Split(new char[]
			{
				'\\'
			}, StringSplitOptions.RemoveEmptyEntries));
			List<string> list2 = new List<string>(basePath.Split(new char[]
			{
				'\\'
			}, StringSplitOptions.RemoveEmptyEntries));
			if (!basePath.EndsWith("\\", StringComparison.Ordinal) && list2.Count > 0)
			{
				list2.RemoveAt(list2.Count - 1);
			}
			int num = 0;
			while (num < Math.Min(list.Count - 1, list2.Count) && !(list[num].ToUpperInvariant() != list2[num].ToUpperInvariant()))
			{
				num++;
			}
			StringBuilder stringBuilder = new StringBuilder();
			if (num == list2.Count)
			{
				stringBuilder.Append(".\\");
			}
			else if (num < list2.Count)
			{
				for (int i = 0; i < list2.Count - num; i++)
				{
					stringBuilder.Append("..\\");
				}
			}
			for (int j = num; j < list.Count - 1; j++)
			{
				stringBuilder.Append(list[j]);
				stringBuilder.Append("\\");
			}
			stringBuilder.Append(list[list.Count - 1]);
			if (path.EndsWith("\\", StringComparison.Ordinal))
			{
				stringBuilder.Append("\\");
			}
			return stringBuilder.ToString();
		}

		// Token: 0x06000439 RID: 1081 RVA: 0x0000CC48 File Offset: 0x0000AE48
		public static bool Is8Dot3(string name)
		{
			if (name.Length > 12)
			{
				return false;
			}
			string[] array = name.Split(new char[]
			{
				'.'
			});
			if (array.Length > 2 || array.Length < 1)
			{
				return false;
			}
			if (array[0].Length > 8)
			{
				return false;
			}
			string text = array[0];
			for (int i = 0; i < text.Length; i++)
			{
				if (!Utilities.Is8Dot3Char(text[i]))
				{
					return false;
				}
			}
			if (array.Length > 1)
			{
				if (array[1].Length > 3)
				{
					return false;
				}
				text = array[1];
				for (int i = 0; i < text.Length; i++)
				{
					if (!Utilities.Is8Dot3Char(text[i]))
					{
						return false;
					}
				}
			}
			return true;
		}

		// Token: 0x0600043A RID: 1082 RVA: 0x0000CCEC File Offset: 0x0000AEEC
		public static bool Is8Dot3Char(char ch)
		{
			return (ch >= 'A' && ch <= 'Z') || (ch >= '0' && ch <= '9') || "_^$~!#%£-{}()@'`&".IndexOf(ch) != -1;
		}

		// Token: 0x0600043B RID: 1083 RVA: 0x0000CD18 File Offset: 0x0000AF18
		public static Regex ConvertWildcardsToRegEx(string pattern)
		{
			if (!pattern.Contains("."))
			{
				pattern += ".";
			}
			return new Regex("^" + Regex.Escape(pattern).Replace("\\*", ".*").Replace("\\?", "[^.]") + "$", RegexOptions.IgnoreCase | RegexOptions.CultureInvariant);
		}

		// Token: 0x0600043C RID: 1084 RVA: 0x0000CD7C File Offset: 0x0000AF7C
		public static FileAttributes FileAttributesFromUnixFileType(UnixFileType fileType)
		{
			switch (fileType)
			{
			case UnixFileType.Fifo:
				return FileAttributes.System | FileAttributes.Device;
			case UnixFileType.Character:
				return FileAttributes.System | FileAttributes.Device;
			case UnixFileType.Directory:
				return FileAttributes.Directory;
			case UnixFileType.Block:
				return FileAttributes.System | FileAttributes.Device;
			case UnixFileType.Regular:
				return FileAttributes.Normal;
			case UnixFileType.Link:
				return FileAttributes.ReparsePoint;
			case UnixFileType.Socket:
				return FileAttributes.System | FileAttributes.Device;
			}
			return (FileAttributes)0;
		}
	}
}
