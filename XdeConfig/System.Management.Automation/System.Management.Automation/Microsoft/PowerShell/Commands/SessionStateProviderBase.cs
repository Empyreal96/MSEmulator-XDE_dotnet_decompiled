using System;
using System.Collections;
using System.Collections.Generic;
using System.Management.Automation;
using System.Management.Automation.Provider;
using System.Security;

namespace Microsoft.PowerShell.Commands
{
	// Token: 0x02000761 RID: 1889
	public abstract class SessionStateProviderBase : ContainerCmdletProvider, IContentCmdletProvider
	{
		// Token: 0x06004B79 RID: 19321
		internal abstract object GetSessionStateItem(string name);

		// Token: 0x06004B7A RID: 19322
		internal abstract void SetSessionStateItem(string name, object value, bool writeItem);

		// Token: 0x06004B7B RID: 19323
		internal abstract void RemoveSessionStateItem(string name);

		// Token: 0x06004B7C RID: 19324
		internal abstract IDictionary GetSessionStateTable();

		// Token: 0x06004B7D RID: 19325 RVA: 0x0018AF14 File Offset: 0x00189114
		internal virtual object GetValueOfItem(object item)
		{
			object result = item;
			if (item is DictionaryEntry)
			{
				result = ((DictionaryEntry)item).Value;
			}
			return result;
		}

		// Token: 0x06004B7E RID: 19326 RVA: 0x0018AF3C File Offset: 0x0018913C
		internal virtual bool CanRenameItem(object item)
		{
			bool flag = true;
			SessionStateProviderBase.tracer.WriteLine("result = {0}", new object[]
			{
				flag
			});
			return flag;
		}

		// Token: 0x06004B7F RID: 19327 RVA: 0x0018AF6C File Offset: 0x0018916C
		protected override void GetItem(string name)
		{
			bool isContainer = false;
			object obj = null;
			IDictionary sessionStateTable = this.GetSessionStateTable();
			if (sessionStateTable != null)
			{
				if (string.IsNullOrEmpty(name))
				{
					isContainer = true;
					obj = sessionStateTable.Values;
				}
				else
				{
					obj = sessionStateTable[name];
				}
			}
			if (obj != null && SessionState.IsVisible(base.Context.Origin, obj))
			{
				base.WriteItemObject(obj, name, isContainer);
			}
		}

		// Token: 0x06004B80 RID: 19328 RVA: 0x0018AFC4 File Offset: 0x001891C4
		protected override void SetItem(string name, object value)
		{
			if (string.IsNullOrEmpty(name))
			{
				base.WriteError(new ErrorRecord(PSTraceSource.NewArgumentNullException("name"), "SetItemNullName", ErrorCategory.InvalidArgument, name));
				return;
			}
			try
			{
				string setItemAction = SessionStateProviderBaseStrings.SetItemAction;
				string setItemResourceTemplate = SessionStateProviderBaseStrings.SetItemResourceTemplate;
				string target = string.Format(base.Host.CurrentCulture, setItemResourceTemplate, new object[]
				{
					name,
					value
				});
				if (base.ShouldProcess(target, setItemAction))
				{
					this.SetSessionStateItem(name, value, true);
				}
			}
			catch (SessionStateException ex)
			{
				base.WriteError(new ErrorRecord(ex.ErrorRecord, ex));
			}
			catch (PSArgumentException ex2)
			{
				base.WriteError(new ErrorRecord(ex2.ErrorRecord, ex2));
			}
		}

