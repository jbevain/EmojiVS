using System;
using System.IO;
using System.Net.Http;
using System.Threading.Tasks;
using SimpleJson;

namespace Emoji
{
	class Downloader
	{
		private readonly HttpClient _client = new HttpClient();

		public async Task<JsonObject> DownloadEmojiListAsync()
		{
			var request = new HttpRequestMessage
			{
				RequestUri = new Uri("https://api.github.com/emojis"),
				Headers =
				{
					{"Connection", "Keep-Alive"},
					{ "Accept", "application/vnd.github.v3+json"},
					{ "User-Agent", "EmojiVS" },
				},
			};

			var response = await _client.SendAsync(request).ConfigureAwait(false);
			var jsonText = await response.Content.ReadAsStringAsync().ConfigureAwait(false);

			return SimpleJson.SimpleJson.DeserializeObject<JsonObject>(jsonText);
		}

		public async Task DownloadEmojiAsync(Emoji emoji)
		{
			var request = new HttpRequestMessage
			{
				RequestUri = emoji.Uri,
				Headers =
				{
					{ "Connection", "Keep-Alive"},
					{ "User-Agent", "EmojiVS" },
				},
			};

			var response = await _client.SendAsync(request);

			var stream = await response.Content.ReadAsStreamAsync();

			using (var file = File.OpenWrite(emoji.FileName))
				await stream.CopyToAsync(file);
		}
	}
}