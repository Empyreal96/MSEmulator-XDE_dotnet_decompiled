using System;
using System.CodeDom.Compiler;
using System.Collections;
using System.Xml.Serialization;

namespace Microsoft.PowerShell.Cmdletization.Xml
{
	// Token: 0x020009F8 RID: 2552
	[GeneratedCode("sgen", "4.0")]
	internal class XmlSerializerContract : XmlSerializerImplementation
	{
		// Token: 0x170012A8 RID: 4776
		// (get) Token: 0x06005D80 RID: 23936 RVA: 0x001FD547 File Offset: 0x001FB747
		public override XmlSerializationReader Reader
		{
			get
			{
				return new XmlSerializationReader1();
			}
		}

		// Token: 0x170012A9 RID: 4777
		// (get) Token: 0x06005D81 RID: 23937 RVA: 0x001FD54E File Offset: 0x001FB74E
		public override XmlSerializationWriter Writer
		{
			get
			{
				return new XmlSerializationWriter1();
			}
		}

		// Token: 0x170012AA RID: 4778
		// (get) Token: 0x06005D82 RID: 23938 RVA: 0x001FD558 File Offset: 0x001FB758
		public override Hashtable ReadMethods
		{
			get
			{
				if (this.readMethods == null)
				{
					Hashtable hashtable = new Hashtable();
					hashtable["Microsoft.PowerShell.Cmdletization.Xml.PowerShellMetadata:http://schemas.microsoft.com/cmdlets-over-objects/2009/11::False:"] = "Read50_PowerShellMetadata";
					hashtable["Microsoft.PowerShell.Cmdletization.Xml.ClassMetadata::"] = "Read51_ClassMetadata";
					hashtable["Microsoft.PowerShell.Cmdletization.Xml.ClassMetadataInstanceCmdlets::"] = "Read52_ClassMetadataInstanceCmdlets";
					hashtable["Microsoft.PowerShell.Cmdletization.Xml.GetCmdletParameters::"] = "Read53_GetCmdletParameters";
					hashtable["Microsoft.PowerShell.Cmdletization.Xml.PropertyMetadata::"] = "Read54_PropertyMetadata";
					hashtable["Microsoft.PowerShell.Cmdletization.Xml.TypeMetadata::"] = "Read55_TypeMetadata";
					hashtable["Microsoft.PowerShell.Cmdletization.Xml.Association::"] = "Read56_Association";
					hashtable["Microsoft.PowerShell.Cmdletization.Xml.AssociationAssociatedInstance::"] = "Read57_AssociationAssociatedInstance";
					hashtable["Microsoft.PowerShell.Cmdletization.Xml.CmdletParameterMetadata::"] = "Read58_CmdletParameterMetadata";
					hashtable["Microsoft.PowerShell.Cmdletization.Xml.CmdletParameterMetadataForGetCmdletParameter::"] = "Read59_Item";
					hashtable["Microsoft.PowerShell.Cmdletization.Xml.CmdletParameterMetadataForGetCmdletFilteringParameter::"] = "Read60_Item";
					hashtable["Microsoft.PowerShell.Cmdletization.Xml.CmdletParameterMetadataValidateCount::"] = "Read61_Item";
					hashtable["Microsoft.PowerShell.Cmdletization.Xml.CmdletParameterMetadataValidateLength::"] = "Read62_Item";
					hashtable["Microsoft.PowerShell.Cmdletization.Xml.CmdletParameterMetadataValidateRange::"] = "Read63_Item";
					hashtable["Microsoft.PowerShell.Cmdletization.Xml.ObsoleteAttributeMetadata::"] = "Read64_ObsoleteAttributeMetadata";
					hashtable["Microsoft.PowerShell.Cmdletization.Xml.CmdletParameterMetadataForInstanceMethodParameter::"] = "Read65_Item";
					hashtable["Microsoft.PowerShell.Cmdletization.Xml.CmdletParameterMetadataForStaticMethodParameter::"] = "Read66_Item";
					hashtable["Microsoft.PowerShell.Cmdletization.Xml.QueryOption::"] = "Read67_QueryOption";
					hashtable["Microsoft.PowerShell.Cmdletization.Xml.GetCmdletMetadata::"] = "Read68_GetCmdletMetadata";
					hashtable["Microsoft.PowerShell.Cmdletization.Xml.CommonCmdletMetadata::"] = "Read69_CommonCmdletMetadata";
					hashtable["Microsoft.PowerShell.Cmdletization.Xml.ConfirmImpact::"] = "Read70_ConfirmImpact";
					hashtable["Microsoft.PowerShell.Cmdletization.Xml.StaticCmdletMetadata::"] = "Read71_StaticCmdletMetadata";
					hashtable["Microsoft.PowerShell.Cmdletization.Xml.StaticCmdletMetadataCmdletMetadata::"] = "Read72_Item";
					hashtable["Microsoft.PowerShell.Cmdletization.Xml.CommonMethodMetadata::"] = "Read73_CommonMethodMetadata";
					hashtable["Microsoft.PowerShell.Cmdletization.Xml.StaticMethodMetadata::"] = "Read74_StaticMethodMetadata";
					hashtable["Microsoft.PowerShell.Cmdletization.Xml.CommonMethodParameterMetadata::"] = "Read75_CommonMethodParameterMetadata";
					hashtable["Microsoft.PowerShell.Cmdletization.Xml.StaticMethodParameterMetadata::"] = "Read76_StaticMethodParameterMetadata";
					hashtable["Microsoft.PowerShell.Cmdletization.Xml.CmdletOutputMetadata::"] = "Read77_CmdletOutputMetadata";
					hashtable["Microsoft.PowerShell.Cmdletization.Xml.InstanceMethodParameterMetadata::"] = "Read78_Item";
					hashtable["Microsoft.PowerShell.Cmdletization.Xml.CommonMethodMetadataReturnValue::"] = "Read79_Item";
					hashtable["Microsoft.PowerShell.Cmdletization.Xml.InstanceMethodMetadata::"] = "Read80_InstanceMethodMetadata";
					hashtable["Microsoft.PowerShell.Cmdletization.Xml.InstanceCmdletMetadata::"] = "Read81_InstanceCmdletMetadata";
					hashtable["Microsoft.PowerShell.Cmdletization.Xml.PropertyQuery::"] = "Read82_PropertyQuery";
					hashtable["Microsoft.PowerShell.Cmdletization.Xml.WildcardablePropertyQuery::"] = "Read83_WildcardablePropertyQuery";
					hashtable["Microsoft.PowerShell.Cmdletization.Xml.ItemsChoiceType::"] = "Read84_ItemsChoiceType";
					hashtable["Microsoft.PowerShell.Cmdletization.Xml.ClassMetadataData::"] = "Read85_ClassMetadataData";
					hashtable["Microsoft.PowerShell.Cmdletization.Xml.EnumMetadataEnum::"] = "Read86_EnumMetadataEnum";
					hashtable["Microsoft.PowerShell.Cmdletization.Xml.EnumMetadataEnumValue::"] = "Read87_EnumMetadataEnumValue";
					if (this.readMethods == null)
					{
						this.readMethods = hashtable;
					}
				}
				return this.readMethods;
			}
		}

