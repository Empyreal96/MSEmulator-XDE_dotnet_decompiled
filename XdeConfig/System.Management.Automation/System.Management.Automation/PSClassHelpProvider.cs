using System;
using System.Collections;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Globalization;
using System.IO;
using System.Security;
using System.Xml;

namespace System.Management.Automation
{
	// Token: 0x020001E6 RID: 486
	internal class PSClassHelpProvider : HelpProviderWithCache
	{
		// Token: 0x0600163B RID: 5691 RVA: 0x0008E362 File Offset: 0x0008C562
		internal PSClassHelpProvider(HelpSystem helpSystem) : base(helpSystem)
		{
			this._context = helpSystem.ExecutionContext;
		}

		// Token: 0x17000547 RID: 1351
		// (get) Token: 0x0600163C RID: 5692 RVA: 0x0008E382 File Offset: 0x0008C582
		internal override string Name
		{
			get
			{
				return "Powershell Class Help Provider";
			}
		}

		// Token: 0x17000548 RID: 1352
		// (get) Token: 0x0600163D RID: 5693 RVA: 0x0008E389 File Offset: 0x0008C589
		internal override HelpCategory HelpCategory
		{
			get
			{
				return HelpCategory.Class;
			}
		}

		// Token: 0x0600163E RID: 5694 RVA: 0x0008E664 File Offset: 0x0008C864
		internal override IEnumerable<HelpInfo> SearchHelp(HelpRequest helpRequest, bool searchOnlyContent)
		{
			string target = helpRequest.Target;
			Collection<string> patternList = new Collection<string>();
			bool decoratedSearch = !WildcardPattern.ContainsWildcardCharacters(helpRequest.Target);
			if (decoratedSearch)
			{
				patternList.Add("*" + target + "*");
			}
			else
			{
				patternList.Add(target);
			}
			bool useWildCards = true;
			foreach (string pattern in patternList)
			{
				PSClassSearcher searcher = new PSClassSearcher(pattern, useWildCards, this._context);
				foreach (HelpInfo helpInfo in this.GetHelpInfo(searcher))
				{
					if (helpInfo != null)
					{
						yield return helpInfo;
					}
				}
			}
			yield break;
		}

		// Token: 0x0600163F RID: 5695 RVA: 0x0008E8A8 File Offset: 0x0008CAA8
		internal override IEnumerable<HelpInfo> ExactMatchHelp(HelpRequest helpRequest)
		{
			if ((helpRequest.HelpCategory & HelpCategory.Class) == HelpCategory.None)
			{
				yield return null;
			}
			string target = helpRequest.Target;
			bool useWildCards = false;
			PSClassSearcher searcher = new PSClassSearcher(target, useWildCards, this._context);
			foreach (HelpInfo helpInfo in this.GetHelpInfo(searcher))
			{
				if (helpInfo != null)
				{
					yield return helpInfo;
				}
			}
			yield break;
		}

		// Token: 0x06001640 RID: 5696 RVA: 0x0008EAE4 File Offset: 0x0008CCE4
		private IEnumerable<HelpInfo> GetHelpInfo(PSClassSearcher searcher)
		{
			while (searcher.MoveNext())
			{
				PSClassInfo current = ((IEnumerator<PSClassInfo>)searcher).Current;
				string moduleName = current.Module.Name;
				string moduleDir = current.Module.ModuleBase;
				if (!string.IsNullOrEmpty(moduleName) && !string.IsNullOrEmpty(moduleDir))
				{
					string helpFileToFind = moduleName + "-Help.xml";
					string helpFileName = null;
					Collection<string> searchPaths = new Collection<string>();
					searchPaths.Add(moduleDir);
					string externalHelpFile = current.HelpFile;
					if (!string.IsNullOrEmpty(externalHelpFile))
					{
						FileInfo fileInfo = new FileInfo(externalHelpFile);
						DirectoryInfo directory = fileInfo.Directory;
						if (directory.Exists)
						{
							searchPaths.Add(directory.FullName);
							helpFileToFind = fileInfo.Name;
						}
					}
					HelpInfo helpInfo = this.GetHelpInfoFromHelpFile(current, helpFileToFind, searchPaths, true, out helpFileName);
					if (helpInfo != null)
					{
						yield return helpInfo;
					}
				}
			}
			yield break;
		}

		// Token: 0x06001641 RID: 5697 RVA: 0x0008EB08 File Offset: 0x0008CD08
		internal static bool IsMamlHelp(string helpFile, XmlNode helpItemsNode)
		{
			if (helpFile.EndsWith(".maml", StringComparison.CurrentCultureIgnoreCase))
			{
				return true;
			}
			if (helpItemsNode.Attributes == null)
			{
				return false;
			}
			foreach (object obj in helpItemsNode.Attributes)
			{
				XmlNode xmlNode = (XmlNode)obj;
				if (xmlNode.Name.Equals("schema", StringComparison.OrdinalIgnoreCase) && xmlNode.Value.Equals("maml", StringComparison.OrdinalIgnoreCase))
				{
					return true;
				}
			}
			return false;
		}

