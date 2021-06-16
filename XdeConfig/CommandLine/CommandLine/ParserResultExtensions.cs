using System;
using System.Collections.Generic;

namespace CommandLine
{
	// Token: 0x0200004C RID: 76
	public static class ParserResultExtensions
	{
		// Token: 0x0600018E RID: 398 RVA: 0x00006604 File Offset: 0x00004804
		public static ParserResult<T> WithParsed<T>(this ParserResult<T> result, Action<T> action)
		{
			Parsed<T> parsed = result as Parsed<T>;
			if (parsed != null)
			{
				action(parsed.Value);
			}
			return result;
		}

		// Token: 0x0600018F RID: 399 RVA: 0x00006628 File Offset: 0x00004828
		public static ParserResult<object> WithParsed<T>(this ParserResult<object> result, Action<T> action)
		{
			Parsed<object> parsed = result as Parsed<object>;
			if (parsed != null && parsed.Value is T)
			{
				action((T)((object)parsed.Value));
			}
			return result;
		}

		// Token: 0x06000190 RID: 400 RVA: 0x00006660 File Offset: 0x00004860
		public static ParserResult<T> WithNotParsed<T>(this ParserResult<T> result, Action<IEnumerable<Error>> action)
		{
			NotParsed<T> notParsed = result as NotParsed<T>;
			if (notParsed != null)
			{
				action(notParsed.Errors);
			}
			return result;
		}

		// Token: 0x06000191 RID: 401 RVA: 0x00006684 File Offset: 0x00004884
		public static TResult MapResult<TSource, TResult>(this ParserResult<TSource> result, Func<TSource, TResult> parsedFunc, Func<IEnumerable<Error>, TResult> notParsedFunc)
		{
			Parsed<TSource> parsed = result as Parsed<TSource>;
			if (parsed != null)
			{
				return parsedFunc(parsed.Value);
			}
			return notParsedFunc(((NotParsed<TSource>)result).Errors);
		}

		// Token: 0x06000192 RID: 402 RVA: 0x000066BC File Offset: 0x000048BC
		public static TResult MapResult<T1, TResult>(this ParserResult<object> result, Func<T1, TResult> parsedFunc1, Func<IEnumerable<Error>, TResult> notParsedFunc)
		{
			Parsed<object> parsed = result as Parsed<object>;
			if (parsed == null)
			{
				return notParsedFunc(((NotParsed<object>)result).Errors);
			}
			if (parsed.Value is T1)
			{
				return parsedFunc1((T1)((object)parsed.Value));
			}
			throw new InvalidOperationException();
		}

		// Token: 0x06000193 RID: 403 RVA: 0x0000670C File Offset: 0x0000490C
		public static TResult MapResult<T1, T2, TResult>(this ParserResult<object> result, Func<T1, TResult> parsedFunc1, Func<T2, TResult> parsedFunc2, Func<IEnumerable<Error>, TResult> notParsedFunc)
		{
			Parsed<object> parsed = result as Parsed<object>;
			if (parsed == null)
			{
				return notParsedFunc(((NotParsed<object>)result).Errors);
			}
			if (parsed.Value is T1)
			{
				return parsedFunc1((T1)((object)parsed.Value));
			}
			if (parsed.Value is T2)
			{
				return parsedFunc2((T2)((object)parsed.Value));
			}
			throw new InvalidOperationException();
		}

		// Token: 0x06000194 RID: 404 RVA: 0x00006778 File Offset: 0x00004978
		public static TResult MapResult<T1, T2, T3, TResult>(this ParserResult<object> result, Func<T1, TResult> parsedFunc1, Func<T2, TResult> parsedFunc2, Func<T3, TResult> parsedFunc3, Func<IEnumerable<Error>, TResult> notParsedFunc)
		{
			Parsed<object> parsed = result as Parsed<object>;
			if (parsed == null)
			{
				return notParsedFunc(((NotParsed<object>)result).Errors);
			}
			if (parsed.Value is T1)
			{
				return parsedFunc1((T1)((object)parsed.Value));
			}
			if (parsed.Value is T2)
			{
				return parsedFunc2((T2)((object)parsed.Value));
			}
			if (parsed.Value is T3)
			{
				return parsedFunc3((T3)((object)parsed.Value));
			}
			throw new InvalidOperationException();
		}

