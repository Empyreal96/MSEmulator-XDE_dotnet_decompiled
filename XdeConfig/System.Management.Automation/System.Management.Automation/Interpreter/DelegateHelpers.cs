using System;
using System.Linq;

namespace System.Management.Automation.Interpreter
{
	// Token: 0x0200074E RID: 1870
	internal static class DelegateHelpers
	{
		// Token: 0x06004AD5 RID: 19157 RVA: 0x0018837C File Offset: 0x0018657C
		internal static Type MakeDelegate(Type[] types)
		{
			if (types.Length <= 17)
			{
				if (!types.Any((Type t) => t.IsByRef))
				{
					Type left = types[types.Length - 1];
					if (left == typeof(void))
					{
						Array.Resize<Type>(ref types, types.Length - 1);
						switch (types.Length)
						{
						case 0:
							return typeof(Action);
						case 1:
							return typeof(Action<>).MakeGenericType(types);
						case 2:
							return typeof(Action<, >).MakeGenericType(types);
						case 3:
							return typeof(Action<, , >).MakeGenericType(types);
						case 4:
							return typeof(Action<, , , >).MakeGenericType(types);
						case 5:
							return typeof(Action<, , , , >).MakeGenericType(types);
						case 6:
							return typeof(Action<, , , , , >).MakeGenericType(types);
						case 7:
							return typeof(Action<, , , , , , >).MakeGenericType(types);
						case 8:
							return typeof(Action<, , , , , , , >).MakeGenericType(types);
						case 9:
							return typeof(Action<, , , , , , , , >).MakeGenericType(types);
						case 10:
							return typeof(Action<, , , , , , , , , >).MakeGenericType(types);
						case 11:
							return typeof(Action<, , , , , , , , , , >).MakeGenericType(types);
						case 12:
							return typeof(Action<, , , , , , , , , , , >).MakeGenericType(types);
						case 13:
							return typeof(Action<, , , , , , , , , , , , >).MakeGenericType(types);
						case 14:
							return typeof(Action<, , , , , , , , , , , , , >).MakeGenericType(types);
						case 15:
							return typeof(Action<, , , , , , , , , , , , , , >).MakeGenericType(types);
						case 16:
							return typeof(Action<, , , , , , , , , , , , , , , >).MakeGenericType(types);
						}
					}
					else
					{
						switch (types.Length)
						{
						case 1:
							return typeof(Func<>).MakeGenericType(types);
						case 2:
							return typeof(Func<, >).MakeGenericType(types);
						case 3:
							return typeof(Func<, , >).MakeGenericType(types);
						case 4:
							return typeof(Func<, , , >).MakeGenericType(types);
						case 5:
							return typeof(Func<, , , , >).MakeGenericType(types);
						case 6:
							return typeof(Func<, , , , , >).MakeGenericType(types);
						case 7:
							return typeof(Func<, , , , , , >).MakeGenericType(types);
						case 8:
							return typeof(Func<, , , , , , , >).MakeGenericType(types);
						case 9:
							return typeof(Func<, , , , , , , , >).MakeGenericType(types);
						case 10:
							return typeof(Func<, , , , , , , , , >).MakeGenericType(types);
						case 11:
							return typeof(Func<, , , , , , , , , , >).MakeGenericType(types);
						case 12:
							return typeof(Func<, , , , , , , , , , , >).MakeGenericType(types);
						case 13:
							return typeof(Func<, , , , , , , , , , , , >).MakeGenericType(types);
						case 14:
							return typeof(Func<, , , , , , , , , , , , , >).MakeGenericType(types);
						case 15:
							return typeof(Func<, , , , , , , , , , , , , , >).MakeGenericType(types);
						case 16:
							return typeof(Func<, , , , , , , , , , , , , , , >).MakeGenericType(types);
						case 17:
							return typeof(Func<, , , , , , , , , , , , , , , , >).MakeGenericType(types);
						}
					}
					throw Assert.Unreachable;
				}
			}
			throw Assert.Unreachable;
		}

		// Token: 0x04002427 RID: 9255
		private const int MaximumArity = 17;
	}
}
