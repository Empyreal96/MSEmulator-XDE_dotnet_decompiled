using System;
using System.CodeDom.Compiler;
using System.ComponentModel;
using System.Diagnostics;
using System.Globalization;
using System.Resources;
using System.Runtime.CompilerServices;

// Token: 0x02000A2C RID: 2604
[GeneratedCode("System.Resources.Tools.StronglyTypedResourceBuilder", "4.0.0.0")]
[DebuggerNonUserCode]
[CompilerGenerated]
internal class SessionStateStrings
{
	// Token: 0x06006428 RID: 25640 RVA: 0x0020BF7A File Offset: 0x0020A17A
	internal SessionStateStrings()
	{
	}

	// Token: 0x1700187F RID: 6271
	// (get) Token: 0x06006429 RID: 25641 RVA: 0x0020BF84 File Offset: 0x0020A184
	[EditorBrowsable(EditorBrowsableState.Advanced)]
	internal static ResourceManager ResourceManager
	{
		get
		{
			if (object.ReferenceEquals(SessionStateStrings.resourceMan, null))
			{
				ResourceManager resourceManager = new ResourceManager("SessionStateStrings", typeof(SessionStateStrings).Assembly);
				SessionStateStrings.resourceMan = resourceManager;
			}
			return SessionStateStrings.resourceMan;
		}
	}

	// Token: 0x17001880 RID: 6272
	// (get) Token: 0x0600642A RID: 25642 RVA: 0x0020BFC3 File Offset: 0x0020A1C3
	// (set) Token: 0x0600642B RID: 25643 RVA: 0x0020BFCA File Offset: 0x0020A1CA
	[EditorBrowsable(EditorBrowsableState.Advanced)]
	internal static CultureInfo Culture
	{
		get
		{
			return SessionStateStrings.resourceCulture;
		}
		set
		{
			SessionStateStrings.resourceCulture = value;
		}
	}

	// Token: 0x17001881 RID: 6273
	// (get) Token: 0x0600642C RID: 25644 RVA: 0x0020BFD2 File Offset: 0x0020A1D2
	internal static string AliasAllScopeOptionCannotBeRemoved
	{
		get
		{
			return SessionStateStrings.ResourceManager.GetString("AliasAllScopeOptionCannotBeRemoved", SessionStateStrings.resourceCulture);
		}
	}

	// Token: 0x17001882 RID: 6274
	// (get) Token: 0x0600642D RID: 25645 RVA: 0x0020BFE8 File Offset: 0x0020A1E8
	internal static string AliasAlreadyExists
	{
		get
		{
			return SessionStateStrings.ResourceManager.GetString("AliasAlreadyExists", SessionStateStrings.resourceCulture);
		}
	}

	// Token: 0x17001883 RID: 6275
	// (get) Token: 0x0600642E RID: 25646 RVA: 0x0020BFFE File Offset: 0x0020A1FE
	internal static string AliasCannotBeMadeConstant
	{
		get
		{
			return SessionStateStrings.ResourceManager.GetString("AliasCannotBeMadeConstant", SessionStateStrings.resourceCulture);
		}
	}

	// Token: 0x17001884 RID: 6276
	// (get) Token: 0x0600642F RID: 25647 RVA: 0x0020C014 File Offset: 0x0020A214
	internal static string AliasDriveDescription
	{
		get
		{
			return SessionStateStrings.ResourceManager.GetString("AliasDriveDescription", SessionStateStrings.resourceCulture);
		}
	}

	// Token: 0x17001885 RID: 6277
	// (get) Token: 0x06006430 RID: 25648 RVA: 0x0020C02A File Offset: 0x0020A22A
	internal static string AliasIsConstant
	{
		get
		{
			return SessionStateStrings.ResourceManager.GetString("AliasIsConstant", SessionStateStrings.resourceCulture);
		}
	}

	// Token: 0x17001886 RID: 6278
	// (get) Token: 0x06006431 RID: 25649 RVA: 0x0020C040 File Offset: 0x0020A240
	internal static string AliasIsReadOnly
	{
		get
		{
			return SessionStateStrings.ResourceManager.GetString("AliasIsReadOnly", SessionStateStrings.resourceCulture);
		}
	}

	// Token: 0x17001887 RID: 6279
	// (get) Token: 0x06006432 RID: 25650 RVA: 0x0020C056 File Offset: 0x0020A256
	internal static string AliasNotFound
	{
		get
		{
			return SessionStateStrings.ResourceManager.GetString("AliasNotFound", SessionStateStrings.resourceCulture);
		}
	}

	// Token: 0x17001888 RID: 6280
	// (get) Token: 0x06006433 RID: 25651 RVA: 0x0020C06C File Offset: 0x0020A26C
	internal static string AliasNotRemovable
	{
		get
		{
			return SessionStateStrings.ResourceManager.GetString("AliasNotRemovable", SessionStateStrings.resourceCulture);
		}
	}

	// Token: 0x17001889 RID: 6281
	// (get) Token: 0x06006434 RID: 25652 RVA: 0x0020C082 File Offset: 0x0020A282
	internal static string AliasNotWritable
	{
		get
		{
			return SessionStateStrings.ResourceManager.GetString("AliasNotWritable", SessionStateStrings.resourceCulture);
		}
	}

	// Token: 0x1700188A RID: 6282
	// (get) Token: 0x06006435 RID: 25653 RVA: 0x0020C098 File Offset: 0x0020A298
	internal static string AliasOverflow
	{
		get
		{
			return SessionStateStrings.ResourceManager.GetString("AliasOverflow", SessionStateStrings.resourceCulture);
		}
	}

	// Token: 0x1700188B RID: 6283
	// (get) Token: 0x06006436 RID: 25654 RVA: 0x0020C0AE File Offset: 0x0020A2AE
	internal static string AliasWithCommandNameAlreadyExists
	{
		get
		{
			return SessionStateStrings.ResourceManager.GetString("AliasWithCommandNameAlreadyExists", SessionStateStrings.resourceCulture);
		}
	}

	// Token: 0x1700188C RID: 6284
	// (get) Token: 0x06006437 RID: 25655 RVA: 0x0020C0C4 File Offset: 0x0020A2C4
	internal static string CannotRenameAlias
	{
		get
		{
			return SessionStateStrings.ResourceManager.GetString("CannotRenameAlias", SessionStateStrings.resourceCulture);
		}
	}

	// Token: 0x1700188D RID: 6285
	// (get) Token: 0x06006438 RID: 25656 RVA: 0x0020C0DA File Offset: 0x0020A2DA
	internal static string CannotRenameFilter
	{
		get
		{
			return SessionStateStrings.ResourceManager.GetString("CannotRenameFilter", SessionStateStrings.resourceCulture);
		}
	}

	// Token: 0x1700188E RID: 6286
	// (get) Token: 0x06006439 RID: 25657 RVA: 0x0020C0F0 File Offset: 0x0020A2F0
	internal static string CannotRenameFunction
	{
		get
		{
			return SessionStateStrings.ResourceManager.GetString("CannotRenameFunction", SessionStateStrings.resourceCulture);
		}
	}

	// Token: 0x1700188F RID: 6287
	// (get) Token: 0x0600643A RID: 25658 RVA: 0x0020C106 File Offset: 0x0020A306
	internal static string CannotRenameVariable
	{
		get
		{
			return SessionStateStrings.ResourceManager.GetString("CannotRenameVariable", SessionStateStrings.resourceCulture);
		}
	}

	// Token: 0x17001890 RID: 6288
	// (get) Token: 0x0600643B RID: 25659 RVA: 0x0020C11C File Offset: 0x0020A31C
	internal static string CanNotRun
	{
		get
		{
			return SessionStateStrings.ResourceManager.GetString("CanNotRun", SessionStateStrings.resourceCulture);
		}
	}

	// Token: 0x17001891 RID: 6289
	// (get) Token: 0x0600643C RID: 25660 RVA: 0x0020C132 File Offset: 0x0020A332
	internal static string ClearContentDynamicParametersProviderException
	{
		get
		{
			return SessionStateStrings.ResourceManager.GetString("ClearContentDynamicParametersProviderException", SessionStateStrings.resourceCulture);
		}
	}

	// Token: 0x17001892 RID: 6290
	// (get) Token: 0x0600643D RID: 25661 RVA: 0x0020C148 File Offset: 0x0020A348
	internal static string ClearContentProviderException
	{
		get
		{
			return SessionStateStrings.ResourceManager.GetString("ClearContentProviderException", SessionStateStrings.resourceCulture);
		}
	}

	// Token: 0x17001893 RID: 6291
	// (get) Token: 0x0600643E RID: 25662 RVA: 0x0020C15E File Offset: 0x0020A35E
	internal static string ClearItemDynamicParametersProviderException
	{
		get
		{
			return SessionStateStrings.ResourceManager.GetString("ClearItemDynamicParametersProviderException", SessionStateStrings.resourceCulture);
		}
	}

	// Token: 0x17001894 RID: 6292
	// (get) Token: 0x0600643F RID: 25663 RVA: 0x0020C174 File Offset: 0x0020A374
	internal static string ClearItemProviderException
	{
		get
		{
			return SessionStateStrings.ResourceManager.GetString("ClearItemProviderException", SessionStateStrings.resourceCulture);
		}
	}

	// Token: 0x17001895 RID: 6293
	// (get) Token: 0x06006440 RID: 25664 RVA: 0x0020C18A File Offset: 0x0020A38A
	internal static string ClearPropertyDynamicParametersProviderException
	{
		get
		{
			return SessionStateStrings.ResourceManager.GetString("ClearPropertyDynamicParametersProviderException", SessionStateStrings.resourceCulture);
		}
	}

	// Token: 0x17001896 RID: 6294
	// (get) Token: 0x06006441 RID: 25665 RVA: 0x0020C1A0 File Offset: 0x0020A3A0
	internal static string ClearPropertyProviderException
	{
		get
		{
			return SessionStateStrings.ResourceManager.GetString("ClearPropertyProviderException", SessionStateStrings.resourceCulture);
		}
	}

