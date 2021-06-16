using System;
using System.Collections.Generic;
using System.Globalization;
using System.IO;
using System.Management.Automation;
using System.Management.Automation.Host;
using System.Management.Automation.Internal;
using System.Management.Automation.Security;
using System.Xml;

namespace Microsoft.PowerShell.Commands.Internal.Format
{
	// Token: 0x0200096E RID: 2414
	internal sealed class TypeInfoDataBaseLoader : XmlLoaderBase
	{
		// Token: 0x0600587F RID: 22655 RVA: 0x001CC9BC File Offset: 0x001CABBC
		internal bool LoadXmlFile(XmlFileLoadInfo info, TypeInfoDataBase db, MshExpressionFactory expressionFactory, AuthorizationManager authorizationManager, PSHost host, bool preValidated)
		{
			if (info == null)
			{
				throw PSTraceSource.NewArgumentNullException("info");
			}
			if (info.filePath == null)
			{
				throw PSTraceSource.NewArgumentNullException("info.filePath");
			}
			if (db == null)
			{
				throw PSTraceSource.NewArgumentNullException("db");
			}
			if (expressionFactory == null)
			{
				throw PSTraceSource.NewArgumentNullException("expressionFactory");
			}
			this.displayResourceManagerCache = db.displayResourceManagerCache;
			this.expressionFactory = expressionFactory;
			base.SetDatabaseLoadingInfo(info);
			base.ReportTrace("loading file started");
			bool loadingInfoIsFullyTrusted = false;
			XmlDocument xmlDocument = base.LoadXmlDocumentFromFileLoadingInfo(authorizationManager, host, out loadingInfoIsFullyTrusted);
			if (SystemPolicy.GetSystemLockdownPolicy() == SystemEnforcementMode.Enforce)
			{
				base.SetLoadingInfoIsFullyTrusted(loadingInfoIsFullyTrusted);
			}
			if (xmlDocument == null)
			{
				return false;
			}
			bool flag = this.suppressValidation;
			try
			{
				this.suppressValidation = preValidated;
				try
				{
					this.LoadData(xmlDocument, db);
				}
				catch (TooManyErrorsException)
				{
					return false;
				}
				catch (Exception ex)
				{
					base.ReportError(StringUtil.Format(FormatAndOutXmlLoadingStrings.ErrorInFile, base.FilePath, ex.Message));
					throw;
				}
				if (base.HasErrors)
				{
					return false;
				}
			}
			finally
			{
				this.suppressValidation = flag;
			}
			base.ReportTrace("file loaded with no errors");
			return true;
		}

		// Token: 0x06005880 RID: 22656 RVA: 0x001CCADC File Offset: 0x001CACDC
		internal bool LoadFormattingData(ExtendedTypeDefinition typeDefinition, TypeInfoDataBase db, MshExpressionFactory expressionFactory)
		{
			if (typeDefinition == null)
			{
				throw PSTraceSource.NewArgumentNullException("typeDefinition");
			}
			if (typeDefinition.TypeName == null)
			{
				throw PSTraceSource.NewArgumentNullException("typeDefinition.TypeName");
			}
			if (db == null)
			{
				throw PSTraceSource.NewArgumentNullException("db");
			}
			if (expressionFactory == null)
			{
				throw PSTraceSource.NewArgumentNullException("expressionFactory");
			}
			this.expressionFactory = expressionFactory;
			base.ReportTrace("loading ExtendedTypeDefinition started");
			try
			{
				this.LoadData(typeDefinition, db);
			}
			catch (TooManyErrorsException)
			{
				return false;
			}
			catch (Exception ex)
			{
				base.ReportErrorForLoadingFromObjectModel(StringUtil.Format(FormatAndOutXmlLoadingStrings.ErrorInFormattingData, typeDefinition.TypeName, ex.Message), typeDefinition.TypeName);
				throw;
			}
			if (base.HasErrors)
			{
				return false;
			}
			base.ReportTrace("ExtendedTypeDefinition loaded with no errors");
			return true;
		}

		// Token: 0x06005881 RID: 22657 RVA: 0x001CCBA0 File Offset: 0x001CADA0
		private void LoadData(XmlDocument doc, TypeInfoDataBase db)
		{
			if (doc == null)
			{
				throw PSTraceSource.NewArgumentNullException("doc");
			}
			if (db == null)
			{
				throw PSTraceSource.NewArgumentNullException("db");
			}
			XmlElement documentElement = doc.DocumentElement;
			bool flag = false;
			bool flag2 = false;
			bool flag3 = false;
			bool flag4 = false;
			if (base.MatchNodeName(documentElement, "Configuration"))
			{
				using (base.StackFrame(documentElement))
				{
					foreach (object obj in documentElement.ChildNodes)
					{
						XmlNode xmlNode = (XmlNode)obj;
						if (base.MatchNodeName(xmlNode, "DefaultSettings"))
						{
							if (flag)
							{
								base.ProcessDuplicateNode(xmlNode);
							}
							flag = true;
							this.LoadDefaultSettings(db, xmlNode);
						}
						else if (base.MatchNodeName(xmlNode, "SelectionSets"))
						{
							if (flag2)
							{
								base.ProcessDuplicateNode(xmlNode);
							}
							flag2 = true;
							this.LoadTypeGroups(db, xmlNode);
						}
						else if (base.MatchNodeName(xmlNode, "ViewDefinitions"))
						{
							if (flag3)
							{
								base.ProcessDuplicateNode(xmlNode);
							}
							flag3 = true;
							this.LoadViewDefinitions(db, xmlNode);
						}
						else if (base.MatchNodeName(xmlNode, "Controls"))
						{
							if (flag4)
							{
								base.ProcessDuplicateNode(xmlNode);
							}
							flag4 = true;
							this.LoadControlDefinitions(xmlNode, db.formatControlDefinitionHolder.controlDefinitionList);
						}
						else
						{
							base.ProcessUnknownNode(xmlNode);
						}
					}
					return;
				}
			}
			base.ProcessUnknownNode(documentElement);
		}

		// Token: 0x06005882 RID: 22658 RVA: 0x001CCD1C File Offset: 0x001CAF1C
		private void LoadData(ExtendedTypeDefinition typeDefinition, TypeInfoDataBase db)
		{
			if (typeDefinition == null)
			{
				throw PSTraceSource.NewArgumentNullException("viewDefinition");
			}
			if (db == null)
			{
				throw PSTraceSource.NewArgumentNullException("db");
			}
			int num = 0;
			foreach (FormatViewDefinition formatView in typeDefinition.FormatViewDefinition)
			{
				ViewDefinition viewDefinition = this.LoadViewFromObjectModle(typeDefinition.TypeName, formatView, num++);
				if (viewDefinition != null)
				{
					base.ReportTrace(string.Format(CultureInfo.InvariantCulture, "{0} view {1} is loaded from the 'FormatViewDefinition' at index {2} in 'ExtendedTypeDefinition' with type name {3}", new object[]
					{
						ControlBase.GetControlShapeName(viewDefinition.mainControl),
						viewDefinition.name,
						num - 1,
						typeDefinition.TypeName
					}));
					db.viewDefinitionsSection.viewDefinitionList.Add(viewDefinition);
				}
			}
		}

		// Token: 0x06005883 RID: 22659 RVA: 0x001CCE00 File Offset: 0x001CB000
		private ViewDefinition LoadViewFromObjectModle(string typeName, FormatViewDefinition formatView, int viewIndex)
		{
			TypeReference item = new TypeReference
			{
				name = typeName
			};
			AppliesTo appliesTo = new AppliesTo();
			appliesTo.referenceList.Add(item);
			ViewDefinition viewDefinition = new ViewDefinition();
			viewDefinition.appliesTo = appliesTo;
			viewDefinition.name = formatView.Name;
			PSControl control = formatView.Control;
			if (control is TableControl)
			{
				TableControl table = control as TableControl;
				viewDefinition.mainControl = this.LoadTableControlFromObjectModel(table, viewIndex, typeName);
			}
			else if (control is ListControl)
			{
				ListControl list = control as ListControl;
				viewDefinition.mainControl = this.LoadListControlFromObjectModel(list, viewIndex, typeName);
			}
			else if (control is WideControl)
			{
				WideControl wide = control as WideControl;
				viewDefinition.mainControl = this.LoadWideControlFromObjectModel(wide, viewIndex, typeName);
			}
			if (viewDefinition.mainControl == null)
			{
				return null;
			}
			return viewDefinition;
		}

		// Token: 0x06005884 RID: 22660 RVA: 0x001CCEC0 File Offset: 0x001CB0C0
		private ControlBase LoadTableControlFromObjectModel(TableControl table, int viewIndex, string typeName)
		{
			TableControlBody tableControlBody = new TableControlBody();
			this.LoadHeadersSectionFromObjectModel(tableControlBody, table.Headers);
			if (table.Rows.Count > 1)
			{
				base.ReportErrorForLoadingFromObjectModel(StringUtil.Format(FormatAndOutXmlLoadingStrings.MultipleRowEntriesFoundInFormattingData, new object[]
				{
					typeName,
					viewIndex,
					"TableRowEntry"
				}), typeName);
				return null;
			}
			this.LoadRowEntriesSectionFromObjectModel(tableControlBody, table.Rows, viewIndex, typeName);
			if (tableControlBody.defaultDefinition == null)
			{
				return null;
			}
			if (tableControlBody.header.columnHeaderDefinitionList.Count != 0 && tableControlBody.header.columnHeaderDefinitionList.Count != tableControlBody.defaultDefinition.rowItemDefinitionList.Count)
			{
				base.ReportErrorForLoadingFromObjectModel(StringUtil.Format(FormatAndOutXmlLoadingStrings.IncorrectHeaderItemCountInFormattingData, new object[]
				{
					typeName,
					viewIndex,
					tableControlBody.header.columnHeaderDefinitionList.Count,
					tableControlBody.defaultDefinition.rowItemDefinitionList.Count
				}), typeName);
				return null;
			}
			return tableControlBody;
		}

		// Token: 0x06005885 RID: 22661 RVA: 0x001CCFC4 File Offset: 0x001CB1C4
		private void LoadHeadersSectionFromObjectModel(TableControlBody tableBody, List<TableControlColumnHeader> headers)
		{
			foreach (TableControlColumnHeader tableControlColumnHeader in headers)
			{
				TableColumnHeaderDefinition tableColumnHeaderDefinition = new TableColumnHeaderDefinition();
				if (!string.IsNullOrEmpty(tableControlColumnHeader.Label))
				{
					tableColumnHeaderDefinition.label = new TextToken
					{
						text = tableControlColumnHeader.Label
					};
				}
				tableColumnHeaderDefinition.width = tableControlColumnHeader.Width;
				tableColumnHeaderDefinition.alignment = (int)tableControlColumnHeader.Alignment;
				tableBody.header.columnHeaderDefinitionList.Add(tableColumnHeaderDefinition);
			}
		}

		// Token: 0x06005886 RID: 22662 RVA: 0x001CD060 File Offset: 0x001CB260
		private void LoadRowEntriesSectionFromObjectModel(TableControlBody tableBody, List<TableControlRow> rowEntries, int viewIndex, string typeName)
		{
			foreach (TableControlRow tableControlRow in rowEntries)
			{
				TableRowDefinition tableRowDefinition = new TableRowDefinition();
				if (tableControlRow.Columns.Count > 0)
				{
					this.LoadColumnEntriesFromObjectModel(tableRowDefinition, tableControlRow.Columns, viewIndex, typeName);
					if (tableRowDefinition.rowItemDefinitionList == null)
					{
						tableBody.defaultDefinition = null;
						return;
					}
				}
				tableBody.defaultDefinition = tableRowDefinition;
			}
			if (tableBody.defaultDefinition == null)
			{
				base.ReportErrorForLoadingFromObjectModel(StringUtil.Format(FormatAndOutXmlLoadingStrings.NoDefaultShapeEntryInFormattingData, new object[]
				{
					typeName,
					viewIndex,
					"TableRowEntry"
				}), typeName);
			}
		}

		// Token: 0x06005887 RID: 22663 RVA: 0x001CD11C File Offset: 0x001CB31C
		private void LoadColumnEntriesFromObjectModel(TableRowDefinition trd, List<TableControlColumn> columns, int viewIndex, string typeName)
		{
			foreach (TableControlColumn tableControlColumn in columns)
			{
				TableRowItemDefinition tableRowItemDefinition = new TableRowItemDefinition();
				if (tableControlColumn.DisplayEntry != null)
				{
					ExpressionToken expressionToken = this.LoadExpressionFromObjectModel(tableControlColumn.DisplayEntry, viewIndex, typeName);
					if (expressionToken == null)
					{
						trd.rowItemDefinitionList = null;
						break;
					}
					FieldPropertyToken fieldPropertyToken = new FieldPropertyToken();
					fieldPropertyToken.expression = expressionToken;
					tableRowItemDefinition.formatTokenList.Add(fieldPropertyToken);
				}
				tableRowItemDefinition.alignment = (int)tableControlColumn.Alignment;
				trd.rowItemDefinitionList.Add(tableRowItemDefinition);
			}
		}

		// Token: 0x06005888 RID: 22664 RVA: 0x001CD1C0 File Offset: 0x001CB3C0
		private ExpressionToken LoadExpressionFromObjectModel(DisplayEntry displayEntry, int viewIndex, string typeName)
		{
			ExpressionToken expressionToken = new ExpressionToken();
			if (displayEntry.ValueType.Equals(DisplayEntryValueType.Property))
			{
				expressionToken.expressionValue = displayEntry.Value;
				return expressionToken;
			}
			if (displayEntry.ValueType.Equals(DisplayEntryValueType.ScriptBlock))
			{
				expressionToken.isScriptBlock = true;
				expressionToken.expressionValue = displayEntry.Value;
				try
				{
					this.expressionFactory.VerifyScriptBlockText(expressionToken.expressionValue);
				}
				catch (ParseException ex)
				{
					base.ReportErrorForLoadingFromObjectModel(StringUtil.Format(FormatAndOutXmlLoadingStrings.InvalidScriptBlockInFormattingData, new object[]
					{
						typeName,
						viewIndex,
						ex.Message
					}), typeName);
					return null;
				}
				catch (Exception)
				{
					throw;
				}
				return expressionToken;
			}
			PSTraceSource.NewInvalidOperationException();
			return null;
		}

		// Token: 0x06005889 RID: 22665 RVA: 0x001CD298 File Offset: 0x001CB498
		private AppliesTo LoadAppliesToSectionFromObjectModel(List<string> selectedBy)
		{
			AppliesTo appliesTo = new AppliesTo();
			foreach (string text in selectedBy)
			{
				if (string.IsNullOrEmpty(text))
				{
					return null;
				}
				TypeReference item = new TypeReference
				{
					name = text
				};
				appliesTo.referenceList.Add(item);
			}
			return appliesTo;
		}

		// Token: 0x0600588A RID: 22666 RVA: 0x001CD314 File Offset: 0x001CB514
		private ListControlBody LoadListControlFromObjectModel(ListControl list, int viewIndex, string typeName)
		{
			ListControlBody listControlBody = new ListControlBody();
			this.LoadListControlEntriesFromObjectModel(listControlBody, list.Entries, viewIndex, typeName);
			if (listControlBody.defaultEntryDefinition == null)
			{
				return null;
			}
			return listControlBody;
		}

