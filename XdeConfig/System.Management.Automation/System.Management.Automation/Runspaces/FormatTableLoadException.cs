using System;
using System.Collections.ObjectModel;
using System.Globalization;
using System.Management.Automation.Internal;
using System.Runtime.Serialization;
using System.Security.Permissions;

namespace System.Management.Automation.Runspaces
{
	// Token: 0x02000966 RID: 2406
	[Serializable]
	public class FormatTableLoadException : RuntimeException
	{
		// Token: 0x06005839 RID: 22585 RVA: 0x001CB088 File Offset: 0x001C9288
		public FormatTableLoadException()
		{
			this.SetDefaultErrorRecord();
		}

		// Token: 0x0600583A RID: 22586 RVA: 0x001CB096 File Offset: 0x001C9296
		public FormatTableLoadException(string message) : base(message)
		{
			this.SetDefaultErrorRecord();
		}

		// Token: 0x0600583B RID: 22587 RVA: 0x001CB0A5 File Offset: 0x001C92A5
		public FormatTableLoadException(string message, Exception innerException) : base(message, innerException)
		{
			this.SetDefaultErrorRecord();
		}

		// Token: 0x0600583C RID: 22588 RVA: 0x001CB0B5 File Offset: 0x001C92B5
		internal FormatTableLoadException(Collection<string> loadErrors) : base(StringUtil.Format(FormatAndOutXmlLoadingStrings.FormatTableLoadErrors, new object[0]))
		{
			this.errors = loadErrors;
			this.SetDefaultErrorRecord();
		}

		// Token: 0x0600583D RID: 22589 RVA: 0x001CB0DC File Offset: 0x001C92DC
		protected FormatTableLoadException(SerializationInfo info, StreamingContext context) : base(info, context)
		{
			if (info == null)
			{
				throw new PSArgumentNullException("info");
			}
			int @int = info.GetInt32("ErrorCount");
			if (@int > 0)
			{
				this.errors = new Collection<string>();
				for (int i = 0; i < @int; i++)
				{
					string name = string.Format(CultureInfo.InvariantCulture, "Error{0}", new object[]
					{
						i
					});
					this.errors.Add(info.GetString(name));
				}
			}
		}

		// Token: 0x0600583E RID: 22590 RVA: 0x001CB15C File Offset: 0x001C935C
		[SecurityPermission(SecurityAction.Demand, SerializationFormatter = true)]
		public override void GetObjectData(SerializationInfo info, StreamingContext context)
		{
			if (info == null)
			{
				throw new PSArgumentNullException("info");
			}
			base.GetObjectData(info, context);
			if (this.errors != null)
			{
				int count = this.errors.Count;
				info.AddValue("ErrorCount", count);
				for (int i = 0; i < count; i++)
				{
					string name = string.Format(CultureInfo.InvariantCulture, "Error{0}", new object[]
					{
						i
					});
					info.AddValue(name, this.errors[i]);
				}
			}
		}

		// Token: 0x0600583F RID: 22591 RVA: 0x001CB1DF File Offset: 0x001C93DF
		protected void SetDefaultErrorRecord()
		{
			base.SetErrorCategory(ErrorCategory.InvalidData);
			base.SetErrorId(typeof(FormatTableLoadException).FullName);
		}

		// Token: 0x170011D4 RID: 4564
		// (get) Token: 0x06005840 RID: 22592 RVA: 0x001CB1FD File Offset: 0x001C93FD
		public Collection<string> Errors
		{
			get
			{
				return this.errors;
			}
		}

		// Token: 0x04002F56 RID: 12118
		private Collection<string> errors;
	}
}
