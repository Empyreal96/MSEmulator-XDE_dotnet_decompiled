using System;
using System.CodeDom.Compiler;
using System.ComponentModel;
using System.Diagnostics;
using System.Globalization;
using System.Resources;
using System.Runtime.CompilerServices;

// Token: 0x02000A2A RID: 2602
[GeneratedCode("System.Resources.Tools.StronglyTypedResourceBuilder", "4.0.0.0")]
[CompilerGenerated]
[DebuggerNonUserCode]
internal class Serialization
{
	// Token: 0x060063FC RID: 25596 RVA: 0x0020BBB4 File Offset: 0x00209DB4
	internal Serialization()
	{
	}

	// Token: 0x17001857 RID: 6231
	// (get) Token: 0x060063FD RID: 25597 RVA: 0x0020BBBC File Offset: 0x00209DBC
	[EditorBrowsable(EditorBrowsableState.Advanced)]
	internal static ResourceManager ResourceManager
	{
		get
		{
			if (object.ReferenceEquals(Serialization.resourceMan, null))
			{
				ResourceManager resourceManager = new ResourceManager("Serialization", typeof(Serialization).Assembly);
				Serialization.resourceMan = resourceManager;
			}
			return Serialization.resourceMan;
		}
	}

	// Token: 0x17001858 RID: 6232
	// (get) Token: 0x060063FE RID: 25598 RVA: 0x0020BBFB File Offset: 0x00209DFB
	// (set) Token: 0x060063FF RID: 25599 RVA: 0x0020BC02 File Offset: 0x00209E02
	[EditorBrowsable(EditorBrowsableState.Advanced)]
	internal static CultureInfo Culture
	{
		get
		{
			return Serialization.resourceCulture;
		}
		set
		{
			Serialization.resourceCulture = value;
		}
	}

	// Token: 0x17001859 RID: 6233
	// (get) Token: 0x06006400 RID: 25600 RVA: 0x0020BC0A File Offset: 0x00209E0A
	internal static string AttributeExpected
	{
		get
		{
			return Serialization.ResourceManager.GetString("AttributeExpected", Serialization.resourceCulture);
		}
	}

	// Token: 0x1700185A RID: 6234
	// (get) Token: 0x06006401 RID: 25601 RVA: 0x0020BC20 File Offset: 0x00209E20
	internal static string DepthOfOneRequired
	{
		get
		{
			return Serialization.ResourceManager.GetString("DepthOfOneRequired", Serialization.resourceCulture);
		}
	}

	// Token: 0x1700185B RID: 6235
	// (get) Token: 0x06006402 RID: 25602 RVA: 0x0020BC36 File Offset: 0x00209E36
	internal static string DeserializationMemoryQuota
	{
		get
		{
			return Serialization.ResourceManager.GetString("DeserializationMemoryQuota", Serialization.resourceCulture);
		}
	}

	// Token: 0x1700185C RID: 6236
	// (get) Token: 0x06006403 RID: 25603 RVA: 0x0020BC4C File Offset: 0x00209E4C
	internal static string DeserializationTooDeep
	{
		get
		{
			return Serialization.ResourceManager.GetString("DeserializationTooDeep", Serialization.resourceCulture);
		}
	}

	// Token: 0x1700185D RID: 6237
	// (get) Token: 0x06006404 RID: 25604 RVA: 0x0020BC62 File Offset: 0x00209E62
	internal static string DeserializeSecureStringFailed
	{
		get
		{
			return Serialization.ResourceManager.GetString("DeserializeSecureStringFailed", Serialization.resourceCulture);
		}
	}

	// Token: 0x1700185E RID: 6238
	// (get) Token: 0x06006405 RID: 25605 RVA: 0x0020BC78 File Offset: 0x00209E78
	internal static string DictionaryKeyNotSpecified
	{
		get
		{
			return Serialization.ResourceManager.GetString("DictionaryKeyNotSpecified", Serialization.resourceCulture);
		}
	}

	// Token: 0x1700185F RID: 6239
	// (get) Token: 0x06006406 RID: 25606 RVA: 0x0020BC8E File Offset: 0x00209E8E
	internal static string DictionaryValueNotSpecified
	{
		get
		{
			return Serialization.ResourceManager.GetString("DictionaryValueNotSpecified", Serialization.resourceCulture);
		}
	}

	// Token: 0x17001860 RID: 6240
	// (get) Token: 0x06006407 RID: 25607 RVA: 0x0020BCA4 File Offset: 0x00209EA4
	internal static string InvalidDictionaryKeyName
	{
		get
		{
			return Serialization.ResourceManager.GetString("InvalidDictionaryKeyName", Serialization.resourceCulture);
		}
	}

	// Token: 0x17001861 RID: 6241
	// (get) Token: 0x06006408 RID: 25608 RVA: 0x0020BCBA File Offset: 0x00209EBA
	internal static string InvalidDictionaryValueName
	{
		get
		{
			return Serialization.ResourceManager.GetString("InvalidDictionaryValueName", Serialization.resourceCulture);
		}
	}

	// Token: 0x17001862 RID: 6242
	// (get) Token: 0x06006409 RID: 25609 RVA: 0x0020BCD0 File Offset: 0x00209ED0
	internal static string InvalidElementTag
	{
		get
		{
			return Serialization.ResourceManager.GetString("InvalidElementTag", Serialization.resourceCulture);
		}
	}

