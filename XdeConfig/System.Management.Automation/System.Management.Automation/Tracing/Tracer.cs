using System;
using System.Diagnostics.Eventing;
using System.Text;

namespace System.Management.Automation.Tracing
{
	// Token: 0x02000901 RID: 2305
	public sealed class Tracer : EtwActivity
	{
		// Token: 0x17001194 RID: 4500
		// (get) Token: 0x06005656 RID: 22102 RVA: 0x001C5133 File Offset: 0x001C3333
		protected override Guid ProviderId
		{
			get
			{
				return Tracer.providerId;
			}
		}

		// Token: 0x17001195 RID: 4501
		// (get) Token: 0x06005657 RID: 22103 RVA: 0x001C513A File Offset: 0x001C333A
		protected override EventDescriptor TransferEvent
		{
			get
			{
				return Tracer.WriteTransferEventEvent;
			}
		}

		// Token: 0x06005658 RID: 22104 RVA: 0x001C5144 File Offset: 0x001C3344
		[EtwEvent(7941L)]
		public void WriteTransferEvent(Guid currentActivityId, Guid parentActivityId)
		{
			base.WriteEvent(Tracer.WriteTransferEventEvent, new object[]
			{
				currentActivityId,
				parentActivityId
			});
		}

		// Token: 0x06005659 RID: 22105 RVA: 0x001C5178 File Offset: 0x001C3378
		[EtwEvent(49152L)]
		public void DebugMessage(string message)
		{
			base.WriteEvent(Tracer.DebugMessageEvent, new object[]
			{
				message
			});
		}

		// Token: 0x0600565A RID: 22106 RVA: 0x001C519C File Offset: 0x001C339C
		[EtwEvent(45112L)]
		public void AbortingWorkflowExecution(Guid workflowId, string reason)
		{
			base.WriteEvent(Tracer.M3PAbortingWorkflowExecutionEvent, new object[]
			{
				workflowId,
				reason
			});
		}

		// Token: 0x0600565B RID: 22107 RVA: 0x001C51CC File Offset: 0x001C33CC
		[EtwEvent(45119L)]
		public void ActivityExecutionFinished(string activityName)
		{
			base.WriteEvent(Tracer.M3PActivityExecutionFinishedEvent, new object[]
			{
				activityName
			});
		}

		// Token: 0x0600565C RID: 22108 RVA: 0x001C51F0 File Offset: 0x001C33F0
		[EtwEvent(45079L)]
		public void ActivityExecutionQueued(Guid workflowId, string activityName)
		{
			base.WriteEvent(Tracer.M3PActivityExecutionQueuedEvent, new object[]
			{
				workflowId,
				activityName
			});
		}

		// Token: 0x0600565D RID: 22109 RVA: 0x001C5220 File Offset: 0x001C3420
		[EtwEvent(45080L)]
		public void ActivityExecutionStarted(string activityName, string activityTypeName)
		{
			base.WriteEvent(Tracer.M3PActivityExecutionStartedEvent, new object[]
			{
				activityName,
				activityTypeName
			});
		}

		// Token: 0x0600565E RID: 22110 RVA: 0x001C5248 File Offset: 0x001C3448
		[EtwEvent(46348L)]
		public void BeginContainerParentJobExecution(Guid containerParentJobInstanceId)
		{
			base.WriteEvent(Tracer.M3PBeginContainerParentJobExecutionEvent, new object[]
			{
				containerParentJobInstanceId
			});
		}

		// Token: 0x0600565F RID: 22111 RVA: 0x001C5274 File Offset: 0x001C3474
		[EtwEvent(46339L)]
		public void BeginCreateNewJob(Guid trackingId)
		{
			base.WriteEvent(Tracer.M3PBeginCreateNewJobEvent, new object[]
			{
				trackingId
			});
		}

		// Token: 0x06005660 RID: 22112 RVA: 0x001C52A0 File Offset: 0x001C34A0
		[EtwEvent(46342L)]
		public void BeginJobLogic(Guid workflowJobJobInstanceId)
		{
			base.WriteEvent(Tracer.M3PBeginJobLogicEvent, new object[]
			{
				workflowJobJobInstanceId
			});
		}

		// Token: 0x06005661 RID: 22113 RVA: 0x001C52CC File Offset: 0x001C34CC
		[EtwEvent(46354L)]
		public void BeginProxyChildJobEventHandler(Guid proxyChildJobInstanceId)
		{
			base.WriteEvent(Tracer.M3PBeginProxyChildJobEventHandlerEvent, new object[]
			{
				proxyChildJobInstanceId
			});
		}

		// Token: 0x06005662 RID: 22114 RVA: 0x001C52F8 File Offset: 0x001C34F8
		[EtwEvent(46352L)]
		public void BeginProxyJobEventHandler(Guid proxyJobInstanceId)
		{
			base.WriteEvent(Tracer.M3PBeginProxyJobEventHandlerEvent, new object[]
			{
				proxyJobInstanceId
			});
		}

		// Token: 0x06005663 RID: 22115 RVA: 0x001C5324 File Offset: 0x001C3524
		[EtwEvent(46350L)]
		public void BeginProxyJobExecution(Guid proxyJobInstanceId)
		{
			base.WriteEvent(Tracer.M3PBeginProxyJobExecutionEvent, new object[]
			{
				proxyJobInstanceId
			});
		}

		// Token: 0x06005664 RID: 22116 RVA: 0x001C534D File Offset: 0x001C354D
		[EtwEvent(46356L)]
		public void BeginRunGarbageCollection()
		{
			base.WriteEvent(Tracer.M3PBeginRunGarbageCollectionEvent, new object[0]);
		}

		// Token: 0x06005665 RID: 22117 RVA: 0x001C5360 File Offset: 0x001C3560
		[EtwEvent(46337L)]
		public void BeginStartWorkflowApplication(Guid trackingId)
		{
			base.WriteEvent(Tracer.M3PBeginStartWorkflowApplicationEvent, new object[]
			{
				trackingId
			});
		}

		// Token: 0x06005666 RID: 22118 RVA: 0x001C538C File Offset: 0x001C358C
		[EtwEvent(46344L)]
		public void BeginWorkflowExecution(Guid workflowJobJobInstanceId)
		{
			base.WriteEvent(Tracer.M3PBeginWorkflowExecutionEvent, new object[]
			{
				workflowJobJobInstanceId
			});
		}

