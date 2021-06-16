using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.IO;
using System.Management.Automation;
using System.Management.Automation.Host;
using System.Management.Automation.Internal;
using System.Management.Automation.Runspaces;

namespace Microsoft.PowerShell.Commands.Internal.Format
{
	// Token: 0x02000968 RID: 2408
	internal sealed class TypeInfoDataBaseManager
	{
		// Token: 0x170011D6 RID: 4566
		// (get) Token: 0x0600584A RID: 22602 RVA: 0x001CB3A6 File Offset: 0x001C95A6
		internal TypeInfoDataBase Database
		{
			get
			{
				return this.dataBase;
			}
		}

		// Token: 0x170011D7 RID: 4567
		// (get) Token: 0x0600584B RID: 22603 RVA: 0x001CB3AE File Offset: 0x001C95AE
		// (set) Token: 0x0600584C RID: 22604 RVA: 0x001CB3B6 File Offset: 0x001C95B6
		internal bool DisableFormatTableUpdates
		{
			get
			{
				return this.disableFormatTableUpdates;
			}
			set
			{
				this.disableFormatTableUpdates = value;
			}
		}

		// Token: 0x0600584D RID: 22605 RVA: 0x001CB3BF File Offset: 0x001C95BF
		internal TypeInfoDataBaseManager()
		{
			this.isShared = false;
			this.formatFileList = new List<string>();
		}

		// Token: 0x0600584E RID: 22606 RVA: 0x001CB3F0 File Offset: 0x001C95F0
		internal TypeInfoDataBaseManager(IEnumerable<string> formatFiles, bool isShared, AuthorizationManager authorizationManager, PSHost host)
		{
			this.formatFileList = new List<string>();
			Collection<PSSnapInTypeAndFormatErrors> collection = new Collection<PSSnapInTypeAndFormatErrors>();
			Collection<string> collection2 = new Collection<string>();
			foreach (string text in formatFiles)
			{
				if (string.IsNullOrEmpty(text) || !Path.IsPathRooted(text))
				{
					throw PSTraceSource.NewArgumentException("formatFiles", FormatAndOutXmlLoadingStrings.FormatFileNotRooted, new object[]
					{
						text
					});
				}
				collection.Add(new PSSnapInTypeAndFormatErrors(string.Empty, text)
				{
					Errors = collection2
				});
				this.formatFileList.Add(text);
			}
			MshExpressionFactory expressionFactory = new MshExpressionFactory();
			List<XmlLoaderLoggerEntry> list = null;
			this.LoadFromFile(collection, expressionFactory, true, authorizationManager, host, false, out list);
			this.isShared = isShared;
			if (collection2.Count > 0)
			{
				throw new FormatTableLoadException(collection2);
			}
		}

		// Token: 0x0600584F RID: 22607 RVA: 0x001CB4F0 File Offset: 0x001C96F0
		internal TypeInfoDataBase GetTypeInfoDataBase()
		{
			return this.dataBase;
		}

		// Token: 0x06005850 RID: 22608 RVA: 0x001CB4F8 File Offset: 0x001C96F8
		internal void Add(string formatFile, bool shouldPrepend)
		{
			if (string.IsNullOrEmpty(formatFile) || !Path.IsPathRooted(formatFile))
			{
				throw PSTraceSource.NewArgumentException("formatFile", FormatAndOutXmlLoadingStrings.FormatFileNotRooted, new object[]
				{
					formatFile
				});
			}
			lock (this.formatFileList)
			{
				if (shouldPrepend)
				{
					this.formatFileList.Insert(0, formatFile);
				}
				else
				{
					this.formatFileList.Add(formatFile);
				}
			}
		}

		// Token: 0x06005851 RID: 22609 RVA: 0x001CB57C File Offset: 0x001C977C
		internal void Remove(string formatFile)
		{
			lock (this.formatFileList)
			{
				this.formatFileList.Remove(formatFile);
			}
		}

