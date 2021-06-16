using System;
using System.Collections;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Management.Automation.Runspaces;

namespace System.Management.Automation.Internal
{
	// Token: 0x0200075E RID: 1886
	internal class Pipe
	{
		// Token: 0x17000FBC RID: 4028
		// (get) Token: 0x06004B1C RID: 19228 RVA: 0x00189475 File Offset: 0x00187675
		internal PipelineProcessor PipelineProcessor
		{
			get
			{
				return this._outputPipeline;
			}
		}

		// Token: 0x17000FBD RID: 4029
		// (get) Token: 0x06004B1D RID: 19229 RVA: 0x0018947D File Offset: 0x0018767D
		// (set) Token: 0x06004B1E RID: 19230 RVA: 0x00189485 File Offset: 0x00187685
		internal CommandProcessorBase DownstreamCmdlet
		{
			get
			{
				return this._downstreamCmdlet;
			}
			set
			{
				this._downstreamCmdlet = value;
			}
		}

		// Token: 0x17000FBE RID: 4030
		// (get) Token: 0x06004B1F RID: 19231 RVA: 0x0018948E File Offset: 0x0018768E
		// (set) Token: 0x06004B20 RID: 19232 RVA: 0x00189496 File Offset: 0x00187696
		internal PipelineReader<object> ExternalReader
		{
			get
			{
				return this._objectReader;
			}
			set
			{
				this._objectReader = value;
			}
		}

		// Token: 0x17000FBF RID: 4031
		// (get) Token: 0x06004B21 RID: 19233 RVA: 0x0018949F File Offset: 0x0018769F
		// (set) Token: 0x06004B22 RID: 19234 RVA: 0x001894A7 File Offset: 0x001876A7
		internal PipelineWriter ExternalWriter
		{
			get
			{
				return this._externalWriter;
			}
			set
			{
				this._externalWriter = value;
			}
		}

		// Token: 0x06004B23 RID: 19235 RVA: 0x001894B0 File Offset: 0x001876B0
		public override string ToString()
		{
			if (this._downstreamCmdlet != null)
			{
				return this._downstreamCmdlet.ToString();
			}
			return base.ToString();
		}

		// Token: 0x17000FC0 RID: 4032
		// (get) Token: 0x06004B24 RID: 19236 RVA: 0x001894CC File Offset: 0x001876CC
		// (set) Token: 0x06004B25 RID: 19237 RVA: 0x001894D4 File Offset: 0x001876D4
		internal int OutBufferCount
		{
			get
			{
				return this._outBufferCount;
			}
			set
			{
				this._outBufferCount = value;
			}
		}

		// Token: 0x17000FC1 RID: 4033
		// (get) Token: 0x06004B26 RID: 19238 RVA: 0x001894DD File Offset: 0x001876DD
		// (set) Token: 0x06004B27 RID: 19239 RVA: 0x001894E5 File Offset: 0x001876E5
		internal bool NullPipe
		{
			get
			{
				return this._nullPipe;
			}
			set
			{
				this._isRedirected = true;
				this._nullPipe = value;
			}
		}

		// Token: 0x17000FC2 RID: 4034
		// (get) Token: 0x06004B28 RID: 19240 RVA: 0x001894F5 File Offset: 0x001876F5
		internal Queue<object> ObjectQueue
		{
			get
			{
				return this._objectQueue;
			}
		}

		// Token: 0x17000FC3 RID: 4035
		// (get) Token: 0x06004B29 RID: 19241 RVA: 0x001894FD File Offset: 0x001876FD
		internal bool Empty
		{
			get
			{
				if (this._enumeratorToProcess != null)
				{
					return this._enumeratorToProcessIsEmpty;
				}
				return this._objectQueue == null || this._objectQueue.Count == 0;
			}
		}

		// Token: 0x17000FC4 RID: 4036
		// (get) Token: 0x06004B2A RID: 19242 RVA: 0x00189526 File Offset: 0x00187726
		internal bool IsRedirected
		{
			get
			{
				return this._downstreamCmdlet != null || this._isRedirected;
			}
		}

