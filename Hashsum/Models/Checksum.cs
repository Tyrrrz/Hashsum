using System;
using Hashsum.Internal;

namespace Hashsum.Models
{
    /// <summary>
    /// Represents a calculated checksum.
    /// </summary>
    public class Checksum
    {
        private readonly byte[] _data;

        /// <summary>
        /// Initializes checksum with given data.
        /// Use <see cref="ChecksumBuilder" /> to generate checksums.
        /// </summary>
        public Checksum(byte[] data)
        {
            _data = data.GuardNotNull(nameof(data));
        }

        /// <summary>
        /// Returns checksum as a raw array of bytes.
        /// </summary>
        public byte[] ToByteArray() => _data;

        /// <summary>
        /// Returns checksum formatted as string.
        /// </summary>
        public string ToString(ChecksumStringFormat format)
        {
            if (format == ChecksumStringFormat.Base64)
                return Convert.ToBase64String(_data);
            if (format == ChecksumStringFormat.Hex)
                return BitConverter.ToString(_data).Replace("-", "");

            throw new ArgumentOutOfRangeException(nameof(format));
        }

        /// <summary>
        /// Returns checksum formatted as base64 string.
        /// </summary>
        public override string ToString() => ToString(ChecksumStringFormat.Base64);
    }
}