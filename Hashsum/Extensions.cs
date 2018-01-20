using System;
using Hashsum.Internal;

namespace Hashsum
{
    public static class Extensions
    {
        public static string CalculateToString(this ChecksumBuilder builder)
        {
            builder.GuardNotNull(nameof(builder));
            return Convert.ToBase64String(builder.Calculate());
        }
    }
}