	// Token: 0x17001897 RID: 6295
	// (get) Token: 0x06006442 RID: 25666 RVA: 0x0020C1B6 File Offset: 0x0020A3B6
	internal static string CmdletIsReadOnly
	{
		get
		{
			return SessionStateStrings.ResourceManager.GetString("CmdletIsReadOnly", SessionStateStrings.resourceCulture);
		}
	}

	// Token: 0x17001898 RID: 6296
	// (get) Token: 0x06006443 RID: 25667 RVA: 0x0020C1CC File Offset: 0x0020A3CC
	internal static string CmdletProvider_NotSupported
	{
		get
		{
			return SessionStateStrings.ResourceManager.GetString("CmdletProvider_NotSupported", SessionStateStrings.resourceCulture);
		}
	}

	// Token: 0x17001899 RID: 6297
	// (get) Token: 0x06006444 RID: 25668 RVA: 0x0020C1E2 File Offset: 0x0020A3E2
	internal static string CmdletProvider_NotSupportedRecursionDepth
	{
		get
		{
			return SessionStateStrings.ResourceManager.GetString("CmdletProvider_NotSupportedRecursionDepth", SessionStateStrings.resourceCulture);
		}
	}

	// Token: 0x1700189A RID: 6298
	// (get) Token: 0x06006445 RID: 25669 RVA: 0x0020C1F8 File Offset: 0x0020A3F8
	internal static string CmdletProviderAlreadyExists
	{
		get
		{
			return SessionStateStrings.ResourceManager.GetString("CmdletProviderAlreadyExists", SessionStateStrings.resourceCulture);
		}
	}

	// Token: 0x1700189B RID: 6299
	// (get) Token: 0x06006446 RID: 25670 RVA: 0x0020C20E File Offset: 0x0020A40E
	internal static string CommandIsPrivate
	{
		get
		{
			return SessionStateStrings.ResourceManager.GetString("CommandIsPrivate", SessionStateStrings.resourceCulture);
		}
	}

	// Token: 0x1700189C RID: 6300
	// (get) Token: 0x06006447 RID: 25671 RVA: 0x0020C224 File Offset: 0x0020A424
	internal static string ContainerCmdletProvider_NotSupported
	{
		get
		{
			return SessionStateStrings.ResourceManager.GetString("ContainerCmdletProvider_NotSupported", SessionStateStrings.resourceCulture);
		}
	}

	// Token: 0x1700189D RID: 6301
	// (get) Token: 0x06006448 RID: 25672 RVA: 0x0020C23A File Offset: 0x0020A43A
	internal static string CopyContainerItemToLeafError
	{
		get
		{
			return SessionStateStrings.ResourceManager.GetString("CopyContainerItemToLeafError", SessionStateStrings.resourceCulture);
		}
	}

	// Token: 0x1700189E RID: 6302
	// (get) Token: 0x06006449 RID: 25673 RVA: 0x0020C250 File Offset: 0x0020A450
	internal static string CopyContainerToContainerWithoutRecurseOrContainer
	{
		get
		{
			return SessionStateStrings.ResourceManager.GetString("CopyContainerToContainerWithoutRecurseOrContainer", SessionStateStrings.resourceCulture);
		}
	}

	// Token: 0x1700189F RID: 6303
	// (get) Token: 0x0600644A RID: 25674 RVA: 0x0020C266 File Offset: 0x0020A466
	internal static string CopyItemDoesntExist
	{
		get
		{
			return SessionStateStrings.ResourceManager.GetString("CopyItemDoesntExist", SessionStateStrings.resourceCulture);
		}
	}

	// Token: 0x170018A0 RID: 6304
	// (get) Token: 0x0600644B RID: 25675 RVA: 0x0020C27C File Offset: 0x0020A47C
	internal static string CopyItemDynamicParametersProviderException
	{
		get
		{
			return SessionStateStrings.ResourceManager.GetString("CopyItemDynamicParametersProviderException", SessionStateStrings.resourceCulture);
		}
	}

	// Token: 0x170018A1 RID: 6305
	// (get) Token: 0x0600644C RID: 25676 RVA: 0x0020C292 File Offset: 0x0020A492
	internal static string CopyItemFromSessionToSession
	{
		get
		{
			return SessionStateStrings.ResourceManager.GetString("CopyItemFromSessionToSession", SessionStateStrings.resourceCulture);
		}
	}

	// Token: 0x170018A2 RID: 6306
	// (get) Token: 0x0600644D RID: 25677 RVA: 0x0020C2A8 File Offset: 0x0020A4A8
	internal static string CopyItemProviderException
	{
		get
		{
			return SessionStateStrings.ResourceManager.GetString("CopyItemProviderException", SessionStateStrings.resourceCulture);
		}
	}

	// Token: 0x170018A3 RID: 6307
	// (get) Token: 0x0600644E RID: 25678 RVA: 0x0020C2BE File Offset: 0x0020A4BE
	internal static string CopyItemRemotelyPathIsNotAbsolute
	{
		get
		{
			return SessionStateStrings.ResourceManager.GetString("CopyItemRemotelyPathIsNotAbsolute", SessionStateStrings.resourceCulture);
		}
	}

	// Token: 0x170018A4 RID: 6308
	// (get) Token: 0x0600644F RID: 25679 RVA: 0x0020C2D4 File Offset: 0x0020A4D4
	internal static string CopyItemRemotelyPathIsNullOrEmpty
	{
		get
		{
			return SessionStateStrings.ResourceManager.GetString("CopyItemRemotelyPathIsNullOrEmpty", SessionStateStrings.resourceCulture);
		}
	}

	// Token: 0x170018A5 RID: 6309
	// (get) Token: 0x06006450 RID: 25680 RVA: 0x0020C2EA File Offset: 0x0020A4EA
	internal static string CopyItemSessionProperties
	{
		get
		{
			return SessionStateStrings.ResourceManager.GetString("CopyItemSessionProperties", SessionStateStrings.resourceCulture);
		}
	}

	// Token: 0x170018A6 RID: 6310
	// (get) Token: 0x06006451 RID: 25681 RVA: 0x0020C300 File Offset: 0x0020A500
	internal static string CopyItemSourceAndDestinationNotSameProvider
	{
		get
		{
			return SessionStateStrings.ResourceManager.GetString("CopyItemSourceAndDestinationNotSameProvider", SessionStateStrings.resourceCulture);
		}
	}

	// Token: 0x170018A7 RID: 6311
	// (get) Token: 0x06006452 RID: 25682 RVA: 0x0020C316 File Offset: 0x0020A516
	internal static string CopyItemValidateRemotePath
	{
		get
		{
			return SessionStateStrings.ResourceManager.GetString("CopyItemValidateRemotePath", SessionStateStrings.resourceCulture);
		}
	}

	// Token: 0x170018A8 RID: 6312
	// (get) Token: 0x06006453 RID: 25683 RVA: 0x0020C32C File Offset: 0x0020A52C
	internal static string CopyPropertyDynamicParametersProviderException
	{
		get
		{
			return SessionStateStrings.ResourceManager.GetString("CopyPropertyDynamicParametersProviderException", SessionStateStrings.resourceCulture);
		}
	}

	// Token: 0x170018A9 RID: 6313
	// (get) Token: 0x06006454 RID: 25684 RVA: 0x0020C342 File Offset: 0x0020A542
	internal static string CopyPropertyProviderException
	{
		get
		{
			return SessionStateStrings.ResourceManager.GetString("CopyPropertyProviderException", SessionStateStrings.resourceCulture);
		}
	}

	// Token: 0x170018AA RID: 6314
	// (get) Token: 0x06006455 RID: 25685 RVA: 0x0020C358 File Offset: 0x0020A558
	internal static string Credentials_NotSupported
	{
		get
		{
			return SessionStateStrings.ResourceManager.GetString("Credentials_NotSupported", SessionStateStrings.resourceCulture);
		}
	}

	// Token: 0x170018AB RID: 6315
	// (get) Token: 0x06006456 RID: 25686 RVA: 0x0020C36E File Offset: 0x0020A56E
	internal static string DollarNullDescription
	{
		get
		{
			return SessionStateStrings.ResourceManager.GetString("DollarNullDescription", SessionStateStrings.resourceCulture);
		}
	}

	// Token: 0x170018AC RID: 6316
	// (get) Token: 0x06006457 RID: 25687 RVA: 0x0020C384 File Offset: 0x0020A584
	internal static string DriveAlreadyExists
	{
		get
		{
			return SessionStateStrings.ResourceManager.GetString("DriveAlreadyExists", SessionStateStrings.resourceCulture);
		}
	}

	// Token: 0x170018AD RID: 6317
	// (get) Token: 0x06006458 RID: 25688 RVA: 0x0020C39A File Offset: 0x0020A59A
	internal static string DriveCmdletProvider_NotSupported
	{
		get
		{
			return SessionStateStrings.ResourceManager.GetString("DriveCmdletProvider_NotSupported", SessionStateStrings.resourceCulture);
		}
	}

	// Token: 0x170018AE RID: 6318
	// (get) Token: 0x06006459 RID: 25689 RVA: 0x0020C3B0 File Offset: 0x0020A5B0
	internal static string DriveNameIllegalCharacters
	{
		get
		{
			return SessionStateStrings.ResourceManager.GetString("DriveNameIllegalCharacters", SessionStateStrings.resourceCulture);
		}
	}

	// Token: 0x170018AF RID: 6319
	// (get) Token: 0x0600645A RID: 25690 RVA: 0x0020C3C6 File Offset: 0x0020A5C6
	internal static string DriveNotFound
	{
		get
		{
			return SessionStateStrings.ResourceManager.GetString("DriveNotFound", SessionStateStrings.resourceCulture);
		}
	}

	// Token: 0x170018B0 RID: 6320
	// (get) Token: 0x0600645B RID: 25691 RVA: 0x0020C3DC File Offset: 0x0020A5DC
	internal static string DriveOverflow
	{
		get
		{
			return SessionStateStrings.ResourceManager.GetString("DriveOverflow", SessionStateStrings.resourceCulture);
		}
	}

