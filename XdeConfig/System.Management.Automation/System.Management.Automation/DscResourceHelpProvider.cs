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
	// Token: 0x020001E5 RID: 485
	internal class DscResourceHelpProvider : HelpProviderWithCache
	{
		// Token: 0x0600162F RID: 5679 RVA: 0x0008D86F File Offset: 0x0008BA6F
		internal DscResourceHelpProvider(HelpSystem helpSystem) : base(helpSystem)
		{
			this._context = helpSystem.ExecutionContext;
		}

		// Token: 0x17000545 RID: 1349
		// (get) Token: 0x06001630 RID: 5680 RVA: 0x0008D88F File Offset: 0x0008BA8F
		internal override string Name
		{
			get
			{
				return "Dsc Resource Help Provider";
			}
		}

		// Token: 0x17000546 RID: 1350
		// (get) Token: 0x06001631 RID: 5681 RVA: 0x0008D896 File Offset: 0x0008BA96
		internal override HelpCategory HelpCategory
		{
			get
			{
				return HelpCategory.DscResource;
			}
		}

		// Token: 0x06001632 RID: 5682 RVA: 0x0008DB68 File Offset: 0x0008BD68
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
			foreach (string pattern in patternList)
			{
				DscResourceSearcher searcher = new DscResourceSearcher(pattern, this._context);
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

		// Token: 0x06001633 RID: 5683 RVA: 0x0008DD94 File Offset: 0x0008BF94
		internal override IEnumerable<HelpInfo> ExactMatchHelp(HelpRequest helpRequest)
		{
			if ((helpRequest.HelpCategory & HelpCategory.DscResource) == HelpCategory.None)
			{
				yield return null;
			}
			string target = helpRequest.Target;
			DscResourceSearcher searcher = new DscResourceSearcher(target, this._context);
			foreach (HelpInfo helpInfo in this.GetHelpInfo(searcher))
			{
				if (helpInfo != null)
				{
					yield return helpInfo;
				}
			}
			yield break;
		}

		// Token: 0x06001634 RID: 5684 RVA: 0x0008DFBC File Offset: 0x0008C1BC
		private IEnumerable<HelpInfo> GetHelpInfo(DscResourceSearcher searcher)
		{
			while (searcher.MoveNext())
			{
				DscResourceInfo current = ((IEnumerator<DscResourceInfo>)searcher).Current;
				string moduleName = null;
				string moduleDir = current.ParentPath;
				if (current.Module != null)
				{
					moduleName = current.Module.Name;
				}
				else if (!string.IsNullOrEmpty(moduleDir))
				{
					string[] array = moduleDir.Split(new char[]
					{
						'\\'
					});
					moduleName = array[array.Length - 1];
				}
				if (!string.IsNullOrEmpty(moduleName) && !string.IsNullOrEmpty(moduleDir))
				{
					string helpFileToFind = moduleName + "-Help.xml";
					string helpFileName = null;
					HelpInfo helpInfo = this.GetHelpInfoFromHelpFile(current, helpFileToFind, new Collection<string>
					{
						moduleDir
					}, true, out helpFileName);
					if (helpInfo != null)
					{
						yield return helpInfo;
					}
				}
			}
			yield break;
		}

		// Token: 0x06001635 RID: 5685 RVA: 0x0008DFE0 File Offset: 0x0008C1E0
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

		// Token: 0x06001636 RID: 5686 RVA: 0x0008E07C File Offset: 0x0008C27C
		private HelpInfo GetHelpInfoFromHelpFile(DscResourceInfo resourceInfo, string helpFileToFind, Collection<string> searchPaths, bool reportErrors, out string helpFile)
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
					this.LoadHelpFile(helpFile, helpFile, resourceInfo.Name, reportErrors);
				}
				result = this.GetFromResourceHelpCache(helpFile, HelpCategory.DscResource);
			}
			return result;
		}

		// Token: 0x06001637 RID: 5687 RVA: 0x0008E0E0 File Offset: 0x0008C2E0
		private HelpInfo GetFromResourceHelpCache(string helpFileIdentifier, HelpCategory helpCategory)
		{
			HelpInfo helpInfo = base.GetCache(helpFileIdentifier);
			if (helpInfo != null)
			{
				MamlCommandHelpInfo mamlCommandHelpInfo = (MamlCommandHelpInfo)helpInfo;
				helpInfo = mamlCommandHelpInfo.Copy(helpCategory);
			}
			return helpInfo;
		}

		// Token: 0x06001638 RID: 5688 RVA: 0x0008E108 File Offset: 0x0008C308
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
				DscResourceHelpProvider.tracer.WriteLine(string.Format(CultureInfo.InvariantCulture, "Error occured in DscResourceHelpProvider {0}", new object[]
				{
					ex.Message
				}), new object[0]);
			}
			if (reportErrors && ex != null)
			{
				base.ReportHelpFileError(ex, commandName, helpFile);
			}
		}

		// Token: 0x06001639 RID: 5689 RVA: 0x0008E1D8 File Offset: 0x0008C3D8
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
				DscResourceHelpProvider.tracer.WriteLine("Unable to find 'helpItems' element in file {0}", new object[]
				{
					helpFile
				});
				return;
			}
			bool flag = DscResourceHelpProvider.IsMamlHelp(helpFile, xmlNode);
			using (base.HelpSystem.Trace(helpFile))
			{
				if (xmlNode.HasChildNodes)
				{
					for (int j = 0; j < xmlNode.ChildNodes.Count; j++)
					{
						XmlNode xmlNode3 = xmlNode.ChildNodes[j];
						string localName = xmlNode3.LocalName;
						bool flag2 = string.Compare(localName, "dscResource", StringComparison.OrdinalIgnoreCase) == 0;
						if (xmlNode3.NodeType == XmlNodeType.Element && flag2)
						{
							MamlCommandHelpInfo mamlCommandHelpInfo = null;
							if (flag && flag2)
							{
								mamlCommandHelpInfo = MamlCommandHelpInfo.Load(xmlNode3, HelpCategory.DscResource);
							}
							if (mamlCommandHelpInfo != null)
							{
								base.HelpSystem.TraceErrors(mamlCommandHelpInfo.Errors);
								base.AddCache(helpFileIdentifier, mamlCommandHelpInfo);
							}
						}
					}
				}
			}
		}

		// Token: 0x04000974 RID: 2420
		private readonly ExecutionContext _context;

		// Token: 0x04000975 RID: 2421
		private readonly Hashtable _helpFiles = new Hashtable();

		// Token: 0x04000976 RID: 2422
		[TraceSource("DscResourceHelpProvider", "DscResourceHelpProvider")]
		private static readonly PSTraceSource tracer = PSTraceSource.GetTracer("DscResourceHelpProvider", "DscResourceHelpProvider");
	}
}
