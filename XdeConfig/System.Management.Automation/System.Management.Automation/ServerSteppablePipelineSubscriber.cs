using System;

namespace System.Management.Automation
{
	// Token: 0x02000304 RID: 772
	internal class ServerSteppablePipelineSubscriber
	{
		// Token: 0x06002474 RID: 9332 RVA: 0x000CCA68 File Offset: 0x000CAC68
		internal void SubscribeEvents(ServerSteppablePipelineDriver driver)
		{
			lock (this.syncObject)
			{
				if (!this.initialized)
				{
					this.eventManager = (driver.LocalPowerShell.Runspace.Events as PSLocalEventManager);
					if (this.eventManager != null)
					{
						this.startSubscriber = this.eventManager.SubscribeEvent(this, "StartSteppablePipeline", Guid.NewGuid().ToString(), null, new PSEventReceivedEventHandler(this.HandleStartEvent), true, false, true, 0);
						this.processSubscriber = this.eventManager.SubscribeEvent(this, "RunProcessRecord", Guid.NewGuid().ToString(), null, new PSEventReceivedEventHandler(this.HandleProcessRecord), true, false, true, 0);
					}
					this.initialized = true;
				}
			}
		}

		// Token: 0x1400006F RID: 111
		// (add) Token: 0x06002475 RID: 9333 RVA: 0x000CCB50 File Offset: 0x000CAD50
		// (remove) Token: 0x06002476 RID: 9334 RVA: 0x000CCB88 File Offset: 0x000CAD88
		public event EventHandler<EventArgs> StartSteppablePipeline;

		// Token: 0x14000070 RID: 112
		// (add) Token: 0x06002477 RID: 9335 RVA: 0x000CCBC0 File Offset: 0x000CADC0
		// (remove) Token: 0x06002478 RID: 9336 RVA: 0x000CCBF8 File Offset: 0x000CADF8
		public event EventHandler<EventArgs> RunProcessRecord;

		// Token: 0x06002479 RID: 9337 RVA: 0x000CCC30 File Offset: 0x000CAE30
		private void HandleStartEvent(object sender, PSEventArgs args)
		{
			ServerSteppablePipelineDriverEventArg serverSteppablePipelineDriverEventArg = args.SourceEventArgs as ServerSteppablePipelineDriverEventArg;
			ServerSteppablePipelineDriver steppableDriver = serverSteppablePipelineDriverEventArg.SteppableDriver;
			Exception ex = null;
			try
			{
				using (ExecutionContextForStepping.PrepareExecutionContext(steppableDriver.LocalPowerShell.GetContextFromTLS(), steppableDriver.LocalPowerShell.InformationalBuffers, steppableDriver.RemoteHost))
				{
					steppableDriver.SteppablePipeline = steppableDriver.LocalPowerShell.GetSteppablePipeline();
					steppableDriver.SteppablePipeline.Begin(!steppableDriver.NoInput);
				}
				if (steppableDriver.NoInput)
				{
					steppableDriver.HandleInputEndReceived(this, EventArgs.Empty);
				}
			}
			catch (Exception ex2)
			{
				ex = ex2;
			}
			if (ex != null)
			{
				steppableDriver.SetState(PSInvocationState.Failed, ex);
			}
		}

