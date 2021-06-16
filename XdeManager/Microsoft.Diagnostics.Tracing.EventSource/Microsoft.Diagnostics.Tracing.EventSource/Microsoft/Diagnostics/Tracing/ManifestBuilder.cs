using System;
using System.Collections.Generic;
using System.Globalization;
using System.Reflection;
using System.Resources;
using System.Text;
using Microsoft.Diagnostics.Tracing.Internal;
using Microsoft.Reflection;

namespace Microsoft.Diagnostics.Tracing
{
	// Token: 0x02000034 RID: 52
	internal class ManifestBuilder
	{
		// Token: 0x060001BE RID: 446 RVA: 0x0000B424 File Offset: 0x00009624
		public ManifestBuilder(string providerName, Guid providerGuid, string dllName, ResourceManager resources, EventManifestOptions flags)
		{
			this.providerName = providerName;
			this.flags = flags;
			this.resources = resources;
			this.sb = new StringBuilder();
			this.events = new StringBuilder();
			this.templates = new StringBuilder();
			this.opcodeTab = new Dictionary<int, string>();
			this.stringTab = new Dictionary<string, string>();
			this.errors = new List<string>();
			this.perEventByteArrayArgIndices = new Dictionary<string, List<int>>();
			this.sb.AppendLine("<instrumentationManifest xmlns=\"http://schemas.microsoft.com/win/2004/08/events\">");
			this.sb.AppendLine(" <instrumentation xmlns:xs=\"http://www.w3.org/2001/XMLSchema\" xmlns:xsi=\"http://www.w3.org/2001/XMLSchema-instance\" xmlns:win=\"http://manifests.microsoft.com/win/2004/08/windows/events\">");
			this.sb.AppendLine("  <events xmlns=\"http://schemas.microsoft.com/win/2004/08/events\">");
			this.sb.Append("<provider name=\"").Append(providerName).Append("\" guid=\"{").Append(providerGuid.ToString()).Append("}");
			if (dllName != null)
			{
				this.sb.Append("\" resourceFileName=\"").Append(dllName).Append("\" messageFileName=\"").Append(dllName);
			}
			string value = providerName.Replace("-", "").Replace(".", "_");
			this.sb.Append("\" symbol=\"").Append(value);
			this.sb.Append("\">").AppendLine();
		}

		// Token: 0x060001BF RID: 447 RVA: 0x0000B594 File Offset: 0x00009794
		public void AddOpcode(string name, int value)
		{
			if ((this.flags & EventManifestOptions.Strict) != EventManifestOptions.None)
			{
				if (value <= 10 || value >= 239)
				{
					this.ManifestError(Microsoft.Diagnostics.Tracing.Internal.Environment.GetResourceString("EventSource_IllegalOpcodeValue", new object[]
					{
						name,
						value
					}), false);
				}
				string text;
				if (this.opcodeTab.TryGetValue(value, out text) && !name.Equals(text, StringComparison.Ordinal))
				{
					this.ManifestError(Microsoft.Diagnostics.Tracing.Internal.Environment.GetResourceString("EventSource_OpcodeCollision", new object[]
					{
						name,
						text,
						value
					}), false);
				}
			}
			this.opcodeTab[value] = name;
		}

		// Token: 0x060001C0 RID: 448 RVA: 0x0000B630 File Offset: 0x00009830
		public void AddTask(string name, int value)
		{
			if ((this.flags & EventManifestOptions.Strict) != EventManifestOptions.None)
			{
				if (value <= 0 || value >= 65535)
				{
					this.ManifestError(Microsoft.Diagnostics.Tracing.Internal.Environment.GetResourceString("EventSource_IllegalTaskValue", new object[]
					{
						name,
						value
					}), false);
				}
				string text;
				if (this.taskTab != null && this.taskTab.TryGetValue(value, out text) && !name.Equals(text, StringComparison.Ordinal))
				{
					this.ManifestError(Microsoft.Diagnostics.Tracing.Internal.Environment.GetResourceString("EventSource_TaskCollision", new object[]
					{
						name,
						text,
						value
					}), false);
				}
			}
			if (this.taskTab == null)
			{
				this.taskTab = new Dictionary<int, string>();
			}
			this.taskTab[value] = name;
		}

