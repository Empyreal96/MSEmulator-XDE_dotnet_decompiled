using System;
using System.IO;

namespace System.Management.Automation.Remoting.Server
{
	// Token: 0x020002F9 RID: 761
	internal sealed class NamedPipeErrorTextWriter : OutOfProcessTextWriter
	{
		// Token: 0x17000879 RID: 2169
		// (get) Token: 0x060023F8 RID: 9208 RVA: 0x000C9E61 File Offset: 0x000C8061
		internal static string ErrorPrepend
		{
			get
			{
				return "__NamedPipeError__:";
			}
		}

		// Token: 0x060023F9 RID: 9209 RVA: 0x000C9E68 File Offset: 0x000C8068
		internal NamedPipeErrorTextWriter(TextWriter textWriter) : base(textWriter)
		{
		}

		// Token: 0x060023FA RID: 9210 RVA: 0x000C9E74 File Offset: 0x000C8074
		internal override void WriteLine(string data)
		{
			string data2 = (data != null) ? ("__NamedPipeError__:" + data) : data;
			base.WriteLine(data2);
		}

		// Token: 0x040011B6 RID: 4534
		private const string _errorPrepend = "__NamedPipeError__:";
	}
}
