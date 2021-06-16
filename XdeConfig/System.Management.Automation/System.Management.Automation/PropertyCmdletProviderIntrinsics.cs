using System;
using System.Collections.ObjectModel;

namespace System.Management.Automation
{
	// Token: 0x02000036 RID: 54
	public sealed class PropertyCmdletProviderIntrinsics
	{
		// Token: 0x060002AF RID: 687 RVA: 0x0000A0CF File Offset: 0x000082CF
		private PropertyCmdletProviderIntrinsics()
		{
		}

		// Token: 0x060002B0 RID: 688 RVA: 0x0000A0D7 File Offset: 0x000082D7
		internal PropertyCmdletProviderIntrinsics(Cmdlet cmdlet)
		{
			if (cmdlet == null)
			{
				throw PSTraceSource.NewArgumentNullException("cmdlet");
			}
			this.cmdlet = cmdlet;
			this.sessionState = cmdlet.Context.EngineSessionState;
		}

		// Token: 0x060002B1 RID: 689 RVA: 0x0000A105 File Offset: 0x00008305
		internal PropertyCmdletProviderIntrinsics(SessionStateInternal sessionState)
		{
			if (sessionState == null)
			{
				throw PSTraceSource.NewArgumentNullException("sessionState");
			}
			this.sessionState = sessionState;
		}

		// Token: 0x060002B2 RID: 690 RVA: 0x0000A124 File Offset: 0x00008324
		public Collection<PSObject> Get(string path, Collection<string> providerSpecificPickList)
		{
			return this.sessionState.GetProperty(new string[]
			{
				path
			}, providerSpecificPickList, false);
		}

		// Token: 0x060002B3 RID: 691 RVA: 0x0000A14A File Offset: 0x0000834A
		public Collection<PSObject> Get(string[] path, Collection<string> providerSpecificPickList, bool literalPath)
		{
			return this.sessionState.GetProperty(path, providerSpecificPickList, literalPath);
		}

		// Token: 0x060002B4 RID: 692 RVA: 0x0000A15C File Offset: 0x0000835C
		internal void Get(string path, Collection<string> providerSpecificPickList, CmdletProviderContext context)
		{
			this.sessionState.GetProperty(new string[]
			{
				path
			}, providerSpecificPickList, context);
		}

		// Token: 0x060002B5 RID: 693 RVA: 0x0000A182 File Offset: 0x00008382
		internal object GetPropertyDynamicParameters(string path, Collection<string> providerSpecificPickList, CmdletProviderContext context)
		{
			return this.sessionState.GetPropertyDynamicParameters(path, providerSpecificPickList, context);
		}

		// Token: 0x060002B6 RID: 694 RVA: 0x0000A194 File Offset: 0x00008394
		public Collection<PSObject> Set(string path, PSObject propertyValue)
		{
			return this.sessionState.SetProperty(new string[]
			{
				path
			}, propertyValue, false, false);
		}

		// Token: 0x060002B7 RID: 695 RVA: 0x0000A1BB File Offset: 0x000083BB
		public Collection<PSObject> Set(string[] path, PSObject propertyValue, bool force, bool literalPath)
		{
			return this.sessionState.SetProperty(path, propertyValue, force, literalPath);
		}

		// Token: 0x060002B8 RID: 696 RVA: 0x0000A1D0 File Offset: 0x000083D0
		internal void Set(string path, PSObject propertyValue, CmdletProviderContext context)
		{
			this.sessionState.SetProperty(new string[]
			{
				path
			}, propertyValue, context);
		}

		// Token: 0x060002B9 RID: 697 RVA: 0x0000A1F6 File Offset: 0x000083F6
		internal object SetPropertyDynamicParameters(string path, PSObject propertyValue, CmdletProviderContext context)
		{
			return this.sessionState.SetPropertyDynamicParameters(path, propertyValue, context);
		}

		// Token: 0x060002BA RID: 698 RVA: 0x0000A208 File Offset: 0x00008408
		public void Clear(string path, Collection<string> propertyToClear)
		{
			this.sessionState.ClearProperty(new string[]
			{
				path
			}, propertyToClear, false, false);
		}