		// Token: 0x170012AB RID: 4779
		// (get) Token: 0x06005D83 RID: 23939 RVA: 0x001FD7EC File Offset: 0x001FB9EC
		public override Hashtable WriteMethods
		{
			get
			{
				if (this.writeMethods == null)
				{
					Hashtable hashtable = new Hashtable();
					hashtable["Microsoft.PowerShell.Cmdletization.Xml.PowerShellMetadata:http://schemas.microsoft.com/cmdlets-over-objects/2009/11::False:"] = "Write50_PowerShellMetadata";
					hashtable["Microsoft.PowerShell.Cmdletization.Xml.ClassMetadata::"] = "Write51_ClassMetadata";
					hashtable["Microsoft.PowerShell.Cmdletization.Xml.ClassMetadataInstanceCmdlets::"] = "Write52_ClassMetadataInstanceCmdlets";
					hashtable["Microsoft.PowerShell.Cmdletization.Xml.GetCmdletParameters::"] = "Write53_GetCmdletParameters";
					hashtable["Microsoft.PowerShell.Cmdletization.Xml.PropertyMetadata::"] = "Write54_PropertyMetadata";
					hashtable["Microsoft.PowerShell.Cmdletization.Xml.TypeMetadata::"] = "Write55_TypeMetadata";
					hashtable["Microsoft.PowerShell.Cmdletization.Xml.Association::"] = "Write56_Association";
					hashtable["Microsoft.PowerShell.Cmdletization.Xml.AssociationAssociatedInstance::"] = "Write57_AssociationAssociatedInstance";
					hashtable["Microsoft.PowerShell.Cmdletization.Xml.CmdletParameterMetadata::"] = "Write58_CmdletParameterMetadata";
					hashtable["Microsoft.PowerShell.Cmdletization.Xml.CmdletParameterMetadataForGetCmdletParameter::"] = "Write59_Item";
					hashtable["Microsoft.PowerShell.Cmdletization.Xml.CmdletParameterMetadataForGetCmdletFilteringParameter::"] = "Write60_Item";
					hashtable["Microsoft.PowerShell.Cmdletization.Xml.CmdletParameterMetadataValidateCount::"] = "Write61_Item";
					hashtable["Microsoft.PowerShell.Cmdletization.Xml.CmdletParameterMetadataValidateLength::"] = "Write62_Item";
					hashtable["Microsoft.PowerShell.Cmdletization.Xml.CmdletParameterMetadataValidateRange::"] = "Write63_Item";
					hashtable["Microsoft.PowerShell.Cmdletization.Xml.ObsoleteAttributeMetadata::"] = "Write64_ObsoleteAttributeMetadata";
					hashtable["Microsoft.PowerShell.Cmdletization.Xml.CmdletParameterMetadataForInstanceMethodParameter::"] = "Write65_Item";
					hashtable["Microsoft.PowerShell.Cmdletization.Xml.CmdletParameterMetadataForStaticMethodParameter::"] = "Write66_Item";
					hashtable["Microsoft.PowerShell.Cmdletization.Xml.QueryOption::"] = "Write67_QueryOption";
					hashtable["Microsoft.PowerShell.Cmdletization.Xml.GetCmdletMetadata::"] = "Write68_GetCmdletMetadata";
					hashtable["Microsoft.PowerShell.Cmdletization.Xml.CommonCmdletMetadata::"] = "Write69_CommonCmdletMetadata";
					hashtable["Microsoft.PowerShell.Cmdletization.Xml.ConfirmImpact::"] = "Write70_ConfirmImpact";
					hashtable["Microsoft.PowerShell.Cmdletization.Xml.StaticCmdletMetadata::"] = "Write71_StaticCmdletMetadata";
					hashtable["Microsoft.PowerShell.Cmdletization.Xml.StaticCmdletMetadataCmdletMetadata::"] = "Write72_Item";
					hashtable["Microsoft.PowerShell.Cmdletization.Xml.CommonMethodMetadata::"] = "Write73_CommonMethodMetadata";
					hashtable["Microsoft.PowerShell.Cmdletization.Xml.StaticMethodMetadata::"] = "Write74_StaticMethodMetadata";
					hashtable["Microsoft.PowerShell.Cmdletization.Xml.CommonMethodParameterMetadata::"] = "Write75_CommonMethodParameterMetadata";
					hashtable["Microsoft.PowerShell.Cmdletization.Xml.StaticMethodParameterMetadata::"] = "Write76_StaticMethodParameterMetadata";
					hashtable["Microsoft.PowerShell.Cmdletization.Xml.CmdletOutputMetadata::"] = "Write77_CmdletOutputMetadata";
					hashtable["Microsoft.PowerShell.Cmdletization.Xml.InstanceMethodParameterMetadata::"] = "Write78_Item";
					hashtable["Microsoft.PowerShell.Cmdletization.Xml.CommonMethodMetadataReturnValue::"] = "Write79_Item";
					hashtable["Microsoft.PowerShell.Cmdletization.Xml.InstanceMethodMetadata::"] = "Write80_InstanceMethodMetadata";
					hashtable["Microsoft.PowerShell.Cmdletization.Xml.InstanceCmdletMetadata::"] = "Write81_InstanceCmdletMetadata";
					hashtable["Microsoft.PowerShell.Cmdletization.Xml.PropertyQuery::"] = "Write82_PropertyQuery";
					hashtable["Microsoft.PowerShell.Cmdletization.Xml.WildcardablePropertyQuery::"] = "Write83_WildcardablePropertyQuery";
					hashtable["Microsoft.PowerShell.Cmdletization.Xml.ItemsChoiceType::"] = "Write84_ItemsChoiceType";
					hashtable["Microsoft.PowerShell.Cmdletization.Xml.ClassMetadataData::"] = "Write85_ClassMetadataData";
					hashtable["Microsoft.PowerShell.Cmdletization.Xml.EnumMetadataEnum::"] = "Write86_EnumMetadataEnum";
					hashtable["Microsoft.PowerShell.Cmdletization.Xml.EnumMetadataEnumValue::"] = "Write87_EnumMetadataEnumValue";
					if (this.writeMethods == null)
					{
						this.writeMethods = hashtable;
					}
				}
				return this.writeMethods;
			}
		}