		// Token: 0x06005667 RID: 22119 RVA: 0x001C53B8 File Offset: 0x001C35B8
		[EtwEvent(45111L)]
		public void CancellingWorkflowExecution(Guid workflowId)
		{
			base.WriteEvent(Tracer.M3PCancellingWorkflowExecutionEvent, new object[]
			{
				workflowId
			});
		}

		// Token: 0x06005668 RID: 22120 RVA: 0x001C53E4 File Offset: 0x001C35E4
		[EtwEvent(46346L)]
		public void ChildWorkflowJobAddition(Guid workflowJobInstanceId, Guid containerParentJobInstanceId)
		{
			base.WriteEvent(Tracer.M3PChildWorkflowJobAdditionEvent, new object[]
			{
				workflowJobInstanceId,
				containerParentJobInstanceId
			});
		}

		// Token: 0x06005669 RID: 22121 RVA: 0x001C5418 File Offset: 0x001C3618
		[EtwEvent(46349L)]
		public void EndContainerParentJobExecution(Guid containerParentJobInstanceId)
		{
			base.WriteEvent(Tracer.M3PEndContainerParentJobExecutionEvent, new object[]
			{
				containerParentJobInstanceId
			});
		}

		// Token: 0x0600566A RID: 22122 RVA: 0x001C5444 File Offset: 0x001C3644
		[EtwEvent(46340L)]
		public void EndCreateNewJob(Guid trackingId)
		{
			base.WriteEvent(Tracer.M3PEndCreateNewJobEvent, new object[]
			{
				trackingId
			});
		}

		// Token: 0x0600566B RID: 22123 RVA: 0x001C5470 File Offset: 0x001C3670
		[EtwEvent(46343L)]
		public void EndJobLogic(Guid workflowJobJobInstanceId)
		{
			base.WriteEvent(Tracer.M3PEndJobLogicEvent, new object[]
			{
				workflowJobJobInstanceId
			});
		}

		// Token: 0x0600566C RID: 22124 RVA: 0x001C549C File Offset: 0x001C369C
		[EtwEvent(45124L)]
		public void EndpointDisabled(string endpointName, string disabledBy)
		{
			base.WriteEvent(Tracer.M3PEndpointDisabledEvent, new object[]
			{
				endpointName,
				disabledBy
			});
		}

		// Token: 0x0600566D RID: 22125 RVA: 0x001C54C4 File Offset: 0x001C36C4
		[EtwEvent(45125L)]
		public void EndpointEnabled(string endpointName, string enabledBy)
		{
			base.WriteEvent(Tracer.M3PEndpointEnabledEvent, new object[]
			{
				endpointName,
				enabledBy
			});
		}

		// Token: 0x0600566E RID: 22126 RVA: 0x001C54EC File Offset: 0x001C36EC
		[EtwEvent(45122L)]
		public void EndpointModified(string endpointName, string modifiedBy)
		{
			base.WriteEvent(Tracer.M3PEndpointModifiedEvent, new object[]
			{
				endpointName,
				modifiedBy
			});
		}

		// Token: 0x0600566F RID: 22127 RVA: 0x001C5514 File Offset: 0x001C3714
		[EtwEvent(45121L)]
		public void EndpointRegistered(string endpointName, string endpointType, string registeredBy)
		{
			base.WriteEvent(Tracer.M3PEndpointRegisteredEvent, new object[]
			{
				endpointName,
				endpointType,
				registeredBy
			});
		}

		// Token: 0x06005670 RID: 22128 RVA: 0x001C5540 File Offset: 0x001C3740
		[EtwEvent(45123L)]
		public void EndpointUnregistered(string endpointName, string unregisteredBy)
		{
			base.WriteEvent(Tracer.M3PEndpointUnregisteredEvent, new object[]
			{
				endpointName,
				unregisteredBy
			});
		}

		// Token: 0x06005671 RID: 22129 RVA: 0x001C5568 File Offset: 0x001C3768
		[EtwEvent(46355L)]
		public void EndProxyChildJobEventHandler(Guid proxyChildJobInstanceId)
		{
			base.WriteEvent(Tracer.M3PEndProxyChildJobEventHandlerEvent, new object[]
			{
				proxyChildJobInstanceId
			});
		}

		// Token: 0x06005672 RID: 22130 RVA: 0x001C5594 File Offset: 0x001C3794
		[EtwEvent(46353L)]
		public void EndProxyJobEventHandler(Guid proxyJobInstanceId)
		{
			base.WriteEvent(Tracer.M3PEndProxyJobEventHandlerEvent, new object[]
			{
				proxyJobInstanceId
			});
		}

		// Token: 0x06005673 RID: 22131 RVA: 0x001C55C0 File Offset: 0x001C37C0
		[EtwEvent(46351L)]
		public void EndProxyJobExecution(Guid proxyJobInstanceId)
		{
			base.WriteEvent(Tracer.M3PEndProxyJobExecutionEvent, new object[]
			{
				proxyJobInstanceId
			});
		}

		// Token: 0x06005674 RID: 22132 RVA: 0x001C55E9 File Offset: 0x001C37E9
		[EtwEvent(46357L)]
		public void EndRunGarbageCollection()
		{
			base.WriteEvent(Tracer.M3PEndRunGarbageCollectionEvent, new object[0]);
		}

		// Token: 0x06005675 RID: 22133 RVA: 0x001C55FC File Offset: 0x001C37FC
		[EtwEvent(46338L)]
		public void EndStartWorkflowApplication(Guid trackingId)
		{
			base.WriteEvent(Tracer.M3PEndStartWorkflowApplicationEvent, new object[]
			{
				trackingId
			});
		}

		// Token: 0x06005676 RID: 22134 RVA: 0x001C5628 File Offset: 0x001C3828
		[EtwEvent(46345L)]
		public void EndWorkflowExecution(Guid workflowJobJobInstanceId)
		{
			base.WriteEvent(Tracer.M3PEndWorkflowExecutionEvent, new object[]
			{
				workflowJobJobInstanceId
			});
		}

		// Token: 0x06005677 RID: 22135 RVA: 0x001C5654 File Offset: 0x001C3854
		[EtwEvent(45083L)]
		public void ErrorImportingWorkflowFromXaml(Guid workflowId, string errorDescription)
		{
			base.WriteEvent(Tracer.M3PErrorImportingWorkflowFromXamlEvent, new object[]
			{
				workflowId,
				errorDescription
			});
		}

