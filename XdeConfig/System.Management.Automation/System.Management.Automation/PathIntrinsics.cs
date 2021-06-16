using System;
using System.Collections.ObjectModel;
using System.Management.Automation.Provider;

namespace System.Management.Automation
{
	// Token: 0x02000031 RID: 49
	public sealed class PathIntrinsics
	{
		// Token: 0x06000251 RID: 593 RVA: 0x000093A4 File Offset: 0x000075A4
		private PathIntrinsics()
		{
		}

		// Token: 0x06000252 RID: 594 RVA: 0x000093AC File Offset: 0x000075AC
		internal PathIntrinsics(SessionStateInternal sessionState)
		{
			if (sessionState == null)
			{
				throw PSTraceSource.NewArgumentNullException("sessionState");
			}
			this.sessionState = sessionState;
		}

		// Token: 0x17000098 RID: 152
		// (get) Token: 0x06000253 RID: 595 RVA: 0x000093C9 File Offset: 0x000075C9
		public PathInfo CurrentLocation
		{
			get
			{
				return this.sessionState.CurrentLocation;
			}
		}

		// Token: 0x06000254 RID: 596 RVA: 0x000093D6 File Offset: 0x000075D6
		public PathInfo CurrentProviderLocation(string providerName)
		{
			return this.sessionState.GetNamespaceCurrentLocation(providerName);
		}

		// Token: 0x17000099 RID: 153
		// (get) Token: 0x06000255 RID: 597 RVA: 0x000093E4 File Offset: 0x000075E4
		public PathInfo CurrentFileSystemLocation
		{
			get
			{
				return this.CurrentProviderLocation(this.sessionState.ExecutionContext.ProviderNames.FileSystem);
			}
		}

		// Token: 0x06000256 RID: 598 RVA: 0x00009401 File Offset: 0x00007601
		public PathInfo SetLocation(string path)
		{
			return this.sessionState.SetLocation(path);
		}

		// Token: 0x06000257 RID: 599 RVA: 0x0000940F File Offset: 0x0000760F
		internal PathInfo SetLocation(string path, CmdletProviderContext context)
		{
			return this.sessionState.SetLocation(path, context);
		}

		// Token: 0x06000258 RID: 600 RVA: 0x0000941E File Offset: 0x0000761E
		internal bool IsCurrentLocationOrAncestor(string path, CmdletProviderContext context)
		{
			return this.sessionState.IsCurrentLocationOrAncestor(path, context);
		}

		// Token: 0x06000259 RID: 601 RVA: 0x0000942D File Offset: 0x0000762D
		public void PushCurrentLocation(string stackName)
		{
			this.sessionState.PushCurrentLocation(stackName);
		}

		// Token: 0x0600025A RID: 602 RVA: 0x0000943B File Offset: 0x0000763B
		public PathInfo PopLocation(string stackName)
		{
			return this.sessionState.PopLocation(stackName);
		}

		// Token: 0x0600025B RID: 603 RVA: 0x00009449 File Offset: 0x00007649
		public PathInfoStack LocationStack(string stackName)
		{
			return this.sessionState.LocationStack(stackName);
		}

		// Token: 0x0600025C RID: 604 RVA: 0x00009457 File Offset: 0x00007657
		public PathInfoStack SetDefaultLocationStack(string stackName)
		{
			return this.sessionState.SetDefaultLocationStack(stackName);
		}

		// Token: 0x0600025D RID: 605 RVA: 0x00009468 File Offset: 0x00007668
		public Collection<PathInfo> GetResolvedPSPathFromPSPath(string path)
		{
			CmdletProvider cmdletProvider = null;
			return this.PathResolver.GetGlobbedMonadPathsFromMonadPath(path, false, out cmdletProvider);
		}

		// Token: 0x0600025E RID: 606 RVA: 0x00009488 File Offset: 0x00007688
		internal Collection<PathInfo> GetResolvedPSPathFromPSPath(string path, CmdletProviderContext context)
		{
			CmdletProvider cmdletProvider = null;
			return this.PathResolver.GetGlobbedMonadPathsFromMonadPath(path, false, context, out cmdletProvider);
		}

		// Token: 0x0600025F RID: 607 RVA: 0x000094A8 File Offset: 0x000076A8
		public Collection<string> GetResolvedProviderPathFromPSPath(string path, out ProviderInfo provider)
		{
			CmdletProvider cmdletProvider = null;
			return this.PathResolver.GetGlobbedProviderPathsFromMonadPath(path, false, out provider, out cmdletProvider);
		}

		// Token: 0x06000260 RID: 608 RVA: 0x000094C8 File Offset: 0x000076C8
		internal Collection<string> GetResolvedProviderPathFromPSPath(string path, bool allowNonexistingPaths, out ProviderInfo provider)
		{
			CmdletProvider cmdletProvider = null;
			return this.PathResolver.GetGlobbedProviderPathsFromMonadPath(path, allowNonexistingPaths, out provider, out cmdletProvider);
		}

		// Token: 0x06000261 RID: 609 RVA: 0x000094E8 File Offset: 0x000076E8
		internal Collection<string> GetResolvedProviderPathFromPSPath(string path, CmdletProviderContext context, out ProviderInfo provider)
		{
			CmdletProvider cmdletProvider = null;
			return this.PathResolver.GetGlobbedProviderPathsFromMonadPath(path, false, context, out provider, out cmdletProvider);
		}

		// Token: 0x06000262 RID: 610 RVA: 0x00009508 File Offset: 0x00007708
		public Collection<string> GetResolvedProviderPathFromProviderPath(string path, string providerId)
		{
			CmdletProvider cmdletProvider = null;
			return this.PathResolver.GetGlobbedProviderPathsFromProviderPath(path, false, providerId, out cmdletProvider);
		}

