using System;
using System.Management.Automation.Internal;

namespace System.Management.Automation.Provider
{
	// Token: 0x02000465 RID: 1125
	public abstract class ContainerCmdletProvider : ItemCmdletProvider
	{
		// Token: 0x060031DD RID: 12765 RVA: 0x0010EAA0 File Offset: 0x0010CCA0
		internal void GetChildItems(string path, bool recurse, uint depth, CmdletProviderContext context)
		{
			base.Context = context;
			this.GetChildItems(path, recurse, depth);
		}

		// Token: 0x060031DE RID: 12766 RVA: 0x0010EAB3 File Offset: 0x0010CCB3
		internal object GetChildItemsDynamicParameters(string path, bool recurse, CmdletProviderContext context)
		{
			base.Context = context;
			return this.GetChildItemsDynamicParameters(path, recurse);
		}

		// Token: 0x060031DF RID: 12767 RVA: 0x0010EAC4 File Offset: 0x0010CCC4
		internal void GetChildNames(string path, ReturnContainers returnContainers, CmdletProviderContext context)
		{
			base.Context = context;
			this.GetChildNames(path, returnContainers);
		}

		// Token: 0x060031E0 RID: 12768 RVA: 0x0010EAD5 File Offset: 0x0010CCD5
		internal virtual bool ConvertPath(string path, string filter, ref string updatedPath, ref string updatedFilter, CmdletProviderContext context)
		{
			base.Context = context;
			return this.ConvertPath(path, filter, ref updatedPath, ref updatedFilter);
		}

		// Token: 0x060031E1 RID: 12769 RVA: 0x0010EAEA File Offset: 0x0010CCEA
		internal object GetChildNamesDynamicParameters(string path, CmdletProviderContext context)
		{
			base.Context = context;
			return this.GetChildNamesDynamicParameters(path);
		}

		// Token: 0x060031E2 RID: 12770 RVA: 0x0010EAFA File Offset: 0x0010CCFA
		internal void RenameItem(string path, string newName, CmdletProviderContext context)
		{
			base.Context = context;
			this.RenameItem(path, newName);
		}

		// Token: 0x060031E3 RID: 12771 RVA: 0x0010EB0B File Offset: 0x0010CD0B
		internal object RenameItemDynamicParameters(string path, string newName, CmdletProviderContext context)
		{
			base.Context = context;
			return this.RenameItemDynamicParameters(path, newName);
		}

		// Token: 0x060031E4 RID: 12772 RVA: 0x0010EB1C File Offset: 0x0010CD1C
		internal void NewItem(string path, string type, object newItemValue, CmdletProviderContext context)
		{
			base.Context = context;
			this.NewItem(path, type, newItemValue);
		}

		// Token: 0x060031E5 RID: 12773 RVA: 0x0010EB2F File Offset: 0x0010CD2F
		internal object NewItemDynamicParameters(string path, string type, object newItemValue, CmdletProviderContext context)
		{
			base.Context = context;
			return this.NewItemDynamicParameters(path, type, newItemValue);
		}

		// Token: 0x060031E6 RID: 12774 RVA: 0x0010EB42 File Offset: 0x0010CD42
		internal void RemoveItem(string path, bool recurse, CmdletProviderContext context)
		{
			base.Context = context;
			this.RemoveItem(path, recurse);
		}

		// Token: 0x060031E7 RID: 12775 RVA: 0x0010EB53 File Offset: 0x0010CD53
		internal object RemoveItemDynamicParameters(string path, bool recurse, CmdletProviderContext context)
		{
			base.Context = context;
			return this.RemoveItemDynamicParameters(path, recurse);
		}

		// Token: 0x060031E8 RID: 12776 RVA: 0x0010EB64 File Offset: 0x0010CD64
		internal bool HasChildItems(string path, CmdletProviderContext context)
		{
			base.Context = context;
			return this.HasChildItems(path);
		}

