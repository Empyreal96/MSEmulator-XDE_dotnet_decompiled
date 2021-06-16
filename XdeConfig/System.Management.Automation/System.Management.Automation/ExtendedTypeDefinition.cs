using System;
using System.Collections.Generic;
using System.Globalization;

namespace System.Management.Automation
{
	// Token: 0x02000959 RID: 2393
	public sealed class ExtendedTypeDefinition
	{
		// Token: 0x170011CB RID: 4555
		// (get) Token: 0x06005808 RID: 22536 RVA: 0x001CA63F File Offset: 0x001C883F
		public string TypeName
		{
			get
			{
				return this._typename;
			}
		}

		// Token: 0x170011CC RID: 4556
		// (get) Token: 0x06005809 RID: 22537 RVA: 0x001CA647 File Offset: 0x001C8847
		public List<FormatViewDefinition> FormatViewDefinition
		{
			get
			{
				return this._viewdefinitions;
			}
		}

		// Token: 0x0600580A RID: 22538 RVA: 0x001CA650 File Offset: 0x001C8850
		public override string ToString()
		{
			return string.Format(CultureInfo.InvariantCulture, "{0}", new object[]
			{
				this._typename
			});
		}

		// Token: 0x0600580B RID: 22539 RVA: 0x001CA680 File Offset: 0x001C8880
		public ExtendedTypeDefinition(string typeName, IEnumerable<FormatViewDefinition> viewDefinitions)
		{
			if (string.IsNullOrEmpty(typeName))
			{
				throw PSTraceSource.NewArgumentNullException("typeName");
			}
			if (viewDefinitions == null)
			{
				throw PSTraceSource.NewArgumentNullException("viewDefinitions");
			}
			this._typename = typeName;
			foreach (FormatViewDefinition item in viewDefinitions)
			{
				this._viewdefinitions.Add(item);
			}
		}

		// Token: 0x0600580C RID: 22540 RVA: 0x001CA708 File Offset: 0x001C8908
		public ExtendedTypeDefinition(string typeName)
		{
			if (string.IsNullOrEmpty(typeName))
			{
				throw PSTraceSource.NewArgumentNullException("typeName");
			}
			this._typename = typeName;
		}

		// Token: 0x04002F26 RID: 12070
		private List<FormatViewDefinition> _viewdefinitions = new List<FormatViewDefinition>();

		// Token: 0x04002F27 RID: 12071
		private string _typename;
	}
}
