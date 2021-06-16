using System;
using System.Collections;
using System.Collections.Generic;
using System.Globalization;
using System.Management.Automation;
using System.Management.Automation.Internal;
using System.Text;

namespace Microsoft.PowerShell.Commands
{
	// Token: 0x02000493 RID: 1171
	[Cmdlet("ForEach", "Object", SupportsShouldProcess = true, DefaultParameterSetName = "ScriptBlockSet", HelpUri = "http://go.microsoft.com/fwlink/?LinkID=113300", RemotingCapability = RemotingCapability.None)]
	public sealed class ForEachObjectCommand : PSCmdlet
	{
		// Token: 0x17000BB8 RID: 3000
		// (get) Token: 0x06003446 RID: 13382 RVA: 0x0011CD99 File Offset: 0x0011AF99
		// (set) Token: 0x06003445 RID: 13381 RVA: 0x0011CD90 File Offset: 0x0011AF90
		[Parameter(ValueFromPipeline = true, ParameterSetName = "ScriptBlockSet")]
		[Parameter(ValueFromPipeline = true, ParameterSetName = "PropertyAndMethodSet")]
		public PSObject InputObject
		{
			get
			{
				return this._inputObject;
			}
			set
			{
				this._inputObject = value;
			}
		}

		// Token: 0x17000BB9 RID: 3001
		// (get) Token: 0x06003448 RID: 13384 RVA: 0x0011CDB0 File Offset: 0x0011AFB0
		// (set) Token: 0x06003447 RID: 13383 RVA: 0x0011CDA1 File Offset: 0x0011AFA1
		[Parameter(ParameterSetName = "ScriptBlockSet")]
		public ScriptBlock Begin
		{
			get
			{
				return null;
			}
			set
			{
				this.scripts.Insert(0, value);
			}
		}

		// Token: 0x17000BBA RID: 3002
		// (get) Token: 0x0600344A RID: 13386 RVA: 0x0011CDD1 File Offset: 0x0011AFD1
		// (set) Token: 0x06003449 RID: 13385 RVA: 0x0011CDB3 File Offset: 0x0011AFB3
		[AllowNull]
		[AllowEmptyCollection]
		[Parameter(Mandatory = true, Position = 0, ParameterSetName = "ScriptBlockSet")]
		public ScriptBlock[] Process
		{
			get
			{
				return null;
			}
			set
			{
				if (value == null)
				{
					this.scripts.Add(null);
					return;
				}
				this.scripts.AddRange(value);
			}
		}

		// Token: 0x17000BBB RID: 3003
		// (get) Token: 0x0600344C RID: 13388 RVA: 0x0011CDE4 File Offset: 0x0011AFE4
		// (set) Token: 0x0600344B RID: 13387 RVA: 0x0011CDD4 File Offset: 0x0011AFD4
		[Parameter(ParameterSetName = "ScriptBlockSet")]
		public ScriptBlock End
		{
			get
			{
				return this.endScript;
			}
			set
			{
				this.endScript = value;
				this.setEndScript = true;
			}
		}

		// Token: 0x17000BBC RID: 3004
		// (get) Token: 0x0600344E RID: 13390 RVA: 0x0011CE0A File Offset: 0x0011B00A
		// (set) Token: 0x0600344D RID: 13389 RVA: 0x0011CDEC File Offset: 0x0011AFEC
		[Parameter(ParameterSetName = "ScriptBlockSet", ValueFromRemainingArguments = true)]
		[AllowNull]
		[AllowEmptyCollection]
		public ScriptBlock[] RemainingScripts
		{
			get
			{
				return null;
			}
			set
			{
				if (value == null)
				{
					this.scripts.Add(null);
					return;
				}
				this.scripts.AddRange(value);
			}
		}

		// Token: 0x17000BBD RID: 3005
		// (get) Token: 0x06003450 RID: 13392 RVA: 0x0011CE16 File Offset: 0x0011B016
		// (set) Token: 0x0600344F RID: 13391 RVA: 0x0011CE0D File Offset: 0x0011B00D
		[Parameter(Mandatory = true, Position = 0, ParameterSetName = "PropertyAndMethodSet")]
		[ValidateNotNullOrEmpty]
		public string MemberName
		{
			get
			{
				return this._propertyOrMethodName;
			}
			set
			{
				this._propertyOrMethodName = value;
			}
		}

