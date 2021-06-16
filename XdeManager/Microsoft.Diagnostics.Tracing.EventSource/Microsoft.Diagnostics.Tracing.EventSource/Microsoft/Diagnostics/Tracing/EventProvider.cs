using System;
using System.Collections.Generic;
using System.Globalization;
using System.Runtime.InteropServices;
using System.Security;
using System.Security.Permissions;
using System.Text;
using Microsoft.Win32;

namespace Microsoft.Diagnostics.Tracing
{
	// Token: 0x02000008 RID: 8
	[HostProtection(SecurityAction.LinkDemand, MayLeakOnAbort = true)]
	internal class EventProvider : IDisposable
	{
		// Token: 0x06000094 RID: 148 RVA: 0x000077B2 File Offset: 0x000059B2
		internal EventProvider()
		{
		}

		// Token: 0x06000095 RID: 149 RVA: 0x000077BC File Offset: 0x000059BC
		[SecurityCritical]
		internal void Register(Guid providerGuid)
		{
			this.m_providerId = providerGuid;
			this.m_etwCallback = new UnsafeNativeMethods.ManifestEtw.EtwEnableCallback(this.EtwEnableCallBack);
			uint num = this.EventRegister(ref this.m_providerId, this.m_etwCallback);
			if (num != 0U)
			{
				throw new ArgumentException(Win32Native.GetMessage((int)num));
			}
		}

		// Token: 0x06000096 RID: 150 RVA: 0x00007804 File Offset: 0x00005A04
		[SecurityCritical]
		internal unsafe int SetInformation(UnsafeNativeMethods.ManifestEtw.EVENT_INFO_CLASS eventInfoClass, void* data, int dataSize)
		{
			int result = 50;
			if (!EventProvider.m_setInformationMissing)
			{
				try
				{
					result = UnsafeNativeMethods.ManifestEtw.EventSetInformation(this.m_regHandle, eventInfoClass, data, dataSize);
				}
				catch (TypeLoadException)
				{
					EventProvider.m_setInformationMissing = true;
				}
			}
			return result;
		}

		// Token: 0x06000097 RID: 151 RVA: 0x00007848 File Offset: 0x00005A48
		public void Dispose()
		{
			this.Dispose(true);
			GC.SuppressFinalize(this);
		}

		// Token: 0x06000098 RID: 152 RVA: 0x00007858 File Offset: 0x00005A58
		[SecuritySafeCritical]
		protected virtual void Dispose(bool disposing)
		{
			if (this.m_disposed)
			{
				return;
			}
			this.m_enabled = false;
			lock (EventListener.EventListenersLock)
			{
				if (!this.m_disposed)
				{
					this.Deregister();
					this.m_disposed = true;
				}
			}
		}

		// Token: 0x06000099 RID: 153 RVA: 0x000078B8 File Offset: 0x00005AB8
		public virtual void Close()
		{
			this.Dispose();
		}

		// Token: 0x0600009A RID: 154 RVA: 0x000078C0 File Offset: 0x00005AC0
		~EventProvider()
		{
			this.Dispose(false);
		}

		// Token: 0x0600009B RID: 155 RVA: 0x000078F0 File Offset: 0x00005AF0
		[SecurityCritical]
		private void Deregister()
		{
			if (this.m_regHandle != 0L)
			{
				this.EventUnregister();
				this.m_regHandle = 0L;
			}
		}

