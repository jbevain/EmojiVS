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
using System.Collections.Generic;
using System.Linq;
using Microsoft.VisualStudio.Language.Intellisense;
using Microsoft.VisualStudio.Text;

namespace Emoji.Intellisense
{
	class EmojiCompletionSource : ICompletionSource
	{
		private readonly ITextBuffer _buffer;
		private readonly IEmojiLocationHandler _locationHandler;
		private readonly List<Completion> _emojiCompletions;
		private bool _disposed = false;

		public EmojiCompletionSource(ITextBuffer buffer, IEmojiStore emojiStore, IEmojiLocationHandler locationHandler)
		{
			_buffer = buffer;
			_locationHandler = locationHandler;

			_emojiCompletions = emojiStore.Emojis()
				.Select(EmojiCompletion)
				.ToList();
		}

		private static Completion EmojiCompletion(Emoji emoji)
		{
			var name = $":{emoji.Name}:";
			return new Completion(name, name, name, emoji.Bitmap(), name);
		}

		public void AugmentCompletionSession(ICompletionSession session, IList<CompletionSet> completionSets)
		{
			if (_disposed)
				throw new ObjectDisposedException("");

			ITextSnapshot snapshot = _buffer.CurrentSnapshot;
			var triggerPoint = (SnapshotPoint)session.GetTriggerPoint(snapshot);

			if (triggerPoint == null)
				return;

			var line = triggerPoint.GetContainingLine();
			SnapshotPoint start = triggerPoint;

			var emojiDetected = false;

			while (start > line.Start)
			{
				var chr = start.GetChar();

				if (chr == ':')
				{
					emojiDetected = true;
					break;
				}

				var previous = (start - 1).GetChar();

				if (!IsEmojiChar(previous))
					break;

				start -= 1;
			}

			if (!emojiDetected)
				return;

			var span = new SnapshotSpan(start, triggerPoint);

			if (!_locationHandler.CanHazEmoji(span))
				return;

			var applicableTo = snapshot.CreateTrackingSpan(span, SpanTrackingMode.EdgeInclusive);

			completionSets.Add(new CompletionSet("All", "All", applicableTo, _emojiCompletions, Enumerable.Empty<Completion>()));
		}

		private bool IsEmojiChar(char chr)
		{
			if (chr >= 'A' && chr <= 'Z')
				return true;

			if (chr >= 'a' && chr <= 'z')
				return true;

			if (chr >= '0' && chr <= '9')
				return true;

			if (chr == '_' || chr == '+' || chr == '-' || chr == ':')
				return true;

			return false;
		}

		public void Dispose()
		{
			_disposed = true;
		}
	}
}
