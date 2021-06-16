using System;
using System.Resources;
using System.Runtime.CompilerServices;
using FxResources.System.Threading.Tasks.Dataflow;

namespace System
{
	// Token: 0x02000006 RID: 6
	internal static class SR
	{
		// Token: 0x06000003 RID: 3 RVA: 0x00002058 File Offset: 0x00000258
		[MethodImpl(MethodImplOptions.NoInlining)]
		private static bool UsingResourceKeys()
		{
			return false;
		}

		// Token: 0x06000004 RID: 4 RVA: 0x0000205C File Offset: 0x0000025C
		internal static string GetResourceString(string resourceKey, string defaultString = null)
		{
			if (System.SR.UsingResourceKeys())
			{
				return defaultString ?? resourceKey;
			}
			string text = null;
			try
			{
				text = System.SR.ResourceManager.GetString(resourceKey);
			}
			catch (MissingManifestResourceException)
			{
			}
			if (defaultString != null && resourceKey.Equals(text))
			{
				return defaultString;
			}
			return text;
		}

		// Token: 0x17000001 RID: 1
		// (get) Token: 0x06000005 RID: 5 RVA: 0x000020AC File Offset: 0x000002AC
		internal static ResourceManager ResourceManager
		{
			get
			{
				ResourceManager result;
				if ((result = System.SR.s_resourceManager) == null)
				{
					result = (System.SR.s_resourceManager = new ResourceManager(typeof(FxResources.System.Threading.Tasks.Dataflow.SR)));
				}
				return result;
			}
		}

		// Token: 0x17000002 RID: 2
		// (get) Token: 0x06000006 RID: 6 RVA: 0x000020CC File Offset: 0x000002CC
		internal static string ArgumentOutOfRange_BatchSizeMustBeNoGreaterThanBoundedCapacity
		{
			get
			{
				return System.SR.GetResourceString("ArgumentOutOfRange_BatchSizeMustBeNoGreaterThanBoundedCapacity", null);
			}
		}

		// Token: 0x17000003 RID: 3
		// (get) Token: 0x06000007 RID: 7 RVA: 0x000020D9 File Offset: 0x000002D9
		internal static string ArgumentOutOfRange_GenericPositive
		{
			get
			{
				return System.SR.GetResourceString("ArgumentOutOfRange_GenericPositive", null);
			}
		}

		// Token: 0x17000004 RID: 4
		// (get) Token: 0x06000008 RID: 8 RVA: 0x000020E6 File Offset: 0x000002E6
		internal static string ArgumentOutOfRange_NeedNonNegOrNegative1
		{
			get
			{
				return System.SR.GetResourceString("ArgumentOutOfRange_NeedNonNegOrNegative1", null);
			}
		}

		// Token: 0x17000005 RID: 5
		// (get) Token: 0x06000009 RID: 9 RVA: 0x000020F3 File Offset: 0x000002F3
		internal static string Argument_BoundedCapacityNotSupported
		{
			get
			{
				return System.SR.GetResourceString("Argument_BoundedCapacityNotSupported", null);
			}
		}

		// Token: 0x17000006 RID: 6
		// (get) Token: 0x0600000A RID: 10 RVA: 0x00002100 File Offset: 0x00000300
		internal static string Argument_CantConsumeFromANullSource
		{
			get
			{
				return System.SR.GetResourceString("Argument_CantConsumeFromANullSource", null);
			}
		}

		// Token: 0x17000007 RID: 7
		// (get) Token: 0x0600000B RID: 11 RVA: 0x0000210D File Offset: 0x0000030D
		internal static string Argument_InvalidMessageHeader
		{
			get
			{
				return System.SR.GetResourceString("Argument_InvalidMessageHeader", null);
			}
		}

		// Token: 0x17000008 RID: 8
		// (get) Token: 0x0600000C RID: 12 RVA: 0x0000211A File Offset: 0x0000031A
		internal static string Argument_InvalidMessageId
		{
			get
			{
				return System.SR.GetResourceString("Argument_InvalidMessageId", null);
			}
		}

		// Token: 0x17000009 RID: 9
		// (get) Token: 0x0600000D RID: 13 RVA: 0x00002127 File Offset: 0x00000327
		internal static string Argument_NonGreedyNotSupported
		{
			get
			{
				return System.SR.GetResourceString("Argument_NonGreedyNotSupported", null);
			}
		}

		// Token: 0x1700000A RID: 10
		// (get) Token: 0x0600000E RID: 14 RVA: 0x00002134 File Offset: 0x00000334
		internal static string InvalidOperation_DataNotAvailableForReceive
		{
			get
			{
				return System.SR.GetResourceString("InvalidOperation_DataNotAvailableForReceive", null);
			}
		}

		// Token: 0x1700000B RID: 11
		// (get) Token: 0x0600000F RID: 15 RVA: 0x00002141 File Offset: 0x00000341
		internal static string InvalidOperation_FailedToConsumeReservedMessage
		{
			get
			{
				return System.SR.GetResourceString("InvalidOperation_FailedToConsumeReservedMessage", null);
			}
		}

		// Token: 0x1700000C RID: 12
		// (get) Token: 0x06000010 RID: 16 RVA: 0x0000214E File Offset: 0x0000034E
		internal static string InvalidOperation_MessageNotReservedByTarget
		{
			get
			{
				return System.SR.GetResourceString("InvalidOperation_MessageNotReservedByTarget", null);
			}
		}

		// Token: 0x1700000D RID: 13
		// (get) Token: 0x06000011 RID: 17 RVA: 0x0000215B File Offset: 0x0000035B
		internal static string NotSupported_MemberNotNeeded
		{
			get
			{
				return System.SR.GetResourceString("NotSupported_MemberNotNeeded", null);
			}
		}

		// Token: 0x1700000E RID: 14
		// (get) Token: 0x06000012 RID: 18 RVA: 0x00002168 File Offset: 0x00000368
		internal static string InvalidOperation_ErrorDuringCleanup
		{
			get
			{
				return System.SR.GetResourceString("InvalidOperation_ErrorDuringCleanup", null);
			}
		}

		// Token: 0x04000001 RID: 1
		private static ResourceManager s_resourceManager;
	}
}
