using System;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.Management.Automation;
using System.Management.Automation.Internal;

namespace Microsoft.PowerShell.Commands.Internal.Format
{
	// Token: 0x02000500 RID: 1280
	internal class OutCommandInner : ImplementationCommandBase
	{
		// Token: 0x06003690 RID: 13968 RVA: 0x0012733C File Offset: 0x0012553C
		internal override void BeginProcessing()
		{
			base.BeginProcessing();
			this.formatObjectDeserializer = new FormatObjectDeserializer(base.TerminatingErrorContext);
			this.ctxManager.contextCreation = new FormatMessagesContextManager.FormatContextCreationCallback(this.CreateOutputContext);
			this.ctxManager.fs = new FormatMessagesContextManager.FormatStartCallback(this.ProcessFormatStart);
			this.ctxManager.fe = new FormatMessagesContextManager.FormatEndCallback(this.ProcessFormatEnd);
			this.ctxManager.gs = new FormatMessagesContextManager.GroupStartCallback(this.ProcessGroupStart);
			this.ctxManager.ge = new FormatMessagesContextManager.GroupEndCallback(this.ProcessGroupEnd);
			this.ctxManager.payload = new FormatMessagesContextManager.PayloadCallback(this.ProcessPayload);
		}

		// Token: 0x06003691 RID: 13969 RVA: 0x001273EC File Offset: 0x001255EC
		internal override void ProcessRecord()
		{
			PSObject psobject = this.ReadObject();
			if (psobject == null || psobject == AutomationNull.Value)
			{
				return;
			}
			if (this.ProcessObject(psobject))
			{
				return;
			}
			Array array = this.ApplyFormatting(psobject);
			if (array != null)
			{
				foreach (object obj in array)
				{
					PSObject psobject2 = PSObjectHelper.AsPSObject(obj);
					psobject2.IsHelpObject = psobject.IsHelpObject;
					this.ProcessObject(psobject2);
				}
			}
		}

		// Token: 0x06003692 RID: 13970 RVA: 0x00127480 File Offset: 0x00125680
		internal override void EndProcessing()
		{
			base.EndProcessing();
			if (this.command != null)
			{
				Array array = this.command.ShutDown();
				if (array != null)
				{
					foreach (object obj in array)
					{
						this.ProcessObject(PSObjectHelper.AsPSObject(obj));
					}
				}
			}
			if (this.LineOutput.RequiresBuffering)
			{
				LineOutput.DoPlayBackCall playback = new LineOutput.DoPlayBackCall(this.DrainCache);
				this.LineOutput.ExecuteBufferPlayBack(playback);
				return;
			}
			this.DrainCache();
		}

		// Token: 0x06003693 RID: 13971 RVA: 0x00127524 File Offset: 0x00125724
		private void DrainCache()
		{
			if (this.cache != null)
			{
				List<PacketInfoData> list = this.cache.Drain();
				if (list != null)
				{
					foreach (object o in list)
					{
						this.ctxManager.Process(o);
					}
				}
			}
		}

		// Token: 0x06003694 RID: 13972 RVA: 0x00127590 File Offset: 0x00125790
		private bool ProcessObject(PSObject so)
		{
			object obj = this.formatObjectDeserializer.Deserialize(so);
			if (this.NeedsPreprocessing(obj))
			{
				return false;
			}
			if (this.cache == null)
			{
				this.cache = new FormattedObjectsCache(this.LineOutput.RequiresBuffering);
			}
			FormatStartData formatStartData = obj as FormatStartData;
			if (formatStartData != null)
			{
				if (formatStartData.autosizeInfo != null)
				{
					FormattedObjectsCache.ProcessCachedGroupNotification callBack = new FormattedObjectsCache.ProcessCachedGroupNotification(this.ProcessCachedGroup);
					this.cache.EnableGroupCaching(callBack, formatStartData.autosizeInfo.objectCount);
				}
				else
				{
					TableHeaderInfo tableHeaderInfo = formatStartData.shapeInfo as TableHeaderInfo;
					if (tableHeaderInfo != null && tableHeaderInfo.tableColumnInfoList.Count > 0 && tableHeaderInfo.tableColumnInfoList[0].width == 0)
					{
						FormattedObjectsCache.ProcessCachedGroupNotification callBack2 = new FormattedObjectsCache.ProcessCachedGroupNotification(this.ProcessCachedGroup);
						this.cache.EnableGroupCaching(callBack2, TimeSpan.FromMilliseconds(300.0));
					}
				}
			}
			List<PacketInfoData> list = this.cache.Add((PacketInfoData)obj);
			if (list != null)
			{
				for (int i = 0; i < list.Count; i++)
				{
					this.ctxManager.Process(list[i]);
				}
			}
			return true;
		}

