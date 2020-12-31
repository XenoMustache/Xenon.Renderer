using OpenTK.Mathematics;
using OpenTK.Windowing.Desktop;
using OpenTK.Windowing.GraphicsLibraryFramework;

namespace Xenon.Renderer {
	public class RenderWindow {
		readonly GameWindow gw;

		public RenderWindow(RenderSettings settings) {
			Utilities.HideConsole(settings.hideConsole);

			var gc = new GameContainer();

			var gws = new GameWindowSettings();
			var nws = new NativeWindowSettings();

			if (settings.fullscreen)
				nws.WindowState = OpenTK.Windowing.Common.WindowState.Fullscreen;

			nws.Size = new Vector2i(settings.resolution[0], settings.resolution[1]);

			gw = new GameWindow(gws, nws);

			gc.Initialize(this);

			if (settings.centerWindow) CenterWindow();

			gw.VSync = settings.vsync;
			gw.Run();
		}

		public GameWindow GetGameWindow() {
			return gw;
		}

		void CenterWindow() {
			unsafe {
				var location = Utilities.GetMonitorResolution(GLFW.GetPrimaryMonitor());

				gw.Location = (location / 2) - gw.Size / 2;
			}
		}
	}
}
