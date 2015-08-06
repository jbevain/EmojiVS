using Microsoft.VisualStudio.Text;

namespace Emoji
{
	interface IEmojiLocationHandler
	{
		bool CanHazEmoji(SnapshotSpan span);
	}
}
