using System;
using System.Globalization;
using System.IO;
using System.Security.Cryptography;
using System.Text;
using Hashsum.Internal;
using Hashsum.Models;

namespace Hashsum
{
    /// <summary>
    /// Fluent interface for mutating and calculating checksums.
    /// </summary>
    public class ChecksumBuilder
    {
        private readonly StringBuilder _buffer;

        /// <summary>
        /// Initializes <see cref="ChecksumBuilder"/>.
        /// </summary>
        public ChecksumBuilder()
        {
            _buffer = new StringBuilder();
        }

        #region Mutators

        private ChecksumBuilder AppendToBuffer(string value)
        {
            // Append value to buffer with separator
            _buffer.Append(value);
            _buffer.Append(';');

            return this;
        }

        private ChecksumBuilder AppendToBuffer(IFormattable value, string format = null)
        {
            var str = value.ToString(format, CultureInfo.InvariantCulture);
            return AppendToBuffer(str);
        }

        /// <summary>
        /// Mutates checksum by given value.
        /// </summary>
        public ChecksumBuilder Mutate(string value)
        {
            value.GuardNotNull(nameof(value));

            return AppendToBuffer(value);
        }

        /// <summary>
        /// Mutates checksum by given value.
        /// </summary>
        public ChecksumBuilder Mutate(char value)
        {
            var str = value.ToString();
            return Mutate(str);
        }

        /// <summary>
        /// Mutates checksum by given value.
        /// </summary>
        public ChecksumBuilder Mutate(IFormattable value)
        {
            value.GuardNotNull(nameof(value));

            return AppendToBuffer(value);
        }

        /// <summary>
        /// Mutates checksum by given value.
        /// </summary>
        public ChecksumBuilder Mutate(bool value)
        {
            var str = value ? "TRUE" : "FALSE";
            return Mutate(str);
        }

        /// <summary>
        /// Mutates checksum by given value.
        /// </summary>
        public ChecksumBuilder Mutate(TimeSpan value)
        {
            var ticks = value.Ticks;
            return Mutate(ticks);
        }

        /// <summary>
        /// Mutates checksum by given value.
        /// </summary>
        public ChecksumBuilder Mutate(DateTime value)
        {
            var ticks = value.ToUniversalTime().Ticks;
            return Mutate(ticks);
        }

        /// <summary>
        /// Mutates checksum by given value.
        /// </summary>
        public ChecksumBuilder Mutate(DateTimeOffset value)
        {
            var ticks = value.ToUniversalTime().Ticks;
            return Mutate(ticks);
        }

        /// <summary>
        /// Mutates checksum by given data.
        /// </summary>
        public ChecksumBuilder Mutate(byte[] data)
        {
            data.GuardNotNull(nameof(data));

            var str = Convert.ToBase64String(data);
            return Mutate(str);
        }

        /// <summary>
        /// Mutates checksum by given stream.
        /// </summary>
        public ChecksumBuilder Mutate(Stream stream)
        {
            stream.GuardNotNull(nameof(stream));

            using (var memoryStream = new MemoryStream())
            {
                stream.CopyTo(memoryStream);
                return Mutate(memoryStream.ToArray());
            }
        }

        #endregion

        /// <summary>
        /// Calculates the checksum using given algorithm.
        /// </summary>
        public Checksum Calculate(HashAlgorithm algorithm, bool disposeAlgorithm = true)
        {
            algorithm.GuardNotNull(nameof(algorithm));

            // Flush buffer and convert to bytes
            var bufferData = Encoding.Unicode.GetBytes(_buffer.ToString());
            _buffer.Clear();

            // Calculate checksum
            try
            {
                var checksumData = algorithm.ComputeHash(bufferData);
                return new Checksum(checksumData);
            }
            finally
            {
                if (disposeAlgorithm)
                    algorithm.Dispose();
            }
        }

        /// <summary>
        /// Calculates the checksum using <see cref="SHA256"/> algorithm.
        /// </summary>
        public Checksum Calculate() => Calculate(SHA256.Create());

        /// <inheritdoc />
        public override string ToString() => _buffer.ToString();
    }
}