		// Token: 0x17000C18 RID: 3096
		// (get) Token: 0x06003695 RID: 13973 RVA: 0x001276AC File Offset: 0x001258AC
		private FormatShape ActiveFormattingShape
		{
			get
			{
				FormatShape result = FormatShape.Table;
				OutCommandInner.FormatOutputContext formatContext = this.FormatContext;
				if (formatContext == null || formatContext.Data.shapeInfo == null)
				{
					return result;
				}
				if (formatContext.Data.shapeInfo is TableHeaderInfo)
				{
					return FormatShape.Table;
				}
				if (formatContext.Data.shapeInfo is ListViewHeaderInfo)
				{
					return FormatShape.List;
				}
				if (formatContext.Data.shapeInfo is WideViewHeaderInfo)
				{
					return FormatShape.Wide;
				}
				if (formatContext.Data.shapeInfo is ComplexViewHeaderInfo)
				{
					return FormatShape.Complex;
				}
				return result;
			}
		}

		// Token: 0x06003696 RID: 13974 RVA: 0x00127725 File Offset: 0x00125925
		protected override void InternalDispose()
		{
			base.InternalDispose();
			if (this.command != null)
			{
				this.command.Dispose();
				this.command = null;
			}
		}

		// Token: 0x06003697 RID: 13975 RVA: 0x00127748 File Offset: 0x00125948
		private bool NeedsPreprocessing(object o)
		{
			FormatEntryData formatEntryData = o as FormatEntryData;
			if (formatEntryData != null)
			{
				if (!formatEntryData.outOfBand)
				{
					this.ValidateCurrentFormattingState(OutCommandInner.FormattingState.InsideGroup, o);
				}
				return false;
			}
			if (o is FormatStartData)
			{
				if (this.currentFormattingState == OutCommandInner.FormattingState.InsideGroup)
				{
					this.EndProcessing();
					this.BeginProcessing();
				}
				this.ValidateCurrentFormattingState(OutCommandInner.FormattingState.Reset, o);
				this.currentFormattingState = OutCommandInner.FormattingState.Formatting;
				return false;
			}
			if (o is FormatEndData)
			{
				this.ValidateCurrentFormattingState(OutCommandInner.FormattingState.Formatting, o);
				this.currentFormattingState = OutCommandInner.FormattingState.Reset;
				return false;
			}
			if (o is GroupStartData)
			{
				this.ValidateCurrentFormattingState(OutCommandInner.FormattingState.Formatting, o);
				this.currentFormattingState = OutCommandInner.FormattingState.InsideGroup;
				return false;
			}
			if (o is GroupEndData)
			{
				this.ValidateCurrentFormattingState(OutCommandInner.FormattingState.InsideGroup, o);
				this.currentFormattingState = OutCommandInner.FormattingState.Formatting;
				return false;
			}
			return true;
		}

		// Token: 0x06003698 RID: 13976 RVA: 0x001277EC File Offset: 0x001259EC
		private void ValidateCurrentFormattingState(OutCommandInner.FormattingState expectedFormattingState, object obj)
		{
			if (this.currentFormattingState != expectedFormattingState)
			{
				string o = "format-*";
				StartData startData = obj as StartData;
				if (startData != null)
				{
					if (startData.shapeInfo.GetType() == typeof(WideViewHeaderInfo))
					{
						o = "format-wide";
					}
					else if (startData.shapeInfo.GetType() == typeof(TableHeaderInfo))
					{
						o = "format-table";
					}
					else if (startData.shapeInfo.GetType() == typeof(ListViewHeaderInfo))
					{
						o = "format-list";
					}
					else if (startData.shapeInfo.GetType() == typeof(ComplexViewHeaderInfo))
					{
						o = "format-complex";
					}
				}
				string message = StringUtil.Format(FormatAndOut_out_xxx.OutLineOutput_OutOfSequencePacket, obj.GetType().FullName, o);
				ErrorRecord errorRecord = new ErrorRecord(new InvalidOperationException(), "ConsoleLineOutputOutOfSequencePacket", ErrorCategory.InvalidData, null);
				errorRecord.ErrorDetails = new ErrorDetails(message);
				base.TerminatingErrorContext.ThrowTerminatingError(errorRecord);
			}
		}

		// Token: 0x06003699 RID: 13977 RVA: 0x001278E8 File Offset: 0x00125AE8
		private Array ApplyFormatting(object o)
		{
			if (this.command == null)
			{
				this.command = new CommandWrapper();
				this.command.Initialize(this.OuterCmdlet().Context, "format-default", typeof(FormatDefaultCommand));
			}
			return this.command.Process(o);
		}

