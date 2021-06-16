using System;
using System.Globalization;
using System.Reflection;
using System.Resources;
using System.Runtime.Serialization;
using System.Security.Permissions;

namespace System.Management.Automation
{
	// Token: 0x02000026 RID: 38
	[Serializable]
	public class ErrorDetails : ISerializable
	{
		// Token: 0x0600016F RID: 367 RVA: 0x000070E0 File Offset: 0x000052E0
		public ErrorDetails(string message)
		{
			this._message = message;
		}

		// Token: 0x06000170 RID: 368 RVA: 0x00007105 File Offset: 0x00005305
		public ErrorDetails(Cmdlet cmdlet, string baseName, string resourceId, params object[] args)
		{
			this._message = this.BuildMessage(cmdlet, baseName, resourceId, args);
		}

		// Token: 0x06000171 RID: 369 RVA: 0x00007134 File Offset: 0x00005334
		public ErrorDetails(IResourceSupplier resourceSupplier, string baseName, string resourceId, params object[] args)
		{
			this._message = this.BuildMessage(resourceSupplier, baseName, resourceId, args);
		}

		// Token: 0x06000172 RID: 370 RVA: 0x00007163 File Offset: 0x00005363
		public ErrorDetails(Assembly assembly, string baseName, string resourceId, params object[] args)
		{
			this._message = this.BuildMessage(assembly, baseName, resourceId, args);
		}

		// Token: 0x06000173 RID: 371 RVA: 0x00007192 File Offset: 0x00005392
		internal ErrorDetails(ErrorDetails errorDetails)
		{
			this._message = errorDetails._message;
			this._recommendedAction = errorDetails._recommendedAction;
		}

		// Token: 0x06000174 RID: 372 RVA: 0x000071C8 File Offset: 0x000053C8
		protected ErrorDetails(SerializationInfo info, StreamingContext context)
		{
			this._message = info.GetString("ErrorDetails_Message");
			this._recommendedAction = info.GetString("ErrorDetails_RecommendedAction");
		}

		// Token: 0x06000175 RID: 373 RVA: 0x00007208 File Offset: 0x00005408
		[SecurityPermission(SecurityAction.Demand, SerializationFormatter = true)]
		public virtual void GetObjectData(SerializationInfo info, StreamingContext context)
		{
			if (info != null)
			{
				info.AddValue("ErrorDetails_Message", this._message);
				info.AddValue("ErrorDetails_RecommendedAction", this._recommendedAction);
			}
		}

		// Token: 0x1700005A RID: 90
		// (get) Token: 0x06000176 RID: 374 RVA: 0x0000722F File Offset: 0x0000542F
		public string Message
		{
			get
			{
				return ErrorRecord.NotNull(this._message);
			}
		}

		// Token: 0x1700005B RID: 91
		// (get) Token: 0x06000177 RID: 375 RVA: 0x0000723C File Offset: 0x0000543C
		// (set) Token: 0x06000178 RID: 376 RVA: 0x00007249 File Offset: 0x00005449
		public string RecommendedAction
		{
			get
			{
				return ErrorRecord.NotNull(this._recommendedAction);
			}
			set
			{
				this._recommendedAction = value;
			}
		}

		// Token: 0x1700005C RID: 92
		// (get) Token: 0x06000179 RID: 377 RVA: 0x00007252 File Offset: 0x00005452
		// (set) Token: 0x0600017A RID: 378 RVA: 0x0000725A File Offset: 0x0000545A
		internal Exception TextLookupError
		{
			get
			{
				return this._textLookupError;
			}
			set
			{
				this._textLookupError = value;
			}
		}

		// Token: 0x0600017B RID: 379 RVA: 0x00007263 File Offset: 0x00005463
		public override string ToString()
		{
			return this.Message;
		}

