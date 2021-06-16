using System;
using System.Collections;
using System.Collections.Generic;
using System.Diagnostics;
using System.Diagnostics.SymbolStore;
using System.Linq;
using System.Management.Automation.Internal;
using System.Management.Automation.Runspaces;
using System.Reflection;
using System.Reflection.Emit;
using System.Threading;

namespace System.Management.Automation
{
	// Token: 0x020000CC RID: 204
	internal class PSLocalEventManager : PSEventManager, IDisposable
	{
		// Token: 0x06000B7C RID: 2940 RVA: 0x00041F58 File Offset: 0x00040158
		internal PSLocalEventManager(ExecutionContext context)
		{
			this.eventSubscribers = new Dictionary<PSEventSubscriber, Delegate>();
			this.engineEventSubscribers = new Dictionary<string, List<PSEventSubscriber>>(StringComparer.OrdinalIgnoreCase);
			this.actionQueue = new Queue<EventAction>();
			this.context = context;
		}

		// Token: 0x17000333 RID: 819
		// (get) Token: 0x06000B7D RID: 2941 RVA: 0x00041FBC File Offset: 0x000401BC
		public override List<PSEventSubscriber> Subscribers
		{
			get
			{
				List<PSEventSubscriber> list = new List<PSEventSubscriber>();
				lock (this.eventSubscribers)
				{
					foreach (PSEventSubscriber item in this.eventSubscribers.Keys)
					{
						list.Add(item);
					}
				}
				return list;
			}
		}

		// Token: 0x06000B7E RID: 2942 RVA: 0x00042048 File Offset: 0x00040248
		public override PSEventSubscriber SubscribeEvent(object source, string eventName, string sourceIdentifier, PSObject data, ScriptBlock action, bool supportEvent, bool forwardEvent)
		{
			return this.SubscribeEvent(source, eventName, sourceIdentifier, data, action, supportEvent, forwardEvent, 0);
		}

		// Token: 0x06000B7F RID: 2943 RVA: 0x00042068 File Offset: 0x00040268
		public override PSEventSubscriber SubscribeEvent(object source, string eventName, string sourceIdentifier, PSObject data, ScriptBlock action, bool supportEvent, bool forwardEvent, int maxTriggerCount)
		{
			PSEventSubscriber pseventSubscriber = new PSEventSubscriber(this.context, this.nextSubscriptionId++, source, eventName, sourceIdentifier, action, supportEvent, forwardEvent, maxTriggerCount);
			this.ProcessNewSubscriber(pseventSubscriber, source, eventName, sourceIdentifier, data, supportEvent, forwardEvent);
			pseventSubscriber.RegisterJob();
			return pseventSubscriber;
		}

		// Token: 0x06000B80 RID: 2944 RVA: 0x000420B4 File Offset: 0x000402B4
		internal override PSEventSubscriber SubscribeEvent(object source, string eventName, string sourceIdentifier, PSObject data, PSEventReceivedEventHandler handlerDelegate, bool supportEvent, bool forwardEvent, bool shouldQueueAndProcessInExecutionThread, int maxTriggerCount = 0)
		{
			PSEventSubscriber pseventSubscriber = this.SubscribeEvent(source, eventName, sourceIdentifier, data, handlerDelegate, supportEvent, forwardEvent, maxTriggerCount);
			pseventSubscriber.ShouldProcessInExecutionThread = shouldQueueAndProcessInExecutionThread;
			return pseventSubscriber;
		}

		// Token: 0x06000B81 RID: 2945 RVA: 0x000420E0 File Offset: 0x000402E0
		public override PSEventSubscriber SubscribeEvent(object source, string eventName, string sourceIdentifier, PSObject data, PSEventReceivedEventHandler handlerDelegate, bool supportEvent, bool forwardEvent)
		{
			return this.SubscribeEvent(source, eventName, sourceIdentifier, data, handlerDelegate, supportEvent, forwardEvent, 0);
		}

		// Token: 0x06000B82 RID: 2946 RVA: 0x00042100 File Offset: 0x00040300
		public override PSEventSubscriber SubscribeEvent(object source, string eventName, string sourceIdentifier, PSObject data, PSEventReceivedEventHandler handlerDelegate, bool supportEvent, bool forwardEvent, int maxTriggerCount)
		{
			PSEventSubscriber pseventSubscriber = new PSEventSubscriber(this.context, this.nextSubscriptionId++, source, eventName, sourceIdentifier, handlerDelegate, supportEvent, forwardEvent, maxTriggerCount);
			this.ProcessNewSubscriber(pseventSubscriber, source, eventName, sourceIdentifier, data, supportEvent, forwardEvent);
			pseventSubscriber.RegisterJob();
			return pseventSubscriber;
		}