		// Token: 0x170012AC RID: 4780
		// (get) Token: 0x06005D84 RID: 23940 RVA: 0x001FDA80 File Offset: 0x001FBC80
		public override Hashtable TypedSerializers
		{
			get
			{
				if (this.typedSerializers == null)
				{
					Hashtable hashtable = new Hashtable();
					hashtable.Add("Microsoft.PowerShell.Cmdletization.Xml.AssociationAssociatedInstance::", new AssociationAssociatedInstanceSerializer());
					hashtable.Add("Microsoft.PowerShell.Cmdletization.Xml.Association::", new AssociationSerializer());
					hashtable.Add("Microsoft.PowerShell.Cmdletization.Xml.ClassMetadataInstanceCmdlets::", new ClassMetadataInstanceCmdletsSerializer());
					hashtable.Add("Microsoft.PowerShell.Cmdletization.Xml.PowerShellMetadata:http://schemas.microsoft.com/cmdlets-over-objects/2009/11::False:", new PowerShellMetadataSerializer());
					hashtable.Add("Microsoft.PowerShell.Cmdletization.Xml.EnumMetadataEnumValue::", new EnumMetadataEnumValueSerializer());
					hashtable.Add("Microsoft.PowerShell.Cmdletization.Xml.StaticCmdletMetadata::", new StaticCmdletMetadataSerializer());
					hashtable.Add("Microsoft.PowerShell.Cmdletization.Xml.ItemsChoiceType::", new ItemsChoiceTypeSerializer());
					hashtable.Add("Microsoft.PowerShell.Cmdletization.Xml.PropertyQuery::", new PropertyQuerySerializer());
					hashtable.Add("Microsoft.PowerShell.Cmdletization.Xml.CmdletParameterMetadata::", new CmdletParameterMetadataSerializer());
					hashtable.Add("Microsoft.PowerShell.Cmdletization.Xml.CommonMethodParameterMetadata::", new CommonMethodParameterMetadataSerializer());
					hashtable.Add("Microsoft.PowerShell.Cmdletization.Xml.StaticMethodMetadata::", new StaticMethodMetadataSerializer());
					hashtable.Add("Microsoft.PowerShell.Cmdletization.Xml.ObsoleteAttributeMetadata::", new ObsoleteAttributeMetadataSerializer());
					hashtable.Add("Microsoft.PowerShell.Cmdletization.Xml.InstanceCmdletMetadata::", new InstanceCmdletMetadataSerializer());
					hashtable.Add("Microsoft.PowerShell.Cmdletization.Xml.CommonMethodMetadataReturnValue::", new CommonMethodMetadataReturnValueSerializer());
					hashtable.Add("Microsoft.PowerShell.Cmdletization.Xml.PropertyMetadata::", new PropertyMetadataSerializer());
					hashtable.Add("Microsoft.PowerShell.Cmdletization.Xml.CmdletParameterMetadataForGetCmdletParameter::", new CmdletParameterMetadataForGetCmdletParameterSerializer());
					hashtable.Add("Microsoft.PowerShell.Cmdletization.Xml.CmdletOutputMetadata::", new CmdletOutputMetadataSerializer());
					hashtable.Add("Microsoft.PowerShell.Cmdletization.Xml.EnumMetadataEnum::", new EnumMetadataEnumSerializer());
					hashtable.Add("Microsoft.PowerShell.Cmdletization.Xml.QueryOption::", new QueryOptionSerializer());
					hashtable.Add("Microsoft.PowerShell.Cmdletization.Xml.InstanceMethodParameterMetadata::", new InstanceMethodParameterMetadataSerializer());
					hashtable.Add("Microsoft.PowerShell.Cmdletization.Xml.CmdletParameterMetadataValidateRange::", new CmdletParameterMetadataValidateRangeSerializer());
					hashtable.Add("Microsoft.PowerShell.Cmdletization.Xml.ClassMetadataData::", new ClassMetadataDataSerializer());
					hashtable.Add("Microsoft.PowerShell.Cmdletization.Xml.ConfirmImpact::", new ConfirmImpactSerializer());
					hashtable.Add("Microsoft.PowerShell.Cmdletization.Xml.StaticCmdletMetadataCmdletMetadata::", new StaticCmdletMetadataCmdletMetadataSerializer());
					hashtable.Add("Microsoft.PowerShell.Cmdletization.Xml.GetCmdletMetadata::", new GetCmdletMetadataSerializer());
					hashtable.Add("Microsoft.PowerShell.Cmdletization.Xml.CmdletParameterMetadataValidateLength::", new CmdletParameterMetadataValidateLengthSerializer());
					hashtable.Add("Microsoft.PowerShell.Cmdletization.Xml.InstanceMethodMetadata::", new InstanceMethodMetadataSerializer());
					hashtable.Add("Microsoft.PowerShell.Cmdletization.Xml.CommonMethodMetadata::", new CommonMethodMetadataSerializer());
					hashtable.Add("Microsoft.PowerShell.Cmdletization.Xml.CmdletParameterMetadataValidateCount::", new CmdletParameterMetadataValidateCountSerializer());
					hashtable.Add("Microsoft.PowerShell.Cmdletization.Xml.GetCmdletParameters::", new GetCmdletParametersSerializer());
					hashtable.Add("Microsoft.PowerShell.Cmdletization.Xml.CmdletParameterMetadataForInstanceMethodParameter::", new CmdletParameterMetadataForInstanceMethodParameterSerializer());
					hashtable.Add("Microsoft.PowerShell.Cmdletization.Xml.CommonCmdletMetadata::", new CommonCmdletMetadataSerializer());
					hashtable.Add("Microsoft.PowerShell.Cmdletization.Xml.TypeMetadata::", new TypeMetadataSerializer());
					hashtable.Add("Microsoft.PowerShell.Cmdletization.Xml.CmdletParameterMetadataForGetCmdletFilteringParameter::", new CmdletParameterMetadataForGetCmdletFilteringParameterSerializer());
					hashtable.Add("Microsoft.PowerShell.Cmdletization.Xml.StaticMethodParameterMetadata::", new StaticMethodParameterMetadataSerializer());
					hashtable.Add("Microsoft.PowerShell.Cmdletization.Xml.CmdletParameterMetadataForStaticMethodParameter::", new CmdletParameterMetadataForStaticMethodParameterSerializer());
					hashtable.Add("Microsoft.PowerShell.Cmdletization.Xml.ClassMetadata::", new ClassMetadataSerializer());
					hashtable.Add("Microsoft.PowerShell.Cmdletization.Xml.WildcardablePropertyQuery::", new WildcardablePropertyQuerySerializer());
					if (this.typedSerializers == null)
					{
						this.typedSerializers = hashtable;
					}
				}
				return this.typedSerializers;
			}
		}

