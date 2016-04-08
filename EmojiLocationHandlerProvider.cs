using System.ComponentModel.Composition;
using Microsoft.VisualStudio.Text;
using Microsoft.VisualStudio.Text.Tagging;

namespace Emoji
{
	[Export(typeof (IEmojiLocationHandlerProvider))]
	class EmojiLocationHandlerProvider : IEmojiLocationHandlerProvider
	{
		private readonly IBufferTagAggregatorFactoryService _bufferTagAggregatorFactoryService;

		[ImportingConstructor]
		public EmojiLocationHandlerProvider(IBufferTagAggregatorFactoryService bufferTagAggregatorFactoryService)
		{
			_bufferTagAggregatorFactoryService = bufferTagAggregatorFactoryService;
		}

		public IEmojiLocationHandler CreateLocationHandler(ITextBuffer buffer)
		{
			return new EmojiLocationHandler(buffer, _bufferTagAggregatorFactoryService);
		}
	}
}
