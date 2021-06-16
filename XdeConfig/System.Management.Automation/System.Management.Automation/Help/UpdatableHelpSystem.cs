using System;
using System.Collections;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Globalization;
using System.IO;
using System.Management.Automation.Internal;
using System.Net;
using System.Security;
using System.Text;
using System.Threading;
using System.Xml;
using System.Xml.Schema;
using Microsoft.PowerShell.Commands;
using Microsoft.Win32;

namespace System.Management.Automation.Help
{
	// Token: 0x020001DE RID: 478
	internal class UpdatableHelpSystem : IDisposable
	{
		// Token: 0x17000533 RID: 1331
		// (get) Token: 0x060015E7 RID: 5607 RVA: 0x0008A7E0 File Offset: 0x000889E0
		internal WebClient WebClient
		{
			get
			{
				return this._webClient;
			}
		}

		// Token: 0x17000534 RID: 1332
		// (get) Token: 0x060015E8 RID: 5608 RVA: 0x0008A7E8 File Offset: 0x000889E8
		// (set) Token: 0x060015E9 RID: 5609 RVA: 0x0008A7F0 File Offset: 0x000889F0
		internal string CurrentModule
		{
			get
			{
				return this._currentModule;
			}
			set
			{
				this._currentModule = value;
			}
		}

		// Token: 0x060015EA RID: 5610 RVA: 0x0008A7FC File Offset: 0x000889FC
		internal UpdatableHelpSystem(UpdatableHelpCommandBase cmdlet, bool useDefaultCredentials)
		{
			this._webClient = new WebClient();
			this._completionEvent = new AutoResetEvent(false);
			this._completed = false;
			this._progressEvents = new Collection<UpdatableHelpProgressEventArgs>();
			this._errors = new Collection<Exception>();
			this._stopping = false;
			this._syncObject = new object();
			this._cmdlet = cmdlet;
			this._cancelTokenSource = new CancellationTokenSource();
			this._webClient.UseDefaultCredentials = useDefaultCredentials;
			this._webClient.DownloadProgressChanged += this.HandleDownloadProgressChanged;
			this._webClient.DownloadFileCompleted += this.HandleDownloadFileCompleted;
		}

		// Token: 0x060015EB RID: 5611 RVA: 0x0008A8A1 File Offset: 0x00088AA1
		public void Dispose()
		{
			this._completionEvent.Dispose();
			this._cancelTokenSource.Dispose();
			this._webClient.Dispose();
			GC.SuppressFinalize(this);
		}

		// Token: 0x17000535 RID: 1333
		// (get) Token: 0x060015EC RID: 5612 RVA: 0x0008A8CA File Offset: 0x00088ACA
		internal Collection<Exception> Errors
		{
			get
			{
				return this._errors;
			}
		}

		// Token: 0x060015ED RID: 5613 RVA: 0x0008A9D8 File Offset: 0x00088BD8
		internal IEnumerable<string> GetCurrentUICulture()
		{
			CultureInfo culture = CultureInfo.CurrentUICulture;
			while (culture != null && !string.IsNullOrEmpty(culture.Name))
			{
				yield return culture.Name;
				culture = culture.Parent;
			}
			yield break;
		}

		// Token: 0x060015EE RID: 5614 RVA: 0x0008A9F5 File Offset: 0x00088BF5
		internal UpdatableHelpUri GetHelpInfoUri(UpdatableHelpModuleInfo module, CultureInfo culture)
		{
			return new UpdatableHelpUri(module.ModuleName, module.ModuleGuid, culture, this.ResolveUri(module.HelpInfoUri, false));
		}

		// Token: 0x060015EF RID: 5615 RVA: 0x0008AA18 File Offset: 0x00088C18
		internal UpdatableHelpInfo GetHelpInfo(UpdatableHelpCommandType commandType, string uri, string moduleName, Guid moduleGuid, string culture)
		{
			UpdatableHelpInfo result;
			try
			{
				this.OnProgressChanged(this, new UpdatableHelpProgressEventArgs(this._currentModule, commandType, StringUtil.Format(HelpDisplayStrings.UpdateProgressLocating, new object[0]), 0));
				string xml = this._webClient.DownloadString(uri);
				UpdatableHelpInfo updatableHelpInfo = this.CreateHelpInfo(xml, moduleName, moduleGuid, culture, null, true, true, false);
				result = updatableHelpInfo;
			}
			catch (WebException)
			{
				result = null;
			}
			finally
			{
				this.OnProgressChanged(this, new UpdatableHelpProgressEventArgs(this._currentModule, commandType, StringUtil.Format(HelpDisplayStrings.UpdateProgressLocating, new object[0]), 100));
			}
			return result;
		}

