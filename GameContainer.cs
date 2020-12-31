using Newtonsoft.Json;
using OpenTK.Graphics.OpenGL4;
using OpenTK.Windowing.Common;
using System;
using System.IO;
using Xenon.Engine;

namespace Xenon.Renderer {
	public class GameContainer {
		bool isInitialized;

		Script scr;
		GameSettings gs;
		RenderWindow rw;
		GameState cs;

		public GameContainer() {
			DeserializeSettings();
			SerializeSettings();

			cs = DeserializeState($"{gs.stateIndex[0]}");

			CompileScripts();
		}

		public void Initialize(RenderWindow window) {
			if (!isInitialized) {
				rw = window;
				rw.gw.Title = gs.name;

				rw.gw.Load += () => Init();
				rw.gw.RenderFrame += (e) => Draw(e);
				rw.gw.Resize += (e) => Resize(e);
				rw.gw.UpdateFrame += (e) => Update(e);
				rw.gw.Unload += () => Exit();

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

		void Draw(FrameEventArgs args) {
			GL.Clear(ClearBufferMask.ColorBufferBit);

			rw.gw.SwapBuffers();
		}

		void Resize(ResizeEventArgs args) {
			GL.Viewport(0, 0, rw.gw.Size.X, rw.gw.Size.Y);
		}

		void Update(FrameEventArgs args) {
			if (scr != null) scr.Execute("Update");
		}

		void Exit() { }

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