		// Token: 0x06000195 RID: 405 RVA: 0x00006804 File Offset: 0x00004A04
		public static TResult MapResult<T1, T2, T3, T4, TResult>(this ParserResult<object> result, Func<T1, TResult> parsedFunc1, Func<T2, TResult> parsedFunc2, Func<T3, TResult> parsedFunc3, Func<T4, TResult> parsedFunc4, Func<IEnumerable<Error>, TResult> notParsedFunc)
		{
			Parsed<object> parsed = result as Parsed<object>;
			if (parsed == null)
			{
				return notParsedFunc(((NotParsed<object>)result).Errors);
			}
			if (parsed.Value is T1)
			{
				return parsedFunc1((T1)((object)parsed.Value));
			}
			if (parsed.Value is T2)
			{
				return parsedFunc2((T2)((object)parsed.Value));
			}
			if (parsed.Value is T3)
			{
				return parsedFunc3((T3)((object)parsed.Value));
			}
			if (parsed.Value is T4)
			{
				return parsedFunc4((T4)((object)parsed.Value));
			}
			throw new InvalidOperationException();
		}

		// Token: 0x06000196 RID: 406 RVA: 0x000068B4 File Offset: 0x00004AB4
		public static TResult MapResult<T1, T2, T3, T4, T5, TResult>(this ParserResult<object> result, Func<T1, TResult> parsedFunc1, Func<T2, TResult> parsedFunc2, Func<T3, TResult> parsedFunc3, Func<T4, TResult> parsedFunc4, Func<T5, TResult> parsedFunc5, Func<IEnumerable<Error>, TResult> notParsedFunc)
		{
			Parsed<object> parsed = result as Parsed<object>;
			if (parsed == null)
			{
				return notParsedFunc(((NotParsed<object>)result).Errors);
			}
			if (parsed.Value is T1)
			{
				return parsedFunc1((T1)((object)parsed.Value));
			}
			if (parsed.Value is T2)
			{
				return parsedFunc2((T2)((object)parsed.Value));
			}
			if (parsed.Value is T3)
			{
				return parsedFunc3((T3)((object)parsed.Value));
			}
			if (parsed.Value is T4)
			{
				return parsedFunc4((T4)((object)parsed.Value));
			}
			if (parsed.Value is T5)
			{
				return parsedFunc5((T5)((object)parsed.Value));
			}
			throw new InvalidOperationException();
		}

		// Token: 0x06000197 RID: 407 RVA: 0x00006984 File Offset: 0x00004B84
		public static TResult MapResult<T1, T2, T3, T4, T5, T6, TResult>(this ParserResult<object> result, Func<T1, TResult> parsedFunc1, Func<T2, TResult> parsedFunc2, Func<T3, TResult> parsedFunc3, Func<T4, TResult> parsedFunc4, Func<T5, TResult> parsedFunc5, Func<T6, TResult> parsedFunc6, Func<IEnumerable<Error>, TResult> notParsedFunc)
		{
			Parsed<object> parsed = result as Parsed<object>;
			if (parsed == null)
			{
				return notParsedFunc(((NotParsed<object>)result).Errors);
			}
			if (parsed.Value is T1)
			{
				return parsedFunc1((T1)((object)parsed.Value));
			}
			if (parsed.Value is T2)
			{
				return parsedFunc2((T2)((object)parsed.Value));
			}
			if (parsed.Value is T3)
			{
				return parsedFunc3((T3)((object)parsed.Value));
			}
			if (parsed.Value is T4)
			{
				return parsedFunc4((T4)((object)parsed.Value));
			}
			if (parsed.Value is T5)
			{
				return parsedFunc5((T5)((object)parsed.Value));
			}
			if (parsed.Value is T6)
			{
				return parsedFunc6((T6)((object)parsed.Value));
			}
			throw new InvalidOperationException();
		}

