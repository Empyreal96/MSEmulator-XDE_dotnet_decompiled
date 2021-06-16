using System;
using System.CodeDom.Compiler;
using System.ComponentModel;
using System.Diagnostics;
using System.Globalization;
using System.Resources;
using System.Runtime.CompilerServices;

// Token: 0x02000A33 RID: 2611
[CompilerGenerated]
[GeneratedCode("System.Resources.Tools.StronglyTypedResourceBuilder", "4.0.0.0")]
[DebuggerNonUserCode]
internal class TransactionStrings
{
	// Token: 0x06006566 RID: 25958 RVA: 0x0020DAC4 File Offset: 0x0020BCC4
	internal TransactionStrings()
	{
	}

	// Token: 0x170019AF RID: 6575
	// (get) Token: 0x06006567 RID: 25959 RVA: 0x0020DACC File Offset: 0x0020BCCC
	[EditorBrowsable(EditorBrowsableState.Advanced)]
	internal static ResourceManager ResourceManager
	{
		get
		{
			if (object.ReferenceEquals(TransactionStrings.resourceMan, null))
			{
				ResourceManager resourceManager = new ResourceManager("TransactionStrings", typeof(TransactionStrings).Assembly);
				TransactionStrings.resourceMan = resourceManager;
			}
			return TransactionStrings.resourceMan;
		}
	}

	// Token: 0x170019B0 RID: 6576
	// (get) Token: 0x06006568 RID: 25960 RVA: 0x0020DB0B File Offset: 0x0020BD0B
	// (set) Token: 0x06006569 RID: 25961 RVA: 0x0020DB12 File Offset: 0x0020BD12
	[EditorBrowsable(EditorBrowsableState.Advanced)]
	internal static CultureInfo Culture
	{
		get
		{
			return TransactionStrings.resourceCulture;
		}
		set
		{
			TransactionStrings.resourceCulture = value;
		}
	}

	// Token: 0x170019B1 RID: 6577
	// (get) Token: 0x0600656A RID: 25962 RVA: 0x0020DB1A File Offset: 0x0020BD1A
	internal static string BaseTransactionMustBeFirst
	{
		get
		{
			return TransactionStrings.ResourceManager.GetString("BaseTransactionMustBeFirst", TransactionStrings.resourceCulture);
		}
	}

	// Token: 0x170019B2 RID: 6578
	// (get) Token: 0x0600656B RID: 25963 RVA: 0x0020DB30 File Offset: 0x0020BD30
	internal static string BaseTransactionNotActive
	{
		get
		{
			return TransactionStrings.ResourceManager.GetString("BaseTransactionNotActive", TransactionStrings.resourceCulture);
		}
	}

	// Token: 0x170019B3 RID: 6579
	// (get) Token: 0x0600656C RID: 25964 RVA: 0x0020DB46 File Offset: 0x0020BD46
	internal static string BaseTransactionNotSet
	{
		get
		{
			return TransactionStrings.ResourceManager.GetString("BaseTransactionNotSet", TransactionStrings.resourceCulture);
		}
	}

	// Token: 0x170019B4 RID: 6580
	// (get) Token: 0x0600656D RID: 25965 RVA: 0x0020DB5C File Offset: 0x0020BD5C
	internal static string CmdletRequiresUseTx
	{
		get
		{
			return TransactionStrings.ResourceManager.GetString("CmdletRequiresUseTx", TransactionStrings.resourceCulture);
		}
	}

	// Token: 0x170019B5 RID: 6581
	// (get) Token: 0x0600656E RID: 25966 RVA: 0x0020DB72 File Offset: 0x0020BD72
	internal static string CommittedTransactionForCommit
	{
		get
		{
			return TransactionStrings.ResourceManager.GetString("CommittedTransactionForCommit", TransactionStrings.resourceCulture);
		}
	}

	// Token: 0x170019B6 RID: 6582
	// (get) Token: 0x0600656F RID: 25967 RVA: 0x0020DB88 File Offset: 0x0020BD88
	internal static string CommittedTransactionForRollback
	{
		get
		{
			return TransactionStrings.ResourceManager.GetString("CommittedTransactionForRollback", TransactionStrings.resourceCulture);
		}
	}

	// Token: 0x170019B7 RID: 6583
	// (get) Token: 0x06006570 RID: 25968 RVA: 0x0020DB9E File Offset: 0x0020BD9E
	internal static string NoTransactionActive
	{
		get
		{
			return TransactionStrings.ResourceManager.GetString("NoTransactionActive", TransactionStrings.resourceCulture);
		}
	}

