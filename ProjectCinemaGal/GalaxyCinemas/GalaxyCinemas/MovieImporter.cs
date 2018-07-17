using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.IO;
using Common;
//Isadora Viana Silva ID: 92017784 26/01/2018
namespace GalaxyCinemas
{
    public class MovieImporter : BaseImporter
    {
        public MovieImporter(string filename) : base(filename)
        {
        }
        /// <summary>
        /// Import movie file. Filename has been provided in the constructor.
        /// </summary>
        public override void Import(object o)
        {

            // Initialise progress to zero for progress bar.
            ImportResult results = new ImportResult();
            

            try
            {
                // Read file
                string fileData = null;
                using (StreamReader reader = File.OpenText(fileName))
                {
                    fileData = reader.ReadToEnd();

                }
                string[] lines = fileData.Replace("\r\n", "\n").Replace("\r", "\n").Split('\n'); // To deal with Windows, Mac and Linux line endings the same.
                string firstLine = lines[1];
                string[] columns = firstLine.Split(',');
               

                // Check if first line is column names.
                
                if (columns.Length == 2 && columns[0].Trim().ToLower() == "movieid" && columns[1].Trim().ToLower() == "title")
                {
                    lines[0] = "";
                }
                                
                int lineCount = lines.Length;
                int lineNum = 1;

                // Get all movies.
                List<Movie> movies = new List<Movie>();
                movies = DataLayer.DataLayer.GetAllMovies();


                // Check whether we need to stop after importing each line.
                // Line count and line numbers to allow progress tracking.
                foreach (string line in lines)
                {
                    try
                    {
                        Progress = (float)lineNum / (float)lineCount;
                        RaiseProgressChanged();
                        // Skip blank lines

                        if (line == "")
                        {
                            continue;
                        }
                        else
                        {
                            results.TotalRows++;
                        }

                        // Just to make it slow enough to test stopping functionality.
                        Thread.Sleep(500);

                        // Break up line by commas, each item in array will be one column.
                        columns = line.Split(',');
                        if (columns.Length != 2)
                        {
                            results.FailedRows++;
                            results.ErrorMessages.Add(string.Format("Line {0}: Wrong number of columns.", lineNum));
                            continue;
                        }

                        // Check the format of data, and update ImportResult accordingly.
                        int movieID = 0;
                        string title = columns[1].Trim();
                        if (!int.TryParse(columns[0], out movieID))
                        {
                            results.FailedRows++;
                            results.ErrorMessages.Add(string.Format("Line {0}: MovieID is not a number.", lineNum));
                            continue;
                        }
                        if (title == "")
                        {
                            results.FailedRows++;
                            results.ErrorMessages.Add(string.Format("Line {0}: Title is not a correct.", lineNum));
                            continue;
                        }

                        // Insert/update DB if okay.
                        Movie movieToUpdate = movies.Where(m => m.MovieID == movieID).FirstOrDefault();
                        if (movieToUpdate == null)
                        {
                            Movie movieToAdd = new Movie() { MovieID = movieID, Title = title };
                            DataLayer.DataLayer.AddMovie(movieToAdd);
                        }
                        else
                        {

                            movieToUpdate.Title = title;
                            DataLayer.DataLayer.UpdateMovie(movieToUpdate);
                        }


                    }
                    catch (System.Data.Common.DbException)
                    {
                        results.FailedRows++;
                        results.ErrorMessages.Add(string.Format("Line {0}: Database error occurred updating data.", lineNum));
                    }
                    finally
                    {
                        lineNum++;
                    }
                    results.ImportedRows++;
                }
            }
            
            catch (System.IO.IOException)
            {
                results.ErrorMessages.Add("Error occurred opening file. Please check that the file exists and that you have permissions to open it.");
            }
            catch (Exception)
            {
                results.ErrorMessages.Add("An unknown error occurred during importing.");
            }
            finally
            {
                // Do callback to end import.
                RaiseCompleted(results);
            }


        }

    }
}
