namespace Basalt.Types
{
	/// <summary>
	/// Represents the initialization parameters for a window.
	/// </summary>
	public class WindowInitParams
	{
		/// <summary>
		/// Gets or sets the title of the window.
		/// </summary>
		public string Title { get; set; } = "New Window";

		/// <summary>
		/// Gets or sets the width of the window.
		/// </summary>
		public int Width { get; set; } = 1920;

		/// <summary>
		/// Gets or sets the height of the window.
		/// </summary>
		public int Height { get; set; } = 1080;

		/// <summary>
		/// Gets or sets the target frames per second (FPS) of the window.
		/// </summary>
		public int TargetFps { get; set; } = 120;

		/// <summary>
		/// Gets or sets a value indicating whether the window should be fullscreen (if supported).
		/// </summary>
		public bool Fullscreen { get; set; } = true;

		/// <summary>
		/// Gets or sets a value indicating whether vertical synchronization (VSync) is enabled for the window (if supported).
		/// </summary>
		public bool VSync { get; set; }

		/// <summary>
		/// Gets or sets a value indicating whether the window should be borderless (if supported).
		/// </summary>
		public bool Borderless { get; set; }

		/// <summary>
		/// Gets or sets a value indicating whether 4x multisample anti-aliasing (MSAA) is enabled for the window (if supported).
		/// </summary>
		public bool MSAA4X { get; set; } = true;

		/// <summary>
		/// Gets or sets a value indicating whether post-processing effects are enabled for the window (if supported).
		/// </summary>
		public bool PostProcessing { get; set; }
	}
}