		// Token: 0x0600369A RID: 13978 RVA: 0x0012793C File Offset: 0x00125B3C
		private FormatMessagesContextManager.OutputContext CreateOutputContext(FormatMessagesContextManager.OutputContext parentContext, FormatInfoData formatInfoData)
		{
			FormatStartData formatStartData = formatInfoData as FormatStartData;
			if (formatStartData != null)
			{
				return new OutCommandInner.FormatOutputContext(parentContext, formatStartData);
			}
			GroupStartData groupStartData = formatInfoData as GroupStartData;
			if (groupStartData != null)
			{
				OutCommandInner.GroupOutputContext groupOutputContext = null;
				switch (this.ActiveFormattingShape)
				{
				case FormatShape.Table:
					groupOutputContext = new OutCommandInner.TableOutputContext(this, parentContext, groupStartData);
					break;
				case FormatShape.List:
					groupOutputContext = new OutCommandInner.ListOutputContext(this, parentContext, groupStartData);
					break;
				case FormatShape.Wide:
					groupOutputContext = new OutCommandInner.WideOutputContext(this, parentContext, groupStartData);
					break;
				case FormatShape.Complex:
					groupOutputContext = new OutCommandInner.ComplexOutputContext(this, parentContext, groupStartData);
					break;
				}
				groupOutputContext.Initialize();
				return groupOutputContext;
			}
			return null;
		}

		// Token: 0x0600369B RID: 13979 RVA: 0x001279BD File Offset: 0x00125BBD
		private void ProcessFormatStart(FormatMessagesContextManager.OutputContext c)
		{
			this.LineOutput.WriteLine("");
		}

		// Token: 0x0600369C RID: 13980 RVA: 0x001279CF File Offset: 0x00125BCF
		private void ProcessFormatEnd(FormatEndData fe, FormatMessagesContextManager.OutputContext c)
		{
			this.LineOutput.WriteLine("");
		}

		// Token: 0x0600369D RID: 13981 RVA: 0x001279E4 File Offset: 0x00125BE4
		private void ProcessGroupStart(FormatMessagesContextManager.OutputContext c)
		{
			OutCommandInner.GroupOutputContext groupOutputContext = (OutCommandInner.GroupOutputContext)c;
			if (groupOutputContext.Data.groupingEntry != null)
			{
				this.lo.WriteLine("");
				ComplexWriter complexWriter = new ComplexWriter();
				complexWriter.Initialize(this.lo, this.lo.ColumnNumber);
				complexWriter.WriteObject(groupOutputContext.Data.groupingEntry.formatValueList);
				this.LineOutput.WriteLine("");
			}
			groupOutputContext.GroupStart();
		}

		// Token: 0x0600369E RID: 13982 RVA: 0x00127A60 File Offset: 0x00125C60
		private void ProcessGroupEnd(GroupEndData ge, FormatMessagesContextManager.OutputContext c)
		{
			OutCommandInner.GroupOutputContext groupOutputContext = (OutCommandInner.GroupOutputContext)c;
			groupOutputContext.GroupEnd();
			this.LineOutput.WriteLine("");
		}

		// Token: 0x0600369F RID: 13983 RVA: 0x00127A8C File Offset: 0x00125C8C
		private void ProcessPayload(FormatEntryData fed, FormatMessagesContextManager.OutputContext c)
		{
			if (fed == null)
			{
				PSTraceSource.NewArgumentNullException("fed");
			}
			if (fed.formatEntryInfo == null)
			{
				PSTraceSource.NewArgumentNullException("fed.formatEntryInfo");
			}
			WriteStreamType writeStream = this.lo.WriteStream;
			try
			{
				this.lo.WriteStream = fed.writeStream;
				if (c == null)
				{
					this.ProcessOutOfBandPayload(fed);
				}
				else
				{
					OutCommandInner.GroupOutputContext groupOutputContext = (OutCommandInner.GroupOutputContext)c;
					groupOutputContext.ProcessPayload(fed);
				}
			}
			finally
			{
				this.lo.WriteStream = writeStream;
			}
		}

		// Token: 0x060036A0 RID: 13984 RVA: 0x00127B10 File Offset: 0x00125D10
		private void ProcessOutOfBandPayload(FormatEntryData fed)
		{
			RawTextFormatEntry rawTextFormatEntry = fed.formatEntryInfo as RawTextFormatEntry;
			if (rawTextFormatEntry != null)
			{
				if (fed.isHelpObject)
				{
					ComplexWriter complexWriter = new ComplexWriter();
					complexWriter.Initialize(this.lo, this.lo.ColumnNumber);
					complexWriter.WriteString(rawTextFormatEntry.text);
					return;
				}
				this.lo.WriteLine(rawTextFormatEntry.text);
				return;
			}
			else
			{
				ComplexViewEntry complexViewEntry = fed.formatEntryInfo as ComplexViewEntry;
				if (complexViewEntry != null && complexViewEntry.formatValueList != null)
				{
					ComplexWriter complexWriter2 = new ComplexWriter();
					complexWriter2.Initialize(this.lo, this.lo.ColumnNumber);
					complexWriter2.WriteObject(complexViewEntry.formatValueList);
					return;
				}
				ListViewEntry listViewEntry = fed.formatEntryInfo as ListViewEntry;
				if (listViewEntry != null && listViewEntry.listViewFieldList != null)
				{
					ListWriter listWriter = new ListWriter();
					this.lo.WriteLine("");
					string[] properties = OutCommandInner.ListOutputContext.GetProperties(listViewEntry);
					listWriter.Initialize(properties, this.lo.ColumnNumber, this.lo.DisplayCells);
					string[] values = OutCommandInner.ListOutputContext.GetValues(listViewEntry);
					listWriter.WriteProperties(values, this.lo);
					this.lo.WriteLine("");
				}
				return;
			}
		}

