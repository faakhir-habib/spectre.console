using System.Collections.Generic;
using Spectre.Console.Internal;
using Spectre.Console.Rendering;

namespace Spectre.Console
{
    /// <summary>
    /// A renderable piece of markup text.
    /// </summary>
    public sealed class Markup : Renderable, IAlignable, IOverflowable
    {
        private readonly Paragraph _paragraph;

        /// <inheritdoc/>
        public Justify? Alignment
        {
            get => _paragraph.Alignment;
            set => _paragraph.Alignment = value;
        }

        /// <inheritdoc/>
        public Overflow? Overflow
        {
            get => _paragraph.Overflow;
            set => _paragraph.Overflow = value;
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="Markup"/> class.
        /// </summary>
        /// <param name="text">The markup text.</param>
        /// <param name="style">The style of the text.</param>
        public Markup(string text, Style? style = null)
        {
            _paragraph = MarkupParser.Parse(text, style);
        }

        /// <inheritdoc/>
        protected override Measurement Measure(RenderContext context, int maxWidth)
        {
            return ((IRenderable)_paragraph).Measure(context, maxWidth);
        }

        /// <inheritdoc/>
        protected override IEnumerable<Segment> Render(RenderContext context, int maxWidth)
        {
            return ((IRenderable)_paragraph).Render(context, maxWidth);
        }
    }
}
