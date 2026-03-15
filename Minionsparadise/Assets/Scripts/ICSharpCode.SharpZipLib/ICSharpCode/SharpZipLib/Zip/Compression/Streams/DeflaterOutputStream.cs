namespace ICSharpCode.SharpZipLib.Zip.Compression.Streams
{
	public class DeflaterOutputStream : global::System.IO.Stream
	{
		private string password;

		private global::System.Security.Cryptography.ICryptoTransform cryptoTransform_;

		protected byte[] AESAuthCode;

		private byte[] buffer_;

		protected global::ICSharpCode.SharpZipLib.Zip.Compression.Deflater deflater_;

		protected global::System.IO.Stream baseOutputStream_;

		private bool isClosed_;

		private bool isStreamOwner_ = true;

		private static global::System.Security.Cryptography.RNGCryptoServiceProvider _aesRnd;

		public bool IsStreamOwner
		{
			get
			{
				return isStreamOwner_;
			}
			set
			{
				isStreamOwner_ = value;
			}
		}

		public bool CanPatchEntries
		{
			get
			{
				return baseOutputStream_.CanSeek;
			}
		}

		public string Password
		{
			get
			{
				return password;
			}
			set
			{
				if (value != null && value.Length == 0)
				{
					password = null;
				}
				else
				{
					password = value;
				}
			}
		}

		public override bool CanRead
		{
			get
			{
				return false;
			}
		}

		public override bool CanSeek
		{
			get
			{
				return false;
			}
		}

		public override bool CanWrite
		{
			get
			{
				return baseOutputStream_.CanWrite;
			}
		}

		public override long Length
		{
			get
			{
				return baseOutputStream_.Length;
			}
		}

		public override long Position
		{
			get
			{
				return baseOutputStream_.Position;
			}
			set
			{
				throw new global::System.NotSupportedException("Position property not supported");
			}
		}

		public DeflaterOutputStream(global::System.IO.Stream baseOutputStream)
			: this(baseOutputStream, new global::ICSharpCode.SharpZipLib.Zip.Compression.Deflater(), 512)
		{
		}

		public DeflaterOutputStream(global::System.IO.Stream baseOutputStream, global::ICSharpCode.SharpZipLib.Zip.Compression.Deflater deflater)
			: this(baseOutputStream, deflater, 512)
		{
		}

		public DeflaterOutputStream(global::System.IO.Stream baseOutputStream, global::ICSharpCode.SharpZipLib.Zip.Compression.Deflater deflater, int bufferSize)
		{
			if (baseOutputStream == null)
			{
				throw new global::System.ArgumentNullException("baseOutputStream");
			}
			if (!baseOutputStream.CanWrite)
			{
				throw new global::System.ArgumentException("Must support writing", "baseOutputStream");
			}
			if (deflater == null)
			{
				throw new global::System.ArgumentNullException("deflater");
			}
			if (bufferSize < 512)
			{
				throw new global::System.ArgumentOutOfRangeException("bufferSize");
			}
			baseOutputStream_ = baseOutputStream;
			buffer_ = new byte[bufferSize];
			deflater_ = deflater;
		}

		public virtual void Finish()
		{
			deflater_.Finish();
			while (!deflater_.IsFinished)
			{
				int num = deflater_.Deflate(buffer_, 0, buffer_.Length);
				if (num <= 0)
				{
					break;
				}
				if (cryptoTransform_ != null)
				{
					EncryptBlock(buffer_, 0, num);
				}
				baseOutputStream_.Write(buffer_, 0, num);
			}
			if (!deflater_.IsFinished)
			{
				throw new global::ICSharpCode.SharpZipLib.SharpZipBaseException("Can't deflate all input?");
			}
			baseOutputStream_.Flush();
			if (cryptoTransform_ != null)
			{
				if (cryptoTransform_ is global::ICSharpCode.SharpZipLib.Encryption.ZipAESTransform)
				{
					AESAuthCode = ((global::ICSharpCode.SharpZipLib.Encryption.ZipAESTransform)cryptoTransform_).GetAuthCode();
				}
				cryptoTransform_.Dispose();
				cryptoTransform_ = null;
			}
		}

		protected void EncryptBlock(byte[] buffer, int offset, int length)
		{
			cryptoTransform_.TransformBlock(buffer, 0, length, buffer, 0);
		}

		protected void InitializePassword(string password)
		{
			global::ICSharpCode.SharpZipLib.Encryption.PkzipClassicManaged pkzipClassicManaged = new global::ICSharpCode.SharpZipLib.Encryption.PkzipClassicManaged();
			byte[] rgbKey = global::ICSharpCode.SharpZipLib.Encryption.PkzipClassic.GenerateKeys(global::ICSharpCode.SharpZipLib.Zip.ZipConstants.ConvertToArray(password));
			cryptoTransform_ = pkzipClassicManaged.CreateEncryptor(rgbKey, null);
		}

		protected void InitializeAESPassword(global::ICSharpCode.SharpZipLib.Zip.ZipEntry entry, string rawPassword, out byte[] salt, out byte[] pwdVerifier)
		{
			salt = new byte[entry.AESSaltLen];
			if (_aesRnd == null)
			{
				_aesRnd = new global::System.Security.Cryptography.RNGCryptoServiceProvider();
			}
			_aesRnd.GetBytes(salt);
			int blockSize = entry.AESKeySize / 8;
			cryptoTransform_ = new global::ICSharpCode.SharpZipLib.Encryption.ZipAESTransform(rawPassword, salt, blockSize, true);
			pwdVerifier = ((global::ICSharpCode.SharpZipLib.Encryption.ZipAESTransform)cryptoTransform_).PwdVerifier;
		}

		protected void Deflate()
		{
			while (!deflater_.IsNeedingInput)
			{
				int num = deflater_.Deflate(buffer_, 0, buffer_.Length);
				if (num <= 0)
				{
					break;
				}
				if (cryptoTransform_ != null)
				{
					EncryptBlock(buffer_, 0, num);
				}
				baseOutputStream_.Write(buffer_, 0, num);
			}
			if (!deflater_.IsNeedingInput)
			{
				throw new global::ICSharpCode.SharpZipLib.SharpZipBaseException("DeflaterOutputStream can't deflate all input?");
			}
		}

		public override long Seek(long offset, global::System.IO.SeekOrigin origin)
		{
			throw new global::System.NotSupportedException("DeflaterOutputStream Seek not supported");
		}

		public override void SetLength(long value)
		{
			throw new global::System.NotSupportedException("DeflaterOutputStream SetLength not supported");
		}

		public override int ReadByte()
		{
			throw new global::System.NotSupportedException("DeflaterOutputStream ReadByte not supported");
		}

		public override int Read(byte[] buffer, int offset, int count)
		{
			throw new global::System.NotSupportedException("DeflaterOutputStream Read not supported");
		}

		public override global::System.IAsyncResult BeginRead(byte[] buffer, int offset, int count, global::System.AsyncCallback callback, object state)
		{
			throw new global::System.NotSupportedException("DeflaterOutputStream BeginRead not currently supported");
		}

		public override global::System.IAsyncResult BeginWrite(byte[] buffer, int offset, int count, global::System.AsyncCallback callback, object state)
		{
			throw new global::System.NotSupportedException("BeginWrite is not supported");
		}

		public override void Flush()
		{
			deflater_.Flush();
			Deflate();
			baseOutputStream_.Flush();
		}

		public override void Close()
		{
			if (isClosed_)
			{
				return;
			}
			isClosed_ = true;
			try
			{
				Finish();
				if (cryptoTransform_ != null)
				{
					GetAuthCodeIfAES();
					cryptoTransform_.Dispose();
					cryptoTransform_ = null;
				}
			}
			finally
			{
				if (isStreamOwner_)
				{
					baseOutputStream_.Close();
				}
			}
		}

		private void GetAuthCodeIfAES()
		{
			if (cryptoTransform_ is global::ICSharpCode.SharpZipLib.Encryption.ZipAESTransform)
			{
				AESAuthCode = ((global::ICSharpCode.SharpZipLib.Encryption.ZipAESTransform)cryptoTransform_).GetAuthCode();
			}
		}

		public override void WriteByte(byte value)
		{
			Write(new byte[1] { value }, 0, 1);
		}

		public override void Write(byte[] buffer, int offset, int count)
		{
			deflater_.SetInput(buffer, offset, count);
			Deflate();
		}
	}
}
