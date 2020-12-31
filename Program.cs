namespace Xenon.Renderer {
	static class Program {
		static void Main(string[] args) {
			var settings = new RenderSettings();

			settings.Deserialize();
			settings.Serialize();

			_ = new RenderWindow(settings);
		}
	}
}
