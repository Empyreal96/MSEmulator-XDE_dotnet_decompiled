using System;
using System.Collections.Generic;

namespace DiscUtils
{
	// Token: 0x02000004 RID: 4
	public sealed class ClusterMap
	{
		// Token: 0x0600000C RID: 12 RVA: 0x000021F9 File Offset: 0x000003F9
		internal ClusterMap(ClusterRoles[] clusterToRole, object[] clusterToFileId, Dictionary<object, string[]> fileIdToPaths)
		{
			this._clusterToRole = clusterToRole;
			this._clusterToFileId = clusterToFileId;
			this._fileIdToPaths = fileIdToPaths;
		}

		// Token: 0x0600000D RID: 13 RVA: 0x00002216 File Offset: 0x00000416
		public ClusterRoles GetRole(long cluster)
		{
			if (this._clusterToRole == null || (long)this._clusterToRole.Length < cluster)
			{
				return ClusterRoles.None;
			}
			return this._clusterToRole[(int)(checked((IntPtr)cluster))];
		}

		// Token: 0x0600000E RID: 14 RVA: 0x00002238 File Offset: 0x00000438
		public string[] ClusterToPaths(long cluster)
		{
			if ((this.GetRole(cluster) & (ClusterRoles.DataFile | ClusterRoles.SystemFile)) != ClusterRoles.None)
			{
				object key = this._clusterToFileId[(int)(checked((IntPtr)cluster))];
				return this._fileIdToPaths[key];
			}
			return new string[0];
		}

		// Token: 0x04000006 RID: 6
		private readonly object[] _clusterToFileId;

		// Token: 0x04000007 RID: 7
		private readonly ClusterRoles[] _clusterToRole;

		// Token: 0x04000008 RID: 8
		private readonly Dictionary<object, string[]> _fileIdToPaths;
	}
}
