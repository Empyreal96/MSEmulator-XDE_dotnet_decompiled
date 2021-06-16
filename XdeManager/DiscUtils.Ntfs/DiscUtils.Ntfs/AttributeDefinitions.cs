using System;
using System.Collections.Generic;
using System.IO;
using DiscUtils.Streams;

namespace DiscUtils.Ntfs
{
	// Token: 0x02000007 RID: 7
	internal sealed class AttributeDefinitions
	{
		// Token: 0x0600000A RID: 10 RVA: 0x000021AC File Offset: 0x000003AC
		public AttributeDefinitions()
		{
			this._attrDefs = new Dictionary<AttributeType, AttributeDefinitionRecord>();
			this.Add(AttributeType.StandardInformation, "$STANDARD_INFORMATION", AttributeTypeFlags.MustBeResident, 48, 72);
			this.Add(AttributeType.AttributeList, "$ATTRIBUTE_LIST", AttributeTypeFlags.CanBeNonResident, 0, -1);
			this.Add(AttributeType.FileName, "$FILE_NAME", AttributeTypeFlags.Indexed | AttributeTypeFlags.MustBeResident, 68, 578);
			this.Add(AttributeType.ObjectId, "$OBJECT_ID", AttributeTypeFlags.MustBeResident, 0, 256);
			this.Add(AttributeType.SecurityDescriptor, "$SECURITY_DESCRIPTOR", AttributeTypeFlags.CanBeNonResident, 0, -1);
			this.Add(AttributeType.VolumeName, "$VOLUME_NAME", AttributeTypeFlags.MustBeResident, 2, 256);
			this.Add(AttributeType.VolumeInformation, "$VOLUME_INFORMATION", AttributeTypeFlags.MustBeResident, 12, 12);
			this.Add(AttributeType.Data, "$DATA", AttributeTypeFlags.None, 0, -1);
			this.Add(AttributeType.IndexRoot, "$INDEX_ROOT", AttributeTypeFlags.MustBeResident, 0, -1);
			this.Add(AttributeType.IndexAllocation, "$INDEX_ALLOCATION", AttributeTypeFlags.CanBeNonResident, 0, -1);
			this.Add(AttributeType.Bitmap, "$BITMAP", AttributeTypeFlags.CanBeNonResident, 0, -1);
			this.Add(AttributeType.ReparsePoint, "$REPARSE_POINT", AttributeTypeFlags.CanBeNonResident, 0, 16384);
			this.Add(AttributeType.ExtendedAttributesInformation, "$EA_INFORMATION", AttributeTypeFlags.MustBeResident, 8, 8);
			this.Add(AttributeType.ExtendedAttributes, "$EA", AttributeTypeFlags.None, 0, 65536);
			this.Add(AttributeType.LoggedUtilityStream, "$LOGGED_UTILITY_STREAM", AttributeTypeFlags.CanBeNonResident, 0, 65536);
		}

		// Token: 0x0600000B RID: 11 RVA: 0x00002310 File Offset: 0x00000510
		public AttributeDefinitions(File file)
		{
			this._attrDefs = new Dictionary<AttributeType, AttributeDefinitionRecord>();
			byte[] array = new byte[160];
			using (Stream stream = file.OpenStream(AttributeType.Data, null, FileAccess.Read))
			{
				while (StreamUtilities.ReadMaximum(stream, array, 0, array.Length) == array.Length)
				{
					AttributeDefinitionRecord attributeDefinitionRecord = new AttributeDefinitionRecord();
					attributeDefinitionRecord.Read(array, 0);
					if (attributeDefinitionRecord.Type != AttributeType.None)
					{
						this._attrDefs.Add(attributeDefinitionRecord.Type, attributeDefinitionRecord);
					}
				}
			}
		}

		// Token: 0x0600000C RID: 12 RVA: 0x000023A0 File Offset: 0x000005A0
		public void WriteTo(File file)
		{
			List<AttributeType> list = new List<AttributeType>(this._attrDefs.Keys);
			list.Sort();
			using (Stream stream = file.OpenStream(AttributeType.Data, null, FileAccess.ReadWrite))
			{
				byte[] array;
				for (int i = 0; i < list.Count; i++)
				{
					array = new byte[160];
					this._attrDefs[list[i]].Write(array, 0);
					stream.Write(array, 0, array.Length);
				}
				array = new byte[160];
				stream.Write(array, 0, array.Length);
			}
		}

		// Token: 0x0600000D RID: 13 RVA: 0x00002444 File Offset: 0x00000644
		internal AttributeDefinitionRecord Lookup(string name)
		{
			foreach (AttributeDefinitionRecord attributeDefinitionRecord in this._attrDefs.Values)
			{
				if (string.Compare(name, attributeDefinitionRecord.Name, StringComparison.OrdinalIgnoreCase) == 0)
				{
					return attributeDefinitionRecord;
				}
			}
			return null;
		}

		// Token: 0x0600000E RID: 14 RVA: 0x000024AC File Offset: 0x000006AC
		internal bool MustBeResident(AttributeType attributeType)
		{
			AttributeDefinitionRecord attributeDefinitionRecord;
			return this._attrDefs.TryGetValue(attributeType, out attributeDefinitionRecord) && (attributeDefinitionRecord.Flags & AttributeTypeFlags.MustBeResident) > AttributeTypeFlags.None;
		}

		// Token: 0x0600000F RID: 15 RVA: 0x000024D8 File Offset: 0x000006D8
		internal bool IsIndexed(AttributeType attributeType)
		{
			AttributeDefinitionRecord attributeDefinitionRecord;
			return this._attrDefs.TryGetValue(attributeType, out attributeDefinitionRecord) && (attributeDefinitionRecord.Flags & AttributeTypeFlags.Indexed) > AttributeTypeFlags.None;
		}

		// Token: 0x06000010 RID: 16 RVA: 0x00002504 File Offset: 0x00000704
		private void Add(AttributeType attributeType, string name, AttributeTypeFlags attributeTypeFlags, int minSize, int maxSize)
		{
			AttributeDefinitionRecord attributeDefinitionRecord = new AttributeDefinitionRecord();
			attributeDefinitionRecord.Type = attributeType;
			attributeDefinitionRecord.Name = name;
			attributeDefinitionRecord.Flags = attributeTypeFlags;
			attributeDefinitionRecord.MinSize = (long)minSize;
			attributeDefinitionRecord.MaxSize = (long)maxSize;
			this._attrDefs.Add(attributeType, attributeDefinitionRecord);
		}

		// Token: 0x04000011 RID: 17
		private readonly Dictionary<AttributeType, AttributeDefinitionRecord> _attrDefs;
	}
}
