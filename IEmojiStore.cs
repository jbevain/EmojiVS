using System.Collections.Generic;

namespace Emoji
{
	interface IEmojiStore
	{
		bool TryGetEmoji(string name, out Emoji emoji);
		IEnumerable<Emoji> Emojis();
	}
}