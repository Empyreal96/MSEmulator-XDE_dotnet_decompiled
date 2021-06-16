using System;
using System.Collections.Generic;
using System.Globalization;
using System.Management.Automation;
using System.Management.Automation.Host;
using System.Management.Automation.Internal;
using System.Text;
using System.Xml;

namespace Microsoft.PowerShell.Commands.Internal.Format
{
	// Token: 0x0200091B RID: 2331
	internal abstract class XmlLoaderBase : IDisposable
	{
		// Token: 0x06005758 RID: 22360 RVA: 0x001C85EF File Offset: 0x001C67EF
		public void Dispose()
		{
			this.Dispose(true);
			GC.SuppressFinalize(this);
		}

		// Token: 0x06005759 RID: 22361 RVA: 0x001C85FE File Offset: 0x001C67FE
		protected virtual void Dispose(bool disposing)
		{
			if (disposing && this.logger != null)
			{
				this.logger.Dispose();
				this.logger = null;
			}
		}

		// Token: 0x170011B1 RID: 4529
		// (get) Token: 0x0600575A RID: 22362 RVA: 0x001C861D File Offset: 0x001C681D
		internal List<XmlLoaderLoggerEntry> LogEntries
		{
			get
			{
				return this.logger.LogEntries;
			}
		}

		// Token: 0x170011B2 RID: 4530
		// (get) Token: 0x0600575B RID: 22363 RVA: 0x001C862A File Offset: 0x001C682A
		internal bool HasErrors
		{
			get
			{
				return this.logger.HasErrors;
			}
		}

		// Token: 0x0600575C RID: 22364 RVA: 0x001C8637 File Offset: 0x001C6837
		protected IDisposable StackFrame(XmlNode n)
		{
			return this.StackFrame(n, -1);
		}

		// Token: 0x0600575D RID: 22365 RVA: 0x001C8644 File Offset: 0x001C6844
		protected IDisposable StackFrame(XmlNode n, int index)
		{
			XmlLoaderBase.XmlLoaderStackFrame xmlLoaderStackFrame = new XmlLoaderBase.XmlLoaderStackFrame(this, n, index);
			this.executionStack.Push(xmlLoaderStackFrame);
			if (this.logStackActivity)
			{
				this.WriteStackLocation("Enter");
			}
			return xmlLoaderStackFrame;
		}

		// Token: 0x0600575E RID: 22366 RVA: 0x001C867A File Offset: 0x001C687A
		private void RemoveStackFrame()
		{
			if (this.logStackActivity)
			{
				this.WriteStackLocation("Exit");
			}
			this.executionStack.Pop();
		}

		// Token: 0x0600575F RID: 22367 RVA: 0x001C869B File Offset: 0x001C689B
		protected void ProcessUnknownNode(XmlNode n)
		{
			if (XmlLoaderBase.IsFilteredOutNode(n))
			{
				return;
			}
			this.ReportIllegalXmlNode(n);
		}

		// Token: 0x06005760 RID: 22368 RVA: 0x001C86AD File Offset: 0x001C68AD
		protected void ProcessUnknownAttribute(XmlAttribute a)
		{
			this.ReportIllegalXmlAttribute(a);
		}

		// Token: 0x06005761 RID: 22369 RVA: 0x001C86B6 File Offset: 0x001C68B6
		protected static bool IsFilteredOutNode(XmlNode n)
		{
			return n is XmlComment || n is XmlWhitespace;
		}

		// Token: 0x06005762 RID: 22370 RVA: 0x001C86CC File Offset: 0x001C68CC
		protected bool VerifyNodeHasNoChildren(XmlNode n)
		{
			if (n.ChildNodes.Count == 0)
			{
				return true;
			}
			if (n.ChildNodes.Count == 1 && n.ChildNodes[0] is XmlText)
			{
				return true;
			}
			this.ReportError(StringUtil.Format(FormatAndOutXmlLoadingStrings.NoChildrenAllowed, new object[]
			{
				this.ComputeCurrentXPath(),
				this.FilePath,
				n.Name
			}));
			return false;
		}

		// Token: 0x06005763 RID: 22371 RVA: 0x001C873F File Offset: 0x001C693F
		internal string GetMandatoryInnerText(XmlNode n)
		{
			if (string.IsNullOrEmpty(n.InnerText))
			{
				this.ReportEmptyNode(n);
				return null;
			}
			return n.InnerText;
		}

