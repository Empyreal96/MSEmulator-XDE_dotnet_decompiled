using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Management.Automation.Runspaces;
using System.Text;

namespace System.Management.Automation
{
	// Token: 0x020001B6 RID: 438
	internal abstract class BaseCommandHelpInfo : HelpInfo
	{
		// Token: 0x06001459 RID: 5209 RVA: 0x0007CDEA File Offset: 0x0007AFEA
		internal BaseCommandHelpInfo(HelpCategory helpCategory)
		{
			this._helpCategory = helpCategory;
		}

		// Token: 0x170004C4 RID: 1220
		// (get) Token: 0x0600145A RID: 5210 RVA: 0x0007CDFC File Offset: 0x0007AFFC
		internal PSObject Details
		{
			get
			{
				if (this.FullHelp == null)
				{
					return null;
				}
				if (this.FullHelp.Properties["Details"] == null || this.FullHelp.Properties["Details"].Value == null)
				{
					return null;
				}
				return PSObject.AsPSObject(this.FullHelp.Properties["Details"].Value);
			}
		}

		// Token: 0x170004C5 RID: 1221
		// (get) Token: 0x0600145B RID: 5211 RVA: 0x0007CE68 File Offset: 0x0007B068
		internal override string Name
		{
			get
			{
				PSObject details = this.Details;
				if (details == null)
				{
					return "";
				}
				if (details.Properties["Name"] == null || details.Properties["Name"].Value == null)
				{
					return "";
				}
				string text = details.Properties["Name"].Value.ToString();
				if (text == null)
				{
					return "";
				}
				return text.Trim();
			}
		}

		// Token: 0x170004C6 RID: 1222
		// (get) Token: 0x0600145C RID: 5212 RVA: 0x0007CEE0 File Offset: 0x0007B0E0
		internal override string Synopsis
		{
			get
			{
				PSObject details = this.Details;
				if (details == null)
				{
					return "";
				}
				if (details.Properties["Description"] == null || details.Properties["Description"].Value == null)
				{
					return "";
				}
				object[] array = (object[])LanguagePrimitives.ConvertTo(details.Properties["Description"].Value, typeof(object[]), CultureInfo.InvariantCulture);
				if (array == null || array.Length == 0)
				{
					return "";
				}
				PSObject psobject = (array[0] == null) ? null : PSObject.AsPSObject(array[0]);
				if (psobject == null || psobject.Properties["Text"] == null || psobject.Properties["Text"].Value == null)
				{
					return "";
				}
				string text = psobject.Properties["Text"].Value.ToString();
				if (text == null)
				{
					return "";
				}
				return text.Trim();
			}
		}

		// Token: 0x170004C7 RID: 1223
		// (get) Token: 0x0600145D RID: 5213 RVA: 0x0007CFD6 File Offset: 0x0007B1D6
		internal override HelpCategory HelpCategory
		{
			get
			{
				return this._helpCategory;
			}
		}

		// Token: 0x0600145E RID: 5214 RVA: 0x0007CFE0 File Offset: 0x0007B1E0
		internal override Uri GetUriForOnlineHelp()
		{
			UriFormatException ex = null;
			Uri uri;
			try
			{
				uri = BaseCommandHelpInfo.GetUriFromCommandPSObject(this.FullHelp);
				if (uri != null)
				{
					return uri;
				}
			}
			catch (UriFormatException ex2)
			{
				ex = ex2;
			}
			uri = this.LookupUriFromCommandInfo();
			if (uri != null)
			{
				return uri;
			}
			if (ex != null)
			{
				throw ex;
			}
			return base.GetUriForOnlineHelp();
		}