		// Token: 0x06000198 RID: 408 RVA: 0x00006A74 File Offset: 0x00004C74
		public static TResult MapResult<T1, T2, T3, T4, T5, T6, T7, TResult>(this ParserResult<object> result, Func<T1, TResult> parsedFunc1, Func<T2, TResult> parsedFunc2, Func<T3, TResult> parsedFunc3, Func<T4, TResult> parsedFunc4, Func<T5, TResult> parsedFunc5, Func<T6, TResult> parsedFunc6, Func<T7, TResult> parsedFunc7, Func<IEnumerable<Error>, TResult> notParsedFunc)
		{
			Parsed<object> parsed = result as Parsed<object>;
			if (parsed == null)
			{
				return notParsedFunc(((NotParsed<object>)result).Errors);
			}
			if (parsed.Value is T1)
			{
				return parsedFunc1((T1)((object)parsed.Value));
			}
			if (parsed.Value is T2)
			{
				return parsedFunc2((T2)((object)parsed.Value));
			}
			if (parsed.Value is T3)
			{
				return parsedFunc3((T3)((object)parsed.Value));
			}
			if (parsed.Value is T4)
			{
				return parsedFunc4((T4)((object)parsed.Value));
			}
			if (parsed.Value is T5)
			{
				return parsedFunc5((T5)((object)parsed.Value));
			}
			if (parsed.Value is T6)
			{
				return parsedFunc6((T6)((object)parsed.Value));
			}
			if (parsed.Value is T7)
			{
				return parsedFunc7((T7)((object)parsed.Value));
			}
			throw new InvalidOperationException();
		}

		// Token: 0x06000199 RID: 409 RVA: 0x00006B84 File Offset: 0x00004D84
		public static TResult MapResult<T1, T2, T3, T4, T5, T6, T7, T8, TResult>(this ParserResult<object> result, Func<T1, TResult> parsedFunc1, Func<T2, TResult> parsedFunc2, Func<T3, TResult> parsedFunc3, Func<T4, TResult> parsedFunc4, Func<T5, TResult> parsedFunc5, Func<T6, TResult> parsedFunc6, Func<T7, TResult> parsedFunc7, Func<T8, TResult> parsedFunc8, Func<IEnumerable<Error>, TResult> notParsedFunc)
		{
			Parsed<object> parsed = result as Parsed<object>;
			if (parsed == null)
			{
				return notParsedFunc(((NotParsed<object>)result).Errors);
			}
			if (parsed.Value is T1)
			{
				return parsedFunc1((T1)((object)parsed.Value));
			}
			if (parsed.Value is T2)
			{
				return parsedFunc2((T2)((object)parsed.Value));
			}
			if (parsed.Value is T3)
			{
				return parsedFunc3((T3)((object)parsed.Value));
			}
			if (parsed.Value is T4)
			{
				return parsedFunc4((T4)((object)parsed.Value));
			}
			if (parsed.Value is T5)
			{
				return parsedFunc5((T5)((object)parsed.Value));
			}
			if (parsed.Value is T6)
			{
				return parsedFunc6((T6)((object)parsed.Value));
			}
			if (parsed.Value is T7)
			{
				return parsedFunc7((T7)((object)parsed.Value));
			}
			if (parsed.Value is T8)
			{
				return parsedFunc8((T8)((object)parsed.Value));
			}
			throw new InvalidOperationException();
		}

