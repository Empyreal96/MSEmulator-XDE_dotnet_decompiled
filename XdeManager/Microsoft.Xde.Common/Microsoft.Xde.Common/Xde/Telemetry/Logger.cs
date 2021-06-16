using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Reflection;
using System.Text;
using System.Windows.Controls;
using System.Windows.Controls.Primitives;
using System.Windows.Forms;
using Microsoft.Diagnostics.Telemetry;
using Microsoft.Diagnostics.Tracing;
using Microsoft.Xde.Common;

namespace Microsoft.Xde.Telemetry
{
	// Token: 0x02000017 RID: 23
	public class Logger
	{
		// Token: 0x06000079 RID: 121 RVA: 0x00004052 File Offset: 0x00002252
		private Logger()
		{
			this.Listeners = new List<IXdeTelemetryListener>();
		}

		// Token: 0x17000038 RID: 56
		// (get) Token: 0x0600007A RID: 122 RVA: 0x00004070 File Offset: 0x00002270
		public static Logger Instance
		{
			get
			{
				if (Logger.logger == null)
				{
					object lockObj = Logger.LockObj;
					lock (lockObj)
					{
						if (Logger.logger == null)
						{
							Logger.logger = new Logger();
						}
					}
				}
				return Logger.logger;
			}
		}

		// Token: 0x17000039 RID: 57
		// (get) Token: 0x0600007B RID: 123 RVA: 0x000040C8 File Offset: 0x000022C8
		// (set) Token: 0x0600007C RID: 124 RVA: 0x000040D0 File Offset: 0x000022D0
		public IList<IXdeTelemetryListener> Listeners { get; private set; }

		// Token: 0x1700003A RID: 58
		// (get) Token: 0x0600007D RID: 125 RVA: 0x000040D9 File Offset: 0x000022D9
		public bool IsUserOptedIn
		{
			get
			{
				if (this.isUserOptedIn == null)
				{
					this.isUserOptedIn = new bool?(!RegistryHelper.TelemetryDisabled && RegistryHelper.KitsCeipOptedIn);
				}
				return this.isUserOptedIn.Value;
			}
		}

		// Token: 0x1700003B RID: 59
		// (get) Token: 0x0600007E RID: 126 RVA: 0x0000410D File Offset: 0x0000230D
		// (set) Token: 0x0600007F RID: 127 RVA: 0x00004115 File Offset: 0x00002315
		public bool IsXdeShuttingDown { get; set; }

		// Token: 0x06000080 RID: 128 RVA: 0x00004120 File Offset: 0x00002320
		public void AddSensitive(string name, string replaceWith)
		{
			if (!string.IsNullOrWhiteSpace(name) && name.Length > 3 && !this.sensitiveStrings.Exists((KeyValuePair<string, string> item) => name.Equals(item.Key)))
			{
				this.sensitiveStrings.Add(new KeyValuePair<string, string>(name, replaceWith ?? string.Empty));
			}
		}

		// Token: 0x06000081 RID: 129 RVA: 0x0000418E File Offset: 0x0000238E
		public void LogButtonClicked(string name, string location)
		{
			this.Log("ButtonClicked", Logger.Level.Measure, new
			{
				PartA_iKey = "A-MSTelDefault",
				name = name,
				location = location
			});
		}

		// Token: 0x06000082 RID: 130 RVA: 0x000041A8 File Offset: 0x000023A8
		public void LogCheckBoxClicked(string name, object sender, string location)
		{
			System.Windows.Controls.CheckBox checkBox = sender as System.Windows.Controls.CheckBox;
			if (checkBox != null)
			{
				this.Log("CheckBoxClicked", Logger.Level.Measure, new
				{
					PartA_iKey = "A-MSTelDefault",
					name = name,
					isChecked = checkBox.IsChecked,
					location = location
				});
			}
		}

		// Token: 0x06000083 RID: 131 RVA: 0x000041E0 File Offset: 0x000023E0
		public void LogToggleButtonClicked(string name, object sender, string location)
		{
			ToggleButton toggleButton = sender as ToggleButton;
			if (toggleButton != null)
			{
				this.Log("CheckBoxClicked", Logger.Level.Measure, new
				{
					PartA_iKey = "A-MSTelDefault",
					name = name,
					isChecked = toggleButton.IsChecked,
					location = location
				});
			}
		}

