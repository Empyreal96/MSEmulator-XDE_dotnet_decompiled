using System;
using System.Globalization;
using System.Runtime.InteropServices;
using Microsoft.WindowsAPICodePack.Resources;

namespace Microsoft.WindowsAPICodePack.Shell.PropertySystem
{
	// Token: 0x0200000D RID: 13
	[StructLayout(LayoutKind.Sequential, Pack = 4)]
	public struct PropertyKey : IEquatable<PropertyKey>
	{
		// Token: 0x1700004E RID: 78
		// (get) Token: 0x06000099 RID: 153 RVA: 0x000034F9 File Offset: 0x000016F9
		public Guid FormatId
		{
			get
			{
				return this.formatId;
			}
		}

		// Token: 0x1700004F RID: 79
		// (get) Token: 0x0600009A RID: 154 RVA: 0x00003501 File Offset: 0x00001701
		public int PropertyId
		{
			get
			{
				return this.propertyId;
			}
		}

		// Token: 0x0600009B RID: 155 RVA: 0x00003509 File Offset: 0x00001709
		public PropertyKey(Guid formatId, int propertyId)
		{
			this.formatId = formatId;
			this.propertyId = propertyId;
		}

		// Token: 0x0600009C RID: 156 RVA: 0x00003519 File Offset: 0x00001719
		public PropertyKey(string formatId, int propertyId)
		{
			this.formatId = new Guid(formatId);
			this.propertyId = propertyId;
		}

		// Token: 0x0600009D RID: 157 RVA: 0x0000352E File Offset: 0x0000172E
		public bool Equals(PropertyKey other)
		{
			return other.Equals(this);
		}

		// Token: 0x0600009E RID: 158 RVA: 0x00003548 File Offset: 0x00001748
		public override int GetHashCode()
		{
			return this.formatId.GetHashCode() ^ this.propertyId;
		}

		// Token: 0x0600009F RID: 159 RVA: 0x00003564 File Offset: 0x00001764
		public override bool Equals(object obj)
		{
			if (obj == null)
			{
				return false;
			}
			if (!(obj is PropertyKey))
			{
				return false;
			}
			PropertyKey propertyKey = (PropertyKey)obj;
			return propertyKey.formatId.Equals(this.formatId) && propertyKey.propertyId == this.propertyId;
		}

		// Token: 0x060000A0 RID: 160 RVA: 0x000035AB File Offset: 0x000017AB
		public static bool operator ==(PropertyKey propKey1, PropertyKey propKey2)
		{
			return propKey1.Equals(propKey2);
		}

		// Token: 0x060000A1 RID: 161 RVA: 0x000035B5 File Offset: 0x000017B5
		public static bool operator !=(PropertyKey propKey1, PropertyKey propKey2)
		{
			return !propKey1.Equals(propKey2);
		}

		// Token: 0x060000A2 RID: 162 RVA: 0x000035C2 File Offset: 0x000017C2
		public override string ToString()
		{
			return string.Format(CultureInfo.InvariantCulture, LocalizedMessages.PropertyKeyFormatString, this.formatId.ToString("B"), this.propertyId);
		}

		// Token: 0x04000102 RID: 258
		private Guid formatId;

		// Token: 0x04000103 RID: 259
		private int propertyId;
	}
}