		// Token: 0x06005D85 RID: 23941 RVA: 0x001FDD14 File Offset: 0x001FBF14
		public override bool CanSerialize(Type type)
		{
			return type == typeof(PowerShellMetadata) || type == typeof(ClassMetadata) || type == typeof(ClassMetadataInstanceCmdlets) || type == typeof(GetCmdletParameters) || type == typeof(PropertyMetadata) || type == typeof(TypeMetadata) || type == typeof(Association) || type == typeof(AssociationAssociatedInstance) || type == typeof(CmdletParameterMetadata) || type == typeof(CmdletParameterMetadataForGetCmdletParameter) || type == typeof(CmdletParameterMetadataForGetCmdletFilteringParameter) || type == typeof(CmdletParameterMetadataValidateCount) || type == typeof(CmdletParameterMetadataValidateLength) || type == typeof(CmdletParameterMetadataValidateRange) || type == typeof(ObsoleteAttributeMetadata) || type == typeof(CmdletParameterMetadataForInstanceMethodParameter) || type == typeof(CmdletParameterMetadataForStaticMethodParameter) || type == typeof(QueryOption) || type == typeof(GetCmdletMetadata) || type == typeof(CommonCmdletMetadata) || type == typeof(ConfirmImpact) || type == typeof(StaticCmdletMetadata) || type == typeof(StaticCmdletMetadataCmdletMetadata) || type == typeof(CommonMethodMetadata) || type == typeof(StaticMethodMetadata) || type == typeof(CommonMethodParameterMetadata) || type == typeof(StaticMethodParameterMetadata) || type == typeof(CmdletOutputMetadata) || type == typeof(InstanceMethodParameterMetadata) || type == typeof(CommonMethodMetadataReturnValue) || type == typeof(InstanceMethodMetadata) || type == typeof(InstanceCmdletMetadata) || type == typeof(PropertyQuery) || type == typeof(WildcardablePropertyQuery) || type == typeof(ItemsChoiceType) || type == typeof(ClassMetadataData) || type == typeof(EnumMetadataEnum) || type == typeof(EnumMetadataEnumValue);
		}

