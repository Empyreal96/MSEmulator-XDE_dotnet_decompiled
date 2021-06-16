using System;
using System.Globalization;
using System.IO;
using System.Management.Automation.Runspaces;
using System.Reflection;
using System.Threading;
using System.Xml;

namespace System.Management.Automation.Remoting
{
	// Token: 0x02000369 RID: 873
	internal class ConfigurationDataFromXML
	{
		// Token: 0x06002B16 RID: 11030 RVA: 0x000ECA4C File Offset: 0x000EAC4C
		private void Update(string optionName, string optionValue)
		{
			string key;
			switch (key = optionName.ToLowerInvariant())
			{
			case "applicationbase":
				this.AssertValueNotAssigned("applicationbase", this.ApplicationBase);
				this.ApplicationBase = Environment.ExpandEnvironmentVariables(optionValue);
				return;
			case "assemblyname":
				this.AssertValueNotAssigned("assemblyname", this.AssemblyName);
				this.AssemblyName = optionValue;
				return;
			case "pssessionconfigurationtypename":
				this.AssertValueNotAssigned("pssessionconfigurationtypename", this.EndPointConfigurationTypeName);
				this.EndPointConfigurationTypeName = optionValue;
				return;
			case "startupscript":
				this.AssertValueNotAssigned("startupscript", this.StartupScript);
				if (!optionValue.EndsWith(".ps1", StringComparison.OrdinalIgnoreCase))
				{
					throw PSTraceSource.NewArgumentException("startupscript", RemotingErrorIdStrings.StartupScriptNotCorrect, new object[]
					{
						"startupscript"
					});
				}
				this.StartupScript = Environment.ExpandEnvironmentVariables(optionValue);
				return;
			case "psmaximumreceivedobjectsizemb":
				this.AssertValueNotAssigned("psmaximumreceivedobjectsizemb", this.MaxReceivedObjectSizeMB);
				this.MaxReceivedObjectSizeMB = ConfigurationDataFromXML.GetIntValueInBytes(optionValue);
				return;
			case "psmaximumreceiveddatasizepercommandmb":
				this.AssertValueNotAssigned("psmaximumreceiveddatasizepercommandmb", this.MaxReceivedCommandSizeMB);
				this.MaxReceivedCommandSizeMB = ConfigurationDataFromXML.GetIntValueInBytes(optionValue);
				return;
			case "pssessionthreadoptions":
				this.AssertValueNotAssigned("pssessionthreadoptions", this.ShellThreadOptions);
				this.ShellThreadOptions = new PSThreadOptions?((PSThreadOptions)LanguagePrimitives.ConvertTo(optionValue, typeof(PSThreadOptions), CultureInfo.InvariantCulture));
				return;
			case "pssessionthreadapartmentstate":
				this.AssertValueNotAssigned("pssessionthreadapartmentstate", this.ShellThreadApartmentState);
				this.ShellThreadApartmentState = new ApartmentState?((ApartmentState)LanguagePrimitives.ConvertTo(optionValue, typeof(ApartmentState), CultureInfo.InvariantCulture));
				return;
			case "sessionconfigurationdata":
				this.AssertValueNotAssigned("sessionconfigurationdata", this.SessionConfigurationData);
				this.SessionConfigurationData = PSSessionConfigurationData.Create(optionValue);
				return;
			case "configfilepath":
				this.AssertValueNotAssigned("configfilepath", this.ConfigFilePath);
				this.ConfigFilePath = optionValue.ToString();
				break;

				return;
			}
		}

		// Token: 0x06002B17 RID: 11031 RVA: 0x000ECCD4 File Offset: 0x000EAED4
		private void AssertValueNotAssigned(string optionName, object originalValue)
		{
			if (originalValue != null)
			{
				throw PSTraceSource.NewArgumentException(optionName, RemotingErrorIdStrings.DuplicateInitializationParameterFound, new object[]
				{
					optionName,
					"InitializationParameters"
				});
			}
		}