		// Token: 0x060015F0 RID: 5616 RVA: 0x0008AAC0 File Offset: 0x00088CC0
		private string ResolveUri(string baseUri, bool verbose)
		{
			if (Directory.Exists(baseUri) || baseUri.EndsWith("/", StringComparison.OrdinalIgnoreCase))
			{
				if (verbose)
				{
					this._cmdlet.WriteVerbose(StringUtil.Format(RemotingErrorIdStrings.URIRedirectWarningToHost, baseUri));
				}
				return baseUri;
			}
			if (verbose)
			{
				this._cmdlet.WriteVerbose(StringUtil.Format(HelpDisplayStrings.UpdateHelpResolveUriVerbose, baseUri));
			}
			string text = baseUri;
			try
			{
				for (int i = 0; i < 10; i++)
				{
					if (this._stopping)
					{
						return text;
					}
					HttpWebRequest httpWebRequest = (HttpWebRequest)WebRequest.Create(text);
					httpWebRequest.AllowAutoRedirect = false;
					httpWebRequest.Timeout = 30000;
					HttpWebResponse httpWebResponse = (HttpWebResponse)httpWebRequest.GetResponse();
					WebHeaderCollection headers = httpWebResponse.Headers;
					try
					{
						if (httpWebResponse.StatusCode == HttpStatusCode.Found || httpWebResponse.StatusCode == HttpStatusCode.Found || httpWebResponse.StatusCode == HttpStatusCode.MovedPermanently || httpWebResponse.StatusCode == HttpStatusCode.MovedPermanently)
						{
							Uri uri = new Uri(headers["Location"], UriKind.RelativeOrAbsolute);
							if (uri.IsAbsoluteUri)
							{
								text = uri.ToString();
							}
							else
							{
								text = text.Replace(httpWebRequest.Address.AbsolutePath, uri.ToString());
							}
							text = text.Trim();
							if (verbose)
							{
								this._cmdlet.WriteVerbose(StringUtil.Format(RemotingErrorIdStrings.URIRedirectWarningToHost, text));
							}
							if (text.EndsWith("/", StringComparison.OrdinalIgnoreCase))
							{
								return text;
							}
						}
						else if (httpWebResponse.StatusCode == HttpStatusCode.OK)
						{
							if (text.EndsWith("/", StringComparison.OrdinalIgnoreCase))
							{
								return text;
							}
							throw new UpdatableHelpSystemException("InvalidHelpInfoUri", StringUtil.Format(HelpDisplayStrings.InvalidHelpInfoUri, text), ErrorCategory.InvalidOperation, null, null);
						}
					}
					finally
					{
						httpWebResponse.Close();
					}
				}
			}
			catch (UriFormatException ex)
			{
				throw new UpdatableHelpSystemException("InvalidUriFormat", ex.Message, ErrorCategory.InvalidData, null, ex);
			}
			throw new UpdatableHelpSystemException("TooManyRedirections", StringUtil.Format(HelpDisplayStrings.TooManyRedirections, new object[0]), ErrorCategory.InvalidOperation, null, null);
		}

		// Token: 0x060015F1 RID: 5617 RVA: 0x0008ACC8 File Offset: 0x00088EC8
		internal UpdatableHelpInfo CreateHelpInfo(string xml, string moduleName, Guid moduleGuid, string currentCulture, string pathOverride, bool verbose, bool shouldResolveUri, bool ignoreValidationException)
		{
			XmlDocument xmlDocument = null;
			try
			{
				xmlDocument = this.CreateValidXmlDocument(xml, "http://schemas.microsoft.com/powershell/help/2010/05", "<?xml version=\"1.0\" encoding=\"utf-8\"?>\r\n            <xs:schema attributeFormDefault=\"unqualified\" elementFormDefault=\"qualified\"\r\n                targetNamespace=\"http://schemas.microsoft.com/powershell/help/2010/05\" xmlns:xs=\"http://www.w3.org/2001/XMLSchema\">\r\n                <xs:element name=\"HelpInfo\">\r\n                    <xs:complexType>\r\n                        <xs:sequence>\r\n                            <xs:element name=\"HelpContentURI\" type=\"xs:anyURI\" minOccurs=\"1\" maxOccurs=\"1\" />\r\n                            <xs:element name=\"SupportedUICultures\" minOccurs=\"1\" maxOccurs=\"1\">\r\n                                <xs:complexType>\r\n                                    <xs:sequence>\r\n                                        <xs:element name=\"UICulture\" minOccurs=\"1\" maxOccurs=\"unbounded\">\r\n                                            <xs:complexType>\r\n                                                <xs:sequence>\r\n                                                    <xs:element name=\"UICultureName\" type=\"xs:language\" minOccurs=\"1\" maxOccurs=\"1\" />\r\n                                                    <xs:element name=\"UICultureVersion\" type=\"xs:string\" minOccurs=\"1\" maxOccurs=\"1\" />\r\n                                                </xs:sequence>\r\n                                            </xs:complexType>\r\n                                        </xs:element>\r\n                                    </xs:sequence>\r\n                                </xs:complexType>\r\n                            </xs:element>\r\n                        </xs:sequence>\r\n                    </xs:complexType>\r\n                </xs:element>\r\n            </xs:schema>", new ValidationEventHandler(this.HelpInfoValidationHandler), true);
			}
			catch (UpdatableHelpSystemException ex)
			{
				if (ignoreValidationException && "HelpInfoXmlValidationFailure".Equals(ex.FullyQualifiedErrorId, StringComparison.Ordinal))
				{
					return null;
				}
				throw;
			}
			catch (XmlException ex2)
			{
				if (ignoreValidationException)
				{
					return null;
				}
				throw new UpdatableHelpSystemException("HelpInfoXmlValidationFailure", ex2.Message, ErrorCategory.InvalidData, null, ex2);
			}
			string resolvedUri = pathOverride;
			string innerText = xmlDocument["HelpInfo"]["HelpContentURI"].InnerText;
			if (string.IsNullOrEmpty(pathOverride))
			{
				if (shouldResolveUri)
				{
					resolvedUri = this.ResolveUri(innerText, verbose);
				}
				else
				{
					resolvedUri = innerText;
				}
			}
			XmlNodeList childNodes = xmlDocument["HelpInfo"]["SupportedUICultures"].ChildNodes;
			CultureSpecificUpdatableHelp[] array = new CultureSpecificUpdatableHelp[childNodes.Count];
			for (int i = 0; i < childNodes.Count; i++)
			{
				array[i] = new CultureSpecificUpdatableHelp(new CultureInfo(childNodes[i]["UICultureName"].InnerText), new Version(childNodes[i]["UICultureVersion"].InnerText));
			}
			UpdatableHelpInfo updatableHelpInfo = new UpdatableHelpInfo(innerText, array);
			if (!string.IsNullOrEmpty(currentCulture))
			{
				WildcardOptions options = WildcardOptions.IgnoreCase | WildcardOptions.CultureInvariant;
				IEnumerable<WildcardPattern> patterns = SessionStateUtilities.CreateWildcardsFromStrings(new string[]
				{
					currentCulture
				}, options);
				for (int j = 0; j < array.Length; j++)
				{
					if (SessionStateUtilities.MatchesAnyWildcardPattern(array[j].Culture.Name, patterns, true))
					{
						updatableHelpInfo.HelpContentUriCollection.Add(new UpdatableHelpUri(moduleName, moduleGuid, array[j].Culture, resolvedUri));
					}
				}
			}
			if (!string.IsNullOrEmpty(currentCulture) && updatableHelpInfo.HelpContentUriCollection.Count == 0)
			{
				throw new UpdatableHelpSystemException("HelpCultureNotSupported", StringUtil.Format(HelpDisplayStrings.HelpCultureNotSupported, currentCulture, updatableHelpInfo.GetSupportedCultures()), ErrorCategory.InvalidOperation, null, null);
			}
			return updatableHelpInfo;
		}

