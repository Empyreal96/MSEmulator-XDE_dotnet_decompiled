using System;
using System.Collections.Generic;
using System.Reflection;
using Microsoft.PowerShell;

namespace System.Management.Automation.Runspaces
{
	// Token: 0x02000168 RID: 360
	internal sealed class TypesV3_Ps1Xml
	{
		// Token: 0x0600126F RID: 4719 RVA: 0x00073528 File Offset: 0x00071728
		private static MethodInfo GetMethodInfo(string typeName, string method)
		{
			Type type = LanguagePrimitives.ConvertTo<Type>(typeName);
			return TypesV3_Ps1Xml.GetMethodInfo(type, method);
		}

		// Token: 0x06001270 RID: 4720 RVA: 0x00073543 File Offset: 0x00071743
		private static MethodInfo GetMethodInfo(Type type, string method)
		{
			return type.GetMethod(method, BindingFlags.IgnoreCase | BindingFlags.Static | BindingFlags.Public);
		}

		// Token: 0x06001271 RID: 4721 RVA: 0x00073550 File Offset: 0x00071750
		private static ScriptBlock GetScriptBlock(string s)
		{
			ScriptBlock scriptBlock = ScriptBlock.CreateDelayParsedScriptBlock(s);
			scriptBlock.LanguageMode = new PSLanguageMode?(PSLanguageMode.FullLanguage);
			return scriptBlock;
		}

		// Token: 0x06001272 RID: 4722 RVA: 0x00073B50 File Offset: 0x00071D50
		public static IEnumerable<TypeData> Get()
		{
			yield return new TypeData("System.Security.Cryptography.X509Certificates.X509Certificate2", true)
			{
				Members = 
				{
					{
						"EnhancedKeyUsageList",
						new ScriptPropertyData("EnhancedKeyUsageList", TypesV3_Ps1Xml.GetScriptBlock(",(new-object Microsoft.Powershell.Commands.EnhancedKeyUsageProperty -argumentlist $this).EnhancedKeyUsageList;"), null)
					},
					{
						"DnsNameList",
						new ScriptPropertyData("DnsNameList", TypesV3_Ps1Xml.GetScriptBlock(",(new-object Microsoft.Powershell.Commands.DnsNameProperty -argumentlist $this).DnsNameList;"), null)
					},
					{
						"SendAsTrustedIssuer",
						new ScriptPropertyData("SendAsTrustedIssuer", TypesV3_Ps1Xml.GetScriptBlock("[Microsoft.Powershell.Commands.SendAsTrustedIssuerProperty]::ReadSendAsTrustedIssuerProperty($this)"), TypesV3_Ps1Xml.GetScriptBlock("$sendAsTrustedIssuer = $args[0]\r\n                    [Microsoft.Powershell.Commands.SendAsTrustedIssuerProperty]::WriteSendAsTrustedIssuerProperty($this,$this.PsPath,$sendAsTrustedIssuer)"))
					}
				}
			};
			yield return new TypeData("System.Management.Automation.Remoting.PSSenderInfo", true)
			{
				Members = 
				{
					{
						"ConnectedUser",
						new ScriptPropertyData("ConnectedUser", TypesV3_Ps1Xml.GetScriptBlock("$this.UserInfo.Identity.Name"), null)
					},
					{
						"RunAsUser",
						new ScriptPropertyData("RunAsUser", TypesV3_Ps1Xml.GetScriptBlock("if($this.UserInfo.WindowsIdentity -ne $null)\r\n\t\t\t{\r\n\t\t\t\t$this.UserInfo.WindowsIdentity.Name\r\n\t\t\t}"), null)
					}
				}
			};
			yield return new TypeData("System.Management.Automation.CompletionResult", true)
			{
				SerializationDepth = 1U
			};
			yield return new TypeData("Deserialized.System.Management.Automation.CompletionResult", true)
			{
				TargetTypeForDeserialization = typeof(DeserializingTypeConverter)
			};
			yield return new TypeData("System.Management.Automation.CommandCompletion", true)
			{
				SerializationDepth = 1U
			};
			yield return new TypeData("Deserialized.System.Management.Automation.CommandCompletion", true)
			{
				TargetTypeForDeserialization = typeof(DeserializingTypeConverter)
			};
			yield return new TypeData("Microsoft.PowerShell.Commands.ModuleSpecification", true)
			{
				SerializationDepth = 1U
			};
			yield return new TypeData("Deserialized.Microsoft.PowerShell.Commands.ModuleSpecification", true)
			{
				TargetTypeForDeserialization = typeof(DeserializingTypeConverter)
			};
			yield return new TypeData("System.Management.Automation.JobStateEventArgs", true)
			{
				SerializationDepth = 2U
			};
			yield return new TypeData("Deserialized.System.Management.Automation.JobStateEventArgs", true)
			{
				TargetTypeForDeserialization = typeof(DeserializingTypeConverter)
			};
			yield return new TypeData("System.Exception", true)
			{
				SerializationDepth = 1U
			};
			yield return new TypeData("System.Management.Automation.Remoting.PSSessionOption", true)
			{
				SerializationDepth = 1U
			};
			yield return new TypeData("Deserialized.System.Management.Automation.Remoting.PSSessionOption", true)
			{
				TargetTypeForDeserialization = typeof(DeserializingTypeConverter)
			};
			yield return new TypeData("System.Management.Automation.DebuggerStopEventArgs", true)
			{
				Members = 
				{
					{
						"SerializedInvocationInfo",
						new CodePropertyData("SerializedInvocationInfo", TypesV3_Ps1Xml.GetMethodInfo(typeof(DeserializingTypeConverter), "GetInvocationInfo"), null)
						{
							IsHidden = true
						}
					}
				},
				SerializationMethod = "SpecificProperties",
				SerializationDepth = 2U,
				PropertySerializationSet = new PropertySetData(new string[]
				{
					"Breakpoints",
					"ResumeAction",
					"SerializedInvocationInfo"
				})
				{
					Name = "PropertySerializationSet"
				}
			};
			yield return new TypeData("Deserialized.System.Management.Automation.DebuggerStopEventArgs", true)
			{
				TargetTypeForDeserialization = typeof(DeserializingTypeConverter)
			};
			yield break;
		}
	}
}
