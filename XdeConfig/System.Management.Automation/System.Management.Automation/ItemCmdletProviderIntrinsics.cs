using System;
using System.Collections.ObjectModel;

namespace System.Management.Automation
{
	// Token: 0x0200002E RID: 46
	public sealed class ItemCmdletProviderIntrinsics
	{
		// Token: 0x06000220 RID: 544 RVA: 0x00008ECC File Offset: 0x000070CC
		private ItemCmdletProviderIntrinsics()
		{
		}

		// Token: 0x06000221 RID: 545 RVA: 0x00008ED4 File Offset: 0x000070D4
		internal ItemCmdletProviderIntrinsics(Cmdlet cmdlet)
		{
			if (cmdlet == null)
			{
				throw PSTraceSource.NewArgumentNullException("cmdlet");
			}
			this.cmdlet = cmdlet;
			this.sessionState = cmdlet.Context.EngineSessionState;
		}

		// Token: 0x06000222 RID: 546 RVA: 0x00008F02 File Offset: 0x00007102
		internal ItemCmdletProviderIntrinsics(SessionStateInternal sessionState)
		{
			if (sessionState == null)
			{
				throw PSTraceSource.NewArgumentNullException("sessionState");
			}
			this.sessionState = sessionState;
		}

		// Token: 0x06000223 RID: 547 RVA: 0x00008F20 File Offset: 0x00007120
		public Collection<PSObject> Get(string path)
		{
			return this.sessionState.GetItem(new string[]
			{
				path
			}, false, false);
		}

		// Token: 0x06000224 RID: 548 RVA: 0x00008F46 File Offset: 0x00007146
		public Collection<PSObject> Get(string[] path, bool force, bool literalPath)
		{
			return this.sessionState.GetItem(path, force, literalPath);
		}

		// Token: 0x06000225 RID: 549 RVA: 0x00008F58 File Offset: 0x00007158
		internal void Get(string path, CmdletProviderContext context)
		{
			this.sessionState.GetItem(new string[]
			{
				path
			}, context);
		}

		// Token: 0x06000226 RID: 550 RVA: 0x00008F7D File Offset: 0x0000717D
		internal object GetItemDynamicParameters(string path, CmdletProviderContext context)
		{
			return this.sessionState.GetItemDynamicParameters(path, context);
		}

		// Token: 0x06000227 RID: 551 RVA: 0x00008F8C File Offset: 0x0000718C
		public Collection<PSObject> Set(string path, object value)
		{
			return this.sessionState.SetItem(new string[]
			{
				path
			}, value, false, false);
		}

		// Token: 0x06000228 RID: 552 RVA: 0x00008FB3 File Offset: 0x000071B3
		public Collection<PSObject> Set(string[] path, object value, bool force, bool literalPath)
		{
			return this.sessionState.SetItem(path, value, force, literalPath);
		}

		// Token: 0x06000229 RID: 553 RVA: 0x00008FC8 File Offset: 0x000071C8
		internal void Set(string path, object value, CmdletProviderContext context)
		{
			this.sessionState.SetItem(new string[]
			{
				path
			}, value, context);
		}

		// Token: 0x0600022A RID: 554 RVA: 0x00008FEE File Offset: 0x000071EE
		internal object SetItemDynamicParameters(string path, object value, CmdletProviderContext context)
		{
			return this.sessionState.SetItemDynamicParameters(path, value, context);
		}

		// Token: 0x0600022B RID: 555 RVA: 0x00009000 File Offset: 0x00007200
		public Collection<PSObject> Clear(string path)
		{
			return this.sessionState.ClearItem(new string[]
			{
				path
			}, false, false);
		}

		// Token: 0x0600022C RID: 556 RVA: 0x00009026 File Offset: 0x00007226
		public Collection<PSObject> Clear(string[] path, bool force, bool literalPath)
		{
			return this.sessionState.ClearItem(path, force, literalPath);
		}

		// Token: 0x0600022D RID: 557 RVA: 0x00009038 File Offset: 0x00007238
		internal void Clear(string path, CmdletProviderContext context)
		{
			this.sessionState.ClearItem(new string[]
			{
				path
			}, context);
		}