		// Token: 0x06000B83 RID: 2947 RVA: 0x0004214C File Offset: 0x0004034C
		private void OnElapsedEvent(object source)
		{
			LocalRunspace localRunspace = this.context.CurrentRunspace as LocalRunspace;
			if (localRunspace == null)
			{
				this._consecutiveIdleSamples = 0;
				return;
			}
			if (localRunspace.GetCurrentlyRunningPipeline() == null)
			{
				this._consecutiveIdleSamples++;
			}
			else
			{
				this._consecutiveIdleSamples = 0;
			}
			if (this._consecutiveIdleSamples == 4)
			{
				this._consecutiveIdleSamples = 0;
				lock (this.engineEventSubscribers)
				{
					List<PSEventSubscriber> list = null;
					if (this.engineEventSubscribers.TryGetValue("PowerShell.OnIdle", out list) && list.Count > 0)
					{
						base.GenerateEvent("PowerShell.OnIdle", null, new object[0], null, false, false);
						this.EnableTimer();
					}
					else
					{
						this._isTimerActive = false;
					}
					return;
				}
			}
			this.EnableTimer();
		}

		// Token: 0x06000B84 RID: 2948 RVA: 0x0004221C File Offset: 0x0004041C
		private void InitializeTimer()
		{
			try
			{
				this._timer = new Timer(new TimerCallback(this.OnElapsedEvent), null, -1, -1);
			}
			catch (ObjectDisposedException)
			{
			}
		}

		// Token: 0x06000B85 RID: 2949 RVA: 0x00042258 File Offset: 0x00040458
		private void EnableTimer()
		{
			try
			{
				this._timer.Change(100, -1);
			}
			catch (ObjectDisposedException)
			{
			}
		}

