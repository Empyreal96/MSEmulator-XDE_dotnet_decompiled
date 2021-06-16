using System;
using System.CodeDom.Compiler;
using System.Globalization;
using System.Xml;
using System.Xml.Serialization;

namespace Microsoft.PowerShell.Cmdletization.Xml
{
	// Token: 0x020009CF RID: 2511
	[GeneratedCode("sgen", "4.0")]
	internal class XmlSerializationWriter1 : XmlSerializationWriter
	{
		// Token: 0x06005C32 RID: 23602 RVA: 0x001ECEFE File Offset: 0x001EB0FE
		public void Write50_PowerShellMetadata(object o)
		{
			base.WriteStartDocument();
			if (o == null)
			{
				base.WriteEmptyTag("PowerShellMetadata", "http://schemas.microsoft.com/cmdlets-over-objects/2009/11");
				return;
			}
			base.TopLevelElement();
			this.Write39_PowerShellMetadata("PowerShellMetadata", "http://schemas.microsoft.com/cmdlets-over-objects/2009/11", (PowerShellMetadata)o, false, false);
		}

		// Token: 0x06005C33 RID: 23603 RVA: 0x001ECF38 File Offset: 0x001EB138
		public void Write51_ClassMetadata(object o)
		{
			base.WriteStartDocument();
			if (o == null)
			{
				base.WriteNullTagLiteral("ClassMetadata", "");
				return;
			}
			base.TopLevelElement();
			this.Write36_ClassMetadata("ClassMetadata", "", (ClassMetadata)o, true, false);
		}

		// Token: 0x06005C34 RID: 23604 RVA: 0x001ECF72 File Offset: 0x001EB172
		public void Write52_ClassMetadataInstanceCmdlets(object o)
		{
			base.WriteStartDocument();
			if (o == null)
			{
				base.WriteNullTagLiteral("ClassMetadataInstanceCmdlets", "");
				return;
			}
			base.TopLevelElement();
			this.Write40_ClassMetadataInstanceCmdlets("ClassMetadataInstanceCmdlets", "", (ClassMetadataInstanceCmdlets)o, true, false);
		}

		// Token: 0x06005C35 RID: 23605 RVA: 0x001ECFAC File Offset: 0x001EB1AC
		public void Write53_GetCmdletParameters(object o)
		{
			base.WriteStartDocument();
			if (o == null)
			{
				base.WriteNullTagLiteral("GetCmdletParameters", "");
				return;
			}
			base.TopLevelElement();
			this.Write19_GetCmdletParameters("GetCmdletParameters", "", (GetCmdletParameters)o, true, false);
		}

		// Token: 0x06005C36 RID: 23606 RVA: 0x001ECFE6 File Offset: 0x001EB1E6
		public void Write54_PropertyMetadata(object o)
		{
			base.WriteStartDocument();
			if (o == null)
			{
				base.WriteNullTagLiteral("PropertyMetadata", "");
				return;
			}
			base.TopLevelElement();
			this.Write15_PropertyMetadata("PropertyMetadata", "", (PropertyMetadata)o, true, false);
		}

		// Token: 0x06005C37 RID: 23607 RVA: 0x001ED020 File Offset: 0x001EB220
		public void Write55_TypeMetadata(object o)
		{
			base.WriteStartDocument();
			if (o == null)
			{
				base.WriteNullTagLiteral("TypeMetadata", "");
				return;
			}
			base.TopLevelElement();
			this.Write2_TypeMetadata("TypeMetadata", "", (TypeMetadata)o, true, false);
		}

		// Token: 0x06005C38 RID: 23608 RVA: 0x001ED05A File Offset: 0x001EB25A
		public void Write56_Association(object o)
		{
			base.WriteStartDocument();
			if (o == null)
			{
				base.WriteNullTagLiteral("Association", "");
				return;
			}
			base.TopLevelElement();
			this.Write17_Association("Association", "", (Association)o, true, false);
		}

		// Token: 0x06005C39 RID: 23609 RVA: 0x001ED094 File Offset: 0x001EB294
		public void Write57_AssociationAssociatedInstance(object o)
		{
			base.WriteStartDocument();
			if (o == null)
			{
				base.WriteNullTagLiteral("AssociationAssociatedInstance", "");
				return;
			}
			base.TopLevelElement();
			this.Write41_AssociationAssociatedInstance("AssociationAssociatedInstance", "", (AssociationAssociatedInstance)o, true, false);
		}

		// Token: 0x06005C3A RID: 23610 RVA: 0x001ED0CE File Offset: 0x001EB2CE
		public void Write58_CmdletParameterMetadata(object o)
		{
			base.WriteStartDocument();
			if (o == null)
			{
				base.WriteNullTagLiteral("CmdletParameterMetadata", "");
				return;
			}
			base.TopLevelElement();
			this.Write10_CmdletParameterMetadata("CmdletParameterMetadata", "", (CmdletParameterMetadata)o, true, false);
		}

		// Token: 0x06005C3B RID: 23611 RVA: 0x001ED108 File Offset: 0x001EB308
		public void Write59_Item(object o)
		{
			base.WriteStartDocument();
			if (o == null)
			{
				base.WriteNullTagLiteral("CmdletParameterMetadataForGetCmdletParameter", "");
				return;
			}
			base.TopLevelElement();
			this.Write11_Item("CmdletParameterMetadataForGetCmdletParameter", "", (CmdletParameterMetadataForGetCmdletParameter)o, true, false);
		}

		// Token: 0x06005C3C RID: 23612 RVA: 0x001ED142 File Offset: 0x001EB342
		public void Write60_Item(object o)
		{
			base.WriteStartDocument();
			if (o == null)
			{
				base.WriteNullTagLiteral("CmdletParameterMetadataForGetCmdletFilteringParameter", "");
				return;
			}
			base.TopLevelElement();
			this.Write12_Item("CmdletParameterMetadataForGetCmdletFilteringParameter", "", (CmdletParameterMetadataForGetCmdletFilteringParameter)o, true, false);
		}

		// Token: 0x06005C3D RID: 23613 RVA: 0x001ED17C File Offset: 0x001EB37C
		public void Write61_Item(object o)
		{
			base.WriteStartDocument();
			if (o == null)
			{
				base.WriteNullTagLiteral("CmdletParameterMetadataValidateCount", "");
				return;
			}
			base.TopLevelElement();
			this.Write42_Item("CmdletParameterMetadataValidateCount", "", (CmdletParameterMetadataValidateCount)o, true, false);
		}

		// Token: 0x06005C3E RID: 23614 RVA: 0x001ED1B6 File Offset: 0x001EB3B6
		public void Write62_Item(object o)
		{
			base.WriteStartDocument();
			if (o == null)
			{
				base.WriteNullTagLiteral("CmdletParameterMetadataValidateLength", "");
				return;
			}
			base.TopLevelElement();
			this.Write43_Item("CmdletParameterMetadataValidateLength", "", (CmdletParameterMetadataValidateLength)o, true, false);
		}

		// Token: 0x06005C3F RID: 23615 RVA: 0x001ED1F0 File Offset: 0x001EB3F0
		public void Write63_Item(object o)
		{
			base.WriteStartDocument();
			if (o == null)
			{
				base.WriteNullTagLiteral("CmdletParameterMetadataValidateRange", "");
				return;
			}
			base.TopLevelElement();
			this.Write44_Item("CmdletParameterMetadataValidateRange", "", (CmdletParameterMetadataValidateRange)o, true, false);
		}

		// Token: 0x06005C40 RID: 23616 RVA: 0x001ED22A File Offset: 0x001EB42A
		public void Write64_ObsoleteAttributeMetadata(object o)
		{
			base.WriteStartDocument();
			if (o == null)
			{
				base.WriteNullTagLiteral("ObsoleteAttributeMetadata", "");
				return;
			}
			base.TopLevelElement();
			this.Write7_ObsoleteAttributeMetadata("ObsoleteAttributeMetadata", "", (ObsoleteAttributeMetadata)o, true, false);
		}

		// Token: 0x06005C41 RID: 23617 RVA: 0x001ED264 File Offset: 0x001EB464
		public void Write65_Item(object o)
		{
			base.WriteStartDocument();
			if (o == null)
			{
				base.WriteNullTagLiteral("CmdletParameterMetadataForInstanceMethodParameter", "");
				return;
			}
			base.TopLevelElement();
			this.Write9_Item("CmdletParameterMetadataForInstanceMethodParameter", "", (CmdletParameterMetadataForInstanceMethodParameter)o, true, false);
		}

		// Token: 0x06005C42 RID: 23618 RVA: 0x001ED29E File Offset: 0x001EB49E
		public void Write66_Item(object o)
		{
			base.WriteStartDocument();
			if (o == null)
			{
				base.WriteNullTagLiteral("CmdletParameterMetadataForStaticMethodParameter", "");
				return;
			}
			base.TopLevelElement();
			this.Write8_Item("CmdletParameterMetadataForStaticMethodParameter", "", (CmdletParameterMetadataForStaticMethodParameter)o, true, false);
		}

		// Token: 0x06005C43 RID: 23619 RVA: 0x001ED2D8 File Offset: 0x001EB4D8
		public void Write67_QueryOption(object o)
		{
			base.WriteStartDocument();
			if (o == null)
			{
				base.WriteNullTagLiteral("QueryOption", "");
				return;
			}
			base.TopLevelElement();
			this.Write18_QueryOption("QueryOption", "", (QueryOption)o, true, false);
		}

		// Token: 0x06005C44 RID: 23620 RVA: 0x001ED312 File Offset: 0x001EB512
		public void Write68_GetCmdletMetadata(object o)
		{
			base.WriteStartDocument();
			if (o == null)
			{
				base.WriteNullTagLiteral("GetCmdletMetadata", "");
				return;
			}
			base.TopLevelElement();
			this.Write22_GetCmdletMetadata("GetCmdletMetadata", "", (GetCmdletMetadata)o, true, false);
		}

		// Token: 0x06005C45 RID: 23621 RVA: 0x001ED34C File Offset: 0x001EB54C
		public void Write69_CommonCmdletMetadata(object o)
		{
			base.WriteStartDocument();
			if (o == null)
			{
				base.WriteNullTagLiteral("CommonCmdletMetadata", "");
				return;
			}
			base.TopLevelElement();
			this.Write21_CommonCmdletMetadata("CommonCmdletMetadata", "", (CommonCmdletMetadata)o, true, false);
		}

		// Token: 0x06005C46 RID: 23622 RVA: 0x001ED386 File Offset: 0x001EB586
		public void Write70_ConfirmImpact(object o)
		{
			base.WriteStartDocument();
			if (o == null)
			{
				base.WriteEmptyTag("ConfirmImpact", "");
				return;
			}
			base.WriteElementString("ConfirmImpact", "", this.Write20_ConfirmImpact((ConfirmImpact)o));
		}

		// Token: 0x06005C47 RID: 23623 RVA: 0x001ED3BE File Offset: 0x001EB5BE
		public void Write71_StaticCmdletMetadata(object o)
		{
			base.WriteStartDocument();
			if (o == null)
			{
				base.WriteNullTagLiteral("StaticCmdletMetadata", "");
				return;
			}
			base.TopLevelElement();
			this.Write34_StaticCmdletMetadata("StaticCmdletMetadata", "", (StaticCmdletMetadata)o, true, false);
		}

		// Token: 0x06005C48 RID: 23624 RVA: 0x001ED3F8 File Offset: 0x001EB5F8
		public void Write72_Item(object o)
		{
			base.WriteStartDocument();
			if (o == null)
			{
				base.WriteNullTagLiteral("StaticCmdletMetadataCmdletMetadata", "");
				return;
			}
			base.TopLevelElement();
			this.Write45_Item("StaticCmdletMetadataCmdletMetadata", "", (StaticCmdletMetadataCmdletMetadata)o, true, false);
		}

		// Token: 0x06005C49 RID: 23625 RVA: 0x001ED432 File Offset: 0x001EB632
		public void Write73_CommonMethodMetadata(object o)
		{
			base.WriteStartDocument();
			if (o == null)
			{
				base.WriteNullTagLiteral("CommonMethodMetadata", "");
				return;
			}
			base.TopLevelElement();
			this.Write29_CommonMethodMetadata("CommonMethodMetadata", "", (CommonMethodMetadata)o, true, false);
		}

		// Token: 0x06005C4A RID: 23626 RVA: 0x001ED46C File Offset: 0x001EB66C
		public void Write74_StaticMethodMetadata(object o)
		{
			base.WriteStartDocument();
			if (o == null)
			{
				base.WriteNullTagLiteral("StaticMethodMetadata", "");
				return;
			}
			base.TopLevelElement();
			this.Write28_StaticMethodMetadata("StaticMethodMetadata", "", (StaticMethodMetadata)o, true, false);
		}

		// Token: 0x06005C4B RID: 23627 RVA: 0x001ED4A6 File Offset: 0x001EB6A6
		public void Write75_CommonMethodParameterMetadata(object o)
		{
			base.WriteStartDocument();
			if (o == null)
			{
				base.WriteNullTagLiteral("CommonMethodParameterMetadata", "");
				return;
			}
			base.TopLevelElement();
			this.Write26_CommonMethodParameterMetadata("CommonMethodParameterMetadata", "", (CommonMethodParameterMetadata)o, true, false);
		}

		// Token: 0x06005C4C RID: 23628 RVA: 0x001ED4E0 File Offset: 0x001EB6E0
		public void Write76_StaticMethodParameterMetadata(object o)
		{
			base.WriteStartDocument();
			if (o == null)
			{
				base.WriteNullTagLiteral("StaticMethodParameterMetadata", "");
				return;
			}
			base.TopLevelElement();
			this.Write27_StaticMethodParameterMetadata("StaticMethodParameterMetadata", "", (StaticMethodParameterMetadata)o, true, false);
		}

		// Token: 0x06005C4D RID: 23629 RVA: 0x001ED51A File Offset: 0x001EB71A
		public void Write77_CmdletOutputMetadata(object o)
		{
			base.WriteStartDocument();
			if (o == null)
			{
				base.WriteNullTagLiteral("CmdletOutputMetadata", "");
				return;
			}
			base.TopLevelElement();
			this.Write23_CmdletOutputMetadata("CmdletOutputMetadata", "", (CmdletOutputMetadata)o, true, false);
		}

