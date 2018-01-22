using System;
using System.Globalization;
using System.Security.Cryptography;
using System.Text;
using Hashsum.Internal;
using Hashsum.Models;

namespace Hashsum
{
    /// <summary>
    /// Fluent interface for mutating and calculating checksums.
    /// </summary>
    public class ChecksumBuilder : IDisposable
    {
        private readonly HashAlgorithm _algorithm;
        private readonly bool _disposeAlgorithm;
        private readonly StringBuilder _buffer;

        /// <summary>
        /// Initializes <see cref="ChecksumBuilder" /> with given hashing algorithm.
        /// </summary>
        public ChecksumBuilder(HashAlgorithm algorithm, bool disposeAlgorithm = false)
        {
            _algorithm = algorithm.GuardNotNull(nameof(algorithm));
            _disposeAlgorithm = disposeAlgorithm;
            _buffer = new StringBuilder();
        }

        /// <summary>
        /// Initializes <see cref="ChecksumBuilder" /> with <see cref="SHA256" /> hashing algorithm.
        /// </summary>
        public ChecksumBuilder()
            : this(SHA256.Create(), true)
        {
        }

        #region Mutators

        private ChecksumBuilder AppendToBuffer(string value)
        {
            _buffer.Append(value);
            _buffer.Append(';');

            return this;
        }

        private ChecksumBuilder AppendToBuffer(IFormattable value, string format = null) =>
            AppendToBuffer(value?.ToString(format, CultureInfo.InvariantCulture));

        /// <summary>
        /// Mutates checksum by given string.
        /// </summary>
        public ChecksumBuilder Mutate(string str) => AppendToBuffer(str);

        /// <summary>
        /// Mutates checksum by given value.
        /// </summary>
        public ChecksumBuilder Mutate(IFormattable value) => AppendToBuffer(value);

        /// <summary>
        /// Mutates checksum by given date.
        /// </summary>
        public ChecksumBuilder Mutate(DateTimeOffset date) => AppendToBuffer(date.ToUniversalTime(), "u");

        /// <summary>
        /// Mutates checksum by given data.
        /// </summary>
        public ChecksumBuilder Mutate(byte[] data) => AppendToBuffer(Convert.ToBase64String(data));

        #endregion

        /// <summary>
        /// Calculates the checksum and clears buffer.
        /// </summary>
        public Checksum Calculate()
        {
            var bufferData = Encoding.Unicode.GetBytes(_buffer.ToString());
            _buffer.Clear();
            var checksumData = _algorithm.ComputeHash(bufferData);

            return new Checksum(checksumData);
        }

        /// <inheritdoc />
        public override string ToString() => _buffer.ToString();

        /// <summary>
        /// Disposes resources.
        /// </summary>
        protected virtual void Dispose(bool disposing)
        {
            if (disposing)
            {
                _buffer.Clear();
                if (_disposeAlgorithm)
                    _algorithm.Dispose();
            }
        }

        /// <inheritdoc />
        public void Dispose()
        {
            Dispose(true);
            GC.SuppressFinalize(this);
        }
    }
}