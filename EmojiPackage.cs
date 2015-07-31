using System;
using System.Runtime.InteropServices;
using Microsoft.VisualStudio.ComponentModelHost;
using Microsoft.VisualStudio.Shell;
using Microsoft.VisualStudio.Shell.Interop;

namespace Emoji
{
	[PackageRegistration(UseManagedResourcesOnly = true)]
	[InstalledProductRegistration("#110", "#112", "1.0", IconResourceID = 400)] // Info on this package for Help/About
	[Guid(PackageGuidString)]
	[ProvideAutoLoad(UIContextGuids80.NoSolution)]
	public sealed class EmojiPackage : Package
	{
		public const string PackageGuidString = "8de7509c-f5cd-4d6e-bca4-b3828e918eb1";

		public IComponentModel ComponentModel => (IComponentModel)((IServiceProvider)this).GetService(typeof(SComponentModel));

		internal IEmojiStore Store { get; private set; }

		protected override void Initialize()
		{
			base.Initialize();

			Store = ComponentModel.GetService<IEmojiStore>();
		}
	}
}