		// Token: 0x06005C4E RID: 23630 RVA: 0x001ED554 File Offset: 0x001EB754
		public void Write78_Item(object o)
		{
			base.WriteStartDocument();
			if (o == null)
			{
				base.WriteNullTagLiteral("InstanceMethodParameterMetadata", "");
				return;
			}
			base.TopLevelElement();
			this.Write25_Item("InstanceMethodParameterMetadata", "", (InstanceMethodParameterMetadata)o, true, false);
		}

		// Token: 0x06005C4F RID: 23631 RVA: 0x001ED58E File Offset: 0x001EB78E
		public void Write79_Item(object o)
		{
			base.WriteStartDocument();
			if (o == null)
			{
				base.WriteNullTagLiteral("CommonMethodMetadataReturnValue", "");
				return;
			}
			base.TopLevelElement();
			this.Write46_Item("CommonMethodMetadataReturnValue", "", (CommonMethodMetadataReturnValue)o, true, false);
		}

		// Token: 0x06005C50 RID: 23632 RVA: 0x001ED5C8 File Offset: 0x001EB7C8
		public void Write80_InstanceMethodMetadata(object o)
		{
			base.WriteStartDocument();
			if (o == null)
			{
				base.WriteNullTagLiteral("InstanceMethodMetadata", "");
				return;
			}
			base.TopLevelElement();
			this.Write30_InstanceMethodMetadata("InstanceMethodMetadata", "", (InstanceMethodMetadata)o, true, false);
		}

		// Token: 0x06005C51 RID: 23633 RVA: 0x001ED602 File Offset: 0x001EB802
		public void Write81_InstanceCmdletMetadata(object o)
		{
			base.WriteStartDocument();
			if (o == null)
			{
				base.WriteNullTagLiteral("InstanceCmdletMetadata", "");
				return;
			}
			base.TopLevelElement();
			this.Write31_InstanceCmdletMetadata("InstanceCmdletMetadata", "", (InstanceCmdletMetadata)o, true, false);
		}

		// Token: 0x06005C52 RID: 23634 RVA: 0x001ED63C File Offset: 0x001EB83C
		public void Write82_PropertyQuery(object o)
		{
			base.WriteStartDocument();
			if (o == null)
			{
				base.WriteNullTagLiteral("PropertyQuery", "");
				return;
			}
			base.TopLevelElement();
			this.Write14_PropertyQuery("PropertyQuery", "", (PropertyQuery)o, true, false);
		}

		// Token: 0x06005C53 RID: 23635 RVA: 0x001ED676 File Offset: 0x001EB876
		public void Write83_WildcardablePropertyQuery(object o)
		{
			base.WriteStartDocument();
			if (o == null)
			{
				base.WriteNullTagLiteral("WildcardablePropertyQuery", "");
				return;
			}
			base.TopLevelElement();
			this.Write13_WildcardablePropertyQuery("WildcardablePropertyQuery", "", (WildcardablePropertyQuery)o, true, false);
		}

		// Token: 0x06005C54 RID: 23636 RVA: 0x001ED6B0 File Offset: 0x001EB8B0
		public void Write84_ItemsChoiceType(object o)
		{
			base.WriteStartDocument();
			if (o == null)
			{
				base.WriteEmptyTag("ItemsChoiceType", "");
				return;
			}
			base.WriteElementString("ItemsChoiceType", "", this.Write3_ItemsChoiceType((ItemsChoiceType)o));
		}

		// Token: 0x06005C55 RID: 23637 RVA: 0x001ED6E8 File Offset: 0x001EB8E8
		public void Write85_ClassMetadataData(object o)
		{
			base.WriteStartDocument();
			if (o == null)
			{
				base.WriteNullTagLiteral("ClassMetadataData", "");
				return;
			}
			base.TopLevelElement();
			this.Write47_ClassMetadataData("ClassMetadataData", "", (ClassMetadataData)o, true, false);
		}

		// Token: 0x06005C56 RID: 23638 RVA: 0x001ED722 File Offset: 0x001EB922
		public void Write86_EnumMetadataEnum(object o)
		{
			base.WriteStartDocument();
			if (o == null)
			{
				base.WriteNullTagLiteral("EnumMetadataEnum", "");
				return;
			}
			base.TopLevelElement();
			this.Write48_EnumMetadataEnum("EnumMetadataEnum", "", (EnumMetadataEnum)o, true, false);
		}

		// Token: 0x06005C57 RID: 23639 RVA: 0x001ED75C File Offset: 0x001EB95C
		public void Write87_EnumMetadataEnumValue(object o)
		{
			base.WriteStartDocument();
			if (o == null)
			{
				base.WriteNullTagLiteral("EnumMetadataEnumValue", "");
				return;
			}
			base.TopLevelElement();
			this.Write49_EnumMetadataEnumValue("EnumMetadataEnumValue", "", (EnumMetadataEnumValue)o, true, false);
		}

		// Token: 0x06005C58 RID: 23640 RVA: 0x001ED798 File Offset: 0x001EB998
		private void Write49_EnumMetadataEnumValue(string n, string ns, EnumMetadataEnumValue o, bool isNullable, bool needType)
		{
			if (o == null)
			{
				if (isNullable)
				{
					base.WriteNullTagLiteral(n, ns);
				}
				return;
			}
			if (!needType)
			{
				Type type = o.GetType();
				if (!(type == typeof(EnumMetadataEnumValue)))
				{
					throw base.CreateUnknownTypeException(o);
				}
			}
			base.WriteStartElement(n, ns, o, false, null);
			if (needType)
			{
				base.WriteXsiType("EnumMetadataEnumValue", "http://schemas.microsoft.com/cmdlets-over-objects/2009/11");
			}
			base.WriteAttribute("Name", "", o.Name);
			base.WriteAttribute("Value", "", o.Value);
			base.WriteEndElement(o);
		}

		// Token: 0x06005C59 RID: 23641 RVA: 0x001ED82C File Offset: 0x001EBA2C
		private void Write48_EnumMetadataEnum(string n, string ns, EnumMetadataEnum o, bool isNullable, bool needType)
		{
			if (o == null)
			{
				if (isNullable)
				{
					base.WriteNullTagLiteral(n, ns);
				}
				return;
			}
			if (!needType)
			{
				Type type = o.GetType();
				if (!(type == typeof(EnumMetadataEnum)))
				{
					throw base.CreateUnknownTypeException(o);
				}
			}
			base.WriteStartElement(n, ns, o, false, null);
			if (needType)
			{
				base.WriteXsiType("EnumMetadataEnum", "http://schemas.microsoft.com/cmdlets-over-objects/2009/11");
			}
			base.WriteAttribute("EnumName", "", o.EnumName);
			base.WriteAttribute("UnderlyingType", "", o.UnderlyingType);
			if (o.BitwiseFlagsSpecified)
			{
				base.WriteAttribute("BitwiseFlags", "", XmlConvert.ToString(o.BitwiseFlags));
			}
			EnumMetadataEnumValue[] value = o.Value;
			if (value != null)
			{
				for (int i = 0; i < value.Length; i++)
				{
					this.Write37_EnumMetadataEnumValue("Value", "http://schemas.microsoft.com/cmdlets-over-objects/2009/11", value[i], false, false);
				}
			}
			bool bitwiseFlagsSpecified = o.BitwiseFlagsSpecified;
			base.WriteEndElement(o);
		}

		// Token: 0x06005C5A RID: 23642 RVA: 0x001ED918 File Offset: 0x001EBB18
		private void Write37_EnumMetadataEnumValue(string n, string ns, EnumMetadataEnumValue o, bool isNullable, bool needType)
		{
			if (o == null)
			{
				if (isNullable)
				{
					base.WriteNullTagLiteral(n, ns);
				}
				return;
			}
			if (!needType)
			{
				Type type = o.GetType();
				if (!(type == typeof(EnumMetadataEnumValue)))
				{
					throw base.CreateUnknownTypeException(o);
				}
			}
			base.WriteStartElement(n, ns, o, false, null);
			if (needType)
			{
				base.WriteXsiType(null, "http://schemas.microsoft.com/cmdlets-over-objects/2009/11");
			}
			base.WriteAttribute("Name", "", o.Name);
			base.WriteAttribute("Value", "", o.Value);
			base.WriteEndElement(o);
		}

		// Token: 0x06005C5B RID: 23643 RVA: 0x001ED9A8 File Offset: 0x001EBBA8
		private void Write47_ClassMetadataData(string n, string ns, ClassMetadataData o, bool isNullable, bool needType)
		{
			if (o == null)
			{
				if (isNullable)
				{
					base.WriteNullTagLiteral(n, ns);
				}
				return;
			}
			if (!needType)
			{
				Type type = o.GetType();
				if (!(type == typeof(ClassMetadataData)))
				{
					throw base.CreateUnknownTypeException(o);
				}
			}
			base.WriteStartElement(n, ns, o, false, null);
			if (needType)
			{
				base.WriteXsiType("ClassMetadataData", "http://schemas.microsoft.com/cmdlets-over-objects/2009/11");
			}
			base.WriteAttribute("Name", "", o.Name);
			if (o.Value != null)
			{
				base.WriteValue(o.Value);
			}
			base.WriteEndElement(o);
		}

		// Token: 0x06005C5C RID: 23644 RVA: 0x001EDA3C File Offset: 0x001EBC3C
		private string Write3_ItemsChoiceType(ItemsChoiceType v)
		{
			string result;
			switch (v)
			{
			case ItemsChoiceType.ExcludeQuery:
				result = "ExcludeQuery";
				break;
			case ItemsChoiceType.MaxValueQuery:
				result = "MaxValueQuery";
				break;
			case ItemsChoiceType.MinValueQuery:
				result = "MinValueQuery";
				break;
			case ItemsChoiceType.RegularQuery:
				result = "RegularQuery";
				break;
			default:
				throw base.CreateInvalidEnumValueException(((long)v).ToString(CultureInfo.InvariantCulture), "Microsoft.PowerShell.Cmdletization.Xml.ItemsChoiceType");
			}
			return result;
		}

		// Token: 0x06005C5D RID: 23645 RVA: 0x001EDAA4 File Offset: 0x001EBCA4
		private void Write13_WildcardablePropertyQuery(string n, string ns, WildcardablePropertyQuery o, bool isNullable, bool needType)
		{
			if (o == null)
			{
				if (isNullable)
				{
					base.WriteNullTagLiteral(n, ns);
				}
				return;
			}
			if (!needType)
			{
				Type type = o.GetType();
				if (!(type == typeof(WildcardablePropertyQuery)))
				{
					throw base.CreateUnknownTypeException(o);
				}
			}
			base.WriteStartElement(n, ns, o, false, null);
			if (needType)
			{
				base.WriteXsiType("WildcardablePropertyQuery", "http://schemas.microsoft.com/cmdlets-over-objects/2009/11");
			}
			if (o.AllowGlobbingSpecified)
			{
				base.WriteAttribute("AllowGlobbing", "", XmlConvert.ToString(o.AllowGlobbing));
			}
			this.Write12_Item("CmdletParameterMetadata", "http://schemas.microsoft.com/cmdlets-over-objects/2009/11", o.CmdletParameterMetadata, false, false);
			bool allowGlobbingSpecified = o.AllowGlobbingSpecified;
			base.WriteEndElement(o);
		}

		// Token: 0x06005C5E RID: 23646 RVA: 0x001EDB50 File Offset: 0x001EBD50
		private void Write12_Item(string n, string ns, CmdletParameterMetadataForGetCmdletFilteringParameter o, bool isNullable, bool needType)
		{
			if (o == null)
			{
				if (isNullable)
				{
					base.WriteNullTagLiteral(n, ns);
				}
				return;
			}
			if (!needType)
			{
				Type type = o.GetType();
				if (!(type == typeof(CmdletParameterMetadataForGetCmdletFilteringParameter)))
				{
					throw base.CreateUnknownTypeException(o);
				}
			}
			base.WriteStartElement(n, ns, o, false, null);
			if (needType)
			{
				base.WriteXsiType("CmdletParameterMetadataForGetCmdletFilteringParameter", "http://schemas.microsoft.com/cmdlets-over-objects/2009/11");
			}
			if (o.IsMandatorySpecified)
			{
				base.WriteAttribute("IsMandatory", "", XmlConvert.ToString(o.IsMandatory));
			}
			string[] aliases = o.Aliases;
			if (aliases != null)
			{
				base.Writer.WriteStartAttribute(null, "Aliases", "");
				for (int i = 0; i < aliases.Length; i++)
				{
					string value = aliases[i];
					if (i != 0)
					{
						base.Writer.WriteString(" ");
					}
					base.WriteValue(value);
				}
				base.Writer.WriteEndAttribute();
			}
			base.WriteAttribute("PSName", "", o.PSName);
			base.WriteAttribute("Position", "", o.Position);
			if (o.ValueFromPipelineSpecified)
			{
				base.WriteAttribute("ValueFromPipeline", "", XmlConvert.ToString(o.ValueFromPipeline));
			}
			if (o.ValueFromPipelineByPropertyNameSpecified)
			{
				base.WriteAttribute("ValueFromPipelineByPropertyName", "", XmlConvert.ToString(o.ValueFromPipelineByPropertyName));
			}
			string[] cmdletParameterSets = o.CmdletParameterSets;
			if (cmdletParameterSets != null)
			{
				base.Writer.WriteStartAttribute(null, "CmdletParameterSets", "");
				for (int j = 0; j < cmdletParameterSets.Length; j++)
				{
					string value2 = cmdletParameterSets[j];
					if (j != 0)
					{
						base.Writer.WriteString(" ");
					}
					base.WriteValue(value2);
				}
				base.Writer.WriteEndAttribute();
			}
			if (o.ErrorOnNoMatchSpecified)
			{
				base.WriteAttribute("ErrorOnNoMatch", "", XmlConvert.ToString(o.ErrorOnNoMatch));
			}
			this.Write1_Object("AllowEmptyCollection", "http://schemas.microsoft.com/cmdlets-over-objects/2009/11", o.AllowEmptyCollection, false, false);
			this.Write1_Object("AllowEmptyString", "http://schemas.microsoft.com/cmdlets-over-objects/2009/11", o.AllowEmptyString, false, false);
			this.Write1_Object("AllowNull", "http://schemas.microsoft.com/cmdlets-over-objects/2009/11", o.AllowNull, false, false);
			this.Write1_Object("ValidateNotNull", "http://schemas.microsoft.com/cmdlets-over-objects/2009/11", o.ValidateNotNull, false, false);
			this.Write1_Object("ValidateNotNullOrEmpty", "http://schemas.microsoft.com/cmdlets-over-objects/2009/11", o.ValidateNotNullOrEmpty, false, false);
			this.Write4_Item("ValidateCount", "http://schemas.microsoft.com/cmdlets-over-objects/2009/11", o.ValidateCount, false, false);
			this.Write5_Item("ValidateLength", "http://schemas.microsoft.com/cmdlets-over-objects/2009/11", o.ValidateLength, false, false);
			this.Write6_Item("ValidateRange", "http://schemas.microsoft.com/cmdlets-over-objects/2009/11", o.ValidateRange, false, false);
			string[] validateSet = o.ValidateSet;
			if (validateSet != null)
			{
				base.WriteStartElement("ValidateSet", "http://schemas.microsoft.com/cmdlets-over-objects/2009/11", null, false);
				for (int k = 0; k < validateSet.Length; k++)
				{
					base.WriteElementString("AllowedValue", "http://schemas.microsoft.com/cmdlets-over-objects/2009/11", validateSet[k]);
				}
				base.WriteEndElement();
			}
			this.Write7_ObsoleteAttributeMetadata("Obsolete", "http://schemas.microsoft.com/cmdlets-over-objects/2009/11", o.Obsolete, false, false);
			bool isMandatorySpecified = o.IsMandatorySpecified;
			bool valueFromPipelineSpecified = o.ValueFromPipelineSpecified;
			bool valueFromPipelineByPropertyNameSpecified = o.ValueFromPipelineByPropertyNameSpecified;
			bool errorOnNoMatchSpecified = o.ErrorOnNoMatchSpecified;
			base.WriteEndElement(o);
		}

