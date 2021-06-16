using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Threading;
using Newtonsoft.Json.Serialization;

namespace Newtonsoft.Json.Utilities
{
	// Token: 0x02000053 RID: 83
	internal static class FSharpUtils
	{
		// Token: 0x170000BC RID: 188
		// (get) Token: 0x06000519 RID: 1305 RVA: 0x0001661B File Offset: 0x0001481B
		// (set) Token: 0x0600051A RID: 1306 RVA: 0x00016622 File Offset: 0x00014822
		public static Assembly FSharpCoreAssembly { get; private set; }

		// Token: 0x170000BD RID: 189
		// (get) Token: 0x0600051B RID: 1307 RVA: 0x0001662A File Offset: 0x0001482A
		// (set) Token: 0x0600051C RID: 1308 RVA: 0x00016631 File Offset: 0x00014831
		public static MethodCall<object, object> IsUnion { get; private set; }

		// Token: 0x170000BE RID: 190
		// (get) Token: 0x0600051D RID: 1309 RVA: 0x00016639 File Offset: 0x00014839
		// (set) Token: 0x0600051E RID: 1310 RVA: 0x00016640 File Offset: 0x00014840
		public static MethodCall<object, object> GetUnionCases { get; private set; }

		// Token: 0x170000BF RID: 191
		// (get) Token: 0x0600051F RID: 1311 RVA: 0x00016648 File Offset: 0x00014848
		// (set) Token: 0x06000520 RID: 1312 RVA: 0x0001664F File Offset: 0x0001484F
		public static MethodCall<object, object> PreComputeUnionTagReader { get; private set; }

		// Token: 0x170000C0 RID: 192
		// (get) Token: 0x06000521 RID: 1313 RVA: 0x00016657 File Offset: 0x00014857
		// (set) Token: 0x06000522 RID: 1314 RVA: 0x0001665E File Offset: 0x0001485E
		public static MethodCall<object, object> PreComputeUnionReader { get; private set; }

		// Token: 0x170000C1 RID: 193
		// (get) Token: 0x06000523 RID: 1315 RVA: 0x00016666 File Offset: 0x00014866
		// (set) Token: 0x06000524 RID: 1316 RVA: 0x0001666D File Offset: 0x0001486D
		public static MethodCall<object, object> PreComputeUnionConstructor { get; private set; }

		// Token: 0x170000C2 RID: 194
		// (get) Token: 0x06000525 RID: 1317 RVA: 0x00016675 File Offset: 0x00014875
		// (set) Token: 0x06000526 RID: 1318 RVA: 0x0001667C File Offset: 0x0001487C
		public static Func<object, object> GetUnionCaseInfoDeclaringType { get; private set; }

		// Token: 0x170000C3 RID: 195
		// (get) Token: 0x06000527 RID: 1319 RVA: 0x00016684 File Offset: 0x00014884
		// (set) Token: 0x06000528 RID: 1320 RVA: 0x0001668B File Offset: 0x0001488B
		public static Func<object, object> GetUnionCaseInfoName { get; private set; }

		// Token: 0x170000C4 RID: 196
		// (get) Token: 0x06000529 RID: 1321 RVA: 0x00016693 File Offset: 0x00014893
		// (set) Token: 0x0600052A RID: 1322 RVA: 0x0001669A File Offset: 0x0001489A
		public static Func<object, object> GetUnionCaseInfoTag { get; private set; }

		// Token: 0x170000C5 RID: 197
		// (get) Token: 0x0600052B RID: 1323 RVA: 0x000166A2 File Offset: 0x000148A2
		// (set) Token: 0x0600052C RID: 1324 RVA: 0x000166A9 File Offset: 0x000148A9
		public static MethodCall<object, object> GetUnionCaseInfoFields { get; private set; }

		// Token: 0x0600052D RID: 1325 RVA: 0x000166B4 File Offset: 0x000148B4
		public static void EnsureInitialized(Assembly fsharpCoreAssembly)
		{
			if (!FSharpUtils._initialized)
			{
				object @lock = FSharpUtils.Lock;
				lock (@lock)
				{
					if (!FSharpUtils._initialized)
					{
						FSharpUtils.FSharpCoreAssembly = fsharpCoreAssembly;
						Type type = fsharpCoreAssembly.GetType("Microsoft.FSharp.Reflection.FSharpType");
						MethodInfo methodWithNonPublicFallback = FSharpUtils.GetMethodWithNonPublicFallback(type, "IsUnion", BindingFlags.Static | BindingFlags.Public);
						FSharpUtils.IsUnion = JsonTypeReflector.ReflectionDelegateFactory.CreateMethodCall<object>(methodWithNonPublicFallback);
						MethodInfo methodWithNonPublicFallback2 = FSharpUtils.GetMethodWithNonPublicFallback(type, "GetUnionCases", BindingFlags.Static | BindingFlags.Public);
						FSharpUtils.GetUnionCases = JsonTypeReflector.ReflectionDelegateFactory.CreateMethodCall<object>(methodWithNonPublicFallback2);
						Type type2 = fsharpCoreAssembly.GetType("Microsoft.FSharp.Reflection.FSharpValue");
						FSharpUtils.PreComputeUnionTagReader = FSharpUtils.CreateFSharpFuncCall(type2, "PreComputeUnionTagReader");
						FSharpUtils.PreComputeUnionReader = FSharpUtils.CreateFSharpFuncCall(type2, "PreComputeUnionReader");
						FSharpUtils.PreComputeUnionConstructor = FSharpUtils.CreateFSharpFuncCall(type2, "PreComputeUnionConstructor");
						Type type3 = fsharpCoreAssembly.GetType("Microsoft.FSharp.Reflection.UnionCaseInfo");
						FSharpUtils.GetUnionCaseInfoName = JsonTypeReflector.ReflectionDelegateFactory.CreateGet<object>(type3.GetProperty("Name"));
						FSharpUtils.GetUnionCaseInfoTag = JsonTypeReflector.ReflectionDelegateFactory.CreateGet<object>(type3.GetProperty("Tag"));
						FSharpUtils.GetUnionCaseInfoDeclaringType = JsonTypeReflector.ReflectionDelegateFactory.CreateGet<object>(type3.GetProperty("DeclaringType"));
						FSharpUtils.GetUnionCaseInfoFields = JsonTypeReflector.ReflectionDelegateFactory.CreateMethodCall<object>(type3.GetMethod("GetFields"));
						FSharpUtils._ofSeq = fsharpCoreAssembly.GetType("Microsoft.FSharp.Collections.ListModule").GetMethod("OfSeq");
						FSharpUtils._mapType = fsharpCoreAssembly.GetType("Microsoft.FSharp.Collections.FSharpMap`2");
						Thread.MemoryBarrier();
						FSharpUtils._initialized = true;
					}
				}
			}
		}