		// Token: 0x06000B86 RID: 2950 RVA: 0x0004228C File Offset: 0x0004048C
		private void ProcessNewSubscriber(PSEventSubscriber subscriber, object source, string eventName, string sourceIdentifier, PSObject data, bool supportEvent, bool forwardEvent)
		{
			Delegate @delegate = null;
			if (this.eventAssembly == null)
			{
				StackFrame stackFrame = new StackFrame(0, true);
				this.debugMode = (stackFrame.GetFileName() != null);
				this.eventAssembly = AssemblyBuilder.DefineDynamicAssembly(new AssemblyName("PSEventHandler"), AssemblyBuilderAccess.Run);
			}
			if (this.eventModule == null)
			{
				this.eventModule = this.eventAssembly.DefineDynamicModule("PSGenericEventModule", this.debugMode);
			}
			string text = null;
			bool flag = false;
			if (source != null)
			{
				if (sourceIdentifier != null && sourceIdentifier.StartsWith("PowerShell.", StringComparison.OrdinalIgnoreCase))
				{
					string message = StringUtil.Format(EventingResources.ReservedIdentifier, sourceIdentifier);
					throw new ArgumentException(message, "sourceIdentifier");
				}
				EventInfo eventInfo = null;
				Type type = source as Type;
				if (type == null)
				{
					type = source.GetType();
				}
				if (WinRTHelper.IsWinRTType(type))
				{
					throw new InvalidOperationException(EventingResources.WinRTEventsNotSupported);
				}
				BindingFlags bindingAttr = BindingFlags.IgnoreCase | BindingFlags.Instance | BindingFlags.Static | BindingFlags.Public;
				eventInfo = type.GetEvent(eventName, bindingAttr);
				if (eventInfo == null)
				{
					string message2 = StringUtil.Format(EventingResources.CouldNotFindEvent, eventName);
					throw new ArgumentException(message2, "eventName");
				}
				PropertyInfo property = type.GetProperty("EnableRaisingEvents");
				if (property != null && property.CanWrite)
				{
					try
					{
						object obj = property.SetMethod.IsStatic ? null : source;
						property.SetValue(obj, true);
					}
					catch (TargetInvocationException ex)
					{
						if (ex.InnerException != null)
						{
							throw ex.InnerException;
						}
						throw;
					}
				}
				ManagementEventWatcher managementEventWatcher = source as ManagementEventWatcher;
				if (managementEventWatcher != null)
				{
					managementEventWatcher.Start();
				}
				MethodInfo method = eventInfo.EventHandlerType.GetMethod("Invoke");
				if (method.ReturnType != typeof(void))
				{
					string nonVoidDelegateNotSupported = EventingResources.NonVoidDelegateNotSupported;
					throw new ArgumentException(nonVoidDelegateNotSupported, "eventName");
				}
				string key = source.GetType().FullName + "|" + eventName;
				Type type2 = null;
				if (PSLocalEventManager.GeneratedEventHandlers.ContainsKey(key))
				{
					type2 = PSLocalEventManager.GeneratedEventHandlers[key];
				}
				else
				{
					lock (PSLocalEventManager.GeneratedEventHandlers)
					{
						if (PSLocalEventManager.GeneratedEventHandlers.ContainsKey(key))
						{
							type2 = PSLocalEventManager.GeneratedEventHandlers[key];
						}
						else
						{
							type2 = this.GenerateEventHandler(method);
							PSLocalEventManager.GeneratedEventHandlers[key] = type2;
						}
					}
				}
				ConstructorInfo constructor = type2.GetConstructor(new Type[]
				{
					typeof(PSEventManager),
					typeof(object),
					typeof(string),
					typeof(PSObject)
				});
				object target = constructor.Invoke(new object[]
				{
					this,
					source,
					sourceIdentifier,
					data
				});
				MethodInfo method2 = type2.GetMethod("EventDelegate", BindingFlags.Instance | BindingFlags.Public);
				@delegate = method2.CreateDelegate(eventInfo.EventHandlerType, target);
				eventInfo.AddEventHandler(source, @delegate);
			}
			else if (PSEngineEvent.EngineEvents.Contains(sourceIdentifier))
			{
				text = sourceIdentifier;
				flag = string.Equals(text, "PowerShell.OnIdle", StringComparison.OrdinalIgnoreCase);
			}
			lock (this.eventSubscribers)
			{
				this.eventSubscribers[subscriber] = @delegate;
				if (text != null)
				{
					lock (this.engineEventSubscribers)
					{
						if (flag && !this._timerInitialized)
						{
							this.InitializeTimer();
							this._timerInitialized = true;
						}
						List<PSEventSubscriber> list = null;
						if (!this.engineEventSubscribers.TryGetValue(text, out list))
						{
							list = new List<PSEventSubscriber>();
							this.engineEventSubscribers.Add(text, list);
						}
						list.Add(subscriber);
						if (flag && !this._isTimerActive)
						{
							this.EnableTimer();
							this._isTimerActive = true;
						}
					}
				}
			}
		}

		// Token: 0x06000B87 RID: 2951 RVA: 0x00042688 File Offset: 0x00040888
		public override void UnsubscribeEvent(PSEventSubscriber subscriber)
		{
			this.UnsubscribeEvent(subscriber, false);
		}

		// Token: 0x06000B88 RID: 2952 RVA: 0x00042694 File Offset: 0x00040894
		private void UnsubscribeEvent(PSEventSubscriber subscriber, bool skipDraining)
		{
			if (subscriber == null)
			{
				throw new ArgumentNullException("subscriber");
			}
			Delegate @delegate = null;
			lock (this.eventSubscribers)
			{
				if (subscriber.IsBeingUnsubscribed || !this.eventSubscribers.TryGetValue(subscriber, out @delegate))
				{
					return;
				}
				subscriber.IsBeingUnsubscribed = true;
			}
			if (@delegate != null && subscriber.SourceObject != null)
			{
				subscriber.OnPSEventUnsubscribed(subscriber.SourceObject, new PSEventUnsubscribedEventArgs(subscriber));
				Type type = subscriber.SourceObject as Type;
				if (type == null)
				{
					type = subscriber.SourceObject.GetType();
				}
				BindingFlags bindingAttr = BindingFlags.IgnoreCase | BindingFlags.Instance | BindingFlags.Static | BindingFlags.Public;
				EventInfo @event = type.GetEvent(subscriber.EventName, bindingAttr);
				if (@event != null && @delegate != null)
				{
					@event.RemoveEventHandler(subscriber.SourceObject, @delegate);
				}
			}
			if (!skipDraining)
			{
				this.DrainPendingActions(subscriber);
			}
			if (subscriber.Action != null)
			{
				subscriber.Action.NotifyJobStopped();
			}
			lock (this.eventSubscribers)
			{
				this.eventSubscribers[subscriber] = null;
				this.eventSubscribers.Remove(subscriber);
				lock (this.engineEventSubscribers)
				{
					if (PSEngineEvent.EngineEvents.Contains(subscriber.SourceIdentifier))
					{
						this.engineEventSubscribers[subscriber.SourceIdentifier].Remove(subscriber);
					}
				}
			}
		}