		// Token: 0x0600009C RID: 156 RVA: 0x0000790C File Offset: 0x00005B0C
		[SecurityCritical]
		private unsafe void EtwEnableCallBack([In] ref Guid sourceId, [In] int controlCode, [In] byte setLevel, [In] long anyKeyword, [In] long allKeyword, [In] UnsafeNativeMethods.ManifestEtw.EVENT_FILTER_DESCRIPTOR* filterData, [In] void* callbackContext)
		{
			checked
			{
				try
				{
					ControllerCommand command = ControllerCommand.Update;
					IDictionary<string, string> dictionary = null;
					bool flag = false;
					if (controlCode == 1)
					{
						this.m_enabled = true;
						this.m_level = setLevel;
						this.m_anyKeywordMask = anyKeyword;
						this.m_allKeywordMask = allKeyword;
						List<Tuple<EventProvider.SessionInfo, bool>> sessions = this.GetSessions();
						using (List<Tuple<EventProvider.SessionInfo, bool>>.Enumerator enumerator = sessions.GetEnumerator())
						{
							while (enumerator.MoveNext())
							{
								Tuple<EventProvider.SessionInfo, bool> tuple = enumerator.Current;
								int sessionIdBit = tuple.Item1.sessionIdBit;
								int etwSessionId = tuple.Item1.etwSessionId;
								bool item = tuple.Item2;
								flag = true;
								dictionary = null;
								if (sessions.Count > 1)
								{
									filterData = null;
								}
								byte[] array;
								int i;
								if (item && this.GetDataFromController(etwSessionId, filterData, out command, out array, out i))
								{
									dictionary = new Dictionary<string, string>(4);
									while (i < array.Length)
									{
										int num = EventProvider.FindNull(array, i);
										int num2 = num + 1;
										int num3 = EventProvider.FindNull(array, num2);
										if (num3 < array.Length)
										{
											string @string = Encoding.UTF8.GetString(array, i, num - i);
											string string2 = Encoding.UTF8.GetString(array, num2, num3 - num2);
											dictionary[@string] = string2;
										}
										i = num3 + 1;
									}
								}
								this.OnControllerCommand(command, dictionary, item ? sessionIdBit : (0 - sessionIdBit), etwSessionId);
							}
							goto IL_16C;
						}
					}
					if (controlCode == 0)
					{
						this.m_enabled = false;
						this.m_level = 0;
						this.m_anyKeywordMask = 0L;
						this.m_allKeywordMask = 0L;
						this.m_liveSessions = null;
					}
					else
					{
						if (controlCode != 2)
						{
							return;
						}
						command = ControllerCommand.SendManifest;
					}
					IL_16C:
					if (!flag)
					{
						this.OnControllerCommand(command, dictionary, 0, 0);
					}
				}
				catch (Exception)
				{
				}
			}
		}

		// Token: 0x0600009D RID: 157 RVA: 0x00007ACC File Offset: 0x00005CCC
		protected virtual void OnControllerCommand(ControllerCommand command, IDictionary<string, string> arguments, int sessionId, int etwSessionId)
		{
		}

		// Token: 0x1700000E RID: 14
		// (get) Token: 0x0600009E RID: 158 RVA: 0x00007ACE File Offset: 0x00005CCE
		// (set) Token: 0x0600009F RID: 159 RVA: 0x00007AD6 File Offset: 0x00005CD6
		protected EventLevel Level
		{
			get
			{
				return (EventLevel)this.m_level;
			}
			set
			{
				this.m_level = checked((byte)value);
			}
		}

		// Token: 0x1700000F RID: 15
		// (get) Token: 0x060000A0 RID: 160 RVA: 0x00007AE0 File Offset: 0x00005CE0
		// (set) Token: 0x060000A1 RID: 161 RVA: 0x00007AE8 File Offset: 0x00005CE8
		protected EventKeywords MatchAnyKeyword
		{
			get
			{
				return (EventKeywords)this.m_anyKeywordMask;
			}
			set
			{
				this.m_anyKeywordMask = (long)value;
			}
		}

		// Token: 0x17000010 RID: 16
		// (get) Token: 0x060000A2 RID: 162 RVA: 0x00007AF1 File Offset: 0x00005CF1
		// (set) Token: 0x060000A3 RID: 163 RVA: 0x00007AF9 File Offset: 0x00005CF9
		protected EventKeywords MatchAllKeyword
		{
			get
			{
				return (EventKeywords)this.m_allKeywordMask;
			}
			set
			{
				this.m_allKeywordMask = (long)value;
			}
		}

		// Token: 0x060000A4 RID: 164 RVA: 0x00007B02 File Offset: 0x00005D02
		private static int FindNull(byte[] buffer, int idx)
		{
			checked
			{
				while (idx < buffer.Length && buffer[idx] != 0)
				{
					idx++;
				}
				return idx;
			}
		}

