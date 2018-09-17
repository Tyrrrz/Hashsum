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
    public class ChecksumBuilder : IChecksumBuilder
    {
        private readonly StringBuilder _buffer;

        /// <summary>
        /// Initializes an instance of <see cref="ChecksumBuilder"/>.
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

        /// <inheritdoc />
        public IChecksumBuilder Mutate(string value)
        {
            value.GuardNotNull(nameof(value));

            return AppendToBuffer(value);
        }

        /// <inheritdoc />
        public IChecksumBuilder Mutate(char value)
        {
            var str = value.ToString();
            return Mutate(str);
        }

        /// <inheritdoc />
        public IChecksumBuilder Mutate(IFormattable value)
        {
            value.GuardNotNull(nameof(value));

            return AppendToBuffer(value);
        }

        /// <inheritdoc />
        public IChecksumBuilder Mutate(bool value)
        {
            var str = value ? "TRUE" : "FALSE";
            return Mutate(str);
        }

        /// <inheritdoc />
        public IChecksumBuilder Mutate(TimeSpan value)
        {
            var ticks = value.Ticks;
            return Mutate(ticks);
        }

        /// <inheritdoc />
        public IChecksumBuilder Mutate(DateTime value)
        {
            var ticks = value.ToUniversalTime().Ticks;
            return Mutate(ticks);
        }

        /// <inheritdoc />
        public IChecksumBuilder Mutate(DateTimeOffset value)
        {
            var ticks = value.ToUniversalTime().Ticks;
            return Mutate(ticks);
        }

        /// <inheritdoc />
        public IChecksumBuilder Mutate(byte[] data)
        {
            data.GuardNotNull(nameof(data));

            var str = Convert.ToBase64String(data);
            return Mutate(str);
        }

        /// <inheritdoc />
        public IChecksumBuilder Mutate(Stream stream)
        {
            stream.GuardNotNull(nameof(stream));

            using (var memoryStream = new MemoryStream())
            {
                stream.CopyTo(memoryStream);
                return Mutate(memoryStream.ToArray());
            }
        }

        #endregion

        /// <inheritdoc />
        public Checksum Calculate(HashAlgorithm algorithm, bool disposeAlgorithm = true)
        {
            algorithm.GuardNotNull(nameof(algorithm));

            try
            {
                // Flush buffer and convert to bytes
                var bufferData = Encoding.Unicode.GetBytes(_buffer.ToString());
                _buffer.Clear();

                // Calculate checksum
                var checksumData = algorithm.ComputeHash(bufferData);
                return new Checksum(checksumData);
            }
            finally
            {
                if (disposeAlgorithm)
                    algorithm.Dispose();
            }
        }

        /// <inheritdoc />
        public Checksum Calculate() => Calculate(SHA256.Create());

        /// <inheritdoc />
        public override string ToString() => _buffer.ToString();
    }
}