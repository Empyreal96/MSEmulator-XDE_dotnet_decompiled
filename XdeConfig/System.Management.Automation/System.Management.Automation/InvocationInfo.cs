using System;
using System.Collections;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Management.Automation.Internal;
using System.Management.Automation.Language;
using System.Management.Automation.Runspaces;

namespace System.Management.Automation
{
	// Token: 0x0200002A RID: 42
	[DebuggerDisplay("Command = {_commandInfo}")]
	public class InvocationInfo
	{
		// Token: 0x060001B3 RID: 435 RVA: 0x00007EDF File Offset: 0x000060DF
		internal InvocationInfo(InternalCommand command) : this(command.CommandInfo, command.InvocationExtent ?? PositionUtilities.EmptyExtent)
		{
			this._commandOrigin = command.CommandOrigin;
		}

		// Token: 0x060001B4 RID: 436 RVA: 0x00007F08 File Offset: 0x00006108
		internal InvocationInfo(CommandInfo commandInfo, IScriptExtent scriptPosition) : this(commandInfo, scriptPosition, null)
		{
		}

		// Token: 0x060001B5 RID: 437 RVA: 0x00007F14 File Offset: 0x00006114
		internal InvocationInfo(CommandInfo commandInfo, IScriptExtent scriptPosition, ExecutionContext context)
		{
			this._historyId = -1L;
			this._pipelineIterationInfo = new int[0];
			base..ctor();
			this._commandInfo = commandInfo;
			this._commandOrigin = CommandOrigin.Internal;
			this._scriptPosition = scriptPosition;
			ExecutionContext executionContext = null;
			if (commandInfo != null && commandInfo.Context != null)
			{
				executionContext = commandInfo.Context;
			}
			else if (context != null)
			{
				executionContext = context;
			}
			if (executionContext != null)
			{
				LocalRunspace localRunspace = executionContext.CurrentRunspace as LocalRunspace;
				if (localRunspace != null && localRunspace.History != null)
				{
					this._historyId = localRunspace.History.GetNextHistoryId();
				}
			}
		}

		// Token: 0x060001B6 RID: 438 RVA: 0x00007F98 File Offset: 0x00006198
		internal InvocationInfo(PSObject psObject)
		{
			this._historyId = -1L;
			this._pipelineIterationInfo = new int[0];
			base..ctor();
			this._commandOrigin = (CommandOrigin)SerializationUtilities.GetPsObjectPropertyBaseObject(psObject, "InvocationInfo_CommandOrigin");
			this._expectingInput = (bool)SerializationUtilities.GetPropertyValue(psObject, "InvocationInfo_ExpectingInput");
			this._invocationName = (string)SerializationUtilities.GetPropertyValue(psObject, "InvocationInfo_InvocationName");
			this._historyId = (long)SerializationUtilities.GetPropertyValue(psObject, "InvocationInfo_HistoryId");
			this._pipelineLength = (int)SerializationUtilities.GetPropertyValue(psObject, "InvocationInfo_PipelineLength");
			this._pipelinePosition = (int)SerializationUtilities.GetPropertyValue(psObject, "InvocationInfo_PipelinePosition");
			string scriptName = (string)SerializationUtilities.GetPropertyValue(psObject, "InvocationInfo_ScriptName");
			int scriptLineNumber = (int)SerializationUtilities.GetPropertyValue(psObject, "InvocationInfo_ScriptLineNumber");
			int offsetInLine = (int)SerializationUtilities.GetPropertyValue(psObject, "InvocationInfo_OffsetInLine");
			string text = (string)SerializationUtilities.GetPropertyValue(psObject, "InvocationInfo_Line");
			ScriptPosition scriptPosition = new ScriptPosition(scriptName, scriptLineNumber, offsetInLine, text);
			ScriptPosition endPosition;
			if (!string.IsNullOrEmpty(text))
			{
				int offsetInLine2 = text.Length + 1;
				endPosition = new ScriptPosition(scriptName, scriptLineNumber, offsetInLine2, text);
			}
			else
			{
				endPosition = scriptPosition;
			}
			this._scriptPosition = new ScriptExtent(scriptPosition, endPosition);
			this._commandInfo = RemoteCommandInfo.FromPSObjectForRemoting(psObject);
			ArrayList arrayList = (ArrayList)SerializationUtilities.GetPsObjectPropertyBaseObject(psObject, "InvocationInfo_PipelineIterationInfo");
			if (arrayList != null)
			{
				this._pipelineIterationInfo = (int[])arrayList.ToArray(typeof(int));
			}
			else
			{
				this._pipelineIterationInfo = new int[0];
			}
			Hashtable hashtable = (Hashtable)SerializationUtilities.GetPsObjectPropertyBaseObject(psObject, "InvocationInfo_BoundParameters");
			Dictionary<string, object> dictionary = new Dictionary<string, object>();
			if (hashtable != null)
			{
				foreach (object obj in hashtable)
				{
					DictionaryEntry dictionaryEntry = (DictionaryEntry)obj;
					dictionary.Add((string)dictionaryEntry.Key, dictionaryEntry.Value);
				}
			}
			this._boundParameters = dictionary;
			ArrayList arrayList2 = (ArrayList)SerializationUtilities.GetPsObjectPropertyBaseObject(psObject, "InvocationInfo_UnboundArguments");
			this._unboundArguments = new List<object>();
			if (arrayList2 != null)
			{
				foreach (object item in arrayList2)
				{
					this._unboundArguments.Add(item);
				}
			}
			object propertyValue = SerializationUtilities.GetPropertyValue(psObject, "SerializeExtent");
			bool flag = false;
			if (propertyValue != null)
			{
				flag = (bool)propertyValue;
			}
			if (flag)
			{
				this._displayScriptPosition = ScriptExtent.FromPSObjectForRemoting(psObject);
			}
		}