		// Token: 0x06005678 RID: 22136 RVA: 0x001C5684 File Offset: 0x001C3884
		[EtwEvent(45116L)]
		public void ForcedWorkflowShutdownError(Guid workflowId, string errorDescription)
		{
			base.WriteEvent(Tracer.M3PForcedWorkflowShutdownErrorEvent, new object[]
			{
				workflowId,
				errorDescription
			});
		}

		// Token: 0x06005679 RID: 22137 RVA: 0x001C56B4 File Offset: 0x001C38B4
		[EtwEvent(45115L)]
		public void ForcedWorkflowShutdownFinished(Guid workflowId)
		{
			base.WriteEvent(Tracer.M3PForcedWorkflowShutdownFinishedEvent, new object[]
			{
				workflowId
			});
		}

		// Token: 0x0600567A RID: 22138 RVA: 0x001C56E0 File Offset: 0x001C38E0
		[EtwEvent(45114L)]
		public void ForcedWorkflowShutdownStarted(Guid workflowId)
		{
			base.WriteEvent(Tracer.M3PForcedWorkflowShutdownStartedEvent, new object[]
			{
				workflowId
			});
		}

		// Token: 0x0600567B RID: 22139 RVA: 0x001C570C File Offset: 0x001C390C
		[EtwEvent(45082L)]
		public void ImportedWorkflowFromXaml(Guid workflowId, string xamlFile)
		{
			base.WriteEvent(Tracer.M3PImportedWorkflowFromXamlEvent, new object[]
			{
				workflowId,
				xamlFile
			});
		}

		// Token: 0x0600567C RID: 22140 RVA: 0x001C573C File Offset: 0x001C393C
		[EtwEvent(45081L)]
		public void ImportingWorkflowFromXaml(Guid workflowId, string xamlFile)
		{
			base.WriteEvent(Tracer.M3PImportingWorkflowFromXamlEvent, new object[]
			{
				workflowId,
				xamlFile
			});
		}

		// Token: 0x0600567D RID: 22141 RVA: 0x001C576C File Offset: 0x001C396C
		[EtwEvent(45106L)]
		public void JobCreationComplete(Guid jobId, Guid workflowId)
		{
			base.WriteEvent(Tracer.M3PJobCreationCompleteEvent, new object[]
			{
				jobId,
				workflowId
			});
		}

		// Token: 0x0600567E RID: 22142 RVA: 0x001C57A0 File Offset: 0x001C39A0
		[EtwEvent(45102L)]
		public void JobError(int jobId, Guid workflowId, string errorDescription)
		{
			base.WriteEvent(Tracer.M3PJobErrorEvent, new object[]
			{
				jobId,
				workflowId,
				errorDescription
			});
		}

		// Token: 0x0600567F RID: 22143 RVA: 0x001C57D8 File Offset: 0x001C39D8
		[EtwEvent(45107L)]
		public void JobRemoved(Guid parentJobId, Guid childJobId, Guid workflowId)
		{
			base.WriteEvent(Tracer.M3PJobRemovedEvent, new object[]
			{
				parentJobId,
				childJobId,
				workflowId
			});
		}

		// Token: 0x06005680 RID: 22144 RVA: 0x001C5814 File Offset: 0x001C3A14
		[EtwEvent(45108L)]
		public void JobRemoveError(Guid parentJobId, Guid childJobId, Guid workflowId, string error)
		{
			base.WriteEvent(Tracer.M3PJobRemoveErrorEvent, new object[]
			{
				parentJobId,
				childJobId,
				workflowId,
				error
			});
		}

		// Token: 0x06005681 RID: 22145 RVA: 0x001C5854 File Offset: 0x001C3A54
		[EtwEvent(45101L)]
		public void JobStateChanged(int jobId, Guid workflowId, string newState, string oldState)
		{
			base.WriteEvent(Tracer.M3PJobStateChangedEvent, new object[]
			{
				jobId,
				workflowId,
				newState,
				oldState
			});
		}

		// Token: 0x06005682 RID: 22146 RVA: 0x001C5890 File Offset: 0x001C3A90
		[EtwEvent(45109L)]
		public void LoadingWorkflowForExecution(Guid workflowId)
		{
			base.WriteEvent(Tracer.M3PLoadingWorkflowForExecutionEvent, new object[]
			{
				workflowId
			});
		}

		// Token: 0x06005683 RID: 22147 RVA: 0x001C58BC File Offset: 0x001C3ABC
		[EtwEvent(45126L)]
		public void OutOfProcessRunspaceStarted(string command)
		{
			base.WriteEvent(Tracer.M3POutOfProcessRunspaceStartedEvent, new object[]
			{
				command
			});
		}

		// Token: 0x06005684 RID: 22148 RVA: 0x001C58E0 File Offset: 0x001C3AE0
		[EtwEvent(45127L)]
		public void ParameterSplattingWasPerformed(string parameters, string computers)
		{
			base.WriteEvent(Tracer.M3PParameterSplattingWasPerformedEvent, new object[]
			{
				parameters,
				computers
			});
		}

		// Token: 0x06005685 RID: 22149 RVA: 0x001C5908 File Offset: 0x001C3B08
		[EtwEvent(45105L)]
		public void ParentJobCreated(Guid jobId)
		{
			base.WriteEvent(Tracer.M3PParentJobCreatedEvent, new object[]
			{
				jobId
			});
		}

		// Token: 0x06005686 RID: 22150 RVA: 0x001C5931 File Offset: 0x001C3B31
		[EtwEvent(46358L)]
		public void PersistenceStoreMaxSizeReached()
		{
			base.WriteEvent(Tracer.M3PPersistenceStoreMaxSizeReachedEvent, new object[0]);
		}

		// Token: 0x06005687 RID: 22151 RVA: 0x001C5944 File Offset: 0x001C3B44
		[EtwEvent(45117L)]
		public void PersistingWorkflow(Guid workflowId, string persistPath)
		{
			base.WriteEvent(Tracer.M3PPersistingWorkflowEvent, new object[]
			{
				workflowId,
				persistPath
			});
		}

		// Token: 0x06005688 RID: 22152 RVA: 0x001C5974 File Offset: 0x001C3B74
		[EtwEvent(46347L)]
		public void ProxyJobRemoteJobAssociation(Guid proxyJobInstanceId, Guid containerParentJobInstanceId)
		{
			base.WriteEvent(Tracer.M3PProxyJobRemoteJobAssociationEvent, new object[]
			{
				proxyJobInstanceId,
				containerParentJobInstanceId
			});
		}