		// Token: 0x06000084 RID: 132 RVA: 0x00004218 File Offset: 0x00002418
		public void LogComboBoxChanged(string name, SelectionChangedEventArgs e, string location)
		{
			if (e != null && e.RemovedItems != null && e.RemovedItems.Count > 0 && e.AddedItems != null && e.AddedItems.Count > 0)
			{
				string text = string.Empty;
				ComboBoxItem comboBoxItem = e.AddedItems[0] as ComboBoxItem;
				if (comboBoxItem != null)
				{
					text = (comboBoxItem.Content as string);
				}
				if ((text == null || text == string.Empty) && e.Source != null && e.Source is System.Windows.Controls.ComboBox)
				{
					text = ((System.Windows.Controls.ComboBox)e.Source).SelectedIndex.ToString();
				}
				this.Log("ComboBoxChanged", Logger.Level.Measure, new
				{
					PartA_iKey = "A-MSTelDefault",
					name = name,
					item = text,
					location = location
				});
			}
		}

		// Token: 0x06000085 RID: 133 RVA: 0x000042DE File Offset: 0x000024DE
		public void LogTabClicked(string name)
		{
			this.Log("TabClicked", Logger.Level.Measure, new
			{
				PartA_iKey = "A-MSTelDefault",
				name = name
			});
		}

		// Token: 0x06000086 RID: 134 RVA: 0x000042F8 File Offset: 0x000024F8
		public void LogTextBoxNumberChanged(string name, object sender, string location)
		{
			System.Windows.Controls.TextBox textBox = sender as System.Windows.Controls.TextBox;
			if (textBox != null)
			{
				this.Log("NumberChanged", Logger.Level.Measure, new
				{
					PartA_iKey = "A-MSTelDefault",
					name = name,
					number = Convert.ToInt32(textBox.Text),
					location = location
				});
			}
		}

		// Token: 0x06000087 RID: 135 RVA: 0x00004332 File Offset: 0x00002532
		public void LogException(string action, Exception e)
		{
			this.LogException(action, e, false);
		}

		// Token: 0x06000088 RID: 136 RVA: 0x00004340 File Offset: 0x00002540
		public void LogException(string action, Exception e, bool isShuttingDown)
		{
			if ((!isShuttingDown && !this.IsXdeShuttingDown) || !(e.GetType().Name == "XdePipeException"))
			{
				string message = this.ReplaceSensitiveStrings(e.Message);
				Exception innerException = e.InnerException;
				string innerMessage = this.ReplaceSensitiveStrings((innerException != null) ? innerException.Message : null);
				string stackTrace;
				try
				{
					stackTrace = e.StackTrace;
				}
				catch (Exception ex)
				{
					stackTrace = "Failed to get stack trace: " + ex.Message;
				}
				this.LogError("ExceptionThrown", new
				{
					PartA_iKey = "A-MSTelDefault",
					action = action,
					name = e.GetType().Name,
					pipeName = Logger.GetPropValue<string>(e, "PipeName"),
					message = message,
					stackTrace = stackTrace,
					shuttingDown = (isShuttingDown || this.IsXdeShuttingDown),
					innerName = ((e.InnerException == null) ? null : e.InnerException.GetType().Name),
					innerMessage = innerMessage
				});
			}
		}

		// Token: 0x06000089 RID: 137 RVA: 0x0000441C File Offset: 0x0000261C
		public void LogCommandLineParameterInvalid(string name, string parameter)
		{
			this.LogError("CommandLineParameterInvalid", new
			{
				PartA_iKey = "A-MSTelDefault",
				name = name,
				parameter = parameter
			});
		}

		// Token: 0x0600008A RID: 138 RVA: 0x00004435 File Offset: 0x00002635
		public void LogDialogRespondedTo(string question, uint result)
		{
			this.Log("DialogRespondedTo", Logger.Level.Measure, new
			{
				PartA_iKey = "A-MSTelDefault",
				question = question,
				result = result
			});
		}