		// Token: 0x06005D86 RID: 23942 RVA: 0x001FE01C File Offset: 0x001FC21C
		public override XmlSerializer GetSerializer(Type type)
		{
			if (type == typeof(PowerShellMetadata))
			{
				return new PowerShellMetadataSerializer();
			}
			if (type == typeof(ClassMetadata))
			{
				return new ClassMetadataSerializer();
			}
			if (type == typeof(ClassMetadataInstanceCmdlets))
			{
				return new ClassMetadataInstanceCmdletsSerializer();
			}
			if (type == typeof(GetCmdletParameters))
			{
				return new GetCmdletParametersSerializer();
			}
			if (type == typeof(PropertyMetadata))
			{
				return new PropertyMetadataSerializer();
			}
			if (type == typeof(TypeMetadata))
			{
				return new TypeMetadataSerializer();
			}
			if (type == typeof(Association))
			{
				return new AssociationSerializer();
			}
			if (type == typeof(AssociationAssociatedInstance))
			{
				return new AssociationAssociatedInstanceSerializer();
			}
			if (type == typeof(CmdletParameterMetadata))
			{
				return new CmdletParameterMetadataSerializer();
			}
			if (type == typeof(CmdletParameterMetadataForGetCmdletParameter))
			{
				return new CmdletParameterMetadataForGetCmdletParameterSerializer();
			}
			if (type == typeof(CmdletParameterMetadataForGetCmdletFilteringParameter))
			{
				return new CmdletParameterMetadataForGetCmdletFilteringParameterSerializer();
			}
			if (type == typeof(CmdletParameterMetadataValidateCount))
			{
				return new CmdletParameterMetadataValidateCountSerializer();
			}
			if (type == typeof(CmdletParameterMetadataValidateLength))
			{
				return new CmdletParameterMetadataValidateLengthSerializer();
			}
			if (type == typeof(CmdletParameterMetadataValidateRange))
			{
				return new CmdletParameterMetadataValidateRangeSerializer();
			}
			if (type == typeof(ObsoleteAttributeMetadata))
			{
				return new ObsoleteAttributeMetadataSerializer();
			}
			if (type == typeof(CmdletParameterMetadataForInstanceMethodParameter))
			{
				return new CmdletParameterMetadataForInstanceMethodParameterSerializer();
			}
			if (type == typeof(CmdletParameterMetadataForStaticMethodParameter))
			{
				return new CmdletParameterMetadataForStaticMethodParameterSerializer();
			}
			if (type == typeof(QueryOption))
			{
				return new QueryOptionSerializer();
			}
			if (type == typeof(GetCmdletMetadata))
			{
				return new GetCmdletMetadataSerializer();
			}
			if (type == typeof(CommonCmdletMetadata))
			{
				return new CommonCmdletMetadataSerializer();
			}
			if (type == typeof(ConfirmImpact))
			{
				return new ConfirmImpactSerializer();
			}
			if (type == typeof(StaticCmdletMetadata))
			{
				return new StaticCmdletMetadataSerializer();
			}
			if (type == typeof(StaticCmdletMetadataCmdletMetadata))
			{
				return new StaticCmdletMetadataCmdletMetadataSerializer();
			}
			if (type == typeof(CommonMethodMetadata))
			{
				return new CommonMethodMetadataSerializer();
			}
			if (type == typeof(StaticMethodMetadata))
			{
				return new StaticMethodMetadataSerializer();
			}
			if (type == typeof(CommonMethodParameterMetadata))
			{
				return new CommonMethodParameterMetadataSerializer();
			}
			if (type == typeof(StaticMethodParameterMetadata))
			{
				return new StaticMethodParameterMetadataSerializer();
			}
			if (type == typeof(CmdletOutputMetadata))
			{
				return new CmdletOutputMetadataSerializer();
			}
			if (type == typeof(InstanceMethodParameterMetadata))
			{
				return new InstanceMethodParameterMetadataSerializer();
			}
			if (type == typeof(CommonMethodMetadataReturnValue))
			{
				return new CommonMethodMetadataReturnValueSerializer();
			}
			if (type == typeof(InstanceMethodMetadata))
			{
				return new InstanceMethodMetadataSerializer();
			}
			if (type == typeof(InstanceCmdletMetadata))
			{
				return new InstanceCmdletMetadataSerializer();
			}
			if (type == typeof(PropertyQuery))
			{
				return new PropertyQuerySerializer();
			}
			if (type == typeof(WildcardablePropertyQuery))
			{
				return new WildcardablePropertyQuerySerializer();
			}
			if (type == typeof(ItemsChoiceType))
			{
				return new ItemsChoiceTypeSerializer();
			}
			if (type == typeof(ClassMetadataData))
			{
				return new ClassMetadataDataSerializer();
			}
			if (type == typeof(EnumMetadataEnum))
			{
				return new EnumMetadataEnumSerializer();
			}
			if (type == typeof(EnumMetadataEnumValue))
			{
				return new EnumMetadataEnumValueSerializer();
			}
			return null;
		}

		// Token: 0x040031CA RID: 12746
		private Hashtable readMethods;

		// Token: 0x040031CB RID: 12747
		private Hashtable writeMethods;

		// Token: 0x040031CC RID: 12748
		private Hashtable typedSerializers;
	}
}
