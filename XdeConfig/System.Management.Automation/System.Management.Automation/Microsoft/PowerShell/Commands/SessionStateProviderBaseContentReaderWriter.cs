using System;
using System.Collections;
using System.IO;
using System.Management.Automation;
using System.Management.Automation.Provider;

namespace Microsoft.PowerShell.Commands
{
	// Token: 0x0200078D RID: 1933
	public class SessionStateProviderBaseContentReaderWriter : IContentReader, IContentWriter, IDisposable
	{
		// Token: 0x06004CAD RID: 19629 RVA: 0x00195516 File Offset: 0x00193716
		internal SessionStateProviderBaseContentReaderWriter(string path, SessionStateProviderBase provider)
		{
			if (string.IsNullOrEmpty(path))
			{
				throw PSTraceSource.NewArgumentException("path");
			}
			if (provider == null)
			{
				throw PSTraceSource.NewArgumentNullException("provider");
			}
			this.path = path;
			this.provider = provider;
		}

		// Token: 0x06004CAE RID: 19630 RVA: 0x00195550 File Offset: 0x00193750
		public IList Read(long readCount)
		{
			IList list = null;
			if (!this.contentRead)
			{
				object sessionStateItem = this.provider.GetSessionStateItem(this.path);
				if (sessionStateItem != null)
				{
					object valueOfItem = this.provider.GetValueOfItem(sessionStateItem);
					if (valueOfItem != null)
					{
						list = (valueOfItem as IList);
						if (list == null)
						{
							list = new object[]
							{
								valueOfItem
							};
						}
					}
					this.contentRead = true;
				}
			}
			return list;
		}

		// Token: 0x06004CAF RID: 19631 RVA: 0x001955AC File Offset: 0x001937AC
		public IList Write(IList content)
		{
			if (content == null)
			{
				throw PSTraceSource.NewArgumentNullException("content");
			}
			object value = content;
			if (content.Count == 1)
			{
				value = content[0];
			}
			this.provider.SetSessionStateItem(this.path, value, false);
			return content;
		}

		// Token: 0x06004CB0 RID: 19632 RVA: 0x001955EE File Offset: 0x001937EE
		public void Seek(long offset, SeekOrigin origin)
		{
			throw PSTraceSource.NewNotSupportedException(SessionStateStrings.IContent_Seek_NotSupported, new object[0]);
		}

		// Token: 0x06004CB1 RID: 19633 RVA: 0x00195600 File Offset: 0x00193800
		public void Close()
		{
		}

		// Token: 0x06004CB2 RID: 19634 RVA: 0x00195602 File Offset: 0x00193802
		public void Dispose()
		{
			this.Close();
			GC.SuppressFinalize(this);
		}

		// Token: 0x0400253A RID: 9530
		private string path;

		// Token: 0x0400253B RID: 9531
		private SessionStateProviderBase provider;

		// Token: 0x0400253C RID: 9532
		private bool contentRead;
	}
}
