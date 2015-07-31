using Microsoft.VisualStudio.Text.Tagging;

namespace Emoji.Tagger
{
	class EmojiTag : ITag
	{
		public Emoji Emoji { get; }

		public EmojiTag(Emoji emoji)
		{
			Emoji = emoji;
		}
	}
}