		// Token: 0x06000B89 RID: 2953 RVA: 0x0004282C File Offset: 0x00040A2C
		protected override PSEventArgs CreateEvent(string sourceIdentifier, object sender, object[] args, PSObject extraData)
		{
			return new PSEventArgs(null, this.context.CurrentRunspace.InstanceId, base.GetNextEventId(), sourceIdentifier, sender, args, extraData);
		}

		// Token: 0x06000B8A RID: 2954 RVA: 0x0004284F File Offset: 0x00040A4F
		internal override void AddForwardedEvent(PSEventArgs forwardedEvent)
		{
			forwardedEvent.EventIdentifier = base.GetNextEventId();
			this.ProcessNewEvent(forwardedEvent, false);
		}

		// Token: 0x06000B8B RID: 2955 RVA: 0x00042865 File Offset: 0x00040A65
		protected override void ProcessNewEvent(PSEventArgs newEvent, bool processInCurrentThread)
		{
			this.ProcessNewEvent(newEvent, processInCurrentThread, false);
		}

		// Token: 0x06000B8C RID: 2956 RVA: 0x0004288C File Offset: 0x00040A8C
		protected internal override void ProcessNewEvent(PSEventArgs newEvent, bool processInCurrentThread, bool waitForCompletionWhenInCurrentThread)
		{
			if (processInCurrentThread)
			{
				this.ProcessNewEventImplementation(newEvent, true);
				ManualResetEventSlim eventProcessed = newEvent.EventProcessed;
				if (eventProcessed != null)
				{
					while (waitForCompletionWhenInCurrentThread && !eventProcessed.Wait(250))
					{
						this.ProcessPendingActions();
					}
					eventProcessed.Dispose();
					return;
				}
			}
			else
			{
				ThreadPool.QueueUserWorkItem(delegate(object unused)
				{
					this.ProcessNewEventImplementation(newEvent, false);
				});
			}
		}

		// Token: 0x06000B8D RID: 2957 RVA: 0x00042904 File Offset: 0x00040B04
		private void ProcessNewEventImplementation(PSEventArgs newEvent, bool processSynchronously)
		{
			bool flag = false;
			List<PSEventSubscriber> list = new List<PSEventSubscriber>();
			List<PSEventSubscriber> list2 = new List<PSEventSubscriber>();
			foreach (PSEventSubscriber pseventSubscriber in this.GetEventSubscribers(newEvent.SourceIdentifier, true))
			{
				newEvent.ForwardEvent = pseventSubscriber.ForwardEvent;
				if (pseventSubscriber.Action != null)
				{
					this.AddAction(new EventAction(pseventSubscriber, newEvent), processSynchronously);
					flag = true;
				}
				else if (pseventSubscriber.HandlerDelegate != null)
				{
					if (pseventSubscriber.ShouldProcessInExecutionThread)
					{
						this.AddAction(new EventAction(pseventSubscriber, newEvent), processSynchronously);
					}
					else
					{
						list.Add(pseventSubscriber);
					}
					flag = true;
				}
				else
				{
					list2.Add(pseventSubscriber);
				}
			}
			foreach (PSEventSubscriber pseventSubscriber2 in list)
			{
				pseventSubscriber2.HandlerDelegate(newEvent.Sender, newEvent);
				this.AutoUnregisterEventIfNecessary(pseventSubscriber2);
			}
			if (!flag)
			{
				if (newEvent.ForwardEvent)
				{
					this.OnForwardEvent(newEvent);
				}
				else
				{
					lock (base.ReceivedEvents.SyncRoot)
					{
						base.ReceivedEvents.Add(newEvent);
					}
				}
				foreach (PSEventSubscriber subscriber in list2)
				{
					this.AutoUnregisterEventIfNecessary(subscriber);
				}
			}
		}