		// Token: 0x060001C1 RID: 449 RVA: 0x0000B6E8 File Offset: 0x000098E8
		public void AddKeyword(string name, ulong value)
		{
			if ((value & checked(value - 1UL)) != 0UL)
			{
				this.ManifestError(Microsoft.Diagnostics.Tracing.Internal.Environment.GetResourceString("EventSource_KeywordNeedPowerOfTwo", new object[]
				{
					"0x" + value.ToString("x", CultureInfo.CurrentCulture),
					name
				}), true);
			}
			if ((this.flags & EventManifestOptions.Strict) != EventManifestOptions.None)
			{
				if (value >= 17592186044416UL && !name.StartsWith("Session", StringComparison.Ordinal))
				{
					this.ManifestError(Microsoft.Diagnostics.Tracing.Internal.Environment.GetResourceString("EventSource_IllegalKeywordsValue", new object[]
					{
						name,
						"0x" + value.ToString("x", CultureInfo.CurrentCulture)
					}), false);
				}
				string text;
				if (this.keywordTab != null && this.keywordTab.TryGetValue(value, out text) && !name.Equals(text, StringComparison.Ordinal))
				{
					this.ManifestError(Microsoft.Diagnostics.Tracing.Internal.Environment.GetResourceString("EventSource_KeywordCollision", new object[]
					{
						name,
						text,
						"0x" + value.ToString("x", CultureInfo.CurrentCulture)
					}), false);
				}
			}
			if (this.keywordTab == null)
			{
				this.keywordTab = new Dictionary<ulong, string>();
			}
			this.keywordTab[value] = name;
		}

		// Token: 0x060001C2 RID: 450 RVA: 0x0000B820 File Offset: 0x00009A20
		public void AddChannel(string name, int value, EventChannelAttribute channelAttribute)
		{
			checked
			{
				EventChannel eventChannel = (EventChannel)value;
				if (value < 16 || value > 255)
				{
					this.ManifestError(Microsoft.Diagnostics.Tracing.Internal.Environment.GetResourceString("EventSource_EventChannelOutOfRange", new object[]
					{
						name,
						value
					}), false);
				}
				else if (eventChannel >= EventChannel.Admin && eventChannel <= EventChannel.Debug && channelAttribute != null && this.EventChannelToChannelType(eventChannel) != channelAttribute.EventChannelType)
				{
					this.ManifestError(Microsoft.Diagnostics.Tracing.Internal.Environment.GetResourceString("EventSource_ChannelTypeDoesNotMatchEventChannelValue", new object[]
					{
						name,
						((EventChannel)value).ToString()
					}), false);
				}
				ulong channelKeyword = this.GetChannelKeyword(eventChannel);
				if (this.channelTab == null)
				{
					this.channelTab = new Dictionary<int, ManifestBuilder.ChannelInfo>(4);
				}
				this.channelTab[value] = new ManifestBuilder.ChannelInfo
				{
					Name = name,
					Keywords = channelKeyword,
					Attribs = channelAttribute
				};
			}
		}

		// Token: 0x060001C3 RID: 451 RVA: 0x0000B8F5 File Offset: 0x00009AF5
		private EventChannelType EventChannelToChannelType(EventChannel channel)
		{
			return (EventChannelType)(checked(channel - 16 + 1));
		}

		// Token: 0x060001C4 RID: 452 RVA: 0x0000B900 File Offset: 0x00009B00
		private EventChannelAttribute GetDefaultChannelAttribute(EventChannel channel)
		{
			EventChannelAttribute eventChannelAttribute = new EventChannelAttribute();
			eventChannelAttribute.EventChannelType = this.EventChannelToChannelType(channel);
			if (eventChannelAttribute.EventChannelType <= EventChannelType.Operational)
			{
				eventChannelAttribute.Enabled = true;
			}
			return eventChannelAttribute;
		}

		// Token: 0x060001C5 RID: 453 RVA: 0x0000B934 File Offset: 0x00009B34
		public ulong[] GetChannelData()
		{
			if (this.channelTab == null)
			{
				return new ulong[0];
			}
			int num = -1;
			foreach (int num2 in this.channelTab.Keys)
			{
				if (num2 > num)
				{
					num = num2;
				}
			}
			ulong[] array = new ulong[checked(num + 1)];
			foreach (KeyValuePair<int, ManifestBuilder.ChannelInfo> keyValuePair in this.channelTab)
			{
				array[keyValuePair.Key] = keyValuePair.Value.Keywords;
			}
			return array;
		}

		// Token: 0x060001C6 RID: 454 RVA: 0x0000B9F8 File Offset: 0x00009BF8
		public void StartEvent(string eventName, EventAttribute eventAttribute)
		{
			this.eventName = eventName;
			this.numParams = 0;
			this.byteArrArgIndices = null;
			this.events.Append("  <event").Append(" value=\"").Append(eventAttribute.EventId).Append("\"").Append(" version=\"").Append(eventAttribute.Version).Append("\"").Append(" level=\"").Append(ManifestBuilder.GetLevelName(eventAttribute.Level)).Append("\"").Append(" symbol=\"").Append(eventName).Append("\"");
			this.WriteMessageAttrib(this.events, "event", eventName, eventAttribute.Message);
			if (eventAttribute.Keywords != EventKeywords.None)
			{
				this.events.Append(" keywords=\"").Append(this.GetKeywords(checked((ulong)eventAttribute.Keywords), eventName)).Append("\"");
			}
			if (eventAttribute.Opcode != EventOpcode.Info)
			{
				this.events.Append(" opcode=\"").Append(this.GetOpcodeName(eventAttribute.Opcode, eventName)).Append("\"");
			}
			if (eventAttribute.Task != EventTask.None)
			{
				this.events.Append(" task=\"").Append(this.GetTaskName(eventAttribute.Task, eventName)).Append("\"");
			}
			if (eventAttribute.Channel != EventChannel.None)
			{
				this.events.Append(" channel=\"").Append(this.GetChannelName(eventAttribute.Channel, eventName, eventAttribute.Message)).Append("\"");
			}
		}

