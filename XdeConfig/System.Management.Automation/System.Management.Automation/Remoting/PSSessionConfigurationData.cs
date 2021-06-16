using System;
using System.Collections.Generic;
using System.IO;
using System.Text;
using System.Xml;
using Microsoft.PowerShell.Commands;

namespace System.Management.Automation.Remoting
{
	// Token: 0x02000372 RID: 882
	public sealed class PSSessionConfigurationData
	{
		// Token: 0x17000A6E RID: 2670
		// (get) Token: 0x06002B5B RID: 11099 RVA: 0x000F01EC File Offset: 0x000EE3EC
		public List<string> ModulesToImport
		{
			get
			{
				return this._modulesToImport;
			}
		}

		// Token: 0x17000A6F RID: 2671
		// (get) Token: 0x06002B5C RID: 11100 RVA: 0x000F01F4 File Offset: 0x000EE3F4
		internal List<object> ModulesToImportInternal
		{
			get
			{
				return this._modulesToImportInternal;
			}
		}

		// Token: 0x17000A70 RID: 2672
		// (get) Token: 0x06002B5D RID: 11101 RVA: 0x000F01FC File Offset: 0x000EE3FC
		// (set) Token: 0x06002B5E RID: 11102 RVA: 0x000F0204 File Offset: 0x000EE404
		public string PrivateData
		{
			get
			{
				return this._privateData;
			}
			internal set
			{
				this._privateData = value;
			}
		}

		// Token: 0x06002B5F RID: 11103 RVA: 0x000F020D File Offset: 0x000EE40D
		private PSSessionConfigurationData()
		{
		}

		// Token: 0x06002B60 RID: 11104 RVA: 0x000F0218 File Offset: 0x000EE418
		internal static string Unescape(string s)
		{
			StringBuilder stringBuilder = new StringBuilder(s);
			stringBuilder.Replace("&lt;", "<");
			stringBuilder.Replace("&gt;", ">");
			stringBuilder.Replace("&quot;", "\"");
			stringBuilder.Replace("&apos;", "'");
			stringBuilder.Replace("&amp;", "&");
			return stringBuilder.ToString();
		}

		// Token: 0x06002B61 RID: 11105 RVA: 0x000F0288 File Offset: 0x000EE488
		internal static PSSessionConfigurationData Create(string configurationData)
		{
			PSSessionConfigurationData pssessionConfigurationData = new PSSessionConfigurationData();
			if (string.IsNullOrEmpty(configurationData))
			{
				return pssessionConfigurationData;
			}
			configurationData = PSSessionConfigurationData.Unescape(configurationData);
			XmlReaderSettings settings = new XmlReaderSettings
			{
				CheckCharacters = false,
				IgnoreComments = true,
				IgnoreProcessingInstructions = true,
				MaxCharactersInDocument = 10000L,
				XmlResolver = null,
				ConformanceLevel = ConformanceLevel.Fragment
			};
			using (XmlReader xmlReader = XmlReader.Create(new StringReader(configurationData), settings))
			{
				if (xmlReader.ReadToFollowing("SessionConfigurationData"))
				{
					bool flag = xmlReader.ReadToDescendant("Param");
					while (flag)
					{
						if (!xmlReader.MoveToAttribute("Name"))
						{
							throw PSTraceSource.NewArgumentException(configurationData, RemotingErrorIdStrings.NoAttributesFoundForParamElement, new object[]
							{
								"Name",
								"Value",
								"Param"
							});
						}
						string value = xmlReader.Value;
						if (string.Equals(value, "PrivateData", StringComparison.OrdinalIgnoreCase))
						{
							if (xmlReader.ReadToFollowing("PrivateData"))
							{
								string privateData = xmlReader.ReadOuterXml();
								PSSessionConfigurationData.AssertValueNotAssigned("PrivateData", pssessionConfigurationData._privateData);
								pssessionConfigurationData._privateData = privateData;
							}
						}
						else
						{
							if (!xmlReader.MoveToAttribute("Value"))
							{
								throw PSTraceSource.NewArgumentException(configurationData, RemotingErrorIdStrings.NoAttributesFoundForParamElement, new object[]
								{
									"Name",
									"Value",
									"Param"
								});
							}
							string value2 = xmlReader.Value;
							pssessionConfigurationData.Update(value, value2);
						}
						flag = xmlReader.ReadToFollowing("Param");
					}
				}
			}
			pssessionConfigurationData.CreateCollectionIfNecessary();
			return pssessionConfigurationData;
		}

		// Token: 0x06002B62 RID: 11106 RVA: 0x000F0430 File Offset: 0x000EE630
		private static void AssertValueNotAssigned(string optionName, object originalValue)
		{
			if (originalValue != null)
			{
				throw PSTraceSource.NewArgumentException(optionName, RemotingErrorIdStrings.DuplicateInitializationParameterFound, new object[]
				{
					optionName,
					"SessionConfigurationData"
				});
			}
		}

		// Token: 0x06002B63 RID: 11107 RVA: 0x000F0460 File Offset: 0x000EE660
		private void Update(string optionName, string optionValue)
		{
			string a;
			if ((a = optionName.ToLowerInvariant()) != null)
			{
				if (!(a == "modulestoimport"))
				{
					return;
				}
				PSSessionConfigurationData.AssertValueNotAssigned("modulestoimport", this._modulesToImport);
				this._modulesToImport = new List<string>();
				this._modulesToImportInternal = new List<object>();
				object[] array = optionValue.Split(new string[]
				{
					","
				}, StringSplitOptions.RemoveEmptyEntries);
				foreach (object obj in array)
				{
					string text = obj as string;
					if (text != null)
					{
						this._modulesToImport.Add(text.Trim());
						ModuleSpecification item = null;
						if (ModuleSpecification.TryParse(text, out item))
						{
							this._modulesToImportInternal.Add(item);
						}
						else
						{
							this._modulesToImportInternal.Add(text.Trim());
						}
					}
				}
			}
		}

		// Token: 0x06002B64 RID: 11108 RVA: 0x000F052D File Offset: 0x000EE72D
		private void CreateCollectionIfNecessary()
		{
			if (this._modulesToImport == null)
			{
				this._modulesToImport = new List<string>();
			}
			if (this._modulesToImportInternal == null)
			{
				this._modulesToImportInternal = new List<object>();
			}
		}

		// Token: 0x040015C6 RID: 5574
		private const string SessionConfigToken = "SessionConfigurationData";

		// Token: 0x040015C7 RID: 5575
		internal const string ModulesToImportToken = "modulestoimport";

		// Token: 0x040015C8 RID: 5576
		internal const string PrivateDataToken = "PrivateData";

		// Token: 0x040015C9 RID: 5577
		internal const string InProcActivityToken = "InProcActivity";

		// Token: 0x040015CA RID: 5578
		private const string ParamToken = "Param";

		// Token: 0x040015CB RID: 5579
		private const string NameToken = "Name";

		// Token: 0x040015CC RID: 5580
		private const string ValueToken = "Value";

		// Token: 0x040015CD RID: 5581
		public static bool IsServerManager;

		// Token: 0x040015CE RID: 5582
		private List<string> _modulesToImport;

		// Token: 0x040015CF RID: 5583
		private List<object> _modulesToImportInternal;

		// Token: 0x040015D0 RID: 5584
		private string _privateData;
	}
}
