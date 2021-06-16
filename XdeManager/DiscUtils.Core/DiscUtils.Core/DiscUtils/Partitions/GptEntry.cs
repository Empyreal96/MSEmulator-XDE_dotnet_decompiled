using System;
using System.Text;
using DiscUtils.Streams;

namespace DiscUtils.Partitions
{
	// Token: 0x02000050 RID: 80
	internal class GptEntry : IComparable<GptEntry>
	{
		// Token: 0x0600035B RID: 859 RVA: 0x000082A7 File Offset: 0x000064A7
		public GptEntry()
		{
			this.PartitionType = Guid.Empty;
			this.Identity = Guid.Empty;
			this.Name = string.Empty;
		}

		// Token: 0x170000F2 RID: 242
		// (get) Token: 0x0600035C RID: 860 RVA: 0x000082D0 File Offset: 0x000064D0
		public string FriendlyPartitionType
		{
			get
			{
				string text = this.PartitionType.ToString().ToUpperInvariant();
				if (text != null)
				{
					uint num = <PrivateImplementationDetails>.ComputeStringHash(text);
					if (num <= 2298181622U)
					{
						if (num <= 683956618U)
						{
							if (num <= 222464649U)
							{
								if (num <= 46471187U)
								{
									if (num != 40773386U)
									{
										if (num == 46471187U)
										{
											if (text == "AF9B60A0-1431-4F62-BC68-3311714A69AD")
											{
												return "Windows Logical Disk Manager Data";
											}
										}
									}
									else if (text == "5808C8AA-7E8F-42E0-85D2-E1E90434CFB3")
									{
										return "Windows Logical Disk Manager Metadata";
									}
								}
								else if (num != 120367926U)
								{
									if (num == 222464649U)
									{
										if (text == "83BD6B9D-7F41-11DC-BE0B-001560B84F0F")
										{
											return "FreeBSD Boot";
										}
									}
								}
								else if (text == "C12A7328-F81F-11D2-BA4B-00A0C93EC93B")
								{
									return "EFI System";
								}
							}
							else if (num <= 260645145U)
							{
								if (num != 250354471U)
								{
									if (num == 260645145U)
									{
										if (text == "E3C9E316-0B5C-4DB8-817D-F92DF00215AE")
										{
											return "Microsoft Reserved";
										}
									}
								}
								else if (text == "516E7CB6-6ECF-11D6-8FF8-00022D09712B")
								{
									return "FreeBSD Unix File System";
								}
							}
							else if (num != 494326649U)
							{
								if (num == 683956618U)
								{
									if (text == "516E7CBA-6ECF-11D6-8FF8-00022D09712B")
									{
										return "FreeBSD ZFS";
									}
								}
							}
							else if (text == "52414944-0000-11AA-AA11-00306543ECAC")
							{
								return "Mac OS X RAID";
							}
						}
						else if (num <= 1871667327U)
						{
							if (num <= 1218763098U)
							{
								if (num != 785903772U)
								{
									if (num == 1218763098U)
									{
										if (text == "4C616265-6C00-11AA-AA11-00306543ECAC")
										{
											return "Mac OS X Label";
										}
									}
								}
								else if (text == "6A898CC3-1DD2-11B2-99A6-080020736631")
								{
									return "Mac OS X ZFS";
								}
							}
							else if (num != 1561072485U)
							{
								if (num == 1871667327U)
								{
									if (text == "E2A1E728-32E3-11D6-A682-7B03A0000000")
									{
										return "HP-UX Service";
									}
								}
							}
							else if (text == "516E7CB8-6ECF-11D6-8FF8-00022D09712B")
							{
								return "FreeBSD Vinum volume manager";
							}
						}
						else if (num <= 2097439790U)
						{
							if (num != 2056481367U)
							{
								if (num == 2097439790U)
								{
									if (text == "49F48D82-B10E-11DC-B99B-0019D1879648")
									{
										return "NetBSD Log-Structed File System";
									}
								}
							}
							else if (text == "75894C1E-3AEB-11D3-B7C1-7B03A0000000")
							{
								return "HP-UX Data";
							}
						}
						else if (num != 2116813178U)
						{
							if (num == 2298181622U)
							{
								if (text == "516E7CB5-6ECF-11D6-8FF8-00022D09712B")
								{
									return "FreeBSD Swap";
								}
							}
						}
						else if (text == "49F48D5A-B10E-11DC-B99B-0019D1879648")
						{
							return "NetBSD Fast File System";
						}
					}
					else if (num <= 3192360657U)
					{
						if (num <= 2593879908U)
						{
							if (num <= 2392212278U)
							{
								if (num != 2385510125U)
								{
									if (num == 2392212278U)
									{
										if (text == "49F48DAA-B10E-11DC-B99B-0019D1879648")
										{
											return "NetBSD RAID";
										}
									}
								}
								else if (text == "0657FD6D-A4AB-43C4-84E5-0933C84B4F4F")
								{
									return "Linux Swap";
								}
							}
							else if (num != 2585566649U)
							{
								if (num == 2593879908U)
								{
									if (text == "A19D880F-05FC-4D3B-A006-743F0F84911E")
									{
										return "Linux RAID";
									}
								}
							}
							else if (text == "516E7CB4-6ECF-11D6-8FF8-00022D09712B")
							{
								return "FreeBSD Data";
							}
						}
						else if (num <= 2878555935U)
						{
							if (num != 2620840184U)
							{
								if (num == 2878555935U)
								{
									if (text == "E6D6D379-F507-44C2-A23C-238F2A3DF928")
									{
										return "Linux Logical Volume Manager";
									}
								}
							}
							else if (text == "48465300-0000-11AA-AA11-00306543ECAC")
							{
								return "Mac OS X HFS+";
							}
						}
						else if (num != 3159896890U)
						{
							if (num == 3192360657U)
							{
								if (text == "00000000-0000-0000-0000-000000000000")
								{
									return "Unused";
								}
							}
						}
						else if (text == "55465300-0000-11AA-AA11-00306543ECAC")
						{
							return "Mac OS X UFS";
						}
					}
					else if (num <= 3485724126U)
					{
						if (num <= 3278824600U)
						{
							if (num != 3193571037U)
							{
								if (num == 3278824600U)
								{
									if (text == "EBD0A0A2-B9E5-4433-87C0-68B6B72699C7")
									{
										return "Windows Basic Data";
									}
								}
							}
							else if (text == "21686148-6449-6E6F-744E-656564454649")
							{
								return "BIOS Boot";
							}
						}
						else if (num != 3332993640U)
						{
							if (num == 3485724126U)
							{
								if (text == "024DEE41-33E7-11D3-9D69-0008C781F39F")
								{
									return "MBR Partition Scheme";
								}
							}
						}
						else if (text == "2DB519C4-B10F-11DC-B99B-0019D1879648")
						{
							return "NetBSD Concatenated";
						}
					}
					else if (num <= 3722238518U)
					{
						if (num != 3636656847U)
						{
							if (num == 3722238518U)
							{
								if (text == "52414944-5F4F-11AA-AA11-00306543ECAC")
								{
									return "Mac OS X RAID, Offline";
								}
							}
						}
						else if (text == "2DB519EC-B10F-11DC-B99B-0019D1879648")
						{
							return "NetBSD Encrypted";
						}
					}
					else if (num != 4055024423U)
					{
						if (num == 4190041653U)
						{
							if (text == "49F48D32-B10E-11DC-B99B-0019D1879648")
							{
								return "NetBSD Swap";
							}
						}
					}
					else if (text == "426F6F74-0000-11AA-AA11-00306543ECAC")
					{
						return "Mac OS X Boot";
					}
				}
				return "Unknown";
			}
		}