	// Token: 0x170019B8 RID: 6584
	// (get) Token: 0x06006571 RID: 25969 RVA: 0x0020DBB4 File Offset: 0x0020BDB4
	internal static string NoTransactionActiveForCommit
	{
		get
		{
			return TransactionStrings.ResourceManager.GetString("NoTransactionActiveForCommit", TransactionStrings.resourceCulture);
		}
	}

	// Token: 0x170019B9 RID: 6585
	// (get) Token: 0x06006572 RID: 25970 RVA: 0x0020DBCA File Offset: 0x0020BDCA
	internal static string NoTransactionActiveForRollback
	{
		get
		{
			return TransactionStrings.ResourceManager.GetString("NoTransactionActiveForRollback", TransactionStrings.resourceCulture);
		}
	}

	// Token: 0x170019BA RID: 6586
	// (get) Token: 0x06006573 RID: 25971 RVA: 0x0020DBE0 File Offset: 0x0020BDE0
	internal static string NoTransactionAvailable
	{
		get
		{
			return TransactionStrings.ResourceManager.GetString("NoTransactionAvailable", TransactionStrings.resourceCulture);
		}
	}

	// Token: 0x170019BB RID: 6587
	// (get) Token: 0x06006574 RID: 25972 RVA: 0x0020DBF6 File Offset: 0x0020BDF6
	internal static string NoTransactionForActivation
	{
		get
		{
			return TransactionStrings.ResourceManager.GetString("NoTransactionForActivation", TransactionStrings.resourceCulture);
		}
	}

	// Token: 0x170019BC RID: 6588
	// (get) Token: 0x06006575 RID: 25973 RVA: 0x0020DC0C File Offset: 0x0020BE0C
	internal static string NoTransactionForActivationBecauseRollback
	{
		get
		{
			return TransactionStrings.ResourceManager.GetString("NoTransactionForActivationBecauseRollback", TransactionStrings.resourceCulture);
		}
	}

	// Token: 0x170019BD RID: 6589
	// (get) Token: 0x06006576 RID: 25974 RVA: 0x0020DC22 File Offset: 0x0020BE22
	internal static string NoTransactionStarted
	{
		get
		{
			return TransactionStrings.ResourceManager.GetString("NoTransactionStarted", TransactionStrings.resourceCulture);
		}
	}

	// Token: 0x170019BE RID: 6590
	// (get) Token: 0x06006577 RID: 25975 RVA: 0x0020DC38 File Offset: 0x0020BE38
	internal static string NoTransactionStartedFromCommit
	{
		get
		{
			return TransactionStrings.ResourceManager.GetString("NoTransactionStartedFromCommit", TransactionStrings.resourceCulture);
		}
	}

	// Token: 0x170019BF RID: 6591
	// (get) Token: 0x06006578 RID: 25976 RVA: 0x0020DC4E File Offset: 0x0020BE4E
	internal static string NoTransactionStartedFromRollback
	{
		get
		{
			return TransactionStrings.ResourceManager.GetString("NoTransactionStartedFromRollback", TransactionStrings.resourceCulture);
		}
	}

	// Token: 0x170019C0 RID: 6592
	// (get) Token: 0x06006579 RID: 25977 RVA: 0x0020DC64 File Offset: 0x0020BE64
	internal static string TransactionRolledBackForCommit
	{
		get
		{
			return TransactionStrings.ResourceManager.GetString("TransactionRolledBackForCommit", TransactionStrings.resourceCulture);
		}
	}

	// Token: 0x170019C1 RID: 6593
	// (get) Token: 0x0600657A RID: 25978 RVA: 0x0020DC7A File Offset: 0x0020BE7A
	internal static string TransactionRolledBackForRollback
	{
		get
		{
			return TransactionStrings.ResourceManager.GetString("TransactionRolledBackForRollback", TransactionStrings.resourceCulture);
		}
	}

	// Token: 0x170019C2 RID: 6594
	// (get) Token: 0x0600657B RID: 25979 RVA: 0x0020DC90 File Offset: 0x0020BE90
	internal static string TransactionTimedOut
	{
		get
		{
			return TransactionStrings.ResourceManager.GetString("TransactionTimedOut", TransactionStrings.resourceCulture);
		}
	}

	// Token: 0x0400325D RID: 12893
	private static ResourceManager resourceMan;

	// Token: 0x0400325E RID: 12894
	private static CultureInfo resourceCulture;
}
