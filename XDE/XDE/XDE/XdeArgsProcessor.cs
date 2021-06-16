using System;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.ComponentModel.Composition;
using System.IO;
using System.Linq;
using System.Text;
using Microsoft.Win32;
using Microsoft.Xde.Common;
using Microsoft.Xde.DeviceManagement;

namespace Microsoft.Xde.Client
{
	// Token: 0x02000024 RID: 36
	[Export(typeof(IXdeArgsProcessor))]
	public class XdeArgsProcessor : IXdeArgsProcessor
	{
		// Token: 0x060001D8 RID: 472 RVA: 0x000088E8 File Offset: 0x00006AE8
		public XdeArgsProcessor()
		{
			this.sensorsEnabled = new BitVector32(383);
			this.Sku = "WP";
		}

		// Token: 0x170000C1 RID: 193
		// (get) Token: 0x060001D9 RID: 473 RVA: 0x0000890B File Offset: 0x00006B0B
		// (set) Token: 0x060001DA RID: 474 RVA: 0x00008913 File Offset: 0x00006B13
		public string VirtualMachineName { get; private set; }

		// Token: 0x170000C2 RID: 194
		// (get) Token: 0x060001DB RID: 475 RVA: 0x0000891C File Offset: 0x00006B1C
		// (set) Token: 0x060001DC RID: 476 RVA: 0x00008924 File Offset: 0x00006B24
		public string VhdPath { get; private set; }

		// Token: 0x170000C3 RID: 195
		// (get) Token: 0x060001DD RID: 477 RVA: 0x0000892D File Offset: 0x00006B2D
		// (set) Token: 0x060001DE RID: 478 RVA: 0x00008935 File Offset: 0x00006B35
		public string OriginalVideoResolution { get; private set; }

		// Token: 0x170000C4 RID: 196
		// (get) Token: 0x060001DF RID: 479 RVA: 0x0000893E File Offset: 0x00006B3E
		public string VideoResolution
		{
			get
			{
				if (!string.IsNullOrEmpty(this.OriginalVideoResolution))
				{
					return this.OriginalVideoResolution;
				}
				return "480x800";
			}
		}

		// Token: 0x170000C5 RID: 197
		// (get) Token: 0x060001E0 RID: 480 RVA: 0x00008959 File Offset: 0x00006B59
		// (set) Token: 0x060001E1 RID: 481 RVA: 0x00008961 File Offset: 0x00006B61
		public string Language { get; private set; }

		// Token: 0x170000C6 RID: 198
		// (get) Token: 0x060001E2 RID: 482 RVA: 0x0000896A File Offset: 0x00006B6A
		// (set) Token: 0x060001E3 RID: 483 RVA: 0x00008972 File Offset: 0x00006B72
		public string BootLanguage { get; private set; }

		// Token: 0x170000C7 RID: 199
		// (get) Token: 0x060001E4 RID: 484 RVA: 0x0000897B File Offset: 0x00006B7B
		// (set) Token: 0x060001E5 RID: 485 RVA: 0x00008983 File Offset: 0x00006B83
		public bool ShowUsage { get; private set; }

		// Token: 0x170000C8 RID: 200
		// (get) Token: 0x060001E6 RID: 486 RVA: 0x0000898C File Offset: 0x00006B8C
		// (set) Token: 0x060001E7 RID: 487 RVA: 0x00008994 File Offset: 0x00006B94
		public string ScreenDiagonalSize { get; private set; }

		// Token: 0x170000C9 RID: 201
		// (get) Token: 0x060001E8 RID: 488 RVA: 0x0000899D File Offset: 0x00006B9D
		// (set) Token: 0x060001E9 RID: 489 RVA: 0x000089A5 File Offset: 0x00006BA5
		public bool ShowName { get; private set; }