		// Token: 0x060002BB RID: 699 RVA: 0x0000A22F File Offset: 0x0000842F
		public void Clear(string[] path, Collection<string> propertyToClear, bool force, bool literalPath)
		{
			this.sessionState.ClearProperty(path, propertyToClear, force, literalPath);
		}

		// Token: 0x060002BC RID: 700 RVA: 0x0000A244 File Offset: 0x00008444
		internal void Clear(string path, Collection<string> propertyToClear, CmdletProviderContext context)
		{
			this.sessionState.ClearProperty(new string[]
			{
				path
			}, propertyToClear, context);
		}

		// Token: 0x060002BD RID: 701 RVA: 0x0000A26A File Offset: 0x0000846A
		internal object ClearPropertyDynamicParameters(string path, Collection<string> propertyToClear, CmdletProviderContext context)
		{
			return this.sessionState.ClearPropertyDynamicParameters(path, propertyToClear, context);
		}

		// Token: 0x060002BE RID: 702 RVA: 0x0000A27C File Offset: 0x0000847C
		public Collection<PSObject> New(string path, string propertyName, string propertyTypeName, object value)
		{
			return this.sessionState.NewProperty(new string[]
			{
				path
			}, propertyName, propertyTypeName, value, false, false);
		}

		// Token: 0x060002BF RID: 703 RVA: 0x0000A2A6 File Offset: 0x000084A6
		public Collection<PSObject> New(string[] path, string propertyName, string propertyTypeName, object value, bool force, bool literalPath)
		{
			return this.sessionState.NewProperty(path, propertyName, propertyTypeName, value, force, literalPath);
		}

		// Token: 0x060002C0 RID: 704 RVA: 0x0000A2BC File Offset: 0x000084BC
		internal void New(string path, string propertyName, string type, object value, CmdletProviderContext context)
		{
			this.sessionState.NewProperty(new string[]
			{
				path
			}, propertyName, type, value, context);
		}

		// Token: 0x060002C1 RID: 705 RVA: 0x0000A2E6 File Offset: 0x000084E6
		internal object NewPropertyDynamicParameters(string path, string propertyName, string type, object value, CmdletProviderContext context)
		{
			return this.sessionState.NewPropertyDynamicParameters(path, propertyName, type, value, context);
		}

		// Token: 0x060002C2 RID: 706 RVA: 0x0000A2FC File Offset: 0x000084FC
		public void Remove(string path, string propertyName)
		{
			this.sessionState.RemoveProperty(new string[]
			{
				path
			}, propertyName, false, false);
		}

		// Token: 0x060002C3 RID: 707 RVA: 0x0000A323 File Offset: 0x00008523
		public void Remove(string[] path, string propertyName, bool force, bool literalPath)
		{
			this.sessionState.RemoveProperty(path, propertyName, force, literalPath);
		}

		// Token: 0x060002C4 RID: 708 RVA: 0x0000A338 File Offset: 0x00008538
		internal void Remove(string path, string propertyName, CmdletProviderContext context)
		{
			this.sessionState.RemoveProperty(new string[]
			{
				path
			}, propertyName, context);
		}

		// Token: 0x060002C5 RID: 709 RVA: 0x0000A35E File Offset: 0x0000855E
		internal object RemovePropertyDynamicParameters(string path, string propertyName, CmdletProviderContext context)
		{
			return this.sessionState.RemovePropertyDynamicParameters(path, propertyName, context);
		}

		// Token: 0x060002C6 RID: 710 RVA: 0x0000A370 File Offset: 0x00008570
		public Collection<PSObject> Rename(string path, string sourceProperty, string destinationProperty)
		{
			return this.sessionState.RenameProperty(new string[]
			{
				path
			}, sourceProperty, destinationProperty, false, false);
		}

		// Token: 0x060002C7 RID: 711 RVA: 0x0000A398 File Offset: 0x00008598
		public Collection<PSObject> Rename(string[] path, string sourceProperty, string destinationProperty, bool force, bool literalPath)
		{
			return this.sessionState.RenameProperty(path, sourceProperty, destinationProperty, force, literalPath);
		}