		// Token: 0x17000BBE RID: 3006
		// (get) Token: 0x06003452 RID: 13394 RVA: 0x0011CE27 File Offset: 0x0011B027
		// (set) Token: 0x06003451 RID: 13393 RVA: 0x0011CE1E File Offset: 0x0011B01E
		[Alias(new string[]
		{
			"Args"
		})]
		[Parameter(ParameterSetName = "PropertyAndMethodSet", ValueFromRemainingArguments = true)]
		public object[] ArgumentList
		{
			get
			{
				return this._arguments;
			}
			set
			{
				this._arguments = value;
			}
		}

		// Token: 0x06003453 RID: 13395 RVA: 0x0011CE30 File Offset: 0x0011B030
		protected override void BeginProcessing()
		{
			if (base.ParameterSetName != "ScriptBlockSet")
			{
				return;
			}
			Dictionary<string, object> boundParameters = base.MyInvocation.BoundParameters;
			if (boundParameters != null)
			{
				SwitchParameter switchParameter = false;
				SwitchParameter switchParameter2 = false;
				if (boundParameters.ContainsKey("whatif"))
				{
					switchParameter = (SwitchParameter)boundParameters["whatif"];
				}
				if (boundParameters.ContainsKey("confirm"))
				{
					switchParameter2 = (SwitchParameter)boundParameters["confirm"];
				}
				if (switchParameter || switchParameter2)
				{
					string noShouldProcessForScriptBlockSet = InternalCommandStrings.NoShouldProcessForScriptBlockSet;
					ErrorRecord errorRecord = new ErrorRecord(new InvalidOperationException(noShouldProcessForScriptBlockSet), "NoShouldProcessForScriptBlockSet", ErrorCategory.InvalidOperation, null);
					base.ThrowTerminatingError(errorRecord);
				}
			}
			this.end = this.scripts.Count;
			this.start = ((this.scripts.Count > 1) ? 1 : 0);
			if (!this.setEndScript && this.scripts.Count > 2)
			{
				this.end = this.scripts.Count - 1;
				this.endScript = this.scripts[this.end];
			}
			if (this.end < 2)
			{
				return;
			}
			if (this.scripts[0] == null)
			{
				return;
			}
			this.scripts[0].InvokeUsingCmdlet(this, false, ScriptBlock.ErrorHandlingBehavior.WriteToCurrentErrorPipe, AutomationNull.Value, new object[0], AutomationNull.Value, new object[0]);
		}

