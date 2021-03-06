using System.Collections.Generic;
using Spectre.Console.Internal;
using Spectre.Console.Rendering;

namespace Spectre.Console
{
    /// <summary>
    /// Represents padding around a <see cref="IRenderable"/> object.
    /// </summary>
    public sealed class Padder : Renderable, IPaddable, IExpandable
    {
        private readonly IRenderable _child;

        /// <inheritdoc/>
        public Padding Padding { get; set; } = new Padding(1, 1, 1, 1);

        /// <summary>
        /// Gets or sets a value indicating whether or not the padding should
        /// fit the available space. If <c>false</c>, the padding width will be
        /// auto calculated. Defaults to <c>false</c>.
        /// </summary>
        public bool Expand { get; set; }

        /// <summary>
        /// Initializes a new instance of the <see cref="Padder"/> class.
        /// </summary>
        /// <param name="child">The thing to pad.</param>
        /// <param name="padding">The padding. Defaults to <c>1,1,1,1</c> if null.</param>
        public Padder(IRenderable child, Padding? padding = null)
        {
            _child = child;
            Padding = padding ?? Padding;
        }

        /// <inheritdoc/>
        protected override Measurement Measure(RenderContext context, int maxWidth)
        {
            var paddingWidth = Padding.GetWidth();
            var measurement = _child.Measure(context, maxWidth - paddingWidth);

            return new Measurement(
                measurement.Min + paddingWidth,
                measurement.Max + paddingWidth);
        }

        /// <inheritdoc/>
        protected override IEnumerable<Segment> Render(RenderContext context, int maxWidth)
        {
            var paddingWidth = Padding.GetWidth();
            var childWidth = maxWidth - paddingWidth;

            if (!Expand)
            {
                var measurement = _child.Measure(context, maxWidth - paddingWidth);
                childWidth = measurement.Max;
            }

            var width = childWidth + paddingWidth;
            var result = new List<Segment>();

            // Top padding
            for (var i = 0; i < Padding.Top; i++)
            {
                result.Add(new Segment(new string(' ', width)));
                result.Add(Segment.LineBreak);
            }

            var child = _child.Render(context, maxWidth - paddingWidth);
            foreach (var (_, _, _, line) in Segment.SplitLines(child).Enumerate())
            {
                // Left padding
                if (Padding.Left != 0)
                {
                    result.Add(new Segment(new string(' ', Padding.Left)));
                }

                result.AddRange(line);

                // Right padding
                if (Padding.Right != 0)
                {
                    result.Add(new Segment(new string(' ', Padding.Right)));
                }

                // Missing space on right side?
                var lineWidth = line.CellWidth(context);
                var diff = width - lineWidth - Padding.Left - Padding.Right;
                if (diff > 0)
                {
                    result.Add(new Segment(new string(' ', diff)));
                }

                result.Add(Segment.LineBreak);
            }

            // Bottom padding
            for (var i = 0; i < Padding.Bottom; i++)
            {
                result.Add(new Segment(new string(' ', width)));
                result.Add(Segment.LineBreak);
            }

            return result;
        }
    }
}