		// Token: 0x0600019A RID: 410 RVA: 0x00006CB4 File Offset: 0x00004EB4
		public static TResult MapResult<T1, T2, T3, T4, T5, T6, T7, T8, T9, TResult>(this ParserResult<object> result, Func<T1, TResult> parsedFunc1, Func<T2, TResult> parsedFunc2, Func<T3, TResult> parsedFunc3, Func<T4, TResult> parsedFunc4, Func<T5, TResult> parsedFunc5, Func<T6, TResult> parsedFunc6, Func<T7, TResult> parsedFunc7, Func<T8, TResult> parsedFunc8, Func<T9, TResult> parsedFunc9, Func<IEnumerable<Error>, TResult> notParsedFunc)
		{
			Parsed<object> parsed = result as Parsed<object>;
			if (parsed == null)
			{
				return notParsedFunc(((NotParsed<object>)result).Errors);
			}
			if (parsed.Value is T1)
			{
				return parsedFunc1((T1)((object)parsed.Value));
			}
			if (parsed.Value is T2)
			{
				return parsedFunc2((T2)((object)parsed.Value));
			}
			if (parsed.Value is T3)
			{
				return parsedFunc3((T3)((object)parsed.Value));
			}
			if (parsed.Value is T4)
			{
				return parsedFunc4((T4)((object)parsed.Value));
			}
			if (parsed.Value is T5)
			{
				return parsedFunc5((T5)((object)parsed.Value));
			}
			if (parsed.Value is T6)
			{
				return parsedFunc6((T6)((object)parsed.Value));
			}
			if (parsed.Value is T7)
			{
				return parsedFunc7((T7)((object)parsed.Value));
			}
			if (parsed.Value is T8)
			{
				return parsedFunc8((T8)((object)parsed.Value));
			}
			if (parsed.Value is T9)
			{
				return parsedFunc9((T9)((object)parsed.Value));
			}
			throw new InvalidOperationException();
		}

		// Token: 0x0600019B RID: 411 RVA: 0x00006E04 File Offset: 0x00005004
		public static TResult MapResult<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, TResult>(this ParserResult<object> result, Func<T1, TResult> parsedFunc1, Func<T2, TResult> parsedFunc2, Func<T3, TResult> parsedFunc3, Func<T4, TResult> parsedFunc4, Func<T5, TResult> parsedFunc5, Func<T6, TResult> parsedFunc6, Func<T7, TResult> parsedFunc7, Func<T8, TResult> parsedFunc8, Func<T9, TResult> parsedFunc9, Func<T10, TResult> parsedFunc10, Func<IEnumerable<Error>, TResult> notParsedFunc)
		{
			Parsed<object> parsed = result as Parsed<object>;
			if (parsed == null)
			{
				return notParsedFunc(((NotParsed<object>)result).Errors);
			}
			if (parsed.Value is T1)
			{
				return parsedFunc1((T1)((object)parsed.Value));
			}
			if (parsed.Value is T2)
			{
				return parsedFunc2((T2)((object)parsed.Value));
			}
			if (parsed.Value is T3)
			{
				return parsedFunc3((T3)((object)parsed.Value));
			}
			if (parsed.Value is T4)
			{
				return parsedFunc4((T4)((object)parsed.Value));
			}
			if (parsed.Value is T5)
			{
				return parsedFunc5((T5)((object)parsed.Value));
			}
			if (parsed.Value is T6)
			{
				return parsedFunc6((T6)((object)parsed.Value));
			}
			if (parsed.Value is T7)
			{
				return parsedFunc7((T7)((object)parsed.Value));
			}
			if (parsed.Value is T8)
			{
				return parsedFunc8((T8)((object)parsed.Value));
			}
			if (parsed.Value is T9)
			{
				return parsedFunc9((T9)((object)parsed.Value));
			}
			if (parsed.Value is T10)
			{
				return parsedFunc10((T10)((object)parsed.Value));
			}
			throw new InvalidOperationException();
		}