		// Token: 0x06004B2B RID: 19243 RVA: 0x00189538 File Offset: 0x00187738
		private static void AddToVarList(List<IList> varList, object obj)
		{
			if (varList != null && varList.Count > 0)
			{
				for (int i = 0; i < varList.Count; i++)
				{
					varList[i].Add(obj);
				}
			}
		}

		// Token: 0x06004B2C RID: 19244 RVA: 0x00189570 File Offset: 0x00187770
		internal void AppendVariableList(VariableStreamKind kind, object obj)
		{
			switch (kind)
			{
			case VariableStreamKind.Output:
				Pipe.AddToVarList(this._outVariableList, obj);
				return;
			case VariableStreamKind.Error:
				Pipe.AddToVarList(this._errorVariableList, obj);
				return;
			case VariableStreamKind.Warning:
				Pipe.AddToVarList(this._warningVariableList, obj);
				return;
			case VariableStreamKind.Information:
				Pipe.AddToVarList(this._informationVariableList, obj);
				return;
			default:
				return;
			}
		}

		// Token: 0x06004B2D RID: 19245 RVA: 0x001895CC File Offset: 0x001877CC
		internal void AddVariableList(VariableStreamKind kind, IList list)
		{
			switch (kind)
			{
			case VariableStreamKind.Output:
				if (this._outVariableList == null)
				{
					this._outVariableList = new List<IList>();
				}
				this._outVariableList.Add(list);
				return;
			case VariableStreamKind.Error:
				if (this._errorVariableList == null)
				{
					this._errorVariableList = new List<IList>();
				}
				this._errorVariableList.Add(list);
				return;
			case VariableStreamKind.Warning:
				if (this._warningVariableList == null)
				{
					this._warningVariableList = new List<IList>();
				}
				this._warningVariableList.Add(list);
				return;
			case VariableStreamKind.Information:
				if (this._informationVariableList == null)
				{
					this._informationVariableList = new List<IList>();
				}
				this._informationVariableList.Add(list);
				return;
			default:
				return;
			}
		}

		// Token: 0x06004B2E RID: 19246 RVA: 0x00189671 File Offset: 0x00187871
		internal void SetPipelineVariable(PSVariable pipelineVariable)
		{
			this._pipelineVariableObject = pipelineVariable;
		}

		// Token: 0x06004B2F RID: 19247 RVA: 0x0018967C File Offset: 0x0018787C
		internal void RemoveVariableList(VariableStreamKind kind, IList list)
		{
			switch (kind)
			{
			case VariableStreamKind.Output:
				this._outVariableList.Remove(list);
				return;
			case VariableStreamKind.Error:
				this._errorVariableList.Remove(list);
				return;
			case VariableStreamKind.Warning:
				this._warningVariableList.Remove(list);
				return;
			case VariableStreamKind.Information:
				this._informationVariableList.Remove(list);
				return;
			default:
				return;
			}
		}

		// Token: 0x06004B30 RID: 19248 RVA: 0x001896D9 File Offset: 0x001878D9
		internal void RemovePipelineVariable()
		{
			if (this._pipelineVariableObject != null)
			{
				this._pipelineVariableObject.Value = null;
				this._pipelineVariableObject = null;
			}
		}

		// Token: 0x06004B31 RID: 19249 RVA: 0x001896F6 File Offset: 0x001878F6
		internal void SetVariableListForTemporaryPipe(Pipe tempPipe)
		{
			this.CopyVariableToTempPipe(VariableStreamKind.Error, this._errorVariableList, tempPipe);
			this.CopyVariableToTempPipe(VariableStreamKind.Warning, this._warningVariableList, tempPipe);
			this.CopyVariableToTempPipe(VariableStreamKind.Information, this._informationVariableList, tempPipe);
		}