		// Token: 0x060001C7 RID: 455 RVA: 0x0000BB9C File Offset: 0x00009D9C
		public void AddEventParameter(Type type, string name)
		{
			if (this.numParams == 0)
			{
				this.templates.Append("  <template tid=\"").Append(this.eventName).Append("Args\">").AppendLine();
			}
			checked
			{
				if (type == typeof(byte[]))
				{
					if (this.byteArrArgIndices == null)
					{
						this.byteArrArgIndices = new List<int>(4);
					}
					this.byteArrArgIndices.Add(this.numParams);
					this.numParams++;
					this.templates.Append("   <data name=\"").Append(name).Append("Size\" inType=\"win:UInt32\"/>").AppendLine();
				}
				this.numParams++;
				this.templates.Append("   <data name=\"").Append(name).Append("\" inType=\"").Append(this.GetTypeName(type)).Append("\"");
				if ((type.IsArray || type.IsPointer) && type.GetElementType() == typeof(byte))
				{
					this.templates.Append(" length=\"").Append(name).Append("Size\"");
				}
				if (type.IsEnum() && Enum.GetUnderlyingType(type) != typeof(ulong) && Enum.GetUnderlyingType(type) != typeof(long))
				{
					this.templates.Append(" map=\"").Append(type.Name).Append("\"");
					if (this.mapsTab == null)
					{
						this.mapsTab = new Dictionary<string, Type>();
					}
					if (!this.mapsTab.ContainsKey(type.Name))
					{
						this.mapsTab.Add(type.Name, type);
					}
				}
				this.templates.Append("/>").AppendLine();
			}
		}

		// Token: 0x060001C8 RID: 456 RVA: 0x0000BD84 File Offset: 0x00009F84
		public void EndEvent()
		{
			if (this.numParams > 0)
			{
				this.templates.Append("  </template>").AppendLine();
				this.events.Append(" template=\"").Append(this.eventName).Append("Args\"");
			}
			this.events.Append("/>").AppendLine();
			if (this.byteArrArgIndices != null)
			{
				this.perEventByteArrayArgIndices[this.eventName] = this.byteArrArgIndices;
			}
			string text;
			if (this.stringTab.TryGetValue("event_" + this.eventName, out text))
			{
				text = this.TranslateToManifestConvention(text, this.eventName);
				this.stringTab["event_" + this.eventName] = text;
			}
			this.eventName = null;
			this.numParams = 0;
			this.byteArrArgIndices = null;
		}

		// Token: 0x060001C9 RID: 457 RVA: 0x0000BE6C File Offset: 0x0000A06C
		public ulong GetChannelKeyword(EventChannel channel)
		{
			if (this.channelTab == null)
			{
				this.channelTab = new Dictionary<int, ManifestBuilder.ChannelInfo>(4);
			}
			if (this.channelTab.Count == 8)
			{
				this.ManifestError(Microsoft.Diagnostics.Tracing.Internal.Environment.GetResourceString("EventSource_MaxChannelExceeded", new object[0]), false);
			}
			ManifestBuilder.ChannelInfo channelInfo;
			ulong keywords;
			if (!this.channelTab.TryGetValue((int)channel, out channelInfo))
			{
				keywords = this.nextChannelKeywordBit;
				this.nextChannelKeywordBit >>= 1;
			}
			else
			{
				keywords = channelInfo.Keywords;
			}
			return keywords;
		}

		// Token: 0x060001CA RID: 458 RVA: 0x0000BEE4 File Offset: 0x0000A0E4
		public byte[] CreateManifest()
		{
			string s = this.CreateManifestString();
			return Encoding.UTF8.GetBytes(s);
		}

		// Token: 0x1700005B RID: 91
		// (get) Token: 0x060001CB RID: 459 RVA: 0x0000BF03 File Offset: 0x0000A103
		public IList<string> Errors
		{
			get
			{
				return this.errors;
			}
		}

		// Token: 0x060001CC RID: 460 RVA: 0x0000BF0B File Offset: 0x0000A10B
		public void ManifestError(string msg, bool runtimeCritical = false)
		{
			if ((this.flags & EventManifestOptions.Strict) != EventManifestOptions.None)
			{
				this.errors.Add(msg);
				return;
			}
			if (runtimeCritical)
			{
				throw new ArgumentException(msg);
			}
		}

