using System;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.Configuration;
using System.Diagnostics;
using System.Globalization;
using System.IO;
using System.Management.Automation.Internal;
using System.Reflection;
using System.Text;
using System.Threading;
using System.Xml;

namespace System.Management.Automation
{
	// Token: 0x02000889 RID: 2185
	public class PSTraceSource
	{
		// Token: 0x0600537A RID: 21370 RVA: 0x001BA899 File Offset: 0x001B8A99
		internal static PSTraceSource GetTracer(string name, string description)
		{
			return PSTraceSource.GetTracer(name, description, true);
		}

		// Token: 0x0600537B RID: 21371 RVA: 0x001BA8A4 File Offset: 0x001B8AA4
		internal static PSTraceSource GetTracer(string name, string description, bool traceHeaders)
		{
			if (string.IsNullOrEmpty(name))
			{
				throw new ArgumentNullException("name");
			}
			PSTraceSource result;
			lock (PSTraceSource.getTracerLock)
			{
				PSTraceSource pstraceSource = null;
				if (PSTraceSource.TraceCatalog.ContainsKey(name))
				{
					pstraceSource = PSTraceSource.TraceCatalog[name];
				}
				if (pstraceSource == null)
				{
					string text = name;
					if (!PSTraceSource.PreConfiguredTraceSource.ContainsKey(text))
					{
						if (text.Length > 16)
						{
							text = text.Substring(0, 16);
							if (!PSTraceSource.PreConfiguredTraceSource.ContainsKey(text))
							{
								text = null;
							}
						}
						else
						{
							text = null;
						}
					}
					if (text != null)
					{
						PSTraceSource pstraceSource2 = PSTraceSource.PreConfiguredTraceSource[text];
						pstraceSource = PSTraceSource.GetNewTraceSource(text, description, traceHeaders);
						pstraceSource.Options = pstraceSource2.Options;
						pstraceSource.Listeners.Clear();
						pstraceSource.Listeners.AddRange(pstraceSource2.Listeners);
						PSTraceSource.TraceCatalog.Add(text, pstraceSource);
						PSTraceSource.PreConfiguredTraceSource.Remove(text);
					}
				}
				if (pstraceSource == null)
				{
					pstraceSource = PSTraceSource.GetNewTraceSource(name, description, traceHeaders);
					PSTraceSource.TraceCatalog[pstraceSource.FullName] = pstraceSource;
				}
				if (pstraceSource.Options != PSTraceSourceOptions.None && traceHeaders)
				{
					pstraceSource.TraceGlobalAppDomainHeader();
					pstraceSource.TracerObjectHeader(Assembly.GetCallingAssembly());
				}
				result = pstraceSource;
			}
			return result;
		}

		// Token: 0x0600537C RID: 21372 RVA: 0x001BA9E0 File Offset: 0x001B8BE0
		internal static PSTraceSource GetNewTraceSource(string name, string description, bool traceHeaders)
		{
			if (string.IsNullOrEmpty(name))
			{
				throw new ArgumentException("name");
			}
			return new PSTraceSource(name, name, description, traceHeaders);
		}

		// Token: 0x0600537D RID: 21373 RVA: 0x001BAA10 File Offset: 0x001B8C10
		internal static PSArgumentNullException NewArgumentNullException(string paramName)
		{
			if (string.IsNullOrEmpty(paramName))
			{
				throw new ArgumentNullException("paramName");
			}
			string message = StringUtil.Format(AutomationExceptions.ArgumentNull, paramName);
			return new PSArgumentNullException(paramName, message);
		}

		// Token: 0x0600537E RID: 21374 RVA: 0x001BAA48 File Offset: 0x001B8C48
		internal static PSArgumentNullException NewArgumentNullException(string paramName, string resourceString, params object[] args)
		{
			if (string.IsNullOrEmpty(paramName))
			{
				throw PSTraceSource.NewArgumentNullException("paramName");
			}
			if (string.IsNullOrEmpty(resourceString))
			{
				throw PSTraceSource.NewArgumentNullException("resourceString");
			}
			string message = StringUtil.Format(resourceString, args);
			return new PSArgumentNullException(paramName, message);
		}

