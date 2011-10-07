using System;
using Mono.Addins;
using MonoDevelop.Components.Commands;
using MonoDevelop.Ide.Extensions;
using MonoDevelop.Projects;
using MonoDevelop.Ide.Gui;
using System.Collections.Generic;
using MonoDevelop.Core;

namespace MonoDevelop.Zeitgeist
{
	[Extension ("/MonoDevelop/Ide/StartupHandlers", NodeName="Class")]
	public class StartupHandler: CommandHandler
	{
		ZeitgeistClient client;
		IList<TrackedDocument> documents = new List<TrackedDocument> ();

		public StartupHandler ()
		{
			client = new ZeitgeistClient ();
		}

		protected override void Run ()
		{
			// TODO Log the project/solu opening/closing too
			// TODO Log renamed & deleted files
			// TODO Don't log files as opened until they before active through usage when opening a project/solu
			// IDEA: Auto-close tabs that aren't being used
			Ide.IdeApp.Workbench.DocumentOpened += HandleDocumentOpened;
		}

		void HandleDocumentOpened (object sender, DocumentEventArgs e)
		{
			if (!e.Document.IsFile)
				return;

			MonoDevelop.Core.LoggingService.LogInfo ("== MonoDevelop.Zeitgeist : Opened {0}", e.Document.FileName.FileName);
			var tracked = new TrackedDocument (client, e.Document);
			tracked.Destroyed += delegate {
				foreach (var t in documents)
					if (!t.IsCurrent ())
						documents.Remove (t);
			};
			documents.Add (tracked);
			client.SendFilePath (e.Document.FileName, EventType.Access);
		}
	}
}