		// Token: 0x17000C19 RID: 3097
		// (get) Token: 0x060036A2 RID: 13986 RVA: 0x00127C3B File Offset: 0x00125E3B
		// (set) Token: 0x060036A1 RID: 13985 RVA: 0x00127C32 File Offset: 0x00125E32
		internal LineOutput LineOutput
		{
			get
			{
				return this.lo;
			}
			set
			{
				this.lo = value;
			}
		}

		// Token: 0x17000C1A RID: 3098
		// (get) Token: 0x060036A3 RID: 13987 RVA: 0x00127C44 File Offset: 0x00125E44
		private ShapeInfo ShapeInfoOnFormatContext
		{
			get
			{
				OutCommandInner.FormatOutputContext formatContext = this.FormatContext;
				if (formatContext == null)
				{
					return null;
				}
				return formatContext.Data.shapeInfo;
			}
		}

		// Token: 0x17000C1B RID: 3099
		// (get) Token: 0x060036A4 RID: 13988 RVA: 0x00127C68 File Offset: 0x00125E68
		private OutCommandInner.FormatOutputContext FormatContext
		{
			get
			{
				for (FormatMessagesContextManager.OutputContext outputContext = this.ctxManager.ActiveOutputContext; outputContext != null; outputContext = outputContext.ParentContext)
				{
					OutCommandInner.FormatOutputContext formatOutputContext = outputContext as OutCommandInner.FormatOutputContext;
					if (formatOutputContext != null)
					{
						return formatOutputContext;
					}
				}
				return null;
			}
		}

		// Token: 0x060036A5 RID: 13989 RVA: 0x00127C9C File Offset: 0x00125E9C
		private void ProcessCachedGroup(FormatStartData formatStartData, List<PacketInfoData> objects)
		{
			this.formattingHint = null;
			TableHeaderInfo tableHeaderInfo = formatStartData.shapeInfo as TableHeaderInfo;
			if (tableHeaderInfo != null)
			{
				this.ProcessCachedGroupOnTable(tableHeaderInfo, objects);
				return;
			}
			WideViewHeaderInfo wideViewHeaderInfo = formatStartData.shapeInfo as WideViewHeaderInfo;
			if (wideViewHeaderInfo != null)
			{
				this.ProcessCachedGroupOnWide(wideViewHeaderInfo, objects);
			}
		}

		// Token: 0x060036A6 RID: 13990 RVA: 0x00127CE0 File Offset: 0x00125EE0
		private void ProcessCachedGroupOnTable(TableHeaderInfo thi, List<PacketInfoData> objects)
		{
			if (thi.tableColumnInfoList.Count == 0)
			{
				return;
			}
			int[] array = new int[thi.tableColumnInfoList.Count];
			for (int i = 0; i < thi.tableColumnInfoList.Count; i++)
			{
				string text = thi.tableColumnInfoList[i].label;
				if (string.IsNullOrEmpty(text))
				{
					text = thi.tableColumnInfoList[i].propertyName;
				}
				if (string.IsNullOrEmpty(text))
				{
					array[i] = 0;
				}
				else
				{
					array[i] = this.lo.DisplayCells.Length(text);
				}
			}
			foreach (PacketInfoData packetInfoData in objects)
			{
				FormatEntryData formatEntryData = packetInfoData as FormatEntryData;
				if (formatEntryData != null)
				{
					TableRowEntry tableRowEntry = formatEntryData.formatEntryInfo as TableRowEntry;
					int num = 0;
					foreach (FormatPropertyField formatPropertyField in tableRowEntry.formatPropertyFieldList)
					{
						int num2 = this.lo.DisplayCells.Length(formatPropertyField.propertyValue);
						if (array[num] < num2)
						{
							array[num] = num2;
						}
						num++;
					}
				}
			}
			this.formattingHint = new OutCommandInner.TableFormattingHint
			{
				columnWidths = array
			};
		}

		// Token: 0x060036A7 RID: 13991 RVA: 0x00127E50 File Offset: 0x00126050
		private void ProcessCachedGroupOnWide(WideViewHeaderInfo wvhi, List<PacketInfoData> objects)
		{
			if (wvhi.columns != 0)
			{
				return;
			}
			int num = 0;
			foreach (PacketInfoData packetInfoData in objects)
			{
				FormatEntryData formatEntryData = packetInfoData as FormatEntryData;
				if (formatEntryData != null)
				{
					WideViewEntry wideViewEntry = formatEntryData.formatEntryInfo as WideViewEntry;
					FormatPropertyField formatPropertyField = wideViewEntry.formatPropertyField;
					if (!string.IsNullOrEmpty(formatPropertyField.propertyValue))
					{
						int num2 = this.lo.DisplayCells.Length(formatPropertyField.propertyValue);
						if (num2 > num)
						{
							num = num2;
						}
					}
				}
			}
			this.formattingHint = new OutCommandInner.WideFormattingHint
			{
				maxWidth = num
			};
		}