		// Token: 0x17000069 RID: 105
		// (get) Token: 0x060001B7 RID: 439 RVA: 0x00008234 File Offset: 0x00006434
		public CommandInfo MyCommand
		{
			get
			{
				return this._commandInfo;
			}
		}

		// Token: 0x1700006A RID: 106
		// (get) Token: 0x060001B8 RID: 440 RVA: 0x0000823C File Offset: 0x0000643C
		// (set) Token: 0x060001B9 RID: 441 RVA: 0x0000825C File Offset: 0x0000645C
		public Dictionary<string, object> BoundParameters
		{
			get
			{
				if (this._boundParameters == null)
				{
					this._boundParameters = new Dictionary<string, object>(StringComparer.OrdinalIgnoreCase);
				}
				return this._boundParameters;
			}
			internal set
			{
				this._boundParameters = value;
			}
		}

		// Token: 0x1700006B RID: 107
		// (get) Token: 0x060001BA RID: 442 RVA: 0x00008265 File Offset: 0x00006465
		// (set) Token: 0x060001BB RID: 443 RVA: 0x00008280 File Offset: 0x00006480
		public List<object> UnboundArguments
		{
			get
			{
				if (this._unboundArguments == null)
				{
					this._unboundArguments = new List<object>();
				}
				return this._unboundArguments;
			}
			internal set
			{
				this._unboundArguments = value;
			}
		}

		// Token: 0x1700006C RID: 108
		// (get) Token: 0x060001BC RID: 444 RVA: 0x00008289 File Offset: 0x00006489
		public int ScriptLineNumber
		{
			get
			{
				return this.ScriptPosition.StartLineNumber;
			}
		}

		// Token: 0x1700006D RID: 109
		// (get) Token: 0x060001BD RID: 445 RVA: 0x00008296 File Offset: 0x00006496
		public int OffsetInLine
		{
			get
			{
				return this.ScriptPosition.StartColumnNumber;
			}
		}

		// Token: 0x1700006E RID: 110
		// (get) Token: 0x060001BE RID: 446 RVA: 0x000082A3 File Offset: 0x000064A3
		// (set) Token: 0x060001BF RID: 447 RVA: 0x000082AB File Offset: 0x000064AB
		public long HistoryId
		{
			get
			{
				return this._historyId;
			}
			internal set
			{
				this._historyId = value;
			}
		}

		// Token: 0x1700006F RID: 111
		// (get) Token: 0x060001C0 RID: 448 RVA: 0x000082B4 File Offset: 0x000064B4
		public string ScriptName
		{
			get
			{
				return this.ScriptPosition.File ?? "";
			}
		}

		// Token: 0x17000070 RID: 112
		// (get) Token: 0x060001C1 RID: 449 RVA: 0x000082CA File Offset: 0x000064CA
		public string Line
		{
			get
			{
				if (this.ScriptPosition.StartScriptPosition != null)
				{
					return this.ScriptPosition.StartScriptPosition.Line;
				}
				return string.Empty;
			}
		}

		// Token: 0x17000071 RID: 113
		// (get) Token: 0x060001C2 RID: 450 RVA: 0x000082EF File Offset: 0x000064EF
		public string PositionMessage
		{
			get
			{
				return PositionUtilities.VerboseMessage(this.ScriptPosition);
			}
		}

