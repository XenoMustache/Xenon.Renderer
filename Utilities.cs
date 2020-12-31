using OpenTK.Mathematics;
using OpenTK.Windowing.GraphicsLibraryFramework;
using System;
using System.Runtime.InteropServices;

namespace Xenon.Renderer {
	public static class Utilities {
		[DllImport("kernel32.dll")]
		static extern IntPtr GetConsoleWindow();

		[DllImport("user32.dll")]
		static extern bool ShowWindow(IntPtr hWnd, int nCmdShow);

		const int SW_HIDE = 0;
		const int SW_SHOW = 5;

		public unsafe static Vector2i GetMonitorResolution(Monitor* handle) {
			if (handle == null) throw new ArgumentNullException(nameof(handle));

			var videoMode = GLFW.GetVideoMode(handle);

			return new Vector2i(videoMode->Width, videoMode->Height);
		}

		public static void HideConsole(bool setting = true) {
			if (setting) {
				var handle = GetConsoleWindow();
				ShowWindow(handle, SW_HIDE);
			}
			else {
				var handle = GetConsoleWindow();
				ShowWindow(handle, SW_SHOW);
			}
		}
	}
}