		// Token: 0x060036A8 RID: 13992 RVA: 0x00127F08 File Offset: 0x00126108
		private OutCommandInner.FormattingHint RetrieveFormattingHint()
		{
			OutCommandInner.FormattingHint result = this.formattingHint;
			this.formattingHint = null;
			return result;
		}

		// Token: 0x04001BF2 RID: 7154
		[TraceSource("format_out_OutCommandInner", "OutCommandInner")]
		internal static PSTraceSource tracer = PSTraceSource.GetTracer("format_out_OutCommandInner", "OutCommandInner");

		// Token: 0x04001BF3 RID: 7155
		private OutCommandInner.FormattingState currentFormattingState;

		// Token: 0x04001BF4 RID: 7156
		private CommandWrapper command;

		// Token: 0x04001BF5 RID: 7157
		private LineOutput lo;

		// Token: 0x04001BF6 RID: 7158
		private FormatMessagesContextManager ctxManager = new FormatMessagesContextManager();

		// Token: 0x04001BF7 RID: 7159
		private FormattedObjectsCache cache;

		// Token: 0x04001BF8 RID: 7160
		private OutCommandInner.FormattingHint formattingHint;

		// Token: 0x04001BF9 RID: 7161
		private FormatObjectDeserializer formatObjectDeserializer;

		// Token: 0x02000501 RID: 1281
		private enum FormattingState
		{
			// Token: 0x04001BFB RID: 7163
			Reset,
			// Token: 0x04001BFC RID: 7164
			Formatting,
			// Token: 0x04001BFD RID: 7165
			InsideGroup
		}

		// Token: 0x02000502 RID: 1282
		private enum PreprocessingState
		{
			// Token: 0x04001BFF RID: 7167
			raw,
			// Token: 0x04001C00 RID: 7168
			processed,
			// Token: 0x04001C01 RID: 7169
			error
		}

		// Token: 0x02000503 RID: 1283
		private abstract class FormattingHint
		{
		}

		// Token: 0x02000504 RID: 1284
		private sealed class TableFormattingHint : OutCommandInner.FormattingHint
		{
			// Token: 0x04001C02 RID: 7170
			internal int[] columnWidths;
		}

		// Token: 0x02000505 RID: 1285
		private sealed class WideFormattingHint : OutCommandInner.FormattingHint
		{
			// Token: 0x04001C03 RID: 7171
			internal int maxWidth;
		}

		// Token: 0x02000506 RID: 1286
		private class FormatOutputContext : FormatMessagesContextManager.OutputContext
		{
			// Token: 0x060036AE RID: 13998 RVA: 0x00127F65 File Offset: 0x00126165
			internal FormatOutputContext(FormatMessagesContextManager.OutputContext parentContext, FormatStartData formatData) : base(parentContext)
			{
				this.formatData = formatData;
			}

			// Token: 0x17000C1C RID: 3100
			// (get) Token: 0x060036AF RID: 13999 RVA: 0x00127F75 File Offset: 0x00126175
			internal FormatStartData Data
			{
				get
				{
					return this.formatData;
				}
			}

			// Token: 0x04001C04 RID: 7172
			private FormatStartData formatData;
		}

		// Token: 0x02000507 RID: 1287
		private abstract class GroupOutputContext : FormatMessagesContextManager.OutputContext
		{
			// Token: 0x060036B0 RID: 14000 RVA: 0x00127F7D File Offset: 0x0012617D
			internal GroupOutputContext(OutCommandInner cmd, FormatMessagesContextManager.OutputContext parentContext, GroupStartData formatData) : base(parentContext)
			{
				this.cmd = cmd;
				this.formatData = formatData;
			}

			// Token: 0x060036B1 RID: 14001 RVA: 0x00127F94 File Offset: 0x00126194
			internal virtual void Initialize()
			{
			}

			// Token: 0x060036B2 RID: 14002 RVA: 0x00127F96 File Offset: 0x00126196
			internal virtual void GroupStart()
			{
			}

			// Token: 0x060036B3 RID: 14003 RVA: 0x00127F98 File Offset: 0x00126198
			internal virtual void GroupEnd()
			{
			}

			// Token: 0x060036B4 RID: 14004 RVA: 0x00127F9A File Offset: 0x0012619A
			internal virtual void ProcessPayload(FormatEntryData fed)
			{
			}

			// Token: 0x17000C1D RID: 3101
			// (get) Token: 0x060036B5 RID: 14005 RVA: 0x00127F9C File Offset: 0x0012619C
			internal GroupStartData Data
			{
				get
				{
					return this.formatData;
				}
			}

			// Token: 0x17000C1E RID: 3102
			// (get) Token: 0x060036B6 RID: 14006 RVA: 0x00127FA4 File Offset: 0x001261A4
			protected OutCommandInner InnerCommand
			{
				get
				{
					return this.cmd;
				}
			}

