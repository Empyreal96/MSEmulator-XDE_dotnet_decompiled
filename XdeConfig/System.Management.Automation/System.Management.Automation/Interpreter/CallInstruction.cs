using System;
using System.Collections.Generic;
using System.Reflection;
using System.Reflection.Emit;

namespace System.Management.Automation.Interpreter
{
	// Token: 0x0200066B RID: 1643
	internal abstract class CallInstruction : Instruction
	{
		// Token: 0x17000ECE RID: 3790
		// (get) Token: 0x06004604 RID: 17924
		public abstract MethodInfo Info { get; }

		// Token: 0x17000ECF RID: 3791
		// (get) Token: 0x06004605 RID: 17925
		public abstract int ArgumentCount { get; }

		// Token: 0x06004606 RID: 17926 RVA: 0x00177410 File Offset: 0x00175610
		internal CallInstruction()
		{
		}

		// Token: 0x06004607 RID: 17927 RVA: 0x00177418 File Offset: 0x00175618
		public static CallInstruction Create(MethodInfo info)
		{
			return CallInstruction.Create(info, info.GetParameters());
		}

		// Token: 0x06004608 RID: 17928 RVA: 0x00177428 File Offset: 0x00175628
		public static CallInstruction Create(MethodInfo info, ParameterInfo[] parameters)
		{
			int num = parameters.Length;
			if (!info.IsStatic)
			{
				num++;
			}
			if (info.DeclaringType != null && info.DeclaringType.IsArray && (info.Name == "Get" || info.Name == "Set"))
			{
				return CallInstruction.GetArrayAccessor(info, num);
			}
			if (info is DynamicMethod || (!info.IsStatic && info.DeclaringType.GetTypeInfo().IsValueType))
			{
				return new MethodInfoCallInstruction(info, num);
			}
			if (num >= 10)
			{
				return new MethodInfoCallInstruction(info, num);
			}
			foreach (ParameterInfo parameterInfo in parameters)
			{
				if (parameterInfo.ParameterType.IsByRef)
				{
					return new MethodInfoCallInstruction(info, num);
				}
			}
			CallInstruction callInstruction;
			if (CallInstruction.ShouldCache(info))
			{
				lock (CallInstruction._cache)
				{
					if (CallInstruction._cache.TryGetValue(info, out callInstruction))
					{
						return callInstruction;
					}
				}
			}
			try
			{
				if (num < 3)
				{
					callInstruction = CallInstruction.FastCreate(info, parameters);
				}
				else
				{
					callInstruction = CallInstruction.SlowCreate(info, parameters);
				}
			}
			catch (TargetInvocationException ex)
			{
				if (!(ex.InnerException is NotSupportedException))
				{
					throw;
				}
				callInstruction = new MethodInfoCallInstruction(info, num);
			}
			catch (NotSupportedException)
			{
				callInstruction = new MethodInfoCallInstruction(info, num);
			}
			if (CallInstruction.ShouldCache(info))
			{
				lock (CallInstruction._cache)
				{
					CallInstruction._cache[info] = callInstruction;
				}
			}
			return callInstruction;
		}

		// Token: 0x06004609 RID: 17929 RVA: 0x001775E4 File Offset: 0x001757E4
		private static CallInstruction GetArrayAccessor(MethodInfo info, int argumentCount)
		{
			Type declaringType = info.DeclaringType;
			bool flag = info.Name == "Get";
			switch (declaringType.GetArrayRank())
			{
			case 1:
				return CallInstruction.Create(flag ? declaringType.GetMethod("GetValue", new Type[]
				{
					typeof(int)
				}) : new Action<Array, int, object>(CallInstruction.ArrayItemSetter1).GetMethodInfo());
			case 2:
				return CallInstruction.Create(flag ? declaringType.GetMethod("GetValue", new Type[]
				{
					typeof(int),
					typeof(int)
				}) : new Action<Array, int, int, object>(CallInstruction.ArrayItemSetter2).GetMethodInfo());
			case 3:
				return CallInstruction.Create(flag ? declaringType.GetMethod("GetValue", new Type[]
				{
					typeof(int),
					typeof(int),
					typeof(int)
				}) : new Action<Array, int, int, int, object>(CallInstruction.ArrayItemSetter3).GetMethodInfo());
			default:
				return new MethodInfoCallInstruction(info, argumentCount);
			}
		}

