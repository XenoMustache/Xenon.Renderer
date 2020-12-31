using Newtonsoft.Json;
using OpenTK.Graphics.OpenGL4;
using System;
using System.IO;
using Xenon.Engine;

namespace Xenon.Renderer {
	public class GameContainer {
		bool isInitialized;

		Script scr;
		GameSettings gs;
		RenderWindow rw;

		readonly GameState cs;

		public GameContainer() {
			DeserializeSettings();
			SerializeSettings();

			cs = DeserializeState($"{gs.stateIndex[0]}");

			CompileScripts();
		}

		public void Initialize(RenderWindow window) {
			if (!isInitialized) {
				rw = window;
				var gw = rw.GetGameWindow();

				gw.Title = gs.name;

				gw.Load += () => Init();
				gw.RenderFrame += (e) => Draw();
				gw.Resize += (e) => Resize();
				gw.UpdateFrame += (e) => Update();

				isInitialized = true;
			}
		}

		void CompileScripts() {
			Console.WriteLine("\nStarting script compilation...");

			if (cs != null) {
				var str = Path.Combine(gs.gameLocation, "Scripts", cs.script);

				scr = new Script(cs.script);
				scr.Compile(str);
			}

			Console.WriteLine("Script compilation successful!");
		}

		void Init() {
			GL.ClearColor(0.2f, 0.3f, 0.3f, 1.0f);

			if (scr != null) scr.Execute("Start");
		}

		void Draw() {
			GL.Clear(ClearBufferMask.ColorBufferBit);

			rw.GetGameWindow().SwapBuffers();
		}

		void Resize() {
			GL.Viewport(0, 0, rw.GetGameWindow().Size.X, rw.GetGameWindow().Size.Y);
		}

		void Update() {
			if (scr != null) scr.Execute("Update");
		}

		GameState DeserializeState(string file) {
			var str = Path.Combine(gs.gameLocation, "States", file);

			if (File.Exists(str)) {
				var state = JsonConvert.DeserializeObject<GameState>(File.ReadAllText(str));

				Console.WriteLine($"\nState \"{file}\" found, loading...");
				return state;
			}
			else {
				Console.WriteLine($"\nUnable to find state \"{file}\"");
				return null;
			}
		}

		void DeserializeSettings() {
			if (File.Exists("GameSettings.json")) {
				gs = JsonConvert.DeserializeObject<GameSettings>(File.ReadAllText("GameSettings.json"));
				Console.WriteLine("Game settings file found, loading...");
			}
			else {
				gs = new GameSettings();
				Console.WriteLine("No game settings file found, generating...");
			}
		}

		void SerializeSettings() {
			if (File.Exists("GameSettings.json")) File.Delete("GameSettings.json");

			var json = JsonConvert.SerializeObject(gs, Formatting.Indented, new JsonSerializerSettings() { ReferenceLoopHandling = ReferenceLoopHandling.Ignore });

			File.WriteAllText("GameSettings.json", json);
			Console.WriteLine($"\n{"GameSettings.json"}:\n{json}");
		}
	}
}