		// Token: 0x060001CD RID: 461 RVA: 0x0000BF54 File Offset: 0x0000A154
		private string CreateManifestString()
		{
			if (this.channelTab != null)
			{
				this.sb.Append(" <channels>").AppendLine();
				List<KeyValuePair<int, ManifestBuilder.ChannelInfo>> list = new List<KeyValuePair<int, ManifestBuilder.ChannelInfo>>();
				foreach (KeyValuePair<int, ManifestBuilder.ChannelInfo> item in this.channelTab)
				{
					list.Add(item);
				}
				list.Sort((KeyValuePair<int, ManifestBuilder.ChannelInfo> p1, KeyValuePair<int, ManifestBuilder.ChannelInfo> p2) => checked(0 - Comparer<ulong>.Default.Compare(p1.Value.Keywords, p2.Value.Keywords)));
				foreach (KeyValuePair<int, ManifestBuilder.ChannelInfo> keyValuePair in list)
				{
					int key = keyValuePair.Key;
					ManifestBuilder.ChannelInfo value = keyValuePair.Value;
					string text = null;
					string text2 = "channel";
					bool flag = false;
					string text3 = null;
					if (value.Attribs != null)
					{
						EventChannelAttribute attribs = value.Attribs;
						if (Enum.IsDefined(typeof(EventChannelType), attribs.EventChannelType))
						{
							text = attribs.EventChannelType.ToString();
						}
						flag = attribs.Enabled;
					}
					if (text3 == null)
					{
						text3 = this.providerName + "/" + value.Name;
					}
					this.sb.Append("  <").Append(text2);
					this.sb.Append(" chid=\"").Append(value.Name).Append("\"");
					this.sb.Append(" name=\"").Append(text3).Append("\"");
					if (text2 == "channel")
					{
						this.WriteMessageAttrib(this.sb, "channel", value.Name, null);
						this.sb.Append(" value=\"").Append(key).Append("\"");
						if (text != null)
						{
							this.sb.Append(" type=\"").Append(text).Append("\"");
						}
						this.sb.Append(" enabled=\"").Append(flag.ToString().ToLower()).Append("\"");
					}
					this.sb.Append("/>").AppendLine();
				}
				this.sb.Append(" </channels>").AppendLine();
			}
			if (this.taskTab != null)
			{
				this.sb.Append(" <tasks>").AppendLine();
				List<int> list2 = new List<int>(this.taskTab.Keys);
				list2.Sort();
				foreach (int num in list2)
				{
					this.sb.Append("  <task");
					this.WriteNameAndMessageAttribs(this.sb, "task", this.taskTab[num]);
					this.sb.Append(" value=\"").Append(num).Append("\"/>").AppendLine();
				}
				this.sb.Append(" </tasks>").AppendLine();
			}
			if (this.mapsTab != null)
			{
				this.sb.Append(" <maps>").AppendLine();
				foreach (Type type in this.mapsTab.Values)
				{
					bool flag2 = EventSource.GetCustomAttributeHelper(type, typeof(FlagsAttribute), this.flags) != null;
					string value2 = flag2 ? "bitMap" : "valueMap";
					this.sb.Append("  <").Append(value2).Append(" name=\"").Append(type.Name).Append("\">").AppendLine();
					FieldInfo[] fields = type.GetFields(BindingFlags.DeclaredOnly | BindingFlags.Static | BindingFlags.Public);
					foreach (FieldInfo fieldInfo in fields)
					{
						object rawConstantValue = fieldInfo.GetRawConstantValue();
						if (rawConstantValue != null)
						{
							long num2;
							if (rawConstantValue is int)
							{
								num2 = (long)((int)rawConstantValue);
							}
							else
							{
								if (!(rawConstantValue is long))
								{
									goto IL_4CC;
								}
								num2 = (long)rawConstantValue;
							}
							if (!flag2 || ((num2 & checked(num2 - 1L)) == 0L && num2 != 0L))
							{
								this.sb.Append("   <map value=\"0x").Append(num2.ToString("x", CultureInfo.InvariantCulture)).Append("\"");
								this.WriteMessageAttrib(this.sb, "map", type.Name + "." + fieldInfo.Name, fieldInfo.Name);
								this.sb.Append("/>").AppendLine();
							}
						}
						IL_4CC:;
					}
					this.sb.Append("  </").Append(value2).Append(">").AppendLine();
				}
				this.sb.Append(" </maps>").AppendLine();
			}
			this.sb.Append(" <opcodes>").AppendLine();
			List<int> list3 = new List<int>(this.opcodeTab.Keys);
			list3.Sort();
			foreach (int num3 in list3)
			{
				this.sb.Append("  <opcode");
				this.WriteNameAndMessageAttribs(this.sb, "opcode", this.opcodeTab[num3]);
				this.sb.Append(" value=\"").Append(num3).Append("\"/>").AppendLine();
			}
			this.sb.Append(" </opcodes>").AppendLine();
			if (this.keywordTab != null)
			{
				this.sb.Append(" <keywords>").AppendLine();
				List<ulong> list4 = new List<ulong>(this.keywordTab.Keys);
				list4.Sort();
				foreach (ulong key2 in list4)
				{
					this.sb.Append("  <keyword");
					this.WriteNameAndMessageAttribs(this.sb, "keyword", this.keywordTab[key2]);
					this.sb.Append(" mask=\"0x").Append(key2.ToString("x", CultureInfo.InvariantCulture)).Append("\"/>").AppendLine();
				}
				this.sb.Append(" </keywords>").AppendLine();
			}
			this.sb.Append(" <events>").AppendLine();
			this.sb.Append(this.events);
			this.sb.Append(" </events>").AppendLine();
			this.sb.Append(" <templates>").AppendLine();
			if (this.templates.Length > 0)
			{
				this.sb.Append(this.templates);
			}
			else
			{
				this.sb.Append("    <template tid=\"_empty\"></template>").AppendLine();
			}
			this.sb.Append(" </templates>").AppendLine();
			this.sb.Append("</provider>").AppendLine();
			this.sb.Append("</events>").AppendLine();
			this.sb.Append("</instrumentation>").AppendLine();
			this.sb.Append("<localization>").AppendLine();
			List<CultureInfo> list5;
			if (this.resources != null && (this.flags & EventManifestOptions.AllCultures) != EventManifestOptions.None)
			{
				list5 = ManifestBuilder.GetSupportedCultures(this.resources);
			}
			else
			{
				list5 = new List<CultureInfo>();
				list5.Add(CultureInfo.CurrentUICulture);
			}
			List<string> list6 = new List<string>(this.stringTab.Keys);
			list6.Sort();
			foreach (CultureInfo cultureInfo in list5)
			{
				this.sb.Append(" <resources culture=\"").Append(cultureInfo.Name).Append("\">").AppendLine();
				this.sb.Append("  <stringTable>").AppendLine();
				foreach (string text4 in list6)
				{
					string localizedMessage = this.GetLocalizedMessage(text4, cultureInfo, true);
					this.sb.Append("   <string id=\"").Append(text4).Append("\" value=\"").Append(localizedMessage).Append("\"/>").AppendLine();
				}
				this.sb.Append("  </stringTable>").AppendLine();
				this.sb.Append(" </resources>").AppendLine();
			}
			this.sb.Append("</localization>").AppendLine();
			this.sb.AppendLine("</instrumentationManifest>");
			return this.sb.ToString();
		}