		// Token: 0x0600008B RID: 139 RVA: 0x0000444F File Offset: 0x0000264F
		public void LogAddUserToSecurityGroupIfRequestedError(int returnValue)
		{
			this.LogError("AddUserToSecurityGroupIfRequestedError", new
			{
				PartA_iKey = "A-MSTelDefault",
				returnValue = returnValue
			});
		}

		// Token: 0x0600008C RID: 140 RVA: 0x00004467 File Offset: 0x00002667
		public void LogTimeTaken(string eventName, uint timeTakenMilliseconds)
		{
			this.Log(eventName, Logger.Level.Measure, new
			{
				PartA_iKey = "A-MSTelDefault",
				timeTakenMilliseconds = timeTakenMilliseconds
			});
		}

		// Token: 0x0600008D RID: 141 RVA: 0x0000447C File Offset: 0x0000267C
		public void LogHotkeyPressed(Keys key, HowCommandExecuted howExecuted)
		{
			string text;
			if (key != Keys.Escape)
			{
				switch (key)
				{
				case Keys.F1:
					text = "Back";
					break;
				case Keys.F2:
					text = "Start";
					break;
				case Keys.F3:
					text = "Search";
					break;
				case Keys.F4:
					text = "ToggleKeyboard";
					break;
				case Keys.F5:
					text = "F5";
					break;
				case Keys.F6:
					text = "CameraHalf";
					break;
				case Keys.F7:
					text = "CameraFull";
					break;
				case Keys.F8:
					text = "F8";
					break;
				case Keys.F9:
					text = "VolumeUp";
					break;
				case Keys.F10:
					text = "VolumeDown";
					break;
				case Keys.F11:
					text = "F11";
					break;
				case Keys.F12:
					text = "Power";
					break;
				default:
					text = string.Empty;
					break;
				}
			}
			else
			{
				text = "Back";
			}
			if (!string.IsNullOrEmpty(text))
			{
				this.Log("HotkeyPressed", Logger.Level.Measure, new
				{
					PartA_iKey = "A-MSTelDefault",
					command = text,
					howExecuted = howExecuted
				});
			}
		}

		// Token: 0x0600008E RID: 142 RVA: 0x0000455A File Offset: 0x0000275A
		public void Log<T>(string eventName, Logger.Level level, T data)
		{
			this.WriteTelemetryEvent<T>(eventName, level, false, data);
		}

		// Token: 0x0600008F RID: 143 RVA: 0x00004566 File Offset: 0x00002766
		public void Log(string eventName, Logger.Level level)
		{
			this.Log(eventName, level, new
			{
				PartA_iKey = "A-MSTelDefault"
			});
		}

		// Token: 0x06000090 RID: 144 RVA: 0x0000457A File Offset: 0x0000277A
		public void LogError<T>(string eventName, T data)
		{
			this.WriteTelemetryEvent<T>(eventName, Logger.Level.Measure, true, data);
		}

		// Token: 0x06000091 RID: 145 RVA: 0x00004586 File Offset: 0x00002786
		public void LogError(string eventName)
		{
			this.LogError(eventName, new
			{
				PartA_iKey = "A-MSTelDefault"
			});
		}

		// Token: 0x06000092 RID: 146 RVA: 0x00004599 File Offset: 0x00002799
		public void LogAssert(bool condition)
		{
			this.LogAssert(condition, string.Empty, string.Empty);
		}

		// Token: 0x06000093 RID: 147 RVA: 0x000045AC File Offset: 0x000027AC
		public void LogAssert(bool condition, string message)
		{
			this.LogAssert(condition, message, string.Empty);
		}

		// Token: 0x06000094 RID: 148 RVA: 0x000045BB File Offset: 0x000027BB
		public void LogAssert(bool condition, string message, string detailMessage)
		{
			this.SendAssertTelemetry(condition, message, detailMessage);
		}

		// Token: 0x06000095 RID: 149 RVA: 0x000045C6 File Offset: 0x000027C6
		public void LogAssertNoPopup(bool condition)
		{
			this.SendAssertTelemetry(condition, string.Empty, string.Empty);
		}