		// Token: 0x0600035D RID: 861 RVA: 0x00008894 File Offset: 0x00006A94
		public int CompareTo(GptEntry other)
		{
			return this.FirstUsedLogicalBlock.CompareTo(other.FirstUsedLogicalBlock);
		}

		// Token: 0x0600035E RID: 862 RVA: 0x000088A8 File Offset: 0x00006AA8
		public void ReadFrom(byte[] buffer, int offset)
		{
			this.PartitionType = EndianUtilities.ToGuidLittleEndian(buffer, offset);
			this.Identity = EndianUtilities.ToGuidLittleEndian(buffer, offset + 16);
			this.FirstUsedLogicalBlock = EndianUtilities.ToInt64LittleEndian(buffer, offset + 32);
			this.LastUsedLogicalBlock = EndianUtilities.ToInt64LittleEndian(buffer, offset + 40);
			this.Attributes = EndianUtilities.ToUInt64LittleEndian(buffer, offset + 48);
			this.Name = Encoding.Unicode.GetString(buffer, offset + 56, 72).TrimEnd(new char[1]);
		}

		// Token: 0x0600035F RID: 863 RVA: 0x00008924 File Offset: 0x00006B24
		public void WriteTo(byte[] buffer, int offset)
		{
			EndianUtilities.WriteBytesLittleEndian(this.PartitionType, buffer, offset);
			EndianUtilities.WriteBytesLittleEndian(this.Identity, buffer, offset + 16);
			EndianUtilities.WriteBytesLittleEndian(this.FirstUsedLogicalBlock, buffer, offset + 32);
			EndianUtilities.WriteBytesLittleEndian(this.LastUsedLogicalBlock, buffer, offset + 40);
			EndianUtilities.WriteBytesLittleEndian(this.Attributes, buffer, offset + 48);
			Encoding.Unicode.GetBytes(this.Name + new string('\0', 36), 0, 36, buffer, offset + 56);
		}

		// Token: 0x040000CC RID: 204
		public ulong Attributes;

		// Token: 0x040000CD RID: 205
		public long FirstUsedLogicalBlock;

		// Token: 0x040000CE RID: 206
		public Guid Identity;

		// Token: 0x040000CF RID: 207
		public long LastUsedLogicalBlock;

		// Token: 0x040000D0 RID: 208
		public string Name;

		// Token: 0x040000D1 RID: 209
		public Guid PartitionType;
	}
}