		// Token: 0x06003454 RID: 13396 RVA: 0x0011CF88 File Offset: 0x0011B188
		protected override void ProcessRecord()
		{
			string parameterSetName;
			if ((parameterSetName = base.ParameterSetName) != null)
			{
				if (parameterSetName == "ScriptBlockSet")
				{
					for (int i = this.start; i < this.end; i++)
					{
						if (this.scripts[i] != null)
						{
							this.scripts[i].InvokeUsingCmdlet(this, false, ScriptBlock.ErrorHandlingBehavior.WriteToCurrentErrorPipe, this.InputObject, new object[]
							{
								this.InputObject
							}, AutomationNull.Value, new object[0]);
						}
					}
					return;
				}
				if (!(parameterSetName == "PropertyAndMethodSet"))
				{
					return;
				}
				this.targetString = string.Format(CultureInfo.InvariantCulture, InternalCommandStrings.ForEachObjectTarget, new object[]
				{
					ForEachObjectCommand.GetStringRepresentation(this.InputObject)
				});
				if (LanguagePrimitives.IsNull(this.InputObject))
				{
					if (this._arguments != null && this._arguments.Length > 0)
					{
						base.WriteError(ForEachObjectCommand.GenerateNameParameterError("InputObject", ParserStrings.InvokeMethodOnNull, "InvokeMethodOnNull", this._inputObject, new object[0]));
						return;
					}
					string action = string.Format(CultureInfo.InvariantCulture, InternalCommandStrings.ForEachObjectPropertyAction, new object[]
					{
						this._propertyOrMethodName
					});
					if (base.ShouldProcess(this.targetString, action))
					{
						if (base.Context.IsStrictVersion(2))
						{
							base.WriteError(ForEachObjectCommand.GenerateNameParameterError("InputObject", InternalCommandStrings.InputObjectIsNull, "InputObjectIsNull", this._inputObject, new object[0]));
							return;
						}
						base.WriteObject(null);
					}
					return;
				}
				else
				{
					ErrorRecord errorRecord = null;
					if (this._arguments != null && this._arguments.Length > 0)
					{
						this.MethodCallWithArguments();
					}
					else
					{
						if (this.GetValueFromIDictionaryInput())
						{
							return;
						}
						PSMemberInfo psmemberInfo = null;
						if (WildcardPattern.ContainsWildcardCharacters(this._propertyOrMethodName))
						{
							ReadOnlyPSMemberInfoCollection<PSMemberInfo> readOnlyPSMemberInfoCollection = this._inputObject.Members.Match(this._propertyOrMethodName, PSMemberTypes.All);
							if (readOnlyPSMemberInfoCollection.Count > 1)
							{
								StringBuilder stringBuilder = new StringBuilder();
								foreach (PSMemberInfo psmemberInfo2 in readOnlyPSMemberInfoCollection)
								{
									stringBuilder.AppendFormat(CultureInfo.InvariantCulture, " {0}", new object[]
									{
										psmemberInfo2.Name
									});
								}
								base.WriteError(ForEachObjectCommand.GenerateNameParameterError("Name", InternalCommandStrings.AmbiguousPropertyOrMethodName, "AmbiguousPropertyOrMethodName", this._inputObject, new object[]
								{
									this._propertyOrMethodName,
									stringBuilder
								}));
								return;
							}
							if (readOnlyPSMemberInfoCollection.Count == 1)
							{
								psmemberInfo = readOnlyPSMemberInfoCollection[0];
							}
						}
						else
						{
							psmemberInfo = this._inputObject.Members[this._propertyOrMethodName];
						}
						if (psmemberInfo == null)
						{
							errorRecord = ForEachObjectCommand.GenerateNameParameterError("Name", InternalCommandStrings.PropertyOrMethodNotFound, "PropertyOrMethodNotFound", this._inputObject, new object[]
							{
								this._propertyOrMethodName
							});
						}
						else
						{
							if (psmemberInfo is PSMethodInfo)
							{
								PSParameterizedProperty psparameterizedProperty = psmemberInfo as PSParameterizedProperty;
								if (psparameterizedProperty != null)
								{
									string action2 = string.Format(CultureInfo.InvariantCulture, InternalCommandStrings.ForEachObjectPropertyAction, new object[]
									{
										psparameterizedProperty.Name
									});
									if (base.ShouldProcess(this.targetString, action2))
									{
										base.WriteObject(psmemberInfo.Value);
									}
									return;
								}
								PSMethodInfo psmethodInfo = psmemberInfo as PSMethodInfo;
								try
								{
									string action3 = string.Format(CultureInfo.InvariantCulture, InternalCommandStrings.ForEachObjectMethodActionWithoutArguments, new object[]
									{
										psmethodInfo.Name
									});
									if (base.ShouldProcess(this.targetString, action3) && !this.BlockMethodInLanguageMode(this.InputObject))
									{
										object obj = psmethodInfo.Invoke(new object[0]);
										this.WriteToPipelineWithUnrolling(obj);
									}
									goto IL_451;
								}
								catch (PipelineStoppedException)
								{
									throw;
								}
								catch (Exception ex)
								{
									CommandProcessorBase.CheckForSevereException(ex);
									MethodException ex2 = ex as MethodException;
									if (ex2 != null && ex2.ErrorRecord != null && ex2.ErrorRecord.FullyQualifiedErrorId == "MethodCountCouldNotFindBest")
									{
										base.WriteObject(psmethodInfo.Value);
									}
									else
									{
										base.WriteError(new ErrorRecord(ex, "MethodInvocationError", ErrorCategory.InvalidOperation, this._inputObject));
									}
									goto IL_451;
								}
							}
							string action4 = string.Format(CultureInfo.InvariantCulture, InternalCommandStrings.ForEachObjectPropertyAction, new object[]
							{
								psmemberInfo.Name
							});
							if (base.ShouldProcess(this.targetString, action4))
							{
								try
								{
									this.WriteToPipelineWithUnrolling(psmemberInfo.Value);
								}
								catch (TerminateException)
								{
									throw;
								}
								catch (MethodException)
								{
									throw;
								}
								catch (PipelineStoppedException)
								{
									throw;
								}
								catch (Exception e)
								{
									CommandProcessorBase.CheckForSevereException(e);
									base.WriteObject(null);
								}
							}
						}
					}
					IL_451:
					if (errorRecord != null)
					{
						string action5 = string.Format(CultureInfo.InvariantCulture, InternalCommandStrings.ForEachObjectPropertyAction, new object[]
						{
							this._propertyOrMethodName
						});
						if (base.ShouldProcess(this.targetString, action5))
						{
							if (base.Context.IsStrictVersion(2))
							{
								base.WriteError(errorRecord);
								return;
							}
							base.WriteObject(null);
						}
					}
				}
			}
		}

