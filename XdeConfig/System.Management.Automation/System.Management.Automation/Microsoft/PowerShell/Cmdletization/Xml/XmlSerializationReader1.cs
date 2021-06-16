using System;
using System.CodeDom.Compiler;
using System.Xml;
using System.Xml.Serialization;

namespace Microsoft.PowerShell.Cmdletization.Xml
{
	// Token: 0x020009D0 RID: 2512
	[GeneratedCode("sgen", "4.0")]
	internal class XmlSerializationReader1 : XmlSerializationReader
	{
		// Token: 0x06005C8B RID: 23691 RVA: 0x001F1128 File Offset: 0x001EF328
		public object Read50_PowerShellMetadata()
		{
			object result = null;
			base.Reader.MoveToContent();
			if (base.Reader.NodeType == XmlNodeType.Element)
			{
				if (base.Reader.LocalName != this.id1_PowerShellMetadata || base.Reader.NamespaceURI != this.id2_Item)
				{
					throw base.CreateUnknownNodeException();
				}
				result = this.Read39_PowerShellMetadata(false, true);
			}
			else
			{
				base.UnknownNode(null, "http://schemas.microsoft.com/cmdlets-over-objects/2009/11:PowerShellMetadata");
			}
			return result;
		}

		// Token: 0x06005C8C RID: 23692 RVA: 0x001F1198 File Offset: 0x001EF398
		public object Read51_ClassMetadata()
		{
			object result = null;
			base.Reader.MoveToContent();
			if (base.Reader.NodeType == XmlNodeType.Element)
			{
				if (base.Reader.LocalName != this.id3_ClassMetadata || base.Reader.NamespaceURI != this.id4_Item)
				{
					throw base.CreateUnknownNodeException();
				}
				result = this.Read36_ClassMetadata(true, true);
			}
			else
			{
				base.UnknownNode(null, ":ClassMetadata");
			}
			return result;
		}

		// Token: 0x06005C8D RID: 23693 RVA: 0x001F1208 File Offset: 0x001EF408
		public object Read52_ClassMetadataInstanceCmdlets()
		{
			object result = null;
			base.Reader.MoveToContent();
			if (base.Reader.NodeType == XmlNodeType.Element)
			{
				if (base.Reader.LocalName != this.id5_ClassMetadataInstanceCmdlets || base.Reader.NamespaceURI != this.id4_Item)
				{
					throw base.CreateUnknownNodeException();
				}
				result = this.Read40_ClassMetadataInstanceCmdlets(true, true);
			}
			else
			{
				base.UnknownNode(null, ":ClassMetadataInstanceCmdlets");
			}
			return result;
		}

		// Token: 0x06005C8E RID: 23694 RVA: 0x001F1278 File Offset: 0x001EF478
		public object Read53_GetCmdletParameters()
		{
			object result = null;
			base.Reader.MoveToContent();
			if (base.Reader.NodeType == XmlNodeType.Element)
			{
				if (base.Reader.LocalName != this.id6_GetCmdletParameters || base.Reader.NamespaceURI != this.id4_Item)
				{
					throw base.CreateUnknownNodeException();
				}
				result = this.Read19_GetCmdletParameters(true, true);
			}
			else
			{
				base.UnknownNode(null, ":GetCmdletParameters");
			}
			return result;
		}

		// Token: 0x06005C8F RID: 23695 RVA: 0x001F12E8 File Offset: 0x001EF4E8
		public object Read54_PropertyMetadata()
		{
			object result = null;
			base.Reader.MoveToContent();
			if (base.Reader.NodeType == XmlNodeType.Element)
			{
				if (base.Reader.LocalName != this.id7_PropertyMetadata || base.Reader.NamespaceURI != this.id4_Item)
				{
					throw base.CreateUnknownNodeException();
				}
				result = this.Read15_PropertyMetadata(true, true);
			}
			else
			{
				base.UnknownNode(null, ":PropertyMetadata");
			}
			return result;
		}

		// Token: 0x06005C90 RID: 23696 RVA: 0x001F1358 File Offset: 0x001EF558
		public object Read55_TypeMetadata()
		{
			object result = null;
			base.Reader.MoveToContent();
			if (base.Reader.NodeType == XmlNodeType.Element)
			{
				if (base.Reader.LocalName != this.id8_TypeMetadata || base.Reader.NamespaceURI != this.id4_Item)
				{
					throw base.CreateUnknownNodeException();
				}
				result = this.Read2_TypeMetadata(true, true);
			}
			else
			{
				base.UnknownNode(null, ":TypeMetadata");
			}
			return result;
		}

		// Token: 0x06005C91 RID: 23697 RVA: 0x001F13C8 File Offset: 0x001EF5C8
		public object Read56_Association()
		{
			object result = null;
			base.Reader.MoveToContent();
			if (base.Reader.NodeType == XmlNodeType.Element)
			{
				if (base.Reader.LocalName != this.id9_Association || base.Reader.NamespaceURI != this.id4_Item)
				{
					throw base.CreateUnknownNodeException();
				}
				result = this.Read17_Association(true, true);
			}
			else
			{
				base.UnknownNode(null, ":Association");
			}
			return result;
		}

		// Token: 0x06005C92 RID: 23698 RVA: 0x001F1438 File Offset: 0x001EF638
		public object Read57_AssociationAssociatedInstance()
		{
			object result = null;
			base.Reader.MoveToContent();
			if (base.Reader.NodeType == XmlNodeType.Element)
			{
				if (base.Reader.LocalName != this.id10_AssociationAssociatedInstance || base.Reader.NamespaceURI != this.id4_Item)
				{
					throw base.CreateUnknownNodeException();
				}
				result = this.Read41_AssociationAssociatedInstance(true, true);
			}
			else
			{
				base.UnknownNode(null, ":AssociationAssociatedInstance");
			}
			return result;
		}

		// Token: 0x06005C93 RID: 23699 RVA: 0x001F14A8 File Offset: 0x001EF6A8
		public object Read58_CmdletParameterMetadata()
		{
			object result = null;
			base.Reader.MoveToContent();
			if (base.Reader.NodeType == XmlNodeType.Element)
			{
				if (base.Reader.LocalName != this.id11_CmdletParameterMetadata || base.Reader.NamespaceURI != this.id4_Item)
				{
					throw base.CreateUnknownNodeException();
				}
				result = this.Read10_CmdletParameterMetadata(true, true);
			}
			else
			{
				base.UnknownNode(null, ":CmdletParameterMetadata");
			}
			return result;
		}

		// Token: 0x06005C94 RID: 23700 RVA: 0x001F1518 File Offset: 0x001EF718
		public object Read59_Item()
		{
			object result = null;
			base.Reader.MoveToContent();
			if (base.Reader.NodeType == XmlNodeType.Element)
			{
				if (base.Reader.LocalName != this.id12_Item || base.Reader.NamespaceURI != this.id4_Item)
				{
					throw base.CreateUnknownNodeException();
				}
				result = this.Read11_Item(true, true);
			}
			else
			{
				base.UnknownNode(null, ":CmdletParameterMetadataForGetCmdletParameter");
			}
			return result;
		}

		// Token: 0x06005C95 RID: 23701 RVA: 0x001F1588 File Offset: 0x001EF788
		public object Read60_Item()
		{
			object result = null;
			base.Reader.MoveToContent();
			if (base.Reader.NodeType == XmlNodeType.Element)
			{
				if (base.Reader.LocalName != this.id13_Item || base.Reader.NamespaceURI != this.id4_Item)
				{
					throw base.CreateUnknownNodeException();
				}
				result = this.Read12_Item(true, true);
			}
			else
			{
				base.UnknownNode(null, ":CmdletParameterMetadataForGetCmdletFilteringParameter");
			}
			return result;
		}

		// Token: 0x06005C96 RID: 23702 RVA: 0x001F15F8 File Offset: 0x001EF7F8
		public object Read61_Item()
		{
			object result = null;
			base.Reader.MoveToContent();
			if (base.Reader.NodeType == XmlNodeType.Element)
			{
				if (base.Reader.LocalName != this.id14_Item || base.Reader.NamespaceURI != this.id4_Item)
				{
					throw base.CreateUnknownNodeException();
				}
				result = this.Read42_Item(true, true);
			}
			else
			{
				base.UnknownNode(null, ":CmdletParameterMetadataValidateCount");
			}
			return result;
		}

		// Token: 0x06005C97 RID: 23703 RVA: 0x001F1668 File Offset: 0x001EF868
		public object Read62_Item()
		{
			object result = null;
			base.Reader.MoveToContent();
			if (base.Reader.NodeType == XmlNodeType.Element)
			{
				if (base.Reader.LocalName != this.id15_Item || base.Reader.NamespaceURI != this.id4_Item)
				{
					throw base.CreateUnknownNodeException();
				}
				result = this.Read43_Item(true, true);
			}
			else
			{
				base.UnknownNode(null, ":CmdletParameterMetadataValidateLength");
			}
			return result;
		}

		// Token: 0x06005C98 RID: 23704 RVA: 0x001F16D8 File Offset: 0x001EF8D8
		public object Read63_Item()
		{
			object result = null;
			base.Reader.MoveToContent();
			if (base.Reader.NodeType == XmlNodeType.Element)
			{
				if (base.Reader.LocalName != this.id16_Item || base.Reader.NamespaceURI != this.id4_Item)
				{
					throw base.CreateUnknownNodeException();
				}
				result = this.Read44_Item(true, true);
			}
			else
			{
				base.UnknownNode(null, ":CmdletParameterMetadataValidateRange");
			}
			return result;
		}

		// Token: 0x06005C99 RID: 23705 RVA: 0x001F1748 File Offset: 0x001EF948
		public object Read64_ObsoleteAttributeMetadata()
		{
			object result = null;
			base.Reader.MoveToContent();
			if (base.Reader.NodeType == XmlNodeType.Element)
			{
				if (base.Reader.LocalName != this.id17_ObsoleteAttributeMetadata || base.Reader.NamespaceURI != this.id4_Item)
				{
					throw base.CreateUnknownNodeException();
				}
				result = this.Read7_ObsoleteAttributeMetadata(true, true);
			}
			else
			{
				base.UnknownNode(null, ":ObsoleteAttributeMetadata");
			}
			return result;
		}

		// Token: 0x06005C9A RID: 23706 RVA: 0x001F17B8 File Offset: 0x001EF9B8
		public object Read65_Item()
		{
			object result = null;
			base.Reader.MoveToContent();
			if (base.Reader.NodeType == XmlNodeType.Element)
			{
				if (base.Reader.LocalName != this.id18_Item || base.Reader.NamespaceURI != this.id4_Item)
				{
					throw base.CreateUnknownNodeException();
				}
				result = this.Read9_Item(true, true);
			}
			else
			{
				base.UnknownNode(null, ":CmdletParameterMetadataForInstanceMethodParameter");
			}
			return result;
		}

		// Token: 0x06005C9B RID: 23707 RVA: 0x001F1828 File Offset: 0x001EFA28
		public object Read66_Item()
		{
			object result = null;
			base.Reader.MoveToContent();
			if (base.Reader.NodeType == XmlNodeType.Element)
			{
				if (base.Reader.LocalName != this.id19_Item || base.Reader.NamespaceURI != this.id4_Item)
				{
					throw base.CreateUnknownNodeException();
				}
				result = this.Read8_Item(true, true);
			}
			else
			{
				base.UnknownNode(null, ":CmdletParameterMetadataForStaticMethodParameter");
			}
			return result;
		}

		// Token: 0x06005C9C RID: 23708 RVA: 0x001F1898 File Offset: 0x001EFA98
		public object Read67_QueryOption()
		{
			object result = null;
			base.Reader.MoveToContent();
			if (base.Reader.NodeType == XmlNodeType.Element)
			{
				if (base.Reader.LocalName != this.id20_QueryOption || base.Reader.NamespaceURI != this.id4_Item)
				{
					throw base.CreateUnknownNodeException();
				}
				result = this.Read18_QueryOption(true, true);
			}
			else
			{
				base.UnknownNode(null, ":QueryOption");
			}
			return result;
		}

		// Token: 0x06005C9D RID: 23709 RVA: 0x001F1908 File Offset: 0x001EFB08
		public object Read68_GetCmdletMetadata()
		{
			object result = null;
			base.Reader.MoveToContent();
			if (base.Reader.NodeType == XmlNodeType.Element)
			{
				if (base.Reader.LocalName != this.id21_GetCmdletMetadata || base.Reader.NamespaceURI != this.id4_Item)
				{
					throw base.CreateUnknownNodeException();
				}
				result = this.Read22_GetCmdletMetadata(true, true);
			}
			else
			{
				base.UnknownNode(null, ":GetCmdletMetadata");
			}
			return result;
		}

		// Token: 0x06005C9E RID: 23710 RVA: 0x001F1978 File Offset: 0x001EFB78
		public object Read69_CommonCmdletMetadata()
		{
			object result = null;
			base.Reader.MoveToContent();
			if (base.Reader.NodeType == XmlNodeType.Element)
			{
				if (base.Reader.LocalName != this.id22_CommonCmdletMetadata || base.Reader.NamespaceURI != this.id4_Item)
				{
					throw base.CreateUnknownNodeException();
				}
				result = this.Read21_CommonCmdletMetadata(true, true);
			}
			else
			{
				base.UnknownNode(null, ":CommonCmdletMetadata");
			}
			return result;
		}

		// Token: 0x06005C9F RID: 23711 RVA: 0x001F19E8 File Offset: 0x001EFBE8
		public object Read70_ConfirmImpact()
		{
			object result = null;
			base.Reader.MoveToContent();
			if (base.Reader.NodeType == XmlNodeType.Element)
			{
				if (base.Reader.LocalName != this.id23_ConfirmImpact || base.Reader.NamespaceURI != this.id4_Item)
				{
					throw base.CreateUnknownNodeException();
				}
				result = this.Read20_ConfirmImpact(base.Reader.ReadElementString());
			}
			else
			{
				base.UnknownNode(null, ":ConfirmImpact");
			}
			return result;
		}

		// Token: 0x06005CA0 RID: 23712 RVA: 0x001F1A64 File Offset: 0x001EFC64
		public object Read71_StaticCmdletMetadata()
		{
			object result = null;
			base.Reader.MoveToContent();
			if (base.Reader.NodeType == XmlNodeType.Element)
			{
				if (base.Reader.LocalName != this.id24_StaticCmdletMetadata || base.Reader.NamespaceURI != this.id4_Item)
				{
					throw base.CreateUnknownNodeException();
				}
				result = this.Read34_StaticCmdletMetadata(true, true);
			}
			else
			{
				base.UnknownNode(null, ":StaticCmdletMetadata");
			}
			return result;
		}

		// Token: 0x06005CA1 RID: 23713 RVA: 0x001F1AD4 File Offset: 0x001EFCD4
		public object Read72_Item()
		{
			object result = null;
			base.Reader.MoveToContent();
			if (base.Reader.NodeType == XmlNodeType.Element)
			{
				if (base.Reader.LocalName != this.id25_Item || base.Reader.NamespaceURI != this.id4_Item)
				{
					throw base.CreateUnknownNodeException();
				}
				result = this.Read45_Item(true, true);
			}
			else
			{
				base.UnknownNode(null, ":StaticCmdletMetadataCmdletMetadata");
			}
			return result;
		}

		// Token: 0x06005CA2 RID: 23714 RVA: 0x001F1B44 File Offset: 0x001EFD44
		public object Read73_CommonMethodMetadata()
		{
			object result = null;
			base.Reader.MoveToContent();
			if (base.Reader.NodeType == XmlNodeType.Element)
			{
				if (base.Reader.LocalName != this.id26_CommonMethodMetadata || base.Reader.NamespaceURI != this.id4_Item)
				{
					throw base.CreateUnknownNodeException();
				}
				result = this.Read29_CommonMethodMetadata(true, true);
			}
			else
			{
				base.UnknownNode(null, ":CommonMethodMetadata");
			}
			return result;
		}

		// Token: 0x06005CA3 RID: 23715 RVA: 0x001F1BB4 File Offset: 0x001EFDB4
		public object Read74_StaticMethodMetadata()
		{
			object result = null;
			base.Reader.MoveToContent();
			if (base.Reader.NodeType == XmlNodeType.Element)
			{
				if (base.Reader.LocalName != this.id27_StaticMethodMetadata || base.Reader.NamespaceURI != this.id4_Item)
				{
					throw base.CreateUnknownNodeException();
				}
				result = this.Read28_StaticMethodMetadata(true, true);
			}
			else
			{
				base.UnknownNode(null, ":StaticMethodMetadata");
			}
			return result;
		}

		// Token: 0x06005CA4 RID: 23716 RVA: 0x001F1C24 File Offset: 0x001EFE24
		public object Read75_CommonMethodParameterMetadata()
		{
			object result = null;
			base.Reader.MoveToContent();
			if (base.Reader.NodeType == XmlNodeType.Element)
			{
				if (base.Reader.LocalName != this.id28_CommonMethodParameterMetadata || base.Reader.NamespaceURI != this.id4_Item)
				{
					throw base.CreateUnknownNodeException();
				}
				result = this.Read26_CommonMethodParameterMetadata(true, true);
			}
			else
			{
				base.UnknownNode(null, ":CommonMethodParameterMetadata");
			}
			return result;
		}

		// Token: 0x06005CA5 RID: 23717 RVA: 0x001F1C94 File Offset: 0x001EFE94
		public object Read76_StaticMethodParameterMetadata()
		{
			object result = null;
			base.Reader.MoveToContent();
			if (base.Reader.NodeType == XmlNodeType.Element)
			{
				if (base.Reader.LocalName != this.id29_StaticMethodParameterMetadata || base.Reader.NamespaceURI != this.id4_Item)
				{
					throw base.CreateUnknownNodeException();
				}
				result = this.Read27_StaticMethodParameterMetadata(true, true);
			}
			else
			{
				base.UnknownNode(null, ":StaticMethodParameterMetadata");
			}
			return result;
		}

		// Token: 0x06005CA6 RID: 23718 RVA: 0x001F1D04 File Offset: 0x001EFF04
		public object Read77_CmdletOutputMetadata()
		{
			object result = null;
			base.Reader.MoveToContent();
			if (base.Reader.NodeType == XmlNodeType.Element)
			{
				if (base.Reader.LocalName != this.id30_CmdletOutputMetadata || base.Reader.NamespaceURI != this.id4_Item)
				{
					throw base.CreateUnknownNodeException();
				}
				result = this.Read23_CmdletOutputMetadata(true, true);
			}
			else
			{
				base.UnknownNode(null, ":CmdletOutputMetadata");
			}
			return result;
		}

		// Token: 0x06005CA7 RID: 23719 RVA: 0x001F1D74 File Offset: 0x001EFF74
		public object Read78_Item()
		{
			object result = null;
			base.Reader.MoveToContent();
			if (base.Reader.NodeType == XmlNodeType.Element)
			{
				if (base.Reader.LocalName != this.id31_Item || base.Reader.NamespaceURI != this.id4_Item)
				{
					throw base.CreateUnknownNodeException();
				}
				result = this.Read25_Item(true, true);
			}
			else
			{
				base.UnknownNode(null, ":InstanceMethodParameterMetadata");
			}
			return result;
		}

		// Token: 0x06005CA8 RID: 23720 RVA: 0x001F1DE4 File Offset: 0x001EFFE4
		public object Read79_Item()
		{
			object result = null;
			base.Reader.MoveToContent();
			if (base.Reader.NodeType == XmlNodeType.Element)
			{
				if (base.Reader.LocalName != this.id32_Item || base.Reader.NamespaceURI != this.id4_Item)
				{
					throw base.CreateUnknownNodeException();
				}
				result = this.Read46_Item(true, true);
			}
			else
			{
				base.UnknownNode(null, ":CommonMethodMetadataReturnValue");
			}
			return result;
		}

		// Token: 0x06005CA9 RID: 23721 RVA: 0x001F1E54 File Offset: 0x001F0054
		public object Read80_InstanceMethodMetadata()
		{
			object result = null;
			base.Reader.MoveToContent();
			if (base.Reader.NodeType == XmlNodeType.Element)
			{
				if (base.Reader.LocalName != this.id33_InstanceMethodMetadata || base.Reader.NamespaceURI != this.id4_Item)
				{
					throw base.CreateUnknownNodeException();
				}
				result = this.Read30_InstanceMethodMetadata(true, true);
			}
			else
			{
				base.UnknownNode(null, ":InstanceMethodMetadata");
			}
			return result;
		}

		// Token: 0x06005CAA RID: 23722 RVA: 0x001F1EC4 File Offset: 0x001F00C4
		public object Read81_InstanceCmdletMetadata()
		{
			object result = null;
			base.Reader.MoveToContent();
			if (base.Reader.NodeType == XmlNodeType.Element)
			{
				if (base.Reader.LocalName != this.id34_InstanceCmdletMetadata || base.Reader.NamespaceURI != this.id4_Item)
				{
					throw base.CreateUnknownNodeException();
				}
				result = this.Read31_InstanceCmdletMetadata(true, true);
			}
			else
			{
				base.UnknownNode(null, ":InstanceCmdletMetadata");
			}
			return result;
		}

		// Token: 0x06005CAB RID: 23723 RVA: 0x001F1F34 File Offset: 0x001F0134
		public object Read82_PropertyQuery()
		{
			object result = null;
			base.Reader.MoveToContent();
			if (base.Reader.NodeType == XmlNodeType.Element)
			{
				if (base.Reader.LocalName != this.id35_PropertyQuery || base.Reader.NamespaceURI != this.id4_Item)
				{
					throw base.CreateUnknownNodeException();
				}
				result = this.Read14_PropertyQuery(true, true);
			}
			else
			{
				base.UnknownNode(null, ":PropertyQuery");
			}
			return result;
		}

		// Token: 0x06005CAC RID: 23724 RVA: 0x001F1FA4 File Offset: 0x001F01A4
		public object Read83_WildcardablePropertyQuery()
		{
			object result = null;
			base.Reader.MoveToContent();
			if (base.Reader.NodeType == XmlNodeType.Element)
			{
				if (base.Reader.LocalName != this.id36_WildcardablePropertyQuery || base.Reader.NamespaceURI != this.id4_Item)
				{
					throw base.CreateUnknownNodeException();
				}
				result = this.Read13_WildcardablePropertyQuery(true, true);
			}
			else
			{
				base.UnknownNode(null, ":WildcardablePropertyQuery");
			}
			return result;
		}

		// Token: 0x06005CAD RID: 23725 RVA: 0x001F2014 File Offset: 0x001F0214
		public object Read84_ItemsChoiceType()
		{
			object result = null;
			base.Reader.MoveToContent();
			if (base.Reader.NodeType == XmlNodeType.Element)
			{
				if (base.Reader.LocalName != this.id37_ItemsChoiceType || base.Reader.NamespaceURI != this.id4_Item)
				{
					throw base.CreateUnknownNodeException();
				}
				result = this.Read3_ItemsChoiceType(base.Reader.ReadElementString());
			}
			else
			{
				base.UnknownNode(null, ":ItemsChoiceType");
			}
			return result;
		}

		// Token: 0x06005CAE RID: 23726 RVA: 0x001F2090 File Offset: 0x001F0290
		public object Read85_ClassMetadataData()
		{
			object result = null;
			base.Reader.MoveToContent();
			if (base.Reader.NodeType == XmlNodeType.Element)
			{
				if (base.Reader.LocalName != this.id38_ClassMetadataData || base.Reader.NamespaceURI != this.id4_Item)
				{
					throw base.CreateUnknownNodeException();
				}
				result = this.Read47_ClassMetadataData(true, true);
			}
			else
			{
				base.UnknownNode(null, ":ClassMetadataData");
			}
			return result;
		}

		// Token: 0x06005CAF RID: 23727 RVA: 0x001F2100 File Offset: 0x001F0300
		public object Read86_EnumMetadataEnum()
		{
			object result = null;
			base.Reader.MoveToContent();
			if (base.Reader.NodeType == XmlNodeType.Element)
			{
				if (base.Reader.LocalName != this.id39_EnumMetadataEnum || base.Reader.NamespaceURI != this.id4_Item)
				{
					throw base.CreateUnknownNodeException();
				}
				result = this.Read48_EnumMetadataEnum(true, true);
			}
			else
			{
				base.UnknownNode(null, ":EnumMetadataEnum");
			}
			return result;
		}

		// Token: 0x06005CB0 RID: 23728 RVA: 0x001F2170 File Offset: 0x001F0370
		public object Read87_EnumMetadataEnumValue()
		{
			object result = null;
			base.Reader.MoveToContent();
			if (base.Reader.NodeType == XmlNodeType.Element)
			{
				if (base.Reader.LocalName != this.id40_EnumMetadataEnumValue || base.Reader.NamespaceURI != this.id4_Item)
				{
					throw base.CreateUnknownNodeException();
				}
				result = this.Read49_EnumMetadataEnumValue(true, true);
			}
			else
			{
				base.UnknownNode(null, ":EnumMetadataEnumValue");
			}
			return result;
		}

		// Token: 0x06005CB1 RID: 23729 RVA: 0x001F21E0 File Offset: 0x001F03E0
		private EnumMetadataEnumValue Read49_EnumMetadataEnumValue(bool isNullable, bool checkType)
		{
			XmlQualifiedName xmlQualifiedName = checkType ? base.GetXsiType() : null;
			bool flag = false;
			if (isNullable)
			{
				flag = base.ReadNull();
			}
			if (checkType && !(xmlQualifiedName == null) && (xmlQualifiedName.Name != this.id40_EnumMetadataEnumValue || xmlQualifiedName.Namespace != this.id2_Item))
			{
				throw base.CreateUnknownTypeException(xmlQualifiedName);
			}
			if (flag)
			{
				return null;
			}
			EnumMetadataEnumValue enumMetadataEnumValue = new EnumMetadataEnumValue();
			bool[] array = new bool[2];
			while (base.Reader.MoveToNextAttribute())
			{
				if (!array[0] && base.Reader.LocalName == this.id41_Name && base.Reader.NamespaceURI == this.id4_Item)
				{
					enumMetadataEnumValue.Name = base.Reader.Value;
					array[0] = true;
				}
				else if (!array[1] && base.Reader.LocalName == this.id42_Value && base.Reader.NamespaceURI == this.id4_Item)
				{
					enumMetadataEnumValue.Value = base.CollapseWhitespace(base.Reader.Value);
					array[1] = true;
				}
				else if (!base.IsXmlnsAttribute(base.Reader.Name))
				{
					base.UnknownNode(enumMetadataEnumValue, ":Name, :Value");
				}
			}
			base.Reader.MoveToElement();
			if (base.Reader.IsEmptyElement)
			{
				base.Reader.Skip();
				return enumMetadataEnumValue;
			}
			base.Reader.ReadStartElement();
			base.Reader.MoveToContent();
			int num = 0;
			int readerCount = base.ReaderCount;
			while (base.Reader.NodeType != XmlNodeType.EndElement && base.Reader.NodeType != XmlNodeType.None)
			{
				if (base.Reader.NodeType == XmlNodeType.Element)
				{
					base.UnknownNode(enumMetadataEnumValue, "");
				}
				else
				{
					base.UnknownNode(enumMetadataEnumValue, "");
				}
				base.Reader.MoveToContent();
				base.CheckReaderCount(ref num, ref readerCount);
			}
			base.ReadEndElement();
			return enumMetadataEnumValue;
		}

		// Token: 0x06005CB2 RID: 23730 RVA: 0x001F23B4 File Offset: 0x001F05B4
		private EnumMetadataEnum Read48_EnumMetadataEnum(bool isNullable, bool checkType)
		{
			XmlQualifiedName xmlQualifiedName = checkType ? base.GetXsiType() : null;
			bool flag = false;
			if (isNullable)
			{
				flag = base.ReadNull();
			}
			if (checkType && !(xmlQualifiedName == null) && (xmlQualifiedName.Name != this.id39_EnumMetadataEnum || xmlQualifiedName.Namespace != this.id2_Item))
			{
				throw base.CreateUnknownTypeException(xmlQualifiedName);
			}
			if (flag)
			{
				return null;
			}
			EnumMetadataEnum enumMetadataEnum = new EnumMetadataEnum();
			EnumMetadataEnumValue[] array = null;
			int num = 0;
			bool[] array2 = new bool[4];
			while (base.Reader.MoveToNextAttribute())
			{
				if (!array2[1] && base.Reader.LocalName == this.id43_EnumName && base.Reader.NamespaceURI == this.id4_Item)
				{
					enumMetadataEnum.EnumName = base.Reader.Value;
					array2[1] = true;
				}
				else if (!array2[2] && base.Reader.LocalName == this.id44_UnderlyingType && base.Reader.NamespaceURI == this.id4_Item)
				{
					enumMetadataEnum.UnderlyingType = base.Reader.Value;
					array2[2] = true;
				}
				else if (!array2[3] && base.Reader.LocalName == this.id45_BitwiseFlags && base.Reader.NamespaceURI == this.id4_Item)
				{
					enumMetadataEnum.BitwiseFlags = XmlConvert.ToBoolean(base.Reader.Value);
					enumMetadataEnum.BitwiseFlagsSpecified = true;
					array2[3] = true;
				}
				else if (!base.IsXmlnsAttribute(base.Reader.Name))
				{
					base.UnknownNode(enumMetadataEnum, ":EnumName, :UnderlyingType, :BitwiseFlags");
				}
			}
			base.Reader.MoveToElement();
			if (base.Reader.IsEmptyElement)
			{
				base.Reader.Skip();
				enumMetadataEnum.Value = (EnumMetadataEnumValue[])base.ShrinkArray(array, num, typeof(EnumMetadataEnumValue), true);
				return enumMetadataEnum;
			}
			base.Reader.ReadStartElement();
			base.Reader.MoveToContent();
			int num2 = 0;
			int readerCount = base.ReaderCount;
			while (base.Reader.NodeType != XmlNodeType.EndElement && base.Reader.NodeType != XmlNodeType.None)
			{
				if (base.Reader.NodeType == XmlNodeType.Element)
				{
					if (base.Reader.LocalName == this.id42_Value && base.Reader.NamespaceURI == this.id2_Item)
					{
						array = (EnumMetadataEnumValue[])base.EnsureArrayIndex(array, num, typeof(EnumMetadataEnumValue));
						array[num++] = this.Read37_EnumMetadataEnumValue(false, true);
					}
					else
					{
						base.UnknownNode(enumMetadataEnum, "http://schemas.microsoft.com/cmdlets-over-objects/2009/11:Value");
					}
				}
				else
				{
					base.UnknownNode(enumMetadataEnum, "http://schemas.microsoft.com/cmdlets-over-objects/2009/11:Value");
				}
				base.Reader.MoveToContent();
				base.CheckReaderCount(ref num2, ref readerCount);
			}
			enumMetadataEnum.Value = (EnumMetadataEnumValue[])base.ShrinkArray(array, num, typeof(EnumMetadataEnumValue), true);
			base.ReadEndElement();
			return enumMetadataEnum;
		}

		// Token: 0x06005CB3 RID: 23731 RVA: 0x001F2674 File Offset: 0x001F0874
		private EnumMetadataEnumValue Read37_EnumMetadataEnumValue(bool isNullable, bool checkType)
		{
			XmlQualifiedName xmlQualifiedName = checkType ? base.GetXsiType() : null;
			bool flag = false;
			if (isNullable)
			{
				flag = base.ReadNull();
			}
			if (checkType && !(xmlQualifiedName == null) && (xmlQualifiedName.Name != this.id4_Item || xmlQualifiedName.Namespace != this.id2_Item))
			{
				throw base.CreateUnknownTypeException(xmlQualifiedName);
			}
			if (flag)
			{
				return null;
			}
			EnumMetadataEnumValue enumMetadataEnumValue = new EnumMetadataEnumValue();
			bool[] array = new bool[2];
			while (base.Reader.MoveToNextAttribute())
			{
				if (!array[0] && base.Reader.LocalName == this.id41_Name && base.Reader.NamespaceURI == this.id4_Item)
				{
					enumMetadataEnumValue.Name = base.Reader.Value;
					array[0] = true;
				}
				else if (!array[1] && base.Reader.LocalName == this.id42_Value && base.Reader.NamespaceURI == this.id4_Item)
				{
					enumMetadataEnumValue.Value = base.CollapseWhitespace(base.Reader.Value);
					array[1] = true;
				}
				else if (!base.IsXmlnsAttribute(base.Reader.Name))
				{
					base.UnknownNode(enumMetadataEnumValue, ":Name, :Value");
				}
			}
			base.Reader.MoveToElement();
			if (base.Reader.IsEmptyElement)
			{
				base.Reader.Skip();
				return enumMetadataEnumValue;
			}
			base.Reader.ReadStartElement();
			base.Reader.MoveToContent();
			int num = 0;
			int readerCount = base.ReaderCount;
			while (base.Reader.NodeType != XmlNodeType.EndElement && base.Reader.NodeType != XmlNodeType.None)
			{
				if (base.Reader.NodeType == XmlNodeType.Element)
				{
					base.UnknownNode(enumMetadataEnumValue, "");
				}
				else
				{
					base.UnknownNode(enumMetadataEnumValue, "");
				}
				base.Reader.MoveToContent();
				base.CheckReaderCount(ref num, ref readerCount);
			}
			base.ReadEndElement();
			return enumMetadataEnumValue;
		}