		// Token: 0x0600588B RID: 22667 RVA: 0x001CD344 File Offset: 0x001CB544
		private void LoadListControlEntriesFromObjectModel(ListControlBody listBody, List<ListControlEntry> entries, int viewIndex, string typeName)
		{
			foreach (ListControlEntry listEntry in entries)
			{
				ListControlEntryDefinition listControlEntryDefinition = this.LoadListControlEntryDefinitionFromObjectModel(listEntry, viewIndex, typeName);
				if (listControlEntryDefinition == null)
				{
					base.ReportErrorForLoadingFromObjectModel(StringUtil.Format(FormatAndOutXmlLoadingStrings.LoadTagFailedInFormattingData, new object[]
					{
						typeName,
						viewIndex,
						"ListEntry"
					}), typeName);
					listBody.defaultEntryDefinition = null;
					return;
				}
				if (listControlEntryDefinition.appliesTo == null)
				{
					if (listBody.defaultEntryDefinition != null)
					{
						base.ReportErrorForLoadingFromObjectModel(StringUtil.Format(FormatAndOutXmlLoadingStrings.TooManyDefaultShapeEntryInFormattingData, new object[]
						{
							typeName,
							viewIndex,
							"ListEntry"
						}), typeName);
						listBody.defaultEntryDefinition = null;
						return;
					}
					listBody.defaultEntryDefinition = listControlEntryDefinition;
				}
				else
				{
					listBody.optionalEntryList.Add(listControlEntryDefinition);
				}
			}
			if (listBody.defaultEntryDefinition == null)
			{
				base.ReportErrorForLoadingFromObjectModel(StringUtil.Format(FormatAndOutXmlLoadingStrings.NoDefaultShapeEntryInFormattingData, new object[]
				{
					typeName,
					viewIndex,
					"ListEntry"
				}), typeName);
			}
		}

		// Token: 0x0600588C RID: 22668 RVA: 0x001CD47C File Offset: 0x001CB67C
		private ListControlEntryDefinition LoadListControlEntryDefinitionFromObjectModel(ListControlEntry listEntry, int viewIndex, string typeName)
		{
			ListControlEntryDefinition listControlEntryDefinition = new ListControlEntryDefinition();
			if (listEntry.SelectedBy.Count > 0)
			{
				listControlEntryDefinition.appliesTo = this.LoadAppliesToSectionFromObjectModel(listEntry.SelectedBy);
			}
			this.LoadListControlItemDefinitionsFromObjectModel(listControlEntryDefinition, listEntry.Items, viewIndex, typeName);
			if (listControlEntryDefinition.itemDefinitionList == null)
			{
				return null;
			}
			return listControlEntryDefinition;
		}

		// Token: 0x0600588D RID: 22669 RVA: 0x001CD4CC File Offset: 0x001CB6CC
		private void LoadListControlItemDefinitionsFromObjectModel(ListControlEntryDefinition lved, List<ListControlEntryItem> listItems, int viewIndex, string typeName)
		{
			foreach (ListControlEntryItem listControlEntryItem in listItems)
			{
				ListControlItemDefinition listControlItemDefinition = new ListControlItemDefinition();
				if (listControlEntryItem.DisplayEntry != null)
				{
					ExpressionToken expressionToken = this.LoadExpressionFromObjectModel(listControlEntryItem.DisplayEntry, viewIndex, typeName);
					if (expressionToken == null)
					{
						lved.itemDefinitionList = null;
						return;
					}
					FieldPropertyToken fieldPropertyToken = new FieldPropertyToken();
					fieldPropertyToken.expression = expressionToken;
					listControlItemDefinition.formatTokenList.Add(fieldPropertyToken);
				}
				if (!string.IsNullOrEmpty(listControlEntryItem.Label))
				{
					listControlItemDefinition.label = new TextToken
					{
						text = listControlEntryItem.Label
					};
				}
				lved.itemDefinitionList.Add(listControlItemDefinition);
			}
			if (lved.itemDefinitionList.Count == 0)
			{
				base.ReportErrorForLoadingFromObjectModel(StringUtil.Format(FormatAndOutXmlLoadingStrings.NoListViewItemInFormattingData, typeName, viewIndex), typeName);
				lved.itemDefinitionList = null;
			}
		}

		// Token: 0x0600588E RID: 22670 RVA: 0x001CD5C4 File Offset: 0x001CB7C4
		private WideControlBody LoadWideControlFromObjectModel(WideControl wide, int viewIndex, string typeName)
		{
			WideControlBody wideControlBody = new WideControlBody();
			wideControlBody.columns = (int)wide.Columns;
			this.LoadWideControlEntriesFromObjectModel(wideControlBody, wide.Entries, viewIndex, typeName);
			if (wideControlBody.defaultEntryDefinition == null)
			{
				return null;
			}
			return wideControlBody;
		}

		// Token: 0x0600588F RID: 22671 RVA: 0x001CD600 File Offset: 0x001CB800
		private void LoadWideControlEntriesFromObjectModel(WideControlBody wideBody, List<WideControlEntryItem> wideEntries, int viewIndex, string typeName)
		{
			foreach (WideControlEntryItem wideItem in wideEntries)
			{
				WideControlEntryDefinition wideControlEntryDefinition = this.LoadWideControlEntryFromObjectModel(wideItem, viewIndex, typeName);
				if (wideControlEntryDefinition == null)
				{
					base.ReportErrorForLoadingFromObjectModel(StringUtil.Format(FormatAndOutXmlLoadingStrings.InvalidFormattingData, new object[]
					{
						typeName,
						viewIndex,
						"WideEntry"
					}), typeName);
					wideBody.defaultEntryDefinition = null;
					return;
				}
				if (wideControlEntryDefinition.appliesTo == null)
				{
					if (wideBody.defaultEntryDefinition != null)
					{
						base.ReportErrorForLoadingFromObjectModel(StringUtil.Format(FormatAndOutXmlLoadingStrings.TooManyDefaultShapeEntryInFormattingData, new object[]
						{
							typeName,
							viewIndex,
							"WideEntry"
						}), typeName);
						wideBody.defaultEntryDefinition = null;
						return;
					}
					wideBody.defaultEntryDefinition = wideControlEntryDefinition;
				}
				else
				{
					wideBody.optionalEntryList.Add(wideControlEntryDefinition);
				}
			}
			if (wideBody.defaultEntryDefinition == null)
			{
				base.ReportErrorForLoadingFromObjectModel(StringUtil.Format(FormatAndOutXmlLoadingStrings.NoDefaultShapeEntryInFormattingData, new object[]
				{
					typeName,
					viewIndex,
					"WideEntry"
				}), typeName);
			}
		}

		// Token: 0x06005890 RID: 22672 RVA: 0x001CD738 File Offset: 0x001CB938
		private WideControlEntryDefinition LoadWideControlEntryFromObjectModel(WideControlEntryItem wideItem, int viewIndex, string typeName)
		{
			WideControlEntryDefinition wideControlEntryDefinition = new WideControlEntryDefinition();
			if (wideItem.SelectedBy.Count > 0)
			{
				wideControlEntryDefinition.appliesTo = this.LoadAppliesToSectionFromObjectModel(wideItem.SelectedBy);
			}
			ExpressionToken expressionToken = this.LoadExpressionFromObjectModel(wideItem.DisplayEntry, viewIndex, typeName);
			if (expressionToken == null)
			{
				return null;
			}
			FieldPropertyToken fieldPropertyToken = new FieldPropertyToken();
			fieldPropertyToken.expression = expressionToken;
			wideControlEntryDefinition.formatTokenList.Add(fieldPropertyToken);
			return wideControlEntryDefinition;
		}

		// Token: 0x06005891 RID: 22673 RVA: 0x001CD79C File Offset: 0x001CB99C
		private void LoadDefaultSettings(TypeInfoDataBase db, XmlNode defaultSettingsNode)
		{
			bool flag = false;
			bool flag2 = false;
			bool flag3 = false;
			bool flag4 = false;
			bool flag5 = false;
			using (base.StackFrame(defaultSettingsNode))
			{
				foreach (object obj in defaultSettingsNode.ChildNodes)
				{
					XmlNode xmlNode = (XmlNode)obj;
					if (base.MatchNodeName(xmlNode, "ShowError"))
					{
						if (flag2)
						{
							base.ProcessDuplicateNode(xmlNode);
						}
						flag2 = true;
						bool flag6;
						if (this.ReadBooleanNode(xmlNode, out flag6))
						{
							db.defaultSettingsSection.formatErrorPolicy.ShowErrorsAsMessages = flag6;
						}
					}
					else if (base.MatchNodeName(xmlNode, "DisplayError"))
					{
						if (flag3)
						{
							base.ProcessDuplicateNode(xmlNode);
						}
						flag3 = true;
						bool flag6;
						if (this.ReadBooleanNode(xmlNode, out flag6))
						{
							db.defaultSettingsSection.formatErrorPolicy.ShowErrorsInFormattedOutput = flag6;
						}
					}
					else if (base.MatchNodeName(xmlNode, "PropertyCountForTable"))
					{
						if (flag)
						{
							base.ProcessDuplicateNode(xmlNode);
						}
						flag = true;
						int propertyCountForTable;
						if (this.ReadPositiveIntegerValue(xmlNode, out propertyCountForTable))
						{
							db.defaultSettingsSection.shapeSelectionDirectives.PropertyCountForTable = propertyCountForTable;
						}
						else
						{
							base.ReportError(StringUtil.Format(FormatAndOutXmlLoadingStrings.InvalidNodeValue, new object[]
							{
								base.ComputeCurrentXPath(),
								base.FilePath,
								"PropertyCountForTable"
							}));
						}
					}
					else if (base.MatchNodeName(xmlNode, "WrapTables"))
					{
						if (flag5)
						{
							base.ProcessDuplicateNode(xmlNode);
						}
						flag5 = true;
						bool flag6;
						if (this.ReadBooleanNode(xmlNode, out flag6))
						{
							db.defaultSettingsSection.MultilineTables = flag6;
						}
						else
						{
							base.ReportError(StringUtil.Format(FormatAndOutXmlLoadingStrings.InvalidNodeValue, new object[]
							{
								base.ComputeCurrentXPath(),
								base.FilePath,
								"WrapTables"
							}));
						}
					}
					else if (base.MatchNodeName(xmlNode, "EnumerableExpansions"))
					{
						if (flag4)
						{
							base.ProcessDuplicateNode(xmlNode);
						}
						flag4 = true;
						db.defaultSettingsSection.enumerableExpansionDirectiveList = this.LoadEnumerableExpansionDirectiveList(xmlNode);
					}
					else
					{
						base.ProcessUnknownNode(xmlNode);
					}
				}
			}
		}

		// Token: 0x06005892 RID: 22674 RVA: 0x001CD9EC File Offset: 0x001CBBEC
		private List<EnumerableExpansionDirective> LoadEnumerableExpansionDirectiveList(XmlNode expansionListNode)
		{
			List<EnumerableExpansionDirective> list = new List<EnumerableExpansionDirective>();
			using (base.StackFrame(expansionListNode))
			{
				int num = 0;
				foreach (object obj in expansionListNode.ChildNodes)
				{
					XmlNode xmlNode = (XmlNode)obj;
					if (base.MatchNodeName(xmlNode, "EnumerableExpansion"))
					{
						EnumerableExpansionDirective enumerableExpansionDirective = this.LoadEnumerableExpansionDirective(xmlNode, num++);
						if (enumerableExpansionDirective == null)
						{
							base.ReportError(StringUtil.Format(FormatAndOutXmlLoadingStrings.LoadTagFailed, new object[]
							{
								base.ComputeCurrentXPath(),
								base.FilePath,
								"EnumerableExpansion"
							}));
							return null;
						}
						list.Add(enumerableExpansionDirective);
					}
					else
					{
						base.ProcessUnknownNode(xmlNode);
					}
				}
			}
			return list;
		}

		// Token: 0x06005893 RID: 22675 RVA: 0x001CDAE0 File Offset: 0x001CBCE0
		private EnumerableExpansionDirective LoadEnumerableExpansionDirective(XmlNode directive, int index)
		{
			EnumerableExpansionDirective result;
			using (base.StackFrame(directive, index))
			{
				EnumerableExpansionDirective enumerableExpansionDirective = new EnumerableExpansionDirective();
				bool flag = false;
				bool flag2 = false;
				foreach (object obj in directive.ChildNodes)
				{
					XmlNode xmlNode = (XmlNode)obj;
					if (base.MatchNodeName(xmlNode, "EntrySelectedBy"))
					{
						if (flag)
						{
							base.ProcessDuplicateNode(xmlNode);
							return null;
						}
						flag = true;
						enumerableExpansionDirective.appliesTo = this.LoadAppliesToSection(xmlNode, true);
					}
					else if (base.MatchNodeName(xmlNode, "Expand"))
					{
						if (flag2)
						{
							base.ProcessDuplicateNode(xmlNode);
							return null;
						}
						flag2 = true;
						string mandatoryInnerText = base.GetMandatoryInnerText(xmlNode);
						if (mandatoryInnerText == null)
						{
							return null;
						}
						if (!EnumerableExpansionConversion.Convert(mandatoryInnerText, out enumerableExpansionDirective.enumerableExpansion))
						{
							base.ReportError(StringUtil.Format(FormatAndOutXmlLoadingStrings.InvalidNodeValue, new object[]
							{
								base.ComputeCurrentXPath(),
								base.FilePath,
								"Expand"
							}));
							return null;
						}
					}
					else
					{
						base.ProcessUnknownNode(xmlNode);
					}
				}
				result = enumerableExpansionDirective;
			}
			return result;
		}

		// Token: 0x06005894 RID: 22676 RVA: 0x001CDC4C File Offset: 0x001CBE4C
		private void LoadTypeGroups(TypeInfoDataBase db, XmlNode typeGroupsNode)
		{
			using (base.StackFrame(typeGroupsNode))
			{
				int num = 0;
				foreach (object obj in typeGroupsNode.ChildNodes)
				{
					XmlNode xmlNode = (XmlNode)obj;
					if (base.MatchNodeName(xmlNode, "SelectionSet"))
					{
						this.LoadTypeGroup(db, xmlNode, num++);
					}
					else
					{
						base.ProcessUnknownNode(xmlNode);
					}
				}
			}
		}

		// Token: 0x06005895 RID: 22677 RVA: 0x001CDCE8 File Offset: 0x001CBEE8
		private void LoadTypeGroup(TypeInfoDataBase db, XmlNode typeGroupNode, int index)
		{
			using (base.StackFrame(typeGroupNode, index))
			{
				TypeGroupDefinition typeGroupDefinition = new TypeGroupDefinition();
				bool flag = false;
				foreach (object obj in typeGroupNode.ChildNodes)
				{
					XmlNode xmlNode = (XmlNode)obj;
					if (base.MatchNodeName(xmlNode, "Name"))
					{
						if (flag)
						{
							base.ProcessDuplicateNode(xmlNode);
						}
						else
						{
							flag = true;
							typeGroupDefinition.name = base.GetMandatoryInnerText(xmlNode);
						}
					}
					else if (base.MatchNodeName(xmlNode, "Types"))
					{
						this.LoadTypeGroupTypeRefs(xmlNode, typeGroupDefinition);
					}
					else
					{
						base.ProcessUnknownNode(xmlNode);
					}
				}
				if (!flag)
				{
					base.ReportMissingNode("Name");
				}
				db.typeGroupSection.typeGroupDefinitionList.Add(typeGroupDefinition);
			}
		}

