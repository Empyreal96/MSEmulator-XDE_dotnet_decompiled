using System;
using System.Collections.ObjectModel;
using System.Management.Automation.Internal;
using System.Xml;

namespace System.Management.Automation.Runspaces
{
	// Token: 0x0200015A RID: 346
	internal class LoadContext
	{
		// Token: 0x060011B6 RID: 4534 RVA: 0x00063525 File Offset: 0x00061725
		internal LoadContext(string PSSnapinName, string fileName, Collection<string> errors)
		{
			this.reader = null;
			this.fileName = fileName;
			this.errors = errors;
			this.PSSnapinName = PSSnapinName;
			this.isFullyTrusted = false;
		}

		// Token: 0x17000456 RID: 1110
		// (get) Token: 0x060011B7 RID: 4535 RVA: 0x00063550 File Offset: 0x00061750
		// (set) Token: 0x060011B8 RID: 4536 RVA: 0x00063558 File Offset: 0x00061758
		internal bool IsFullyTrusted
		{
			get
			{
				return this.isFullyTrusted;
			}
			set
			{
				this.isFullyTrusted = value;
			}
		}

		// Token: 0x060011B9 RID: 4537 RVA: 0x00063564 File Offset: 0x00061764
		internal void AddError(string resourceString, params object[] formatArguments)
		{
			string text = StringUtil.Format(resourceString, formatArguments);
			string item = StringUtil.Format(TypesXmlStrings.FileError, new object[]
			{
				this.PSSnapinName,
				this.fileName,
				text
			});
			this.errors.Add(item);
		}

		// Token: 0x060011BA RID: 4538 RVA: 0x000635B0 File Offset: 0x000617B0
		internal void AddError(int errorLineNumber, string resourceString, params object[] formatArguments)
		{
			string text = StringUtil.Format(resourceString, formatArguments);
			string item = StringUtil.Format(TypesXmlStrings.FileLineError, new object[]
			{
				this.PSSnapinName,
				this.fileName,
				errorLineNumber,
				text
			});
			this.errors.Add(item);
		}

		// Token: 0x060011BB RID: 4539 RVA: 0x00063604 File Offset: 0x00061804
		internal void AddError(string typeName, int errorLineNumber, string resourceString, params object[] formatArguments)
		{
			string text = StringUtil.Format(resourceString, formatArguments);
			string item = StringUtil.Format(TypesXmlStrings.FileLineTypeError, new object[]
			{
				this.PSSnapinName,
				this.fileName,
				errorLineNumber,
				typeName,
				text
			});
			this.errors.Add(item);
		}

		// Token: 0x04000799 RID: 1945
		internal XmlReader reader;

		// Token: 0x0400079A RID: 1946
		internal Collection<string> errors;

		// Token: 0x0400079B RID: 1947
		internal string fileName;

		// Token: 0x0400079C RID: 1948
		internal string PSSnapinName;

		// Token: 0x0400079D RID: 1949
		internal bool isFullyTrusted;
	}
}
