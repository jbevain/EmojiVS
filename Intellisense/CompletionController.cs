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

using System.Collections.Generic;
using System.ComponentModel.Composition;
using System.Diagnostics;
using Microsoft.VisualStudio.Editor;
using Microsoft.VisualStudio.Language.Intellisense;
using Microsoft.VisualStudio.OLE.Interop;
using Microsoft.VisualStudio.Text.Editor;
using Microsoft.VisualStudio.TextManager.Interop;
using Microsoft.VisualStudio.Utilities;

namespace Emoji.Intellisense
{
	[Export(typeof(IVsTextViewCreationListener))]
	[ContentType("code")]
	[TextViewRole(PredefinedTextViewRoles.Interactive)]
	internal sealed class VsTextViewCreationListener : IVsTextViewCreationListener
	{
		public IVsEditorAdaptersFactoryService AdaptersFactoryService { get; }

		public ICompletionBroker CompletionBroker { get; }

		public IEmojiLocationHandlerProvider EmojiLocationHandlerProvider { get; }

		[ImportingConstructor]
		public VsTextViewCreationListener(IVsEditorAdaptersFactoryService adaptersFactoryService, ICompletionBroker completionBroker, IEmojiLocationHandlerProvider emojiLocationHandlerProvider)
		{
			AdaptersFactoryService = adaptersFactoryService;
			CompletionBroker = completionBroker;
			EmojiLocationHandlerProvider = emojiLocationHandlerProvider;
		}

		public void VsTextViewCreated(IVsTextView textViewAdapter)
		{
			IWpfTextView view = AdaptersFactoryService.GetWpfTextView(textViewAdapter);
			Debug.Assert(view != null);

			CommandFilter filter = new CommandFilter(view, CompletionBroker, EmojiLocationHandlerProvider.CreateLocationHandler(view.TextBuffer));

			IOleCommandTarget next;
			textViewAdapter.AddCommandFilter(filter, out next);
			filter.Next = next;
		}
	}
}