		// Token: 0x06000B8E RID: 2958 RVA: 0x00042AA4 File Offset: 0x00040CA4
		private void AddAction(EventAction action, bool processSynchronously)
		{
			if (processSynchronously)
			{
				action.Args.EventProcessed = new ManualResetEventSlim();
			}
			lock (((ICollection)this.actionQueue).SyncRoot)
			{
				this.actionQueue.Enqueue(action);
			}
			this.PulseEngine();
		}

		// Token: 0x06000B8F RID: 2959 RVA: 0x00042B08 File Offset: 0x00040D08
		private void PulseEngine()
		{
			try
			{
				((LocalRunspace)this.context.CurrentRunspace).Pulse();
			}
			catch (ObjectDisposedException)
			{
			}
		}

		// Token: 0x06000B90 RID: 2960 RVA: 0x00042B40 File Offset: 0x00040D40
		internal void ProcessPendingActions()
		{
			if (this.actionQueue.Count == 0)
			{
				return;
			}
			this.ProcessPendingActionsImpl();
		}

		// Token: 0x06000B91 RID: 2961 RVA: 0x00042B68 File Offset: 0x00040D68
		private void ProcessPendingActionsImpl()
		{
			if (this.IsExecutingEventAction)
			{
				return;
			}
			try
			{
				lock (this.actionProcessingLock)
				{
					if (!this.IsExecutingEventAction)
					{
						int num = 0;
						this.throttleChecks++;
						while (this.throttleLimit * (double)this.throttleChecks >= (double)num)
						{
							EventAction eventAction;
							lock (((ICollection)this.actionQueue).SyncRoot)
							{
								if (this.actionQueue.Count == 0)
								{
									return;
								}
								eventAction = this.actionQueue.Dequeue();
							}
							bool flag3 = false;
							this.InvokeAction(eventAction, out flag3);
							num++;
							if (!flag3)
							{
								this.AutoUnregisterEventIfNecessary(eventAction.Sender);
							}
						}
						if (num > 0)
						{
							this.throttleChecks = 0;
						}
					}
				}
			}
			finally
			{
				if (this.actionQueue.Count > 0)
				{
					ThreadPool.QueueUserWorkItem(delegate(object unused)
					{
						Thread.Sleep(100);
						this.PulseEngine();
					});
				}
			}
		}

		// Token: 0x06000B92 RID: 2962 RVA: 0x00042C98 File Offset: 0x00040E98
		private void AutoUnregisterEventIfNecessary(PSEventSubscriber subscriber)
		{
			bool flag = false;
			if (subscriber.AutoUnregister)
			{
				lock (subscriber)
				{
					subscriber.RemainingActionsToProcess--;
					flag = (subscriber.RemainingTriggerCount == 0 && subscriber.RemainingActionsToProcess == 0);
				}
			}
			if (flag)
			{
				this.UnsubscribeEvent(subscriber, true);
			}
		}

		// Token: 0x06000B93 RID: 2963 RVA: 0x00042D08 File Offset: 0x00040F08
		internal void DrainPendingActions(PSEventSubscriber subscriber)
		{
			if (this.actionQueue.Count == 0)
			{
				return;
			}
			lock (this.actionProcessingLock)
			{
				lock (((ICollection)this.actionQueue).SyncRoot)
				{
					if (this.actionQueue.Count != 0)
					{
						bool flag3 = false;
						do
						{
							EventAction[] array = this.actionQueue.ToArray();
							this.actionQueue.Clear();
							foreach (EventAction eventAction in array)
							{
								if (eventAction.Sender == subscriber && eventAction != this.processingAction)
								{
									while (this.IsExecutingEventAction)
									{
										Thread.Sleep(100);
									}
									bool flag4 = false;
									this.InvokeAction(eventAction, out flag4);
									if (flag4)
									{
										flag3 = true;
									}
								}
								else
								{
									this.actionQueue.Enqueue(eventAction);
								}
							}
						}
						while (flag3);
					}
				}
			}
		}

