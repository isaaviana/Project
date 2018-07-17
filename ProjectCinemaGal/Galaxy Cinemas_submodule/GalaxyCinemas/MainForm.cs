using Common;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Windows.Forms;

namespace GalaxyCinemas
{
    public partial class MainForm : Form
    {
        private List<ISpecialPlugin> specialPlugins = new List<ISpecialPlugin>();

        public MainForm()
        {
            InitializeComponent();

            try
            {
                DirectoryInfo dir = new DirectoryInfo(Application.StartupPath);

                foreach (FileInfo file in dir.GetFiles("Plugin*.dll"))
                {

                    string name = Path.GetFileNameWithoutExtension(file.Name);
                    //Firts load the assembly by name. Check wich plugins are defined in it and then construct your ISpecialPlugin objects. 
                    Assembly pluginAssem = Assembly.Load(name);
                    var plugins = from type in pluginAssem.GetTypes() where typeof(ISpecialPlugin).IsAssignableFrom(type) && !type.IsInterface select type;

                    foreach(Type pluginType in plugins)
                    {
                        ISpecialPlugin plugin = Activator.CreateInstance(pluginType) as ISpecialPlugin;
                        specialPlugins.Add(plugin);
                    }


                }
            }
            catch (Exception)
            {
                MessageBox.Show("An error ocurred while loading special pricing plugins");
            }
        }

        private void ChildFormClosed(object sender, FormClosedEventArgs e)
        {
            // To ensure the main form has focus after a child form is closed.
            this.Focus();
        }

        private void btnImport_Click(object sender, EventArgs e)
        {
            ImportDataForm idf = new ImportDataForm();
            idf.FormClosed += ChildFormClosed;
            idf.Show();
        }

        private void MainForm_Load(object sender, EventArgs e)
        {

        }
        //ChildFormClosed ensure that when the child form closes, the Main form becames visible
        private void btnBookingForm_Click(object sender, EventArgs e)
        {
            BookingForm bookForm = new BookingForm(specialPlugins);
            bookForm.FormClosed += ChildFormClosed; 
            bookForm.Show();

        }

        private void btnExpDataForm_Click(object sender, EventArgs e)
        {
            ExportDataForm exDataForm = new ExportDataForm();
            exDataForm.FormClosed += ChildFormClosed;
            exDataForm.Show();
        }

        private void btnExit_Click(object sender, EventArgs e)
        {
            Application.Exit();
        }
    }
}
