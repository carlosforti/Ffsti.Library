using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Configuration;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;

namespace Ffsti.Library.WinForms.Forms
{
	public partial class ConfigureConnectionStrings : Form
	{
		private ConnectionStringSettings setting;
		Configuration config = ConfigurationManager.OpenExeConfiguration(ConfigurationUserLevel.PerUserRoaming);

		public ConfigureConnectionStrings()
		{
			InitializeComponent();
			InitializeForm();
		}

		private void InitializeForm()
		{
			PopulateConnectionStringsCombo();
			PopulateProviderNameCombo();
		}

		private void PopulateConnectionStringsCombo()
		{
			cboConnectionStrings.Items.Clear();
			Configuration config = ConfigurationManager.OpenExeConfiguration(ConfigurationUserLevel.None);

			foreach (ConnectionStringSettings setting in config.ConnectionStrings.ConnectionStrings)
			{
				cboConnectionStrings.Items.Add(setting);
			}

			cboConnectionStrings.DisplayMember = "Name";
		}

		private void PopulateProviderNameCombo()
		{
			var f = System.Data.Common.DbProviderFactories.GetFactoryClasses();
			var result = new List<Classes.Factory>();

			foreach (System.Data.DataRow row in f.Rows)
			{
				result.Add(new Classes.Factory()
				{
					Name = row[0].ToString(),
					Description = row[1].ToString(),
					InvariantName = row[2].ToString(),
					AssemblyQualifiedName = row[3].ToString()
				});
			}

			cboProviderName.DataSource = result;
			cboProviderName.DisplayMember = "Name";
			cboProviderName.ValueMember = "InvariantName";
		}

		private void ConfigureConnectionStrings_Load(object sender, EventArgs e)
		{

		}

		private void cboConnectionStrings_SelectionChangeCommitted(object sender, EventArgs e)
		{
			setting = cboConnectionStrings.SelectedItem as ConnectionStringSettings;

			if (setting != null)
			{
				txtNome.Text = setting.Name;
				txtConnectionString.Text = setting.ConnectionString;
				cboProviderName.SelectedValue = setting.ProviderName;
			}
		}

		private void btnSave_Click(object sender, EventArgs e)
		{
			bool newItem = false;
			
			var settings = config.ConnectionStrings.ConnectionStrings[setting.Name];
			
			if (settings == null)
			{
				newItem = true;
				settings = new ConnectionStringSettings();
				settings.Name = txtNome.Text;
			}
			
			settings.ConnectionString = txtConnectionString.Text;
			settings.ProviderName = cboProviderName.SelectedValue.ToString();
			
			if (newItem)
				config.ConnectionStrings.ConnectionStrings.Add(settings);

			config.Save(ConfigurationSaveMode.Modified, true);
			ConfigurationManager.RefreshSection("ConnectionStrings");
		}

		private void btnNew_Click(object sender, EventArgs e)
		{
			cboConnectionStrings.SelectedIndex = -1;
			setting = new ConnectionStringSettings();
			txtNome.Text = setting.Name;
			txtConnectionString.Text = setting.ConnectionString;
			cboProviderName.SelectedValue = setting.ProviderName;
		}
	}
}
