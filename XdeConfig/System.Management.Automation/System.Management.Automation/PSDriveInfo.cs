using System;
using System.Threading;

namespace System.Management.Automation
{
	// Token: 0x02000460 RID: 1120
	public class PSDriveInfo : IComparable
	{
		// Token: 0x17000B2C RID: 2860
		// (get) Token: 0x0600312C RID: 12588 RVA: 0x0010C781 File Offset: 0x0010A981
		// (set) Token: 0x0600312D RID: 12589 RVA: 0x0010C789 File Offset: 0x0010A989
		public string CurrentLocation
		{
			get
			{
				return this.currentWorkingDirectory;
			}
			set
			{
				this.currentWorkingDirectory = value;
			}
		}

		// Token: 0x17000B2D RID: 2861
		// (get) Token: 0x0600312E RID: 12590 RVA: 0x0010C792 File Offset: 0x0010A992
		public string Name
		{
			get
			{
				return this.name;
			}
		}

		// Token: 0x17000B2E RID: 2862
		// (get) Token: 0x0600312F RID: 12591 RVA: 0x0010C79A File Offset: 0x0010A99A
		public ProviderInfo Provider
		{
			get
			{
				return this.provider;
			}
		}

		// Token: 0x17000B2F RID: 2863
		// (get) Token: 0x06003130 RID: 12592 RVA: 0x0010C7A2 File Offset: 0x0010A9A2
		// (set) Token: 0x06003131 RID: 12593 RVA: 0x0010C7AA File Offset: 0x0010A9AA
		public string Root
		{
			get
			{
				return this.root;
			}
			internal set
			{
				this.root = value;
			}
		}

		// Token: 0x06003132 RID: 12594 RVA: 0x0010C7B4 File Offset: 0x0010A9B4
		internal void SetRoot(string path)
		{
			if (path == null)
			{
				throw PSTraceSource.NewArgumentNullException("path");
			}
			if (!this.driveBeingCreated)
			{
				NotSupportedException ex = PSTraceSource.NewNotSupportedException();
				throw ex;
			}
			this.root = path;
		}

		// Token: 0x17000B30 RID: 2864
		// (get) Token: 0x06003133 RID: 12595 RVA: 0x0010C7E6 File Offset: 0x0010A9E6
		// (set) Token: 0x06003134 RID: 12596 RVA: 0x0010C7EE File Offset: 0x0010A9EE
		public string Description
		{
			get
			{
				return this.description;
			}
			set
			{
				this.description = value;
			}
		}

		// Token: 0x17000B31 RID: 2865
		// (get) Token: 0x06003135 RID: 12597 RVA: 0x0010C7F7 File Offset: 0x0010A9F7
		public PSCredential Credential
		{
			get
			{
				return this.credentials;
			}
		}

		// Token: 0x17000B32 RID: 2866
		// (set) Token: 0x06003136 RID: 12598 RVA: 0x0010C7FF File Offset: 0x0010A9FF
		internal bool DriveBeingCreated
		{
			set
			{
				this.driveBeingCreated = value;
			}
		}

		// Token: 0x17000B33 RID: 2867
		// (get) Token: 0x06003137 RID: 12599 RVA: 0x0010C808 File Offset: 0x0010AA08
		// (set) Token: 0x06003138 RID: 12600 RVA: 0x0010C810 File Offset: 0x0010AA10
		internal bool IsAutoMounted
		{
			get
			{
				return this.isAutoMounted;
			}
			set
			{
				this.isAutoMounted = value;
			}
		}

		// Token: 0x17000B34 RID: 2868
		// (get) Token: 0x06003139 RID: 12601 RVA: 0x0010C819 File Offset: 0x0010AA19
		// (set) Token: 0x0600313A RID: 12602 RVA: 0x0010C821 File Offset: 0x0010AA21
		internal bool IsAutoMountedManuallyRemoved
		{
			get
			{
				return this.isAutoMountedManuallyRemoved;
			}
			set
			{
				this.isAutoMountedManuallyRemoved = value;
			}
		}

		// Token: 0x17000B35 RID: 2869
		// (get) Token: 0x0600313B RID: 12603 RVA: 0x0010C82A File Offset: 0x0010AA2A
		internal bool Persist
		{
			get
			{
				return this.persist;
			}
		}

		// Token: 0x17000B36 RID: 2870
		// (get) Token: 0x0600313C RID: 12604 RVA: 0x0010C832 File Offset: 0x0010AA32
		// (set) Token: 0x0600313D RID: 12605 RVA: 0x0010C83A File Offset: 0x0010AA3A
		internal bool IsNetworkDrive
		{
			get
			{
				return this.isNetworkDrive;
			}
			set
			{
				this.isNetworkDrive = value;
			}
		}