	// Token: 0x17001863 RID: 6243
	// (get) Token: 0x0600640A RID: 25610 RVA: 0x0020BCE6 File Offset: 0x00209EE6
	internal static string InvalidEncryptedString
	{
		get
		{
			return Serialization.ResourceManager.GetString("InvalidEncryptedString", Serialization.resourceCulture);
		}
	}

	// Token: 0x17001864 RID: 6244
	// (get) Token: 0x0600640B RID: 25611 RVA: 0x0020BCFC File Offset: 0x00209EFC
	internal static string InvalidKey
	{
		get
		{
			return Serialization.ResourceManager.GetString("InvalidKey", Serialization.resourceCulture);
		}
	}

	// Token: 0x17001865 RID: 6245
	// (get) Token: 0x0600640C RID: 25612 RVA: 0x0020BD12 File Offset: 0x00209F12
	internal static string InvalidKeyLength
	{
		get
		{
			return Serialization.ResourceManager.GetString("InvalidKeyLength", Serialization.resourceCulture);
		}
	}

	// Token: 0x17001866 RID: 6246
	// (get) Token: 0x0600640D RID: 25613 RVA: 0x0020BD28 File Offset: 0x00209F28
	internal static string InvalidNodeType
	{
		get
		{
			return Serialization.ResourceManager.GetString("InvalidNodeType", Serialization.resourceCulture);
		}
	}

	// Token: 0x17001867 RID: 6247
	// (get) Token: 0x0600640E RID: 25614 RVA: 0x0020BD3E File Offset: 0x00209F3E
	internal static string InvalidPrimitiveType
	{
		get
		{
			return Serialization.ResourceManager.GetString("InvalidPrimitiveType", Serialization.resourceCulture);
		}
	}

	// Token: 0x17001868 RID: 6248
	// (get) Token: 0x0600640F RID: 25615 RVA: 0x0020BD54 File Offset: 0x00209F54
	internal static string InvalidReferenceId
	{
		get
		{
			return Serialization.ResourceManager.GetString("InvalidReferenceId", Serialization.resourceCulture);
		}
	}

	// Token: 0x17001869 RID: 6249
	// (get) Token: 0x06006410 RID: 25616 RVA: 0x0020BD6A File Offset: 0x00209F6A
	internal static string InvalidTypeHierarchyReferenceId
	{
		get
		{
			return Serialization.ResourceManager.GetString("InvalidTypeHierarchyReferenceId", Serialization.resourceCulture);
		}
	}

	// Token: 0x1700186A RID: 6250
	// (get) Token: 0x06006411 RID: 25617 RVA: 0x0020BD80 File Offset: 0x00209F80
	internal static string InvalidVersion
	{
		get
		{
			return Serialization.ResourceManager.GetString("InvalidVersion", Serialization.resourceCulture);
		}
	}

	// Token: 0x1700186B RID: 6251
	// (get) Token: 0x06006412 RID: 25618 RVA: 0x0020BD96 File Offset: 0x00209F96
	internal static string NullAsDictionaryKey
	{
		get
		{
			return Serialization.ResourceManager.GetString("NullAsDictionaryKey", Serialization.resourceCulture);
		}
	}

	// Token: 0x1700186C RID: 6252
	// (get) Token: 0x06006413 RID: 25619 RVA: 0x0020BDAC File Offset: 0x00209FAC
	internal static string PrimitiveHashtableInvalidKey
	{
		get
		{
			return Serialization.ResourceManager.GetString("PrimitiveHashtableInvalidKey", Serialization.resourceCulture);
		}
	}

	// Token: 0x1700186D RID: 6253
	// (get) Token: 0x06006414 RID: 25620 RVA: 0x0020BDC2 File Offset: 0x00209FC2
	internal static string PrimitiveHashtableInvalidValue
	{
		get
		{
			return Serialization.ResourceManager.GetString("PrimitiveHashtableInvalidValue", Serialization.resourceCulture);
		}
	}

	// Token: 0x1700186E RID: 6254
	// (get) Token: 0x06006415 RID: 25621 RVA: 0x0020BDD8 File Offset: 0x00209FD8
	internal static string ReadCalledAfterDone
	{
		get
		{
			return Serialization.ResourceManager.GetString("ReadCalledAfterDone", Serialization.resourceCulture);
		}
	}

	// Token: 0x1700186F RID: 6255
	// (get) Token: 0x06006416 RID: 25622 RVA: 0x0020BDEE File Offset: 0x00209FEE
	internal static string Stopping
	{
		get
		{
			return Serialization.ResourceManager.GetString("Stopping", Serialization.resourceCulture);
		}
	}

	// Token: 0x17001870 RID: 6256
	// (get) Token: 0x06006417 RID: 25623 RVA: 0x0020BE04 File Offset: 0x0020A004
	internal static string UnexpectedVersion
	{
		get
		{
			return Serialization.ResourceManager.GetString("UnexpectedVersion", Serialization.resourceCulture);
		}
	}

	// Token: 0x0400324B RID: 12875
	private static ResourceManager resourceMan;

	// Token: 0x0400324C RID: 12876
	private static CultureInfo resourceCulture;
}
