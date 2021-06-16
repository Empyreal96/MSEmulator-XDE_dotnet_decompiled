using System;
using System.Collections.Generic;
using System.Diagnostics;

namespace System.Management.Automation.Interpreter
{
	// Token: 0x02000751 RID: 1873
	internal static class ExceptionHelpers
	{
		// Token: 0x06004AE2 RID: 19170 RVA: 0x001888AC File Offset: 0x00186AAC
		public static Exception UpdateForRethrow(Exception rethrow)
		{
			StackTrace item = new StackTrace(rethrow, true);
			List<StackTrace> list;
			if (!ExceptionHelpers.TryGetAssociatedStackTraces(rethrow, out list))
			{
				list = new List<StackTrace>();
				ExceptionHelpers.AssociateStackTraces(rethrow, list);
			}
			list.Add(item);
			return rethrow;
		}

		// Token: 0x06004AE3 RID: 19171 RVA: 0x001888E0 File Offset: 0x00186AE0
		public static IList<StackTrace> GetExceptionStackTraces(Exception rethrow)
		{
			List<StackTrace> result;
			if (!ExceptionHelpers.TryGetAssociatedStackTraces(rethrow, out result))
			{
				return null;
			}
			return result;
		}

		// Token: 0x06004AE4 RID: 19172 RVA: 0x001888FA File Offset: 0x00186AFA
		private static void AssociateStackTraces(Exception e, List<StackTrace> traces)
		{
			e.Data["PreviousStackTraces"] = traces;
		}

		// Token: 0x06004AE5 RID: 19173 RVA: 0x0018890D File Offset: 0x00186B0D
		private static bool TryGetAssociatedStackTraces(Exception e, out List<StackTrace> traces)
		{
			traces = (e.Data["PreviousStackTraces"] as List<StackTrace>);
			return traces != null;
		}

		// Token: 0x04002431 RID: 9265
		private const string prevStackTraces = "PreviousStackTraces";
	}
}
