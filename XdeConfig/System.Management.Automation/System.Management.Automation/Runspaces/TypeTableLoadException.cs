using System;
using System.Collections.ObjectModel;
using System.Globalization;
using System.Runtime.Serialization;
using System.Security.Permissions;

namespace System.Management.Automation.Runspaces
{
	// Token: 0x0200015B RID: 347
	[Serializable]
	public class TypeTableLoadException : RuntimeException
	{
		// Token: 0x060011BC RID: 4540 RVA: 0x0006365C File Offset: 0x0006185C
		public TypeTableLoadException()
		{
			this.SetDefaultErrorRecord();
		}

		// Token: 0x060011BD RID: 4541 RVA: 0x0006366A File Offset: 0x0006186A
		public TypeTableLoadException(string message) : base(message)
		{
			this.SetDefaultErrorRecord();
		}

		// Token: 0x060011BE RID: 4542 RVA: 0x00063679 File Offset: 0x00061879
		public TypeTableLoadException(string message, Exception innerException) : base(message, innerException)
		{
			this.SetDefaultErrorRecord();
		}

		// Token: 0x060011BF RID: 4543 RVA: 0x00063689 File Offset: 0x00061889
		internal TypeTableLoadException(Collection<string> loadErrors) : base(TypesXmlStrings.TypeTableLoadErrors)
		{
			this.errors = loadErrors;
			this.SetDefaultErrorRecord();
		}

		// Token: 0x060011C0 RID: 4544 RVA: 0x000636A4 File Offset: 0x000618A4
		protected TypeTableLoadException(SerializationInfo info, StreamingContext context) : base(info, context)
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

		// Token: 0x060011C1 RID: 4545 RVA: 0x00063724 File Offset: 0x00061924
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

		// Token: 0x060011C2 RID: 4546 RVA: 0x000637A7 File Offset: 0x000619A7
		protected void SetDefaultErrorRecord()
		{
			base.SetErrorCategory(ErrorCategory.InvalidData);
			base.SetErrorId(typeof(TypeTableLoadException).FullName);
		}

		// Token: 0x17000457 RID: 1111
		// (get) Token: 0x060011C3 RID: 4547 RVA: 0x000637C5 File Offset: 0x000619C5
		public Collection<string> Errors
		{
			get
			{
				return this.errors;
			}
		}

		// Token: 0x0400079E RID: 1950
		private Collection<string> errors;
	}
}