			// Token: 0x04001C05 RID: 7173
			private OutCommandInner cmd;

			// Token: 0x04001C06 RID: 7174
			private GroupStartData formatData;
		}

		// Token: 0x02000508 RID: 1288
		private class TableOutputContextBase : OutCommandInner.GroupOutputContext
		{
			// Token: 0x060036B7 RID: 14007 RVA: 0x00127FAC File Offset: 0x001261AC
			internal TableOutputContextBase(OutCommandInner cmd, FormatMessagesContextManager.OutputContext parentContext, GroupStartData formatData) : base(cmd, parentContext, formatData)
			{
			}

			// Token: 0x17000C1F RID: 3103
			// (get) Token: 0x060036B8 RID: 14008 RVA: 0x00127FC2 File Offset: 0x001261C2
			protected TableWriter Writer
			{
				get
				{
					return this.tableWriter;
				}
			}

			// Token: 0x04001C07 RID: 7175
			private TableWriter tableWriter = new TableWriter();
		}

		// Token: 0x02000509 RID: 1289
		private sealed class TableOutputContext : OutCommandInner.TableOutputContextBase
		{
			// Token: 0x060036B9 RID: 14009 RVA: 0x00127FCA File Offset: 0x001261CA
			internal TableOutputContext(OutCommandInner cmd, FormatMessagesContextManager.OutputContext parentContext, GroupStartData formatData) : base(cmd, parentContext, formatData)
			{
			}

			// Token: 0x060036BA RID: 14010 RVA: 0x00127FD8 File Offset: 0x001261D8
			internal override void Initialize()
			{
				OutCommandInner.TableFormattingHint tableFormattingHint = base.InnerCommand.RetrieveFormattingHint() as OutCommandInner.TableFormattingHint;
				int[] array = null;
				if (tableFormattingHint != null)
				{
					array = tableFormattingHint.columnWidths;
				}
				int columnNumber = base.InnerCommand.lo.ColumnNumber;
				int count = this.CurrentTableHeaderInfo.tableColumnInfoList.Count;
				if (count == 0)
				{
					return;
				}
				int[] array2 = new int[count];
				int[] array3 = new int[count];
				int num = 0;
				foreach (TableColumnInfo tableColumnInfo in this.CurrentTableHeaderInfo.tableColumnInfoList)
				{
					array2[num] = ((array != null) ? array[num] : tableColumnInfo.width);
					array3[num] = tableColumnInfo.alignment;
					num++;
				}
				base.Writer.Initialize(0, columnNumber, array2, array3, this.CurrentTableHeaderInfo.hideHeader);
			}

			// Token: 0x060036BB RID: 14011 RVA: 0x001280C4 File Offset: 0x001262C4
			internal override void GroupStart()
			{
				int count = this.CurrentTableHeaderInfo.tableColumnInfoList.Count;
				if (count == 0)
				{
					return;
				}
				string[] array = new string[count];
				int num = 0;
				foreach (TableColumnInfo tableColumnInfo in this.CurrentTableHeaderInfo.tableColumnInfoList)
				{
					array[num++] = ((tableColumnInfo.label != null) ? tableColumnInfo.label : tableColumnInfo.propertyName);
				}
				base.Writer.GenerateHeader(array, base.InnerCommand.lo);
			}

			// Token: 0x060036BC RID: 14012 RVA: 0x00128168 File Offset: 0x00126368
			internal override void ProcessPayload(FormatEntryData fed)
			{
				int count = this.CurrentTableHeaderInfo.tableColumnInfoList.Count;
				if (count == 0)
				{
					return;
				}
				TableRowEntry tableRowEntry = fed.formatEntryInfo as TableRowEntry;
				string[] array = new string[count];
				int[] array2 = new int[count];
				int count2 = tableRowEntry.formatPropertyFieldList.Count;
				for (int i = 0; i < count; i++)
				{
					if (i < count2)
					{
						array[i] = tableRowEntry.formatPropertyFieldList[i].propertyValue;
						array2[i] = tableRowEntry.formatPropertyFieldList[i].alignment;
					}
					else
					{
						array[i] = "";
						array2[i] = 1;
					}
				}
				base.Writer.GenerateRow(array, base.InnerCommand.lo, tableRowEntry.multiLine, array2, base.InnerCommand.lo.DisplayCells);
			}

			// Token: 0x17000C20 RID: 3104
			// (get) Token: 0x060036BD RID: 14013 RVA: 0x00128231 File Offset: 0x00126431
			private TableHeaderInfo CurrentTableHeaderInfo
			{
				get
				{
					return (TableHeaderInfo)base.InnerCommand.ShapeInfoOnFormatContext;
				}
			}
		}