		// Token: 0x060000A5 RID: 165 RVA: 0x00007B30 File Offset: 0x00005D30
		[SecuritySafeCritical]
		private List<Tuple<EventProvider.SessionInfo, bool>> GetSessions()
		{
			List<EventProvider.SessionInfo> liveSessionList = null;
			this.GetSessionInfo(delegate(int etwSessionId, long matchAllKeywords)
			{
				EventProvider.GetSessionInfoCallback(etwSessionId, matchAllKeywords, ref liveSessionList);
			});
			List<Tuple<EventProvider.SessionInfo, bool>> list = new List<Tuple<EventProvider.SessionInfo, bool>>();
			if (this.m_liveSessions != null)
			{
				foreach (EventProvider.SessionInfo item in this.m_liveSessions)
				{
					int index;
					if ((index = EventProvider.IndexOfSessionInList(liveSessionList, item.etwSessionId)) < 0 || liveSessionList[index].sessionIdBit != item.sessionIdBit)
					{
						list.Add(Tuple.Create<EventProvider.SessionInfo, bool>(item, false));
					}
				}
			}
			if (liveSessionList != null)
			{
				foreach (EventProvider.SessionInfo item2 in liveSessionList)
				{
					int index2;
					if ((index2 = EventProvider.IndexOfSessionInList(this.m_liveSessions, item2.etwSessionId)) < 0 || this.m_liveSessions[index2].sessionIdBit != item2.sessionIdBit)
					{
						list.Add(Tuple.Create<EventProvider.SessionInfo, bool>(item2, true));
					}
				}
			}
			this.m_liveSessions = liveSessionList;
			return list;
		}

		// Token: 0x060000A6 RID: 166 RVA: 0x00007C84 File Offset: 0x00005E84
		private static void GetSessionInfoCallback(int etwSessionId, long matchAllKeywords, ref List<EventProvider.SessionInfo> sessionList)
		{
			uint n = (uint)SessionMask.FromEventKeywords((ulong)matchAllKeywords);
			if (EventProvider.bitcount(n) > 1)
			{
				return;
			}
			if (sessionList == null)
			{
				sessionList = new List<EventProvider.SessionInfo>(8);
			}
			checked
			{
				if (EventProvider.bitcount(n) == 1)
				{
					sessionList.Add(new EventProvider.SessionInfo(EventProvider.bitindex(n) + 1, etwSessionId));
					return;
				}
				sessionList.Add(new EventProvider.SessionInfo(EventProvider.bitcount((uint)SessionMask.All) + 1, etwSessionId));
			}
		}

		// Token: 0x060000A7 RID: 167 RVA: 0x00007CF0 File Offset: 0x00005EF0
		[SecurityCritical]
		private void GetSessionInfo(Action<int, long> action)
		{
			string text = "\\Microsoft\\Windows\\CurrentVersion\\Winevt\\Publishers\\{" + this.m_providerId + "}";
			if (Marshal.SizeOf(typeof(IntPtr)) == 8)
			{
				text = "Software\\Wow6432Node" + text;
			}
			else
			{
				text = "Software" + text;
			}
			RegistryKey registryKey = Registry.LocalMachine.OpenSubKey(text);
			checked
			{
				if (registryKey != null)
				{
					foreach (string text2 in registryKey.GetValueNames())
					{
						if (text2.StartsWith("ControllerData_Session_"))
						{
							string s = text2.Substring(23);
							int arg;
							if (int.TryParse(s, out arg))
							{
								new RegistryPermission(RegistryPermissionAccess.Read, text).Assert();
								byte[] array = registryKey.GetValue(text2) as byte[];
								if (array != null)
								{
									string @string = Encoding.UTF8.GetString(array);
									int num = @string.IndexOf("EtwSessionKeyword");
									if (0 <= num)
									{
										int num2 = num + 18;
										int num3 = @string.IndexOf('\0', num2);
										string s2 = @string.Substring(num2, num3 - num2);
										int num4;
										if (0 < num3 && int.TryParse(s2, out num4))
										{
											action(arg, 1L << num4);
										}
									}
								}
							}
						}
					}
				}
			}
		}

