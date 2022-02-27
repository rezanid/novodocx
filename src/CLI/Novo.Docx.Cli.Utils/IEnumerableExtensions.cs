using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Novo.Docx.Cli.Utils;

internal static class IEnumerableExtensions
{
    public static IEnumerable<T> OrEmptyIfNull<T>(this IEnumerable<T> enumerable)
    {
        return enumerable ?? Enumerable.Empty<T>();
    }
}
