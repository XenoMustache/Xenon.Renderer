using OpenTK.Mathematics;
using OpenTK.Windowing.Desktop;
using OpenTK.Windowing.GraphicsLibraryFramework;

namespace Xenon.Renderer {
	public class RenderWindow {
		readonly GameWindow gw;

		public RenderWindow() {
			var gc = new GameContainer();
			var rs = new RenderSettings();

			rs.Deserialize();
			rs.Serialize();

			var gws = new GameWindowSettings();
			var nws = new NativeWindowSettings();

			// TODO: Fullscreen and fullscreen switching
			nws.Size = new Vector2i(rs.resolution[0], rs.resolution[1]);

			gw = new GameWindow(gws, nws);

			gc.Initialize(this);

			if (rs.centerWindow) CenterWindow();

			Utilities.HideConsole(rs.hideConsole);

			gw.VSync = rs.vsync;
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
