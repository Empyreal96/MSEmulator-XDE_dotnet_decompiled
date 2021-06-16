using System;
using System.Collections;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Linq.Expressions;
using System.Management.Automation.Language;
using System.Reflection;

namespace System.Management.Automation
{
	// Token: 0x020005F9 RID: 1529
	internal abstract class MutableTuple
	{
		// Token: 0x060041DD RID: 16861 RVA: 0x0015CC24 File Offset: 0x0015AE24
		internal bool IsValueSet(int index)
		{
			if (this._size < 128)
			{
				return this._valuesSet[index];
			}
			MutableTuple mutableTuple = this;
			int[] array = MutableTuple.GetAccessPath(this._size, index).ToArray<int>();
			for (int i = 0; i < array.Length - 1; i++)
			{
				mutableTuple = (MutableTuple)mutableTuple.GetValueImpl(array[i]);
			}
			return mutableTuple._valuesSet[array.Last<int>()];
		}

		// Token: 0x060041DE RID: 16862 RVA: 0x0015CC8F File Offset: 0x0015AE8F
		internal void SetAutomaticVariable(AutomaticVariable auto, object value, ExecutionContext context)
		{
			if (context._debuggingMode > 0)
			{
				context.Debugger.CheckVariableWrite(SpecialVariables.AutomaticVariables[(int)auto]);
			}
			this.SetValue((int)auto, value);
		}

		// Token: 0x060041DF RID: 16863 RVA: 0x0015CCB4 File Offset: 0x0015AEB4
		internal object GetAutomaticVariable(AutomaticVariable auto)
		{
			return this.GetValue((int)auto);
		}

		// Token: 0x060041E0 RID: 16864 RVA: 0x0015CCBD File Offset: 0x0015AEBD
		internal void SetPreferenceVariable(PreferenceVariable pref, object value)
		{
			this.SetValue((int)pref, value);
		}

		// Token: 0x060041E1 RID: 16865 RVA: 0x0015CCC8 File Offset: 0x0015AEC8
		internal bool TryGetLocalVariable(string name, bool fromNewOrSet, out PSVariable result)
		{
			name = VariableAnalysis.GetUnaliasedVariableName(name);
			int num;
			if (this._nameToIndexMap.TryGetValue(name, out num) && (fromNewOrSet || this.IsValueSet(num)))
			{
				result = new LocalVariable(name, this, num);
				return true;
			}
			result = null;
			return false;
		}

		// Token: 0x060041E2 RID: 16866 RVA: 0x0015CD0C File Offset: 0x0015AF0C
		internal bool TrySetParameter(string name, object value)
		{
			name = VariableAnalysis.GetUnaliasedVariableName(name);
			int index;
			if (this._nameToIndexMap.TryGetValue(name, out index))
			{
				this.SetValue(index, value);
				return true;
			}
			return false;
		}

		// Token: 0x060041E3 RID: 16867 RVA: 0x0015CD3C File Offset: 0x0015AF3C
		internal PSVariable TrySetVariable(string name, object value)
		{
			name = VariableAnalysis.GetUnaliasedVariableName(name);
			int num;
			if (this._nameToIndexMap.TryGetValue(name, out num))
			{
				this.SetValue(num, value);
				return new LocalVariable(name, this, num);
			}
			return null;
		}

		// Token: 0x060041E4 RID: 16868 RVA: 0x0015CD88 File Offset: 0x0015AF88
		internal void GetVariableTable(Dictionary<string, PSVariable> result, bool includePrivate)
		{
			string[] array = (from keyValuePairs in this._nameToIndexMap
			orderby keyValuePairs.Value
			select keyValuePairs.Key).ToArray<string>();
			for (int i = 0; i < array.Length; i++)
			{
				string text = array[i];
				if (this.IsValueSet(i) && !result.ContainsKey(text))
				{
					result.Add(text, new LocalVariable(text, this, i));
					if (text.Equals("_", StringComparison.OrdinalIgnoreCase))
					{
						result.Add("PSItem", new LocalVariable("PSItem", this, i));
					}
				}
			}
		}