		// Token: 0x06000B94 RID: 2964 RVA: 0x00042E18 File Offset: 0x00041018
		private void InvokeAction(EventAction nextAction, out bool addActionBack)
		{
			lock (this.actionProcessingLock)
			{
				this.processingAction = nextAction;
				addActionBack = false;
				SessionStateInternal engineSessionState = this.context.EngineSessionState;
				if (nextAction.Sender.Action != null)
				{
					this.context.EngineSessionState = nextAction.Sender.Action.ScriptBlock.SessionStateInternal;
				}
				Runspace defaultRunspace = Runspace.DefaultRunspace;
				try
				{
					Runspace.DefaultRunspace = this.context.CurrentRunspace;
					if (nextAction.Sender.Action != null)
					{
						nextAction.Sender.Action.Invoke(nextAction.Sender, nextAction.Args);
					}
					else
					{
						nextAction.Sender.HandlerDelegate(nextAction.Sender, nextAction.Args);
					}
				}
				catch (Exception ex)
				{
					CommandProcessorBase.CheckForSevereException(ex);
					if (ex is PipelineStoppedException)
					{
						this.AddAction(nextAction, false);
						addActionBack = true;
					}
				}
				finally
				{
					ManualResetEventSlim eventProcessed = nextAction.Args.EventProcessed;
					if (!addActionBack && eventProcessed != null)
					{
						eventProcessed.Set();
					}
					Runspace.DefaultRunspace = defaultRunspace;
					this.context.EngineSessionState = engineSessionState;
					this.processingAction = null;
				}
			}
		}

		// Token: 0x17000334 RID: 820
		// (get) Token: 0x06000B95 RID: 2965 RVA: 0x00042F88 File Offset: 0x00041188
		internal bool IsExecutingEventAction
		{
			get
			{
				return this.processingAction != null;
			}
		}

		// Token: 0x06000B96 RID: 2966 RVA: 0x00042F96 File Offset: 0x00041196
		public override IEnumerable<PSEventSubscriber> GetEventSubscribers(string sourceIdentifier)
		{
			return this.GetEventSubscribers(sourceIdentifier, false);
		}

		// Token: 0x06000B97 RID: 2967 RVA: 0x00042FA0 File Offset: 0x000411A0
		private IEnumerable<PSEventSubscriber> GetEventSubscribers(string sourceIdentifier, bool forNewEventProcessing)
		{
			List<PSEventSubscriber> list = new List<PSEventSubscriber>();
			List<PSEventSubscriber> list2 = new List<PSEventSubscriber>();
			lock (this.eventSubscribers)
			{
				foreach (PSEventSubscriber pseventSubscriber in this.eventSubscribers.Keys)
				{
					bool flag2 = false;
					if (string.Equals(pseventSubscriber.SourceIdentifier, sourceIdentifier, StringComparison.OrdinalIgnoreCase))
					{
						if (forNewEventProcessing)
						{
							if (!pseventSubscriber.AutoUnregister || pseventSubscriber.RemainingTriggerCount > 0)
							{
								flag2 = true;
								list.Add(pseventSubscriber);
							}
						}
						else
						{
							list.Add(pseventSubscriber);
						}
						if (forNewEventProcessing && pseventSubscriber.AutoUnregister && pseventSubscriber.RemainingTriggerCount > 0)
						{
							lock (pseventSubscriber)
							{
								pseventSubscriber.RemainingTriggerCount--;
								if (flag2)
								{
									pseventSubscriber.RemainingActionsToProcess++;
								}
								if (pseventSubscriber.RemainingTriggerCount == 0 && pseventSubscriber.RemainingActionsToProcess == 0)
								{
									list2.Add(pseventSubscriber);
								}
							}
						}
					}
				}
			}
			if (list2.Count > 0)
			{
				foreach (PSEventSubscriber subscriber in list2)
				{
					this.UnsubscribeEvent(subscriber, true);
				}
			}
			return list;
		}