		// Token: 0x06005C5F RID: 23647 RVA: 0x001EDE6C File Offset: 0x001EC06C
		private void Write7_ObsoleteAttributeMetadata(string n, string ns, ObsoleteAttributeMetadata o, bool isNullable, bool needType)
		{
			if (o == null)
			{
				if (isNullable)
				{
					base.WriteNullTagLiteral(n, ns);
				}
				return;
			}
			if (!needType)
			{
				Type type = o.GetType();
				if (!(type == typeof(ObsoleteAttributeMetadata)))
				{
					throw base.CreateUnknownTypeException(o);
				}
			}
			base.WriteStartElement(n, ns, o, false, null);
			if (needType)
			{
				base.WriteXsiType("ObsoleteAttributeMetadata", "http://schemas.microsoft.com/cmdlets-over-objects/2009/11");
			}
			base.WriteAttribute("Message", "", o.Message);
			base.WriteEndElement(o);
		}

		// Token: 0x06005C60 RID: 23648 RVA: 0x001EDEEC File Offset: 0x001EC0EC
		private void Write6_Item(string n, string ns, CmdletParameterMetadataValidateRange o, bool isNullable, bool needType)
		{
			if (o == null)
			{
				if (isNullable)
				{
					base.WriteNullTagLiteral(n, ns);
				}
				return;
			}
			if (!needType)
			{
				Type type = o.GetType();
				if (!(type == typeof(CmdletParameterMetadataValidateRange)))
				{
					throw base.CreateUnknownTypeException(o);
				}
			}
			base.WriteStartElement(n, ns, o, false, null);
			if (needType)
			{
				base.WriteXsiType(null, "http://schemas.microsoft.com/cmdlets-over-objects/2009/11");
			}
			base.WriteAttribute("Min", "", o.Min);
			base.WriteAttribute("Max", "", o.Max);
			base.WriteEndElement(o);
		}

		// Token: 0x06005C61 RID: 23649 RVA: 0x001EDF7C File Offset: 0x001EC17C
		private void Write5_Item(string n, string ns, CmdletParameterMetadataValidateLength o, bool isNullable, bool needType)
		{
			if (o == null)
			{
				if (isNullable)
				{
					base.WriteNullTagLiteral(n, ns);
				}
				return;
			}
			if (!needType)
			{
				Type type = o.GetType();
				if (!(type == typeof(CmdletParameterMetadataValidateLength)))
				{
					throw base.CreateUnknownTypeException(o);
				}
			}
			base.WriteStartElement(n, ns, o, false, null);
			if (needType)
			{
				base.WriteXsiType(null, "http://schemas.microsoft.com/cmdlets-over-objects/2009/11");
			}
			base.WriteAttribute("Min", "", o.Min);
			base.WriteAttribute("Max", "", o.Max);
			base.WriteEndElement(o);
		}

		// Token: 0x06005C62 RID: 23650 RVA: 0x001EE00C File Offset: 0x001EC20C
		private void Write4_Item(string n, string ns, CmdletParameterMetadataValidateCount o, bool isNullable, bool needType)
		{
			if (o == null)
			{
				if (isNullable)
				{
					base.WriteNullTagLiteral(n, ns);
				}
				return;
			}
			if (!needType)
			{
				Type type = o.GetType();
				if (!(type == typeof(CmdletParameterMetadataValidateCount)))
				{
					throw base.CreateUnknownTypeException(o);
				}
			}
			base.WriteStartElement(n, ns, o, false, null);
			if (needType)
			{
				base.WriteXsiType(null, "http://schemas.microsoft.com/cmdlets-over-objects/2009/11");
			}
			base.WriteAttribute("Min", "", o.Min);
			base.WriteAttribute("Max", "", o.Max);
			base.WriteEndElement(o);
		}

		// Token: 0x06005C63 RID: 23651 RVA: 0x001EE09C File Offset: 0x001EC29C
		private void Write1_Object(string n, string ns, object o, bool isNullable, bool needType)
		{
			if (o == null)
			{
				if (isNullable)
				{
					base.WriteNullTagLiteral(n, ns);
				}
				return;
			}
			if (!needType)
			{
				Type type = o.GetType();
				if (!(type == typeof(object)))
				{
					if (type == typeof(EnumMetadataEnumValue))
					{
						this.Write49_EnumMetadataEnumValue(n, ns, (EnumMetadataEnumValue)o, isNullable, true);
						return;
					}
					if (type == typeof(EnumMetadataEnum))
					{
						this.Write48_EnumMetadataEnum(n, ns, (EnumMetadataEnum)o, isNullable, true);
						return;
					}
					if (type == typeof(ClassMetadataData))
					{
						this.Write47_ClassMetadataData(n, ns, (ClassMetadataData)o, isNullable, true);
						return;
					}
					if (type == typeof(CommonMethodMetadataReturnValue))
					{
						this.Write46_Item(n, ns, (CommonMethodMetadataReturnValue)o, isNullable, true);
						return;
					}
					if (type == typeof(CmdletParameterMetadataValidateRange))
					{
						this.Write44_Item(n, ns, (CmdletParameterMetadataValidateRange)o, isNullable, true);
						return;
					}
					if (type == typeof(CmdletParameterMetadataValidateLength))
					{
						this.Write43_Item(n, ns, (CmdletParameterMetadataValidateLength)o, isNullable, true);
						return;
					}
					if (type == typeof(CmdletParameterMetadataValidateCount))
					{
						this.Write42_Item(n, ns, (CmdletParameterMetadataValidateCount)o, isNullable, true);
						return;
					}
					if (type == typeof(AssociationAssociatedInstance))
					{
						this.Write41_AssociationAssociatedInstance(n, ns, (AssociationAssociatedInstance)o, isNullable, true);
						return;
					}
					if (type == typeof(ClassMetadataInstanceCmdlets))
					{
						this.Write40_ClassMetadataInstanceCmdlets(n, ns, (ClassMetadataInstanceCmdlets)o, isNullable, true);
						return;
					}
					if (type == typeof(ClassMetadata))
					{
						this.Write36_ClassMetadata(n, ns, (ClassMetadata)o, isNullable, true);
						return;
					}
					if (type == typeof(StaticCmdletMetadata))
					{
						this.Write34_StaticCmdletMetadata(n, ns, (StaticCmdletMetadata)o, isNullable, true);
						return;
					}
					if (type == typeof(InstanceCmdletMetadata))
					{
						this.Write31_InstanceCmdletMetadata(n, ns, (InstanceCmdletMetadata)o, isNullable, true);
						return;
					}
					if (type == typeof(CommonMethodParameterMetadata))
					{
						this.Write26_CommonMethodParameterMetadata(n, ns, (CommonMethodParameterMetadata)o, isNullable, true);
						return;
					}
					if (type == typeof(StaticMethodParameterMetadata))
					{
						this.Write27_StaticMethodParameterMetadata(n, ns, (StaticMethodParameterMetadata)o, isNullable, true);
						return;
					}
					if (type == typeof(InstanceMethodParameterMetadata))
					{
						this.Write25_Item(n, ns, (InstanceMethodParameterMetadata)o, isNullable, true);
						return;
					}
					if (type == typeof(CommonMethodMetadata))
					{
						this.Write29_CommonMethodMetadata(n, ns, (CommonMethodMetadata)o, isNullable, true);
						return;
					}
					if (type == typeof(InstanceMethodMetadata))
					{
						this.Write30_InstanceMethodMetadata(n, ns, (InstanceMethodMetadata)o, isNullable, true);
						return;
					}
					if (type == typeof(StaticMethodMetadata))
					{
						this.Write28_StaticMethodMetadata(n, ns, (StaticMethodMetadata)o, isNullable, true);
						return;
					}
					if (type == typeof(CmdletOutputMetadata))
					{
						this.Write23_CmdletOutputMetadata(n, ns, (CmdletOutputMetadata)o, isNullable, true);
						return;
					}
					if (type == typeof(GetCmdletMetadata))
					{
						this.Write22_GetCmdletMetadata(n, ns, (GetCmdletMetadata)o, isNullable, true);
						return;
					}
					if (type == typeof(CommonCmdletMetadata))
					{
						this.Write21_CommonCmdletMetadata(n, ns, (CommonCmdletMetadata)o, isNullable, true);
						return;
					}
					if (type == typeof(StaticCmdletMetadataCmdletMetadata))
					{
						this.Write45_Item(n, ns, (StaticCmdletMetadataCmdletMetadata)o, isNullable, true);
						return;
					}
					if (type == typeof(GetCmdletParameters))
					{
						this.Write19_GetCmdletParameters(n, ns, (GetCmdletParameters)o, isNullable, true);
						return;
					}
					if (type == typeof(QueryOption))
					{
						this.Write18_QueryOption(n, ns, (QueryOption)o, isNullable, true);
						return;
					}
					if (type == typeof(Association))
					{
						this.Write17_Association(n, ns, (Association)o, isNullable, true);
						return;
					}
					if (type == typeof(PropertyMetadata))
					{
						this.Write15_PropertyMetadata(n, ns, (PropertyMetadata)o, isNullable, true);
						return;
					}
					if (type == typeof(PropertyQuery))
					{
						this.Write14_PropertyQuery(n, ns, (PropertyQuery)o, isNullable, true);
						return;
					}
					if (type == typeof(WildcardablePropertyQuery))
					{
						this.Write13_WildcardablePropertyQuery(n, ns, (WildcardablePropertyQuery)o, isNullable, true);
						return;
					}
					if (type == typeof(CmdletParameterMetadata))
					{
						this.Write10_CmdletParameterMetadata(n, ns, (CmdletParameterMetadata)o, isNullable, true);
						return;
					}
					if (type == typeof(CmdletParameterMetadataForGetCmdletParameter))
					{
						this.Write11_Item(n, ns, (CmdletParameterMetadataForGetCmdletParameter)o, isNullable, true);
						return;
					}
					if (type == typeof(CmdletParameterMetadataForGetCmdletFilteringParameter))
					{
						this.Write12_Item(n, ns, (CmdletParameterMetadataForGetCmdletFilteringParameter)o, isNullable, true);
						return;
					}
					if (type == typeof(CmdletParameterMetadataForInstanceMethodParameter))
					{
						this.Write9_Item(n, ns, (CmdletParameterMetadataForInstanceMethodParameter)o, isNullable, true);
						return;
					}
					if (type == typeof(CmdletParameterMetadataForStaticMethodParameter))
					{
						this.Write8_Item(n, ns, (CmdletParameterMetadataForStaticMethodParameter)o, isNullable, true);
						return;
					}
					if (type == typeof(ObsoleteAttributeMetadata))
					{
						this.Write7_ObsoleteAttributeMetadata(n, ns, (ObsoleteAttributeMetadata)o, isNullable, true);
						return;
					}
					if (type == typeof(TypeMetadata))
					{
						this.Write2_TypeMetadata(n, ns, (TypeMetadata)o, isNullable, true);
						return;
					}
					if (type == typeof(ItemsChoiceType))
					{
						base.Writer.WriteStartElement(n, ns);
						base.WriteXsiType("ItemsChoiceType", "http://schemas.microsoft.com/cmdlets-over-objects/2009/11");
						base.Writer.WriteString(this.Write3_ItemsChoiceType((ItemsChoiceType)o));
						base.Writer.WriteEndElement();
						return;
					}
					if (type == typeof(string[]))
					{
						base.Writer.WriteStartElement(n, ns);
						base.WriteXsiType("ArrayOfString", "http://schemas.microsoft.com/cmdlets-over-objects/2009/11");
						string[] array = (string[])o;
						if (array != null)
						{
							for (int i = 0; i < array.Length; i++)
							{
								base.WriteElementString("AllowedValue", "http://schemas.microsoft.com/cmdlets-over-objects/2009/11", array[i]);
							}
						}
						base.Writer.WriteEndElement();
						return;
					}
					if (type == typeof(PropertyMetadata[]))
					{
						base.Writer.WriteStartElement(n, ns);
						base.WriteXsiType("ArrayOfPropertyMetadata", "http://schemas.microsoft.com/cmdlets-over-objects/2009/11");
						PropertyMetadata[] array2 = (PropertyMetadata[])o;
						if (array2 != null)
						{
							for (int j = 0; j < array2.Length; j++)
							{
								this.Write15_PropertyMetadata("Property", "http://schemas.microsoft.com/cmdlets-over-objects/2009/11", array2[j], false, false);
							}
						}
						base.Writer.WriteEndElement();
						return;
					}
					if (type == typeof(Association[]))
					{
						base.Writer.WriteStartElement(n, ns);
						base.WriteXsiType("ArrayOfAssociation", "http://schemas.microsoft.com/cmdlets-over-objects/2009/11");
						Association[] array3 = (Association[])o;
						if (array3 != null)
						{
							for (int k = 0; k < array3.Length; k++)
							{
								this.Write17_Association("Association", "http://schemas.microsoft.com/cmdlets-over-objects/2009/11", array3[k], false, false);
							}
						}
						base.Writer.WriteEndElement();
						return;
					}
					if (type == typeof(QueryOption[]))
					{
						base.Writer.WriteStartElement(n, ns);
						base.WriteXsiType("ArrayOfQueryOption", "http://schemas.microsoft.com/cmdlets-over-objects/2009/11");
						QueryOption[] array4 = (QueryOption[])o;
						if (array4 != null)
						{
							for (int l = 0; l < array4.Length; l++)
							{
								this.Write18_QueryOption("Option", "http://schemas.microsoft.com/cmdlets-over-objects/2009/11", array4[l], false, false);
							}
						}
						base.Writer.WriteEndElement();
						return;
					}
					if (type == typeof(ConfirmImpact))
					{
						base.Writer.WriteStartElement(n, ns);
						base.WriteXsiType("ConfirmImpact", "http://schemas.microsoft.com/cmdlets-over-objects/2009/11");
						base.Writer.WriteString(this.Write20_ConfirmImpact((ConfirmImpact)o));
						base.Writer.WriteEndElement();
						return;
					}
					if (type == typeof(StaticMethodParameterMetadata[]))
					{
						base.Writer.WriteStartElement(n, ns);
						base.WriteXsiType("ArrayOfStaticMethodParameterMetadata", "http://schemas.microsoft.com/cmdlets-over-objects/2009/11");
						StaticMethodParameterMetadata[] array5 = (StaticMethodParameterMetadata[])o;
						if (array5 != null)
						{
							for (int m = 0; m < array5.Length; m++)
							{
								this.Write27_StaticMethodParameterMetadata("Parameter", "http://schemas.microsoft.com/cmdlets-over-objects/2009/11", array5[m], false, false);
							}
						}
						base.Writer.WriteEndElement();
						return;
					}
					if (type == typeof(InstanceMethodParameterMetadata[]))
					{
						base.Writer.WriteStartElement(n, ns);
						base.WriteXsiType("ArrayOfInstanceMethodParameterMetadata", "http://schemas.microsoft.com/cmdlets-over-objects/2009/11");
						InstanceMethodParameterMetadata[] array6 = (InstanceMethodParameterMetadata[])o;
						if (array6 != null)
						{
							for (int num = 0; num < array6.Length; num++)
							{
								this.Write25_Item("Parameter", "http://schemas.microsoft.com/cmdlets-over-objects/2009/11", array6[num], false, false);
							}
						}
						base.Writer.WriteEndElement();
						return;
					}
					if (type == typeof(StaticCmdletMetadata[]))
					{
						base.Writer.WriteStartElement(n, ns);
						base.WriteXsiType("ArrayOfStaticCmdletMetadata", "http://schemas.microsoft.com/cmdlets-over-objects/2009/11");
						StaticCmdletMetadata[] array7 = (StaticCmdletMetadata[])o;
						if (array7 != null)
						{
							for (int num2 = 0; num2 < array7.Length; num2++)
							{
								this.Write34_StaticCmdletMetadata("Cmdlet", "http://schemas.microsoft.com/cmdlets-over-objects/2009/11", array7[num2], false, false);
							}
						}
						base.Writer.WriteEndElement();
						return;
					}
					if (type == typeof(ClassMetadataData[]))
					{
						base.Writer.WriteStartElement(n, ns);
						base.WriteXsiType("ArrayOfClassMetadataData", "http://schemas.microsoft.com/cmdlets-over-objects/2009/11");
						ClassMetadataData[] array8 = (ClassMetadataData[])o;
						if (array8 != null)
						{
							for (int num3 = 0; num3 < array8.Length; num3++)
							{
								this.Write35_ClassMetadataData("Data", "http://schemas.microsoft.com/cmdlets-over-objects/2009/11", array8[num3], false, false);
							}
						}
						base.Writer.WriteEndElement();
						return;
					}
					if (type == typeof(EnumMetadataEnum[]))
					{
						base.Writer.WriteStartElement(n, ns);
						base.WriteXsiType("ArrayOfEnumMetadataEnum", "http://schemas.microsoft.com/cmdlets-over-objects/2009/11");
						EnumMetadataEnum[] array9 = (EnumMetadataEnum[])o;
						if (array9 != null)
						{
							for (int num4 = 0; num4 < array9.Length; num4++)
							{
								this.Write38_EnumMetadataEnum("Enum", "http://schemas.microsoft.com/cmdlets-over-objects/2009/11", array9[num4], false, false);
							}
						}
						base.Writer.WriteEndElement();
						return;
					}
					base.WriteTypedPrimitive(n, ns, o, true);
					return;
				}
			}
			base.WriteStartElement(n, ns, o, false, null);
			base.WriteEndElement(o);
		}