		// Token: 0x06004B32 RID: 19250 RVA: 0x00189724 File Offset: 0x00187924
		private void CopyVariableToTempPipe(VariableStreamKind streamKind, List<IList> variableList, Pipe tempPipe)
		{
			if (variableList != null && variableList.Count > 0)
			{
				for (int i = 0; i < variableList.Count; i++)
				{
					tempPipe.AddVariableList(streamKind, variableList[i]);
				}
			}
		}

		// Token: 0x06004B33 RID: 19251 RVA: 0x0018975C File Offset: 0x0018795C
		internal Pipe()
		{
			this._objectQueue = new Queue<object>();
		}

		// Token: 0x06004B34 RID: 19252 RVA: 0x0018976F File Offset: 0x0018796F
		internal Pipe(List<object> resultList)
		{
			this._isRedirected = true;
			this._resultList = resultList;
		}

		// Token: 0x06004B35 RID: 19253 RVA: 0x00189785 File Offset: 0x00187985
		internal Pipe(Collection<PSObject> resultCollection)
		{
			this._isRedirected = true;
			this._resultCollection = resultCollection;
		}

		// Token: 0x06004B36 RID: 19254 RVA: 0x0018979B File Offset: 0x0018799B
		internal Pipe(ExecutionContext context, PipelineProcessor outputPipeline)
		{
			this._isRedirected = true;
			this._context = context;
			this._outputPipeline = outputPipeline;
		}

		// Token: 0x06004B37 RID: 19255 RVA: 0x001897B8 File Offset: 0x001879B8
		internal Pipe(IEnumerator enumeratorToProcess)
		{
			this._enumeratorToProcess = enumeratorToProcess;
			this._enumeratorToProcessIsEmpty = false;
		}

		// Token: 0x06004B38 RID: 19256 RVA: 0x001897CE File Offset: 0x001879CE
		internal void Add(object obj)
		{
			if (obj == AutomationNull.Value)
			{
				return;
			}
			Pipe.AddToVarList(this._outVariableList, obj);
			if (this._nullPipe)
			{
				return;
			}
			if (this._pipelineVariableObject != null)
			{
				this._pipelineVariableObject.Value = obj;
			}
			this.AddToPipe(obj);
		}

		// Token: 0x06004B39 RID: 19257 RVA: 0x00189809 File Offset: 0x00187A09
		internal void AddWithoutAppendingOutVarList(object obj)
		{
			if (obj == AutomationNull.Value || this._nullPipe)
			{
				return;
			}
			this.AddToPipe(obj);
		}

		// Token: 0x06004B3A RID: 19258 RVA: 0x00189824 File Offset: 0x00187A24
		private void AddToPipe(object obj)
		{
			if (this._outputPipeline != null)
			{
				this._context.PushPipelineProcessor(this._outputPipeline);
				this._outputPipeline.Step(obj);
				this._context.PopPipelineProcessor(false);
				return;
			}
			if (this._resultCollection != null)
			{
				this._resultCollection.Add((obj != null) ? PSObject.AsPSObject(obj) : null);
				return;
			}
			if (this._resultList != null)
			{
				this._resultList.Add(obj);
				return;
			}
			if (this._externalWriter != null)
			{
				this._externalWriter.Write(obj);
				return;
			}
			if (this._objectQueue != null)
			{
				this._objectQueue.Enqueue(obj);
				if (this._downstreamCmdlet != null && this._objectQueue.Count > this._outBufferCount)
				{
					this._downstreamCmdlet.DoExecute();
				}
			}
		}