		// Token: 0x0600537F RID: 21375 RVA: 0x001BAA8C File Offset: 0x001B8C8C
		internal static PSArgumentException NewArgumentException(string paramName)
		{
			if (string.IsNullOrEmpty(paramName))
			{
				throw new ArgumentNullException("paramName");
			}
			string message = StringUtil.Format(AutomationExceptions.Argument, paramName);
			return new PSArgumentException(message, paramName);
		}

		// Token: 0x06005380 RID: 21376 RVA: 0x001BAAC4 File Offset: 0x001B8CC4
		internal static PSArgumentException NewArgumentException(string paramName, string resourceString, params object[] args)
		{
			if (string.IsNullOrEmpty(paramName))
			{
				throw PSTraceSource.NewArgumentNullException("paramName");
			}
			if (string.IsNullOrEmpty(resourceString))
			{
				throw PSTraceSource.NewArgumentNullException("resourceString");
			}
			string message = StringUtil.Format(resourceString, args);
			return new PSArgumentException(message, paramName);
		}

		// Token: 0x06005381 RID: 21377 RVA: 0x001BAB08 File Offset: 0x001B8D08
		internal static PSInvalidOperationException NewInvalidOperationException()
		{
			string message = StringUtil.Format(AutomationExceptions.InvalidOperation, new StackTrace().GetFrame(1).GetMethod().Name);
			return new PSInvalidOperationException(message);
		}

		// Token: 0x06005382 RID: 21378 RVA: 0x001BAB40 File Offset: 0x001B8D40
		internal static PSInvalidOperationException NewInvalidOperationException(string resourceString, params object[] args)
		{
			if (string.IsNullOrEmpty(resourceString))
			{
				throw PSTraceSource.NewArgumentNullException("resourceString");
			}
			string message = StringUtil.Format(resourceString, args);
			return new PSInvalidOperationException(message);
		}

		// Token: 0x06005383 RID: 21379 RVA: 0x001BAB70 File Offset: 0x001B8D70
		internal static PSInvalidOperationException NewInvalidOperationException(Exception innerException, string resourceString, params object[] args)
		{
			if (string.IsNullOrEmpty(resourceString))
			{
				throw PSTraceSource.NewArgumentNullException("resourceString");
			}
			string message = StringUtil.Format(resourceString, args);
			return new PSInvalidOperationException(message, innerException);
		}

		// Token: 0x06005384 RID: 21380 RVA: 0x001BABA4 File Offset: 0x001B8DA4
		internal static PSNotSupportedException NewNotSupportedException()
		{
			string message = StringUtil.Format(AutomationExceptions.NotSupported, new StackTrace().GetFrame(0).ToString());
			return new PSNotSupportedException(message);
		}

		// Token: 0x06005385 RID: 21381 RVA: 0x001BABD4 File Offset: 0x001B8DD4
		internal static PSNotSupportedException NewNotSupportedException(string resourceString, params object[] args)
		{
			if (string.IsNullOrEmpty(resourceString))
			{
				throw PSTraceSource.NewArgumentNullException("resourceString");
			}
			string message = StringUtil.Format(resourceString, args);
			return new PSNotSupportedException(message);
		}

		// Token: 0x06005386 RID: 21382 RVA: 0x001BAC04 File Offset: 0x001B8E04
		internal static PSNotImplementedException NewNotImplementedException()
		{
			string message = StringUtil.Format(AutomationExceptions.NotImplemented, new StackTrace().GetFrame(0).ToString());
			return new PSNotImplementedException(message);
		}

