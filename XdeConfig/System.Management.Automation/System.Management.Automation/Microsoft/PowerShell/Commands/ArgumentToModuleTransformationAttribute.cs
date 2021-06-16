using System;
using System.Collections;
using System.IO;
using System.Management.Automation;

namespace Microsoft.PowerShell.Commands
{
	// Token: 0x020001E4 RID: 484
	internal sealed class ArgumentToModuleTransformationAttribute : ArgumentTransformationAttribute
	{
		// Token: 0x0600162C RID: 5676 RVA: 0x0008D658 File Offset: 0x0008B858
		public override object Transform(EngineIntrinsics engineIntrinsics, object inputData)
		{
			object obj = PSObject.Base(inputData);
			string text = obj as string;
			if (text != null)
			{
				return new PSModuleInfo(text, null, null, null);
			}
			IList ilist = ParameterBinderBase.GetIList(obj);
			if (ilist != null && ilist.Count > 0)
			{
				int count = ilist.Count;
				int num = 0;
				Array array = Array.CreateInstance(typeof(object), count);
				foreach (object obj2 in ilist)
				{
					object obj3 = PSObject.Base(obj2);
					if (obj3 is PSModuleInfo)
					{
						array.SetValue(obj3, num++);
					}
					else if (obj3 is string)
					{
						PSModuleInfo value = new PSModuleInfo((string)obj3, null, null, null);
						array.SetValue(value, num++);
					}
					else
					{
						PSModuleInfo value2 = null;
						if (this.TryConvertFromDeserializedModuleInfo(obj3, out value2))
						{
							array.SetValue(value2, num++);
						}
						else
						{
							array.SetValue(obj2, num++);
						}
					}
				}
				return array;
			}
			PSModuleInfo result = null;
			if (this.TryConvertFromDeserializedModuleInfo(inputData, out result))
			{
				return result;
			}
			return inputData;
		}

		// Token: 0x0600162D RID: 5677 RVA: 0x0008D794 File Offset: 0x0008B994
		private bool TryConvertFromDeserializedModuleInfo(object inputData, out PSModuleInfo moduleInfo)
		{
			moduleInfo = null;
			PSObject psobject = inputData as PSObject;
			if (Deserializer.IsDeserializedInstanceOfType(psobject, typeof(PSModuleInfo)))
			{
				string name;
				LanguagePrimitives.TryConvertTo<string>(psobject.Properties["Name"].Value, out name);
				Guid guid;
				LanguagePrimitives.TryConvertTo<Guid>(psobject.Properties["Guid"].Value, out guid);
				Version version;
				LanguagePrimitives.TryConvertTo<Version>(psobject.Properties["Version"].Value, out version);
				string helpInfoUri;
				LanguagePrimitives.TryConvertTo<string>(psobject.Properties["HelpInfoUri"].Value, out helpInfoUri);
				moduleInfo = new PSModuleInfo(name, null, null, null);
				moduleInfo.SetGuid(guid);
				moduleInfo.SetVersion(version);
				moduleInfo.SetHelpInfoUri(helpInfoUri);
				moduleInfo.SetModuleBase(Path.GetTempPath());
				return true;
			}
			return false;
		}
	}
}
