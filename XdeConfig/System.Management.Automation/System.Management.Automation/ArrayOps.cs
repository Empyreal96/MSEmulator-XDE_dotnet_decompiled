using System;
using System.Collections;
using System.Collections.Generic;
using System.Globalization;
using System.Management.Automation.Internal;
using System.Management.Automation.Runspaces;

namespace System.Management.Automation
{
	// Token: 0x02000627 RID: 1575
	internal static class ArrayOps
	{
		// Token: 0x06004478 RID: 17528 RVA: 0x0016DAC0 File Offset: 0x0016BCC0
		internal static object[] SlicingIndex(object target, object[] indexes, Func<object, object, object> indexer)
		{
			object[] array = new object[indexes.Length];
			int num = 0;
			foreach (object arg in indexes)
			{
				object obj = indexer(target, arg);
				if (obj != AutomationNull.Value)
				{
					array[num++] = obj;
				}
			}
			if (num != indexes.Length)
			{
				object[] array2 = new object[num];
				Array.Copy(array, array2, num);
				return array2;
			}
			return array;
		}

		// Token: 0x06004479 RID: 17529 RVA: 0x0016DB28 File Offset: 0x0016BD28
		internal static T[] Multiply<T>(T[] array, uint times)
		{
			if (times == 1U)
			{
				return array;
			}
			if (times == 0U || array.Length == 0)
			{
				return new T[0];
			}
			ExecutionContext executionContextFromTLS = LocalPipeline.GetExecutionContextFromTLS();
			if (executionContextFromTLS != null && executionContextFromTLS.LanguageMode == PSLanguageMode.RestrictedLanguage && (long)array.Length * (long)((ulong)times) > 1024L)
			{
				throw InterpreterError.NewInterpreterException(times, typeof(RuntimeException), null, "ArrayMultiplyToolongInDataSection", ParserStrings.ArrayMultiplyToolongInDataSection, new object[]
				{
					1024
				});
			}
			long num = (long)array.Length * (long)((ulong)times);
			int num2 = -1;
			try
			{
				num2 = checked((int)num);
			}
			catch (OverflowException)
			{
				LanguagePrimitives.ThrowInvalidCastException(num, typeof(int));
			}
			T[] array2 = new T[num2];
			int num3 = array.Length;
			Array.Copy(array, 0, array2, 0, num3);
			for (times >>= 1; times != 0U; times >>= 1)
			{
				Array.Copy(array2, 0, array2, num3, num3);
				num3 *= 2;
			}
			if (array2.Length != num3)
			{
				Array.Copy(array2, 0, array2, num3, array2.Length - num3);
			}
			return array2;
		}

		// Token: 0x0600447A RID: 17530 RVA: 0x0016DC2C File Offset: 0x0016BE2C
		internal static object GetMDArrayValue(Array array, int[] indexes, bool slicing)
		{
			if (array.Rank != indexes.Length)
			{
				ArrayOps.ReportIndexingError(array, indexes, null);
			}
			for (int i = 0; i < indexes.Length; i++)
			{
				int upperBound = array.GetUpperBound(i);
				int lowerBound = array.GetLowerBound(i);
				if (indexes[i] < lowerBound)
				{
					indexes[i] = indexes[i] + upperBound + 1;
				}
				if (indexes[i] < lowerBound || indexes[i] > upperBound)
				{
					ExecutionContext executionContextFromTLS = LocalPipeline.GetExecutionContextFromTLS();
					if (executionContextFromTLS != null && !executionContextFromTLS.IsStrictVersion(3))
					{
						if (!slicing)
						{
							return null;
						}
						return AutomationNull.Value;
					}
				}
			}
			return array.GetValue(indexes);
		}

