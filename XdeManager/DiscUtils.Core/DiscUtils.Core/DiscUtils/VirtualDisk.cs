using System;
using System.Collections.Generic;
using System.Globalization;
using System.IO;
using DiscUtils.Internal;
using DiscUtils.Partitions;
using DiscUtils.Streams;

namespace DiscUtils
{
	// Token: 0x0200002C RID: 44
	public abstract class VirtualDisk : MarshalByRefObject, IDisposable
	{
		// Token: 0x060001B1 RID: 433 RVA: 0x000045B0 File Offset: 0x000027B0
		~VirtualDisk()
		{
			this.Dispose(false);
		}

		// Token: 0x17000069 RID: 105
		// (get) Token: 0x060001B2 RID: 434 RVA: 0x000045E0 File Offset: 0x000027E0
		[Obsolete("Use VirtualDiskManager.SupportedDiskFormats")]
		public static ICollection<string> SupportedDiskFormats
		{
			get
			{
				return VirtualDiskManager.SupportedDiskFormats;
			}
		}

		// Token: 0x1700006A RID: 106
		// (get) Token: 0x060001B3 RID: 435 RVA: 0x000045E7 File Offset: 0x000027E7
		[Obsolete("Use VirtualDiskManager.SupportedDiskTypes")]
		public static ICollection<string> SupportedDiskTypes
		{
			get
			{
				return VirtualDiskManager.SupportedDiskTypes;
			}
		}

		// Token: 0x1700006B RID: 107
		// (get) Token: 0x060001B4 RID: 436
		public abstract Geometry Geometry { get; }

		// Token: 0x1700006C RID: 108
		// (get) Token: 0x060001B5 RID: 437 RVA: 0x000045EE File Offset: 0x000027EE
		public virtual Geometry BiosGeometry
		{
			get
			{
				return Geometry.MakeBiosSafe(this.Geometry, this.Capacity);
			}
		}

		// Token: 0x1700006D RID: 109
		// (get) Token: 0x060001B6 RID: 438
		public abstract VirtualDiskClass DiskClass { get; }

		// Token: 0x1700006E RID: 110
		// (get) Token: 0x060001B7 RID: 439
		public abstract long Capacity { get; }

		// Token: 0x1700006F RID: 111
		// (get) Token: 0x060001B8 RID: 440 RVA: 0x00004601 File Offset: 0x00002801
		public virtual int BlockSize
		{
			get
			{
				return 512;
			}
		}

		// Token: 0x17000070 RID: 112
		// (get) Token: 0x060001B9 RID: 441 RVA: 0x00004608 File Offset: 0x00002808
		public int SectorSize
		{
			get
			{
				return this.BlockSize;
			}
		}

		// Token: 0x17000071 RID: 113
		// (get) Token: 0x060001BA RID: 442
		public abstract SparseStream Content { get; }

		// Token: 0x17000072 RID: 114
		// (get) Token: 0x060001BB RID: 443
		public abstract IEnumerable<VirtualDiskLayer> Layers { get; }

		// Token: 0x17000073 RID: 115
		// (get) Token: 0x060001BC RID: 444 RVA: 0x00004610 File Offset: 0x00002810
		// (set) Token: 0x060001BD RID: 445 RVA: 0x00004624 File Offset: 0x00002824
		public virtual int Signature
		{
			get
			{
				return EndianUtilities.ToInt32LittleEndian(this.GetMasterBootRecord(), 440);
			}
			set
			{
				byte[] masterBootRecord = this.GetMasterBootRecord();
				EndianUtilities.WriteBytesLittleEndian(value, masterBootRecord, 440);
				this.SetMasterBootRecord(masterBootRecord);
			}
		}

		// Token: 0x17000074 RID: 116
		// (get) Token: 0x060001BE RID: 446 RVA: 0x0000464B File Offset: 0x0000284B
		public virtual bool IsPartitioned
		{
			get
			{
				return PartitionTable.IsPartitioned(this.Content);
			}
		}