		// Token: 0x060015F2 RID: 5618 RVA: 0x0008AED0 File Offset: 0x000890D0
		private XmlDocument CreateValidXmlDocument(string xml, string ns, string schema, ValidationEventHandler handler, bool helpInfo)
		{
			XmlReaderSettings xmlReaderSettings = new XmlReaderSettings();
			xmlReaderSettings.Schemas.Add(ns, new XmlTextReader(new StringReader(schema)));
			xmlReaderSettings.ValidationType = ValidationType.Schema;
			XmlReader reader = XmlReader.Create(new StringReader(xml), xmlReaderSettings);
			XmlDocument xmlDocument = new XmlDocument();
			try
			{
				xmlDocument.Load(reader);
				xmlDocument.Validate(handler);
			}
			catch (XmlSchemaValidationException ex)
			{
				if (helpInfo)
				{
					throw new UpdatableHelpSystemException("HelpInfoXmlValidationFailure", StringUtil.Format(HelpDisplayStrings.HelpInfoXmlValidationFailure, ex.Message), ErrorCategory.InvalidData, null, ex);
				}
				throw new UpdatableHelpSystemException("HelpContentXmlValidationFailure", StringUtil.Format(HelpDisplayStrings.HelpContentXmlValidationFailure, ex.Message), ErrorCategory.InvalidData, null, ex);
			}
			return xmlDocument;
		}

		// Token: 0x060015F3 RID: 5619 RVA: 0x0008AF78 File Offset: 0x00089178
		private void HelpInfoValidationHandler(object sender, ValidationEventArgs arg)
		{
			switch (arg.Severity)
			{
			case XmlSeverityType.Error:
				throw new UpdatableHelpSystemException("HelpInfoXmlValidationFailure", StringUtil.Format(HelpDisplayStrings.HelpInfoXmlValidationFailure, new object[0]), ErrorCategory.InvalidData, null, arg.Exception);
			case XmlSeverityType.Warning:
				return;
			default:
				return;
			}
		}

		// Token: 0x060015F4 RID: 5620 RVA: 0x0008AFC0 File Offset: 0x000891C0
		private void HelpContentValidationHandler(object sender, ValidationEventArgs arg)
		{
			switch (arg.Severity)
			{
			case XmlSeverityType.Error:
				throw new UpdatableHelpSystemException("HelpContentXmlValidationFailure", StringUtil.Format(HelpDisplayStrings.HelpContentXmlValidationFailure, new object[0]), ErrorCategory.InvalidData, null, arg.Exception);
			case XmlSeverityType.Warning:
				return;
			default:
				return;
			}
		}

		// Token: 0x060015F5 RID: 5621 RVA: 0x0008B006 File Offset: 0x00089206
		internal void CancelDownload()
		{
			if (this._webClient.IsBusy)
			{
				this._webClient.CancelAsync();
				this._completed = true;
				this._completionEvent.Set();
			}
			this._stopping = true;
		}

		// Token: 0x060015F6 RID: 5622 RVA: 0x0008B03C File Offset: 0x0008923C
		internal bool DownloadAndInstallHelpContent(UpdatableHelpCommandType commandType, ExecutionContext context, Collection<string> destPaths, string fileName, CultureInfo culture, string helpContentUri, string xsdPath, out Collection<string> installed)
		{
			if (this._stopping)
			{
				installed = new Collection<string>();
				return false;
			}
			string text = Path.Combine(Path.GetTempPath(), Path.GetFileNameWithoutExtension(Path.GetRandomFileName()));
			if (!this.DownloadHelpContent(commandType, text, helpContentUri, fileName, culture.Name))
			{
				installed = new Collection<string>();
				return false;
			}
			this.InstallHelpContent(commandType, context, text, destPaths, fileName, text, culture, xsdPath, out installed);
			return true;
		}

