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

#define HIDING_TEXT

using System;
using System.Collections.Generic;
using System.Windows;
using Emoji.Tagging;
using Microsoft.VisualStudio.Text;
using Microsoft.VisualStudio.Text.Editor;
using Microsoft.VisualStudio.Text.Editor.OptionsExtensionMethods;
using Microsoft.VisualStudio.Text.Tagging;

namespace Emoji.Adornment
{
	internal sealed class EmojiAdornmentTagger

#if HIDING_TEXT
		: IntraTextAdornmentTagTransformer<EmojiTag, EmojiAdornment>
#else
        : IntraTextAdornmentTagger<EmojiTag, EmojiAdornment>
#endif

	{
		internal static ITagger<IntraTextAdornmentTag> GetTagger(IWpfTextView view, Lazy<ITagAggregator<EmojiTag>> emojiTagger)
		{
			return view.Properties.GetOrCreateSingletonProperty<EmojiAdornmentTagger>(
				() => new EmojiAdornmentTagger(view, emojiTagger.Value));
		}

#if HIDING_TEXT
		private EmojiAdornmentTagger(IWpfTextView view, ITagAggregator<EmojiTag> emojiTagger)
			: base(view, emojiTagger)
		{
		}

		public override void Dispose()
		{
			base.view.Properties.RemoveProperty(typeof(EmojiAdornmentTagger));
		}
#else
        private ITagAggregator<EmojiTag> _emojiTagger;

        private EmojiAdornmentTagger(IWpfTextView view, ITagAggregator<EmojiTag> colorTagger)
            : base(view)
        {
            this._emojiTagger = colorTagger;
        }

        public void Dispose()
        {
			_emojiTagger.Dispose();

            view.Properties.RemoveProperty(typeof(EmojiAdornmentTagger));
        }

        // To produce adornments that don't obscure the text, the adornment tags
        // should have zero length spans. Overriding this method allows control
        // over the tag spans.
        protected override IEnumerable<Tuple<SnapshotSpan, PositionAffinity?, EmojiTag>> GetAdornmentData(NormalizedSnapshotSpanCollection spans)
        {
            if (spans.Count == 0)
                yield break;

            ITextSnapshot snapshot = spans[0].Snapshot;

            var colorTags = _emojiTagger.GetTags(spans);

            foreach (IMappingTagSpan<EmojiTag> dataTagSpan in colorTags)
            {
                NormalizedSnapshotSpanCollection colorTagSpans = dataTagSpan.Span.GetSpans(snapshot);

                // Ignore data tags that are split by projection.
                // This is theoretically possible but unlikely in current scenarios.
                if (colorTagSpans.Count != 1)
                    continue;

                SnapshotSpan adornmentSpan = new SnapshotSpan(colorTagSpans[0].Start, 0);

                yield return Tuple.Create(adornmentSpan, (PositionAffinity?)PositionAffinity.Successor, dataTagSpan.Tag);
            }
        }
#endif

		protected override EmojiAdornment CreateAdornment(EmojiTag dataTag, SnapshotSpan span)
		{
			return new EmojiAdornment(dataTag, ((System.Windows.Controls.Control)view).FontSize);
		}

		protected override bool UpdateAdornment(EmojiAdornment adornment, EmojiTag dataTag)
		{
			adornment.Update(dataTag);
			return true;
		}
	}
}