	// Token: 0x170018B1 RID: 6321
	// (get) Token: 0x0600645C RID: 25692 RVA: 0x0020C3F2 File Offset: 0x0020A5F2
	internal static string DriveRemovalPreventedByProvider
	{
		get
		{
			return SessionStateStrings.ResourceManager.GetString("DriveRemovalPreventedByProvider", SessionStateStrings.resourceCulture);
		}
	}

	// Token: 0x170018B2 RID: 6322
	// (get) Token: 0x0600645D RID: 25693 RVA: 0x0020C408 File Offset: 0x0020A608
	internal static string EnvironmentDriveDescription
	{
		get
		{
			return SessionStateStrings.ResourceManager.GetString("EnvironmentDriveDescription", SessionStateStrings.resourceCulture);
		}
	}

	// Token: 0x170018B3 RID: 6323
	// (get) Token: 0x0600645E RID: 25694 RVA: 0x0020C41E File Offset: 0x0020A61E
	internal static string ErrorStreamingNotEnabled
	{
		get
		{
			return SessionStateStrings.ResourceManager.GetString("ErrorStreamingNotEnabled", SessionStateStrings.resourceCulture);
		}
	}

	// Token: 0x170018B4 RID: 6324
	// (get) Token: 0x0600645F RID: 25695 RVA: 0x0020C434 File Offset: 0x0020A634
	internal static string FileSystemProviderCredentials_NotSupported
	{
		get
		{
			return SessionStateStrings.ResourceManager.GetString("FileSystemProviderCredentials_NotSupported", SessionStateStrings.resourceCulture);
		}
	}

	// Token: 0x170018B5 RID: 6325
	// (get) Token: 0x06006460 RID: 25696 RVA: 0x0020C44A File Offset: 0x0020A64A
	internal static string Filter_NotSupported
	{
		get
		{
			return SessionStateStrings.ResourceManager.GetString("Filter_NotSupported", SessionStateStrings.resourceCulture);
		}
	}

	// Token: 0x170018B6 RID: 6326
	// (get) Token: 0x06006461 RID: 25697 RVA: 0x0020C460 File Offset: 0x0020A660
	internal static string FilterAllScopeOptionCannotBeRemoved
	{
		get
		{
			return SessionStateStrings.ResourceManager.GetString("FilterAllScopeOptionCannotBeRemoved", SessionStateStrings.resourceCulture);
		}
	}

	// Token: 0x170018B7 RID: 6327
	// (get) Token: 0x06006462 RID: 25698 RVA: 0x0020C476 File Offset: 0x0020A676
	internal static string FilterCannotBeMadeConstant
	{
		get
		{
			return SessionStateStrings.ResourceManager.GetString("FilterCannotBeMadeConstant", SessionStateStrings.resourceCulture);
		}
	}

	// Token: 0x170018B8 RID: 6328
	// (get) Token: 0x06006463 RID: 25699 RVA: 0x0020C48C File Offset: 0x0020A68C
	internal static string FilterIsConstant
	{
		get
		{
			return SessionStateStrings.ResourceManager.GetString("FilterIsConstant", SessionStateStrings.resourceCulture);
		}
	}

	// Token: 0x170018B9 RID: 6329
	// (get) Token: 0x06006464 RID: 25700 RVA: 0x0020C4A2 File Offset: 0x0020A6A2
	internal static string FilterIsReadOnly
	{
		get
		{
			return SessionStateStrings.ResourceManager.GetString("FilterIsReadOnly", SessionStateStrings.resourceCulture);
		}
	}

	// Token: 0x170018BA RID: 6330
	// (get) Token: 0x06006465 RID: 25701 RVA: 0x0020C4B8 File Offset: 0x0020A6B8
	internal static string FunctionAllScopeOptionCannotBeRemoved
	{
		get
		{
			return SessionStateStrings.ResourceManager.GetString("FunctionAllScopeOptionCannotBeRemoved", SessionStateStrings.resourceCulture);
		}
	}

	// Token: 0x170018BB RID: 6331
	// (get) Token: 0x06006466 RID: 25702 RVA: 0x0020C4CE File Offset: 0x0020A6CE
	internal static string FunctionCannotBeMadeConstant
	{
		get
		{
			return SessionStateStrings.ResourceManager.GetString("FunctionCannotBeMadeConstant", SessionStateStrings.resourceCulture);
		}
	}

	// Token: 0x170018BC RID: 6332
	// (get) Token: 0x06006467 RID: 25703 RVA: 0x0020C4E4 File Offset: 0x0020A6E4
	internal static string FunctionDriveDescription
	{
		get
		{
			return SessionStateStrings.ResourceManager.GetString("FunctionDriveDescription", SessionStateStrings.resourceCulture);
		}
	}

	// Token: 0x170018BD RID: 6333
	// (get) Token: 0x06006468 RID: 25704 RVA: 0x0020C4FA File Offset: 0x0020A6FA
	internal static string FunctionIsConstant
	{
		get
		{
			return SessionStateStrings.ResourceManager.GetString("FunctionIsConstant", SessionStateStrings.resourceCulture);
		}
	}

	// Token: 0x170018BE RID: 6334
	// (get) Token: 0x06006469 RID: 25705 RVA: 0x0020C510 File Offset: 0x0020A710
	internal static string FunctionIsReadOnly
	{
		get
		{
			return SessionStateStrings.ResourceManager.GetString("FunctionIsReadOnly", SessionStateStrings.resourceCulture);
		}
	}

	// Token: 0x170018BF RID: 6335
	// (get) Token: 0x0600646A RID: 25706 RVA: 0x0020C526 File Offset: 0x0020A726
	internal static string FunctionNotRemovable
	{
		get
		{
			return SessionStateStrings.ResourceManager.GetString("FunctionNotRemovable", SessionStateStrings.resourceCulture);
		}
	}

	// Token: 0x170018C0 RID: 6336
	// (get) Token: 0x0600646B RID: 25707 RVA: 0x0020C53C File Offset: 0x0020A73C
	internal static string FunctionNotWritable
	{
		get
		{
			return SessionStateStrings.ResourceManager.GetString("FunctionNotWritable", SessionStateStrings.resourceCulture);
		}
	}

	// Token: 0x170018C1 RID: 6337
	// (get) Token: 0x0600646C RID: 25708 RVA: 0x0020C552 File Offset: 0x0020A752
	internal static string FunctionOverflow
	{
		get
		{
			return SessionStateStrings.ResourceManager.GetString("FunctionOverflow", SessionStateStrings.resourceCulture);
		}
	}

	// Token: 0x170018C2 RID: 6338
	// (get) Token: 0x0600646D RID: 25709 RVA: 0x0020C568 File Offset: 0x0020A768
	internal static string GetChildNameProviderException
	{
		get
		{
			return SessionStateStrings.ResourceManager.GetString("GetChildNameProviderException", SessionStateStrings.resourceCulture);
		}
	}

	// Token: 0x170018C3 RID: 6339
	// (get) Token: 0x0600646E RID: 25710 RVA: 0x0020C57E File Offset: 0x0020A77E
	internal static string GetChildNamesDynamicParametersProviderException
	{
		get
		{
			return SessionStateStrings.ResourceManager.GetString("GetChildNamesDynamicParametersProviderException", SessionStateStrings.resourceCulture);
		}
	}

	// Token: 0x170018C4 RID: 6340
	// (get) Token: 0x0600646F RID: 25711 RVA: 0x0020C594 File Offset: 0x0020A794
	internal static string GetChildNamesProviderException
	{
		get
		{
			return SessionStateStrings.ResourceManager.GetString("GetChildNamesProviderException", SessionStateStrings.resourceCulture);
		}
	}

	// Token: 0x170018C5 RID: 6341
	// (get) Token: 0x06006470 RID: 25712 RVA: 0x0020C5AA File Offset: 0x0020A7AA
	internal static string GetChildrenDynamicParametersProviderException
	{
		get
		{
			return SessionStateStrings.ResourceManager.GetString("GetChildrenDynamicParametersProviderException", SessionStateStrings.resourceCulture);
		}
	}

	// Token: 0x170018C6 RID: 6342
	// (get) Token: 0x06006471 RID: 25713 RVA: 0x0020C5C0 File Offset: 0x0020A7C0
	internal static string GetChildrenProviderException
	{
		get
		{
			return SessionStateStrings.ResourceManager.GetString("GetChildrenProviderException", SessionStateStrings.resourceCulture);
		}
	}

	// Token: 0x170018C7 RID: 6343
	// (get) Token: 0x06006472 RID: 25714 RVA: 0x0020C5D6 File Offset: 0x0020A7D6
	internal static string GetContent_TailAndHeadCannotCoexist
	{
		get
		{
			return SessionStateStrings.ResourceManager.GetString("GetContent_TailAndHeadCannotCoexist", SessionStateStrings.resourceCulture);
		}
	}

	// Token: 0x170018C8 RID: 6344
	// (get) Token: 0x06006473 RID: 25715 RVA: 0x0020C5EC File Offset: 0x0020A7EC
	internal static string GetContent_TailNotSupported
	{
		get
		{
			return SessionStateStrings.ResourceManager.GetString("GetContent_TailNotSupported", SessionStateStrings.resourceCulture);
		}
	}

	// Token: 0x170018C9 RID: 6345
	// (get) Token: 0x06006474 RID: 25716 RVA: 0x0020C602 File Offset: 0x0020A802
	internal static string GetContentReaderDynamicParametersProviderException
	{
		get
		{
			return SessionStateStrings.ResourceManager.GetString("GetContentReaderDynamicParametersProviderException", SessionStateStrings.resourceCulture);
		}
	}

	// Token: 0x170018CA RID: 6346
	// (get) Token: 0x06006475 RID: 25717 RVA: 0x0020C618 File Offset: 0x0020A818
	internal static string GetContentReaderProviderException
	{
		get
		{
			return SessionStateStrings.ResourceManager.GetString("GetContentReaderProviderException", SessionStateStrings.resourceCulture);
		}
	}

