using System;
using System.Globalization;
using System.Text;

namespace System.Management.Automation
{
	// Token: 0x020008AE RID: 2222
	internal class ScopeTracer : IDisposable
	{
		// Token: 0x060054C6 RID: 21702 RVA: 0x001BF48E File Offset: 0x001BD68E
		internal ScopeTracer(PSTraceSource tracer, PSTraceSourceOptions flag, string scopeOutputFormatter, string leavingScopeFormatter, string scopeName)
		{
			this._tracer = tracer;
			this.ScopeTracerHelper(flag, scopeOutputFormatter, leavingScopeFormatter, scopeName, "", new object[0]);
		}

		// Token: 0x060054C7 RID: 21703 RVA: 0x001BF4B4 File Offset: 0x001BD6B4
		internal ScopeTracer(PSTraceSource tracer, PSTraceSourceOptions flag, string scopeOutputFormatter, string leavingScopeFormatter, string scopeName, string format, params object[] args)
		{
			this._tracer = tracer;
			if (format != null)
			{
				this.ScopeTracerHelper(flag, scopeOutputFormatter, leavingScopeFormatter, scopeName, format, args);
				return;
			}
			this.ScopeTracerHelper(flag, scopeOutputFormatter, leavingScopeFormatter, scopeName, "", new object[0]);
		}

		// Token: 0x060054C8 RID: 21704 RVA: 0x001BF4F0 File Offset: 0x001BD6F0
		internal void ScopeTracerHelper(PSTraceSourceOptions flag, string scopeOutputFormatter, string leavingScopeFormatter, string scopeName, string format, params object[] args)
		{
			this._flag = flag;
			this._scopeName = scopeName;
			this._leavingScopeFormatter = leavingScopeFormatter;
			StringBuilder stringBuilder = new StringBuilder();
			if (!string.IsNullOrEmpty(scopeOutputFormatter))
			{
				stringBuilder.AppendFormat(CultureInfo.CurrentCulture, scopeOutputFormatter, new object[]
				{
					this._scopeName
				});
			}
			if (!string.IsNullOrEmpty(format))
			{
				stringBuilder.AppendFormat(CultureInfo.CurrentCulture, format, args);
			}
			this._tracer.OutputLine(this._flag, stringBuilder.ToString(), new object[0]);
			PSTraceSource.ThreadIndentLevel++;
		}

		// Token: 0x060054C9 RID: 21705 RVA: 0x001BF584 File Offset: 0x001BD784
		public void Dispose()
		{
			PSTraceSource.ThreadIndentLevel--;
			if (!string.IsNullOrEmpty(this._leavingScopeFormatter))
			{
				this._tracer.OutputLine(this._flag, this._leavingScopeFormatter, new object[]
				{
					this._scopeName
				});
			}
			GC.SuppressFinalize(this);
		}

		// Token: 0x04002B94 RID: 11156
		private PSTraceSource _tracer;

		// Token: 0x04002B95 RID: 11157
		private PSTraceSourceOptions _flag;

		// Token: 0x04002B96 RID: 11158
		private string _scopeName;

		// Token: 0x04002B97 RID: 11159
		private string _leavingScopeFormatter;
	}
}