		// Token: 0x06005896 RID: 22678 RVA: 0x001CDDD4 File Offset: 0x001CBFD4
		private void LoadTypeGroupTypeRefs(XmlNode typesNode, TypeGroupDefinition typeGroupDefinition)
		{
			using (base.StackFrame(typesNode))
			{
				int num = 0;
				foreach (object obj in typesNode.ChildNodes)
				{
					XmlNode n = (XmlNode)obj;
					if (base.MatchNodeName(n, "TypeName"))
					{
						using (base.StackFrame(n, num++))
						{
							TypeReference typeReference = new TypeReference();
							typeReference.name = base.GetMandatoryInnerText(n);
							typeGroupDefinition.typeReferenceList.Add(typeReference);
							continue;
						}
					}
					base.ProcessUnknownNode(n);
				}
			}
		}

		// Token: 0x06005897 RID: 22679 RVA: 0x001CDEAC File Offset: 0x001CC0AC
		private AppliesTo LoadAppliesToSection(XmlNode appliesToNode, bool allowSelectionCondition)
		{
			AppliesTo result;
			using (base.StackFrame(appliesToNode))
			{
				AppliesTo appliesTo = new AppliesTo();
				foreach (object obj in appliesToNode.ChildNodes)
				{
					XmlNode xmlNode = (XmlNode)obj;
					using (base.StackFrame(xmlNode))
					{
						if (base.MatchNodeName(xmlNode, "SelectionSetName"))
						{
							TypeGroupReference typeGroupReference = this.LoadTypeGroupReference(xmlNode);
							if (typeGroupReference == null)
							{
								return null;
							}
							appliesTo.referenceList.Add(typeGroupReference);
						}
						else if (base.MatchNodeName(xmlNode, "TypeName"))
						{
							TypeReference typeReference = this.LoadTypeReference(xmlNode);
							if (typeReference == null)
							{
								return null;
							}
							appliesTo.referenceList.Add(typeReference);
						}
						else if (allowSelectionCondition && base.MatchNodeName(xmlNode, "SelectionCondition"))
						{
							TypeOrGroupReference typeOrGroupReference = this.LoadSelectionConditionNode(xmlNode);
							if (typeOrGroupReference == null)
							{
								return null;
							}
							appliesTo.referenceList.Add(typeOrGroupReference);
						}
						else
						{
							base.ProcessUnknownNode(xmlNode);
						}
					}
				}
				if (appliesTo.referenceList.Count == 0)
				{
					base.ReportError(StringUtil.Format(FormatAndOutXmlLoadingStrings.EmptyAppliesTo, base.ComputeCurrentXPath(), base.FilePath));
					result = null;
				}
				else
				{
					result = appliesTo;
				}
			}
			return result;
		}

		// Token: 0x06005898 RID: 22680 RVA: 0x001CE048 File Offset: 0x001CC248
		private TypeReference LoadTypeReference(XmlNode n)
		{
			string mandatoryInnerText = base.GetMandatoryInnerText(n);
			if (mandatoryInnerText != null)
			{
				return new TypeReference
				{
					name = mandatoryInnerText
				};
			}
			return null;
		}

		// Token: 0x06005899 RID: 22681 RVA: 0x001CE070 File Offset: 0x001CC270
		private TypeGroupReference LoadTypeGroupReference(XmlNode n)
		{
			string mandatoryInnerText = base.GetMandatoryInnerText(n);
			if (mandatoryInnerText != null)
			{
				return new TypeGroupReference
				{
					name = mandatoryInnerText
				};
			}
			return null;
		}

		// Token: 0x0600589A RID: 22682 RVA: 0x001CE098 File Offset: 0x001CC298
		private TypeOrGroupReference LoadSelectionConditionNode(XmlNode selectionConditionNode)
		{
			TypeOrGroupReference result;
			using (base.StackFrame(selectionConditionNode))
			{
				TypeOrGroupReference typeOrGroupReference = null;
				bool flag = false;
				bool flag2 = false;
				bool flag3 = false;
				TypeInfoDataBaseLoader.ExpressionNodeMatch expressionNodeMatch = new TypeInfoDataBaseLoader.ExpressionNodeMatch(this);
				foreach (object obj in selectionConditionNode.ChildNodes)
				{
					XmlNode n = (XmlNode)obj;
					if (base.MatchNodeName(n, "SelectionSetName"))
					{
						if (flag3)
						{
							base.ProcessDuplicateAlternateNode(n, "SelectionSetName", "TypeName");
							return null;
						}
						flag3 = true;
						TypeGroupReference typeGroupReference = this.LoadTypeGroupReference(n);
						if (typeGroupReference == null)
						{
							return null;
						}
						typeOrGroupReference = typeGroupReference;
					}
					else if (base.MatchNodeName(n, "TypeName"))
					{
						if (flag2)
						{
							base.ProcessDuplicateAlternateNode(n, "SelectionSetName", "TypeName");
							return null;
						}
						flag2 = true;
						TypeReference typeReference = this.LoadTypeReference(n);
						if (typeReference == null)
						{
							return null;
						}
						typeOrGroupReference = typeReference;
					}
					else if (expressionNodeMatch.MatchNode(n))
					{
						if (flag)
						{
							base.ProcessDuplicateNode(n);
							return null;
						}
						flag = true;
						if (!expressionNodeMatch.ProcessNode(n))
						{
							return null;
						}
					}
					else
					{
						base.ProcessUnknownNode(n);
					}
				}
				if (flag2 && flag3)
				{
					base.ReportError(StringUtil.Format(FormatAndOutXmlLoadingStrings.SelectionSetNameAndTypeName, base.ComputeCurrentXPath(), base.FilePath));
					result = null;
				}
				else if (typeOrGroupReference == null)
				{
					base.ReportMissingNodes(new string[]
					{
						"SelectionSetName",
						"TypeName"
					});
					result = null;
				}
				else if (flag)
				{
					typeOrGroupReference.conditionToken = expressionNodeMatch.GenerateExpressionToken();
					if (typeOrGroupReference.conditionToken == null)
					{
						result = null;
					}
					else
					{
						result = typeOrGroupReference;
					}
				}
				else
				{
					base.ReportError(StringUtil.Format(FormatAndOutXmlLoadingStrings.ExpectExpression, base.ComputeCurrentXPath(), base.FilePath));
					result = null;
				}
			}
			return result;
		}

		// Token: 0x0600589B RID: 22683 RVA: 0x001CE2A8 File Offset: 0x001CC4A8
		private GroupBy LoadGroupBySection(XmlNode groupByNode)
		{
			GroupBy result;
			using (base.StackFrame(groupByNode))
			{
				TypeInfoDataBaseLoader.ExpressionNodeMatch expressionNodeMatch = new TypeInfoDataBaseLoader.ExpressionNodeMatch(this);
				TypeInfoDataBaseLoader.ComplexControlMatch complexControlMatch = new TypeInfoDataBaseLoader.ComplexControlMatch(this);
				bool flag = false;
				bool flag2 = false;
				bool flag3 = false;
				GroupBy groupBy = new GroupBy();
				TextToken textToken = null;
				foreach (object obj in groupByNode)
				{
					XmlNode xmlNode = (XmlNode)obj;
					if (expressionNodeMatch.MatchNode(xmlNode))
					{
						if (flag)
						{
							base.ProcessDuplicateNode(xmlNode);
							return null;
						}
						flag = true;
						if (!expressionNodeMatch.ProcessNode(xmlNode))
						{
							return null;
						}
					}
					else if (complexControlMatch.MatchNode(xmlNode))
					{
						if (flag2)
						{
							base.ProcessDuplicateAlternateNode(xmlNode, "CustomControl", "CustomControlName");
							return null;
						}
						flag2 = true;
						if (!complexControlMatch.ProcessNode(xmlNode))
						{
							return null;
						}
					}
					else if (base.MatchNodeNameWithAttributes(xmlNode, "Label"))
					{
						if (flag3)
						{
							base.ProcessDuplicateAlternateNode(xmlNode, "CustomControl", "CustomControlName");
							return null;
						}
						flag3 = true;
						textToken = this.LoadLabel(xmlNode);
						if (textToken == null)
						{
							return null;
						}
					}
					else
					{
						base.ProcessUnknownNode(xmlNode);
					}
				}
				if (flag2 && flag3)
				{
					base.ReportError(StringUtil.Format(FormatAndOutXmlLoadingStrings.ControlAndLabel, base.ComputeCurrentXPath(), base.FilePath));
					result = null;
				}
				else
				{
					if (flag2 || flag3)
					{
						if (!flag)
						{
							base.ReportError(StringUtil.Format(FormatAndOutXmlLoadingStrings.ControlLabelWithoutExpression, base.ComputeCurrentXPath(), base.FilePath));
							return null;
						}
						if (flag2)
						{
							groupBy.startGroup.control = complexControlMatch.Control;
						}
						else if (flag3)
						{
							groupBy.startGroup.labelTextToken = textToken;
						}
					}
					if (flag)
					{
						ExpressionToken expressionToken = expressionNodeMatch.GenerateExpressionToken();
						if (expressionToken == null)
						{
							result = null;
						}
						else
						{
							groupBy.startGroup.expression = expressionToken;
							result = groupBy;
						}
					}
					else
					{
						base.ReportError(StringUtil.Format(FormatAndOutXmlLoadingStrings.ExpectExpression, base.ComputeCurrentXPath(), base.FilePath));
						result = null;
					}
				}
			}
			return result;
		}

		// Token: 0x0600589C RID: 22684 RVA: 0x001CE4E4 File Offset: 0x001CC6E4
		private TextToken LoadLabel(XmlNode textNode)
		{
			TextToken result;
			using (base.StackFrame(textNode))
			{
				result = this.LoadTextToken(textNode);
			}
			return result;
		}

		// Token: 0x0600589D RID: 22685 RVA: 0x001CE520 File Offset: 0x001CC720
		private TextToken LoadTextToken(XmlNode n)
		{
			TextToken textToken = new TextToken();
			if (!this.LoadStringResourceReference(n, out textToken.resource))
			{
				return null;
			}
			if (textToken.resource != null)
			{
				textToken.text = n.InnerText;
				return textToken;
			}
			textToken.text = base.GetMandatoryInnerText(n);
			if (textToken.text == null)
			{
				return null;
			}
			return textToken;
		}

		// Token: 0x0600589E RID: 22686 RVA: 0x001CE574 File Offset: 0x001CC774
		private bool LoadStringResourceReference(XmlNode n, out StringResourceReference resource)
		{
			resource = null;
			XmlElement xmlElement = n as XmlElement;
			if (xmlElement == null)
			{
				base.ReportError(StringUtil.Format(FormatAndOutXmlLoadingStrings.NonXmlElementNode, base.ComputeCurrentXPath(), base.FilePath));
				return false;
			}
			if (xmlElement.Attributes.Count <= 0)
			{
				return true;
			}
			resource = this.LoadResourceAttributes(xmlElement.Attributes);
			return resource != null;
		}

		// Token: 0x0600589F RID: 22687 RVA: 0x001CE5D4 File Offset: 0x001CC7D4
		private StringResourceReference LoadResourceAttributes(XmlAttributeCollection attributes)
		{
			StringResourceReference stringResourceReference = new StringResourceReference();
			foreach (object obj in attributes)
			{
				XmlAttribute a = (XmlAttribute)obj;
				if (base.MatchAttributeName(a, "AssemblyName"))
				{
					stringResourceReference.assemblyName = base.GetMandatoryAttributeValue(a);
					if (stringResourceReference.assemblyName == null)
					{
						return null;
					}
				}
				else if (base.MatchAttributeName(a, "BaseName"))
				{
					stringResourceReference.baseName = base.GetMandatoryAttributeValue(a);
					if (stringResourceReference.baseName == null)
					{
						return null;
					}
				}
				else
				{
					if (!base.MatchAttributeName(a, "ResourceId"))
					{
						base.ProcessUnknownAttribute(a);
						return null;
					}
					stringResourceReference.resourceId = base.GetMandatoryAttributeValue(a);
					if (stringResourceReference.resourceId == null)
					{
						return null;
					}
				}
			}
			if (stringResourceReference.assemblyName == null)
			{
				base.ReportMissingAttribute("AssemblyName");
				return null;
			}
			if (stringResourceReference.baseName == null)
			{
				base.ReportMissingAttribute("BaseName");
				return null;
			}
			if (stringResourceReference.resourceId == null)
			{
				base.ReportMissingAttribute("ResourceId");
				return null;
			}
			stringResourceReference.loadingInfo = base.LoadingInfo;
			if (base.VerifyStringResources)
			{
				DisplayResourceManagerCache.LoadingResult loadingResult;
				DisplayResourceManagerCache.AssemblyBindingStatus bindingStatus;
				this.displayResourceManagerCache.VerifyResource(stringResourceReference, out loadingResult, out bindingStatus);
				if (loadingResult != DisplayResourceManagerCache.LoadingResult.NoError)
				{
					this.ReportStringResourceFailure(stringResourceReference, loadingResult, bindingStatus);
					return null;
				}
			}
			return stringResourceReference;
		}

		// Token: 0x060058A0 RID: 22688 RVA: 0x001CE73C File Offset: 0x001CC93C
		private void ReportStringResourceFailure(StringResourceReference resource, DisplayResourceManagerCache.LoadingResult result, DisplayResourceManagerCache.AssemblyBindingStatus bindingStatus)
		{
			string text;
			switch (bindingStatus)
			{
			case DisplayResourceManagerCache.AssemblyBindingStatus.FoundInGac:
				text = StringUtil.Format(FormatAndOutXmlLoadingStrings.AssemblyInGAC, resource.assemblyName);
				break;
			case DisplayResourceManagerCache.AssemblyBindingStatus.FoundInPath:
				text = Path.Combine(resource.loadingInfo.fileDirectory, resource.assemblyName);
				break;
			default:
				text = resource.assemblyName;
				break;
			}
			string message = null;
			switch (result)
			{
			case DisplayResourceManagerCache.LoadingResult.AssemblyNotFound:
				message = StringUtil.Format(FormatAndOutXmlLoadingStrings.AssemblyNotFound, new object[]
				{
					base.ComputeCurrentXPath(),
					base.FilePath,
					text
				});
				break;
			case DisplayResourceManagerCache.LoadingResult.ResourceNotFound:
				message = StringUtil.Format(FormatAndOutXmlLoadingStrings.ResourceNotFound, new object[]
				{
					base.ComputeCurrentXPath(),
					base.FilePath,
					resource.baseName,
					text
				});
				break;
			case DisplayResourceManagerCache.LoadingResult.StringNotFound:
				message = StringUtil.Format(FormatAndOutXmlLoadingStrings.StringResourceNotFound, new object[]
				{
					base.ComputeCurrentXPath(),
					base.FilePath,
					resource.resourceId,
					resource.baseName,
					text
				});
				break;
			}
			base.ReportError(message);
		}

		// Token: 0x060058A1 RID: 22689 RVA: 0x001CE860 File Offset: 0x001CCA60
		internal bool VerifyScriptBlock(string scriptBlockText)
		{
			try
			{
				this.expressionFactory.VerifyScriptBlockText(scriptBlockText);
			}
			catch (ParseException ex)
			{
				base.ReportError(StringUtil.Format(FormatAndOutXmlLoadingStrings.InvalidScriptBlock, new object[]
				{
					base.ComputeCurrentXPath(),
					base.FilePath,
					ex.Message
				}));
				return false;
			}
			catch (Exception)
			{
				throw;
			}
			return true;
		}

