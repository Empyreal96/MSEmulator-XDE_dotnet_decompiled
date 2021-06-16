using System;
using System.Management.Automation.Internal;

namespace System.Management.Automation.Provider
{
	// Token: 0x02000464 RID: 1124
	public abstract class ItemCmdletProvider : DriveCmdletProvider
	{
		// Token: 0x060031C4 RID: 12740 RVA: 0x0010E6E4 File Offset: 0x0010C8E4
		internal void GetItem(string path, CmdletProviderContext context)
		{
			base.Context = context;
			this.GetItem(path);
		}

		// Token: 0x060031C5 RID: 12741 RVA: 0x0010E6F4 File Offset: 0x0010C8F4
		internal object GetItemDynamicParameters(string path, CmdletProviderContext context)
		{
			base.Context = context;
			return this.GetItemDynamicParameters(path);
		}

		// Token: 0x060031C6 RID: 12742 RVA: 0x0010E704 File Offset: 0x0010C904
		internal void SetItem(string path, object value, CmdletProviderContext context)
		{
			CmdletProvider.providerBaseTracer.WriteLine("ItemCmdletProvider.SetItem", new object[0]);
			base.Context = context;
			this.SetItem(path, value);
		}

		// Token: 0x060031C7 RID: 12743 RVA: 0x0010E72A File Offset: 0x0010C92A
		internal object SetItemDynamicParameters(string path, object value, CmdletProviderContext context)
		{
			base.Context = context;
			return this.SetItemDynamicParameters(path, value);
		}

		// Token: 0x060031C8 RID: 12744 RVA: 0x0010E73B File Offset: 0x0010C93B
		internal void ClearItem(string path, CmdletProviderContext context)
		{
			CmdletProvider.providerBaseTracer.WriteLine("ItemCmdletProvider.ClearItem", new object[0]);
			base.Context = context;
			this.ClearItem(path);
		}

		// Token: 0x060031C9 RID: 12745 RVA: 0x0010E760 File Offset: 0x0010C960
		internal object ClearItemDynamicParameters(string path, CmdletProviderContext context)
		{
			base.Context = context;
			return this.ClearItemDynamicParameters(path);
		}

		// Token: 0x060031CA RID: 12746 RVA: 0x0010E770 File Offset: 0x0010C970
		internal void InvokeDefaultAction(string path, CmdletProviderContext context)
		{
			CmdletProvider.providerBaseTracer.WriteLine("ItemCmdletProvider.InvokeDefaultAction", new object[0]);
			base.Context = context;
			this.InvokeDefaultAction(path);
		}

		// Token: 0x060031CB RID: 12747 RVA: 0x0010E795 File Offset: 0x0010C995
		internal object InvokeDefaultActionDynamicParameters(string path, CmdletProviderContext context)
		{
			base.Context = context;
			return this.InvokeDefaultActionDynamicParameters(path);
		}

		// Token: 0x060031CC RID: 12748 RVA: 0x0010E7A8 File Offset: 0x0010C9A8
		internal bool ItemExists(string path, CmdletProviderContext context)
		{
			base.Context = context;
			bool result = false;
			try
			{
				result = this.ItemExists(path);
			}
			catch (Exception e)
			{
				CommandProcessorBase.CheckForSevereException(e);
			}
			return result;
		}

		// Token: 0x060031CD RID: 12749 RVA: 0x0010E7E4 File Offset: 0x0010C9E4
		internal object ItemExistsDynamicParameters(string path, CmdletProviderContext context)
		{
			base.Context = context;
			return this.ItemExistsDynamicParameters(path);
		}

		// Token: 0x060031CE RID: 12750 RVA: 0x0010E7F4 File Offset: 0x0010C9F4
		internal bool IsValidPath(string path, CmdletProviderContext context)
		{
			base.Context = context;
			return this.IsValidPath(path);
		}

		// Token: 0x060031CF RID: 12751 RVA: 0x0010E804 File Offset: 0x0010CA04
		internal string[] ExpandPath(string path, CmdletProviderContext context)
		{
			base.Context = context;
			return this.ExpandPath(path);
		}

		// Token: 0x060031D0 RID: 12752 RVA: 0x0010E814 File Offset: 0x0010CA14
		protected virtual void GetItem(string path)
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

		// Token: 0x060031D1 RID: 12753 RVA: 0x0010E854 File Offset: 0x0010CA54
		protected virtual object GetItemDynamicParameters(string path)
		{
			object result;
			using (PSTransactionManager.GetEngineProtectionScope())
			{
				result = null;
			}
			return result;
		}

		// Token: 0x060031D2 RID: 12754 RVA: 0x0010E888 File Offset: 0x0010CA88
		protected virtual void SetItem(string path, object value)
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

		// Token: 0x060031D3 RID: 12755 RVA: 0x0010E8C8 File Offset: 0x0010CAC8
		protected virtual object SetItemDynamicParameters(string path, object value)
		{
			object result;
			using (PSTransactionManager.GetEngineProtectionScope())
			{
				result = null;
			}
			return result;
		}

		// Token: 0x060031D4 RID: 12756 RVA: 0x0010E8FC File Offset: 0x0010CAFC
		protected virtual void ClearItem(string path)
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

		// Token: 0x060031D5 RID: 12757 RVA: 0x0010E93C File Offset: 0x0010CB3C
		protected virtual object ClearItemDynamicParameters(string path)
		{
			object result;
			using (PSTransactionManager.GetEngineProtectionScope())
			{
				result = null;
			}
			return result;
		}

		// Token: 0x060031D6 RID: 12758 RVA: 0x0010E970 File Offset: 0x0010CB70
		protected virtual void InvokeDefaultAction(string path)
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

		// Token: 0x060031D7 RID: 12759 RVA: 0x0010E9B0 File Offset: 0x0010CBB0
		protected virtual object InvokeDefaultActionDynamicParameters(string path)
		{
			object result;
			using (PSTransactionManager.GetEngineProtectionScope())
			{
				result = null;
			}
			return result;
		}

		// Token: 0x060031D8 RID: 12760 RVA: 0x0010E9E4 File Offset: 0x0010CBE4
		protected virtual bool ItemExists(string path)
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

		// Token: 0x060031D9 RID: 12761 RVA: 0x0010EA24 File Offset: 0x0010CC24
		protected virtual object ItemExistsDynamicParameters(string path)
		{
			object result;
			using (PSTransactionManager.GetEngineProtectionScope())
			{
				result = null;
			}
			return result;
		}

		// Token: 0x060031DA RID: 12762
		protected abstract bool IsValidPath(string path);

		// Token: 0x060031DB RID: 12763 RVA: 0x0010EA58 File Offset: 0x0010CC58
		protected virtual string[] ExpandPath(string path)
		{
			string[] result;
			using (PSTransactionManager.GetEngineProtectionScope())
			{
				result = new string[]
				{
					path
				};
			}
			return result;
		}
	}
}