		// Token: 0x0600019C RID: 412 RVA: 0x00006F74 File Offset: 0x00005174
		public static TResult MapResult<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, TResult>(this ParserResult<object> result, Func<T1, TResult> parsedFunc1, Func<T2, TResult> parsedFunc2, Func<T3, TResult> parsedFunc3, Func<T4, TResult> parsedFunc4, Func<T5, TResult> parsedFunc5, Func<T6, TResult> parsedFunc6, Func<T7, TResult> parsedFunc7, Func<T8, TResult> parsedFunc8, Func<T9, TResult> parsedFunc9, Func<T10, TResult> parsedFunc10, Func<T11, TResult> parsedFunc11, Func<IEnumerable<Error>, TResult> notParsedFunc)
		{
			Parsed<object> parsed = result as Parsed<object>;
			if (parsed == null)
			{
				return notParsedFunc(((NotParsed<object>)result).Errors);
			}
			if (parsed.Value is T1)
			{
				return parsedFunc1((T1)((object)parsed.Value));
			}
			if (parsed.Value is T2)
			{
				return parsedFunc2((T2)((object)parsed.Value));
			}
			if (parsed.Value is T3)
			{
				return parsedFunc3((T3)((object)parsed.Value));
			}
			if (parsed.Value is T4)
			{
				return parsedFunc4((T4)((object)parsed.Value));
			}
			if (parsed.Value is T5)
			{
				return parsedFunc5((T5)((object)parsed.Value));
			}
			if (parsed.Value is T6)
			{
				return parsedFunc6((T6)((object)parsed.Value));
			}
			if (parsed.Value is T7)
			{
				return parsedFunc7((T7)((object)parsed.Value));
			}
			if (parsed.Value is T8)
			{
				return parsedFunc8((T8)((object)parsed.Value));
			}
			if (parsed.Value is T9)
			{
				return parsedFunc9((T9)((object)parsed.Value));
			}
			if (parsed.Value is T10)
			{
				return parsedFunc10((T10)((object)parsed.Value));
			}
			if (parsed.Value is T11)
			{
				return parsedFunc11((T11)((object)parsed.Value));
			}
			throw new InvalidOperationException();
		}

		// Token: 0x0600019D RID: 413 RVA: 0x00007104 File Offset: 0x00005304
		public static TResult MapResult<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, TResult>(this ParserResult<object> result, Func<T1, TResult> parsedFunc1, Func<T2, TResult> parsedFunc2, Func<T3, TResult> parsedFunc3, Func<T4, TResult> parsedFunc4, Func<T5, TResult> parsedFunc5, Func<T6, TResult> parsedFunc6, Func<T7, TResult> parsedFunc7, Func<T8, TResult> parsedFunc8, Func<T9, TResult> parsedFunc9, Func<T10, TResult> parsedFunc10, Func<T11, TResult> parsedFunc11, Func<T12, TResult> parsedFunc12, Func<IEnumerable<Error>, TResult> notParsedFunc)
		{
			Parsed<object> parsed = result as Parsed<object>;
			if (parsed == null)
			{
				return notParsedFunc(((NotParsed<object>)result).Errors);
			}
			if (parsed.Value is T1)
			{
				return parsedFunc1((T1)((object)parsed.Value));
			}
			if (parsed.Value is T2)
			{
				return parsedFunc2((T2)((object)parsed.Value));
			}
			if (parsed.Value is T3)
			{
				return parsedFunc3((T3)((object)parsed.Value));
			}
			if (parsed.Value is T4)
			{
				return parsedFunc4((T4)((object)parsed.Value));
			}
			if (parsed.Value is T5)
			{
				return parsedFunc5((T5)((object)parsed.Value));
			}
			if (parsed.Value is T6)
			{
				return parsedFunc6((T6)((object)parsed.Value));
			}
			if (parsed.Value is T7)
			{
				return parsedFunc7((T7)((object)parsed.Value));
			}
			if (parsed.Value is T8)
			{
				return parsedFunc8((T8)((object)parsed.Value));
			}
			if (parsed.Value is T9)
			{
				return parsedFunc9((T9)((object)parsed.Value));
			}
			if (parsed.Value is T10)
			{
				return parsedFunc10((T10)((object)parsed.Value));
			}
			if (parsed.Value is T11)
			{
				return parsedFunc11((T11)((object)parsed.Value));
			}
			if (parsed.Value is T12)
			{
				return parsedFunc12((T12)((object)parsed.Value));
			}
			throw new InvalidOperationException();
		}

