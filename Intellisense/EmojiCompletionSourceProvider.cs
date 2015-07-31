using System.ComponentModel.Composition;
using Microsoft.VisualStudio.Language.Intellisense;
using Microsoft.VisualStudio.Text;
using Microsoft.VisualStudio.Utilities;

namespace Emoji.Intellisense
{
	[Export(typeof(ICompletionSourceProvider))]
	[ContentType("text")]
	[Name("Emoji")]
	class EmojiCompletionSourceProvider : ICompletionSourceProvider
	{
		public IEmojiStore EmojiStore { get; }

		[ImportingConstructor]
		public EmojiCompletionSourceProvider(IEmojiStore emojiStore)
		{
			EmojiStore = emojiStore;
		}

		public ICompletionSource TryCreateCompletionSource(ITextBuffer textBuffer)
		{
			return new EmojiCompletionSource(textBuffer, EmojiStore);
		}
	}
}