		// Token: 0x0600460A RID: 17930 RVA: 0x00177714 File Offset: 0x00175914
		public static void ArrayItemSetter1(Array array, int index0, object value)
		{
			array.SetValue(value, index0);
		}

		// Token: 0x0600460B RID: 17931 RVA: 0x0017771E File Offset: 0x0017591E
		public static void ArrayItemSetter2(Array array, int index0, int index1, object value)
		{
			array.SetValue(value, index0, index1);
		}

		// Token: 0x0600460C RID: 17932 RVA: 0x00177729 File Offset: 0x00175929
		public static void ArrayItemSetter3(Array array, int index0, int index1, int index2, object value)
		{
			array.SetValue(value, index0, index1, index2);
		}

		// Token: 0x0600460D RID: 17933 RVA: 0x00177736 File Offset: 0x00175936
		private static bool ShouldCache(MethodInfo info)
		{
			return !(info is DynamicMethod);
		}

		// Token: 0x0600460E RID: 17934 RVA: 0x00177744 File Offset: 0x00175944
		private static Type TryGetParameterOrReturnType(MethodInfo target, ParameterInfo[] pi, int index)
		{
			if (!target.IsStatic)
			{
				index--;
				if (index < 0)
				{
					return target.DeclaringType;
				}
			}
			if (index < pi.Length)
			{
				return pi[index].ParameterType;
			}
			if (target.ReturnType == typeof(void) || index > pi.Length)
			{
				return null;
			}
			return target.ReturnType;
		}

		// Token: 0x0600460F RID: 17935 RVA: 0x0017779D File Offset: 0x0017599D
		private static bool IndexIsNotReturnType(int index, MethodInfo target, ParameterInfo[] pi)
		{
			return pi.Length != index || (pi.Length == index && !target.IsStatic);
		}

		// Token: 0x06004610 RID: 17936 RVA: 0x001777B8 File Offset: 0x001759B8
		private static CallInstruction SlowCreate(MethodInfo info, ParameterInfo[] pis)
		{
			List<Type> list = new List<Type>();
			if (!info.IsStatic)
			{
				list.Add(info.DeclaringType);
			}
			foreach (ParameterInfo parameterInfo in pis)
			{
				list.Add(parameterInfo.ParameterType);
			}
			if (info.ReturnType != typeof(void))
			{
				list.Add(info.ReturnType);
			}
			Type[] arrTypes = list.ToArray();
			return (CallInstruction)Activator.CreateInstance(CallInstruction.GetHelperType(info, arrTypes), new object[]
			{
				info
			});
		}

		// Token: 0x17000ED0 RID: 3792
		// (get) Token: 0x06004611 RID: 17937 RVA: 0x0017784E File Offset: 0x00175A4E
		public sealed override int ProducedStack
		{
			get
			{
				if (!(this.Info.ReturnType == typeof(void)))
				{
					return 1;
				}
				return 0;
			}
		}

		// Token: 0x17000ED1 RID: 3793
		// (get) Token: 0x06004612 RID: 17938 RVA: 0x0017786F File Offset: 0x00175A6F
		public sealed override int ConsumedStack
		{
			get
			{
				return this.ArgumentCount;
			}
		}

		// Token: 0x17000ED2 RID: 3794
		// (get) Token: 0x06004613 RID: 17939 RVA: 0x00177877 File Offset: 0x00175A77
		public sealed override string InstructionName
		{
			get
			{
				return "Call";
			}
		}

		// Token: 0x06004614 RID: 17940 RVA: 0x0017787E File Offset: 0x00175A7E
		public override string ToString()
		{
			return "Call(" + this.Info + ")";
		}