		// Token: 0x06005CB4 RID: 23732 RVA: 0x001F2848 File Offset: 0x001F0A48
		private ClassMetadataData Read47_ClassMetadataData(bool isNullable, bool checkType)
		{
			XmlQualifiedName xmlQualifiedName = checkType ? base.GetXsiType() : null;
			bool flag = false;
			if (isNullable)
			{
				flag = base.ReadNull();
			}
			if (checkType && !(xmlQualifiedName == null) && (xmlQualifiedName.Name != this.id38_ClassMetadataData || xmlQualifiedName.Namespace != this.id2_Item))
			{
				throw base.CreateUnknownTypeException(xmlQualifiedName);
			}
			if (flag)
			{
				return null;
			}
			ClassMetadataData classMetadataData = new ClassMetadataData();
			bool[] array = new bool[2];
			while (base.Reader.MoveToNextAttribute())
			{
				if (!array[0] && base.Reader.LocalName == this.id41_Name && base.Reader.NamespaceURI == this.id4_Item)
				{
					classMetadataData.Name = base.Reader.Value;
					array[0] = true;
				}
				else if (!base.IsXmlnsAttribute(base.Reader.Name))
				{
					base.UnknownNode(classMetadataData, ":Name");
				}
			}
			base.Reader.MoveToElement();
			if (base.Reader.IsEmptyElement)
			{
				base.Reader.Skip();
				return classMetadataData;
			}
			base.Reader.ReadStartElement();
			base.Reader.MoveToContent();
			int num = 0;
			int readerCount = base.ReaderCount;
			while (base.Reader.NodeType != XmlNodeType.EndElement && base.Reader.NodeType != XmlNodeType.None)
			{
				string value = null;
				if (base.Reader.NodeType == XmlNodeType.Element)
				{
					base.UnknownNode(classMetadataData, "");
				}
				else if (base.Reader.NodeType == XmlNodeType.Text || base.Reader.NodeType == XmlNodeType.CDATA || base.Reader.NodeType == XmlNodeType.Whitespace || base.Reader.NodeType == XmlNodeType.SignificantWhitespace)
				{
					value = base.ReadString(value, false);
					classMetadataData.Value = value;
				}
				else
				{
					base.UnknownNode(classMetadataData, "");
				}
				base.Reader.MoveToContent();
				base.CheckReaderCount(ref num, ref readerCount);
			}
			base.ReadEndElement();
			return classMetadataData;
		}

		// Token: 0x06005CB5 RID: 23733 RVA: 0x001F2A24 File Offset: 0x001F0C24
		private ItemsChoiceType Read3_ItemsChoiceType(string s)
		{
			if (s != null)
			{
				if (s == "ExcludeQuery")
				{
					return ItemsChoiceType.ExcludeQuery;
				}
				if (s == "MaxValueQuery")
				{
					return ItemsChoiceType.MaxValueQuery;
				}
				if (s == "MinValueQuery")
				{
					return ItemsChoiceType.MinValueQuery;
				}
				if (s == "RegularQuery")
				{
					return ItemsChoiceType.RegularQuery;
				}
			}
			throw base.CreateUnknownConstantException(s, typeof(ItemsChoiceType));
		}

		// Token: 0x06005CB6 RID: 23734 RVA: 0x001F2A88 File Offset: 0x001F0C88
		private WildcardablePropertyQuery Read13_WildcardablePropertyQuery(bool isNullable, bool checkType)
		{
			XmlQualifiedName xmlQualifiedName = checkType ? base.GetXsiType() : null;
			bool flag = false;
			if (isNullable)
			{
				flag = base.ReadNull();
			}
			if (checkType && !(xmlQualifiedName == null) && (xmlQualifiedName.Name != this.id36_WildcardablePropertyQuery || xmlQualifiedName.Namespace != this.id2_Item))
			{
				throw base.CreateUnknownTypeException(xmlQualifiedName);
			}
			if (flag)
			{
				return null;
			}
			WildcardablePropertyQuery wildcardablePropertyQuery = new WildcardablePropertyQuery();
			bool[] array = new bool[2];
			while (base.Reader.MoveToNextAttribute())
			{
				if (!array[1] && base.Reader.LocalName == this.id46_AllowGlobbing && base.Reader.NamespaceURI == this.id4_Item)
				{
					wildcardablePropertyQuery.AllowGlobbing = XmlConvert.ToBoolean(base.Reader.Value);
					wildcardablePropertyQuery.AllowGlobbingSpecified = true;
					array[1] = true;
				}
				else if (!base.IsXmlnsAttribute(base.Reader.Name))
				{
					base.UnknownNode(wildcardablePropertyQuery, ":AllowGlobbing");
				}
			}
			base.Reader.MoveToElement();
			if (base.Reader.IsEmptyElement)
			{
				base.Reader.Skip();
				return wildcardablePropertyQuery;
			}
			base.Reader.ReadStartElement();
			base.Reader.MoveToContent();
			int num = 0;
			int readerCount = base.ReaderCount;
			while (base.Reader.NodeType != XmlNodeType.EndElement && base.Reader.NodeType != XmlNodeType.None)
			{
				if (base.Reader.NodeType == XmlNodeType.Element)
				{
					if (!array[0] && base.Reader.LocalName == this.id11_CmdletParameterMetadata && base.Reader.NamespaceURI == this.id2_Item)
					{
						wildcardablePropertyQuery.CmdletParameterMetadata = this.Read12_Item(false, true);
						array[0] = true;
					}
					else
					{
						base.UnknownNode(wildcardablePropertyQuery, "http://schemas.microsoft.com/cmdlets-over-objects/2009/11:CmdletParameterMetadata");
					}
				}
				else
				{
					base.UnknownNode(wildcardablePropertyQuery, "http://schemas.microsoft.com/cmdlets-over-objects/2009/11:CmdletParameterMetadata");
				}
				base.Reader.MoveToContent();
				base.CheckReaderCount(ref num, ref readerCount);
			}
			base.ReadEndElement();
			return wildcardablePropertyQuery;
		}

		// Token: 0x06005CB7 RID: 23735 RVA: 0x001F2C5C File Offset: 0x001F0E5C
		private CmdletParameterMetadataForGetCmdletFilteringParameter Read12_Item(bool isNullable, bool checkType)
		{
			XmlQualifiedName xmlQualifiedName = checkType ? base.GetXsiType() : null;
			bool flag = false;
			if (isNullable)
			{
				flag = base.ReadNull();
			}
			if (checkType && !(xmlQualifiedName == null) && (xmlQualifiedName.Name != this.id13_Item || xmlQualifiedName.Namespace != this.id2_Item))
			{
				throw base.CreateUnknownTypeException(xmlQualifiedName);
			}
			if (flag)
			{
				return null;
			}
			CmdletParameterMetadataForGetCmdletFilteringParameter cmdletParameterMetadataForGetCmdletFilteringParameter = new CmdletParameterMetadataForGetCmdletFilteringParameter();
			string[] array = null;
			int num = 0;
			string[] array2 = null;
			int num2 = 0;
			bool[] array3 = new bool[18];
			while (base.Reader.MoveToNextAttribute())
			{
				if (!array3[10] && base.Reader.LocalName == this.id47_IsMandatory && base.Reader.NamespaceURI == this.id4_Item)
				{
					cmdletParameterMetadataForGetCmdletFilteringParameter.IsMandatory = XmlConvert.ToBoolean(base.Reader.Value);
					cmdletParameterMetadataForGetCmdletFilteringParameter.IsMandatorySpecified = true;
					array3[10] = true;
				}
				else if (base.Reader.LocalName == this.id48_Aliases && base.Reader.NamespaceURI == this.id4_Item)
				{
					string value = base.Reader.Value;
					string[] array4 = value.Split(null);
					for (int i = 0; i < array4.Length; i++)
					{
						array = (string[])base.EnsureArrayIndex(array, num, typeof(string));
						array[num++] = array4[i];
					}
				}
				else if (!array3[12] && base.Reader.LocalName == this.id49_PSName && base.Reader.NamespaceURI == this.id4_Item)
				{
					cmdletParameterMetadataForGetCmdletFilteringParameter.PSName = base.Reader.Value;
					array3[12] = true;
				}
				else if (!array3[13] && base.Reader.LocalName == this.id50_Position && base.Reader.NamespaceURI == this.id4_Item)
				{
					cmdletParameterMetadataForGetCmdletFilteringParameter.Position = base.CollapseWhitespace(base.Reader.Value);
					array3[13] = true;
				}
				else if (!array3[14] && base.Reader.LocalName == this.id51_ValueFromPipeline && base.Reader.NamespaceURI == this.id4_Item)
				{
					cmdletParameterMetadataForGetCmdletFilteringParameter.ValueFromPipeline = XmlConvert.ToBoolean(base.Reader.Value);
					cmdletParameterMetadataForGetCmdletFilteringParameter.ValueFromPipelineSpecified = true;
					array3[14] = true;
				}
				else if (!array3[15] && base.Reader.LocalName == this.id52_Item && base.Reader.NamespaceURI == this.id4_Item)
				{
					cmdletParameterMetadataForGetCmdletFilteringParameter.ValueFromPipelineByPropertyName = XmlConvert.ToBoolean(base.Reader.Value);
					cmdletParameterMetadataForGetCmdletFilteringParameter.ValueFromPipelineByPropertyNameSpecified = true;
					array3[15] = true;
				}
				else if (base.Reader.LocalName == this.id53_CmdletParameterSets && base.Reader.NamespaceURI == this.id4_Item)
				{
					string value2 = base.Reader.Value;
					string[] array5 = value2.Split(null);
					for (int j = 0; j < array5.Length; j++)
					{
						array2 = (string[])base.EnsureArrayIndex(array2, num2, typeof(string));
						array2[num2++] = array5[j];
					}
				}
				else if (!array3[17] && base.Reader.LocalName == this.id54_ErrorOnNoMatch && base.Reader.NamespaceURI == this.id4_Item)
				{
					cmdletParameterMetadataForGetCmdletFilteringParameter.ErrorOnNoMatch = XmlConvert.ToBoolean(base.Reader.Value);
					cmdletParameterMetadataForGetCmdletFilteringParameter.ErrorOnNoMatchSpecified = true;
					array3[17] = true;
				}
				else if (!base.IsXmlnsAttribute(base.Reader.Name))
				{
					base.UnknownNode(cmdletParameterMetadataForGetCmdletFilteringParameter, ":IsMandatory, :Aliases, :PSName, :Position, :ValueFromPipeline, :ValueFromPipelineByPropertyName, :CmdletParameterSets, :ErrorOnNoMatch");
				}
			}
			base.Reader.MoveToElement();
			if (base.Reader.IsEmptyElement)
			{
				base.Reader.Skip();
				cmdletParameterMetadataForGetCmdletFilteringParameter.Aliases = (string[])base.ShrinkArray(array, num, typeof(string), true);
				cmdletParameterMetadataForGetCmdletFilteringParameter.CmdletParameterSets = (string[])base.ShrinkArray(array2, num2, typeof(string), true);
				return cmdletParameterMetadataForGetCmdletFilteringParameter;
			}
			base.Reader.ReadStartElement();
			base.Reader.MoveToContent();
			int num3 = 0;
			int readerCount = base.ReaderCount;
			while (base.Reader.NodeType != XmlNodeType.EndElement && base.Reader.NodeType != XmlNodeType.None)
			{
				if (base.Reader.NodeType == XmlNodeType.Element)
				{
					if (!array3[0] && base.Reader.LocalName == this.id55_AllowEmptyCollection && base.Reader.NamespaceURI == this.id2_Item)
					{
						cmdletParameterMetadataForGetCmdletFilteringParameter.AllowEmptyCollection = this.Read1_Object(false, true);
						array3[0] = true;
					}
					else if (!array3[1] && base.Reader.LocalName == this.id56_AllowEmptyString && base.Reader.NamespaceURI == this.id2_Item)
					{
						cmdletParameterMetadataForGetCmdletFilteringParameter.AllowEmptyString = this.Read1_Object(false, true);
						array3[1] = true;
					}
					else if (!array3[2] && base.Reader.LocalName == this.id57_AllowNull && base.Reader.NamespaceURI == this.id2_Item)
					{
						cmdletParameterMetadataForGetCmdletFilteringParameter.AllowNull = this.Read1_Object(false, true);
						array3[2] = true;
					}
					else if (!array3[3] && base.Reader.LocalName == this.id58_ValidateNotNull && base.Reader.NamespaceURI == this.id2_Item)
					{
						cmdletParameterMetadataForGetCmdletFilteringParameter.ValidateNotNull = this.Read1_Object(false, true);
						array3[3] = true;
					}
					else if (!array3[4] && base.Reader.LocalName == this.id59_ValidateNotNullOrEmpty && base.Reader.NamespaceURI == this.id2_Item)
					{
						cmdletParameterMetadataForGetCmdletFilteringParameter.ValidateNotNullOrEmpty = this.Read1_Object(false, true);
						array3[4] = true;
					}
					else if (!array3[5] && base.Reader.LocalName == this.id60_ValidateCount && base.Reader.NamespaceURI == this.id2_Item)
					{
						cmdletParameterMetadataForGetCmdletFilteringParameter.ValidateCount = this.Read4_Item(false, true);
						array3[5] = true;
					}
					else if (!array3[6] && base.Reader.LocalName == this.id61_ValidateLength && base.Reader.NamespaceURI == this.id2_Item)
					{
						cmdletParameterMetadataForGetCmdletFilteringParameter.ValidateLength = this.Read5_Item(false, true);
						array3[6] = true;
					}
					else if (!array3[7] && base.Reader.LocalName == this.id62_ValidateRange && base.Reader.NamespaceURI == this.id2_Item)
					{
						cmdletParameterMetadataForGetCmdletFilteringParameter.ValidateRange = this.Read6_Item(false, true);
						array3[7] = true;
					}
					else if (base.Reader.LocalName == this.id63_ValidateSet && base.Reader.NamespaceURI == this.id2_Item)
					{
						if (!base.ReadNull())
						{
							string[] array6 = null;
							int num4 = 0;
							if (base.Reader.IsEmptyElement)
							{
								base.Reader.Skip();
							}
							else
							{
								base.Reader.ReadStartElement();
								base.Reader.MoveToContent();
								int num5 = 0;
								int readerCount2 = base.ReaderCount;
								while (base.Reader.NodeType != XmlNodeType.EndElement && base.Reader.NodeType != XmlNodeType.None)
								{
									if (base.Reader.NodeType == XmlNodeType.Element)
									{
										if (base.Reader.LocalName == this.id64_AllowedValue && base.Reader.NamespaceURI == this.id2_Item)
										{
											array6 = (string[])base.EnsureArrayIndex(array6, num4, typeof(string));
											array6[num4++] = base.Reader.ReadElementString();
										}
										else
										{
											base.UnknownNode(null, "http://schemas.microsoft.com/cmdlets-over-objects/2009/11:AllowedValue");
										}
									}
									else
									{
										base.UnknownNode(null, "http://schemas.microsoft.com/cmdlets-over-objects/2009/11:AllowedValue");
									}
									base.Reader.MoveToContent();
									base.CheckReaderCount(ref num5, ref readerCount2);
								}
								base.ReadEndElement();
							}
							cmdletParameterMetadataForGetCmdletFilteringParameter.ValidateSet = (string[])base.ShrinkArray(array6, num4, typeof(string), false);
						}
					}
					else if (!array3[9] && base.Reader.LocalName == this.id65_Obsolete && base.Reader.NamespaceURI == this.id2_Item)
					{
						cmdletParameterMetadataForGetCmdletFilteringParameter.Obsolete = this.Read7_ObsoleteAttributeMetadata(false, true);
						array3[9] = true;
					}
					else
					{
						base.UnknownNode(cmdletParameterMetadataForGetCmdletFilteringParameter, "http://schemas.microsoft.com/cmdlets-over-objects/2009/11:AllowEmptyCollection, http://schemas.microsoft.com/cmdlets-over-objects/2009/11:AllowEmptyString, http://schemas.microsoft.com/cmdlets-over-objects/2009/11:AllowNull, http://schemas.microsoft.com/cmdlets-over-objects/2009/11:ValidateNotNull, http://schemas.microsoft.com/cmdlets-over-objects/2009/11:ValidateNotNullOrEmpty, http://schemas.microsoft.com/cmdlets-over-objects/2009/11:ValidateCount, http://schemas.microsoft.com/cmdlets-over-objects/2009/11:ValidateLength, http://schemas.microsoft.com/cmdlets-over-objects/2009/11:ValidateRange, http://schemas.microsoft.com/cmdlets-over-objects/2009/11:ValidateSet, http://schemas.microsoft.com/cmdlets-over-objects/2009/11:Obsolete");
					}
				}
				else
				{
					base.UnknownNode(cmdletParameterMetadataForGetCmdletFilteringParameter, "http://schemas.microsoft.com/cmdlets-over-objects/2009/11:AllowEmptyCollection, http://schemas.microsoft.com/cmdlets-over-objects/2009/11:AllowEmptyString, http://schemas.microsoft.com/cmdlets-over-objects/2009/11:AllowNull, http://schemas.microsoft.com/cmdlets-over-objects/2009/11:ValidateNotNull, http://schemas.microsoft.com/cmdlets-over-objects/2009/11:ValidateNotNullOrEmpty, http://schemas.microsoft.com/cmdlets-over-objects/2009/11:ValidateCount, http://schemas.microsoft.com/cmdlets-over-objects/2009/11:ValidateLength, http://schemas.microsoft.com/cmdlets-over-objects/2009/11:ValidateRange, http://schemas.microsoft.com/cmdlets-over-objects/2009/11:ValidateSet, http://schemas.microsoft.com/cmdlets-over-objects/2009/11:Obsolete");
				}
				base.Reader.MoveToContent();
				base.CheckReaderCount(ref num3, ref readerCount);
			}
			cmdletParameterMetadataForGetCmdletFilteringParameter.Aliases = (string[])base.ShrinkArray(array, num, typeof(string), true);
			cmdletParameterMetadataForGetCmdletFilteringParameter.CmdletParameterSets = (string[])base.ShrinkArray(array2, num2, typeof(string), true);
			base.ReadEndElement();
			return cmdletParameterMetadataForGetCmdletFilteringParameter;
		}

		// Token: 0x06005CB8 RID: 23736 RVA: 0x001F34DC File Offset: 0x001F16DC
		private ObsoleteAttributeMetadata Read7_ObsoleteAttributeMetadata(bool isNullable, bool checkType)
		{
			XmlQualifiedName xmlQualifiedName = checkType ? base.GetXsiType() : null;
			bool flag = false;
			if (isNullable)
			{
				flag = base.ReadNull();
			}
			if (checkType && !(xmlQualifiedName == null) && (xmlQualifiedName.Name != this.id17_ObsoleteAttributeMetadata || xmlQualifiedName.Namespace != this.id2_Item))
			{
				throw base.CreateUnknownTypeException(xmlQualifiedName);
			}
			if (flag)
			{
				return null;
			}
			ObsoleteAttributeMetadata obsoleteAttributeMetadata = new ObsoleteAttributeMetadata();
			bool[] array = new bool[1];
			while (base.Reader.MoveToNextAttribute())
			{
				if (!array[0] && base.Reader.LocalName == this.id66_Message && base.Reader.NamespaceURI == this.id4_Item)
				{
					obsoleteAttributeMetadata.Message = base.Reader.Value;
					array[0] = true;
				}
				else if (!base.IsXmlnsAttribute(base.Reader.Name))
				{
					base.UnknownNode(obsoleteAttributeMetadata, ":Message");
				}
			}
			base.Reader.MoveToElement();
			if (base.Reader.IsEmptyElement)
			{
				base.Reader.Skip();
				return obsoleteAttributeMetadata;
			}
			base.Reader.ReadStartElement();
			base.Reader.MoveToContent();
			int num = 0;
			int readerCount = base.ReaderCount;
			while (base.Reader.NodeType != XmlNodeType.EndElement && base.Reader.NodeType != XmlNodeType.None)
			{
				if (base.Reader.NodeType == XmlNodeType.Element)
				{
					base.UnknownNode(obsoleteAttributeMetadata, "");
				}
				else
				{
					base.UnknownNode(obsoleteAttributeMetadata, "");
				}
				base.Reader.MoveToContent();
				base.CheckReaderCount(ref num, ref readerCount);
			}
			base.ReadEndElement();
			return obsoleteAttributeMetadata;
		}

		// Token: 0x06005CB9 RID: 23737 RVA: 0x001F3660 File Offset: 0x001F1860
		private CmdletParameterMetadataValidateRange Read6_Item(bool isNullable, bool checkType)
		{
			XmlQualifiedName xmlQualifiedName = checkType ? base.GetXsiType() : null;
			bool flag = false;
			if (isNullable)
			{
				flag = base.ReadNull();
			}
			if (checkType && !(xmlQualifiedName == null) && (xmlQualifiedName.Name != this.id4_Item || xmlQualifiedName.Namespace != this.id2_Item))
			{
				throw base.CreateUnknownTypeException(xmlQualifiedName);
			}
			if (flag)
			{
				return null;
			}
			CmdletParameterMetadataValidateRange cmdletParameterMetadataValidateRange = new CmdletParameterMetadataValidateRange();
			bool[] array = new bool[2];
			while (base.Reader.MoveToNextAttribute())
			{
				if (!array[0] && base.Reader.LocalName == this.id67_Min && base.Reader.NamespaceURI == this.id4_Item)
				{
					cmdletParameterMetadataValidateRange.Min = base.CollapseWhitespace(base.Reader.Value);
					array[0] = true;
				}
				else if (!array[1] && base.Reader.LocalName == this.id68_Max && base.Reader.NamespaceURI == this.id4_Item)
				{
					cmdletParameterMetadataValidateRange.Max = base.CollapseWhitespace(base.Reader.Value);
					array[1] = true;
				}
				else if (!base.IsXmlnsAttribute(base.Reader.Name))
				{
					base.UnknownNode(cmdletParameterMetadataValidateRange, ":Min, :Max");
				}
			}
			base.Reader.MoveToElement();
			if (base.Reader.IsEmptyElement)
			{
				base.Reader.Skip();
				return cmdletParameterMetadataValidateRange;
			}
			base.Reader.ReadStartElement();
			base.Reader.MoveToContent();
			int num = 0;
			int readerCount = base.ReaderCount;
			while (base.Reader.NodeType != XmlNodeType.EndElement && base.Reader.NodeType != XmlNodeType.None)
			{
				if (base.Reader.NodeType == XmlNodeType.Element)
				{
					base.UnknownNode(cmdletParameterMetadataValidateRange, "");
				}
				else
				{
					base.UnknownNode(cmdletParameterMetadataValidateRange, "");
				}
				base.Reader.MoveToContent();
				base.CheckReaderCount(ref num, ref readerCount);
			}
			base.ReadEndElement();
			return cmdletParameterMetadataValidateRange;
		}

		// Token: 0x06005CBA RID: 23738 RVA: 0x001F3838 File Offset: 0x001F1A38
		private CmdletParameterMetadataValidateLength Read5_Item(bool isNullable, bool checkType)
		{
			XmlQualifiedName xmlQualifiedName = checkType ? base.GetXsiType() : null;
			bool flag = false;
			if (isNullable)
			{
				flag = base.ReadNull();
			}
			if (checkType && !(xmlQualifiedName == null) && (xmlQualifiedName.Name != this.id4_Item || xmlQualifiedName.Namespace != this.id2_Item))
			{
				throw base.CreateUnknownTypeException(xmlQualifiedName);
			}
			if (flag)
			{
				return null;
			}
			CmdletParameterMetadataValidateLength cmdletParameterMetadataValidateLength = new CmdletParameterMetadataValidateLength();
			bool[] array = new bool[2];
			while (base.Reader.MoveToNextAttribute())
			{
				if (!array[0] && base.Reader.LocalName == this.id67_Min && base.Reader.NamespaceURI == this.id4_Item)
				{
					cmdletParameterMetadataValidateLength.Min = base.CollapseWhitespace(base.Reader.Value);
					array[0] = true;
				}
				else if (!array[1] && base.Reader.LocalName == this.id68_Max && base.Reader.NamespaceURI == this.id4_Item)
				{
					cmdletParameterMetadataValidateLength.Max = base.CollapseWhitespace(base.Reader.Value);
					array[1] = true;
				}
				else if (!base.IsXmlnsAttribute(base.Reader.Name))
				{
					base.UnknownNode(cmdletParameterMetadataValidateLength, ":Min, :Max");
				}
			}
			base.Reader.MoveToElement();
			if (base.Reader.IsEmptyElement)
			{
				base.Reader.Skip();
				return cmdletParameterMetadataValidateLength;
			}
			base.Reader.ReadStartElement();
			base.Reader.MoveToContent();
			int num = 0;
			int readerCount = base.ReaderCount;
			while (base.Reader.NodeType != XmlNodeType.EndElement && base.Reader.NodeType != XmlNodeType.None)
			{
				if (base.Reader.NodeType == XmlNodeType.Element)
				{
					base.UnknownNode(cmdletParameterMetadataValidateLength, "");
				}
				else
				{
					base.UnknownNode(cmdletParameterMetadataValidateLength, "");
				}
				base.Reader.MoveToContent();
				base.CheckReaderCount(ref num, ref readerCount);
			}
			base.ReadEndElement();
			return cmdletParameterMetadataValidateLength;
		}

		// Token: 0x06005CBB RID: 23739 RVA: 0x001F3A10 File Offset: 0x001F1C10
		private CmdletParameterMetadataValidateCount Read4_Item(bool isNullable, bool checkType)
		{
			XmlQualifiedName xmlQualifiedName = checkType ? base.GetXsiType() : null;
			bool flag = false;
			if (isNullable)
			{
				flag = base.ReadNull();
			}
			if (checkType && !(xmlQualifiedName == null) && (xmlQualifiedName.Name != this.id4_Item || xmlQualifiedName.Namespace != this.id2_Item))
			{
				throw base.CreateUnknownTypeException(xmlQualifiedName);
			}
			if (flag)
			{
				return null;
			}
			CmdletParameterMetadataValidateCount cmdletParameterMetadataValidateCount = new CmdletParameterMetadataValidateCount();
			bool[] array = new bool[2];
			while (base.Reader.MoveToNextAttribute())
			{
				if (!array[0] && base.Reader.LocalName == this.id67_Min && base.Reader.NamespaceURI == this.id4_Item)
				{
					cmdletParameterMetadataValidateCount.Min = base.CollapseWhitespace(base.Reader.Value);
					array[0] = true;
				}
				else if (!array[1] && base.Reader.LocalName == this.id68_Max && base.Reader.NamespaceURI == this.id4_Item)
				{
					cmdletParameterMetadataValidateCount.Max = base.CollapseWhitespace(base.Reader.Value);
					array[1] = true;
				}
				else if (!base.IsXmlnsAttribute(base.Reader.Name))
				{
					base.UnknownNode(cmdletParameterMetadataValidateCount, ":Min, :Max");
				}
			}
			base.Reader.MoveToElement();
			if (base.Reader.IsEmptyElement)
			{
				base.Reader.Skip();
				return cmdletParameterMetadataValidateCount;
			}
			base.Reader.ReadStartElement();
			base.Reader.MoveToContent();
			int num = 0;
			int readerCount = base.ReaderCount;
			while (base.Reader.NodeType != XmlNodeType.EndElement && base.Reader.NodeType != XmlNodeType.None)
			{
				if (base.Reader.NodeType == XmlNodeType.Element)
				{
					base.UnknownNode(cmdletParameterMetadataValidateCount, "");
				}
				else
				{
					base.UnknownNode(cmdletParameterMetadataValidateCount, "");
				}
				base.Reader.MoveToContent();
				base.CheckReaderCount(ref num, ref readerCount);
			}
			base.ReadEndElement();
			return cmdletParameterMetadataValidateCount;
		}