		// Token: 0x17000072 RID: 114
		// (get) Token: 0x060001C3 RID: 451 RVA: 0x000082FC File Offset: 0x000064FC
		public string PSScriptRoot
		{
			get
			{
				if (!string.IsNullOrEmpty(this.ScriptPosition.File))
				{
					return Path.GetDirectoryName(this.ScriptPosition.File);
				}
				return string.Empty;
			}
		}

		// Token: 0x17000073 RID: 115
		// (get) Token: 0x060001C4 RID: 452 RVA: 0x00008326 File Offset: 0x00006526
		public string PSCommandPath
		{
			get
			{
				return this.ScriptPosition.File;
			}
		}

		// Token: 0x17000074 RID: 116
		// (get) Token: 0x060001C5 RID: 453 RVA: 0x00008333 File Offset: 0x00006533
		// (set) Token: 0x060001C6 RID: 454 RVA: 0x00008344 File Offset: 0x00006544
		public string InvocationName
		{
			get
			{
				return this._invocationName ?? "";
			}
			internal set
			{
				this._invocationName = value;
			}
		}

		// Token: 0x17000075 RID: 117
		// (get) Token: 0x060001C7 RID: 455 RVA: 0x0000834D File Offset: 0x0000654D
		// (set) Token: 0x060001C8 RID: 456 RVA: 0x00008355 File Offset: 0x00006555
		public int PipelineLength
		{
			get
			{
				return this._pipelineLength;
			}
			internal set
			{
				this._pipelineLength = value;
			}
		}

		// Token: 0x17000076 RID: 118
		// (get) Token: 0x060001C9 RID: 457 RVA: 0x0000835E File Offset: 0x0000655E
		// (set) Token: 0x060001CA RID: 458 RVA: 0x00008366 File Offset: 0x00006566
		public int PipelinePosition
		{
			get
			{
				return this._pipelinePosition;
			}
			internal set
			{
				this._pipelinePosition = value;
			}
		}

		// Token: 0x17000077 RID: 119
		// (get) Token: 0x060001CB RID: 459 RVA: 0x0000836F File Offset: 0x0000656F
		// (set) Token: 0x060001CC RID: 460 RVA: 0x00008377 File Offset: 0x00006577
		public bool ExpectingInput
		{
			get
			{
				return this._expectingInput;
			}
			internal set
			{
				this._expectingInput = value;
			}
		}

		// Token: 0x17000078 RID: 120
		// (get) Token: 0x060001CD RID: 461 RVA: 0x00008380 File Offset: 0x00006580
		// (set) Token: 0x060001CE RID: 462 RVA: 0x00008388 File Offset: 0x00006588
		public CommandOrigin CommandOrigin
		{
			get
			{
				return this._commandOrigin;
			}
			internal set
			{
				this._commandOrigin = value;
			}
		}

		// Token: 0x17000079 RID: 121
		// (get) Token: 0x060001CF RID: 463 RVA: 0x00008391 File Offset: 0x00006591
		// (set) Token: 0x060001D0 RID: 464 RVA: 0x00008399 File Offset: 0x00006599
		public IScriptExtent DisplayScriptPosition
		{
			get
			{
				return this._displayScriptPosition;
			}
			set
			{
				this._displayScriptPosition = value;
			}
		}

		// Token: 0x060001D1 RID: 465 RVA: 0x000083A4 File Offset: 0x000065A4
		public static InvocationInfo Create(CommandInfo commandInfo, IScriptExtent scriptPosition)
		{
			return new InvocationInfo(commandInfo, scriptPosition)
			{
				DisplayScriptPosition = scriptPosition
			};
		}

		// Token: 0x1700007A RID: 122
		// (get) Token: 0x060001D2 RID: 466 RVA: 0x000083C1 File Offset: 0x000065C1
		// (set) Token: 0x060001D3 RID: 467 RVA: 0x000083D8 File Offset: 0x000065D8
		internal IScriptExtent ScriptPosition
		{
			get
			{
				if (this._displayScriptPosition != null)
				{
					return this._displayScriptPosition;
				}
				return this._scriptPosition;
			}
			set
			{
				this._scriptPosition = value;
			}
		}

		// Token: 0x060001D4 RID: 468 RVA: 0x000083E1 File Offset: 0x000065E1
		internal string GetFullScript()
		{
			if (this.ScriptPosition == null || this.ScriptPosition.StartScriptPosition == null)
			{
				return null;
			}
			return this.ScriptPosition.StartScriptPosition.GetFullScript();
		}