		// Token: 0x06005852 RID: 22610 RVA: 0x001CB5C4 File Offset: 0x001C97C4
		internal void AddFormatData(IEnumerable<ExtendedTypeDefinition> formatData, bool shouldPrepend)
		{
			Collection<PSSnapInTypeAndFormatErrors> collection = new Collection<PSSnapInTypeAndFormatErrors>();
			Collection<string> collection2 = new Collection<string>();
			if (shouldPrepend)
			{
				foreach (ExtendedTypeDefinition typeDefinition in formatData)
				{
					collection.Add(new PSSnapInTypeAndFormatErrors(string.Empty, typeDefinition)
					{
						Errors = collection2
					});
				}
				if (collection.Count == 0)
				{
					return;
				}
			}
			lock (this.formatFileList)
			{
				foreach (string fullPath in this.formatFileList)
				{
					collection.Add(new PSSnapInTypeAndFormatErrors(string.Empty, fullPath)
					{
						Errors = collection2
					});
				}
			}
			if (!shouldPrepend)
			{
				foreach (ExtendedTypeDefinition typeDefinition2 in formatData)
				{
					collection.Add(new PSSnapInTypeAndFormatErrors(string.Empty, typeDefinition2)
					{
						Errors = collection2
					});
				}
				if (collection.Count == this.formatFileList.Count)
				{
					return;
				}
			}
			MshExpressionFactory expressionFactory = new MshExpressionFactory();
			List<XmlLoaderLoggerEntry> list = null;
			this.LoadFromFile(collection, expressionFactory, false, null, null, false, out list);
			if (collection2.Count > 0)
			{
				throw new FormatTableLoadException(collection2);
			}
		}

		// Token: 0x06005853 RID: 22611 RVA: 0x001CB75C File Offset: 0x001C995C
		internal void Update(AuthorizationManager authorizationManager, PSHost host)
		{
			if (this.DisableFormatTableUpdates)
			{
				return;
			}
			if (this.isShared)
			{
				throw PSTraceSource.NewInvalidOperationException(FormatAndOutXmlLoadingStrings.SharedFormatTableCannotBeUpdated, new object[0]);
			}
			Collection<PSSnapInTypeAndFormatErrors> collection = new Collection<PSSnapInTypeAndFormatErrors>();
			lock (this.formatFileList)
			{
				foreach (string fullPath in this.formatFileList)
				{
					PSSnapInTypeAndFormatErrors item = new PSSnapInTypeAndFormatErrors(string.Empty, fullPath);
					collection.Add(item);
				}
			}
			this.UpdateDataBase(collection, authorizationManager, host, false);
		}

		// Token: 0x06005854 RID: 22612 RVA: 0x001CB81C File Offset: 0x001C9A1C
		internal void UpdateDataBase(Collection<PSSnapInTypeAndFormatErrors> mshsnapins, AuthorizationManager authorizationManager, PSHost host, bool preValidated)
		{
			if (this.DisableFormatTableUpdates)
			{
				return;
			}
			if (this.isShared)
			{
				throw PSTraceSource.NewInvalidOperationException(FormatAndOutXmlLoadingStrings.SharedFormatTableCannotBeUpdated, new object[0]);
			}
			MshExpressionFactory expressionFactory = new MshExpressionFactory();
			List<XmlLoaderLoggerEntry> list = null;
			this.LoadFromFile(mshsnapins, expressionFactory, false, authorizationManager, host, preValidated, out list);
		}

		// Token: 0x06005855 RID: 22613 RVA: 0x001CB864 File Offset: 0x001C9A64
		internal bool LoadFromFile(Collection<PSSnapInTypeAndFormatErrors> files, MshExpressionFactory expressionFactory, bool acceptLoadingErrors, AuthorizationManager authorizationManager, PSHost host, bool preValidated, out List<XmlLoaderLoggerEntry> logEntries)
		{
			bool flag2;
			try
			{
				TypeInfoDataBase typeInfoDataBase = null;
				lock (this.updateDatabaseLock)
				{
					typeInfoDataBase = TypeInfoDataBaseManager.LoadFromFileHelper(files, expressionFactory, authorizationManager, host, preValidated, out logEntries, out flag2);
				}
				lock (this.databaseLock)
				{
					if (acceptLoadingErrors || flag2)
					{
						this.dataBase = typeInfoDataBase;
					}
				}
			}
			finally
			{
				lock (this.databaseLock)
				{
					if (this.dataBase == null)
					{
						TypeInfoDataBase db = new TypeInfoDataBase();
						TypeInfoDataBaseManager.AddPreLoadInstrinsics(db);
						TypeInfoDataBaseManager.AddPostLoadInstrinsics(db);
						this.dataBase = db;
					}
				}
			}
			return flag2;
		}