		// Token: 0x060000A8 RID: 168 RVA: 0x00007E28 File Offset: 0x00006028
		private static int IndexOfSessionInList(List<EventProvider.SessionInfo> sessions, int etwSessionId)
		{
			if (sessions == null)
			{
				return -1;
			}
			checked
			{
				for (int i = 0; i < sessions.Count; i++)
				{
					if (sessions[i].etwSessionId == etwSessionId)
					{
						return i;
					}
				}
				return -1;
			}
		}

		// Token: 0x060000A9 RID: 169 RVA: 0x00007E60 File Offset: 0x00006060
		[SecurityCritical]
		private unsafe bool GetDataFromController(int etwSessionId, UnsafeNativeMethods.ManifestEtw.EVENT_FILTER_DESCRIPTOR* filterData, out ControllerCommand command, out byte[] data, out int dataStart)
		{
			data = null;
			dataStart = 0;
			if (filterData != null)
			{
				if (filterData->Ptr != 0L && 0 < filterData->Size && filterData->Size <= 1024)
				{
					data = new byte[filterData->Size];
					Marshal.Copy((IntPtr)filterData->Ptr, data, 0, data.Length);
				}
				command = (ControllerCommand)filterData->Type;
				return true;
			}
			string text = "\\Microsoft\\Windows\\CurrentVersion\\Winevt\\Publishers\\{" + this.m_providerId + "}";
			if (Marshal.SizeOf(typeof(IntPtr)) == 8)
			{
				text = "HKEY_LOCAL_MACHINE\\Software\\Wow6432Node" + text;
			}
			else
			{
				text = "HKEY_LOCAL_MACHINE\\Software" + text;
			}
			string valueName = "ControllerData_Session_" + etwSessionId.ToString(CultureInfo.InvariantCulture);
			new RegistryPermission(RegistryPermissionAccess.Read, text).Assert();
			data = (Registry.GetValue(text, valueName, null) as byte[]);
			if (data != null)
			{
				command = ControllerCommand.Update;
				return true;
			}
			command = ControllerCommand.Update;
			return false;
		}

		// Token: 0x060000AA RID: 170 RVA: 0x00007F56 File Offset: 0x00006156
		public bool IsEnabled()
		{
			return this.m_enabled;
		}

		// Token: 0x060000AB RID: 171 RVA: 0x00007F60 File Offset: 0x00006160
		public bool IsEnabled(byte level, long keywords)
		{
			return this.m_enabled && ((level <= this.m_level || this.m_level == 0) && (keywords == 0L || ((keywords & this.m_anyKeywordMask) != 0L && (keywords & this.m_allKeywordMask) == this.m_allKeywordMask)));
		}

		// Token: 0x060000AC RID: 172 RVA: 0x00007FAC File Offset: 0x000061AC
		public static EventProvider.WriteEventErrorCode GetLastWriteEventError()
		{
			return EventProvider.s_returnCode;
		}

		// Token: 0x060000AD RID: 173 RVA: 0x00007FB4 File Offset: 0x000061B4
		private static void SetLastError(int error)
		{
			if (error == 8)
			{
				EventProvider.s_returnCode = EventProvider.WriteEventErrorCode.NoFreeBuffers;
				return;
			}
			if (error != 234 && error != 534)
			{
				return;
			}
			EventProvider.s_returnCode = EventProvider.WriteEventErrorCode.EventTooBig;
		}