		// Token: 0x17000B37 RID: 2871
		// (get) Token: 0x0600313E RID: 12606 RVA: 0x0010C843 File Offset: 0x0010AA43
		// (set) Token: 0x0600313F RID: 12607 RVA: 0x0010C84B File Offset: 0x0010AA4B
		public string DisplayRoot
		{
			get
			{
				return this.displayRoot;
			}
			internal set
			{
				this.displayRoot = value;
			}
		}

		// Token: 0x06003140 RID: 12608 RVA: 0x0010C854 File Offset: 0x0010AA54
		protected PSDriveInfo(PSDriveInfo driveInfo)
		{
			if (driveInfo == null)
			{
				throw PSTraceSource.NewArgumentNullException("driveInfo");
			}
			this.name = driveInfo.Name;
			this.provider = driveInfo.Provider;
			this.credentials = driveInfo.Credential;
			this.currentWorkingDirectory = driveInfo.CurrentLocation;
			this.description = driveInfo.Description;
			this.driveBeingCreated = driveInfo.driveBeingCreated;
			this.hidden = driveInfo.hidden;
			this.isAutoMounted = driveInfo.isAutoMounted;
			this.root = driveInfo.root;
			this.persist = driveInfo.Persist;
			this.Trace();
		}

		// Token: 0x06003141 RID: 12609 RVA: 0x0010C904 File Offset: 0x0010AB04
		public PSDriveInfo(string name, ProviderInfo provider, string root, string description, PSCredential credential)
		{
			if (name == null)
			{
				throw PSTraceSource.NewArgumentNullException("name");
			}
			if (provider == null)
			{
				throw PSTraceSource.NewArgumentNullException("provider");
			}
			if (root == null)
			{
				throw PSTraceSource.NewArgumentNullException("root");
			}
			this.name = name;
			this.provider = provider;
			this.root = root;
			this.description = description;
			if (credential != null)
			{
				this.credentials = credential;
			}
			this.currentWorkingDirectory = string.Empty;
			this.Trace();
		}

		// Token: 0x06003142 RID: 12610 RVA: 0x0010C986 File Offset: 0x0010AB86
		public PSDriveInfo(string name, ProviderInfo provider, string root, string description, PSCredential credential, string displayRoot) : this(name, provider, root, description, credential)
		{
			this.displayRoot = displayRoot;
		}

		// Token: 0x06003143 RID: 12611 RVA: 0x0010C99D File Offset: 0x0010AB9D
		public PSDriveInfo(string name, ProviderInfo provider, string root, string description, PSCredential credential, bool persist) : this(name, provider, root, description, credential)
		{
			this.persist = persist;
		}

		// Token: 0x06003144 RID: 12612 RVA: 0x0010C9B4 File Offset: 0x0010ABB4
		public override string ToString()
		{
			return this.Name;
		}

		// Token: 0x17000B38 RID: 2872
		// (get) Token: 0x06003145 RID: 12613 RVA: 0x0010C9BC File Offset: 0x0010ABBC
		// (set) Token: 0x06003146 RID: 12614 RVA: 0x0010C9C4 File Offset: 0x0010ABC4
		internal bool Hidden
		{
			get
			{
				return this.hidden;
			}
			set
			{
				this.hidden = value;
			}
		}

		// Token: 0x06003147 RID: 12615 RVA: 0x0010C9CD File Offset: 0x0010ABCD
		internal void SetName(string newName)
		{
			if (string.IsNullOrEmpty(newName))
			{
				throw PSTraceSource.NewArgumentException("newName");
			}
			this.name = newName;
		}

		// Token: 0x06003148 RID: 12616 RVA: 0x0010C9E9 File Offset: 0x0010ABE9
		internal void SetProvider(ProviderInfo newProvider)
		{
			if (newProvider == null)
			{
				throw PSTraceSource.NewArgumentNullException("newProvider");
			}
			this.provider = newProvider;
		}

		// Token: 0x06003149 RID: 12617 RVA: 0x0010CA00 File Offset: 0x0010AC00
		internal void Trace()
		{
			PSDriveInfo.tracer.WriteLine("A drive was found:", new object[0]);
			if (this.Name != null)
			{
				PSDriveInfo.tracer.WriteLine("\tName: {0}", new object[]
				{
					this.Name
				});
			}
			if (this.Provider != null)
			{
				PSDriveInfo.tracer.WriteLine("\tProvider: {0}", new object[]
				{
					this.Provider
				});
			}
			if (this.Root != null)
			{
				PSDriveInfo.tracer.WriteLine("\tRoot: {0}", new object[]
				{
					this.Root
				});
			}
			if (this.CurrentLocation != null)
			{
				PSDriveInfo.tracer.WriteLine("\tCWD: {0}", new object[]
				{
					this.CurrentLocation
				});
			}
			if (this.Description != null)
			{
				PSDriveInfo.tracer.WriteLine("\tDescription: {0}", new object[]
				{
					this.Description
				});
			}
		}