		// Token: 0x170000CA RID: 202
		// (get) Token: 0x060001EA RID: 490 RVA: 0x000089AE File Offset: 0x00006BAE
		// (set) Token: 0x060001EB RID: 491 RVA: 0x000089B6 File Offset: 0x00006BB6
		public bool NoStart { get; private set; }

		// Token: 0x170000CB RID: 203
		// (get) Token: 0x060001EC RID: 492 RVA: 0x000089BF File Offset: 0x00006BBF
		// (set) Token: 0x060001ED RID: 493 RVA: 0x000089C7 File Offset: 0x00006BC7
		public bool FastShutdown { get; private set; }

		// Token: 0x170000CC RID: 204
		// (get) Token: 0x060001EE RID: 494 RVA: 0x000089D0 File Offset: 0x00006BD0
		// (set) Token: 0x060001EF RID: 495 RVA: 0x000089D8 File Offset: 0x00006BD8
		public int? MemSize { get; private set; }

		// Token: 0x170000CD RID: 205
		// (get) Token: 0x060001F0 RID: 496 RVA: 0x000089E1 File Offset: 0x00006BE1
		// (set) Token: 0x060001F1 RID: 497 RVA: 0x000089E9 File Offset: 0x00006BE9
		public string Com1PipeName { get; private set; }

		// Token: 0x170000CE RID: 206
		// (get) Token: 0x060001F2 RID: 498 RVA: 0x000089F2 File Offset: 0x00006BF2
		// (set) Token: 0x060001F3 RID: 499 RVA: 0x000089FA File Offset: 0x00006BFA
		public string Com2PipeName { get; private set; }

		// Token: 0x170000CF RID: 207
		// (get) Token: 0x060001F4 RID: 500 RVA: 0x00008A04 File Offset: 0x00006C04
		// (set) Token: 0x060001F5 RID: 501 RVA: 0x00008A61 File Offset: 0x00006C61
		public string DiffDiskVhd
		{
			get
			{
				if (string.IsNullOrEmpty(this.diffDiskVhd) && this.calculateDiffDisk && !string.IsNullOrEmpty(this.VhdPath))
				{
					string directoryName = Path.GetDirectoryName(this.VhdPath);
					string path = "diff." + Path.GetFileName(this.VhdPath);
					return Path.Combine(directoryName, path);
				}
				return this.diffDiskVhd;
			}
			private set
			{
				this.diffDiskVhd = value;
			}
		}

		// Token: 0x170000D0 RID: 208
		// (get) Token: 0x060001F6 RID: 502 RVA: 0x00008A6A File Offset: 0x00006C6A
		// (set) Token: 0x060001F7 RID: 503 RVA: 0x00008A72 File Offset: 0x00006C72
		public bool ReuseExistingDiffDisk { get; private set; }

		// Token: 0x170000D1 RID: 209
		// (get) Token: 0x060001F8 RID: 504 RVA: 0x00008A7B File Offset: 0x00006C7B
		// (set) Token: 0x060001F9 RID: 505 RVA: 0x00008A83 File Offset: 0x00006C83
		public bool SilentUi { get; private set; }

		// Token: 0x170000D2 RID: 210
		// (get) Token: 0x060001FA RID: 506 RVA: 0x00008A8C File Offset: 0x00006C8C
		// (set) Token: 0x060001FB RID: 507 RVA: 0x00008A94 File Offset: 0x00006C94
		public bool SilentSnapshot { get; private set; }

		// Token: 0x170000D3 RID: 211
		// (get) Token: 0x060001FC RID: 508 RVA: 0x00008A9D File Offset: 0x00006C9D
		// (set) Token: 0x060001FD RID: 509 RVA: 0x00008AA5 File Offset: 0x00006CA5
		public bool WaitForClientConnection { get; private set; }

		// Token: 0x170000D4 RID: 212
		// (get) Token: 0x060001FE RID: 510 RVA: 0x00008AAE File Offset: 0x00006CAE
		// (set) Token: 0x060001FF RID: 511 RVA: 0x00008AB6 File Offset: 0x00006CB6
		public bool BootToSnapshot { get; private set; }