		// Token: 0x0600052E RID: 1326 RVA: 0x00016844 File Offset: 0x00014A44
		private static MethodInfo GetMethodWithNonPublicFallback(Type type, string methodName, BindingFlags bindingFlags)
		{
			MethodInfo method = type.GetMethod(methodName, bindingFlags);
			if (method == null && (bindingFlags & BindingFlags.NonPublic) != BindingFlags.NonPublic)
			{
				method = type.GetMethod(methodName, bindingFlags | BindingFlags.NonPublic);
			}
			return method;
		}

		// Token: 0x0600052F RID: 1327 RVA: 0x00016878 File Offset: 0x00014A78
		private static MethodCall<object, object> CreateFSharpFuncCall(Type type, string methodName)
		{
			MethodInfo methodWithNonPublicFallback = FSharpUtils.GetMethodWithNonPublicFallback(type, methodName, BindingFlags.Static | BindingFlags.Public);
			MethodInfo method = methodWithNonPublicFallback.ReturnType.GetMethod("Invoke", BindingFlags.Instance | BindingFlags.Public);
			MethodCall<object, object> call = JsonTypeReflector.ReflectionDelegateFactory.CreateMethodCall<object>(methodWithNonPublicFallback);
			MethodCall<object, object> invoke = JsonTypeReflector.ReflectionDelegateFactory.CreateMethodCall<object>(method);
			return (object target, object[] args) => new FSharpFunction(call(target, args), invoke);
		}

		// Token: 0x06000530 RID: 1328 RVA: 0x000168D4 File Offset: 0x00014AD4
		public static ObjectConstructor<object> CreateSeq(Type t)
		{
			MethodInfo method = FSharpUtils._ofSeq.MakeGenericMethod(new Type[]
			{
				t
			});
			return JsonTypeReflector.ReflectionDelegateFactory.CreateParameterizedConstructor(method);
		}

		// Token: 0x06000531 RID: 1329 RVA: 0x00016901 File Offset: 0x00014B01
		public static ObjectConstructor<object> CreateMap(Type keyType, Type valueType)
		{
			return (ObjectConstructor<object>)typeof(FSharpUtils).GetMethod("BuildMapCreator").MakeGenericMethod(new Type[]
			{
				keyType,
				valueType
			}).Invoke(null, null);
		}

		// Token: 0x06000532 RID: 1330 RVA: 0x00016938 File Offset: 0x00014B38
		public static ObjectConstructor<object> BuildMapCreator<TKey, TValue>()
		{
			ConstructorInfo constructor = FSharpUtils._mapType.MakeGenericType(new Type[]
			{
				typeof(TKey),
				typeof(TValue)
			}).GetConstructor(new Type[]
			{
				typeof(IEnumerable<Tuple<TKey, TValue>>)
			});
			ObjectConstructor<object> ctorDelegate = JsonTypeReflector.ReflectionDelegateFactory.CreateParameterizedConstructor(constructor);
			return delegate(object[] args)
			{
				IEnumerable<Tuple<TKey, TValue>> enumerable = from kv in (IEnumerable<KeyValuePair<TKey, TValue>>)args[0]
				select new Tuple<TKey, TValue>(kv.Key, kv.Value);
				return ctorDelegate(new object[]
				{
					enumerable
				});
			};
		}

		// Token: 0x040001C8 RID: 456
		private static readonly object Lock = new object();

		// Token: 0x040001C9 RID: 457
		private static bool _initialized;

		// Token: 0x040001CA RID: 458
		private static MethodInfo _ofSeq;

		// Token: 0x040001CB RID: 459
		private static Type _mapType;

		// Token: 0x040001D6 RID: 470
		public const string FSharpSetTypeName = "FSharpSet`1";

		// Token: 0x040001D7 RID: 471
		public const string FSharpListTypeName = "FSharpList`1";

		// Token: 0x040001D8 RID: 472
		public const string FSharpMapTypeName = "FSharpMap`2";
	}
}