		// Token: 0x060001CE RID: 462 RVA: 0x0000C994 File Offset: 0x0000AB94
		private void WriteNameAndMessageAttribs(StringBuilder stringBuilder, string elementName, string name)
		{
			stringBuilder.Append(" name=\"").Append(name).Append("\"");
			this.WriteMessageAttrib(this.sb, elementName, name, name);
		}

		// Token: 0x060001CF RID: 463 RVA: 0x0000C9C4 File Offset: 0x0000ABC4
		private void WriteMessageAttrib(StringBuilder stringBuilder, string elementName, string name, string value)
		{
			string text = elementName + "_" + name;
			if (this.resources != null)
			{
				string @string = this.resources.GetString(text, CultureInfo.InvariantCulture);
				if (@string != null)
				{
					value = @string;
				}
			}
			if (value == null)
			{
				return;
			}
			stringBuilder.Append(" message=\"$(string.").Append(text).Append(")\"");
			string text2;
			if (this.stringTab.TryGetValue(text, out text2) && !text2.Equals(value))
			{
				this.ManifestError(Microsoft.Diagnostics.Tracing.Internal.Environment.GetResourceString("EventSource_DuplicateStringKey", new object[]
				{
					text
				}), true);
				return;
			}
			this.stringTab[text] = value;
		}

		// Token: 0x060001D0 RID: 464 RVA: 0x0000CA68 File Offset: 0x0000AC68
		internal string GetLocalizedMessage(string key, CultureInfo ci, bool etwFormat)
		{
			string text = null;
			if (this.resources != null)
			{
				string @string = this.resources.GetString(key, ci);
				if (@string != null)
				{
					text = @string;
					if (etwFormat && key.StartsWith("event_"))
					{
						string evtName = key.Substring("event_".Length);
						text = this.TranslateToManifestConvention(text, evtName);
					}
				}
			}
			if (etwFormat && text == null)
			{
				this.stringTab.TryGetValue(key, out text);
			}
			return text;
		}

		// Token: 0x060001D1 RID: 465 RVA: 0x0000CAD4 File Offset: 0x0000ACD4
		private static List<CultureInfo> GetSupportedCultures(ResourceManager resources)
		{
			List<CultureInfo> list = new List<CultureInfo>();
			foreach (CultureInfo cultureInfo in CultureInfo.GetCultures(CultureTypes.SpecificCultures))
			{
				if (resources.GetResourceSet(cultureInfo, true, false) != null)
				{
					list.Add(cultureInfo);
				}
			}
			if (!list.Contains(CultureInfo.CurrentUICulture))
			{
				list.Insert(0, CultureInfo.CurrentUICulture);
			}
			return list;
		}