		// Token: 0x06005C64 RID: 23652 RVA: 0x001EEA74 File Offset: 0x001ECC74
		private void Write38_EnumMetadataEnum(string n, string ns, EnumMetadataEnum o, bool isNullable, bool needType)
		{
			if (o == null)
			{
				if (isNullable)
				{
					base.WriteNullTagLiteral(n, ns);
				}
				return;
			}
			if (!needType)
			{
				Type type = o.GetType();
				if (!(type == typeof(EnumMetadataEnum)))
				{
					throw base.CreateUnknownTypeException(o);
				}
			}
			base.WriteStartElement(n, ns, o, false, null);
			if (needType)
			{
				base.WriteXsiType(null, "http://schemas.microsoft.com/cmdlets-over-objects/2009/11");
			}
			base.WriteAttribute("EnumName", "", o.EnumName);
			base.WriteAttribute("UnderlyingType", "", o.UnderlyingType);
			if (o.BitwiseFlagsSpecified)
			{
				base.WriteAttribute("BitwiseFlags", "", XmlConvert.ToString(o.BitwiseFlags));
			}
			EnumMetadataEnumValue[] value = o.Value;
			if (value != null)
			{
				for (int i = 0; i < value.Length; i++)
				{
					this.Write37_EnumMetadataEnumValue("Value", "http://schemas.microsoft.com/cmdlets-over-objects/2009/11", value[i], false, false);
				}
			}
			bool bitwiseFlagsSpecified = o.BitwiseFlagsSpecified;
			base.WriteEndElement(o);
		}

		// Token: 0x06005C65 RID: 23653 RVA: 0x001EEB5C File Offset: 0x001ECD5C
		private void Write35_ClassMetadataData(string n, string ns, ClassMetadataData o, bool isNullable, bool needType)
		{
			if (o == null)
			{
				if (isNullable)
				{
					base.WriteNullTagLiteral(n, ns);
				}
				return;
			}
			if (!needType)
			{
				Type type = o.GetType();
				if (!(type == typeof(ClassMetadataData)))
				{
					throw base.CreateUnknownTypeException(o);
				}
			}
			base.WriteStartElement(n, ns, o, false, null);
			if (needType)
			{
				base.WriteXsiType(null, "http://schemas.microsoft.com/cmdlets-over-objects/2009/11");
			}
			base.WriteAttribute("Name", "", o.Name);
			if (o.Value != null)
			{
				base.WriteValue(o.Value);
			}
			base.WriteEndElement(o);
		}

		// Token: 0x06005C66 RID: 23654 RVA: 0x001EEBEC File Offset: 0x001ECDEC
		private void Write34_StaticCmdletMetadata(string n, string ns, StaticCmdletMetadata o, bool isNullable, bool needType)
		{
			if (o == null)
			{
				if (isNullable)
				{
					base.WriteNullTagLiteral(n, ns);
				}
				return;
			}
			if (!needType)
			{
				Type type = o.GetType();
				if (!(type == typeof(StaticCmdletMetadata)))
				{
					throw base.CreateUnknownTypeException(o);
				}
			}
			base.WriteStartElement(n, ns, o, false, null);
			if (needType)
			{
				base.WriteXsiType("StaticCmdletMetadata", "http://schemas.microsoft.com/cmdlets-over-objects/2009/11");
			}
			this.Write33_Item("CmdletMetadata", "http://schemas.microsoft.com/cmdlets-over-objects/2009/11", o.CmdletMetadata, false, false);
			StaticMethodMetadata[] method = o.Method;
			if (method != null)
			{
				for (int i = 0; i < method.Length; i++)
				{
					this.Write28_StaticMethodMetadata("Method", "http://schemas.microsoft.com/cmdlets-over-objects/2009/11", method[i], false, false);
				}
			}
			base.WriteEndElement(o);
		}

		// Token: 0x06005C67 RID: 23655 RVA: 0x001EEC9C File Offset: 0x001ECE9C
		private void Write28_StaticMethodMetadata(string n, string ns, StaticMethodMetadata o, bool isNullable, bool needType)
		{
			if (o == null)
			{
				if (isNullable)
				{
					base.WriteNullTagLiteral(n, ns);
				}
				return;
			}
			if (!needType)
			{
				Type type = o.GetType();
				if (!(type == typeof(StaticMethodMetadata)))
				{
					throw base.CreateUnknownTypeException(o);
				}
			}
			base.WriteStartElement(n, ns, o, false, null);
			if (needType)
			{
				base.WriteXsiType("StaticMethodMetadata", "http://schemas.microsoft.com/cmdlets-over-objects/2009/11");
			}
			base.WriteAttribute("MethodName", "", o.MethodName);
			base.WriteAttribute("CmdletParameterSet", "", o.CmdletParameterSet);
			this.Write24_Item("ReturnValue", "http://schemas.microsoft.com/cmdlets-over-objects/2009/11", o.ReturnValue, false, false);
			StaticMethodParameterMetadata[] parameters = o.Parameters;
			if (parameters != null)
			{
				base.WriteStartElement("Parameters", "http://schemas.microsoft.com/cmdlets-over-objects/2009/11", null, false);
				for (int i = 0; i < parameters.Length; i++)
				{
					this.Write27_StaticMethodParameterMetadata("Parameter", "http://schemas.microsoft.com/cmdlets-over-objects/2009/11", parameters[i], false, false);
				}
				base.WriteEndElement();
			}
			base.WriteEndElement(o);
		}

		// Token: 0x06005C68 RID: 23656 RVA: 0x001EED90 File Offset: 0x001ECF90
		private void Write27_StaticMethodParameterMetadata(string n, string ns, StaticMethodParameterMetadata o, bool isNullable, bool needType)
		{
			if (o == null)
			{
				if (isNullable)
				{
					base.WriteNullTagLiteral(n, ns);
				}
				return;
			}
			if (!needType)
			{
				Type type = o.GetType();
				if (!(type == typeof(StaticMethodParameterMetadata)))
				{
					throw base.CreateUnknownTypeException(o);
				}
			}
			base.WriteStartElement(n, ns, o, false, null);
			if (needType)
			{
				base.WriteXsiType("StaticMethodParameterMetadata", "http://schemas.microsoft.com/cmdlets-over-objects/2009/11");
			}
			base.WriteAttribute("ParameterName", "", o.ParameterName);
			base.WriteAttribute("DefaultValue", "", o.DefaultValue);
			this.Write2_TypeMetadata("Type", "http://schemas.microsoft.com/cmdlets-over-objects/2009/11", o.Type, false, false);
			this.Write8_Item("CmdletParameterMetadata", "http://schemas.microsoft.com/cmdlets-over-objects/2009/11", o.CmdletParameterMetadata, false, false);
			this.Write23_CmdletOutputMetadata("CmdletOutputMetadata", "http://schemas.microsoft.com/cmdlets-over-objects/2009/11", o.CmdletOutputMetadata, false, false);
			base.WriteEndElement(o);
		}

		// Token: 0x06005C69 RID: 23657 RVA: 0x001EEE6C File Offset: 0x001ED06C
		private void Write23_CmdletOutputMetadata(string n, string ns, CmdletOutputMetadata o, bool isNullable, bool needType)
		{
			if (o == null)
			{
				if (isNullable)
				{
					base.WriteNullTagLiteral(n, ns);
				}
				return;
			}
			if (!needType)
			{
				Type type = o.GetType();
				if (!(type == typeof(CmdletOutputMetadata)))
				{
					throw base.CreateUnknownTypeException(o);
				}
			}
			base.WriteStartElement(n, ns, o, false, null);
			if (needType)
			{
				base.WriteXsiType("CmdletOutputMetadata", "http://schemas.microsoft.com/cmdlets-over-objects/2009/11");
			}
			base.WriteAttribute("PSName", "", o.PSName);
			this.Write1_Object("ErrorCode", "http://schemas.microsoft.com/cmdlets-over-objects/2009/11", o.ErrorCode, false, false);
			base.WriteEndElement(o);
		}

