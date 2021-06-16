using System;
using System.Collections.ObjectModel;

namespace System.Management.Automation
{
	// Token: 0x02000144 RID: 324
	public class PSMethod : PSMethodInfo
	{
		// Token: 0x060010E6 RID: 4326 RVA: 0x0005E5E7 File Offset: 0x0005C7E7
		internal override void ReplicateInstance(object particularInstance)
		{
			base.ReplicateInstance(particularInstance);
			this.baseObject = particularInstance;
		}

		// Token: 0x060010E7 RID: 4327 RVA: 0x0005E5F7 File Offset: 0x0005C7F7
		public override string ToString()
		{
			return this.adapter.BaseMethodToString(this);
		}

		// Token: 0x060010E8 RID: 4328 RVA: 0x0005E605 File Offset: 0x0005C805
		internal PSMethod(string name, Adapter adapter, object baseObject, object adapterData)
		{
			if (string.IsNullOrEmpty(name))
			{
				throw PSTraceSource.NewArgumentException("name");
			}
			this.name = name;
			this.adapterData = adapterData;
			this.adapter = adapter;
			this.baseObject = baseObject;
		}

		// Token: 0x060010E9 RID: 4329 RVA: 0x0005E63D File Offset: 0x0005C83D
		internal PSMethod(string name, Adapter adapter, object baseObject, object adapterData, bool isSpecial, bool isHidden) : this(name, adapter, baseObject, adapterData)
		{
			this.IsSpecial = isSpecial;
			base.IsHidden = isHidden;
		}

		// Token: 0x060010EA RID: 4330 RVA: 0x0005E65C File Offset: 0x0005C85C
		public override PSMemberInfo Copy()
		{
			PSMethod psmethod = new PSMethod(this.name, this.adapter, this.baseObject, this.adapterData, this.IsSpecial, base.IsHidden);
			base.CloneBaseProperties(psmethod);
			return psmethod;
		}

		// Token: 0x17000427 RID: 1063
		// (get) Token: 0x060010EB RID: 4331 RVA: 0x0005E69B File Offset: 0x0005C89B
		public override PSMemberTypes MemberType
		{
			get
			{
				return PSMemberTypes.Method;
			}
		}

		// Token: 0x060010EC RID: 4332 RVA: 0x0005E69F File Offset: 0x0005C89F
		public override object Invoke(params object[] arguments)
		{
			return this.Invoke(null, arguments);
		}

		// Token: 0x060010ED RID: 4333 RVA: 0x0005E6A9 File Offset: 0x0005C8A9
		internal object Invoke(PSMethodInvocationConstraints invocationConstraints, params object[] arguments)
		{
			if (arguments == null)
			{
				throw PSTraceSource.NewArgumentNullException("arguments");
			}
			return this.adapter.BaseMethodInvoke(this, invocationConstraints, arguments);
		}

		// Token: 0x17000428 RID: 1064
		// (get) Token: 0x060010EE RID: 4334 RVA: 0x0005E6C7 File Offset: 0x0005C8C7
		public override Collection<string> OverloadDefinitions
		{
			get
			{
				return this.adapter.BaseMethodDefinitions(this);
			}
		}

		// Token: 0x17000429 RID: 1065
		// (get) Token: 0x060010EF RID: 4335 RVA: 0x0005E6D5 File Offset: 0x0005C8D5
		public override string TypeNameOfValue
		{
			get
			{
				return typeof(PSMethod).FullName;
			}
		}

		// Token: 0x1700042A RID: 1066
		// (get) Token: 0x060010F0 RID: 4336 RVA: 0x0005E6E6 File Offset: 0x0005C8E6
		// (set) Token: 0x060010F1 RID: 4337 RVA: 0x0005E6EE File Offset: 0x0005C8EE
		internal bool IsSpecial { get; private set; }

		// Token: 0x04000747 RID: 1863
		internal object adapterData;

		// Token: 0x04000748 RID: 1864
		private Adapter adapter;

		// Token: 0x04000749 RID: 1865
		internal object baseObject;
	}
}
