using System;
using System.IO;
using System.Management.Automation.Internal;
using System.Management.Automation.Runspaces;
using System.Text;
using System.Xml;

namespace System.Management.Automation.Remoting
{
	// Token: 0x020002B4 RID: 692
	internal class Fragmentor
	{
		// Token: 0x0600217C RID: 8572 RVA: 0x000C0BB5 File Offset: 0x000BEDB5
		internal Fragmentor(int fragmentSize, PSRemotingCryptoHelper cryptoHelper)
		{
			this._fragmentSize = fragmentSize;
			this._serializationContext = new SerializationContext(1, SerializationOptions.RemotingOptions, cryptoHelper);
			this._deserializationContext = new DeserializationContext(DeserializationOptions.RemotingOptions, cryptoHelper);
		}

		// Token: 0x0600217D RID: 8573 RVA: 0x000C0BE4 File Offset: 0x000BEDE4
		internal void Fragment<T>(RemoteDataObject<T> obj, SerializedDataStream dataToBeSent)
		{
			dataToBeSent.Enter();
			try
			{
				obj.Serialize(dataToBeSent, this);
			}
			finally
			{
				dataToBeSent.Exit();
			}
		}

		// Token: 0x170007DE RID: 2014
		// (get) Token: 0x0600217E RID: 8574 RVA: 0x000C0C18 File Offset: 0x000BEE18
		internal DeserializationContext DeserializationContext
		{
			get
			{
				return this._deserializationContext;
			}
		}

		// Token: 0x170007DF RID: 2015
		// (get) Token: 0x0600217F RID: 8575 RVA: 0x000C0C20 File Offset: 0x000BEE20
		// (set) Token: 0x06002180 RID: 8576 RVA: 0x000C0C28 File Offset: 0x000BEE28
		internal int FragmentSize
		{
			get
			{
				return this._fragmentSize;
			}
			set
			{
				this._fragmentSize = value;
			}
		}

		// Token: 0x170007E0 RID: 2016
		// (get) Token: 0x06002181 RID: 8577 RVA: 0x000C0C31 File Offset: 0x000BEE31
		// (set) Token: 0x06002182 RID: 8578 RVA: 0x000C0C39 File Offset: 0x000BEE39
		internal TypeTable TypeTable
		{
			get
			{
				return this._typeTable;
			}
			set
			{
				this._typeTable = value;
			}
		}

		// Token: 0x06002183 RID: 8579 RVA: 0x000C0C44 File Offset: 0x000BEE44
		internal void SerializeToBytes(object obj, Stream streamToWriteTo)
		{
			using (XmlWriter xmlWriter = XmlWriter.Create(streamToWriteTo, new XmlWriterSettings
			{
				CheckCharacters = false,
				Indent = false,
				CloseOutput = false,
				Encoding = Encoding.UTF8,
				NewLineHandling = NewLineHandling.None,
				OmitXmlDeclaration = true,
				ConformanceLevel = ConformanceLevel.Fragment
			}))
			{
				Serializer serializer = new Serializer(xmlWriter, this._serializationContext);
				serializer.TypeTable = this._typeTable;
				serializer.Serialize(obj);
				serializer.Done();
				xmlWriter.Flush();
			}
		}

		// Token: 0x06002184 RID: 8580 RVA: 0x000C0CDC File Offset: 0x000BEEDC
		internal PSObject DeserializeToPSObject(Stream serializedDataStream)
		{
			object obj = null;
			using (XmlReader xmlReader = XmlReader.Create(serializedDataStream, InternalDeserializer.XmlReaderSettingsForCliXml))
			{
				Deserializer deserializer = new Deserializer(xmlReader, this._deserializationContext);
				deserializer.TypeTable = this._typeTable;
				obj = deserializer.Deserialize();
				deserializer.Done();
			}
			if (obj == null)
			{
				throw new PSRemotingDataStructureException(RemotingErrorIdStrings.DeserializedObjectIsNull);
			}
			return PSObject.AsPSObject(obj);
		}

		// Token: 0x04000EDB RID: 3803
		private const int SerializationDepthForRemoting = 1;

		// Token: 0x04000EDC RID: 3804
		private static UTF8Encoding _utf8Encoding = new UTF8Encoding();

		// Token: 0x04000EDD RID: 3805
		private int _fragmentSize;

		// Token: 0x04000EDE RID: 3806
		private SerializationContext _serializationContext;

		// Token: 0x04000EDF RID: 3807
		private DeserializationContext _deserializationContext;

		// Token: 0x04000EE0 RID: 3808
		private TypeTable _typeTable;
	}
}