		// Token: 0x060058A2 RID: 22690 RVA: 0x001CE8D8 File Offset: 0x001CCAD8
		private ComplexControlBody LoadComplexControl(XmlNode controlNode)
		{
			ComplexControlBody result;
			using (base.StackFrame(controlNode))
			{
				ComplexControlBody complexControlBody = new ComplexControlBody();
				bool flag = false;
				foreach (object obj in controlNode.ChildNodes)
				{
					XmlNode xmlNode = (XmlNode)obj;
					if (base.MatchNodeName(xmlNode, "CustomEntries"))
					{
						if (flag)
						{
							base.ProcessDuplicateNode(xmlNode);
						}
						else
						{
							flag = true;
							this.LoadComplexControlEntries(xmlNode, complexControlBody);
							if (complexControlBody.defaultEntry == null)
							{
								return null;
							}
						}
					}
					else
					{
						base.ProcessUnknownNode(xmlNode);
					}
				}
				if (!flag)
				{
					base.ReportMissingNode("CustomEntries");
					result = null;
				}
				else
				{
					result = complexControlBody;
				}
			}
			return result;
		}

		// Token: 0x060058A3 RID: 22691 RVA: 0x001CE9A8 File Offset: 0x001CCBA8
		private void LoadComplexControlEntries(XmlNode complexControlEntriesNode, ComplexControlBody complexBody)
		{
			using (base.StackFrame(complexControlEntriesNode))
			{
				int num = 0;
				foreach (object obj in complexControlEntriesNode.ChildNodes)
				{
					XmlNode xmlNode = (XmlNode)obj;
					if (base.MatchNodeName(xmlNode, "CustomEntry"))
					{
						ComplexControlEntryDefinition complexControlEntryDefinition = this.LoadComplexControlEntryDefinition(xmlNode, num++);
						if (complexControlEntryDefinition == null)
						{
							base.ReportError(StringUtil.Format(FormatAndOutXmlLoadingStrings.LoadTagFailed, new object[]
							{
								base.ComputeCurrentXPath(),
								base.FilePath,
								"CustomEntry"
							}));
							complexBody.defaultEntry = null;
							return;
						}
						if (complexControlEntryDefinition.appliesTo == null)
						{
							if (complexBody.defaultEntry != null)
							{
								base.ReportError(StringUtil.Format(FormatAndOutXmlLoadingStrings.TooManyDefaultShapeEntry, new object[]
								{
									base.ComputeCurrentXPath(),
									base.FilePath,
									"CustomEntry"
								}));
								complexBody.defaultEntry = null;
								return;
							}
							complexBody.defaultEntry = complexControlEntryDefinition;
						}
						else
						{
							complexBody.optionalEntryList.Add(complexControlEntryDefinition);
						}
					}
					else
					{
						base.ProcessUnknownNode(xmlNode);
					}
				}
				if (complexBody.defaultEntry == null)
				{
					base.ReportError(StringUtil.Format(FormatAndOutXmlLoadingStrings.NoDefaultShapeEntry, new object[]
					{
						base.ComputeCurrentXPath(),
						base.FilePath,
						"CustomEntry"
					}));
				}
			}
		}

		// Token: 0x060058A4 RID: 22692 RVA: 0x001CEB58 File Offset: 0x001CCD58
		private ComplexControlEntryDefinition LoadComplexControlEntryDefinition(XmlNode complexControlEntryNode, int index)
		{
			ComplexControlEntryDefinition result;
			using (base.StackFrame(complexControlEntryNode, index))
			{
				bool flag = false;
				bool flag2 = false;
				ComplexControlEntryDefinition complexControlEntryDefinition = new ComplexControlEntryDefinition();
				foreach (object obj in complexControlEntryNode.ChildNodes)
				{
					XmlNode xmlNode = (XmlNode)obj;
					if (base.MatchNodeName(xmlNode, "EntrySelectedBy"))
					{
						if (flag)
						{
							base.ProcessDuplicateNode(xmlNode);
							return null;
						}
						flag = true;
						complexControlEntryDefinition.appliesTo = this.LoadAppliesToSection(xmlNode, true);
					}
					else if (base.MatchNodeName(xmlNode, "CustomItem"))
					{
						if (flag2)
						{
							base.ProcessDuplicateNode(xmlNode);
							return null;
						}
						flag2 = true;
						complexControlEntryDefinition.itemDefinition.formatTokenList = this.LoadComplexControlTokenListDefinitions(xmlNode);
					}
					else
					{
						base.ProcessUnknownNode(xmlNode);
					}
				}
				if (complexControlEntryDefinition.itemDefinition.formatTokenList == null)
				{
					base.ReportError(StringUtil.Format(FormatAndOutXmlLoadingStrings.MissingNode, new object[]
					{
						base.ComputeCurrentXPath(),
						base.FilePath,
						"CustomItem"
					}));
					result = null;
				}
				else
				{
					result = complexControlEntryDefinition;
				}
			}
			return result;
		}

		// Token: 0x060058A5 RID: 22693 RVA: 0x001CECB8 File Offset: 0x001CCEB8
		private List<FormatToken> LoadComplexControlTokenListDefinitions(XmlNode bodyNode)
		{
			List<FormatToken> result;
			using (base.StackFrame(bodyNode))
			{
				List<FormatToken> list = new List<FormatToken>();
				int num = 0;
				int num2 = 0;
				int num3 = 0;
				int num4 = 0;
				foreach (object obj in bodyNode.ChildNodes)
				{
					XmlNode xmlNode = (XmlNode)obj;
					if (base.MatchNodeName(xmlNode, "ExpressionBinding"))
					{
						CompoundPropertyToken compoundPropertyToken = this.LoadCompoundProperty(xmlNode, num++);
						if (compoundPropertyToken == null)
						{
							base.ReportError(StringUtil.Format(FormatAndOutXmlLoadingStrings.LoadTagFailed, new object[]
							{
								base.ComputeCurrentXPath(),
								base.FilePath,
								"ExpressionBinding"
							}));
							return null;
						}
						list.Add(compoundPropertyToken);
					}
					else if (base.MatchNodeName(xmlNode, "NewLine"))
					{
						NewLineToken newLineToken = this.LoadNewLine(xmlNode, num2++);
						if (newLineToken == null)
						{
							base.ReportError(StringUtil.Format(FormatAndOutXmlLoadingStrings.LoadTagFailed, new object[]
							{
								base.ComputeCurrentXPath(),
								base.FilePath,
								"NewLine"
							}));
							return null;
						}
						list.Add(newLineToken);
					}
					else if (base.MatchNodeNameWithAttributes(xmlNode, "Text"))
					{
						TextToken textToken = this.LoadText(xmlNode, num3++);
						if (textToken == null)
						{
							base.ReportError(StringUtil.Format(FormatAndOutXmlLoadingStrings.LoadTagFailed, new object[]
							{
								base.ComputeCurrentXPath(),
								base.FilePath,
								"Text"
							}));
							return null;
						}
						list.Add(textToken);
					}
					else if (base.MatchNodeName(xmlNode, "Frame"))
					{
						FrameToken frameToken = this.LoadFrameDefinition(xmlNode, num4++);
						if (frameToken == null)
						{
							base.ReportError(StringUtil.Format(FormatAndOutXmlLoadingStrings.LoadTagFailed, new object[]
							{
								base.ComputeCurrentXPath(),
								base.FilePath,
								"Frame"
							}));
							return null;
						}
						list.Add(frameToken);
					}
					else
					{
						base.ProcessUnknownNode(xmlNode);
					}
				}
				if (list.Count == 0)
				{
					base.ReportError(StringUtil.Format(FormatAndOutXmlLoadingStrings.EmptyCustomControlList, base.ComputeCurrentXPath(), base.FilePath));
					result = null;
				}
				else
				{
					result = list;
				}
			}
			return result;
		}

		// Token: 0x060058A6 RID: 22694 RVA: 0x001CEF4C File Offset: 0x001CD14C
		private bool LoadPropertyBaseHelper(XmlNode propertyBaseNode, PropertyTokenBase ptb, List<XmlNode> unprocessedNodes)
		{
			TypeInfoDataBaseLoader.ExpressionNodeMatch expressionNodeMatch = new TypeInfoDataBaseLoader.ExpressionNodeMatch(this);
			bool flag = false;
			bool flag2 = false;
			bool flag3 = false;
			ExpressionToken expressionToken = null;
			foreach (object obj in propertyBaseNode.ChildNodes)
			{
				XmlNode xmlNode = (XmlNode)obj;
				if (expressionNodeMatch.MatchNode(xmlNode))
				{
					if (flag)
					{
						base.ProcessDuplicateNode(xmlNode);
						return false;
					}
					flag = true;
					if (!expressionNodeMatch.ProcessNode(xmlNode))
					{
						return false;
					}
				}
				else if (base.MatchNodeName(xmlNode, "EnumerateCollection"))
				{
					if (flag2)
					{
						base.ProcessDuplicateNode(xmlNode);
						return false;
					}
					flag2 = true;
					if (!this.ReadBooleanNode(xmlNode, out ptb.enumerateCollection))
					{
						return false;
					}
				}
				else if (base.MatchNodeName(xmlNode, "ItemSelectionCondition"))
				{
					if (flag3)
					{
						base.ProcessDuplicateNode(xmlNode);
						return false;
					}
					flag3 = true;
					expressionToken = this.LoadItemSelectionCondition(xmlNode);
					if (expressionToken == null)
					{
						return false;
					}
				}
				else if (!XmlLoaderBase.IsFilteredOutNode(xmlNode))
				{
					unprocessedNodes.Add(xmlNode);
				}
			}
			if (flag)
			{
				ExpressionToken expressionToken2 = expressionNodeMatch.GenerateExpressionToken();
				if (expressionToken2 == null)
				{
					return false;
				}
				ptb.expression = expressionToken2;
				ptb.conditionToken = expressionToken;
			}
			return true;
		}

		// Token: 0x060058A7 RID: 22695 RVA: 0x001CF098 File Offset: 0x001CD298
		private CompoundPropertyToken LoadCompoundProperty(XmlNode compoundPropertyNode, int index)
		{
			CompoundPropertyToken result;
			using (base.StackFrame(compoundPropertyNode, index))
			{
				CompoundPropertyToken compoundPropertyToken = new CompoundPropertyToken();
				List<XmlNode> list = new List<XmlNode>();
				if (!this.LoadPropertyBaseHelper(compoundPropertyNode, compoundPropertyToken, list))
				{
					result = null;
				}
				else
				{
					compoundPropertyToken.control = null;
					bool flag = false;
					bool flag2 = false;
					TypeInfoDataBaseLoader.ComplexControlMatch complexControlMatch = new TypeInfoDataBaseLoader.ComplexControlMatch(this);
					FieldControlBody fieldControlBody = null;
					foreach (XmlNode n in list)
					{
						if (complexControlMatch.MatchNode(n))
						{
							if (flag)
							{
								base.ProcessDuplicateAlternateNode(n, "CustomControl", "CustomControlName");
								return null;
							}
							flag = true;
							if (!complexControlMatch.ProcessNode(n))
							{
								return null;
							}
						}
						else if (base.MatchNodeName(n, "FieldControl"))
						{
							if (flag2)
							{
								base.ProcessDuplicateAlternateNode(n, "CustomControl", "CustomControlName");
								return null;
							}
							flag2 = true;
							fieldControlBody = new FieldControlBody();
							fieldControlBody.fieldFormattingDirective.formatString = base.GetMandatoryInnerText(n);
							if (fieldControlBody.fieldFormattingDirective.formatString == null)
							{
								return null;
							}
						}
						else
						{
							base.ProcessUnknownNode(n);
						}
					}
					if (flag2 && flag)
					{
						base.ProcessDuplicateAlternateNode("CustomControl", "CustomControlName");
						result = null;
					}
					else
					{
						if (flag2)
						{
							compoundPropertyToken.control = fieldControlBody;
						}
						else
						{
							compoundPropertyToken.control = complexControlMatch.Control;
						}
						result = compoundPropertyToken;
					}
				}
			}
			return result;
		}

		// Token: 0x060058A8 RID: 22696 RVA: 0x001CF23C File Offset: 0x001CD43C
		private NewLineToken LoadNewLine(XmlNode newLineNode, int index)
		{
			NewLineToken result;
			using (base.StackFrame(newLineNode, index))
			{
				if (!base.VerifyNodeHasNoChildren(newLineNode))
				{
					result = null;
				}
				else
				{
					NewLineToken newLineToken = new NewLineToken();
					result = newLineToken;
				}
			}
			return result;
		}

		// Token: 0x060058A9 RID: 22697 RVA: 0x001CF284 File Offset: 0x001CD484
		private TextToken LoadText(XmlNode textNode, int index)
		{
			TextToken result;
			using (base.StackFrame(textNode, index))
			{
				result = this.LoadTextToken(textNode);
			}
			return result;
		}

		// Token: 0x060058AA RID: 22698 RVA: 0x001CF2C0 File Offset: 0x001CD4C0
		internal TextToken LoadText(XmlNode textNode)
		{
			TextToken result;
			using (base.StackFrame(textNode))
			{
				result = this.LoadTextToken(textNode);
			}
			return result;
		}

		// Token: 0x060058AB RID: 22699 RVA: 0x001CF2FC File Offset: 0x001CD4FC
		private int LoadIntegerValue(XmlNode node, out bool success)
		{
			int result;
			using (base.StackFrame(node))
			{
				success = false;
				int num = 0;
				if (!base.VerifyNodeHasNoChildren(node))
				{
					result = num;
				}
				else
				{
					string mandatoryInnerText = base.GetMandatoryInnerText(node);
					if (mandatoryInnerText == null)
					{
						base.ReportError(StringUtil.Format(FormatAndOutXmlLoadingStrings.MissingInnerText, base.ComputeCurrentXPath(), base.FilePath));
						result = num;
					}
					else if (!int.TryParse(mandatoryInnerText, out num))
					{
						base.ReportError(StringUtil.Format(FormatAndOutXmlLoadingStrings.ExpectInteger, base.ComputeCurrentXPath(), base.FilePath));
						result = num;
					}
					else
					{
						success = true;
						result = num;
					}
				}
			}
			return result;
		}

		// Token: 0x060058AC RID: 22700 RVA: 0x001CF39C File Offset: 0x001CD59C
		private int LoadPositiveOrZeroIntegerValue(XmlNode node, out bool success)
		{
			int num = this.LoadIntegerValue(node, out success);
			if (!success)
			{
				return num;
			}
			int result;
			using (base.StackFrame(node))
			{
				if (num < 0)
				{
					base.ReportError(StringUtil.Format(FormatAndOutXmlLoadingStrings.ExpectNaturalNumber, base.ComputeCurrentXPath(), base.FilePath));
					success = false;
				}
				result = num;
			}
			return result;
		}