		// Token: 0x0200050A RID: 1290
		private sealed class ListOutputContext : OutCommandInner.GroupOutputContext
		{
			// Token: 0x060036BE RID: 14014 RVA: 0x00128243 File Offset: 0x00126443
			internal ListOutputContext(OutCommandInner cmd, FormatMessagesContextManager.OutputContext parentContext, GroupStartData formatData) : base(cmd, parentContext, formatData)
			{
			}

			// Token: 0x060036BF RID: 14015 RVA: 0x00128259 File Offset: 0x00126459
			internal override void Initialize()
			{
			}

			// Token: 0x060036C0 RID: 14016 RVA: 0x0012825B File Offset: 0x0012645B
			private void InternalInitialize(ListViewEntry lve)
			{
				this.properties = OutCommandInner.ListOutputContext.GetProperties(lve);
				this.listWriter.Initialize(this.properties, base.InnerCommand.lo.ColumnNumber, base.InnerCommand.lo.DisplayCells);
			}

			// Token: 0x060036C1 RID: 14017 RVA: 0x0012829C File Offset: 0x0012649C
			internal static string[] GetProperties(ListViewEntry lve)
			{
				StringCollection stringCollection = new StringCollection();
				foreach (ListViewField listViewField in lve.listViewFieldList)
				{
					stringCollection.Add((listViewField.label != null) ? listViewField.label : listViewField.propertyName);
				}
				if (stringCollection.Count == 0)
				{
					return null;
				}
				string[] array = new string[stringCollection.Count];
				stringCollection.CopyTo(array, 0);
				return array;
			}

			// Token: 0x060036C2 RID: 14018 RVA: 0x0012832C File Offset: 0x0012652C
			internal static string[] GetValues(ListViewEntry lve)
			{
				StringCollection stringCollection = new StringCollection();
				foreach (ListViewField listViewField in lve.listViewFieldList)
				{
					stringCollection.Add(listViewField.formatPropertyField.propertyValue);
				}
				if (stringCollection.Count == 0)
				{
					return null;
				}
				string[] array = new string[stringCollection.Count];
				stringCollection.CopyTo(array, 0);
				return array;
			}

			// Token: 0x060036C3 RID: 14019 RVA: 0x001283B0 File Offset: 0x001265B0
			internal override void GroupStart()
			{
				base.InnerCommand.lo.WriteLine("");
			}

			// Token: 0x060036C4 RID: 14020 RVA: 0x001283C8 File Offset: 0x001265C8
			internal override void ProcessPayload(FormatEntryData fed)
			{
				ListViewEntry lve = fed.formatEntryInfo as ListViewEntry;
				this.InternalInitialize(lve);
				string[] values = OutCommandInner.ListOutputContext.GetValues(lve);
				this.listWriter.WriteProperties(values, base.InnerCommand.lo);
				base.InnerCommand.lo.WriteLine("");
			}

			// Token: 0x04001C08 RID: 7176
			private string[] properties;

			// Token: 0x04001C09 RID: 7177
			private ListWriter listWriter = new ListWriter();
		}

		// Token: 0x0200050B RID: 1291
		private sealed class WideOutputContext : OutCommandInner.TableOutputContextBase
		{
			// Token: 0x060036C5 RID: 14021 RVA: 0x0012841B File Offset: 0x0012661B
			internal WideOutputContext(OutCommandInner cmd, FormatMessagesContextManager.OutputContext parentContext, GroupStartData formatData) : base(cmd, parentContext, formatData)
			{
			}

			// Token: 0x060036C6 RID: 14022 RVA: 0x00128428 File Offset: 0x00126628
			internal override void Initialize()
			{
				int num = 2;
				OutCommandInner.WideFormattingHint wideFormattingHint = base.InnerCommand.RetrieveFormattingHint() as OutCommandInner.WideFormattingHint;
				if (wideFormattingHint != null && wideFormattingHint.maxWidth > 0)
				{
					num = TableWriter.ComputeWideViewBestItemsPerRowFit(wideFormattingHint.maxWidth, base.InnerCommand.lo.ColumnNumber);
				}
				else if (this.CurrentWideHeaderInfo.columns > 0)
				{
					num = this.CurrentWideHeaderInfo.columns;
				}
				this.buffer = new OutCommandInner.WideOutputContext.StringValuesBuffer(num);
				int[] array = new int[num];
				int[] array2 = new int[num];
				for (int i = 0; i < num; i++)
				{
					array[i] = 0;
					array2[i] = 1;
				}
				base.Writer.Initialize(0, base.InnerCommand.lo.ColumnNumber, array, array2, false);
			}

			// Token: 0x060036C7 RID: 14023 RVA: 0x001284DF File Offset: 0x001266DF
			internal override void GroupStart()
			{
				base.InnerCommand.lo.WriteLine("");
			}

			// Token: 0x060036C8 RID: 14024 RVA: 0x001284F6 File Offset: 0x001266F6
			internal override void GroupEnd()
			{
				this.WriteStringBuffer();
			}

