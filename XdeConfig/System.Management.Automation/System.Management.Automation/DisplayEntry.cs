using System;
using System.Xml;

namespace System.Management.Automation
{
	// Token: 0x0200095C RID: 2396
	public sealed class DisplayEntry
	{
		// Token: 0x170011CD RID: 4557
		// (get) Token: 0x0600580D RID: 22541 RVA: 0x001CA735 File Offset: 0x001C8935
		// (set) Token: 0x0600580E RID: 22542 RVA: 0x001CA73D File Offset: 0x001C893D
		public DisplayEntryValueType ValueType
		{
			get
			{
				return this._type;
			}
			internal set
			{
				this._type = value;
			}
		}

		// Token: 0x170011CE RID: 4558
		// (get) Token: 0x0600580F RID: 22543 RVA: 0x001CA746 File Offset: 0x001C8946
		// (set) Token: 0x06005810 RID: 22544 RVA: 0x001CA74E File Offset: 0x001C894E
		public string Value
		{
			get
			{
				return this._value;
			}
			internal set
			{
				this._value = value;
			}
		}

		// Token: 0x06005811 RID: 22545 RVA: 0x001CA757 File Offset: 0x001C8957
		internal DisplayEntry()
		{
		}

		// Token: 0x06005812 RID: 22546 RVA: 0x001CA75F File Offset: 0x001C895F
		public DisplayEntry(string value, DisplayEntryValueType type)
		{
			if (string.IsNullOrEmpty(value))
			{
				throw PSTraceSource.NewArgumentNullException("value");
			}
			this._value = value;
			this._type = type;
		}

		// Token: 0x06005813 RID: 22547 RVA: 0x001CA788 File Offset: 0x001C8988
		internal void WriteToXML(XmlWriter _writer, bool exportScriptBlock)
		{
			if (this._type == DisplayEntryValueType.Property)
			{
				_writer.WriteElementString(DisplayEntry._tagPropertyName, this._value);
				return;
			}
			if (this._type == DisplayEntryValueType.ScriptBlock)
			{
				_writer.WriteStartElement(DisplayEntry._tagScriptBlock);
				if (exportScriptBlock)
				{
					_writer.WriteValue(this._value);
				}
				else
				{
					_writer.WriteValue(DisplayEntry._safeScriptBlock);
				}
				_writer.WriteEndElement();
			}
		}

		// Token: 0x06005814 RID: 22548 RVA: 0x001CA7E5 File Offset: 0x001C89E5
		public override string ToString()
		{
			return this._value;
		}

		// Token: 0x04002F30 RID: 12080
		private DisplayEntryValueType _type;

		// Token: 0x04002F31 RID: 12081
		private string _value;

		// Token: 0x04002F32 RID: 12082
		private static string _tagPropertyName = "PropertyName";

		// Token: 0x04002F33 RID: 12083
		private static string _tagScriptBlock = "ScriptBlock";

		// Token: 0x04002F34 RID: 12084
		private static string _safeScriptBlock = ";";
	}
}