		// Token: 0x0600022E RID: 558 RVA: 0x0000905D File Offset: 0x0000725D
		internal object ClearItemDynamicParameters(string path, CmdletProviderContext context)
		{
			return this.sessionState.ClearItemDynamicParameters(path, context);
		}

		// Token: 0x0600022F RID: 559 RVA: 0x0000906C File Offset: 0x0000726C
		public void Invoke(string path)
		{
			this.sessionState.InvokeDefaultAction(new string[]
			{
				path
			}, false);
		}

		// Token: 0x06000230 RID: 560 RVA: 0x00009091 File Offset: 0x00007291
		public void Invoke(string[] path, bool literalPath)
		{
			this.sessionState.InvokeDefaultAction(path, literalPath);
		}

		// Token: 0x06000231 RID: 561 RVA: 0x000090A0 File Offset: 0x000072A0
		internal void Invoke(string path, CmdletProviderContext context)
		{
			this.sessionState.InvokeDefaultAction(new string[]
			{
				path
			}, context);
		}

		// Token: 0x06000232 RID: 562 RVA: 0x000090C5 File Offset: 0x000072C5
		internal object InvokeItemDynamicParameters(string path, CmdletProviderContext context)
		{
			return this.sessionState.InvokeDefaultActionDynamicParameters(path, context);
		}

		// Token: 0x06000233 RID: 563 RVA: 0x000090D4 File Offset: 0x000072D4
		public Collection<PSObject> Rename(string path, string newName)
		{
			return this.sessionState.RenameItem(path, newName, false);
		}

		// Token: 0x06000234 RID: 564 RVA: 0x000090E4 File Offset: 0x000072E4
		public Collection<PSObject> Rename(string path, string newName, bool force)
		{
			return this.sessionState.RenameItem(path, newName, force);
		}

		// Token: 0x06000235 RID: 565 RVA: 0x000090F4 File Offset: 0x000072F4
		internal void Rename(string path, string newName, CmdletProviderContext context)
		{
			this.sessionState.RenameItem(path, newName, context);
		}

		// Token: 0x06000236 RID: 566 RVA: 0x00009104 File Offset: 0x00007304
		internal object RenameItemDynamicParameters(string path, string newName, CmdletProviderContext context)
		{
			return this.sessionState.RenameItemDynamicParameters(path, newName, context);
		}

		// Token: 0x06000237 RID: 567 RVA: 0x00009114 File Offset: 0x00007314
		public Collection<PSObject> New(string path, string name, string itemTypeName, object content)
		{
			return this.sessionState.NewItem(new string[]
			{
				path
			}, name, itemTypeName, content, false);
		}

		// Token: 0x06000238 RID: 568 RVA: 0x0000913D File Offset: 0x0000733D
		public Collection<PSObject> New(string[] path, string name, string itemTypeName, object content, bool force)
		{
			return this.sessionState.NewItem(path, name, itemTypeName, content, force);
		}

		// Token: 0x06000239 RID: 569 RVA: 0x00009154 File Offset: 0x00007354
		internal void New(string path, string name, string type, object content, CmdletProviderContext context)
		{
			this.sessionState.NewItem(new string[]
			{
				path
			}, name, type, content, context);
		}

		// Token: 0x0600023A RID: 570 RVA: 0x0000917E File Offset: 0x0000737E
		internal object NewItemDynamicParameters(string path, string type, object content, CmdletProviderContext context)
		{
			return this.sessionState.NewItemDynamicParameters(path, type, content, context);
		}

		// Token: 0x0600023B RID: 571 RVA: 0x00009190 File Offset: 0x00007390
		public void Remove(string path, bool recurse)
		{
			this.sessionState.RemoveItem(new string[]
			{
				path
			}, recurse, false, false);
		}

		// Token: 0x0600023C RID: 572 RVA: 0x000091B7 File Offset: 0x000073B7
		public void Remove(string[] path, bool recurse, bool force, bool literalPath)
		{
			this.sessionState.RemoveItem(path, recurse, force, literalPath);
		}

		// Token: 0x0600023D RID: 573 RVA: 0x000091CC File Offset: 0x000073CC
		internal void Remove(string path, bool recurse, CmdletProviderContext context)
		{
			this.sessionState.RemoveItem(new string[]
			{
				path
			}, recurse, context);
		}