		// Token: 0x060058AD RID: 22701 RVA: 0x001CF404 File Offset: 0x001CD604
		private FrameToken LoadFrameDefinition(XmlNode frameNode, int index)
		{
			FrameToken result;
			using (base.StackFrame(frameNode, index))
			{
				bool flag = false;
				bool flag2 = false;
				bool flag3 = false;
				bool flag4 = false;
				bool flag5 = false;
				FrameToken frameToken = new FrameToken();
				foreach (object obj in frameNode.ChildNodes)
				{
					XmlNode xmlNode = (XmlNode)obj;
					if (base.MatchNodeName(xmlNode, "LeftIndent"))
					{
						if (flag2)
						{
							base.ProcessDuplicateNode(xmlNode);
							return null;
						}
						flag2 = true;
						bool flag6;
						frameToken.frameInfoDefinition.leftIndentation = this.LoadPositiveOrZeroIntegerValue(xmlNode, out flag6);
						if (!flag6)
						{
							return null;
						}
					}
					else if (base.MatchNodeName(xmlNode, "RightIndent"))
					{
						if (flag3)
						{
							base.ProcessDuplicateNode(xmlNode);
							return null;
						}
						flag3 = true;
						bool flag6;
						frameToken.frameInfoDefinition.rightIndentation = this.LoadPositiveOrZeroIntegerValue(xmlNode, out flag6);
						if (!flag6)
						{
							return null;
						}
					}
					else if (base.MatchNodeName(xmlNode, "FirstLineIndent"))
					{
						if (flag4)
						{
							base.ProcessDuplicateAlternateNode(xmlNode, "FirstLineIndent", "FirstLineHanging");
							return null;
						}
						flag4 = true;
						bool flag6;
						frameToken.frameInfoDefinition.firstLine = this.LoadPositiveOrZeroIntegerValue(xmlNode, out flag6);
						if (!flag6)
						{
							return null;
						}
					}
					else if (base.MatchNodeName(xmlNode, "FirstLineHanging"))
					{
						if (flag5)
						{
							base.ProcessDuplicateAlternateNode(xmlNode, "FirstLineIndent", "FirstLineHanging");
							return null;
						}
						flag5 = true;
						bool flag6;
						frameToken.frameInfoDefinition.firstLine = this.LoadPositiveOrZeroIntegerValue(xmlNode, out flag6);
						if (!flag6)
						{
							return null;
						}
						frameToken.frameInfoDefinition.firstLine = -frameToken.frameInfoDefinition.firstLine;
					}
					else if (base.MatchNodeName(xmlNode, "CustomItem"))
					{
						if (flag)
						{
							base.ProcessDuplicateNode(xmlNode);
							return null;
						}
						flag = true;
						frameToken.itemDefinition.formatTokenList = this.LoadComplexControlTokenListDefinitions(xmlNode);
					}
					else
					{
						base.ProcessUnknownNode(xmlNode);
					}
				}
				if (flag5 && flag4)
				{
					base.ProcessDuplicateAlternateNode("FirstLineIndent", "FirstLineHanging");
					result = null;
				}
				else if (frameToken.itemDefinition.formatTokenList == null)
				{
					base.ReportError(StringUtil.Format(FormatAndOutXmlLoadingStrings.MissingNode, new object[]
					{
						base.ComputeCurrentXPath(),
						base.FilePath,
						"CustomItem"
					}));
					result = null;
				}
				else
				{
					result = frameToken;
				}
			}
			return result;
		}

		// Token: 0x060058AE RID: 22702 RVA: 0x001CF6B4 File Offset: 0x001CD8B4
		private bool ReadBooleanNode(XmlNode collectionElement, out bool val)
		{
			val = false;
			if (!base.VerifyNodeHasNoChildren(collectionElement))
			{
				return false;
			}
			string innerText = collectionElement.InnerText;
			if (string.IsNullOrEmpty(innerText))
			{
				val = true;
				return true;
			}
			if (string.Equals(innerText, "FALSE", StringComparison.OrdinalIgnoreCase))
			{
				val = false;
				return true;
			}
			if (string.Equals(innerText, "TRUE", StringComparison.OrdinalIgnoreCase))
			{
				val = true;
				return true;
			}
			base.ReportError(StringUtil.Format(FormatAndOutXmlLoadingStrings.ExpectBoolean, base.ComputeCurrentXPath(), base.FilePath));
			return false;
		}

		// Token: 0x060058AF RID: 22703 RVA: 0x001CF728 File Offset: 0x001CD928
		private ListControlBody LoadListControl(XmlNode controlNode)
		{
			ListControlBody result;
			using (base.StackFrame(controlNode))
			{
				ListControlBody listControlBody = new ListControlBody();
				bool flag = false;
				foreach (object obj in controlNode.ChildNodes)
				{
					XmlNode xmlNode = (XmlNode)obj;
					if (base.MatchNodeName(xmlNode, "ListEntries"))
					{
						if (flag)
						{
							base.ProcessDuplicateNode(xmlNode);
						}
						else
						{
							flag = true;
							this.LoadListControlEntries(xmlNode, listControlBody);
							if (listControlBody.defaultEntryDefinition == null)
							{
								return null;
							}
						}
					}
					else
					{
						base.ProcessUnknownNode(xmlNode);
					}
				}
				if (!flag)
				{
					base.ReportMissingNode("ListEntries");
					result = null;
				}
				else
				{
					result = listControlBody;
				}
			}
			return result;
		}

		// Token: 0x060058B0 RID: 22704 RVA: 0x001CF7F8 File Offset: 0x001CD9F8
		private void LoadListControlEntries(XmlNode listViewEntriesNode, ListControlBody listBody)
		{
			using (base.StackFrame(listViewEntriesNode))
			{
				int num = 0;
				foreach (object obj in listViewEntriesNode.ChildNodes)
				{
					XmlNode xmlNode = (XmlNode)obj;
					if (base.MatchNodeName(xmlNode, "ListEntry"))
					{
						ListControlEntryDefinition listControlEntryDefinition = this.LoadListControlEntryDefinition(xmlNode, num++);
						if (listControlEntryDefinition == null)
						{
							base.ReportError(StringUtil.Format(FormatAndOutXmlLoadingStrings.LoadTagFailed, new object[]
							{
								base.ComputeCurrentXPath(),
								base.FilePath,
								"ListEntry"
							}));
							listBody.defaultEntryDefinition = null;
							return;
						}
						if (listControlEntryDefinition.appliesTo == null)
						{
							if (listBody.defaultEntryDefinition != null)
							{
								base.ReportError(StringUtil.Format(FormatAndOutXmlLoadingStrings.TooManyDefaultShapeEntry, new object[]
								{
									base.ComputeCurrentXPath(),
									base.FilePath,
									"ListEntry"
								}));
								listBody.defaultEntryDefinition = null;
								return;
							}
							listBody.defaultEntryDefinition = listControlEntryDefinition;
						}
						else
						{
							listBody.optionalEntryList.Add(listControlEntryDefinition);
						}
					}
					else
					{
						base.ProcessUnknownNode(xmlNode);
					}
				}
				if (listBody.optionalEntryList == null)
				{
					base.ReportError(StringUtil.Format(FormatAndOutXmlLoadingStrings.NoDefaultShapeEntry, new object[]
					{
						base.ComputeCurrentXPath(),
						base.FilePath,
						"ListEntry"
					}));
				}
			}
		}

		// Token: 0x060058B1 RID: 22705 RVA: 0x001CF9A8 File Offset: 0x001CDBA8
		private ListControlEntryDefinition LoadListControlEntryDefinition(XmlNode listViewEntryNode, int index)
		{
			ListControlEntryDefinition result;
			using (base.StackFrame(listViewEntryNode, index))
			{
				bool flag = false;
				bool flag2 = false;
				ListControlEntryDefinition listControlEntryDefinition = new ListControlEntryDefinition();
				foreach (object obj in listViewEntryNode.ChildNodes)
				{
					XmlNode xmlNode = (XmlNode)obj;
					if (base.MatchNodeName(xmlNode, "EntrySelectedBy"))
					{
						if (flag)
						{
							base.ProcessDuplicateNode(xmlNode);
							return null;
						}
						flag = true;
						listControlEntryDefinition.appliesTo = this.LoadAppliesToSection(xmlNode, true);
					}
					else if (base.MatchNodeName(xmlNode, "ListItems"))
					{
						if (flag2)
						{
							base.ProcessDuplicateNode(xmlNode);
							return null;
						}
						flag2 = true;
						this.LoadListControlItemDefinitions(listControlEntryDefinition, xmlNode);
					}
					else
					{
						base.ProcessUnknownNode(xmlNode);
					}
				}
				if (listControlEntryDefinition.itemDefinitionList == null)
				{
					base.ReportError(StringUtil.Format(FormatAndOutXmlLoadingStrings.NoDefinitionList, base.ComputeCurrentXPath(), base.FilePath));
					result = null;
				}
				else
				{
					result = listControlEntryDefinition;
				}
			}
			return result;
		}

		// Token: 0x060058B2 RID: 22706 RVA: 0x001CFAC0 File Offset: 0x001CDCC0
		private void LoadListControlItemDefinitions(ListControlEntryDefinition lved, XmlNode bodyNode)
		{
			using (base.StackFrame(bodyNode))
			{
				int num = 0;
				foreach (object obj in bodyNode.ChildNodes)
				{
					XmlNode xmlNode = (XmlNode)obj;
					if (base.MatchNodeName(xmlNode, "ListItem"))
					{
						num++;
						ListControlItemDefinition listControlItemDefinition = this.LoadListControlItemDefinition(xmlNode);
						if (listControlItemDefinition == null)
						{
							base.ReportError(StringUtil.Format(FormatAndOutXmlLoadingStrings.InvalidPropertyEntry, base.ComputeCurrentXPath(), base.FilePath));
							lved.itemDefinitionList = null;
							return;
						}
						lved.itemDefinitionList.Add(listControlItemDefinition);
					}
					else
					{
						base.ProcessUnknownNode(xmlNode);
					}
				}
				if (lved.itemDefinitionList.Count == 0)
				{
					base.ReportError(StringUtil.Format(FormatAndOutXmlLoadingStrings.NoListViewItem, base.ComputeCurrentXPath(), base.FilePath));
					lved.itemDefinitionList = null;
				}
			}
		}

		// Token: 0x060058B3 RID: 22707 RVA: 0x001CFBC4 File Offset: 0x001CDDC4
		private ListControlItemDefinition LoadListControlItemDefinition(XmlNode propertyEntryNode)
		{
			ListControlItemDefinition result;
			using (base.StackFrame(propertyEntryNode))
			{
				TypeInfoDataBaseLoader.ViewEntryNodeMatch viewEntryNodeMatch = new TypeInfoDataBaseLoader.ViewEntryNodeMatch(this);
				List<XmlNode> list = new List<XmlNode>();
				if (!viewEntryNodeMatch.ProcessExpressionDirectives(propertyEntryNode, list))
				{
					result = null;
				}
				else
				{
					TextToken textToken = null;
					ExpressionToken expressionToken = null;
					bool flag = false;
					bool flag2 = false;
					foreach (XmlNode xmlNode in list)
					{
						if (base.MatchNodeName(xmlNode, "ItemSelectionCondition"))
						{
							if (flag2)
							{
								base.ProcessDuplicateNode(xmlNode);
								return null;
							}
							flag2 = true;
							expressionToken = this.LoadItemSelectionCondition(xmlNode);
							if (expressionToken == null)
							{
								return null;
							}
						}
						else if (base.MatchNodeNameWithAttributes(xmlNode, "Label"))
						{
							if (flag)
							{
								base.ProcessDuplicateNode(xmlNode);
								return null;
							}
							flag = true;
							textToken = this.LoadLabel(xmlNode);
							if (textToken == null)
							{
								return null;
							}
						}
						else
						{
							base.ProcessUnknownNode(xmlNode);
						}
					}
					ListControlItemDefinition listControlItemDefinition = new ListControlItemDefinition();
					listControlItemDefinition.label = textToken;
					listControlItemDefinition.conditionToken = expressionToken;
					if (viewEntryNodeMatch.TextToken != null)
					{
						listControlItemDefinition.formatTokenList.Add(viewEntryNodeMatch.TextToken);
					}
					else
					{
						FieldPropertyToken fieldPropertyToken = new FieldPropertyToken();
						fieldPropertyToken.expression = viewEntryNodeMatch.Expression;
						fieldPropertyToken.fieldFormattingDirective.formatString = viewEntryNodeMatch.FormatString;
						listControlItemDefinition.formatTokenList.Add(fieldPropertyToken);
					}
					result = listControlItemDefinition;
				}
			}
			return result;
		}

		// Token: 0x060058B4 RID: 22708 RVA: 0x001CFD60 File Offset: 0x001CDF60
		private ExpressionToken LoadItemSelectionCondition(XmlNode itemNode)
		{
			ExpressionToken result;
			using (base.StackFrame(itemNode))
			{
				bool flag = false;
				TypeInfoDataBaseLoader.ExpressionNodeMatch expressionNodeMatch = new TypeInfoDataBaseLoader.ExpressionNodeMatch(this);
				foreach (object obj in itemNode.ChildNodes)
				{
					XmlNode n = (XmlNode)obj;
					if (expressionNodeMatch.MatchNode(n))
					{
						if (flag)
						{
							base.ProcessDuplicateNode(n);
							return null;
						}
						flag = true;
						if (!expressionNodeMatch.ProcessNode(n))
						{
							return null;
						}
					}
					else
					{
						base.ProcessUnknownNode(n);
					}
				}
				result = expressionNodeMatch.GenerateExpressionToken();
			}
			return result;
		}

		// Token: 0x060058B5 RID: 22709 RVA: 0x001CFE1C File Offset: 0x001CE01C
		private ControlBase LoadTableControl(XmlNode controlNode)
		{
			ControlBase result;
			using (base.StackFrame(controlNode))
			{
				TableControlBody tableControlBody = new TableControlBody();
				bool flag = false;
				bool flag2 = false;
				bool flag3 = false;
				bool flag4 = false;
				foreach (object obj in controlNode.ChildNodes)
				{
					XmlNode xmlNode = (XmlNode)obj;
					if (base.MatchNodeName(xmlNode, "HideTableHeaders"))
					{
						if (flag3)
						{
							base.ProcessDuplicateNode(xmlNode);
							return null;
						}
						flag3 = true;
						if (!this.ReadBooleanNode(xmlNode, out tableControlBody.header.hideHeader))
						{
							return null;
						}
					}
					else if (base.MatchNodeName(xmlNode, "AutoSize"))
					{
						if (flag4)
						{
							base.ProcessDuplicateNode(xmlNode);
							return null;
						}
						flag4 = true;
						bool value;
						if (!this.ReadBooleanNode(xmlNode, out value))
						{
							return null;
						}
						tableControlBody.autosize = new bool?(value);
					}
					else if (base.MatchNodeName(xmlNode, "TableHeaders"))
					{
						if (flag)
						{
							base.ProcessDuplicateNode(xmlNode);
							return null;
						}
						flag = true;
						this.LoadHeadersSection(tableControlBody, xmlNode);
						if (tableControlBody.header.columnHeaderDefinitionList == null)
						{
							return null;
						}
					}
					else if (base.MatchNodeName(xmlNode, "TableRowEntries"))
					{
						if (flag2)
						{
							base.ProcessDuplicateNode(xmlNode);
							return null;
						}
						flag2 = true;
						this.LoadRowEntriesSection(tableControlBody, xmlNode);
						if (tableControlBody.defaultDefinition == null)
						{
							return null;
						}
					}
					else
					{
						base.ProcessUnknownNode(xmlNode);
					}
				}
				if (!flag2)
				{
					base.ReportMissingNode("TableRowEntries");
					result = null;
				}
				else if (tableControlBody.header.columnHeaderDefinitionList.Count != 0 && tableControlBody.header.columnHeaderDefinitionList.Count != tableControlBody.defaultDefinition.rowItemDefinitionList.Count)
				{
					base.ReportError(StringUtil.Format(FormatAndOutXmlLoadingStrings.IncorrectHeaderItemCount, new object[]
					{
						base.ComputeCurrentXPath(),
						base.FilePath,
						tableControlBody.header.columnHeaderDefinitionList.Count,
						tableControlBody.defaultDefinition.rowItemDefinitionList.Count
					}));
					result = null;
				}
				else
				{
					if (tableControlBody.optionalDefinitionList.Count != 0)
					{
						int num = 0;
						foreach (TableRowDefinition tableRowDefinition in tableControlBody.optionalDefinitionList)
						{
							if (tableRowDefinition.rowItemDefinitionList.Count != tableControlBody.defaultDefinition.rowItemDefinitionList.Count)
							{
								base.ReportError(StringUtil.Format(FormatAndOutXmlLoadingStrings.IncorrectRowItemCount, new object[]
								{
									base.ComputeCurrentXPath(),
									base.FilePath,
									tableRowDefinition.rowItemDefinitionList.Count,
									tableControlBody.defaultDefinition.rowItemDefinitionList.Count,
									num + 1
								}));
								return null;
							}
							num++;
						}
					}
					result = tableControlBody;
				}
			}
			return result;
		}

