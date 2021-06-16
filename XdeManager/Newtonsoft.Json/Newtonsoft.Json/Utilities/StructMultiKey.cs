using System;

namespace Newtonsoft.Json.Utilities
{
	// Token: 0x02000066 RID: 102
	internal readonly struct StructMultiKey<T1, T2> : IEquatable<StructMultiKey<T1, T2>>
	{
		// Token: 0x060005DE RID: 1502 RVA: 0x00019831 File Offset: 0x00017A31
		public StructMultiKey(T1 v1, T2 v2)
		{
			this.Value1 = v1;
			this.Value2 = v2;
		}

		// Token: 0x060005DF RID: 1503 RVA: 0x00019844 File Offset: 0x00017A44
		public override int GetHashCode()
		{
			T1 value = this.Value1;
			ref T1 ptr = ref value;
			T1 t = default(T1);
			int num;
			if (t == null)
			{
				t = value;
				ptr = ref t;
				if (t == null)
				{
					num = 0;
					goto IL_38;
				}
			}
			num = ptr.GetHashCode();
			IL_38:
			T2 value2 = this.Value2;
			ref T2 ptr2 = ref value2;
			T2 t2 = default(T2);
			int num2;
			if (t2 == null)
			{
				t2 = value2;
				ptr2 = ref t2;
				if (t2 == null)
				{
					num2 = 0;
					goto IL_70;
				}
			}
			num2 = ptr2.GetHashCode();
			IL_70:
			return num ^ num2;
		}

		// Token: 0x060005E0 RID: 1504 RVA: 0x000198C4 File Offset: 0x00017AC4
		public override bool Equals(object obj)
		{
			if (obj is StructMultiKey<T1, T2>)
			{
				StructMultiKey<T1, T2> other = (StructMultiKey<T1, T2>)obj;
				return this.Equals(other);
			}
			return false;
		}

		// Token: 0x060005E1 RID: 1505 RVA: 0x000198ED File Offset: 0x00017AED
		public bool Equals(StructMultiKey<T1, T2> other)
		{
			return object.Equals(this.Value1, other.Value1) && object.Equals(this.Value2, other.Value2);
		}

		// Token: 0x04000206 RID: 518
		public readonly T1 Value1;

		// Token: 0x04000207 RID: 519
		public readonly T2 Value2;
	}
}