		// Token: 0x0600145F RID: 5215 RVA: 0x0007D040 File Offset: 0x0007B240
		internal Uri LookupUriFromCommandInfo()
		{
			HelpCategory helpCategory = this.HelpCategory;
			if (helpCategory <= HelpCategory.Function)
			{
				if (helpCategory == HelpCategory.Cmdlet)
				{
					CommandTypes commandTypes = CommandTypes.Cmdlet;
					goto IL_69;
				}
				if (helpCategory == HelpCategory.ScriptCommand)
				{
					CommandTypes commandTypes = CommandTypes.Script;
					goto IL_69;
				}
				if (helpCategory == HelpCategory.Function)
				{
					CommandTypes commandTypes = CommandTypes.Function;
					goto IL_69;
				}
			}
			else
			{
				if (helpCategory == HelpCategory.Filter)
				{
					CommandTypes commandTypes = CommandTypes.Filter;
					goto IL_69;
				}
				if (helpCategory == HelpCategory.ExternalScript)
				{
					CommandTypes commandTypes = CommandTypes.ExternalScript;
					goto IL_69;
				}
				if (helpCategory == HelpCategory.Configuration)
				{
					CommandTypes commandTypes = CommandTypes.Configuration;
					goto IL_69;
				}
			}
			return null;
			IL_69:
			string name = this.Name;
			string empty = string.Empty;
			if (this.FullHelp.Properties["ModuleName"] != null)
			{
				PSNoteProperty psnoteProperty = this.FullHelp.Properties["ModuleName"] as PSNoteProperty;
				if (psnoteProperty != null)
				{
					LanguagePrimitives.TryConvertTo<string>(psnoteProperty.Value, CultureInfo.InvariantCulture, out empty);
				}
			}
			string text = name;
			if (!string.IsNullOrEmpty(empty))
			{
				text = string.Format(CultureInfo.InvariantCulture, "{0}\\{1}", new object[]
				{
					empty,
					name
				});
			}
			ExecutionContext executionContextFromTLS = LocalPipeline.GetExecutionContextFromTLS();
			if (executionContextFromTLS == null)
			{
				return null;
			}
			try
			{
				CommandInfo commandInfo = null;
				CommandTypes commandTypes;
				if (commandTypes == CommandTypes.Cmdlet)
				{
					commandInfo = executionContextFromTLS.SessionState.InvokeCommand.GetCmdlet(text);
				}
				else
				{
					commandInfo = executionContextFromTLS.SessionState.InvokeCommand.GetCommands(text, commandTypes, false).FirstOrDefault<CommandInfo>();
				}
				if (commandInfo == null || commandInfo.CommandMetadata == null)
				{
					return null;
				}
				string text2 = commandInfo.CommandMetadata.HelpUri;
				if (!string.IsNullOrEmpty(text2))
				{
					if (!Uri.IsWellFormedUriString(text2, UriKind.RelativeOrAbsolute))
					{
						string[] array = text2.Split(new char[]
						{
							' '
						});
						text2 = array[0];
					}
					try
					{
						return new Uri(text2);
					}
					catch (UriFormatException)
					{
						throw PSTraceSource.NewInvalidOperationException(HelpErrors.InvalidURI, new object[]
						{
							commandInfo.CommandMetadata.HelpUri
						});
					}
				}
			}
			catch (CommandNotFoundException)
			{
			}
			return null;
		}

		// Token: 0x06001460 RID: 5216 RVA: 0x0007D22C File Offset: 0x0007B42C
		internal static Uri GetUriFromCommandPSObject(PSObject commandFullHelp)
		{
			if (commandFullHelp == null || commandFullHelp.Properties["relatedLinks"] == null || commandFullHelp.Properties["relatedLinks"].Value == null)
			{
				return null;
			}
			PSObject psobject = PSObject.AsPSObject(commandFullHelp.Properties["relatedLinks"].Value);
			if (psobject.Properties["navigationLink"] == null)
			{
				return null;
			}
			object[] array = (object[])LanguagePrimitives.ConvertTo(psobject.Properties["navigationLink"].Value, typeof(object[]), CultureInfo.InvariantCulture);
			foreach (object obj in array)
			{
				if (obj != null)
				{
					PSObject psobject2 = PSObject.AsPSObject(obj);
					PSNoteProperty psnoteProperty = psobject2.Properties["uri"] as PSNoteProperty;
					if (psnoteProperty != null)
					{
						string text = string.Empty;
						LanguagePrimitives.TryConvertTo<string>(psnoteProperty.Value, CultureInfo.InvariantCulture, out text);
						if (!string.IsNullOrEmpty(text))
						{
							if (!Uri.IsWellFormedUriString(text, UriKind.RelativeOrAbsolute))
							{
								string[] array3 = text.Split(new char[]
								{
									' '
								});
								text = array3[0];
							}
							try
							{
								return new Uri(text);
							}
							catch (UriFormatException)
							{
								throw PSTraceSource.NewInvalidOperationException(HelpErrors.InvalidURI, new object[]
								{
									text
								});
							}
						}
					}
				}
			}
			return null;
		}