		// Token: 0x060000AE RID: 174 RVA: 0x00007FE8 File Offset: 0x000061E8
		[SecurityCritical]
		private unsafe static object EncodeObject(ref object data, ref EventProvider.EventData* dataDescriptor, ref byte* dataBuffer, ref uint totalEventSize)
		{
			string text;
			byte[] array;
			for (;;)
			{
				dataDescriptor.Reserved = 0U;
				text = (data as string);
				array = null;
				if (text != null)
				{
					break;
				}
				if ((array = (data as byte[])) != null)
				{
					goto Block_1;
				}
				if (data is IntPtr)
				{
					goto Block_2;
				}
				if (data is int)
				{
					goto Block_3;
				}
				if (data is long)
				{
					goto Block_4;
				}
				if (data is uint)
				{
					goto Block_5;
				}
				if (data is ulong)
				{
					goto Block_6;
				}
				if (data is char)
				{
					goto Block_7;
				}
				if (data is byte)
				{
					goto Block_8;
				}
				if (data is short)
				{
					goto Block_9;
				}
				if (data is sbyte)
				{
					goto Block_10;
				}
				if (data is ushort)
				{
					goto Block_11;
				}
				if (data is float)
				{
					goto Block_12;
				}
				if (data is double)
				{
					goto Block_13;
				}
				if (data is bool)
				{
					goto Block_14;
				}
				if (data is Guid)
				{
					goto Block_16;
				}
				if (data is decimal)
				{
					goto Block_17;
				}
				if (data is DateTime)
				{
					goto Block_18;
				}
				if (!(data is Enum))
				{
					goto IL_415;
				}
				Type underlyingType = Enum.GetUnderlyingType(data.GetType());
				if (underlyingType == typeof(int))
				{
					data = ((IConvertible)data).ToInt32(null);
				}
				else
				{
					if (!(underlyingType == typeof(long)))
					{
						goto IL_415;
					}
					data = ((IConvertible)data).ToInt64(null);
				}
			}
			dataDescriptor.Size = checked(((uint)text.Length + 1U) * 2U);
			goto IL_43B;
			Block_1:
			*dataBuffer = array.Length;
			dataDescriptor.Ptr = (ulong)dataBuffer;
			dataDescriptor.Size = 4U;
			checked
			{
				totalEventSize += dataDescriptor.Size;
				dataDescriptor += (IntPtr)sizeof(EventProvider.EventData);
				dataBuffer += unchecked((IntPtr)16);
				dataDescriptor.Size = (uint)array.Length;
				goto IL_43B;
				Block_2:
				dataDescriptor.Size = (uint)sizeof(IntPtr);
				IntPtr* ptr = dataBuffer;
				*ptr = (IntPtr)data;
				dataDescriptor.Ptr = ptr;
				goto IL_43B;
				Block_3:
				dataDescriptor.Size = 4U;
				int* ptr2 = dataBuffer;
				*ptr2 = (int)data;
				dataDescriptor.Ptr = ptr2;
				goto IL_43B;
				Block_4:
				dataDescriptor.Size = 8U;
				long* ptr3 = dataBuffer;
				*ptr3 = (long)data;
				dataDescriptor.Ptr = ptr3;
				goto IL_43B;
				Block_5:
				dataDescriptor.Size = 4U;
				uint* ptr4 = dataBuffer;
				*ptr4 = (uint)data;
				dataDescriptor.Ptr = ptr4;
				goto IL_43B;
				Block_6:
				dataDescriptor.Size = 8U;
				ulong* ptr5 = dataBuffer;
				*ptr5 = (ulong)data;
				dataDescriptor.Ptr = ptr5;
				goto IL_43B;
				Block_7:
				dataDescriptor.Size = 2U;
				char* ptr6 = dataBuffer;
				*ptr6 = (char)data;
				dataDescriptor.Ptr = ptr6;
				goto IL_43B;
				Block_8:
				dataDescriptor.Size = 1U;
				byte* ptr7 = dataBuffer;
				*ptr7 = (byte)data;
				dataDescriptor.Ptr = ptr7;
				goto IL_43B;
				Block_9:
				dataDescriptor.Size = 2U;
				short* ptr8 = dataBuffer;
				*ptr8 = (short)data;
				dataDescriptor.Ptr = ptr8;
				goto IL_43B;
				Block_10:
				dataDescriptor.Size = 1U;
				sbyte* ptr9 = dataBuffer;
				*ptr9 = (sbyte)data;
				dataDescriptor.Ptr = ptr9;
				goto IL_43B;
				Block_11:
				dataDescriptor.Size = 2U;
				ushort* ptr10 = dataBuffer;
				*ptr10 = (ushort)data;
				dataDescriptor.Ptr = ptr10;
				goto IL_43B;
				Block_12:
				dataDescriptor.Size = 4U;
				float* ptr11 = dataBuffer;
				*ptr11 = (float)data;
				dataDescriptor.Ptr = ptr11;
				goto IL_43B;
				Block_13:
				dataDescriptor.Size = 8U;
				double* ptr12 = dataBuffer;
				*ptr12 = (double)data;
				dataDescriptor.Ptr = ptr12;
				goto IL_43B;
				Block_14:
				dataDescriptor.Size = 4U;
				int* ptr13 = dataBuffer;
				if ((bool)data)
				{
					*ptr13 = 1;
				}
				else
				{
					*ptr13 = 0;
				}
				dataDescriptor.Ptr = ptr13;
				goto IL_43B;
				Block_16:
				dataDescriptor.Size = (uint)sizeof(Guid);
				Guid* ptr14 = dataBuffer;
				*ptr14 = (Guid)data;
				dataDescriptor.Ptr = ptr14;
				goto IL_43B;
				Block_17:
				dataDescriptor.Size = 16U;
				decimal* ptr15 = dataBuffer;
				*ptr15 = (decimal)data;
				dataDescriptor.Ptr = ptr15;
				goto IL_43B;
				Block_18:
				long num = 0L;
				if (((DateTime)data).Ticks > 504911232000000000L)
				{
					num = ((DateTime)data).ToFileTimeUtc();
				}
				dataDescriptor.Size = 8U;
				long* ptr16 = dataBuffer;
				*ptr16 = num;
				dataDescriptor.Ptr = ptr16;
				goto IL_43B;
				IL_415:
				if (data == null)
				{
					text = "";
				}
				else
				{
					text = data.ToString();
				}
				dataDescriptor.Size = ((uint)text.Length + 1U) * 2U;
				IL_43B:
				totalEventSize += dataDescriptor.Size;
				dataDescriptor += (IntPtr)sizeof(EventProvider.EventData);
				dataBuffer += unchecked((IntPtr)16);
				return text ?? array;
			}
		}

