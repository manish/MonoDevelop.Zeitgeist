using System;
using MonoDevelop.Ide.Gui;
using MonoDevelop.Core;

namespace MonoDevelop.Zeitgeist
{
	internal class TrackedDocument
	{
		public Document Document { get; set; }
		public FilePath FilePath { get; set; }

		ZeitgeistClient client;

		public TrackedDocument (ZeitgeistClient _client, Document doc)
		{
			client = _client;

			doc.Closed += HandleDocClosed;
			doc.Saved += HandleDocSaved;
			Document = doc;
			FilePath = new FilePath (doc.FileName);
		}

		void HandleDocSaved (object sender, EventArgs e)
		{
			MonoDevelop.Core.LoggingService.LogInfo ("== MonoDevelop.Zeitgeist : Saved {0}", Document.FileName.FileName);
			client.SendFilePath (FilePath, EventType.Modify);
		}

		void HandleDocClosed (object sender, EventArgs e)
		{
			MonoDevelop.Core.LoggingService.LogInfo ("== MonoDevelop.Zeitgeist : Closed {0}", FilePath.FileName);
			client.SendFilePath (FilePath, EventType.Leave);
		}

		public virtual void Dispose ()
		{
			if (Destroyed != null)
				Destroyed (this, EventArgs.Empty);
		}

		public event EventHandler Destroyed;

		public bool IsCurrent ()
		{
			if (Document == null)
				return false;
			return true;
		}
	}
}

