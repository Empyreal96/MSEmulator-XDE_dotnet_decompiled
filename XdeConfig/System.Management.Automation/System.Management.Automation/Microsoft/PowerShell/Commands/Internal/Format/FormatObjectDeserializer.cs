using System;
using System.Management.Automation;
using System.Management.Automation.Internal;

namespace Microsoft.PowerShell.Commands.Internal.Format
{
	// Token: 0x020004D7 RID: 1239
	internal sealed class FormatObjectDeserializer
	{
		// Token: 0x17000C14 RID: 3092
		// (get) Token: 0x0600360F RID: 13839 RVA: 0x001253EA File Offset: 0x001235EA
		internal TerminatingErrorContext TerminatingErrorContext
		{
			get
			{
				return this._errorContext;
			}
		}

		// Token: 0x06003610 RID: 13840 RVA: 0x001253F2 File Offset: 0x001235F2
		internal FormatObjectDeserializer(TerminatingErrorContext errorContext)
		{
			this._errorContext = errorContext;
		}

		// Token: 0x06003611 RID: 13841 RVA: 0x00125404 File Offset: 0x00123604
		internal bool IsFormatInfoData(PSObject so)
		{
			if (this._useBaseObject)
			{
				object baseObject = so.BaseObject;
				if (baseObject != null && baseObject is PacketInfoData)
				{
					return true;
				}
			}
			if (!Deserializer.IsInstanceOfType(so, typeof(FormatInfoData)))
			{
				return false;
			}
			string text = FormatObjectDeserializer.GetProperty(so, "ClassId2e4f51ef21dd47e99d3c952918aff9cd") as string;
			if (text == null)
			{
				return false;
			}
			if (FormatObjectDeserializer.IsClass(text, "033ecb2bc07a4d43b5ef94ed5a35d280"))
			{
				return true;
			}
			if (FormatObjectDeserializer.IsClass(text, "cf522b78d86c486691226b40aa69e95c"))
			{
				return true;
			}
			if (FormatObjectDeserializer.IsClass(text, "9e210fe47d09416682b841769c78b8a3"))
			{
				return true;
			}
			if (FormatObjectDeserializer.IsClass(text, "4ec4f0187cb04f4cb6973460dfe252df"))
			{
				return true;
			}
			if (FormatObjectDeserializer.IsClass(text, "27c87ef9bbda4f709f6b4002fa4af63c"))
			{
				return true;
			}
			this.ProcessUnknownInvalidClassId(text, so, "FormatObjectDeserializerIsFormatInfoDataInvalidClassId");
			return false;
		}

		// Token: 0x06003612 RID: 13842 RVA: 0x001254B0 File Offset: 0x001236B0
		internal object Deserialize(PSObject so)
		{
			if (this._useBaseObject)
			{
				object baseObject = so.BaseObject;
				if (baseObject != null && baseObject is PacketInfoData)
				{
					return baseObject;
				}
			}
			if (!Deserializer.IsInstanceOfType(so, typeof(FormatInfoData)))
			{
				return so;
			}
			string text = FormatObjectDeserializer.GetProperty(so, "ClassId2e4f51ef21dd47e99d3c952918aff9cd") as string;
			if (text == null)
			{
				return so;
			}
			if (FormatObjectDeserializer.IsClass(text, "033ecb2bc07a4d43b5ef94ed5a35d280"))
			{
				return this.DeserializeObject(so);
			}
			if (FormatObjectDeserializer.IsClass(text, "cf522b78d86c486691226b40aa69e95c"))
			{
				return this.DeserializeObject(so);
			}
			if (FormatObjectDeserializer.IsClass(text, "9e210fe47d09416682b841769c78b8a3"))
			{
				return this.DeserializeObject(so);
			}
			if (FormatObjectDeserializer.IsClass(text, "4ec4f0187cb04f4cb6973460dfe252df"))
			{
				return this.DeserializeObject(so);
			}
			if (FormatObjectDeserializer.IsClass(text, "27c87ef9bbda4f709f6b4002fa4af63c"))
			{
				return this.DeserializeObject(so);
			}
			this.ProcessUnknownInvalidClassId(text, so, "FormatObjectDeserializerDeserializeInvalidClassId");
			return null;
		}

		// Token: 0x06003613 RID: 13843 RVA: 0x0012557C File Offset: 0x0012377C
		private void ProcessUnknownInvalidClassId(string classId, object obj, string errorId)
		{
			string message = StringUtil.Format(FormatAndOut_format_xxx.FOD_ClassIdInvalid, classId);
			ErrorRecord errorRecord = new ErrorRecord(PSTraceSource.NewArgumentException("classId"), errorId, ErrorCategory.InvalidData, obj);
			errorRecord.ErrorDetails = new ErrorDetails(message);
			this.TerminatingErrorContext.ThrowTerminatingError(errorRecord);
		}

		// Token: 0x06003614 RID: 13844 RVA: 0x001255C0 File Offset: 0x001237C0
		private static bool IsClass(string x, string y)
		{
			return string.Compare(x, y, StringComparison.OrdinalIgnoreCase) == 0;
		}

		// Token: 0x06003615 RID: 13845 RVA: 0x001255D0 File Offset: 0x001237D0
		internal static object GetProperty(PSObject so, string name)
		{
			PSMemberInfo psmemberInfo = so.Properties[name];
			if (psmemberInfo == null)
			{
				return null;
			}
			return psmemberInfo.Value;
		}

