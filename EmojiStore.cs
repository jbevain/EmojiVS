using System;
using System.Collections.Generic;
using System.ComponentModel.Composition;
using System.IO;
using System.Threading.Tasks;

namespace Emoji
{
	[Export(typeof(IEmojiStore))]
	class EmojiStore : IEmojiStore
	{
		private readonly string _storeDirectory;
		private readonly Downloader _downloader;
		private readonly Dictionary<string, Emoji> _emojis = new Dictionary<string, Emoji>();

		public EmojiStore()
		{
			_storeDirectory = StoreDirectory();
			_downloader = new Downloader();

			if (!Directory.Exists(_storeDirectory))
				Directory.CreateDirectory(_storeDirectory);

			InitializeStore();
		}

		private void InitializeStore()
		{
			Task.Run(async () =>
			{
				foreach (var pair in await _downloader.DownloadEmojiListAsync())
				{
					var name = pair.Key;
					var uri = new Uri((string)pair.Value);

					var emoji = new Emoji(name, uri, EmojiFileName(uri));
					if (!emoji.IsRetrieved)
						DownloadEmoji(emoji);

					_emojis.Add(name, emoji);
				}
			});
		}

		private void DownloadEmoji(Emoji emoji)
		{
			Task.Run(async () => await _downloader.DownloadEmojiAsync(emoji));
		}

		private string EmojiFileName(Uri uri)
		{
			return Path.Combine(_storeDirectory, Path.GetFileName(uri.LocalPath));
		}

		private static string StoreDirectory() => Path.Combine(Path.GetDirectoryName(typeof(EmojiPackage).Assembly.Location), "EmojiStore");

		public bool TryGetEmoji(string name, out Emoji emoji)
		{
			return _emojis.TryGetValue(name, out emoji);
		}

		public IEnumerable<Emoji> Emojis()
		{
			return _emojis.Values;
		}
	}
}
