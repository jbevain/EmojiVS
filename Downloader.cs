using System;
using System.IO;
using System.Net.Http;
using System.Threading.Tasks;

namespace Emoji
{
	class Downloader
	{
		private readonly HttpClient _client = new HttpClient();

		public async Task<string> DownloadEmojiListAsync()
		{
			var request = new HttpRequestMessage
			{
				RequestUri = new Uri("https://api.github.com/emojis"),
				Headers =
				{
					{ "Connection", "Keep-Alive"},
					{ "Accept", "application/vnd.github.v3+json"},
					{ "User-Agent", "EmojiVS" },
				},
			};

			var response = await _client.SendAsync(request);
			return await response.Content.ReadAsStringAsync();
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