		// Token: 0x06005387 RID: 21383 RVA: 0x001BAC34 File Offset: 0x001B8E34
		internal static PSArgumentOutOfRangeException NewArgumentOutOfRangeException(string paramName, object actualValue)
		{
			if (string.IsNullOrEmpty(paramName))
			{
				throw new ArgumentNullException("paramName");
			}
			string message = StringUtil.Format(AutomationExceptions.ArgumentOutOfRange, paramName);
			return new PSArgumentOutOfRangeException(paramName, actualValue, message);
		}

		// Token: 0x06005388 RID: 21384 RVA: 0x001BAC6C File Offset: 0x001B8E6C
		internal static PSArgumentOutOfRangeException NewArgumentOutOfRangeException(string paramName, object actualValue, string resourceString, params object[] args)
		{
			if (string.IsNullOrEmpty(paramName))
			{
				throw PSTraceSource.NewArgumentNullException("paramName");
			}
			if (string.IsNullOrEmpty(resourceString))
			{
				throw PSTraceSource.NewArgumentNullException("resourceString");
			}
			string message = StringUtil.Format(resourceString, args);
			return new PSArgumentOutOfRangeException(paramName, actualValue, message);
		}

		// Token: 0x06005389 RID: 21385 RVA: 0x001BACB4 File Offset: 0x001B8EB4
		internal static PSObjectDisposedException NewObjectDisposedException(string objectName)
		{
			if (string.IsNullOrEmpty(objectName))
			{
				throw PSTraceSource.NewArgumentNullException("objectName");
			}
			string message = StringUtil.Format(AutomationExceptions.ObjectDisposed, objectName);
			return new PSObjectDisposedException(objectName, message);
		}

		// Token: 0x0600538A RID: 21386 RVA: 0x001BACEC File Offset: 0x001B8EEC
		internal PSTraceSource(string fullName, string name, string description, bool traceHeaders)
		{
			if (string.IsNullOrEmpty(fullName))
			{
				throw new ArgumentNullException("fullName");
			}
			try
			{
				this.fullName = fullName;
				this.name = name;
				string environmentVariable = Environment.GetEnvironmentVariable("MshEnableTrace");
				if (string.Equals(environmentVariable, "True", StringComparison.OrdinalIgnoreCase))
				{
					string text = this.TraceSource.Attributes["Options"];
					if (text != null)
					{
						this.flags = (PSTraceSourceOptions)Enum.Parse(typeof(PSTraceSourceOptions), text, true);
					}
				}
				this.showHeaders = traceHeaders;
				this.description = description;
			}
			catch (XmlException)
			{
				this.flags = PSTraceSourceOptions.None;
			}
			catch (ConfigurationException)
			{
				this.flags = PSTraceSourceOptions.None;
			}
		}

		// Token: 0x0600538B RID: 21387 RVA: 0x001BADCC File Offset: 0x001B8FCC
		internal void TraceGlobalAppDomainHeader()
		{
			if (PSTraceSource.globalTraceInitialized)
			{
				return;
			}
			this.OutputLine(PSTraceSourceOptions.All, "Initializing tracing for AppDomain: {0}", new object[]
			{
				AppDomain.CurrentDomain.FriendlyName
			});
			this.OutputLine(PSTraceSourceOptions.All, "\tCurrent time: {0}", new object[]
			{
				DateTime.Now
			});
			this.OutputLine(PSTraceSourceOptions.All, "\tOS Build: {0}", new object[]
			{
				Environment.OSVersion.ToString()
			});
			this.OutputLine(PSTraceSourceOptions.All, "\tFramework Build: {0}\n", new object[]
			{
				Environment.Version.ToString()
			});
			PSTraceSource.globalTraceInitialized = true;
		}

