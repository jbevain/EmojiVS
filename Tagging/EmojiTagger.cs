using System.Text.RegularExpressions;
using Microsoft.VisualStudio.Text;
using Microsoft.VisualStudio.Text.Tagging;

namespace Emoji.Tagging
{
	internal sealed class EmojiTagger : RegexTagger<EmojiTag>
	{
		private readonly IEmojiStore _store;
		private readonly IEmojiLocationHandler _locationHandler;

		internal EmojiTagger(ITextBuffer buffer, IEmojiStore store, IEmojiLocationHandler locationHandler)
			: base(buffer, new Regex(@":(?<name>[0-9a-zA-Z\-\+_]+):", RegexOptions.Compiled | RegexOptions.CultureInvariant | RegexOptions.IgnoreCase))
		{
			_store = store;
			_locationHandler = locationHandler;
		}

		protected override EmojiTag TryCreateTagForMatch(Match match)
		{
			var name = match.Groups["name"].Value;

			Emoji emoji;
			return _store.TryGetEmoji(name, out emoji)
				? new EmojiTag(emoji)
				: null;
		}

		protected override ITagSpan<EmojiTag> TryCreateTagSpan(SnapshotSpan span, EmojiTag tag)
		{
			return _locationHandler.CanHazEmoji(span)
				? new TagSpan<EmojiTag>(span, tag)
				: null;
		}
	}
}
