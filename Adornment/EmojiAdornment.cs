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

using System.Windows.Controls;
using System.Windows.Media;
using Emoji.Tagger;

namespace Emoji.Adornment
{
    internal sealed class EmojiAdornment : Image
    {
        internal EmojiAdornment(EmojiTag tag, double fontSize)
        {
	        const double factor = 1.5;

	        ToolTip = tag.Emoji.Name;

	        Width = fontSize * factor;
	        Height = fontSize * factor;

			RenderOptions.SetBitmapScalingMode(this, BitmapScalingMode.HighQuality);
			RenderOptions.SetEdgeMode(this, EdgeMode.Aliased);

			Update(tag);
		}

	    public void Update(EmojiTag tag)
	    {
		    Source = tag.Emoji.Bitmap();
		}
    }
}