		// Token: 0x06005764 RID: 22372 RVA: 0x001C875D File Offset: 0x001C695D
		internal string GetMandatoryAttributeValue(XmlAttribute a)
		{
			if (string.IsNullOrEmpty(a.Value))
			{
				this.ReportEmptyAttribute(a);
				return null;
			}
			return a.Value;
		}

		// Token: 0x06005765 RID: 22373 RVA: 0x001C877C File Offset: 0x001C697C
		private bool MatchNodeNameHelper(XmlNode n, string s, bool allowAttributes)
		{
			bool flag = false;
			if (string.Equals(n.Name, s, StringComparison.Ordinal))
			{
				flag = true;
			}
			else if (string.Equals(n.Name, s, StringComparison.OrdinalIgnoreCase))
			{
				string format = "XML tag differ in case only {0} {1}";
				this.ReportTrace(string.Format(CultureInfo.InvariantCulture, format, new object[]
				{
					n.Name,
					s
				}));
				flag = true;
			}
			if (flag && !allowAttributes)
			{
				XmlElement xmlElement = n as XmlElement;
				if (xmlElement != null && xmlElement.Attributes.Count > 0)
				{
					this.ReportError(StringUtil.Format(FormatAndOutXmlLoadingStrings.AttributesNotAllowed, new object[]
					{
						this.ComputeCurrentXPath(),
						this.FilePath,
						n.Name
					}));
				}
			}
			return flag;
		}

		// Token: 0x06005766 RID: 22374 RVA: 0x001C8832 File Offset: 0x001C6A32
		internal bool MatchNodeNameWithAttributes(XmlNode n, string s)
		{
			return this.MatchNodeNameHelper(n, s, true);
		}

		// Token: 0x06005767 RID: 22375 RVA: 0x001C883D File Offset: 0x001C6A3D
		internal bool MatchNodeName(XmlNode n, string s)
		{
			return this.MatchNodeNameHelper(n, s, false);
		}

		// Token: 0x06005768 RID: 22376 RVA: 0x001C8848 File Offset: 0x001C6A48
		internal bool MatchAttributeName(XmlAttribute a, string s)
		{
			if (string.Equals(a.Name, s, StringComparison.Ordinal))
			{
				return true;
			}
			if (string.Equals(a.Name, s, StringComparison.OrdinalIgnoreCase))
			{
				string format = "XML attribute differ in case only {0} {1}";
				this.ReportTrace(string.Format(CultureInfo.InvariantCulture, format, new object[]
				{
					a.Name,
					s
				}));
				return true;
			}
			return false;
		}

		// Token: 0x06005769 RID: 22377 RVA: 0x001C88A4 File Offset: 0x001C6AA4
		internal void ProcessDuplicateNode(XmlNode n)
		{
			this.ReportLogEntryHelper(StringUtil.Format(FormatAndOutXmlLoadingStrings.DuplicatedNode, this.ComputeCurrentXPath(), this.FilePath), XmlLoaderLoggerEntry.EntryType.Error, false);
		}

		// Token: 0x0600576A RID: 22378 RVA: 0x001C88C4 File Offset: 0x001C6AC4
		internal void ProcessDuplicateAlternateNode(string node1, string node2)
		{
			this.ReportLogEntryHelper(StringUtil.Format(FormatAndOutXmlLoadingStrings.MutuallyExclusiveNode, new object[]
			{
				this.ComputeCurrentXPath(),
				this.FilePath,
				node1,
				node2
			}), XmlLoaderLoggerEntry.EntryType.Error, false);
		}

		// Token: 0x0600576B RID: 22379 RVA: 0x001C8908 File Offset: 0x001C6B08
		internal void ProcessDuplicateAlternateNode(XmlNode n, string node1, string node2)
		{
			this.ReportLogEntryHelper(StringUtil.Format(FormatAndOutXmlLoadingStrings.ThreeMutuallyExclusiveNode, new object[]
			{
				this.ComputeCurrentXPath(),
				this.FilePath,
				n.Name,
				node1,
				node2
			}), XmlLoaderLoggerEntry.EntryType.Error, false);
		}

		// Token: 0x0600576C RID: 22380 RVA: 0x001C8954 File Offset: 0x001C6B54
		private void ReportIllegalXmlNode(XmlNode n)
		{
			this.ReportLogEntryHelper(StringUtil.Format(FormatAndOutXmlLoadingStrings.UnknownNode, new object[]
			{
				this.ComputeCurrentXPath(),
				this.FilePath,
				n.Name
			}), XmlLoaderLoggerEntry.EntryType.Error, false);
		}