			// Token: 0x060036C9 RID: 14025 RVA: 0x00128500 File Offset: 0x00126700
			internal override void ProcessPayload(FormatEntryData fed)
			{
				WideViewEntry wideViewEntry = fed.formatEntryInfo as WideViewEntry;
				FormatPropertyField formatPropertyField = wideViewEntry.formatPropertyField;
				this.buffer.Add(formatPropertyField.propertyValue);
				if (this.buffer.IsFull)
				{
					this.WriteStringBuffer();
				}
			}

			// Token: 0x17000C21 RID: 3105
			// (get) Token: 0x060036CA RID: 14026 RVA: 0x00128544 File Offset: 0x00126744
			private WideViewHeaderInfo CurrentWideHeaderInfo
			{
				get
				{
					return (WideViewHeaderInfo)base.InnerCommand.ShapeInfoOnFormatContext;
				}
			}

			// Token: 0x060036CB RID: 14027 RVA: 0x00128558 File Offset: 0x00126758
			private void WriteStringBuffer()
			{
				if (this.buffer.IsEmpty)
				{
					return;
				}
				string[] array = new string[this.buffer.Lenght];
				for (int i = 0; i < array.Length; i++)
				{
					if (i < this.buffer.CurrentCount)
					{
						array[i] = this.buffer[i];
					}
					else
					{
						array[i] = "";
					}
				}
				base.Writer.GenerateRow(array, base.InnerCommand.lo, false, null, base.InnerCommand.lo.DisplayCells);
				this.buffer.Reset();
			}

			// Token: 0x04001C0A RID: 7178
			private OutCommandInner.WideOutputContext.StringValuesBuffer buffer;

			// Token: 0x0200050C RID: 1292
			private class StringValuesBuffer
			{
				// Token: 0x060036CC RID: 14028 RVA: 0x001285ED File Offset: 0x001267ED
				internal StringValuesBuffer(int size)
				{
					this.arr = new string[size];
					this.Reset();
				}

				// Token: 0x17000C22 RID: 3106
				// (get) Token: 0x060036CD RID: 14029 RVA: 0x00128607 File Offset: 0x00126807
				internal int Lenght
				{
					get
					{
						return this.arr.Length;
					}
				}

				// Token: 0x17000C23 RID: 3107
				// (get) Token: 0x060036CE RID: 14030 RVA: 0x00128611 File Offset: 0x00126811
				internal int CurrentCount
				{
					get
					{
						return this.lastEmptySpot;
					}
				}

				// Token: 0x17000C24 RID: 3108
				// (get) Token: 0x060036CF RID: 14031 RVA: 0x00128619 File Offset: 0x00126819
				internal bool IsFull
				{
					get
					{
						return this.lastEmptySpot == this.arr.Length;
					}
				}

				// Token: 0x17000C25 RID: 3109
				// (get) Token: 0x060036D0 RID: 14032 RVA: 0x0012862B File Offset: 0x0012682B
				internal bool IsEmpty
				{
					get
					{
						return this.lastEmptySpot == 0;
					}
				}

				// Token: 0x17000C26 RID: 3110
				internal string this[int k]
				{
					get
					{
						return this.arr[k];
					}
				}

				// Token: 0x060036D2 RID: 14034 RVA: 0x00128640 File Offset: 0x00126840
				internal void Add(string s)
				{
					this.arr[this.lastEmptySpot++] = s;
				}

				// Token: 0x060036D3 RID: 14035 RVA: 0x00128668 File Offset: 0x00126868
				internal void Reset()
				{
					this.lastEmptySpot = 0;
					for (int i = 0; i < this.arr.Length; i++)
					{
						this.arr[i] = null;
					}
				}

				// Token: 0x04001C0B RID: 7179
				private string[] arr;

				// Token: 0x04001C0C RID: 7180
				private int lastEmptySpot;
			}
		}

		// Token: 0x0200050D RID: 1293
		private sealed class ComplexOutputContext : OutCommandInner.GroupOutputContext
		{
			// Token: 0x060036D4 RID: 14036 RVA: 0x00128698 File Offset: 0x00126898
			internal ComplexOutputContext(OutCommandInner cmd, FormatMessagesContextManager.OutputContext parentContext, GroupStartData formatData) : base(cmd, parentContext, formatData)
			{
			}

			// Token: 0x060036D5 RID: 14037 RVA: 0x001286AE File Offset: 0x001268AE
			internal override void Initialize()
			{
				this.writer.Initialize(base.InnerCommand.lo, base.InnerCommand.lo.ColumnNumber);
			}

			// Token: 0x060036D6 RID: 14038 RVA: 0x001286D8 File Offset: 0x001268D8
			internal override void ProcessPayload(FormatEntryData fed)
			{
				ComplexViewEntry complexViewEntry = fed.formatEntryInfo as ComplexViewEntry;
				if (complexViewEntry == null || complexViewEntry.formatValueList == null)
				{
					return;
				}
				this.writer.WriteObject(complexViewEntry.formatValueList);
			}

			// Token: 0x04001C0D RID: 7181
			private ComplexWriter writer = new ComplexWriter();
		}
	}
}
