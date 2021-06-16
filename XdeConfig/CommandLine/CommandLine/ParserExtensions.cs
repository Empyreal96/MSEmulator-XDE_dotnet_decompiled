using System;
using System.Collections.Generic;

namespace CommandLine
{
	// Token: 0x02000046 RID: 70
	public static class ParserExtensions
	{
		// Token: 0x0600016C RID: 364 RVA: 0x00005AD9 File Offset: 0x00003CD9
		public static ParserResult<object> ParseArguments<T1, T2>(this Parser parser, IEnumerable<string> args)
		{
			if (parser == null)
			{
				throw new ArgumentNullException("parser");
			}
			return parser.ParseArguments(args, new Type[]
			{
				typeof(T1),
				typeof(T2)
			});
		}

		// Token: 0x0600016D RID: 365 RVA: 0x00005B10 File Offset: 0x00003D10
		public static ParserResult<object> ParseArguments<T1, T2, T3>(this Parser parser, IEnumerable<string> args)
		{
			if (parser == null)
			{
				throw new ArgumentNullException("parser");
			}
			return parser.ParseArguments(args, new Type[]
			{
				typeof(T1),
				typeof(T2),
				typeof(T3)
			});
		}

		// Token: 0x0600016E RID: 366 RVA: 0x00005B60 File Offset: 0x00003D60
		public static ParserResult<object> ParseArguments<T1, T2, T3, T4>(this Parser parser, IEnumerable<string> args)
		{
			if (parser == null)
			{
				throw new ArgumentNullException("parser");
			}
			return parser.ParseArguments(args, new Type[]
			{
				typeof(T1),
				typeof(T2),
				typeof(T3),
				typeof(T4)
			});
		}

		// Token: 0x0600016F RID: 367 RVA: 0x00005BBC File Offset: 0x00003DBC
		public static ParserResult<object> ParseArguments<T1, T2, T3, T4, T5>(this Parser parser, IEnumerable<string> args)
		{
			if (parser == null)
			{
				throw new ArgumentNullException("parser");
			}
			return parser.ParseArguments(args, new Type[]
			{
				typeof(T1),
				typeof(T2),
				typeof(T3),
				typeof(T4),
				typeof(T5)
			});
		}

		// Token: 0x06000170 RID: 368 RVA: 0x00005C28 File Offset: 0x00003E28
		public static ParserResult<object> ParseArguments<T1, T2, T3, T4, T5, T6>(this Parser parser, IEnumerable<string> args)
		{
			if (parser == null)
			{
				throw new ArgumentNullException("parser");
			}
			return parser.ParseArguments(args, new Type[]
			{
				typeof(T1),
				typeof(T2),
				typeof(T3),
				typeof(T4),
				typeof(T5),
				typeof(T6)
			});
		}

		// Token: 0x06000171 RID: 369 RVA: 0x00005CA0 File Offset: 0x00003EA0
		public static ParserResult<object> ParseArguments<T1, T2, T3, T4, T5, T6, T7>(this Parser parser, IEnumerable<string> args)
		{
			if (parser == null)
			{
				throw new ArgumentNullException("parser");
			}
			return parser.ParseArguments(args, new Type[]
			{
				typeof(T1),
				typeof(T2),
				typeof(T3),
				typeof(T4),
				typeof(T5),
				typeof(T6),
				typeof(T7)
			});
		}

		// Token: 0x06000172 RID: 370 RVA: 0x00005D24 File Offset: 0x00003F24
		public static ParserResult<object> ParseArguments<T1, T2, T3, T4, T5, T6, T7, T8>(this Parser parser, IEnumerable<string> args)
		{
			if (parser == null)
			{
				throw new ArgumentNullException("parser");
			}
			return parser.ParseArguments(args, new Type[]
			{
				typeof(T1),
				typeof(T2),
				typeof(T3),
				typeof(T4),
				typeof(T5),
				typeof(T6),
				typeof(T7),
				typeof(T8)
			});
		}

		// Token: 0x06000173 RID: 371 RVA: 0x00005DB4 File Offset: 0x00003FB4
		public static ParserResult<object> ParseArguments<T1, T2, T3, T4, T5, T6, T7, T8, T9>(this Parser parser, IEnumerable<string> args)
		{
			if (parser == null)
			{
				throw new ArgumentNullException("parser");
			}
			return parser.ParseArguments(args, new Type[]
			{
				typeof(T1),
				typeof(T2),
				typeof(T3),
				typeof(T4),
				typeof(T5),
				typeof(T6),
				typeof(T7),
				typeof(T8),
				typeof(T9)
			});
		}