		// Token: 0x060015F7 RID: 5623 RVA: 0x0008B0A4 File Offset: 0x000892A4
		internal bool DownloadHelpContent(UpdatableHelpCommandType commandType, string path, string helpContentUri, string fileName, string culture)
		{
			if (this._stopping)
			{
				return false;
			}
			if (!Directory.Exists(path))
			{
				Directory.CreateDirectory(path);
			}
			this.OnProgressChanged(this, new UpdatableHelpProgressEventArgs(this._currentModule, commandType, StringUtil.Format(HelpDisplayStrings.UpdateProgressConnecting, new object[0]), 0));
			string uri = helpContentUri + fileName;
			return this.DownloadHelpContentWebClient(uri, Path.Combine(path, fileName), culture, commandType);
		}

		// Token: 0x060015F8 RID: 5624 RVA: 0x0008B110 File Offset: 0x00089310
		private bool DownloadHelpContentWebClient(string uri, string fileName, string culture, UpdatableHelpCommandType commandType)
		{
			this._webClient.DownloadFileAsync(new Uri(uri), fileName, culture);
			this.OnProgressChanged(this, new UpdatableHelpProgressEventArgs(this._currentModule, commandType, StringUtil.Format(HelpDisplayStrings.UpdateProgressConnecting, new object[0]), 100));
			while (!this._completed || this._webClient.IsBusy)
			{
				this._completionEvent.WaitOne();
				this.SendProgressEvents(commandType);
			}
			return this._errors.Count == 0;
		}

		// Token: 0x060015F9 RID: 5625 RVA: 0x0008B194 File Offset: 0x00089394
		private void SendProgressEvents(UpdatableHelpCommandType commandType)
		{
			lock (this._syncObject)
			{
				if (this._progressEvents.Count > 0)
				{
					foreach (UpdatableHelpProgressEventArgs updatableHelpProgressEventArgs in this._progressEvents)
					{
						updatableHelpProgressEventArgs.CommandType = commandType;
						this.OnProgressChanged(this, updatableHelpProgressEventArgs);
					}
					this._progressEvents.Clear();
				}
			}
		}

		// Token: 0x060015FA RID: 5626 RVA: 0x0008B230 File Offset: 0x00089430
		internal void GenerateHelpInfo(string moduleName, Guid moduleGuid, string contentUri, string culture, Version version, string destPath, string fileName, bool force)
		{
			if (this._stopping)
			{
				return;
			}
			string path = Path.Combine(destPath, fileName);
			if (force)
			{
				this.RemoveReadOnly(path);
			}
			UpdatableHelpInfo updatableHelpInfo = null;
			string text = UpdatableHelpSystem.LoadStringFromPath(this._cmdlet, path, null);
			if (text != null)
			{
				updatableHelpInfo = this.CreateHelpInfo(text, moduleName, moduleGuid, null, null, false, false, force);
			}
			using (FileStream fileStream = new FileStream(path, FileMode.Create, FileAccess.Write))
			{
				using (XmlWriter xmlWriter = XmlWriter.Create(fileStream, new XmlWriterSettings
				{
					Encoding = Encoding.UTF8,
					Indent = true
				}))
				{
					xmlWriter.WriteStartDocument();
					xmlWriter.WriteStartElement("HelpInfo", "http://schemas.microsoft.com/powershell/help/2010/05");
					xmlWriter.WriteStartElement("HelpContentURI");
					xmlWriter.WriteValue(contentUri);
					xmlWriter.WriteEndElement();
					xmlWriter.WriteStartElement("SupportedUICultures");
					bool flag = false;
					if (updatableHelpInfo != null)
					{
						foreach (CultureSpecificUpdatableHelp cultureSpecificUpdatableHelp in updatableHelpInfo.UpdatableHelpItems)
						{
							if (cultureSpecificUpdatableHelp.Culture.Name.Equals(culture, StringComparison.OrdinalIgnoreCase))
							{
								if (cultureSpecificUpdatableHelp.Version.Equals(version))
								{
									xmlWriter.WriteStartElement("UICulture");
									xmlWriter.WriteStartElement("UICultureName");
									xmlWriter.WriteValue(cultureSpecificUpdatableHelp.Culture.Name);
									xmlWriter.WriteEndElement();
									xmlWriter.WriteStartElement("UICultureVersion");
									xmlWriter.WriteValue(cultureSpecificUpdatableHelp.Version.ToString());
									xmlWriter.WriteEndElement();
									xmlWriter.WriteEndElement();
								}
								else
								{
									xmlWriter.WriteStartElement("UICulture");
									xmlWriter.WriteStartElement("UICultureName");
									xmlWriter.WriteValue(culture);
									xmlWriter.WriteEndElement();
									xmlWriter.WriteStartElement("UICultureVersion");
									xmlWriter.WriteValue(version.ToString());
									xmlWriter.WriteEndElement();
									xmlWriter.WriteEndElement();
								}
								flag = true;
							}
							else
							{
								xmlWriter.WriteStartElement("UICulture");
								xmlWriter.WriteStartElement("UICultureName");
								xmlWriter.WriteValue(cultureSpecificUpdatableHelp.Culture.Name);
								xmlWriter.WriteEndElement();
								xmlWriter.WriteStartElement("UICultureVersion");
								xmlWriter.WriteValue(cultureSpecificUpdatableHelp.Version.ToString());
								xmlWriter.WriteEndElement();
								xmlWriter.WriteEndElement();
							}
						}
					}
					if (!flag)
					{
						xmlWriter.WriteStartElement("UICulture");
						xmlWriter.WriteStartElement("UICultureName");
						xmlWriter.WriteValue(culture);
						xmlWriter.WriteEndElement();
						xmlWriter.WriteStartElement("UICultureVersion");
						xmlWriter.WriteValue(version.ToString());
						xmlWriter.WriteEndElement();
						xmlWriter.WriteEndElement();
					}
					xmlWriter.WriteEndElement();
					xmlWriter.WriteEndDocument();
				}
			}
		}

