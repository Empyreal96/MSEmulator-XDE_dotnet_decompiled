using System;
using System.Collections.Generic;
using System.Globalization;
using System.Management.Automation;

namespace Microsoft.PowerShell.Commands.Internal.Format
{
	// Token: 0x0200091A RID: 2330
	internal class XmlLoaderLogger : IDisposable
	{
		// Token: 0x06005750 RID: 22352 RVA: 0x001C84B8 File Offset: 0x001C66B8
		internal void LogEntry(XmlLoaderLoggerEntry entry)
		{
			if (entry.entryType == XmlLoaderLoggerEntry.EntryType.Error)
			{
				this.hasErrors = true;
			}
			if (this.saveInMemory)
			{
				this.entries.Add(entry);
			}
			if ((XmlLoaderLogger.formatFileLoadingtracer.Options | PSTraceSourceOptions.WriteLine) != PSTraceSourceOptions.None)
			{
				this.WriteToTracer(entry);
			}
		}

		// Token: 0x06005751 RID: 22353 RVA: 0x001C84F8 File Offset: 0x001C66F8
		private void WriteToTracer(XmlLoaderLoggerEntry entry)
		{
			if (entry.entryType == XmlLoaderLoggerEntry.EntryType.Error)
			{
				XmlLoaderLogger.formatFileLoadingtracer.WriteLine(string.Format(CultureInfo.InvariantCulture, "ERROR:\r\n FilePath: {0}\r\n XPath: {1}\r\n Message = {2}", new object[]
				{
					entry.filePath,
					entry.xPath,
					entry.message
				}), new object[0]);
				return;
			}
			if (entry.entryType == XmlLoaderLoggerEntry.EntryType.Trace)
			{
				XmlLoaderLogger.formatFileLoadingtracer.WriteLine(string.Format(CultureInfo.InvariantCulture, "TRACE:\r\n FilePath: {0}\r\n XPath: {1}\r\n Message = {2}", new object[]
				{
					entry.filePath,
					entry.xPath,
					entry.message
				}), new object[0]);
			}
		}

		// Token: 0x06005752 RID: 22354 RVA: 0x001C859B File Offset: 0x001C679B
		public void Dispose()
		{
			this.Dispose(true);
			GC.SuppressFinalize(this);
		}

		// Token: 0x06005753 RID: 22355 RVA: 0x001C85AA File Offset: 0x001C67AA
		protected virtual void Dispose(bool disposing)
		{
		}

		// Token: 0x170011AF RID: 4527
		// (get) Token: 0x06005754 RID: 22356 RVA: 0x001C85AE File Offset: 0x001C67AE
		internal List<XmlLoaderLoggerEntry> LogEntries
		{
			get
			{
				return this.entries;
			}
		}

		// Token: 0x170011B0 RID: 4528
		// (get) Token: 0x06005755 RID: 22357 RVA: 0x001C85B6 File Offset: 0x001C67B6
		internal bool HasErrors
		{
			get
			{
				return this.hasErrors;
			}
		}

		// Token: 0x04002E84 RID: 11908
		[TraceSource("FormatFileLoading", "Loading format files")]
		private static PSTraceSource formatFileLoadingtracer = PSTraceSource.GetTracer("FormatFileLoading", "Loading format files", false);

		// Token: 0x04002E85 RID: 11909
		private bool saveInMemory = true;

		// Token: 0x04002E86 RID: 11910
		private List<XmlLoaderLoggerEntry> entries = new List<XmlLoaderLoggerEntry>();

		// Token: 0x04002E87 RID: 11911
		private bool hasErrors;
	}
}
