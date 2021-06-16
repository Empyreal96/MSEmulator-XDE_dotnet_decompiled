using System;
using System.Management.Automation.Host;

namespace Microsoft.PowerShell.Commands.Internal.Format
{
	// Token: 0x0200052D RID: 1325
	internal class DisplayCellsPSHost : DisplayCells
	{
		// Token: 0x0600374D RID: 14157 RVA: 0x00129D18 File Offset: 0x00127F18
		internal DisplayCellsPSHost(PSHostRawUserInterface rawUserInterface)
		{
			this._rawUserInterface = rawUserInterface;
		}

		// Token: 0x0600374E RID: 14158 RVA: 0x00129D28 File Offset: 0x00127F28
		internal override int Length(string str, int offset)
		{
			try
			{
				return this._rawUserInterface.LengthInBufferCells(str, offset);
			}
			catch (HostException)
			{
			}
			if (!string.IsNullOrEmpty(str))
			{
				return str.Length - offset;
			}
			return 0;
		}

		// Token: 0x0600374F RID: 14159 RVA: 0x00129D6C File Offset: 0x00127F6C
		internal override int Length(string str)
		{
			try
			{
				return this._rawUserInterface.LengthInBufferCells(str);
			}
			catch (HostException)
			{
			}
			if (!string.IsNullOrEmpty(str))
			{
				return str.Length;
			}
			return 0;
		}

		// Token: 0x06003750 RID: 14160 RVA: 0x00129DB0 File Offset: 0x00127FB0
		internal override int Length(char character)
		{
			try
			{
				return this._rawUserInterface.LengthInBufferCells(character);
			}
			catch (HostException)
			{
			}
			return 1;
		}

		// Token: 0x06003751 RID: 14161 RVA: 0x00129DE4 File Offset: 0x00127FE4
		internal override int GetHeadSplitLength(string str, int offset, int displayCells)
		{
			return base.GetSplitLengthInternalHelper(str, offset, displayCells, true);
		}

		// Token: 0x06003752 RID: 14162 RVA: 0x00129DF0 File Offset: 0x00127FF0
		internal override int GetTailSplitLength(string str, int offset, int displayCells)
		{
			return base.GetSplitLengthInternalHelper(str, offset, displayCells, false);
		}

		// Token: 0x04001C4F RID: 7247
		private PSHostRawUserInterface _rawUserInterface;
	}
}
