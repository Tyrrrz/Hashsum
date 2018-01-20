using System;
using Hashsum.Internal;

namespace Hashsum
{
    /// <summary>
    /// Extensions for <see cref="Hashsum"/>.
    /// </summary>
    public static class Extensions
    {
        /// <summary>
        /// Calculates checksum and converts it to base64 string.
        /// </summary>
        public static string CalculateToString(this ChecksumBuilder builder)
        {
            builder.GuardNotNull(nameof(builder));
            return Convert.ToBase64String(builder.Calculate());
        }
    }
}