		// Token: 0x0600576D RID: 22381 RVA: 0x001C8998 File Offset: 0x001C6B98
		private void ReportIllegalXmlAttribute(XmlAttribute a)
		{
			this.ReportLogEntryHelper(StringUtil.Format(FormatAndOutXmlLoadingStrings.UnknownAttribute, new object[]
			{
				this.ComputeCurrentXPath(),
				this.FilePath,
				a.Name
			}), XmlLoaderLoggerEntry.EntryType.Error, false);
		}

		// Token: 0x0600576E RID: 22382 RVA: 0x001C89DC File Offset: 0x001C6BDC
		protected void ReportMissingAttribute(string name)
		{
			this.ReportLogEntryHelper(StringUtil.Format(FormatAndOutXmlLoadingStrings.MissingAttribute, new object[]
			{
				this.ComputeCurrentXPath(),
				this.FilePath,
				name
			}), XmlLoaderLoggerEntry.EntryType.Error, false);
		}

		// Token: 0x0600576F RID: 22383 RVA: 0x001C8A1C File Offset: 0x001C6C1C
		protected void ReportMissingNode(string name)
		{
			this.ReportLogEntryHelper(StringUtil.Format(FormatAndOutXmlLoadingStrings.MissingNode, new object[]
			{
				this.ComputeCurrentXPath(),
				this.FilePath,
				name
			}), XmlLoaderLoggerEntry.EntryType.Error, false);
		}

		// Token: 0x06005770 RID: 22384 RVA: 0x001C8A5C File Offset: 0x001C6C5C
		protected void ReportMissingNodes(string[] names)
		{
			string text = string.Join(", ", names);
			this.ReportLogEntryHelper(StringUtil.Format(FormatAndOutXmlLoadingStrings.MissingNodeFromList, new object[]
			{
				this.ComputeCurrentXPath(),
				this.FilePath,
				text
			}), XmlLoaderLoggerEntry.EntryType.Error, false);
		}

		// Token: 0x06005771 RID: 22385 RVA: 0x001C8AA8 File Offset: 0x001C6CA8
		protected void ReportEmptyNode(XmlNode n)
		{
			this.ReportLogEntryHelper(StringUtil.Format(FormatAndOutXmlLoadingStrings.EmptyNode, new object[]
			{
				this.ComputeCurrentXPath(),
				this.FilePath,
				n.Name
			}), XmlLoaderLoggerEntry.EntryType.Error, false);
		}

		// Token: 0x06005772 RID: 22386 RVA: 0x001C8AEC File Offset: 0x001C6CEC
		protected void ReportEmptyAttribute(XmlAttribute a)
		{
			this.ReportLogEntryHelper(StringUtil.Format(FormatAndOutXmlLoadingStrings.EmptyAttribute, new object[]
			{
				this.ComputeCurrentXPath(),
				this.FilePath,
				a.Name
			}), XmlLoaderLoggerEntry.EntryType.Error, false);
		}

		// Token: 0x06005773 RID: 22387 RVA: 0x001C8B2E File Offset: 0x001C6D2E
		protected void ReportTrace(string message)
		{
			this.ReportLogEntryHelper(message, XmlLoaderLoggerEntry.EntryType.Trace, false);
		}

		// Token: 0x06005774 RID: 22388 RVA: 0x001C8B39 File Offset: 0x001C6D39
		protected void ReportError(string message)
		{
			this.ReportLogEntryHelper(message, XmlLoaderLoggerEntry.EntryType.Error, false);
		}

		// Token: 0x06005775 RID: 22389 RVA: 0x001C8B44 File Offset: 0x001C6D44
		private void ReportLogEntryHelper(string message, XmlLoaderLoggerEntry.EntryType entryType, bool failToLoadFile = false)
		{
			string xPath = this.ComputeCurrentXPath();
			XmlLoaderLoggerEntry xmlLoaderLoggerEntry = new XmlLoaderLoggerEntry();
			xmlLoaderLoggerEntry.entryType = entryType;
			xmlLoaderLoggerEntry.filePath = this.FilePath;
			xmlLoaderLoggerEntry.xPath = xPath;
			xmlLoaderLoggerEntry.message = message;
			if (failToLoadFile)
			{
				xmlLoaderLoggerEntry.failToLoadFile = true;
			}
			this.logger.LogEntry(xmlLoaderLoggerEntry);
			if (entryType == XmlLoaderLoggerEntry.EntryType.Error)
			{
				this.currentErrorCount++;
				if (this.currentErrorCount >= this.maxNumberOfErrors)
				{
					if (this.maxNumberOfErrors > 1)
					{
						XmlLoaderLoggerEntry xmlLoaderLoggerEntry2 = new XmlLoaderLoggerEntry();
						xmlLoaderLoggerEntry2.entryType = XmlLoaderLoggerEntry.EntryType.Error;
						xmlLoaderLoggerEntry2.filePath = this.FilePath;
						xmlLoaderLoggerEntry2.xPath = xPath;
						xmlLoaderLoggerEntry2.message = StringUtil.Format(FormatAndOutXmlLoadingStrings.TooManyErrors, this.FilePath);
						this.logger.LogEntry(xmlLoaderLoggerEntry2);
						this.currentErrorCount++;
					}
					throw new TooManyErrorsException
					{
						errorCount = this.currentErrorCount
					};
				}
			}
		}

