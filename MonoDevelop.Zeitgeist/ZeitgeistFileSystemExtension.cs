using MonoDevelop.Core;
using MonoDevelop.Projects;
using MonoDevelop.Core.ProgressMonitoring;
using MonoDevelop.Core.FileSystem;
using MonoDevelop.Ide;

using System;
using System.Collections.Generic;

namespace MonoDevelop.Zeitgeist
{
	public class ZeitgeistFileSystemExtension : FileSystemExtension
	{

		public override bool CanHandlePath (FilePath path, bool isDirectory)
		{
			// FIXME: don't load this extension if the ide is not loaded.
			if (IdeApp.ProjectOperations == null || !IdeApp.Workspace.IsOpen)
				return false;
			else
				return true;
		}

		public ZeitgeistFileSystemExtension ()
		{
			/*Console.WriteLine("Zg#: init new");
			
			Event ev = new Event();
			ev.Actor = ZeitgeistAddin.TomboyUri;
			Subject sub = new Subject();
			sub.Interpretation = Interpretation.Instance.Document.Document;
			sub.Manifestation = Manifestation.Instance.FileDataObject.FileDataObject;
			ev.Subjects.Add(sub);
			
			try
			{
				dsReg.RegisterDataSources(tomboyDataSourceId, 
			                          	tomboyDataSourceName, 
			                          	tomboyDataSourceDesc , 
			                          	new List<Event>(){ev});
			}
			catch(Exception)
			{}*/
		}
		
		public override void RenameFile (FilePath file, string newName)
		{
			Console.WriteLine ("== ZeitgeistFileSystemExtension : RenameFile");
		}
		
		public override void MoveFile (FilePath source, FilePath dest)
		{
			Console.WriteLine ("== ZeitgeistFileSystemExtension : MoveFile");
		}
		
		public override void NotifyFilesChanged (IEnumerable<FilePath> file)
		{
			Console.WriteLine ("== ZeitgeistFileSystemExtension : NotifyFilesChanged");
		}

	}
}