		// Token: 0x06005CBC RID: 23740 RVA: 0x001F3BE8 File Offset: 0x001F1DE8
		private object Read1_Object(bool isNullable, bool checkType)
		{
			XmlQualifiedName xmlQualifiedName = checkType ? base.GetXsiType() : null;
			bool flag = false;
			if (isNullable)
			{
				flag = base.ReadNull();
			}
			if (checkType)
			{
				if (flag)
				{
					if (xmlQualifiedName != null)
					{
						return base.ReadTypedNull(xmlQualifiedName);
					}
					return null;
				}
				else
				{
					if (xmlQualifiedName == null)
					{
						return base.ReadTypedPrimitive(new XmlQualifiedName("anyType", "http://www.w3.org/2001/XMLSchema"));
					}
					if (xmlQualifiedName.Name == this.id40_EnumMetadataEnumValue && xmlQualifiedName.Namespace == this.id2_Item)
					{
						return this.Read49_EnumMetadataEnumValue(isNullable, false);
					}
					if (xmlQualifiedName.Name == this.id39_EnumMetadataEnum && xmlQualifiedName.Namespace == this.id2_Item)
					{
						return this.Read48_EnumMetadataEnum(isNullable, false);
					}
					if (xmlQualifiedName.Name == this.id38_ClassMetadataData && xmlQualifiedName.Namespace == this.id2_Item)
					{
						return this.Read47_ClassMetadataData(isNullable, false);
					}
					if (xmlQualifiedName.Name == this.id32_Item && xmlQualifiedName.Namespace == this.id2_Item)
					{
						return this.Read46_Item(isNullable, false);
					}
					if (xmlQualifiedName.Name == this.id16_Item && xmlQualifiedName.Namespace == this.id2_Item)
					{
						return this.Read44_Item(isNullable, false);
					}
					if (xmlQualifiedName.Name == this.id15_Item && xmlQualifiedName.Namespace == this.id2_Item)
					{
						return this.Read43_Item(isNullable, false);
					}
					if (xmlQualifiedName.Name == this.id14_Item && xmlQualifiedName.Namespace == this.id2_Item)
					{
						return this.Read42_Item(isNullable, false);
					}
					if (xmlQualifiedName.Name == this.id10_AssociationAssociatedInstance && xmlQualifiedName.Namespace == this.id2_Item)
					{
						return this.Read41_AssociationAssociatedInstance(isNullable, false);
					}
					if (xmlQualifiedName.Name == this.id5_ClassMetadataInstanceCmdlets && xmlQualifiedName.Namespace == this.id2_Item)
					{
						return this.Read40_ClassMetadataInstanceCmdlets(isNullable, false);
					}
					if (xmlQualifiedName.Name == this.id3_ClassMetadata && xmlQualifiedName.Namespace == this.id2_Item)
					{
						return this.Read36_ClassMetadata(isNullable, false);
					}
					if (xmlQualifiedName.Name == this.id24_StaticCmdletMetadata && xmlQualifiedName.Namespace == this.id2_Item)
					{
						return this.Read34_StaticCmdletMetadata(isNullable, false);
					}
					if (xmlQualifiedName.Name == this.id34_InstanceCmdletMetadata && xmlQualifiedName.Namespace == this.id2_Item)
					{
						return this.Read31_InstanceCmdletMetadata(isNullable, false);
					}
					if (xmlQualifiedName.Name == this.id28_CommonMethodParameterMetadata && xmlQualifiedName.Namespace == this.id2_Item)
					{
						return this.Read26_CommonMethodParameterMetadata(isNullable, false);
					}
					if (xmlQualifiedName.Name == this.id29_StaticMethodParameterMetadata && xmlQualifiedName.Namespace == this.id2_Item)
					{
						return this.Read27_StaticMethodParameterMetadata(isNullable, false);
					}
					if (xmlQualifiedName.Name == this.id31_Item && xmlQualifiedName.Namespace == this.id2_Item)
					{
						return this.Read25_Item(isNullable, false);
					}
					if (xmlQualifiedName.Name == this.id26_CommonMethodMetadata && xmlQualifiedName.Namespace == this.id2_Item)
					{
						return this.Read29_CommonMethodMetadata(isNullable, false);
					}
					if (xmlQualifiedName.Name == this.id33_InstanceMethodMetadata && xmlQualifiedName.Namespace == this.id2_Item)
					{
						return this.Read30_InstanceMethodMetadata(isNullable, false);
					}
					if (xmlQualifiedName.Name == this.id27_StaticMethodMetadata && xmlQualifiedName.Namespace == this.id2_Item)
					{
						return this.Read28_StaticMethodMetadata(isNullable, false);
					}
					if (xmlQualifiedName.Name == this.id30_CmdletOutputMetadata && xmlQualifiedName.Namespace == this.id2_Item)
					{
						return this.Read23_CmdletOutputMetadata(isNullable, false);
					}
					if (xmlQualifiedName.Name == this.id21_GetCmdletMetadata && xmlQualifiedName.Namespace == this.id2_Item)
					{
						return this.Read22_GetCmdletMetadata(isNullable, false);
					}
					if (xmlQualifiedName.Name == this.id22_CommonCmdletMetadata && xmlQualifiedName.Namespace == this.id2_Item)
					{
						return this.Read21_CommonCmdletMetadata(isNullable, false);
					}
					if (xmlQualifiedName.Name == this.id25_Item && xmlQualifiedName.Namespace == this.id2_Item)
					{
						return this.Read45_Item(isNullable, false);
					}
					if (xmlQualifiedName.Name == this.id6_GetCmdletParameters && xmlQualifiedName.Namespace == this.id2_Item)
					{
						return this.Read19_GetCmdletParameters(isNullable, false);
					}
					if (xmlQualifiedName.Name == this.id20_QueryOption && xmlQualifiedName.Namespace == this.id2_Item)
					{
						return this.Read18_QueryOption(isNullable, false);
					}
					if (xmlQualifiedName.Name == this.id9_Association && xmlQualifiedName.Namespace == this.id2_Item)
					{
						return this.Read17_Association(isNullable, false);
					}
					if (xmlQualifiedName.Name == this.id7_PropertyMetadata && xmlQualifiedName.Namespace == this.id2_Item)
					{
						return this.Read15_PropertyMetadata(isNullable, false);
					}
					if (xmlQualifiedName.Name == this.id35_PropertyQuery && xmlQualifiedName.Namespace == this.id2_Item)
					{
						return this.Read14_PropertyQuery(isNullable, false);
					}
					if (xmlQualifiedName.Name == this.id36_WildcardablePropertyQuery && xmlQualifiedName.Namespace == this.id2_Item)
					{
						return this.Read13_WildcardablePropertyQuery(isNullable, false);
					}
					if (xmlQualifiedName.Name == this.id11_CmdletParameterMetadata && xmlQualifiedName.Namespace == this.id2_Item)
					{
						return this.Read10_CmdletParameterMetadata(isNullable, false);
					}
					if (xmlQualifiedName.Name == this.id12_Item && xmlQualifiedName.Namespace == this.id2_Item)
					{
						return this.Read11_Item(isNullable, false);
					}
					if (xmlQualifiedName.Name == this.id13_Item && xmlQualifiedName.Namespace == this.id2_Item)
					{
						return this.Read12_Item(isNullable, false);
					}
					if (xmlQualifiedName.Name == this.id18_Item && xmlQualifiedName.Namespace == this.id2_Item)
					{
						return this.Read9_Item(isNullable, false);
					}
					if (xmlQualifiedName.Name == this.id19_Item && xmlQualifiedName.Namespace == this.id2_Item)
					{
						return this.Read8_Item(isNullable, false);
					}
					if (xmlQualifiedName.Name == this.id17_ObsoleteAttributeMetadata && xmlQualifiedName.Namespace == this.id2_Item)
					{
						return this.Read7_ObsoleteAttributeMetadata(isNullable, false);
					}
					if (xmlQualifiedName.Name == this.id8_TypeMetadata && xmlQualifiedName.Namespace == this.id2_Item)
					{
						return this.Read2_TypeMetadata(isNullable, false);
					}
					if (xmlQualifiedName.Name == this.id37_ItemsChoiceType && xmlQualifiedName.Namespace == this.id2_Item)
					{
						base.Reader.ReadStartElement();
						object result = this.Read3_ItemsChoiceType(base.CollapseWhitespace(base.Reader.ReadString()));
						base.ReadEndElement();
						return result;
					}
					if (xmlQualifiedName.Name == this.id69_ArrayOfString && xmlQualifiedName.Namespace == this.id2_Item)
					{
						string[] result2 = null;
						if (!base.ReadNull())
						{
							string[] array = null;
							int num = 0;
							if (base.Reader.IsEmptyElement)
							{
								base.Reader.Skip();
							}
							else
							{
								base.Reader.ReadStartElement();
								base.Reader.MoveToContent();
								int num2 = 0;
								int readerCount = base.ReaderCount;
								while (base.Reader.NodeType != XmlNodeType.EndElement && base.Reader.NodeType != XmlNodeType.None)
								{
									if (base.Reader.NodeType == XmlNodeType.Element)
									{
										if (base.Reader.LocalName == this.id64_AllowedValue && base.Reader.NamespaceURI == this.id2_Item)
										{
											array = (string[])base.EnsureArrayIndex(array, num, typeof(string));
											array[num++] = base.Reader.ReadElementString();
										}
										else
										{
											base.UnknownNode(null, "http://schemas.microsoft.com/cmdlets-over-objects/2009/11:AllowedValue");
										}
									}
									else
									{
										base.UnknownNode(null, "http://schemas.microsoft.com/cmdlets-over-objects/2009/11:AllowedValue");
									}
									base.Reader.MoveToContent();
									base.CheckReaderCount(ref num2, ref readerCount);
								}
								base.ReadEndElement();
							}
							result2 = (string[])base.ShrinkArray(array, num, typeof(string), false);
						}
						return result2;
					}
					if (xmlQualifiedName.Name == this.id70_ArrayOfPropertyMetadata && xmlQualifiedName.Namespace == this.id2_Item)
					{
						PropertyMetadata[] result3 = null;
						if (!base.ReadNull())
						{
							PropertyMetadata[] array2 = null;
							int num3 = 0;
							if (base.Reader.IsEmptyElement)
							{
								base.Reader.Skip();
							}
							else
							{
								base.Reader.ReadStartElement();
								base.Reader.MoveToContent();
								int num4 = 0;
								int readerCount2 = base.ReaderCount;
								while (base.Reader.NodeType != XmlNodeType.EndElement && base.Reader.NodeType != XmlNodeType.None)
								{
									if (base.Reader.NodeType == XmlNodeType.Element)
									{
										if (base.Reader.LocalName == this.id71_Property && base.Reader.NamespaceURI == this.id2_Item)
										{
											array2 = (PropertyMetadata[])base.EnsureArrayIndex(array2, num3, typeof(PropertyMetadata));
											array2[num3++] = this.Read15_PropertyMetadata(false, true);
										}
										else
										{
											base.UnknownNode(null, "http://schemas.microsoft.com/cmdlets-over-objects/2009/11:Property");
										}
									}
									else
									{
										base.UnknownNode(null, "http://schemas.microsoft.com/cmdlets-over-objects/2009/11:Property");
									}
									base.Reader.MoveToContent();
									base.CheckReaderCount(ref num4, ref readerCount2);
								}
								base.ReadEndElement();
							}
							result3 = (PropertyMetadata[])base.ShrinkArray(array2, num3, typeof(PropertyMetadata), false);
						}
						return result3;
					}
					if (xmlQualifiedName.Name == this.id72_ArrayOfAssociation && xmlQualifiedName.Namespace == this.id2_Item)
					{
						Association[] result4 = null;
						if (!base.ReadNull())
						{
							Association[] array3 = null;
							int num5 = 0;
							if (base.Reader.IsEmptyElement)
							{
								base.Reader.Skip();
							}
							else
							{
								base.Reader.ReadStartElement();
								base.Reader.MoveToContent();
								int num6 = 0;
								int readerCount3 = base.ReaderCount;
								while (base.Reader.NodeType != XmlNodeType.EndElement && base.Reader.NodeType != XmlNodeType.None)
								{
									if (base.Reader.NodeType == XmlNodeType.Element)
									{
										if (base.Reader.LocalName == this.id9_Association && base.Reader.NamespaceURI == this.id2_Item)
										{
											array3 = (Association[])base.EnsureArrayIndex(array3, num5, typeof(Association));
											array3[num5++] = this.Read17_Association(false, true);
										}
										else
										{
											base.UnknownNode(null, "http://schemas.microsoft.com/cmdlets-over-objects/2009/11:Association");
										}
									}
									else
									{
										base.UnknownNode(null, "http://schemas.microsoft.com/cmdlets-over-objects/2009/11:Association");
									}
									base.Reader.MoveToContent();
									base.CheckReaderCount(ref num6, ref readerCount3);
								}
								base.ReadEndElement();
							}
							result4 = (Association[])base.ShrinkArray(array3, num5, typeof(Association), false);
						}
						return result4;
					}
					if (xmlQualifiedName.Name == this.id73_ArrayOfQueryOption && xmlQualifiedName.Namespace == this.id2_Item)
					{
						QueryOption[] result5 = null;
						if (!base.ReadNull())
						{
							QueryOption[] array4 = null;
							int num7 = 0;
							if (base.Reader.IsEmptyElement)
							{
								base.Reader.Skip();
							}
							else
							{
								base.Reader.ReadStartElement();
								base.Reader.MoveToContent();
								int num8 = 0;
								int readerCount4 = base.ReaderCount;
								while (base.Reader.NodeType != XmlNodeType.EndElement && base.Reader.NodeType != XmlNodeType.None)
								{
									if (base.Reader.NodeType == XmlNodeType.Element)
									{
										if (base.Reader.LocalName == this.id74_Option && base.Reader.NamespaceURI == this.id2_Item)
										{
											array4 = (QueryOption[])base.EnsureArrayIndex(array4, num7, typeof(QueryOption));
											array4[num7++] = this.Read18_QueryOption(false, true);
										}
										else
										{
											base.UnknownNode(null, "http://schemas.microsoft.com/cmdlets-over-objects/2009/11:Option");
										}
									}
									else
									{
										base.UnknownNode(null, "http://schemas.microsoft.com/cmdlets-over-objects/2009/11:Option");
									}
									base.Reader.MoveToContent();
									base.CheckReaderCount(ref num8, ref readerCount4);
								}
								base.ReadEndElement();
							}
							result5 = (QueryOption[])base.ShrinkArray(array4, num7, typeof(QueryOption), false);
						}
						return result5;
					}
					if (xmlQualifiedName.Name == this.id23_ConfirmImpact && xmlQualifiedName.Namespace == this.id2_Item)
					{
						base.Reader.ReadStartElement();
						object result6 = this.Read20_ConfirmImpact(base.CollapseWhitespace(base.Reader.ReadString()));
						base.ReadEndElement();
						return result6;
					}
					if (xmlQualifiedName.Name == this.id75_Item && xmlQualifiedName.Namespace == this.id2_Item)
					{
						StaticMethodParameterMetadata[] result7 = null;
						if (!base.ReadNull())
						{
							StaticMethodParameterMetadata[] array5 = null;
							int num9 = 0;
							if (base.Reader.IsEmptyElement)
							{
								base.Reader.Skip();
							}
							else
							{
								base.Reader.ReadStartElement();
								base.Reader.MoveToContent();
								int num10 = 0;
								int readerCount5 = base.ReaderCount;
								while (base.Reader.NodeType != XmlNodeType.EndElement && base.Reader.NodeType != XmlNodeType.None)
								{
									if (base.Reader.NodeType == XmlNodeType.Element)
									{
										if (base.Reader.LocalName == this.id76_Parameter && base.Reader.NamespaceURI == this.id2_Item)
										{
											array5 = (StaticMethodParameterMetadata[])base.EnsureArrayIndex(array5, num9, typeof(StaticMethodParameterMetadata));
											array5[num9++] = this.Read27_StaticMethodParameterMetadata(false, true);
										}
										else
										{
											base.UnknownNode(null, "http://schemas.microsoft.com/cmdlets-over-objects/2009/11:Parameter");
										}
									}
									else
									{
										base.UnknownNode(null, "http://schemas.microsoft.com/cmdlets-over-objects/2009/11:Parameter");
									}
									base.Reader.MoveToContent();
									base.CheckReaderCount(ref num10, ref readerCount5);
								}
								base.ReadEndElement();
							}
							result7 = (StaticMethodParameterMetadata[])base.ShrinkArray(array5, num9, typeof(StaticMethodParameterMetadata), false);
						}
						return result7;
					}
					if (xmlQualifiedName.Name == this.id77_Item && xmlQualifiedName.Namespace == this.id2_Item)
					{
						InstanceMethodParameterMetadata[] result8 = null;
						if (!base.ReadNull())
						{
							InstanceMethodParameterMetadata[] array6 = null;
							int num11 = 0;
							if (base.Reader.IsEmptyElement)
							{
								base.Reader.Skip();
							}
							else
							{
								base.Reader.ReadStartElement();
								base.Reader.MoveToContent();
								int num12 = 0;
								int readerCount6 = base.ReaderCount;
								while (base.Reader.NodeType != XmlNodeType.EndElement && base.Reader.NodeType != XmlNodeType.None)
								{
									if (base.Reader.NodeType == XmlNodeType.Element)
									{
										if (base.Reader.LocalName == this.id76_Parameter && base.Reader.NamespaceURI == this.id2_Item)
										{
											array6 = (InstanceMethodParameterMetadata[])base.EnsureArrayIndex(array6, num11, typeof(InstanceMethodParameterMetadata));
											array6[num11++] = this.Read25_Item(false, true);
										}
										else
										{
											base.UnknownNode(null, "http://schemas.microsoft.com/cmdlets-over-objects/2009/11:Parameter");
										}
									}
									else
									{
										base.UnknownNode(null, "http://schemas.microsoft.com/cmdlets-over-objects/2009/11:Parameter");
									}
									base.Reader.MoveToContent();
									base.CheckReaderCount(ref num12, ref readerCount6);
								}
								base.ReadEndElement();
							}
							result8 = (InstanceMethodParameterMetadata[])base.ShrinkArray(array6, num11, typeof(InstanceMethodParameterMetadata), false);
						}
						return result8;
					}
					if (xmlQualifiedName.Name == this.id78_ArrayOfStaticCmdletMetadata && xmlQualifiedName.Namespace == this.id2_Item)
					{
						StaticCmdletMetadata[] result9 = null;
						if (!base.ReadNull())
						{
							StaticCmdletMetadata[] array7 = null;
							int num13 = 0;
							if (base.Reader.IsEmptyElement)
							{
								base.Reader.Skip();
							}
							else
							{
								base.Reader.ReadStartElement();
								base.Reader.MoveToContent();
								int num14 = 0;
								int readerCount7 = base.ReaderCount;
								while (base.Reader.NodeType != XmlNodeType.EndElement && base.Reader.NodeType != XmlNodeType.None)
								{
									if (base.Reader.NodeType == XmlNodeType.Element)
									{
										if (base.Reader.LocalName == this.id79_Cmdlet && base.Reader.NamespaceURI == this.id2_Item)
										{
											array7 = (StaticCmdletMetadata[])base.EnsureArrayIndex(array7, num13, typeof(StaticCmdletMetadata));
											array7[num13++] = this.Read34_StaticCmdletMetadata(false, true);
										}
										else
										{
											base.UnknownNode(null, "http://schemas.microsoft.com/cmdlets-over-objects/2009/11:Cmdlet");
										}
									}
									else
									{
										base.UnknownNode(null, "http://schemas.microsoft.com/cmdlets-over-objects/2009/11:Cmdlet");
									}
									base.Reader.MoveToContent();
									base.CheckReaderCount(ref num14, ref readerCount7);
								}
								base.ReadEndElement();
							}
							result9 = (StaticCmdletMetadata[])base.ShrinkArray(array7, num13, typeof(StaticCmdletMetadata), false);
						}
						return result9;
					}
					if (xmlQualifiedName.Name == this.id80_ArrayOfClassMetadataData && xmlQualifiedName.Namespace == this.id2_Item)
					{
						ClassMetadataData[] result10 = null;
						if (!base.ReadNull())
						{
							ClassMetadataData[] array8 = null;
							int num15 = 0;
							if (base.Reader.IsEmptyElement)
							{
								base.Reader.Skip();
							}
							else
							{
								base.Reader.ReadStartElement();
								base.Reader.MoveToContent();
								int num16 = 0;
								int readerCount8 = base.ReaderCount;
								while (base.Reader.NodeType != XmlNodeType.EndElement && base.Reader.NodeType != XmlNodeType.None)
								{
									if (base.Reader.NodeType == XmlNodeType.Element)
									{
										if (base.Reader.LocalName == this.id81_Data && base.Reader.NamespaceURI == this.id2_Item)
										{
											array8 = (ClassMetadataData[])base.EnsureArrayIndex(array8, num15, typeof(ClassMetadataData));
											array8[num15++] = this.Read35_ClassMetadataData(false, true);
										}
										else
										{
											base.UnknownNode(null, "http://schemas.microsoft.com/cmdlets-over-objects/2009/11:Data");
										}
									}
									else
									{
										base.UnknownNode(null, "http://schemas.microsoft.com/cmdlets-over-objects/2009/11:Data");
									}
									base.Reader.MoveToContent();
									base.CheckReaderCount(ref num16, ref readerCount8);
								}
								base.ReadEndElement();
							}
							result10 = (ClassMetadataData[])base.ShrinkArray(array8, num15, typeof(ClassMetadataData), false);
						}
						return result10;
					}
					if (xmlQualifiedName.Name == this.id82_ArrayOfEnumMetadataEnum && xmlQualifiedName.Namespace == this.id2_Item)
					{
						EnumMetadataEnum[] result11 = null;
						if (!base.ReadNull())
						{
							EnumMetadataEnum[] array9 = null;
							int num17 = 0;
							if (base.Reader.IsEmptyElement)
							{
								base.Reader.Skip();
							}
							else
							{
								base.Reader.ReadStartElement();
								base.Reader.MoveToContent();
								int num18 = 0;
								int readerCount9 = base.ReaderCount;
								while (base.Reader.NodeType != XmlNodeType.EndElement && base.Reader.NodeType != XmlNodeType.None)
								{
									if (base.Reader.NodeType == XmlNodeType.Element)
									{
										if (base.Reader.LocalName == this.id83_Enum && base.Reader.NamespaceURI == this.id2_Item)
										{
											array9 = (EnumMetadataEnum[])base.EnsureArrayIndex(array9, num17, typeof(EnumMetadataEnum));
											array9[num17++] = this.Read38_EnumMetadataEnum(false, true);
										}
										else
										{
											base.UnknownNode(null, "http://schemas.microsoft.com/cmdlets-over-objects/2009/11:Enum");
										}
									}
									else
									{
										base.UnknownNode(null, "http://schemas.microsoft.com/cmdlets-over-objects/2009/11:Enum");
									}
									base.Reader.MoveToContent();
									base.CheckReaderCount(ref num18, ref readerCount9);
								}
								base.ReadEndElement();
							}
							result11 = (EnumMetadataEnum[])base.ShrinkArray(array9, num17, typeof(EnumMetadataEnum), false);
						}
						return result11;
					}
					return base.ReadTypedPrimitive(xmlQualifiedName);
				}
			}
			else
			{
				if (flag)
				{
					return null;
				}
				object obj = new object();
				while (base.Reader.MoveToNextAttribute())
				{
					if (!base.IsXmlnsAttribute(base.Reader.Name))
					{
						base.UnknownNode(obj);
					}
				}
				base.Reader.MoveToElement();
				if (base.Reader.IsEmptyElement)
				{
					base.Reader.Skip();
					return obj;
				}
				base.Reader.ReadStartElement();
				base.Reader.MoveToContent();
				int num19 = 0;
				int readerCount10 = base.ReaderCount;
				while (base.Reader.NodeType != XmlNodeType.EndElement && base.Reader.NodeType != XmlNodeType.None)
				{
					if (base.Reader.NodeType == XmlNodeType.Element)
					{
						base.UnknownNode(obj, "");
					}
					else
					{
						base.UnknownNode(obj, "");
					}
					base.Reader.MoveToContent();
					base.CheckReaderCount(ref num19, ref readerCount10);
				}
				base.ReadEndElement();
				return obj;
			}
		}

		// Token: 0x06005CBD RID: 23741 RVA: 0x001F4EB8 File Offset: 0x001F30B8
		private EnumMetadataEnum Read38_EnumMetadataEnum(bool isNullable, bool checkType)
		{
			XmlQualifiedName xmlQualifiedName = checkType ? base.GetXsiType() : null;
			bool flag = false;
			if (isNullable)
			{
				flag = base.ReadNull();
			}
			if (checkType && !(xmlQualifiedName == null) && (xmlQualifiedName.Name != this.id4_Item || xmlQualifiedName.Namespace != this.id2_Item))
			{
				throw base.CreateUnknownTypeException(xmlQualifiedName);
			}
			if (flag)
			{
				return null;
			}
			EnumMetadataEnum enumMetadataEnum = new EnumMetadataEnum();
			EnumMetadataEnumValue[] array = null;
			int num = 0;
			bool[] array2 = new bool[4];
			while (base.Reader.MoveToNextAttribute())
			{
				if (!array2[1] && base.Reader.LocalName == this.id43_EnumName && base.Reader.NamespaceURI == this.id4_Item)
				{
					enumMetadataEnum.EnumName = base.Reader.Value;
					array2[1] = true;
				}
				else if (!array2[2] && base.Reader.LocalName == this.id44_UnderlyingType && base.Reader.NamespaceURI == this.id4_Item)
				{
					enumMetadataEnum.UnderlyingType = base.Reader.Value;
					array2[2] = true;
				}
				else if (!array2[3] && base.Reader.LocalName == this.id45_BitwiseFlags && base.Reader.NamespaceURI == this.id4_Item)
				{
					enumMetadataEnum.BitwiseFlags = XmlConvert.ToBoolean(base.Reader.Value);
					enumMetadataEnum.BitwiseFlagsSpecified = true;
					array2[3] = true;
				}
				else if (!base.IsXmlnsAttribute(base.Reader.Name))
				{
					base.UnknownNode(enumMetadataEnum, ":EnumName, :UnderlyingType, :BitwiseFlags");
				}
			}
			base.Reader.MoveToElement();
			if (base.Reader.IsEmptyElement)
			{
				base.Reader.Skip();
				enumMetadataEnum.Value = (EnumMetadataEnumValue[])base.ShrinkArray(array, num, typeof(EnumMetadataEnumValue), true);
				return enumMetadataEnum;
			}
			base.Reader.ReadStartElement();
			base.Reader.MoveToContent();
			int num2 = 0;
			int readerCount = base.ReaderCount;
			while (base.Reader.NodeType != XmlNodeType.EndElement && base.Reader.NodeType != XmlNodeType.None)
			{
				if (base.Reader.NodeType == XmlNodeType.Element)
				{
					if (base.Reader.LocalName == this.id42_Value && base.Reader.NamespaceURI == this.id2_Item)
					{
						array = (EnumMetadataEnumValue[])base.EnsureArrayIndex(array, num, typeof(EnumMetadataEnumValue));
						array[num++] = this.Read37_EnumMetadataEnumValue(false, true);
					}
					else
					{
						base.UnknownNode(enumMetadataEnum, "http://schemas.microsoft.com/cmdlets-over-objects/2009/11:Value");
					}
				}
				else
				{
					base.UnknownNode(enumMetadataEnum, "http://schemas.microsoft.com/cmdlets-over-objects/2009/11:Value");
				}
				base.Reader.MoveToContent();
				base.CheckReaderCount(ref num2, ref readerCount);
			}
			enumMetadataEnum.Value = (EnumMetadataEnumValue[])base.ShrinkArray(array, num, typeof(EnumMetadataEnumValue), true);
			base.ReadEndElement();
			return enumMetadataEnum;
		}

		// Token: 0x06005CBE RID: 23742 RVA: 0x001F5178 File Offset: 0x001F3378
		private ClassMetadataData Read35_ClassMetadataData(bool isNullable, bool checkType)
		{
			XmlQualifiedName xmlQualifiedName = checkType ? base.GetXsiType() : null;
			bool flag = false;
			if (isNullable)
			{
				flag = base.ReadNull();
			}
			if (checkType && !(xmlQualifiedName == null) && (xmlQualifiedName.Name != this.id4_Item || xmlQualifiedName.Namespace != this.id2_Item))
			{
				throw base.CreateUnknownTypeException(xmlQualifiedName);
			}
			if (flag)
			{
				return null;
			}
			ClassMetadataData classMetadataData = new ClassMetadataData();
			bool[] array = new bool[2];
			while (base.Reader.MoveToNextAttribute())
			{
				if (!array[0] && base.Reader.LocalName == this.id41_Name && base.Reader.NamespaceURI == this.id4_Item)
				{
					classMetadataData.Name = base.Reader.Value;
					array[0] = true;
				}
				else if (!base.IsXmlnsAttribute(base.Reader.Name))
				{
					base.UnknownNode(classMetadataData, ":Name");
				}
			}
			base.Reader.MoveToElement();
			if (base.Reader.IsEmptyElement)
			{
				base.Reader.Skip();
				return classMetadataData;
			}
			base.Reader.ReadStartElement();
			base.Reader.MoveToContent();
			int num = 0;
			int readerCount = base.ReaderCount;
			while (base.Reader.NodeType != XmlNodeType.EndElement && base.Reader.NodeType != XmlNodeType.None)
			{
				string value = null;
				if (base.Reader.NodeType == XmlNodeType.Element)
				{
					base.UnknownNode(classMetadataData, "");
				}
				else if (base.Reader.NodeType == XmlNodeType.Text || base.Reader.NodeType == XmlNodeType.CDATA || base.Reader.NodeType == XmlNodeType.Whitespace || base.Reader.NodeType == XmlNodeType.SignificantWhitespace)
				{
					value = base.ReadString(value, false);
					classMetadataData.Value = value;
				}
				else
				{
					base.UnknownNode(classMetadataData, "");
				}
				base.Reader.MoveToContent();
				base.CheckReaderCount(ref num, ref readerCount);
			}
			base.ReadEndElement();
			return classMetadataData;
		}

		// Token: 0x06005CBF RID: 23743 RVA: 0x001F5354 File Offset: 0x001F3554
		private StaticCmdletMetadata Read34_StaticCmdletMetadata(bool isNullable, bool checkType)
		{
			XmlQualifiedName xmlQualifiedName = checkType ? base.GetXsiType() : null;
			bool flag = false;
			if (isNullable)
			{
				flag = base.ReadNull();
			}
			if (checkType && !(xmlQualifiedName == null) && (xmlQualifiedName.Name != this.id24_StaticCmdletMetadata || xmlQualifiedName.Namespace != this.id2_Item))
			{
				throw base.CreateUnknownTypeException(xmlQualifiedName);
			}
			if (flag)
			{
				return null;
			}
			StaticCmdletMetadata staticCmdletMetadata = new StaticCmdletMetadata();
			StaticMethodMetadata[] array = null;
			int num = 0;
			bool[] array2 = new bool[2];
			while (base.Reader.MoveToNextAttribute())
			{
				if (!base.IsXmlnsAttribute(base.Reader.Name))
				{
					base.UnknownNode(staticCmdletMetadata);
				}
			}
			base.Reader.MoveToElement();
			if (base.Reader.IsEmptyElement)
			{
				base.Reader.Skip();
				staticCmdletMetadata.Method = (StaticMethodMetadata[])base.ShrinkArray(array, num, typeof(StaticMethodMetadata), true);
				return staticCmdletMetadata;
			}
			base.Reader.ReadStartElement();
			base.Reader.MoveToContent();
			int num2 = 0;
			int readerCount = base.ReaderCount;
			while (base.Reader.NodeType != XmlNodeType.EndElement && base.Reader.NodeType != XmlNodeType.None)
			{
				if (base.Reader.NodeType == XmlNodeType.Element)
				{
					if (!array2[0] && base.Reader.LocalName == this.id84_CmdletMetadata && base.Reader.NamespaceURI == this.id2_Item)
					{
						staticCmdletMetadata.CmdletMetadata = this.Read33_Item(false, true);
						array2[0] = true;
					}
					else if (base.Reader.LocalName == this.id85_Method && base.Reader.NamespaceURI == this.id2_Item)
					{
						array = (StaticMethodMetadata[])base.EnsureArrayIndex(array, num, typeof(StaticMethodMetadata));
						array[num++] = this.Read28_StaticMethodMetadata(false, true);
					}
					else
					{
						base.UnknownNode(staticCmdletMetadata, "http://schemas.microsoft.com/cmdlets-over-objects/2009/11:CmdletMetadata, http://schemas.microsoft.com/cmdlets-over-objects/2009/11:Method");
					}
				}
				else
				{
					base.UnknownNode(staticCmdletMetadata, "http://schemas.microsoft.com/cmdlets-over-objects/2009/11:CmdletMetadata, http://schemas.microsoft.com/cmdlets-over-objects/2009/11:Method");
				}
				base.Reader.MoveToContent();
				base.CheckReaderCount(ref num2, ref readerCount);
			}
			staticCmdletMetadata.Method = (StaticMethodMetadata[])base.ShrinkArray(array, num, typeof(StaticMethodMetadata), true);
			base.ReadEndElement();
			return staticCmdletMetadata;
		}

		// Token: 0x06005CC0 RID: 23744 RVA: 0x001F5570 File Offset: 0x001F3770
		private StaticMethodMetadata Read28_StaticMethodMetadata(bool isNullable, bool checkType)
		{
			XmlQualifiedName xmlQualifiedName = checkType ? base.GetXsiType() : null;
			bool flag = false;
			if (isNullable)
			{
				flag = base.ReadNull();
			}
			if (checkType && !(xmlQualifiedName == null) && (xmlQualifiedName.Name != this.id27_StaticMethodMetadata || xmlQualifiedName.Namespace != this.id2_Item))
			{
				throw base.CreateUnknownTypeException(xmlQualifiedName);
			}
			if (flag)
			{
				return null;
			}
			StaticMethodMetadata staticMethodMetadata = new StaticMethodMetadata();
			bool[] array = new bool[4];
			while (base.Reader.MoveToNextAttribute())
			{
				if (!array[1] && base.Reader.LocalName == this.id86_MethodName && base.Reader.NamespaceURI == this.id4_Item)
				{
					staticMethodMetadata.MethodName = base.Reader.Value;
					array[1] = true;
				}
				else if (!array[3] && base.Reader.LocalName == this.id87_CmdletParameterSet && base.Reader.NamespaceURI == this.id4_Item)
				{
					staticMethodMetadata.CmdletParameterSet = base.Reader.Value;
					array[3] = true;
				}
				else if (!base.IsXmlnsAttribute(base.Reader.Name))
				{
					base.UnknownNode(staticMethodMetadata, ":MethodName, :CmdletParameterSet");
				}
			}
			base.Reader.MoveToElement();
			if (base.Reader.IsEmptyElement)
			{
				base.Reader.Skip();
				return staticMethodMetadata;
			}
			base.Reader.ReadStartElement();
			base.Reader.MoveToContent();
			int num = 0;
			int readerCount = base.ReaderCount;
			while (base.Reader.NodeType != XmlNodeType.EndElement && base.Reader.NodeType != XmlNodeType.None)
			{
				if (base.Reader.NodeType == XmlNodeType.Element)
				{
					if (!array[0] && base.Reader.LocalName == this.id88_ReturnValue && base.Reader.NamespaceURI == this.id2_Item)
					{
						staticMethodMetadata.ReturnValue = this.Read24_Item(false, true);
						array[0] = true;
					}
					else if (base.Reader.LocalName == this.id89_Parameters && base.Reader.NamespaceURI == this.id2_Item)
					{
						if (!base.ReadNull())
						{
							StaticMethodParameterMetadata[] array2 = null;
							int num2 = 0;
							if (base.Reader.IsEmptyElement)
							{
								base.Reader.Skip();
							}
							else
							{
								base.Reader.ReadStartElement();
								base.Reader.MoveToContent();
								int num3 = 0;
								int readerCount2 = base.ReaderCount;
								while (base.Reader.NodeType != XmlNodeType.EndElement && base.Reader.NodeType != XmlNodeType.None)
								{
									if (base.Reader.NodeType == XmlNodeType.Element)
									{
										if (base.Reader.LocalName == this.id76_Parameter && base.Reader.NamespaceURI == this.id2_Item)
										{
											array2 = (StaticMethodParameterMetadata[])base.EnsureArrayIndex(array2, num2, typeof(StaticMethodParameterMetadata));
											array2[num2++] = this.Read27_StaticMethodParameterMetadata(false, true);
										}
										else
										{
											base.UnknownNode(null, "http://schemas.microsoft.com/cmdlets-over-objects/2009/11:Parameter");
										}
									}
									else
									{
										base.UnknownNode(null, "http://schemas.microsoft.com/cmdlets-over-objects/2009/11:Parameter");
									}
									base.Reader.MoveToContent();
									base.CheckReaderCount(ref num3, ref readerCount2);
								}
								base.ReadEndElement();
							}
							staticMethodMetadata.Parameters = (StaticMethodParameterMetadata[])base.ShrinkArray(array2, num2, typeof(StaticMethodParameterMetadata), false);
						}
					}
					else
					{
						base.UnknownNode(staticMethodMetadata, "http://schemas.microsoft.com/cmdlets-over-objects/2009/11:ReturnValue, http://schemas.microsoft.com/cmdlets-over-objects/2009/11:Parameters");
					}
				}
				else
				{
					base.UnknownNode(staticMethodMetadata, "http://schemas.microsoft.com/cmdlets-over-objects/2009/11:ReturnValue, http://schemas.microsoft.com/cmdlets-over-objects/2009/11:Parameters");
				}
				base.Reader.MoveToContent();
				base.CheckReaderCount(ref num, ref readerCount);
			}
			base.ReadEndElement();
			return staticMethodMetadata;
		}