		// Token: 0x170000D5 RID: 213
		// (get) Token: 0x06000200 RID: 512 RVA: 0x00008ABF File Offset: 0x00006CBF
		// (set) Token: 0x06000201 RID: 513 RVA: 0x00008AC7 File Offset: 0x00006CC7
		public bool UseWmi { get; private set; }

		// Token: 0x170000D6 RID: 214
		// (get) Token: 0x06000202 RID: 514 RVA: 0x00008AD0 File Offset: 0x00006CD0
		// (set) Token: 0x06000203 RID: 515 RVA: 0x00008AD8 File Offset: 0x00006CD8
		public bool DisableStateSep { get; private set; }

		// Token: 0x170000D7 RID: 215
		// (get) Token: 0x06000204 RID: 516 RVA: 0x00008AE1 File Offset: 0x00006CE1
		// (set) Token: 0x06000205 RID: 517 RVA: 0x00008AE9 File Offset: 0x00006CE9
		public string DisplayName { get; private set; }

		// Token: 0x170000D8 RID: 216
		// (get) Token: 0x06000206 RID: 518 RVA: 0x00008AF2 File Offset: 0x00006CF2
		// (set) Token: 0x06000207 RID: 519 RVA: 0x00008AFA File Offset: 0x00006CFA
		public string AddUserToHyperVAdmins { get; private set; }

		// Token: 0x170000D9 RID: 217
		// (get) Token: 0x06000208 RID: 520 RVA: 0x00008B03 File Offset: 0x00006D03
		// (set) Token: 0x06000209 RID: 521 RVA: 0x00008B0B File Offset: 0x00006D0B
		public string AddUserToPerformanceLogUsersGroup { get; private set; }

		// Token: 0x170000DA RID: 218
		// (get) Token: 0x0600020A RID: 522 RVA: 0x00008B14 File Offset: 0x00006D14
		public XdeSensors SensorsEnabled
		{
			get
			{
				return (XdeSensors)this.sensorsEnabled.Data;
			}
		}

		// Token: 0x170000DB RID: 219
		// (get) Token: 0x0600020B RID: 523 RVA: 0x00008B21 File Offset: 0x00006D21
		// (set) Token: 0x0600020C RID: 524 RVA: 0x00008B29 File Offset: 0x00006D29
		public string Sku { get; private set; }

		// Token: 0x170000DC RID: 220
		// (get) Token: 0x0600020D RID: 525 RVA: 0x00008B32 File Offset: 0x00006D32
		// (set) Token: 0x0600020E RID: 526 RVA: 0x00008B3A File Offset: 0x00006D3A
		public bool AutomateFeatures { get; private set; }

		// Token: 0x170000DD RID: 221
		// (get) Token: 0x0600020F RID: 527 RVA: 0x00008B43 File Offset: 0x00006D43
		// (set) Token: 0x06000210 RID: 528 RVA: 0x00008B4B File Offset: 0x00006D4B
		public int PipeTimeout { get; private set; }

		// Token: 0x170000DE RID: 222
		// (get) Token: 0x06000211 RID: 529 RVA: 0x00008B54 File Offset: 0x00006D54
		// (set) Token: 0x06000212 RID: 530 RVA: 0x00008B5C File Offset: 0x00006D5C
		public bool ServiceDebugEnabled { get; private set; }