		// Token: 0x0600538C RID: 21388 RVA: 0x001BAE7C File Offset: 0x001B907C
		internal void TracerObjectHeader(Assembly callingAssembly)
		{
			if (this.flags == PSTraceSourceOptions.None)
			{
				return;
			}
			this.OutputLine(PSTraceSourceOptions.All, "Creating tracer:", new object[0]);
			this.OutputLine(PSTraceSourceOptions.All, "\tCategory: {0}", new object[]
			{
				this.Name
			});
			this.OutputLine(PSTraceSourceOptions.All, "\tDescription: {0}", new object[]
			{
				this.Description
			});
			if (callingAssembly != null)
			{
				this.OutputLine(PSTraceSourceOptions.All, "\tAssembly: {0}", new object[]
				{
					callingAssembly.FullName
				});
				this.OutputLine(PSTraceSourceOptions.All, "\tAssembly Location: {0}", new object[]
				{
					ClrFacade.GetAssemblyLocation(callingAssembly)
				});
				FileInfo fileInfo = new FileInfo(ClrFacade.GetAssemblyLocation(callingAssembly));
				this.OutputLine(PSTraceSourceOptions.All, "\tAssembly File Timestamp: {0}", new object[]
				{
					fileInfo.CreationTime
				});
			}
			StringBuilder stringBuilder = new StringBuilder();
			stringBuilder.Append("\tFlags: ");
			stringBuilder.Append(this.flags.ToString());
			this.OutputLine(PSTraceSourceOptions.All, stringBuilder.ToString(), new object[0]);
		}

		// Token: 0x0600538D RID: 21389 RVA: 0x001BAFB0 File Offset: 0x001B91B0
		internal IDisposable TraceScope(string format, params object[] args)
		{
			if ((this.flags & PSTraceSourceOptions.Scope) != PSTraceSourceOptions.None)
			{
				try
				{
					return new ScopeTracer(this, PSTraceSourceOptions.Scope, null, null, string.Empty, format, args);
				}
				catch
				{
				}
			}
			return null;
		}

		// Token: 0x0600538E RID: 21390 RVA: 0x001BAFF8 File Offset: 0x001B91F8
		internal IDisposable TraceMethod(string format, params object[] args)
		{
			if ((this.flags & PSTraceSourceOptions.Method) != PSTraceSourceOptions.None)
			{
				try
				{
					string callingMethodNameAndParameters = PSTraceSource.GetCallingMethodNameAndParameters(1);
					return new ScopeTracer(this, PSTraceSourceOptions.Method, "Enter {0}:", "Leave {0}", callingMethodNameAndParameters, format, args);
				}
				catch
				{
				}
			}
			return null;
		}

		// Token: 0x0600538F RID: 21391 RVA: 0x001BB044 File Offset: 0x001B9244
		internal IDisposable TraceEventHandlers()
		{
			if ((this.flags & PSTraceSourceOptions.Events) != PSTraceSourceOptions.None)
			{
				try
				{
					string callingMethodNameAndParameters = PSTraceSource.GetCallingMethodNameAndParameters(1);
					return new ScopeTracer(this, PSTraceSourceOptions.Events, "Enter event handler: {0}:", "Leave event handler: {0}", callingMethodNameAndParameters, "", new object[0]);
				}
				catch
				{
				}
			}
			return null;
		}

		// Token: 0x06005390 RID: 21392 RVA: 0x001BB09C File Offset: 0x001B929C
		internal IDisposable TraceEventHandlers(string format, params object[] args)
		{
			if ((this.flags & PSTraceSourceOptions.Events) != PSTraceSourceOptions.None)
			{
				try
				{
					string callingMethodNameAndParameters = PSTraceSource.GetCallingMethodNameAndParameters(1);
					return new ScopeTracer(this, PSTraceSourceOptions.Events, "Enter event handler: {0}:", "Leave event handler: {0}", callingMethodNameAndParameters, format, args);
				}
				catch
				{
				}
			}
			return null;
		}