		// Token: 0x06001461 RID: 5217 RVA: 0x0007D398 File Offset: 0x0007B598
		internal override bool MatchPatternInContent(WildcardPattern pattern)
		{
			string text = this.Synopsis;
			string text2 = this.DetailedDescription;
			if (text == null)
			{
				text = string.Empty;
			}
			if (text2 == null)
			{
				text2 = string.Empty;
			}
			return pattern.IsMatch(text) || pattern.IsMatch(text2);
		}

		// Token: 0x06001462 RID: 5218 RVA: 0x0007D3D8 File Offset: 0x0007B5D8
		internal override PSObject[] GetParameter(string pattern)
		{
			if (this.FullHelp == null || this.FullHelp.Properties["parameters"] == null || this.FullHelp.Properties["parameters"].Value == null)
			{
				return base.GetParameter(pattern);
			}
			PSObject psobject = PSObject.AsPSObject(this.FullHelp.Properties["parameters"].Value);
			if (psobject.Properties["parameter"] == null)
			{
				return base.GetParameter(pattern);
			}
			PSObject[] array = (PSObject[])LanguagePrimitives.ConvertTo(psobject.Properties["parameter"].Value, typeof(PSObject[]), CultureInfo.InvariantCulture);
			if (string.IsNullOrEmpty(pattern))
			{
				return array;
			}
			List<PSObject> list = new List<PSObject>();
			WildcardPattern wildcardPattern = new WildcardPattern(pattern, WildcardOptions.IgnoreCase);
			foreach (PSObject psobject2 in array)
			{
				if (psobject2.Properties["name"] != null && psobject2.Properties["name"].Value != null)
				{
					string input = psobject2.Properties["name"].Value.ToString();
					if (wildcardPattern.IsMatch(input))
					{
						list.Add(psobject2);
					}
				}
			}
			return list.ToArray();
		}

		// Token: 0x170004C8 RID: 1224
		// (get) Token: 0x06001463 RID: 5219 RVA: 0x0007D528 File Offset: 0x0007B728
		internal string DetailedDescription
		{
			get
			{
				if (this.FullHelp == null)
				{
					return "";
				}
				if (this.FullHelp.Properties["Description"] == null || this.FullHelp.Properties["Description"].Value == null)
				{
					return "";
				}
				object[] array = (object[])LanguagePrimitives.ConvertTo(this.FullHelp.Properties["Description"].Value, typeof(object[]), CultureInfo.InvariantCulture);
				if (array == null || array.Length == 0)
				{
					return "";
				}
				StringBuilder stringBuilder = new StringBuilder(400);
				foreach (object obj in array)
				{
					if (obj != null)
					{
						PSObject psobject = PSObject.AsPSObject(obj);
						if (psobject != null && psobject.Properties["Text"] != null && psobject.Properties["Text"].Value != null)
						{
							string value = psobject.Properties["Text"].Value.ToString();
							stringBuilder.Append(value);
							stringBuilder.Append(Environment.NewLine);
						}
					}
				}
				return stringBuilder.ToString().Trim();
			}
		}

		// Token: 0x040008D2 RID: 2258
		private HelpCategory _helpCategory;
	}
}