		// Token: 0x06005689 RID: 22153 RVA: 0x001C59A8 File Offset: 0x001C3BA8
		[EtwEvent(45100L)]
		public void RemoveJobStarted(Guid jobId)
		{
			base.WriteEvent(Tracer.M3PRemoveJobStartedEvent, new object[]
			{
				jobId
			});
		}

		// Token: 0x0600568A RID: 22154 RVA: 0x001C59D4 File Offset: 0x001C3BD4
		[EtwEvent(45090L)]
		public void RunspaceAvailabilityChanged(string runspaceId, string availability)
		{
			base.WriteEvent(Tracer.M3PRunspaceAvailabilityChangedEvent, new object[]
			{
				runspaceId,
				availability
			});
		}

		// Token: 0x0600568B RID: 22155 RVA: 0x001C59FC File Offset: 0x001C3BFC
		[EtwEvent(45091L)]
		public void RunspaceStateChanged(string runspaceId, string newState, string oldState)
		{
			base.WriteEvent(Tracer.M3PRunspaceStateChangedEvent, new object[]
			{
				runspaceId,
				newState,
				oldState
			});
		}

		// Token: 0x0600568C RID: 22156 RVA: 0x001C5A28 File Offset: 0x001C3C28
		[EtwEvent(46341L)]
		public void TrackingGuidContainerParentJobCorrelation(Guid trackingId, Guid containerParentJobInstanceId)
		{
			base.WriteEvent(Tracer.M3PTrackingGuidContainerParentJobCorrelationEvent, new object[]
			{
				trackingId,
				containerParentJobInstanceId
			});
		}

		// Token: 0x0600568D RID: 22157 RVA: 0x001C5A5C File Offset: 0x001C3C5C
		[EtwEvent(45113L)]
		public void UnloadingWorkflow(Guid workflowId)
		{
			base.WriteEvent(Tracer.M3PUnloadingWorkflowEvent, new object[]
			{
				workflowId
			});
		}

		// Token: 0x0600568E RID: 22158 RVA: 0x001C5A88 File Offset: 0x001C3C88
		[EtwEvent(45089L)]
		public void WorkflowActivityExecutionFailed(Guid workflowId, string activityName, string failureDescription)
		{
			base.WriteEvent(Tracer.M3PWorkflowActivityExecutionFailedEvent, new object[]
			{
				workflowId,
				activityName,
				failureDescription
			});
		}

		// Token: 0x0600568F RID: 22159 RVA: 0x001C5ABC File Offset: 0x001C3CBC
		[EtwEvent(45087L)]
		public void WorkflowActivityValidated(Guid workflowId, string activityDisplayName, string activityType)
		{
			base.WriteEvent(Tracer.M3PWorkflowActivityValidatedEvent, new object[]
			{
				workflowId,
				activityDisplayName,
				activityType
			});
		}

		// Token: 0x06005690 RID: 22160 RVA: 0x001C5AF0 File Offset: 0x001C3CF0
		[EtwEvent(45088L)]
		public void WorkflowActivityValidationFailed(Guid workflowId, string activityDisplayName, string activityType)
		{
			base.WriteEvent(Tracer.M3PWorkflowActivityValidationFailedEvent, new object[]
			{
				workflowId,
				activityDisplayName,
				activityType
			});
		}

		// Token: 0x06005691 RID: 22161 RVA: 0x001C5B24 File Offset: 0x001C3D24
		[EtwEvent(45096L)]
		public void WorkflowCleanupPerformed(Guid workflowId)
		{
			base.WriteEvent(Tracer.M3PWorkflowCleanupPerformedEvent, new object[]
			{
				workflowId
			});
		}

		// Token: 0x06005692 RID: 22162 RVA: 0x001C5B50 File Offset: 0x001C3D50
		[EtwEvent(45098L)]
		public void WorkflowDeletedFromDisk(Guid workflowId, string path)
		{
			base.WriteEvent(Tracer.M3PWorkflowDeletedFromDiskEvent, new object[]
			{
				workflowId,
				path
			});
		}

		// Token: 0x06005693 RID: 22163 RVA: 0x001C5B80 File Offset: 0x001C3D80
		[EtwEvent(45128L)]
		public void WorkflowEngineStarted(string endpointName)
		{
			base.WriteEvent(Tracer.M3PWorkflowEngineStartedEvent, new object[]
			{
				endpointName
			});
		}

		// Token: 0x06005694 RID: 22164 RVA: 0x001C5BA4 File Offset: 0x001C3DA4
		[EtwEvent(45095L)]
		public void WorkflowExecutionAborted(Guid workflowId)
		{
			base.WriteEvent(Tracer.M3PWorkflowExecutionAbortedEvent, new object[]
			{
				workflowId
			});
		}

		// Token: 0x06005695 RID: 22165 RVA: 0x001C5BD0 File Offset: 0x001C3DD0
		[EtwEvent(45094L)]
		public void WorkflowExecutionCancelled(Guid workflowId)
		{
			base.WriteEvent(Tracer.M3PWorkflowExecutionCancelledEvent, new object[]
			{
				workflowId
			});
		}

		// Token: 0x06005696 RID: 22166 RVA: 0x001C5BFC File Offset: 0x001C3DFC
		[EtwEvent(45120L)]
		public void WorkflowExecutionError(Guid workflowId, string errorDescription)
		{
			base.WriteEvent(Tracer.M3PWorkflowExecutionErrorEvent, new object[]
			{
				workflowId,
				errorDescription
			});
		}

		// Token: 0x06005697 RID: 22167 RVA: 0x001C5C2C File Offset: 0x001C3E2C
		[EtwEvent(45110L)]
		public void WorkflowExecutionFinished(Guid workflowId)
		{
			base.WriteEvent(Tracer.M3PWorkflowExecutionFinishedEvent, new object[]
			{
				workflowId
			});
		}

		// Token: 0x06005698 RID: 22168 RVA: 0x001C5C58 File Offset: 0x001C3E58
		[EtwEvent(45064L)]
		public void WorkflowExecutionStarted(Guid workflowId, string managedNodes)
		{
			base.WriteEvent(Tracer.M3PWorkflowExecutionStartedEvent, new object[]
			{
				workflowId,
				managedNodes
			});
		}

		// Token: 0x06005699 RID: 22169 RVA: 0x001C5C88 File Offset: 0x001C3E88
		[EtwEvent(45104L)]
		public void WorkflowJobCreated(Guid parentJobId, Guid childJobId, Guid childWorkflowId)
		{
			base.WriteEvent(Tracer.M3PWorkflowJobCreatedEvent, new object[]
			{
				parentJobId,
				childJobId,
				childWorkflowId
			});
		}