		// Token: 0x0600314A RID: 12618 RVA: 0x0010CAED File Offset: 0x0010ACED
		public int CompareTo(PSDriveInfo drive)
		{
			if (drive == null)
			{
				throw PSTraceSource.NewArgumentNullException("drive");
			}
			return string.Compare(this.Name, drive.Name, StringComparison.CurrentCultureIgnoreCase);
		}

		// Token: 0x0600314B RID: 12619 RVA: 0x0010CB18 File Offset: 0x0010AD18
		public int CompareTo(object obj)
		{
			PSDriveInfo psdriveInfo = obj as PSDriveInfo;
			if (psdriveInfo == null)
			{
				ArgumentException ex = PSTraceSource.NewArgumentException("obj", SessionStateStrings.OnlyAbleToComparePSDriveInfo, new object[0]);
				throw ex;
			}
			return this.CompareTo(psdriveInfo);
		}

		// Token: 0x0600314C RID: 12620 RVA: 0x0010CB54 File Offset: 0x0010AD54
		public override bool Equals(object obj)
		{
			return obj is PSDriveInfo && this.CompareTo(obj) == 0;
		}

		// Token: 0x0600314D RID: 12621 RVA: 0x0010CB6A File Offset: 0x0010AD6A
		public bool Equals(PSDriveInfo drive)
		{
			return this.CompareTo(drive) == 0;
		}

		// Token: 0x0600314E RID: 12622 RVA: 0x0010CB78 File Offset: 0x0010AD78
		public static bool operator ==(PSDriveInfo drive1, PSDriveInfo drive2)
		{
			return drive1 == null == (drive2 == null) && (drive1 == null || drive1.Equals(drive2));
		}

		// Token: 0x0600314F RID: 12623 RVA: 0x0010CBA1 File Offset: 0x0010ADA1
		public static bool operator !=(PSDriveInfo drive1, PSDriveInfo drive2)
		{
			return !(drive1 == drive2);
		}

		// Token: 0x06003150 RID: 12624 RVA: 0x0010CBB0 File Offset: 0x0010ADB0
		public static bool operator <(PSDriveInfo drive1, PSDriveInfo drive2)
		{
			if (drive1 == null)
			{
				return drive2 != null;
			}
			return drive2 != null && drive1.CompareTo(drive2) < 0;
		}

		// Token: 0x06003151 RID: 12625 RVA: 0x0010CBDC File Offset: 0x0010ADDC
		public static bool operator >(PSDriveInfo drive1, PSDriveInfo drive2)
		{
			if (drive1 == null)
			{
				return drive2 == null && false;
			}
			return drive2 == null || drive1.CompareTo(drive2) > 0;
		}

		// Token: 0x06003152 RID: 12626 RVA: 0x0010CC06 File Offset: 0x0010AE06
		public override int GetHashCode()
		{
			return base.GetHashCode();
		}

		// Token: 0x06003153 RID: 12627 RVA: 0x0010CC0E File Offset: 0x0010AE0E
		internal PSNoteProperty GetNotePropertyForProviderCmdlets(string name)
		{
			if (this._noteProperty == null)
			{
				Interlocked.CompareExchange<PSNoteProperty>(ref this._noteProperty, new PSNoteProperty(name, this), null);
			}
			return this._noteProperty;
		}

		// Token: 0x04001A3E RID: 6718
		[TraceSource("PSDriveInfo", "The namespace navigation tracer")]
		private static PSTraceSource tracer = PSTraceSource.GetTracer("PSDriveInfo", "The namespace navigation tracer");

		// Token: 0x04001A3F RID: 6719
		private string currentWorkingDirectory;

		// Token: 0x04001A40 RID: 6720
		private string name;

		// Token: 0x04001A41 RID: 6721
		private ProviderInfo provider;

		// Token: 0x04001A42 RID: 6722
		private string root;

		// Token: 0x04001A43 RID: 6723
		private string description;

		// Token: 0x04001A44 RID: 6724
		private PSCredential credentials = PSCredential.Empty;

		// Token: 0x04001A45 RID: 6725
		private bool driveBeingCreated;

		// Token: 0x04001A46 RID: 6726
		private bool isAutoMounted;

		// Token: 0x04001A47 RID: 6727
		private bool isAutoMountedManuallyRemoved;

		// Token: 0x04001A48 RID: 6728
		private bool persist;

		// Token: 0x04001A49 RID: 6729
		private bool isNetworkDrive;

		// Token: 0x04001A4A RID: 6730
		private string displayRoot;

		// Token: 0x04001A4B RID: 6731
		private bool hidden;

		// Token: 0x04001A4C RID: 6732
		private PSNoteProperty _noteProperty;
	}
}
