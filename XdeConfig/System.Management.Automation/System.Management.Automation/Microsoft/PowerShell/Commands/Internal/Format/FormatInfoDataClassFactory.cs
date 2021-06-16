using System;
using System.Collections;
using System.Management.Automation;
using System.Management.Automation.Internal;
using System.Reflection;
using System.Runtime.InteropServices;

namespace Microsoft.PowerShell.Commands.Internal.Format
{
	// Token: 0x020004D8 RID: 1240
	internal static class FormatInfoDataClassFactory
	{
		// Token: 0x06003621 RID: 13857 RVA: 0x00125858 File Offset: 0x00123A58
		static FormatInfoDataClassFactory()
		{
			FormatInfoDataClassFactory.constructors.Add("033ecb2bc07a4d43b5ef94ed5a35d280", typeof(FormatStartData));
			FormatInfoDataClassFactory.constructors.Add("cf522b78d86c486691226b40aa69e95c", typeof(FormatEndData));
			FormatInfoDataClassFactory.constructors.Add("9e210fe47d09416682b841769c78b8a3", typeof(GroupStartData));
			FormatInfoDataClassFactory.constructors.Add("4ec4f0187cb04f4cb6973460dfe252df", typeof(GroupEndData));
			FormatInfoDataClassFactory.constructors.Add("27c87ef9bbda4f709f6b4002fa4af63c", typeof(FormatEntryData));
			FormatInfoDataClassFactory.constructors.Add("b2e2775d33d544c794d0081f27021b5c", typeof(WideViewHeaderInfo));
			FormatInfoDataClassFactory.constructors.Add("e3b7a39c089845d388b2e84c5d38f5dd", typeof(TableHeaderInfo));
			FormatInfoDataClassFactory.constructors.Add("7572aa4155ec4558817a615acf7dd92e", typeof(TableColumnInfo));
			FormatInfoDataClassFactory.constructors.Add("830bdcb24c1642258724e441512233a4", typeof(ListViewHeaderInfo));
			FormatInfoDataClassFactory.constructors.Add("cf58f450baa848ef8eb3504008be6978", typeof(ListViewEntry));
			FormatInfoDataClassFactory.constructors.Add("b761477330ce4fb2a665999879324d73", typeof(ListViewField));
			FormatInfoDataClassFactory.constructors.Add("0e59526e2dd441aa91e7fc952caf4a36", typeof(TableRowEntry));
			FormatInfoDataClassFactory.constructors.Add("59bf79de63354a7b9e4d1697940ff188", typeof(WideViewEntry));
			FormatInfoDataClassFactory.constructors.Add("5197dd85ca6f4cce9ae9e6fd6ded9d76", typeof(ComplexViewHeaderInfo));
			FormatInfoDataClassFactory.constructors.Add("22e7ef3c896449d4a6f2dedea05dd737", typeof(ComplexViewEntry));
			FormatInfoDataClassFactory.constructors.Add("919820b7eadb48be8e202c5afa5c2716", typeof(GroupingEntry));
			FormatInfoDataClassFactory.constructors.Add("dd1290a5950b4b27aa76d9f06199c3b3", typeof(PageHeaderEntry));
			FormatInfoDataClassFactory.constructors.Add("93565e84730645c79d4af091123eecbc", typeof(PageFooterEntry));
			FormatInfoDataClassFactory.constructors.Add("a27f094f0eec4d64845801a4c06a32ae", typeof(AutosizeInfo));
			FormatInfoDataClassFactory.constructors.Add("de7e8b96fbd84db5a43aa82eb34580ec", typeof(FormatNewLine));
			FormatInfoDataClassFactory.constructors.Add("091C9E762E33499eBE318901B6EFB733", typeof(FrameInfo));
			FormatInfoDataClassFactory.constructors.Add("b8d9e369024a43a580b9e0c9279e3354", typeof(FormatTextField));
			FormatInfoDataClassFactory.constructors.Add("78b102e894f742aca8c1d6737b6ff86a", typeof(FormatPropertyField));
			FormatInfoDataClassFactory.constructors.Add("fba029a113a5458d932a2ed4871fadf2", typeof(FormatEntry));
			FormatInfoDataClassFactory.constructors.Add("29ED81BA914544d4BC430F027EE053E9", typeof(RawTextFormatEntry));
		}