		// Token: 0x0600569A RID: 22170 RVA: 0x001C5CC4 File Offset: 0x001C3EC4
		[EtwEvent(45092L)]
		public void WorkflowLoadedForExecution(Guid workflowId)
		{
			base.WriteEvent(Tracer.M3PWorkflowLoadedForExecutionEvent, new object[]
			{
				workflowId
			});
		}

		// Token: 0x0600569B RID: 22171 RVA: 0x001C5CF0 File Offset: 0x001C3EF0
		[EtwEvent(45097L)]
		public void WorkflowLoadedFromDisk(Guid workflowId, string path)
		{
			base.WriteEvent(Tracer.M3PWorkflowLoadedFromDiskEvent, new object[]
			{
				workflowId,
				path
			});
		}

		// Token: 0x0600569C RID: 22172 RVA: 0x001C5D20 File Offset: 0x001C3F20
		[EtwEvent(45129L)]
		public void WorkflowManagerCheckpoint(string checkpointPath, string configProviderId, string userName, string path)
		{
			base.WriteEvent(Tracer.M3PWorkflowManagerCheckpointEvent, new object[]
			{
				checkpointPath,
				configProviderId,
				userName,
				path
			});
		}

		// Token: 0x0600569D RID: 22173 RVA: 0x001C5D54 File Offset: 0x001C3F54
		[EtwEvent(45118L)]
		public void WorkflowPersisted(Guid workflowId)
		{
			base.WriteEvent(Tracer.M3PWorkflowPersistedEvent, new object[]
			{
				workflowId
			});
		}

		// Token: 0x0600569E RID: 22174 RVA: 0x001C5D80 File Offset: 0x001C3F80
		[EtwEvent(45072L)]
		public void WorkflowPluginRequestedToShutdown(string endpointName)
		{
			base.WriteEvent(Tracer.M3PWorkflowPluginRequestedToShutdownEvent, new object[]
			{
				endpointName
			});
		}

		// Token: 0x0600569F RID: 22175 RVA: 0x001C5DA4 File Offset: 0x001C3FA4
		[EtwEvent(45073L)]
		public void WorkflowPluginRestarted(string endpointName)
		{
			base.WriteEvent(Tracer.M3PWorkflowPluginRestartedEvent, new object[]
			{
				endpointName
			});
		}

		// Token: 0x060056A0 RID: 22176 RVA: 0x001C5DC8 File Offset: 0x001C3FC8
		[EtwEvent(45063L)]
		public void WorkflowPluginStarted(string endpointName, string user, string hostingMode, string protocol, string configuration)
		{
			base.WriteEvent(Tracer.M3PWorkflowPluginStartedEvent, new object[]
			{
				endpointName,
				user,
				hostingMode,
				protocol,
				configuration
			});
		}

		// Token: 0x060056A1 RID: 22177 RVA: 0x001C5E00 File Offset: 0x001C4000
		[EtwEvent(45075L)]
		public void WorkflowQuotaViolated(string endpointName, string configName, string allowedValue, string valueInQuestion)
		{
			base.WriteEvent(Tracer.M3PWorkflowQuotaViolatedEvent, new object[]
			{
				endpointName,
				configName,
				allowedValue,
				valueInQuestion
			});
		}

		// Token: 0x060056A2 RID: 22178 RVA: 0x001C5E34 File Offset: 0x001C4034
		[EtwEvent(45076L)]
		public void WorkflowResumed(Guid workflowId)
		{
			base.WriteEvent(Tracer.M3PWorkflowResumedEvent, new object[]
			{
				workflowId
			});
		}

		// Token: 0x060056A3 RID: 22179 RVA: 0x001C5E60 File Offset: 0x001C4060
		[EtwEvent(45074L)]
		public void WorkflowResuming(Guid workflowId)
		{
			base.WriteEvent(Tracer.M3PWorkflowResumingEvent, new object[]
			{
				workflowId
			});
		}

		// Token: 0x060056A4 RID: 22180 RVA: 0x001C5E8C File Offset: 0x001C408C
		[EtwEvent(45078L)]
		public void WorkflowRunspacePoolCreated(Guid workflowId, string managedNode)
		{
			base.WriteEvent(Tracer.M3PWorkflowRunspacePoolCreatedEvent, new object[]
			{
				workflowId,
				managedNode
			});
		}

		// Token: 0x060056A5 RID: 22181 RVA: 0x001C5EBC File Offset: 0x001C40BC
		[EtwEvent(45065L)]
		public void WorkflowStateChanged(Guid workflowId, string newState, string oldState)
		{
			base.WriteEvent(Tracer.M3PWorkflowStateChangedEvent, new object[]
			{
				workflowId,
				newState,
				oldState
			});
		}

		// Token: 0x060056A6 RID: 22182 RVA: 0x001C5EF0 File Offset: 0x001C40F0
		[EtwEvent(45093L)]
		public void WorkflowUnloaded(Guid workflowId)
		{
			base.WriteEvent(Tracer.M3PWorkflowUnloadedEvent, new object[]
			{
				workflowId
			});
		}

		// Token: 0x060056A7 RID: 22183 RVA: 0x001C5F1C File Offset: 0x001C411C
		[EtwEvent(45086L)]
		public void WorkflowValidationError(Guid workflowId)
		{
			base.WriteEvent(Tracer.M3PWorkflowValidationErrorEvent, new object[]
			{
				workflowId
			});
		}

		// Token: 0x060056A8 RID: 22184 RVA: 0x001C5F48 File Offset: 0x001C4148
		[EtwEvent(45085L)]
		public void WorkflowValidationFinished(Guid workflowId)
		{
			base.WriteEvent(Tracer.M3PWorkflowValidationFinishedEvent, new object[]
			{
				workflowId
			});
		}

		// Token: 0x060056A9 RID: 22185 RVA: 0x001C5F74 File Offset: 0x001C4174
		[EtwEvent(45084L)]
		public void WorkflowValidationStarted(Guid workflowId)
		{
			base.WriteEvent(Tracer.M3PWorkflowValidationStartedEvent, new object[]
			{
				workflowId
			});
		}

		// Token: 0x060056AA RID: 22186 RVA: 0x001C5F9D File Offset: 0x001C419D
		[EtwEvent(49152L)]
		public void DebugMessage(Exception exception)
		{
			if (exception == null)
			{
				return;
			}
			this.DebugMessage(Tracer.GetExceptionString(exception));
		}