		// Token: 0x17000075 RID: 117
		// (get) Token: 0x060001BF RID: 447 RVA: 0x00004658 File Offset: 0x00002858
		public virtual PartitionTable Partitions
		{
			get
			{
				IList<PartitionTable> partitionTables = PartitionTable.GetPartitionTables(this);
				if (partitionTables == null || partitionTables.Count == 0)
				{
					return null;
				}
				if (partitionTables.Count == 1)
				{
					return partitionTables[0];
				}
				PartitionTable result = null;
				int num = -1;
				for (int i = 0; i < partitionTables.Count; i++)
				{
					int num2 = 0;
					if (partitionTables[i] is GuidPartitionTable)
					{
						num2 = 2;
					}
					else if (partitionTables[i] is BiosPartitionTable)
					{
						num2 = 1;
					}
					if (num2 > num)
					{
						num = num2;
						result = partitionTables[i];
					}
				}
				return result;
			}
		}

		// Token: 0x17000076 RID: 118
		// (get) Token: 0x060001C0 RID: 448 RVA: 0x000046D7 File Offset: 0x000028D7
		public virtual VirtualDiskParameters Parameters
		{
			get
			{
				return new VirtualDiskParameters
				{
					DiskType = this.DiskClass,
					Capacity = this.Capacity,
					Geometry = this.Geometry,
					BiosGeometry = this.BiosGeometry,
					AdapterType = GenericDiskAdapterType.Ide
				};
			}
		}

		// Token: 0x17000077 RID: 119
		// (get) Token: 0x060001C1 RID: 449
		public abstract VirtualDiskTypeInfo DiskTypeInfo { get; }

		// Token: 0x060001C2 RID: 450 RVA: 0x00004715 File Offset: 0x00002915
		public static ICollection<string> GetSupportedDiskVariants(string type)
		{
			return VirtualDiskManager.TypeMap[type].Variants;
		}

		// Token: 0x060001C3 RID: 451 RVA: 0x00004727 File Offset: 0x00002927
		public static VirtualDiskTypeInfo GetDiskType(string type, string variant)
		{
			return VirtualDiskManager.TypeMap[type].GetDiskTypeInformation(variant);
		}

		// Token: 0x060001C4 RID: 452 RVA: 0x0000473C File Offset: 0x0000293C
		public static VirtualDisk CreateDisk(DiscFileSystem fileSystem, string type, string variant, string path, long capacity, Geometry geometry, Dictionary<string, string> parameters)
		{
			VirtualDiskFactory virtualDiskFactory = VirtualDiskManager.TypeMap[type];
			VirtualDiskParameters virtualDiskParameters = new VirtualDiskParameters
			{
				AdapterType = GenericDiskAdapterType.Scsi,
				Capacity = capacity,
				Geometry = geometry
			};
			if (parameters != null)
			{
				foreach (string key in parameters.Keys)
				{
					virtualDiskParameters.ExtendedParameters[key] = parameters[key];
				}
			}
			return virtualDiskFactory.CreateDisk(new DiscFileLocator(fileSystem, Utilities.GetDirectoryFromPath(path)), variant.ToLowerInvariant(), Utilities.GetFileFromPath(path), virtualDiskParameters);
		}

		// Token: 0x060001C5 RID: 453 RVA: 0x000047EC File Offset: 0x000029EC
		public static VirtualDisk CreateDisk(string type, string variant, string path, long capacity, Geometry geometry, Dictionary<string, string> parameters)
		{
			return VirtualDisk.CreateDisk(type, variant, path, capacity, geometry, null, null, parameters);
		}

		// Token: 0x060001C6 RID: 454 RVA: 0x00004800 File Offset: 0x00002A00
		public static VirtualDisk CreateDisk(string type, string variant, string path, long capacity, Geometry geometry, string user, string password, Dictionary<string, string> parameters)
		{
			VirtualDiskParameters virtualDiskParameters = new VirtualDiskParameters
			{
				AdapterType = GenericDiskAdapterType.Scsi,
				Capacity = capacity,
				Geometry = geometry
			};
			if (parameters != null)
			{
				foreach (string key in parameters.Keys)
				{
					virtualDiskParameters.ExtendedParameters[key] = parameters[key];
				}
			}
			return VirtualDisk.CreateDisk(type, variant, path, virtualDiskParameters, user, password);
		}