		// Token: 0x06004B81 RID: 19329 RVA: 0x0018B088 File Offset: 0x00189288
		protected override void ClearItem(string path)
		{
			if (string.IsNullOrEmpty(path))
			{
				base.WriteError(new ErrorRecord(PSTraceSource.NewArgumentNullException("path"), "ClearItemNullPath", ErrorCategory.InvalidArgument, path));
				return;
			}
			try
			{
				string clearItemAction = SessionStateProviderBaseStrings.ClearItemAction;
				string clearItemResourceTemplate = SessionStateProviderBaseStrings.ClearItemResourceTemplate;
				string target = string.Format(base.Host.CurrentCulture, clearItemResourceTemplate, new object[]
				{
					path
				});
				if (base.ShouldProcess(target, clearItemAction))
				{
					this.SetSessionStateItem(path, null, false);
				}
			}
			catch (SessionStateException ex)
			{
				base.WriteError(new ErrorRecord(ex.ErrorRecord, ex));
			}
			catch (PSArgumentException ex2)
			{
				base.WriteError(new ErrorRecord(ex2.ErrorRecord, ex2));
			}
		}

		// Token: 0x06004B82 RID: 19330 RVA: 0x0018B180 File Offset: 0x00189380
		protected override void GetChildItems(string path, bool recurse)
		{
			CommandOrigin origin = base.Context.Origin;
			if (string.IsNullOrEmpty(path))
			{
				IDictionary dictionary = null;
				try
				{
					dictionary = this.GetSessionStateTable();
				}
				catch (SecurityException exception)
				{
					base.WriteError(new ErrorRecord(exception, "GetTableSecurityException", ErrorCategory.ReadError, path));
					return;
				}
				List<DictionaryEntry> list = new List<DictionaryEntry>(dictionary.Count + 1);
				foreach (object obj in dictionary)
				{
					DictionaryEntry item = (DictionaryEntry)obj;
					list.Add(item);
				}
				list.Sort(delegate(DictionaryEntry left, DictionaryEntry right)
				{
					string x = (string)left.Key;
					string y = (string)right.Key;
					IComparer<string> currentCultureIgnoreCase = StringComparer.CurrentCultureIgnoreCase;
					return currentCultureIgnoreCase.Compare(x, y);
				});
				using (List<DictionaryEntry>.Enumerator enumerator2 = list.GetEnumerator())
				{
					while (enumerator2.MoveNext())
					{
						DictionaryEntry dictionaryEntry = enumerator2.Current;
						try
						{
							if (SessionState.IsVisible(origin, dictionaryEntry.Value))
							{
								base.WriteItemObject(dictionaryEntry.Value, (string)dictionaryEntry.Key, false);
							}
						}
						catch (PSArgumentException ex)
						{
							base.WriteError(new ErrorRecord(ex.ErrorRecord, ex));
							break;
						}
						catch (SecurityException exception2)
						{
							base.WriteError(new ErrorRecord(exception2, "GetItemSecurityException", ErrorCategory.PermissionDenied, (string)dictionaryEntry.Key));
							break;
						}
					}
					return;
				}
			}
			object obj2 = null;
			try
			{
				obj2 = this.GetSessionStateItem(path);
			}
			catch (PSArgumentException ex2)
			{
				base.WriteError(new ErrorRecord(ex2.ErrorRecord, ex2));
				return;
			}
			catch (SecurityException exception3)
			{
				base.WriteError(new ErrorRecord(exception3, "GetItemSecurityException", ErrorCategory.PermissionDenied, path));
				return;
			}
			if (obj2 != null && SessionState.IsVisible(origin, obj2))
			{
				base.WriteItemObject(obj2, path, false);
			}
		}

