using System;
using System.ComponentModel.Composition;
using Microsoft.VisualStudio.Text;
using Microsoft.VisualStudio.Text.Tagging;
using Microsoft.VisualStudio.Utilities;

namespace Emoji.Tagging
{
	[Export(typeof(ITaggerProvider))]
	[ContentType("text")]
	[TagType(typeof(EmojiTag))]
	internal sealed class EmojiTaggerProvider : ITaggerProvider
	{
		public IEmojiStore EmojiStore { get; }

		[ImportingConstructor]
		public EmojiTaggerProvider(IEmojiStore emojiStore)
		{
			EmojiStore = emojiStore;
		}

		public ITagger<T> CreateTagger<T>(ITextBuffer buffer) where T : ITag
		{
			if (buffer == null)
				throw new ArgumentNullException(nameof(buffer));

			return buffer.Properties.GetOrCreateSingletonProperty(() => new EmojiTagger(buffer, EmojiStore)) as ITagger<T>;
		}
	}
}