		// Token: 0x06005856 RID: 22614 RVA: 0x001CB950 File Offset: 0x001C9B50
		private static TypeInfoDataBase LoadFromFileHelper(Collection<PSSnapInTypeAndFormatErrors> files, MshExpressionFactory expressionFactory, AuthorizationManager authorizationManager, PSHost host, bool preValidated, out List<XmlLoaderLoggerEntry> logEntries, out bool success)
		{
			success = true;
			logEntries = new List<XmlLoaderLoggerEntry>();
			TypeInfoDataBase typeInfoDataBase = new TypeInfoDataBase();
			TypeInfoDataBaseManager.AddPreLoadInstrinsics(typeInfoDataBase);
			foreach (PSSnapInTypeAndFormatErrors pssnapInTypeAndFormatErrors in files)
			{
				if (pssnapInTypeAndFormatErrors.FormatData != null)
				{
					using (TypeInfoDataBaseLoader typeInfoDataBaseLoader = new TypeInfoDataBaseLoader())
					{
						if (!typeInfoDataBaseLoader.LoadFormattingData(pssnapInTypeAndFormatErrors.FormatData, typeInfoDataBase, expressionFactory))
						{
							success = false;
						}
						foreach (XmlLoaderLoggerEntry xmlLoaderLoggerEntry in typeInfoDataBaseLoader.LogEntries)
						{
							if (xmlLoaderLoggerEntry.entryType == XmlLoaderLoggerEntry.EntryType.Error)
							{
								string item = StringUtil.Format(FormatAndOutXmlLoadingStrings.MshSnapinQualifiedError, pssnapInTypeAndFormatErrors.PSSnapinName, xmlLoaderLoggerEntry.message);
								pssnapInTypeAndFormatErrors.Errors.Add(item);
							}
						}
						logEntries.AddRange(typeInfoDataBaseLoader.LogEntries);
						continue;
					}
				}
				XmlFileLoadInfo xmlFileLoadInfo = new XmlFileLoadInfo(Path.GetPathRoot(pssnapInTypeAndFormatErrors.FullPath), pssnapInTypeAndFormatErrors.FullPath, pssnapInTypeAndFormatErrors.Errors, pssnapInTypeAndFormatErrors.PSSnapinName);
				using (TypeInfoDataBaseLoader typeInfoDataBaseLoader2 = new TypeInfoDataBaseLoader())
				{
					if (!typeInfoDataBaseLoader2.LoadXmlFile(xmlFileLoadInfo, typeInfoDataBase, expressionFactory, authorizationManager, host, preValidated))
					{
						success = false;
					}
					foreach (XmlLoaderLoggerEntry xmlLoaderLoggerEntry2 in typeInfoDataBaseLoader2.LogEntries)
					{
						if (xmlLoaderLoggerEntry2.entryType == XmlLoaderLoggerEntry.EntryType.Error)
						{
							string item2 = StringUtil.Format(FormatAndOutXmlLoadingStrings.MshSnapinQualifiedError, xmlFileLoadInfo.psSnapinName, xmlLoaderLoggerEntry2.message);
							xmlFileLoadInfo.errors.Add(item2);
							if (xmlLoaderLoggerEntry2.failToLoadFile)
							{
								pssnapInTypeAndFormatErrors.FailToLoadFile = true;
							}
						}
					}
					logEntries.AddRange(typeInfoDataBaseLoader2.LogEntries);
				}
			}
			TypeInfoDataBaseManager.AddPostLoadInstrinsics(typeInfoDataBase);
			return typeInfoDataBase;
		}

		// Token: 0x06005857 RID: 22615 RVA: 0x001CBB98 File Offset: 0x001C9D98
		private static void AddPreLoadInstrinsics(TypeInfoDataBase db)
		{
		}

		// Token: 0x06005858 RID: 22616 RVA: 0x001CBB9C File Offset: 0x001C9D9C
		private static void AddPostLoadInstrinsics(TypeInfoDataBase db)
		{
			FormatShapeSelectionOnType formatShapeSelectionOnType = new FormatShapeSelectionOnType();
			formatShapeSelectionOnType.appliesTo = new AppliesTo();
			formatShapeSelectionOnType.appliesTo.AddAppliesToType("Microsoft.PowerShell.Commands.FormatDataLoadingInfo");
			formatShapeSelectionOnType.formatShape = FormatShape.List;
			db.defaultSettingsSection.shapeSelectionDirectives.formatShapeSelectionOnTypeList.Add(formatShapeSelectionOnType);
		}

		// Token: 0x04002F58 RID: 12120
		private TypeInfoDataBase dataBase;

		// Token: 0x04002F59 RID: 12121
		internal object databaseLock = new object();

		// Token: 0x04002F5A RID: 12122
		internal object updateDatabaseLock = new object();

		// Token: 0x04002F5B RID: 12123
		internal bool isShared;

		// Token: 0x04002F5C RID: 12124
		private List<string> formatFileList;

		// Token: 0x04002F5D RID: 12125
		private bool disableFormatTableUpdates;
	}
}
