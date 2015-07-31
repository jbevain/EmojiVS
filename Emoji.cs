using System;
using System.IO;
using System.Windows.Media.Imaging;

namespace Emoji
{
	class Emoji
	{
		public string Name { get; }
		public string FileName { get; }
		public Uri Uri { get; }

		public bool IsRetrieved => File.Exists(FileName);

		private BitmapImage _bitmap;

		public Emoji(string name, Uri uri, string fileName)
		{
			Name = name;
			Uri = uri;
			FileName = fileName;
		}

		public BitmapImage Bitmap()
		{
			if (_bitmap == null)
			{
				_bitmap = new BitmapImage();
				_bitmap.BeginInit();
				_bitmap.UriSource = new Uri(FileName, UriKind.Absolute);
				_bitmap.EndInit();
			}

			return _bitmap;
		}

		public override string ToString() => Name;
	}
}