		// Token: 0x060001D2 RID: 466 RVA: 0x0000CB2C File Offset: 0x0000AD2C
		private static string GetLevelName(EventLevel level)
		{
			return ((level >= (EventLevel)16) ? "" : "win:") + level.ToString();
		}

		// Token: 0x060001D3 RID: 467 RVA: 0x0000CB50 File Offset: 0x0000AD50
		private string GetChannelName(EventChannel channel, string eventName, string eventMessage)
		{
			ManifestBuilder.ChannelInfo channelInfo = null;
			if (this.channelTab == null || !this.channelTab.TryGetValue((int)channel, out channelInfo))
			{
				if (channel < EventChannel.Admin)
				{
					this.ManifestError(Microsoft.Diagnostics.Tracing.Internal.Environment.GetResourceString("EventSource_UndefinedChannel", new object[]
					{
						channel,
						eventName
					}), false);
				}
				if (this.channelTab == null)
				{
					this.channelTab = new Dictionary<int, ManifestBuilder.ChannelInfo>(4);
				}
				string text = channel.ToString();
				if (EventChannel.Debug < channel)
				{
					text = "Channel" + text;
				}
				this.AddChannel(text, (int)channel, this.GetDefaultChannelAttribute(channel));
				if (!this.channelTab.TryGetValue((int)channel, out channelInfo))
				{
					this.ManifestError(Microsoft.Diagnostics.Tracing.Internal.Environment.GetResourceString("EventSource_UndefinedChannel", new object[]
					{
						channel,
						eventName
					}), false);
				}
			}
			if (this.resources != null && eventMessage == null)
			{
				eventMessage = this.resources.GetString("event_" + eventName, CultureInfo.InvariantCulture);
			}
			if (channelInfo.Attribs.EventChannelType == EventChannelType.Admin && eventMessage == null)
			{
				this.ManifestError(Microsoft.Diagnostics.Tracing.Internal.Environment.GetResourceString("EventSource_EventWithAdminChannelMustHaveMessage", new object[]
				{
					eventName,
					channelInfo.Name
				}), false);
			}
			return channelInfo.Name;
		}

		// Token: 0x060001D4 RID: 468 RVA: 0x0000CC84 File Offset: 0x0000AE84
		private string GetTaskName(EventTask task, string eventName)
		{
			if (task == EventTask.None)
			{
				return "";
			}
			if (this.taskTab == null)
			{
				this.taskTab = new Dictionary<int, string>();
			}
			string result;
			if (!this.taskTab.TryGetValue((int)task, out result))
			{
				this.taskTab[(int)task] = eventName;
				result = eventName;
			}
			return result;
		}

		// Token: 0x060001D5 RID: 469 RVA: 0x0000CCD0 File Offset: 0x0000AED0
		private string GetOpcodeName(EventOpcode opcode, string eventName)
		{
			switch (opcode)
			{
			case EventOpcode.Info:
				return "win:Info";
			case EventOpcode.Start:
				return "win:Start";
			case EventOpcode.Stop:
				return "win:Stop";
			case EventOpcode.DataCollectionStart:
				return "win:DC_Start";
			case EventOpcode.DataCollectionStop:
				return "win:DC_Stop";
			case EventOpcode.Extension:
				return "win:Extension";
			case EventOpcode.Reply:
				return "win:Reply";
			case EventOpcode.Resume:
				return "win:Resume";
			case EventOpcode.Suspend:
				return "win:Suspend";
			case EventOpcode.Send:
				return "win:Send";
			default:
				if (opcode != EventOpcode.Receive)
				{
					string result;
					if (this.opcodeTab == null || !this.opcodeTab.TryGetValue((int)opcode, out result))
					{
						this.ManifestError(Microsoft.Diagnostics.Tracing.Internal.Environment.GetResourceString("EventSource_UndefinedOpcode", new object[]
						{
							opcode,
							eventName
						}), true);
						result = null;
					}
					return result;
				}
				return "win:Receive";
			}
		}

		// Token: 0x060001D6 RID: 470 RVA: 0x0000CD9C File Offset: 0x0000AF9C
		private string GetKeywords(ulong keywords, string eventName)
		{
			string text = "";
			for (ulong num = 1UL; num != 0UL; num <<= 1)
			{
				if ((keywords & num) != 0UL)
				{
					string text2 = null;
					if ((this.keywordTab == null || !this.keywordTab.TryGetValue(num, out text2)) && num >= 281474976710656UL)
					{
						text2 = string.Empty;
					}
					if (text2 == null)
					{
						this.ManifestError(Microsoft.Diagnostics.Tracing.Internal.Environment.GetResourceString("EventSource_UndefinedKeyword", new object[]
						{
							"0x" + num.ToString("x", CultureInfo.CurrentCulture),
							eventName
						}), true);
						text2 = string.Empty;
					}
					if (text.Length != 0 && text2.Length != 0)
					{
						text += " ";
					}
					text += text2;
				}
			}
			return text;
		}

