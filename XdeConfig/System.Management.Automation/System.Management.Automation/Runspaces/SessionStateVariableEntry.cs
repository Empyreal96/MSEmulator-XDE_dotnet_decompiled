using System;
using System.Collections.ObjectModel;

namespace System.Management.Automation.Runspaces
{
	// Token: 0x02000832 RID: 2098
	public sealed class SessionStateVariableEntry : ConstrainedSessionStateEntry
	{
		// Token: 0x06005044 RID: 20548 RVA: 0x001A8342 File Offset: 0x001A6542
		public SessionStateVariableEntry(string name, object value, string description) : base(name, SessionStateEntryVisibility.Public)
		{
			this._value = value;
			this._description = description;
		}

		// Token: 0x06005045 RID: 20549 RVA: 0x001A8365 File Offset: 0x001A6565
		public SessionStateVariableEntry(string name, object value, string description, ScopedItemOptions options) : base(name, SessionStateEntryVisibility.Public)
		{
			this._value = value;
			this._description = description;
			this._options = options;
		}

		// Token: 0x06005046 RID: 20550 RVA: 0x001A8390 File Offset: 0x001A6590
		public SessionStateVariableEntry(string name, object value, string description, ScopedItemOptions options, Collection<Attribute> attributes) : base(name, SessionStateEntryVisibility.Public)
		{
			this._value = value;
			this._description = description;
			this._options = options;
			this._attributes = attributes;
		}

		// Token: 0x06005047 RID: 20551 RVA: 0x001A83C4 File Offset: 0x001A65C4
		public SessionStateVariableEntry(string name, object value, string description, ScopedItemOptions options, Attribute attribute) : base(name, SessionStateEntryVisibility.Public)
		{
			this._value = value;
			this._description = description;
			this._options = options;
			this._attributes = new Collection<Attribute>();
			this._attributes.Add(attribute);
		}

		// Token: 0x06005048 RID: 20552 RVA: 0x001A8412 File Offset: 0x001A6612
		internal SessionStateVariableEntry(string name, object value, string description, ScopedItemOptions options, Collection<Attribute> attributes, SessionStateEntryVisibility visibility) : base(name, visibility)
		{
			this._value = value;
			this._description = description;
			this._options = options;
			this._attributes = new Collection<Attribute>();
			this._attributes = attributes;
		}

		// Token: 0x06005049 RID: 20553 RVA: 0x001A8454 File Offset: 0x001A6654
		public override InitialSessionStateEntry Clone()
		{
			Collection<Attribute> attributes = null;
			if (this._attributes != null && this._attributes.Count > 0)
			{
				attributes = new Collection<Attribute>(this._attributes);
			}
			return new SessionStateVariableEntry(base.Name, this._value, this._description, this._options, attributes, base.Visibility);
		}

		// Token: 0x17001064 RID: 4196
		// (get) Token: 0x0600504A RID: 20554 RVA: 0x001A84A9 File Offset: 0x001A66A9
		public object Value
		{
			get
			{
				return this._value;
			}
		}

		// Token: 0x17001065 RID: 4197
		// (get) Token: 0x0600504B RID: 20555 RVA: 0x001A84B1 File Offset: 0x001A66B1
		public string Description
		{
			get
			{
				return this._description;
			}
		}

		// Token: 0x17001066 RID: 4198
		// (get) Token: 0x0600504C RID: 20556 RVA: 0x001A84B9 File Offset: 0x001A66B9
		public ScopedItemOptions Options
		{
			get
			{
				return this._options;
			}
		}

		// Token: 0x17001067 RID: 4199
		// (get) Token: 0x0600504D RID: 20557 RVA: 0x001A84C1 File Offset: 0x001A66C1
		public Collection<Attribute> Attributes
		{
			get
			{
				if (this._attributes == null)
				{
					this._attributes = new Collection<Attribute>();
				}
				return this._attributes;
			}
		}

		// Token: 0x04002906 RID: 10502
		private object _value;

		// Token: 0x04002907 RID: 10503
		private string _description = string.Empty;

		// Token: 0x04002908 RID: 10504
		private ScopedItemOptions _options;

		// Token: 0x04002909 RID: 10505
		private Collection<Attribute> _attributes;
	}
}