		// Token: 0x060031E9 RID: 12777 RVA: 0x0010EB74 File Offset: 0x0010CD74
		internal void CopyItem(string path, string copyPath, bool recurse, CmdletProviderContext context)
		{
			base.Context = context;
			this.CopyItem(path, copyPath, recurse);
		}

		// Token: 0x060031EA RID: 12778 RVA: 0x0010EB87 File Offset: 0x0010CD87
		internal object CopyItemDynamicParameters(string path, string destination, bool recurse, CmdletProviderContext context)
		{
			base.Context = context;
			return this.CopyItemDynamicParameters(path, destination, recurse);
		}

		// Token: 0x060031EB RID: 12779 RVA: 0x0010EB9C File Offset: 0x0010CD9C
		protected virtual void GetChildItems(string path, bool recurse)
		{
			IDisposable engineProtectionScope = PSTransactionManager.GetEngineProtectionScope();
			try
			{
				throw PSTraceSource.NewNotSupportedException(SessionStateStrings.CmdletProvider_NotSupported, new object[0]);
			}
			finally
			{
				if (engineProtectionScope != null)
				{
					engineProtectionScope.Dispose();
					goto IL_20;
				}
				goto IL_20;
				IL_20:;
			}
		}

		// Token: 0x060031EC RID: 12780 RVA: 0x0010EBDC File Offset: 0x0010CDDC
		protected virtual void GetChildItems(string path, bool recurse, uint depth)
		{
			using (PSTransactionManager.GetEngineProtectionScope())
			{
				if (depth != 4294967295U)
				{
					throw PSTraceSource.NewNotSupportedException(SessionStateStrings.CmdletProvider_NotSupportedRecursionDepth, new object[0]);
				}
				this.GetChildItems(path, recurse);
			}
		}

		// Token: 0x060031ED RID: 12781 RVA: 0x0010EC2C File Offset: 0x0010CE2C
		protected virtual object GetChildItemsDynamicParameters(string path, bool recurse)
		{
			object result;
			using (PSTransactionManager.GetEngineProtectionScope())
			{
				result = null;
			}
			return result;
		}

		// Token: 0x060031EE RID: 12782 RVA: 0x0010EC60 File Offset: 0x0010CE60
		protected virtual void GetChildNames(string path, ReturnContainers returnContainers)
		{
			IDisposable engineProtectionScope = PSTransactionManager.GetEngineProtectionScope();
			try
			{
				throw PSTraceSource.NewNotSupportedException(SessionStateStrings.CmdletProvider_NotSupported, new object[0]);
			}
			finally
			{
				if (engineProtectionScope != null)
				{
					engineProtectionScope.Dispose();
					goto IL_20;
				}
				goto IL_20;
				IL_20:;
			}
		}

		// Token: 0x060031EF RID: 12783 RVA: 0x0010ECA0 File Offset: 0x0010CEA0
		protected virtual bool ConvertPath(string path, string filter, ref string updatedPath, ref string updatedFilter)
		{
			bool result;
			using (PSTransactionManager.GetEngineProtectionScope())
			{
				result = false;
			}
			return result;
		}

		// Token: 0x060031F0 RID: 12784 RVA: 0x0010ECD4 File Offset: 0x0010CED4
		protected virtual object GetChildNamesDynamicParameters(string path)
		{
			object result;
			using (PSTransactionManager.GetEngineProtectionScope())
			{
				result = null;
			}
			return result;
		}

		// Token: 0x060031F1 RID: 12785 RVA: 0x0010ED08 File Offset: 0x0010CF08
		protected virtual void RenameItem(string path, string newName)
		{
			IDisposable engineProtectionScope = PSTransactionManager.GetEngineProtectionScope();
			try
			{
				throw PSTraceSource.NewNotSupportedException(SessionStateStrings.CmdletProvider_NotSupported, new object[0]);
			}
			finally
			{
				if (engineProtectionScope != null)
				{
					engineProtectionScope.Dispose();
					goto IL_20;
				}
				goto IL_20;
				IL_20:;
			}
		}