		// Token: 0x06004B83 RID: 19331 RVA: 0x0018B380 File Offset: 0x00189580
		protected override void GetChildNames(string path, ReturnContainers returnContainers)
		{
			CommandOrigin origin = base.Context.Origin;
			if (string.IsNullOrEmpty(path))
			{
				IDictionary dictionary = null;
				try
				{
					dictionary = this.GetSessionStateTable();
				}
				catch (SecurityException exception)
				{
					base.WriteError(new ErrorRecord(exception, "GetChildNamesSecurityException", ErrorCategory.ReadError, path));
					return;
				}
				using (IDictionaryEnumerator enumerator = dictionary.GetEnumerator())
				{
					while (enumerator.MoveNext())
					{
						object obj = enumerator.Current;
						DictionaryEntry dictionaryEntry = (DictionaryEntry)obj;
						try
						{
							if (SessionState.IsVisible(origin, dictionaryEntry.Value))
							{
								base.WriteItemObject(dictionaryEntry.Key, (string)dictionaryEntry.Key, false);
							}
						}
						catch (PSArgumentException ex)
						{
							base.WriteError(new ErrorRecord(ex.ErrorRecord, ex));
							break;
						}
						catch (SecurityException exception2)
						{
							base.WriteError(new ErrorRecord(exception2, "GetItemSecurityException", ErrorCategory.PermissionDenied, (string)dictionaryEntry.Key));
							break;
						}
					}
					return;
				}
			}
			object obj2 = null;
			try
			{
				obj2 = this.GetSessionStateItem(path);
			}
			catch (SecurityException exception3)
			{
				base.WriteError(new ErrorRecord(exception3, "GetChildNamesSecurityException", ErrorCategory.ReadError, path));
				return;
			}
			if (obj2 != null && SessionState.IsVisible(origin, obj2))
			{
				base.WriteItemObject(path, path, false);
			}
		}

		// Token: 0x06004B84 RID: 19332 RVA: 0x0018B4E8 File Offset: 0x001896E8
		protected override bool HasChildItems(string path)
		{
			bool flag = false;
			if (string.IsNullOrEmpty(path))
			{
				try
				{
					if (this.GetSessionStateTable().Count > 0)
					{
						flag = true;
					}
					goto IL_35;
				}
				catch (SecurityException exception)
				{
					base.WriteError(new ErrorRecord(exception, "HasChildItemsSecurityException", ErrorCategory.ReadError, path));
					goto IL_35;
				}
			}
			flag = false;
			IL_35:
			SessionStateProviderBase.tracer.WriteLine("result = {0}", new object[]
			{
				flag
			});
			return flag;
		}

		// Token: 0x06004B85 RID: 19333 RVA: 0x0018B55C File Offset: 0x0018975C
		protected override bool ItemExists(string path)
		{
			bool flag = false;
			if (string.IsNullOrEmpty(path))
			{
				flag = true;
			}
			else
			{
				object obj = null;
				try
				{
					obj = this.GetSessionStateItem(path);
				}
				catch (SecurityException exception)
				{
					base.WriteError(new ErrorRecord(exception, "ItemExistsSecurityException", ErrorCategory.ReadError, path));
				}
				if (obj != null)
				{
					flag = true;
				}
			}
			SessionStateProviderBase.tracer.WriteLine("result = {0}", new object[]
			{
				flag
			});
			return flag;
		}

		// Token: 0x06004B86 RID: 19334 RVA: 0x0018B5D0 File Offset: 0x001897D0
		protected override bool IsValidPath(string path)
		{
			bool flag = true;
			if (string.IsNullOrEmpty(path))
			{
				flag = false;
			}
			SessionStateProviderBase.tracer.WriteLine("result = {0}", new object[]
			{
				flag
			});
			return flag;
		}

		// Token: 0x06004B87 RID: 19335 RVA: 0x0018B60C File Offset: 0x0018980C
		protected override void RemoveItem(string path, bool recurse)
		{
			if (string.IsNullOrEmpty(path))
			{
				Exception exception = PSTraceSource.NewArgumentException("path");
				base.WriteError(new ErrorRecord(exception, "RemoveItemNullPath", ErrorCategory.InvalidArgument, path));
				return;
			}
			string removeItemAction = SessionStateProviderBaseStrings.RemoveItemAction;
			string removeItemResourceTemplate = SessionStateProviderBaseStrings.RemoveItemResourceTemplate;
			string target = string.Format(base.Host.CurrentCulture, removeItemResourceTemplate, new object[]
			{
				path
			});
			if (base.ShouldProcess(target, removeItemAction))
			{
				try
				{
					this.RemoveSessionStateItem(path);
				}
				catch (SessionStateException ex)
				{
					base.WriteError(new ErrorRecord(ex.ErrorRecord, ex));
				}
				catch (SecurityException exception2)
				{
					base.WriteError(new ErrorRecord(exception2, "RemoveItemSecurityException", ErrorCategory.PermissionDenied, path));
				}
				catch (PSArgumentException ex2)
				{
					base.WriteError(new ErrorRecord(ex2.ErrorRecord, ex2));
				}
			}
		}

