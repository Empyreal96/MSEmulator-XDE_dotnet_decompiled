using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.PowerShell;

namespace System.Management.Automation
{
	// Token: 0x02000140 RID: 320
	internal class PSMethodInvocationConstraints
	{
		// Token: 0x060010C3 RID: 4291 RVA: 0x0005DDE6 File Offset: 0x0005BFE6
		internal PSMethodInvocationConstraints(Type methodTargetType, Type[] parameterTypes)
		{
			this.MethodTargetType = methodTargetType;
			this.parameterTypes = parameterTypes;
		}

		// Token: 0x1700041B RID: 1051
		// (get) Token: 0x060010C4 RID: 4292 RVA: 0x0005DDFC File Offset: 0x0005BFFC
		// (set) Token: 0x060010C5 RID: 4293 RVA: 0x0005DE04 File Offset: 0x0005C004
		public Type MethodTargetType { get; private set; }

		// Token: 0x1700041C RID: 1052
		// (get) Token: 0x060010C6 RID: 4294 RVA: 0x0005DE0D File Offset: 0x0005C00D
		public IEnumerable<Type> ParameterTypes
		{
			get
			{
				return this.parameterTypes;
			}
		}

		// Token: 0x060010C7 RID: 4295 RVA: 0x0005DE15 File Offset: 0x0005C015
		internal static bool EqualsForCollection<T>(ICollection<T> xs, ICollection<T> ys)
		{
			if (xs == null)
			{
				return ys == null;
			}
			return ys != null && xs.Count == ys.Count && xs.SequenceEqual(ys);
		}

		// Token: 0x060010C8 RID: 4296 RVA: 0x0005DE3C File Offset: 0x0005C03C
		public bool Equals(PSMethodInvocationConstraints other)
		{
			return !object.ReferenceEquals(null, other) && (object.ReferenceEquals(this, other) || (!(other.MethodTargetType != this.MethodTargetType) && PSMethodInvocationConstraints.EqualsForCollection<Type>(this.parameterTypes, other.parameterTypes)));
		}

		// Token: 0x060010C9 RID: 4297 RVA: 0x0005DE8A File Offset: 0x0005C08A
		public override bool Equals(object obj)
		{
			return !object.ReferenceEquals(null, obj) && (object.ReferenceEquals(this, obj) || (!(obj.GetType() != typeof(PSMethodInvocationConstraints)) && this.Equals((PSMethodInvocationConstraints)obj)));
		}

		// Token: 0x060010CA RID: 4298 RVA: 0x0005DEC8 File Offset: 0x0005C0C8
		public override int GetHashCode()
		{
			int num = 61;
			num = num * 397 + ((this.MethodTargetType != null) ? this.MethodTargetType.GetHashCode() : 0);
			return num * 397 + this.ParameterTypes.SequenceGetHashCode<Type>();
		}

		// Token: 0x060010CB RID: 4299 RVA: 0x0005DF14 File Offset: 0x0005C114
		public override string ToString()
		{
			StringBuilder stringBuilder = new StringBuilder();
			string value = "";
			if (this.MethodTargetType != null)
			{
				stringBuilder.Append("this: ");
				stringBuilder.Append(ToStringCodeMethods.Type(this.MethodTargetType, true));
				value = " ";
			}
			if (this.parameterTypes != null)
			{
				stringBuilder.Append(value);
				stringBuilder.Append("args: ");
				value = "";
				foreach (Type type in this.parameterTypes)
				{
					stringBuilder.Append(value);
					stringBuilder.Append(ToStringCodeMethods.Type(type, true));
					value = ", ";
				}
			}
			if (stringBuilder.Length == 0)
			{
				stringBuilder.Append("<empty>");
			}
			return stringBuilder.ToString();
		}

		// Token: 0x04000740 RID: 1856
		private readonly Type[] parameterTypes;
	}
}