		// Token: 0x06003455 RID: 13397 RVA: 0x0011D494 File Offset: 0x0011B694
		private void MethodCallWithArguments()
		{
			ReadOnlyPSMemberInfoCollection<PSMemberInfo> readOnlyPSMemberInfoCollection = this._inputObject.Members.Match(this._propertyOrMethodName, PSMemberTypes.Method | PSMemberTypes.CodeMethod | PSMemberTypes.ScriptMethod | PSMemberTypes.ParameterizedProperty);
			if (readOnlyPSMemberInfoCollection.Count > 1)
			{
				StringBuilder stringBuilder = new StringBuilder();
				foreach (PSMemberInfo psmemberInfo in readOnlyPSMemberInfoCollection)
				{
					stringBuilder.AppendFormat(CultureInfo.InvariantCulture, " {0}", new object[]
					{
						psmemberInfo.Name
					});
				}
				base.WriteError(ForEachObjectCommand.GenerateNameParameterError("Name", InternalCommandStrings.AmbiguousMethodName, "AmbiguousMethodName", this._inputObject, new object[]
				{
					this._propertyOrMethodName,
					stringBuilder
				}));
				return;
			}
			if (readOnlyPSMemberInfoCollection.Count == 0 || !(readOnlyPSMemberInfoCollection[0] is PSMethodInfo))
			{
				base.WriteError(ForEachObjectCommand.GenerateNameParameterError("Name", InternalCommandStrings.MethodNotFound, "MethodNotFound", this._inputObject, new object[]
				{
					this._propertyOrMethodName
				}));
				return;
			}
			PSMethodInfo psmethodInfo = readOnlyPSMemberInfoCollection[0] as PSMethodInfo;
			StringBuilder stringBuilder2 = new StringBuilder(ForEachObjectCommand.GetStringRepresentation(this._arguments[0]));
			for (int i = 1; i < this._arguments.Length; i++)
			{
				stringBuilder2.AppendFormat(CultureInfo.InvariantCulture, ", {0}", new object[]
				{
					ForEachObjectCommand.GetStringRepresentation(this._arguments[i])
				});
			}
			string action = string.Format(CultureInfo.InvariantCulture, InternalCommandStrings.ForEachObjectMethodActionWithArguments, new object[]
			{
				psmethodInfo.Name,
				stringBuilder2
			});
			try
			{
				if (base.ShouldProcess(this.targetString, action) && !this.BlockMethodInLanguageMode(this.InputObject))
				{
					object obj = psmethodInfo.Invoke(this._arguments);
					this.WriteToPipelineWithUnrolling(obj);
				}
			}
			catch (PipelineStoppedException)
			{
				throw;
			}
			catch (Exception ex)
			{
				CommandProcessorBase.CheckForSevereException(ex);
				base.WriteError(new ErrorRecord(ex, "MethodInvocationError", ErrorCategory.InvalidOperation, this._inputObject));
			}
		}

		// Token: 0x06003456 RID: 13398 RVA: 0x0011D6B8 File Offset: 0x0011B8B8
		private static string GetStringRepresentation(object obj)
		{
			string text;
			try
			{
				text = (LanguagePrimitives.IsNull(obj) ? "null" : obj.ToString());
			}
			catch (Exception e)
			{
				CommandProcessorBase.CheckForSevereException(e);
				text = null;
			}
			if (string.IsNullOrEmpty(text))
			{
				PSObject psobject = obj as PSObject;
				text = ((psobject != null) ? psobject.BaseObject.GetType().FullName : obj.GetType().FullName);
			}
			return text;
		}