		// Token: 0x06005391 RID: 21393 RVA: 0x001BB0EC File Offset: 0x001B92EC
		internal IDisposable TraceLock(string lockName)
		{
			if ((this.flags & PSTraceSourceOptions.Lock) != PSTraceSourceOptions.None)
			{
				try
				{
					return new ScopeTracer(this, PSTraceSourceOptions.Lock, "Enter Lock: {0}", "Leave Lock: {0}", lockName);
				}
				catch
				{
				}
			}
			return null;
		}

		// Token: 0x06005392 RID: 21394 RVA: 0x001BB138 File Offset: 0x001B9338
		internal void TraceLockAcquiring(string lockName)
		{
			if ((this.flags & PSTraceSourceOptions.Lock) != PSTraceSourceOptions.None)
			{
				this.TraceLockHelper("Acquiring Lock: {0}", lockName);
			}
		}

		// Token: 0x06005393 RID: 21395 RVA: 0x001BB154 File Offset: 0x001B9354
		internal void TraceLockAcquired(string lockName)
		{
			if ((this.flags & PSTraceSourceOptions.Lock) != PSTraceSourceOptions.None)
			{
				this.TraceLockHelper("Enter Lock: {0}", lockName);
			}
		}

		// Token: 0x06005394 RID: 21396 RVA: 0x001BB170 File Offset: 0x001B9370
		internal void TraceLockReleased(string lockName)
		{
			if ((this.flags & PSTraceSourceOptions.Lock) != PSTraceSourceOptions.None)
			{
				this.TraceLockHelper("Leave Lock: {0}", lockName);
			}
		}

		// Token: 0x06005395 RID: 21397 RVA: 0x001BB18C File Offset: 0x001B938C
		private void TraceLockHelper(string formatter, string lockName)
		{
			try
			{
				this.OutputLine(PSTraceSourceOptions.Lock, formatter, new object[]
				{
					lockName
				});
			}
			catch
			{
			}
		}

		// Token: 0x06005396 RID: 21398 RVA: 0x001BB1C8 File Offset: 0x001B93C8
		internal void TraceError(string errorMessageFormat, params object[] args)
		{
			if ((this.flags & PSTraceSourceOptions.Error) != PSTraceSourceOptions.None)
			{
				this.FormatOutputLine(PSTraceSourceOptions.Error, "ERROR: ", errorMessageFormat, args);
			}
		}

		// Token: 0x06005397 RID: 21399 RVA: 0x001BB1EA File Offset: 0x001B93EA
		internal void TraceWarning(string warningMessageFormat, params object[] args)
		{
			if ((this.flags & PSTraceSourceOptions.Warning) != PSTraceSourceOptions.None)
			{
				this.FormatOutputLine(PSTraceSourceOptions.Warning, "Warning: ", warningMessageFormat, args);
			}
		}

		// Token: 0x06005398 RID: 21400 RVA: 0x001BB20C File Offset: 0x001B940C
		internal void TraceVerbose(string verboseMessageFormat, params object[] args)
		{
			if ((this.flags & PSTraceSourceOptions.Verbose) != PSTraceSourceOptions.None)
			{
				this.FormatOutputLine(PSTraceSourceOptions.Verbose, "Verbose: ", verboseMessageFormat, args);
			}
		}

		// Token: 0x06005399 RID: 21401 RVA: 0x001BB22E File Offset: 0x001B942E
		internal void WriteLine(string format, params object[] args)
		{
			if ((this.flags & PSTraceSourceOptions.WriteLine) != PSTraceSourceOptions.None)
			{
				this.FormatOutputLine(PSTraceSourceOptions.WriteLine, "", format, args);
			}
		}

		// Token: 0x0600539A RID: 21402 RVA: 0x001BB250 File Offset: 0x001B9450
		internal void WriteLine(object arg)
		{
			if ((this.flags & PSTraceSourceOptions.WriteLine) != PSTraceSourceOptions.None)
			{
				this.WriteLine("{0}", new object[]
				{
					(arg == null) ? "null" : arg.ToString()
				});
			}
		}

