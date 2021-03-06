using System;

namespace Spectre.Console
{
    /// <summary>
    /// Contains extension methods for <see cref="IAnsiConsole"/>.
    /// </summary>
    public static partial class AnsiConsoleExtensions
    {
        /// <summary>
        /// Writes an exception to the console.
        /// </summary>
        /// <param name="console">The console.</param>
        /// <param name="exception">The exception to write to the console.</param>
        /// <param name="format">The exception format options.</param>
        public static void WriteException(this IAnsiConsole console, Exception exception, ExceptionFormats format = ExceptionFormats.None)
        {
            Render(console, exception.GetRenderable(format));
        }
    }
}
