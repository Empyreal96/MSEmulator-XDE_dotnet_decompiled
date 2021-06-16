using System;
using System.Globalization;

namespace System.Management.Automation
{
	// Token: 0x02000958 RID: 2392
	public sealed class FormatViewDefinition
	{
		// Token: 0x170011C8 RID: 4552
		// (get) Token: 0x06005802 RID: 22530 RVA: 0x001CA595 File Offset: 0x001C8795
		public string Name
		{
			get
			{
				return this._name;
			}
		}

		// Token: 0x170011C9 RID: 4553
		// (get) Token: 0x06005803 RID: 22531 RVA: 0x001CA59D File Offset: 0x001C879D
		public PSControl Control
		{
			get
			{
				return this._control;
			}
		}

		// Token: 0x170011CA RID: 4554
		// (get) Token: 0x06005804 RID: 22532 RVA: 0x001CA5A5 File Offset: 0x001C87A5
		internal Guid InstanceId
		{
			get
			{
				return this._instanceId;
			}
		}

		// Token: 0x06005805 RID: 22533 RVA: 0x001CA5B0 File Offset: 0x001C87B0
		public override string ToString()
		{
			return string.Format(CultureInfo.InvariantCulture, "{0} , {1}", new object[]
			{
				this._name,
				this._control.ToString()
			});
		}

		// Token: 0x06005806 RID: 22534 RVA: 0x001CA5EB File Offset: 0x001C87EB
		internal FormatViewDefinition(string name, PSControl control, Guid instanceid)
		{
			this._name = name;
			this._control = control;
			this._instanceId = instanceid;
		}

		// Token: 0x06005807 RID: 22535 RVA: 0x001CA608 File Offset: 0x001C8808
		public FormatViewDefinition(string name, PSControl control)
		{
			if (string.IsNullOrEmpty(name))
			{
				throw PSTraceSource.NewArgumentNullException("name");
			}
			if (control == null)
			{
				throw PSTraceSource.NewArgumentNullException("control");
			}
			this._name = name;
			this._control = control;
		}

		// Token: 0x04002F23 RID: 12067
		private string _name;

		// Token: 0x04002F24 RID: 12068
		private PSControl _control;

		// Token: 0x04002F25 RID: 12069
		private Guid _instanceId;
	}
}