		// Token: 0x0600017C RID: 380 RVA: 0x0000726C File Offset: 0x0000546C
		private string BuildMessage(Cmdlet cmdlet, string baseName, string resourceId, params object[] args)
		{
			if (cmdlet == null)
			{
				throw PSTraceSource.NewArgumentNullException("cmdlet");
			}
			if (string.IsNullOrEmpty(baseName))
			{
				throw PSTraceSource.NewArgumentNullException("baseName");
			}
			if (string.IsNullOrEmpty(resourceId))
			{
				throw PSTraceSource.NewArgumentNullException("resourceId");
			}
			string template = "";
			try
			{
				template = cmdlet.GetResourceString(baseName, resourceId);
			}
			catch (MissingManifestResourceException textLookupError)
			{
				this._textLookupError = textLookupError;
				return "";
			}
			catch (ArgumentException textLookupError2)
			{
				this._textLookupError = textLookupError2;
				return "";
			}
			return this.BuildMessage(template, baseName, resourceId, args);
		}

		// Token: 0x0600017D RID: 381 RVA: 0x00007308 File Offset: 0x00005508
		private string BuildMessage(IResourceSupplier resourceSupplier, string baseName, string resourceId, params object[] args)
		{
			if (resourceSupplier == null)
			{
				throw PSTraceSource.NewArgumentNullException("resourceSupplier");
			}
			if (string.IsNullOrEmpty(baseName))
			{
				throw PSTraceSource.NewArgumentNullException("baseName");
			}
			if (string.IsNullOrEmpty(resourceId))
			{
				throw PSTraceSource.NewArgumentNullException("resourceId");
			}
			string template = "";
			try
			{
				template = resourceSupplier.GetResourceString(baseName, resourceId);
			}
			catch (MissingManifestResourceException textLookupError)
			{
				this._textLookupError = textLookupError;
				return "";
			}
			catch (ArgumentException textLookupError2)
			{
				this._textLookupError = textLookupError2;
				return "";
			}
			return this.BuildMessage(template, baseName, resourceId, args);
		}

		// Token: 0x0600017E RID: 382 RVA: 0x000073A4 File Offset: 0x000055A4
		private string BuildMessage(Assembly assembly, string baseName, string resourceId, params object[] args)
		{
			if (null == assembly)
			{
				throw PSTraceSource.NewArgumentNullException("assembly");
			}
			if (string.IsNullOrEmpty(baseName))
			{
				throw PSTraceSource.NewArgumentNullException("baseName");
			}
			if (string.IsNullOrEmpty(resourceId))
			{
				throw PSTraceSource.NewArgumentNullException("resourceId");
			}
			string template = "";
			ResourceManager resourceManager = ResourceManagerCache.GetResourceManager(assembly, baseName);
			try
			{
				template = resourceManager.GetString(resourceId, CultureInfo.CurrentUICulture);
			}
			catch (MissingManifestResourceException textLookupError)
			{
				this._textLookupError = textLookupError;
				return "";
			}
			return this.BuildMessage(template, baseName, resourceId, args);
		}

		// Token: 0x0600017F RID: 383 RVA: 0x00007438 File Offset: 0x00005638
		private string BuildMessage(string template, string baseName, string resourceId, params object[] args)
		{
			if (string.IsNullOrEmpty(template) || 1 >= template.Trim().Length)
			{
				this._textLookupError = PSTraceSource.NewInvalidOperationException(ErrorPackage.ErrorDetailsEmptyTemplate, new object[]
				{
					baseName,
					resourceId
				});
				return "";
			}
			string result;
			try
			{
				result = string.Format(CultureInfo.CurrentCulture, template, args);
			}
			catch (FormatException textLookupError)
			{
				this._textLookupError = textLookupError;
				result = "";
			}
			return result;
		}

		// Token: 0x04000093 RID: 147
		private string _message = "";

		// Token: 0x04000094 RID: 148
		private string _recommendedAction = "";

		// Token: 0x04000095 RID: 149
		private Exception _textLookupError;
	}
}