	// Token: 0x170018CB RID: 6347
	// (get) Token: 0x06006476 RID: 25718 RVA: 0x0020C62E File Offset: 0x0020A82E
	internal static string GetContentWriterDynamicParametersProviderException
	{
		get
		{
			return SessionStateStrings.ResourceManager.GetString("GetContentWriterDynamicParametersProviderException", SessionStateStrings.resourceCulture);
		}
	}

	// Token: 0x170018CC RID: 6348
	// (get) Token: 0x06006477 RID: 25719 RVA: 0x0020C644 File Offset: 0x0020A844
	internal static string GetContentWriterProviderException
	{
		get
		{
			return SessionStateStrings.ResourceManager.GetString("GetContentWriterProviderException", SessionStateStrings.resourceCulture);
		}
	}

	// Token: 0x170018CD RID: 6349
	// (get) Token: 0x06006478 RID: 25720 RVA: 0x0020C65A File Offset: 0x0020A85A
	internal static string GetItemDynamicParametersProviderException
	{
		get
		{
			return SessionStateStrings.ResourceManager.GetString("GetItemDynamicParametersProviderException", SessionStateStrings.resourceCulture);
		}
	}

	// Token: 0x170018CE RID: 6350
	// (get) Token: 0x06006479 RID: 25721 RVA: 0x0020C670 File Offset: 0x0020A870
	internal static string GetItemProviderException
	{
		get
		{
			return SessionStateStrings.ResourceManager.GetString("GetItemProviderException", SessionStateStrings.resourceCulture);
		}
	}

	// Token: 0x170018CF RID: 6351
	// (get) Token: 0x0600647A RID: 25722 RVA: 0x0020C686 File Offset: 0x0020A886
	internal static string GetParentPathProviderException
	{
		get
		{
			return SessionStateStrings.ResourceManager.GetString("GetParentPathProviderException", SessionStateStrings.resourceCulture);
		}
	}

	// Token: 0x170018D0 RID: 6352
	// (get) Token: 0x0600647B RID: 25723 RVA: 0x0020C69C File Offset: 0x0020A89C
	internal static string GetPropertyDynamicParametersProviderException
	{
		get
		{
			return SessionStateStrings.ResourceManager.GetString("GetPropertyDynamicParametersProviderException", SessionStateStrings.resourceCulture);
		}
	}

	// Token: 0x170018D1 RID: 6353
	// (get) Token: 0x0600647C RID: 25724 RVA: 0x0020C6B2 File Offset: 0x0020A8B2
	internal static string GetPropertyProviderException
	{
		get
		{
			return SessionStateStrings.ResourceManager.GetString("GetPropertyProviderException", SessionStateStrings.resourceCulture);
		}
	}

	// Token: 0x170018D2 RID: 6354
	// (get) Token: 0x0600647D RID: 25725 RVA: 0x0020C6C8 File Offset: 0x0020A8C8
	internal static string GetSecurityDescriptorProviderException
	{
		get
		{
			return SessionStateStrings.ResourceManager.GetString("GetSecurityDescriptorProviderException", SessionStateStrings.resourceCulture);
		}
	}

	// Token: 0x170018D3 RID: 6355
	// (get) Token: 0x0600647E RID: 25726 RVA: 0x0020C6DE File Offset: 0x0020A8DE
	internal static string GlobalScopeCannotRemove
	{
		get
		{
			return SessionStateStrings.ResourceManager.GetString("GlobalScopeCannotRemove", SessionStateStrings.resourceCulture);
		}
	}

	// Token: 0x170018D4 RID: 6356
	// (get) Token: 0x0600647F RID: 25727 RVA: 0x0020C6F4 File Offset: 0x0020A8F4
	internal static string HasChildItemsProviderException
	{
		get
		{
			return SessionStateStrings.ResourceManager.GetString("HasChildItemsProviderException", SessionStateStrings.resourceCulture);
		}
	}

	// Token: 0x170018D5 RID: 6357
	// (get) Token: 0x06006480 RID: 25728 RVA: 0x0020C70A File Offset: 0x0020A90A
	internal static string HomePathNotSet
	{
		get
		{
			return SessionStateStrings.ResourceManager.GetString("HomePathNotSet", SessionStateStrings.resourceCulture);
		}
	}

	// Token: 0x170018D6 RID: 6358
	// (get) Token: 0x06006481 RID: 25729 RVA: 0x0020C720 File Offset: 0x0020A920
	internal static string IContent_Clear_NotSupported
	{
		get
		{
			return SessionStateStrings.ResourceManager.GetString("IContent_Clear_NotSupported", SessionStateStrings.resourceCulture);
		}
	}

	// Token: 0x170018D7 RID: 6359
	// (get) Token: 0x06006482 RID: 25730 RVA: 0x0020C736 File Offset: 0x0020A936
	internal static string IContent_Seek_NotSupported
	{
		get
		{
			return SessionStateStrings.ResourceManager.GetString("IContent_Seek_NotSupported", SessionStateStrings.resourceCulture);
		}
	}

	// Token: 0x170018D8 RID: 6360
	// (get) Token: 0x06006483 RID: 25731 RVA: 0x0020C74C File Offset: 0x0020A94C
	internal static string IContentCmdletProvider_NotSupported
	{
		get
		{
			return SessionStateStrings.ResourceManager.GetString("IContentCmdletProvider_NotSupported", SessionStateStrings.resourceCulture);
		}
	}

	// Token: 0x170018D9 RID: 6361
	// (get) Token: 0x06006484 RID: 25732 RVA: 0x0020C762 File Offset: 0x0020A962
	internal static string IDynamicPropertyCmdletProvider_NotSupported
	{
		get
		{
			return SessionStateStrings.ResourceManager.GetString("IDynamicPropertyCmdletProvider_NotSupported", SessionStateStrings.resourceCulture);
		}
	}

	// Token: 0x170018DA RID: 6362
	// (get) Token: 0x06006485 RID: 25733 RVA: 0x0020C778 File Offset: 0x0020A978
	internal static string InitializeDefaultDrivesException
	{
		get
		{
			return SessionStateStrings.ResourceManager.GetString("InitializeDefaultDrivesException", SessionStateStrings.resourceCulture);
		}
	}

	// Token: 0x170018DB RID: 6363
	// (get) Token: 0x06006486 RID: 25734 RVA: 0x0020C78E File Offset: 0x0020A98E
	internal static string InvalidProviderInfo
	{
		get
		{
			return SessionStateStrings.ResourceManager.GetString("InvalidProviderInfo", SessionStateStrings.resourceCulture);
		}
	}

	// Token: 0x170018DC RID: 6364
	// (get) Token: 0x06006487 RID: 25735 RVA: 0x0020C7A4 File Offset: 0x0020A9A4
	internal static string InvalidProviderInfoNull
	{
		get
		{
			return SessionStateStrings.ResourceManager.GetString("InvalidProviderInfoNull", SessionStateStrings.resourceCulture);
		}
	}

	// Token: 0x170018DD RID: 6365
	// (get) Token: 0x06006488 RID: 25736 RVA: 0x0020C7BA File Offset: 0x0020A9BA
	internal static string InvokeDefaultActionDynamicParametersProviderException
	{
		get
		{
			return SessionStateStrings.ResourceManager.GetString("InvokeDefaultActionDynamicParametersProviderException", SessionStateStrings.resourceCulture);
		}
	}

	// Token: 0x170018DE RID: 6366
	// (get) Token: 0x06006489 RID: 25737 RVA: 0x0020C7D0 File Offset: 0x0020A9D0
	internal static string InvokeDefaultActionProviderException
	{
		get
		{
			return SessionStateStrings.ResourceManager.GetString("InvokeDefaultActionProviderException", SessionStateStrings.resourceCulture);
		}
	}

	// Token: 0x170018DF RID: 6367
	// (get) Token: 0x0600648A RID: 25738 RVA: 0x0020C7E6 File Offset: 0x0020A9E6
	internal static string IPropertyCmdletProvider_NotSupported
	{
		get
		{
			return SessionStateStrings.ResourceManager.GetString("IPropertyCmdletProvider_NotSupported", SessionStateStrings.resourceCulture);
		}
	}

	// Token: 0x170018E0 RID: 6368
	// (get) Token: 0x0600648B RID: 25739 RVA: 0x0020C7FC File Offset: 0x0020A9FC
	internal static string IsItemContainerDynamicParametersProviderException
	{
		get
		{
			return SessionStateStrings.ResourceManager.GetString("IsItemContainerDynamicParametersProviderException", SessionStateStrings.resourceCulture);
		}
	}

	// Token: 0x170018E1 RID: 6369
	// (get) Token: 0x0600648C RID: 25740 RVA: 0x0020C812 File Offset: 0x0020AA12
	internal static string IsItemContainerProviderException
	{
		get
		{
			return SessionStateStrings.ResourceManager.GetString("IsItemContainerProviderException", SessionStateStrings.resourceCulture);
		}
	}

	// Token: 0x170018E2 RID: 6370
	// (get) Token: 0x0600648D RID: 25741 RVA: 0x0020C828 File Offset: 0x0020AA28
	internal static string IsValidPathProviderException
	{
		get
		{
			return SessionStateStrings.ResourceManager.GetString("IsValidPathProviderException", SessionStateStrings.resourceCulture);
		}
	}

	// Token: 0x170018E3 RID: 6371
	// (get) Token: 0x0600648E RID: 25742 RVA: 0x0020C83E File Offset: 0x0020AA3E
	internal static string ItemCmdletProvider_NotSupported
	{
		get
		{
			return SessionStateStrings.ResourceManager.GetString("ItemCmdletProvider_NotSupported", SessionStateStrings.resourceCulture);
		}
	}

	// Token: 0x170018E4 RID: 6372
	// (get) Token: 0x0600648F RID: 25743 RVA: 0x0020C854 File Offset: 0x0020AA54
	internal static string ItemExistsDynamicParametersProviderException
	{
		get
		{
			return SessionStateStrings.ResourceManager.GetString("ItemExistsDynamicParametersProviderException", SessionStateStrings.resourceCulture);
		}
	}

