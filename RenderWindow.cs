using Newtonsoft.Json;
using OpenTK.Mathematics;
using OpenTK.Windowing.Desktop;
using OpenTK.Windowing.GraphicsLibraryFramework;
using System;
using System.IO;

namespace Xenon.Renderer {
	public class RenderWindow {
		public GameWindow gw;

		GameWindowSettings gws;
		NativeWindowSettings nws;
		GameContainer gc;
		RenderSettings rs;

		//IGLFWGraphicsContext context;

		public RenderWindow() {
			gc = new GameContainer();

			DeserializeSettings();
			SerializeSettings();

			gws = new GameWindowSettings();
			nws = new NativeWindowSettings();

			//nws.Title = title;

			// TODO: Fullscreen and fullscreen switching
			nws.Size = new Vector2i(rs.windowWidth, rs.windowHeight);

			gw = new GameWindow(gws, nws);

			gc.Initialize(this);

			if (rs.centerWindow) CenterWindow();

			Utilities.HideConsole(rs.hideConsole);

			gw.VSync = rs.vsync;
			gw.Run();
		}

		void CenterWindow() {
			unsafe {
				var location = Utilities.GetMonitorResolution(GLFW.GetPrimaryMonitor());

				gw.Location = (location / 2) - gw.Size / 2;
			}
		}

		void DeserializeSettings() {
			if (File.Exists("RenderSettings.json")) {
				rs = JsonConvert.DeserializeObject<RenderSettings>(File.ReadAllText("RenderSettings.json"));
				Console.WriteLine("\nRender settings file found, loading...");
			}
			else {
				rs = new RenderSettings();

				Console.WriteLine("\nNo render settings file found, generating...");
			}
		}

		void SerializeSettings() {
			if (File.Exists("RenderSettings.json")) File.Delete("RenderSettings.json");

			var json = JsonConvert.SerializeObject(rs, Formatting.Indented, new JsonSerializerSettings() { ReferenceLoopHandling = ReferenceLoopHandling.Ignore });

			File.WriteAllText("RenderSettings.json", json);
			Console.WriteLine($"\n{"RenderSettings.json"}:\n{json}");
		}
	}
}