		// Token: 0x060041E5 RID: 16869 RVA: 0x0015CE3D File Offset: 0x0015B03D
		public object GetValue(int index)
		{
			return this.GetNestedValue(this._size, index);
		}

		// Token: 0x060041E6 RID: 16870 RVA: 0x0015CE4C File Offset: 0x0015B04C
		public void SetValue(int index, object value)
		{
			this.SetNestedValue(this._size, index, value);
		}

		// Token: 0x060041E7 RID: 16871
		protected abstract object GetValueImpl(int index);

		// Token: 0x060041E8 RID: 16872
		protected abstract void SetValueImpl(int index, object value);

		// Token: 0x060041E9 RID: 16873 RVA: 0x0015CE5C File Offset: 0x0015B05C
		private void SetNestedValue(int size, int index, object value)
		{
			if (size < 128)
			{
				this.SetValueImpl(index, value);
				return;
			}
			MutableTuple mutableTuple = this;
			int num = -1;
			foreach (int num2 in MutableTuple.GetAccessPath(size, index))
			{
				if (num != -1)
				{
					mutableTuple = (MutableTuple)mutableTuple.GetValueImpl(num);
				}
				num = num2;
			}
			mutableTuple.SetValueImpl(num, value);
		}

		// Token: 0x060041EA RID: 16874 RVA: 0x0015CED4 File Offset: 0x0015B0D4
		private object GetNestedValue(int size, int index)
		{
			if (size < 128)
			{
				return this.GetValueImpl(index);
			}
			object obj = this;
			foreach (int index2 in MutableTuple.GetAccessPath(size, index))
			{
				obj = ((MutableTuple)obj).GetValueImpl(index2);
			}
			return obj;
		}

		// Token: 0x060041EB RID: 16875 RVA: 0x0015CF3C File Offset: 0x0015B13C
		private static Type GetTupleType(int size)
		{
			if (size > 128)
			{
				return null;
			}
			if (size <= 1)
			{
				return typeof(MutableTuple<>);
			}
			if (size <= 2)
			{
				return typeof(MutableTuple<, >);
			}
			if (size <= 4)
			{
				return typeof(MutableTuple<, , , >);
			}
			if (size <= 8)
			{
				return typeof(MutableTuple<, , , , , , , >);
			}
			if (size <= 16)
			{
				return typeof(MutableTuple<, , , , , , , , , , , , , , , >);
			}
			if (size <= 32)
			{
				return typeof(MutableTuple<, , , , , , , , , , , , , , , , , , , , , , , , , , , , , , , >);
			}
			if (size <= 64)
			{
				return typeof(MutableTuple<, , , , , , , , , , , , , , , , , , , , , , , , , , , , , , , , , , , , , , , , , , , , , , , , , , , , , , , , , , , , , , , >);
			}
			return typeof(MutableTuple<, , , , , , , , , , , , , , , , , , , , , , , , , , , , , , , , , , , , , , , , , , , , , , , , , , , , , , , , , , , , , , , , , , , , , , , , , , , , , , , , , , , , , , , , , , , , , , , , , , , , , , , , , , , , , , , , , , , , , , , , , , , , , , , >);
		}

		// Token: 0x060041EC RID: 16876 RVA: 0x0015CFC9 File Offset: 0x0015B1C9
		public static Type MakeTupleType(params Type[] types)
		{
			return MutableTuple.MakeTupleType(types, 0, types.Length);
		}

		// Token: 0x060041ED RID: 16877 RVA: 0x0015CFD8 File Offset: 0x0015B1D8
		public static int GetSize(Type tupleType)
		{
			int num = 0;
			lock (MutableTuple._sizeDict)
			{
				if (MutableTuple._sizeDict.TryGetValue(tupleType, out num))
				{
					return num;
				}
			}
			Stack<Type> stack = new Stack<Type>(tupleType.GetGenericArguments());
			while (stack.Count != 0)
			{
				Type type = stack.Pop();
				if (typeof(MutableTuple).IsAssignableFrom(type))
				{
					foreach (Type item in type.GetGenericArguments())
					{
						stack.Push(item);
					}
				}
				else if (!(type == typeof(LanguagePrimitives.Null)))
				{
					num++;
				}
			}
			lock (MutableTuple._sizeDict)
			{
				MutableTuple._sizeDict[tupleType] = num;
			}
			return num;
		}

