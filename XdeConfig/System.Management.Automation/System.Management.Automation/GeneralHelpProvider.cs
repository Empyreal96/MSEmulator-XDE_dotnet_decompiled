using System;
using System.Collections;
using System.Collections.ObjectModel;
using System.IO;
using System.Reflection;
using System.Security;
using System.Xml;

namespace System.Management.Automation
{
	// Token: 0x020001C9 RID: 457
	internal class GeneralHelpProvider : HelpProviderWithFullCache
	{
		// Token: 0x0600151F RID: 5407 RVA: 0x00084066 File Offset: 0x00082266
		internal GeneralHelpProvider(HelpSystem helpSystem) : base(helpSystem)
		{
		}

		// Token: 0x170004FF RID: 1279
		// (get) Token: 0x06001520 RID: 5408 RVA: 0x0008407A File Offset: 0x0008227A
		internal override string Name
		{
			get
			{
				return "General Help Provider";
			}
		}

		// Token: 0x17000500 RID: 1280
		// (get) Token: 0x06001521 RID: 5409 RVA: 0x00084081 File Offset: 0x00082281
		internal override HelpCategory HelpCategory
		{
			get
			{
				return HelpCategory.General;
			}
		}

		// Token: 0x06001522 RID: 5410 RVA: 0x00084088 File Offset: 0x00082288
		internal sealed override void LoadCache()
		{
			Collection<string> collection = MUIFileSearcher.SearchFiles("*.concept.xml", base.GetSearchPaths());
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

		// Token: 0x06001523 RID: 5411 RVA: 0x00084108 File Offset: 0x00082308
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
				errorRecord.ErrorDetails = new ErrorDetails(typeof(GeneralHelpProvider).GetTypeInfo().Assembly, "HelpErrors", "HelpFileLoadFailure", new object[]
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
				errorRecord2.ErrorDetails = new ErrorDetails(typeof(GeneralHelpProvider).GetTypeInfo().Assembly, "HelpErrors", "HelpFileNotAccessible", new object[]
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
				errorRecord3.ErrorDetails = new ErrorDetails(typeof(GeneralHelpProvider).GetTypeInfo().Assembly, "HelpErrors", "HelpFileNotValid", new object[]
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
					if (xmlNode2.NodeType == XmlNodeType.Element && string.Compare(xmlNode2.Name, "conceptuals", StringComparison.OrdinalIgnoreCase) == 0)
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
							if (xmlNode3.NodeType == XmlNodeType.Element && string.Compare(xmlNode3.Name, "conceptual", StringComparison.OrdinalIgnoreCase) == 0)
							{
								HelpInfo helpInfo = GeneralHelpInfo.Load(xmlNode3);
								if (helpInfo != null)
								{
									base.HelpSystem.TraceErrors(helpInfo.Errors);
									base.AddCache(helpInfo.Name, helpInfo);
								}
							}
						}
					}
				}
				return;
			}
		}

		// Token: 0x06001524 RID: 5412 RVA: 0x000843A8 File Offset: 0x000825A8
		internal override void Reset()
		{
			base.Reset();
			this._helpFiles.Clear();
		}

		// Token: 0x040008F7 RID: 2295
		private readonly Hashtable _helpFiles = new Hashtable();
	}
}
