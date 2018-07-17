using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.IO;
using System.Windows.Forms;
using Common;
//Isadora Viana Silva ID: 92017784 26/01/2018
namespace GalaxyCinemas
{
    public class SessionImporter : BaseImporter
    {
        public SessionImporter(string filename) : base(filename)
        {
        }
        
        /// <summary>
        /// Import session file. Filename has been provided in the constructor.
        /// </summary>
        public override void Import(object o)
        {
            // Initialise progress to zero for progress bar.
            //float Progress = 0f;
            int ImportedRows = 0;
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
                string firstLine = lines[0];
                string[] columns = firstLine.Split(',');

                // Check if first line is column names.
                if (columns.Length == 4)
                {
                    if(columns[0].Trim().ToLower() == "sessionid" && columns[1].Trim().ToLower() == "movieid" && columns[2].Trim().ToLower() == "sessiondate" && columns[3].Trim().ToLower() == "cinemanumber")
                    {
                        lines[0] = "";
                    }
                }
                // Line count and line numbers to allow progress tracking.
                int lineCount = lines.Length;
                int lineNum = 1;
                // Get all movies. These will be used to check that MovieIDs are valid. 

                //List<Session> movies = DataLayer.DataLayer.GetAllSessionsForMovie(int.Parse(columns[1]),DateTime.Parse(columns[2]));

                List<Movie> movies = DataLayer.DataLayer.GetAllMovies();
                

                foreach (string line in lines)
                {
                    try
                    {
                        // Update progress of import.
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
                        // Just to make it slow enough to testing stopping functionality.
                        Thread.Sleep(500);

                        // Break up line by commas, each item in array will be one column.
                        columns = line.Split(',');
                        if (columns.Length != 4)
                        {
                            results.FailedRows++;
                            results.ErrorMessages.Add(string.Format("Line {0}: Wrong number of columns.", lineNum));
                            continue;
                        }


                        // Check the format of data, and update ImportResult accordingly.

                        // Check session ID.
                        int sessionID = 0;
                        if (!int.TryParse(columns[0].Trim(), out sessionID))
                        {
                            
                            results.FailedRows++;
                            results.ErrorMessages.Add(string.Format("Line {0}: sessionID not found in movie database.", lineNum));
                            continue;
                        }
                        // Check movie ID.
                        int movieID = 0;
                        if (!int.TryParse(columns[1].Trim(), out movieID))
                        {
                            results.FailedRows++;
                            results.ErrorMessages.Add(string.Format("Line {0}: MovieID is not a number.", lineNum));
                            continue;
                        }
                        if (movies.Count(m => m.MovieID == movieID) < 1)
                        {
                            
                            results.FailedRows++;
                            results.ErrorMessages.Add(string.Format("Line {0}: MovieID not found in movie database.", lineNum));
                            continue;
                            
                        }
                        // Check session date/time.
                        DateTime sessionDate = DateTime.MinValue;
                        if (!DateTime.TryParse(columns[2].Trim(), out sessionDate))
                        {
                            results.FailedRows++;
                            results.ErrorMessages.Add(string.Format("Line{0}: Session date is not a date/time", lineNum));
                            continue;
                        }
                        // Check cinema number.
                        byte cinemaNumber = 0;
                        if (!byte.TryParse(columns[3].Trim(), out cinemaNumber))
                        {
                            if (cinemaNumber < 1)
                            {
                                results.FailedRows++;
                                results.ErrorMessages.Add(string.Format("Line {0}: Cinema Number must be greater or equal to 1.", lineNum));
                                continue;
                            }
                        }

                        // Insert/update DB if okay.
                        Session sessionToUpdate = new Session();
                        sessionToUpdate = DataLayer.DataLayer.GetSessionByID(sessionID);
                        if (sessionToUpdate == null)
                        {
                            Session sessionToAdd = new Session() { SessionID = sessionID, MovieID = movieID, SessionDate = sessionDate, CinemaNumber = cinemaNumber };
                            DataLayer.DataLayer.AddSession(sessionToAdd);
                        }
                        else
                        {
                            sessionToUpdate.SessionID = sessionID;
                            sessionToUpdate.MovieID = movieID;
                            sessionToUpdate.SessionDate = sessionDate;
                            sessionToUpdate.CinemaNumber = cinemaNumber;
                            DataLayer.DataLayer.UpdateSession(sessionToUpdate);
                        }

                        ImportedRows++;

                        
                    }
                    catch (System.Data.Common.DbException db)
                    {
                        MessageBox.Show(db.ToString());
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
            catch (Exception e)
            {
                MessageBox.Show(e.ToString());
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
