using OpenTK.Graphics.OpenGL4;
using System;
using System.Collections.Generic;
using System.IO;
using Xenon.Engine;

namespace Xenon.Renderer {
	public class GameContainer {
		bool isInitialized;

		readonly GameSettings gs;
		readonly List<GameObject> objects;
		readonly List<Script> scripts;
		RenderWindow rw;

		public GameContainer() {
			gs = new GameSettings();
			gs.Deserialize();
			gs.Serialize();

			// TODO: State changing
			var cs = new GameState(gs);
			cs.Deserialize($"{gs.stateIndex[0]}");
			cs.Serialize($"{gs.stateIndex[0]}");

			objects = new List<GameObject>();
			scripts = new List<Script>();

			for (var i = 0; i < cs.objects.Count; i++) {
				objects.Add(cs.objects[i].Instantiate(gs));
			}

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

			for (var i = 0; i < objects.Count; i++) {
				for (var j = 0; j < objects[i].scripts.Count; j++) {
					var script = objects[i].scripts[j];
					var str = "";

					if (objects[i] != null && script != null) {
						if (gs.gameLocation != null)
							str = Path.Combine(gs.gameLocation, "Scripts", script);
						else
							str = Path.Combine("Scripts", script);

						var scr = new Script(script);
						scr.Compile(str);

						scripts.Add(scr);
					}
				}
			}

			Console.WriteLine("Script compilation successful!");
		}

		void Init() {
			GL.ClearColor(0.2f, 0.3f, 0.3f, 1.0f);

			for (var i = 0; i < scripts.Count; i++) {
				if (scripts[i] != null) scripts[i].Execute("Start");
			}
		}

		void Draw() {
			GL.Clear(ClearBufferMask.ColorBufferBit);

			rw.GetGameWindow().SwapBuffers();
		}

		void Resize() {
			GL.Viewport(0, 0, rw.GetGameWindow().Size.X, rw.GetGameWindow().Size.Y);
		}

		void Update() {
			for (var i = 0; i < scripts.Count; i++) {
				if (scripts[i] != null) scripts[i].Execute("Update");
			}
		}
	}
}
