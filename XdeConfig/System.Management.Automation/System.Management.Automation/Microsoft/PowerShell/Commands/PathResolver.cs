using System;
using System.Collections.ObjectModel;
using System.Management.Automation;

namespace Microsoft.PowerShell.Commands
{
	// Token: 0x0200033E RID: 830
	internal class PathResolver
	{
		// Token: 0x06002845 RID: 10309 RVA: 0x000E028C File Offset: 0x000DE48C
		internal string ResolveProviderAndPath(string path, bool isLiteralPath, PSCmdlet cmdlet, bool allowNonexistingPaths, string resourceString)
		{
			PathInfo pathInfo = this.ResolvePath(path, isLiteralPath, allowNonexistingPaths, cmdlet);
			if (pathInfo.Provider.ImplementingType == typeof(FileSystemProvider))
			{
				return pathInfo.ProviderPath;
			}
			throw PSTraceSource.NewInvalidOperationException(resourceString, new object[]
			{
				pathInfo.Provider.Name
			});
		}

		// Token: 0x06002846 RID: 10310 RVA: 0x000E02E8 File Offset: 0x000DE4E8
		private PathInfo ResolvePath(string pathToResolve, bool isLiteralPath, bool allowNonexistingPaths, PSCmdlet cmdlet)
		{
			CmdletProviderContext cmdletProviderContext = new CmdletProviderContext(cmdlet);
			cmdletProviderContext.SuppressWildcardExpansion = isLiteralPath;
			Collection<PathInfo> collection = new Collection<PathInfo>();
			try
			{
				Collection<PathInfo> resolvedPSPathFromPSPath = cmdlet.SessionState.Path.GetResolvedPSPathFromPSPath(pathToResolve, cmdletProviderContext);
				foreach (PathInfo item in resolvedPSPathFromPSPath)
				{
					collection.Add(item);
				}
			}
			catch (PSNotSupportedException ex)
			{
				cmdlet.ThrowTerminatingError(new ErrorRecord(ex.ErrorRecord, ex));
			}
			catch (DriveNotFoundException ex2)
			{
				cmdlet.ThrowTerminatingError(new ErrorRecord(ex2.ErrorRecord, ex2));
			}
			catch (ProviderNotFoundException ex3)
			{
				cmdlet.ThrowTerminatingError(new ErrorRecord(ex3.ErrorRecord, ex3));
			}
			catch (ItemNotFoundException ex4)
			{
				if (allowNonexistingPaths)
				{
					ProviderInfo provider = null;
					PSDriveInfo drive = null;
					string unresolvedProviderPathFromPSPath = cmdlet.SessionState.Path.GetUnresolvedProviderPathFromPSPath(pathToResolve, cmdletProviderContext, out provider, out drive);
					PathInfo item2 = new PathInfo(drive, provider, unresolvedProviderPathFromPSPath, cmdlet.SessionState);
					collection.Add(item2);
				}
				else
				{
					cmdlet.ThrowTerminatingError(new ErrorRecord(ex4.ErrorRecord, ex4));
				}
			}
			if (collection.Count == 1)
			{
				return collection[0];
			}
			Exception exception = PSTraceSource.NewNotSupportedException();
			cmdlet.ThrowTerminatingError(new ErrorRecord(exception, "NotSupported", ErrorCategory.NotImplemented, collection));
			return null;
		}
	}
}