		// Token: 0x06005CC1 RID: 23745 RVA: 0x001F58E4 File Offset: 0x001F3AE4
		private StaticMethodParameterMetadata Read27_StaticMethodParameterMetadata(bool isNullable, bool checkType)
		{
			XmlQualifiedName xmlQualifiedName = checkType ? base.GetXsiType() : null;
			bool flag = false;
			if (isNullable)
			{
				flag = base.ReadNull();
			}
			if (checkType && !(xmlQualifiedName == null) && (xmlQualifiedName.Name != this.id29_StaticMethodParameterMetadata || xmlQualifiedName.Namespace != this.id2_Item))
			{
				throw base.CreateUnknownTypeException(xmlQualifiedName);
			}
			if (flag)
			{
				return null;
			}
			StaticMethodParameterMetadata staticMethodParameterMetadata = new StaticMethodParameterMetadata();
			bool[] array = new bool[5];
			while (base.Reader.MoveToNextAttribute())
			{
				if (!array[1] && base.Reader.LocalName == this.id90_ParameterName && base.Reader.NamespaceURI == this.id4_Item)
				{
					staticMethodParameterMetadata.ParameterName = base.Reader.Value;
					array[1] = true;
				}
				else if (!array[2] && base.Reader.LocalName == this.id91_DefaultValue && base.Reader.NamespaceURI == this.id4_Item)
				{
					staticMethodParameterMetadata.DefaultValue = base.Reader.Value;
					array[2] = true;
				}
				else if (!base.IsXmlnsAttribute(base.Reader.Name))
				{
					base.UnknownNode(staticMethodParameterMetadata, ":ParameterName, :DefaultValue");
				}
			}
			base.Reader.MoveToElement();
			if (base.Reader.IsEmptyElement)
			{
				base.Reader.Skip();
				return staticMethodParameterMetadata;
			}
			base.Reader.ReadStartElement();
			base.Reader.MoveToContent();
			int num = 0;
			int readerCount = base.ReaderCount;
			while (base.Reader.NodeType != XmlNodeType.EndElement && base.Reader.NodeType != XmlNodeType.None)
			{
				if (base.Reader.NodeType == XmlNodeType.Element)
				{
					if (!array[0] && base.Reader.LocalName == this.id92_Type && base.Reader.NamespaceURI == this.id2_Item)
					{
						staticMethodParameterMetadata.Type = this.Read2_TypeMetadata(false, true);
						array[0] = true;
					}
					else if (!array[3] && base.Reader.LocalName == this.id11_CmdletParameterMetadata && base.Reader.NamespaceURI == this.id2_Item)
					{
						staticMethodParameterMetadata.CmdletParameterMetadata = this.Read8_Item(false, true);
						array[3] = true;
					}
					else if (!array[4] && base.Reader.LocalName == this.id30_CmdletOutputMetadata && base.Reader.NamespaceURI == this.id2_Item)
					{
						staticMethodParameterMetadata.CmdletOutputMetadata = this.Read23_CmdletOutputMetadata(false, true);
						array[4] = true;
					}
					else
					{
						base.UnknownNode(staticMethodParameterMetadata, "http://schemas.microsoft.com/cmdlets-over-objects/2009/11:Type, http://schemas.microsoft.com/cmdlets-over-objects/2009/11:CmdletParameterMetadata, http://schemas.microsoft.com/cmdlets-over-objects/2009/11:CmdletOutputMetadata");
					}
				}
				else
				{
					base.UnknownNode(staticMethodParameterMetadata, "http://schemas.microsoft.com/cmdlets-over-objects/2009/11:Type, http://schemas.microsoft.com/cmdlets-over-objects/2009/11:CmdletParameterMetadata, http://schemas.microsoft.com/cmdlets-over-objects/2009/11:CmdletOutputMetadata");
				}
				base.Reader.MoveToContent();
				base.CheckReaderCount(ref num, ref readerCount);
			}
			base.ReadEndElement();
			return staticMethodParameterMetadata;
		}

		// Token: 0x06005CC2 RID: 23746 RVA: 0x001F5B78 File Offset: 0x001F3D78
		private CmdletOutputMetadata Read23_CmdletOutputMetadata(bool isNullable, bool checkType)
		{
			XmlQualifiedName xmlQualifiedName = checkType ? base.GetXsiType() : null;
			bool flag = false;
			if (isNullable)
			{
				flag = base.ReadNull();
			}
			if (checkType && !(xmlQualifiedName == null) && (xmlQualifiedName.Name != this.id30_CmdletOutputMetadata || xmlQualifiedName.Namespace != this.id2_Item))
			{
				throw base.CreateUnknownTypeException(xmlQualifiedName);
			}
			if (flag)
			{
				return null;
			}
			CmdletOutputMetadata cmdletOutputMetadata = new CmdletOutputMetadata();
			bool[] array = new bool[2];
			while (base.Reader.MoveToNextAttribute())
			{
				if (!array[1] && base.Reader.LocalName == this.id49_PSName && base.Reader.NamespaceURI == this.id4_Item)
				{
					cmdletOutputMetadata.PSName = base.Reader.Value;
					array[1] = true;
				}
				else if (!base.IsXmlnsAttribute(base.Reader.Name))
				{
					base.UnknownNode(cmdletOutputMetadata, ":PSName");
				}
			}
			base.Reader.MoveToElement();
			if (base.Reader.IsEmptyElement)
			{
				base.Reader.Skip();
				return cmdletOutputMetadata;
			}
			base.Reader.ReadStartElement();
			base.Reader.MoveToContent();
			int num = 0;
			int readerCount = base.ReaderCount;
			while (base.Reader.NodeType != XmlNodeType.EndElement && base.Reader.NodeType != XmlNodeType.None)
			{
				if (base.Reader.NodeType == XmlNodeType.Element)
				{
					if (!array[0] && base.Reader.LocalName == this.id93_ErrorCode && base.Reader.NamespaceURI == this.id2_Item)
					{
						cmdletOutputMetadata.ErrorCode = this.Read1_Object(false, true);
						array[0] = true;
					}
					else
					{
						base.UnknownNode(cmdletOutputMetadata, "http://schemas.microsoft.com/cmdlets-over-objects/2009/11:ErrorCode");
					}
				}
				else
				{
					base.UnknownNode(cmdletOutputMetadata, "http://schemas.microsoft.com/cmdlets-over-objects/2009/11:ErrorCode");
				}
				base.Reader.MoveToContent();
				base.CheckReaderCount(ref num, ref readerCount);
			}
			base.ReadEndElement();
			return cmdletOutputMetadata;
		}

		// Token: 0x06005CC3 RID: 23747 RVA: 0x001F5D40 File Offset: 0x001F3F40
		private CmdletParameterMetadataForStaticMethodParameter Read8_Item(bool isNullable, bool checkType)
		{
			XmlQualifiedName xmlQualifiedName = checkType ? base.GetXsiType() : null;
			bool flag = false;
			if (isNullable)
			{
				flag = base.ReadNull();
			}
			if (checkType && !(xmlQualifiedName == null) && (xmlQualifiedName.Name != this.id19_Item || xmlQualifiedName.Namespace != this.id2_Item))
			{
				throw base.CreateUnknownTypeException(xmlQualifiedName);
			}
			if (flag)
			{
				return null;
			}
			CmdletParameterMetadataForStaticMethodParameter cmdletParameterMetadataForStaticMethodParameter = new CmdletParameterMetadataForStaticMethodParameter();
			string[] array = null;
			int num = 0;
			bool[] array2 = new bool[16];
			while (base.Reader.MoveToNextAttribute())
			{
				if (!array2[10] && base.Reader.LocalName == this.id47_IsMandatory && base.Reader.NamespaceURI == this.id4_Item)
				{
					cmdletParameterMetadataForStaticMethodParameter.IsMandatory = XmlConvert.ToBoolean(base.Reader.Value);
					cmdletParameterMetadataForStaticMethodParameter.IsMandatorySpecified = true;
					array2[10] = true;
				}
				else if (base.Reader.LocalName == this.id48_Aliases && base.Reader.NamespaceURI == this.id4_Item)
				{
					string value = base.Reader.Value;
					string[] array3 = value.Split(null);
					for (int i = 0; i < array3.Length; i++)
					{
						array = (string[])base.EnsureArrayIndex(array, num, typeof(string));
						array[num++] = array3[i];
					}
				}
				else if (!array2[12] && base.Reader.LocalName == this.id49_PSName && base.Reader.NamespaceURI == this.id4_Item)
				{
					cmdletParameterMetadataForStaticMethodParameter.PSName = base.Reader.Value;
					array2[12] = true;
				}
				else if (!array2[13] && base.Reader.LocalName == this.id50_Position && base.Reader.NamespaceURI == this.id4_Item)
				{
					cmdletParameterMetadataForStaticMethodParameter.Position = base.CollapseWhitespace(base.Reader.Value);
					array2[13] = true;
				}
				else if (!array2[14] && base.Reader.LocalName == this.id51_ValueFromPipeline && base.Reader.NamespaceURI == this.id4_Item)
				{
					cmdletParameterMetadataForStaticMethodParameter.ValueFromPipeline = XmlConvert.ToBoolean(base.Reader.Value);
					cmdletParameterMetadataForStaticMethodParameter.ValueFromPipelineSpecified = true;
					array2[14] = true;
				}
				else if (!array2[15] && base.Reader.LocalName == this.id52_Item && base.Reader.NamespaceURI == this.id4_Item)
				{
					cmdletParameterMetadataForStaticMethodParameter.ValueFromPipelineByPropertyName = XmlConvert.ToBoolean(base.Reader.Value);
					cmdletParameterMetadataForStaticMethodParameter.ValueFromPipelineByPropertyNameSpecified = true;
					array2[15] = true;
				}
				else if (!base.IsXmlnsAttribute(base.Reader.Name))
				{
					base.UnknownNode(cmdletParameterMetadataForStaticMethodParameter, ":IsMandatory, :Aliases, :PSName, :Position, :ValueFromPipeline, :ValueFromPipelineByPropertyName");
				}
			}
			base.Reader.MoveToElement();
			if (base.Reader.IsEmptyElement)
			{
				base.Reader.Skip();
				cmdletParameterMetadataForStaticMethodParameter.Aliases = (string[])base.ShrinkArray(array, num, typeof(string), true);
				return cmdletParameterMetadataForStaticMethodParameter;
			}
			base.Reader.ReadStartElement();
			base.Reader.MoveToContent();
			int num2 = 0;
			int readerCount = base.ReaderCount;
			while (base.Reader.NodeType != XmlNodeType.EndElement && base.Reader.NodeType != XmlNodeType.None)
			{
				if (base.Reader.NodeType == XmlNodeType.Element)
				{
					if (!array2[0] && base.Reader.LocalName == this.id55_AllowEmptyCollection && base.Reader.NamespaceURI == this.id2_Item)
					{
						cmdletParameterMetadataForStaticMethodParameter.AllowEmptyCollection = this.Read1_Object(false, true);
						array2[0] = true;
					}
					else if (!array2[1] && base.Reader.LocalName == this.id56_AllowEmptyString && base.Reader.NamespaceURI == this.id2_Item)
					{
						cmdletParameterMetadataForStaticMethodParameter.AllowEmptyString = this.Read1_Object(false, true);
						array2[1] = true;
					}
					else if (!array2[2] && base.Reader.LocalName == this.id57_AllowNull && base.Reader.NamespaceURI == this.id2_Item)
					{
						cmdletParameterMetadataForStaticMethodParameter.AllowNull = this.Read1_Object(false, true);
						array2[2] = true;
					}
					else if (!array2[3] && base.Reader.LocalName == this.id58_ValidateNotNull && base.Reader.NamespaceURI == this.id2_Item)
					{
						cmdletParameterMetadataForStaticMethodParameter.ValidateNotNull = this.Read1_Object(false, true);
						array2[3] = true;
					}
					else if (!array2[4] && base.Reader.LocalName == this.id59_ValidateNotNullOrEmpty && base.Reader.NamespaceURI == this.id2_Item)
					{
						cmdletParameterMetadataForStaticMethodParameter.ValidateNotNullOrEmpty = this.Read1_Object(false, true);
						array2[4] = true;
					}
					else if (!array2[5] && base.Reader.LocalName == this.id60_ValidateCount && base.Reader.NamespaceURI == this.id2_Item)
					{
						cmdletParameterMetadataForStaticMethodParameter.ValidateCount = this.Read4_Item(false, true);
						array2[5] = true;
					}
					else if (!array2[6] && base.Reader.LocalName == this.id61_ValidateLength && base.Reader.NamespaceURI == this.id2_Item)
					{
						cmdletParameterMetadataForStaticMethodParameter.ValidateLength = this.Read5_Item(false, true);
						array2[6] = true;
					}
					else if (!array2[7] && base.Reader.LocalName == this.id62_ValidateRange && base.Reader.NamespaceURI == this.id2_Item)
					{
						cmdletParameterMetadataForStaticMethodParameter.ValidateRange = this.Read6_Item(false, true);
						array2[7] = true;
					}
					else if (base.Reader.LocalName == this.id63_ValidateSet && base.Reader.NamespaceURI == this.id2_Item)
					{
						if (!base.ReadNull())
						{
							string[] array4 = null;
							int num3 = 0;
							if (base.Reader.IsEmptyElement)
							{
								base.Reader.Skip();
							}
							else
							{
								base.Reader.ReadStartElement();
								base.Reader.MoveToContent();
								int num4 = 0;
								int readerCount2 = base.ReaderCount;
								while (base.Reader.NodeType != XmlNodeType.EndElement && base.Reader.NodeType != XmlNodeType.None)
								{
									if (base.Reader.NodeType == XmlNodeType.Element)
									{
										if (base.Reader.LocalName == this.id64_AllowedValue && base.Reader.NamespaceURI == this.id2_Item)
										{
											array4 = (string[])base.EnsureArrayIndex(array4, num3, typeof(string));
											array4[num3++] = base.Reader.ReadElementString();
										}
										else
										{
											base.UnknownNode(null, "http://schemas.microsoft.com/cmdlets-over-objects/2009/11:AllowedValue");
										}
									}
									else
									{
										base.UnknownNode(null, "http://schemas.microsoft.com/cmdlets-over-objects/2009/11:AllowedValue");
									}
									base.Reader.MoveToContent();
									base.CheckReaderCount(ref num4, ref readerCount2);
								}
								base.ReadEndElement();
							}
							cmdletParameterMetadataForStaticMethodParameter.ValidateSet = (string[])base.ShrinkArray(array4, num3, typeof(string), false);
						}
					}
					else if (!array2[9] && base.Reader.LocalName == this.id65_Obsolete && base.Reader.NamespaceURI == this.id2_Item)
					{
						cmdletParameterMetadataForStaticMethodParameter.Obsolete = this.Read7_ObsoleteAttributeMetadata(false, true);
						array2[9] = true;
					}
					else
					{
						base.UnknownNode(cmdletParameterMetadataForStaticMethodParameter, "http://schemas.microsoft.com/cmdlets-over-objects/2009/11:AllowEmptyCollection, http://schemas.microsoft.com/cmdlets-over-objects/2009/11:AllowEmptyString, http://schemas.microsoft.com/cmdlets-over-objects/2009/11:AllowNull, http://schemas.microsoft.com/cmdlets-over-objects/2009/11:ValidateNotNull, http://schemas.microsoft.com/cmdlets-over-objects/2009/11:ValidateNotNullOrEmpty, http://schemas.microsoft.com/cmdlets-over-objects/2009/11:ValidateCount, http://schemas.microsoft.com/cmdlets-over-objects/2009/11:ValidateLength, http://schemas.microsoft.com/cmdlets-over-objects/2009/11:ValidateRange, http://schemas.microsoft.com/cmdlets-over-objects/2009/11:ValidateSet, http://schemas.microsoft.com/cmdlets-over-objects/2009/11:Obsolete");
					}
				}
				else
				{
					base.UnknownNode(cmdletParameterMetadataForStaticMethodParameter, "http://schemas.microsoft.com/cmdlets-over-objects/2009/11:AllowEmptyCollection, http://schemas.microsoft.com/cmdlets-over-objects/2009/11:AllowEmptyString, http://schemas.microsoft.com/cmdlets-over-objects/2009/11:AllowNull, http://schemas.microsoft.com/cmdlets-over-objects/2009/11:ValidateNotNull, http://schemas.microsoft.com/cmdlets-over-objects/2009/11:ValidateNotNullOrEmpty, http://schemas.microsoft.com/cmdlets-over-objects/2009/11:ValidateCount, http://schemas.microsoft.com/cmdlets-over-objects/2009/11:ValidateLength, http://schemas.microsoft.com/cmdlets-over-objects/2009/11:ValidateRange, http://schemas.microsoft.com/cmdlets-over-objects/2009/11:ValidateSet, http://schemas.microsoft.com/cmdlets-over-objects/2009/11:Obsolete");
				}
				base.Reader.MoveToContent();
				base.CheckReaderCount(ref num2, ref readerCount);
			}
			cmdletParameterMetadataForStaticMethodParameter.Aliases = (string[])base.ShrinkArray(array, num, typeof(string), true);
			base.ReadEndElement();
			return cmdletParameterMetadataForStaticMethodParameter;
		}

		// Token: 0x06005CC4 RID: 23748 RVA: 0x001F64A4 File Offset: 0x001F46A4
		private TypeMetadata Read2_TypeMetadata(bool isNullable, bool checkType)
		{
			XmlQualifiedName xmlQualifiedName = checkType ? base.GetXsiType() : null;
			bool flag = false;
			if (isNullable)
			{
				flag = base.ReadNull();
			}
			if (checkType && !(xmlQualifiedName == null) && (xmlQualifiedName.Name != this.id8_TypeMetadata || xmlQualifiedName.Namespace != this.id2_Item))
			{
				throw base.CreateUnknownTypeException(xmlQualifiedName);
			}
			if (flag)
			{
				return null;
			}
			TypeMetadata typeMetadata = new TypeMetadata();
			bool[] array = new bool[2];
			while (base.Reader.MoveToNextAttribute())
			{
				if (!array[0] && base.Reader.LocalName == this.id94_PSType && base.Reader.NamespaceURI == this.id4_Item)
				{
					typeMetadata.PSType = base.Reader.Value;
					array[0] = true;
				}
				else if (!array[1] && base.Reader.LocalName == this.id95_ETSType && base.Reader.NamespaceURI == this.id4_Item)
				{
					typeMetadata.ETSType = base.Reader.Value;
					array[1] = true;
				}
				else if (!base.IsXmlnsAttribute(base.Reader.Name))
				{
					base.UnknownNode(typeMetadata, ":PSType, :ETSType");
				}
			}
			base.Reader.MoveToElement();
			if (base.Reader.IsEmptyElement)
			{
				base.Reader.Skip();
				return typeMetadata;
			}
			base.Reader.ReadStartElement();
			base.Reader.MoveToContent();
			int num = 0;
			int readerCount = base.ReaderCount;
			while (base.Reader.NodeType != XmlNodeType.EndElement && base.Reader.NodeType != XmlNodeType.None)
			{
				if (base.Reader.NodeType == XmlNodeType.Element)
				{
					base.UnknownNode(typeMetadata, "");
				}
				else
				{
					base.UnknownNode(typeMetadata, "");
				}
				base.Reader.MoveToContent();
				base.CheckReaderCount(ref num, ref readerCount);
			}
			base.ReadEndElement();
			return typeMetadata;
		}

		// Token: 0x06005CC5 RID: 23749 RVA: 0x001F6670 File Offset: 0x001F4870
		private CommonMethodMetadataReturnValue Read24_Item(bool isNullable, bool checkType)
		{
			XmlQualifiedName xmlQualifiedName = checkType ? base.GetXsiType() : null;
			bool flag = false;
			if (isNullable)
			{
				flag = base.ReadNull();
			}
			if (checkType && !(xmlQualifiedName == null) && (xmlQualifiedName.Name != this.id4_Item || xmlQualifiedName.Namespace != this.id2_Item))
			{
				throw base.CreateUnknownTypeException(xmlQualifiedName);
			}
			if (flag)
			{
				return null;
			}
			CommonMethodMetadataReturnValue commonMethodMetadataReturnValue = new CommonMethodMetadataReturnValue();
			bool[] array = new bool[2];
			while (base.Reader.MoveToNextAttribute())
			{
				if (!base.IsXmlnsAttribute(base.Reader.Name))
				{
					base.UnknownNode(commonMethodMetadataReturnValue);
				}
			}
			base.Reader.MoveToElement();
			if (base.Reader.IsEmptyElement)
			{
				base.Reader.Skip();
				return commonMethodMetadataReturnValue;
			}
			base.Reader.ReadStartElement();
			base.Reader.MoveToContent();
			int num = 0;
			int readerCount = base.ReaderCount;
			while (base.Reader.NodeType != XmlNodeType.EndElement && base.Reader.NodeType != XmlNodeType.None)
			{
				if (base.Reader.NodeType == XmlNodeType.Element)
				{
					if (!array[0] && base.Reader.LocalName == this.id92_Type && base.Reader.NamespaceURI == this.id2_Item)
					{
						commonMethodMetadataReturnValue.Type = this.Read2_TypeMetadata(false, true);
						array[0] = true;
					}
					else if (!array[1] && base.Reader.LocalName == this.id30_CmdletOutputMetadata && base.Reader.NamespaceURI == this.id2_Item)
					{
						commonMethodMetadataReturnValue.CmdletOutputMetadata = this.Read23_CmdletOutputMetadata(false, true);
						array[1] = true;
					}
					else
					{
						base.UnknownNode(commonMethodMetadataReturnValue, "http://schemas.microsoft.com/cmdlets-over-objects/2009/11:Type, http://schemas.microsoft.com/cmdlets-over-objects/2009/11:CmdletOutputMetadata");
					}
				}
				else
				{
					base.UnknownNode(commonMethodMetadataReturnValue, "http://schemas.microsoft.com/cmdlets-over-objects/2009/11:Type, http://schemas.microsoft.com/cmdlets-over-objects/2009/11:CmdletOutputMetadata");
				}
				base.Reader.MoveToContent();
				base.CheckReaderCount(ref num, ref readerCount);
			}
			base.ReadEndElement();
			return commonMethodMetadataReturnValue;
		}

		// Token: 0x06005CC6 RID: 23750 RVA: 0x001F6834 File Offset: 0x001F4A34
		private StaticCmdletMetadataCmdletMetadata Read33_Item(bool isNullable, bool checkType)
		{
			XmlQualifiedName xmlQualifiedName = checkType ? base.GetXsiType() : null;
			bool flag = false;
			if (isNullable)
			{
				flag = base.ReadNull();
			}
			if (checkType && !(xmlQualifiedName == null) && (xmlQualifiedName.Name != this.id4_Item || xmlQualifiedName.Namespace != this.id2_Item))
			{
				throw base.CreateUnknownTypeException(xmlQualifiedName);
			}
			if (flag)
			{
				return null;
			}
			StaticCmdletMetadataCmdletMetadata staticCmdletMetadataCmdletMetadata = new StaticCmdletMetadataCmdletMetadata();
			string[] array = null;
			int num = 0;
			bool[] array2 = new bool[7];
			while (base.Reader.MoveToNextAttribute())
			{
				if (!array2[1] && base.Reader.LocalName == this.id96_Verb && base.Reader.NamespaceURI == this.id4_Item)
				{
					staticCmdletMetadataCmdletMetadata.Verb = base.Reader.Value;
					array2[1] = true;
				}
				else if (!array2[2] && base.Reader.LocalName == this.id97_Noun && base.Reader.NamespaceURI == this.id4_Item)
				{
					staticCmdletMetadataCmdletMetadata.Noun = base.Reader.Value;
					array2[2] = true;
				}
				else if (base.Reader.LocalName == this.id48_Aliases && base.Reader.NamespaceURI == this.id4_Item)
				{
					string value = base.Reader.Value;
					string[] array3 = value.Split(null);
					for (int i = 0; i < array3.Length; i++)
					{
						array = (string[])base.EnsureArrayIndex(array, num, typeof(string));
						array[num++] = array3[i];
					}
				}
				else if (!array2[4] && base.Reader.LocalName == this.id23_ConfirmImpact && base.Reader.NamespaceURI == this.id4_Item)
				{
					staticCmdletMetadataCmdletMetadata.ConfirmImpact = this.Read20_ConfirmImpact(base.Reader.Value);
					staticCmdletMetadataCmdletMetadata.ConfirmImpactSpecified = true;
					array2[4] = true;
				}
				else if (!array2[5] && base.Reader.LocalName == this.id98_HelpUri && base.Reader.NamespaceURI == this.id4_Item)
				{
					staticCmdletMetadataCmdletMetadata.HelpUri = base.CollapseWhitespace(base.Reader.Value);
					array2[5] = true;
				}
				else if (!array2[6] && base.Reader.LocalName == this.id99_DefaultCmdletParameterSet && base.Reader.NamespaceURI == this.id4_Item)
				{
					staticCmdletMetadataCmdletMetadata.DefaultCmdletParameterSet = base.Reader.Value;
					array2[6] = true;
				}
				else if (!base.IsXmlnsAttribute(base.Reader.Name))
				{
					base.UnknownNode(staticCmdletMetadataCmdletMetadata, ":Verb, :Noun, :Aliases, :ConfirmImpact, :HelpUri, :DefaultCmdletParameterSet");
				}
			}
			base.Reader.MoveToElement();
			if (base.Reader.IsEmptyElement)
			{
				base.Reader.Skip();
				staticCmdletMetadataCmdletMetadata.Aliases = (string[])base.ShrinkArray(array, num, typeof(string), true);
				return staticCmdletMetadataCmdletMetadata;
			}
			base.Reader.ReadStartElement();
			base.Reader.MoveToContent();
			int num2 = 0;
			int readerCount = base.ReaderCount;
			while (base.Reader.NodeType != XmlNodeType.EndElement && base.Reader.NodeType != XmlNodeType.None)
			{
				if (base.Reader.NodeType == XmlNodeType.Element)
				{
					if (!array2[0] && base.Reader.LocalName == this.id65_Obsolete && base.Reader.NamespaceURI == this.id2_Item)
					{
						staticCmdletMetadataCmdletMetadata.Obsolete = this.Read7_ObsoleteAttributeMetadata(false, true);
						array2[0] = true;
					}
					else
					{
						base.UnknownNode(staticCmdletMetadataCmdletMetadata, "http://schemas.microsoft.com/cmdlets-over-objects/2009/11:Obsolete");
					}
				}
				else
				{
					base.UnknownNode(staticCmdletMetadataCmdletMetadata, "http://schemas.microsoft.com/cmdlets-over-objects/2009/11:Obsolete");
				}
				base.Reader.MoveToContent();
				base.CheckReaderCount(ref num2, ref readerCount);
			}
			staticCmdletMetadataCmdletMetadata.Aliases = (string[])base.ShrinkArray(array, num, typeof(string), true);
			base.ReadEndElement();
			return staticCmdletMetadataCmdletMetadata;
		}

		// Token: 0x06005CC7 RID: 23751 RVA: 0x001F6BF0 File Offset: 0x001F4DF0
		private ConfirmImpact Read20_ConfirmImpact(string s)
		{
			if (s != null)
			{
				if (s == "None")
				{
					return ConfirmImpact.None;
				}
				if (s == "Low")
				{
					return ConfirmImpact.Low;
				}
				if (s == "Medium")
				{
					return ConfirmImpact.Medium;
				}
				if (s == "High")
				{
					return ConfirmImpact.High;
				}
			}
			throw base.CreateUnknownConstantException(s, typeof(ConfirmImpact));
		}

		// Token: 0x06005CC8 RID: 23752 RVA: 0x001F6C54 File Offset: 0x001F4E54
		private InstanceMethodParameterMetadata Read25_Item(bool isNullable, bool checkType)
		{
			XmlQualifiedName xmlQualifiedName = checkType ? base.GetXsiType() : null;
			bool flag = false;
			if (isNullable)
			{
				flag = base.ReadNull();
			}
			if (checkType && !(xmlQualifiedName == null) && (xmlQualifiedName.Name != this.id31_Item || xmlQualifiedName.Namespace != this.id2_Item))
			{
				throw base.CreateUnknownTypeException(xmlQualifiedName);
			}
			if (flag)
			{
				return null;
			}
			InstanceMethodParameterMetadata instanceMethodParameterMetadata = new InstanceMethodParameterMetadata();
			bool[] array = new bool[5];
			while (base.Reader.MoveToNextAttribute())
			{
				if (!array[1] && base.Reader.LocalName == this.id90_ParameterName && base.Reader.NamespaceURI == this.id4_Item)
				{
					instanceMethodParameterMetadata.ParameterName = base.Reader.Value;
					array[1] = true;
				}
				else if (!array[2] && base.Reader.LocalName == this.id91_DefaultValue && base.Reader.NamespaceURI == this.id4_Item)
				{
					instanceMethodParameterMetadata.DefaultValue = base.Reader.Value;
					array[2] = true;
				}
				else if (!base.IsXmlnsAttribute(base.Reader.Name))
				{
					base.UnknownNode(instanceMethodParameterMetadata, ":ParameterName, :DefaultValue");
				}
			}
			base.Reader.MoveToElement();
			if (base.Reader.IsEmptyElement)
			{
				base.Reader.Skip();
				return instanceMethodParameterMetadata;
			}
			base.Reader.ReadStartElement();
			base.Reader.MoveToContent();
			int num = 0;
			int readerCount = base.ReaderCount;
			while (base.Reader.NodeType != XmlNodeType.EndElement && base.Reader.NodeType != XmlNodeType.None)
			{
				if (base.Reader.NodeType == XmlNodeType.Element)
				{
					if (!array[0] && base.Reader.LocalName == this.id92_Type && base.Reader.NamespaceURI == this.id2_Item)
					{
						instanceMethodParameterMetadata.Type = this.Read2_TypeMetadata(false, true);
						array[0] = true;
					}
					else if (!array[3] && base.Reader.LocalName == this.id11_CmdletParameterMetadata && base.Reader.NamespaceURI == this.id2_Item)
					{
						instanceMethodParameterMetadata.CmdletParameterMetadata = this.Read9_Item(false, true);
						array[3] = true;
					}
					else if (!array[4] && base.Reader.LocalName == this.id30_CmdletOutputMetadata && base.Reader.NamespaceURI == this.id2_Item)
					{
						instanceMethodParameterMetadata.CmdletOutputMetadata = this.Read23_CmdletOutputMetadata(false, true);
						array[4] = true;
					}
					else
					{
						base.UnknownNode(instanceMethodParameterMetadata, "http://schemas.microsoft.com/cmdlets-over-objects/2009/11:Type, http://schemas.microsoft.com/cmdlets-over-objects/2009/11:CmdletParameterMetadata, http://schemas.microsoft.com/cmdlets-over-objects/2009/11:CmdletOutputMetadata");
					}
				}
				else
				{
					base.UnknownNode(instanceMethodParameterMetadata, "http://schemas.microsoft.com/cmdlets-over-objects/2009/11:Type, http://schemas.microsoft.com/cmdlets-over-objects/2009/11:CmdletParameterMetadata, http://schemas.microsoft.com/cmdlets-over-objects/2009/11:CmdletOutputMetadata");
				}
				base.Reader.MoveToContent();
				base.CheckReaderCount(ref num, ref readerCount);
			}
			base.ReadEndElement();
			return instanceMethodParameterMetadata;
		}

