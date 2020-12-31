using Newtonsoft.Json;
using OpenTK.Windowing.Common;
using System;
using System.IO;

namespace Xenon.Renderer {
	public class RenderSettings {
		public bool centerWindow { get; set; }
		public bool hideConsole { get; set; }
		public bool fullscreen { get; set; }

		public VSyncMode vsync { get; set; }

		[JsonProperty]
		public int[] resolution { get; set; }

		public RenderSettings() {
			resolution = new int[2] { 256, 256 };
			centerWindow = true;

			vsync = 0;
		}

		public void Deserialize() {
			if (File.Exists("Config\\RenderSettings.json")) {
				JsonConvert.PopulateObject(File.ReadAllText("Config\\RenderSettings.json"), this);
				Console.WriteLine("Render settings file found, loading...");
			}
			else {
				Console.WriteLine("\nNo render settings file found, generating...");
			}
		}

		public void Serialize() {
			if (File.Exists("Config\\RenderSettings.json")) File.Delete("Config\\RenderSettings.json");

			var json = JsonConvert.SerializeObject(this, Formatting.Indented, new JsonSerializerSettings() { ReferenceLoopHandling = ReferenceLoopHandling.Ignore });

			File.WriteAllText("Config\\RenderSettings.json", json);
			Console.WriteLine($"\n{"RenderSettings.json"}:\n{json}");
		}
	}
}