		// Token: 0x0600539B RID: 21403 RVA: 0x001BB294 File Offset: 0x001B9494
		private void FormatOutputLine(PSTraceSourceOptions flag, string classFormatter, string format, params object[] args)
		{
			try
			{
				StringBuilder stringBuilder = new StringBuilder();
				if (classFormatter != null)
				{
					stringBuilder.Append(classFormatter);
				}
				if (format != null)
				{
					stringBuilder.AppendFormat(CultureInfo.CurrentCulture, format, args);
				}
				this.OutputLine(flag, stringBuilder.ToString(), new object[0]);
			}
			catch
			{
			}
		}

		// Token: 0x0600539C RID: 21404 RVA: 0x001BB2EC File Offset: 0x001B94EC
		private static string GetCallingMethodNameAndParameters(int skipFrames)
		{
			StringBuilder stringBuilder = null;
			try
			{
				StackFrame stackFrame = new StackFrame(++skipFrames);
				MethodBase method = stackFrame.GetMethod();
				Type declaringType = method.DeclaringType;
				stringBuilder = new StringBuilder();
				stringBuilder.AppendFormat(CultureInfo.CurrentCulture, "{0}.{1}(", new object[]
				{
					declaringType.Name,
					method.Name
				});
				stringBuilder.Append(")");
			}
			catch
			{
			}
			return stringBuilder.ToString();
		}

		// Token: 0x0600539D RID: 21405 RVA: 0x001BB374 File Offset: 0x001B9574
		private static StringBuilder GetLinePrefix(PSTraceSourceOptions flag)
		{
			StringBuilder stringBuilder = new StringBuilder();
			stringBuilder.AppendFormat(CultureInfo.CurrentCulture, " {0,-11} ", new object[]
			{
				Enum.GetName(typeof(PSTraceSourceOptions), flag)
			});
			return stringBuilder;
		}

		// Token: 0x0600539E RID: 21406 RVA: 0x001BB3BC File Offset: 0x001B95BC
		private static void AddTab(ref StringBuilder lineBuilder)
		{
			int indentSize = Trace.IndentSize;
			int threadIndentLevel = PSTraceSource.ThreadIndentLevel;
			for (int i = 0; i < indentSize * threadIndentLevel; i++)
			{
				lineBuilder.Append(" ");
			}
		}

		// Token: 0x0600539F RID: 21407 RVA: 0x001BB3F0 File Offset: 0x001B95F0
		internal void OutputLine(PSTraceSourceOptions flag, string format, params object[] args)
		{
			if (this.alreadyTracing)
			{
				return;
			}
			this.alreadyTracing = true;
			try
			{
				StringBuilder stringBuilder = new StringBuilder();
				if (this.showHeaders)
				{
					stringBuilder.Append(PSTraceSource.GetLinePrefix(flag));
				}
				PSTraceSource.AddTab(ref stringBuilder);
				if (args != null && args.Length > 0)
				{
					for (int i = 0; i < args.Length; i++)
					{
						if (args[i] == null)
						{
							args[i] = "null";
						}
					}
					stringBuilder.AppendFormat(CultureInfo.CurrentCulture, format, args);
				}
				else
				{
					stringBuilder.Append(format);
				}
				this.TraceSource.TraceInformation(stringBuilder.ToString());
			}
			finally
			{
				this.alreadyTracing = false;
			}
		}

		// Token: 0x17001132 RID: 4402
		// (get) Token: 0x060053A0 RID: 21408 RVA: 0x001BB498 File Offset: 0x001B9698
		// (set) Token: 0x060053A1 RID: 21409 RVA: 0x001BB4A4 File Offset: 0x001B96A4
		internal static int ThreadIndentLevel
		{
			get
			{
				return PSTraceSource.LocalIndentLevel.Value;
			}
			set
			{
				if (value >= 0)
				{
					PSTraceSource.LocalIndentLevel.Value = value;
				}
			}
		}

