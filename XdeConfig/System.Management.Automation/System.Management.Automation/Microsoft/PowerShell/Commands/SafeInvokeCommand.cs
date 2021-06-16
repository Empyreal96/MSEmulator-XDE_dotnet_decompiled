using System;
using System.Collections;
using System.Collections.ObjectModel;
using System.Management.Automation;

namespace Microsoft.PowerShell.Commands
{
	// Token: 0x02000770 RID: 1904
	internal static class SafeInvokeCommand
	{
		// Token: 0x06004C4A RID: 19530 RVA: 0x001943F8 File Offset: 0x001925F8
		public static Hashtable Invoke(PowerShell ps, FileSystemProvider fileSystemContext, CmdletProviderContext cmdletContext)
		{
			return SafeInvokeCommand.Invoke(ps, fileSystemContext, cmdletContext, true);
		}

		// Token: 0x06004C4B RID: 19531 RVA: 0x00194404 File Offset: 0x00192604
		public static Hashtable Invoke(PowerShell ps, FileSystemProvider fileSystemContext, CmdletProviderContext cmdletContext, bool shouldHaveOutput)
		{
			bool flag = cmdletContext == null;
			Collection<Hashtable> collection = new Collection<Hashtable>();
			try
			{
				collection = ps.Invoke<Hashtable>();
			}
			catch (Exception exception)
			{
				if (flag)
				{
					fileSystemContext.WriteError(new ErrorRecord(exception, "CopyFileRemoteExecutionError", ErrorCategory.InvalidOperation, ps));
					ps.Commands.Clear();
				}
				else
				{
					cmdletContext.WriteError(new ErrorRecord(exception, "CopyFileRemoteExecutionError", ErrorCategory.InvalidOperation, ps));
					ps.Commands.Clear();
				}
				return null;
			}
			if (ps.HadErrors)
			{
				foreach (ErrorRecord errorRecord in ps.Streams.Error)
				{
					if (flag)
					{
						fileSystemContext.WriteError(errorRecord);
					}
					else
					{
						cmdletContext.WriteError(errorRecord);
					}
				}
			}
			ps.Commands.Clear();
			if (!shouldHaveOutput)
			{
				return null;
			}
			if (collection.Count != 1 || collection[0].GetType() != typeof(Hashtable))
			{
				return null;
			}
			return collection[0];
		}
	}
}