		// Token: 0x06004615 RID: 17941 RVA: 0x00177898 File Offset: 0x00175A98
		public virtual object InvokeInstance(object instance, params object[] args)
		{
			switch (args.Length)
			{
			case 0:
				return this.Invoke(instance);
			case 1:
				return this.Invoke(instance, args[0]);
			case 2:
				return this.Invoke(instance, args[0], args[1]);
			case 3:
				return this.Invoke(instance, args[0], args[1], args[2]);
			case 4:
				return this.Invoke(instance, args[0], args[1], args[2], args[3]);
			case 5:
				return this.Invoke(instance, args[0], args[1], args[2], args[3], args[4]);
			case 6:
				return this.Invoke(instance, args[0], args[1], args[2], args[3], args[4], args[5]);
			case 7:
				return this.Invoke(instance, args[0], args[1], args[2], args[3], args[4], args[5], args[6]);
			case 8:
				return this.Invoke(instance, args[0], args[1], args[2], args[3], args[4], args[5], args[6], args[7]);
			default:
				throw new InvalidOperationException();
			}
		}

		// Token: 0x06004616 RID: 17942 RVA: 0x00177994 File Offset: 0x00175B94
		public virtual object Invoke(params object[] args)
		{
			switch (args.Length)
			{
			case 0:
				return this.Invoke();
			case 1:
				return this.Invoke(args[0]);
			case 2:
				return this.Invoke(args[0], args[1]);
			case 3:
				return this.Invoke(args[0], args[1], args[2]);
			case 4:
				return this.Invoke(args[0], args[1], args[2], args[3]);
			case 5:
				return this.Invoke(args[0], args[1], args[2], args[3], args[4]);
			case 6:
				return this.Invoke(args[0], args[1], args[2], args[3], args[4], args[5]);
			case 7:
				return this.Invoke(args[0], args[1], args[2], args[3], args[4], args[5], args[6]);
			case 8:
				return this.Invoke(args[0], args[1], args[2], args[3], args[4], args[5], args[6], args[7]);
			case 9:
				return this.Invoke(args[0], args[1], args[2], args[3], args[4], args[5], args[6], args[7], args[8]);
			default:
				throw new InvalidOperationException();
			}
		}

		// Token: 0x06004617 RID: 17943 RVA: 0x00177AAA File Offset: 0x00175CAA
		public virtual object Invoke()
		{
			throw new InvalidOperationException();
		}

		// Token: 0x06004618 RID: 17944 RVA: 0x00177AB1 File Offset: 0x00175CB1
		public virtual object Invoke(object arg0)
		{
			throw new InvalidOperationException();
		}

		// Token: 0x06004619 RID: 17945 RVA: 0x00177AB8 File Offset: 0x00175CB8
		public virtual object Invoke(object arg0, object arg1)
		{
			throw new InvalidOperationException();
		}

		// Token: 0x0600461A RID: 17946 RVA: 0x00177ABF File Offset: 0x00175CBF
		public virtual object Invoke(object arg0, object arg1, object arg2)
		{
			throw new InvalidOperationException();
		}

		// Token: 0x0600461B RID: 17947 RVA: 0x00177AC6 File Offset: 0x00175CC6
		public virtual object Invoke(object arg0, object arg1, object arg2, object arg3)
		{
			throw new InvalidOperationException();
		}

		// Token: 0x0600461C RID: 17948 RVA: 0x00177ACD File Offset: 0x00175CCD
		public virtual object Invoke(object arg0, object arg1, object arg2, object arg3, object arg4)
		{
			throw new InvalidOperationException();
		}

		// Token: 0x0600461D RID: 17949 RVA: 0x00177AD4 File Offset: 0x00175CD4
		public virtual object Invoke(object arg0, object arg1, object arg2, object arg3, object arg4, object arg5)
		{
			throw new InvalidOperationException();
		}

		// Token: 0x0600461E RID: 17950 RVA: 0x00177ADB File Offset: 0x00175CDB
		public virtual object Invoke(object arg0, object arg1, object arg2, object arg3, object arg4, object arg5, object arg6)
		{
			throw new InvalidOperationException();
		}

		// Token: 0x0600461F RID: 17951 RVA: 0x00177AE2 File Offset: 0x00175CE2
		public virtual object Invoke(object arg0, object arg1, object arg2, object arg3, object arg4, object arg5, object arg6, object arg7)
		{
			throw new InvalidOperationException();
		}

		// Token: 0x06004620 RID: 17952 RVA: 0x00177AE9 File Offset: 0x00175CE9
		public virtual object Invoke(object arg0, object arg1, object arg2, object arg3, object arg4, object arg5, object arg6, object arg7, object arg8)
		{
			throw new InvalidOperationException();
		}

