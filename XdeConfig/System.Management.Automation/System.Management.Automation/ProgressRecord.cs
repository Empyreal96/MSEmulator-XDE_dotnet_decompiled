using System;
using System.Globalization;
using System.Runtime.Serialization;

namespace System.Management.Automation
{
	// Token: 0x02000032 RID: 50
	[DataContract]
	public class ProgressRecord
	{
		// Token: 0x06000276 RID: 630 RVA: 0x00009698 File Offset: 0x00007898
		public ProgressRecord(int activityId, string activity, string statusDescription)
		{
			if (activityId < 0)
			{
				throw PSTraceSource.NewArgumentOutOfRangeException("activityId", activityId, ProgressRecordStrings.ArgMayNotBeNegative, new object[]
				{
					"activityId"
				});
			}
			if (string.IsNullOrEmpty(activity))
			{
				throw PSTraceSource.NewArgumentException("activity", ProgressRecordStrings.ArgMayNotBeNullOrEmpty, new object[]
				{
					"activity"
				});
			}
			if (string.IsNullOrEmpty(statusDescription))
			{
				throw PSTraceSource.NewArgumentException("activity", ProgressRecordStrings.ArgMayNotBeNullOrEmpty, new object[]
				{
					"statusDescription"
				});
			}
			this.id = activityId;
			this.activity = activity;
			this.status = statusDescription;
		}

		// Token: 0x06000277 RID: 631 RVA: 0x00009750 File Offset: 0x00007950
		internal ProgressRecord(ProgressRecord other)
		{
			this.activity = other.activity;
			this.currentOperation = other.currentOperation;
			this.id = other.id;
			this.parentId = other.parentId;
			this.percent = other.percent;
			this.secondsRemaining = other.secondsRemaining;
			this.status = other.status;
			this.type = other.type;
		}

		// Token: 0x06000278 RID: 632 RVA: 0x000097D8 File Offset: 0x000079D8
		internal ProgressRecord()
		{
		}

		// Token: 0x1700009B RID: 155
		// (get) Token: 0x06000279 RID: 633 RVA: 0x000097F5 File Offset: 0x000079F5
		public int ActivityId
		{
			get
			{
				return this.id;
			}
		}

		// Token: 0x1700009C RID: 156
		// (get) Token: 0x0600027A RID: 634 RVA: 0x000097FD File Offset: 0x000079FD
		// (set) Token: 0x0600027B RID: 635 RVA: 0x00009805 File Offset: 0x00007A05
		public int ParentActivityId
		{
			get
			{
				return this.parentId;
			}
			set
			{
				if (value == this.ActivityId)
				{
					throw PSTraceSource.NewArgumentException("value", ProgressRecordStrings.ParentActivityIdCantBeActivityId, new object[0]);
				}
				this.parentId = value;
			}
		}

		// Token: 0x1700009D RID: 157
		// (get) Token: 0x0600027C RID: 636 RVA: 0x0000982D File Offset: 0x00007A2D
		// (set) Token: 0x0600027D RID: 637 RVA: 0x00009838 File Offset: 0x00007A38
		public string Activity
		{
			get
			{
				return this.activity;
			}
			set
			{
				if (string.IsNullOrEmpty(value))
				{
					throw PSTraceSource.NewArgumentException("value", ProgressRecordStrings.ArgMayNotBeNullOrEmpty, new object[]
					{
						"value"
					});
				}
				this.activity = value;
			}
		}

		// Token: 0x1700009E RID: 158
		// (get) Token: 0x0600027E RID: 638 RVA: 0x00009874 File Offset: 0x00007A74
		// (set) Token: 0x0600027F RID: 639 RVA: 0x0000987C File Offset: 0x00007A7C
		public string StatusDescription
		{
			get
			{
				return this.status;
			}
			set
			{
				if (string.IsNullOrEmpty(value))
				{
					throw PSTraceSource.NewArgumentException("value", ProgressRecordStrings.ArgMayNotBeNullOrEmpty, new object[]
					{
						"value"
					});
				}
				this.status = value;
			}
		}

		// Token: 0x1700009F RID: 159
		// (get) Token: 0x06000280 RID: 640 RVA: 0x000098B8 File Offset: 0x00007AB8
		// (set) Token: 0x06000281 RID: 641 RVA: 0x000098C0 File Offset: 0x00007AC0
		public string CurrentOperation
		{
			get
			{
				return this.currentOperation;
			}
			set
			{
				this.currentOperation = value;
			}
		}

		// Token: 0x170000A0 RID: 160
		// (get) Token: 0x06000282 RID: 642 RVA: 0x000098C9 File Offset: 0x00007AC9
		// (set) Token: 0x06000283 RID: 643 RVA: 0x000098D4 File Offset: 0x00007AD4
		public int PercentComplete
		{
			get
			{
				return this.percent;
			}
			set
			{
				if (value > 100)
				{
					throw PSTraceSource.NewArgumentOutOfRangeException("value", value, ProgressRecordStrings.PercentMayNotBeMoreThan100, new object[]
					{
						"PercentComplete"
					});
				}
				this.percent = value;
			}
		}

		// Token: 0x170000A1 RID: 161
		// (get) Token: 0x06000284 RID: 644 RVA: 0x00009913 File Offset: 0x00007B13
		// (set) Token: 0x06000285 RID: 645 RVA: 0x0000991B File Offset: 0x00007B1B
		public int SecondsRemaining
		{
			get
			{
				return this.secondsRemaining;
			}
			set
			{
				this.secondsRemaining = value;
			}
		}