		// Token: 0x060015FB RID: 5627 RVA: 0x0008B50C File Offset: 0x0008970C
		private void RemoveReadOnly(string path)
		{
			if (File.Exists(path))
			{
				FileAttributes fileAttributes = File.GetAttributes(path);
				if ((fileAttributes & FileAttributes.ReadOnly) == FileAttributes.ReadOnly)
				{
					fileAttributes &= ~FileAttributes.ReadOnly;
					File.SetAttributes(path, fileAttributes);
				}
			}
		}

		// Token: 0x060015FC RID: 5628 RVA: 0x0008B53C File Offset: 0x0008973C
		internal void InstallHelpContent(UpdatableHelpCommandType commandType, ExecutionContext context, string sourcePath, Collection<string> destPaths, string fileName, string tempPath, CultureInfo culture, string xsdPath, out Collection<string> installed)
		{
			installed = new Collection<string>();
			if (this._stopping)
			{
				installed = new Collection<string>();
				return;
			}
			if (!Directory.Exists(tempPath))
			{
				Directory.CreateDirectory(tempPath);
			}
			try
			{
				this.OnProgressChanged(this, new UpdatableHelpProgressEventArgs(this._currentModule, commandType, StringUtil.Format(HelpDisplayStrings.UpdateProgressInstalling, new object[0]), 0));
				string text = Path.Combine(sourcePath, fileName);
				if (!File.Exists(text))
				{
					throw new UpdatableHelpSystemException("HelpContentNotFound", StringUtil.Format(HelpDisplayStrings.HelpContentNotFound, new object[0]), ErrorCategory.ResourceUnavailable, null, null);
				}
				string text2 = Path.Combine(tempPath, Path.GetFileNameWithoutExtension(fileName));
				if (Directory.Exists(text2))
				{
					Directory.Delete(text2, true);
				}
				bool flag = true;
				this.UnzipHelpContent(context, text, text2, out flag);
				if (flag)
				{
					this.ValidateAndCopyHelpContent(text2, destPaths, culture.Name, xsdPath, out installed);
				}
			}
			finally
			{
				this.OnProgressChanged(this, new UpdatableHelpProgressEventArgs(this._currentModule, commandType, StringUtil.Format(HelpDisplayStrings.UpdateProgressInstalling, new object[0]), 100));
				try
				{
					if (Directory.Exists(tempPath))
					{
						Directory.Delete(tempPath);
					}
				}
				catch (IOException)
				{
				}
				catch (UnauthorizedAccessException)
				{
				}
				catch (ArgumentException)
				{
				}
			}
		}

		// Token: 0x060015FD RID: 5629 RVA: 0x0008B68C File Offset: 0x0008988C
		private void UnzipHelpContent(ExecutionContext context, string srcPath, string destPath, out bool needToCopy)
		{
			needToCopy = true;
			if (!Directory.Exists(destPath))
			{
				Directory.CreateDirectory(destPath);
			}
			string text = Path.GetDirectoryName(srcPath);
			if (!text.EndsWith("\\", StringComparison.Ordinal))
			{
				text += "\\";
			}
			if (!destPath.EndsWith("\\", StringComparison.Ordinal))
			{
				destPath += "\\";
			}
			if (!CabinetExtractorFactory.GetCabinetExtractor().Extract(Path.GetFileName(srcPath), text, destPath))
			{
				throw new UpdatableHelpSystemException("UnableToExtract", StringUtil.Format(HelpDisplayStrings.UnzipFailure, new object[0]), ErrorCategory.InvalidOperation, null, null);
			}
			string[] files = Directory.GetFiles(destPath);
			if (files.Length == 1)
			{
				string fileName = Path.GetFileName(files[0]);
				if (string.IsNullOrEmpty(fileName) || !fileName.Equals("placeholder.txt", StringComparison.OrdinalIgnoreCase))
				{
					return;
				}
				FileInfo fileInfo = new FileInfo(files[0]);
				if (fileInfo.Length != 0L)
				{
					return;
				}
				needToCopy = false;
				try
				{
					File.Delete(files[0]);
					string directoryName = Path.GetDirectoryName(files[0]);
					if (!string.IsNullOrEmpty(directoryName))
					{
						Directory.Delete(directoryName);
					}
					return;
				}
				catch (FileNotFoundException)
				{
					return;
				}
				catch (DirectoryNotFoundException)
				{
					return;
				}
				catch (UnauthorizedAccessException)
				{
					return;
				}
				catch (SecurityException)
				{
					return;
				}
				catch (ArgumentNullException)
				{
					return;
				}
				catch (ArgumentException)
				{
					return;
				}
				catch (PathTooLongException)
				{
					return;
				}
				catch (NotSupportedException)
				{
					return;
				}
				catch (IOException)
				{
					return;
				}
			}
			foreach (string text2 in files)
			{
				if (File.Exists(text2))
				{
					FileInfo fileInfo2 = new FileInfo(text2);
					if ((fileInfo2.Attributes & FileAttributes.ReadOnly) == FileAttributes.ReadOnly)
					{
						fileInfo2.Attributes &= ~FileAttributes.ReadOnly;
					}
				}
			}
		}