		// Token: 0x060041EE RID: 16878 RVA: 0x0015D0DC File Offset: 0x0015B2DC
		public static MutableTuple MakeTuple(Type tupleType, Dictionary<string, int> nameToIndexMap)
		{
			int size = MutableTuple.GetSize(tupleType);
			BitArray bitArray = new BitArray(size);
			MutableTuple mutableTuple = MutableTuple.MakeTuple(tupleType, size, bitArray);
			mutableTuple._nameToIndexMap = nameToIndexMap;
			return mutableTuple;
		}

		// Token: 0x060041EF RID: 16879 RVA: 0x0015D108 File Offset: 0x0015B308
		public static object[] GetTupleValues(MutableTuple tuple)
		{
			List<object> list = new List<object>();
			MutableTuple.GetTupleValues(tuple, list);
			return list.ToArray();
		}

		// Token: 0x060041F0 RID: 16880 RVA: 0x0015D128 File Offset: 0x0015B328
		public static IEnumerable<PropertyInfo> GetAccessPath(Type tupleType, int index)
		{
			return MutableTuple.GetAccessProperties(tupleType, MutableTuple.GetSize(tupleType), index);
		}

		// Token: 0x060041F1 RID: 16881 RVA: 0x0015D368 File Offset: 0x0015B568
		internal static IEnumerable<PropertyInfo> GetAccessProperties(Type tupleType, int size, int index)
		{
			if (index < 0 || index >= size)
			{
				throw new ArgumentException("index");
			}
			foreach (int curIndex in MutableTuple.GetAccessPath(size, index))
			{
				PropertyInfo pi = tupleType.GetProperty("Item" + string.Format(CultureInfo.InvariantCulture, "{0:D3}", new object[]
				{
					curIndex
				}));
				yield return pi;
				tupleType = pi.PropertyType;
			}
			yield break;
		}

		// Token: 0x060041F2 RID: 16882 RVA: 0x0015D538 File Offset: 0x0015B738
		internal static IEnumerable<int> GetAccessPath(int size, int index)
		{
			int depth = 0;
			int mask = 127;
			int adjust = 1;
			int count = size;
			while (count > 128)
			{
				depth++;
				count /= 128;
				mask *= 128;
				adjust *= 128;
			}
			for (;;)
			{
				int num;
				depth = (num = depth) - 1;
				if (num < 0)
				{
					break;
				}
				int curIndex = (index & mask) / adjust;
				yield return curIndex;
				mask /= 128;
				adjust /= 128;
			}
			yield break;
		}

		// Token: 0x060041F3 RID: 16883 RVA: 0x0015D55C File Offset: 0x0015B75C
		private static void GetTupleValues(MutableTuple tuple, List<object> args)
		{
			Type[] genericArguments = tuple.GetType().GetGenericArguments();
			for (int i = 0; i < genericArguments.Length; i++)
			{
				if (typeof(MutableTuple).IsAssignableFrom(genericArguments[i]))
				{
					MutableTuple.GetTupleValues((MutableTuple)tuple.GetValue(i), args);
				}
				else if (genericArguments[i] != typeof(LanguagePrimitives.Null))
				{
					args.Add(tuple.GetValue(i));
				}
			}
		}

		// Token: 0x060041F4 RID: 16884 RVA: 0x0015D5CC File Offset: 0x0015B7CC
		private static MutableTuple MakeTuple(Type tupleType, int size, BitArray bitArray)
		{
			MutableTuple mutableTuple = (MutableTuple)Activator.CreateInstance(tupleType);
			mutableTuple._size = size;
			mutableTuple._valuesSet = bitArray;
			if (size > 128)
			{
				while (size > 128)
				{
					size = (size + 128 - 1) / 128;
				}
				for (int i = 0; i < size; i++)
				{
					PropertyInfo property = tupleType.GetProperty("Item" + string.Format(CultureInfo.InvariantCulture, "{0:D3}", new object[]
					{
						i
					}));
					mutableTuple.SetValueImpl(i, MutableTuple.MakeTuple(property.PropertyType, null));
				}
			}
			return mutableTuple;
		}