		// Token: 0x060058B6 RID: 22710 RVA: 0x001D018C File Offset: 0x001CE38C
		private void LoadHeadersSection(TableControlBody tableBody, XmlNode headersNode)
		{
			using (base.StackFrame(headersNode))
			{
				int num = 0;
				foreach (object obj in headersNode.ChildNodes)
				{
					XmlNode xmlNode = (XmlNode)obj;
					if (base.MatchNodeName(xmlNode, "TableColumnHeader"))
					{
						TableColumnHeaderDefinition tableColumnHeaderDefinition = this.LoadColumnHeaderDefinition(xmlNode, num++);
						if (tableColumnHeaderDefinition == null)
						{
							base.ReportError(StringUtil.Format(FormatAndOutXmlLoadingStrings.InvalidColumnHeader, base.ComputeCurrentXPath(), base.FilePath));
							tableBody.header.columnHeaderDefinitionList = null;
							break;
						}
						tableBody.header.columnHeaderDefinitionList.Add(tableColumnHeaderDefinition);
					}
					else
					{
						base.ProcessUnknownNode(xmlNode);
					}
				}
			}
		}

		// Token: 0x060058B7 RID: 22711 RVA: 0x001D026C File Offset: 0x001CE46C
		private TableColumnHeaderDefinition LoadColumnHeaderDefinition(XmlNode columnHeaderNode, int index)
		{
			TableColumnHeaderDefinition result;
			using (base.StackFrame(columnHeaderNode, index))
			{
				TableColumnHeaderDefinition tableColumnHeaderDefinition = new TableColumnHeaderDefinition();
				bool flag = false;
				bool flag2 = false;
				bool flag3 = false;
				foreach (object obj in columnHeaderNode.ChildNodes)
				{
					XmlNode xmlNode = (XmlNode)obj;
					if (base.MatchNodeNameWithAttributes(xmlNode, "Label"))
					{
						if (flag)
						{
							base.ProcessDuplicateNode(xmlNode);
							return null;
						}
						flag = true;
						tableColumnHeaderDefinition.label = this.LoadLabel(xmlNode);
						if (tableColumnHeaderDefinition.label == null)
						{
							return null;
						}
					}
					else if (base.MatchNodeName(xmlNode, "Width"))
					{
						if (flag2)
						{
							base.ProcessDuplicateNode(xmlNode);
							return null;
						}
						flag2 = true;
						int width;
						if (!this.ReadPositiveIntegerValue(xmlNode, out width))
						{
							base.ReportError(StringUtil.Format(FormatAndOutXmlLoadingStrings.InvalidNodeValue, new object[]
							{
								base.ComputeCurrentXPath(),
								base.FilePath,
								"Width"
							}));
							return null;
						}
						tableColumnHeaderDefinition.width = width;
					}
					else if (base.MatchNodeName(xmlNode, "Alignment"))
					{
						if (flag3)
						{
							base.ProcessDuplicateNode(xmlNode);
							return null;
						}
						flag3 = true;
						if (!this.LoadAlignmentValue(xmlNode, out tableColumnHeaderDefinition.alignment))
						{
							return null;
						}
					}
					else
					{
						base.ProcessUnknownNode(xmlNode);
					}
				}
				result = tableColumnHeaderDefinition;
			}
			return result;
		}

		// Token: 0x060058B8 RID: 22712 RVA: 0x001D0414 File Offset: 0x001CE614
		private bool ReadPositiveIntegerValue(XmlNode n, out int val)
		{
			val = -1;
			string mandatoryInnerText = base.GetMandatoryInnerText(n);
			if (mandatoryInnerText == null)
			{
				return false;
			}
			bool flag = int.TryParse(mandatoryInnerText, out val);
			if (!flag || val <= 0)
			{
				base.ReportError(StringUtil.Format(FormatAndOutXmlLoadingStrings.ExpectPositiveInteger, base.ComputeCurrentXPath(), base.FilePath));
				return false;
			}
			return true;
		}

		// Token: 0x060058B9 RID: 22713 RVA: 0x001D0460 File Offset: 0x001CE660
		private bool LoadAlignmentValue(XmlNode n, out int alignmentValue)
		{
			alignmentValue = 0;
			string mandatoryInnerText = base.GetMandatoryInnerText(n);
			if (mandatoryInnerText == null)
			{
				return false;
			}
			if (string.Equals(n.InnerText, "left", StringComparison.OrdinalIgnoreCase))
			{
				alignmentValue = 1;
			}
			else if (string.Equals(n.InnerText, "right", StringComparison.OrdinalIgnoreCase))
			{
				alignmentValue = 3;
			}
			else
			{
				if (!string.Equals(n.InnerText, "center", StringComparison.OrdinalIgnoreCase))
				{
					base.ReportError(StringUtil.Format(FormatAndOutXmlLoadingStrings.InvalidAlignmentValue, new object[]
					{
						base.ComputeCurrentXPath(),
						base.FilePath,
						mandatoryInnerText
					}));
					return false;
				}
				alignmentValue = 2;
			}
			return true;
		}

		// Token: 0x060058BA RID: 22714 RVA: 0x001D04F8 File Offset: 0x001CE6F8
		private void LoadRowEntriesSection(TableControlBody tableBody, XmlNode rowEntriesNode)
		{
			using (base.StackFrame(rowEntriesNode))
			{
				int num = 0;
				foreach (object obj in rowEntriesNode.ChildNodes)
				{
					XmlNode xmlNode = (XmlNode)obj;
					if (base.MatchNodeName(xmlNode, "TableRowEntry"))
					{
						TableRowDefinition tableRowDefinition = this.LoadRowEntryDefinition(xmlNode, num++);
						if (tableRowDefinition == null)
						{
							base.ReportError(StringUtil.Format(FormatAndOutXmlLoadingStrings.LoadTagFailed, new object[]
							{
								base.ComputeCurrentXPath(),
								base.FilePath,
								"TableRowEntry"
							}));
							tableBody.defaultDefinition = null;
							return;
						}
						if (tableRowDefinition.appliesTo == null)
						{
							if (tableBody.defaultDefinition != null)
							{
								base.ReportError(StringUtil.Format(FormatAndOutXmlLoadingStrings.TooManyDefaultShapeEntry, new object[]
								{
									base.ComputeCurrentXPath(),
									base.FilePath,
									"TableRowEntry"
								}));
								tableBody.defaultDefinition = null;
								return;
							}
							tableBody.defaultDefinition = tableRowDefinition;
						}
						else
						{
							tableBody.optionalDefinitionList.Add(tableRowDefinition);
						}
					}
					else
					{
						base.ProcessUnknownNode(xmlNode);
					}
				}
				if (tableBody.defaultDefinition == null)
				{
					base.ReportError(StringUtil.Format(FormatAndOutXmlLoadingStrings.NoDefaultShapeEntry, new object[]
					{
						base.ComputeCurrentXPath(),
						base.FilePath,
						"TableRowEntry"
					}));
				}
			}
		}

		// Token: 0x060058BB RID: 22715 RVA: 0x001D06A8 File Offset: 0x001CE8A8
		private TableRowDefinition LoadRowEntryDefinition(XmlNode rowEntryNode, int index)
		{
			TableRowDefinition result;
			using (base.StackFrame(rowEntryNode, index))
			{
				bool flag = false;
				bool flag2 = false;
				bool flag3 = false;
				TableRowDefinition tableRowDefinition = new TableRowDefinition();
				foreach (object obj in rowEntryNode.ChildNodes)
				{
					XmlNode xmlNode = (XmlNode)obj;
					if (base.MatchNodeName(xmlNode, "EntrySelectedBy"))
					{
						if (flag)
						{
							base.ProcessDuplicateNode(xmlNode);
							return null;
						}
						flag = true;
						tableRowDefinition.appliesTo = this.LoadAppliesToSection(xmlNode, true);
					}
					else if (base.MatchNodeName(xmlNode, "TableColumnItems"))
					{
						if (flag2)
						{
							base.ProcessDuplicateNode(xmlNode);
							return null;
						}
						this.LoadColumnEntries(xmlNode, tableRowDefinition);
						if (tableRowDefinition.rowItemDefinitionList == null)
						{
							return null;
						}
					}
					else if (base.MatchNodeName(xmlNode, "Wrap"))
					{
						if (flag3)
						{
							base.ProcessDuplicateNode(xmlNode);
							return null;
						}
						flag3 = true;
						if (!this.ReadBooleanNode(xmlNode, out tableRowDefinition.multiLine))
						{
							return null;
						}
					}
					else
					{
						base.ProcessUnknownNode(xmlNode);
					}
				}
				result = tableRowDefinition;
			}
			return result;
		}

		// Token: 0x060058BC RID: 22716 RVA: 0x001D07E8 File Offset: 0x001CE9E8
		private void LoadColumnEntries(XmlNode columnEntriesNode, TableRowDefinition trd)
		{
			using (base.StackFrame(columnEntriesNode))
			{
				int num = 0;
				foreach (object obj in columnEntriesNode.ChildNodes)
				{
					XmlNode xmlNode = (XmlNode)obj;
					if (base.MatchNodeName(xmlNode, "TableColumnItem"))
					{
						TableRowItemDefinition tableRowItemDefinition = this.LoadColumnEntry(xmlNode, num++);
						if (tableRowItemDefinition == null)
						{
							trd.rowItemDefinitionList = null;
							break;
						}
						trd.rowItemDefinitionList.Add(tableRowItemDefinition);
					}
					else
					{
						base.ProcessUnknownNode(xmlNode);
					}
				}
			}
		}

		// Token: 0x060058BD RID: 22717 RVA: 0x001D08A0 File Offset: 0x001CEAA0
		private TableRowItemDefinition LoadColumnEntry(XmlNode columnEntryNode, int index)
		{
			TableRowItemDefinition result;
			using (base.StackFrame(columnEntryNode, index))
			{
				TypeInfoDataBaseLoader.ViewEntryNodeMatch viewEntryNodeMatch = new TypeInfoDataBaseLoader.ViewEntryNodeMatch(this);
				List<XmlNode> list = new List<XmlNode>();
				if (!viewEntryNodeMatch.ProcessExpressionDirectives(columnEntryNode, list))
				{
					result = null;
				}
				else
				{
					TableRowItemDefinition tableRowItemDefinition = new TableRowItemDefinition();
					bool flag = false;
					foreach (XmlNode n in list)
					{
						if (base.MatchNodeName(n, "Alignment"))
						{
							if (flag)
							{
								base.ProcessDuplicateNode(n);
								return null;
							}
							flag = true;
							if (!this.LoadAlignmentValue(n, out tableRowItemDefinition.alignment))
							{
								return null;
							}
						}
						else
						{
							base.ProcessUnknownNode(n);
						}
					}
					if (viewEntryNodeMatch.TextToken != null)
					{
						tableRowItemDefinition.formatTokenList.Add(viewEntryNodeMatch.TextToken);
					}
					else if (viewEntryNodeMatch.Expression != null)
					{
						FieldPropertyToken fieldPropertyToken = new FieldPropertyToken();
						fieldPropertyToken.expression = viewEntryNodeMatch.Expression;
						fieldPropertyToken.fieldFormattingDirective.formatString = viewEntryNodeMatch.FormatString;
						tableRowItemDefinition.formatTokenList.Add(fieldPropertyToken);
					}
					result = tableRowItemDefinition;
				}
			}
			return result;
		}

		// Token: 0x060058BE RID: 22718 RVA: 0x001D09D4 File Offset: 0x001CEBD4
		private void LoadViewDefinitions(TypeInfoDataBase db, XmlNode viewDefinitionsNode)
		{
			using (base.StackFrame(viewDefinitionsNode))
			{
				int num = 0;
				foreach (object obj in viewDefinitionsNode.ChildNodes)
				{
					XmlNode xmlNode = (XmlNode)obj;
					if (base.MatchNodeName(xmlNode, "View"))
					{
						ViewDefinition viewDefinition = this.LoadView(xmlNode, num++);
						if (viewDefinition != null)
						{
							base.ReportTrace(string.Format(CultureInfo.InvariantCulture, "{0} view {1} is loaded from file {2}", new object[]
							{
								ControlBase.GetControlShapeName(viewDefinition.mainControl),
								viewDefinition.name,
								viewDefinition.loadingInfo.filePath
							}));
							db.viewDefinitionsSection.viewDefinitionList.Add(viewDefinition);
						}
					}
					else
					{
						base.ProcessUnknownNode(xmlNode);
					}
				}
			}
		}

		// Token: 0x060058BF RID: 22719 RVA: 0x001D0AD4 File Offset: 0x001CECD4
		private ViewDefinition LoadView(XmlNode viewNode, int index)
		{
			ViewDefinition result;
			using (base.StackFrame(viewNode, index))
			{
				ViewDefinition viewDefinition = new ViewDefinition();
				List<XmlNode> list = new List<XmlNode>();
				if (!this.LoadCommonViewData(viewNode, viewDefinition, list))
				{
					base.ReportError(StringUtil.Format(FormatAndOutXmlLoadingStrings.ViewNotLoaded, base.ComputeCurrentXPath(), base.FilePath));
					result = null;
				}
				else
				{
					string[] names = new string[]
					{
						"TableControl",
						"ListControl",
						"WideControl",
						"CustomControl"
					};
					List<XmlNode> list2 = new List<XmlNode>();
					bool flag = false;
					foreach (XmlNode xmlNode in list)
					{
						if (base.MatchNodeName(xmlNode, "TableControl"))
						{
							if (flag)
							{
								base.ProcessDuplicateNode(xmlNode);
								return null;
							}
							flag = true;
							viewDefinition.mainControl = this.LoadTableControl(xmlNode);
						}
						else if (base.MatchNodeName(xmlNode, "ListControl"))
						{
							if (flag)
							{
								base.ProcessDuplicateNode(xmlNode);
								return null;
							}
							flag = true;
							viewDefinition.mainControl = this.LoadListControl(xmlNode);
						}
						else if (base.MatchNodeName(xmlNode, "WideControl"))
						{
							if (flag)
							{
								base.ProcessDuplicateNode(xmlNode);
								return null;
							}
							flag = true;
							viewDefinition.mainControl = this.LoadWideControl(xmlNode);
						}
						else if (base.MatchNodeName(xmlNode, "CustomControl"))
						{
							if (flag)
							{
								base.ProcessDuplicateNode(xmlNode);
								return null;
							}
							flag = true;
							viewDefinition.mainControl = this.LoadComplexControl(xmlNode);
						}
						else
						{
							list2.Add(xmlNode);
						}
					}
					if (viewDefinition.mainControl == null)
					{
						base.ReportMissingNodes(names);
						result = null;
					}
					else if (!this.LoadMainControlDependentData(list2, viewDefinition))
					{
						result = null;
					}
					else if (viewDefinition.outOfBand && viewDefinition.groupBy != null)
					{
						base.ReportError(StringUtil.Format(FormatAndOutXmlLoadingStrings.OutOfBandGroupByConflict, base.ComputeCurrentXPath(), base.FilePath));
						result = null;
					}
					else
					{
						result = viewDefinition;
					}
				}
			}
			return result;
		}