		// Token: 0x0600019E RID: 414 RVA: 0x000072B4 File Offset: 0x000054B4
		public static TResult MapResult<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, T13, TResult>(this ParserResult<object> result, Func<T1, TResult> parsedFunc1, Func<T2, TResult> parsedFunc2, Func<T3, TResult> parsedFunc3, Func<T4, TResult> parsedFunc4, Func<T5, TResult> parsedFunc5, Func<T6, TResult> parsedFunc6, Func<T7, TResult> parsedFunc7, Func<T8, TResult> parsedFunc8, Func<T9, TResult> parsedFunc9, Func<T10, TResult> parsedFunc10, Func<T11, TResult> parsedFunc11, Func<T12, TResult> parsedFunc12, Func<T13, TResult> parsedFunc13, Func<IEnumerable<Error>, TResult> notParsedFunc)
		{
			Parsed<object> parsed = result as Parsed<object>;
			if (parsed == null)
			{
				return notParsedFunc(((NotParsed<object>)result).Errors);
			}
			if (parsed.Value is T1)
			{
				return parsedFunc1((T1)((object)parsed.Value));
			}
			if (parsed.Value is T2)
			{
				return parsedFunc2((T2)((object)parsed.Value));
			}
			if (parsed.Value is T3)
			{
				return parsedFunc3((T3)((object)parsed.Value));
			}
			if (parsed.Value is T4)
			{
				return parsedFunc4((T4)((object)parsed.Value));
			}
			if (parsed.Value is T5)
			{
				return parsedFunc5((T5)((object)parsed.Value));
			}
			if (parsed.Value is T6)
			{
				return parsedFunc6((T6)((object)parsed.Value));
			}
			if (parsed.Value is T7)
			{
				return parsedFunc7((T7)((object)parsed.Value));
			}
			if (parsed.Value is T8)
			{
				return parsedFunc8((T8)((object)parsed.Value));
			}
			if (parsed.Value is T9)
			{
				return parsedFunc9((T9)((object)parsed.Value));
			}
			if (parsed.Value is T10)
			{
				return parsedFunc10((T10)((object)parsed.Value));
			}
			if (parsed.Value is T11)
			{
				return parsedFunc11((T11)((object)parsed.Value));
			}
			if (parsed.Value is T12)
			{
				return parsedFunc12((T12)((object)parsed.Value));
			}
			if (parsed.Value is T13)
			{
				return parsedFunc13((T13)((object)parsed.Value));
			}
			throw new InvalidOperationException();
		}