		// Token: 0x17001133 RID: 4403
		// (get) Token: 0x060053A2 RID: 21410 RVA: 0x001BB4B5 File Offset: 0x001B96B5
		// (set) Token: 0x060053A3 RID: 21411 RVA: 0x001BB4BD File Offset: 0x001B96BD
		public string Description
		{
			get
			{
				return this.description;
			}
			set
			{
				this.description = value;
			}
		}

		// Token: 0x17001134 RID: 4404
		// (get) Token: 0x060053A4 RID: 21412 RVA: 0x001BB4C6 File Offset: 0x001B96C6
		// (set) Token: 0x060053A5 RID: 21413 RVA: 0x001BB4CE File Offset: 0x001B96CE
		internal bool ShowHeaders
		{
			get
			{
				return this.showHeaders;
			}
			set
			{
				this.showHeaders = value;
			}
		}

		// Token: 0x17001135 RID: 4405
		// (get) Token: 0x060053A6 RID: 21414 RVA: 0x001BB4D7 File Offset: 0x001B96D7
		internal string FullName
		{
			get
			{
				return this.fullName;
			}
		}

		// Token: 0x17001136 RID: 4406
		// (get) Token: 0x060053A7 RID: 21415 RVA: 0x001BB4DF File Offset: 0x001B96DF
		internal TraceSource TraceSource
		{
			get
			{
				if (this.traceSource == null)
				{
					this.traceSource = new MonadTraceSource(this.name);
				}
				return this.traceSource;
			}
		}

		// Token: 0x17001137 RID: 4407
		// (get) Token: 0x060053A8 RID: 21416 RVA: 0x001BB500 File Offset: 0x001B9700
		// (set) Token: 0x060053A9 RID: 21417 RVA: 0x001BB508 File Offset: 0x001B9708
		public PSTraceSourceOptions Options
		{
			get
			{
				return this.flags;
			}
			set
			{
				this.flags = value;
				this.TraceSource.Switch.Level = (SourceLevels)this.flags;
			}
		}

		// Token: 0x17001138 RID: 4408
		// (get) Token: 0x060053AA RID: 21418 RVA: 0x001BB527 File Offset: 0x001B9727
		public StringDictionary Attributes
		{
			get
			{
				return this.TraceSource.Attributes;
			}
		}

		// Token: 0x17001139 RID: 4409
		// (get) Token: 0x060053AB RID: 21419 RVA: 0x001BB534 File Offset: 0x001B9734
		public TraceListenerCollection Listeners
		{
			get
			{
				return this.TraceSource.Listeners;
			}
		}

		// Token: 0x1700113A RID: 4410
		// (get) Token: 0x060053AC RID: 21420 RVA: 0x001BB541 File Offset: 0x001B9741
		public string Name
		{
			get
			{
				return this.name;
			}
		}

		// Token: 0x1700113B RID: 4411
		// (get) Token: 0x060053AD RID: 21421 RVA: 0x001BB549 File Offset: 0x001B9749
		// (set) Token: 0x060053AE RID: 21422 RVA: 0x001BB556 File Offset: 0x001B9756
		public SourceSwitch Switch
		{
			get
			{
				return this.TraceSource.Switch;
			}
			set
			{
				this.TraceSource.Switch = value;
			}
		}

		// Token: 0x1700113C RID: 4412
		// (get) Token: 0x060053AF RID: 21423 RVA: 0x001BB564 File Offset: 0x001B9764
		internal static Dictionary<string, PSTraceSource> TraceCatalog
		{
			get
			{
				return PSTraceSource.traceCatalog;
			}
		}

		// Token: 0x1700113D RID: 4413
		// (get) Token: 0x060053B0 RID: 21424 RVA: 0x001BB56B File Offset: 0x001B976B
		internal static Dictionary<string, PSTraceSource> PreConfiguredTraceSource
		{
			get
			{
				return PSTraceSource.preConfiguredTraceSource;
			}
		}

