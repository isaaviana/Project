using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.IO;
using System.Windows.Forms;
using System.Xml.Serialization;
using Common;
namespace GalaxyCinemas
{
    
    public partial class ExportDataForm : Form
    {
        public ExportDataForm()
        {
            InitializeComponent();
            this.FormClosing += ExportDataForm_FormClosing;
        }
        //method to get a list to serialize all items from the list.

        /// <summary>
        /// Serialize bookings to XML file.
        /// </summary>
        /// <param name="list"></param>
        public void Serialize(List<Booking> list, string filename)
        {
            XmlSerializer serializer = new XmlSerializer(typeof(List<Booking>)); //type of to show which the serializer will accept 
            using (TextWriter writer = File.CreateText(filename))
            {
                serializer.Serialize(writer, list);
                
            }

        } 
        /// <summary>
        /// Allows user to browse to a save location for the XML file.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void btnSelectExportBooking_Click(object sender, EventArgs e)
        {
            
            txtFileBooking.Focus(); // Set focus on this field. Moving focus will force validation of the value.
        }

        #region Form validation

        /// <summary>
        /// Check all form fields are valid. This works even if they haven't clicked into every field.
        /// </summary>
        /// <returns></returns>
        private bool IsFormValid()
        {
            foreach (Control control in Controls)
            {
                // Set focus on control
                control.Focus();
                // Validate causes the control's Validating event to be fired,
                // if CausesValidation is True
                if (!Validate())
                {
                    return false;
                }
            }
            return true;
        }

        /// <summary>
        /// Validate the filename.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void txtFileBooking_Validating(object sender, CancelEventArgs e)
        {
            // Check if file path is valid.
            bool pathValid = true;
            try
            {
                FileInfo fi = new FileInfo(txtFileBooking.Text);
                pathValid = fi.Directory.Exists;
            }
            catch (Exception)
            {
                pathValid = false;
            }

            if (!pathValid)
            {
                errorProvider.SetError(txtFileBooking, "Please choose a valid path to export to");
                e.Cancel = true; // Don't allow moving to the next field.
            }
            else errorProvider.SetError(txtFileBooking, ""); // Clear error if all fine.
        }

        /// <summary>
        /// Validate the to date.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void dtpTo_Validating(object sender, CancelEventArgs e)
        {
            if (dtpTo.Value.Date < dtpFrom.Value.Date)
            {
                errorProvider.SetError(dtpTo, "The 'to' date must be greater than or equal to the 'from' date");
                e.Cancel = true; // Don't allow moving to the next field.
            }
            else errorProvider.SetError(dtpTo, ""); // Clear error if all fine.
        }

        private void ExportDataForm_FormClosing(object sender, FormClosingEventArgs e)
        {
            // Don't allow validation to prevent closing.
            e.Cancel = false;
        }

        #endregion

        /// <summary>
        /// Export bookings to XML file.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void btnExportBookings_Click(object sender, EventArgs e)
        {
            if (!IsFormValid())
            {
                return;
            }
            else
            {
                try
                {
                    if (txtFileBooking.Text != "")
                    {
                        List<Booking> bookings = DataLayer.DataLayer.GetBookingsInDateRange(DateTime.Parse(dtpFrom.Text), DateTime.Parse(dtpTo.Text));
                        
                            Serialize(bookings, txtFileBooking.Text);
                        


                        MessageBox.Show("Number of bookings: " + bookings.Count.ToString() + " exported successfully.");
                    }
                    else
                    {
                        MessageBox.Show("Save Location cannot be empty", "Error");
                    }
                }
                catch (Exception ex)
                {
                    MessageBox.Show(ex.ToString(), "Error occurred while booking");
                }

            }
        }
        /// <summary>
        /// Closes the form and goes back to main menu.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void btnBack_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        private void btnBrowse_Click(object sender, EventArgs e)
        {

            
            //Display a SaveFileDialog so the user can save the xml
            SaveFileDialog saveFileDialog = new SaveFileDialog();            
            saveFileDialog.Filter = "txt file (*.xml)|*.xml";
            DialogResult result = saveFileDialog.ShowDialog();

            if (result == DialogResult.OK)
            {
                txtFileBooking.Text = saveFileDialog.FileName;
                File.WriteAllText(saveFileDialog.FileName, txtFileBooking.Text);
            }
            else
            {
                MessageBox.Show("You have clicked to cancel operation", "Error");
            }



        }
    }
}