		// Token: 0x060056AB RID: 22187 RVA: 0x001C5FB0 File Offset: 0x001C41B0
		public static string GetExceptionString(Exception exception)
		{
			if (exception == null)
			{
				return string.Empty;
			}
			StringBuilder stringBuilder = new StringBuilder();
			while (Tracer.WriteExceptionText(stringBuilder, exception))
			{
				exception = exception.InnerException;
			}
			return stringBuilder.ToString();
		}

		// Token: 0x060056AC RID: 22188 RVA: 0x001C5FE5 File Offset: 0x001C41E5
		private static bool WriteExceptionText(StringBuilder sb, Exception e)
		{
			if (e == null)
			{
				return false;
			}
			sb.Append(e.GetType().Name);
			sb.Append(Environment.NewLine);
			sb.Append(e.Message);
			sb.Append(Environment.NewLine);
			return true;
		}

		// Token: 0x04002DEB RID: 11755
		public const byte LevelCritical = 1;

		// Token: 0x04002DEC RID: 11756
		public const byte LevelError = 2;

		// Token: 0x04002DED RID: 11757
		public const byte LevelWarning = 3;

		// Token: 0x04002DEE RID: 11758
		public const byte LevelInformational = 4;

		// Token: 0x04002DEF RID: 11759
		public const byte LevelVerbose = 5;

		// Token: 0x04002DF0 RID: 11760
		public const long KeywordAll = 4294967295L;

		// Token: 0x04002DF1 RID: 11761
		private static Guid providerId = Guid.Parse("a0c1853b-5c40-4b15-8766-3cf1c58f985a");

		// Token: 0x04002DF2 RID: 11762
		private static EventDescriptor WriteTransferEventEvent = new EventDescriptor(7941, 1, 17, 5, 20, 0, 4611686018427387904L);

		// Token: 0x04002DF3 RID: 11763
		private static EventDescriptor DebugMessageEvent = new EventDescriptor(49152, 1, 18, 4, 0, 0, 2305843009213693952L);

		// Token: 0x04002DF4 RID: 11764
		private static EventDescriptor M3PAbortingWorkflowExecutionEvent = new EventDescriptor(45112, 1, 17, 5, 20, 6, 4611686018427388416L);

		// Token: 0x04002DF5 RID: 11765
		private static EventDescriptor M3PActivityExecutionFinishedEvent = new EventDescriptor(45119, 1, 17, 5, 20, 6, 4611686018427388416L);

		// Token: 0x04002DF6 RID: 11766
		private static EventDescriptor M3PActivityExecutionQueuedEvent = new EventDescriptor(45079, 1, 17, 5, 20, 6, 4611686018427388416L);

		// Token: 0x04002DF7 RID: 11767
		private static EventDescriptor M3PActivityExecutionStartedEvent = new EventDescriptor(45080, 1, 17, 5, 20, 6, 4611686018427388416L);

		// Token: 0x04002DF8 RID: 11768
		private static EventDescriptor M3PBeginContainerParentJobExecutionEvent = new EventDescriptor(46348, 1, 18, 4, 0, 0, 2305843009213693952L);

		// Token: 0x04002DF9 RID: 11769
		private static EventDescriptor M3PBeginCreateNewJobEvent = new EventDescriptor(46339, 1, 18, 4, 0, 0, 2305843009213693952L);

		// Token: 0x04002DFA RID: 11770
		private static EventDescriptor M3PBeginJobLogicEvent = new EventDescriptor(46342, 1, 18, 4, 0, 0, 2305843009213693952L);

		// Token: 0x04002DFB RID: 11771
		private static EventDescriptor M3PBeginProxyChildJobEventHandlerEvent = new EventDescriptor(46354, 1, 18, 4, 0, 0, 2305843009213693952L);

		// Token: 0x04002DFC RID: 11772
		private static EventDescriptor M3PBeginProxyJobEventHandlerEvent = new EventDescriptor(46352, 1, 18, 4, 0, 0, 2305843009213693952L);

		// Token: 0x04002DFD RID: 11773
		private static EventDescriptor M3PBeginProxyJobExecutionEvent = new EventDescriptor(46350, 1, 18, 4, 0, 0, 2305843009213693952L);

		// Token: 0x04002DFE RID: 11774
		private static EventDescriptor M3PBeginRunGarbageCollectionEvent = new EventDescriptor(46356, 1, 18, 4, 0, 0, 2305843009213693952L);

		// Token: 0x04002DFF RID: 11775
		private static EventDescriptor M3PBeginStartWorkflowApplicationEvent = new EventDescriptor(46337, 1, 18, 4, 0, 0, 2305843009213693952L);

		// Token: 0x04002E00 RID: 11776
		private static EventDescriptor M3PBeginWorkflowExecutionEvent = new EventDescriptor(46344, 1, 18, 4, 0, 0, 2305843009213693952L);

		// Token: 0x04002E01 RID: 11777
		private static EventDescriptor M3PCancellingWorkflowExecutionEvent = new EventDescriptor(45111, 1, 17, 5, 20, 6, 4611686018427388416L);

		// Token: 0x04002E02 RID: 11778
		private static EventDescriptor M3PChildWorkflowJobAdditionEvent = new EventDescriptor(46346, 1, 18, 4, 0, 0, 2305843009213693952L);

		// Token: 0x04002E03 RID: 11779
		private static EventDescriptor M3PEndContainerParentJobExecutionEvent = new EventDescriptor(46349, 1, 18, 4, 0, 0, 2305843009213693952L);

		// Token: 0x04002E04 RID: 11780
		private static EventDescriptor M3PEndCreateNewJobEvent = new EventDescriptor(46340, 1, 18, 4, 0, 0, 2305843009213693952L);

		// Token: 0x04002E05 RID: 11781
		private static EventDescriptor M3PEndJobLogicEvent = new EventDescriptor(46343, 1, 18, 4, 0, 0, 2305843009213693952L);

		// Token: 0x04002E06 RID: 11782
		private static EventDescriptor M3PEndpointDisabledEvent = new EventDescriptor(45124, 1, 17, 5, 20, 9, 4611686018427388416L);

		// Token: 0x04002E07 RID: 11783
		private static EventDescriptor M3PEndpointEnabledEvent = new EventDescriptor(45125, 1, 17, 5, 20, 9, 4611686018427388416L);

