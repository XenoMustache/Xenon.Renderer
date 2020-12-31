using Newtonsoft.Json;
using OpenTK.Mathematics;
using OpenTK.Windowing.Desktop;
using OpenTK.Windowing.GraphicsLibraryFramework;
using System;
using System.IO;

namespace Xenon.Renderer {
	public class RenderWindow {
		RenderSettings rs;

		readonly GameWindow gw;

		public RenderWindow() {
			var gc = new GameContainer();

			DeserializeSettings();
			SerializeSettings();

			var gws = new GameWindowSettings();
			var nws = new NativeWindowSettings();

			// TODO: Fullscreen and fullscreen switching
			nws.Size = new Vector2i(rs.windowWidth, rs.windowHeight);

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
