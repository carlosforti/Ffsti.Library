using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System.IO;

namespace Ffsti.Library.Tests
{
	[TestClass]
	public class LogTest
	{
		[TestMethod]
		public void LoggingDebugTest()
		{
			var assembly = System.Reflection.Assembly.GetExecutingAssembly();

			assembly.Use(a =>
			{
				FileInfo fileInfo = new FileInfo(a.Location);
				Directory.CreateDirectory(fileInfo.Directory.FullName + "\\logs");
				DirectoryInfo dirInfo = new DirectoryInfo(fileInfo.Directory.FullName + "\\logs");
				var logFiles = dirInfo.GetFiles("*.log");
				foreach (var logFile in logFiles)
					File.Delete(logFile.FullName);

				Logging.Debug("Teste");

				logFiles = dirInfo.GetFiles("*.log");
				TextReader reader = new StringReader(logFiles[0].FullName);
				var linha = reader.ReadLine();

				Assert.AreNotEqual(linha, "");
			});
		}
	}
}
