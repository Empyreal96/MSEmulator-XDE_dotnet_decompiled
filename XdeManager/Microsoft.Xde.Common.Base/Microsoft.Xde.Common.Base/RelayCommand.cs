using System;
using System.Diagnostics;
using System.Windows.Input;

namespace Microsoft.Xde.Common
{
	// Token: 0x0200000E RID: 14
	public class RelayCommand : ICommand
	{
		// Token: 0x0600009D RID: 157 RVA: 0x00003223 File Offset: 0x00001423
		public RelayCommand(Action<object> execute) : this(execute, null)
		{
		}

		// Token: 0x0600009E RID: 158 RVA: 0x0000322D File Offset: 0x0000142D
		public RelayCommand(Action<object> execute, Predicate<object> canExecute)
		{
			if (execute == null)
			{
				throw new ArgumentNullException("execute");
			}
			this.execute = execute;
			this.canExecute = canExecute;
		}

		// Token: 0x14000002 RID: 2
		// (add) Token: 0x0600009F RID: 159 RVA: 0x00003251 File Offset: 0x00001451
		// (remove) Token: 0x060000A0 RID: 160 RVA: 0x00003259 File Offset: 0x00001459
		public event EventHandler CanExecuteChanged
		{
			add
			{
				CommandManager.RequerySuggested += value;
			}
			remove
			{
				CommandManager.RequerySuggested -= value;
			}
		}

		// Token: 0x060000A1 RID: 161 RVA: 0x00003261 File Offset: 0x00001461
		[DebuggerStepThrough]
		public bool CanExecute(object parameter)
		{
			return this.canExecute == null || this.canExecute(parameter);
		}

		// Token: 0x060000A2 RID: 162 RVA: 0x00003279 File Offset: 0x00001479
		public void Execute(object parameter)
		{
			this.execute(parameter);
		}

		// Token: 0x060000A3 RID: 163 RVA: 0x00003287 File Offset: 0x00001487
		public void UpdateCanExecute()
		{
			CommandManager.InvalidateRequerySuggested();
		}

		// Token: 0x0400006A RID: 106
		private readonly Action<object> execute;

		// Token: 0x0400006B RID: 107
		private readonly Predicate<object> canExecute;
	}
}