		// Token: 0x06005C6A RID: 23658 RVA: 0x001EEF04 File Offset: 0x001ED104
		private void Write8_Item(string n, string ns, CmdletParameterMetadataForStaticMethodParameter o, bool isNullable, bool needType)
		{
			if (o == null)
			{
				if (isNullable)
				{
					base.WriteNullTagLiteral(n, ns);
				}
				return;
			}
			if (!needType)
			{
				Type type = o.GetType();
				if (!(type == typeof(CmdletParameterMetadataForStaticMethodParameter)))
				{
					throw base.CreateUnknownTypeException(o);
				}
			}
			base.WriteStartElement(n, ns, o, false, null);
			if (needType)
			{
				base.WriteXsiType("CmdletParameterMetadataForStaticMethodParameter", "http://schemas.microsoft.com/cmdlets-over-objects/2009/11");
			}
			if (o.IsMandatorySpecified)
			{
				base.WriteAttribute("IsMandatory", "", XmlConvert.ToString(o.IsMandatory));
			}
			string[] aliases = o.Aliases;
			if (aliases != null)
			{
				base.Writer.WriteStartAttribute(null, "Aliases", "");
				for (int i = 0; i < aliases.Length; i++)
				{
					string value = aliases[i];
					if (i != 0)
					{
						base.Writer.WriteString(" ");
					}
					base.WriteValue(value);
				}
				base.Writer.WriteEndAttribute();
			}
			base.WriteAttribute("PSName", "", o.PSName);
			base.WriteAttribute("Position", "", o.Position);
			if (o.ValueFromPipelineSpecified)
			{
				base.WriteAttribute("ValueFromPipeline", "", XmlConvert.ToString(o.ValueFromPipeline));
			}
			if (o.ValueFromPipelineByPropertyNameSpecified)
			{
				base.WriteAttribute("ValueFromPipelineByPropertyName", "", XmlConvert.ToString(o.ValueFromPipelineByPropertyName));
			}
			this.Write1_Object("AllowEmptyCollection", "http://schemas.microsoft.com/cmdlets-over-objects/2009/11", o.AllowEmptyCollection, false, false);
			this.Write1_Object("AllowEmptyString", "http://schemas.microsoft.com/cmdlets-over-objects/2009/11", o.AllowEmptyString, false, false);
			this.Write1_Object("AllowNull", "http://schemas.microsoft.com/cmdlets-over-objects/2009/11", o.AllowNull, false, false);
			this.Write1_Object("ValidateNotNull", "http://schemas.microsoft.com/cmdlets-over-objects/2009/11", o.ValidateNotNull, false, false);
			this.Write1_Object("ValidateNotNullOrEmpty", "http://schemas.microsoft.com/cmdlets-over-objects/2009/11", o.ValidateNotNullOrEmpty, false, false);
			this.Write4_Item("ValidateCount", "http://schemas.microsoft.com/cmdlets-over-objects/2009/11", o.ValidateCount, false, false);
			this.Write5_Item("ValidateLength", "http://schemas.microsoft.com/cmdlets-over-objects/2009/11", o.ValidateLength, false, false);
			this.Write6_Item("ValidateRange", "http://schemas.microsoft.com/cmdlets-over-objects/2009/11", o.ValidateRange, false, false);
			string[] validateSet = o.ValidateSet;
			if (validateSet != null)
			{
				base.WriteStartElement("ValidateSet", "http://schemas.microsoft.com/cmdlets-over-objects/2009/11", null, false);
				for (int j = 0; j < validateSet.Length; j++)
				{
					base.WriteElementString("AllowedValue", "http://schemas.microsoft.com/cmdlets-over-objects/2009/11", validateSet[j]);
				}
				base.WriteEndElement();
			}
			this.Write7_ObsoleteAttributeMetadata("Obsolete", "http://schemas.microsoft.com/cmdlets-over-objects/2009/11", o.Obsolete, false, false);
			bool isMandatorySpecified = o.IsMandatorySpecified;
			bool valueFromPipelineSpecified = o.ValueFromPipelineSpecified;
			bool valueFromPipelineByPropertyNameSpecified = o.ValueFromPipelineByPropertyNameSpecified;
			base.WriteEndElement(o);
		}

		// Token: 0x06005C6B RID: 23659 RVA: 0x001EF194 File Offset: 0x001ED394
		private void Write2_TypeMetadata(string n, string ns, TypeMetadata o, bool isNullable, bool needType)
		{
			if (o == null)
			{
				if (isNullable)
				{
					base.WriteNullTagLiteral(n, ns);
				}
				return;
			}
			if (!needType)
			{
				Type type = o.GetType();
				if (!(type == typeof(TypeMetadata)))
				{
					throw base.CreateUnknownTypeException(o);
				}
			}
			base.WriteStartElement(n, ns, o, false, null);
			if (needType)
			{
				base.WriteXsiType("TypeMetadata", "http://schemas.microsoft.com/cmdlets-over-objects/2009/11");
			}
			base.WriteAttribute("PSType", "", o.PSType);
			base.WriteAttribute("ETSType", "", o.ETSType);
			base.WriteEndElement(o);
		}

		// Token: 0x06005C6C RID: 23660 RVA: 0x001EF228 File Offset: 0x001ED428
		private void Write24_Item(string n, string ns, CommonMethodMetadataReturnValue o, bool isNullable, bool needType)
		{
			if (o == null)
			{
				if (isNullable)
				{
					base.WriteNullTagLiteral(n, ns);
				}
				return;
			}
			if (!needType)
			{
				Type type = o.GetType();
				if (!(type == typeof(CommonMethodMetadataReturnValue)))
				{
					throw base.CreateUnknownTypeException(o);
				}
			}
			base.WriteStartElement(n, ns, o, false, null);
			if (needType)
			{
				base.WriteXsiType(null, "http://schemas.microsoft.com/cmdlets-over-objects/2009/11");
			}
			this.Write2_TypeMetadata("Type", "http://schemas.microsoft.com/cmdlets-over-objects/2009/11", o.Type, false, false);
			this.Write23_CmdletOutputMetadata("CmdletOutputMetadata", "http://schemas.microsoft.com/cmdlets-over-objects/2009/11", o.CmdletOutputMetadata, false, false);
			base.WriteEndElement(o);
		}

		// Token: 0x06005C6D RID: 23661 RVA: 0x001EF2BC File Offset: 0x001ED4BC
		private void Write33_Item(string n, string ns, StaticCmdletMetadataCmdletMetadata o, bool isNullable, bool needType)
		{
			if (o == null)
			{
				if (isNullable)
				{
					base.WriteNullTagLiteral(n, ns);
				}
				return;
			}
			if (!needType)
			{
				Type type = o.GetType();
				if (!(type == typeof(StaticCmdletMetadataCmdletMetadata)))
				{
					throw base.CreateUnknownTypeException(o);
				}
			}
			base.WriteStartElement(n, ns, o, false, null);
			if (needType)
			{
				base.WriteXsiType(null, "http://schemas.microsoft.com/cmdlets-over-objects/2009/11");
			}
			base.WriteAttribute("Verb", "", o.Verb);
			base.WriteAttribute("Noun", "", o.Noun);
			string[] aliases = o.Aliases;
			if (aliases != null)
			{
				base.Writer.WriteStartAttribute(null, "Aliases", "");
				for (int i = 0; i < aliases.Length; i++)
				{
					string value = aliases[i];
					if (i != 0)
					{
						base.Writer.WriteString(" ");
					}
					base.WriteValue(value);
				}
				base.Writer.WriteEndAttribute();
			}
			if (o.ConfirmImpactSpecified)
			{
				base.WriteAttribute("ConfirmImpact", "", this.Write20_ConfirmImpact(o.ConfirmImpact));
			}
			base.WriteAttribute("HelpUri", "", o.HelpUri);
			base.WriteAttribute("DefaultCmdletParameterSet", "", o.DefaultCmdletParameterSet);
			this.Write7_ObsoleteAttributeMetadata("Obsolete", "http://schemas.microsoft.com/cmdlets-over-objects/2009/11", o.Obsolete, false, false);
			bool confirmImpactSpecified = o.ConfirmImpactSpecified;
			base.WriteEndElement(o);
		}

		// Token: 0x06005C6E RID: 23662 RVA: 0x001EF414 File Offset: 0x001ED614
		private string Write20_ConfirmImpact(ConfirmImpact v)
		{
			string result;
			switch (v)
			{
			case ConfirmImpact.None:
				result = "None";
				break;
			case ConfirmImpact.Low:
				result = "Low";
				break;
			case ConfirmImpact.Medium:
				result = "Medium";
				break;
			case ConfirmImpact.High:
				result = "High";
				break;
			default:
				throw base.CreateInvalidEnumValueException(((long)v).ToString(CultureInfo.InvariantCulture), "Microsoft.PowerShell.Cmdletization.Xml.ConfirmImpact");
			}
			return result;
		}

		// Token: 0x06005C6F RID: 23663 RVA: 0x001EF47C File Offset: 0x001ED67C
		private void Write25_Item(string n, string ns, InstanceMethodParameterMetadata o, bool isNullable, bool needType)
		{
			if (o == null)
			{
				if (isNullable)
				{
					base.WriteNullTagLiteral(n, ns);
				}
				return;
			}
			if (!needType)
			{
				Type type = o.GetType();
				if (!(type == typeof(InstanceMethodParameterMetadata)))
				{
					throw base.CreateUnknownTypeException(o);
				}
			}
			base.WriteStartElement(n, ns, o, false, null);
			if (needType)
			{
				base.WriteXsiType("InstanceMethodParameterMetadata", "http://schemas.microsoft.com/cmdlets-over-objects/2009/11");
			}
			base.WriteAttribute("ParameterName", "", o.ParameterName);
			base.WriteAttribute("DefaultValue", "", o.DefaultValue);
			this.Write2_TypeMetadata("Type", "http://schemas.microsoft.com/cmdlets-over-objects/2009/11", o.Type, false, false);
			this.Write9_Item("CmdletParameterMetadata", "http://schemas.microsoft.com/cmdlets-over-objects/2009/11", o.CmdletParameterMetadata, false, false);
			this.Write23_CmdletOutputMetadata("CmdletOutputMetadata", "http://schemas.microsoft.com/cmdlets-over-objects/2009/11", o.CmdletOutputMetadata, false, false);
			base.WriteEndElement(o);
		}

		// Token: 0x06005C70 RID: 23664 RVA: 0x001EF558 File Offset: 0x001ED758
		private void Write9_Item(string n, string ns, CmdletParameterMetadataForInstanceMethodParameter o, bool isNullable, bool needType)
		{
			if (o == null)
			{
				if (isNullable)
				{
					base.WriteNullTagLiteral(n, ns);
				}
				return;
			}
			if (!needType)
			{
				Type type = o.GetType();
				if (!(type == typeof(CmdletParameterMetadataForInstanceMethodParameter)))
				{
					throw base.CreateUnknownTypeException(o);
				}
			}
			base.WriteStartElement(n, ns, o, false, null);
			if (needType)
			{
				base.WriteXsiType("CmdletParameterMetadataForInstanceMethodParameter", "http://schemas.microsoft.com/cmdlets-over-objects/2009/11");
			}
			if (o.IsMandatorySpecified)
			{
				base.WriteAttribute("IsMandatory", "", XmlConvert.ToString(o.IsMandatory));
			}
			string[] aliases = o.Aliases;
			if (aliases != null)
			{
				base.Writer.WriteStartAttribute(null, "Aliases", "");
				for (int i = 0; i < aliases.Length; i++)
				{
					string value = aliases[i];
					if (i != 0)
					{
						base.Writer.WriteString(" ");
					}
					base.WriteValue(value);
				}
				base.Writer.WriteEndAttribute();
			}
			base.WriteAttribute("PSName", "", o.PSName);
			base.WriteAttribute("Position", "", o.Position);
			if (o.ValueFromPipelineByPropertyNameSpecified)
			{
				base.WriteAttribute("ValueFromPipelineByPropertyName", "", XmlConvert.ToString(o.ValueFromPipelineByPropertyName));
			}
			this.Write1_Object("AllowEmptyCollection", "http://schemas.microsoft.com/cmdlets-over-objects/2009/11", o.AllowEmptyCollection, false, false);
			this.Write1_Object("AllowEmptyString", "http://schemas.microsoft.com/cmdlets-over-objects/2009/11", o.AllowEmptyString, false, false);
			this.Write1_Object("AllowNull", "http://schemas.microsoft.com/cmdlets-over-objects/2009/11", o.AllowNull, false, false);
			this.Write1_Object("ValidateNotNull", "http://schemas.microsoft.com/cmdlets-over-objects/2009/11", o.ValidateNotNull, false, false);
			this.Write1_Object("ValidateNotNullOrEmpty", "http://schemas.microsoft.com/cmdlets-over-objects/2009/11", o.ValidateNotNullOrEmpty, false, false);
			this.Write4_Item("ValidateCount", "http://schemas.microsoft.com/cmdlets-over-objects/2009/11", o.ValidateCount, false, false);
			this.Write5_Item("ValidateLength", "http://schemas.microsoft.com/cmdlets-over-objects/2009/11", o.ValidateLength, false, false);
			this.Write6_Item("ValidateRange", "http://schemas.microsoft.com/cmdlets-over-objects/2009/11", o.ValidateRange, false, false);
			string[] validateSet = o.ValidateSet;
			if (validateSet != null)
			{
				base.WriteStartElement("ValidateSet", "http://schemas.microsoft.com/cmdlets-over-objects/2009/11", null, false);
				for (int j = 0; j < validateSet.Length; j++)
				{
					base.WriteElementString("AllowedValue", "http://schemas.microsoft.com/cmdlets-over-objects/2009/11", validateSet[j]);
				}
				base.WriteEndElement();
			}
			this.Write7_ObsoleteAttributeMetadata("Obsolete", "http://schemas.microsoft.com/cmdlets-over-objects/2009/11", o.Obsolete, false, false);
			bool isMandatorySpecified = o.IsMandatorySpecified;
			bool valueFromPipelineByPropertyNameSpecified = o.ValueFromPipelineByPropertyNameSpecified;
			base.WriteEndElement(o);
		}

		// Token: 0x06005C71 RID: 23665 RVA: 0x001EF7BC File Offset: 0x001ED9BC
		private void Write18_QueryOption(string n, string ns, QueryOption o, bool isNullable, bool needType)
		{
			if (o == null)
			{
				if (isNullable)
				{
					base.WriteNullTagLiteral(n, ns);
				}
				return;
			}
			if (!needType)
			{
				Type type = o.GetType();
				if (!(type == typeof(QueryOption)))
				{
					throw base.CreateUnknownTypeException(o);
				}
			}
			base.WriteStartElement(n, ns, o, false, null);
			if (needType)
			{
				base.WriteXsiType("QueryOption", "http://schemas.microsoft.com/cmdlets-over-objects/2009/11");
			}
			base.WriteAttribute("OptionName", "", o.OptionName);
			this.Write2_TypeMetadata("Type", "http://schemas.microsoft.com/cmdlets-over-objects/2009/11", o.Type, false, false);
			this.Write11_Item("CmdletParameterMetadata", "http://schemas.microsoft.com/cmdlets-over-objects/2009/11", o.CmdletParameterMetadata, false, false);
			base.WriteEndElement(o);
		}