		// Token: 0x06000096 RID: 150 RVA: 0x000045D9 File Offset: 0x000027D9
		public void LogAssertNoPopup(bool condition, string message)
		{
			this.SendAssertTelemetry(condition, message, string.Empty);
		}

		// Token: 0x06000097 RID: 151 RVA: 0x000045BB File Offset: 0x000027BB
		public void LogAssertNoPopup(bool condition, string message, string detailMessage)
		{
			this.SendAssertTelemetry(condition, message, detailMessage);
		}

		// Token: 0x06000098 RID: 152 RVA: 0x000045E8 File Offset: 0x000027E8
		public void LogSensorsEnabled(string reason, XdeSensors xdeSensors)
		{
			this.Log("SensorsEnabled", Logger.Level.Measure, new
			{
				PartA_iKey = "A-MSTelDefault",
				reason = reason,
				als = ((xdeSensors & XdeSensors.ALS) > XdeSensors.None),
				camera_8_1 = ((xdeSensors & XdeSensors.Camera_8_1) > XdeSensors.None),
				ffc = ((xdeSensors & XdeSensors.FFC) > XdeSensors.None),
				gyro = ((xdeSensors & XdeSensors.Gyro) > XdeSensors.None),
				magnetometer = ((xdeSensors & XdeSensors.Magnetometer) > XdeSensors.None),
				nFC = ((xdeSensors & XdeSensors.NFC) > XdeSensors.None),
				softwareButtons = ((xdeSensors & XdeSensors.SoftwareButtons) > XdeSensors.None),
				desktopGPU = ((xdeSensors & XdeSensors.DesktopGPU) > XdeSensors.None)
			});
		}

		// Token: 0x06000099 RID: 153 RVA: 0x00004648 File Offset: 0x00002848
		public string ReplaceSensitiveStrings(string message)
		{
			if (message != null)
			{
				foreach (KeyValuePair<string, string> keyValuePair in this.sensitiveStrings)
				{
					StringBuilder stringBuilder = new StringBuilder();
					int num;
					for (int i = 0; i < message.Length; i = num + keyValuePair.Key.Length)
					{
						num = message.IndexOf(keyValuePair.Key, i, StringComparison.OrdinalIgnoreCase);
						if (num < 0)
						{
							stringBuilder.Append(message, i, message.Length - i);
							message = stringBuilder.ToString();
							break;
						}
						stringBuilder.Append(message, i, num - i);
						stringBuilder.Append(keyValuePair.Value);
					}
				}
			}
			return message;
		}

		// Token: 0x0600009A RID: 154 RVA: 0x0000470C File Offset: 0x0000290C
		private static object GetPropValue(object obj, string name)
		{
			foreach (string name2 in name.Split(new char[]
			{
				'.'
			}))
			{
				if (obj == null)
				{
					return null;
				}
				PropertyInfo property = obj.GetType().GetProperty(name2);
				if (property == null)
				{
					return null;
				}
				obj = property.GetValue(obj, null);
			}
			return obj;
		}

		// Token: 0x0600009B RID: 155 RVA: 0x00004768 File Offset: 0x00002968
		private static T GetPropValue<T>(object obj, string name)
		{
			object propValue = Logger.GetPropValue(obj, name);
			if (propValue == null)
			{
				return default(T);
			}
			return (T)((object)propValue);
		}

		// Token: 0x0600009C RID: 156 RVA: 0x00004790 File Offset: 0x00002990
		private void WriteTelemetryEvent<T>(string eventName, Logger.Level level, bool isError, T data)
		{
			foreach (IXdeTelemetryListener xdeTelemetryListener in this.Listeners)
			{
				xdeTelemetryListener.WriteEvent<T>(eventName, level, isError, data);
			}
			EventSourceOptions eventSourceOptions;
			if (this.IsUserOptedIn)
			{
				switch (level)
				{
				case Logger.Level.Info:
					eventSourceOptions = (isError ? Logger.measureErrorOption : Logger.measureOption);
					goto IL_94;
				case Logger.Level.Measure:
					eventSourceOptions = (isError ? Logger.criticalErrorOption : Logger.criticalOption);
					goto IL_94;
				}
				eventSourceOptions = (isError ? Logger.localErrorOption : Logger.localOption);
			}
			else
			{
				eventSourceOptions = (isError ? Logger.localErrorOption : Logger.localOption);
			}
			IL_94:
			Logger.eventSource.Write<T>(eventName, ref eventSourceOptions, ref Logger.activityId, ref Logger.emptyGuid, ref data);
		}