		// Token: 0x060001D7 RID: 471 RVA: 0x0000CE64 File Offset: 0x0000B064
		private string GetTypeName(Type type)
		{
			if (type.IsEnum())
			{
				FieldInfo[] fields = type.GetFields(BindingFlags.Instance | BindingFlags.Public | BindingFlags.NonPublic);
				string typeName = this.GetTypeName(fields[0].FieldType);
				return typeName.Replace("win:Int", "win:UInt");
			}
			switch (type.GetTypeCode())
			{
			case TypeCode.Boolean:
				return "win:Boolean";
			case TypeCode.Char:
			case TypeCode.UInt16:
				return "win:UInt16";
			case TypeCode.SByte:
				return "win:Int8";
			case TypeCode.Byte:
				return "win:UInt8";
			case TypeCode.Int16:
				return "win:Int16";
			case TypeCode.Int32:
				return "win:Int32";
			case TypeCode.UInt32:
				return "win:UInt32";
			case TypeCode.Int64:
				return "win:Int64";
			case TypeCode.UInt64:
				return "win:UInt64";
			case TypeCode.Single:
				return "win:Float";
			case TypeCode.Double:
				return "win:Double";
			case TypeCode.DateTime:
				return "win:FILETIME";
			case TypeCode.String:
				return "win:UnicodeString";
			}
			if (type == typeof(Guid))
			{
				return "win:GUID";
			}
			if (type == typeof(IntPtr))
			{
				return "win:Pointer";
			}
			if ((type.IsArray || type.IsPointer) && type.GetElementType() == typeof(byte))
			{
				return "win:Binary";
			}
			this.ManifestError(Microsoft.Diagnostics.Tracing.Internal.Environment.GetResourceString("EventSource_UnsupportedEventTypeInManifest", new object[]
			{
				type.Name
			}), true);
			return string.Empty;
		}

		// Token: 0x060001D8 RID: 472 RVA: 0x0000CFC5 File Offset: 0x0000B1C5
		private static void UpdateStringBuilder(ref StringBuilder stringBuilder, string eventMessage, int startIndex, int count)
		{
			if (stringBuilder == null)
			{
				stringBuilder = new StringBuilder();
			}
			stringBuilder.Append(eventMessage, startIndex, count);
		}