	// Token: 0x170018E5 RID: 6373
	// (get) Token: 0x06006490 RID: 25744 RVA: 0x0020C86A File Offset: 0x0020AA6A
	internal static string ItemExistsProviderException
	{
		get
		{
			return SessionStateStrings.ResourceManager.GetString("ItemExistsProviderException", SessionStateStrings.resourceCulture);
		}
	}

	// Token: 0x170018E6 RID: 6374
	// (get) Token: 0x06006491 RID: 25745 RVA: 0x0020C880 File Offset: 0x0020AA80
	internal static string MakePathProviderException
	{
		get
		{
			return SessionStateStrings.ResourceManager.GetString("MakePathProviderException", SessionStateStrings.resourceCulture);
		}
	}

	// Token: 0x170018E7 RID: 6375
	// (get) Token: 0x06006492 RID: 25746 RVA: 0x0020C896 File Offset: 0x0020AA96
	internal static string MaxAliasCountDescription
	{
		get
		{
			return SessionStateStrings.ResourceManager.GetString("MaxAliasCountDescription", SessionStateStrings.resourceCulture);
		}
	}

	// Token: 0x170018E8 RID: 6376
	// (get) Token: 0x06006493 RID: 25747 RVA: 0x0020C8AC File Offset: 0x0020AAAC
	internal static string MaxDriveCountDescription
	{
		get
		{
			return SessionStateStrings.ResourceManager.GetString("MaxDriveCountDescription", SessionStateStrings.resourceCulture);
		}
	}

	// Token: 0x170018E9 RID: 6377
	// (get) Token: 0x06006494 RID: 25748 RVA: 0x0020C8C2 File Offset: 0x0020AAC2
	internal static string MaxErrorCountDescription
	{
		get
		{
			return SessionStateStrings.ResourceManager.GetString("MaxErrorCountDescription", SessionStateStrings.resourceCulture);
		}
	}

	// Token: 0x170018EA RID: 6378
	// (get) Token: 0x06006495 RID: 25749 RVA: 0x0020C8D8 File Offset: 0x0020AAD8
	internal static string MaxFunctionCountDescription
	{
		get
		{
			return SessionStateStrings.ResourceManager.GetString("MaxFunctionCountDescription", SessionStateStrings.resourceCulture);
		}
	}

	// Token: 0x170018EB RID: 6379
	// (get) Token: 0x06006496 RID: 25750 RVA: 0x0020C8EE File Offset: 0x0020AAEE
	internal static string MaxHistoryCountDescription
	{
		get
		{
			return SessionStateStrings.ResourceManager.GetString("MaxHistoryCountDescription", SessionStateStrings.resourceCulture);
		}
	}

	// Token: 0x170018EC RID: 6380
	// (get) Token: 0x06006497 RID: 25751 RVA: 0x0020C904 File Offset: 0x0020AB04
	internal static string MaxVariableCountDescription
	{
		get
		{
			return SessionStateStrings.ResourceManager.GetString("MaxVariableCountDescription", SessionStateStrings.resourceCulture);
		}
	}

	// Token: 0x170018ED RID: 6381
	// (get) Token: 0x06006498 RID: 25752 RVA: 0x0020C91A File Offset: 0x0020AB1A
	internal static string MoveItemDynamicParametersProviderException
	{
		get
		{
			return SessionStateStrings.ResourceManager.GetString("MoveItemDynamicParametersProviderException", SessionStateStrings.resourceCulture);
		}
	}

	// Token: 0x170018EE RID: 6382
	// (get) Token: 0x06006499 RID: 25753 RVA: 0x0020C930 File Offset: 0x0020AB30
	internal static string MoveItemOneDestination
	{
		get
		{
			return SessionStateStrings.ResourceManager.GetString("MoveItemOneDestination", SessionStateStrings.resourceCulture);
		}
	}

	// Token: 0x170018EF RID: 6383
	// (get) Token: 0x0600649A RID: 25754 RVA: 0x0020C946 File Offset: 0x0020AB46
	internal static string MoveItemPathMultipleDestinationNotContainer
	{
		get
		{
			return SessionStateStrings.ResourceManager.GetString("MoveItemPathMultipleDestinationNotContainer", SessionStateStrings.resourceCulture);
		}
	}

	// Token: 0x170018F0 RID: 6384
	// (get) Token: 0x0600649B RID: 25755 RVA: 0x0020C95C File Offset: 0x0020AB5C
	internal static string MoveItemProviderException
	{
		get
		{
			return SessionStateStrings.ResourceManager.GetString("MoveItemProviderException", SessionStateStrings.resourceCulture);
		}
	}

	// Token: 0x170018F1 RID: 6385
	// (get) Token: 0x0600649C RID: 25756 RVA: 0x0020C972 File Offset: 0x0020AB72
	internal static string MoveItemSourceAndDestinationNotSameProvider
	{
		get
		{
			return SessionStateStrings.ResourceManager.GetString("MoveItemSourceAndDestinationNotSameProvider", SessionStateStrings.resourceCulture);
		}
	}

	// Token: 0x170018F2 RID: 6386
	// (get) Token: 0x0600649D RID: 25757 RVA: 0x0020C988 File Offset: 0x0020AB88
	internal static string MovePropertyDestinationResolveToSingle
	{
		get
		{
			return SessionStateStrings.ResourceManager.GetString("MovePropertyDestinationResolveToSingle", SessionStateStrings.resourceCulture);
		}
	}

	// Token: 0x170018F3 RID: 6387
	// (get) Token: 0x0600649E RID: 25758 RVA: 0x0020C99E File Offset: 0x0020AB9E
	internal static string MovePropertyDynamicParametersProviderException
	{
		get
		{
			return SessionStateStrings.ResourceManager.GetString("MovePropertyDynamicParametersProviderException", SessionStateStrings.resourceCulture);
		}
	}

	// Token: 0x170018F4 RID: 6388
	// (get) Token: 0x0600649F RID: 25759 RVA: 0x0020C9B4 File Offset: 0x0020ABB4
	internal static string MovePropertyProviderException
	{
		get
		{
			return SessionStateStrings.ResourceManager.GetString("MovePropertyProviderException", SessionStateStrings.resourceCulture);
		}
	}

	// Token: 0x170018F5 RID: 6389
	// (get) Token: 0x060064A0 RID: 25760 RVA: 0x0020C9CA File Offset: 0x0020ABCA
	internal static string MustBeFileSystemPath
	{
		get
		{
			return SessionStateStrings.ResourceManager.GetString("MustBeFileSystemPath", SessionStateStrings.resourceCulture);
		}
	}

	// Token: 0x170018F6 RID: 6390
	// (get) Token: 0x060064A1 RID: 25761 RVA: 0x0020C9E0 File Offset: 0x0020ABE0
	internal static string NamedCommandIsPrivate
	{
		get
		{
			return SessionStateStrings.ResourceManager.GetString("NamedCommandIsPrivate", SessionStateStrings.resourceCulture);
		}
	}

	// Token: 0x170018F7 RID: 6391
	// (get) Token: 0x060064A2 RID: 25762 RVA: 0x0020C9F6 File Offset: 0x0020ABF6
	internal static string NavigationCmdletProvider_NotSupported
	{
		get
		{
			return SessionStateStrings.ResourceManager.GetString("NavigationCmdletProvider_NotSupported", SessionStateStrings.resourceCulture);
		}
	}

	// Token: 0x170018F8 RID: 6392
	// (get) Token: 0x060064A3 RID: 25763 RVA: 0x0020CA0C File Offset: 0x0020AC0C
	internal static string NewDriveCredentials_NotSupported
	{
		get
		{
			return SessionStateStrings.ResourceManager.GetString("NewDriveCredentials_NotSupported", SessionStateStrings.resourceCulture);
		}
	}

	// Token: 0x170018F9 RID: 6393
	// (get) Token: 0x060064A4 RID: 25764 RVA: 0x0020CA22 File Offset: 0x0020AC22
	internal static string NewDriveDynamicParametersProviderException
	{
		get
		{
			return SessionStateStrings.ResourceManager.GetString("NewDriveDynamicParametersProviderException", SessionStateStrings.resourceCulture);
		}
	}

	// Token: 0x170018FA RID: 6394
	// (get) Token: 0x060064A5 RID: 25765 RVA: 0x0020CA38 File Offset: 0x0020AC38
	internal static string NewDriveProviderException
	{
		get
		{
			return SessionStateStrings.ResourceManager.GetString("NewDriveProviderException", SessionStateStrings.resourceCulture);
		}
	}

	// Token: 0x170018FB RID: 6395
	// (get) Token: 0x060064A6 RID: 25766 RVA: 0x0020CA4E File Offset: 0x0020AC4E
	internal static string NewDriveProviderFailed
	{
		get
		{
			return SessionStateStrings.ResourceManager.GetString("NewDriveProviderFailed", SessionStateStrings.resourceCulture);
		}
	}

	// Token: 0x170018FC RID: 6396
	// (get) Token: 0x060064A7 RID: 25767 RVA: 0x0020CA64 File Offset: 0x0020AC64
	internal static string NewItemAlreadyExists
	{
		get
		{
			return SessionStateStrings.ResourceManager.GetString("NewItemAlreadyExists", SessionStateStrings.resourceCulture);
		}
	}

	// Token: 0x170018FD RID: 6397
	// (get) Token: 0x060064A8 RID: 25768 RVA: 0x0020CA7A File Offset: 0x0020AC7A
	internal static string NewItemCannotModifyDriveRoot
	{
		get
		{
			return SessionStateStrings.ResourceManager.GetString("NewItemCannotModifyDriveRoot", SessionStateStrings.resourceCulture);
		}
	}

	// Token: 0x170018FE RID: 6398
	// (get) Token: 0x060064A9 RID: 25769 RVA: 0x0020CA90 File Offset: 0x0020AC90
	internal static string NewItemDriveNameConflict
	{
		get
		{
			return SessionStateStrings.ResourceManager.GetString("NewItemDriveNameConflict", SessionStateStrings.resourceCulture);
		}
	}