		// Token: 0x06002B18 RID: 11032 RVA: 0x000ECD04 File Offset: 0x000EAF04
		private static int? GetIntValueInBytes(string optionValueInMB)
		{
			int? num = null;
			try
			{
				double num2 = (double)LanguagePrimitives.ConvertTo(optionValueInMB, typeof(double), CultureInfo.InvariantCulture);
				num = new int?((int)(num2 * 1024.0 * 1024.0));
			}
			catch (InvalidCastException)
			{
			}
			if (num < 0)
			{
				num = null;
			}
			return num;
		}

		// Token: 0x06002B19 RID: 11033 RVA: 0x000ECD88 File Offset: 0x000EAF88
		internal static ConfigurationDataFromXML Create(string initializationParameters)
		{
			ConfigurationDataFromXML configurationDataFromXML = new ConfigurationDataFromXML();
			if (string.IsNullOrEmpty(initializationParameters))
			{
				return configurationDataFromXML;
			}
			XmlReaderSettings xmlReaderSettings = new XmlReaderSettings();
			xmlReaderSettings.CheckCharacters = false;
			xmlReaderSettings.IgnoreComments = true;
			xmlReaderSettings.IgnoreProcessingInstructions = true;
			xmlReaderSettings.MaxCharactersInDocument = 10000L;
			xmlReaderSettings.ConformanceLevel = ConformanceLevel.Fragment;
			xmlReaderSettings.XmlResolver = null;
			using (XmlReader xmlReader = XmlReader.Create(new StringReader(initializationParameters), xmlReaderSettings))
			{
				if (xmlReader.ReadToFollowing("InitializationParameters"))
				{
					bool flag = xmlReader.ReadToDescendant("Param");
					while (flag)
					{
						if (!xmlReader.MoveToAttribute("Name"))
						{
							throw PSTraceSource.NewArgumentException(initializationParameters, RemotingErrorIdStrings.NoAttributesFoundForParamElement, new object[]
							{
								"Name",
								"Value",
								"Param"
							});
						}
						string value = xmlReader.Value;
						if (!xmlReader.MoveToAttribute("Value"))
						{
							throw PSTraceSource.NewArgumentException(initializationParameters, RemotingErrorIdStrings.NoAttributesFoundForParamElement, new object[]
							{
								"Name",
								"Value",
								"Param"
							});
						}
						string value2 = xmlReader.Value;
						configurationDataFromXML.Update(value, value2);
						flag = xmlReader.ReadToFollowing("Param");
					}
				}
			}
			if (configurationDataFromXML.MaxReceivedObjectSizeMB == null)
			{
				configurationDataFromXML.MaxReceivedObjectSizeMB = new int?(10485760);
			}
			if (configurationDataFromXML.MaxReceivedCommandSizeMB == null)
			{
				configurationDataFromXML.MaxReceivedCommandSizeMB = new int?(52428800);
			}
			return configurationDataFromXML;
		}

		// Token: 0x06002B1A RID: 11034 RVA: 0x000ECF08 File Offset: 0x000EB108
		internal PSSessionConfiguration CreateEndPointConfigurationInstance()
		{
			try
			{
				return (PSSessionConfiguration)Activator.CreateInstance(this.EndPointConfigurationType);
			}
			catch (TypeLoadException)
			{
			}
			catch (ArgumentException)
			{
			}
			catch (MissingMethodException)
			{
			}
			catch (InvalidCastException)
			{
			}
			catch (TargetInvocationException)
			{
			}
			throw PSTraceSource.NewArgumentException("typeToLoad", RemotingErrorIdStrings.UnableToLoadType, new object[]
			{
				this.EndPointConfigurationTypeName,
				"InitializationParameters"
			});
		}

		// Token: 0x04001569 RID: 5481
		internal const string INITPARAMETERSTOKEN = "InitializationParameters";

		// Token: 0x0400156A RID: 5482
		internal const string PARAMTOKEN = "Param";

		// Token: 0x0400156B RID: 5483
		internal const string NAMETOKEN = "Name";

		// Token: 0x0400156C RID: 5484
		internal const string VALUETOKEN = "Value";

		// Token: 0x0400156D RID: 5485
		internal const string APPBASETOKEN = "applicationbase";

