using System.Collections.Generic;
using System.Linq;
using Microsoft.VisualStudio.Text;
using Microsoft.VisualStudio.Text.Tagging;

namespace Emoji
{
	class EmojiLocationHandler : IEmojiLocationHandler
	{
		private static readonly HashSet<string> CommentClassifications = new HashSet<string>(new[]
		{
			"css comment",
			"xml doc comment",
			"vb xml doc comment",
			"vb xml comment",
			"html comment",
			"vbscript comment",
			"comment",
		});

		private readonly ITagAggregator<IClassificationTag> _classificationTagAggregator;

		public EmojiLocationHandler(ITextBuffer buffer, IBufferTagAggregatorFactoryService bufferTagAggregatorFactoryService)
		{
			_classificationTagAggregator = bufferTagAggregatorFactoryService.CreateTagAggregator<IClassificationTag>(buffer);
		}

		public bool CanHazEmoji(SnapshotSpan span)
		{
			return _classificationTagAggregator
				.GetTags(span)
				.Select(t => t.Tag.ClassificationType.Classification.ToLowerInvariant())
				.Any(CommentClassifications.Contains);
		}
	}
}
