using System;
using System.Globalization;
using System.Security.Cryptography;
using System.Text;
using Hashsum.Internal;

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
        private readonly string _separator;
        private readonly IFormatProvider _formatProvider;

        /// <summary>
        /// Initializes <see cref="ChecksumBuilder" /> with given hashing algorithm.
        /// </summary>
        public ChecksumBuilder(HashAlgorithm algorithm, bool disposeAlgorithm = true)
        {
            algorithm.GuardNotNull(nameof(algorithm));

            _algorithm = algorithm;
            _disposeAlgorithm = disposeAlgorithm;
            _buffer = new StringBuilder();
            _separator = ";";
            _formatProvider = CultureInfo.InvariantCulture;
        }

        /// <summary>
        /// Initializes <see cref="ChecksumBuilder" /> with <see cref="SHA256" /> hashing algorithm.
        /// </summary>
        public ChecksumBuilder()
            : this(SHA256.Create())
        {
        }

        #region Mutators

        private ChecksumBuilder AppendToBuffer(string value)
        {
            _buffer.Append(value);
            _buffer.Append(_separator);

            return this;
        }

        private ChecksumBuilder AppendToBuffer(IFormattable value, string format) =>
            AppendToBuffer(value?.ToString(format, _formatProvider));

        /// <summary>
        /// Mutates checksum by given value.
        /// </summary>
        public ChecksumBuilder Mutate(string value) => AppendToBuffer(value);

        /// <summary>
        /// Mutates checksum by given value.
        /// </summary>
        public ChecksumBuilder Mutate(short value) => AppendToBuffer(value, "D");

        /// <summary>
        /// Mutates checksum by given value.
        /// </summary>
        public ChecksumBuilder Mutate(int value) => AppendToBuffer(value, "D");

        /// <summary>
        /// Mutates checksum by given value.
        /// </summary>
        public ChecksumBuilder Mutate(long value) => AppendToBuffer(value, "D");

        /// <summary>
        /// Mutates checksum by given value.
        /// </summary>
        public ChecksumBuilder Mutate(ushort value) => AppendToBuffer(value, "D");

        /// <summary>
        /// Mutates checksum by given value.
        /// </summary>
        public ChecksumBuilder Mutate(uint value) => AppendToBuffer(value, "D");

        /// <summary>
        /// Mutates checksum by given value.
        /// </summary>
        public ChecksumBuilder Mutate(ulong value) => AppendToBuffer(value, "D");

        /// <summary>
        /// Mutates checksum by given value.
        /// </summary>
        public ChecksumBuilder Mutate(Guid value) => AppendToBuffer(value, "D");

        /// <summary>
        /// Mutates checksum by given value.
        /// </summary>
        public ChecksumBuilder Mutate(float value) => AppendToBuffer(value, "F");

        /// <summary>
        /// Mutates checksum by given value.
        /// </summary>
        public ChecksumBuilder Mutate(double value) => AppendToBuffer(value, "F");

        /// <summary>
        /// Mutates checksum by given value.
        /// </summary>
        public ChecksumBuilder Mutate(decimal value) => AppendToBuffer(value, "F");

        /// <summary>
        /// Mutates checksum by given value.
        /// </summary>
        public ChecksumBuilder Mutate(DateTime value) => AppendToBuffer(value, "u");

        /// <summary>
        /// Mutates checksum by given value.
        /// </summary>
        public ChecksumBuilder Mutate(DateTimeOffset value) => AppendToBuffer(value, "u");

        /// <summary>
        /// Mutates checksum by given value.
        /// </summary>
        public ChecksumBuilder Mutate(byte[] value) => AppendToBuffer(Convert.ToBase64String(value));

        #endregion

        /// <summary>
        /// Calculates the checksum and clears buffer.
        /// </summary>
        public byte[] Calculate()
        {
            var data = Encoding.Unicode.GetBytes(_buffer.ToString());
            _buffer.Clear();
            return _algorithm.ComputeHash(data);
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