		// Token: 0x06005C72 RID: 23666 RVA: 0x001EF86C File Offset: 0x001EDA6C
		private void Write11_Item(string n, string ns, CmdletParameterMetadataForGetCmdletParameter o, bool isNullable, bool needType)
		{
			if (o == null)
			{
				if (isNullable)
				{
					base.WriteNullTagLiteral(n, ns);
				}
				return;
			}
			if (!needType)
			{
				Type type = o.GetType();
				if (!(type == typeof(CmdletParameterMetadataForGetCmdletParameter)))
				{
					if (type == typeof(CmdletParameterMetadataForGetCmdletFilteringParameter))
					{
						this.Write12_Item(n, ns, (CmdletParameterMetadataForGetCmdletFilteringParameter)o, isNullable, true);
						return;
					}
					throw base.CreateUnknownTypeException(o);
				}
			}
			base.WriteStartElement(n, ns, o, false, null);
			if (needType)
			{
				base.WriteXsiType("CmdletParameterMetadataForGetCmdletParameter", "http://schemas.microsoft.com/cmdlets-over-objects/2009/11");
			}
			if (o.IsMandatorySpecified)
			{
				base.WriteAttribute("IsMandatory", "", XmlConvert.ToString(o.IsMandatory));
			}
			string[] aliases = o.Aliases;
			if (aliases != null)
			{
				base.Writer.WriteStartAttribute(null, "Aliases", "");
				for (int i = 0; i < aliases.Length; i++)
				{
					string value = aliases[i];
					if (i != 0)
					{
						base.Writer.WriteString(" ");
					}
					base.WriteValue(value);
				}
				base.Writer.WriteEndAttribute();
			}
			base.WriteAttribute("PSName", "", o.PSName);
			base.WriteAttribute("Position", "", o.Position);
			if (o.ValueFromPipelineSpecified)
			{
				base.WriteAttribute("ValueFromPipeline", "", XmlConvert.ToString(o.ValueFromPipeline));
			}
			if (o.ValueFromPipelineByPropertyNameSpecified)
			{
				base.WriteAttribute("ValueFromPipelineByPropertyName", "", XmlConvert.ToString(o.ValueFromPipelineByPropertyName));
			}
			string[] cmdletParameterSets = o.CmdletParameterSets;
			if (cmdletParameterSets != null)
			{
				base.Writer.WriteStartAttribute(null, "CmdletParameterSets", "");
				for (int j = 0; j < cmdletParameterSets.Length; j++)
				{
					string value2 = cmdletParameterSets[j];
					if (j != 0)
					{
						base.Writer.WriteString(" ");
					}
					base.WriteValue(value2);
				}
				base.Writer.WriteEndAttribute();
			}
			this.Write1_Object("AllowEmptyCollection", "http://schemas.microsoft.com/cmdlets-over-objects/2009/11", o.AllowEmptyCollection, false, false);
			this.Write1_Object("AllowEmptyString", "http://schemas.microsoft.com/cmdlets-over-objects/2009/11", o.AllowEmptyString, false, false);
			this.Write1_Object("AllowNull", "http://schemas.microsoft.com/cmdlets-over-objects/2009/11", o.AllowNull, false, false);
			this.Write1_Object("ValidateNotNull", "http://schemas.microsoft.com/cmdlets-over-objects/2009/11", o.ValidateNotNull, false, false);
			this.Write1_Object("ValidateNotNullOrEmpty", "http://schemas.microsoft.com/cmdlets-over-objects/2009/11", o.ValidateNotNullOrEmpty, false, false);
			this.Write4_Item("ValidateCount", "http://schemas.microsoft.com/cmdlets-over-objects/2009/11", o.ValidateCount, false, false);
			this.Write5_Item("ValidateLength", "http://schemas.microsoft.com/cmdlets-over-objects/2009/11", o.ValidateLength, false, false);
			this.Write6_Item("ValidateRange", "http://schemas.microsoft.com/cmdlets-over-objects/2009/11", o.ValidateRange, false, false);
			string[] validateSet = o.ValidateSet;
			if (validateSet != null)
			{
				base.WriteStartElement("ValidateSet", "http://schemas.microsoft.com/cmdlets-over-objects/2009/11", null, false);
				for (int k = 0; k < validateSet.Length; k++)
				{
					base.WriteElementString("AllowedValue", "http://schemas.microsoft.com/cmdlets-over-objects/2009/11", validateSet[k]);
				}
				base.WriteEndElement();
			}
			this.Write7_ObsoleteAttributeMetadata("Obsolete", "http://schemas.microsoft.com/cmdlets-over-objects/2009/11", o.Obsolete, false, false);
			bool isMandatorySpecified = o.IsMandatorySpecified;
			bool valueFromPipelineSpecified = o.ValueFromPipelineSpecified;
			bool valueFromPipelineByPropertyNameSpecified = o.ValueFromPipelineByPropertyNameSpecified;
			base.WriteEndElement(o);
		}

		// Token: 0x06005C73 RID: 23667 RVA: 0x001EFB80 File Offset: 0x001EDD80
		private void Write17_Association(string n, string ns, Association o, bool isNullable, bool needType)
		{
			if (o == null)
			{
				if (isNullable)
				{
					base.WriteNullTagLiteral(n, ns);
				}
				return;
			}
			if (!needType)
			{
				Type type = o.GetType();
				if (!(type == typeof(Association)))
				{
					throw base.CreateUnknownTypeException(o);
				}
			}
			base.WriteStartElement(n, ns, o, false, null);
			if (needType)
			{
				base.WriteXsiType("Association", "http://schemas.microsoft.com/cmdlets-over-objects/2009/11");
			}
			base.WriteAttribute("Association", "", o.Association1);
			base.WriteAttribute("SourceRole", "", o.SourceRole);
			base.WriteAttribute("ResultRole", "", o.ResultRole);
			this.Write16_AssociationAssociatedInstance("AssociatedInstance", "http://schemas.microsoft.com/cmdlets-over-objects/2009/11", o.AssociatedInstance, false, false);
			base.WriteEndElement(o);
		}

		// Token: 0x06005C74 RID: 23668 RVA: 0x001EFC44 File Offset: 0x001EDE44
		private void Write16_AssociationAssociatedInstance(string n, string ns, AssociationAssociatedInstance o, bool isNullable, bool needType)
		{
			if (o == null)
			{
				if (isNullable)
				{
					base.WriteNullTagLiteral(n, ns);
				}
				return;
			}
			if (!needType)
			{
				Type type = o.GetType();
				if (!(type == typeof(AssociationAssociatedInstance)))
				{
					throw base.CreateUnknownTypeException(o);
				}
			}
			base.WriteStartElement(n, ns, o, false, null);
			if (needType)
			{
				base.WriteXsiType(null, "http://schemas.microsoft.com/cmdlets-over-objects/2009/11");
			}
			this.Write2_TypeMetadata("Type", "http://schemas.microsoft.com/cmdlets-over-objects/2009/11", o.Type, false, false);
			this.Write12_Item("CmdletParameterMetadata", "http://schemas.microsoft.com/cmdlets-over-objects/2009/11", o.CmdletParameterMetadata, false, false);
			base.WriteEndElement(o);
		}

		// Token: 0x06005C75 RID: 23669 RVA: 0x001EFCD8 File Offset: 0x001EDED8
		private void Write15_PropertyMetadata(string n, string ns, PropertyMetadata o, bool isNullable, bool needType)
		{
			if (o == null)
			{
				if (isNullable)
				{
					base.WriteNullTagLiteral(n, ns);
				}
				return;
			}
			if (!needType)
			{
				Type type = o.GetType();
				if (!(type == typeof(PropertyMetadata)))
				{
					throw base.CreateUnknownTypeException(o);
				}
			}
			base.WriteStartElement(n, ns, o, false, null);
			if (needType)
			{
				base.WriteXsiType("PropertyMetadata", "http://schemas.microsoft.com/cmdlets-over-objects/2009/11");
			}
			base.WriteAttribute("PropertyName", "", o.PropertyName);
			this.Write2_TypeMetadata("Type", "http://schemas.microsoft.com/cmdlets-over-objects/2009/11", o.Type, false, false);
			PropertyQuery[] items = o.Items;
			if (items != null)
			{
				ItemsChoiceType[] itemsElementName = o.ItemsElementName;
				if (itemsElementName == null || itemsElementName.Length < items.Length)
				{
					throw base.CreateInvalidChoiceIdentifierValueException("Microsoft.PowerShell.Cmdletization.Xml.ItemsChoiceType", "ItemsElementName");
				}
				for (int i = 0; i < items.Length; i++)
				{
					PropertyQuery propertyQuery = items[i];
					ItemsChoiceType itemsChoiceType = itemsElementName[i];
					if (itemsChoiceType == ItemsChoiceType.RegularQuery && propertyQuery != null)
					{
						if (propertyQuery != null && !(propertyQuery is WildcardablePropertyQuery))
						{
							throw base.CreateMismatchChoiceException("Microsoft.PowerShell.Cmdletization.Xml.WildcardablePropertyQuery", "ItemsElementName", "Microsoft.PowerShell.Cmdletization.Xml.ItemsChoiceType.@RegularQuery");
						}
						this.Write13_WildcardablePropertyQuery("RegularQuery", "http://schemas.microsoft.com/cmdlets-over-objects/2009/11", (WildcardablePropertyQuery)propertyQuery, false, false);
					}
					else if (itemsChoiceType == ItemsChoiceType.ExcludeQuery && propertyQuery != null)
					{
						if (propertyQuery != null && !(propertyQuery is WildcardablePropertyQuery))
						{
							throw base.CreateMismatchChoiceException("Microsoft.PowerShell.Cmdletization.Xml.WildcardablePropertyQuery", "ItemsElementName", "Microsoft.PowerShell.Cmdletization.Xml.ItemsChoiceType.@ExcludeQuery");
						}
						this.Write13_WildcardablePropertyQuery("ExcludeQuery", "http://schemas.microsoft.com/cmdlets-over-objects/2009/11", (WildcardablePropertyQuery)propertyQuery, false, false);
					}
					else if (itemsChoiceType == ItemsChoiceType.MaxValueQuery && propertyQuery != null)
					{
						if (propertyQuery != null && propertyQuery == null)
						{
							throw base.CreateMismatchChoiceException("Microsoft.PowerShell.Cmdletization.Xml.PropertyQuery", "ItemsElementName", "Microsoft.PowerShell.Cmdletization.Xml.ItemsChoiceType.@MaxValueQuery");
						}
						this.Write14_PropertyQuery("MaxValueQuery", "http://schemas.microsoft.com/cmdlets-over-objects/2009/11", propertyQuery, false, false);
					}
					else if (itemsChoiceType == ItemsChoiceType.MinValueQuery && propertyQuery != null)
					{
						if (propertyQuery != null && propertyQuery == null)
						{
							throw base.CreateMismatchChoiceException("Microsoft.PowerShell.Cmdletization.Xml.PropertyQuery", "ItemsElementName", "Microsoft.PowerShell.Cmdletization.Xml.ItemsChoiceType.@MinValueQuery");
						}
						this.Write14_PropertyQuery("MinValueQuery", "http://schemas.microsoft.com/cmdlets-over-objects/2009/11", propertyQuery, false, false);
					}
					else if (propertyQuery != null)
					{
						throw base.CreateUnknownTypeException(propertyQuery);
					}
				}
			}
			base.WriteEndElement(o);
		}

		// Token: 0x06005C76 RID: 23670 RVA: 0x001EFED8 File Offset: 0x001EE0D8
		private void Write14_PropertyQuery(string n, string ns, PropertyQuery o, bool isNullable, bool needType)
		{
			if (o == null)
			{
				if (isNullable)
				{
					base.WriteNullTagLiteral(n, ns);
				}
				return;
			}
			if (!needType)
			{
				Type type = o.GetType();
				if (!(type == typeof(PropertyQuery)))
				{
					if (type == typeof(WildcardablePropertyQuery))
					{
						this.Write13_WildcardablePropertyQuery(n, ns, (WildcardablePropertyQuery)o, isNullable, true);
						return;
					}
					throw base.CreateUnknownTypeException(o);
				}
			}
			base.WriteStartElement(n, ns, o, false, null);
			if (needType)
			{
				base.WriteXsiType("PropertyQuery", "http://schemas.microsoft.com/cmdlets-over-objects/2009/11");
			}
			this.Write12_Item("CmdletParameterMetadata", "http://schemas.microsoft.com/cmdlets-over-objects/2009/11", o.CmdletParameterMetadata, false, false);
			base.WriteEndElement(o);
		}