		// Token: 0x06000B98 RID: 2968 RVA: 0x00043128 File Offset: 0x00041328
		private Type GenerateEventHandler(MethodInfo invokeSignature)
		{
			int num = invokeSignature.GetParameters().Length;
			StackFrame stackFrame = new StackFrame(0, true);
			ISymbolDocumentWriter document = null;
			if (this.debugMode)
			{
				document = this.eventModule.DefineDocument(stackFrame.GetFileName(), Guid.Empty, Guid.Empty, Guid.Empty);
			}
			TypeBuilder typeBuilder = this.eventModule.DefineType("PSEventHandler_" + this.typeId, TypeAttributes.Public, typeof(PSEventHandler));
			this.typeId++;
			ConstructorInfo constructor = typeof(PSEventHandler).GetConstructor(new Type[]
			{
				typeof(PSEventManager),
				typeof(object),
				typeof(string),
				typeof(PSObject)
			});
			if (this.debugMode)
			{
				Type typeFromHandle = typeof(DebuggableAttribute);
				ConstructorInfo constructor2 = typeFromHandle.GetConstructor(new Type[]
				{
					typeof(DebuggableAttribute.DebuggingModes)
				});
				CustomAttributeBuilder customAttribute = new CustomAttributeBuilder(constructor2, new object[]
				{
					DebuggableAttribute.DebuggingModes.Default | DebuggableAttribute.DebuggingModes.DisableOptimizations
				});
				this.eventAssembly.SetCustomAttribute(customAttribute);
			}
			ConstructorBuilder constructorBuilder = typeBuilder.DefineConstructor(MethodAttributes.Public, CallingConventions.Standard, new Type[]
			{
				typeof(PSEventManager),
				typeof(object),
				typeof(string),
				typeof(PSObject)
			});
			ILGenerator ilgenerator = constructorBuilder.GetILGenerator();
			ilgenerator.Emit(OpCodes.Ldarg_0);
			ilgenerator.Emit(OpCodes.Ldarg_1);
			ilgenerator.Emit(OpCodes.Ldarg_2);
			ilgenerator.Emit(OpCodes.Ldarg_3);
			ilgenerator.Emit(OpCodes.Ldarg, 4);
			ilgenerator.Emit(OpCodes.Call, constructor);
			ilgenerator.Emit(OpCodes.Ret);
			Type[] array = new Type[num];
			int num2 = 0;
			foreach (ParameterInfo parameterInfo in invokeSignature.GetParameters())
			{
				array[num2] = parameterInfo.ParameterType;
				num2++;
			}
			MethodBuilder methodBuilder = typeBuilder.DefineMethod("EventDelegate", MethodAttributes.Public, CallingConventions.Standard, invokeSignature.ReturnType, array);
			num2 = 1;
			foreach (ParameterInfo parameterInfo2 in invokeSignature.GetParameters())
			{
				methodBuilder.DefineParameter(num2, parameterInfo2.Attributes, parameterInfo2.Name);
				num2++;
			}
			ILGenerator ilgenerator2 = methodBuilder.GetILGenerator();
			LocalBuilder localBuilder = ilgenerator2.DeclareLocal(typeof(object[]));
			if (this.debugMode)
			{
				localBuilder.SetLocalSymInfo("args");
				ilgenerator2.MarkSequencePoint(document, stackFrame.GetFileLineNumber() - 1, 1, stackFrame.GetFileLineNumber(), 100);
			}
			ilgenerator2.Emit(OpCodes.Ldc_I4, num);
			ilgenerator2.Emit(OpCodes.Newarr, typeof(object));
			ilgenerator2.Emit(OpCodes.Stloc_0);
			for (int k = 1; k <= num; k++)
			{
				if (this.debugMode)
				{
					ilgenerator2.MarkSequencePoint(document, stackFrame.GetFileLineNumber() - 1, 1, stackFrame.GetFileLineNumber(), 100);
				}
				ilgenerator2.Emit(OpCodes.Ldloc_0);
				ilgenerator2.Emit(OpCodes.Ldc_I4, k - 1);
				ilgenerator2.Emit(OpCodes.Ldarg, k);
				if (array[k - 1].GetTypeInfo().IsValueType)
				{
					ilgenerator2.Emit(OpCodes.Box, array[k - 1]);
				}
				ilgenerator2.Emit(OpCodes.Stelem_Ref);
			}
			ilgenerator2.Emit(OpCodes.Ldarg_0);
			FieldInfo field = typeof(PSEventHandler).GetField("eventManager", BindingFlags.Instance | BindingFlags.NonPublic);
			ilgenerator2.Emit(OpCodes.Ldfld, field);
			ilgenerator2.Emit(OpCodes.Ldarg_0);
			FieldInfo field2 = typeof(PSEventHandler).GetField("sourceIdentifier", BindingFlags.Instance | BindingFlags.NonPublic);
			ilgenerator2.Emit(OpCodes.Ldfld, field2);
			ilgenerator2.Emit(OpCodes.Ldarg_0);
			FieldInfo field3 = typeof(PSEventHandler).GetField("sender", BindingFlags.Instance | BindingFlags.NonPublic);
			ilgenerator2.Emit(OpCodes.Ldfld, field3);
			ilgenerator2.Emit(OpCodes.Ldloc_0);
			ilgenerator2.Emit(OpCodes.Ldarg_0);
			FieldInfo field4 = typeof(PSEventHandler).GetField("extraData", BindingFlags.Instance | BindingFlags.NonPublic);
			ilgenerator2.Emit(OpCodes.Ldfld, field4);
			MethodInfo method = typeof(PSEventManager).GetMethod("GenerateEvent", new Type[]
			{
				typeof(string),
				typeof(object),
				typeof(object[]),
				typeof(PSObject)
			});
			if (this.debugMode)
			{
				ilgenerator2.MarkSequencePoint(document, stackFrame.GetFileLineNumber() - 1, 1, stackFrame.GetFileLineNumber(), 100);
			}
			ilgenerator2.Emit(OpCodes.Callvirt, method);
			ilgenerator2.Emit(OpCodes.Pop);
			ilgenerator2.Emit(OpCodes.Ret);
			return typeBuilder.CreateTypeInfo().AsType();
		}