		// Token: 0x060000AF RID: 175 RVA: 0x00008458 File Offset: 0x00006658
		[SecurityCritical]
		internal unsafe bool WriteEvent(ref EventDescriptor eventDescriptor, Guid* activityID, Guid* childActivityID, params object[] eventPayload)
		{
			int num = 0;
			checked
			{
				if (this.IsEnabled(eventDescriptor.Level, eventDescriptor.Keywords))
				{
					int num2 = eventPayload.Length;
					if (num2 > 32)
					{
						EventProvider.s_returnCode = EventProvider.WriteEventErrorCode.TooManyArgs;
						return false;
					}
					uint num3 = 0U;
					int i = 0;
					List<int> list = new List<int>(8);
					List<object> list2 = new List<object>(8);
					EventProvider.EventData* ptr = stackalloc EventProvider.EventData[unchecked((UIntPtr)(checked(2 * num2))) * (UIntPtr)sizeof(EventProvider.EventData)];
					EventProvider.EventData* ptr2 = ptr;
					byte* ptr3 = stackalloc byte[unchecked((UIntPtr)(checked(32 * num2)))];
					byte* ptr4 = ptr3;
					bool flag = false;
					for (int j = 0; j < eventPayload.Length; j++)
					{
						if (eventPayload[j] == null)
						{
							EventProvider.s_returnCode = EventProvider.WriteEventErrorCode.NullInput;
							return false;
						}
						object obj = EventProvider.EncodeObject(ref eventPayload[j], ref ptr2, ref ptr4, ref num3);
						if (obj != null)
						{
							int num4 = (int)(unchecked((long)(ptr2 - ptr)) - 1L);
							if (!(obj is string))
							{
								if (eventPayload.Length + num4 + 1 - j > 32)
								{
									EventProvider.s_returnCode = EventProvider.WriteEventErrorCode.TooManyArgs;
									return false;
								}
								flag = true;
							}
							list2.Add(obj);
							list.Add(num4);
							i++;
						}
					}
					num2 = (int)(unchecked((long)(ptr2 - ptr)));
					if (num3 > 65482U)
					{
						EventProvider.s_returnCode = EventProvider.WriteEventErrorCode.EventTooBig;
						return false;
					}
					if (!flag && i < 8)
					{
						while (i < 8)
						{
							list2.Add(null);
							i++;
						}
						unchecked
						{
							fixed (char* ptr5 = (string)list2[0], ptr6 = (string)list2[1], ptr7 = (string)list2[2], ptr8 = (string)list2[3], ptr9 = (string)list2[4], ptr10 = (string)list2[5], ptr11 = (string)list2[6], ptr12 = (string)list2[7])
							{
								ptr2 = ptr;
								if (list2[0] != null)
								{
									ptr2[list[0]].Ptr = ptr5;
								}
								if (list2[1] != null)
								{
									ptr2[list[1]].Ptr = ptr6;
								}
								if (list2[2] != null)
								{
									ptr2[list[2]].Ptr = ptr7;
								}
								if (list2[3] != null)
								{
									ptr2[list[3]].Ptr = ptr8;
								}
								if (list2[4] != null)
								{
									ptr2[list[4]].Ptr = ptr9;
								}
								if (list2[5] != null)
								{
									ptr2[list[5]].Ptr = ptr10;
								}
								if (list2[6] != null)
								{
									ptr2[list[6]].Ptr = ptr11;
								}
								if (list2[7] != null)
								{
									ptr2[list[7]].Ptr = ptr12;
								}
								num = UnsafeNativeMethods.ManifestEtw.EventWriteTransferWrapper(this.m_regHandle, ref eventDescriptor, activityID, childActivityID, num2, ptr);
							}
						}
					}
					else
					{
						ptr2 = ptr;
						GCHandle[] array = new GCHandle[i];
						for (int k = 0; k < i; k++)
						{
							array[k] = GCHandle.Alloc(list2[k], GCHandleType.Pinned);
							unchecked
							{
								if (list2[k] is string)
								{
									fixed (char* ptr13 = (string)list2[k])
									{
										ptr2[list[k]].Ptr = ptr13;
									}
								}
								else
								{
									fixed (byte* ptr14 = (byte[])list2[k])
									{
										ptr2[list[k]].Ptr = ptr14;
									}
								}
							}
						}
						num = UnsafeNativeMethods.ManifestEtw.EventWriteTransferWrapper(this.m_regHandle, ref eventDescriptor, activityID, childActivityID, num2, ptr);
						for (int l = 0; l < i; l++)
						{
							array[l].Free();
						}
					}
				}
				if (num != 0)
				{
					EventProvider.SetLastError(num);
					return false;
				}
				return true;
			}
		}