		// Token: 0x060001C7 RID: 455 RVA: 0x00004890 File Offset: 0x00002A90
		public static VirtualDisk CreateDisk(string type, string variant, string path, VirtualDiskParameters diskParameters, string user, string password)
		{
			Uri uri = VirtualDisk.PathToUri(path);
			Type type2;
			if (!VirtualDiskManager.DiskTransports.TryGetValue(uri.Scheme.ToUpperInvariant(), out type2))
			{
				throw new FileNotFoundException(string.Format(CultureInfo.InvariantCulture, "Unable to parse path '{0}'", new object[]
				{
					path
				}), path);
			}
			VirtualDiskTransport virtualDiskTransport = (VirtualDiskTransport)Activator.CreateInstance(type2);
			VirtualDisk result;
			try
			{
				virtualDiskTransport.Connect(uri, user, password);
				VirtualDisk virtualDisk;
				if (virtualDiskTransport.IsRawDisk)
				{
					virtualDisk = virtualDiskTransport.OpenDisk(FileAccess.ReadWrite);
				}
				else
				{
					virtualDisk = VirtualDiskManager.TypeMap[type].CreateDisk(virtualDiskTransport.GetFileLocator(), variant.ToLowerInvariant(), Utilities.GetFileFromPath(path), diskParameters);
				}
				if (virtualDisk != null)
				{
					virtualDisk._transport = virtualDiskTransport;
					virtualDiskTransport = null;
				}
				result = virtualDisk;
			}
			finally
			{
				if (virtualDiskTransport != null)
				{
					virtualDiskTransport.Dispose();
				}
			}
			return result;
		}

		// Token: 0x060001C8 RID: 456 RVA: 0x0000495C File Offset: 0x00002B5C
		public static VirtualDisk OpenDisk(string path, FileAccess access)
		{
			return VirtualDisk.OpenDisk(path, null, access, null, null);
		}

		// Token: 0x060001C9 RID: 457 RVA: 0x00004968 File Offset: 0x00002B68
		public static VirtualDisk OpenDisk(string path, FileAccess access, string user, string password)
		{
			return VirtualDisk.OpenDisk(path, null, access, user, password);
		}

		// Token: 0x060001CA RID: 458 RVA: 0x00004974 File Offset: 0x00002B74
		public static VirtualDisk OpenDisk(string path, string forceType, FileAccess access, string user, string password)
		{
			Uri uri = VirtualDisk.PathToUri(path);
			VirtualDisk virtualDisk = null;
			Type type;
			if (!VirtualDiskManager.DiskTransports.TryGetValue(uri.Scheme.ToUpperInvariant(), out type))
			{
				throw new FileNotFoundException(string.Format(CultureInfo.InvariantCulture, "Unable to parse path '{0}'", new object[]
				{
					path
				}), path);
			}
			VirtualDiskTransport virtualDiskTransport = (VirtualDiskTransport)Activator.CreateInstance(type);
			VirtualDisk result;
			try
			{
				virtualDiskTransport.Connect(uri, user, password);
				if (virtualDiskTransport.IsRawDisk)
				{
					virtualDisk = virtualDiskTransport.OpenDisk(access);
				}
				else
				{
					VirtualDiskFactory virtualDiskFactory;
					bool flag;
					if (!string.IsNullOrEmpty(forceType))
					{
						flag = VirtualDiskManager.TypeMap.TryGetValue(forceType, out virtualDiskFactory);
					}
					else
					{
						string text = Path.GetExtension(uri.AbsolutePath).ToUpperInvariant();
						if (text.StartsWith(".", StringComparison.Ordinal))
						{
							text = text.Substring(1);
						}
						flag = VirtualDiskManager.ExtensionMap.TryGetValue(text, out virtualDiskFactory);
					}
					if (flag)
					{
						virtualDisk = virtualDiskFactory.OpenDisk(virtualDiskTransport.GetFileLocator(), virtualDiskTransport.GetFileName(), access);
					}
				}
				if (virtualDisk != null)
				{
					virtualDisk._transport = virtualDiskTransport;
					virtualDiskTransport = null;
				}
				result = virtualDisk;
			}
			finally
			{
				if (virtualDiskTransport != null)
				{
					virtualDiskTransport.Dispose();
				}
			}
			return result;
		}