		// Token: 0x0600019F RID: 415 RVA: 0x00007484 File Offset: 0x00005684
		public static TResult MapResult<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, T13, T14, TResult>(this ParserResult<object> result, Func<T1, TResult> parsedFunc1, Func<T2, TResult> parsedFunc2, Func<T3, TResult> parsedFunc3, Func<T4, TResult> parsedFunc4, Func<T5, TResult> parsedFunc5, Func<T6, TResult> parsedFunc6, Func<T7, TResult> parsedFunc7, Func<T8, TResult> parsedFunc8, Func<T9, TResult> parsedFunc9, Func<T10, TResult> parsedFunc10, Func<T11, TResult> parsedFunc11, Func<T12, TResult> parsedFunc12, Func<T13, TResult> parsedFunc13, Func<T14, TResult> parsedFunc14, Func<IEnumerable<Error>, TResult> notParsedFunc)
		{
			Parsed<object> parsed = result as Parsed<object>;
			if (parsed == null)
			{
				return notParsedFunc(((NotParsed<object>)result).Errors);
			}
			if (parsed.Value is T1)
			{
				return parsedFunc1((T1)((object)parsed.Value));
			}
			if (parsed.Value is T2)
			{
				return parsedFunc2((T2)((object)parsed.Value));
			}
			if (parsed.Value is T3)
			{
				return parsedFunc3((T3)((object)parsed.Value));
			}
			if (parsed.Value is T4)
			{
				return parsedFunc4((T4)((object)parsed.Value));
			}
			if (parsed.Value is T5)
			{
				return parsedFunc5((T5)((object)parsed.Value));
			}
			if (parsed.Value is T6)
			{
				return parsedFunc6((T6)((object)parsed.Value));
			}
			if (parsed.Value is T7)
			{
				return parsedFunc7((T7)((object)parsed.Value));
			}
			if (parsed.Value is T8)
			{
				return parsedFunc8((T8)((object)parsed.Value));
			}
			if (parsed.Value is T9)
			{
				return parsedFunc9((T9)((object)parsed.Value));
			}
			if (parsed.Value is T10)
			{
				return parsedFunc10((T10)((object)parsed.Value));
			}
			if (parsed.Value is T11)
			{
				return parsedFunc11((T11)((object)parsed.Value));
			}
			if (parsed.Value is T12)
			{
				return parsedFunc12((T12)((object)parsed.Value));
			}
			if (parsed.Value is T13)
			{
				return parsedFunc13((T13)((object)parsed.Value));
			}
			if (parsed.Value is T14)
			{
				return parsedFunc14((T14)((object)parsed.Value));
			}
			throw new InvalidOperationException();
		}

		// Token: 0x060001A0 RID: 416 RVA: 0x00007674 File Offset: 0x00005874
		public static TResult MapResult<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, T13, T14, T15, TResult>(this ParserResult<object> result, Func<T1, TResult> parsedFunc1, Func<T2, TResult> parsedFunc2, Func<T3, TResult> parsedFunc3, Func<T4, TResult> parsedFunc4, Func<T5, TResult> parsedFunc5, Func<T6, TResult> parsedFunc6, Func<T7, TResult> parsedFunc7, Func<T8, TResult> parsedFunc8, Func<T9, TResult> parsedFunc9, Func<T10, TResult> parsedFunc10, Func<T11, TResult> parsedFunc11, Func<T12, TResult> parsedFunc12, Func<T13, TResult> parsedFunc13, Func<T14, TResult> parsedFunc14, Func<T15, TResult> parsedFunc15, Func<IEnumerable<Error>, TResult> notParsedFunc)
		{
			Parsed<object> parsed = result as Parsed<object>;
			if (parsed == null)
			{
				return notParsedFunc(((NotParsed<object>)result).Errors);
			}
			if (parsed.Value is T1)
			{
				return parsedFunc1((T1)((object)parsed.Value));
			}
			if (parsed.Value is T2)
			{
				return parsedFunc2((T2)((object)parsed.Value));
			}
			if (parsed.Value is T3)
			{
				return parsedFunc3((T3)((object)parsed.Value));
			}
			if (parsed.Value is T4)
			{
				return parsedFunc4((T4)((object)parsed.Value));
			}
			if (parsed.Value is T5)
			{
				return parsedFunc5((T5)((object)parsed.Value));
			}
			if (parsed.Value is T6)
			{
				return parsedFunc6((T6)((object)parsed.Value));
			}
			if (parsed.Value is T7)
			{
				return parsedFunc7((T7)((object)parsed.Value));
			}
			if (parsed.Value is T8)
			{
				return parsedFunc8((T8)((object)parsed.Value));
			}
			if (parsed.Value is T9)
			{
				return parsedFunc9((T9)((object)parsed.Value));
			}
			if (parsed.Value is T10)
			{
				return parsedFunc10((T10)((object)parsed.Value));
			}
			if (parsed.Value is T11)
			{
				return parsedFunc11((T11)((object)parsed.Value));
			}
			if (parsed.Value is T12)
			{
				return parsedFunc12((T12)((object)parsed.Value));
			}
			if (parsed.Value is T13)
			{
				return parsedFunc13((T13)((object)parsed.Value));
			}
			if (parsed.Value is T14)
			{
				return parsedFunc14((T14)((object)parsed.Value));
			}
			if (parsed.Value is T15)
			{
				return parsedFunc15((T15)((object)parsed.Value));
			}
			throw new InvalidOperationException();
		}