		// Token: 0x060015FE RID: 5630 RVA: 0x0008B860 File Offset: 0x00089A60
		private void ValidateAndCopyHelpContent(string sourcePath, Collection<string> destPaths, string culture, string xsdPath, out Collection<string> installed)
		{
			installed = new Collection<string>();
			string text = UpdatableHelpSystem.LoadStringFromPath(this._cmdlet, xsdPath, null);
			foreach (string path in Directory.GetFiles(sourcePath))
			{
				if (!string.Equals(Path.GetExtension(path), ".xml", StringComparison.OrdinalIgnoreCase) && !string.Equals(Path.GetExtension(path), ".txt", StringComparison.OrdinalIgnoreCase))
				{
					throw new UpdatableHelpSystemException("HelpContentContainsInvalidFiles", StringUtil.Format(HelpDisplayStrings.HelpContentContainsInvalidFiles, new object[0]), ErrorCategory.InvalidData, null, null);
				}
			}
			string[] files2 = Directory.GetFiles(sourcePath);
			int j = 0;
			while (j < files2.Length)
			{
				string text2 = files2[j];
				if (!string.Equals(Path.GetExtension(text2), ".xml", StringComparison.OrdinalIgnoreCase))
				{
					goto IL_32F;
				}
				if (text == null)
				{
					throw new ItemNotFoundException(StringUtil.Format(HelpDisplayStrings.HelpContentXsdNotFound, xsdPath));
				}
				string s = UpdatableHelpSystem.LoadStringFromPath(this._cmdlet, text2, null);
				XmlReader reader = XmlReader.Create(new StringReader(s));
				XmlDocument xmlDocument = new XmlDocument();
				xmlDocument.Load(reader);
				if (xmlDocument.ChildNodes.Count != 1 && xmlDocument.ChildNodes.Count != 2)
				{
					throw new UpdatableHelpSystemException("HelpContentXmlValidationFailure", StringUtil.Format(HelpDisplayStrings.HelpContentXmlValidationFailure, HelpDisplayStrings.RootElementMustBeHelpItems), ErrorCategory.InvalidData, null, null);
				}
				XmlNode xmlNode = null;
				if (xmlDocument.DocumentElement != null && xmlDocument.DocumentElement.LocalName.Equals("providerHelp", StringComparison.OrdinalIgnoreCase))
				{
					xmlNode = xmlDocument;
				}
				else if (xmlDocument.ChildNodes.Count == 1)
				{
					if (!xmlDocument.ChildNodes[0].LocalName.Equals("helpItems", StringComparison.OrdinalIgnoreCase))
					{
						throw new UpdatableHelpSystemException("HelpContentXmlValidationFailure", StringUtil.Format(HelpDisplayStrings.HelpContentXmlValidationFailure, HelpDisplayStrings.RootElementMustBeHelpItems), ErrorCategory.InvalidData, null, null);
					}
					xmlNode = xmlDocument.ChildNodes[0];
				}
				else if (xmlDocument.ChildNodes.Count == 2)
				{
					if (!xmlDocument.ChildNodes[1].LocalName.Equals("helpItems", StringComparison.OrdinalIgnoreCase))
					{
						throw new UpdatableHelpSystemException("HelpContentXmlValidationFailure", StringUtil.Format(HelpDisplayStrings.HelpContentXmlValidationFailure, HelpDisplayStrings.RootElementMustBeHelpItems), ErrorCategory.InvalidData, null, null);
					}
					xmlNode = xmlDocument.ChildNodes[1];
				}
				string text3 = "http://schemas.microsoft.com/maml/2004/10";
				using (IEnumerator enumerator = xmlNode.ChildNodes.GetEnumerator())
				{
					while (enumerator.MoveNext())
					{
						object obj = enumerator.Current;
						XmlNode xmlNode2 = (XmlNode)obj;
						if (xmlNode2.NodeType == XmlNodeType.Element)
						{
							if (!xmlNode2.LocalName.Equals("providerHelp", StringComparison.OrdinalIgnoreCase))
							{
								if (xmlNode2.LocalName.Equals("para", StringComparison.OrdinalIgnoreCase))
								{
									if (!xmlNode2.NamespaceURI.Equals("http://schemas.microsoft.com/maml/2004/10", StringComparison.OrdinalIgnoreCase))
									{
										throw new UpdatableHelpSystemException("HelpContentXmlValidationFailure", StringUtil.Format(HelpDisplayStrings.HelpContentXmlValidationFailure, StringUtil.Format(HelpDisplayStrings.HelpContentMustBeInTargetNamespace, text3)), ErrorCategory.InvalidData, null, null);
									}
									continue;
								}
								else if (!xmlNode2.NamespaceURI.Equals("http://schemas.microsoft.com/maml/dev/command/2004/10", StringComparison.OrdinalIgnoreCase) && !xmlNode2.NamespaceURI.Equals("http://schemas.microsoft.com/maml/dev/dscResource/2004/10", StringComparison.OrdinalIgnoreCase))
								{
									throw new UpdatableHelpSystemException("HelpContentXmlValidationFailure", StringUtil.Format(HelpDisplayStrings.HelpContentXmlValidationFailure, StringUtil.Format(HelpDisplayStrings.HelpContentMustBeInTargetNamespace, text3)), ErrorCategory.InvalidData, null, null);
								}
							}
							this.CreateValidXmlDocument(xmlNode2.OuterXml, text3, text, new ValidationEventHandler(this.HelpContentValidationHandler), false);
						}
					}
					goto IL_399;
				}
				goto IL_32F;
				IL_399:
				foreach (string path2 in destPaths)
				{
					string text4 = Path.Combine(path2, culture);
					if (!Directory.Exists(text4))
					{
						Directory.CreateDirectory(text4);
					}
					string text5 = Path.Combine(text4, Path.GetFileName(text2));
					FileAttributes? fileAttributes = null;
					try
					{
						if (File.Exists(text5) && this._cmdlet.Force)
						{
							FileInfo fileInfo = new FileInfo(text5);
							if ((fileInfo.Attributes & FileAttributes.ReadOnly) == FileAttributes.ReadOnly)
							{
								fileAttributes = new FileAttributes?(fileInfo.Attributes);
								fileInfo.Attributes &= ~FileAttributes.ReadOnly;
							}
						}
						File.Copy(text2, text5, true);
					}
					finally
					{
						if (fileAttributes != null)
						{
							File.SetAttributes(text5, fileAttributes.Value);
						}
					}
					installed.Add(text5);
				}
				j++;
				continue;
				IL_32F:
				if (!string.Equals(Path.GetExtension(text2), ".txt", StringComparison.OrdinalIgnoreCase))
				{
					goto IL_399;
				}
				FileStream fileStream = new FileStream(text2, FileMode.Open, FileAccess.Read);
				if (fileStream.Length <= 2L)
				{
					goto IL_399;
				}
				byte[] array = new byte[2];
				fileStream.Read(array, 0, 2);
				if (array[0] == 77 && array[1] == 90)
				{
					throw new UpdatableHelpSystemException("HelpContentContainsInvalidFiles", StringUtil.Format(HelpDisplayStrings.HelpContentContainsInvalidFiles, new object[0]), ErrorCategory.InvalidData, null, null);
				}
				goto IL_399;
			}
		}

