using System;
using System.Collections;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Globalization;
using System.IO;
using System.Reflection;
using System.Security;
using System.Xml;

namespace System.Management.Automation
{
	// Token: 0x020001C0 RID: 448
	internal class ProviderHelpProvider : HelpProviderWithCache
	{
		// Token: 0x060014C0 RID: 5312 RVA: 0x00081122 File Offset: 0x0007F322
		internal ProviderHelpProvider(HelpSystem helpSystem) : base(helpSystem)
		{
			this._sessionState = helpSystem.ExecutionContext.SessionState;
		}

		// Token: 0x170004E7 RID: 1255
		// (get) Token: 0x060014C1 RID: 5313 RVA: 0x00081147 File Offset: 0x0007F347
		internal override string Name
		{
			get
			{
				return "Provider Help Provider";
			}
		}

		// Token: 0x170004E8 RID: 1256
		// (get) Token: 0x060014C2 RID: 5314 RVA: 0x0008114E File Offset: 0x0007F34E
		internal override HelpCategory HelpCategory
		{
			get
			{
				return HelpCategory.Provider;
			}
		}

		// Token: 0x060014C3 RID: 5315 RVA: 0x000814DC File Offset: 0x0007F6DC
		internal override IEnumerable<HelpInfo> ExactMatchHelp(HelpRequest helpRequest)
		{
			Collection<ProviderInfo> matchingProviders = null;
			try
			{
				matchingProviders = this._sessionState.Provider.Get(helpRequest.Target);
			}
			catch (ProviderNotFoundException ex)
			{
				if (base.HelpSystem.LastHelpCategory == HelpCategory.Provider)
				{
					ErrorRecord errorRecord = new ErrorRecord(ex, "ProviderLoadError", ErrorCategory.ResourceUnavailable, null);
					errorRecord.ErrorDetails = new ErrorDetails(typeof(ProviderHelpProvider).GetTypeInfo().Assembly, "HelpErrors", "ProviderLoadError", new object[]
					{
						helpRequest.Target,
						ex.Message
					});
					base.HelpSystem.LastErrors.Add(errorRecord);
				}
			}
			if (matchingProviders != null)
			{
				foreach (ProviderInfo providerInfo in matchingProviders)
				{
					try
					{
						this.LoadHelpFile(providerInfo);
					}
					catch (IOException exception)
					{
						base.ReportHelpFileError(exception, helpRequest.Target, providerInfo.HelpFile);
					}
					catch (SecurityException exception2)
					{
						base.ReportHelpFileError(exception2, helpRequest.Target, providerInfo.HelpFile);
					}
					catch (XmlException exception3)
					{
						base.ReportHelpFileError(exception3, helpRequest.Target, providerInfo.HelpFile);
					}
					HelpInfo helpInfo = base.GetCache(providerInfo.PSSnapInName + "\\" + providerInfo.Name);
					if (helpInfo != null)
					{
						yield return helpInfo;
					}
				}
			}
			yield break;
		}

		// Token: 0x060014C4 RID: 5316 RVA: 0x00081500 File Offset: 0x0007F700
		private static string GetProviderAssemblyPath(ProviderInfo providerInfo)
		{
			if (providerInfo == null)
			{
				return null;
			}
			if (providerInfo.ImplementingType == null)
			{
				return null;
			}
			return Path.GetDirectoryName(ClrFacade.GetAssemblyLocation(providerInfo.ImplementingType.GetTypeInfo().Assembly));
		}