		// Token: 0x06005CC9 RID: 23753 RVA: 0x001F6EE8 File Offset: 0x001F50E8
		private CmdletParameterMetadataForInstanceMethodParameter Read9_Item(bool isNullable, bool checkType)
		{
			XmlQualifiedName xmlQualifiedName = checkType ? base.GetXsiType() : null;
			bool flag = false;
			if (isNullable)
			{
				flag = base.ReadNull();
			}
			if (checkType && !(xmlQualifiedName == null) && (xmlQualifiedName.Name != this.id18_Item || xmlQualifiedName.Namespace != this.id2_Item))
			{
				throw base.CreateUnknownTypeException(xmlQualifiedName);
			}
			if (flag)
			{
				return null;
			}
			CmdletParameterMetadataForInstanceMethodParameter cmdletParameterMetadataForInstanceMethodParameter = new CmdletParameterMetadataForInstanceMethodParameter();
			string[] array = null;
			int num = 0;
			bool[] array2 = new bool[15];
			while (base.Reader.MoveToNextAttribute())
			{
				if (!array2[10] && base.Reader.LocalName == this.id47_IsMandatory && base.Reader.NamespaceURI == this.id4_Item)
				{
					cmdletParameterMetadataForInstanceMethodParameter.IsMandatory = XmlConvert.ToBoolean(base.Reader.Value);
					cmdletParameterMetadataForInstanceMethodParameter.IsMandatorySpecified = true;
					array2[10] = true;
				}
				else if (base.Reader.LocalName == this.id48_Aliases && base.Reader.NamespaceURI == this.id4_Item)
				{
					string value = base.Reader.Value;
					string[] array3 = value.Split(null);
					for (int i = 0; i < array3.Length; i++)
					{
						array = (string[])base.EnsureArrayIndex(array, num, typeof(string));
						array[num++] = array3[i];
					}
				}
				else if (!array2[12] && base.Reader.LocalName == this.id49_PSName && base.Reader.NamespaceURI == this.id4_Item)
				{
					cmdletParameterMetadataForInstanceMethodParameter.PSName = base.Reader.Value;
					array2[12] = true;
				}
				else if (!array2[13] && base.Reader.LocalName == this.id50_Position && base.Reader.NamespaceURI == this.id4_Item)
				{
					cmdletParameterMetadataForInstanceMethodParameter.Position = base.CollapseWhitespace(base.Reader.Value);
					array2[13] = true;
				}
				else if (!array2[14] && base.Reader.LocalName == this.id52_Item && base.Reader.NamespaceURI == this.id4_Item)
				{
					cmdletParameterMetadataForInstanceMethodParameter.ValueFromPipelineByPropertyName = XmlConvert.ToBoolean(base.Reader.Value);
					cmdletParameterMetadataForInstanceMethodParameter.ValueFromPipelineByPropertyNameSpecified = true;
					array2[14] = true;
				}
				else if (!base.IsXmlnsAttribute(base.Reader.Name))
				{
					base.UnknownNode(cmdletParameterMetadataForInstanceMethodParameter, ":IsMandatory, :Aliases, :PSName, :Position, :ValueFromPipelineByPropertyName");
				}
			}
			base.Reader.MoveToElement();
			if (base.Reader.IsEmptyElement)
			{
				base.Reader.Skip();
				cmdletParameterMetadataForInstanceMethodParameter.Aliases = (string[])base.ShrinkArray(array, num, typeof(string), true);
				return cmdletParameterMetadataForInstanceMethodParameter;
			}
			base.Reader.ReadStartElement();
			base.Reader.MoveToContent();
			int num2 = 0;
			int readerCount = base.ReaderCount;
			while (base.Reader.NodeType != XmlNodeType.EndElement && base.Reader.NodeType != XmlNodeType.None)
			{
				if (base.Reader.NodeType == XmlNodeType.Element)
				{
					if (!array2[0] && base.Reader.LocalName == this.id55_AllowEmptyCollection && base.Reader.NamespaceURI == this.id2_Item)
					{
						cmdletParameterMetadataForInstanceMethodParameter.AllowEmptyCollection = this.Read1_Object(false, true);
						array2[0] = true;
					}
					else if (!array2[1] && base.Reader.LocalName == this.id56_AllowEmptyString && base.Reader.NamespaceURI == this.id2_Item)
					{
						cmdletParameterMetadataForInstanceMethodParameter.AllowEmptyString = this.Read1_Object(false, true);
						array2[1] = true;
					}
					else if (!array2[2] && base.Reader.LocalName == this.id57_AllowNull && base.Reader.NamespaceURI == this.id2_Item)
					{
						cmdletParameterMetadataForInstanceMethodParameter.AllowNull = this.Read1_Object(false, true);
						array2[2] = true;
					}
					else if (!array2[3] && base.Reader.LocalName == this.id58_ValidateNotNull && base.Reader.NamespaceURI == this.id2_Item)
					{
						cmdletParameterMetadataForInstanceMethodParameter.ValidateNotNull = this.Read1_Object(false, true);
						array2[3] = true;
					}
					else if (!array2[4] && base.Reader.LocalName == this.id59_ValidateNotNullOrEmpty && base.Reader.NamespaceURI == this.id2_Item)
					{
						cmdletParameterMetadataForInstanceMethodParameter.ValidateNotNullOrEmpty = this.Read1_Object(false, true);
						array2[4] = true;
					}
					else if (!array2[5] && base.Reader.LocalName == this.id60_ValidateCount && base.Reader.NamespaceURI == this.id2_Item)
					{
						cmdletParameterMetadataForInstanceMethodParameter.ValidateCount = this.Read4_Item(false, true);
						array2[5] = true;
					}
					else if (!array2[6] && base.Reader.LocalName == this.id61_ValidateLength && base.Reader.NamespaceURI == this.id2_Item)
					{
						cmdletParameterMetadataForInstanceMethodParameter.ValidateLength = this.Read5_Item(false, true);
						array2[6] = true;
					}
					else if (!array2[7] && base.Reader.LocalName == this.id62_ValidateRange && base.Reader.NamespaceURI == this.id2_Item)
					{
						cmdletParameterMetadataForInstanceMethodParameter.ValidateRange = this.Read6_Item(false, true);
						array2[7] = true;
					}
					else if (base.Reader.LocalName == this.id63_ValidateSet && base.Reader.NamespaceURI == this.id2_Item)
					{
						if (!base.ReadNull())
						{
							string[] array4 = null;
							int num3 = 0;
							if (base.Reader.IsEmptyElement)
							{
								base.Reader.Skip();
							}
							else
							{
								base.Reader.ReadStartElement();
								base.Reader.MoveToContent();
								int num4 = 0;
								int readerCount2 = base.ReaderCount;
								while (base.Reader.NodeType != XmlNodeType.EndElement && base.Reader.NodeType != XmlNodeType.None)
								{
									if (base.Reader.NodeType == XmlNodeType.Element)
									{
										if (base.Reader.LocalName == this.id64_AllowedValue && base.Reader.NamespaceURI == this.id2_Item)
										{
											array4 = (string[])base.EnsureArrayIndex(array4, num3, typeof(string));
											array4[num3++] = base.Reader.ReadElementString();
										}
										else
										{
											base.UnknownNode(null, "http://schemas.microsoft.com/cmdlets-over-objects/2009/11:AllowedValue");
										}
									}
									else
									{
										base.UnknownNode(null, "http://schemas.microsoft.com/cmdlets-over-objects/2009/11:AllowedValue");
									}
									base.Reader.MoveToContent();
									base.CheckReaderCount(ref num4, ref readerCount2);
								}
								base.ReadEndElement();
							}
							cmdletParameterMetadataForInstanceMethodParameter.ValidateSet = (string[])base.ShrinkArray(array4, num3, typeof(string), false);
						}
					}
					else if (!array2[9] && base.Reader.LocalName == this.id65_Obsolete && base.Reader.NamespaceURI == this.id2_Item)
					{
						cmdletParameterMetadataForInstanceMethodParameter.Obsolete = this.Read7_ObsoleteAttributeMetadata(false, true);
						array2[9] = true;
					}
					else
					{
						base.UnknownNode(cmdletParameterMetadataForInstanceMethodParameter, "http://schemas.microsoft.com/cmdlets-over-objects/2009/11:AllowEmptyCollection, http://schemas.microsoft.com/cmdlets-over-objects/2009/11:AllowEmptyString, http://schemas.microsoft.com/cmdlets-over-objects/2009/11:AllowNull, http://schemas.microsoft.com/cmdlets-over-objects/2009/11:ValidateNotNull, http://schemas.microsoft.com/cmdlets-over-objects/2009/11:ValidateNotNullOrEmpty, http://schemas.microsoft.com/cmdlets-over-objects/2009/11:ValidateCount, http://schemas.microsoft.com/cmdlets-over-objects/2009/11:ValidateLength, http://schemas.microsoft.com/cmdlets-over-objects/2009/11:ValidateRange, http://schemas.microsoft.com/cmdlets-over-objects/2009/11:ValidateSet, http://schemas.microsoft.com/cmdlets-over-objects/2009/11:Obsolete");
					}
				}
				else
				{
					base.UnknownNode(cmdletParameterMetadataForInstanceMethodParameter, "http://schemas.microsoft.com/cmdlets-over-objects/2009/11:AllowEmptyCollection, http://schemas.microsoft.com/cmdlets-over-objects/2009/11:AllowEmptyString, http://schemas.microsoft.com/cmdlets-over-objects/2009/11:AllowNull, http://schemas.microsoft.com/cmdlets-over-objects/2009/11:ValidateNotNull, http://schemas.microsoft.com/cmdlets-over-objects/2009/11:ValidateNotNullOrEmpty, http://schemas.microsoft.com/cmdlets-over-objects/2009/11:ValidateCount, http://schemas.microsoft.com/cmdlets-over-objects/2009/11:ValidateLength, http://schemas.microsoft.com/cmdlets-over-objects/2009/11:ValidateRange, http://schemas.microsoft.com/cmdlets-over-objects/2009/11:ValidateSet, http://schemas.microsoft.com/cmdlets-over-objects/2009/11:Obsolete");
				}
				base.Reader.MoveToContent();
				base.CheckReaderCount(ref num2, ref readerCount);
			}
			cmdletParameterMetadataForInstanceMethodParameter.Aliases = (string[])base.ShrinkArray(array, num, typeof(string), true);
			base.ReadEndElement();
			return cmdletParameterMetadataForInstanceMethodParameter;
		}

		// Token: 0x06005CCA RID: 23754 RVA: 0x001F75F8 File Offset: 0x001F57F8
		private QueryOption Read18_QueryOption(bool isNullable, bool checkType)
		{
			XmlQualifiedName xmlQualifiedName = checkType ? base.GetXsiType() : null;
			bool flag = false;
			if (isNullable)
			{
				flag = base.ReadNull();
			}
			if (checkType && !(xmlQualifiedName == null) && (xmlQualifiedName.Name != this.id20_QueryOption || xmlQualifiedName.Namespace != this.id2_Item))
			{
				throw base.CreateUnknownTypeException(xmlQualifiedName);
			}
			if (flag)
			{
				return null;
			}
			QueryOption queryOption = new QueryOption();
			bool[] array = new bool[3];
			while (base.Reader.MoveToNextAttribute())
			{
				if (!array[2] && base.Reader.LocalName == this.id100_OptionName && base.Reader.NamespaceURI == this.id4_Item)
				{
					queryOption.OptionName = base.Reader.Value;
					array[2] = true;
				}
				else if (!base.IsXmlnsAttribute(base.Reader.Name))
				{
					base.UnknownNode(queryOption, ":OptionName");
				}
			}
			base.Reader.MoveToElement();
			if (base.Reader.IsEmptyElement)
			{
				base.Reader.Skip();
				return queryOption;
			}
			base.Reader.ReadStartElement();
			base.Reader.MoveToContent();
			int num = 0;
			int readerCount = base.ReaderCount;
			while (base.Reader.NodeType != XmlNodeType.EndElement && base.Reader.NodeType != XmlNodeType.None)
			{
				if (base.Reader.NodeType == XmlNodeType.Element)
				{
					if (!array[0] && base.Reader.LocalName == this.id92_Type && base.Reader.NamespaceURI == this.id2_Item)
					{
						queryOption.Type = this.Read2_TypeMetadata(false, true);
						array[0] = true;
					}
					else if (!array[1] && base.Reader.LocalName == this.id11_CmdletParameterMetadata && base.Reader.NamespaceURI == this.id2_Item)
					{
						queryOption.CmdletParameterMetadata = this.Read11_Item(false, true);
						array[1] = true;
					}
					else
					{
						base.UnknownNode(queryOption, "http://schemas.microsoft.com/cmdlets-over-objects/2009/11:Type, http://schemas.microsoft.com/cmdlets-over-objects/2009/11:CmdletParameterMetadata");
					}
				}
				else
				{
					base.UnknownNode(queryOption, "http://schemas.microsoft.com/cmdlets-over-objects/2009/11:Type, http://schemas.microsoft.com/cmdlets-over-objects/2009/11:CmdletParameterMetadata");
				}
				base.Reader.MoveToContent();
				base.CheckReaderCount(ref num, ref readerCount);
			}
			base.ReadEndElement();
			return queryOption;
		}

		// Token: 0x06005CCB RID: 23755 RVA: 0x001F7804 File Offset: 0x001F5A04
		private CmdletParameterMetadataForGetCmdletParameter Read11_Item(bool isNullable, bool checkType)
		{
			XmlQualifiedName xmlQualifiedName = checkType ? base.GetXsiType() : null;
			bool flag = false;
			if (isNullable)
			{
				flag = base.ReadNull();
			}
			if (checkType && !(xmlQualifiedName == null) && (xmlQualifiedName.Name != this.id12_Item || xmlQualifiedName.Namespace != this.id2_Item))
			{
				if (xmlQualifiedName.Name == this.id13_Item && xmlQualifiedName.Namespace == this.id2_Item)
				{
					return this.Read12_Item(isNullable, false);
				}
				throw base.CreateUnknownTypeException(xmlQualifiedName);
			}
			else
			{
				if (flag)
				{
					return null;
				}
				CmdletParameterMetadataForGetCmdletParameter cmdletParameterMetadataForGetCmdletParameter = new CmdletParameterMetadataForGetCmdletParameter();
				string[] array = null;
				int num = 0;
				string[] array2 = null;
				int num2 = 0;
				bool[] array3 = new bool[17];
				while (base.Reader.MoveToNextAttribute())
				{
					if (!array3[10] && base.Reader.LocalName == this.id47_IsMandatory && base.Reader.NamespaceURI == this.id4_Item)
					{
						cmdletParameterMetadataForGetCmdletParameter.IsMandatory = XmlConvert.ToBoolean(base.Reader.Value);
						cmdletParameterMetadataForGetCmdletParameter.IsMandatorySpecified = true;
						array3[10] = true;
					}
					else if (base.Reader.LocalName == this.id48_Aliases && base.Reader.NamespaceURI == this.id4_Item)
					{
						string value = base.Reader.Value;
						string[] array4 = value.Split(null);
						for (int i = 0; i < array4.Length; i++)
						{
							array = (string[])base.EnsureArrayIndex(array, num, typeof(string));
							array[num++] = array4[i];
						}
					}
					else if (!array3[12] && base.Reader.LocalName == this.id49_PSName && base.Reader.NamespaceURI == this.id4_Item)
					{
						cmdletParameterMetadataForGetCmdletParameter.PSName = base.Reader.Value;
						array3[12] = true;
					}
					else if (!array3[13] && base.Reader.LocalName == this.id50_Position && base.Reader.NamespaceURI == this.id4_Item)
					{
						cmdletParameterMetadataForGetCmdletParameter.Position = base.CollapseWhitespace(base.Reader.Value);
						array3[13] = true;
					}
					else if (!array3[14] && base.Reader.LocalName == this.id51_ValueFromPipeline && base.Reader.NamespaceURI == this.id4_Item)
					{
						cmdletParameterMetadataForGetCmdletParameter.ValueFromPipeline = XmlConvert.ToBoolean(base.Reader.Value);
						cmdletParameterMetadataForGetCmdletParameter.ValueFromPipelineSpecified = true;
						array3[14] = true;
					}
					else if (!array3[15] && base.Reader.LocalName == this.id52_Item && base.Reader.NamespaceURI == this.id4_Item)
					{
						cmdletParameterMetadataForGetCmdletParameter.ValueFromPipelineByPropertyName = XmlConvert.ToBoolean(base.Reader.Value);
						cmdletParameterMetadataForGetCmdletParameter.ValueFromPipelineByPropertyNameSpecified = true;
						array3[15] = true;
					}
					else if (base.Reader.LocalName == this.id53_CmdletParameterSets && base.Reader.NamespaceURI == this.id4_Item)
					{
						string value2 = base.Reader.Value;
						string[] array5 = value2.Split(null);
						for (int j = 0; j < array5.Length; j++)
						{
							array2 = (string[])base.EnsureArrayIndex(array2, num2, typeof(string));
							array2[num2++] = array5[j];
						}
					}
					else if (!base.IsXmlnsAttribute(base.Reader.Name))
					{
						base.UnknownNode(cmdletParameterMetadataForGetCmdletParameter, ":IsMandatory, :Aliases, :PSName, :Position, :ValueFromPipeline, :ValueFromPipelineByPropertyName, :CmdletParameterSets");
					}
				}
				base.Reader.MoveToElement();
				if (base.Reader.IsEmptyElement)
				{
					base.Reader.Skip();
					cmdletParameterMetadataForGetCmdletParameter.Aliases = (string[])base.ShrinkArray(array, num, typeof(string), true);
					cmdletParameterMetadataForGetCmdletParameter.CmdletParameterSets = (string[])base.ShrinkArray(array2, num2, typeof(string), true);
					return cmdletParameterMetadataForGetCmdletParameter;
				}
				base.Reader.ReadStartElement();
				base.Reader.MoveToContent();
				int num3 = 0;
				int readerCount = base.ReaderCount;
				while (base.Reader.NodeType != XmlNodeType.EndElement && base.Reader.NodeType != XmlNodeType.None)
				{
					if (base.Reader.NodeType == XmlNodeType.Element)
					{
						if (!array3[0] && base.Reader.LocalName == this.id55_AllowEmptyCollection && base.Reader.NamespaceURI == this.id2_Item)
						{
							cmdletParameterMetadataForGetCmdletParameter.AllowEmptyCollection = this.Read1_Object(false, true);
							array3[0] = true;
						}
						else if (!array3[1] && base.Reader.LocalName == this.id56_AllowEmptyString && base.Reader.NamespaceURI == this.id2_Item)
						{
							cmdletParameterMetadataForGetCmdletParameter.AllowEmptyString = this.Read1_Object(false, true);
							array3[1] = true;
						}
						else if (!array3[2] && base.Reader.LocalName == this.id57_AllowNull && base.Reader.NamespaceURI == this.id2_Item)
						{
							cmdletParameterMetadataForGetCmdletParameter.AllowNull = this.Read1_Object(false, true);
							array3[2] = true;
						}
						else if (!array3[3] && base.Reader.LocalName == this.id58_ValidateNotNull && base.Reader.NamespaceURI == this.id2_Item)
						{
							cmdletParameterMetadataForGetCmdletParameter.ValidateNotNull = this.Read1_Object(false, true);
							array3[3] = true;
						}
						else if (!array3[4] && base.Reader.LocalName == this.id59_ValidateNotNullOrEmpty && base.Reader.NamespaceURI == this.id2_Item)
						{
							cmdletParameterMetadataForGetCmdletParameter.ValidateNotNullOrEmpty = this.Read1_Object(false, true);
							array3[4] = true;
						}
						else if (!array3[5] && base.Reader.LocalName == this.id60_ValidateCount && base.Reader.NamespaceURI == this.id2_Item)
						{
							cmdletParameterMetadataForGetCmdletParameter.ValidateCount = this.Read4_Item(false, true);
							array3[5] = true;
						}
						else if (!array3[6] && base.Reader.LocalName == this.id61_ValidateLength && base.Reader.NamespaceURI == this.id2_Item)
						{
							cmdletParameterMetadataForGetCmdletParameter.ValidateLength = this.Read5_Item(false, true);
							array3[6] = true;
						}
						else if (!array3[7] && base.Reader.LocalName == this.id62_ValidateRange && base.Reader.NamespaceURI == this.id2_Item)
						{
							cmdletParameterMetadataForGetCmdletParameter.ValidateRange = this.Read6_Item(false, true);
							array3[7] = true;
						}
						else if (base.Reader.LocalName == this.id63_ValidateSet && base.Reader.NamespaceURI == this.id2_Item)
						{
							if (!base.ReadNull())
							{
								string[] array6 = null;
								int num4 = 0;
								if (base.Reader.IsEmptyElement)
								{
									base.Reader.Skip();
								}
								else
								{
									base.Reader.ReadStartElement();
									base.Reader.MoveToContent();
									int num5 = 0;
									int readerCount2 = base.ReaderCount;
									while (base.Reader.NodeType != XmlNodeType.EndElement && base.Reader.NodeType != XmlNodeType.None)
									{
										if (base.Reader.NodeType == XmlNodeType.Element)
										{
											if (base.Reader.LocalName == this.id64_AllowedValue && base.Reader.NamespaceURI == this.id2_Item)
											{
												array6 = (string[])base.EnsureArrayIndex(array6, num4, typeof(string));
												array6[num4++] = base.Reader.ReadElementString();
											}
											else
											{
												base.UnknownNode(null, "http://schemas.microsoft.com/cmdlets-over-objects/2009/11:AllowedValue");
											}
										}
										else
										{
											base.UnknownNode(null, "http://schemas.microsoft.com/cmdlets-over-objects/2009/11:AllowedValue");
										}
										base.Reader.MoveToContent();
										base.CheckReaderCount(ref num5, ref readerCount2);
									}
									base.ReadEndElement();
								}
								cmdletParameterMetadataForGetCmdletParameter.ValidateSet = (string[])base.ShrinkArray(array6, num4, typeof(string), false);
							}
						}
						else if (!array3[9] && base.Reader.LocalName == this.id65_Obsolete && base.Reader.NamespaceURI == this.id2_Item)
						{
							cmdletParameterMetadataForGetCmdletParameter.Obsolete = this.Read7_ObsoleteAttributeMetadata(false, true);
							array3[9] = true;
						}
						else
						{
							base.UnknownNode(cmdletParameterMetadataForGetCmdletParameter, "http://schemas.microsoft.com/cmdlets-over-objects/2009/11:AllowEmptyCollection, http://schemas.microsoft.com/cmdlets-over-objects/2009/11:AllowEmptyString, http://schemas.microsoft.com/cmdlets-over-objects/2009/11:AllowNull, http://schemas.microsoft.com/cmdlets-over-objects/2009/11:ValidateNotNull, http://schemas.microsoft.com/cmdlets-over-objects/2009/11:ValidateNotNullOrEmpty, http://schemas.microsoft.com/cmdlets-over-objects/2009/11:ValidateCount, http://schemas.microsoft.com/cmdlets-over-objects/2009/11:ValidateLength, http://schemas.microsoft.com/cmdlets-over-objects/2009/11:ValidateRange, http://schemas.microsoft.com/cmdlets-over-objects/2009/11:ValidateSet, http://schemas.microsoft.com/cmdlets-over-objects/2009/11:Obsolete");
						}
					}
					else
					{
						base.UnknownNode(cmdletParameterMetadataForGetCmdletParameter, "http://schemas.microsoft.com/cmdlets-over-objects/2009/11:AllowEmptyCollection, http://schemas.microsoft.com/cmdlets-over-objects/2009/11:AllowEmptyString, http://schemas.microsoft.com/cmdlets-over-objects/2009/11:AllowNull, http://schemas.microsoft.com/cmdlets-over-objects/2009/11:ValidateNotNull, http://schemas.microsoft.com/cmdlets-over-objects/2009/11:ValidateNotNullOrEmpty, http://schemas.microsoft.com/cmdlets-over-objects/2009/11:ValidateCount, http://schemas.microsoft.com/cmdlets-over-objects/2009/11:ValidateLength, http://schemas.microsoft.com/cmdlets-over-objects/2009/11:ValidateRange, http://schemas.microsoft.com/cmdlets-over-objects/2009/11:ValidateSet, http://schemas.microsoft.com/cmdlets-over-objects/2009/11:Obsolete");
					}
					base.Reader.MoveToContent();
					base.CheckReaderCount(ref num3, ref readerCount);
				}
				cmdletParameterMetadataForGetCmdletParameter.Aliases = (string[])base.ShrinkArray(array, num, typeof(string), true);
				cmdletParameterMetadataForGetCmdletParameter.CmdletParameterSets = (string[])base.ShrinkArray(array2, num2, typeof(string), true);
				base.ReadEndElement();
				return cmdletParameterMetadataForGetCmdletParameter;
			}
		}

		// Token: 0x06005CCC RID: 23756 RVA: 0x001F8054 File Offset: 0x001F6254
		private Association Read17_Association(bool isNullable, bool checkType)
		{
			XmlQualifiedName xmlQualifiedName = checkType ? base.GetXsiType() : null;
			bool flag = false;
			if (isNullable)
			{
				flag = base.ReadNull();
			}
			if (checkType && !(xmlQualifiedName == null) && (xmlQualifiedName.Name != this.id9_Association || xmlQualifiedName.Namespace != this.id2_Item))
			{
				throw base.CreateUnknownTypeException(xmlQualifiedName);
			}
			if (flag)
			{
				return null;
			}
			Association association = new Association();
			bool[] array = new bool[4];
			while (base.Reader.MoveToNextAttribute())
			{
				if (!array[1] && base.Reader.LocalName == this.id9_Association && base.Reader.NamespaceURI == this.id4_Item)
				{
					association.Association1 = base.Reader.Value;
					array[1] = true;
				}
				else if (!array[2] && base.Reader.LocalName == this.id101_SourceRole && base.Reader.NamespaceURI == this.id4_Item)
				{
					association.SourceRole = base.Reader.Value;
					array[2] = true;
				}
				else if (!array[3] && base.Reader.LocalName == this.id102_ResultRole && base.Reader.NamespaceURI == this.id4_Item)
				{
					association.ResultRole = base.Reader.Value;
					array[3] = true;
				}
				else if (!base.IsXmlnsAttribute(base.Reader.Name))
				{
					base.UnknownNode(association, ":Association, :SourceRole, :ResultRole");
				}
			}
			base.Reader.MoveToElement();
			if (base.Reader.IsEmptyElement)
			{
				base.Reader.Skip();
				return association;
			}
			base.Reader.ReadStartElement();
			base.Reader.MoveToContent();
			int num = 0;
			int readerCount = base.ReaderCount;
			while (base.Reader.NodeType != XmlNodeType.EndElement && base.Reader.NodeType != XmlNodeType.None)
			{
				if (base.Reader.NodeType == XmlNodeType.Element)
				{
					if (!array[0] && base.Reader.LocalName == this.id103_AssociatedInstance && base.Reader.NamespaceURI == this.id2_Item)
					{
						association.AssociatedInstance = this.Read16_AssociationAssociatedInstance(false, true);
						array[0] = true;
					}
					else
					{
						base.UnknownNode(association, "http://schemas.microsoft.com/cmdlets-over-objects/2009/11:AssociatedInstance");
					}
				}
				else
				{
					base.UnknownNode(association, "http://schemas.microsoft.com/cmdlets-over-objects/2009/11:AssociatedInstance");
				}
				base.Reader.MoveToContent();
				base.CheckReaderCount(ref num, ref readerCount);
			}
			base.ReadEndElement();
			return association;
		}

		// Token: 0x06005CCD RID: 23757 RVA: 0x001F82A8 File Offset: 0x001F64A8
		private AssociationAssociatedInstance Read16_AssociationAssociatedInstance(bool isNullable, bool checkType)
		{
			XmlQualifiedName xmlQualifiedName = checkType ? base.GetXsiType() : null;
			bool flag = false;
			if (isNullable)
			{
				flag = base.ReadNull();
			}
			if (checkType && !(xmlQualifiedName == null) && (xmlQualifiedName.Name != this.id4_Item || xmlQualifiedName.Namespace != this.id2_Item))
			{
				throw base.CreateUnknownTypeException(xmlQualifiedName);
			}
			if (flag)
			{
				return null;
			}
			AssociationAssociatedInstance associationAssociatedInstance = new AssociationAssociatedInstance();
			bool[] array = new bool[2];
			while (base.Reader.MoveToNextAttribute())
			{
				if (!base.IsXmlnsAttribute(base.Reader.Name))
				{
					base.UnknownNode(associationAssociatedInstance);
				}
			}
			base.Reader.MoveToElement();
			if (base.Reader.IsEmptyElement)
			{
				base.Reader.Skip();
				return associationAssociatedInstance;
			}
			base.Reader.ReadStartElement();
			base.Reader.MoveToContent();
			int num = 0;
			int readerCount = base.ReaderCount;
			while (base.Reader.NodeType != XmlNodeType.EndElement && base.Reader.NodeType != XmlNodeType.None)
			{
				if (base.Reader.NodeType == XmlNodeType.Element)
				{
					if (!array[0] && base.Reader.LocalName == this.id92_Type && base.Reader.NamespaceURI == this.id2_Item)
					{
						associationAssociatedInstance.Type = this.Read2_TypeMetadata(false, true);
						array[0] = true;
					}
					else if (!array[1] && base.Reader.LocalName == this.id11_CmdletParameterMetadata && base.Reader.NamespaceURI == this.id2_Item)
					{
						associationAssociatedInstance.CmdletParameterMetadata = this.Read12_Item(false, true);
						array[1] = true;
					}
					else
					{
						base.UnknownNode(associationAssociatedInstance, "http://schemas.microsoft.com/cmdlets-over-objects/2009/11:Type, http://schemas.microsoft.com/cmdlets-over-objects/2009/11:CmdletParameterMetadata");
					}
				}
				else
				{
					base.UnknownNode(associationAssociatedInstance, "http://schemas.microsoft.com/cmdlets-over-objects/2009/11:Type, http://schemas.microsoft.com/cmdlets-over-objects/2009/11:CmdletParameterMetadata");
				}
				base.Reader.MoveToContent();
				base.CheckReaderCount(ref num, ref readerCount);
			}
			base.ReadEndElement();
			return associationAssociatedInstance;
		}

		// Token: 0x06005CCE RID: 23758 RVA: 0x001F846C File Offset: 0x001F666C
		private PropertyMetadata Read15_PropertyMetadata(bool isNullable, bool checkType)
		{
			XmlQualifiedName xmlQualifiedName = checkType ? base.GetXsiType() : null;
			bool flag = false;
			if (isNullable)
			{
				flag = base.ReadNull();
			}
			if (checkType && !(xmlQualifiedName == null) && (xmlQualifiedName.Name != this.id7_PropertyMetadata || xmlQualifiedName.Namespace != this.id2_Item))
			{
				throw base.CreateUnknownTypeException(xmlQualifiedName);
			}
			if (flag)
			{
				return null;
			}
			PropertyMetadata propertyMetadata = new PropertyMetadata();
			PropertyQuery[] array = null;
			int num = 0;
			ItemsChoiceType[] array2 = null;
			int num2 = 0;
			bool[] array3 = new bool[3];
			while (base.Reader.MoveToNextAttribute())
			{
				if (!array3[2] && base.Reader.LocalName == this.id104_PropertyName && base.Reader.NamespaceURI == this.id4_Item)
				{
					propertyMetadata.PropertyName = base.Reader.Value;
					array3[2] = true;
				}
				else if (!base.IsXmlnsAttribute(base.Reader.Name))
				{
					base.UnknownNode(propertyMetadata, ":PropertyName");
				}
			}
			base.Reader.MoveToElement();
			if (base.Reader.IsEmptyElement)
			{
				base.Reader.Skip();
				propertyMetadata.Items = (PropertyQuery[])base.ShrinkArray(array, num, typeof(PropertyQuery), true);
				propertyMetadata.ItemsElementName = (ItemsChoiceType[])base.ShrinkArray(array2, num2, typeof(ItemsChoiceType), true);
				return propertyMetadata;
			}
			base.Reader.ReadStartElement();
			base.Reader.MoveToContent();
			int num3 = 0;
			int readerCount = base.ReaderCount;
			while (base.Reader.NodeType != XmlNodeType.EndElement && base.Reader.NodeType != XmlNodeType.None)
			{
				if (base.Reader.NodeType == XmlNodeType.Element)
				{
					if (!array3[0] && base.Reader.LocalName == this.id92_Type && base.Reader.NamespaceURI == this.id2_Item)
					{
						propertyMetadata.Type = this.Read2_TypeMetadata(false, true);
						array3[0] = true;
					}
					else if (base.Reader.LocalName == this.id105_MaxValueQuery && base.Reader.NamespaceURI == this.id2_Item)
					{
						array = (PropertyQuery[])base.EnsureArrayIndex(array, num, typeof(PropertyQuery));
						array[num++] = this.Read14_PropertyQuery(false, true);
						array2 = (ItemsChoiceType[])base.EnsureArrayIndex(array2, num2, typeof(ItemsChoiceType));
						array2[num2++] = ItemsChoiceType.MaxValueQuery;
					}
					else if (base.Reader.LocalName == this.id106_RegularQuery && base.Reader.NamespaceURI == this.id2_Item)
					{
						array = (PropertyQuery[])base.EnsureArrayIndex(array, num, typeof(PropertyQuery));
						array[num++] = this.Read13_WildcardablePropertyQuery(false, true);
						array2 = (ItemsChoiceType[])base.EnsureArrayIndex(array2, num2, typeof(ItemsChoiceType));
						array2[num2++] = ItemsChoiceType.RegularQuery;
					}
					else if (base.Reader.LocalName == this.id107_ExcludeQuery && base.Reader.NamespaceURI == this.id2_Item)
					{
						array = (PropertyQuery[])base.EnsureArrayIndex(array, num, typeof(PropertyQuery));
						array[num++] = this.Read13_WildcardablePropertyQuery(false, true);
						array2 = (ItemsChoiceType[])base.EnsureArrayIndex(array2, num2, typeof(ItemsChoiceType));
						array2[num2++] = ItemsChoiceType.ExcludeQuery;
					}
					else if (base.Reader.LocalName == this.id108_MinValueQuery && base.Reader.NamespaceURI == this.id2_Item)
					{
						array = (PropertyQuery[])base.EnsureArrayIndex(array, num, typeof(PropertyQuery));
						array[num++] = this.Read14_PropertyQuery(false, true);
						array2 = (ItemsChoiceType[])base.EnsureArrayIndex(array2, num2, typeof(ItemsChoiceType));
						array2[num2++] = ItemsChoiceType.MinValueQuery;
					}
					else
					{
						base.UnknownNode(propertyMetadata, "http://schemas.microsoft.com/cmdlets-over-objects/2009/11:Type, http://schemas.microsoft.com/cmdlets-over-objects/2009/11:MaxValueQuery, http://schemas.microsoft.com/cmdlets-over-objects/2009/11:RegularQuery, http://schemas.microsoft.com/cmdlets-over-objects/2009/11:ExcludeQuery, http://schemas.microsoft.com/cmdlets-over-objects/2009/11:MinValueQuery");
					}
				}
				else
				{
					base.UnknownNode(propertyMetadata, "http://schemas.microsoft.com/cmdlets-over-objects/2009/11:Type, http://schemas.microsoft.com/cmdlets-over-objects/2009/11:MaxValueQuery, http://schemas.microsoft.com/cmdlets-over-objects/2009/11:RegularQuery, http://schemas.microsoft.com/cmdlets-over-objects/2009/11:ExcludeQuery, http://schemas.microsoft.com/cmdlets-over-objects/2009/11:MinValueQuery");
				}
				base.Reader.MoveToContent();
				base.CheckReaderCount(ref num3, ref readerCount);
			}
			propertyMetadata.Items = (PropertyQuery[])base.ShrinkArray(array, num, typeof(PropertyQuery), true);
			propertyMetadata.ItemsElementName = (ItemsChoiceType[])base.ShrinkArray(array2, num2, typeof(ItemsChoiceType), true);
			base.ReadEndElement();
			return propertyMetadata;
		}