		// Token: 0x060000B0 RID: 176 RVA: 0x000088F0 File Offset: 0x00006AF0
		[SecurityCritical]
		protected internal unsafe bool WriteEvent(ref EventDescriptor eventDescriptor, Guid* activityID, Guid* childActivityID, int dataCount, IntPtr data)
		{
			UIntPtr uintPtr = (UIntPtr)0;
			int num = UnsafeNativeMethods.ManifestEtw.EventWriteTransferWrapper(this.m_regHandle, ref eventDescriptor, activityID, childActivityID, dataCount, (EventProvider.EventData*)((void*)data));
			if (num != 0)
			{
				EventProvider.SetLastError(num);
				return false;
			}
			return true;
		}

		// Token: 0x060000B1 RID: 177 RVA: 0x00008928 File Offset: 0x00006B28
		[SecurityCritical]
		internal unsafe bool WriteEventRaw(ref EventDescriptor eventDescriptor, Guid* activityID, Guid* relatedActivityID, int dataCount, IntPtr data)
		{
			int num = UnsafeNativeMethods.ManifestEtw.EventWriteTransferWrapper(this.m_regHandle, ref eventDescriptor, activityID, relatedActivityID, dataCount, (EventProvider.EventData*)((void*)data));
			if (num != 0)
			{
				EventProvider.SetLastError(num);
				return false;
			}
			return true;
		}

		// Token: 0x060000B2 RID: 178 RVA: 0x00008959 File Offset: 0x00006B59
		[SecurityCritical]
		private uint EventRegister(ref Guid providerId, UnsafeNativeMethods.ManifestEtw.EtwEnableCallback enableCallback)
		{
			this.m_providerId = providerId;
			this.m_etwCallback = enableCallback;
			return UnsafeNativeMethods.ManifestEtw.EventRegister(ref providerId, enableCallback, null, ref this.m_regHandle);
		}