		// Token: 0x06000213 RID: 531 RVA: 0x00008B68 File Offset: 0x00006D68
		public void ParseArgs(string[] args)
		{
			this.VirtualMachineName = "Default Emulator";
			this.AutomateFeatures = true;
			foreach (ParsedArg parsedArg in ArgsParser.ParseArgs(args))
			{
				if (string.IsNullOrEmpty(parsedArg.Name))
				{
					this.ShowUsage = true;
				}
				else
				{
					string text = parsedArg.Name.ToUpperInvariant();
					uint num = <PrivateImplementationDetails>.ComputeStringHash(text);
					if (num <= 1701974294U)
					{
						if (num <= 1019099605U)
						{
							if (num <= 351217429U)
							{
								if (num <= 210398395U)
								{
									if (num != 116841796U)
									{
										if (num == 210398395U)
										{
											if (text == "LANGUAGE")
											{
												if (parsedArg.Values.Count != 1)
												{
													this.ShowUsage = true;
													continue;
												}
												this.Language = parsedArg.Values[0];
												continue;
											}
										}
									}
									else if (text == "SILENT")
									{
										if (parsedArg.Values.Count != 0)
										{
											this.ShowUsage = true;
											continue;
										}
										this.SilentUi = true;
										continue;
									}
								}
								else if (num != 224379551U)
								{
									if (num == 351217429U)
									{
										if (text == "VHD")
										{
											if (parsedArg.Values.Count != 1)
											{
												this.ShowUsage = true;
												continue;
											}
											this.VhdPath = XdeArgsProcessor.EnsurePathIsRooted(parsedArg.Values[0]);
											continue;
										}
									}
								}
								else if (text == "MEMSIZE")
								{
									int value;
									if (parsedArg.Values.Count != 1 || !int.TryParse(parsedArg.Values[0], out value))
									{
										this.ShowUsage = true;
										continue;
									}
									this.MemSize = new int?(value);
									continue;
								}
							}
							else if (num <= 749812577U)
							{
								if (num != 470137146U)
								{
									if (num == 749812577U)
									{
										if (text == "NOMAG")
										{
											if (parsedArg.Values.Count != 0)
											{
												this.ShowUsage = true;
												continue;
											}
											this.sensorsEnabled[16] = false;
											continue;
										}
									}
								}
								else if (text == "SKU")
								{
									if (parsedArg.Values.Count != 1)
									{
										this.ShowUsage = true;
										continue;
									}
									this.Sku = parsedArg.Values[0];
									continue;
								}
							}
							else if (num != 857235725U)
							{
								if (num != 1015612641U)
								{
									if (num == 1019099605U)
									{
										if (text == "NOGYRO")
										{
											if (parsedArg.Values.Count != 0)
											{
												this.ShowUsage = true;
												continue;
											}
											this.sensorsEnabled[8] = false;
											continue;
										}
									}
								}
								else if (text == "ADDUSERTOPERFORMANCELOGUSERSGROUP")
								{
									if (parsedArg.Values.Count != 1)
									{
										this.ShowUsage = true;
										continue;
									}
									this.AddUserToPerformanceLogUsersGroup = parsedArg.Values[0];
									continue;
								}
							}
							else if (text == "SNAPSHOT")
							{
								if (parsedArg.Values.Count != 0)
								{
									this.ShowUsage = true;
									continue;
								}
								this.BootToSnapshot = true;
								continue;
							}
						}
						else if (num <= 1268059172U)
						{
							if (num <= 1114469871U)
							{
								if (num != 1092809359U)
								{
									if (num == 1114469871U)
									{
										if (text == "NOFFC")
										{
											if (parsedArg.Values.Count != 0)
											{
												this.ShowUsage = true;
												continue;
											}
											this.sensorsEnabled[4] = false;
											continue;
										}
									}
								}
								else if (text == "USEWMI")
								{
									if (parsedArg.Values.Count != 0)
									{
										this.ShowUsage = true;
										continue;
									}
									this.UseWmi = true;
									continue;
								}
							}
							else if (num != 1150800334U)
							{
								if (num == 1268059172U)
								{
									if (text == "COM2")
									{
										if (parsedArg.Values.Count != 1)
										{
											this.ShowUsage = true;
											continue;
										}
										this.Com2PipeName = XdeArgsProcessor.FixPipeName(parsedArg.Values[0]);
										continue;
									}
								}
							}
							else if (text == "CAMERA")
							{
								if (parsedArg.Values.Count != 1 || parsedArg.Values[0].ToUpperInvariant() != "WP8")
								{
									this.ShowUsage = true;
									continue;
								}
								this.sensorsEnabled[2] = false;
								continue;
							}
						}
						else if (num <= 1387956774U)
						{
							if (num != 1318392029U)
							{
								if (num == 1387956774U)
								{
									if (text == "NAME")
									{
										if (parsedArg.Values.Count != 1)
										{
											this.ShowUsage = true;
											continue;
										}
										this.VirtualMachineName = parsedArg.Values[0];
										continue;
									}
								}
							}
							else if (text == "COM1")
							{
								if (parsedArg.Values.Count != 1)
								{
									this.ShowUsage = true;
									continue;
								}
								this.Com1PipeName = XdeArgsProcessor.FixPipeName(parsedArg.Values[0]);
								continue;
							}
						}
						else if (num != 1579987875U)
						{
							if (num != 1629691752U)
							{
								if (num == 1701974294U)
								{
									if (text == "DISPLAYNAME")
									{
										if (parsedArg.Values.Count != 1)
										{
											this.ShowUsage = true;
											continue;
										}
										this.DisplayName = parsedArg.Values[0];
										continue;
									}
								}
							}
							else if (text == "TIMEOUT")
							{
								int num2 = 10;
								if (parsedArg.Values.Count != 1 || !int.TryParse(parsedArg.Values[0], out num2))
								{
									this.ShowUsage = true;
									continue;
								}
								this.PipeTimeout = num2 * 60 * 1000;
								continue;
							}
						}
						else if (text == "DIAGONALSIZE")
						{
							if (parsedArg.Values.Count != 1)
							{
								this.ShowUsage = true;
								continue;
							}
							this.ScreenDiagonalSize = parsedArg.Values[0];
							continue;
						}
					}
					else
					{
						if (num <= 2554608160U)
						{
							if (num <= 1807303837U)
							{
								if (num <= 1719708149U)
								{
									if (num != 1710918320U)
									{
										if (num != 1719708149U)
										{
											goto IL_C6F;
										}
										if (!(text == "FASTSHUTDOWN"))
										{
											goto IL_C6F;
										}
										if (parsedArg.Values.Count != 0)
										{
											this.ShowUsage = true;
											continue;
										}
										this.FastShutdown = true;
										continue;
									}
									else
									{
										if (!(text == "SILENTSNAPSHOT"))
										{
											goto IL_C6F;
										}
										if (parsedArg.Values.Count != 0)
										{
											this.ShowUsage = true;
											continue;
										}
										this.SilentSnapshot = true;
										if (!this.BootToSnapshot)
										{
											this.BootToSnapshot = true;
										}
										this.SilentUi = true;
										continue;
									}
								}
								else if (num != 1746005427U)
								{
									if (num != 1807303837U)
									{
										goto IL_C6F;
									}
									if (!(text == "NOSENSORS"))
									{
										goto IL_C6F;
									}
									if (parsedArg.Values.Count != 0)
									{
										this.ShowUsage = true;
										continue;
									}
									this.sensorsEnabled[511] = false;
									continue;
								}
								else
								{
									if (!(text == "DEVICE"))
									{
										goto IL_C6F;
									}
									if (parsedArg.Values.Count != 1)
									{
										this.ShowUsage = true;
										continue;
									}
									XdeDevice xdeDevice = XdeDeviceFactory.FindDevice(parsedArg.Values[0]);
									if (xdeDevice == null)
									{
										this.ShowUsage = true;
										continue;
									}
									string[] args2 = XdeArgsProcessor.ParseCommandLineToArgs(xdeDevice.GetCommandLine()).ToArray<string>();
									this.ParseArgs(args2);
									continue;
								}
							}
							else if (num <= 2242519436U)
							{
								if (num != 1917552076U)
								{
									if (num != 2242519436U)
									{
										goto IL_C6F;
									}
									if (!(text == "NOALS"))
									{
										goto IL_C6F;
									}
									if (parsedArg.Values.Count != 0)
									{
										this.ShowUsage = true;
										continue;
									}
									this.sensorsEnabled[1] = false;
									continue;
								}
								else
								{
									if (!(text == "VIDEO"))
									{
										goto IL_C6F;
									}
									if (parsedArg.Values.Count != 1)
									{
										this.ShowUsage = true;
										continue;
									}
									this.OriginalVideoResolution = parsedArg.Values[0];
									continue;
								}
							}
							else if (num != 2539323847U)
							{
								if (num != 2551305963U)
								{
									if (num != 2554608160U)
									{
										goto IL_C6F;
									}
									if (!(text == "NOGPU"))
									{
										goto IL_C6F;
									}
									if (parsedArg.Values.Count != 0)
									{
										this.ShowUsage = true;
										continue;
									}
									this.sensorsEnabled[256] = false;
									continue;
								}
								else
								{
									if (!(text == "BOOTLANGUAGE"))
									{
										goto IL_C6F;
									}
									if (parsedArg.Values.Count != 1)
									{
										this.ShowUsage = true;
										continue;
									}
									this.BootLanguage = parsedArg.Values[0];
									continue;
								}
							}
							else if (!(text == "CREATEDIFFDISK"))
							{
								goto IL_C6F;
							}
						}
						else if (num <= 3409487127U)
						{
							if (num <= 2867730470U)
							{
								if (num != 2612140010U)
								{
									if (num != 2867730470U)
									{
										goto IL_C6F;
									}
									if (!(text == "DBGSVC"))
									{
										goto IL_C6F;
									}
									if (parsedArg.Values.Count != 0)
									{
										this.ShowUsage = true;
										continue;
									}
									this.ServiceDebugEnabled = true;
									continue;
								}
								else if (!(text == "USEDIFFDISK"))
								{
									goto IL_C6F;
								}
							}
							else if (num != 3253716432U)
							{
								if (num != 3409487127U)
								{
									goto IL_C6F;
								}
								if (!(text == "NONFC"))
								{
									goto IL_C6F;
								}
								if (parsedArg.Values.Count != 0)
								{
									this.ShowUsage = true;
									continue;
								}
								this.sensorsEnabled[32] = false;
								continue;
							}
							else
							{
								if (!(text == "DISABLESTATESEP"))
								{
									goto IL_C6F;
								}
								if (parsedArg.Values.Count != 0)
								{
									this.ShowUsage = true;
									continue;
								}
								this.DisableStateSep = true;
								continue;
							}
						}
						else if (num <= 3720960482U)
						{
							if (num != 3533906295U)
							{
								if (num != 3720960482U)
								{
									goto IL_C6F;
								}
								if (!(text == "NOSTART"))
								{
									goto IL_C6F;
								}
								if (parsedArg.Values.Count != 0)
								{
									this.ShowUsage = true;
									continue;
								}
								this.NoStart = true;
								continue;
							}
							else
							{
								if (!(text == "SHOWNAME"))
								{
									goto IL_C6F;
								}
								if (parsedArg.Values.Count != 0)
								{
									this.ShowUsage = true;
									continue;
								}
								this.ShowName = true;
								continue;
							}
						}
						else if (num != 3818140743U)
						{
							if (num != 3882089686U)
							{
								if (num != 4245048658U)
								{
									goto IL_C6F;
								}
								if (!(text == "ADDUSERTOHYPERVADMINS"))
								{
									goto IL_C6F;
								}
								if (parsedArg.Values.Count != 1)
								{
									this.ShowUsage = true;
									continue;
								}
								this.AddUserToHyperVAdmins = parsedArg.Values[0];
								continue;
							}
							else
							{
								if (!(text == "SOFTBUTTONS"))
								{
									goto IL_C6F;
								}
								if (parsedArg.Values.Count != 0)
								{
									this.ShowUsage = true;
									continue;
								}
								this.sensorsEnabled[128] = true;
								continue;
							}
						}
						else
						{
							if (!(text == "WAITFORCONNECTION"))
							{
								goto IL_C6F;
							}
							if (parsedArg.Values.Count != 0)
							{
								this.ShowUsage = true;
								continue;
							}
							this.WaitForClientConnection = true;
							continue;
						}
						if (parsedArg.Values.Count > 1)
						{
							this.ShowUsage = true;
							continue;
						}
						this.ReuseExistingDiffDisk = (text == "USEDIFFDISK");
						if (parsedArg.Values.Count == 1)
						{
							this.DiffDiskVhd = XdeArgsProcessor.EnsurePathIsRooted(parsedArg.Values[0]);
							continue;
						}
						this.calculateDiffDisk = true;
						continue;
					}
					IL_C6F:
					this.ShowUsage = true;
				}
			}
		}

