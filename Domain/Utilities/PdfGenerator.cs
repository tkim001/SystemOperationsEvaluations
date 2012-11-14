using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using iTextSharp;
using iTextSharp.text;
using iTextSharp.text.pdf;
using System.IO;
using System.Diagnostics;
using System.Collections;

namespace SystemOperationsEvaluation.Domain.Utilities
{
	public class PdfGenerator
	{
		#region Concatenate
		// This class uses an open-source PDF engine to concatenate multiple PDFs into one organized by bookmarks
		public static bool ConcatenateEvaluations(string destination, String[] args)
		{
			try
			{
				int f = 0;  // index of current document
				int i = 0;  // current page of the current document
				int n = 0; // total pages for current document
				int j = 0; // counter used to display two portrait pages on one landscape page
				float width = PageSize.LETTER.Width; //PageSize.LETTER.Height;
				float height = PageSize.LETTER.Height; //PageSize.LETTER.Width;

				// we create a reader for a certain document
				PdfReader reader;

				// step 1: creation of a document-object with portrait orientation
				Document document = new Document(PageSize.LETTER, 0, 0, 0, 0);

				// step 2: we create a writer that listens to the document
				PdfWriter writer = PdfWriter.GetInstance(document, new FileStream(destination, FileMode.Create));
				writer.ViewerPreferences = PdfWriter.PageModeUseOutlines;

				// step 3: we open the document
				document.Open();
				PdfContentByte cb = writer.DirectContent;
				PdfImportedPage page;
				int rotation;

				// step 4: we add the content
				while (f < args.Length)
				{
					i = 0;
					if (args[f] != "")
					{
						reader = new PdfReader(args[f]);
						// we retrieve the total number of pages
						n = reader.NumberOfPages;

						while (i < n)
						{
							i++;
							page = writer.GetImportedPage(reader, i);
							rotation = reader.GetPageRotation(i);

							//portrait - default
							document.SetPageSize(PageSize.LETTER);
							float pageHeight = reader.GetCropBox(i).Height;
							float bottom = reader.GetCropBox(i).Bottom;

							document.NewPage();
							cb.AddTemplate(page, 0, 0);
						}
						// free up memory
						writer.FreeReader(reader);
					}
					f++;

				}
				// step 5: we close the document
				document.Close();

			}
			catch (Exception e)
			{
				return false;
			}
			return true;
		}


		// http://stackoverflow.com/questions/566899/is-there-a-straight-forward-way-to-append-one-pdf-doc-to-another-using-itextsharp
		public static bool CombineMultiplePDFs(string outFile, string[] fileNames)
		{
			int pageOffset = 0;
			int f = 0;

			Document document = null;
			PdfCopy writer = null;
			IList<Dictionary<string, object>> master = new List<Dictionary<string, object>>();
			while (f < fileNames.Length)
			{
				// we create a reader for a certain document
				PdfReader reader = new PdfReader(fileNames[f]);
				reader.ConsolidateNamedDestinations();
				// we retrieve the total number of pages
				int n = reader.NumberOfPages;
				IList<Dictionary<string, object>> bookmarks = SimpleBookmark.GetBookmark(reader);

				if (bookmarks != null)
				{
					if (pageOffset != 0)
					{
						SimpleBookmark.ShiftPageNumbers(bookmarks, pageOffset, null);
					}
					foreach (Dictionary<string, object> dictionary in bookmarks)
					{
						master.Add(dictionary);
					}
				}
				pageOffset += n;

				if (f == 0)
				{
					// step 1: creation of a document-object
					document = new Document(reader.GetPageSizeWithRotation(1));
					// step 2: we create a writer that listens to the document
					writer = new PdfCopy(document, new FileStream(outFile, FileMode.Create));
					writer.ViewerPreferences = PdfWriter.PageModeUseOutlines;
					// step 3: we open the document
					document.Open();
				}
				// step 4: we add content
				for (int i = 0; i < n; )
				{
					++i;
					if (writer != null)
					{
						PdfImportedPage page = writer.GetImportedPage(reader, i);
						writer.AddPage(page);
					}
				}
				PRAcroForm form = reader.AcroForm;
				if (form != null && writer != null)
				{
					writer.CopyAcroForm(reader);
				}
				f++;
			}
			if (master.Count > 0 && writer != null)
			{
				writer.Outlines = master;
			}
			// step 5: we close the document
			if (document != null)
			{
				document.Close();
			}
			return true;
		}

		#endregion
	}
}