		// Token: 0x06004621 RID: 17953 RVA: 0x00177AF0 File Offset: 0x00175CF0
		private static CallInstruction FastCreate(MethodInfo target, ParameterInfo[] pi)
		{
			Type type = CallInstruction.TryGetParameterOrReturnType(target, pi, 0);
			if (type == null)
			{
				return new ActionCallInstruction(target);
			}
			TypeInfo typeInfo = type.GetTypeInfo();
			if (typeInfo.IsEnum)
			{
				return CallInstruction.SlowCreate(target, pi);
			}
			switch (type.GetTypeCode())
			{
			case TypeCode.Object:
				if (!(type != typeof(object)) || (!CallInstruction.IndexIsNotReturnType(0, target, pi) && !typeInfo.IsValueType))
				{
					return CallInstruction.FastCreate<object>(target, pi);
				}
				break;
			case TypeCode.Boolean:
				return CallInstruction.FastCreate<bool>(target, pi);
			case TypeCode.Char:
				return CallInstruction.FastCreate<char>(target, pi);
			case TypeCode.SByte:
				return CallInstruction.FastCreate<sbyte>(target, pi);
			case TypeCode.Byte:
				return CallInstruction.FastCreate<byte>(target, pi);
			case TypeCode.Int16:
				return CallInstruction.FastCreate<short>(target, pi);
			case TypeCode.UInt16:
				return CallInstruction.FastCreate<ushort>(target, pi);
			case TypeCode.Int32:
				return CallInstruction.FastCreate<int>(target, pi);
			case TypeCode.UInt32:
				return CallInstruction.FastCreate<uint>(target, pi);
			case TypeCode.Int64:
				return CallInstruction.FastCreate<long>(target, pi);
			case TypeCode.UInt64:
				return CallInstruction.FastCreate<ulong>(target, pi);
			case TypeCode.Single:
				return CallInstruction.FastCreate<float>(target, pi);
			case TypeCode.Double:
				return CallInstruction.FastCreate<double>(target, pi);
			case TypeCode.Decimal:
				return CallInstruction.FastCreate<decimal>(target, pi);
			case TypeCode.DateTime:
				return CallInstruction.FastCreate<DateTime>(target, pi);
			case TypeCode.String:
				return CallInstruction.FastCreate<string>(target, pi);
			}
			return CallInstruction.SlowCreate(target, pi);
		}

		// Token: 0x06004622 RID: 17954 RVA: 0x00177C3C File Offset: 0x00175E3C
		private static CallInstruction FastCreate<T0>(MethodInfo target, ParameterInfo[] pi)
		{
			Type type = CallInstruction.TryGetParameterOrReturnType(target, pi, 1);
			if (type == null)
			{
				if (target.ReturnType == typeof(void))
				{
					return new ActionCallInstruction<T0>(target);
				}
				return new FuncCallInstruction<T0>(target);
			}
			else
			{
				TypeInfo typeInfo = type.GetTypeInfo();
				if (typeInfo.IsEnum)
				{
					return CallInstruction.SlowCreate(target, pi);
				}
				switch (type.GetTypeCode())
				{
				case TypeCode.Object:
					if (!(type != typeof(object)) || (!CallInstruction.IndexIsNotReturnType(1, target, pi) && !typeInfo.IsValueType))
					{
						return CallInstruction.FastCreate<T0, object>(target, pi);
					}
					break;
				case TypeCode.Boolean:
					return CallInstruction.FastCreate<T0, bool>(target, pi);
				case TypeCode.Char:
					return CallInstruction.FastCreate<T0, char>(target, pi);
				case TypeCode.SByte:
					return CallInstruction.FastCreate<T0, sbyte>(target, pi);
				case TypeCode.Byte:
					return CallInstruction.FastCreate<T0, byte>(target, pi);
				case TypeCode.Int16:
					return CallInstruction.FastCreate<T0, short>(target, pi);
				case TypeCode.UInt16:
					return CallInstruction.FastCreate<T0, ushort>(target, pi);
				case TypeCode.Int32:
					return CallInstruction.FastCreate<T0, int>(target, pi);
				case TypeCode.UInt32:
					return CallInstruction.FastCreate<T0, uint>(target, pi);
				case TypeCode.Int64:
					return CallInstruction.FastCreate<T0, long>(target, pi);
				case TypeCode.UInt64:
					return CallInstruction.FastCreate<T0, ulong>(target, pi);
				case TypeCode.Single:
					return CallInstruction.FastCreate<T0, float>(target, pi);
				case TypeCode.Double:
					return CallInstruction.FastCreate<T0, double>(target, pi);
				case TypeCode.Decimal:
					return CallInstruction.FastCreate<T0, decimal>(target, pi);
				case TypeCode.DateTime:
					return CallInstruction.FastCreate<T0, DateTime>(target, pi);
				case TypeCode.String:
					return CallInstruction.FastCreate<T0, string>(target, pi);
				}
				return CallInstruction.SlowCreate(target, pi);
			}
		}