		// Token: 0x06003457 RID: 13399 RVA: 0x0011D72C File Offset: 0x0011B92C
		private bool GetValueFromIDictionaryInput()
		{
			object obj = PSObject.Base(this._inputObject);
			IDictionary dictionary = obj as IDictionary;
			try
			{
				if (dictionary != null && dictionary.Contains(this._propertyOrMethodName))
				{
					string action = string.Format(CultureInfo.InvariantCulture, InternalCommandStrings.ForEachObjectKeyAction, new object[]
					{
						this._propertyOrMethodName
					});
					if (base.ShouldProcess(this.targetString, action))
					{
						object obj2 = dictionary[this._propertyOrMethodName];
						this.WriteToPipelineWithUnrolling(obj2);
					}
					return true;
				}
			}
			catch (InvalidOperationException)
			{
			}
			return false;
		}

		// Token: 0x06003458 RID: 13400 RVA: 0x0011D7C4 File Offset: 0x0011B9C4
		private void WriteToPipelineWithUnrolling(object obj)
		{
			IEnumerator enumerator = LanguagePrimitives.GetEnumerator(obj);
			if (enumerator != null)
			{
				this.WriteOutIEnumerator(enumerator);
				return;
			}
			base.WriteObject(obj, true);
		}

		// Token: 0x06003459 RID: 13401 RVA: 0x0011D7EC File Offset: 0x0011B9EC
		private void WriteOutIEnumerator(IEnumerator list)
		{
			if (list != null)
			{
				while (ParserOps.MoveNext(base.Context, null, list))
				{
					object obj = ParserOps.Current(null, list);
					if (obj != AutomationNull.Value)
					{
						base.WriteObject(obj);
					}
				}
			}
		}

		// Token: 0x0600345A RID: 13402 RVA: 0x0011D824 File Offset: 0x0011BA24
		private bool BlockMethodInLanguageMode(object inputObject)
		{
			if (base.Context.LanguageMode.Equals(PSLanguageMode.RestrictedLanguage))
			{
				PSInvalidOperationException exception = new PSInvalidOperationException(InternalCommandStrings.NoMethodInvocationInRestrictedLanguageMode);
				base.WriteError(new ErrorRecord(exception, "NoMethodInvocationInRestrictedLanguageMode", ErrorCategory.InvalidOperation, null));
				return true;
			}
			if (base.Context.LanguageMode.Equals(PSLanguageMode.ConstrainedLanguage))
			{
				object obj = PSObject.Base(inputObject);
				if (!CoreTypes.Contains(obj.GetType()))
				{
					PSInvalidOperationException exception2 = new PSInvalidOperationException(ParserStrings.InvokeMethodConstrainedLanguage);
					base.WriteError(new ErrorRecord(exception2, "MethodInvocationNotSupportedInConstrainedLanguage", ErrorCategory.InvalidOperation, null));
					return true;
				}
			}
			return false;
		}

		// Token: 0x0600345B RID: 13403 RVA: 0x0011D8C0 File Offset: 0x0011BAC0
		internal static ErrorRecord GenerateNameParameterError(string paraName, string resourceString, string errorId, object target, params object[] args)
		{
			string text;
			if (args == null || args.Length == 0)
			{
				text = resourceString;
			}
			else
			{
				text = StringUtil.Format(resourceString, args);
			}
			string.IsNullOrEmpty(text);
			return new ErrorRecord(new PSArgumentException(text, paraName), errorId, ErrorCategory.InvalidArgument, target);
		}

		// Token: 0x0600345C RID: 13404 RVA: 0x0011D8FC File Offset: 0x0011BAFC
		protected override void EndProcessing()
		{
			if (base.ParameterSetName != "ScriptBlockSet")
			{
				return;
			}
			if (this.endScript == null)
			{
				return;
			}
			this.endScript.InvokeUsingCmdlet(this, false, ScriptBlock.ErrorHandlingBehavior.WriteToCurrentErrorPipe, AutomationNull.Value, new object[0], AutomationNull.Value, new object[0]);
		}

		// Token: 0x04001AE2 RID: 6882
		private PSObject _inputObject = AutomationNull.Value;

		// Token: 0x04001AE3 RID: 6883
		private List<ScriptBlock> scripts = new List<ScriptBlock>();

		// Token: 0x04001AE4 RID: 6884
		private ScriptBlock endScript;

		// Token: 0x04001AE5 RID: 6885
		private bool setEndScript;

		// Token: 0x04001AE6 RID: 6886
		private int start;

		// Token: 0x04001AE7 RID: 6887
		private int end;

		// Token: 0x04001AE8 RID: 6888
		private string _propertyOrMethodName;

		// Token: 0x04001AE9 RID: 6889
		private string targetString;

		// Token: 0x04001AEA RID: 6890
		private object[] _arguments;
	}
}
