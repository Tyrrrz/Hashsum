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
    public class ChecksumBuilder : IDisposable
    {
        private readonly HashAlgorithm _algorithm;
        private readonly bool _disposeAlgorithm;
        private readonly StringBuilder _buffer;
        private bool _isDisposed;

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

            // Check if disposed
            ThrowIfDisposed();

            return AppendToBuffer(value);
        }

        /// <summary>
        /// Mutates checksum by given value.
        /// </summary>
        public ChecksumBuilder Mutate(char value)
        {
            // Check if disposed
            ThrowIfDisposed();

            var str = value.ToString();
            return Mutate(str);
        }

        /// <summary>
        /// Mutates checksum by given value.
        /// </summary>
        public ChecksumBuilder Mutate(IFormattable value)
        {
            value.GuardNotNull(nameof(value));

            // Check if disposed
            ThrowIfDisposed();

            return AppendToBuffer(value);
        }

        /// <summary>
        /// Mutates checksum by given value.
        /// </summary>
        public ChecksumBuilder Mutate(bool value)
        {
            // Check if disposed
            ThrowIfDisposed();

            var str = value ? "TRUE" : "FALSE";
            return Mutate(str);
        }

        /// <summary>
        /// Mutates checksum by given value.
        /// </summary>
        public ChecksumBuilder Mutate(TimeSpan value)
        {
            // Check if disposed
            ThrowIfDisposed();

            var ticks = value.Ticks;
            return Mutate(ticks);
        }

        /// <summary>
        /// Mutates checksum by given value.
        /// </summary>
        public ChecksumBuilder Mutate(DateTime value)
        {
            // Check if disposed
            ThrowIfDisposed();

            var ticks = value.ToUniversalTime().Ticks;
            return Mutate(ticks);
        }

        /// <summary>
        /// Mutates checksum by given value.
        /// </summary>
        public ChecksumBuilder Mutate(DateTimeOffset value)
        {
            // Check if disposed
            ThrowIfDisposed();

            var ticks = value.ToUniversalTime().Ticks;
            return Mutate(ticks);
        }

        /// <summary>
        /// Mutates checksum by given data.
        /// </summary>
        public ChecksumBuilder Mutate(byte[] data)
        {
            data.GuardNotNull(nameof(data));

            // Check if disposed
            ThrowIfDisposed();

            var str = Convert.ToBase64String(data);

            return Mutate(str);
        }

        /// <summary>
        /// Mutates checksum by given stream.
        /// </summary>
        public ChecksumBuilder Mutate(Stream stream)
        {
            stream.GuardNotNull(nameof(stream));

            // Check if disposed
            ThrowIfDisposed();

            using (var memoryStream = new MemoryStream())
            {
                stream.CopyTo(memoryStream);
                return Mutate(memoryStream.ToArray());
            }
        }

        #endregion

        /// <summary>
        /// Calculates the checksum and clears buffer.
        /// </summary>
        public Checksum Calculate()
        {
            // Check if disposed
            ThrowIfDisposed();

            // Flush buffer and convert to bytes
            var bufferData = Encoding.Unicode.GetBytes(_buffer.ToString());
            _buffer.Clear();

            // Calculate checksum
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
            if (disposing && !_isDisposed)
            {
                _isDisposed = true;
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

        private void ThrowIfDisposed()
        {
            if (_isDisposed)
                throw new ObjectDisposedException(GetType().ToString());
        }
    }
}