		// Token: 0x060015FF RID: 5631 RVA: 0x0008BD1C File Offset: 0x00089F1C
		internal static string LoadStringFromPath(PSCmdlet cmdlet, string path, PSCredential credential)
		{
			if (credential != null)
			{
				using (UpdatableHelpSystemDrive updatableHelpSystemDrive = new UpdatableHelpSystemDrive(cmdlet, Path.GetDirectoryName(path), credential))
				{
					string text = Path.Combine(Path.GetTempPath(), Path.GetFileNameWithoutExtension(Path.GetTempFileName()));
					if (!cmdlet.InvokeProvider.Item.Exists(Path.Combine(updatableHelpSystemDrive.DriveName, Path.GetFileName(path))))
					{
						return null;
					}
					cmdlet.InvokeProvider.Item.Copy(new string[]
					{
						Path.Combine(updatableHelpSystemDrive.DriveName, Path.GetFileName(path))
					}, text, false, CopyContainers.CopyTargetContainer, true, true);
					path = text;
				}
			}
			if (File.Exists(path))
			{
				using (FileStream fileStream = new FileStream(path, FileMode.Open, FileAccess.Read))
				{
					StreamReader streamReader = new StreamReader(fileStream);
					return streamReader.ReadToEnd();
				}
			}
			return null;
		}

		// Token: 0x06001600 RID: 5632 RVA: 0x0008BE0C File Offset: 0x0008A00C
		internal string GetDefaultSourcePath()
		{
			try
			{
				using (RegistryKey registryKey = Registry.LocalMachine.OpenSubKey("Software\\Policies\\Microsoft\\Windows\\PowerShell\\UpdatableHelp"))
				{
					if (registryKey != null)
					{
						object value = registryKey.GetValue("DefaultSourcePath", null, RegistryValueOptions.None);
						if (value != null)
						{
							return value as string;
						}
					}
				}
			}
			catch (SecurityException)
			{
				return null;
			}
			return null;
		}

		// Token: 0x06001601 RID: 5633 RVA: 0x0008BE78 File Offset: 0x0008A078
		internal static void SetDisablePromptToUpdateHelp()
		{
			try
			{
				using (RegistryKey registryKey = Registry.LocalMachine.OpenSubKey("Software\\Microsoft\\PowerShell", true))
				{
					if (registryKey != null)
					{
						registryKey.SetValue("DisablePromptToUpdateHelp", 1, RegistryValueKind.DWord);
					}
				}
				using (RegistryKey registryKey2 = Registry.LocalMachine.OpenSubKey("Software\\Wow6432Node\\Microsoft\\PowerShell", true))
				{
					if (registryKey2 != null)
					{
						registryKey2.SetValue("DisablePromptToUpdateHelp", 1, RegistryValueKind.DWord);
					}
				}
			}
			catch (UnauthorizedAccessException)
			{
			}
			catch (SecurityException)
			{
			}
		}

		// Token: 0x06001602 RID: 5634 RVA: 0x0008BF28 File Offset: 0x0008A128
		internal static bool ShouldPromptToUpdateHelp()
		{
			bool result;
			try
			{
				if (!Utils.IsAdministrator())
				{
					result = false;
				}
				else
				{
					using (RegistryKey registryKey = Registry.LocalMachine.OpenSubKey("Software\\Microsoft\\PowerShell"))
					{
						if (registryKey != null)
						{
							object value = registryKey.GetValue("DisablePromptToUpdateHelp", null, RegistryValueOptions.None);
							int num;
							if (value == null)
							{
								result = true;
							}
							else if (LanguagePrimitives.TryConvertTo<int>(value, out num))
							{
								result = (num != 1);
							}
							else
							{
								result = true;
							}
						}
						else
						{
							result = true;
						}
					}
				}
			}
			catch (SecurityException)
			{
				result = false;
			}
			return result;
		}

