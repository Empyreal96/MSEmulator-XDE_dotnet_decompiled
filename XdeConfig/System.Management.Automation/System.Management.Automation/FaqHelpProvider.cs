using System;
using System.Collections;
using System.Collections.ObjectModel;
using System.IO;
using System.Reflection;
using System.Security;
using System.Xml;

namespace System.Management.Automation
{
	// Token: 0x020001C5 RID: 453
	internal class FaqHelpProvider : HelpProviderWithFullCache
	{
		// Token: 0x06001501 RID: 5377 RVA: 0x000831FF File Offset: 0x000813FF
		internal FaqHelpProvider(HelpSystem helpSystem) : base(helpSystem)
		{
		}

		// Token: 0x170004F2 RID: 1266
		// (get) Token: 0x06001502 RID: 5378 RVA: 0x00083213 File Offset: 0x00081413
		internal override string Name
		{
			get
			{
				return "FAQ Help Provider";
			}
		}

		// Token: 0x170004F3 RID: 1267
		// (get) Token: 0x06001503 RID: 5379 RVA: 0x0008321A File Offset: 0x0008141A
		internal override HelpCategory HelpCategory
		{
			get
			{
				return HelpCategory.FAQ;
			}
		}

		// Token: 0x06001504 RID: 5380 RVA: 0x00083220 File Offset: 0x00081420
		internal sealed override void LoadCache()
		{
			Collection<string> collection = MUIFileSearcher.SearchFiles("*.faq.xml", base.GetSearchPaths());
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

		// Token: 0x06001505 RID: 5381 RVA: 0x000832A0 File Offset: 0x000814A0
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
				errorRecord.ErrorDetails = new ErrorDetails(typeof(FaqHelpProvider).GetTypeInfo().Assembly, "HelpErrors", "HelpFileLoadFailure", new object[]
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
				errorRecord2.ErrorDetails = new ErrorDetails(typeof(FaqHelpProvider).GetTypeInfo().Assembly, "HelpErrors", "HelpFileNotAccessible", new object[]
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
				errorRecord3.ErrorDetails = new ErrorDetails(typeof(FaqHelpProvider).GetTypeInfo().Assembly, "HelpErrors", "HelpFileNotValid", new object[]
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
					if (xmlNode2.NodeType == XmlNodeType.Element && string.Compare(xmlNode2.Name, "faq", StringComparison.OrdinalIgnoreCase) == 0)
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
							if (xmlNode3.NodeType == XmlNodeType.Element && string.Compare(xmlNode3.Name, "faqEntry", StringComparison.OrdinalIgnoreCase) == 0)
							{
								HelpInfo helpInfo = FaqHelpInfo.Load(xmlNode3);
								if (helpInfo != null)
								{
									base.HelpSystem.TraceErrors(helpInfo.Errors);
									base.AddCache(helpInfo.Name, helpInfo);
								}
							}
							else if (xmlNode3.NodeType == XmlNodeType.Element && string.Compare(xmlNode3.Name, "faqDiv", StringComparison.OrdinalIgnoreCase) == 0)
							{
								this.LoadFaqDiv(xmlNode3);
							}
						}
					}
				}
				return;
			}
		}

		// Token: 0x06001506 RID: 5382 RVA: 0x00083570 File Offset: 0x00081770
		private void LoadFaqDiv(XmlNode xmlNode)
		{
			if (xmlNode == null)
			{
				return;
			}
			for (int i = 0; i < xmlNode.ChildNodes.Count; i++)
			{
				XmlNode xmlNode2 = xmlNode.ChildNodes[i];
				if (xmlNode2.NodeType == XmlNodeType.Element && string.Compare(xmlNode2.Name, "faqEntry", StringComparison.OrdinalIgnoreCase) == 0)
				{
					HelpInfo helpInfo = FaqHelpInfo.Load(xmlNode2);
					if (helpInfo != null)
					{
						base.HelpSystem.TraceErrors(helpInfo.Errors);
						base.AddCache(helpInfo.Name, helpInfo);
					}
				}
			}
		}

		// Token: 0x06001507 RID: 5383 RVA: 0x000835EA File Offset: 0x000817EA
		internal override void Reset()
		{
			base.Reset();
			this._helpFiles.Clear();
		}

		// Token: 0x040008F2 RID: 2290
		private readonly Hashtable _helpFiles = new Hashtable();
	}
}