		// Token: 0x06004B88 RID: 19336 RVA: 0x0018B6F4 File Offset: 0x001898F4
		protected override void NewItem(string path, string type, object newItem)
		{
			if (string.IsNullOrEmpty(path))
			{
				Exception exception = PSTraceSource.NewArgumentException("path");
				base.WriteError(new ErrorRecord(exception, "NewItemNullPath", ErrorCategory.InvalidArgument, path));
				return;
			}
			if (newItem == null)
			{
				ArgumentNullException exception2 = PSTraceSource.NewArgumentNullException("value");
				base.WriteError(new ErrorRecord(exception2, "NewItemValueNotSpecified", ErrorCategory.InvalidArgument, path));
				return;
			}
			if (this.ItemExists(path) && !base.Force)
			{
				PSArgumentException ex = PSTraceSource.NewArgumentException("path", SessionStateStrings.NewItemAlreadyExists, new object[]
				{
					path
				});
				base.WriteError(new ErrorRecord(ex.ErrorRecord, ex));
				return;
			}
			string newItemAction = SessionStateProviderBaseStrings.NewItemAction;
			string newItemResourceTemplate = SessionStateProviderBaseStrings.NewItemResourceTemplate;
			string target = string.Format(base.Host.CurrentCulture, newItemResourceTemplate, new object[]
			{
				path,
				type,
				newItem
			});
			if (base.ShouldProcess(target, newItemAction))
			{
				this.SetItem(path, newItem);
			}
		}

		// Token: 0x06004B89 RID: 19337 RVA: 0x0018B7E0 File Offset: 0x001899E0
		protected override void CopyItem(string path, string copyPath, bool recurse)
		{
			if (string.IsNullOrEmpty(path))
			{
				Exception exception = PSTraceSource.NewArgumentException("path");
				base.WriteError(new ErrorRecord(exception, "CopyItemNullPath", ErrorCategory.InvalidArgument, path));
				return;
			}
			if (string.IsNullOrEmpty(copyPath))
			{
				this.GetItem(path);
				return;
			}
			object obj = null;
			try
			{
				obj = this.GetSessionStateItem(path);
			}
			catch (SecurityException exception2)
			{
				base.WriteError(new ErrorRecord(exception2, "CopyItemSecurityException", ErrorCategory.ReadError, path));
				return;
			}
			if (obj != null)
			{
				string copyItemAction = SessionStateProviderBaseStrings.CopyItemAction;
				string copyItemResourceTemplate = SessionStateProviderBaseStrings.CopyItemResourceTemplate;
				string target = string.Format(base.Host.CurrentCulture, copyItemResourceTemplate, new object[]
				{
					path,
					copyPath
				});
				if (!base.ShouldProcess(target, copyItemAction))
				{
					return;
				}
				try
				{
					this.SetSessionStateItem(copyPath, this.GetValueOfItem(obj), true);
					return;
				}
				catch (SessionStateException ex)
				{
					base.WriteError(new ErrorRecord(ex.ErrorRecord, ex));
					return;
				}
				catch (PSArgumentException ex2)
				{
					base.WriteError(new ErrorRecord(ex2.ErrorRecord, ex2));
					return;
				}
			}
			PSArgumentException ex3 = PSTraceSource.NewArgumentException("path", SessionStateStrings.CopyItemDoesntExist, new object[]
			{
				path
			});
			base.WriteError(new ErrorRecord(ex3.ErrorRecord, ex3));
		}