		// Token: 0x06001642 RID: 5698 RVA: 0x0008EBA4 File Offset: 0x0008CDA4
		private HelpInfo GetHelpInfoFromHelpFile(PSClassInfo classInfo, string helpFileToFind, Collection<string> searchPaths, bool reportErrors, out string helpFile)
		{
			HelpInfo result = null;
			helpFile = MUIFileSearcher.LocateFile(helpFileToFind, searchPaths);
			if (!File.Exists(helpFile))
			{
				return result;
			}
			if (!string.IsNullOrEmpty(helpFile))
			{
				if (!this._helpFiles.Contains(helpFile))
				{
					this.LoadHelpFile(helpFile, helpFile, classInfo.Name, reportErrors);
				}
				result = this.GetFromPSClasseHelpCache(helpFile, HelpCategory.Class);
			}
			return result;
		}

		// Token: 0x06001643 RID: 5699 RVA: 0x0008EC08 File Offset: 0x0008CE08
		private HelpInfo GetFromPSClasseHelpCache(string helpFileIdentifier, HelpCategory helpCategory)
		{
			HelpInfo helpInfo = base.GetCache(helpFileIdentifier);
			if (helpInfo != null)
			{
				MamlClassHelpInfo mamlClassHelpInfo = (MamlClassHelpInfo)helpInfo;
				helpInfo = mamlClassHelpInfo.Copy(helpCategory);
			}
			return helpInfo;
		}

		// Token: 0x06001644 RID: 5700 RVA: 0x0008EC30 File Offset: 0x0008CE30
		private void LoadHelpFile(string helpFile, string helpFileIdentifier, string commandName, bool reportErrors)
		{
			Exception ex = null;
			try
			{
				this.LoadHelpFile(helpFile, helpFileIdentifier);
			}
			catch (IOException ex2)
			{
				ex = ex2;
			}
			catch (SecurityException ex3)
			{
				ex = ex3;
			}
			catch (XmlException ex4)
			{
				ex = ex4;
			}
			catch (NotSupportedException ex5)
			{
				ex = ex5;
			}
			catch (UnauthorizedAccessException ex6)
			{
				ex = ex6;
			}
			catch (InvalidOperationException ex7)
			{
				ex = ex7;
			}
			if (ex != null)
			{
				PSClassHelpProvider.tracer.WriteLine(string.Format(CultureInfo.InvariantCulture, "Error occured in PSClassHelpProvider {0}", new object[]
				{
					ex.Message
				}), new object[0]);
			}
			if (reportErrors && ex != null)
			{
				base.ReportHelpFileError(ex, commandName, helpFile);
			}
		}

		// Token: 0x06001645 RID: 5701 RVA: 0x0008ED00 File Offset: 0x0008CF00
		private void LoadHelpFile(string helpFile, string helpFileIdentifier)
		{
			XmlDocument xmlDocument = InternalDeserializer.LoadUnsafeXmlDocument(new FileInfo(helpFile), false, null);
			this._helpFiles[helpFile] = 0;
			XmlNode xmlNode = null;
			if (xmlDocument.HasChildNodes)
			{
				for (int i = 0; i < xmlDocument.ChildNodes.Count; i++)
				{
					XmlNode xmlNode2 = xmlDocument.ChildNodes[i];
					if (xmlNode2.NodeType == XmlNodeType.Element && string.Compare(xmlNode2.LocalName, "helpItems", StringComparison.OrdinalIgnoreCase) == 0)
					{
						xmlNode = xmlNode2;
						break;
					}
				}
			}
			if (xmlNode == null)
			{
				PSClassHelpProvider.tracer.WriteLine("Unable to find 'helpItems' element in file {0}", new object[]
				{
					helpFile
				});
				return;
			}
			bool flag = PSClassHelpProvider.IsMamlHelp(helpFile, xmlNode);
			using (base.HelpSystem.Trace(helpFile))
			{
				if (xmlNode.HasChildNodes)
				{
					for (int j = 0; j < xmlNode.ChildNodes.Count; j++)
					{
						XmlNode xmlNode3 = xmlNode.ChildNodes[j];
						string localName = xmlNode3.LocalName;
						bool flag2 = string.Compare(localName, "class", StringComparison.OrdinalIgnoreCase) == 0;
						if (xmlNode3.NodeType == XmlNodeType.Element && flag2)
						{
							MamlClassHelpInfo mamlClassHelpInfo = null;
							if (flag && flag2)
							{
								mamlClassHelpInfo = MamlClassHelpInfo.Load(xmlNode3, HelpCategory.Class);
							}
							if (mamlClassHelpInfo != null)
							{
								base.HelpSystem.TraceErrors(mamlClassHelpInfo.Errors);
								base.AddCache(helpFileIdentifier, mamlClassHelpInfo);
							}
						}
					}
				}
			}
		}

		// Token: 0x04000977 RID: 2423
		private readonly ExecutionContext _context;

		// Token: 0x04000978 RID: 2424
		private readonly Hashtable _helpFiles = new Hashtable();

		// Token: 0x04000979 RID: 2425
		[TraceSource("PSClassHelpProvider", "PSClassHelpProvider")]
		private static readonly PSTraceSource tracer = PSTraceSource.GetTracer("PSClassHelpProvider", "PSClassHelpProvider");
	}
}