		// Token: 0x06004B3B RID: 19259 RVA: 0x001898EC File Offset: 0x00187AEC
		internal void AddItems(object objects)
		{
			IEnumerator enumerator = LanguagePrimitives.GetEnumerator(objects);
			try
			{
				if (enumerator == null)
				{
					this.Add(objects);
				}
				else
				{
					while (ParserOps.MoveNext(this._context, null, enumerator))
					{
						object obj = ParserOps.Current(null, enumerator);
						if (obj != AutomationNull.Value)
						{
							this.Add(obj);
						}
					}
				}
			}
			finally
			{
				IDisposable disposable = enumerator as IDisposable;
				if (disposable != null && !(objects is IEnumerator))
				{
					disposable.Dispose();
				}
			}
			if (this._externalWriter != null)
			{
				return;
			}
			if (this._downstreamCmdlet != null && this._objectQueue != null && this._objectQueue.Count > this._outBufferCount)
			{
				this._downstreamCmdlet.DoExecute();
			}
		}

		// Token: 0x06004B3C RID: 19260 RVA: 0x00189994 File Offset: 0x00187B94
		internal object Retrieve()
		{
			if (this._objectQueue != null && this._objectQueue.Count != 0)
			{
				return this._objectQueue.Dequeue();
			}
			if (this._enumeratorToProcess == null)
			{
				if (this.ExternalReader != null)
				{
					try
					{
						object obj = this.ExternalReader.Read();
						if (AutomationNull.Value == obj)
						{
							this.ExternalReader = null;
						}
						return obj;
					}
					catch (PipelineClosedException)
					{
						return AutomationNull.Value;
					}
					catch (ObjectDisposedException)
					{
						return AutomationNull.Value;
					}
				}
				return AutomationNull.Value;
			}
			if (this._enumeratorToProcessIsEmpty)
			{
				return AutomationNull.Value;
			}
			if (!ParserOps.MoveNext(this._context, null, this._enumeratorToProcess))
			{
				this._enumeratorToProcessIsEmpty = true;
				return AutomationNull.Value;
			}
			return ParserOps.Current(null, this._enumeratorToProcess);
		}

		// Token: 0x06004B3D RID: 19261 RVA: 0x00189A64 File Offset: 0x00187C64
		internal void Clear()
		{
			if (this._objectQueue != null)
			{
				this._objectQueue.Clear();
			}
		}

		// Token: 0x06004B3E RID: 19262 RVA: 0x00189A79 File Offset: 0x00187C79
		internal object[] ToArray()
		{
			if (this._objectQueue == null || this._objectQueue.Count == 0)
			{
				return MshCommandRuntime.StaticEmptyArray;
			}
			return this._objectQueue.ToArray();
		}

		// Token: 0x0400244D RID: 9293
		private ExecutionContext _context;

		// Token: 0x0400244E RID: 9294
		private PipelineProcessor _outputPipeline;

		// Token: 0x0400244F RID: 9295
		private CommandProcessorBase _downstreamCmdlet;

		// Token: 0x04002450 RID: 9296
		private PipelineReader<object> _objectReader;

		// Token: 0x04002451 RID: 9297
		private PipelineWriter _externalWriter;

		// Token: 0x04002452 RID: 9298
		private int _outBufferCount;

		// Token: 0x04002453 RID: 9299
		private bool _nullPipe;

		// Token: 0x04002454 RID: 9300
		private Queue<object> _objectQueue;

		// Token: 0x04002455 RID: 9301
		private bool _isRedirected;

		// Token: 0x04002456 RID: 9302
		private List<IList> _outVariableList;

		// Token: 0x04002457 RID: 9303
		private List<IList> _errorVariableList;

		// Token: 0x04002458 RID: 9304
		private List<IList> _warningVariableList;

		// Token: 0x04002459 RID: 9305
		private List<IList> _informationVariableList;

		// Token: 0x0400245A RID: 9306
		private PSVariable _pipelineVariableObject;

		// Token: 0x0400245B RID: 9307
		private readonly List<object> _resultList;

		// Token: 0x0400245C RID: 9308
		private Collection<PSObject> _resultCollection;

		// Token: 0x0400245D RID: 9309
		private IEnumerator _enumeratorToProcess;

		// Token: 0x0400245E RID: 9310
		private bool _enumeratorToProcessIsEmpty;
	}
}