	// Token: 0x170018FF RID: 6399
	// (get) Token: 0x060064AA RID: 25770 RVA: 0x0020CAA6 File Offset: 0x0020ACA6
	internal static string NewItemDynamicParametersProviderException
	{
		get
		{
			return SessionStateStrings.ResourceManager.GetString("NewItemDynamicParametersProviderException", SessionStateStrings.resourceCulture);
		}
	}

	// Token: 0x17001900 RID: 6400
	// (get) Token: 0x060064AB RID: 25771 RVA: 0x0020CABC File Offset: 0x0020ACBC
	internal static string NewItemProviderException
	{
		get
		{
			return SessionStateStrings.ResourceManager.GetString("NewItemProviderException", SessionStateStrings.resourceCulture);
		}
	}

	// Token: 0x17001901 RID: 6401
	// (get) Token: 0x060064AC RID: 25772 RVA: 0x0020CAD2 File Offset: 0x0020ACD2
	internal static string NewItemProviderNameConflict
	{
		get
		{
			return SessionStateStrings.ResourceManager.GetString("NewItemProviderNameConflict", SessionStateStrings.resourceCulture);
		}
	}

	// Token: 0x17001902 RID: 6402
	// (get) Token: 0x060064AD RID: 25773 RVA: 0x0020CAE8 File Offset: 0x0020ACE8
	internal static string NewItemTypeDrive
	{
		get
		{
			return SessionStateStrings.ResourceManager.GetString("NewItemTypeDrive", SessionStateStrings.resourceCulture);
		}
	}

	// Token: 0x17001903 RID: 6403
	// (get) Token: 0x060064AE RID: 25774 RVA: 0x0020CAFE File Offset: 0x0020ACFE
	internal static string NewItemTypeProvider
	{
		get
		{
			return SessionStateStrings.ResourceManager.GetString("NewItemTypeProvider", SessionStateStrings.resourceCulture);
		}
	}

	// Token: 0x17001904 RID: 6404
	// (get) Token: 0x060064AF RID: 25775 RVA: 0x0020CB14 File Offset: 0x0020AD14
	internal static string NewItemValueMustBeProviderInfo
	{
		get
		{
			return SessionStateStrings.ResourceManager.GetString("NewItemValueMustBeProviderInfo", SessionStateStrings.resourceCulture);
		}
	}

	// Token: 0x17001905 RID: 6405
	// (get) Token: 0x060064B0 RID: 25776 RVA: 0x0020CB2A File Offset: 0x0020AD2A
	internal static string NewItemValueMustBePSDriveInfo
	{
		get
		{
			return SessionStateStrings.ResourceManager.GetString("NewItemValueMustBePSDriveInfo", SessionStateStrings.resourceCulture);
		}
	}

	// Token: 0x17001906 RID: 6406
	// (get) Token: 0x060064B1 RID: 25777 RVA: 0x0020CB40 File Offset: 0x0020AD40
	internal static string NewItemValueNotSpecified
	{
		get
		{
			return SessionStateStrings.ResourceManager.GetString("NewItemValueNotSpecified", SessionStateStrings.resourceCulture);
		}
	}

	// Token: 0x17001907 RID: 6407
	// (get) Token: 0x060064B2 RID: 25778 RVA: 0x0020CB56 File Offset: 0x0020AD56
	internal static string NewPropertyDynamicParametersProviderException
	{
		get
		{
			return SessionStateStrings.ResourceManager.GetString("NewPropertyDynamicParametersProviderException", SessionStateStrings.resourceCulture);
		}
	}

	// Token: 0x17001908 RID: 6408
	// (get) Token: 0x060064B3 RID: 25779 RVA: 0x0020CB6C File Offset: 0x0020AD6C
	internal static string NewPropertyProviderException
	{
		get
		{
			return SessionStateStrings.ResourceManager.GetString("NewPropertyProviderException", SessionStateStrings.resourceCulture);
		}
	}

	// Token: 0x17001909 RID: 6409
	// (get) Token: 0x060064B4 RID: 25780 RVA: 0x0020CB82 File Offset: 0x0020AD82
	internal static string NormalizeRelativePathLengthLessThanBase
	{
		get
		{
			return SessionStateStrings.ResourceManager.GetString("NormalizeRelativePathLengthLessThanBase", SessionStateStrings.resourceCulture);
		}
	}

	// Token: 0x1700190A RID: 6410
	// (get) Token: 0x060064B5 RID: 25781 RVA: 0x0020CB98 File Offset: 0x0020AD98
	internal static string NormalizeRelativePathOutsideBase
	{
		get
		{
			return SessionStateStrings.ResourceManager.GetString("NormalizeRelativePathOutsideBase", SessionStateStrings.resourceCulture);
		}
	}

	// Token: 0x1700190B RID: 6411
	// (get) Token: 0x060064B6 RID: 25782 RVA: 0x0020CBAE File Offset: 0x0020ADAE
	internal static string NormalizeRelativePathProviderException
	{
		get
		{
			return SessionStateStrings.ResourceManager.GetString("NormalizeRelativePathProviderException", SessionStateStrings.resourceCulture);
		}
	}

	// Token: 0x1700190C RID: 6412
	// (get) Token: 0x060064B7 RID: 25783 RVA: 0x0020CBC4 File Offset: 0x0020ADC4
	internal static string NotProviderQualifiedPath
	{
		get
		{
			return SessionStateStrings.ResourceManager.GetString("NotProviderQualifiedPath", SessionStateStrings.resourceCulture);
		}
	}

	// Token: 0x1700190D RID: 6413
	// (get) Token: 0x060064B8 RID: 25784 RVA: 0x0020CBDA File Offset: 0x0020ADDA
	internal static string OnlyAbleToComparePSDriveInfo
	{
		get
		{
			return SessionStateStrings.ResourceManager.GetString("OnlyAbleToComparePSDriveInfo", SessionStateStrings.resourceCulture);
		}
	}

	// Token: 0x1700190E RID: 6414
	// (get) Token: 0x060064B9 RID: 25785 RVA: 0x0020CBF0 File Offset: 0x0020ADF0
	internal static string OutputStreamingNotEnabled
	{
		get
		{
			return SessionStateStrings.ResourceManager.GetString("OutputStreamingNotEnabled", SessionStateStrings.resourceCulture);
		}
	}

	// Token: 0x1700190F RID: 6415
	// (get) Token: 0x060064BA RID: 25786 RVA: 0x0020CC06 File Offset: 0x0020AE06
	internal static string PathNotFound
	{
		get
		{
			return SessionStateStrings.ResourceManager.GetString("PathNotFound", SessionStateStrings.resourceCulture);
		}
	}

	// Token: 0x17001910 RID: 6416
	// (get) Token: 0x060064BB RID: 25787 RVA: 0x0020CC1C File Offset: 0x0020AE1C
	internal static string PathResolvedToMultiple
	{
		get
		{
			return SessionStateStrings.ResourceManager.GetString("PathResolvedToMultiple", SessionStateStrings.resourceCulture);
		}
	}

	// Token: 0x17001911 RID: 6417
	// (get) Token: 0x060064BC RID: 25788 RVA: 0x0020CC32 File Offset: 0x0020AE32
	internal static string ProviderCannotBeUsedAsVariable
	{
		get
		{
			return SessionStateStrings.ResourceManager.GetString("ProviderCannotBeUsedAsVariable", SessionStateStrings.resourceCulture);
		}
	}

	// Token: 0x17001912 RID: 6418
	// (get) Token: 0x060064BD RID: 25789 RVA: 0x0020CC48 File Offset: 0x0020AE48
	internal static string ProviderContentCloseError
	{
		get
		{
			return SessionStateStrings.ResourceManager.GetString("ProviderContentCloseError", SessionStateStrings.resourceCulture);
		}
	}

	// Token: 0x17001913 RID: 6419
	// (get) Token: 0x060064BE RID: 25790 RVA: 0x0020CC5E File Offset: 0x0020AE5E
	internal static string ProviderContentReadError
	{
		get
		{
			return SessionStateStrings.ResourceManager.GetString("ProviderContentReadError", SessionStateStrings.resourceCulture);
		}
	}

	// Token: 0x17001914 RID: 6420
	// (get) Token: 0x060064BF RID: 25791 RVA: 0x0020CC74 File Offset: 0x0020AE74
	internal static string ProviderContentWriteError
	{
		get
		{
			return SessionStateStrings.ResourceManager.GetString("ProviderContentWriteError", SessionStateStrings.resourceCulture);
		}
	}

	// Token: 0x17001915 RID: 6421
	// (get) Token: 0x060064C0 RID: 25792 RVA: 0x0020CC8A File Offset: 0x0020AE8A
	internal static string ProviderCtorException
	{
		get
		{
			return SessionStateStrings.ResourceManager.GetString("ProviderCtorException", SessionStateStrings.resourceCulture);
		}
	}

	// Token: 0x17001916 RID: 6422
	// (get) Token: 0x060064C1 RID: 25793 RVA: 0x0020CCA0 File Offset: 0x0020AEA0
	internal static string ProviderDriveDescription
	{
		get
		{
			return SessionStateStrings.ResourceManager.GetString("ProviderDriveDescription", SessionStateStrings.resourceCulture);
		}
	}

	// Token: 0x17001917 RID: 6423
	// (get) Token: 0x060064C2 RID: 25794 RVA: 0x0020CCB6 File Offset: 0x0020AEB6
	internal static string ProviderImplementationInconsistent
	{
		get
		{
			return SessionStateStrings.ResourceManager.GetString("ProviderImplementationInconsistent", SessionStateStrings.resourceCulture);
		}
	}

	// Token: 0x17001918 RID: 6424
	// (get) Token: 0x060064C3 RID: 25795 RVA: 0x0020CCCC File Offset: 0x0020AECC
	internal static string ProviderNameAmbiguous
	{
		get
		{
			return SessionStateStrings.ResourceManager.GetString("ProviderNameAmbiguous", SessionStateStrings.resourceCulture);
		}
	}

	// Token: 0x17001919 RID: 6425
	// (get) Token: 0x060064C4 RID: 25796 RVA: 0x0020CCE2 File Offset: 0x0020AEE2
	internal static string ProviderNameNotValid
	{
		get
		{
			return SessionStateStrings.ResourceManager.GetString("ProviderNameNotValid", SessionStateStrings.resourceCulture);
		}
	}