		// Token: 0x06004623 RID: 17955 RVA: 0x00177DA4 File Offset: 0x00175FA4
		private static CallInstruction FastCreate<T0, T1>(MethodInfo target, ParameterInfo[] pi)
		{
			Type type = CallInstruction.TryGetParameterOrReturnType(target, pi, 2);
			if (type == null)
			{
				if (target.ReturnType == typeof(void))
				{
					return new ActionCallInstruction<T0, T1>(target);
				}
				return new FuncCallInstruction<T0, T1>(target);
			}
			else
			{
				TypeInfo typeInfo = type.GetTypeInfo();
				if (typeInfo.IsEnum)
				{
					return CallInstruction.SlowCreate(target, pi);
				}
				switch (type.GetTypeCode())
				{
				case TypeCode.Object:
					if (!typeInfo.IsValueType)
					{
						return new FuncCallInstruction<T0, T1, object>(target);
					}
					break;
				case TypeCode.Boolean:
					return new FuncCallInstruction<T0, T1, bool>(target);
				case TypeCode.Char:
					return new FuncCallInstruction<T0, T1, char>(target);
				case TypeCode.SByte:
					return new FuncCallInstruction<T0, T1, sbyte>(target);
				case TypeCode.Byte:
					return new FuncCallInstruction<T0, T1, byte>(target);
				case TypeCode.Int16:
					return new FuncCallInstruction<T0, T1, short>(target);
				case TypeCode.UInt16:
					return new FuncCallInstruction<T0, T1, ushort>(target);
				case TypeCode.Int32:
					return new FuncCallInstruction<T0, T1, int>(target);
				case TypeCode.UInt32:
					return new FuncCallInstruction<T0, T1, uint>(target);
				case TypeCode.Int64:
					return new FuncCallInstruction<T0, T1, long>(target);
				case TypeCode.UInt64:
					return new FuncCallInstruction<T0, T1, ulong>(target);
				case TypeCode.Single:
					return new FuncCallInstruction<T0, T1, float>(target);
				case TypeCode.Double:
					return new FuncCallInstruction<T0, T1, double>(target);
				case TypeCode.Decimal:
					return new FuncCallInstruction<T0, T1, decimal>(target);
				case TypeCode.DateTime:
					return new FuncCallInstruction<T0, T1, DateTime>(target);
				case TypeCode.String:
					return new FuncCallInstruction<T0, T1, string>(target);
				}
				return CallInstruction.SlowCreate(target, pi);
			}
		}