		// Token: 0x06004B8A RID: 19338 RVA: 0x0018B930 File Offset: 0x00189B30
		protected override void RenameItem(string name, string newName)
		{
			if (string.IsNullOrEmpty(name))
			{
				Exception exception = PSTraceSource.NewArgumentException("name");
				base.WriteError(new ErrorRecord(exception, "RenameItemNullPath", ErrorCategory.InvalidArgument, name));
				return;
			}
			object obj = null;
			try
			{
				obj = this.GetSessionStateItem(name);
			}
			catch (SecurityException exception2)
			{
				base.WriteError(new ErrorRecord(exception2, "RenameItemSecurityException", ErrorCategory.ReadError, name));
				return;
			}
			if (obj != null)
			{
				if (this.ItemExists(newName) && !base.Force)
				{
					PSArgumentException ex = PSTraceSource.NewArgumentException("newName", SessionStateStrings.NewItemAlreadyExists, new object[]
					{
						newName
					});
					base.WriteError(new ErrorRecord(ex.ErrorRecord, ex));
					return;
				}
				try
				{
					if (this.CanRenameItem(obj))
					{
						string renameItemAction = SessionStateProviderBaseStrings.RenameItemAction;
						string renameItemResourceTemplate = SessionStateProviderBaseStrings.RenameItemResourceTemplate;
						string target = string.Format(base.Host.CurrentCulture, renameItemResourceTemplate, new object[]
						{
							name,
							newName
						});
						if (base.ShouldProcess(target, renameItemAction))
						{
							if (string.Equals(name, newName, StringComparison.OrdinalIgnoreCase))
							{
								this.GetItem(newName);
								return;
							}
							try
							{
								this.SetSessionStateItem(newName, obj, true);
								this.RemoveSessionStateItem(name);
							}
							catch (SessionStateException ex2)
							{
								base.WriteError(new ErrorRecord(ex2.ErrorRecord, ex2));
								return;
							}
							catch (PSArgumentException ex3)
							{
								base.WriteError(new ErrorRecord(ex3.ErrorRecord, ex3));
								return;
							}
							catch (SecurityException exception3)
							{
								base.WriteError(new ErrorRecord(exception3, "RenameItemSecurityException", ErrorCategory.PermissionDenied, name));
								return;
							}
						}
					}
					return;
				}
				catch (SessionStateException ex4)
				{
					base.WriteError(new ErrorRecord(ex4.ErrorRecord, ex4));
					return;
				}
			}
			PSArgumentException ex5 = PSTraceSource.NewArgumentException("name", SessionStateStrings.RenameItemDoesntExist, new object[]
			{
				name
			});
			base.WriteError(new ErrorRecord(ex5.ErrorRecord, ex5));
		}

		// Token: 0x06004B8B RID: 19339 RVA: 0x0018BB2C File Offset: 0x00189D2C
		public IContentReader GetContentReader(string path)
		{
			return new SessionStateProviderBaseContentReaderWriter(path, this);
		}

		// Token: 0x06004B8C RID: 19340 RVA: 0x0018BB35 File Offset: 0x00189D35
		public IContentWriter GetContentWriter(string path)
		{
			return new SessionStateProviderBaseContentReaderWriter(path, this);
		}

		// Token: 0x06004B8D RID: 19341 RVA: 0x0018BB3E File Offset: 0x00189D3E
		public void ClearContent(string path)
		{
			throw PSTraceSource.NewNotSupportedException(SessionStateStrings.IContent_Clear_NotSupported, new object[0]);
		}

		// Token: 0x06004B8E RID: 19342 RVA: 0x0018BB50 File Offset: 0x00189D50
		public object GetContentReaderDynamicParameters(string path)
		{
			return null;
		}

		// Token: 0x06004B8F RID: 19343 RVA: 0x0018BB53 File Offset: 0x00189D53
		public object GetContentWriterDynamicParameters(string path)
		{
			return null;
		}

		// Token: 0x06004B90 RID: 19344 RVA: 0x0018BB56 File Offset: 0x00189D56
		public object ClearContentDynamicParameters(string path)
		{
			return null;
		}

		// Token: 0x0400247B RID: 9339
		[TraceSource("SessionStateProvider", "Providers that produce a view of session state data.")]
		private static readonly PSTraceSource tracer = PSTraceSource.GetTracer("SessionStateProvider", "Providers that produce a view of session state data.");
	}
}