		// Token: 0x06000214 RID: 532 RVA: 0x00009820 File Offset: 0x00007A20
		public void LoadRegistryOverrides()
		{
			string name = "Software\\Microsoft\\XDE\\" + Globals.XdeVersionShort;
			using (RegistryKey registryKey = Registry.LocalMachine.OpenSubKey(name))
			{
				if (registryKey != null)
				{
					object value = registryKey.GetValue("SilentUI");
					if (value != null)
					{
						this.SilentUi = (value.ToString() == "1");
					}
				}
			}
		}

		// Token: 0x06000215 RID: 533 RVA: 0x00009890 File Offset: 0x00007A90
		private static IEnumerable<string> ParseCommandLineToArgs(string commandLine)
		{
			if (string.IsNullOrWhiteSpace(commandLine))
			{
				yield break;
			}
			StringBuilder sb = new StringBuilder();
			bool inQuote = false;
			foreach (char c in commandLine)
			{
				if (c == '"' && !inQuote)
				{
					inQuote = true;
				}
				else if (c != '"' && (!char.IsWhiteSpace(c) || inQuote))
				{
					sb.Append(c);
				}
				else if (sb.Length > 0)
				{
					string text2 = sb.ToString();
					sb.Clear();
					inQuote = false;
					yield return text2;
				}
			}
			string text = null;
			if (sb.Length > 0)
			{
				yield return sb.ToString();
			}
			yield break;
		}