		// Token: 0x06005CCF RID: 23759 RVA: 0x001F88B4 File Offset: 0x001F6AB4
		private PropertyQuery Read14_PropertyQuery(bool isNullable, bool checkType)
		{
			XmlQualifiedName xmlQualifiedName = checkType ? base.GetXsiType() : null;
			bool flag = false;
			if (isNullable)
			{
				flag = base.ReadNull();
			}
			if (checkType && !(xmlQualifiedName == null) && (xmlQualifiedName.Name != this.id35_PropertyQuery || xmlQualifiedName.Namespace != this.id2_Item))
			{
				if (xmlQualifiedName.Name == this.id36_WildcardablePropertyQuery && xmlQualifiedName.Namespace == this.id2_Item)
				{
					return this.Read13_WildcardablePropertyQuery(isNullable, false);
				}
				throw base.CreateUnknownTypeException(xmlQualifiedName);
			}
			else
			{
				if (flag)
				{
					return null;
				}
				PropertyQuery propertyQuery = new PropertyQuery();
				bool[] array = new bool[1];
				while (base.Reader.MoveToNextAttribute())
				{
					if (!base.IsXmlnsAttribute(base.Reader.Name))
					{
						base.UnknownNode(propertyQuery);
					}
				}
				base.Reader.MoveToElement();
				if (base.Reader.IsEmptyElement)
				{
					base.Reader.Skip();
					return propertyQuery;
				}
				base.Reader.ReadStartElement();
				base.Reader.MoveToContent();
				int num = 0;
				int readerCount = base.ReaderCount;
				while (base.Reader.NodeType != XmlNodeType.EndElement && base.Reader.NodeType != XmlNodeType.None)
				{
					if (base.Reader.NodeType == XmlNodeType.Element)
					{
						if (!array[0] && base.Reader.LocalName == this.id11_CmdletParameterMetadata && base.Reader.NamespaceURI == this.id2_Item)
						{
							propertyQuery.CmdletParameterMetadata = this.Read12_Item(false, true);
							array[0] = true;
						}
						else
						{
							base.UnknownNode(propertyQuery, "http://schemas.microsoft.com/cmdlets-over-objects/2009/11:CmdletParameterMetadata");
						}
					}
					else
					{
						base.UnknownNode(propertyQuery, "http://schemas.microsoft.com/cmdlets-over-objects/2009/11:CmdletParameterMetadata");
					}
					base.Reader.MoveToContent();
					base.CheckReaderCount(ref num, ref readerCount);
				}
				base.ReadEndElement();
				return propertyQuery;
			}
		}

		// Token: 0x06005CD0 RID: 23760 RVA: 0x001F8A58 File Offset: 0x001F6C58
		private CmdletParameterMetadata Read10_CmdletParameterMetadata(bool isNullable, bool checkType)
		{
			XmlQualifiedName xmlQualifiedName = checkType ? base.GetXsiType() : null;
			bool flag = false;
			if (isNullable)
			{
				flag = base.ReadNull();
			}
			if (checkType && !(xmlQualifiedName == null) && (xmlQualifiedName.Name != this.id11_CmdletParameterMetadata || xmlQualifiedName.Namespace != this.id2_Item))
			{
				if (xmlQualifiedName.Name == this.id12_Item && xmlQualifiedName.Namespace == this.id2_Item)
				{
					return this.Read11_Item(isNullable, false);
				}
				if (xmlQualifiedName.Name == this.id13_Item && xmlQualifiedName.Namespace == this.id2_Item)
				{
					return this.Read12_Item(isNullable, false);
				}
				if (xmlQualifiedName.Name == this.id18_Item && xmlQualifiedName.Namespace == this.id2_Item)
				{
					return this.Read9_Item(isNullable, false);
				}
				if (xmlQualifiedName.Name == this.id19_Item && xmlQualifiedName.Namespace == this.id2_Item)
				{
					return this.Read8_Item(isNullable, false);
				}
				throw base.CreateUnknownTypeException(xmlQualifiedName);
			}
			else
			{
				if (flag)
				{
					return null;
				}
				CmdletParameterMetadata cmdletParameterMetadata = new CmdletParameterMetadata();
				string[] array = null;
				int num = 0;
				bool[] array2 = new bool[14];
				while (base.Reader.MoveToNextAttribute())
				{
					if (!array2[10] && base.Reader.LocalName == this.id47_IsMandatory && base.Reader.NamespaceURI == this.id4_Item)
					{
						cmdletParameterMetadata.IsMandatory = XmlConvert.ToBoolean(base.Reader.Value);
						cmdletParameterMetadata.IsMandatorySpecified = true;
						array2[10] = true;
					}
					else if (base.Reader.LocalName == this.id48_Aliases && base.Reader.NamespaceURI == this.id4_Item)
					{
						string value = base.Reader.Value;
						string[] array3 = value.Split(null);
						for (int i = 0; i < array3.Length; i++)
						{
							array = (string[])base.EnsureArrayIndex(array, num, typeof(string));
							array[num++] = array3[i];
						}
					}
					else if (!array2[12] && base.Reader.LocalName == this.id49_PSName && base.Reader.NamespaceURI == this.id4_Item)
					{
						cmdletParameterMetadata.PSName = base.Reader.Value;
						array2[12] = true;
					}
					else if (!array2[13] && base.Reader.LocalName == this.id50_Position && base.Reader.NamespaceURI == this.id4_Item)
					{
						cmdletParameterMetadata.Position = base.CollapseWhitespace(base.Reader.Value);
						array2[13] = true;
					}
					else if (!base.IsXmlnsAttribute(base.Reader.Name))
					{
						base.UnknownNode(cmdletParameterMetadata, ":IsMandatory, :Aliases, :PSName, :Position");
					}
				}
				base.Reader.MoveToElement();
				if (base.Reader.IsEmptyElement)
				{
					base.Reader.Skip();
					cmdletParameterMetadata.Aliases = (string[])base.ShrinkArray(array, num, typeof(string), true);
					return cmdletParameterMetadata;
				}
				base.Reader.ReadStartElement();
				base.Reader.MoveToContent();
				int num2 = 0;
				int readerCount = base.ReaderCount;
				while (base.Reader.NodeType != XmlNodeType.EndElement && base.Reader.NodeType != XmlNodeType.None)
				{
					if (base.Reader.NodeType == XmlNodeType.Element)
					{
						if (!array2[0] && base.Reader.LocalName == this.id55_AllowEmptyCollection && base.Reader.NamespaceURI == this.id2_Item)
						{
							cmdletParameterMetadata.AllowEmptyCollection = this.Read1_Object(false, true);
							array2[0] = true;
						}
						else if (!array2[1] && base.Reader.LocalName == this.id56_AllowEmptyString && base.Reader.NamespaceURI == this.id2_Item)
						{
							cmdletParameterMetadata.AllowEmptyString = this.Read1_Object(false, true);
							array2[1] = true;
						}
						else if (!array2[2] && base.Reader.LocalName == this.id57_AllowNull && base.Reader.NamespaceURI == this.id2_Item)
						{
							cmdletParameterMetadata.AllowNull = this.Read1_Object(false, true);
							array2[2] = true;
						}
						else if (!array2[3] && base.Reader.LocalName == this.id58_ValidateNotNull && base.Reader.NamespaceURI == this.id2_Item)
						{
							cmdletParameterMetadata.ValidateNotNull = this.Read1_Object(false, true);
							array2[3] = true;
						}
						else if (!array2[4] && base.Reader.LocalName == this.id59_ValidateNotNullOrEmpty && base.Reader.NamespaceURI == this.id2_Item)
						{
							cmdletParameterMetadata.ValidateNotNullOrEmpty = this.Read1_Object(false, true);
							array2[4] = true;
						}
						else if (!array2[5] && base.Reader.LocalName == this.id60_ValidateCount && base.Reader.NamespaceURI == this.id2_Item)
						{
							cmdletParameterMetadata.ValidateCount = this.Read4_Item(false, true);
							array2[5] = true;
						}
						else if (!array2[6] && base.Reader.LocalName == this.id61_ValidateLength && base.Reader.NamespaceURI == this.id2_Item)
						{
							cmdletParameterMetadata.ValidateLength = this.Read5_Item(false, true);
							array2[6] = true;
						}
						else if (!array2[7] && base.Reader.LocalName == this.id62_ValidateRange && base.Reader.NamespaceURI == this.id2_Item)
						{
							cmdletParameterMetadata.ValidateRange = this.Read6_Item(false, true);
							array2[7] = true;
						}
						else if (base.Reader.LocalName == this.id63_ValidateSet && base.Reader.NamespaceURI == this.id2_Item)
						{
							if (!base.ReadNull())
							{
								string[] array4 = null;
								int num3 = 0;
								if (base.Reader.IsEmptyElement)
								{
									base.Reader.Skip();
								}
								else
								{
									base.Reader.ReadStartElement();
									base.Reader.MoveToContent();
									int num4 = 0;
									int readerCount2 = base.ReaderCount;
									while (base.Reader.NodeType != XmlNodeType.EndElement && base.Reader.NodeType != XmlNodeType.None)
									{
										if (base.Reader.NodeType == XmlNodeType.Element)
										{
											if (base.Reader.LocalName == this.id64_AllowedValue && base.Reader.NamespaceURI == this.id2_Item)
											{
												array4 = (string[])base.EnsureArrayIndex(array4, num3, typeof(string));
												array4[num3++] = base.Reader.ReadElementString();
											}
											else
											{
												base.UnknownNode(null, "http://schemas.microsoft.com/cmdlets-over-objects/2009/11:AllowedValue");
											}
										}
										else
										{
											base.UnknownNode(null, "http://schemas.microsoft.com/cmdlets-over-objects/2009/11:AllowedValue");
										}
										base.Reader.MoveToContent();
										base.CheckReaderCount(ref num4, ref readerCount2);
									}
									base.ReadEndElement();
								}
								cmdletParameterMetadata.ValidateSet = (string[])base.ShrinkArray(array4, num3, typeof(string), false);
							}
						}
						else if (!array2[9] && base.Reader.LocalName == this.id65_Obsolete && base.Reader.NamespaceURI == this.id2_Item)
						{
							cmdletParameterMetadata.Obsolete = this.Read7_ObsoleteAttributeMetadata(false, true);
							array2[9] = true;
						}
						else
						{
							base.UnknownNode(cmdletParameterMetadata, "http://schemas.microsoft.com/cmdlets-over-objects/2009/11:AllowEmptyCollection, http://schemas.microsoft.com/cmdlets-over-objects/2009/11:AllowEmptyString, http://schemas.microsoft.com/cmdlets-over-objects/2009/11:AllowNull, http://schemas.microsoft.com/cmdlets-over-objects/2009/11:ValidateNotNull, http://schemas.microsoft.com/cmdlets-over-objects/2009/11:ValidateNotNullOrEmpty, http://schemas.microsoft.com/cmdlets-over-objects/2009/11:ValidateCount, http://schemas.microsoft.com/cmdlets-over-objects/2009/11:ValidateLength, http://schemas.microsoft.com/cmdlets-over-objects/2009/11:ValidateRange, http://schemas.microsoft.com/cmdlets-over-objects/2009/11:ValidateSet, http://schemas.microsoft.com/cmdlets-over-objects/2009/11:Obsolete");
						}
					}
					else
					{
						base.UnknownNode(cmdletParameterMetadata, "http://schemas.microsoft.com/cmdlets-over-objects/2009/11:AllowEmptyCollection, http://schemas.microsoft.com/cmdlets-over-objects/2009/11:AllowEmptyString, http://schemas.microsoft.com/cmdlets-over-objects/2009/11:AllowNull, http://schemas.microsoft.com/cmdlets-over-objects/2009/11:ValidateNotNull, http://schemas.microsoft.com/cmdlets-over-objects/2009/11:ValidateNotNullOrEmpty, http://schemas.microsoft.com/cmdlets-over-objects/2009/11:ValidateCount, http://schemas.microsoft.com/cmdlets-over-objects/2009/11:ValidateLength, http://schemas.microsoft.com/cmdlets-over-objects/2009/11:ValidateRange, http://schemas.microsoft.com/cmdlets-over-objects/2009/11:ValidateSet, http://schemas.microsoft.com/cmdlets-over-objects/2009/11:Obsolete");
					}
					base.Reader.MoveToContent();
					base.CheckReaderCount(ref num2, ref readerCount);
				}
				cmdletParameterMetadata.Aliases = (string[])base.ShrinkArray(array, num, typeof(string), true);
				base.ReadEndElement();
				return cmdletParameterMetadata;
			}
		}

		// Token: 0x06005CD1 RID: 23761 RVA: 0x001F91B0 File Offset: 0x001F73B0
		private GetCmdletParameters Read19_GetCmdletParameters(bool isNullable, bool checkType)
		{
			XmlQualifiedName xmlQualifiedName = checkType ? base.GetXsiType() : null;
			bool flag = false;
			if (isNullable)
			{
				flag = base.ReadNull();
			}
			if (checkType && !(xmlQualifiedName == null) && (xmlQualifiedName.Name != this.id6_GetCmdletParameters || xmlQualifiedName.Namespace != this.id2_Item))
			{
				throw base.CreateUnknownTypeException(xmlQualifiedName);
			}
			if (flag)
			{
				return null;
			}
			GetCmdletParameters getCmdletParameters = new GetCmdletParameters();
			bool[] array = new bool[4];
			while (base.Reader.MoveToNextAttribute())
			{
				if (!array[3] && base.Reader.LocalName == this.id99_DefaultCmdletParameterSet && base.Reader.NamespaceURI == this.id4_Item)
				{
					getCmdletParameters.DefaultCmdletParameterSet = base.Reader.Value;
					array[3] = true;
				}
				else if (!base.IsXmlnsAttribute(base.Reader.Name))
				{
					base.UnknownNode(getCmdletParameters, ":DefaultCmdletParameterSet");
				}
			}
			base.Reader.MoveToElement();
			if (base.Reader.IsEmptyElement)
			{
				base.Reader.Skip();
				return getCmdletParameters;
			}
			base.Reader.ReadStartElement();
			base.Reader.MoveToContent();
			int num = 0;
			int readerCount = base.ReaderCount;
			while (base.Reader.NodeType != XmlNodeType.EndElement && base.Reader.NodeType != XmlNodeType.None)
			{
				if (base.Reader.NodeType == XmlNodeType.Element)
				{
					if (base.Reader.LocalName == this.id109_QueryableProperties && base.Reader.NamespaceURI == this.id2_Item)
					{
						if (!base.ReadNull())
						{
							PropertyMetadata[] array2 = null;
							int num2 = 0;
							if (base.Reader.IsEmptyElement)
							{
								base.Reader.Skip();
							}
							else
							{
								base.Reader.ReadStartElement();
								base.Reader.MoveToContent();
								int num3 = 0;
								int readerCount2 = base.ReaderCount;
								while (base.Reader.NodeType != XmlNodeType.EndElement && base.Reader.NodeType != XmlNodeType.None)
								{
									if (base.Reader.NodeType == XmlNodeType.Element)
									{
										if (base.Reader.LocalName == this.id71_Property && base.Reader.NamespaceURI == this.id2_Item)
										{
											array2 = (PropertyMetadata[])base.EnsureArrayIndex(array2, num2, typeof(PropertyMetadata));
											array2[num2++] = this.Read15_PropertyMetadata(false, true);
										}
										else
										{
											base.UnknownNode(null, "http://schemas.microsoft.com/cmdlets-over-objects/2009/11:Property");
										}
									}
									else
									{
										base.UnknownNode(null, "http://schemas.microsoft.com/cmdlets-over-objects/2009/11:Property");
									}
									base.Reader.MoveToContent();
									base.CheckReaderCount(ref num3, ref readerCount2);
								}
								base.ReadEndElement();
							}
							getCmdletParameters.QueryableProperties = (PropertyMetadata[])base.ShrinkArray(array2, num2, typeof(PropertyMetadata), false);
						}
					}
					else if (base.Reader.LocalName == this.id110_QueryableAssociations && base.Reader.NamespaceURI == this.id2_Item)
					{
						if (!base.ReadNull())
						{
							Association[] array3 = null;
							int num4 = 0;
							if (base.Reader.IsEmptyElement)
							{
								base.Reader.Skip();
							}
							else
							{
								base.Reader.ReadStartElement();
								base.Reader.MoveToContent();
								int num5 = 0;
								int readerCount3 = base.ReaderCount;
								while (base.Reader.NodeType != XmlNodeType.EndElement && base.Reader.NodeType != XmlNodeType.None)
								{
									if (base.Reader.NodeType == XmlNodeType.Element)
									{
										if (base.Reader.LocalName == this.id9_Association && base.Reader.NamespaceURI == this.id2_Item)
										{
											array3 = (Association[])base.EnsureArrayIndex(array3, num4, typeof(Association));
											array3[num4++] = this.Read17_Association(false, true);
										}
										else
										{
											base.UnknownNode(null, "http://schemas.microsoft.com/cmdlets-over-objects/2009/11:Association");
										}
									}
									else
									{
										base.UnknownNode(null, "http://schemas.microsoft.com/cmdlets-over-objects/2009/11:Association");
									}
									base.Reader.MoveToContent();
									base.CheckReaderCount(ref num5, ref readerCount3);
								}
								base.ReadEndElement();
							}
							getCmdletParameters.QueryableAssociations = (Association[])base.ShrinkArray(array3, num4, typeof(Association), false);
						}
					}
					else if (base.Reader.LocalName == this.id111_QueryOptions && base.Reader.NamespaceURI == this.id2_Item)
					{
						if (!base.ReadNull())
						{
							QueryOption[] array4 = null;
							int num6 = 0;
							if (base.Reader.IsEmptyElement)
							{
								base.Reader.Skip();
							}
							else
							{
								base.Reader.ReadStartElement();
								base.Reader.MoveToContent();
								int num7 = 0;
								int readerCount4 = base.ReaderCount;
								while (base.Reader.NodeType != XmlNodeType.EndElement && base.Reader.NodeType != XmlNodeType.None)
								{
									if (base.Reader.NodeType == XmlNodeType.Element)
									{
										if (base.Reader.LocalName == this.id74_Option && base.Reader.NamespaceURI == this.id2_Item)
										{
											array4 = (QueryOption[])base.EnsureArrayIndex(array4, num6, typeof(QueryOption));
											array4[num6++] = this.Read18_QueryOption(false, true);
										}
										else
										{
											base.UnknownNode(null, "http://schemas.microsoft.com/cmdlets-over-objects/2009/11:Option");
										}
									}
									else
									{
										base.UnknownNode(null, "http://schemas.microsoft.com/cmdlets-over-objects/2009/11:Option");
									}
									base.Reader.MoveToContent();
									base.CheckReaderCount(ref num7, ref readerCount4);
								}
								base.ReadEndElement();
							}
							getCmdletParameters.QueryOptions = (QueryOption[])base.ShrinkArray(array4, num6, typeof(QueryOption), false);
						}
					}
					else
					{
						base.UnknownNode(getCmdletParameters, "http://schemas.microsoft.com/cmdlets-over-objects/2009/11:QueryableProperties, http://schemas.microsoft.com/cmdlets-over-objects/2009/11:QueryableAssociations, http://schemas.microsoft.com/cmdlets-over-objects/2009/11:QueryOptions");
					}
				}
				else
				{
					base.UnknownNode(getCmdletParameters, "http://schemas.microsoft.com/cmdlets-over-objects/2009/11:QueryableProperties, http://schemas.microsoft.com/cmdlets-over-objects/2009/11:QueryableAssociations, http://schemas.microsoft.com/cmdlets-over-objects/2009/11:QueryOptions");
				}
				base.Reader.MoveToContent();
				base.CheckReaderCount(ref num, ref readerCount);
			}
			base.ReadEndElement();
			return getCmdletParameters;
		}

		// Token: 0x06005CD2 RID: 23762 RVA: 0x001F9754 File Offset: 0x001F7954
		private StaticCmdletMetadataCmdletMetadata Read45_Item(bool isNullable, bool checkType)
		{
			XmlQualifiedName xmlQualifiedName = checkType ? base.GetXsiType() : null;
			bool flag = false;
			if (isNullable)
			{
				flag = base.ReadNull();
			}
			if (checkType && !(xmlQualifiedName == null) && (xmlQualifiedName.Name != this.id25_Item || xmlQualifiedName.Namespace != this.id2_Item))
			{
				throw base.CreateUnknownTypeException(xmlQualifiedName);
			}
			if (flag)
			{
				return null;
			}
			StaticCmdletMetadataCmdletMetadata staticCmdletMetadataCmdletMetadata = new StaticCmdletMetadataCmdletMetadata();
			string[] array = null;
			int num = 0;
			bool[] array2 = new bool[7];
			while (base.Reader.MoveToNextAttribute())
			{
				if (!array2[1] && base.Reader.LocalName == this.id96_Verb && base.Reader.NamespaceURI == this.id4_Item)
				{
					staticCmdletMetadataCmdletMetadata.Verb = base.Reader.Value;
					array2[1] = true;
				}
				else if (!array2[2] && base.Reader.LocalName == this.id97_Noun && base.Reader.NamespaceURI == this.id4_Item)
				{
					staticCmdletMetadataCmdletMetadata.Noun = base.Reader.Value;
					array2[2] = true;
				}
				else if (base.Reader.LocalName == this.id48_Aliases && base.Reader.NamespaceURI == this.id4_Item)
				{
					string value = base.Reader.Value;
					string[] array3 = value.Split(null);
					for (int i = 0; i < array3.Length; i++)
					{
						array = (string[])base.EnsureArrayIndex(array, num, typeof(string));
						array[num++] = array3[i];
					}
				}
				else if (!array2[4] && base.Reader.LocalName == this.id23_ConfirmImpact && base.Reader.NamespaceURI == this.id4_Item)
				{
					staticCmdletMetadataCmdletMetadata.ConfirmImpact = this.Read20_ConfirmImpact(base.Reader.Value);
					staticCmdletMetadataCmdletMetadata.ConfirmImpactSpecified = true;
					array2[4] = true;
				}
				else if (!array2[5] && base.Reader.LocalName == this.id98_HelpUri && base.Reader.NamespaceURI == this.id4_Item)
				{
					staticCmdletMetadataCmdletMetadata.HelpUri = base.CollapseWhitespace(base.Reader.Value);
					array2[5] = true;
				}
				else if (!array2[6] && base.Reader.LocalName == this.id99_DefaultCmdletParameterSet && base.Reader.NamespaceURI == this.id4_Item)
				{
					staticCmdletMetadataCmdletMetadata.DefaultCmdletParameterSet = base.Reader.Value;
					array2[6] = true;
				}
				else if (!base.IsXmlnsAttribute(base.Reader.Name))
				{
					base.UnknownNode(staticCmdletMetadataCmdletMetadata, ":Verb, :Noun, :Aliases, :ConfirmImpact, :HelpUri, :DefaultCmdletParameterSet");
				}
			}
			base.Reader.MoveToElement();
			if (base.Reader.IsEmptyElement)
			{
				base.Reader.Skip();
				staticCmdletMetadataCmdletMetadata.Aliases = (string[])base.ShrinkArray(array, num, typeof(string), true);
				return staticCmdletMetadataCmdletMetadata;
			}
			base.Reader.ReadStartElement();
			base.Reader.MoveToContent();
			int num2 = 0;
			int readerCount = base.ReaderCount;
			while (base.Reader.NodeType != XmlNodeType.EndElement && base.Reader.NodeType != XmlNodeType.None)
			{
				if (base.Reader.NodeType == XmlNodeType.Element)
				{
					if (!array2[0] && base.Reader.LocalName == this.id65_Obsolete && base.Reader.NamespaceURI == this.id2_Item)
					{
						staticCmdletMetadataCmdletMetadata.Obsolete = this.Read7_ObsoleteAttributeMetadata(false, true);
						array2[0] = true;
					}
					else
					{
						base.UnknownNode(staticCmdletMetadataCmdletMetadata, "http://schemas.microsoft.com/cmdlets-over-objects/2009/11:Obsolete");
					}
				}
				else
				{
					base.UnknownNode(staticCmdletMetadataCmdletMetadata, "http://schemas.microsoft.com/cmdlets-over-objects/2009/11:Obsolete");
				}
				base.Reader.MoveToContent();
				base.CheckReaderCount(ref num2, ref readerCount);
			}
			staticCmdletMetadataCmdletMetadata.Aliases = (string[])base.ShrinkArray(array, num, typeof(string), true);
			base.ReadEndElement();
			return staticCmdletMetadataCmdletMetadata;
		}

		// Token: 0x06005CD3 RID: 23763 RVA: 0x001F9B10 File Offset: 0x001F7D10
		private CommonCmdletMetadata Read21_CommonCmdletMetadata(bool isNullable, bool checkType)
		{
			XmlQualifiedName xmlQualifiedName = checkType ? base.GetXsiType() : null;
			bool flag = false;
			if (isNullable)
			{
				flag = base.ReadNull();
			}
			if (checkType && !(xmlQualifiedName == null) && (xmlQualifiedName.Name != this.id22_CommonCmdletMetadata || xmlQualifiedName.Namespace != this.id2_Item))
			{
				if (xmlQualifiedName.Name == this.id25_Item && xmlQualifiedName.Namespace == this.id2_Item)
				{
					return this.Read45_Item(isNullable, false);
				}
				throw base.CreateUnknownTypeException(xmlQualifiedName);
			}
			else
			{
				if (flag)
				{
					return null;
				}
				CommonCmdletMetadata commonCmdletMetadata = new CommonCmdletMetadata();
				string[] array = null;
				int num = 0;
				bool[] array2 = new bool[6];
				while (base.Reader.MoveToNextAttribute())
				{
					if (!array2[1] && base.Reader.LocalName == this.id96_Verb && base.Reader.NamespaceURI == this.id4_Item)
					{
						commonCmdletMetadata.Verb = base.Reader.Value;
						array2[1] = true;
					}
					else if (!array2[2] && base.Reader.LocalName == this.id97_Noun && base.Reader.NamespaceURI == this.id4_Item)
					{
						commonCmdletMetadata.Noun = base.Reader.Value;
						array2[2] = true;
					}
					else if (base.Reader.LocalName == this.id48_Aliases && base.Reader.NamespaceURI == this.id4_Item)
					{
						string value = base.Reader.Value;
						string[] array3 = value.Split(null);
						for (int i = 0; i < array3.Length; i++)
						{
							array = (string[])base.EnsureArrayIndex(array, num, typeof(string));
							array[num++] = array3[i];
						}
					}
					else if (!array2[4] && base.Reader.LocalName == this.id23_ConfirmImpact && base.Reader.NamespaceURI == this.id4_Item)
					{
						commonCmdletMetadata.ConfirmImpact = this.Read20_ConfirmImpact(base.Reader.Value);
						commonCmdletMetadata.ConfirmImpactSpecified = true;
						array2[4] = true;
					}
					else if (!array2[5] && base.Reader.LocalName == this.id98_HelpUri && base.Reader.NamespaceURI == this.id4_Item)
					{
						commonCmdletMetadata.HelpUri = base.CollapseWhitespace(base.Reader.Value);
						array2[5] = true;
					}
					else if (!base.IsXmlnsAttribute(base.Reader.Name))
					{
						base.UnknownNode(commonCmdletMetadata, ":Verb, :Noun, :Aliases, :ConfirmImpact, :HelpUri");
					}
				}
				base.Reader.MoveToElement();
				if (base.Reader.IsEmptyElement)
				{
					base.Reader.Skip();
					commonCmdletMetadata.Aliases = (string[])base.ShrinkArray(array, num, typeof(string), true);
					return commonCmdletMetadata;
				}
				base.Reader.ReadStartElement();
				base.Reader.MoveToContent();
				int num2 = 0;
				int readerCount = base.ReaderCount;
				while (base.Reader.NodeType != XmlNodeType.EndElement && base.Reader.NodeType != XmlNodeType.None)
				{
					if (base.Reader.NodeType == XmlNodeType.Element)
					{
						if (!array2[0] && base.Reader.LocalName == this.id65_Obsolete && base.Reader.NamespaceURI == this.id2_Item)
						{
							commonCmdletMetadata.Obsolete = this.Read7_ObsoleteAttributeMetadata(false, true);
							array2[0] = true;
						}
						else
						{
							base.UnknownNode(commonCmdletMetadata, "http://schemas.microsoft.com/cmdlets-over-objects/2009/11:Obsolete");
						}
					}
					else
					{
						base.UnknownNode(commonCmdletMetadata, "http://schemas.microsoft.com/cmdlets-over-objects/2009/11:Obsolete");
					}
					base.Reader.MoveToContent();
					base.CheckReaderCount(ref num2, ref readerCount);
				}
				commonCmdletMetadata.Aliases = (string[])base.ShrinkArray(array, num, typeof(string), true);
				base.ReadEndElement();
				return commonCmdletMetadata;
			}
		}

		// Token: 0x06005CD4 RID: 23764 RVA: 0x001F9EAC File Offset: 0x001F80AC
		private GetCmdletMetadata Read22_GetCmdletMetadata(bool isNullable, bool checkType)
		{
			XmlQualifiedName xmlQualifiedName = checkType ? base.GetXsiType() : null;
			bool flag = false;
			if (isNullable)
			{
				flag = base.ReadNull();
			}
			if (checkType && !(xmlQualifiedName == null) && (xmlQualifiedName.Name != this.id21_GetCmdletMetadata || xmlQualifiedName.Namespace != this.id2_Item))
			{
				throw base.CreateUnknownTypeException(xmlQualifiedName);
			}
			if (flag)
			{
				return null;
			}
			GetCmdletMetadata getCmdletMetadata = new GetCmdletMetadata();
			bool[] array = new bool[2];
			while (base.Reader.MoveToNextAttribute())
			{
				if (!base.IsXmlnsAttribute(base.Reader.Name))
				{
					base.UnknownNode(getCmdletMetadata);
				}
			}
			base.Reader.MoveToElement();
			if (base.Reader.IsEmptyElement)
			{
				base.Reader.Skip();
				return getCmdletMetadata;
			}
			base.Reader.ReadStartElement();
			base.Reader.MoveToContent();
			int num = 0;
			int readerCount = base.ReaderCount;
			while (base.Reader.NodeType != XmlNodeType.EndElement && base.Reader.NodeType != XmlNodeType.None)
			{
				if (base.Reader.NodeType == XmlNodeType.Element)
				{
					if (!array[0] && base.Reader.LocalName == this.id84_CmdletMetadata && base.Reader.NamespaceURI == this.id2_Item)
					{
						getCmdletMetadata.CmdletMetadata = this.Read21_CommonCmdletMetadata(false, true);
						array[0] = true;
					}
					else if (!array[1] && base.Reader.LocalName == this.id6_GetCmdletParameters && base.Reader.NamespaceURI == this.id2_Item)
					{
						getCmdletMetadata.GetCmdletParameters = this.Read19_GetCmdletParameters(false, true);
						array[1] = true;
					}
					else
					{
						base.UnknownNode(getCmdletMetadata, "http://schemas.microsoft.com/cmdlets-over-objects/2009/11:CmdletMetadata, http://schemas.microsoft.com/cmdlets-over-objects/2009/11:GetCmdletParameters");
					}
				}
				else
				{
					base.UnknownNode(getCmdletMetadata, "http://schemas.microsoft.com/cmdlets-over-objects/2009/11:CmdletMetadata, http://schemas.microsoft.com/cmdlets-over-objects/2009/11:GetCmdletParameters");
				}
				base.Reader.MoveToContent();
				base.CheckReaderCount(ref num, ref readerCount);
			}
			base.ReadEndElement();
			return getCmdletMetadata;
		}

