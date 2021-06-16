using System;
using System.Collections.Generic;
using System.Management.Automation.Host;

namespace System.Management.Automation.Remoting
{
	// Token: 0x020002E5 RID: 741
	internal class HostDefaultData
	{
		// Token: 0x06002364 RID: 9060 RVA: 0x000C72AD File Offset: 0x000C54AD
		private HostDefaultData()
		{
			this.data = new Dictionary<HostDefaultDataId, object>();
		}

		// Token: 0x17000851 RID: 2129
		internal object this[HostDefaultDataId id]
		{
			get
			{
				return this.GetValue(id);
			}
		}

		// Token: 0x06002366 RID: 9062 RVA: 0x000C72C9 File Offset: 0x000C54C9
		internal bool HasValue(HostDefaultDataId id)
		{
			return this.data.ContainsKey(id);
		}

		// Token: 0x06002367 RID: 9063 RVA: 0x000C72D7 File Offset: 0x000C54D7
		internal void SetValue(HostDefaultDataId id, object dataValue)
		{
			this.data[id] = dataValue;
		}

		// Token: 0x06002368 RID: 9064 RVA: 0x000C72E6 File Offset: 0x000C54E6
		internal object GetValue(HostDefaultDataId id)
		{
			if (this.data.ContainsKey(id))
			{
				return this.data[id];
			}
			return null;
		}

		// Token: 0x06002369 RID: 9065 RVA: 0x000C7304 File Offset: 0x000C5504
		internal static HostDefaultData Create(PSHostRawUserInterface hostRawUI)
		{
			if (hostRawUI == null)
			{
				return null;
			}
			HostDefaultData hostDefaultData = new HostDefaultData();
			try
			{
				hostDefaultData.SetValue(HostDefaultDataId.ForegroundColor, hostRawUI.ForegroundColor);
			}
			catch (Exception e)
			{
				CommandProcessorBase.CheckForSevereException(e);
			}
			try
			{
				hostDefaultData.SetValue(HostDefaultDataId.BackgroundColor, hostRawUI.BackgroundColor);
			}
			catch (Exception e2)
			{
				CommandProcessorBase.CheckForSevereException(e2);
			}
			try
			{
				hostDefaultData.SetValue(HostDefaultDataId.CursorPosition, hostRawUI.CursorPosition);
			}
			catch (Exception e3)
			{
				CommandProcessorBase.CheckForSevereException(e3);
			}
			try
			{
				hostDefaultData.SetValue(HostDefaultDataId.WindowPosition, hostRawUI.WindowPosition);
			}
			catch (Exception e4)
			{
				CommandProcessorBase.CheckForSevereException(e4);
			}
			try
			{
				hostDefaultData.SetValue(HostDefaultDataId.CursorSize, hostRawUI.CursorSize);
			}
			catch (Exception e5)
			{
				CommandProcessorBase.CheckForSevereException(e5);
			}
			try
			{
				hostDefaultData.SetValue(HostDefaultDataId.BufferSize, hostRawUI.BufferSize);
			}
			catch (Exception e6)
			{
				CommandProcessorBase.CheckForSevereException(e6);
			}
			try
			{
				hostDefaultData.SetValue(HostDefaultDataId.WindowSize, hostRawUI.WindowSize);
			}
			catch (Exception e7)
			{
				CommandProcessorBase.CheckForSevereException(e7);
			}
			try
			{
				hostDefaultData.SetValue(HostDefaultDataId.MaxWindowSize, hostRawUI.MaxWindowSize);
			}
			catch (Exception e8)
			{
				CommandProcessorBase.CheckForSevereException(e8);
			}
			try
			{
				hostDefaultData.SetValue(HostDefaultDataId.MaxPhysicalWindowSize, hostRawUI.MaxPhysicalWindowSize);
			}
			catch (Exception e9)
			{
				CommandProcessorBase.CheckForSevereException(e9);
			}
			try
			{
				hostDefaultData.SetValue(HostDefaultDataId.WindowTitle, hostRawUI.WindowTitle);
			}
			catch (Exception e10)
			{
				CommandProcessorBase.CheckForSevereException(e10);
			}
			return hostDefaultData;
		}

		// Token: 0x04001136 RID: 4406
		private Dictionary<HostDefaultDataId, object> data;
	}
}