		// Token: 0x06003622 RID: 13858 RVA: 0x00125AF4 File Offset: 0x00123CF4
		internal static FormatInfoData CreateInstance(PSObject so, FormatObjectDeserializer deserializer)
		{
			if (so == null)
			{
				throw PSTraceSource.NewArgumentNullException("so");
			}
			string text = FormatObjectDeserializer.GetProperty(so, "ClassId2e4f51ef21dd47e99d3c952918aff9cd") as string;
			if (text == null)
			{
				string message = StringUtil.Format(FormatAndOut_format_xxx.FOD_InvalidClassidProperty, new object[0]);
				ErrorRecord errorRecord = new ErrorRecord(PSTraceSource.NewArgumentException("classid"), "FormatObjectDeserializerInvalidClassidProperty", ErrorCategory.InvalidData, so);
				errorRecord.ErrorDetails = new ErrorDetails(message);
				deserializer.TerminatingErrorContext.ThrowTerminatingError(errorRecord);
			}
			return FormatInfoDataClassFactory.CreateInstance(text, deserializer);
		}

		// Token: 0x06003623 RID: 13859 RVA: 0x00125B6C File Offset: 0x00123D6C
		private static FormatInfoData CreateInstance(string clsid, FormatObjectDeserializer deserializer)
		{
			Type type = FormatInfoDataClassFactory.GetType(clsid);
			if (null == type)
			{
				FormatInfoDataClassFactory.CreateInstanceError(PSTraceSource.NewArgumentException("clsid"), clsid, deserializer);
				return null;
			}
			try
			{
				return (FormatInfoData)Activator.CreateInstance(type);
			}
			catch (ArgumentException e)
			{
				FormatInfoDataClassFactory.CreateInstanceError(e, clsid, deserializer);
			}
			catch (NotSupportedException e2)
			{
				FormatInfoDataClassFactory.CreateInstanceError(e2, clsid, deserializer);
			}
			catch (TargetInvocationException e3)
			{
				FormatInfoDataClassFactory.CreateInstanceError(e3, clsid, deserializer);
			}
			catch (MemberAccessException e4)
			{
				FormatInfoDataClassFactory.CreateInstanceError(e4, clsid, deserializer);
			}
			catch (InvalidComObjectException e5)
			{
				FormatInfoDataClassFactory.CreateInstanceError(e5, clsid, deserializer);
			}
			catch (COMException e6)
			{
				FormatInfoDataClassFactory.CreateInstanceError(e6, clsid, deserializer);
			}
			catch (TypeLoadException e7)
			{
				FormatInfoDataClassFactory.CreateInstanceError(e7, clsid, deserializer);
			}
			catch (Exception)
			{
				throw;
			}
			return null;
		}

		// Token: 0x06003624 RID: 13860 RVA: 0x00125C70 File Offset: 0x00123E70
		private static Type GetType(string clsid)
		{
			object obj = FormatInfoDataClassFactory.constructors[clsid];
			return obj as Type;
		}

		// Token: 0x06003625 RID: 13861 RVA: 0x00125C90 File Offset: 0x00123E90
		private static void CreateInstanceError(Exception e, string clsid, FormatObjectDeserializer deserializer)
		{
			string message = StringUtil.Format(FormatAndOut_format_xxx.FOD_InvalidClassid, clsid);
			ErrorRecord errorRecord = new ErrorRecord(e, "FormatObjectDeserializerInvalidClassid", ErrorCategory.InvalidData, null);
			errorRecord.ErrorDetails = new ErrorDetails(message);
			deserializer.TerminatingErrorContext.ThrowTerminatingError(errorRecord);
		}

		// Token: 0x04001B96 RID: 7062
		[TraceSource("FormatInfoDataClassFactory", "FormatInfoDataClassFactory")]
		internal static PSTraceSource tracer = PSTraceSource.GetTracer("FormatInfoDataClassFactory", "FormatInfoDataClassFactory");

		// Token: 0x04001B97 RID: 7063
		private static readonly Hashtable constructors = new Hashtable();
	}
}