		// Token: 0x14000002 RID: 2
		// (add) Token: 0x06000B99 RID: 2969 RVA: 0x00043630 File Offset: 0x00041830
		// (remove) Token: 0x06000B9A RID: 2970 RVA: 0x00043668 File Offset: 0x00041868
		internal override event EventHandler<PSEventArgs> ForwardEvent;

		// Token: 0x06000B9B RID: 2971 RVA: 0x000436A0 File Offset: 0x000418A0
		protected virtual void OnForwardEvent(PSEventArgs e)
		{
			EventHandler<PSEventArgs> forwardEvent = this.ForwardEvent;
			if (forwardEvent != null)
			{
				forwardEvent(this, e);
			}
		}

		// Token: 0x06000B9C RID: 2972 RVA: 0x000436C0 File Offset: 0x000418C0
		~PSLocalEventManager()
		{
			this.Dispose(false);
		}

		// Token: 0x06000B9D RID: 2973 RVA: 0x000436F0 File Offset: 0x000418F0
		public void Dispose()
		{
			this.Dispose(true);
			GC.SuppressFinalize(this);
		}

		// Token: 0x06000B9E RID: 2974 RVA: 0x00043700 File Offset: 0x00041900
		public void Dispose(bool disposing)
		{
			if (disposing)
			{
				lock (this.eventSubscribers)
				{
					if (this._timer != null)
					{
						this._timer.Dispose();
					}
					foreach (PSEventSubscriber subscriber in this.eventSubscribers.Keys.ToList<PSEventSubscriber>())
					{
						this.UnsubscribeEvent(subscriber);
					}
				}
			}
		}

		// Token: 0x04000516 RID: 1302
		private Dictionary<PSEventSubscriber, Delegate> eventSubscribers;

		// Token: 0x04000517 RID: 1303
		private Dictionary<string, List<PSEventSubscriber>> engineEventSubscribers;

		// Token: 0x04000518 RID: 1304
		private Queue<EventAction> actionQueue;

		// Token: 0x04000519 RID: 1305
		private ExecutionContext context;

		// Token: 0x0400051A RID: 1306
		private int nextSubscriptionId = 1;

		// Token: 0x0400051B RID: 1307
		private double throttleLimit = 1.0;

		// Token: 0x0400051C RID: 1308
		private int throttleChecks;

		// Token: 0x0400051D RID: 1309
		private AssemblyBuilder eventAssembly;

		// Token: 0x0400051E RID: 1310
		private ModuleBuilder eventModule;

		// Token: 0x0400051F RID: 1311
		private int typeId;

		// Token: 0x04000520 RID: 1312
		private bool debugMode;

		// Token: 0x04000521 RID: 1313
		private Timer _timer;

		// Token: 0x04000522 RID: 1314
		private bool _timerInitialized;

		// Token: 0x04000523 RID: 1315
		private bool _isTimerActive;

		// Token: 0x04000524 RID: 1316
		private int _consecutiveIdleSamples;

		// Token: 0x04000525 RID: 1317
		private static Dictionary<string, Type> GeneratedEventHandlers = new Dictionary<string, Type>();

		// Token: 0x04000526 RID: 1318
		private object actionProcessingLock = new object();

		// Token: 0x04000527 RID: 1319
		private EventAction processingAction;
	}
}