		// Token: 0x060001A1 RID: 417 RVA: 0x00007884 File Offset: 0x00005A84
		public static TResult MapResult<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, T13, T14, T15, T16, TResult>(this ParserResult<object> result, Func<T1, TResult> parsedFunc1, Func<T2, TResult> parsedFunc2, Func<T3, TResult> parsedFunc3, Func<T4, TResult> parsedFunc4, Func<T5, TResult> parsedFunc5, Func<T6, TResult> parsedFunc6, Func<T7, TResult> parsedFunc7, Func<T8, TResult> parsedFunc8, Func<T9, TResult> parsedFunc9, Func<T10, TResult> parsedFunc10, Func<T11, TResult> parsedFunc11, Func<T12, TResult> parsedFunc12, Func<T13, TResult> parsedFunc13, Func<T14, TResult> parsedFunc14, Func<T15, TResult> parsedFunc15, Func<T16, TResult> parsedFunc16, Func<IEnumerable<Error>, TResult> notParsedFunc)
		{
			Parsed<object> parsed = result as Parsed<object>;
			if (parsed == null)
			{
				return notParsedFunc(((NotParsed<object>)result).Errors);
			}
			if (parsed.Value is T1)
			{
				return parsedFunc1((T1)((object)parsed.Value));
			}
			if (parsed.Value is T2)
			{
				return parsedFunc2((T2)((object)parsed.Value));
			}
			if (parsed.Value is T3)
			{
				return parsedFunc3((T3)((object)parsed.Value));
			}
			if (parsed.Value is T4)
			{
				return parsedFunc4((T4)((object)parsed.Value));
			}
			if (parsed.Value is T5)
			{
				return parsedFunc5((T5)((object)parsed.Value));
			}
			if (parsed.Value is T6)
			{
				return parsedFunc6((T6)((object)parsed.Value));
			}
			if (parsed.Value is T7)
			{
				return parsedFunc7((T7)((object)parsed.Value));
			}
			if (parsed.Value is T8)
			{
				return parsedFunc8((T8)((object)parsed.Value));
			}
			if (parsed.Value is T9)
			{
				return parsedFunc9((T9)((object)parsed.Value));
			}
			if (parsed.Value is T10)
			{
				return parsedFunc10((T10)((object)parsed.Value));
			}
			if (parsed.Value is T11)
			{
				return parsedFunc11((T11)((object)parsed.Value));
			}
			if (parsed.Value is T12)
			{
				return parsedFunc12((T12)((object)parsed.Value));
			}
			if (parsed.Value is T13)
			{
				return parsedFunc13((T13)((object)parsed.Value));
			}
			if (parsed.Value is T14)
			{
				return parsedFunc14((T14)((object)parsed.Value));
			}
			if (parsed.Value is T15)
			{
				return parsedFunc15((T15)((object)parsed.Value));
			}
			if (parsed.Value is T16)
			{
				return parsedFunc16((T16)((object)parsed.Value));
			}
			throw new InvalidOperationException();
		}
	}
}
