using System;
using System.ComponentModel.Composition;
using Microsoft.VisualStudio.Text;
using Microsoft.VisualStudio.Text.Tagging;
using Microsoft.VisualStudio.Utilities;

namespace Emoji.Tagging
{
	[Export(typeof(ITaggerProvider))]
	[ContentType("code")]
	[TagType(typeof(EmojiTag))]
	internal sealed class EmojiTaggerProvider : ITaggerProvider
	{
		public IEmojiStore EmojiStore { get; }

		public IEmojiLocationHandlerProvider EmojiLocationHandlerProvider { get; set; }

		[ImportingConstructor]
		public EmojiTaggerProvider(IEmojiStore emojiStore, IEmojiLocationHandlerProvider emojiLocationHandlerProvider)
		{
			EmojiStore = emojiStore;
			EmojiLocationHandlerProvider = emojiLocationHandlerProvider;
		}

		public ITagger<T> CreateTagger<T>(ITextBuffer buffer) where T : ITag
		{
			if (buffer == null)
				throw new ArgumentNullException(nameof(buffer));

			return buffer.Properties.GetOrCreateSingletonProperty(() => new EmojiTagger(buffer, EmojiStore, EmojiLocationHandlerProvider.CreateLocationHandler(buffer))) as ITagger<T>;
		}
	}
}
