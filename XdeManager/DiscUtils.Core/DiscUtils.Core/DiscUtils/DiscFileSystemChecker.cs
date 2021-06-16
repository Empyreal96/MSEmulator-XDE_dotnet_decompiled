using System;
using System.IO;

namespace DiscUtils
{
	// Token: 0x0200000A RID: 10
	public abstract class DiscFileSystemChecker
	{
		// Token: 0x06000078 RID: 120
		public abstract bool Check(TextWriter reportOutput, ReportLevels levels);
	}
}