	// Token: 0x1700191A RID: 6426
	// (get) Token: 0x060064C5 RID: 25797 RVA: 0x0020CCF8 File Offset: 0x0020AEF8
	internal static string ProviderNotFound
	{
		get
		{
			return SessionStateStrings.ResourceManager.GetString("ProviderNotFound", SessionStateStrings.resourceCulture);
		}
	}

	// Token: 0x1700191B RID: 6427
	// (get) Token: 0x060064C6 RID: 25798 RVA: 0x0020CD0E File Offset: 0x0020AF0E
	internal static string ProviderNotFoundBadFormat
	{
		get
		{
			return SessionStateStrings.ResourceManager.GetString("ProviderNotFoundBadFormat", SessionStateStrings.resourceCulture);
		}
	}

	// Token: 0x1700191C RID: 6428
	// (get) Token: 0x060064C7 RID: 25799 RVA: 0x0020CD24 File Offset: 0x0020AF24
	internal static string ProviderNotFoundInAssembly
	{
		get
		{
			return SessionStateStrings.ResourceManager.GetString("ProviderNotFoundInAssembly", SessionStateStrings.resourceCulture);
		}
	}

	// Token: 0x1700191D RID: 6429
	// (get) Token: 0x060064C8 RID: 25800 RVA: 0x0020CD3A File Offset: 0x0020AF3A
	internal static string ProviderProviderCannotCreateProvider
	{
		get
		{
			return SessionStateStrings.ResourceManager.GetString("ProviderProviderCannotCreateProvider", SessionStateStrings.resourceCulture);
		}
	}

	// Token: 0x1700191E RID: 6430
	// (get) Token: 0x060064C9 RID: 25801 RVA: 0x0020CD50 File Offset: 0x0020AF50
	internal static string ProviderProviderCannotRemoveProvider
	{
		get
		{
			return SessionStateStrings.ResourceManager.GetString("ProviderProviderCannotRemoveProvider", SessionStateStrings.resourceCulture);
		}
	}

	// Token: 0x1700191F RID: 6431
	// (get) Token: 0x060064CA RID: 25802 RVA: 0x0020CD66 File Offset: 0x0020AF66
	internal static string ProviderProviderPathFormatException
	{
		get
		{
			return SessionStateStrings.ResourceManager.GetString("ProviderProviderPathFormatException", SessionStateStrings.resourceCulture);
		}
	}

	// Token: 0x17001920 RID: 6432
	// (get) Token: 0x060064CB RID: 25803 RVA: 0x0020CD7C File Offset: 0x0020AF7C
	internal static string ProviderSeekError
	{
		get
		{
			return SessionStateStrings.ResourceManager.GetString("ProviderSeekError", SessionStateStrings.resourceCulture);
		}
	}

	// Token: 0x17001921 RID: 6433
	// (get) Token: 0x060064CC RID: 25804 RVA: 0x0020CD92 File Offset: 0x0020AF92
	internal static string ProviderStartException
	{
		get
		{
			return SessionStateStrings.ResourceManager.GetString("ProviderStartException", SessionStateStrings.resourceCulture);
		}
	}

	// Token: 0x17001922 RID: 6434
	// (get) Token: 0x060064CD RID: 25805 RVA: 0x0020CDA8 File Offset: 0x0020AFA8
	internal static string ProviderVariableSyntaxInvalid
	{
		get
		{
			return SessionStateStrings.ResourceManager.GetString("ProviderVariableSyntaxInvalid", SessionStateStrings.resourceCulture);
		}
	}

	// Token: 0x17001923 RID: 6435
	// (get) Token: 0x060064CE RID: 25806 RVA: 0x0020CDBE File Offset: 0x0020AFBE
	internal static string RemoveDriveProviderException
	{
		get
		{
			return SessionStateStrings.ResourceManager.GetString("RemoveDriveProviderException", SessionStateStrings.resourceCulture);
		}
	}

	// Token: 0x17001924 RID: 6436
	// (get) Token: 0x060064CF RID: 25807 RVA: 0x0020CDD4 File Offset: 0x0020AFD4
	internal static string RemoveDriveRoot
	{
		get
		{
			return SessionStateStrings.ResourceManager.GetString("RemoveDriveRoot", SessionStateStrings.resourceCulture);
		}
	}

	// Token: 0x17001925 RID: 6437
	// (get) Token: 0x060064D0 RID: 25808 RVA: 0x0020CDEA File Offset: 0x0020AFEA
	internal static string RemoveDrivesBeforeRemovingProvider
	{
		get
		{
			return SessionStateStrings.ResourceManager.GetString("RemoveDrivesBeforeRemovingProvider", SessionStateStrings.resourceCulture);
		}
	}

	// Token: 0x17001926 RID: 6438
	// (get) Token: 0x060064D1 RID: 25809 RVA: 0x0020CE00 File Offset: 0x0020B000
	internal static string RemoveItemDynamicParametersProviderException
	{
		get
		{
			return SessionStateStrings.ResourceManager.GetString("RemoveItemDynamicParametersProviderException", SessionStateStrings.resourceCulture);
		}
	}

	// Token: 0x17001927 RID: 6439
	// (get) Token: 0x060064D2 RID: 25810 RVA: 0x0020CE16 File Offset: 0x0020B016
	internal static string RemoveItemProviderException
	{
		get
		{
			return SessionStateStrings.ResourceManager.GetString("RemoveItemProviderException", SessionStateStrings.resourceCulture);
		}
	}

	// Token: 0x17001928 RID: 6440
	// (get) Token: 0x060064D3 RID: 25811 RVA: 0x0020CE2C File Offset: 0x0020B02C
	internal static string RemovePropertyDynamicParametersProviderException
	{
		get
		{
			return SessionStateStrings.ResourceManager.GetString("RemovePropertyDynamicParametersProviderException", SessionStateStrings.resourceCulture);
		}
	}

	// Token: 0x17001929 RID: 6441
	// (get) Token: 0x060064D4 RID: 25812 RVA: 0x0020CE42 File Offset: 0x0020B042
	internal static string RemovePropertyProviderException
	{
		get
		{
			return SessionStateStrings.ResourceManager.GetString("RemovePropertyProviderException", SessionStateStrings.resourceCulture);
		}
	}

	// Token: 0x1700192A RID: 6442
	// (get) Token: 0x060064D5 RID: 25813 RVA: 0x0020CE58 File Offset: 0x0020B058
	internal static string RenameItemDoesntExist
	{
		get
		{
			return SessionStateStrings.ResourceManager.GetString("RenameItemDoesntExist", SessionStateStrings.resourceCulture);
		}
	}

	// Token: 0x1700192B RID: 6443
	// (get) Token: 0x060064D6 RID: 25814 RVA: 0x0020CE6E File Offset: 0x0020B06E
	internal static string RenameItemDynamicParametersProviderException
	{
		get
		{
			return SessionStateStrings.ResourceManager.GetString("RenameItemDynamicParametersProviderException", SessionStateStrings.resourceCulture);
		}
	}

	// Token: 0x1700192C RID: 6444
	// (get) Token: 0x060064D7 RID: 25815 RVA: 0x0020CE84 File Offset: 0x0020B084
	internal static string RenameItemProviderException
	{
		get
		{
			return SessionStateStrings.ResourceManager.GetString("RenameItemProviderException", SessionStateStrings.resourceCulture);
		}
	}

	// Token: 0x1700192D RID: 6445
	// (get) Token: 0x060064D8 RID: 25816 RVA: 0x0020CE9A File Offset: 0x0020B09A
	internal static string RenameMultipleItemError
	{
		get
		{
			return SessionStateStrings.ResourceManager.GetString("RenameMultipleItemError", SessionStateStrings.resourceCulture);
		}
	}

	// Token: 0x1700192E RID: 6446
	// (get) Token: 0x060064D9 RID: 25817 RVA: 0x0020CEB0 File Offset: 0x0020B0B0
	internal static string RenamePropertyDynamicParametersProviderException
	{
		get
		{
			return SessionStateStrings.ResourceManager.GetString("RenamePropertyDynamicParametersProviderException", SessionStateStrings.resourceCulture);
		}
	}

	// Token: 0x1700192F RID: 6447
	// (get) Token: 0x060064DA RID: 25818 RVA: 0x0020CEC6 File Offset: 0x0020B0C6
	internal static string RenamePropertyProviderException
	{
		get
		{
			return SessionStateStrings.ResourceManager.GetString("RenamePropertyProviderException", SessionStateStrings.resourceCulture);
		}
	}

	// Token: 0x17001930 RID: 6448
	// (get) Token: 0x060064DB RID: 25819 RVA: 0x0020CEDC File Offset: 0x0020B0DC
	internal static string ResourceIsPrivate
	{
		get
		{
			return SessionStateStrings.ResourceManager.GetString("ResourceIsPrivate", SessionStateStrings.resourceCulture);
		}
	}

	// Token: 0x17001931 RID: 6449
	// (get) Token: 0x060064DC RID: 25820 RVA: 0x0020CEF2 File Offset: 0x0020B0F2
	internal static string ScopeDepthOverflow
	{
		get
		{
			return SessionStateStrings.ResourceManager.GetString("ScopeDepthOverflow", SessionStateStrings.resourceCulture);
		}
	}

	// Token: 0x17001932 RID: 6450
	// (get) Token: 0x060064DD RID: 25821 RVA: 0x0020CF08 File Offset: 0x0020B108
	internal static string ScopedFunctionMustHaveName
	{
		get
		{
			return SessionStateStrings.ResourceManager.GetString("ScopedFunctionMustHaveName", SessionStateStrings.resourceCulture);
		}
	}

	// Token: 0x17001933 RID: 6451
	// (get) Token: 0x060064DE RID: 25822 RVA: 0x0020CF1E File Offset: 0x0020B11E
	internal static string ScopeIDExceedsAvailableScopes
	{
		get
		{
			return SessionStateStrings.ResourceManager.GetString("ScopeIDExceedsAvailableScopes", SessionStateStrings.resourceCulture);
		}
	}