		// Token: 0x06003616 RID: 13846 RVA: 0x001255F8 File Offset: 0x001237F8
		internal FormatInfoData DeserializeMemberObject(PSObject so, string property)
		{
			object property2 = FormatObjectDeserializer.GetProperty(so, property);
			if (property2 == null)
			{
				return null;
			}
			if (so == property2)
			{
				string message = StringUtil.Format(FormatAndOut_format_xxx.FOD_RecursiveProperty, property);
				ErrorRecord errorRecord = new ErrorRecord(PSTraceSource.NewArgumentException("property"), "FormatObjectDeserializerRecursiveProperty", ErrorCategory.InvalidData, so);
				errorRecord.ErrorDetails = new ErrorDetails(message);
				this.TerminatingErrorContext.ThrowTerminatingError(errorRecord);
			}
			return this.DeserializeObject(PSObject.AsPSObject(property2));
		}

		// Token: 0x06003617 RID: 13847 RVA: 0x00125660 File Offset: 0x00123860
		internal FormatInfoData DeserializeMandatoryMemberObject(PSObject so, string property)
		{
			FormatInfoData formatInfoData = this.DeserializeMemberObject(so, property);
			this.VerifyDataNotNull(formatInfoData, property);
			return formatInfoData;
		}

		// Token: 0x06003618 RID: 13848 RVA: 0x00125680 File Offset: 0x00123880
		private object DeserializeMemberVariable(PSObject so, string property, Type t, bool cannotBeNull)
		{
			object property2 = FormatObjectDeserializer.GetProperty(so, property);
			if (cannotBeNull)
			{
				this.VerifyDataNotNull(property2, property);
			}
			if (property2 != null && t != property2.GetType())
			{
				string message = StringUtil.Format(FormatAndOut_format_xxx.FOD_InvalidPropertyType, t.Name, property);
				ErrorRecord errorRecord = new ErrorRecord(PSTraceSource.NewArgumentException("property"), "FormatObjectDeserializerInvalidPropertyType", ErrorCategory.InvalidData, so);
				errorRecord.ErrorDetails = new ErrorDetails(message);
				this.TerminatingErrorContext.ThrowTerminatingError(errorRecord);
			}
			return property2;
		}

		// Token: 0x06003619 RID: 13849 RVA: 0x001256F4 File Offset: 0x001238F4
		internal string DeserializeStringMemberVariableRaw(PSObject so, string property)
		{
			return (string)this.DeserializeMemberVariable(so, property, typeof(string), false);
		}

		// Token: 0x0600361A RID: 13850 RVA: 0x00125710 File Offset: 0x00123910
		internal string DeserializeStringMemberVariable(PSObject so, string property)
		{
			string text = (string)this.DeserializeMemberVariable(so, property, typeof(string), false);
			if (string.IsNullOrEmpty(text))
			{
				return text;
			}
			return text.Replace("\t", "    ");
		}

		// Token: 0x0600361B RID: 13851 RVA: 0x00125750 File Offset: 0x00123950
		internal int DeserializeIntMemberVariable(PSObject so, string property)
		{
			return (int)this.DeserializeMemberVariable(so, property, typeof(int), true);
		}

		// Token: 0x0600361C RID: 13852 RVA: 0x0012576A File Offset: 0x0012396A
		internal bool DeserializeBoolMemberVariable(PSObject so, string property)
		{
			return (bool)this.DeserializeMemberVariable(so, property, typeof(bool), true);
		}

		// Token: 0x0600361D RID: 13853 RVA: 0x00125784 File Offset: 0x00123984
		internal WriteStreamType DeserializeWriteStreamTypeMemberVariable(PSObject so)
		{
			object property = FormatObjectDeserializer.GetProperty(so, "writeStream");
			if (property == null)
			{
				return WriteStreamType.None;
			}
			WriteStreamType result;
			if (property is WriteStreamType)
			{
				result = (WriteStreamType)property;
			}
			else if (property is string)
			{
				if (!Enum.TryParse<WriteStreamType>(property as string, true, out result))
				{
					result = WriteStreamType.None;
				}
			}
			else
			{
				result = WriteStreamType.None;
			}
			return result;
		}

		// Token: 0x0600361E RID: 13854 RVA: 0x001257D4 File Offset: 0x001239D4
		internal FormatInfoData DeserializeObject(PSObject so)
		{
			FormatInfoData formatInfoData = FormatInfoDataClassFactory.CreateInstance(so, this);
			if (formatInfoData != null)
			{
				formatInfoData.Deserialize(so, this);
			}
			return formatInfoData;
		}

		// Token: 0x0600361F RID: 13855 RVA: 0x001257F8 File Offset: 0x001239F8
		internal void VerifyDataNotNull(object obj, string name)
		{
			if (obj != null)
			{
				return;
			}
			string message = StringUtil.Format(FormatAndOut_format_xxx.FOD_NullDataMember, name);
			ErrorRecord errorRecord = new ErrorRecord(new ArgumentException(), "FormatObjectDeserializerNullDataMember", ErrorCategory.InvalidData, null);
			errorRecord.ErrorDetails = new ErrorDetails(message);
			this.TerminatingErrorContext.ThrowTerminatingError(errorRecord);
		}

		// Token: 0x04001B92 RID: 7058
		private const string TabExpansionString = "    ";

		// Token: 0x04001B93 RID: 7059
		[TraceSource("FormatObjectDeserializer", "FormatObjectDeserializer")]
		internal static PSTraceSource tracer = PSTraceSource.GetTracer("FormatObjectDeserializer", "class to deserialize property bags into formatting objects");

		// Token: 0x04001B94 RID: 7060
		private bool _useBaseObject;

		// Token: 0x04001B95 RID: 7061
		private TerminatingErrorContext _errorContext;
	}
}