		// Token: 0x06004624 RID: 17956 RVA: 0x00177ED8 File Offset: 0x001760D8
		private static Type GetHelperType(MethodInfo info, Type[] arrTypes)
		{
			Type result;
			if (info.ReturnType == typeof(void))
			{
				switch (arrTypes.Length)
				{
				case 0:
					result = typeof(ActionCallInstruction);
					break;
				case 1:
					result = typeof(ActionCallInstruction<>).MakeGenericType(arrTypes);
					break;
				case 2:
					result = typeof(ActionCallInstruction<, >).MakeGenericType(arrTypes);
					break;
				case 3:
					result = typeof(ActionCallInstruction<, , >).MakeGenericType(arrTypes);
					break;
				case 4:
					result = typeof(ActionCallInstruction<, , , >).MakeGenericType(arrTypes);
					break;
				case 5:
					result = typeof(ActionCallInstruction<, , , , >).MakeGenericType(arrTypes);
					break;
				case 6:
					result = typeof(ActionCallInstruction<, , , , , >).MakeGenericType(arrTypes);
					break;
				case 7:
					result = typeof(ActionCallInstruction<, , , , , , >).MakeGenericType(arrTypes);
					break;
				case 8:
					result = typeof(ActionCallInstruction<, , , , , , , >).MakeGenericType(arrTypes);
					break;
				case 9:
					result = typeof(ActionCallInstruction<, , , , , , , , >).MakeGenericType(arrTypes);
					break;
				default:
					throw new InvalidOperationException();
				}
			}
			else
			{
				switch (arrTypes.Length)
				{
				case 1:
					result = typeof(FuncCallInstruction<>).MakeGenericType(arrTypes);
					break;
				case 2:
					result = typeof(FuncCallInstruction<, >).MakeGenericType(arrTypes);
					break;
				case 3:
					result = typeof(FuncCallInstruction<, , >).MakeGenericType(arrTypes);
					break;
				case 4:
					result = typeof(FuncCallInstruction<, , , >).MakeGenericType(arrTypes);
					break;
				case 5:
					result = typeof(FuncCallInstruction<, , , , >).MakeGenericType(arrTypes);
					break;
				case 6:
					result = typeof(FuncCallInstruction<, , , , , >).MakeGenericType(arrTypes);
					break;
				case 7:
					result = typeof(FuncCallInstruction<, , , , , , >).MakeGenericType(arrTypes);
					break;
				case 8:
					result = typeof(FuncCallInstruction<, , , , , , , >).MakeGenericType(arrTypes);
					break;
				case 9:
					result = typeof(FuncCallInstruction<, , , , , , , , >).MakeGenericType(arrTypes);
					break;
				case 10:
					result = typeof(FuncCallInstruction<, , , , , , , , , >).MakeGenericType(arrTypes);
					break;
				default:
					throw new InvalidOperationException();
				}
			}
			return result;
		}

		// Token: 0x06004625 RID: 17957 RVA: 0x0017811C File Offset: 0x0017631C
		public static MethodInfo CacheFunc<TRet>(Func<TRet> method)
		{
			MethodInfo methodInfo = method.GetMethodInfo();
			lock (CallInstruction._cache)
			{
				CallInstruction._cache[methodInfo] = new FuncCallInstruction<TRet>(method);
			}
			return methodInfo;
		}

		// Token: 0x06004626 RID: 17958 RVA: 0x00178170 File Offset: 0x00176370
		public static MethodInfo CacheFunc<T0, TRet>(Func<T0, TRet> method)
		{
			MethodInfo methodInfo = method.GetMethodInfo();
			lock (CallInstruction._cache)
			{
				CallInstruction._cache[methodInfo] = new FuncCallInstruction<T0, TRet>(method);
			}
			return methodInfo;
		}

		// Token: 0x06004627 RID: 17959 RVA: 0x001781C4 File Offset: 0x001763C4
		public static MethodInfo CacheFunc<T0, T1, TRet>(Func<T0, T1, TRet> method)
		{
			MethodInfo methodInfo = method.GetMethodInfo();
			lock (CallInstruction._cache)
			{
				CallInstruction._cache[methodInfo] = new FuncCallInstruction<T0, T1, TRet>(method);
			}
			return methodInfo;
		}

		// Token: 0x06004628 RID: 17960 RVA: 0x00178218 File Offset: 0x00176418
		public static MethodInfo CacheFunc<T0, T1, T2, TRet>(Func<T0, T1, T2, TRet> method)
		{
			MethodInfo methodInfo = method.GetMethodInfo();
			lock (CallInstruction._cache)
			{
				CallInstruction._cache[methodInfo] = new FuncCallInstruction<T0, T1, T2, TRet>(method);
			}
			return methodInfo;
		}

		// Token: 0x06004629 RID: 17961 RVA: 0x0017826C File Offset: 0x0017646C
		public static MethodInfo CacheFunc<T0, T1, T2, T3, TRet>(Func<T0, T1, T2, T3, TRet> method)
		{
			MethodInfo methodInfo = method.GetMethodInfo();
			lock (CallInstruction._cache)
			{
				CallInstruction._cache[methodInfo] = new FuncCallInstruction<T0, T1, T2, T3, TRet>(method);
			}
			return methodInfo;
		}

