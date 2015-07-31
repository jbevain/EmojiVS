//***************************************************************************
// 
//    Copyright (c) Microsoft Corporation. All rights reserved.
//    This code is licensed under the Visual Studio SDK license terms.
//    THIS CODE IS PROVIDED *AS IS* WITHOUT WARRANTY OF
//    ANY KIND, EITHER EXPRESS OR IMPLIED, INCLUDING ANY
//    IMPLIED WARRANTIES OF FITNESS FOR A PARTICULAR
//    PURPOSE, MERCHANTABILITY, OR NON-INFRINGEMENT.
//
//***************************************************************************

using System;
using System.ComponentModel.Composition;
using Microsoft.VisualStudio.Text;
using Microsoft.VisualStudio.Text.Editor;
using Microsoft.VisualStudio.Text.Tagging;
using Microsoft.VisualStudio.Utilities;
using Emoji.Tagger;

namespace Emoji.Adornment
{
	[Export(typeof(IViewTaggerProvider))]
	[ContentType("text")]
	[ContentType("projection")]
	[TagType(typeof(IntraTextAdornmentTag))]
	internal sealed class EmojiAdornmentTaggerProvider : IViewTaggerProvider
	{
		public IBufferTagAggregatorFactoryService BufferTagAggregatorFactoryService { get; }

		[ImportingConstructor]
		public EmojiAdornmentTaggerProvider(IBufferTagAggregatorFactoryService bufferTagAggregatorFactoryService)
		{
			BufferTagAggregatorFactoryService = bufferTagAggregatorFactoryService;
		}

		public ITagger<T> CreateTagger<T>(ITextView textView, ITextBuffer buffer) where T : ITag
		{
			if (textView == null)
				throw new ArgumentNullException(nameof(textView));

			if (buffer == null)
				throw new ArgumentNullException(nameof(buffer));

			if (buffer != textView.TextBuffer)
				return null;

			return EmojiAdornmentTagger.GetTagger(
				(IWpfTextView)textView,
				new Lazy<ITagAggregator<EmojiTag>>(
					() => BufferTagAggregatorFactoryService.CreateTagAggregator<EmojiTag>(textView.TextBuffer)))
				as ITagger<T>;
		}
	}
}