		// Token: 0x060014C5 RID: 5317 RVA: 0x00081534 File Offset: 0x0007F734
		private void LoadHelpFile(ProviderInfo providerInfo)
		{
			if (providerInfo == null)
			{
				throw PSTraceSource.NewArgumentNullException("providerInfo");
			}
			string helpFile = providerInfo.HelpFile;
			if (string.IsNullOrEmpty(helpFile) || this._helpFiles.Contains(helpFile))
			{
				return;
			}
			string file = helpFile;
			PSSnapInInfo pssnapIn = providerInfo.PSSnapIn;
			Collection<string> collection = new Collection<string>();
			if (pssnapIn != null)
			{
				file = Path.Combine(pssnapIn.ApplicationBase, helpFile);
			}
			else if (providerInfo.Module != null && !string.IsNullOrEmpty(providerInfo.Module.Path))
			{
				file = Path.Combine(providerInfo.Module.ModuleBase, helpFile);
			}
			else
			{
				collection.Add(base.GetDefaultShellSearchPath());
				collection.Add(ProviderHelpProvider.GetProviderAssemblyPath(providerInfo));
			}
			string text = MUIFileSearcher.LocateFile(file, collection);
			if (string.IsNullOrEmpty(text))
			{
				throw new FileNotFoundException(helpFile);
			}
			XmlDocument xmlDocument = InternalDeserializer.LoadUnsafeXmlDocument(new FileInfo(text), false, null);
			this._helpFiles[helpFile] = 0;
			XmlNode xmlNode = null;
			if (xmlDocument.HasChildNodes)
			{
				for (int i = 0; i < xmlDocument.ChildNodes.Count; i++)
				{
					XmlNode xmlNode2 = xmlDocument.ChildNodes[i];
					if (xmlNode2.NodeType == XmlNodeType.Element && string.Compare(xmlNode2.Name, "helpItems", StringComparison.OrdinalIgnoreCase) == 0)
					{
						xmlNode = xmlNode2;
						break;
					}
				}
			}
			if (xmlNode == null)
			{
				return;
			}
			using (base.HelpSystem.Trace(text))
			{
				if (xmlNode.HasChildNodes)
				{
					for (int j = 0; j < xmlNode.ChildNodes.Count; j++)
					{
						XmlNode xmlNode3 = xmlNode.ChildNodes[j];
						if (xmlNode3.NodeType == XmlNodeType.Element && string.Compare(xmlNode3.Name, "providerHelp", StringComparison.OrdinalIgnoreCase) == 0)
						{
							HelpInfo helpInfo = ProviderHelpInfo.Load(xmlNode3);
							if (helpInfo != null)
							{
								base.HelpSystem.TraceErrors(helpInfo.Errors);
								helpInfo.FullHelp.TypeNames.Insert(0, string.Format(CultureInfo.InvariantCulture, "ProviderHelpInfo#{0}#{1}", new object[]
								{
									providerInfo.PSSnapInName,
									helpInfo.Name
								}));
								if (!string.IsNullOrEmpty(providerInfo.PSSnapInName))
								{
									helpInfo.FullHelp.Properties.Add(new PSNoteProperty("PSSnapIn", providerInfo.PSSnapIn));
									helpInfo.FullHelp.TypeNames.Insert(1, string.Format(CultureInfo.InvariantCulture, "ProviderHelpInfo#{0}", new object[]
									{
										providerInfo.PSSnapInName
									}));
								}
								base.AddCache(providerInfo.PSSnapInName + "\\" + helpInfo.Name, helpInfo);
							}
						}
					}
				}
			}
		}

		// Token: 0x060014C6 RID: 5318 RVA: 0x00081C0C File Offset: 0x0007FE0C
		internal override IEnumerable<HelpInfo> SearchHelp(HelpRequest helpRequest, bool searchOnlyContent)
		{
			int countOfHelpInfoObjectsFound = 0;
			string target = helpRequest.Target;
			string pattern = target;
			WildcardPattern wildCardPattern = null;
			bool decoratedSearch = !WildcardPattern.ContainsWildcardCharacters(target);
			if (!searchOnlyContent)
			{
				if (decoratedSearch)
				{
					pattern += "*";
				}
			}
			else
			{
				string pattern2 = helpRequest.Target;
				if (decoratedSearch)
				{
					pattern2 = "*" + helpRequest.Target + "*";
				}
				wildCardPattern = new WildcardPattern(pattern2, WildcardOptions.Compiled | WildcardOptions.IgnoreCase);
				pattern = "*";
			}
			PSSnapinQualifiedName snapinQualifiedNameForPattern = PSSnapinQualifiedName.GetInstance(pattern);
			if (snapinQualifiedNameForPattern != null)
			{
				foreach (ProviderInfo providerInfo in this._sessionState.Provider.GetAll())
				{
					if (providerInfo.IsMatch(pattern))
					{
						try
						{
							this.LoadHelpFile(providerInfo);
						}
						catch (IOException exception)
						{
							if (!decoratedSearch)
							{
								base.ReportHelpFileError(exception, providerInfo.Name, providerInfo.HelpFile);
							}
						}
						catch (SecurityException exception2)
						{
							if (!decoratedSearch)
							{
								base.ReportHelpFileError(exception2, providerInfo.Name, providerInfo.HelpFile);
							}
						}
						catch (XmlException exception3)
						{
							if (!decoratedSearch)
							{
								base.ReportHelpFileError(exception3, providerInfo.Name, providerInfo.HelpFile);
							}
						}
						HelpInfo helpInfo = base.GetCache(providerInfo.PSSnapInName + "\\" + providerInfo.Name);
						if (helpInfo != null && (!searchOnlyContent || helpInfo.MatchPatternInContent(wildCardPattern)))
						{
							countOfHelpInfoObjectsFound++;
							yield return helpInfo;
							if (countOfHelpInfoObjectsFound >= helpRequest.MaxResults && helpRequest.MaxResults > 0)
							{
								yield break;
							}
						}
					}
				}
			}
			yield break;
		}

		// Token: 0x060014C7 RID: 5319 RVA: 0x00081D34 File Offset: 0x0007FF34
		internal override IEnumerable<HelpInfo> ProcessForwardedHelp(HelpInfo helpInfo, HelpRequest helpRequest)
		{
			ProviderCommandHelpInfo providerCommandHelpInfo = new ProviderCommandHelpInfo(helpInfo, helpRequest.ProviderContext);
			yield return providerCommandHelpInfo;
			yield break;
		}

		// Token: 0x060014C8 RID: 5320 RVA: 0x00081D5F File Offset: 0x0007FF5F
		internal override void Reset()
		{
			base.Reset();
			this._helpFiles.Clear();
		}

		// Token: 0x040008E9 RID: 2281
		private readonly SessionState _sessionState;

		// Token: 0x040008EA RID: 2282
		private readonly Hashtable _helpFiles = new Hashtable();
	}
}