		// Token: 0x0600462A RID: 17962 RVA: 0x001782C0 File Offset: 0x001764C0
		public static MethodInfo CacheFunc<T0, T1, T2, T3, T4, TRet>(Func<T0, T1, T2, T3, T4, TRet> method)
		{
			MethodInfo methodInfo = method.GetMethodInfo();
			lock (CallInstruction._cache)
			{
				CallInstruction._cache[methodInfo] = new FuncCallInstruction<T0, T1, T2, T3, T4, TRet>(method);
			}
			return methodInfo;
		}

		// Token: 0x0600462B RID: 17963 RVA: 0x00178314 File Offset: 0x00176514
		public static MethodInfo CacheFunc<T0, T1, T2, T3, T4, T5, TRet>(Func<T0, T1, T2, T3, T4, T5, TRet> method)
		{
			MethodInfo methodInfo = method.GetMethodInfo();
			lock (CallInstruction._cache)
			{
				CallInstruction._cache[methodInfo] = new FuncCallInstruction<T0, T1, T2, T3, T4, T5, TRet>(method);
			}
			return methodInfo;
		}

		// Token: 0x0600462C RID: 17964 RVA: 0x00178368 File Offset: 0x00176568
		public static MethodInfo CacheFunc<T0, T1, T2, T3, T4, T5, T6, TRet>(Func<T0, T1, T2, T3, T4, T5, T6, TRet> method)
		{
			MethodInfo methodInfo = method.GetMethodInfo();
			lock (CallInstruction._cache)
			{
				CallInstruction._cache[methodInfo] = new FuncCallInstruction<T0, T1, T2, T3, T4, T5, T6, TRet>(method);
			}
			return methodInfo;
		}

		// Token: 0x0600462D RID: 17965 RVA: 0x001783BC File Offset: 0x001765BC
		public static MethodInfo CacheFunc<T0, T1, T2, T3, T4, T5, T6, T7, TRet>(Func<T0, T1, T2, T3, T4, T5, T6, T7, TRet> method)
		{
			MethodInfo methodInfo = method.GetMethodInfo();
			lock (CallInstruction._cache)
			{
				CallInstruction._cache[methodInfo] = new FuncCallInstruction<T0, T1, T2, T3, T4, T5, T6, T7, TRet>(method);
			}
			return methodInfo;
		}

		// Token: 0x0600462E RID: 17966 RVA: 0x00178410 File Offset: 0x00176610
		public static MethodInfo CacheFunc<T0, T1, T2, T3, T4, T5, T6, T7, T8, TRet>(Func<T0, T1, T2, T3, T4, T5, T6, T7, T8, TRet> method)
		{
			MethodInfo methodInfo = method.GetMethodInfo();
			lock (CallInstruction._cache)
			{
				CallInstruction._cache[methodInfo] = new FuncCallInstruction<T0, T1, T2, T3, T4, T5, T6, T7, T8, TRet>(method);
			}
			return methodInfo;
		}

		// Token: 0x0600462F RID: 17967 RVA: 0x00178464 File Offset: 0x00176664
		public static MethodInfo CacheAction(Action method)
		{
			MethodInfo methodInfo = method.GetMethodInfo();
			lock (CallInstruction._cache)
			{
				CallInstruction._cache[methodInfo] = new ActionCallInstruction(method);
			}
			return methodInfo;
		}

		// Token: 0x06004630 RID: 17968 RVA: 0x001784B8 File Offset: 0x001766B8
		public static MethodInfo CacheAction<T0>(Action<T0> method)
		{
			MethodInfo methodInfo = method.GetMethodInfo();
			lock (CallInstruction._cache)
			{
				CallInstruction._cache[methodInfo] = new ActionCallInstruction<T0>(method);
			}
			return methodInfo;
		}

		// Token: 0x06004631 RID: 17969 RVA: 0x0017850C File Offset: 0x0017670C
		public static MethodInfo CacheAction<T0, T1>(Action<T0, T1> method)
		{
			MethodInfo methodInfo = method.GetMethodInfo();
			lock (CallInstruction._cache)
			{
				CallInstruction._cache[methodInfo] = new ActionCallInstruction<T0, T1>(method);
			}
			return methodInfo;
		}

