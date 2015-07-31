using System.Text.RegularExpressions;
using Microsoft.VisualStudio.Text;

namespace Emoji.Tagger
{
	internal sealed class EmojiTagger : RegexTagger<EmojiTag>
	{
		private readonly IEmojiStore _store;

		internal EmojiTagger(ITextBuffer buffer, IEmojiStore store)
			: base(buffer, new[] { new Regex(@":(?<name>[0-9a-zA-Z\-\+_]+):", RegexOptions.Compiled | RegexOptions.CultureInvariant | RegexOptions.IgnoreCase) })
		{
			_store = store;
		}

		protected override EmojiTag TryCreateTagForMatch(Match match)
		{
			var name = match.Groups["name"].Value;
			Emoji emoji;
			if (!_store.TryGetEmoji(name, out emoji))
				return null;

			return new EmojiTag(emoji);
		}
	}
}
