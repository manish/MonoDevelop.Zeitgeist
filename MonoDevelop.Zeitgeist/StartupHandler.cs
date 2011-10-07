//
//  StartupHandler.cs
//
//  Author:
//       Patrick McEvoy <patrick@qmtech.net>
//
//  Copyright (c) 2011 QMTech.
//
// Permission is hereby granted, free of charge, to any person obtaining a copy of this
// software and associated documentation files (the "Software"), to deal in the Software
// without restriction, including without limitation the rights to use, copy, modify,
// merge, publish, distribute, sublicense, and/or sell copies of the Software, and to
// permit persons to whom the Software is furnished to do so, subject to the following
// conditions:
//
// The above copyright notice and this permission notice shall be included in all copies or
// substantial portions of the Software.
//
// THE SOFTWARE IS PROVIDED "AS IS", WITHOUT WARRANTY OF ANY KIND, EXPRESS
// OR IMPLIED, INCLUDING BUT NOT LIMITED TO THE WARRANTIES OF
// MERCHANTABILITY, FITNESS FOR A PARTICULAR PURPOSE AND NONINFRINGEMENT.
// IN NO EVENT SHALL THE AUTHORS OR COPYRIGHT HOLDERS BE LIABLE FOR ANY
// CLAIM, DAMAGES OR OTHER LIABILITY, WHETHER IN AN ACTION OF CONTRACT,
// TORT OR OTHERWISE, ARISING FROM, OUT OF OR IN CONNECTION WITH THE
// SOFTWARE OR THE USE OR OTHER DEALINGS IN THE SOFTWARE.

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

			MonoDevelop.Core.LoggingService.LogInfo ("Logging access of {0} to Zeitgeist", e.Document.FileName.FileName);
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