		// Token: 0x1700007B RID: 123
		// (get) Token: 0x060001D5 RID: 469 RVA: 0x0000840A File Offset: 0x0000660A
		// (set) Token: 0x060001D6 RID: 470 RVA: 0x00008412 File Offset: 0x00006612
		internal int[] PipelineIterationInfo
		{
			get
			{
				return this._pipelineIterationInfo;
			}
			set
			{
				this._pipelineIterationInfo = value;
			}
		}

		// Token: 0x060001D7 RID: 471 RVA: 0x000084A4 File Offset: 0x000066A4
		internal void ToPSObjectForRemoting(PSObject psObject)
		{
			RemotingEncoder.AddNoteProperty<object>(psObject, "InvocationInfo_BoundParameters", () => this.BoundParameters);
			RemotingEncoder.AddNoteProperty<CommandOrigin>(psObject, "InvocationInfo_CommandOrigin", () => this.CommandOrigin);
			RemotingEncoder.AddNoteProperty<bool>(psObject, "InvocationInfo_ExpectingInput", () => this.ExpectingInput);
			RemotingEncoder.AddNoteProperty<string>(psObject, "InvocationInfo_InvocationName", () => this.InvocationName);
			RemotingEncoder.AddNoteProperty<string>(psObject, "InvocationInfo_Line", () => this.Line);
			RemotingEncoder.AddNoteProperty<int>(psObject, "InvocationInfo_OffsetInLine", () => this.OffsetInLine);
			RemotingEncoder.AddNoteProperty<long>(psObject, "InvocationInfo_HistoryId", () => this.HistoryId);
			RemotingEncoder.AddNoteProperty<int[]>(psObject, "InvocationInfo_PipelineIterationInfo", () => this.PipelineIterationInfo);
			RemotingEncoder.AddNoteProperty<int>(psObject, "InvocationInfo_PipelineLength", () => this.PipelineLength);
			RemotingEncoder.AddNoteProperty<int>(psObject, "InvocationInfo_PipelinePosition", () => this.PipelinePosition);
			RemotingEncoder.AddNoteProperty<string>(psObject, "InvocationInfo_PSScriptRoot", () => this.PSScriptRoot);
			RemotingEncoder.AddNoteProperty<string>(psObject, "InvocationInfo_PSCommandPath", () => this.PSCommandPath);
			RemotingEncoder.AddNoteProperty<string>(psObject, "InvocationInfo_PositionMessage", () => this.PositionMessage);
			RemotingEncoder.AddNoteProperty<int>(psObject, "InvocationInfo_ScriptLineNumber", () => this.ScriptLineNumber);
			RemotingEncoder.AddNoteProperty<string>(psObject, "InvocationInfo_ScriptName", () => this.ScriptName);
			RemotingEncoder.AddNoteProperty<object>(psObject, "InvocationInfo_UnboundArguments", () => this.UnboundArguments);
			ScriptExtent scriptExtent = this.DisplayScriptPosition as ScriptExtent;
			if (scriptExtent != null)
			{
				scriptExtent.ToPSObjectForRemoting(psObject);
				RemotingEncoder.AddNoteProperty<bool>(psObject, "SerializeExtent", () => true);
			}
			else
			{
				RemotingEncoder.AddNoteProperty<bool>(psObject, "SerializeExtent", () => false);
			}
			RemoteCommandInfo.ToPSObjectForRemoting(this.MyCommand, psObject);
		}

		// Token: 0x040000AA RID: 170
		private readonly CommandInfo _commandInfo;

		// Token: 0x040000AB RID: 171
		private IScriptExtent _scriptPosition;

		// Token: 0x040000AC RID: 172
		private string _invocationName;

		// Token: 0x040000AD RID: 173
		private long _historyId;

		// Token: 0x040000AE RID: 174
		private int _pipelinePosition;

		// Token: 0x040000AF RID: 175
		private int _pipelineLength;

		// Token: 0x040000B0 RID: 176
		private bool _expectingInput;

		// Token: 0x040000B1 RID: 177
		private int[] _pipelineIterationInfo;

		// Token: 0x040000B2 RID: 178
		private CommandOrigin _commandOrigin;

		// Token: 0x040000B3 RID: 179
		private Dictionary<string, object> _boundParameters;

		// Token: 0x040000B4 RID: 180
		private List<object> _unboundArguments;

		// Token: 0x040000B5 RID: 181
		private IScriptExtent _displayScriptPosition;
	}
}
