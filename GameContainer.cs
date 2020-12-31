using OpenTK.Graphics.OpenGL4;
using System;
using System.IO;
using Xenon.Engine;

namespace Xenon.Renderer {
	public class GameContainer {
		bool isInitialized;

		readonly GameSettings gs;

		Script scr;
		RenderWindow rw;

		readonly GameState cs;

		public GameContainer() {
			gs = new GameSettings();
			gs.Deserialize();
			gs.Serialize();

			cs = new GameState(gs);
			cs.Deserialize($"{gs.stateIndex[0]}");

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
	}
}