		// Token: 0x060001D9 RID: 473 RVA: 0x0000D068 File Offset: 0x0000B268
		private string TranslateToManifestConvention(string eventMessage, string evtName)
		{
			ManifestBuilder.<>c__DisplayClass4 CS$<>8__locals1 = new ManifestBuilder.<>c__DisplayClass4();
			CS$<>8__locals1.eventMessage = eventMessage;
			CS$<>8__locals1.stringBuilder = null;
			CS$<>8__locals1.writtenSoFar = 0;
			int i = 0;
			checked
			{
				while (i < CS$<>8__locals1.eventMessage.Length)
				{
					int num2;
					if (CS$<>8__locals1.eventMessage[i] == '%')
					{
						ManifestBuilder.UpdateStringBuilder(ref CS$<>8__locals1.stringBuilder, CS$<>8__locals1.eventMessage, CS$<>8__locals1.writtenSoFar, i - CS$<>8__locals1.writtenSoFar);
						CS$<>8__locals1.stringBuilder.Append("%%");
						i++;
						CS$<>8__locals1.writtenSoFar = i;
					}
					else if (i < CS$<>8__locals1.eventMessage.Length - 1 && ((CS$<>8__locals1.eventMessage[i] == '{' && CS$<>8__locals1.eventMessage[i + 1] == '{') || (CS$<>8__locals1.eventMessage[i] == '}' && CS$<>8__locals1.eventMessage[i + 1] == '}')))
					{
						ManifestBuilder.UpdateStringBuilder(ref CS$<>8__locals1.stringBuilder, CS$<>8__locals1.eventMessage, CS$<>8__locals1.writtenSoFar, i - CS$<>8__locals1.writtenSoFar);
						CS$<>8__locals1.stringBuilder.Append(CS$<>8__locals1.eventMessage[i]);
						i++;
						i++;
						CS$<>8__locals1.writtenSoFar = i;
					}
					else if (CS$<>8__locals1.eventMessage[i] == '{')
					{
						int j = i;
						i++;
						int num = 0;
						while (i < CS$<>8__locals1.eventMessage.Length && char.IsDigit(CS$<>8__locals1.eventMessage[i]))
						{
							num = num * 10 + (int)CS$<>8__locals1.eventMessage[i] - 48;
							i++;
						}
						if (i < CS$<>8__locals1.eventMessage.Length && CS$<>8__locals1.eventMessage[i] == '}')
						{
							i++;
							ManifestBuilder.UpdateStringBuilder(ref CS$<>8__locals1.stringBuilder, CS$<>8__locals1.eventMessage, CS$<>8__locals1.writtenSoFar, j - CS$<>8__locals1.writtenSoFar);
							int value = this.TranslateIndexToManifestConvention(num, evtName);
							CS$<>8__locals1.stringBuilder.Append('%').Append(value);
							if (i < CS$<>8__locals1.eventMessage.Length && CS$<>8__locals1.eventMessage[i] == '!')
							{
								i++;
								CS$<>8__locals1.stringBuilder.Append("%!");
							}
							CS$<>8__locals1.writtenSoFar = i;
						}
						else
						{
							this.ManifestError(Microsoft.Diagnostics.Tracing.Internal.Environment.GetResourceString("EventSource_UnsupportedMessageProperty", new object[]
							{
								evtName,
								CS$<>8__locals1.eventMessage
							}), false);
						}
					}
					else if ((num2 = "&<>'\"\r\n\t".IndexOf(CS$<>8__locals1.eventMessage[i])) >= 0)
					{
						string[] array = new string[]
						{
							"&amp;",
							"&lt;",
							"&gt;",
							"&apos;",
							"&quot;",
							"%r",
							"%n",
							"%t"
						};
						Action<char, string> action = delegate(char ch, string escape)
						{
							ManifestBuilder.UpdateStringBuilder(ref CS$<>8__locals1.stringBuilder, CS$<>8__locals1.eventMessage, CS$<>8__locals1.writtenSoFar, i - CS$<>8__locals1.writtenSoFar);
							i++;
							CS$<>8__locals1.stringBuilder.Append(escape);
							CS$<>8__locals1.writtenSoFar = i;
						};
						action(CS$<>8__locals1.eventMessage[i], array[num2]);
					}
					else
					{
						i++;
					}
				}
				if (CS$<>8__locals1.stringBuilder == null)
				{
					return CS$<>8__locals1.eventMessage;
				}
				ManifestBuilder.UpdateStringBuilder(ref CS$<>8__locals1.stringBuilder, CS$<>8__locals1.eventMessage, CS$<>8__locals1.writtenSoFar, i - CS$<>8__locals1.writtenSoFar);
				return CS$<>8__locals1.stringBuilder.ToString();
			}
		}

		// Token: 0x060001DA RID: 474 RVA: 0x0000D4E0 File Offset: 0x0000B6E0
		private int TranslateIndexToManifestConvention(int idx, string evtName)
		{
			checked
			{
				List<int> list;
				if (this.perEventByteArrayArgIndices.TryGetValue(evtName, out list))
				{
					foreach (int num in list)
					{
						if (idx < num)
						{
							break;
						}
						idx++;
					}
				}
				return idx + 1;
			}
		}

		// Token: 0x040000FE RID: 254
		private const int MaxCountChannels = 8;

		// Token: 0x040000FF RID: 255
		private Dictionary<int, string> opcodeTab;

		// Token: 0x04000100 RID: 256
		private Dictionary<int, string> taskTab;

		// Token: 0x04000101 RID: 257
		private Dictionary<int, ManifestBuilder.ChannelInfo> channelTab;

		// Token: 0x04000102 RID: 258
		private Dictionary<ulong, string> keywordTab;

		// Token: 0x04000103 RID: 259
		private Dictionary<string, Type> mapsTab;

		// Token: 0x04000104 RID: 260
		private Dictionary<string, string> stringTab;

		// Token: 0x04000105 RID: 261
		private ulong nextChannelKeywordBit = 9223372036854775808UL;

		// Token: 0x04000106 RID: 262
		private StringBuilder sb;

		// Token: 0x04000107 RID: 263
		private StringBuilder events;

		// Token: 0x04000108 RID: 264
		private StringBuilder templates;

		// Token: 0x04000109 RID: 265
		private string providerName;

		// Token: 0x0400010A RID: 266
		private ResourceManager resources;

		// Token: 0x0400010B RID: 267
		private EventManifestOptions flags;

		// Token: 0x0400010C RID: 268
		private IList<string> errors;

		// Token: 0x0400010D RID: 269
		private Dictionary<string, List<int>> perEventByteArrayArgIndices;

		// Token: 0x0400010E RID: 270
		private string eventName;

		// Token: 0x0400010F RID: 271
		private int numParams;

		// Token: 0x04000110 RID: 272
		private List<int> byteArrArgIndices;

		// Token: 0x02000035 RID: 53
		private class ChannelInfo
		{
			// Token: 0x04000112 RID: 274
			public string Name;

			// Token: 0x04000113 RID: 275
			public ulong Keywords;

			// Token: 0x04000114 RID: 276
			public EventChannelAttribute Attribs;
		}
	}
}