		// Token: 0x04002E08 RID: 11784
		private static EventDescriptor M3PEndpointModifiedEvent = new EventDescriptor(45122, 1, 17, 5, 20, 9, 4611686018427388416L);

		// Token: 0x04002E09 RID: 11785
		private static EventDescriptor M3PEndpointRegisteredEvent = new EventDescriptor(45121, 1, 17, 5, 20, 9, 4611686018427388416L);

		// Token: 0x04002E0A RID: 11786
		private static EventDescriptor M3PEndpointUnregisteredEvent = new EventDescriptor(45123, 1, 17, 5, 20, 9, 4611686018427388416L);

		// Token: 0x04002E0B RID: 11787
		private static EventDescriptor M3PEndProxyChildJobEventHandlerEvent = new EventDescriptor(46355, 1, 18, 4, 0, 0, 2305843009213693952L);

		// Token: 0x04002E0C RID: 11788
		private static EventDescriptor M3PEndProxyJobEventHandlerEvent = new EventDescriptor(46353, 1, 18, 4, 0, 0, 2305843009213693952L);

		// Token: 0x04002E0D RID: 11789
		private static EventDescriptor M3PEndProxyJobExecutionEvent = new EventDescriptor(46351, 1, 18, 4, 0, 0, 2305843009213693952L);

		// Token: 0x04002E0E RID: 11790
		private static EventDescriptor M3PEndRunGarbageCollectionEvent = new EventDescriptor(46357, 1, 18, 4, 0, 0, 2305843009213693952L);

		// Token: 0x04002E0F RID: 11791
		private static EventDescriptor M3PEndStartWorkflowApplicationEvent = new EventDescriptor(46338, 1, 18, 4, 0, 0, 2305843009213693952L);

		// Token: 0x04002E10 RID: 11792
		private static EventDescriptor M3PEndWorkflowExecutionEvent = new EventDescriptor(46345, 1, 18, 4, 0, 0, 2305843009213693952L);

		// Token: 0x04002E11 RID: 11793
		private static EventDescriptor M3PErrorImportingWorkflowFromXamlEvent = new EventDescriptor(45083, 1, 17, 5, 20, 6, 4611686018427388416L);

		// Token: 0x04002E12 RID: 11794
		private static EventDescriptor M3PForcedWorkflowShutdownErrorEvent = new EventDescriptor(45116, 1, 17, 5, 20, 6, 4611686018427388416L);

		// Token: 0x04002E13 RID: 11795
		private static EventDescriptor M3PForcedWorkflowShutdownFinishedEvent = new EventDescriptor(45115, 1, 17, 5, 20, 6, 4611686018427388416L);

		// Token: 0x04002E14 RID: 11796
		private static EventDescriptor M3PForcedWorkflowShutdownStartedEvent = new EventDescriptor(45114, 1, 17, 5, 20, 6, 4611686018427388416L);

		// Token: 0x04002E15 RID: 11797
		private static EventDescriptor M3PImportedWorkflowFromXamlEvent = new EventDescriptor(45082, 1, 17, 5, 20, 6, 4611686018427388416L);

		// Token: 0x04002E16 RID: 11798
		private static EventDescriptor M3PImportingWorkflowFromXamlEvent = new EventDescriptor(45081, 1, 17, 5, 20, 6, 4611686018427388416L);

		// Token: 0x04002E17 RID: 11799
		private static EventDescriptor M3PJobCreationCompleteEvent = new EventDescriptor(45106, 1, 17, 5, 20, 6, 4611686018427388416L);

		// Token: 0x04002E18 RID: 11800
		private static EventDescriptor M3PJobErrorEvent = new EventDescriptor(45102, 1, 17, 5, 20, 6, 4611686018427388416L);

		// Token: 0x04002E19 RID: 11801
		private static EventDescriptor M3PJobRemovedEvent = new EventDescriptor(45107, 1, 17, 5, 20, 6, 4611686018427388416L);

		// Token: 0x04002E1A RID: 11802
		private static EventDescriptor M3PJobRemoveErrorEvent = new EventDescriptor(45108, 1, 17, 5, 20, 6, 4611686018427388416L);

		// Token: 0x04002E1B RID: 11803
		private static EventDescriptor M3PJobStateChangedEvent = new EventDescriptor(45101, 1, 17, 5, 20, 6, 4611686018427388416L);

		// Token: 0x04002E1C RID: 11804
		private static EventDescriptor M3PLoadingWorkflowForExecutionEvent = new EventDescriptor(45109, 1, 17, 5, 20, 6, 4611686018427388416L);

		// Token: 0x04002E1D RID: 11805
		private static EventDescriptor M3POutOfProcessRunspaceStartedEvent = new EventDescriptor(45126, 1, 17, 5, 20, 6, 4611686018427388416L);

		// Token: 0x04002E1E RID: 11806
		private static EventDescriptor M3PParameterSplattingWasPerformedEvent = new EventDescriptor(45127, 1, 17, 5, 20, 6, 4611686018427388416L);

		// Token: 0x04002E1F RID: 11807
		private static EventDescriptor M3PParentJobCreatedEvent = new EventDescriptor(45105, 1, 17, 5, 20, 6, 4611686018427388416L);

		// Token: 0x04002E20 RID: 11808
		private static EventDescriptor M3PPersistenceStoreMaxSizeReachedEvent = new EventDescriptor(46358, 1, 16, 3, 0, 0, long.MinValue);

		// Token: 0x04002E21 RID: 11809
		private static EventDescriptor M3PPersistingWorkflowEvent = new EventDescriptor(45117, 1, 17, 5, 20, 6, 4611686018427388416L);

		// Token: 0x04002E22 RID: 11810
		private static EventDescriptor M3PProxyJobRemoteJobAssociationEvent = new EventDescriptor(46347, 1, 18, 4, 0, 0, 2305843009213693952L);

		// Token: 0x04002E23 RID: 11811
		private static EventDescriptor M3PRemoveJobStartedEvent = new EventDescriptor(45100, 1, 17, 5, 20, 6, 4611686018427388416L);

		// Token: 0x04002E24 RID: 11812
		private static EventDescriptor M3PRunspaceAvailabilityChangedEvent = new EventDescriptor(45090, 1, 17, 5, 20, 6, 4611686018427388416L);

		// Token: 0x04002E25 RID: 11813
		private static EventDescriptor M3PRunspaceStateChangedEvent = new EventDescriptor(45091, 1, 17, 5, 20, 6, 4611686018427388416L);