		// Token: 0x06005CD5 RID: 23765 RVA: 0x001FA070 File Offset: 0x001F8270
		private InstanceMethodMetadata Read30_InstanceMethodMetadata(bool isNullable, bool checkType)
		{
			XmlQualifiedName xmlQualifiedName = checkType ? base.GetXsiType() : null;
			bool flag = false;
			if (isNullable)
			{
				flag = base.ReadNull();
			}
			if (checkType && !(xmlQualifiedName == null) && (xmlQualifiedName.Name != this.id33_InstanceMethodMetadata || xmlQualifiedName.Namespace != this.id2_Item))
			{
				throw base.CreateUnknownTypeException(xmlQualifiedName);
			}
			if (flag)
			{
				return null;
			}
			InstanceMethodMetadata instanceMethodMetadata = new InstanceMethodMetadata();
			bool[] array = new bool[3];
			while (base.Reader.MoveToNextAttribute())
			{
				if (!array[1] && base.Reader.LocalName == this.id86_MethodName && base.Reader.NamespaceURI == this.id4_Item)
				{
					instanceMethodMetadata.MethodName = base.Reader.Value;
					array[1] = true;
				}
				else if (!base.IsXmlnsAttribute(base.Reader.Name))
				{
					base.UnknownNode(instanceMethodMetadata, ":MethodName");
				}
			}
			base.Reader.MoveToElement();
			if (base.Reader.IsEmptyElement)
			{
				base.Reader.Skip();
				return instanceMethodMetadata;
			}
			base.Reader.ReadStartElement();
			base.Reader.MoveToContent();
			int num = 0;
			int readerCount = base.ReaderCount;
			while (base.Reader.NodeType != XmlNodeType.EndElement && base.Reader.NodeType != XmlNodeType.None)
			{
				if (base.Reader.NodeType == XmlNodeType.Element)
				{
					if (!array[0] && base.Reader.LocalName == this.id88_ReturnValue && base.Reader.NamespaceURI == this.id2_Item)
					{
						instanceMethodMetadata.ReturnValue = this.Read24_Item(false, true);
						array[0] = true;
					}
					else if (base.Reader.LocalName == this.id89_Parameters && base.Reader.NamespaceURI == this.id2_Item)
					{
						if (!base.ReadNull())
						{
							InstanceMethodParameterMetadata[] array2 = null;
							int num2 = 0;
							if (base.Reader.IsEmptyElement)
							{
								base.Reader.Skip();
							}
							else
							{
								base.Reader.ReadStartElement();
								base.Reader.MoveToContent();
								int num3 = 0;
								int readerCount2 = base.ReaderCount;
								while (base.Reader.NodeType != XmlNodeType.EndElement && base.Reader.NodeType != XmlNodeType.None)
								{
									if (base.Reader.NodeType == XmlNodeType.Element)
									{
										if (base.Reader.LocalName == this.id76_Parameter && base.Reader.NamespaceURI == this.id2_Item)
										{
											array2 = (InstanceMethodParameterMetadata[])base.EnsureArrayIndex(array2, num2, typeof(InstanceMethodParameterMetadata));
											array2[num2++] = this.Read25_Item(false, true);
										}
										else
										{
											base.UnknownNode(null, "http://schemas.microsoft.com/cmdlets-over-objects/2009/11:Parameter");
										}
									}
									else
									{
										base.UnknownNode(null, "http://schemas.microsoft.com/cmdlets-over-objects/2009/11:Parameter");
									}
									base.Reader.MoveToContent();
									base.CheckReaderCount(ref num3, ref readerCount2);
								}
								base.ReadEndElement();
							}
							instanceMethodMetadata.Parameters = (InstanceMethodParameterMetadata[])base.ShrinkArray(array2, num2, typeof(InstanceMethodParameterMetadata), false);
						}
					}
					else
					{
						base.UnknownNode(instanceMethodMetadata, "http://schemas.microsoft.com/cmdlets-over-objects/2009/11:ReturnValue, http://schemas.microsoft.com/cmdlets-over-objects/2009/11:Parameters");
					}
				}
				else
				{
					base.UnknownNode(instanceMethodMetadata, "http://schemas.microsoft.com/cmdlets-over-objects/2009/11:ReturnValue, http://schemas.microsoft.com/cmdlets-over-objects/2009/11:Parameters");
				}
				base.Reader.MoveToContent();
				base.CheckReaderCount(ref num, ref readerCount);
			}
			base.ReadEndElement();
			return instanceMethodMetadata;
		}

		// Token: 0x06005CD6 RID: 23766 RVA: 0x001FA39C File Offset: 0x001F859C
		private CommonMethodMetadata Read29_CommonMethodMetadata(bool isNullable, bool checkType)
		{
			XmlQualifiedName xmlQualifiedName = checkType ? base.GetXsiType() : null;
			bool flag = false;
			if (isNullable)
			{
				flag = base.ReadNull();
			}
			if (checkType && !(xmlQualifiedName == null) && (xmlQualifiedName.Name != this.id26_CommonMethodMetadata || xmlQualifiedName.Namespace != this.id2_Item))
			{
				if (xmlQualifiedName.Name == this.id33_InstanceMethodMetadata && xmlQualifiedName.Namespace == this.id2_Item)
				{
					return this.Read30_InstanceMethodMetadata(isNullable, false);
				}
				if (xmlQualifiedName.Name == this.id27_StaticMethodMetadata && xmlQualifiedName.Namespace == this.id2_Item)
				{
					return this.Read28_StaticMethodMetadata(isNullable, false);
				}
				throw base.CreateUnknownTypeException(xmlQualifiedName);
			}
			else
			{
				if (flag)
				{
					return null;
				}
				CommonMethodMetadata commonMethodMetadata = new CommonMethodMetadata();
				bool[] array = new bool[2];
				while (base.Reader.MoveToNextAttribute())
				{
					if (!array[1] && base.Reader.LocalName == this.id86_MethodName && base.Reader.NamespaceURI == this.id4_Item)
					{
						commonMethodMetadata.MethodName = base.Reader.Value;
						array[1] = true;
					}
					else if (!base.IsXmlnsAttribute(base.Reader.Name))
					{
						base.UnknownNode(commonMethodMetadata, ":MethodName");
					}
				}
				base.Reader.MoveToElement();
				if (base.Reader.IsEmptyElement)
				{
					base.Reader.Skip();
					return commonMethodMetadata;
				}
				base.Reader.ReadStartElement();
				base.Reader.MoveToContent();
				int num = 0;
				int readerCount = base.ReaderCount;
				while (base.Reader.NodeType != XmlNodeType.EndElement && base.Reader.NodeType != XmlNodeType.None)
				{
					if (base.Reader.NodeType == XmlNodeType.Element)
					{
						if (!array[0] && base.Reader.LocalName == this.id88_ReturnValue && base.Reader.NamespaceURI == this.id2_Item)
						{
							commonMethodMetadata.ReturnValue = this.Read24_Item(false, true);
							array[0] = true;
						}
						else
						{
							base.UnknownNode(commonMethodMetadata, "http://schemas.microsoft.com/cmdlets-over-objects/2009/11:ReturnValue");
						}
					}
					else
					{
						base.UnknownNode(commonMethodMetadata, "http://schemas.microsoft.com/cmdlets-over-objects/2009/11:ReturnValue");
					}
					base.Reader.MoveToContent();
					base.CheckReaderCount(ref num, ref readerCount);
				}
				base.ReadEndElement();
				return commonMethodMetadata;
			}
		}

		// Token: 0x06005CD7 RID: 23767 RVA: 0x001FA5AC File Offset: 0x001F87AC
		private CommonMethodParameterMetadata Read26_CommonMethodParameterMetadata(bool isNullable, bool checkType)
		{
			XmlQualifiedName xmlQualifiedName = checkType ? base.GetXsiType() : null;
			bool flag = false;
			if (isNullable)
			{
				flag = base.ReadNull();
			}
			if (checkType && !(xmlQualifiedName == null) && (xmlQualifiedName.Name != this.id28_CommonMethodParameterMetadata || xmlQualifiedName.Namespace != this.id2_Item))
			{
				if (xmlQualifiedName.Name == this.id29_StaticMethodParameterMetadata && xmlQualifiedName.Namespace == this.id2_Item)
				{
					return this.Read27_StaticMethodParameterMetadata(isNullable, false);
				}
				if (xmlQualifiedName.Name == this.id31_Item && xmlQualifiedName.Namespace == this.id2_Item)
				{
					return this.Read25_Item(isNullable, false);
				}
				throw base.CreateUnknownTypeException(xmlQualifiedName);
			}
			else
			{
				if (flag)
				{
					return null;
				}
				CommonMethodParameterMetadata commonMethodParameterMetadata = new CommonMethodParameterMetadata();
				bool[] array = new bool[3];
				while (base.Reader.MoveToNextAttribute())
				{
					if (!array[1] && base.Reader.LocalName == this.id90_ParameterName && base.Reader.NamespaceURI == this.id4_Item)
					{
						commonMethodParameterMetadata.ParameterName = base.Reader.Value;
						array[1] = true;
					}
					else if (!array[2] && base.Reader.LocalName == this.id91_DefaultValue && base.Reader.NamespaceURI == this.id4_Item)
					{
						commonMethodParameterMetadata.DefaultValue = base.Reader.Value;
						array[2] = true;
					}
					else if (!base.IsXmlnsAttribute(base.Reader.Name))
					{
						base.UnknownNode(commonMethodParameterMetadata, ":ParameterName, :DefaultValue");
					}
				}
				base.Reader.MoveToElement();
				if (base.Reader.IsEmptyElement)
				{
					base.Reader.Skip();
					return commonMethodParameterMetadata;
				}
				base.Reader.ReadStartElement();
				base.Reader.MoveToContent();
				int num = 0;
				int readerCount = base.ReaderCount;
				while (base.Reader.NodeType != XmlNodeType.EndElement && base.Reader.NodeType != XmlNodeType.None)
				{
					if (base.Reader.NodeType == XmlNodeType.Element)
					{
						if (!array[0] && base.Reader.LocalName == this.id92_Type && base.Reader.NamespaceURI == this.id2_Item)
						{
							commonMethodParameterMetadata.Type = this.Read2_TypeMetadata(false, true);
							array[0] = true;
						}
						else
						{
							base.UnknownNode(commonMethodParameterMetadata, "http://schemas.microsoft.com/cmdlets-over-objects/2009/11:Type");
						}
					}
					else
					{
						base.UnknownNode(commonMethodParameterMetadata, "http://schemas.microsoft.com/cmdlets-over-objects/2009/11:Type");
					}
					base.Reader.MoveToContent();
					base.CheckReaderCount(ref num, ref readerCount);
				}
				base.ReadEndElement();
				return commonMethodParameterMetadata;
			}
		}

		// Token: 0x06005CD8 RID: 23768 RVA: 0x001FA804 File Offset: 0x001F8A04
		private InstanceCmdletMetadata Read31_InstanceCmdletMetadata(bool isNullable, bool checkType)
		{
			XmlQualifiedName xmlQualifiedName = checkType ? base.GetXsiType() : null;
			bool flag = false;
			if (isNullable)
			{
				flag = base.ReadNull();
			}
			if (checkType && !(xmlQualifiedName == null) && (xmlQualifiedName.Name != this.id34_InstanceCmdletMetadata || xmlQualifiedName.Namespace != this.id2_Item))
			{
				throw base.CreateUnknownTypeException(xmlQualifiedName);
			}
			if (flag)
			{
				return null;
			}
			InstanceCmdletMetadata instanceCmdletMetadata = new InstanceCmdletMetadata();
			bool[] array = new bool[3];
			while (base.Reader.MoveToNextAttribute())
			{
				if (!base.IsXmlnsAttribute(base.Reader.Name))
				{
					base.UnknownNode(instanceCmdletMetadata);
				}
			}
			base.Reader.MoveToElement();
			if (base.Reader.IsEmptyElement)
			{
				base.Reader.Skip();
				return instanceCmdletMetadata;
			}
			base.Reader.ReadStartElement();
			base.Reader.MoveToContent();
			int num = 0;
			int readerCount = base.ReaderCount;
			while (base.Reader.NodeType != XmlNodeType.EndElement && base.Reader.NodeType != XmlNodeType.None)
			{
				if (base.Reader.NodeType == XmlNodeType.Element)
				{
					if (!array[0] && base.Reader.LocalName == this.id84_CmdletMetadata && base.Reader.NamespaceURI == this.id2_Item)
					{
						instanceCmdletMetadata.CmdletMetadata = this.Read21_CommonCmdletMetadata(false, true);
						array[0] = true;
					}
					else if (!array[1] && base.Reader.LocalName == this.id85_Method && base.Reader.NamespaceURI == this.id2_Item)
					{
						instanceCmdletMetadata.Method = this.Read30_InstanceMethodMetadata(false, true);
						array[1] = true;
					}
					else if (!array[2] && base.Reader.LocalName == this.id6_GetCmdletParameters && base.Reader.NamespaceURI == this.id2_Item)
					{
						instanceCmdletMetadata.GetCmdletParameters = this.Read19_GetCmdletParameters(false, true);
						array[2] = true;
					}
					else
					{
						base.UnknownNode(instanceCmdletMetadata, "http://schemas.microsoft.com/cmdlets-over-objects/2009/11:CmdletMetadata, http://schemas.microsoft.com/cmdlets-over-objects/2009/11:Method, http://schemas.microsoft.com/cmdlets-over-objects/2009/11:GetCmdletParameters");
					}
				}
				else
				{
					base.UnknownNode(instanceCmdletMetadata, "http://schemas.microsoft.com/cmdlets-over-objects/2009/11:CmdletMetadata, http://schemas.microsoft.com/cmdlets-over-objects/2009/11:Method, http://schemas.microsoft.com/cmdlets-over-objects/2009/11:GetCmdletParameters");
				}
				base.Reader.MoveToContent();
				base.CheckReaderCount(ref num, ref readerCount);
			}
			base.ReadEndElement();
			return instanceCmdletMetadata;
		}

		// Token: 0x06005CD9 RID: 23769 RVA: 0x001FAA0C File Offset: 0x001F8C0C
		private ClassMetadata Read36_ClassMetadata(bool isNullable, bool checkType)
		{
			XmlQualifiedName xmlQualifiedName = checkType ? base.GetXsiType() : null;
			bool flag = false;
			if (isNullable)
			{
				flag = base.ReadNull();
			}
			if (checkType && !(xmlQualifiedName == null) && (xmlQualifiedName.Name != this.id3_ClassMetadata || xmlQualifiedName.Namespace != this.id2_Item))
			{
				throw base.CreateUnknownTypeException(xmlQualifiedName);
			}
			if (flag)
			{
				return null;
			}
			ClassMetadata classMetadata = new ClassMetadata();
			bool[] array = new bool[8];
			while (base.Reader.MoveToNextAttribute())
			{
				if (!array[5] && base.Reader.LocalName == this.id112_CmdletAdapter && base.Reader.NamespaceURI == this.id4_Item)
				{
					classMetadata.CmdletAdapter = base.Reader.Value;
					array[5] = true;
				}
				else if (!array[6] && base.Reader.LocalName == this.id113_ClassName && base.Reader.NamespaceURI == this.id4_Item)
				{
					classMetadata.ClassName = base.Reader.Value;
					array[6] = true;
				}
				else if (!array[7] && base.Reader.LocalName == this.id114_ClassVersion && base.Reader.NamespaceURI == this.id4_Item)
				{
					classMetadata.ClassVersion = base.Reader.Value;
					array[7] = true;
				}
				else if (!base.IsXmlnsAttribute(base.Reader.Name))
				{
					base.UnknownNode(classMetadata, ":CmdletAdapter, :ClassName, :ClassVersion");
				}
			}
			base.Reader.MoveToElement();
			if (base.Reader.IsEmptyElement)
			{
				base.Reader.Skip();
				return classMetadata;
			}
			base.Reader.ReadStartElement();
			base.Reader.MoveToContent();
			int num = 0;
			int readerCount = base.ReaderCount;
			while (base.Reader.NodeType != XmlNodeType.EndElement && base.Reader.NodeType != XmlNodeType.None)
			{
				if (base.Reader.NodeType == XmlNodeType.Element)
				{
					if (!array[0] && base.Reader.LocalName == this.id115_Version && base.Reader.NamespaceURI == this.id2_Item)
					{
						classMetadata.Version = base.Reader.ReadElementString();
						array[0] = true;
					}
					else if (!array[1] && base.Reader.LocalName == this.id116_DefaultNoun && base.Reader.NamespaceURI == this.id2_Item)
					{
						classMetadata.DefaultNoun = base.Reader.ReadElementString();
						array[1] = true;
					}
					else if (!array[2] && base.Reader.LocalName == this.id117_InstanceCmdlets && base.Reader.NamespaceURI == this.id2_Item)
					{
						classMetadata.InstanceCmdlets = this.Read32_ClassMetadataInstanceCmdlets(false, true);
						array[2] = true;
					}
					else if (base.Reader.LocalName == this.id118_StaticCmdlets && base.Reader.NamespaceURI == this.id2_Item)
					{
						if (!base.ReadNull())
						{
							StaticCmdletMetadata[] array2 = null;
							int num2 = 0;
							if (base.Reader.IsEmptyElement)
							{
								base.Reader.Skip();
							}
							else
							{
								base.Reader.ReadStartElement();
								base.Reader.MoveToContent();
								int num3 = 0;
								int readerCount2 = base.ReaderCount;
								while (base.Reader.NodeType != XmlNodeType.EndElement && base.Reader.NodeType != XmlNodeType.None)
								{
									if (base.Reader.NodeType == XmlNodeType.Element)
									{
										if (base.Reader.LocalName == this.id79_Cmdlet && base.Reader.NamespaceURI == this.id2_Item)
										{
											array2 = (StaticCmdletMetadata[])base.EnsureArrayIndex(array2, num2, typeof(StaticCmdletMetadata));
											array2[num2++] = this.Read34_StaticCmdletMetadata(false, true);
										}
										else
										{
											base.UnknownNode(null, "http://schemas.microsoft.com/cmdlets-over-objects/2009/11:Cmdlet");
										}
									}
									else
									{
										base.UnknownNode(null, "http://schemas.microsoft.com/cmdlets-over-objects/2009/11:Cmdlet");
									}
									base.Reader.MoveToContent();
									base.CheckReaderCount(ref num3, ref readerCount2);
								}
								base.ReadEndElement();
							}
							classMetadata.StaticCmdlets = (StaticCmdletMetadata[])base.ShrinkArray(array2, num2, typeof(StaticCmdletMetadata), false);
						}
					}
					else if (base.Reader.LocalName == this.id119_CmdletAdapterPrivateData && base.Reader.NamespaceURI == this.id2_Item)
					{
						if (!base.ReadNull())
						{
							ClassMetadataData[] array3 = null;
							int num4 = 0;
							if (base.Reader.IsEmptyElement)
							{
								base.Reader.Skip();
							}
							else
							{
								base.Reader.ReadStartElement();
								base.Reader.MoveToContent();
								int num5 = 0;
								int readerCount3 = base.ReaderCount;
								while (base.Reader.NodeType != XmlNodeType.EndElement && base.Reader.NodeType != XmlNodeType.None)
								{
									if (base.Reader.NodeType == XmlNodeType.Element)
									{
										if (base.Reader.LocalName == this.id81_Data && base.Reader.NamespaceURI == this.id2_Item)
										{
											array3 = (ClassMetadataData[])base.EnsureArrayIndex(array3, num4, typeof(ClassMetadataData));
											array3[num4++] = this.Read35_ClassMetadataData(false, true);
										}
										else
										{
											base.UnknownNode(null, "http://schemas.microsoft.com/cmdlets-over-objects/2009/11:Data");
										}
									}
									else
									{
										base.UnknownNode(null, "http://schemas.microsoft.com/cmdlets-over-objects/2009/11:Data");
									}
									base.Reader.MoveToContent();
									base.CheckReaderCount(ref num5, ref readerCount3);
								}
								base.ReadEndElement();
							}
							classMetadata.CmdletAdapterPrivateData = (ClassMetadataData[])base.ShrinkArray(array3, num4, typeof(ClassMetadataData), false);
						}
					}
					else
					{
						base.UnknownNode(classMetadata, "http://schemas.microsoft.com/cmdlets-over-objects/2009/11:Version, http://schemas.microsoft.com/cmdlets-over-objects/2009/11:DefaultNoun, http://schemas.microsoft.com/cmdlets-over-objects/2009/11:InstanceCmdlets, http://schemas.microsoft.com/cmdlets-over-objects/2009/11:StaticCmdlets, http://schemas.microsoft.com/cmdlets-over-objects/2009/11:CmdletAdapterPrivateData");
					}
				}
				else
				{
					base.UnknownNode(classMetadata, "http://schemas.microsoft.com/cmdlets-over-objects/2009/11:Version, http://schemas.microsoft.com/cmdlets-over-objects/2009/11:DefaultNoun, http://schemas.microsoft.com/cmdlets-over-objects/2009/11:InstanceCmdlets, http://schemas.microsoft.com/cmdlets-over-objects/2009/11:StaticCmdlets, http://schemas.microsoft.com/cmdlets-over-objects/2009/11:CmdletAdapterPrivateData");
				}
				base.Reader.MoveToContent();
				base.CheckReaderCount(ref num, ref readerCount);
			}
			base.ReadEndElement();
			return classMetadata;
		}

		// Token: 0x06005CDA RID: 23770 RVA: 0x001FAFAC File Offset: 0x001F91AC
		private ClassMetadataInstanceCmdlets Read32_ClassMetadataInstanceCmdlets(bool isNullable, bool checkType)
		{
			XmlQualifiedName xmlQualifiedName = checkType ? base.GetXsiType() : null;
			bool flag = false;
			if (isNullable)
			{
				flag = base.ReadNull();
			}
			if (checkType && !(xmlQualifiedName == null) && (xmlQualifiedName.Name != this.id4_Item || xmlQualifiedName.Namespace != this.id2_Item))
			{
				throw base.CreateUnknownTypeException(xmlQualifiedName);
			}
			if (flag)
			{
				return null;
			}
			ClassMetadataInstanceCmdlets classMetadataInstanceCmdlets = new ClassMetadataInstanceCmdlets();
			InstanceCmdletMetadata[] array = null;
			int num = 0;
			bool[] array2 = new bool[3];
			while (base.Reader.MoveToNextAttribute())
			{
				if (!base.IsXmlnsAttribute(base.Reader.Name))
				{
					base.UnknownNode(classMetadataInstanceCmdlets);
				}
			}
			base.Reader.MoveToElement();
			if (base.Reader.IsEmptyElement)
			{
				base.Reader.Skip();
				classMetadataInstanceCmdlets.Cmdlet = (InstanceCmdletMetadata[])base.ShrinkArray(array, num, typeof(InstanceCmdletMetadata), true);
				return classMetadataInstanceCmdlets;
			}
			base.Reader.ReadStartElement();
			base.Reader.MoveToContent();
			int num2 = 0;
			int readerCount = base.ReaderCount;
			while (base.Reader.NodeType != XmlNodeType.EndElement && base.Reader.NodeType != XmlNodeType.None)
			{
				if (base.Reader.NodeType == XmlNodeType.Element)
				{
					if (!array2[0] && base.Reader.LocalName == this.id6_GetCmdletParameters && base.Reader.NamespaceURI == this.id2_Item)
					{
						classMetadataInstanceCmdlets.GetCmdletParameters = this.Read19_GetCmdletParameters(false, true);
						array2[0] = true;
					}
					else if (!array2[1] && base.Reader.LocalName == this.id120_GetCmdlet && base.Reader.NamespaceURI == this.id2_Item)
					{
						classMetadataInstanceCmdlets.GetCmdlet = this.Read22_GetCmdletMetadata(false, true);
						array2[1] = true;
					}
					else if (base.Reader.LocalName == this.id79_Cmdlet && base.Reader.NamespaceURI == this.id2_Item)
					{
						array = (InstanceCmdletMetadata[])base.EnsureArrayIndex(array, num, typeof(InstanceCmdletMetadata));
						array[num++] = this.Read31_InstanceCmdletMetadata(false, true);
					}
					else
					{
						base.UnknownNode(classMetadataInstanceCmdlets, "http://schemas.microsoft.com/cmdlets-over-objects/2009/11:GetCmdletParameters, http://schemas.microsoft.com/cmdlets-over-objects/2009/11:GetCmdlet, http://schemas.microsoft.com/cmdlets-over-objects/2009/11:Cmdlet");
					}
				}
				else
				{
					base.UnknownNode(classMetadataInstanceCmdlets, "http://schemas.microsoft.com/cmdlets-over-objects/2009/11:GetCmdletParameters, http://schemas.microsoft.com/cmdlets-over-objects/2009/11:GetCmdlet, http://schemas.microsoft.com/cmdlets-over-objects/2009/11:Cmdlet");
				}
				base.Reader.MoveToContent();
				base.CheckReaderCount(ref num2, ref readerCount);
			}
			classMetadataInstanceCmdlets.Cmdlet = (InstanceCmdletMetadata[])base.ShrinkArray(array, num, typeof(InstanceCmdletMetadata), true);
			base.ReadEndElement();
			return classMetadataInstanceCmdlets;
		}

		// Token: 0x06005CDB RID: 23771 RVA: 0x001FB20C File Offset: 0x001F940C
		private ClassMetadataInstanceCmdlets Read40_ClassMetadataInstanceCmdlets(bool isNullable, bool checkType)
		{
			XmlQualifiedName xmlQualifiedName = checkType ? base.GetXsiType() : null;
			bool flag = false;
			if (isNullable)
			{
				flag = base.ReadNull();
			}
			if (checkType && !(xmlQualifiedName == null) && (xmlQualifiedName.Name != this.id5_ClassMetadataInstanceCmdlets || xmlQualifiedName.Namespace != this.id2_Item))
			{
				throw base.CreateUnknownTypeException(xmlQualifiedName);
			}
			if (flag)
			{
				return null;
			}
			ClassMetadataInstanceCmdlets classMetadataInstanceCmdlets = new ClassMetadataInstanceCmdlets();
			InstanceCmdletMetadata[] array = null;
			int num = 0;
			bool[] array2 = new bool[3];
			while (base.Reader.MoveToNextAttribute())
			{
				if (!base.IsXmlnsAttribute(base.Reader.Name))
				{
					base.UnknownNode(classMetadataInstanceCmdlets);
				}
			}
			base.Reader.MoveToElement();
			if (base.Reader.IsEmptyElement)
			{
				base.Reader.Skip();
				classMetadataInstanceCmdlets.Cmdlet = (InstanceCmdletMetadata[])base.ShrinkArray(array, num, typeof(InstanceCmdletMetadata), true);
				return classMetadataInstanceCmdlets;
			}
			base.Reader.ReadStartElement();
			base.Reader.MoveToContent();
			int num2 = 0;
			int readerCount = base.ReaderCount;
			while (base.Reader.NodeType != XmlNodeType.EndElement && base.Reader.NodeType != XmlNodeType.None)
			{
				if (base.Reader.NodeType == XmlNodeType.Element)
				{
					if (!array2[0] && base.Reader.LocalName == this.id6_GetCmdletParameters && base.Reader.NamespaceURI == this.id2_Item)
					{
						classMetadataInstanceCmdlets.GetCmdletParameters = this.Read19_GetCmdletParameters(false, true);
						array2[0] = true;
					}
					else if (!array2[1] && base.Reader.LocalName == this.id120_GetCmdlet && base.Reader.NamespaceURI == this.id2_Item)
					{
						classMetadataInstanceCmdlets.GetCmdlet = this.Read22_GetCmdletMetadata(false, true);
						array2[1] = true;
					}
					else if (base.Reader.LocalName == this.id79_Cmdlet && base.Reader.NamespaceURI == this.id2_Item)
					{
						array = (InstanceCmdletMetadata[])base.EnsureArrayIndex(array, num, typeof(InstanceCmdletMetadata));
						array[num++] = this.Read31_InstanceCmdletMetadata(false, true);
					}
					else
					{
						base.UnknownNode(classMetadataInstanceCmdlets, "http://schemas.microsoft.com/cmdlets-over-objects/2009/11:GetCmdletParameters, http://schemas.microsoft.com/cmdlets-over-objects/2009/11:GetCmdlet, http://schemas.microsoft.com/cmdlets-over-objects/2009/11:Cmdlet");
					}
				}
				else
				{
					base.UnknownNode(classMetadataInstanceCmdlets, "http://schemas.microsoft.com/cmdlets-over-objects/2009/11:GetCmdletParameters, http://schemas.microsoft.com/cmdlets-over-objects/2009/11:GetCmdlet, http://schemas.microsoft.com/cmdlets-over-objects/2009/11:Cmdlet");
				}
				base.Reader.MoveToContent();
				base.CheckReaderCount(ref num2, ref readerCount);
			}
			classMetadataInstanceCmdlets.Cmdlet = (InstanceCmdletMetadata[])base.ShrinkArray(array, num, typeof(InstanceCmdletMetadata), true);
			base.ReadEndElement();
			return classMetadataInstanceCmdlets;
		}

		// Token: 0x06005CDC RID: 23772 RVA: 0x001FB46C File Offset: 0x001F966C
		private AssociationAssociatedInstance Read41_AssociationAssociatedInstance(bool isNullable, bool checkType)
		{
			XmlQualifiedName xmlQualifiedName = checkType ? base.GetXsiType() : null;
			bool flag = false;
			if (isNullable)
			{
				flag = base.ReadNull();
			}
			if (checkType && !(xmlQualifiedName == null) && (xmlQualifiedName.Name != this.id10_AssociationAssociatedInstance || xmlQualifiedName.Namespace != this.id2_Item))
			{
				throw base.CreateUnknownTypeException(xmlQualifiedName);
			}
			if (flag)
			{
				return null;
			}
			AssociationAssociatedInstance associationAssociatedInstance = new AssociationAssociatedInstance();
			bool[] array = new bool[2];
			while (base.Reader.MoveToNextAttribute())
			{
				if (!base.IsXmlnsAttribute(base.Reader.Name))
				{
					base.UnknownNode(associationAssociatedInstance);
				}
			}
			base.Reader.MoveToElement();
			if (base.Reader.IsEmptyElement)
			{
				base.Reader.Skip();
				return associationAssociatedInstance;
			}
			base.Reader.ReadStartElement();
			base.Reader.MoveToContent();
			int num = 0;
			int readerCount = base.ReaderCount;
			while (base.Reader.NodeType != XmlNodeType.EndElement && base.Reader.NodeType != XmlNodeType.None)
			{
				if (base.Reader.NodeType == XmlNodeType.Element)
				{
					if (!array[0] && base.Reader.LocalName == this.id92_Type && base.Reader.NamespaceURI == this.id2_Item)
					{
						associationAssociatedInstance.Type = this.Read2_TypeMetadata(false, true);
						array[0] = true;
					}
					else if (!array[1] && base.Reader.LocalName == this.id11_CmdletParameterMetadata && base.Reader.NamespaceURI == this.id2_Item)
					{
						associationAssociatedInstance.CmdletParameterMetadata = this.Read12_Item(false, true);
						array[1] = true;
					}
					else
					{
						base.UnknownNode(associationAssociatedInstance, "http://schemas.microsoft.com/cmdlets-over-objects/2009/11:Type, http://schemas.microsoft.com/cmdlets-over-objects/2009/11:CmdletParameterMetadata");
					}
				}
				else
				{
					base.UnknownNode(associationAssociatedInstance, "http://schemas.microsoft.com/cmdlets-over-objects/2009/11:Type, http://schemas.microsoft.com/cmdlets-over-objects/2009/11:CmdletParameterMetadata");
				}
				base.Reader.MoveToContent();
				base.CheckReaderCount(ref num, ref readerCount);
			}
			base.ReadEndElement();
			return associationAssociatedInstance;
		}

		// Token: 0x06005CDD RID: 23773 RVA: 0x001FB630 File Offset: 0x001F9830
		private CmdletParameterMetadataValidateCount Read42_Item(bool isNullable, bool checkType)
		{
			XmlQualifiedName xmlQualifiedName = checkType ? base.GetXsiType() : null;
			bool flag = false;
			if (isNullable)
			{
				flag = base.ReadNull();
			}
			if (checkType && !(xmlQualifiedName == null) && (xmlQualifiedName.Name != this.id14_Item || xmlQualifiedName.Namespace != this.id2_Item))
			{
				throw base.CreateUnknownTypeException(xmlQualifiedName);
			}
			if (flag)
			{
				return null;
			}
			CmdletParameterMetadataValidateCount cmdletParameterMetadataValidateCount = new CmdletParameterMetadataValidateCount();
			bool[] array = new bool[2];
			while (base.Reader.MoveToNextAttribute())
			{
				if (!array[0] && base.Reader.LocalName == this.id67_Min && base.Reader.NamespaceURI == this.id4_Item)
				{
					cmdletParameterMetadataValidateCount.Min = base.CollapseWhitespace(base.Reader.Value);
					array[0] = true;
				}
				else if (!array[1] && base.Reader.LocalName == this.id68_Max && base.Reader.NamespaceURI == this.id4_Item)
				{
					cmdletParameterMetadataValidateCount.Max = base.CollapseWhitespace(base.Reader.Value);
					array[1] = true;
				}
				else if (!base.IsXmlnsAttribute(base.Reader.Name))
				{
					base.UnknownNode(cmdletParameterMetadataValidateCount, ":Min, :Max");
				}
			}
			base.Reader.MoveToElement();
			if (base.Reader.IsEmptyElement)
			{
				base.Reader.Skip();
				return cmdletParameterMetadataValidateCount;
			}
			base.Reader.ReadStartElement();
			base.Reader.MoveToContent();
			int num = 0;
			int readerCount = base.ReaderCount;
			while (base.Reader.NodeType != XmlNodeType.EndElement && base.Reader.NodeType != XmlNodeType.None)
			{
				if (base.Reader.NodeType == XmlNodeType.Element)
				{
					base.UnknownNode(cmdletParameterMetadataValidateCount, "");
				}
				else
				{
					base.UnknownNode(cmdletParameterMetadataValidateCount, "");
				}
				base.Reader.MoveToContent();
				base.CheckReaderCount(ref num, ref readerCount);
			}
			base.ReadEndElement();
			return cmdletParameterMetadataValidateCount;
		}

