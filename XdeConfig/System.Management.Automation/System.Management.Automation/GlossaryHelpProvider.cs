using System;
using System.Collections;
using System.Collections.ObjectModel;
using System.IO;
using System.Reflection;
using System.Security;
using System.Xml;

namespace System.Management.Automation
{
	// Token: 0x020001C7 RID: 455
	internal class GlossaryHelpProvider : HelpProviderWithFullCache
	{
		// Token: 0x06001510 RID: 5392 RVA: 0x0008396A File Offset: 0x00081B6A
		internal GlossaryHelpProvider(HelpSystem helpSystem) : base(helpSystem)
		{
			base.HasCustomMatch = true;
		}

		// Token: 0x170004F9 RID: 1273
		// (get) Token: 0x06001511 RID: 5393 RVA: 0x00083985 File Offset: 0x00081B85
		internal override string Name
		{
			get
			{
				return "Glossary Help Provider";
			}
		}

		// Token: 0x170004FA RID: 1274
		// (get) Token: 0x06001512 RID: 5394 RVA: 0x0008398C File Offset: 0x00081B8C
		internal override HelpCategory HelpCategory
		{
			get
			{
				return HelpCategory.Glossary;
			}
		}

		// Token: 0x06001513 RID: 5395 RVA: 0x00083990 File Offset: 0x00081B90
		protected sealed override bool CustomMatch(string target, string key)
		{
			if (string.IsNullOrEmpty(target) || string.IsNullOrEmpty(key))
			{
				return false;
			}
			string[] array = key.Split(new char[]
			{
				','
			});
			for (int i = 0; i < array.Length; i++)
			{
				string text = array[i].Trim();
				if (text.Equals(target, StringComparison.OrdinalIgnoreCase))
				{
					return true;
				}
			}
			return false;
		}

		// Token: 0x06001514 RID: 5396 RVA: 0x000839E8 File Offset: 0x00081BE8
		internal sealed override void LoadCache()
		{
			Collection<string> collection = MUIFileSearcher.SearchFiles("*.glossary.xml", base.GetSearchPaths());
			if (collection == null)
			{
				return;
			}
			foreach (string text in collection)
			{
				if (!this._helpFiles.ContainsKey(text))
				{
					this.LoadHelpFile(text);
					this._helpFiles[text] = 0;
				}
			}
		}

		// Token: 0x06001515 RID: 5397 RVA: 0x00083A68 File Offset: 0x00081C68
		private void LoadHelpFile(string helpFile)
		{
			if (string.IsNullOrEmpty(helpFile))
			{
				return;
			}
			XmlDocument xmlDocument;
			try
			{
				xmlDocument = InternalDeserializer.LoadUnsafeXmlDocument(new FileInfo(helpFile), false, null);
			}
			catch (IOException ex)
			{
				ErrorRecord errorRecord = new ErrorRecord(ex, "HelpFileLoadFailure", ErrorCategory.OpenError, null);
				errorRecord.ErrorDetails = new ErrorDetails(typeof(GlossaryHelpProvider).GetTypeInfo().Assembly, "HelpErrors", "HelpFileLoadFailure", new object[]
				{
					helpFile,
					ex.Message
				});
				base.HelpSystem.LastErrors.Add(errorRecord);
				return;
			}
			catch (SecurityException ex2)
			{
				ErrorRecord errorRecord2 = new ErrorRecord(ex2, "HelpFileNotAccessible", ErrorCategory.OpenError, null);
				errorRecord2.ErrorDetails = new ErrorDetails(typeof(GlossaryHelpProvider).GetTypeInfo().Assembly, "HelpErrors", "HelpFileNotAccessible", new object[]
				{
					helpFile,
					ex2.Message
				});
				base.HelpSystem.LastErrors.Add(errorRecord2);
				return;
			}
			catch (XmlException ex3)
			{
				ErrorRecord errorRecord3 = new ErrorRecord(ex3, "HelpFileNotValid", ErrorCategory.SyntaxError, null);
				errorRecord3.ErrorDetails = new ErrorDetails(typeof(GlossaryHelpProvider).GetTypeInfo().Assembly, "HelpErrors", "HelpFileNotValid", new object[]
				{
					helpFile,
					ex3.Message
				});
				base.HelpSystem.LastErrors.Add(errorRecord3);
				return;
			}
			XmlNode xmlNode = null;
			if (xmlDocument.HasChildNodes)
			{
				for (int i = 0; i < xmlDocument.ChildNodes.Count; i++)
				{
					XmlNode xmlNode2 = xmlDocument.ChildNodes[i];
					if (xmlNode2.NodeType == XmlNodeType.Element && string.Compare(xmlNode2.Name, "glossary", StringComparison.OrdinalIgnoreCase) == 0)
					{
						xmlNode = xmlNode2;
						break;
					}
				}
			}
			if (xmlNode != null)
			{
				using (base.HelpSystem.Trace(helpFile))
				{
					if (xmlNode.HasChildNodes)
					{
						for (int j = 0; j < xmlNode.ChildNodes.Count; j++)
						{
							XmlNode xmlNode3 = xmlNode.ChildNodes[j];
							if (xmlNode3.NodeType == XmlNodeType.Element && string.Compare(xmlNode3.Name, "glossaryEntry", StringComparison.OrdinalIgnoreCase) == 0)
							{
								HelpInfo helpInfo = GlossaryHelpInfo.Load(xmlNode3);
								if (helpInfo != null)
								{
									base.HelpSystem.TraceErrors(helpInfo.Errors);
									base.AddCache(helpInfo.Name, helpInfo);
								}
							}
							else if (xmlNode3.NodeType == XmlNodeType.Element && string.Compare(xmlNode3.Name, "glossaryDiv", StringComparison.OrdinalIgnoreCase) == 0)
							{
								this.LoadGlossaryDiv(xmlNode3);
							}
						}
					}
				}
				return;
			}
		}

		// Token: 0x06001516 RID: 5398 RVA: 0x00083D38 File Offset: 0x00081F38
		private void LoadGlossaryDiv(XmlNode xmlNode)
		{
			if (xmlNode == null)
			{
				return;
			}
			for (int i = 0; i < xmlNode.ChildNodes.Count; i++)
			{
				XmlNode xmlNode2 = xmlNode.ChildNodes[i];
				if (xmlNode2.NodeType == XmlNodeType.Element && string.Compare(xmlNode2.Name, "glossaryEntry", StringComparison.OrdinalIgnoreCase) == 0)
				{
					HelpInfo helpInfo = GlossaryHelpInfo.Load(xmlNode2);
					if (helpInfo != null)
					{
						base.HelpSystem.TraceErrors(helpInfo.Errors);
						base.AddCache(helpInfo.Name, helpInfo);
					}
				}
			}
		}

		// Token: 0x06001517 RID: 5399 RVA: 0x00083DB2 File Offset: 0x00081FB2
		internal override void Reset()
		{
			base.Reset();
			this._helpFiles.Clear();
		}

		// Token: 0x040008F4 RID: 2292
		private readonly Hashtable _helpFiles = new Hashtable();
	}
}
