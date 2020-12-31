using OpenTK.Windowing.Common;

namespace Xenon.Renderer {
	public class RenderSettings {
		public int windowWidth = 256, windowHeight = 256;
		public bool centerWindow = true, hideConsole /*fullscreen*/;
		public VSyncMode vsync = 0;
	}
}