		// Token: 0x170000A2 RID: 162
		// (get) Token: 0x06000286 RID: 646 RVA: 0x00009924 File Offset: 0x00007B24
		// (set) Token: 0x06000287 RID: 647 RVA: 0x0000992C File Offset: 0x00007B2C
		public ProgressRecordType RecordType
		{
			get
			{
				return this.type;
			}
			set
			{
				if (value != ProgressRecordType.Completed && value != ProgressRecordType.Processing)
				{
					throw PSTraceSource.NewArgumentException("value");
				}
				this.type = value;
			}
		}

		// Token: 0x06000288 RID: 648 RVA: 0x00009948 File Offset: 0x00007B48
		public override string ToString()
		{
			return string.Format(CultureInfo.CurrentCulture, "parent = {0} id = {1} act = {2} stat = {3} cur = {4} pct = {5} sec = {6} type = {7}", new object[]
			{
				this.parentId,
				this.id,
				this.activity,
				this.status,
				this.currentOperation,
				this.percent,
				this.secondsRemaining,
				this.type
			});
		}

		// Token: 0x06000289 RID: 649 RVA: 0x000099D0 File Offset: 0x00007BD0
		internal static int? GetSecondsRemaining(DateTime startTime, double percentageComplete)
		{
			if (percentageComplete < 1E-05 || double.IsNaN(percentageComplete))
			{
				return null;
			}
			DateTime utcNow = DateTime.UtcNow;
			TimeSpan t = utcNow - startTime;
			TimeSpan t2;
			try
			{
				t2 = TimeSpan.FromMilliseconds(t.TotalMilliseconds / percentageComplete);
			}
			catch (OverflowException)
			{
				return null;
			}
			catch (ArgumentException)
			{
				return null;
			}
			return new int?((int)(t2 - t).TotalSeconds);
		}

		// Token: 0x0600028A RID: 650 RVA: 0x00009A70 File Offset: 0x00007C70
		internal static int GetPercentageComplete(DateTime startTime, TimeSpan expectedDuration)
		{
			DateTime utcNow = DateTime.UtcNow;
			if (startTime > utcNow)
			{
				throw new ArgumentOutOfRangeException("startTime");
			}
			if (expectedDuration <= TimeSpan.Zero)
			{
				throw new ArgumentOutOfRangeException("expectedDuration");
			}
			TimeSpan timeSpan = utcNow - startTime;
			double num = expectedDuration.TotalSeconds / 9.0;
			double num2 = 100.0 * num;
			double num3 = num2 / (timeSpan.TotalSeconds + num);
			double d = 100.0 - num3;
			return (int)Math.Floor(d);
		}

		// Token: 0x0600028B RID: 651 RVA: 0x00009AF8 File Offset: 0x00007CF8
		internal static ProgressRecord FromPSObjectForRemoting(PSObject progressAsPSObject)
		{
			if (progressAsPSObject == null)
			{
				throw PSTraceSource.NewArgumentNullException("progressAsPSObject");
			}
			string propertyValue = RemotingDecoder.GetPropertyValue<string>(progressAsPSObject, "Activity");
			int propertyValue2 = RemotingDecoder.GetPropertyValue<int>(progressAsPSObject, "ActivityId");
			string propertyValue3 = RemotingDecoder.GetPropertyValue<string>(progressAsPSObject, "StatusDescription");
			return new ProgressRecord(propertyValue2, propertyValue, propertyValue3)
			{
				CurrentOperation = RemotingDecoder.GetPropertyValue<string>(progressAsPSObject, "CurrentOperation"),
				ParentActivityId = RemotingDecoder.GetPropertyValue<int>(progressAsPSObject, "ParentActivityId"),
				PercentComplete = RemotingDecoder.GetPropertyValue<int>(progressAsPSObject, "PercentComplete"),
				RecordType = RemotingDecoder.GetPropertyValue<ProgressRecordType>(progressAsPSObject, "Type"),
				SecondsRemaining = RemotingDecoder.GetPropertyValue<int>(progressAsPSObject, "SecondsRemaining")
			};
		}

		// Token: 0x0600028C RID: 652 RVA: 0x00009B98 File Offset: 0x00007D98
		internal PSObject ToPSObjectForRemoting()
		{
			PSObject psobject = RemotingEncoder.CreateEmptyPSObject();
			psobject.Properties.Add(new PSNoteProperty("Activity", this.Activity));
			psobject.Properties.Add(new PSNoteProperty("ActivityId", this.ActivityId));
			psobject.Properties.Add(new PSNoteProperty("StatusDescription", this.StatusDescription));
			psobject.Properties.Add(new PSNoteProperty("CurrentOperation", this.CurrentOperation));
			psobject.Properties.Add(new PSNoteProperty("ParentActivityId", this.ParentActivityId));
			psobject.Properties.Add(new PSNoteProperty("PercentComplete", this.PercentComplete));
			psobject.Properties.Add(new PSNoteProperty("Type", this.RecordType));
			psobject.Properties.Add(new PSNoteProperty("SecondsRemaining", this.SecondsRemaining));
			return psobject;
		}

		// Token: 0x040000D2 RID: 210
		[DataMember]
		private int id;

		// Token: 0x040000D3 RID: 211
		[DataMember]
		private int parentId = -1;

		// Token: 0x040000D4 RID: 212
		[DataMember]
		private string activity;

		// Token: 0x040000D5 RID: 213
		[DataMember]
		private string status;

		// Token: 0x040000D6 RID: 214
		[DataMember]
		private string currentOperation;

		// Token: 0x040000D7 RID: 215
		[DataMember]
		private int percent = -1;

		// Token: 0x040000D8 RID: 216
		[DataMember]
		private int secondsRemaining = -1;

		// Token: 0x040000D9 RID: 217
		[DataMember]
		private ProgressRecordType type;
	}
}