		// Token: 0x06005CDE RID: 23774 RVA: 0x001FB808 File Offset: 0x001F9A08
		private CmdletParameterMetadataValidateLength Read43_Item(bool isNullable, bool checkType)
		{
			XmlQualifiedName xmlQualifiedName = checkType ? base.GetXsiType() : null;
			bool flag = false;
			if (isNullable)
			{
				flag = base.ReadNull();
			}
			if (checkType && !(xmlQualifiedName == null) && (xmlQualifiedName.Name != this.id15_Item || xmlQualifiedName.Namespace != this.id2_Item))
			{
				throw base.CreateUnknownTypeException(xmlQualifiedName);
			}
			if (flag)
			{
				return null;
			}
			CmdletParameterMetadataValidateLength cmdletParameterMetadataValidateLength = new CmdletParameterMetadataValidateLength();
			bool[] array = new bool[2];
			while (base.Reader.MoveToNextAttribute())
			{
				if (!array[0] && base.Reader.LocalName == this.id67_Min && base.Reader.NamespaceURI == this.id4_Item)
				{
					cmdletParameterMetadataValidateLength.Min = base.CollapseWhitespace(base.Reader.Value);
					array[0] = true;
				}
				else if (!array[1] && base.Reader.LocalName == this.id68_Max && base.Reader.NamespaceURI == this.id4_Item)
				{
					cmdletParameterMetadataValidateLength.Max = base.CollapseWhitespace(base.Reader.Value);
					array[1] = true;
				}
				else if (!base.IsXmlnsAttribute(base.Reader.Name))
				{
					base.UnknownNode(cmdletParameterMetadataValidateLength, ":Min, :Max");
				}
			}
			base.Reader.MoveToElement();
			if (base.Reader.IsEmptyElement)
			{
				base.Reader.Skip();
				return cmdletParameterMetadataValidateLength;
			}
			base.Reader.ReadStartElement();
			base.Reader.MoveToContent();
			int num = 0;
			int readerCount = base.ReaderCount;
			while (base.Reader.NodeType != XmlNodeType.EndElement && base.Reader.NodeType != XmlNodeType.None)
			{
				if (base.Reader.NodeType == XmlNodeType.Element)
				{
					base.UnknownNode(cmdletParameterMetadataValidateLength, "");
				}
				else
				{
					base.UnknownNode(cmdletParameterMetadataValidateLength, "");
				}
				base.Reader.MoveToContent();
				base.CheckReaderCount(ref num, ref readerCount);
			}
			base.ReadEndElement();
			return cmdletParameterMetadataValidateLength;
		}

		// Token: 0x06005CDF RID: 23775 RVA: 0x001FB9E0 File Offset: 0x001F9BE0
		private CmdletParameterMetadataValidateRange Read44_Item(bool isNullable, bool checkType)
		{
			XmlQualifiedName xmlQualifiedName = checkType ? base.GetXsiType() : null;
			bool flag = false;
			if (isNullable)
			{
				flag = base.ReadNull();
			}
			if (checkType && !(xmlQualifiedName == null) && (xmlQualifiedName.Name != this.id16_Item || xmlQualifiedName.Namespace != this.id2_Item))
			{
				throw base.CreateUnknownTypeException(xmlQualifiedName);
			}
			if (flag)
			{
				return null;
			}
			CmdletParameterMetadataValidateRange cmdletParameterMetadataValidateRange = new CmdletParameterMetadataValidateRange();
			bool[] array = new bool[2];
			while (base.Reader.MoveToNextAttribute())
			{
				if (!array[0] && base.Reader.LocalName == this.id67_Min && base.Reader.NamespaceURI == this.id4_Item)
				{
					cmdletParameterMetadataValidateRange.Min = base.CollapseWhitespace(base.Reader.Value);
					array[0] = true;
				}
				else if (!array[1] && base.Reader.LocalName == this.id68_Max && base.Reader.NamespaceURI == this.id4_Item)
				{
					cmdletParameterMetadataValidateRange.Max = base.CollapseWhitespace(base.Reader.Value);
					array[1] = true;
				}
				else if (!base.IsXmlnsAttribute(base.Reader.Name))
				{
					base.UnknownNode(cmdletParameterMetadataValidateRange, ":Min, :Max");
				}
			}
			base.Reader.MoveToElement();
			if (base.Reader.IsEmptyElement)
			{
				base.Reader.Skip();
				return cmdletParameterMetadataValidateRange;
			}
			base.Reader.ReadStartElement();
			base.Reader.MoveToContent();
			int num = 0;
			int readerCount = base.ReaderCount;
			while (base.Reader.NodeType != XmlNodeType.EndElement && base.Reader.NodeType != XmlNodeType.None)
			{
				if (base.Reader.NodeType == XmlNodeType.Element)
				{
					base.UnknownNode(cmdletParameterMetadataValidateRange, "");
				}
				else
				{
					base.UnknownNode(cmdletParameterMetadataValidateRange, "");
				}
				base.Reader.MoveToContent();
				base.CheckReaderCount(ref num, ref readerCount);
			}
			base.ReadEndElement();
			return cmdletParameterMetadataValidateRange;
		}

		// Token: 0x06005CE0 RID: 23776 RVA: 0x001FBBB8 File Offset: 0x001F9DB8
		private CommonMethodMetadataReturnValue Read46_Item(bool isNullable, bool checkType)
		{
			XmlQualifiedName xmlQualifiedName = checkType ? base.GetXsiType() : null;
			bool flag = false;
			if (isNullable)
			{
				flag = base.ReadNull();
			}
			if (checkType && !(xmlQualifiedName == null) && (xmlQualifiedName.Name != this.id32_Item || xmlQualifiedName.Namespace != this.id2_Item))
			{
				throw base.CreateUnknownTypeException(xmlQualifiedName);
			}
			if (flag)
			{
				return null;
			}
			CommonMethodMetadataReturnValue commonMethodMetadataReturnValue = new CommonMethodMetadataReturnValue();
			bool[] array = new bool[2];
			while (base.Reader.MoveToNextAttribute())
			{
				if (!base.IsXmlnsAttribute(base.Reader.Name))
				{
					base.UnknownNode(commonMethodMetadataReturnValue);
				}
			}
			base.Reader.MoveToElement();
			if (base.Reader.IsEmptyElement)
			{
				base.Reader.Skip();
				return commonMethodMetadataReturnValue;
			}
			base.Reader.ReadStartElement();
			base.Reader.MoveToContent();
			int num = 0;
			int readerCount = base.ReaderCount;
			while (base.Reader.NodeType != XmlNodeType.EndElement && base.Reader.NodeType != XmlNodeType.None)
			{
				if (base.Reader.NodeType == XmlNodeType.Element)
				{
					if (!array[0] && base.Reader.LocalName == this.id92_Type && base.Reader.NamespaceURI == this.id2_Item)
					{
						commonMethodMetadataReturnValue.Type = this.Read2_TypeMetadata(false, true);
						array[0] = true;
					}
					else if (!array[1] && base.Reader.LocalName == this.id30_CmdletOutputMetadata && base.Reader.NamespaceURI == this.id2_Item)
					{
						commonMethodMetadataReturnValue.CmdletOutputMetadata = this.Read23_CmdletOutputMetadata(false, true);
						array[1] = true;
					}
					else
					{
						base.UnknownNode(commonMethodMetadataReturnValue, "http://schemas.microsoft.com/cmdlets-over-objects/2009/11:Type, http://schemas.microsoft.com/cmdlets-over-objects/2009/11:CmdletOutputMetadata");
					}
				}
				else
				{
					base.UnknownNode(commonMethodMetadataReturnValue, "http://schemas.microsoft.com/cmdlets-over-objects/2009/11:Type, http://schemas.microsoft.com/cmdlets-over-objects/2009/11:CmdletOutputMetadata");
				}
				base.Reader.MoveToContent();
				base.CheckReaderCount(ref num, ref readerCount);
			}
			base.ReadEndElement();
			return commonMethodMetadataReturnValue;
		}

		// Token: 0x06005CE1 RID: 23777 RVA: 0x001FBD7C File Offset: 0x001F9F7C
		private PowerShellMetadata Read39_PowerShellMetadata(bool isNullable, bool checkType)
		{
			XmlQualifiedName xmlQualifiedName = checkType ? base.GetXsiType() : null;
			bool flag = false;
			if (isNullable)
			{
				flag = base.ReadNull();
			}
			if (checkType && !(xmlQualifiedName == null) && (xmlQualifiedName.Name != this.id4_Item || xmlQualifiedName.Namespace != this.id2_Item))
			{
				throw base.CreateUnknownTypeException(xmlQualifiedName);
			}
			if (flag)
			{
				return null;
			}
			PowerShellMetadata powerShellMetadata = new PowerShellMetadata();
			bool[] array = new bool[2];
			while (base.Reader.MoveToNextAttribute())
			{
				if (!base.IsXmlnsAttribute(base.Reader.Name))
				{
					base.UnknownNode(powerShellMetadata);
				}
			}
			base.Reader.MoveToElement();
			if (base.Reader.IsEmptyElement)
			{
				base.Reader.Skip();
				return powerShellMetadata;
			}
			base.Reader.ReadStartElement();
			base.Reader.MoveToContent();
			int num = 0;
			int readerCount = base.ReaderCount;
			while (base.Reader.NodeType != XmlNodeType.EndElement && base.Reader.NodeType != XmlNodeType.None)
			{
				if (base.Reader.NodeType == XmlNodeType.Element)
				{
					if (!array[0] && base.Reader.LocalName == this.id121_Class && base.Reader.NamespaceURI == this.id2_Item)
					{
						powerShellMetadata.Class = this.Read36_ClassMetadata(false, true);
						array[0] = true;
					}
					else if (base.Reader.LocalName == this.id122_Enums && base.Reader.NamespaceURI == this.id2_Item)
					{
						if (!base.ReadNull())
						{
							EnumMetadataEnum[] array2 = null;
							int num2 = 0;
							if (base.Reader.IsEmptyElement)
							{
								base.Reader.Skip();
							}
							else
							{
								base.Reader.ReadStartElement();
								base.Reader.MoveToContent();
								int num3 = 0;
								int readerCount2 = base.ReaderCount;
								while (base.Reader.NodeType != XmlNodeType.EndElement && base.Reader.NodeType != XmlNodeType.None)
								{
									if (base.Reader.NodeType == XmlNodeType.Element)
									{
										if (base.Reader.LocalName == this.id83_Enum && base.Reader.NamespaceURI == this.id2_Item)
										{
											array2 = (EnumMetadataEnum[])base.EnsureArrayIndex(array2, num2, typeof(EnumMetadataEnum));
											array2[num2++] = this.Read38_EnumMetadataEnum(false, true);
										}
										else
										{
											base.UnknownNode(null, "http://schemas.microsoft.com/cmdlets-over-objects/2009/11:Enum");
										}
									}
									else
									{
										base.UnknownNode(null, "http://schemas.microsoft.com/cmdlets-over-objects/2009/11:Enum");
									}
									base.Reader.MoveToContent();
									base.CheckReaderCount(ref num3, ref readerCount2);
								}
								base.ReadEndElement();
							}
							powerShellMetadata.Enums = (EnumMetadataEnum[])base.ShrinkArray(array2, num2, typeof(EnumMetadataEnum), false);
						}
					}
					else
					{
						base.UnknownNode(powerShellMetadata, "http://schemas.microsoft.com/cmdlets-over-objects/2009/11:Class, http://schemas.microsoft.com/cmdlets-over-objects/2009/11:Enums");
					}
				}
				else
				{
					base.UnknownNode(powerShellMetadata, "http://schemas.microsoft.com/cmdlets-over-objects/2009/11:Class, http://schemas.microsoft.com/cmdlets-over-objects/2009/11:Enums");
				}
				base.Reader.MoveToContent();
				base.CheckReaderCount(ref num, ref readerCount);
			}
			base.ReadEndElement();
			return powerShellMetadata;
		}

		// Token: 0x06005CE2 RID: 23778 RVA: 0x001FC05E File Offset: 0x001FA25E
		protected override void InitCallbacks()
		{
		}

		// Token: 0x06005CE3 RID: 23779 RVA: 0x001FC060 File Offset: 0x001FA260
		protected override void InitIDs()
		{
			this.id72_ArrayOfAssociation = base.Reader.NameTable.Add("ArrayOfAssociation");
			this.id46_AllowGlobbing = base.Reader.NameTable.Add("AllowGlobbing");
			this.id6_GetCmdletParameters = base.Reader.NameTable.Add("GetCmdletParameters");
			this.id25_Item = base.Reader.NameTable.Add("StaticCmdletMetadataCmdletMetadata");
			this.id62_ValidateRange = base.Reader.NameTable.Add("ValidateRange");
			this.id118_StaticCmdlets = base.Reader.NameTable.Add("StaticCmdlets");
			this.id58_ValidateNotNull = base.Reader.NameTable.Add("ValidateNotNull");
			this.id17_ObsoleteAttributeMetadata = base.Reader.NameTable.Add("ObsoleteAttributeMetadata");
			this.id49_PSName = base.Reader.NameTable.Add("PSName");
			this.id116_DefaultNoun = base.Reader.NameTable.Add("DefaultNoun");
			this.id38_ClassMetadataData = base.Reader.NameTable.Add("ClassMetadataData");
			this.id114_ClassVersion = base.Reader.NameTable.Add("ClassVersion");
			this.id66_Message = base.Reader.NameTable.Add("Message");
			this.id65_Obsolete = base.Reader.NameTable.Add("Obsolete");
			this.id51_ValueFromPipeline = base.Reader.NameTable.Add("ValueFromPipeline");
			this.id108_MinValueQuery = base.Reader.NameTable.Add("MinValueQuery");
			this.id119_CmdletAdapterPrivateData = base.Reader.NameTable.Add("CmdletAdapterPrivateData");
			this.id21_GetCmdletMetadata = base.Reader.NameTable.Add("GetCmdletMetadata");
			this.id120_GetCmdlet = base.Reader.NameTable.Add("GetCmdlet");
			this.id67_Min = base.Reader.NameTable.Add("Min");
			this.id56_AllowEmptyString = base.Reader.NameTable.Add("AllowEmptyString");
			this.id30_CmdletOutputMetadata = base.Reader.NameTable.Add("CmdletOutputMetadata");
			this.id106_RegularQuery = base.Reader.NameTable.Add("RegularQuery");
			this.id74_Option = base.Reader.NameTable.Add("Option");
			this.id75_Item = base.Reader.NameTable.Add("ArrayOfStaticMethodParameterMetadata");
			this.id23_ConfirmImpact = base.Reader.NameTable.Add("ConfirmImpact");
			this.id117_InstanceCmdlets = base.Reader.NameTable.Add("InstanceCmdlets");
			this.id83_Enum = base.Reader.NameTable.Add("Enum");
			this.id40_EnumMetadataEnumValue = base.Reader.NameTable.Add("EnumMetadataEnumValue");
			this.id111_QueryOptions = base.Reader.NameTable.Add("QueryOptions");
			this.id34_InstanceCmdletMetadata = base.Reader.NameTable.Add("InstanceCmdletMetadata");
			this.id60_ValidateCount = base.Reader.NameTable.Add("ValidateCount");
			this.id45_BitwiseFlags = base.Reader.NameTable.Add("BitwiseFlags");
			this.id81_Data = base.Reader.NameTable.Add("Data");
			this.id31_Item = base.Reader.NameTable.Add("InstanceMethodParameterMetadata");
			this.id1_PowerShellMetadata = base.Reader.NameTable.Add("PowerShellMetadata");
			this.id98_HelpUri = base.Reader.NameTable.Add("HelpUri");
			this.id91_DefaultValue = base.Reader.NameTable.Add("DefaultValue");
			this.id4_Item = base.Reader.NameTable.Add("");
			this.id32_Item = base.Reader.NameTable.Add("CommonMethodMetadataReturnValue");
			this.id43_EnumName = base.Reader.NameTable.Add("EnumName");
			this.id122_Enums = base.Reader.NameTable.Add("Enums");
			this.id82_ArrayOfEnumMetadataEnum = base.Reader.NameTable.Add("ArrayOfEnumMetadataEnum");
			this.id14_Item = base.Reader.NameTable.Add("CmdletParameterMetadataValidateCount");
			this.id48_Aliases = base.Reader.NameTable.Add("Aliases");
			this.id115_Version = base.Reader.NameTable.Add("Version");
			this.id11_CmdletParameterMetadata = base.Reader.NameTable.Add("CmdletParameterMetadata");
			this.id70_ArrayOfPropertyMetadata = base.Reader.NameTable.Add("ArrayOfPropertyMetadata");
			this.id9_Association = base.Reader.NameTable.Add("Association");
			this.id102_ResultRole = base.Reader.NameTable.Add("ResultRole");
			this.id29_StaticMethodParameterMetadata = base.Reader.NameTable.Add("StaticMethodParameterMetadata");
			this.id97_Noun = base.Reader.NameTable.Add("Noun");
			this.id47_IsMandatory = base.Reader.NameTable.Add("IsMandatory");
			this.id35_PropertyQuery = base.Reader.NameTable.Add("PropertyQuery");
			this.id54_ErrorOnNoMatch = base.Reader.NameTable.Add("ErrorOnNoMatch");
			this.id3_ClassMetadata = base.Reader.NameTable.Add("ClassMetadata");
			this.id77_Item = base.Reader.NameTable.Add("ArrayOfInstanceMethodParameterMetadata");
			this.id2_Item = base.Reader.NameTable.Add("http://schemas.microsoft.com/cmdlets-over-objects/2009/11");
			this.id22_CommonCmdletMetadata = base.Reader.NameTable.Add("CommonCmdletMetadata");
			this.id37_ItemsChoiceType = base.Reader.NameTable.Add("ItemsChoiceType");
			this.id36_WildcardablePropertyQuery = base.Reader.NameTable.Add("WildcardablePropertyQuery");
			this.id113_ClassName = base.Reader.NameTable.Add("ClassName");
			this.id64_AllowedValue = base.Reader.NameTable.Add("AllowedValue");
			this.id52_Item = base.Reader.NameTable.Add("ValueFromPipelineByPropertyName");
			this.id55_AllowEmptyCollection = base.Reader.NameTable.Add("AllowEmptyCollection");
			this.id13_Item = base.Reader.NameTable.Add("CmdletParameterMetadataForGetCmdletFilteringParameter");
			this.id76_Parameter = base.Reader.NameTable.Add("Parameter");
			this.id19_Item = base.Reader.NameTable.Add("CmdletParameterMetadataForStaticMethodParameter");
			this.id105_MaxValueQuery = base.Reader.NameTable.Add("MaxValueQuery");
			this.id101_SourceRole = base.Reader.NameTable.Add("SourceRole");
			this.id5_ClassMetadataInstanceCmdlets = base.Reader.NameTable.Add("ClassMetadataInstanceCmdlets");
			this.id112_CmdletAdapter = base.Reader.NameTable.Add("CmdletAdapter");
			this.id10_AssociationAssociatedInstance = base.Reader.NameTable.Add("AssociationAssociatedInstance");
			this.id93_ErrorCode = base.Reader.NameTable.Add("ErrorCode");
			this.id41_Name = base.Reader.NameTable.Add("Name");
			this.id68_Max = base.Reader.NameTable.Add("Max");
			this.id50_Position = base.Reader.NameTable.Add("Position");
			this.id100_OptionName = base.Reader.NameTable.Add("OptionName");
			this.id84_CmdletMetadata = base.Reader.NameTable.Add("CmdletMetadata");
			this.id87_CmdletParameterSet = base.Reader.NameTable.Add("CmdletParameterSet");
			this.id104_PropertyName = base.Reader.NameTable.Add("PropertyName");
			this.id28_CommonMethodParameterMetadata = base.Reader.NameTable.Add("CommonMethodParameterMetadata");
			this.id107_ExcludeQuery = base.Reader.NameTable.Add("ExcludeQuery");
			this.id92_Type = base.Reader.NameTable.Add("Type");
			this.id33_InstanceMethodMetadata = base.Reader.NameTable.Add("InstanceMethodMetadata");
			this.id63_ValidateSet = base.Reader.NameTable.Add("ValidateSet");
			this.id53_CmdletParameterSets = base.Reader.NameTable.Add("CmdletParameterSets");
			this.id15_Item = base.Reader.NameTable.Add("CmdletParameterMetadataValidateLength");
			this.id109_QueryableProperties = base.Reader.NameTable.Add("QueryableProperties");
			this.id57_AllowNull = base.Reader.NameTable.Add("AllowNull");
			this.id80_ArrayOfClassMetadataData = base.Reader.NameTable.Add("ArrayOfClassMetadataData");
			this.id99_DefaultCmdletParameterSet = base.Reader.NameTable.Add("DefaultCmdletParameterSet");
			this.id20_QueryOption = base.Reader.NameTable.Add("QueryOption");
			this.id89_Parameters = base.Reader.NameTable.Add("Parameters");
			this.id90_ParameterName = base.Reader.NameTable.Add("ParameterName");
			this.id61_ValidateLength = base.Reader.NameTable.Add("ValidateLength");
			this.id78_ArrayOfStaticCmdletMetadata = base.Reader.NameTable.Add("ArrayOfStaticCmdletMetadata");
			this.id16_Item = base.Reader.NameTable.Add("CmdletParameterMetadataValidateRange");
			this.id39_EnumMetadataEnum = base.Reader.NameTable.Add("EnumMetadataEnum");
			this.id7_PropertyMetadata = base.Reader.NameTable.Add("PropertyMetadata");
			this.id110_QueryableAssociations = base.Reader.NameTable.Add("QueryableAssociations");
			this.id86_MethodName = base.Reader.NameTable.Add("MethodName");
			this.id8_TypeMetadata = base.Reader.NameTable.Add("TypeMetadata");
			this.id71_Property = base.Reader.NameTable.Add("Property");
			this.id27_StaticMethodMetadata = base.Reader.NameTable.Add("StaticMethodMetadata");
			this.id94_PSType = base.Reader.NameTable.Add("PSType");
			this.id44_UnderlyingType = base.Reader.NameTable.Add("UnderlyingType");
			this.id103_AssociatedInstance = base.Reader.NameTable.Add("AssociatedInstance");
			this.id79_Cmdlet = base.Reader.NameTable.Add("Cmdlet");
			this.id18_Item = base.Reader.NameTable.Add("CmdletParameterMetadataForInstanceMethodParameter");
			this.id85_Method = base.Reader.NameTable.Add("Method");
			this.id95_ETSType = base.Reader.NameTable.Add("ETSType");
			this.id26_CommonMethodMetadata = base.Reader.NameTable.Add("CommonMethodMetadata");
			this.id88_ReturnValue = base.Reader.NameTable.Add("ReturnValue");
			this.id69_ArrayOfString = base.Reader.NameTable.Add("ArrayOfString");
			this.id24_StaticCmdletMetadata = base.Reader.NameTable.Add("StaticCmdletMetadata");
			this.id59_ValidateNotNullOrEmpty = base.Reader.NameTable.Add("ValidateNotNullOrEmpty");
			this.id96_Verb = base.Reader.NameTable.Add("Verb");
			this.id121_Class = base.Reader.NameTable.Add("Class");
			this.id73_ArrayOfQueryOption = base.Reader.NameTable.Add("ArrayOfQueryOption");
			this.id12_Item = base.Reader.NameTable.Add("CmdletParameterMetadataForGetCmdletParameter");
			this.id42_Value = base.Reader.NameTable.Add("Value");
		}

		// Token: 0x04003150 RID: 12624
		private string id72_ArrayOfAssociation;

		// Token: 0x04003151 RID: 12625
		private string id46_AllowGlobbing;

		// Token: 0x04003152 RID: 12626
		private string id6_GetCmdletParameters;

		// Token: 0x04003153 RID: 12627
		private string id25_Item;

		// Token: 0x04003154 RID: 12628
		private string id62_ValidateRange;

		// Token: 0x04003155 RID: 12629
		private string id118_StaticCmdlets;

		// Token: 0x04003156 RID: 12630
		private string id58_ValidateNotNull;

		// Token: 0x04003157 RID: 12631
		private string id17_ObsoleteAttributeMetadata;

		// Token: 0x04003158 RID: 12632
		private string id49_PSName;

		// Token: 0x04003159 RID: 12633
		private string id116_DefaultNoun;

		// Token: 0x0400315A RID: 12634
		private string id38_ClassMetadataData;

		// Token: 0x0400315B RID: 12635
		private string id114_ClassVersion;

		// Token: 0x0400315C RID: 12636
		private string id66_Message;

		// Token: 0x0400315D RID: 12637
		private string id65_Obsolete;

		// Token: 0x0400315E RID: 12638
		private string id51_ValueFromPipeline;

		// Token: 0x0400315F RID: 12639
		private string id108_MinValueQuery;

		// Token: 0x04003160 RID: 12640
		private string id119_CmdletAdapterPrivateData;

		// Token: 0x04003161 RID: 12641
		private string id21_GetCmdletMetadata;

		// Token: 0x04003162 RID: 12642
		private string id120_GetCmdlet;

		// Token: 0x04003163 RID: 12643
		private string id67_Min;

		// Token: 0x04003164 RID: 12644
		private string id56_AllowEmptyString;

		// Token: 0x04003165 RID: 12645
		private string id30_CmdletOutputMetadata;

		// Token: 0x04003166 RID: 12646
		private string id106_RegularQuery;

		// Token: 0x04003167 RID: 12647
		private string id74_Option;

		// Token: 0x04003168 RID: 12648
		private string id75_Item;

		// Token: 0x04003169 RID: 12649
		private string id23_ConfirmImpact;

		// Token: 0x0400316A RID: 12650
		private string id117_InstanceCmdlets;

		// Token: 0x0400316B RID: 12651
		private string id83_Enum;

		// Token: 0x0400316C RID: 12652
		private string id40_EnumMetadataEnumValue;

		// Token: 0x0400316D RID: 12653
		private string id111_QueryOptions;

		// Token: 0x0400316E RID: 12654
		private string id34_InstanceCmdletMetadata;

		// Token: 0x0400316F RID: 12655
		private string id60_ValidateCount;

		// Token: 0x04003170 RID: 12656
		private string id45_BitwiseFlags;

		// Token: 0x04003171 RID: 12657
		private string id81_Data;

		// Token: 0x04003172 RID: 12658
		private string id31_Item;

		// Token: 0x04003173 RID: 12659
		private string id1_PowerShellMetadata;

		// Token: 0x04003174 RID: 12660
		private string id98_HelpUri;

		// Token: 0x04003175 RID: 12661
		private string id91_DefaultValue;

		// Token: 0x04003176 RID: 12662
		private string id4_Item;

		// Token: 0x04003177 RID: 12663
		private string id32_Item;

		// Token: 0x04003178 RID: 12664
		private string id43_EnumName;

		// Token: 0x04003179 RID: 12665
		private string id122_Enums;

		// Token: 0x0400317A RID: 12666
		private string id82_ArrayOfEnumMetadataEnum;

		// Token: 0x0400317B RID: 12667
		private string id14_Item;

		// Token: 0x0400317C RID: 12668
		private string id48_Aliases;

		// Token: 0x0400317D RID: 12669
		private string id115_Version;

		// Token: 0x0400317E RID: 12670
		private string id11_CmdletParameterMetadata;

		// Token: 0x0400317F RID: 12671
		private string id70_ArrayOfPropertyMetadata;

		// Token: 0x04003180 RID: 12672
		private string id9_Association;

		// Token: 0x04003181 RID: 12673
		private string id102_ResultRole;

		// Token: 0x04003182 RID: 12674
		private string id29_StaticMethodParameterMetadata;

		// Token: 0x04003183 RID: 12675
		private string id97_Noun;

		// Token: 0x04003184 RID: 12676
		private string id47_IsMandatory;

		// Token: 0x04003185 RID: 12677
		private string id35_PropertyQuery;

		// Token: 0x04003186 RID: 12678
		private string id54_ErrorOnNoMatch;

		// Token: 0x04003187 RID: 12679
		private string id3_ClassMetadata;

		// Token: 0x04003188 RID: 12680
		private string id77_Item;

		// Token: 0x04003189 RID: 12681
		private string id2_Item;

		// Token: 0x0400318A RID: 12682
		private string id22_CommonCmdletMetadata;

		// Token: 0x0400318B RID: 12683
		private string id37_ItemsChoiceType;

		// Token: 0x0400318C RID: 12684
		private string id36_WildcardablePropertyQuery;

		// Token: 0x0400318D RID: 12685
		private string id113_ClassName;

		// Token: 0x0400318E RID: 12686
		private string id64_AllowedValue;

		// Token: 0x0400318F RID: 12687
		private string id52_Item;

		// Token: 0x04003190 RID: 12688
		private string id55_AllowEmptyCollection;

		// Token: 0x04003191 RID: 12689
		private string id13_Item;

		// Token: 0x04003192 RID: 12690
		private string id76_Parameter;

		// Token: 0x04003193 RID: 12691
		private string id19_Item;

		// Token: 0x04003194 RID: 12692
		private string id105_MaxValueQuery;

		// Token: 0x04003195 RID: 12693
		private string id101_SourceRole;

		// Token: 0x04003196 RID: 12694
		private string id5_ClassMetadataInstanceCmdlets;

		// Token: 0x04003197 RID: 12695
		private string id112_CmdletAdapter;

		// Token: 0x04003198 RID: 12696
		private string id10_AssociationAssociatedInstance;

		// Token: 0x04003199 RID: 12697
		private string id93_ErrorCode;

		// Token: 0x0400319A RID: 12698
		private string id41_Name;

		// Token: 0x0400319B RID: 12699
		private string id68_Max;

		// Token: 0x0400319C RID: 12700
		private string id50_Position;

		// Token: 0x0400319D RID: 12701
		private string id100_OptionName;

		// Token: 0x0400319E RID: 12702
		private string id84_CmdletMetadata;

		// Token: 0x0400319F RID: 12703
		private string id87_CmdletParameterSet;

		// Token: 0x040031A0 RID: 12704
		private string id104_PropertyName;

		// Token: 0x040031A1 RID: 12705
		private string id28_CommonMethodParameterMetadata;

		// Token: 0x040031A2 RID: 12706
		private string id107_ExcludeQuery;

		// Token: 0x040031A3 RID: 12707
		private string id92_Type;

		// Token: 0x040031A4 RID: 12708
		private string id33_InstanceMethodMetadata;

		// Token: 0x040031A5 RID: 12709
		private string id63_ValidateSet;

		// Token: 0x040031A6 RID: 12710
		private string id53_CmdletParameterSets;

		// Token: 0x040031A7 RID: 12711
		private string id15_Item;

		// Token: 0x040031A8 RID: 12712
		private string id109_QueryableProperties;

		// Token: 0x040031A9 RID: 12713
		private string id57_AllowNull;

		// Token: 0x040031AA RID: 12714
		private string id80_ArrayOfClassMetadataData;

		// Token: 0x040031AB RID: 12715
		private string id99_DefaultCmdletParameterSet;

		// Token: 0x040031AC RID: 12716
		private string id20_QueryOption;

		// Token: 0x040031AD RID: 12717
		private string id89_Parameters;

		// Token: 0x040031AE RID: 12718
		private string id90_ParameterName;

		// Token: 0x040031AF RID: 12719
		private string id61_ValidateLength;

		// Token: 0x040031B0 RID: 12720
		private string id78_ArrayOfStaticCmdletMetadata;

		// Token: 0x040031B1 RID: 12721
		private string id16_Item;

		// Token: 0x040031B2 RID: 12722
		private string id39_EnumMetadataEnum;

		// Token: 0x040031B3 RID: 12723
		private string id7_PropertyMetadata;

		// Token: 0x040031B4 RID: 12724
		private string id110_QueryableAssociations;

		// Token: 0x040031B5 RID: 12725
		private string id86_MethodName;

		// Token: 0x040031B6 RID: 12726
		private string id8_TypeMetadata;

		// Token: 0x040031B7 RID: 12727
		private string id71_Property;

		// Token: 0x040031B8 RID: 12728
		private string id27_StaticMethodMetadata;

		// Token: 0x040031B9 RID: 12729
		private string id94_PSType;

		// Token: 0x040031BA RID: 12730
		private string id44_UnderlyingType;

		// Token: 0x040031BB RID: 12731
		private string id103_AssociatedInstance;

		// Token: 0x040031BC RID: 12732
		private string id79_Cmdlet;

		// Token: 0x040031BD RID: 12733
		private string id18_Item;

		// Token: 0x040031BE RID: 12734
		private string id85_Method;

		// Token: 0x040031BF RID: 12735
		private string id95_ETSType;

		// Token: 0x040031C0 RID: 12736
		private string id26_CommonMethodMetadata;

		// Token: 0x040031C1 RID: 12737
		private string id88_ReturnValue;

		// Token: 0x040031C2 RID: 12738
		private string id69_ArrayOfString;

		// Token: 0x040031C3 RID: 12739
		private string id24_StaticCmdletMetadata;

		// Token: 0x040031C4 RID: 12740
		private string id59_ValidateNotNullOrEmpty;

		// Token: 0x040031C5 RID: 12741
		private string id96_Verb;

		// Token: 0x040031C6 RID: 12742
		private string id121_Class;

		// Token: 0x040031C7 RID: 12743
		private string id73_ArrayOfQueryOption;

		// Token: 0x040031C8 RID: 12744
		private string id12_Item;

		// Token: 0x040031C9 RID: 12745
		private string id42_Value;
	}
}