		// Token: 0x06005776 RID: 22390 RVA: 0x001C8C24 File Offset: 0x001C6E24
		protected void ReportErrorForLoadingFromObjectModel(string message, string typeName)
		{
			XmlLoaderLoggerEntry xmlLoaderLoggerEntry = new XmlLoaderLoggerEntry();
			xmlLoaderLoggerEntry.entryType = XmlLoaderLoggerEntry.EntryType.Error;
			xmlLoaderLoggerEntry.message = message;
			this.logger.LogEntry(xmlLoaderLoggerEntry);
			this.currentErrorCount++;
			if (this.currentErrorCount >= this.maxNumberOfErrors)
			{
				if (this.maxNumberOfErrors > 1)
				{
					XmlLoaderLoggerEntry xmlLoaderLoggerEntry2 = new XmlLoaderLoggerEntry();
					xmlLoaderLoggerEntry2.entryType = XmlLoaderLoggerEntry.EntryType.Error;
					xmlLoaderLoggerEntry2.message = StringUtil.Format(FormatAndOutXmlLoadingStrings.TooManyErrorsInFormattingData, typeName);
					this.logger.LogEntry(xmlLoaderLoggerEntry2);
					this.currentErrorCount++;
				}
				throw new TooManyErrorsException
				{
					errorCount = this.currentErrorCount
				};
			}
		}

		// Token: 0x06005777 RID: 22391 RVA: 0x001C8CC2 File Offset: 0x001C6EC2
		private void WriteStackLocation(string label)
		{
			this.ReportTrace(label);
		}

		// Token: 0x06005778 RID: 22392 RVA: 0x001C8CCC File Offset: 0x001C6ECC
		protected string ComputeCurrentXPath()
		{
			StringBuilder stringBuilder = new StringBuilder();
			foreach (XmlLoaderBase.XmlLoaderStackFrame xmlLoaderStackFrame in this.executionStack)
			{
				stringBuilder.Insert(0, "/");
				if (xmlLoaderStackFrame.index != -1)
				{
					stringBuilder.Insert(1, string.Format(CultureInfo.InvariantCulture, "{0}[{1}]", new object[]
					{
						xmlLoaderStackFrame.node.Name,
						xmlLoaderStackFrame.index + 1
					}));
				}
				else
				{
					stringBuilder.Insert(1, xmlLoaderStackFrame.node.Name);
				}
			}
			if (stringBuilder.Length <= 0)
			{
				return null;
			}
			return stringBuilder.ToString();
		}

		// Token: 0x06005779 RID: 22393 RVA: 0x001C8D98 File Offset: 0x001C6F98
		protected XmlDocument LoadXmlDocumentFromFileLoadingInfo(AuthorizationManager authorizationManager, PSHost host, out bool isFullyTrusted)
		{
			ExternalScriptInfo externalScriptInfo = new ExternalScriptInfo(this.FilePath, this.FilePath);
			string scriptContents = externalScriptInfo.ScriptContents;
			isFullyTrusted = false;
			if (externalScriptInfo.DefiningLanguageMode == PSLanguageMode.FullLanguage)
			{
				isFullyTrusted = true;
			}
			if (authorizationManager != null)
			{
				try
				{
					authorizationManager.ShouldRunInternal(externalScriptInfo, CommandOrigin.Internal, host);
				}
				catch (PSSecurityException ex)
				{
					string message = StringUtil.Format(TypesXmlStrings.ValidationException, new object[]
					{
						string.Empty,
						this.FilePath,
						ex.Message
					});
					this.ReportLogEntryHelper(message, XmlLoaderLoggerEntry.EntryType.Error, true);
					return null;
				}
			}
			XmlDocument result;
			try
			{
				XmlDocument xmlDocument = InternalDeserializer.LoadUnsafeXmlDocument(scriptContents, true, null);
				this.ReportTrace("XmlDocument loaded OK");
				result = xmlDocument;
			}
			catch (XmlException ex2)
			{
				this.ReportError(StringUtil.Format(FormatAndOutXmlLoadingStrings.ErrorInFile, this.FilePath, ex2.Message));
				this.ReportTrace("XmlDocument discarded");
				result = null;
			}
			catch (Exception e)
			{
				CommandProcessorBase.CheckForSevereException(e);
				throw;
			}
			return result;
		}