		// Token: 0x060058C0 RID: 22720 RVA: 0x001D0D14 File Offset: 0x001CEF14
		private bool LoadMainControlDependentData(List<XmlNode> unprocessedNodes, ViewDefinition view)
		{
			foreach (XmlNode xmlNode in unprocessedNodes)
			{
				bool flag = false;
				bool flag2 = false;
				if (base.MatchNodeName(xmlNode, "OutOfBand"))
				{
					if (flag)
					{
						base.ProcessDuplicateNode(xmlNode);
						return false;
					}
					if (!this.ReadBooleanNode(xmlNode, out view.outOfBand))
					{
						return false;
					}
					if (!(view.mainControl is ComplexControlBody) && !(view.mainControl is ListControlBody))
					{
						base.ReportError(StringUtil.Format(FormatAndOutXmlLoadingStrings.InvalidControlForOutOfBandView, base.ComputeCurrentXPath(), base.FilePath));
						return false;
					}
				}
				else if (base.MatchNodeName(xmlNode, "Controls"))
				{
					if (flag2)
					{
						base.ProcessDuplicateNode(xmlNode);
						return false;
					}
					this.LoadControlDefinitions(xmlNode, view.formatControlDefinitionHolder.controlDefinitionList);
				}
				else
				{
					base.ProcessUnknownNode(xmlNode);
				}
			}
			return true;
		}

		// Token: 0x060058C1 RID: 22721 RVA: 0x001D0E14 File Offset: 0x001CF014
		private bool LoadCommonViewData(XmlNode viewNode, ViewDefinition view, List<XmlNode> unprocessedNodes)
		{
			if (viewNode == null)
			{
				throw PSTraceSource.NewArgumentNullException("viewNode");
			}
			if (view == null)
			{
				throw PSTraceSource.NewArgumentNullException("view");
			}
			view.loadingInfo = base.LoadingInfo;
			view.loadingInfo.xPath = base.ComputeCurrentXPath();
			bool flag = false;
			bool flag2 = false;
			bool flag3 = false;
			foreach (object obj in viewNode.ChildNodes)
			{
				XmlNode xmlNode = (XmlNode)obj;
				if (base.MatchNodeName(xmlNode, "Name"))
				{
					if (flag)
					{
						base.ProcessDuplicateNode(xmlNode);
						return false;
					}
					flag = true;
					view.name = base.GetMandatoryInnerText(xmlNode);
					if (view.name == null)
					{
						return false;
					}
				}
				else if (base.MatchNodeName(xmlNode, "ViewSelectedBy"))
				{
					if (flag2)
					{
						base.ProcessDuplicateNode(xmlNode);
						return false;
					}
					flag2 = true;
					view.appliesTo = this.LoadAppliesToSection(xmlNode, false);
					if (view.appliesTo == null)
					{
						return false;
					}
				}
				else if (base.MatchNodeName(xmlNode, "GroupBy"))
				{
					if (flag3)
					{
						base.ProcessDuplicateNode(xmlNode);
						return false;
					}
					flag3 = true;
					view.groupBy = this.LoadGroupBySection(xmlNode);
					if (view.groupBy == null)
					{
						return false;
					}
				}
				else
				{
					unprocessedNodes.Add(xmlNode);
				}
			}
			if (!flag)
			{
				base.ReportMissingNode("Name");
				return false;
			}
			if (!flag2)
			{
				base.ReportMissingNode("ViewSelectedBy");
				return false;
			}
			return true;
		}

		// Token: 0x060058C2 RID: 22722 RVA: 0x001D0F98 File Offset: 0x001CF198
		private void LoadControlDefinitions(XmlNode definitionsNode, List<ControlDefinition> controlDefinitionList)
		{
			using (base.StackFrame(definitionsNode))
			{
				int num = 0;
				foreach (object obj in definitionsNode.ChildNodes)
				{
					XmlNode xmlNode = (XmlNode)obj;
					if (base.MatchNodeName(xmlNode, "Control"))
					{
						ControlDefinition controlDefinition = this.LoadControlDefinition(xmlNode, num++);
						if (controlDefinition != null)
						{
							controlDefinitionList.Add(controlDefinition);
						}
					}
					else
					{
						base.ProcessUnknownNode(xmlNode);
					}
				}
			}
		}

		// Token: 0x060058C3 RID: 22723 RVA: 0x001D1044 File Offset: 0x001CF244
		private ControlDefinition LoadControlDefinition(XmlNode controlDefinitionNode, int index)
		{
			ControlDefinition result;
			using (base.StackFrame(controlDefinitionNode, index))
			{
				bool flag = false;
				bool flag2 = false;
				ControlDefinition controlDefinition = new ControlDefinition();
				foreach (object obj in controlDefinitionNode.ChildNodes)
				{
					XmlNode xmlNode = (XmlNode)obj;
					if (base.MatchNodeName(xmlNode, "Name"))
					{
						if (flag)
						{
							base.ProcessDuplicateNode(xmlNode);
						}
						else
						{
							flag = true;
							controlDefinition.name = base.GetMandatoryInnerText(xmlNode);
							if (controlDefinition.name == null)
							{
								base.ReportError(StringUtil.Format(FormatAndOutXmlLoadingStrings.NullControlName, base.ComputeCurrentXPath(), base.FilePath));
								return null;
							}
						}
					}
					else if (base.MatchNodeName(xmlNode, "CustomControl"))
					{
						if (flag2)
						{
							base.ProcessDuplicateNode(xmlNode);
							return null;
						}
						flag2 = true;
						controlDefinition.controlBody = this.LoadComplexControl(xmlNode);
						if (controlDefinition.controlBody == null)
						{
							return null;
						}
					}
					else
					{
						base.ProcessUnknownNode(xmlNode);
					}
				}
				if (controlDefinition.name == null)
				{
					base.ReportMissingNode("Name");
					result = null;
				}
				else if (controlDefinition.controlBody == null)
				{
					base.ReportMissingNode("CustomControl");
					result = null;
				}
				else
				{
					result = controlDefinition;
				}
			}
			return result;
		}

		// Token: 0x060058C4 RID: 22724 RVA: 0x001D11B8 File Offset: 0x001CF3B8
		private WideControlBody LoadWideControl(XmlNode controlNode)
		{
			WideControlBody result;
			using (base.StackFrame(controlNode))
			{
				WideControlBody wideControlBody = new WideControlBody();
				bool flag = false;
				bool flag2 = false;
				bool flag3 = false;
				foreach (object obj in controlNode.ChildNodes)
				{
					XmlNode xmlNode = (XmlNode)obj;
					if (base.MatchNodeName(xmlNode, "AutoSize"))
					{
						if (flag2)
						{
							base.ProcessDuplicateAlternateNode(xmlNode, "AutoSize", "ColumnNumber");
							return null;
						}
						flag2 = true;
						bool value;
						if (!this.ReadBooleanNode(xmlNode, out value))
						{
							return null;
						}
						wideControlBody.autosize = new bool?(value);
					}
					else if (base.MatchNodeName(xmlNode, "ColumnNumber"))
					{
						if (flag3)
						{
							base.ProcessDuplicateAlternateNode(xmlNode, "AutoSize", "ColumnNumber");
							return null;
						}
						flag3 = true;
						if (!this.ReadPositiveIntegerValue(xmlNode, out wideControlBody.columns))
						{
							return null;
						}
					}
					else if (base.MatchNodeName(xmlNode, "WideEntries"))
					{
						if (flag)
						{
							base.ProcessDuplicateNode(xmlNode);
						}
						else
						{
							flag = true;
							this.LoadWideControlEntries(xmlNode, wideControlBody);
							if (wideControlBody.defaultEntryDefinition == null)
							{
								return null;
							}
						}
					}
					else
					{
						base.ProcessUnknownNode(xmlNode);
					}
				}
				if (flag2 && flag3)
				{
					base.ProcessDuplicateAlternateNode("AutoSize", "ColumnNumber");
					result = null;
				}
				else if (!flag)
				{
					base.ReportMissingNode("WideEntries");
					result = null;
				}
				else
				{
					result = wideControlBody;
				}
			}
			return result;
		}

		// Token: 0x060058C5 RID: 22725 RVA: 0x001D1368 File Offset: 0x001CF568
		private void LoadWideControlEntries(XmlNode wideControlEntriesNode, WideControlBody wideBody)
		{
			using (base.StackFrame(wideControlEntriesNode))
			{
				int num = 0;
				foreach (object obj in wideControlEntriesNode.ChildNodes)
				{
					XmlNode xmlNode = (XmlNode)obj;
					if (base.MatchNodeName(xmlNode, "WideEntry"))
					{
						WideControlEntryDefinition wideControlEntryDefinition = this.LoadWideControlEntry(xmlNode, num++);
						if (wideControlEntryDefinition == null)
						{
							base.ReportError(StringUtil.Format(FormatAndOutXmlLoadingStrings.InvalidNode, new object[]
							{
								base.ComputeCurrentXPath(),
								base.FilePath,
								"WideEntry"
							}));
							return;
						}
						if (wideControlEntryDefinition.appliesTo == null)
						{
							if (wideBody.defaultEntryDefinition != null)
							{
								base.ReportError(StringUtil.Format(FormatAndOutXmlLoadingStrings.TooManyDefaultShapeEntry, new object[]
								{
									base.ComputeCurrentXPath(),
									base.FilePath,
									"WideEntry"
								}));
								wideBody.defaultEntryDefinition = null;
								return;
							}
							wideBody.defaultEntryDefinition = wideControlEntryDefinition;
						}
						else
						{
							wideBody.optionalEntryList.Add(wideControlEntryDefinition);
						}
					}
					else
					{
						base.ProcessUnknownNode(xmlNode);
					}
				}
				if (wideBody.defaultEntryDefinition == null)
				{
					base.ReportError(StringUtil.Format(FormatAndOutXmlLoadingStrings.NoDefaultShapeEntry, new object[]
					{
						base.ComputeCurrentXPath(),
						base.FilePath,
						"WideEntry"
					}));
				}
			}
		}

		// Token: 0x060058C6 RID: 22726 RVA: 0x001D1510 File Offset: 0x001CF710
		private WideControlEntryDefinition LoadWideControlEntry(XmlNode wideControlEntryNode, int index)
		{
			WideControlEntryDefinition result;
			using (base.StackFrame(wideControlEntryNode, index))
			{
				bool flag = false;
				bool flag2 = false;
				WideControlEntryDefinition wideControlEntryDefinition = new WideControlEntryDefinition();
				foreach (object obj in wideControlEntryNode.ChildNodes)
				{
					XmlNode xmlNode = (XmlNode)obj;
					if (base.MatchNodeName(xmlNode, "EntrySelectedBy"))
					{
						if (flag)
						{
							base.ProcessDuplicateNode(xmlNode);
							return null;
						}
						flag = true;
						wideControlEntryDefinition.appliesTo = this.LoadAppliesToSection(xmlNode, true);
					}
					else if (base.MatchNodeName(xmlNode, "WideItem"))
					{
						if (flag2)
						{
							base.ProcessDuplicateNode(xmlNode);
							return null;
						}
						flag2 = true;
						wideControlEntryDefinition.formatTokenList = this.LoadPropertyEntry(xmlNode);
						if (wideControlEntryDefinition.formatTokenList == null)
						{
							base.ReportError(StringUtil.Format(FormatAndOutXmlLoadingStrings.InvalidNode, new object[]
							{
								base.ComputeCurrentXPath(),
								base.FilePath,
								"WideItem"
							}));
							return null;
						}
					}
					else
					{
						base.ProcessUnknownNode(xmlNode);
					}
				}
				if (wideControlEntryDefinition.formatTokenList.Count == 0)
				{
					base.ReportMissingNode("WideItem");
					result = null;
				}
				else
				{
					result = wideControlEntryDefinition;
				}
			}
			return result;
		}

		// Token: 0x060058C7 RID: 22727 RVA: 0x001D1684 File Offset: 0x001CF884
		private List<FormatToken> LoadPropertyEntry(XmlNode propertyEntryNode)
		{
			List<FormatToken> result;
			using (base.StackFrame(propertyEntryNode))
			{
				TypeInfoDataBaseLoader.ViewEntryNodeMatch viewEntryNodeMatch = new TypeInfoDataBaseLoader.ViewEntryNodeMatch(this);
				List<XmlNode> list = new List<XmlNode>();
				if (!viewEntryNodeMatch.ProcessExpressionDirectives(propertyEntryNode, list))
				{
					result = null;
				}
				else
				{
					foreach (XmlNode n in list)
					{
						base.ProcessUnknownNode(n);
					}
					List<FormatToken> list2 = new List<FormatToken>();
					if (viewEntryNodeMatch.TextToken != null)
					{
						list2.Add(viewEntryNodeMatch.TextToken);
					}
					else
					{
						list2.Add(new FieldPropertyToken
						{
							expression = viewEntryNodeMatch.Expression,
							fieldFormattingDirective = 
							{
								formatString = viewEntryNodeMatch.FormatString
							}
						});
					}
					result = list2;
				}
			}
			return result;
		}

		// Token: 0x04002F72 RID: 12146
		private const string resBaseName = "TypeInfoDataBaseLoaderStrings";

		// Token: 0x04002F73 RID: 12147
		[TraceSource("TypeInfoDataBaseLoader", "TypeInfoDataBaseLoader")]
		private static PSTraceSource tracer = PSTraceSource.GetTracer("TypeInfoDataBaseLoader", "TypeInfoDataBaseLoader");

		// Token: 0x04002F74 RID: 12148
		private bool suppressValidation;

		// Token: 0x0200096F RID: 2415
		private static class XmlTags
		{
			// Token: 0x04002F75 RID: 12149
			internal const string DefaultSettingsNode = "DefaultSettings";

			// Token: 0x04002F76 RID: 12150
			internal const string ConfigurationNode = "Configuration";

			// Token: 0x04002F77 RID: 12151
			internal const string SelectionSetsNode = "SelectionSets";

			// Token: 0x04002F78 RID: 12152
			internal const string ViewDefinitionsNode = "ViewDefinitions";

			// Token: 0x04002F79 RID: 12153
			internal const string ControlsNode = "Controls";

			// Token: 0x04002F7A RID: 12154
			internal const string MultilineTablesNode = "WrapTables";

			// Token: 0x04002F7B RID: 12155
			internal const string PropertyCountForTableNode = "PropertyCountForTable";

			// Token: 0x04002F7C RID: 12156
			internal const string ShowErrorsAsMessagesNode = "ShowError";

			// Token: 0x04002F7D RID: 12157
			internal const string ShowErrorsInFormattedOutputNode = "DisplayError";

			// Token: 0x04002F7E RID: 12158
			internal const string EnumerableExpansionsNode = "EnumerableExpansions";

			// Token: 0x04002F7F RID: 12159
			internal const string EnumerableExpansionNode = "EnumerableExpansion";

			// Token: 0x04002F80 RID: 12160
			internal const string ExpandNode = "Expand";

			// Token: 0x04002F81 RID: 12161
			internal const string ControlNode = "Control";

			// Token: 0x04002F82 RID: 12162
			internal const string ComplexControlNameNode = "CustomControlName";

			// Token: 0x04002F83 RID: 12163
			internal const string SelectionSetNode = "SelectionSet";

			// Token: 0x04002F84 RID: 12164
			internal const string SelectionSetNameNode = "SelectionSetName";

			// Token: 0x04002F85 RID: 12165
			internal const string SelectionConditionNode = "SelectionCondition";

			// Token: 0x04002F86 RID: 12166
			internal const string NameNode = "Name";

			// Token: 0x04002F87 RID: 12167
			internal const string TypesNode = "Types";

