using System;
using System.Management.Automation.Provider;
using System.Xml;

namespace System.Management.Automation
{
	// Token: 0x020001AB RID: 427
	internal class ProviderContext
	{
		// Token: 0x170004AB RID: 1195
		// (get) Token: 0x060013F5 RID: 5109 RVA: 0x0007A0B2 File Offset: 0x000782B2
		internal string RequestedPath
		{
			get
			{
				return this._requestedPath;
			}
		}

		// Token: 0x060013F6 RID: 5110 RVA: 0x0007A0BA File Offset: 0x000782BA
		internal ProviderContext(string requestedPath, ExecutionContext executionContext, PathIntrinsics pathIntrinsics)
		{
			this._requestedPath = requestedPath;
			this._executionContext = executionContext;
			this._pathIntrinsics = pathIntrinsics;
		}

		// Token: 0x060013F7 RID: 5111 RVA: 0x0007A0D8 File Offset: 0x000782D8
		internal MamlCommandHelpInfo GetProviderSpecificHelpInfo(string helpItemName)
		{
			ProviderInfo providerInfo = null;
			PSDriveInfo psdriveInfo = null;
			string text = null;
			CmdletProviderContext cmdletProviderContext = new CmdletProviderContext(this._executionContext);
			try
			{
				string path = this._requestedPath;
				if (string.IsNullOrEmpty(this._requestedPath))
				{
					path = this._pathIntrinsics.CurrentLocation.Path;
				}
				text = this._executionContext.LocationGlobber.GetProviderPath(path, cmdletProviderContext, out providerInfo, out psdriveInfo);
			}
			catch (ArgumentNullException)
			{
			}
			catch (ProviderNotFoundException)
			{
			}
			catch (DriveNotFoundException)
			{
			}
			catch (ProviderInvocationException)
			{
			}
			catch (NotSupportedException)
			{
			}
			catch (InvalidOperationException)
			{
			}
			catch (ItemNotFoundException)
			{
			}
			if (providerInfo == null)
			{
				return null;
			}
			CmdletProvider cmdletProvider = providerInfo.CreateInstance();
			ICmdletProviderSupportsHelp cmdletProviderSupportsHelp = cmdletProvider as ICmdletProviderSupportsHelp;
			if (cmdletProviderSupportsHelp == null)
			{
				return null;
			}
			if (text == null)
			{
				throw new ItemNotFoundException(this._requestedPath, "PathNotFound", SessionStateStrings.PathNotFound);
			}
			cmdletProvider.Start(providerInfo, cmdletProviderContext);
			string path2 = text;
			string helpMaml = cmdletProviderSupportsHelp.GetHelpMaml(helpItemName, path2);
			if (string.IsNullOrEmpty(helpMaml))
			{
				return null;
			}
			XmlDocument xmlDocument = InternalDeserializer.LoadUnsafeXmlDocument(helpMaml, false, null);
			return MamlCommandHelpInfo.Load(xmlDocument.DocumentElement, HelpCategory.Provider);
		}

		// Token: 0x04000893 RID: 2195
		private readonly string _requestedPath;

		// Token: 0x04000894 RID: 2196
		private readonly ExecutionContext _executionContext;

		// Token: 0x04000895 RID: 2197
		private readonly PathIntrinsics _pathIntrinsics;
	}
}