		// Token: 0x170011B3 RID: 4531
		// (get) Token: 0x0600577A RID: 22394 RVA: 0x001C8EB8 File Offset: 0x001C70B8
		protected string FilePath
		{
			get
			{
				return this.loadingInfo.filePath;
			}
		}

		// Token: 0x0600577B RID: 22395 RVA: 0x001C8EC5 File Offset: 0x001C70C5
		protected void SetDatabaseLoadingInfo(XmlFileLoadInfo info)
		{
			this.loadingInfo.fileDirectory = info.fileDirectory;
			this.loadingInfo.filePath = info.filePath;
		}

		// Token: 0x0600577C RID: 22396 RVA: 0x001C8EE9 File Offset: 0x001C70E9
		protected void SetLoadingInfoIsFullyTrusted(bool isFullyTrusted)
		{
			this.loadingInfo.isFullyTrusted = isFullyTrusted;
		}

		// Token: 0x170011B4 RID: 4532
		// (get) Token: 0x0600577D RID: 22397 RVA: 0x001C8EF8 File Offset: 0x001C70F8
		protected DatabaseLoadingInfo LoadingInfo
		{
			get
			{
				return new DatabaseLoadingInfo
				{
					filePath = this.loadingInfo.filePath,
					fileDirectory = this.loadingInfo.fileDirectory,
					isFullyTrusted = this.loadingInfo.isFullyTrusted
				};
			}
		}

		// Token: 0x170011B5 RID: 4533
		// (get) Token: 0x0600577E RID: 22398 RVA: 0x001C8F3F File Offset: 0x001C713F
		internal bool VerifyStringResources
		{
			get
			{
				return this.verifyStringResources;
			}
		}

		// Token: 0x04002E88 RID: 11912
		[TraceSource("XmlLoaderBase", "XmlLoaderBase")]
		private static PSTraceSource tracer = PSTraceSource.GetTracer("XmlLoaderBase", "XmlLoaderBase");

		// Token: 0x04002E89 RID: 11913
		private DatabaseLoadingInfo loadingInfo = new DatabaseLoadingInfo();

		// Token: 0x04002E8A RID: 11914
		protected MshExpressionFactory expressionFactory;

		// Token: 0x04002E8B RID: 11915
		protected DisplayResourceManagerCache displayResourceManagerCache;

		// Token: 0x04002E8C RID: 11916
		private bool verifyStringResources = true;

		// Token: 0x04002E8D RID: 11917
		private int maxNumberOfErrors = 30;

		// Token: 0x04002E8E RID: 11918
		private int currentErrorCount;

		// Token: 0x04002E8F RID: 11919
		private bool logStackActivity;

		// Token: 0x04002E90 RID: 11920
		private Stack<XmlLoaderBase.XmlLoaderStackFrame> executionStack = new Stack<XmlLoaderBase.XmlLoaderStackFrame>();

		// Token: 0x04002E91 RID: 11921
		private XmlLoaderLogger logger = new XmlLoaderLogger();

		// Token: 0x0200091C RID: 2332
		private sealed class XmlLoaderStackFrame : IDisposable
		{
			// Token: 0x06005781 RID: 22401 RVA: 0x001C8F95 File Offset: 0x001C7195
			internal XmlLoaderStackFrame(XmlLoaderBase loader, XmlNode n, int index)
			{
				this.loader = loader;
				this.node = n;
				this.index = index;
			}

			// Token: 0x06005782 RID: 22402 RVA: 0x001C8FB9 File Offset: 0x001C71B9
			public void Dispose()
			{
				if (this.loader != null)
				{
					this.loader.RemoveStackFrame();
					this.loader = null;
				}
			}

			// Token: 0x04002E92 RID: 11922
			private XmlLoaderBase loader;

			// Token: 0x04002E93 RID: 11923
			internal XmlNode node;

			// Token: 0x04002E94 RID: 11924
			internal int index = -1;
		}
	}
}
