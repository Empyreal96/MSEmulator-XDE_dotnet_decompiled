using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Management.Automation.Internal;
using System.Management.Automation.Runspaces;
using System.Management.Automation.Tracing;
using System.Xml;

namespace System.Management.Automation
{
	// Token: 0x0200044F RID: 1103
	internal class Deserializer
	{
		// Token: 0x06002FF4 RID: 12276 RVA: 0x001058CE File Offset: 0x00103ACE
		internal Deserializer(XmlReader reader) : this(reader, new DeserializationContext())
		{
		}

		// Token: 0x06002FF5 RID: 12277 RVA: 0x001058DC File Offset: 0x00103ADC
		internal Deserializer(XmlReader reader, DeserializationContext context)
		{
			if (reader == null)
			{
				throw PSTraceSource.NewArgumentNullException("reader");
			}
			this._reader = reader;
			this._context = context;
			this._deserializer = new InternalDeserializer(this._reader, this._context);
			try
			{
				this.Start();
			}
			catch (XmlException exception)
			{
				Deserializer.ReportExceptionForETW(exception);
				throw;
			}
		}

		// Token: 0x06002FF6 RID: 12278 RVA: 0x00105944 File Offset: 0x00103B44
		private static void ReportExceptionForETW(XmlException exception)
		{
			PSEtwLog.LogAnalyticError(PSEventId.Serializer_XmlExceptionWhenDeserializing, PSOpcode.Exception, PSTask.Serialization, (PSKeyword)4611686018427387968UL, new object[]
			{
				exception.LineNumber,
				exception.LinePosition,
				exception.ToString()
			});
		}

		// Token: 0x17000B15 RID: 2837
		// (get) Token: 0x06002FF7 RID: 12279 RVA: 0x00105994 File Offset: 0x00103B94
		// (set) Token: 0x06002FF8 RID: 12280 RVA: 0x001059A1 File Offset: 0x00103BA1
		internal TypeTable TypeTable
		{
			get
			{
				return this._deserializer.TypeTable;
			}
			set
			{
				this._deserializer.TypeTable = value;
			}
		}

		// Token: 0x06002FF9 RID: 12281 RVA: 0x001059B0 File Offset: 0x00103BB0
		private void Start()
		{
			this._reader.Read();
			string version = "1.1.0.1";
			if (DeserializationOptions.NoRootElement == (this._context.options & DeserializationOptions.NoRootElement))
			{
				this._done = this._reader.EOF;
			}
			else
			{
				this._reader.MoveToContent();
				string attribute = this._reader.GetAttribute("Version");
				if (attribute != null)
				{
					version = attribute;
				}
				if (!this._deserializer.ReadStartElementAndHandleEmpty("Objs"))
				{
					this._done = true;
				}
			}
			this._deserializer.ValidateVersion(version);
		}

		// Token: 0x06002FFA RID: 12282 RVA: 0x00105A44 File Offset: 0x00103C44
		internal bool Done()
		{
			if (!this._done)
			{
				if (DeserializationOptions.NoRootElement == (this._context.options & DeserializationOptions.NoRootElement))
				{
					this._done = this._reader.EOF;
				}
				else if (this._reader.NodeType == XmlNodeType.EndElement)
				{
					try
					{
						this._reader.ReadEndElement();
					}
					catch (XmlException exception)
					{
						Deserializer.ReportExceptionForETW(exception);
						throw;
					}
					this._done = true;
				}
			}
			return this._done;
		}

		// Token: 0x06002FFB RID: 12283 RVA: 0x00105AC8 File Offset: 0x00103CC8
		internal void Stop()
		{
			this._deserializer.Stop();
		}

		// Token: 0x06002FFC RID: 12284 RVA: 0x00105AD8 File Offset: 0x00103CD8
		internal object Deserialize()
		{
			string text;
			return this.Deserialize(out text);
		}

		// Token: 0x06002FFD RID: 12285 RVA: 0x00105AF0 File Offset: 0x00103CF0
		internal object Deserialize(out string streamName)
		{
			if (this.Done())
			{
				throw PSTraceSource.NewInvalidOperationException(Serialization.ReadCalledAfterDone, new object[0]);
			}
			object result;
			try
			{
				result = this._deserializer.ReadOneObject(out streamName);
			}
			catch (XmlException exception)
			{
				Deserializer.ReportExceptionForETW(exception);
				throw;
			}
			return result;
		}

		// Token: 0x06002FFE RID: 12286 RVA: 0x00105B40 File Offset: 0x00103D40
		internal static void AddDeserializationPrefix(ref string type)
		{
			if (!type.StartsWith("Deserialized.", StringComparison.OrdinalIgnoreCase))
			{
				type = type.Insert(0, "Deserialized.");
			}
		}

		// Token: 0x06002FFF RID: 12287 RVA: 0x00105B60 File Offset: 0x00103D60
		internal static bool IsInstanceOfType(object o, Type type)
		{
			if (type == null)
			{
				throw PSTraceSource.NewArgumentNullException("type");
			}
			return o != null && (type.IsAssignableFrom(PSObject.Base(o).GetType()) || Deserializer.IsDeserializedInstanceOfType(o, type));
		}

		// Token: 0x06003000 RID: 12288 RVA: 0x00105B98 File Offset: 0x00103D98
		internal static bool IsDeserializedInstanceOfType(object o, Type type)
		{
			if (type == null)
			{
				throw PSTraceSource.NewArgumentNullException("type");
			}
			if (o == null)
			{
				return false;
			}
			PSObject psobject = o as PSObject;
			if (psobject != null)
			{
				IEnumerable<string> internalTypeNames = psobject.InternalTypeNames;
				if (internalTypeNames != null)
				{
					foreach (string text in internalTypeNames)
					{
						if (text.Length == "Deserialized.".Length + type.FullName.Length && text.StartsWith("Deserialized.", StringComparison.OrdinalIgnoreCase) && text.EndsWith(type.FullName, StringComparison.OrdinalIgnoreCase))
						{
							return true;
						}
					}
					return false;
				}
			}
			return false;
		}

		// Token: 0x06003001 RID: 12289 RVA: 0x00105C50 File Offset: 0x00103E50
		internal static string MaskDeserializationPrefix(string typeName)
		{
			if (typeName == null)
			{
				return null;
			}
			if (typeName.StartsWith("Deserialized.", StringComparison.OrdinalIgnoreCase))
			{
				typeName = typeName.Substring("Deserialized.".Length);
			}
			return typeName;
		}

		// Token: 0x06003002 RID: 12290 RVA: 0x00105C78 File Offset: 0x00103E78
		internal static Collection<string> MaskDeserializationPrefix(Collection<string> typeNames)
		{
			bool flag = false;
			Collection<string> collection = new Collection<string>();
			foreach (string text in typeNames)
			{
				if (text.StartsWith("Deserialized.", StringComparison.OrdinalIgnoreCase))
				{
					flag = true;
					collection.Add(text.Substring("Deserialized.".Length));
				}
				else
				{
					collection.Add(text);
				}
			}
			if (flag)
			{
				return collection;
			}
			return null;
		}

		// Token: 0x040019F5 RID: 6645
		private const string DeserializationTypeNamePrefix = "Deserialized.";

		// Token: 0x040019F6 RID: 6646
		private readonly XmlReader _reader;

		// Token: 0x040019F7 RID: 6647
		private readonly InternalDeserializer _deserializer;

		// Token: 0x040019F8 RID: 6648
		private readonly DeserializationContext _context;

		// Token: 0x040019F9 RID: 6649
		private bool _done;
	}
}