		// Token: 0x0600023E RID: 574 RVA: 0x000091F2 File Offset: 0x000073F2
		internal object RemoveItemDynamicParameters(string path, bool recurse, CmdletProviderContext context)
		{
			return this.sessionState.RemoveItemDynamicParameters(path, recurse, context);
		}

		// Token: 0x0600023F RID: 575 RVA: 0x00009204 File Offset: 0x00007404
		public Collection<PSObject> Copy(string path, string destinationPath, bool recurse, CopyContainers copyContainers)
		{
			return this.sessionState.CopyItem(new string[]
			{
				path
			}, destinationPath, recurse, copyContainers, false, false);
		}

		// Token: 0x06000240 RID: 576 RVA: 0x0000922E File Offset: 0x0000742E
		public Collection<PSObject> Copy(string[] path, string destinationPath, bool recurse, CopyContainers copyContainers, bool force, bool literalPath)
		{
			return this.sessionState.CopyItem(path, destinationPath, recurse, copyContainers, force, literalPath);
		}

		// Token: 0x06000241 RID: 577 RVA: 0x00009244 File Offset: 0x00007444
		internal void Copy(string path, string destinationPath, bool recurse, CopyContainers copyContainers, CmdletProviderContext context)
		{
			this.sessionState.CopyItem(new string[]
			{
				path
			}, destinationPath, recurse, copyContainers, context);
		}

		// Token: 0x06000242 RID: 578 RVA: 0x0000926E File Offset: 0x0000746E
		internal object CopyItemDynamicParameters(string path, string destination, bool recurse, CmdletProviderContext context)
		{
			return this.sessionState.CopyItemDynamicParameters(path, destination, recurse, context);
		}

		// Token: 0x06000243 RID: 579 RVA: 0x00009280 File Offset: 0x00007480
		public Collection<PSObject> Move(string path, string destination)
		{
			return this.sessionState.MoveItem(new string[]
			{
				path
			}, destination, false, false);
		}

		// Token: 0x06000244 RID: 580 RVA: 0x000092A7 File Offset: 0x000074A7
		public Collection<PSObject> Move(string[] path, string destination, bool force, bool literalPath)
		{
			return this.sessionState.MoveItem(path, destination, force, literalPath);
		}

		// Token: 0x06000245 RID: 581 RVA: 0x000092BC File Offset: 0x000074BC
		internal void Move(string path, string destination, CmdletProviderContext context)
		{
			this.sessionState.MoveItem(new string[]
			{
				path
			}, destination, context);
		}

		// Token: 0x06000246 RID: 582 RVA: 0x000092E2 File Offset: 0x000074E2
		internal object MoveItemDynamicParameters(string path, string destination, CmdletProviderContext context)
		{
			return this.sessionState.MoveItemDynamicParameters(path, destination, context);
		}

		// Token: 0x06000247 RID: 583 RVA: 0x000092F2 File Offset: 0x000074F2
		public bool Exists(string path)
		{
			return this.sessionState.ItemExists(path, false, false);
		}

		// Token: 0x06000248 RID: 584 RVA: 0x00009302 File Offset: 0x00007502
		public bool Exists(string path, bool force, bool literalPath)
		{
			return this.sessionState.ItemExists(path, force, literalPath);
		}

		// Token: 0x06000249 RID: 585 RVA: 0x00009312 File Offset: 0x00007512
		internal bool Exists(string path, CmdletProviderContext context)
		{
			return this.sessionState.ItemExists(path, context);
		}

		// Token: 0x0600024A RID: 586 RVA: 0x00009321 File Offset: 0x00007521
		internal object ItemExistsDynamicParameters(string path, CmdletProviderContext context)
		{
			return this.sessionState.ItemExistsDynamicParameters(path, context);
		}

		// Token: 0x0600024B RID: 587 RVA: 0x00009330 File Offset: 0x00007530
		public bool IsContainer(string path)
		{
			return this.sessionState.IsItemContainer(path);
		}

		// Token: 0x0600024C RID: 588 RVA: 0x0000933E File Offset: 0x0000753E
		internal bool IsContainer(string path, CmdletProviderContext context)
		{
			return this.sessionState.IsItemContainer(path, context);
		}

		// Token: 0x040000CA RID: 202
		private Cmdlet cmdlet;

		// Token: 0x040000CB RID: 203
		private SessionStateInternal sessionState;
	}
}