		// Token: 0x0600247A RID: 9338 RVA: 0x000CCCEC File Offset: 0x000CAEEC
		private void HandleProcessRecord(object sender, PSEventArgs args)
		{
			ServerSteppablePipelineDriverEventArg serverSteppablePipelineDriverEventArg = args.SourceEventArgs as ServerSteppablePipelineDriverEventArg;
			ServerSteppablePipelineDriver steppableDriver = serverSteppablePipelineDriverEventArg.SteppableDriver;
			lock (steppableDriver.SyncObject)
			{
				if (steppableDriver.SteppablePipeline == null)
				{
					return;
				}
				if (steppableDriver.ProcessingInput)
				{
					return;
				}
				steppableDriver.ProcessingInput = true;
				steppableDriver.Pulsed = false;
			}
			bool flag2 = false;
			Exception ex = null;
			try
			{
				using (ExecutionContextForStepping.PrepareExecutionContext(steppableDriver.LocalPowerShell.GetContextFromTLS(), steppableDriver.LocalPowerShell.InformationalBuffers, steppableDriver.RemoteHost))
				{
					bool flag3 = false;
					while (steppableDriver.PipelineState == PSInvocationState.Running)
					{
						if (!steppableDriver.InputEnumerator.MoveNext())
						{
							flag2 = true;
							if (!steppableDriver.NoInput || flag3)
							{
								goto IL_1A4;
							}
						}
						flag3 = true;
						Array array = new int[0];
						if (steppableDriver.NoInput)
						{
							array = steppableDriver.SteppablePipeline.Process();
						}
						else
						{
							array = steppableDriver.SteppablePipeline.Process(steppableDriver.InputEnumerator.Current);
						}
						foreach (object obj2 in array)
						{
							if (steppableDriver.PipelineState != PSInvocationState.Running)
							{
								steppableDriver.SetState(steppableDriver.PipelineState, null);
								return;
							}
							steppableDriver.DataStructureHandler.SendOutputDataToClient(PSObject.AsPSObject(obj2));
						}
						lock (steppableDriver.SyncObject)
						{
							steppableDriver.TotalObjectsProcessed++;
							if (steppableDriver.TotalObjectsProcessed < steppableDriver.Input.Count)
							{
								continue;
							}
						}
						IL_1A4:
						goto IL_1B2;
					}
					steppableDriver.SetState(steppableDriver.PipelineState, null);
					return;
				}
				IL_1B2:;
			}
			catch (Exception ex2)
			{
				CommandProcessorBase.CheckForSevereException(ex2);
				ex = ex2;
			}
			finally
			{
				lock (steppableDriver.SyncObject)
				{
					steppableDriver.ProcessingInput = false;
					steppableDriver.CheckAndPulseForProcessing(false);
				}
				if (steppableDriver.PipelineState == PSInvocationState.Stopping)
				{
					steppableDriver.PerformStop();
				}
			}
			if (flag2)
			{
				try
				{
					using (ExecutionContextForStepping.PrepareExecutionContext(steppableDriver.LocalPowerShell.GetContextFromTLS(), steppableDriver.LocalPowerShell.InformationalBuffers, steppableDriver.RemoteHost))
					{
						Array array2 = steppableDriver.SteppablePipeline.End();
						foreach (object obj5 in array2)
						{
							if (steppableDriver.PipelineState != PSInvocationState.Running)
							{
								steppableDriver.SetState(steppableDriver.PipelineState, null);
								return;
							}
							steppableDriver.DataStructureHandler.SendOutputDataToClient(PSObject.AsPSObject(obj5));
						}
						steppableDriver.SetState(PSInvocationState.Completed, null);
						return;
					}
				}
				catch (Exception ex3)
				{
					CommandProcessorBase.CheckForSevereException(ex3);
					ex = ex3;
				}
				finally
				{
					if (steppableDriver.PipelineState == PSInvocationState.Stopping)
					{
						steppableDriver.PerformStop();
					}
				}
			}
			if (ex != null)
			{
				steppableDriver.SetState(PSInvocationState.Failed, ex);
			}
		}

		// Token: 0x0600247B RID: 9339 RVA: 0x000CD0E0 File Offset: 0x000CB2E0
		internal void FireStartSteppablePipeline(ServerSteppablePipelineDriver driver)
		{
			lock (this.syncObject)
			{
				if (this.eventManager != null)
				{
					this.eventManager.GenerateEvent(this.startSubscriber.SourceIdentifier, this, new object[]
					{
						new ServerSteppablePipelineDriverEventArg(driver)
					}, null, true, false);
				}
			}
		}

		// Token: 0x0600247C RID: 9340 RVA: 0x000CD150 File Offset: 0x000CB350
		internal void FireHandleProcessRecord(ServerSteppablePipelineDriver driver)
		{
			lock (this.syncObject)
			{
				if (this.eventManager != null)
				{
					this.eventManager.GenerateEvent(this.processSubscriber.SourceIdentifier, this, new object[]
					{
						new ServerSteppablePipelineDriverEventArg(driver)
					}, null, true, false);
				}
			}
		}

		// Token: 0x040011EF RID: 4591
		private object syncObject = new object();

		// Token: 0x040011F0 RID: 4592
		private bool initialized;

		// Token: 0x040011F1 RID: 4593
		private PSLocalEventManager eventManager;

		// Token: 0x040011F2 RID: 4594
		private PSEventSubscriber startSubscriber;

		// Token: 0x040011F3 RID: 4595
		private PSEventSubscriber processSubscriber;
	}
}