		// Token: 0x06000216 RID: 534 RVA: 0x000098A0 File Offset: 0x00007AA0
		private static string FixPipeName(string pipeName)
		{
			if (!pipeName.StartsWith("\\\\.\\", StringComparison.OrdinalIgnoreCase))
			{
				pipeName = "\\\\.\\pipe\\" + pipeName;
			}
			return pipeName;
		}

		// Token: 0x06000217 RID: 535 RVA: 0x000098C0 File Offset: 0x00007AC0
		private static string EnsurePathIsRooted(string path)
		{
			string text = path;
			if (!Path.IsPathRooted(text))
			{
				text = Path.Combine(Environment.CurrentDirectory, text);
			}
			return text;
		}

		// Token: 0x040000C1 RID: 193
		public const string DefaultVideoResolution = "480x800";

		// Token: 0x040000C2 RID: 194
		public const string DefaultSku = "WP";

		// Token: 0x040000C3 RID: 195
		public const int DefaultMemorySize = 512;

		// Token: 0x040000C4 RID: 196
		private const int DefaultAfterBootResPipeTimeout = 10;

		// Token: 0x040000C5 RID: 197
		private BitVector32 sensorsEnabled;

		// Token: 0x040000C6 RID: 198
		private bool calculateDiffDisk;

		// Token: 0x040000C7 RID: 199
		private string diffDiskVhd;
	}
}
