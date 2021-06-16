using System;
using System.Net;
using System.Net.Sockets;

namespace Microsoft.Xde.Common
{
	// Token: 0x02000026 RID: 38
	public static class ValidationUtilities
	{
		// Token: 0x06000193 RID: 403 RVA: 0x0000401A File Offset: 0x0000221A
		public static void CheckNotBlank(string argument, string propertyName)
		{
			ValidationUtilities.CheckNotNull(argument, propertyName);
			if (argument.Length == 0)
			{
				if (propertyName == null)
				{
					propertyName = "value";
				}
				throw new ArgumentException(StringUtilities.CurrentCultureFormat(Strings.CantBeEmpty, new object[]
				{
					propertyName
				}));
			}
		}

		// Token: 0x06000194 RID: 404 RVA: 0x0000404F File Offset: 0x0000224F
		public static void CheckNotNull(object obj, string propertyName)
		{
			if (obj == null)
			{
				if (propertyName == null)
				{
					propertyName = "value";
				}
				throw new ArgumentNullException(propertyName, StringUtilities.CurrentCultureFormat(Strings.CantBeNullFormat, new object[]
				{
					propertyName
				}));
			}
		}

		// Token: 0x06000195 RID: 405 RVA: 0x00004079 File Offset: 0x00002279
		public static string NullIfBlank(string valueToCheck)
		{
			if (string.IsNullOrEmpty(valueToCheck))
			{
				return null;
			}
			return valueToCheck;
		}

		// Token: 0x06000196 RID: 406 RVA: 0x00004088 File Offset: 0x00002288
		public static bool IsValidIPv4Format(IPAddress address)
		{
			bool result = true;
			if (address == null)
			{
				result = false;
			}
			else
			{
				try
				{
					if (address.AddressFamily != AddressFamily.InterNetwork)
					{
						result = false;
					}
				}
				catch (ArgumentException)
				{
					result = false;
				}
			}
			return result;
		}

		// Token: 0x06000197 RID: 407 RVA: 0x000040C4 File Offset: 0x000022C4
		public static bool IsValidIPv4Format(string address)
		{
			bool flag;
			if (string.IsNullOrEmpty(address))
			{
				flag = false;
			}
			else
			{
				try
				{
					IPAddress address2;
					flag = IPAddress.TryParse(address, out address2);
					if (!flag || !ValidationUtilities.IsValidIPv4Format(address2))
					{
						flag = false;
					}
				}
				catch (ArgumentException)
				{
					flag = false;
				}
			}
			return flag;
		}
	}
}