		// Token: 0x0600009D RID: 157 RVA: 0x0000485C File Offset: 0x00002A5C
		private void SendAssertTelemetry(bool condition, string message = "", string detailMessage = "")
		{
			try
			{
				if (!condition && Logger.maxNumberTelemetryAsserts >= 0)
				{
					Logger.maxNumberTelemetryAsserts--;
					StackTrace stackTrace = new StackTrace();
					string method = string.Empty;
					if (stackTrace.FrameCount > 2)
					{
						MethodBase method2 = stackTrace.GetFrame(2).GetMethod();
						if (method2 != null)
						{
							method = method2.Name;
						}
					}
					this.LogError("Assert", new
					{
						PartA_iKey = "A-MSTelDefault",
						method = method,
						message = message,
						detailMessage = detailMessage,
						stackTrace = stackTrace.ToString()
					});
				}
			}
			catch (Exception e)
			{
				this.LogException("SendAssertTelemetry", e);
			}
		}

		// Token: 0x040000B5 RID: 181
		public const string XdeIkeyName = "A-MSTelDefault";

		// Token: 0x040000B6 RID: 182
		private const string XdeProviderName = "Microsoft.Emulator.Xde.RunTime";

		// Token: 0x040000B7 RID: 183
		private static readonly object LockObj = new object();

		// Token: 0x040000B8 RID: 184
		private static Logger logger;

		// Token: 0x040000B9 RID: 185
		private static Guid activityId = Guid.NewGuid();

		// Token: 0x040000BA RID: 186
		private static Guid emptyGuid = Guid.Empty;

		// Token: 0x040000BB RID: 187
		private static int maxNumberTelemetryAsserts = 5;

		// Token: 0x040000BC RID: 188
		private static EventSource eventSource = new PartnerTelemetryEventSource("Microsoft.Emulator.Xde.RunTime");

		// Token: 0x040000BD RID: 189
		private static EventSourceOptions localOption = new EventSourceOptions
		{
			Level = EventLevel.Verbose
		};

		// Token: 0x040000BE RID: 190
		private static EventSourceOptions localErrorOption = new EventSourceOptions
		{
			Level = EventLevel.Error
		};

		// Token: 0x040000BF RID: 191
		private static EventSourceOptions infoOption = new EventSourceOptions
		{
			Keywords = (EventKeywords)35184372088832L
		};

		// Token: 0x040000C0 RID: 192
		private static EventSourceOptions infoErrorOption = new EventSourceOptions
		{
			Level = EventLevel.Error,
			Keywords = (EventKeywords)35184372088832L
		};

		// Token: 0x040000C1 RID: 193
		private static EventSourceOptions measureOption = new EventSourceOptions
		{
			Keywords = (EventKeywords)70368744177664L
		};

		// Token: 0x040000C2 RID: 194
		private static EventSourceOptions measureErrorOption = new EventSourceOptions
		{
			Level = EventLevel.Error,
			Keywords = (EventKeywords)70368744177664L
		};

		// Token: 0x040000C3 RID: 195
		private static EventSourceOptions criticalOption = new EventSourceOptions
		{
			Keywords = (EventKeywords)140737488355328L
		};

		// Token: 0x040000C4 RID: 196
		private static EventSourceOptions criticalErrorOption = new EventSourceOptions
		{
			Level = EventLevel.Error,
			Keywords = (EventKeywords)140737488355328L
		};

		// Token: 0x040000C5 RID: 197
		private bool? isUserOptedIn;

		// Token: 0x040000C6 RID: 198
		private List<KeyValuePair<string, string>> sensitiveStrings = new List<KeyValuePair<string, string>>();

		// Token: 0x0200007E RID: 126
		public enum Level
		{
			// Token: 0x040001C7 RID: 455
			Local,
			// Token: 0x040001C8 RID: 456
			Info,
			// Token: 0x040001C9 RID: 457
			Measure
		}
	}
}
