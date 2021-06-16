using System;
using System.Collections.Generic;
using System.Runtime.InteropServices;

namespace System.Management.Automation.ComInterop
{
	// Token: 0x02000A5C RID: 2652
	internal class ComEventSinksContainer : List<ComEventSink>, IDisposable
	{
		// Token: 0x060069E2 RID: 27106 RVA: 0x00214ED1 File Offset: 0x002130D1
		private ComEventSinksContainer()
		{
		}

		// Token: 0x060069E3 RID: 27107 RVA: 0x00214EDC File Offset: 0x002130DC
		public static ComEventSinksContainer FromRuntimeCallableWrapper(object rcw, bool createIfNotFound)
		{
			object comObjectData = Marshal.GetComObjectData(rcw, ComEventSinksContainer._ComObjectEventSinksKey);
			if (comObjectData != null || !createIfNotFound)
			{
				return (ComEventSinksContainer)comObjectData;
			}
			ComEventSinksContainer result;
			lock (ComEventSinksContainer._ComObjectEventSinksKey)
			{
				comObjectData = Marshal.GetComObjectData(rcw, ComEventSinksContainer._ComObjectEventSinksKey);
				if (comObjectData != null)
				{
					result = (ComEventSinksContainer)comObjectData;
				}
				else
				{
					ComEventSinksContainer comEventSinksContainer = new ComEventSinksContainer();
					if (!Marshal.SetComObjectData(rcw, ComEventSinksContainer._ComObjectEventSinksKey, comEventSinksContainer))
					{
						throw Error.SetComObjectDataFailed();
					}
					result = comEventSinksContainer;
				}
			}
			return result;
		}

		// Token: 0x060069E4 RID: 27108 RVA: 0x00214F68 File Offset: 0x00213168
		public void Dispose()
		{
			this.DisposeAll();
			GC.SuppressFinalize(this);
		}

		// Token: 0x060069E5 RID: 27109 RVA: 0x00214F78 File Offset: 0x00213178
		private void DisposeAll()
		{
			foreach (ComEventSink comEventSink in this)
			{
				comEventSink.Dispose();
			}
		}

		// Token: 0x060069E6 RID: 27110 RVA: 0x00214FC8 File Offset: 0x002131C8
		~ComEventSinksContainer()
		{
			this.DisposeAll();
		}

		// Token: 0x040032AD RID: 12973
		private static readonly object _ComObjectEventSinksKey = new object();
	}
}