		// Token: 0x06004632 RID: 17970 RVA: 0x00178560 File Offset: 0x00176760
		public static MethodInfo CacheAction<T0, T1, T2>(Action<T0, T1, T2> method)
		{
			MethodInfo methodInfo = method.GetMethodInfo();
			lock (CallInstruction._cache)
			{
				CallInstruction._cache[methodInfo] = new ActionCallInstruction<T0, T1, T2>(method);
			}
			return methodInfo;
		}

		// Token: 0x06004633 RID: 17971 RVA: 0x001785B4 File Offset: 0x001767B4
		public static MethodInfo CacheAction<T0, T1, T2, T3>(Action<T0, T1, T2, T3> method)
		{
			MethodInfo methodInfo = method.GetMethodInfo();
			lock (CallInstruction._cache)
			{
				CallInstruction._cache[methodInfo] = new ActionCallInstruction<T0, T1, T2, T3>(method);
			}
			return methodInfo;
		}

		// Token: 0x06004634 RID: 17972 RVA: 0x00178608 File Offset: 0x00176808
		public static MethodInfo CacheAction<T0, T1, T2, T3, T4>(Action<T0, T1, T2, T3, T4> method)
		{
			MethodInfo methodInfo = method.GetMethodInfo();
			lock (CallInstruction._cache)
			{
				CallInstruction._cache[methodInfo] = new ActionCallInstruction<T0, T1, T2, T3, T4>(method);
			}
			return methodInfo;
		}

		// Token: 0x06004635 RID: 17973 RVA: 0x0017865C File Offset: 0x0017685C
		public static MethodInfo CacheAction<T0, T1, T2, T3, T4, T5>(Action<T0, T1, T2, T3, T4, T5> method)
		{
			MethodInfo methodInfo = method.GetMethodInfo();
			lock (CallInstruction._cache)
			{
				CallInstruction._cache[methodInfo] = new ActionCallInstruction<T0, T1, T2, T3, T4, T5>(method);
			}
			return methodInfo;
		}

		// Token: 0x06004636 RID: 17974 RVA: 0x001786B0 File Offset: 0x001768B0
		public static MethodInfo CacheAction<T0, T1, T2, T3, T4, T5, T6>(Action<T0, T1, T2, T3, T4, T5, T6> method)
		{
			MethodInfo methodInfo = method.GetMethodInfo();
			lock (CallInstruction._cache)
			{
				CallInstruction._cache[methodInfo] = new ActionCallInstruction<T0, T1, T2, T3, T4, T5, T6>(method);
			}
			return methodInfo;
		}

		// Token: 0x06004637 RID: 17975 RVA: 0x00178704 File Offset: 0x00176904
		public static MethodInfo CacheAction<T0, T1, T2, T3, T4, T5, T6, T7>(Action<T0, T1, T2, T3, T4, T5, T6, T7> method)
		{
			MethodInfo methodInfo = method.GetMethodInfo();
			lock (CallInstruction._cache)
			{
				CallInstruction._cache[methodInfo] = new ActionCallInstruction<T0, T1, T2, T3, T4, T5, T6, T7>(method);
			}
			return methodInfo;
		}

		// Token: 0x06004638 RID: 17976 RVA: 0x00178758 File Offset: 0x00176958
		public static MethodInfo CacheAction<T0, T1, T2, T3, T4, T5, T6, T7, T8>(Action<T0, T1, T2, T3, T4, T5, T6, T7, T8> method)
		{
			MethodInfo methodInfo = method.GetMethodInfo();
			lock (CallInstruction._cache)
			{
				CallInstruction._cache[methodInfo] = new ActionCallInstruction<T0, T1, T2, T3, T4, T5, T6, T7, T8>(method);
			}
			return methodInfo;
		}

		// Token: 0x040022A3 RID: 8867
		private const int MaxHelpers = 10;

		// Token: 0x040022A4 RID: 8868
		private const int MaxArgs = 3;

		// Token: 0x040022A5 RID: 8869
		private static readonly Dictionary<MethodInfo, CallInstruction> _cache = new Dictionary<MethodInfo, CallInstruction>();
	}
}