		// Token: 0x04002E26 RID: 11814
		private static EventDescriptor M3PTrackingGuidContainerParentJobCorrelationEvent = new EventDescriptor(46341, 1, 18, 4, 0, 0, 2305843009213693952L);

		// Token: 0x04002E27 RID: 11815
		private static EventDescriptor M3PUnloadingWorkflowEvent = new EventDescriptor(45113, 1, 17, 5, 20, 6, 4611686018427388416L);

		// Token: 0x04002E28 RID: 11816
		private static EventDescriptor M3PWorkflowActivityExecutionFailedEvent = new EventDescriptor(45089, 1, 17, 5, 20, 6, 4611686018427388416L);

		// Token: 0x04002E29 RID: 11817
		private static EventDescriptor M3PWorkflowActivityValidatedEvent = new EventDescriptor(45087, 1, 17, 5, 20, 6, 4611686018427388416L);

		// Token: 0x04002E2A RID: 11818
		private static EventDescriptor M3PWorkflowActivityValidationFailedEvent = new EventDescriptor(45088, 1, 17, 5, 20, 8, 4611686018427388416L);

		// Token: 0x04002E2B RID: 11819
		private static EventDescriptor M3PWorkflowCleanupPerformedEvent = new EventDescriptor(45096, 1, 17, 5, 20, 6, 4611686018427388416L);

		// Token: 0x04002E2C RID: 11820
		private static EventDescriptor M3PWorkflowDeletedFromDiskEvent = new EventDescriptor(45098, 1, 17, 5, 20, 6, 4611686018427388416L);

		// Token: 0x04002E2D RID: 11821
		private static EventDescriptor M3PWorkflowEngineStartedEvent = new EventDescriptor(45128, 1, 17, 5, 20, 5, 4611686018427388416L);

		// Token: 0x04002E2E RID: 11822
		private static EventDescriptor M3PWorkflowExecutionAbortedEvent = new EventDescriptor(45095, 1, 17, 5, 20, 6, 4611686018427388416L);

		// Token: 0x04002E2F RID: 11823
		private static EventDescriptor M3PWorkflowExecutionCancelledEvent = new EventDescriptor(45094, 1, 17, 5, 20, 6, 4611686018427388416L);

		// Token: 0x04002E30 RID: 11824
		private static EventDescriptor M3PWorkflowExecutionErrorEvent = new EventDescriptor(45120, 1, 17, 5, 20, 6, 4611686018427388416L);

		// Token: 0x04002E31 RID: 11825
		private static EventDescriptor M3PWorkflowExecutionFinishedEvent = new EventDescriptor(45110, 1, 17, 5, 20, 6, 4611686018427388416L);

		// Token: 0x04002E32 RID: 11826
		private static EventDescriptor M3PWorkflowExecutionStartedEvent = new EventDescriptor(45064, 1, 17, 5, 20, 6, 4611686018427388416L);

		// Token: 0x04002E33 RID: 11827
		private static EventDescriptor M3PWorkflowJobCreatedEvent = new EventDescriptor(45104, 1, 17, 5, 20, 6, 4611686018427388416L);

		// Token: 0x04002E34 RID: 11828
		private static EventDescriptor M3PWorkflowLoadedForExecutionEvent = new EventDescriptor(45092, 1, 17, 5, 20, 6, 4611686018427388416L);

		// Token: 0x04002E35 RID: 11829
		private static EventDescriptor M3PWorkflowLoadedFromDiskEvent = new EventDescriptor(45097, 1, 17, 5, 20, 6, 4611686018427388416L);

		// Token: 0x04002E36 RID: 11830
		private static EventDescriptor M3PWorkflowManagerCheckpointEvent = new EventDescriptor(45129, 1, 18, 4, 0, 0, 2305843009213694464L);

		// Token: 0x04002E37 RID: 11831
		private static EventDescriptor M3PWorkflowPersistedEvent = new EventDescriptor(45118, 1, 17, 5, 20, 6, 4611686018427388416L);

		// Token: 0x04002E38 RID: 11832
		private static EventDescriptor M3PWorkflowPluginRequestedToShutdownEvent = new EventDescriptor(45072, 1, 17, 5, 20, 5, 4611686018427388416L);

		// Token: 0x04002E39 RID: 11833
		private static EventDescriptor M3PWorkflowPluginRestartedEvent = new EventDescriptor(45073, 1, 17, 5, 20, 5, 4611686018427388416L);

		// Token: 0x04002E3A RID: 11834
		private static EventDescriptor M3PWorkflowPluginStartedEvent = new EventDescriptor(45063, 1, 17, 5, 20, 5, 4611686018427388416L);

		// Token: 0x04002E3B RID: 11835
		private static EventDescriptor M3PWorkflowQuotaViolatedEvent = new EventDescriptor(45075, 1, 17, 5, 20, 6, 4611686018427388416L);

		// Token: 0x04002E3C RID: 11836
		private static EventDescriptor M3PWorkflowResumedEvent = new EventDescriptor(45076, 1, 17, 5, 20, 6, 4611686018427388416L);

		// Token: 0x04002E3D RID: 11837
		private static EventDescriptor M3PWorkflowResumingEvent = new EventDescriptor(45074, 1, 17, 5, 20, 6, 4611686018427388416L);

		// Token: 0x04002E3E RID: 11838
		private static EventDescriptor M3PWorkflowRunspacePoolCreatedEvent = new EventDescriptor(45078, 1, 17, 5, 20, 6, 4611686018427388416L);

		// Token: 0x04002E3F RID: 11839
		private static EventDescriptor M3PWorkflowStateChangedEvent = new EventDescriptor(45065, 1, 17, 5, 20, 6, 4611686018427388416L);

		// Token: 0x04002E40 RID: 11840
		private static EventDescriptor M3PWorkflowUnloadedEvent = new EventDescriptor(45093, 1, 17, 5, 20, 6, 4611686018427388416L);

		// Token: 0x04002E41 RID: 11841
		private static EventDescriptor M3PWorkflowValidationErrorEvent = new EventDescriptor(45086, 1, 17, 5, 20, 8, 4611686018427388416L);

		// Token: 0x04002E42 RID: 11842
		private static EventDescriptor M3PWorkflowValidationFinishedEvent = new EventDescriptor(45085, 1, 17, 5, 20, 8, 4611686018427388416L);

		// Token: 0x04002E43 RID: 11843
		private static EventDescriptor M3PWorkflowValidationStartedEvent = new EventDescriptor(45084, 1, 17, 5, 20, 8, 4611686018427388416L);
	}
}
