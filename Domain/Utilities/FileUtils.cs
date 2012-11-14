using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.IO;

namespace SystemOperationsEvaluation.Domain.Utilities
{
    public class FileUtils
    {
        public static void CleanFileDirectory(string directory)
        {
            if (Directory.Exists(directory))
            {
                string[] files = Directory.GetFiles(directory);

                foreach (string fileName in files)
                {
                    FileInfo file = new FileInfo(fileName);
                    try
                    {
                        file.Delete();
                    }
                    catch (Exception e)
                    {
                        //
                    }
                }

            }
        }
    }
}
