using Microsoft.Extensions.DependencyInjection;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.Common.Extensions
{
    public static class PhoneNormalization
    {
        public static string NormalizeForTopic(this string phoneE164)
        {
            var normalized = phoneE164.Replace("+", "plus-");
            return new string(normalized.Where(c => char.IsLetterOrDigit(c) || c == '-' || c == '_').ToArray()).ToLowerInvariant();
        }
    }
}
