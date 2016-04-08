using Microsoft.VisualStudio.Text;

namespace Emoji
{
	interface IEmojiLocationHandlerProvider
	{
		IEmojiLocationHandler CreateLocationHandler(ITextBuffer buffer);
	}
}
