using System;
using System.Collections.Generic;
using System.Reflection;

namespace System.Management.Automation.Runspaces
{
	// Token: 0x02000169 RID: 361
	internal sealed class GetEvent_Types_Ps1Xml
	{
		// Token: 0x06001274 RID: 4724 RVA: 0x00073B70 File Offset: 0x00071D70
		private static MethodInfo GetMethodInfo(string typeName, string method)
		{
			Type type = LanguagePrimitives.ConvertTo<Type>(typeName);
			return GetEvent_Types_Ps1Xml.GetMethodInfo(type, method);
		}

		// Token: 0x06001275 RID: 4725 RVA: 0x00073B8B File Offset: 0x00071D8B
		private static MethodInfo GetMethodInfo(Type type, string method)
		{
			return type.GetMethod(method, BindingFlags.IgnoreCase | BindingFlags.Static | BindingFlags.Public);
		}

		// Token: 0x06001276 RID: 4726 RVA: 0x00073B98 File Offset: 0x00071D98
		private static ScriptBlock GetScriptBlock(string s)
		{
			ScriptBlock scriptBlock = ScriptBlock.CreateDelayParsedScriptBlock(s);
			scriptBlock.LanguageMode = new PSLanguageMode?(PSLanguageMode.FullLanguage);
			return scriptBlock;
		}

		// Token: 0x06001277 RID: 4727 RVA: 0x00073FA8 File Offset: 0x000721A8
		public static IEnumerable<TypeData> Get()
		{
			yield return new TypeData("System.Diagnostics.Eventing.Reader.EventLogConfiguration", true)
			{
				DefaultDisplayPropertySet = new PropertySetData(new string[]
				{
					"LogName",
					"MaximumSizeInBytes",
					"RecordCount",
					"LogMode"
				})
				{
					Name = "DefaultDisplayPropertySet"
				}
			};
			yield return new TypeData("System.Diagnostics.Eventing.Reader.EventLogRecord", true)
			{
				DefaultDisplayPropertySet = new PropertySetData(new string[]
				{
					"TimeCreated",
					"ProviderName",
					"Id",
					"Message"
				})
				{
					Name = "DefaultDisplayPropertySet"
				}
			};
			yield return new TypeData("System.Diagnostics.Eventing.Reader.ProviderMetadata", true)
			{
				Members = 
				{
					{
						"ProviderName",
						new AliasPropertyData("ProviderName", "Name")
					}
				},
				DefaultDisplayPropertySet = new PropertySetData(new string[]
				{
					"Name",
					"LogLinks"
				})
				{
					Name = "DefaultDisplayPropertySet"
				}
			};
			yield return new TypeData("Microsoft.PowerShell.Commands.GetCounter.CounterSet", true)
			{
				Members = 
				{
					{
						"Counter",
						new AliasPropertyData("Counter", "Paths")
					}
				}
			};
			yield return new TypeData("Microsoft.PowerShell.Commands.GetCounter.PerformanceCounterSample", true)
			{
				DefaultDisplayPropertySet = new PropertySetData(new string[]
				{
					"Path",
					"InstanceName",
					"CookedValue"
				})
				{
					Name = "DefaultDisplayPropertySet"
				}
			};
			yield return new TypeData("Microsoft.PowerShell.Commands.GetCounter.PerformanceCounterSampleSet", true)
			{
				DefaultDisplayPropertySet = new PropertySetData(new string[]
				{
					"Timestamp",
					"Readings"
				})
				{
					Name = "DefaultDisplayPropertySet"
				}
			};
			yield return new TypeData("Microsoft.PowerShell.Commands.GetCounter.PerformanceCounterSampleSet", true)
			{
				Members = 
				{
					{
						"Readings",
						new ScriptPropertyData("Readings", GetEvent_Types_Ps1Xml.GetScriptBlock("$strPaths = \"\"\r\n          foreach ($ctr in $this.CounterSamples)\r\n          {\r\n              $strPaths += ($ctr.Path + \" :\" + \"`n\")\t\r\n              $strPaths += ($ctr.CookedValue.ToString() + \"`n`n\")     \r\n          }                   \r\n          return $strPaths"), null)
					}
				}
			};
			yield break;
		}
	}
}
