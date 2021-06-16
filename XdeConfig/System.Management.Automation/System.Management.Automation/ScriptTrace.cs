using System;
using System.Management.Automation.Internal;
using System.Management.Automation.Internal.Host;
using System.Management.Automation.Runspaces;

namespace System.Management.Automation
{
	// Token: 0x0200048D RID: 1165
	internal static class ScriptTrace
	{
		// Token: 0x0600338C RID: 13196 RVA: 0x00119A4C File Offset: 0x00117C4C
		internal static void Trace(int level, string messageId, string resourceString, params object[] args)
		{
			ExecutionContext executionContextFromTLS = LocalPipeline.GetExecutionContextFromTLS();
			if (executionContextFromTLS == null)
			{
				return;
			}
			ScriptTrace.Trace(executionContextFromTLS, level, messageId, resourceString, args);
		}

		// Token: 0x0600338D RID: 13197 RVA: 0x00119A70 File Offset: 0x00117C70
		internal static void Trace(ExecutionContext context, int level, string messageId, string resourceString, params object[] args)
		{
			ActionPreference actionPreference = ActionPreference.Continue;
			if (context.PSDebugTraceLevel > level)
			{
				string text;
				if (args == null || args.Length == 0)
				{
					text = resourceString;
				}
				else
				{
					text = StringUtil.Format(resourceString, args);
				}
				if (string.IsNullOrEmpty(text))
				{
					text = "Could not load text for msh script tracing message id '" + messageId + "'";
				}
				((InternalHostUserInterface)context.EngineHostInterface.UI).WriteDebugLine(text, ref actionPreference);
			}
		}
	}
}