		// Token: 0x060001CB RID: 459 RVA: 0x00004A88 File Offset: 0x00002C88
		public static VirtualDisk OpenDisk(DiscFileSystem fs, string path, FileAccess access)
		{
			if (fs == null)
			{
				return VirtualDisk.OpenDisk(path, access);
			}
			string text = Path.GetExtension(path).ToUpperInvariant();
			if (text.StartsWith(".", StringComparison.Ordinal))
			{
				text = text.Substring(1);
			}
			VirtualDiskFactory virtualDiskFactory;
			if (VirtualDiskManager.ExtensionMap.TryGetValue(text, out virtualDiskFactory))
			{
				return virtualDiskFactory.OpenDisk(fs, path, access);
			}
			return null;
		}

		// Token: 0x060001CC RID: 460 RVA: 0x00004ADC File Offset: 0x00002CDC
		public void Dispose()
		{
			this.Dispose(true);
			GC.SuppressFinalize(this);
		}

		// Token: 0x060001CD RID: 461 RVA: 0x00004AEC File Offset: 0x00002CEC
		public virtual byte[] GetMasterBootRecord()
		{
			byte[] array = new byte[512];
			long position = this.Content.Position;
			this.Content.Position = 0L;
			StreamUtilities.ReadExact(this.Content, array, 0, 512);
			this.Content.Position = position;
			return array;
		}

		// Token: 0x060001CE RID: 462 RVA: 0x00004B3C File Offset: 0x00002D3C
		public virtual void SetMasterBootRecord(byte[] data)
		{
			if (data == null)
			{
				throw new ArgumentNullException("data");
			}
			if (data.Length != 512)
			{
				throw new ArgumentException("The Master Boot Record must be exactly 512 bytes in length", "data");
			}
			long position = this.Content.Position;
			this.Content.Position = 0L;
			this.Content.Write(data, 0, 512);
			this.Content.Position = position;
		}

		// Token: 0x060001CF RID: 463
		public abstract VirtualDisk CreateDifferencingDisk(DiscFileSystem fileSystem, string path);

		// Token: 0x060001D0 RID: 464
		public abstract VirtualDisk CreateDifferencingDisk(string path);

		// Token: 0x060001D1 RID: 465 RVA: 0x00004BA8 File Offset: 0x00002DA8
		internal static VirtualDiskLayer OpenDiskLayer(FileLocator locator, string path, FileAccess access)
		{
			string text = Path.GetExtension(path).ToUpperInvariant();
			if (text.StartsWith(".", StringComparison.Ordinal))
			{
				text = text.Substring(1);
			}
			VirtualDiskFactory virtualDiskFactory;
			if (VirtualDiskManager.ExtensionMap.TryGetValue(text, out virtualDiskFactory))
			{
				return virtualDiskFactory.OpenDiskLayer(locator, path, access);
			}
			return null;
		}

		// Token: 0x060001D2 RID: 466 RVA: 0x00004BF1 File Offset: 0x00002DF1
		protected virtual void Dispose(bool disposing)
		{
			if (disposing)
			{
				if (this._transport != null)
				{
					this._transport.Dispose();
				}
				this._transport = null;
			}
		}

		// Token: 0x060001D3 RID: 467 RVA: 0x00004C10 File Offset: 0x00002E10
		private static Uri PathToUri(string path)
		{
			if (string.IsNullOrEmpty(path))
			{
				throw new ArgumentException("Path must not be null or empty", "path");
			}
			if (path.Contains("://"))
			{
				return new Uri(path);
			}
			if (!Path.IsPathRooted(path))
			{
				path = Path.GetFullPath(path);
			}
			if (path.Length >= 1 && path[0] == '\\')
			{
				return new UriBuilder("file:" + path.Replace('\\', '/')).Uri;
			}
			if (path.StartsWith("//", StringComparison.OrdinalIgnoreCase))
			{
				return new UriBuilder("file:" + path).Uri;
			}
			if (path.Length >= 2 && path[1] == ':')
			{
				return new UriBuilder("file:///" + path.Replace('\\', '/')).Uri;
			}
			return new Uri(path);
		}

		// Token: 0x04000073 RID: 115
		private VirtualDiskTransport _transport;
	}
}