		// Token: 0x0400156E RID: 5486
		internal const string ASSEMBLYTOKEN = "assemblyname";

		// Token: 0x0400156F RID: 5487
		internal const string SHELLCONFIGTYPETOKEN = "pssessionconfigurationtypename";

		// Token: 0x04001570 RID: 5488
		internal const string STARTUPSCRIPTTOKEN = "startupscript";

		// Token: 0x04001571 RID: 5489
		internal const string MAXRCVDOBJSIZETOKEN = "psmaximumreceivedobjectsizemb";

		// Token: 0x04001572 RID: 5490
		internal const string MAXRCVDOBJSIZETOKEN_CamelCase = "PSMaximumReceivedObjectSizeMB";

		// Token: 0x04001573 RID: 5491
		internal const string MAXRCVDCMDSIZETOKEN = "psmaximumreceiveddatasizepercommandmb";

		// Token: 0x04001574 RID: 5492
		internal const string MAXRCVDCMDSIZETOKEN_CamelCase = "PSMaximumReceivedDataSizePerCommandMB";

		// Token: 0x04001575 RID: 5493
		internal const string THREADOPTIONSTOKEN = "pssessionthreadoptions";

		// Token: 0x04001576 RID: 5494
		internal const string THREADAPTSTATETOKEN = "pssessionthreadapartmentstate";

		// Token: 0x04001577 RID: 5495
		internal const string SESSIONCONFIGTOKEN = "sessionconfigurationdata";

		// Token: 0x04001578 RID: 5496
		internal const string PSVERSIONTOKEN = "PSVersion";

		// Token: 0x04001579 RID: 5497
		internal const string MAXPSVERSIONTOKEN = "MaxPSVersion";

		// Token: 0x0400157A RID: 5498
		internal const string MODULESTOIMPORT = "ModulesToImport";

		// Token: 0x0400157B RID: 5499
		internal const string HOSTMODE = "hostmode";

		// Token: 0x0400157C RID: 5500
		internal const string ENDPOINTCONFIGURATIONTYPE = "sessiontype";

		// Token: 0x0400157D RID: 5501
		internal const string WORKFLOWCOREASSEMBLY = "Microsoft.PowerShell.Workflow.ServiceCore, Version=3.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35, processorArchitecture=MSIL";

		// Token: 0x0400157E RID: 5502
		internal const string WORKFLOWCORETYPENAME = "Microsoft.PowerShell.Workflow.PSWorkflowSessionConfiguration";

		// Token: 0x0400157F RID: 5503
		internal const string PSWORKFLOWMODULE = "%windir%\\system32\\windowspowershell\\v1.0\\Modules\\PSWorkflow";

		// Token: 0x04001580 RID: 5504
		internal const string CONFIGFILEPATH = "configfilepath";

		// Token: 0x04001581 RID: 5505
		internal const string CONFIGFILEPATH_CamelCase = "ConfigFilePath";

		// Token: 0x04001582 RID: 5506
		internal string StartupScript;

		// Token: 0x04001583 RID: 5507
		internal string InitializationScriptForOutOfProcessRunspace;

		// Token: 0x04001584 RID: 5508
		internal string ApplicationBase;

		// Token: 0x04001585 RID: 5509
		internal string AssemblyName;

		// Token: 0x04001586 RID: 5510
		internal string EndPointConfigurationTypeName;

		// Token: 0x04001587 RID: 5511
		internal Type EndPointConfigurationType;

		// Token: 0x04001588 RID: 5512
		internal int? MaxReceivedObjectSizeMB;

		// Token: 0x04001589 RID: 5513
		internal int? MaxReceivedCommandSizeMB;

		// Token: 0x0400158A RID: 5514
		internal PSThreadOptions? ShellThreadOptions;

		// Token: 0x0400158B RID: 5515
		internal ApartmentState? ShellThreadApartmentState;

		// Token: 0x0400158C RID: 5516
		internal PSSessionConfigurationData SessionConfigurationData;

		// Token: 0x0400158D RID: 5517
		internal string ConfigFilePath;
	}
}