		// Token: 0x06005C77 RID: 23671 RVA: 0x001EFF7C File Offset: 0x001EE17C
		private void Write10_CmdletParameterMetadata(string n, string ns, CmdletParameterMetadata o, bool isNullable, bool needType)
		{
			if (o == null)
			{
				if (isNullable)
				{
					base.WriteNullTagLiteral(n, ns);
				}
				return;
			}
			if (!needType)
			{
				Type type = o.GetType();
				if (!(type == typeof(CmdletParameterMetadata)))
				{
					if (type == typeof(CmdletParameterMetadataForGetCmdletParameter))
					{
						this.Write11_Item(n, ns, (CmdletParameterMetadataForGetCmdletParameter)o, isNullable, true);
						return;
					}
					if (type == typeof(CmdletParameterMetadataForGetCmdletFilteringParameter))
					{
						this.Write12_Item(n, ns, (CmdletParameterMetadataForGetCmdletFilteringParameter)o, isNullable, true);
						return;
					}
					if (type == typeof(CmdletParameterMetadataForInstanceMethodParameter))
					{
						this.Write9_Item(n, ns, (CmdletParameterMetadataForInstanceMethodParameter)o, isNullable, true);
						return;
					}
					if (type == typeof(CmdletParameterMetadataForStaticMethodParameter))
					{
						this.Write8_Item(n, ns, (CmdletParameterMetadataForStaticMethodParameter)o, isNullable, true);
						return;
					}
					throw base.CreateUnknownTypeException(o);
				}
			}
			base.WriteStartElement(n, ns, o, false, null);
			if (needType)
			{
				base.WriteXsiType("CmdletParameterMetadata", "http://schemas.microsoft.com/cmdlets-over-objects/2009/11");
			}
			if (o.IsMandatorySpecified)
			{
				base.WriteAttribute("IsMandatory", "", XmlConvert.ToString(o.IsMandatory));
			}
			string[] aliases = o.Aliases;
			if (aliases != null)
			{
				base.Writer.WriteStartAttribute(null, "Aliases", "");
				for (int i = 0; i < aliases.Length; i++)
				{
					string value = aliases[i];
					if (i != 0)
					{
						base.Writer.WriteString(" ");
					}
					base.WriteValue(value);
				}
				base.Writer.WriteEndAttribute();
			}
			base.WriteAttribute("PSName", "", o.PSName);
			base.WriteAttribute("Position", "", o.Position);
			this.Write1_Object("AllowEmptyCollection", "http://schemas.microsoft.com/cmdlets-over-objects/2009/11", o.AllowEmptyCollection, false, false);
			this.Write1_Object("AllowEmptyString", "http://schemas.microsoft.com/cmdlets-over-objects/2009/11", o.AllowEmptyString, false, false);
			this.Write1_Object("AllowNull", "http://schemas.microsoft.com/cmdlets-over-objects/2009/11", o.AllowNull, false, false);
			this.Write1_Object("ValidateNotNull", "http://schemas.microsoft.com/cmdlets-over-objects/2009/11", o.ValidateNotNull, false, false);
			this.Write1_Object("ValidateNotNullOrEmpty", "http://schemas.microsoft.com/cmdlets-over-objects/2009/11", o.ValidateNotNullOrEmpty, false, false);
			this.Write4_Item("ValidateCount", "http://schemas.microsoft.com/cmdlets-over-objects/2009/11", o.ValidateCount, false, false);
			this.Write5_Item("ValidateLength", "http://schemas.microsoft.com/cmdlets-over-objects/2009/11", o.ValidateLength, false, false);
			this.Write6_Item("ValidateRange", "http://schemas.microsoft.com/cmdlets-over-objects/2009/11", o.ValidateRange, false, false);
			string[] validateSet = o.ValidateSet;
			if (validateSet != null)
			{
				base.WriteStartElement("ValidateSet", "http://schemas.microsoft.com/cmdlets-over-objects/2009/11", null, false);
				for (int j = 0; j < validateSet.Length; j++)
				{
					base.WriteElementString("AllowedValue", "http://schemas.microsoft.com/cmdlets-over-objects/2009/11", validateSet[j]);
				}
				base.WriteEndElement();
			}
			this.Write7_ObsoleteAttributeMetadata("Obsolete", "http://schemas.microsoft.com/cmdlets-over-objects/2009/11", o.Obsolete, false, false);
			bool isMandatorySpecified = o.IsMandatorySpecified;
			base.WriteEndElement(o);
		}

		// Token: 0x06005C78 RID: 23672 RVA: 0x001F024C File Offset: 0x001EE44C
		private void Write19_GetCmdletParameters(string n, string ns, GetCmdletParameters o, bool isNullable, bool needType)
		{
			if (o == null)
			{
				if (isNullable)
				{
					base.WriteNullTagLiteral(n, ns);
				}
				return;
			}
			if (!needType)
			{
				Type type = o.GetType();
				if (!(type == typeof(GetCmdletParameters)))
				{
					throw base.CreateUnknownTypeException(o);
				}
			}
			base.WriteStartElement(n, ns, o, false, null);
			if (needType)
			{
				base.WriteXsiType("GetCmdletParameters", "http://schemas.microsoft.com/cmdlets-over-objects/2009/11");
			}
			base.WriteAttribute("DefaultCmdletParameterSet", "", o.DefaultCmdletParameterSet);
			PropertyMetadata[] queryableProperties = o.QueryableProperties;
			if (queryableProperties != null)
			{
				base.WriteStartElement("QueryableProperties", "http://schemas.microsoft.com/cmdlets-over-objects/2009/11", null, false);
				for (int i = 0; i < queryableProperties.Length; i++)
				{
					this.Write15_PropertyMetadata("Property", "http://schemas.microsoft.com/cmdlets-over-objects/2009/11", queryableProperties[i], false, false);
				}
				base.WriteEndElement();
			}
			Association[] queryableAssociations = o.QueryableAssociations;
			if (queryableAssociations != null)
			{
				base.WriteStartElement("QueryableAssociations", "http://schemas.microsoft.com/cmdlets-over-objects/2009/11", null, false);
				for (int j = 0; j < queryableAssociations.Length; j++)
				{
					this.Write17_Association("Association", "http://schemas.microsoft.com/cmdlets-over-objects/2009/11", queryableAssociations[j], false, false);
				}
				base.WriteEndElement();
			}
			QueryOption[] queryOptions = o.QueryOptions;
			if (queryOptions != null)
			{
				base.WriteStartElement("QueryOptions", "http://schemas.microsoft.com/cmdlets-over-objects/2009/11", null, false);
				for (int k = 0; k < queryOptions.Length; k++)
				{
					this.Write18_QueryOption("Option", "http://schemas.microsoft.com/cmdlets-over-objects/2009/11", queryOptions[k], false, false);
				}
				base.WriteEndElement();
			}
			base.WriteEndElement(o);
		}

		// Token: 0x06005C79 RID: 23673 RVA: 0x001F03A8 File Offset: 0x001EE5A8
		private void Write45_Item(string n, string ns, StaticCmdletMetadataCmdletMetadata o, bool isNullable, bool needType)
		{
			if (o == null)
			{
				if (isNullable)
				{
					base.WriteNullTagLiteral(n, ns);
				}
				return;
			}
			if (!needType)
			{
				Type type = o.GetType();
				if (!(type == typeof(StaticCmdletMetadataCmdletMetadata)))
				{
					throw base.CreateUnknownTypeException(o);
				}
			}
			base.WriteStartElement(n, ns, o, false, null);
			if (needType)
			{
				base.WriteXsiType("StaticCmdletMetadataCmdletMetadata", "http://schemas.microsoft.com/cmdlets-over-objects/2009/11");
			}
			base.WriteAttribute("Verb", "", o.Verb);
			base.WriteAttribute("Noun", "", o.Noun);
			string[] aliases = o.Aliases;
			if (aliases != null)
			{
				base.Writer.WriteStartAttribute(null, "Aliases", "");
				for (int i = 0; i < aliases.Length; i++)
				{
					string value = aliases[i];
					if (i != 0)
					{
						base.Writer.WriteString(" ");
					}
					base.WriteValue(value);
				}
				base.Writer.WriteEndAttribute();
			}
			if (o.ConfirmImpactSpecified)
			{
				base.WriteAttribute("ConfirmImpact", "", this.Write20_ConfirmImpact(o.ConfirmImpact));
			}
			base.WriteAttribute("HelpUri", "", o.HelpUri);
			base.WriteAttribute("DefaultCmdletParameterSet", "", o.DefaultCmdletParameterSet);
			this.Write7_ObsoleteAttributeMetadata("Obsolete", "http://schemas.microsoft.com/cmdlets-over-objects/2009/11", o.Obsolete, false, false);
			bool confirmImpactSpecified = o.ConfirmImpactSpecified;
			base.WriteEndElement(o);
		}

		// Token: 0x06005C7A RID: 23674 RVA: 0x001F0504 File Offset: 0x001EE704
		private void Write21_CommonCmdletMetadata(string n, string ns, CommonCmdletMetadata o, bool isNullable, bool needType)
		{
			if (o == null)
			{
				if (isNullable)
				{
					base.WriteNullTagLiteral(n, ns);
				}
				return;
			}
			if (!needType)
			{
				Type type = o.GetType();
				if (!(type == typeof(CommonCmdletMetadata)))
				{
					if (type == typeof(StaticCmdletMetadataCmdletMetadata))
					{
						this.Write45_Item(n, ns, (StaticCmdletMetadataCmdletMetadata)o, isNullable, true);
						return;
					}
					throw base.CreateUnknownTypeException(o);
				}
			}
			base.WriteStartElement(n, ns, o, false, null);
			if (needType)
			{
				base.WriteXsiType("CommonCmdletMetadata", "http://schemas.microsoft.com/cmdlets-over-objects/2009/11");
			}
			base.WriteAttribute("Verb", "", o.Verb);
			base.WriteAttribute("Noun", "", o.Noun);
			string[] aliases = o.Aliases;
			if (aliases != null)
			{
				base.Writer.WriteStartAttribute(null, "Aliases", "");
				for (int i = 0; i < aliases.Length; i++)
				{
					string value = aliases[i];
					if (i != 0)
					{
						base.Writer.WriteString(" ");
					}
					base.WriteValue(value);
				}
				base.Writer.WriteEndAttribute();
			}
			if (o.ConfirmImpactSpecified)
			{
				base.WriteAttribute("ConfirmImpact", "", this.Write20_ConfirmImpact(o.ConfirmImpact));
			}
			base.WriteAttribute("HelpUri", "", o.HelpUri);
			this.Write7_ObsoleteAttributeMetadata("Obsolete", "http://schemas.microsoft.com/cmdlets-over-objects/2009/11", o.Obsolete, false, false);
			bool confirmImpactSpecified = o.ConfirmImpactSpecified;
			base.WriteEndElement(o);
		}

		// Token: 0x06005C7B RID: 23675 RVA: 0x001F066C File Offset: 0x001EE86C
		private void Write22_GetCmdletMetadata(string n, string ns, GetCmdletMetadata o, bool isNullable, bool needType)
		{
			if (o == null)
			{
				if (isNullable)
				{
					base.WriteNullTagLiteral(n, ns);
				}
				return;
			}
			if (!needType)
			{
				Type type = o.GetType();
				if (!(type == typeof(GetCmdletMetadata)))
				{
					throw base.CreateUnknownTypeException(o);
				}
			}
			base.WriteStartElement(n, ns, o, false, null);
			if (needType)
			{
				base.WriteXsiType("GetCmdletMetadata", "http://schemas.microsoft.com/cmdlets-over-objects/2009/11");
			}
			this.Write21_CommonCmdletMetadata("CmdletMetadata", "http://schemas.microsoft.com/cmdlets-over-objects/2009/11", o.CmdletMetadata, false, false);
			this.Write19_GetCmdletParameters("GetCmdletParameters", "http://schemas.microsoft.com/cmdlets-over-objects/2009/11", o.GetCmdletParameters, false, false);
			base.WriteEndElement(o);
		}

		// Token: 0x06005C7C RID: 23676 RVA: 0x001F0704 File Offset: 0x001EE904
		private void Write30_InstanceMethodMetadata(string n, string ns, InstanceMethodMetadata o, bool isNullable, bool needType)
		{
			if (o == null)
			{
				if (isNullable)
				{
					base.WriteNullTagLiteral(n, ns);
				}
				return;
			}
			if (!needType)
			{
				Type type = o.GetType();
				if (!(type == typeof(InstanceMethodMetadata)))
				{
					throw base.CreateUnknownTypeException(o);
				}
			}
			base.WriteStartElement(n, ns, o, false, null);
			if (needType)
			{
				base.WriteXsiType("InstanceMethodMetadata", "http://schemas.microsoft.com/cmdlets-over-objects/2009/11");
			}
			base.WriteAttribute("MethodName", "", o.MethodName);
			this.Write24_Item("ReturnValue", "http://schemas.microsoft.com/cmdlets-over-objects/2009/11", o.ReturnValue, false, false);
			InstanceMethodParameterMetadata[] parameters = o.Parameters;
			if (parameters != null)
			{
				base.WriteStartElement("Parameters", "http://schemas.microsoft.com/cmdlets-over-objects/2009/11", null, false);
				for (int i = 0; i < parameters.Length; i++)
				{
					this.Write25_Item("Parameter", "http://schemas.microsoft.com/cmdlets-over-objects/2009/11", parameters[i], false, false);
				}
				base.WriteEndElement();
			}
			base.WriteEndElement(o);
		}

		// Token: 0x06005C7D RID: 23677 RVA: 0x001F07E0 File Offset: 0x001EE9E0
		private void Write29_CommonMethodMetadata(string n, string ns, CommonMethodMetadata o, bool isNullable, bool needType)
		{
			if (o == null)
			{
				if (isNullable)
				{
					base.WriteNullTagLiteral(n, ns);
				}
				return;
			}
			if (!needType)
			{
				Type type = o.GetType();
				if (!(type == typeof(CommonMethodMetadata)))
				{
					if (type == typeof(InstanceMethodMetadata))
					{
						this.Write30_InstanceMethodMetadata(n, ns, (InstanceMethodMetadata)o, isNullable, true);
						return;
					}
					if (type == typeof(StaticMethodMetadata))
					{
						this.Write28_StaticMethodMetadata(n, ns, (StaticMethodMetadata)o, isNullable, true);
						return;
					}
					throw base.CreateUnknownTypeException(o);
				}
			}
			base.WriteStartElement(n, ns, o, false, null);
			if (needType)
			{
				base.WriteXsiType("CommonMethodMetadata", "http://schemas.microsoft.com/cmdlets-over-objects/2009/11");
			}
			base.WriteAttribute("MethodName", "", o.MethodName);
			this.Write24_Item("ReturnValue", "http://schemas.microsoft.com/cmdlets-over-objects/2009/11", o.ReturnValue, false, false);
			base.WriteEndElement(o);
		}

		// Token: 0x06005C7E RID: 23678 RVA: 0x001F08C0 File Offset: 0x001EEAC0
		private void Write26_CommonMethodParameterMetadata(string n, string ns, CommonMethodParameterMetadata o, bool isNullable, bool needType)
		{
			if (o == null)
			{
				if (isNullable)
				{
					base.WriteNullTagLiteral(n, ns);
				}
				return;
			}
			if (!needType)
			{
				Type type = o.GetType();
				if (!(type == typeof(CommonMethodParameterMetadata)))
				{
					if (type == typeof(StaticMethodParameterMetadata))
					{
						this.Write27_StaticMethodParameterMetadata(n, ns, (StaticMethodParameterMetadata)o, isNullable, true);
						return;
					}
					if (type == typeof(InstanceMethodParameterMetadata))
					{
						this.Write25_Item(n, ns, (InstanceMethodParameterMetadata)o, isNullable, true);
						return;
					}
					throw base.CreateUnknownTypeException(o);
				}
			}
			base.WriteStartElement(n, ns, o, false, null);
			if (needType)
			{
				base.WriteXsiType("CommonMethodParameterMetadata", "http://schemas.microsoft.com/cmdlets-over-objects/2009/11");
			}
			base.WriteAttribute("ParameterName", "", o.ParameterName);
			base.WriteAttribute("DefaultValue", "", o.DefaultValue);
			this.Write2_TypeMetadata("Type", "http://schemas.microsoft.com/cmdlets-over-objects/2009/11", o.Type, false, false);
			base.WriteEndElement(o);
		}