		// Token: 0x060000B3 RID: 179 RVA: 0x00008980 File Offset: 0x00006B80
		[SecurityCritical]
		private uint EventUnregister()
		{
			uint result = UnsafeNativeMethods.ManifestEtw.EventUnregister(this.m_regHandle);
			this.m_regHandle = 0L;
			return result;
		}

		// Token: 0x060000B4 RID: 180 RVA: 0x000089A4 File Offset: 0x00006BA4
		private static int bitcount(uint n)
		{
			int num = 0;
			checked
			{
				while (n != 0U)
				{
					num += EventProvider.nibblebits[(int)(unchecked((UIntPtr)(n & 15U)))];
					n >>= 4;
				}
				return num;
			}
		}

		// Token: 0x060000B5 RID: 181 RVA: 0x000089CC File Offset: 0x00006BCC
		private static int bitindex(uint n)
		{
			int num = 0;
			while (((ulong)n & (ulong)(1L << (num & 31))) == 0UL)
			{
				checked
				{
					num++;
				}
			}
			return num;
		}

		// Token: 0x04000034 RID: 52
		private const int s_basicTypeAllocationBufferSize = 16;

		// Token: 0x04000035 RID: 53
		private const int s_etwMaxNumberArguments = 32;

		// Token: 0x04000036 RID: 54
		private const int s_etwAPIMaxRefObjCount = 8;

		// Token: 0x04000037 RID: 55
		private const int s_maxEventDataDescriptors = 128;

		// Token: 0x04000038 RID: 56
		private const int s_traceEventMaximumSize = 65482;

		// Token: 0x04000039 RID: 57
		private const int s_traceEventMaximumStringSize = 32724;

		// Token: 0x0400003A RID: 58
		private static bool m_setInformationMissing;

		// Token: 0x0400003B RID: 59
		[SecurityCritical]
		private UnsafeNativeMethods.ManifestEtw.EtwEnableCallback m_etwCallback;

		// Token: 0x0400003C RID: 60
		private long m_regHandle;

		// Token: 0x0400003D RID: 61
		private byte m_level;

		// Token: 0x0400003E RID: 62
		private long m_anyKeywordMask;

		// Token: 0x0400003F RID: 63
		private long m_allKeywordMask;

		// Token: 0x04000040 RID: 64
		private List<EventProvider.SessionInfo> m_liveSessions;

		// Token: 0x04000041 RID: 65
		private bool m_enabled;

		// Token: 0x04000042 RID: 66
		private Guid m_providerId;

		// Token: 0x04000043 RID: 67
		internal bool m_disposed;

		// Token: 0x04000044 RID: 68
		[ThreadStatic]
		private static EventProvider.WriteEventErrorCode s_returnCode;

		// Token: 0x04000045 RID: 69
		private static int[] nibblebits = new int[]
		{
			0,
			1,
			1,
			2,
			1,
			2,
			2,
			3,
			1,
			2,
			2,
			3,
			2,
			3,
			3,
			4
		};

		// Token: 0x02000009 RID: 9
		public struct EventData
		{
			// Token: 0x04000046 RID: 70
			internal ulong Ptr;

			// Token: 0x04000047 RID: 71
			internal uint Size;

			// Token: 0x04000048 RID: 72
			internal uint Reserved;
		}

		// Token: 0x0200000A RID: 10
		public struct SessionInfo
		{
			// Token: 0x060000B7 RID: 183 RVA: 0x00008A49 File Offset: 0x00006C49
			internal SessionInfo(int sessionIdBit_, int etwSessionId_)
			{
				this.sessionIdBit = sessionIdBit_;
				this.etwSessionId = etwSessionId_;
			}

			// Token: 0x04000049 RID: 73
			internal int sessionIdBit;

			// Token: 0x0400004A RID: 74
			internal int etwSessionId;
		}

		// Token: 0x0200000B RID: 11
		public enum WriteEventErrorCode
		{
			// Token: 0x0400004C RID: 76
			NoError,
			// Token: 0x0400004D RID: 77
			NoFreeBuffers,
			// Token: 0x0400004E RID: 78
			EventTooBig,
			// Token: 0x0400004F RID: 79
			NullInput,
			// Token: 0x04000050 RID: 80
			TooManyArgs,
			// Token: 0x04000051 RID: 81
			Other
		}
	}
}