		// Token: 0x06000174 RID: 372 RVA: 0x00005E54 File Offset: 0x00004054
		public static ParserResult<object> ParseArguments<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10>(this Parser parser, IEnumerable<string> args)
		{
			if (parser == null)
			{
				throw new ArgumentNullException("parser");
			}
			return parser.ParseArguments(args, new Type[]
			{
				typeof(T1),
				typeof(T2),
				typeof(T3),
				typeof(T4),
				typeof(T5),
				typeof(T6),
				typeof(T7),
				typeof(T8),
				typeof(T9),
				typeof(T10)
			});
		}

		// Token: 0x06000175 RID: 373 RVA: 0x00005F00 File Offset: 0x00004100
		public static ParserResult<object> ParseArguments<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11>(this Parser parser, IEnumerable<string> args)
		{
			if (parser == null)
			{
				throw new ArgumentNullException("parser");
			}
			return parser.ParseArguments(args, new Type[]
			{
				typeof(T1),
				typeof(T2),
				typeof(T3),
				typeof(T4),
				typeof(T5),
				typeof(T6),
				typeof(T7),
				typeof(T8),
				typeof(T9),
				typeof(T10),
				typeof(T11)
			});
		}

		// Token: 0x06000176 RID: 374 RVA: 0x00005FBC File Offset: 0x000041BC
		public static ParserResult<object> ParseArguments<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12>(this Parser parser, IEnumerable<string> args)
		{
			if (parser == null)
			{
				throw new ArgumentNullException("parser");
			}
			return parser.ParseArguments(args, new Type[]
			{
				typeof(T1),
				typeof(T2),
				typeof(T3),
				typeof(T4),
				typeof(T5),
				typeof(T6),
				typeof(T7),
				typeof(T8),
				typeof(T9),
				typeof(T10),
				typeof(T11),
				typeof(T12)
			});
		}

		// Token: 0x06000177 RID: 375 RVA: 0x00006084 File Offset: 0x00004284
		public static ParserResult<object> ParseArguments<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, T13>(this Parser parser, IEnumerable<string> args)
		{
			if (parser == null)
			{
				throw new ArgumentNullException("parser");
			}
			return parser.ParseArguments(args, new Type[]
			{
				typeof(T1),
				typeof(T2),
				typeof(T3),
				typeof(T4),
				typeof(T5),
				typeof(T6),
				typeof(T7),
				typeof(T8),
				typeof(T9),
				typeof(T10),
				typeof(T11),
				typeof(T12),
				typeof(T13)
			});
		}

		// Token: 0x06000178 RID: 376 RVA: 0x0000615C File Offset: 0x0000435C
		public static ParserResult<object> ParseArguments<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, T13, T14>(this Parser parser, IEnumerable<string> args)
		{
			if (parser == null)
			{
				throw new ArgumentNullException("parser");
			}
			return parser.ParseArguments(args, new Type[]
			{
				typeof(T1),
				typeof(T2),
				typeof(T3),
				typeof(T4),
				typeof(T5),
				typeof(T6),
				typeof(T7),
				typeof(T8),
				typeof(T9),
				typeof(T10),
				typeof(T11),
				typeof(T12),
				typeof(T13),
				typeof(T14)
			});
		}

		// Token: 0x06000179 RID: 377 RVA: 0x00006240 File Offset: 0x00004440
		public static ParserResult<object> ParseArguments<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, T13, T14, T15>(this Parser parser, IEnumerable<string> args)
		{
			if (parser == null)
			{
				throw new ArgumentNullException("parser");
			}
			return parser.ParseArguments(args, new Type[]
			{
				typeof(T1),
				typeof(T2),
				typeof(T3),
				typeof(T4),
				typeof(T5),
				typeof(T6),
				typeof(T7),
				typeof(T8),
				typeof(T9),
				typeof(T10),
				typeof(T11),
				typeof(T12),
				typeof(T13),
				typeof(T14),
				typeof(T15)
			});
		}

		// Token: 0x0600017A RID: 378 RVA: 0x00006334 File Offset: 0x00004534
		public static ParserResult<object> ParseArguments<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, T13, T14, T15, T16>(this Parser parser, IEnumerable<string> args)
		{
			if (parser == null)
			{
				throw new ArgumentNullException("parser");
			}
			return parser.ParseArguments(args, new Type[]
			{
				typeof(T1),
				typeof(T2),
				typeof(T3),
				typeof(T4),
				typeof(T5),
				typeof(T6),
				typeof(T7),
				typeof(T8),
				typeof(T9),
				typeof(T10),
				typeof(T11),
				typeof(T12),
				typeof(T13),
				typeof(T14),
				typeof(T15),
				typeof(T16)
			});
		}
	}
}