		// Token: 0x04002AE4 RID: 10980
		private const string errorFormatter = "ERROR: ";

		// Token: 0x04002AE5 RID: 10981
		private const string warningFormatter = "Warning: ";

		// Token: 0x04002AE6 RID: 10982
		private const string verboseFormatter = "Verbose: ";

		// Token: 0x04002AE7 RID: 10983
		private const string writeLineFormatter = "";

		// Token: 0x04002AE8 RID: 10984
		private const string constructorOutputFormatter = "Enter Ctor {0}";

		// Token: 0x04002AE9 RID: 10985
		private const string constructorLeavingFormatter = "Leave Ctor {0}";

		// Token: 0x04002AEA RID: 10986
		private const string disposeOutputFormatter = "Enter Disposer {0}";

		// Token: 0x04002AEB RID: 10987
		private const string disposeLeavingFormatter = "Leave Disposer {0}";

		// Token: 0x04002AEC RID: 10988
		private const string methodOutputFormatter = "Enter {0}:";

		// Token: 0x04002AED RID: 10989
		private const string methodLeavingFormatter = "Leave {0}";

		// Token: 0x04002AEE RID: 10990
		private const string propertyOutputFormatter = "Enter property {0}:";

		// Token: 0x04002AEF RID: 10991
		private const string propertyLeavingFormatter = "Leave property {0}";

		// Token: 0x04002AF0 RID: 10992
		private const string delegateHandlerOutputFormatter = "Enter delegate handler: {0}:";

		// Token: 0x04002AF1 RID: 10993
		private const string delegateHandlerLeavingFormatter = "Leave delegate handler: {0}";

		// Token: 0x04002AF2 RID: 10994
		private const string eventHandlerOutputFormatter = "Enter event handler: {0}:";

		// Token: 0x04002AF3 RID: 10995
		private const string eventHandlerLeavingFormatter = "Leave event handler: {0}";

		// Token: 0x04002AF4 RID: 10996
		private const string exceptionOutputFormatter = "{0}: {1}\n{2}";

		// Token: 0x04002AF5 RID: 10997
		private const string innermostExceptionOutputFormatter = "Inner-most {0}: {1}\n{2}";

		// Token: 0x04002AF6 RID: 10998
		private const string lockEnterFormatter = "Enter Lock: {0}";

		// Token: 0x04002AF7 RID: 10999
		private const string lockLeavingFormatter = "Leave Lock: {0}";

		// Token: 0x04002AF8 RID: 11000
		private const string lockAcquiringFormatter = "Acquiring Lock: {0}";

		// Token: 0x04002AF9 RID: 11001
		private static object getTracerLock = new object();

		// Token: 0x04002AFA RID: 11002
		private static bool globalTraceInitialized;

		// Token: 0x04002AFB RID: 11003
		private bool alreadyTracing;

		// Token: 0x04002AFC RID: 11004
		private static readonly ThreadLocal<int> LocalIndentLevel = new ThreadLocal<int>();

		// Token: 0x04002AFD RID: 11005
		private PSTraceSourceOptions flags;

		// Token: 0x04002AFE RID: 11006
		private string description = string.Empty;

		// Token: 0x04002AFF RID: 11007
		private bool showHeaders = true;

		// Token: 0x04002B00 RID: 11008
		private string fullName = string.Empty;

		// Token: 0x04002B01 RID: 11009
		private string name;

		// Token: 0x04002B02 RID: 11010
		private TraceSource traceSource;

		// Token: 0x04002B03 RID: 11011
		private static Dictionary<string, PSTraceSource> traceCatalog = new Dictionary<string, PSTraceSource>(StringComparer.OrdinalIgnoreCase);

		// Token: 0x04002B04 RID: 11012
		private static Dictionary<string, PSTraceSource> preConfiguredTraceSource = new Dictionary<string, PSTraceSource>(StringComparer.OrdinalIgnoreCase);
	}
}