		// Token: 0x14000012 RID: 18
		// (add) Token: 0x06001603 RID: 5635 RVA: 0x0008BFB0 File Offset: 0x0008A1B0
		// (remove) Token: 0x06001604 RID: 5636 RVA: 0x0008BFE8 File Offset: 0x0008A1E8
		internal event EventHandler<UpdatableHelpProgressEventArgs> OnProgressChanged;

		// Token: 0x06001605 RID: 5637 RVA: 0x0008C020 File Offset: 0x0008A220
		private void HandleDownloadFileCompleted(object sender, AsyncCompletedEventArgs e)
		{
			if (this._stopping)
			{
				return;
			}
			if (!e.Cancelled)
			{
				if (e.Error != null)
				{
					if (e.Error is WebException)
					{
						this._errors.Add(new UpdatableHelpSystemException("HelpContentNotFound", StringUtil.Format(HelpDisplayStrings.HelpContentNotFound, e.UserState.ToString()), ErrorCategory.ResourceUnavailable, null, null));
					}
					else
					{
						this._errors.Add(e.Error);
					}
				}
				else
				{
					lock (this._syncObject)
					{
						this._progressEvents.Add(new UpdatableHelpProgressEventArgs(this._currentModule, StringUtil.Format(HelpDisplayStrings.UpdateProgressDownloading, new object[0]), 100));
					}
				}
				this._completed = true;
				this._completionEvent.Set();
			}
		}

		// Token: 0x06001606 RID: 5638 RVA: 0x0008C104 File Offset: 0x0008A304
		private void HandleDownloadProgressChanged(object sender, DownloadProgressChangedEventArgs e)
		{
			if (this._stopping)
			{
				return;
			}
			lock (this._syncObject)
			{
				this._progressEvents.Add(new UpdatableHelpProgressEventArgs(this._currentModule, StringUtil.Format(HelpDisplayStrings.UpdateProgressDownloading, new object[0]), e.ProgressPercentage));
			}
			this._completionEvent.Set();
		}

		// Token: 0x0400094F RID: 2383
		internal const string DisablePromptToUpdateHelpRegPath = "Software\\Microsoft\\PowerShell";

		// Token: 0x04000950 RID: 2384
		internal const string DisablePromptToUpdateHelpRegPath32 = "Software\\Wow6432Node\\Microsoft\\PowerShell";

		// Token: 0x04000951 RID: 2385
		internal const string DisablePromptToUpdateHelpRegKey = "DisablePromptToUpdateHelp";

		// Token: 0x04000952 RID: 2386
		internal const string DefaultSourcePathRegPath = "Software\\Policies\\Microsoft\\Windows\\PowerShell\\UpdatableHelp";

		// Token: 0x04000953 RID: 2387
		internal const string DefaultSourcePathRegKey = "DefaultSourcePath";

		// Token: 0x04000954 RID: 2388
		private const string HelpInfoXmlSchema = "<?xml version=\"1.0\" encoding=\"utf-8\"?>\r\n            <xs:schema attributeFormDefault=\"unqualified\" elementFormDefault=\"qualified\"\r\n                targetNamespace=\"http://schemas.microsoft.com/powershell/help/2010/05\" xmlns:xs=\"http://www.w3.org/2001/XMLSchema\">\r\n                <xs:element name=\"HelpInfo\">\r\n                    <xs:complexType>\r\n                        <xs:sequence>\r\n                            <xs:element name=\"HelpContentURI\" type=\"xs:anyURI\" minOccurs=\"1\" maxOccurs=\"1\" />\r\n                            <xs:element name=\"SupportedUICultures\" minOccurs=\"1\" maxOccurs=\"1\">\r\n                                <xs:complexType>\r\n                                    <xs:sequence>\r\n                                        <xs:element name=\"UICulture\" minOccurs=\"1\" maxOccurs=\"unbounded\">\r\n                                            <xs:complexType>\r\n                                                <xs:sequence>\r\n                                                    <xs:element name=\"UICultureName\" type=\"xs:language\" minOccurs=\"1\" maxOccurs=\"1\" />\r\n                                                    <xs:element name=\"UICultureVersion\" type=\"xs:string\" minOccurs=\"1\" maxOccurs=\"1\" />\r\n                                                </xs:sequence>\r\n                                            </xs:complexType>\r\n                                        </xs:element>\r\n                                    </xs:sequence>\r\n                                </xs:complexType>\r\n                            </xs:element>\r\n                        </xs:sequence>\r\n                    </xs:complexType>\r\n                </xs:element>\r\n            </xs:schema>";

		// Token: 0x04000955 RID: 2389
		private const string HelpInfoXmlNamespace = "http://schemas.microsoft.com/powershell/help/2010/05";

		// Token: 0x04000956 RID: 2390
		private const string HelpInfoXmlValidationFailure = "HelpInfoXmlValidationFailure";

		// Token: 0x04000957 RID: 2391
		private WebClient _webClient;

		// Token: 0x04000958 RID: 2392
		private AutoResetEvent _completionEvent;

		// Token: 0x04000959 RID: 2393
		private bool _completed;

		// Token: 0x0400095A RID: 2394
		private Collection<UpdatableHelpProgressEventArgs> _progressEvents;

		// Token: 0x0400095B RID: 2395
		private bool _stopping;

		// Token: 0x0400095C RID: 2396
		private object _syncObject;

		// Token: 0x0400095D RID: 2397
		private UpdatableHelpCommandBase _cmdlet;

		// Token: 0x0400095E RID: 2398
		private string _currentModule;

		// Token: 0x0400095F RID: 2399
		private CancellationTokenSource _cancelTokenSource;

		// Token: 0x04000960 RID: 2400
		private Collection<Exception> _errors;
	}
}