		// Token: 0x0600447B RID: 17531 RVA: 0x0016DCAC File Offset: 0x0016BEAC
		internal static object GetMDArrayValueOrSlice(Array array, object indexes)
		{
			Exception ex = null;
			int[] array2 = null;
			try
			{
				array2 = (int[])LanguagePrimitives.ConvertTo(indexes, typeof(int[]), NumberFormatInfo.InvariantInfo);
			}
			catch (InvalidCastException ex2)
			{
				ex = ex2;
			}
			if (array2 != null)
			{
				if (array2.Length != array.Rank)
				{
					ArrayOps.ReportIndexingError(array, indexes, null);
				}
				return ArrayOps.GetMDArrayValue(array, array2, false);
			}
			List<int[]> list = new List<int[]>();
			IEnumerator enumerator = LanguagePrimitives.GetEnumerator(indexes);
			while (EnumerableOps.MoveNext(null, enumerator))
			{
				object obj = EnumerableOps.Current(enumerator);
				try
				{
					array2 = LanguagePrimitives.ConvertTo<int[]>(obj);
				}
				catch (InvalidCastException)
				{
					array2 = null;
				}
				if (array2 == null || array2.Length != array.Rank)
				{
					if (ex != null)
					{
						ArrayOps.ReportIndexingError(array, indexes, ex);
					}
					ArrayOps.ReportIndexingError(array, obj, null);
				}
				ex = null;
				list.Add(array2);
			}
			object[] array3 = new object[list.Count];
			int num = 0;
			foreach (int[] indexes2 in list)
			{
				object mdarrayValue = ArrayOps.GetMDArrayValue(array, indexes2, true);
				if (mdarrayValue != AutomationNull.Value)
				{
					array3[num++] = mdarrayValue;
				}
			}
			if (num != list.Count)
			{
				object[] array4 = new object[num];
				Array.Copy(array3, array4, num);
				return array4;
			}
			return array3;
		}

		// Token: 0x0600447C RID: 17532 RVA: 0x0016DE04 File Offset: 0x0016C004
		private static void ReportIndexingError(Array array, object index, Exception reason)
		{
			string text = ArrayOps.IndexStringMessage(index);
			if (reason == null)
			{
				throw InterpreterError.NewInterpreterException(index, typeof(RuntimeException), null, "NeedMultidimensionalIndex", ParserStrings.NeedMultidimensionalIndex, new object[]
				{
					array.Rank,
					text
				});
			}
			throw InterpreterError.NewInterpreterExceptionWithInnerException(index, typeof(RuntimeException), null, "NeedMultidimensionalIndex", ParserStrings.NeedMultidimensionalIndex, reason, new object[]
			{
				array.Rank,
				text
			});
		}

		// Token: 0x0600447D RID: 17533 RVA: 0x0016DE88 File Offset: 0x0016C088
		private static string IndexStringMessage(object index)
		{
			string text = PSObject.ToString(null, index, ",", null, null, true, true);
			if (text.Length > 20)
			{
				text = text.Substring(0, 20) + " ...";
			}
			return text;
		}

		// Token: 0x0600447E RID: 17534 RVA: 0x0016DEC8 File Offset: 0x0016C0C8
		internal static object SetMDArrayValue(Array array, int[] indexes, object value)
		{
			if (array.Rank != indexes.Length)
			{
				ArrayOps.ReportIndexingError(array, indexes, null);
			}
			for (int i = 0; i < indexes.Length; i++)
			{
				int upperBound = array.GetUpperBound(i);
				int lowerBound = array.GetLowerBound(i);
				if (indexes[i] < lowerBound)
				{
					indexes[i] = indexes[i] + upperBound + 1;
				}
			}
			array.SetValue(value, indexes);
			return value;
		}

		// Token: 0x0600447F RID: 17535 RVA: 0x0016DF20 File Offset: 0x0016C120
		internal static object GetNonIndexable(object target, object[] indices)
		{
			if (indices.Length == 1)
			{
				object obj = indices[0];
				if (obj != null && (LanguagePrimitives.Equals(0, obj) || LanguagePrimitives.Equals(-1, obj)))
				{
					return target;
				}
			}
			ExecutionContext executionContextFromTLS = LocalPipeline.GetExecutionContextFromTLS();
			if (executionContextFromTLS == null || !executionContextFromTLS.IsStrictVersion(2))
			{
				return AutomationNull.Value;
			}
			throw InterpreterError.NewInterpreterException(target, typeof(RuntimeException), null, "CannotIndex", ParserStrings.CannotIndex, new object[]
			{
				target.GetType()
			});
		}
	}
}
