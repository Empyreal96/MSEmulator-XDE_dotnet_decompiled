using System;
using System.IO;

namespace System.Management.Automation.Remoting.Server
{
	// Token: 0x020002FB RID: 763
	internal sealed class HyperVSocketErrorTextWriter : OutOfProcessTextWriter
	{
		// Token: 0x1700087B RID: 2171
		// (get) Token: 0x060023FE RID: 9214 RVA: 0x000C9F6C File Offset: 0x000C816C
		internal static string ErrorPrepend
		{
			get
			{
				return "__HyperVSocketError__:";
			}
		}

		// Token: 0x060023FF RID: 9215 RVA: 0x000C9F73 File Offset: 0x000C8173
		internal HyperVSocketErrorTextWriter(TextWriter textWriter) : base(textWriter)
		{
		}

		// Token: 0x06002400 RID: 9216 RVA: 0x000C9F7C File Offset: 0x000C817C
		internal override void WriteLine(string data)
		{
			string data2 = (data != null) ? ("__HyperVSocketError__:" + data) : data;
			base.WriteLine(data2);
		}

		// Token: 0x040011B9 RID: 4537
		private const string _errorPrepend = "__HyperVSocketError__:";
	}
}