	// Token: 0x17001934 RID: 6452
	// (get) Token: 0x060064DF RID: 25823 RVA: 0x0020CF34 File Offset: 0x0020B134
	internal static string SecurityDescriptorInterfaceNotSupported
	{
		get
		{
			return SessionStateStrings.ResourceManager.GetString("SecurityDescriptorInterfaceNotSupported", SessionStateStrings.resourceCulture);
		}
	}

	// Token: 0x17001935 RID: 6453
	// (get) Token: 0x060064E0 RID: 25824 RVA: 0x0020CF4A File Offset: 0x0020B14A
	internal static string SetItemDynamicParametersProviderException
	{
		get
		{
			return SessionStateStrings.ResourceManager.GetString("SetItemDynamicParametersProviderException", SessionStateStrings.resourceCulture);
		}
	}

	// Token: 0x17001936 RID: 6454
	// (get) Token: 0x060064E1 RID: 25825 RVA: 0x0020CF60 File Offset: 0x0020B160
	internal static string SetItemProviderException
	{
		get
		{
			return SessionStateStrings.ResourceManager.GetString("SetItemProviderException", SessionStateStrings.resourceCulture);
		}
	}

	// Token: 0x17001937 RID: 6455
	// (get) Token: 0x060064E2 RID: 25826 RVA: 0x0020CF76 File Offset: 0x0020B176
	internal static string SetPropertyDynamicParametersProviderException
	{
		get
		{
			return SessionStateStrings.ResourceManager.GetString("SetPropertyDynamicParametersProviderException", SessionStateStrings.resourceCulture);
		}
	}

	// Token: 0x17001938 RID: 6456
	// (get) Token: 0x060064E3 RID: 25827 RVA: 0x0020CF8C File Offset: 0x0020B18C
	internal static string SetPropertyProviderException
	{
		get
		{
			return SessionStateStrings.ResourceManager.GetString("SetPropertyProviderException", SessionStateStrings.resourceCulture);
		}
	}

	// Token: 0x17001939 RID: 6457
	// (get) Token: 0x060064E4 RID: 25828 RVA: 0x0020CFA2 File Offset: 0x0020B1A2
	internal static string SetSecurityDescriptorProviderException
	{
		get
		{
			return SessionStateStrings.ResourceManager.GetString("SetSecurityDescriptorProviderException", SessionStateStrings.resourceCulture);
		}
	}

	// Token: 0x1700193A RID: 6458
	// (get) Token: 0x060064E5 RID: 25829 RVA: 0x0020CFB8 File Offset: 0x0020B1B8
	internal static string StackNameResolvedToMultiple
	{
		get
		{
			return SessionStateStrings.ResourceManager.GetString("StackNameResolvedToMultiple", SessionStateStrings.resourceCulture);
		}
	}

	// Token: 0x1700193B RID: 6459
	// (get) Token: 0x060064E6 RID: 25830 RVA: 0x0020CFCE File Offset: 0x0020B1CE
	internal static string StackNotFound
	{
		get
		{
			return SessionStateStrings.ResourceManager.GetString("StackNotFound", SessionStateStrings.resourceCulture);
		}
	}

	// Token: 0x1700193C RID: 6460
	// (get) Token: 0x060064E7 RID: 25831 RVA: 0x0020CFE4 File Offset: 0x0020B1E4
	internal static string StartDynamicParmatersProviderException
	{
		get
		{
			return SessionStateStrings.ResourceManager.GetString("StartDynamicParmatersProviderException", SessionStateStrings.resourceCulture);
		}
	}

	// Token: 0x1700193D RID: 6461
	// (get) Token: 0x060064E8 RID: 25832 RVA: 0x0020CFFA File Offset: 0x0020B1FA
	internal static string TraceSourceNotFound
	{
		get
		{
			return SessionStateStrings.ResourceManager.GetString("TraceSourceNotFound", SessionStateStrings.resourceCulture);
		}
	}

	// Token: 0x1700193E RID: 6462
	// (get) Token: 0x060064E9 RID: 25833 RVA: 0x0020D010 File Offset: 0x0020B210
	internal static string Transactions_NotSupported
	{
		get
		{
			return SessionStateStrings.ResourceManager.GetString("Transactions_NotSupported", SessionStateStrings.resourceCulture);
		}
	}

	// Token: 0x1700193F RID: 6463
	// (get) Token: 0x060064EA RID: 25834 RVA: 0x0020D026 File Offset: 0x0020B226
	internal static string VariableAllScopeOptionCannotBeRemoved
	{
		get
		{
			return SessionStateStrings.ResourceManager.GetString("VariableAllScopeOptionCannotBeRemoved", SessionStateStrings.resourceCulture);
		}
	}

	// Token: 0x17001940 RID: 6464
	// (get) Token: 0x060064EB RID: 25835 RVA: 0x0020D03C File Offset: 0x0020B23C
	internal static string VariableAlreadyExists
	{
		get
		{
			return SessionStateStrings.ResourceManager.GetString("VariableAlreadyExists", SessionStateStrings.resourceCulture);
		}
	}

	// Token: 0x17001941 RID: 6465
	// (get) Token: 0x060064EC RID: 25836 RVA: 0x0020D052 File Offset: 0x0020B252
	internal static string VariableCannotBeMadeConstant
	{
		get
		{
			return SessionStateStrings.ResourceManager.GetString("VariableCannotBeMadeConstant", SessionStateStrings.resourceCulture);
		}
	}

	// Token: 0x17001942 RID: 6466
	// (get) Token: 0x060064ED RID: 25837 RVA: 0x0020D068 File Offset: 0x0020B268
	internal static string VariableDriveDescription
	{
		get
		{
			return SessionStateStrings.ResourceManager.GetString("VariableDriveDescription", SessionStateStrings.resourceCulture);
		}
	}

	// Token: 0x17001943 RID: 6467
	// (get) Token: 0x060064EE RID: 25838 RVA: 0x0020D07E File Offset: 0x0020B27E
	internal static string VariableIsConstant
	{
		get
		{
			return SessionStateStrings.ResourceManager.GetString("VariableIsConstant", SessionStateStrings.resourceCulture);
		}
	}

	// Token: 0x17001944 RID: 6468
	// (get) Token: 0x060064EF RID: 25839 RVA: 0x0020D094 File Offset: 0x0020B294
	internal static string VariableIsPrivate
	{
		get
		{
			return SessionStateStrings.ResourceManager.GetString("VariableIsPrivate", SessionStateStrings.resourceCulture);
		}
	}

	// Token: 0x17001945 RID: 6469
	// (get) Token: 0x060064F0 RID: 25840 RVA: 0x0020D0AA File Offset: 0x0020B2AA
	internal static string VariableNotFound
	{
		get
		{
			return SessionStateStrings.ResourceManager.GetString("VariableNotFound", SessionStateStrings.resourceCulture);
		}
	}

	// Token: 0x17001946 RID: 6470
	// (get) Token: 0x060064F1 RID: 25841 RVA: 0x0020D0C0 File Offset: 0x0020B2C0
	internal static string VariableNotRemovable
	{
		get
		{
			return SessionStateStrings.ResourceManager.GetString("VariableNotRemovable", SessionStateStrings.resourceCulture);
		}
	}

	// Token: 0x17001947 RID: 6471
	// (get) Token: 0x060064F2 RID: 25842 RVA: 0x0020D0D6 File Offset: 0x0020B2D6
	internal static string VariableNotRemovableRare
	{
		get
		{
			return SessionStateStrings.ResourceManager.GetString("VariableNotRemovableRare", SessionStateStrings.resourceCulture);
		}
	}

	// Token: 0x17001948 RID: 6472
	// (get) Token: 0x060064F3 RID: 25843 RVA: 0x0020D0EC File Offset: 0x0020B2EC
	internal static string VariableNotRemovableSystem
	{
		get
		{
			return SessionStateStrings.ResourceManager.GetString("VariableNotRemovableSystem", SessionStateStrings.resourceCulture);
		}
	}

	// Token: 0x17001949 RID: 6473
	// (get) Token: 0x060064F4 RID: 25844 RVA: 0x0020D102 File Offset: 0x0020B302
	internal static string VariableNotWritable
	{
		get
		{
			return SessionStateStrings.ResourceManager.GetString("VariableNotWritable", SessionStateStrings.resourceCulture);
		}
	}

	// Token: 0x1700194A RID: 6474
	// (get) Token: 0x060064F5 RID: 25845 RVA: 0x0020D118 File Offset: 0x0020B318
	internal static string VariableNotWritableRare
	{
		get
		{
			return SessionStateStrings.ResourceManager.GetString("VariableNotWritableRare", SessionStateStrings.resourceCulture);
		}
	}

	// Token: 0x1700194B RID: 6475
	// (get) Token: 0x060064F6 RID: 25846 RVA: 0x0020D12E File Offset: 0x0020B32E
	internal static string VariableOptionsNotSettable
	{
		get
		{
			return SessionStateStrings.ResourceManager.GetString("VariableOptionsNotSettable", SessionStateStrings.resourceCulture);
		}
	}

	// Token: 0x1700194C RID: 6476
	// (get) Token: 0x060064F7 RID: 25847 RVA: 0x0020D144 File Offset: 0x0020B344
	internal static string VariableOverflow
	{
		get
		{
			return SessionStateStrings.ResourceManager.GetString("VariableOverflow", SessionStateStrings.resourceCulture);
		}
	}

	// Token: 0x1700194D RID: 6477
	// (get) Token: 0x060064F8 RID: 25848 RVA: 0x0020D15A File Offset: 0x0020B35A
	internal static string VariablePathResolvedToMultiple
	{
		get
		{
			return SessionStateStrings.ResourceManager.GetString("VariablePathResolvedToMultiple", SessionStateStrings.resourceCulture);
		}
	}

	// Token: 0x0400324F RID: 12879
	private static ResourceManager resourceMan;

	// Token: 0x04003250 RID: 12880
	private static CultureInfo resourceCulture;
}
