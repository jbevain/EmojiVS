using System.ComponentModel.Composition;
using Microsoft.VisualStudio.Language.Intellisense;
using Microsoft.VisualStudio.Text;
using Microsoft.VisualStudio.Utilities;

namespace Emoji.Intellisense
{
	[Export(typeof(ICompletionSourceProvider))]
	[ContentType("code")]
	[Name("Emoji")]
	class EmojiCompletionSourceProvider : ICompletionSourceProvider
	{
		public IEmojiStore EmojiStore { get; }

		public IEmojiLocationHandlerProvider EmojiLocationHandlerProvider { get; set; }

		[ImportingConstructor]
		public EmojiCompletionSourceProvider(IEmojiStore emojiStore, IEmojiLocationHandlerProvider emojiLocationHandlerProvider)
		{
			EmojiStore = emojiStore;
			EmojiLocationHandlerProvider = emojiLocationHandlerProvider;
		}

		public ICompletionSource TryCreateCompletionSource(ITextBuffer buffer)
		{
			return new EmojiCompletionSource(buffer, EmojiStore, EmojiLocationHandlerProvider.CreateLocationHandler(buffer));
		}
	}
}