		// Token: 0x060002C8 RID: 712 RVA: 0x0000A3AC File Offset: 0x000085AC
		internal void Rename(string path, string sourceProperty, string destinationProperty, CmdletProviderContext context)
		{
			this.sessionState.RenameProperty(new string[]
			{
				path
			}, sourceProperty, destinationProperty, context);
		}

		// Token: 0x060002C9 RID: 713 RVA: 0x0000A3D4 File Offset: 0x000085D4
		internal object RenamePropertyDynamicParameters(string path, string sourceProperty, string destinationProperty, CmdletProviderContext context)
		{
			return this.sessionState.RenamePropertyDynamicParameters(path, sourceProperty, destinationProperty, context);
		}

		// Token: 0x060002CA RID: 714 RVA: 0x0000A3E8 File Offset: 0x000085E8
		public Collection<PSObject> Copy(string sourcePath, string sourceProperty, string destinationPath, string destinationProperty)
		{
			return this.sessionState.CopyProperty(new string[]
			{
				sourcePath
			}, sourceProperty, destinationPath, destinationProperty, false, false);
		}

		// Token: 0x060002CB RID: 715 RVA: 0x0000A412 File Offset: 0x00008612
		public Collection<PSObject> Copy(string[] sourcePath, string sourceProperty, string destinationPath, string destinationProperty, bool force, bool literalPath)
		{
			return this.sessionState.CopyProperty(sourcePath, sourceProperty, destinationPath, destinationProperty, force, literalPath);
		}

		// Token: 0x060002CC RID: 716 RVA: 0x0000A428 File Offset: 0x00008628
		internal void Copy(string sourcePath, string sourceProperty, string destinationPath, string destinationProperty, CmdletProviderContext context)
		{
			this.sessionState.CopyProperty(new string[]
			{
				sourcePath
			}, sourceProperty, destinationPath, destinationProperty, context);
		}

		// Token: 0x060002CD RID: 717 RVA: 0x0000A452 File Offset: 0x00008652
		internal object CopyPropertyDynamicParameters(string path, string sourceProperty, string destinationPath, string destinationProperty, CmdletProviderContext context)
		{
			return this.sessionState.CopyPropertyDynamicParameters(path, sourceProperty, destinationPath, destinationProperty, context);
		}

		// Token: 0x060002CE RID: 718 RVA: 0x0000A468 File Offset: 0x00008668
		public Collection<PSObject> Move(string sourcePath, string sourceProperty, string destinationPath, string destinationProperty)
		{
			return this.sessionState.MoveProperty(new string[]
			{
				sourcePath
			}, sourceProperty, destinationPath, destinationProperty, false, false);
		}

		// Token: 0x060002CF RID: 719 RVA: 0x0000A492 File Offset: 0x00008692
		public Collection<PSObject> Move(string[] sourcePath, string sourceProperty, string destinationPath, string destinationProperty, bool force, bool literalPath)
		{
			return this.sessionState.MoveProperty(sourcePath, sourceProperty, destinationPath, destinationProperty, force, literalPath);
		}

		// Token: 0x060002D0 RID: 720 RVA: 0x0000A4A8 File Offset: 0x000086A8
		internal void Move(string sourcePath, string sourceProperty, string destinationPath, string destinationProperty, CmdletProviderContext context)
		{
			this.sessionState.MoveProperty(new string[]
			{
				sourcePath
			}, sourceProperty, destinationPath, destinationProperty, context);
		}

		// Token: 0x060002D1 RID: 721 RVA: 0x0000A4D2 File Offset: 0x000086D2
		internal object MovePropertyDynamicParameters(string path, string sourceProperty, string destinationPath, string destinationProperty, CmdletProviderContext context)
		{
			return this.sessionState.MovePropertyDynamicParameters(path, sourceProperty, destinationPath, destinationProperty, context);
		}

		// Token: 0x040000EA RID: 234
		private Cmdlet cmdlet;

		// Token: 0x040000EB RID: 235
		private SessionStateInternal sessionState;
	}
}