		// Token: 0x06000263 RID: 611 RVA: 0x00009528 File Offset: 0x00007728
		internal Collection<string> GetResolvedProviderPathFromProviderPath(string path, string providerId, CmdletProviderContext context)
		{
			CmdletProvider cmdletProvider = null;
			return this.PathResolver.GetGlobbedProviderPathsFromProviderPath(path, false, providerId, context, out cmdletProvider);
		}

		// Token: 0x06000264 RID: 612 RVA: 0x00009548 File Offset: 0x00007748
		public string GetUnresolvedProviderPathFromPSPath(string path)
		{
			return this.PathResolver.GetProviderPath(path);
		}

		// Token: 0x06000265 RID: 613 RVA: 0x00009558 File Offset: 0x00007758
		public string GetUnresolvedProviderPathFromPSPath(string path, out ProviderInfo provider, out PSDriveInfo drive)
		{
			CmdletProviderContext cmdletProviderContext = new CmdletProviderContext(this.sessionState.ExecutionContext);
			string providerPath = this.PathResolver.GetProviderPath(path, cmdletProviderContext, out provider, out drive);
			cmdletProviderContext.ThrowFirstErrorOrDoNothing();
			return providerPath;
		}

		// Token: 0x06000266 RID: 614 RVA: 0x0000958D File Offset: 0x0000778D
		internal string GetUnresolvedProviderPathFromPSPath(string path, CmdletProviderContext context, out ProviderInfo provider, out PSDriveInfo drive)
		{
			return this.PathResolver.GetProviderPath(path, context, out provider, out drive);
		}

		// Token: 0x06000267 RID: 615 RVA: 0x0000959F File Offset: 0x0000779F
		public bool IsProviderQualified(string path)
		{
			return LocationGlobber.IsProviderQualifiedPath(path);
		}

		// Token: 0x06000268 RID: 616 RVA: 0x000095A7 File Offset: 0x000077A7
		public bool IsPSAbsolute(string path, out string driveName)
		{
			return this.PathResolver.IsAbsolutePath(path, out driveName);
		}

		// Token: 0x06000269 RID: 617 RVA: 0x000095B6 File Offset: 0x000077B6
		public string Combine(string parent, string child)
		{
			return this.sessionState.MakePath(parent, child);
		}

		// Token: 0x0600026A RID: 618 RVA: 0x000095C5 File Offset: 0x000077C5
		internal string Combine(string parent, string child, CmdletProviderContext context)
		{
			return this.sessionState.MakePath(parent, child, context);
		}

		// Token: 0x0600026B RID: 619 RVA: 0x000095D5 File Offset: 0x000077D5
		public string ParseParent(string path, string root)
		{
			return this.sessionState.GetParentPath(path, root);
		}

		// Token: 0x0600026C RID: 620 RVA: 0x000095E4 File Offset: 0x000077E4
		internal string ParseParent(string path, string root, CmdletProviderContext context)
		{
			return this.sessionState.GetParentPath(path, root, context, false);
		}

		// Token: 0x0600026D RID: 621 RVA: 0x000095F5 File Offset: 0x000077F5
		internal string ParseParent(string path, string root, CmdletProviderContext context, bool useDefaultProvider)
		{
			return this.sessionState.GetParentPath(path, root, context, useDefaultProvider);
		}

		// Token: 0x0600026E RID: 622 RVA: 0x00009607 File Offset: 0x00007807
		public string ParseChildName(string path)
		{
			return this.sessionState.GetChildName(path);
		}

		// Token: 0x0600026F RID: 623 RVA: 0x00009615 File Offset: 0x00007815
		internal string ParseChildName(string path, CmdletProviderContext context)
		{
			return this.sessionState.GetChildName(path, context, false);
		}

		// Token: 0x06000270 RID: 624 RVA: 0x00009625 File Offset: 0x00007825
		internal string ParseChildName(string path, CmdletProviderContext context, bool useDefaultProvider)
		{
			return this.sessionState.GetChildName(path, context, useDefaultProvider);
		}

		// Token: 0x06000271 RID: 625 RVA: 0x00009635 File Offset: 0x00007835
		public string NormalizeRelativePath(string path, string basePath)
		{
			return this.sessionState.NormalizeRelativePath(path, basePath);
		}

		// Token: 0x06000272 RID: 626 RVA: 0x00009644 File Offset: 0x00007844
		internal string NormalizeRelativePath(string path, string basePath, CmdletProviderContext context)
		{
			return this.sessionState.NormalizeRelativePath(path, basePath, context);
		}

		// Token: 0x06000273 RID: 627 RVA: 0x00009654 File Offset: 0x00007854
		public bool IsValid(string path)
		{
			return this.sessionState.IsValidPath(path);
		}

		// Token: 0x06000274 RID: 628 RVA: 0x00009662 File Offset: 0x00007862
		internal bool IsValid(string path, CmdletProviderContext context)
		{
			return this.sessionState.IsValidPath(path, context);
		}

		// Token: 0x1700009A RID: 154
		// (get) Token: 0x06000275 RID: 629 RVA: 0x00009671 File Offset: 0x00007871
		private LocationGlobber PathResolver
		{
			get
			{
				if (this.pathResolver == null)
				{
					this.pathResolver = this.sessionState.ExecutionContext.LocationGlobber;
				}
				return this.pathResolver;
			}
		}

		// Token: 0x040000D0 RID: 208
		private LocationGlobber pathResolver;

		// Token: 0x040000D1 RID: 209
		private SessionStateInternal sessionState;
	}
}