		// Token: 0x060041F5 RID: 16885 RVA: 0x0015D668 File Offset: 0x0015B868
		private static Type MakeTupleType(Type[] types, int start, int end)
		{
			int i = end - start;
			Type tupleType = MutableTuple.GetTupleType(i);
			if (tupleType != null)
			{
				Type[] array = new Type[tupleType.GetGenericArguments().Length];
				int j = 0;
				for (int k = start; k < end; k++)
				{
					array[j++] = types[k];
				}
				while (j < array.Length)
				{
					array[j++] = typeof(LanguagePrimitives.Null);
				}
				return tupleType.MakeGenericType(array);
			}
			int num = 1;
			while (i > 128)
			{
				i = (i + 128 - 1) / 128;
				num *= 128;
			}
			tupleType = MutableTuple.GetTupleType(i);
			Type[] array2 = new Type[tupleType.GetGenericArguments().Length];
			for (int l = 0; l < i; l++)
			{
				int start2 = start + l * num;
				int end2 = Math.Min(end, start + (l + 1) * num);
				array2[l] = MutableTuple.MakeTupleType(types, start2, end2);
			}
			for (int m = i; m < array2.Length; m++)
			{
				array2[m] = typeof(LanguagePrimitives.Null);
			}
			return tupleType.MakeGenericType(array2);
		}

		// Token: 0x17000E1E RID: 3614
		// (get) Token: 0x060041F6 RID: 16886
		public abstract int Capacity { get; }

		// Token: 0x060041F7 RID: 16887 RVA: 0x0015D77D File Offset: 0x0015B97D
		public static Expression Create(params Expression[] values)
		{
			return MutableTuple.CreateNew(MutableTuple.MakeTupleType((from x in values
			select x.Type).ToArray<Type>()), 0, values.Length, values);
		}

		// Token: 0x060041F8 RID: 16888 RVA: 0x0015D7B8 File Offset: 0x0015B9B8
		private static int PowerOfTwoRound(int value)
		{
			int num = 1;
			while (value > num)
			{
				num <<= 1;
			}
			return num;
		}

		// Token: 0x060041F9 RID: 16889 RVA: 0x0015D7DC File Offset: 0x0015B9DC
		internal static Expression CreateNew(Type tupleType, int start, int end, Expression[] values)
		{
			int i = end - start;
			Expression[] array;
			if (i > 128)
			{
				int num = 1;
				while (i > 128)
				{
					i = (i + 128 - 1) / 128;
					num *= 128;
				}
				array = new Expression[MutableTuple.PowerOfTwoRound(i)];
				for (int j = 0; j < i; j++)
				{
					int start2 = start + j * num;
					int end2 = Math.Min(end, start + (j + 1) * num);
					PropertyInfo property = tupleType.GetProperty("Item" + string.Format(CultureInfo.InvariantCulture, "{0:D3}", new object[]
					{
						j
					}));
					array[j] = MutableTuple.CreateNew(property.PropertyType, start2, end2, values);
				}
				for (int k = i; k < array.Length; k++)
				{
					array[k] = Expression.Constant(null, typeof(LanguagePrimitives.Null));
				}
			}
			else
			{
				array = new Expression[MutableTuple.PowerOfTwoRound(i)];
				for (int l = 0; l < i; l++)
				{
					array[l] = values[l + start];
				}
				for (int m = i; m < array.Length; m++)
				{
					array[m] = Expression.Constant(null, typeof(LanguagePrimitives.Null));
				}
			}
			return Expression.New(tupleType.GetConstructor((from x in array
			select x.Type).ToArray<Type>()), array);
		}

		// Token: 0x04002101 RID: 8449
		private const int MaxSize = 128;

		// Token: 0x04002102 RID: 8450
		private static readonly Dictionary<Type, int> _sizeDict = new Dictionary<Type, int>();

		// Token: 0x04002103 RID: 8451
		private int _size;

		// Token: 0x04002104 RID: 8452
		protected BitArray _valuesSet;

		// Token: 0x04002105 RID: 8453
		private Dictionary<string, int> _nameToIndexMap;
	}
}
