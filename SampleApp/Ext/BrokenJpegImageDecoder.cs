using Nostra13UniversalImageLoader.Core.Decode;
using System.IO;

namespace Nostra13UniversalImageLoader.SampleApp.Ext
{
    /**
     * @author Sergey Tarasevich (nostra13[at]gmail[dot]com)
     */
    public class BrokenJpegImageDecoder : BaseImageDecoder
    {
	    public BrokenJpegImageDecoder(bool loggingEnabled)
		    : base(loggingEnabled)
        {
	    }

        /// <exception cref="IOException">This method might throw this exception.</exception>
	    protected override Stream GetImageStream(ImageDecodingInfo decodingInfo)
        {
		    Stream stream = decodingInfo.Downloader
				    .GetStream(decodingInfo.ImageUri, decodingInfo.ExtraForDownloader);
		    return stream ?? new JpegClosedInputStream(stream);
	    }

	    private class JpegClosedInputStream : Stream
        {
		    private const int JPEG_EOI_1 = 0xFF;
		    private const int JPEG_EOI_2 = 0xD9;

		    private readonly Stream inputStream;
		    private int bytesPastEnd;

            public JpegClosedInputStream(Stream inputStream)
            {
                this.inputStream = inputStream;
                bytesPastEnd = 0;
            }

            public override bool CanRead
            {
                get { return inputStream.CanRead; }
            }

            public override bool CanSeek
            {
                get { return inputStream.CanSeek; }
            }

            public override bool CanWrite
            {
                get { return inputStream.CanWrite; }
            }

            public override long Length
            {
                get { return inputStream.Length; }
            }

            public override long Position
            {
                get { return inputStream.Position; }
                set { inputStream.Position = value; }
            }

            /// <exception cref="IOException">This method might throw this exception.</exception>
	        public override int ReadByte()
            {
			    int buffer = inputStream.ReadByte();
			    if (buffer == -1)
                {
				    if (bytesPastEnd > 0)
                    {
					    buffer = JPEG_EOI_2;
				    }
                    else
                    {
					    ++bytesPastEnd;
					    buffer = JPEG_EOI_1;
				    }
			    }

			    return buffer;
		    }

            public override void Flush()
            {
                inputStream.Flush();
            }

            public override long Seek(long offset, SeekOrigin origin)
            {
                return inputStream.Seek(offset, origin);
            }

            public override void SetLength(long value)
            {
                inputStream.SetLength(value);
            }

            public override int Read(byte[] buffer, int offset, int count)
            {
                return inputStream.Read(buffer, offset, count);
            }

            public override void Write(byte[] buffer, int offset, int count)
            {
                inputStream.Write(buffer, offset, count);
            }
        }
    }
}