			// Token: 0x04002F88 RID: 12168
			internal const string TypeNameNode = "TypeName";

			// Token: 0x04002F89 RID: 12169
			internal const string ViewNode = "View";

			// Token: 0x04002F8A RID: 12170
			internal const string TableControlNode = "TableControl";

			// Token: 0x04002F8B RID: 12171
			internal const string ListControlNode = "ListControl";

			// Token: 0x04002F8C RID: 12172
			internal const string WideControlNode = "WideControl";

			// Token: 0x04002F8D RID: 12173
			internal const string ComplexControlNode = "CustomControl";

			// Token: 0x04002F8E RID: 12174
			internal const string FieldControlNode = "FieldControl";

			// Token: 0x04002F8F RID: 12175
			internal const string ViewSelectedByNode = "ViewSelectedBy";

			// Token: 0x04002F90 RID: 12176
			internal const string GroupByNode = "GroupBy";

			// Token: 0x04002F91 RID: 12177
			internal const string OutOfBandNode = "OutOfBand";

			// Token: 0x04002F92 RID: 12178
			internal const string HideTableHeadersNode = "HideTableHeaders";

			// Token: 0x04002F93 RID: 12179
			internal const string TableHeadersNode = "TableHeaders";

			// Token: 0x04002F94 RID: 12180
			internal const string TableColumnHeaderNode = "TableColumnHeader";

			// Token: 0x04002F95 RID: 12181
			internal const string TableRowEntriesNode = "TableRowEntries";

			// Token: 0x04002F96 RID: 12182
			internal const string TableRowEntryNode = "TableRowEntry";

			// Token: 0x04002F97 RID: 12183
			internal const string MultiLineNode = "Wrap";

			// Token: 0x04002F98 RID: 12184
			internal const string TableColumnItemsNode = "TableColumnItems";

			// Token: 0x04002F99 RID: 12185
			internal const string TableColumnItemNode = "TableColumnItem";

			// Token: 0x04002F9A RID: 12186
			internal const string WidthNode = "Width";

			// Token: 0x04002F9B RID: 12187
			internal const string ListEntriesNode = "ListEntries";

			// Token: 0x04002F9C RID: 12188
			internal const string ListEntryNode = "ListEntry";

			// Token: 0x04002F9D RID: 12189
			internal const string ListItemsNode = "ListItems";

			// Token: 0x04002F9E RID: 12190
			internal const string ListItemNode = "ListItem";

			// Token: 0x04002F9F RID: 12191
			internal const string ColumnNumberNode = "ColumnNumber";

			// Token: 0x04002FA0 RID: 12192
			internal const string WideEntriesNode = "WideEntries";

			// Token: 0x04002FA1 RID: 12193
			internal const string WideEntryNode = "WideEntry";

			// Token: 0x04002FA2 RID: 12194
			internal const string WideItemNode = "WideItem";

			// Token: 0x04002FA3 RID: 12195
			internal const string ComplexEntriesNode = "CustomEntries";

			// Token: 0x04002FA4 RID: 12196
			internal const string ComplexEntryNode = "CustomEntry";

			// Token: 0x04002FA5 RID: 12197
			internal const string ComplexItemNode = "CustomItem";

			// Token: 0x04002FA6 RID: 12198
			internal const string ExpressionBindingNode = "ExpressionBinding";

			// Token: 0x04002FA7 RID: 12199
			internal const string NewLineNode = "NewLine";

			// Token: 0x04002FA8 RID: 12200
			internal const string TextNode = "Text";

			// Token: 0x04002FA9 RID: 12201
			internal const string FrameNode = "Frame";

			// Token: 0x04002FAA RID: 12202
			internal const string LeftIndentNode = "LeftIndent";

			// Token: 0x04002FAB RID: 12203
			internal const string RightIndentNode = "RightIndent";

			// Token: 0x04002FAC RID: 12204
			internal const string FirstLineIndentNode = "FirstLineIndent";

			// Token: 0x04002FAD RID: 12205
			internal const string FirstLineHangingNode = "FirstLineHanging";

			// Token: 0x04002FAE RID: 12206
			internal const string EnumerateCollectionNode = "EnumerateCollection";

			// Token: 0x04002FAF RID: 12207
			internal const string AutoSizeNode = "AutoSize";

			// Token: 0x04002FB0 RID: 12208
			internal const string AlignmentNode = "Alignment";

			// Token: 0x04002FB1 RID: 12209
			internal const string PropertyNameNode = "PropertyName";

			// Token: 0x04002FB2 RID: 12210
			internal const string ScriptBlockNode = "ScriptBlock";

			// Token: 0x04002FB3 RID: 12211
			internal const string FormatStringNode = "FormatString";

			// Token: 0x04002FB4 RID: 12212
			internal const string LabelNode = "Label";

			// Token: 0x04002FB5 RID: 12213
			internal const string EntrySelectedByNode = "EntrySelectedBy";

			// Token: 0x04002FB6 RID: 12214
			internal const string ItemSelectionConditionNode = "ItemSelectionCondition";

			// Token: 0x04002FB7 RID: 12215
			internal const string AssemblyNameAttribute = "AssemblyName";

			// Token: 0x04002FB8 RID: 12216
			internal const string BaseNameAttribute = "BaseName";

			// Token: 0x04002FB9 RID: 12217
			internal const string ResourceIdAttribute = "ResourceId";
		}

		// Token: 0x02000970 RID: 2416
		private static class XMLStringValues
		{
			// Token: 0x04002FBA RID: 12218
			internal const string True = "TRUE";

			// Token: 0x04002FBB RID: 12219
			internal const string False = "FALSE";

			// Token: 0x04002FBC RID: 12220
			internal const string AligmentLeft = "left";

			// Token: 0x04002FBD RID: 12221
			internal const string AligmentCenter = "center";

			// Token: 0x04002FBE RID: 12222
			internal const string AligmentRight = "right";
		}

		// Token: 0x02000971 RID: 2417
		private sealed class ExpressionNodeMatch
		{
			// Token: 0x060058CA RID: 22730 RVA: 0x001D1782 File Offset: 0x001CF982
			internal ExpressionNodeMatch(TypeInfoDataBaseLoader loader)
			{
				this._loader = loader;
			}

			// Token: 0x060058CB RID: 22731 RVA: 0x001D1791 File Offset: 0x001CF991
			internal bool MatchNode(XmlNode n)
			{
				return this._loader.MatchNodeName(n, "PropertyName") || this._loader.MatchNodeName(n, "ScriptBlock");
			}

			// Token: 0x060058CC RID: 22732 RVA: 0x001D17BC File Offset: 0x001CF9BC
			internal bool ProcessNode(XmlNode n)
			{
				if (this._loader.MatchNodeName(n, "PropertyName"))
				{
					if (this._token != null)
					{
						if (this._token.isScriptBlock)
						{
							this._loader.ProcessDuplicateAlternateNode(n, "PropertyName", "ScriptBlock");
						}
						else
						{
							this._loader.ProcessDuplicateNode(n);
						}
						return false;
					}
					this._token = new ExpressionToken();
					this._token.expressionValue = this._loader.GetMandatoryInnerText(n);
					if (this._token.expressionValue == null)
					{
						this._loader.ReportError(StringUtil.Format(FormatAndOutXmlLoadingStrings.NoProperty, this._loader.ComputeCurrentXPath(), this._loader.FilePath));
						this._fatalError = true;
						return false;
					}
					return true;
				}
				else
				{
					if (!this._loader.MatchNodeName(n, "ScriptBlock"))
					{
						PSTraceSource.NewInvalidOperationException();
						return false;
					}
					if (this._token != null)
					{
						if (!this._token.isScriptBlock)
						{
							this._loader.ProcessDuplicateAlternateNode(n, "PropertyName", "ScriptBlock");
						}
						else
						{
							this._loader.ProcessDuplicateNode(n);
						}
						return false;
					}
					this._token = new ExpressionToken();
					this._token.isScriptBlock = true;
					this._token.expressionValue = this._loader.GetMandatoryInnerText(n);
					if (this._token.expressionValue == null)
					{
						this._loader.ReportError(StringUtil.Format(FormatAndOutXmlLoadingStrings.NoScriptBlockText, this._loader.ComputeCurrentXPath(), this._loader.FilePath));
						this._fatalError = true;
						return false;
					}
					if (!this._loader.suppressValidation && !this._loader.VerifyScriptBlock(this._token.expressionValue))
					{
						this._fatalError = true;
						return false;
					}
					return true;
				}
			}

			// Token: 0x060058CD RID: 22733 RVA: 0x001D1978 File Offset: 0x001CFB78
			internal ExpressionToken GenerateExpressionToken()
			{
				if (this._fatalError)
				{
					return null;
				}
				if (this._token == null)
				{
					this._loader.ReportMissingNodes(new string[]
					{
						"PropertyName",
						"ScriptBlock"
					});
					return null;
				}
				return this._token;
			}

			// Token: 0x04002FBF RID: 12223
			private TypeInfoDataBaseLoader _loader;

			// Token: 0x04002FC0 RID: 12224
			private ExpressionToken _token;

			// Token: 0x04002FC1 RID: 12225
			private bool _fatalError;
		}

		// Token: 0x02000972 RID: 2418
		private sealed class ViewEntryNodeMatch
		{
			// Token: 0x060058CE RID: 22734 RVA: 0x001D19C2 File Offset: 0x001CFBC2
			internal ViewEntryNodeMatch(TypeInfoDataBaseLoader loader)
			{
				this._loader = loader;
			}

			// Token: 0x060058CF RID: 22735 RVA: 0x001D19D4 File Offset: 0x001CFBD4
			internal bool ProcessExpressionDirectives(XmlNode containerNode, List<XmlNode> unprocessedNodes)
			{
				if (containerNode == null)
				{
					throw PSTraceSource.NewArgumentNullException("containerNode");
				}
				string text = null;
				TextToken textToken = null;
				TypeInfoDataBaseLoader.ExpressionNodeMatch expressionNodeMatch = new TypeInfoDataBaseLoader.ExpressionNodeMatch(this._loader);
				bool flag = false;
				bool flag2 = false;
				bool flag3 = false;
				foreach (object obj in containerNode.ChildNodes)
				{
					XmlNode xmlNode = (XmlNode)obj;
					if (expressionNodeMatch.MatchNode(xmlNode))
					{
						if (flag2)
						{
							this._loader.ProcessDuplicateNode(xmlNode);
							return false;
						}
						flag2 = true;
						if (!expressionNodeMatch.ProcessNode(xmlNode))
						{
							return false;
						}
					}
					else if (this._loader.MatchNodeName(xmlNode, "FormatString"))
					{
						if (flag)
						{
							this._loader.ProcessDuplicateNode(xmlNode);
							return false;
						}
						flag = true;
						text = this._loader.GetMandatoryInnerText(xmlNode);
						if (text == null)
						{
							this._loader.ReportError(StringUtil.Format(FormatAndOutXmlLoadingStrings.NoFormatString, this._loader.ComputeCurrentXPath(), this._loader.FilePath));
							return false;
						}
					}
					else if (this._loader.MatchNodeNameWithAttributes(xmlNode, "Text"))
					{
						if (flag3)
						{
							this._loader.ProcessDuplicateNode(xmlNode);
							return false;
						}
						flag3 = true;
						textToken = this._loader.LoadText(xmlNode);
						if (textToken == null)
						{
							this._loader.ReportError(StringUtil.Format(FormatAndOutXmlLoadingStrings.InvalidNode, new object[]
							{
								this._loader.ComputeCurrentXPath(),
								this._loader.FilePath,
								"Text"
							}));
							return false;
						}
					}
					else
					{
						unprocessedNodes.Add(xmlNode);
					}
				}
				if (flag2)
				{
					if (flag3)
					{
						this._loader.ReportError(StringUtil.Format(FormatAndOutXmlLoadingStrings.NodeWithExpression, new object[]
						{
							this._loader.ComputeCurrentXPath(),
							this._loader.FilePath,
							"Text"
						}));
						return false;
					}
					ExpressionToken expressionToken = expressionNodeMatch.GenerateExpressionToken();
					if (expressionToken == null)
					{
						return false;
					}
					if (!string.IsNullOrEmpty(text))
					{
						this._formatString = text;
					}
					this._expression = expressionToken;
				}
				else
				{
					if (flag)
					{
						this._loader.ReportError(StringUtil.Format(FormatAndOutXmlLoadingStrings.NodeWithoutExpression, new object[]
						{
							this._loader.ComputeCurrentXPath(),
							this._loader.FilePath,
							"FormatString"
						}));
						return false;
					}
					if (flag3)
					{
						this._textToken = textToken;
					}
				}
				return true;
			}

			// Token: 0x170011DE RID: 4574
			// (get) Token: 0x060058D0 RID: 22736 RVA: 0x001D1C80 File Offset: 0x001CFE80
			internal string FormatString
			{
				get
				{
					return this._formatString;
				}
			}

			// Token: 0x170011DF RID: 4575
			// (get) Token: 0x060058D1 RID: 22737 RVA: 0x001D1C88 File Offset: 0x001CFE88
			internal TextToken TextToken
			{
				get
				{
					return this._textToken;
				}
			}

			// Token: 0x170011E0 RID: 4576
			// (get) Token: 0x060058D2 RID: 22738 RVA: 0x001D1C90 File Offset: 0x001CFE90
			internal ExpressionToken Expression
			{
				get
				{
					return this._expression;
				}
			}

			// Token: 0x04002FC2 RID: 12226
			private string _formatString;

			// Token: 0x04002FC3 RID: 12227
			private TextToken _textToken;

			// Token: 0x04002FC4 RID: 12228
			private ExpressionToken _expression;

			// Token: 0x04002FC5 RID: 12229
			private TypeInfoDataBaseLoader _loader;
		}

		// Token: 0x02000973 RID: 2419
		private sealed class ComplexControlMatch
		{
			// Token: 0x060058D3 RID: 22739 RVA: 0x001D1C98 File Offset: 0x001CFE98
			internal ComplexControlMatch(TypeInfoDataBaseLoader loader)
			{
				this._loader = loader;
			}

			// Token: 0x060058D4 RID: 22740 RVA: 0x001D1CA7 File Offset: 0x001CFEA7
			internal bool MatchNode(XmlNode n)
			{
				return this._loader.MatchNodeName(n, "CustomControl") || this._loader.MatchNodeName(n, "CustomControlName");
			}

			// Token: 0x060058D5 RID: 22741 RVA: 0x001D1CD0 File Offset: 0x001CFED0
			internal bool ProcessNode(XmlNode n)
			{
				if (this._loader.MatchNodeName(n, "CustomControl"))
				{
					this._control = this._loader.LoadComplexControl(n);
					return true;
				}
				if (!this._loader.MatchNodeName(n, "CustomControlName"))
				{
					PSTraceSource.NewInvalidOperationException();
					return false;
				}
				string mandatoryInnerText = this._loader.GetMandatoryInnerText(n);
				if (mandatoryInnerText == null)
				{
					return false;
				}
				this._control = new ControlReference
				{
					name = mandatoryInnerText,
					controlType = typeof(ComplexControlBody)
				};
				return true;
			}

			// Token: 0x170011E1 RID: 4577
			// (get) Token: 0x060058D6 RID: 22742 RVA: 0x001D1D56 File Offset: 0x001CFF56
			internal ControlBase Control
			{
				get
				{
					return this._control;
				}
			}

			// Token: 0x04002FC6 RID: 12230
			private ControlBase _control;

			// Token: 0x04002FC7 RID: 12231
			private TypeInfoDataBaseLoader _loader;
		}
	}
}