		// Token: 0x06005C7F RID: 23679 RVA: 0x001F09B4 File Offset: 0x001EEBB4
		private void Write31_InstanceCmdletMetadata(string n, string ns, InstanceCmdletMetadata o, bool isNullable, bool needType)
		{
			if (o == null)
			{
				if (isNullable)
				{
					base.WriteNullTagLiteral(n, ns);
				}
				return;
			}
			if (!needType)
			{
				Type type = o.GetType();
				if (!(type == typeof(InstanceCmdletMetadata)))
				{
					throw base.CreateUnknownTypeException(o);
				}
			}
			base.WriteStartElement(n, ns, o, false, null);
			if (needType)
			{
				base.WriteXsiType("InstanceCmdletMetadata", "http://schemas.microsoft.com/cmdlets-over-objects/2009/11");
			}
			this.Write21_CommonCmdletMetadata("CmdletMetadata", "http://schemas.microsoft.com/cmdlets-over-objects/2009/11", o.CmdletMetadata, false, false);
			this.Write30_InstanceMethodMetadata("Method", "http://schemas.microsoft.com/cmdlets-over-objects/2009/11", o.Method, false, false);
			this.Write19_GetCmdletParameters("GetCmdletParameters", "http://schemas.microsoft.com/cmdlets-over-objects/2009/11", o.GetCmdletParameters, false, false);
			base.WriteEndElement(o);
		}

		// Token: 0x06005C80 RID: 23680 RVA: 0x001F0A64 File Offset: 0x001EEC64
		private void Write36_ClassMetadata(string n, string ns, ClassMetadata o, bool isNullable, bool needType)
		{
			if (o == null)
			{
				if (isNullable)
				{
					base.WriteNullTagLiteral(n, ns);
				}
				return;
			}
			if (!needType)
			{
				Type type = o.GetType();
				if (!(type == typeof(ClassMetadata)))
				{
					throw base.CreateUnknownTypeException(o);
				}
			}
			base.WriteStartElement(n, ns, o, false, null);
			if (needType)
			{
				base.WriteXsiType("ClassMetadata", "http://schemas.microsoft.com/cmdlets-over-objects/2009/11");
			}
			base.WriteAttribute("CmdletAdapter", "", o.CmdletAdapter);
			base.WriteAttribute("ClassName", "", o.ClassName);
			base.WriteAttribute("ClassVersion", "", o.ClassVersion);
			base.WriteElementString("Version", "http://schemas.microsoft.com/cmdlets-over-objects/2009/11", o.Version);
			base.WriteElementString("DefaultNoun", "http://schemas.microsoft.com/cmdlets-over-objects/2009/11", o.DefaultNoun);
			this.Write32_ClassMetadataInstanceCmdlets("InstanceCmdlets", "http://schemas.microsoft.com/cmdlets-over-objects/2009/11", o.InstanceCmdlets, false, false);
			StaticCmdletMetadata[] staticCmdlets = o.StaticCmdlets;
			if (staticCmdlets != null)
			{
				base.WriteStartElement("StaticCmdlets", "http://schemas.microsoft.com/cmdlets-over-objects/2009/11", null, false);
				for (int i = 0; i < staticCmdlets.Length; i++)
				{
					this.Write34_StaticCmdletMetadata("Cmdlet", "http://schemas.microsoft.com/cmdlets-over-objects/2009/11", staticCmdlets[i], false, false);
				}
				base.WriteEndElement();
			}
			ClassMetadataData[] cmdletAdapterPrivateData = o.CmdletAdapterPrivateData;
			if (cmdletAdapterPrivateData != null)
			{
				base.WriteStartElement("CmdletAdapterPrivateData", "http://schemas.microsoft.com/cmdlets-over-objects/2009/11", null, false);
				for (int j = 0; j < cmdletAdapterPrivateData.Length; j++)
				{
					this.Write35_ClassMetadataData("Data", "http://schemas.microsoft.com/cmdlets-over-objects/2009/11", cmdletAdapterPrivateData[j], false, false);
				}
				base.WriteEndElement();
			}
			base.WriteEndElement(o);
		}

		// Token: 0x06005C81 RID: 23681 RVA: 0x001F0BE4 File Offset: 0x001EEDE4
		private void Write32_ClassMetadataInstanceCmdlets(string n, string ns, ClassMetadataInstanceCmdlets o, bool isNullable, bool needType)
		{
			if (o == null)
			{
				if (isNullable)
				{
					base.WriteNullTagLiteral(n, ns);
				}
				return;
			}
			if (!needType)
			{
				Type type = o.GetType();
				if (!(type == typeof(ClassMetadataInstanceCmdlets)))
				{
					throw base.CreateUnknownTypeException(o);
				}
			}
			base.WriteStartElement(n, ns, o, false, null);
			if (needType)
			{
				base.WriteXsiType(null, "http://schemas.microsoft.com/cmdlets-over-objects/2009/11");
			}
			this.Write19_GetCmdletParameters("GetCmdletParameters", "http://schemas.microsoft.com/cmdlets-over-objects/2009/11", o.GetCmdletParameters, false, false);
			this.Write22_GetCmdletMetadata("GetCmdlet", "http://schemas.microsoft.com/cmdlets-over-objects/2009/11", o.GetCmdlet, false, false);
			InstanceCmdletMetadata[] cmdlet = o.Cmdlet;
			if (cmdlet != null)
			{
				for (int i = 0; i < cmdlet.Length; i++)
				{
					this.Write31_InstanceCmdletMetadata("Cmdlet", "http://schemas.microsoft.com/cmdlets-over-objects/2009/11", cmdlet[i], false, false);
				}
			}
			base.WriteEndElement(o);
		}

		// Token: 0x06005C82 RID: 23682 RVA: 0x001F0CA8 File Offset: 0x001EEEA8
		private void Write40_ClassMetadataInstanceCmdlets(string n, string ns, ClassMetadataInstanceCmdlets o, bool isNullable, bool needType)
		{
			if (o == null)
			{
				if (isNullable)
				{
					base.WriteNullTagLiteral(n, ns);
				}
				return;
			}
			if (!needType)
			{
				Type type = o.GetType();
				if (!(type == typeof(ClassMetadataInstanceCmdlets)))
				{
					throw base.CreateUnknownTypeException(o);
				}
			}
			base.WriteStartElement(n, ns, o, false, null);
			if (needType)
			{
				base.WriteXsiType("ClassMetadataInstanceCmdlets", "http://schemas.microsoft.com/cmdlets-over-objects/2009/11");
			}
			this.Write19_GetCmdletParameters("GetCmdletParameters", "http://schemas.microsoft.com/cmdlets-over-objects/2009/11", o.GetCmdletParameters, false, false);
			this.Write22_GetCmdletMetadata("GetCmdlet", "http://schemas.microsoft.com/cmdlets-over-objects/2009/11", o.GetCmdlet, false, false);
			InstanceCmdletMetadata[] cmdlet = o.Cmdlet;
			if (cmdlet != null)
			{
				for (int i = 0; i < cmdlet.Length; i++)
				{
					this.Write31_InstanceCmdletMetadata("Cmdlet", "http://schemas.microsoft.com/cmdlets-over-objects/2009/11", cmdlet[i], false, false);
				}
			}
			base.WriteEndElement(o);
		}

		// Token: 0x06005C83 RID: 23683 RVA: 0x001F0D70 File Offset: 0x001EEF70
		private void Write41_AssociationAssociatedInstance(string n, string ns, AssociationAssociatedInstance o, bool isNullable, bool needType)
		{
			if (o == null)
			{
				if (isNullable)
				{
					base.WriteNullTagLiteral(n, ns);
				}
				return;
			}
			if (!needType)
			{
				Type type = o.GetType();
				if (!(type == typeof(AssociationAssociatedInstance)))
				{
					throw base.CreateUnknownTypeException(o);
				}
			}
			base.WriteStartElement(n, ns, o, false, null);
			if (needType)
			{
				base.WriteXsiType("AssociationAssociatedInstance", "http://schemas.microsoft.com/cmdlets-over-objects/2009/11");
			}
			this.Write2_TypeMetadata("Type", "http://schemas.microsoft.com/cmdlets-over-objects/2009/11", o.Type, false, false);
			this.Write12_Item("CmdletParameterMetadata", "http://schemas.microsoft.com/cmdlets-over-objects/2009/11", o.CmdletParameterMetadata, false, false);
			base.WriteEndElement(o);
		}

		// Token: 0x06005C84 RID: 23684 RVA: 0x001F0E08 File Offset: 0x001EF008
		private void Write42_Item(string n, string ns, CmdletParameterMetadataValidateCount o, bool isNullable, bool needType)
		{
			if (o == null)
			{
				if (isNullable)
				{
					base.WriteNullTagLiteral(n, ns);
				}
				return;
			}
			if (!needType)
			{
				Type type = o.GetType();
				if (!(type == typeof(CmdletParameterMetadataValidateCount)))
				{
					throw base.CreateUnknownTypeException(o);
				}
			}
			base.WriteStartElement(n, ns, o, false, null);
			if (needType)
			{
				base.WriteXsiType("CmdletParameterMetadataValidateCount", "http://schemas.microsoft.com/cmdlets-over-objects/2009/11");
			}
			base.WriteAttribute("Min", "", o.Min);
			base.WriteAttribute("Max", "", o.Max);
			base.WriteEndElement(o);
		}

		// Token: 0x06005C85 RID: 23685 RVA: 0x001F0E9C File Offset: 0x001EF09C
		private void Write43_Item(string n, string ns, CmdletParameterMetadataValidateLength o, bool isNullable, bool needType)
		{
			if (o == null)
			{
				if (isNullable)
				{
					base.WriteNullTagLiteral(n, ns);
				}
				return;
			}
			if (!needType)
			{
				Type type = o.GetType();
				if (!(type == typeof(CmdletParameterMetadataValidateLength)))
				{
					throw base.CreateUnknownTypeException(o);
				}
			}
			base.WriteStartElement(n, ns, o, false, null);
			if (needType)
			{
				base.WriteXsiType("CmdletParameterMetadataValidateLength", "http://schemas.microsoft.com/cmdlets-over-objects/2009/11");
			}
			base.WriteAttribute("Min", "", o.Min);
			base.WriteAttribute("Max", "", o.Max);
			base.WriteEndElement(o);
		}

		// Token: 0x06005C86 RID: 23686 RVA: 0x001F0F30 File Offset: 0x001EF130
		private void Write44_Item(string n, string ns, CmdletParameterMetadataValidateRange o, bool isNullable, bool needType)
		{
			if (o == null)
			{
				if (isNullable)
				{
					base.WriteNullTagLiteral(n, ns);
				}
				return;
			}
			if (!needType)
			{
				Type type = o.GetType();
				if (!(type == typeof(CmdletParameterMetadataValidateRange)))
				{
					throw base.CreateUnknownTypeException(o);
				}
			}
			base.WriteStartElement(n, ns, o, false, null);
			if (needType)
			{
				base.WriteXsiType("CmdletParameterMetadataValidateRange", "http://schemas.microsoft.com/cmdlets-over-objects/2009/11");
			}
			base.WriteAttribute("Min", "", o.Min);
			base.WriteAttribute("Max", "", o.Max);
			base.WriteEndElement(o);
		}

		// Token: 0x06005C87 RID: 23687 RVA: 0x001F0FC4 File Offset: 0x001EF1C4
		private void Write46_Item(string n, string ns, CommonMethodMetadataReturnValue o, bool isNullable, bool needType)
		{
			if (o == null)
			{
				if (isNullable)
				{
					base.WriteNullTagLiteral(n, ns);
				}
				return;
			}
			if (!needType)
			{
				Type type = o.GetType();
				if (!(type == typeof(CommonMethodMetadataReturnValue)))
				{
					throw base.CreateUnknownTypeException(o);
				}
			}
			base.WriteStartElement(n, ns, o, false, null);
			if (needType)
			{
				base.WriteXsiType("CommonMethodMetadataReturnValue", "http://schemas.microsoft.com/cmdlets-over-objects/2009/11");
			}
			this.Write2_TypeMetadata("Type", "http://schemas.microsoft.com/cmdlets-over-objects/2009/11", o.Type, false, false);
			this.Write23_CmdletOutputMetadata("CmdletOutputMetadata", "http://schemas.microsoft.com/cmdlets-over-objects/2009/11", o.CmdletOutputMetadata, false, false);
			base.WriteEndElement(o);
		}

		// Token: 0x06005C88 RID: 23688 RVA: 0x001F105C File Offset: 0x001EF25C
		private void Write39_PowerShellMetadata(string n, string ns, PowerShellMetadata o, bool isNullable, bool needType)
		{
			if (o == null)
			{
				if (isNullable)
				{
					base.WriteNullTagLiteral(n, ns);
				}
				return;
			}
			if (!needType)
			{
				Type type = o.GetType();
				if (!(type == typeof(PowerShellMetadata)))
				{
					throw base.CreateUnknownTypeException(o);
				}
			}
			base.WriteStartElement(n, ns, o, false, null);
			if (needType)
			{
				base.WriteXsiType(null, "http://schemas.microsoft.com/cmdlets-over-objects/2009/11");
			}
			this.Write36_ClassMetadata("Class", "http://schemas.microsoft.com/cmdlets-over-objects/2009/11", o.Class, false, false);
			EnumMetadataEnum[] enums = o.Enums;
			if (enums != null)
			{
				base.WriteStartElement("Enums", "http://schemas.microsoft.com/cmdlets-over-objects/2009/11", null, false);
				for (int i = 0; i < enums.Length; i++)
				{
					this.Write38_EnumMetadataEnum("Enum", "http://schemas.microsoft.com/cmdlets-over-objects/2009/11", enums[i], false, false);
				}
				base.WriteEndElement();
			}
			base.WriteEndElement(o);
		}

		// Token: 0x06005C89 RID: 23689 RVA: 0x001F111D File Offset: 0x001EF31D
		protected override void InitCallbacks()
		{
		}
	}
}
