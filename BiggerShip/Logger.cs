using MrovLib;

namespace BiggerShip
{
	public class Logger : MrovLib.Logger
	{
		public Logger(string SourceName, LoggingType defaultLoggingType = LoggingType.Debug)
			: base(SourceName, defaultLoggingType)
		{
			ModName = SourceName;
			LogSource = BepInEx.Logging.Logger.CreateLogSource("BiggerShip");
			_name = SourceName;
		}

		public override bool ShouldLog(LoggingType type)
		{
			return LocalConfigManager.Debug.Value >= type;
		}
	}
}