		// Token: 0x060031F2 RID: 12786 RVA: 0x0010ED48 File Offset: 0x0010CF48
		protected virtual object RenameItemDynamicParameters(string path, string newName)
		{
			object result;
			using (PSTransactionManager.GetEngineProtectionScope())
			{
				result = null;
			}
			return result;
		}

		// Token: 0x060031F3 RID: 12787 RVA: 0x0010ED7C File Offset: 0x0010CF7C
		protected virtual void NewItem(string path, string itemTypeName, object newItemValue)
		{
			IDisposable engineProtectionScope = PSTransactionManager.GetEngineProtectionScope();
			try
			{
				throw PSTraceSource.NewNotSupportedException(SessionStateStrings.CmdletProvider_NotSupported, new object[0]);
			}
			finally
			{
				if (engineProtectionScope != null)
				{
					engineProtectionScope.Dispose();
					goto IL_20;
				}
				goto IL_20;
				IL_20:;
			}
		}

		// Token: 0x060031F4 RID: 12788 RVA: 0x0010EDBC File Offset: 0x0010CFBC
		protected virtual object NewItemDynamicParameters(string path, string itemTypeName, object newItemValue)
		{
			object result;
			using (PSTransactionManager.GetEngineProtectionScope())
			{
				result = null;
			}
			return result;
		}

		// Token: 0x060031F5 RID: 12789 RVA: 0x0010EDF0 File Offset: 0x0010CFF0
		protected virtual void RemoveItem(string path, bool recurse)
		{
			IDisposable engineProtectionScope = PSTransactionManager.GetEngineProtectionScope();
			try
			{
				throw PSTraceSource.NewNotSupportedException(SessionStateStrings.CmdletProvider_NotSupported, new object[0]);
			}
			finally
			{
				if (engineProtectionScope != null)
				{
					engineProtectionScope.Dispose();
					goto IL_20;
				}
				goto IL_20;
				IL_20:;
			}
		}

		// Token: 0x060031F6 RID: 12790 RVA: 0x0010EE30 File Offset: 0x0010D030
		protected virtual object RemoveItemDynamicParameters(string path, bool recurse)
		{
			object result;
			using (PSTransactionManager.GetEngineProtectionScope())
			{
				result = null;
			}
			return result;
		}

		// Token: 0x060031F7 RID: 12791 RVA: 0x0010EE64 File Offset: 0x0010D064
		protected virtual bool HasChildItems(string path)
		{
			IDisposable engineProtectionScope = PSTransactionManager.GetEngineProtectionScope();
			try
			{
				throw PSTraceSource.NewNotSupportedException(SessionStateStrings.CmdletProvider_NotSupported, new object[0]);
			}
			finally
			{
				if (engineProtectionScope != null)
				{
					engineProtectionScope.Dispose();
					goto IL_20;
				}
				goto IL_20;
				IL_20:;
			}
		}

		// Token: 0x060031F8 RID: 12792 RVA: 0x0010EEA4 File Offset: 0x0010D0A4
		protected virtual void CopyItem(string path, string copyPath, bool recurse)
		{
			IDisposable engineProtectionScope = PSTransactionManager.GetEngineProtectionScope();
			try
			{
				throw PSTraceSource.NewNotSupportedException(SessionStateStrings.CmdletProvider_NotSupported, new object[0]);
			}
			finally
			{
				if (engineProtectionScope != null)
				{
					engineProtectionScope.Dispose();
					goto IL_20;
				}
				goto IL_20;
				IL_20:;
			}
		}

		// Token: 0x060031F9 RID: 12793 RVA: 0x0010EEE4 File Offset: 0x0010D0E4
		protected virtual object CopyItemDynamicParameters(string path, string destination, bool recurse)
		{
			object result;
			using (PSTransactionManager.GetEngineProtectionScope())
			{
				result = null;
			}
			return result;
		}
	}
}
