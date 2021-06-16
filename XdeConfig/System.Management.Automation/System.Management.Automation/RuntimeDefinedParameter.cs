using System;
using System.Collections.ObjectModel;

namespace System.Management.Automation
{
	// Token: 0x0200007F RID: 127
	public class RuntimeDefinedParameter
	{
		// Token: 0x06000691 RID: 1681 RVA: 0x0001FE29 File Offset: 0x0001E029
		public RuntimeDefinedParameter()
		{
		}

		// Token: 0x06000692 RID: 1682 RVA: 0x0001FE48 File Offset: 0x0001E048
		public RuntimeDefinedParameter(string name, Type parameterType, Collection<Attribute> attributes)
		{
			if (string.IsNullOrEmpty(name))
			{
				throw PSTraceSource.NewArgumentException("name");
			}
			if (parameterType == null)
			{
				throw PSTraceSource.NewArgumentNullException("parameterType");
			}
			this._name = name;
			this._parameterType = parameterType;
			if (attributes != null)
			{
				this._attributes = attributes;
			}
		}

		// Token: 0x170001CF RID: 463
		// (get) Token: 0x06000693 RID: 1683 RVA: 0x0001FEB0 File Offset: 0x0001E0B0
		// (set) Token: 0x06000694 RID: 1684 RVA: 0x0001FEB8 File Offset: 0x0001E0B8
		public string Name
		{
			get
			{
				return this._name;
			}
			set
			{
				if (string.IsNullOrEmpty(value))
				{
					throw PSTraceSource.NewArgumentException("name");
				}
				this._name = value;
			}
		}

		// Token: 0x170001D0 RID: 464
		// (get) Token: 0x06000695 RID: 1685 RVA: 0x0001FED4 File Offset: 0x0001E0D4
		// (set) Token: 0x06000696 RID: 1686 RVA: 0x0001FEDC File Offset: 0x0001E0DC
		public Type ParameterType
		{
			get
			{
				return this._parameterType;
			}
			set
			{
				if (value == null)
				{
					throw PSTraceSource.NewArgumentNullException("value");
				}
				this._parameterType = value;
			}
		}

		// Token: 0x170001D1 RID: 465
		// (get) Token: 0x06000697 RID: 1687 RVA: 0x0001FEF9 File Offset: 0x0001E0F9
		// (set) Token: 0x06000698 RID: 1688 RVA: 0x0001FF01 File Offset: 0x0001E101
		public object Value
		{
			get
			{
				return this._value;
			}
			set
			{
				this.IsSet = true;
				this._value = value;
			}
		}

		// Token: 0x170001D2 RID: 466
		// (get) Token: 0x06000699 RID: 1689 RVA: 0x0001FF11 File Offset: 0x0001E111
		// (set) Token: 0x0600069A RID: 1690 RVA: 0x0001FF19 File Offset: 0x0001E119
		public bool IsSet { get; set; }

		// Token: 0x170001D3 RID: 467
		// (get) Token: 0x0600069B RID: 1691 RVA: 0x0001FF22 File Offset: 0x0001E122
		public Collection<Attribute> Attributes
		{
			get
			{
				return this._attributes;
			}
		}

		// Token: 0x040002B5 RID: 693
		private string _name = string.Empty;

		// Token: 0x040002B6 RID: 694
		private Type _parameterType;

		// Token: 0x040002B7 RID: 695
		private object _value;

		// Token: 0x040002B8 RID: 696
		private readonly Collection<Attribute> _attributes = new Collection<Attribute>();
	}
}
