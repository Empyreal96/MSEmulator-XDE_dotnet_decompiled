using System;
using System.Globalization;
using System.IO;
using Microsoft.Win32;

namespace Microsoft.PowerShell.Commands
{
	// Token: 0x02000477 RID: 1143
	internal static class RegistryWrapperUtils
	{
		// Token: 0x06003303 RID: 13059 RVA: 0x00117724 File Offset: 0x00115924
		public static object ConvertValueToUIntFromRegistryIfNeeded(string name, object value, RegistryValueKind kind)
		{
			try
			{
				if (kind == RegistryValueKind.DWord)
				{
					value = (int)value;
					if ((int)value < 0)
					{
						value = BitConverter.ToUInt32(BitConverter.GetBytes((int)value), 0);
					}
				}
				else if (kind == RegistryValueKind.QWord)
				{
					value = (long)value;
					if ((long)value < 0L)
					{
						value = BitConverter.ToUInt64(BitConverter.GetBytes((long)value), 0);
					}
				}
			}
			catch (IOException)
			{
			}
			return value;
		}

		// Token: 0x06003304 RID: 13060 RVA: 0x001177B0 File Offset: 0x001159B0
		public static object ConvertUIntToValueForRegistryIfNeeded(object value, RegistryValueKind kind)
		{
			if (kind == RegistryValueKind.DWord)
			{
				try
				{
					uint value2 = Convert.ToUInt32(value, CultureInfo.InvariantCulture);
					return BitConverter.ToInt32(BitConverter.GetBytes(value2), 0);
				}
				catch (OverflowException)
				{
					return value;
				}
			}
			if (kind == RegistryValueKind.QWord)
			{
				try
				{
					ulong value3 = Convert.ToUInt64(value, CultureInfo.InvariantCulture);
					value = BitConverter.ToInt64(BitConverter.GetBytes(value3), 0);
				}
				catch (OverflowException)
				{
				}
			}
			return value;
		}
	}
}
