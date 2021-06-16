using System;

namespace System.Management.Automation
{
	// Token: 0x02000042 RID: 66
	public sealed class PSVariableIntrinsics
	{
		// Token: 0x06000322 RID: 802 RVA: 0x0000BC2C File Offset: 0x00009E2C
		private PSVariableIntrinsics()
		{
		}

		// Token: 0x06000323 RID: 803 RVA: 0x0000BC34 File Offset: 0x00009E34
		internal PSVariableIntrinsics(SessionStateInternal sessionState)
		{
			if (sessionState == null)
			{
				throw PSTraceSource.NewArgumentException("sessionState");
			}
			this.sessionState = sessionState;
		}

		// Token: 0x06000324 RID: 804 RVA: 0x0000BC51 File Offset: 0x00009E51
		public PSVariable Get(string name)
		{
			if (name != null && name.Equals(string.Empty))
			{
				return null;
			}
			return this.sessionState.GetVariable(name);
		}

		// Token: 0x06000325 RID: 805 RVA: 0x0000BC71 File Offset: 0x00009E71
		internal PSVariable GetAtScope(string name, string scope)
		{
			return this.sessionState.GetVariableAtScope(name, scope);
		}

		// Token: 0x06000326 RID: 806 RVA: 0x0000BC80 File Offset: 0x00009E80
		public object GetValue(string name)
		{
			return this.sessionState.GetVariableValue(name);
		}

		// Token: 0x06000327 RID: 807 RVA: 0x0000BC8E File Offset: 0x00009E8E
		public object GetValue(string name, object defaultValue)
		{
			return this.sessionState.GetVariableValue(name) ?? defaultValue;
		}

		// Token: 0x06000328 RID: 808 RVA: 0x0000BCA1 File Offset: 0x00009EA1
		internal object GetValueAtScope(string name, string scope)
		{
			return this.sessionState.GetVariableValueAtScope(name, scope);
		}

		// Token: 0x06000329 RID: 809 RVA: 0x0000BCB0 File Offset: 0x00009EB0
		public void Set(string name, object value)
		{
			this.sessionState.SetVariableValue(name, value, CommandOrigin.Internal);
		}

		// Token: 0x0600032A RID: 810 RVA: 0x0000BCC0 File Offset: 0x00009EC0
		public void Set(PSVariable variable)
		{
			this.sessionState.SetVariable(variable, false, CommandOrigin.Internal);
		}

		// Token: 0x0600032B RID: 811 RVA: 0x0000BCD1 File Offset: 0x00009ED1
		public void Remove(string name)
		{
			this.sessionState.RemoveVariable(name);
		}

		// Token: 0x0600032C RID: 812 RVA: 0x0000BCDF File Offset: 0x00009EDF
		public void Remove(PSVariable variable)
		{
			this.sessionState.RemoveVariable(variable);
		}

		// Token: 0x0600032D RID: 813 RVA: 0x0000BCED File Offset: 0x00009EED
		internal void RemoveAtScope(string name, string scope)
		{
			this.sessionState.RemoveVariableAtScope(name, scope);
		}

		// Token: 0x0600032E RID: 814 RVA: 0x0000BCFC File Offset: 0x00009EFC
		internal void RemoveAtScope(PSVariable variable, string scope)
		{
			this.sessionState.RemoveVariableAtScope(variable, scope);
		}

		// Token: 0x0400010B RID: 267
		private SessionStateInternal sessionState;
	}
}
