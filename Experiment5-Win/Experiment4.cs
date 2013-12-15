using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Data.SQLite;
using GuiTestLib;

namespace Experiment5_Win
{
	public partial class Experiment4 : UserControl
	{
		private const int COLUMNS = 2;
		private const int ROWS = 10;
		private const int LABELWIDTH = 300;
		private const int TEXTWIDTH = 300;
		private const int TEXTHEIGHT = 50;

		private DataGridView _datagridview = new DataGridView();
		private string _connectionstring = "Data Source=" + Directory.GetCurrentDirectory().ToString() + "\\Databases\\Northwind.sl3;Version=3;";
		private int _querynumber = 0;
		private int _paintedcells = 0;
		private int _expectedcells = 0;
		private SQLiteConnection _connection;

		private Form1 _parent;
		private GuiTracker _gt;

		public Experiment4(Form1 parent, GuiTracker tracker)
		{
			InitializeComponent();
			_parent = parent;
			_gt = tracker;

			_gt.Usage.TakeSnapshot("init start");
			DoStuff();
			_gt.Usage.TakeSnapshot("init end");
		}

		private void DoStuff()
		{
			_connection = new SQLiteConnection(_connectionstring);

			Panel panel = new Panel();
			panel.Padding = new Padding(5);
			panel.Dock = DockStyle.Fill;
			panel.AutoScroll = true;

			_datagridview = new DataGridView();
			_datagridview.Dock = DockStyle.Fill;
			_datagridview.CellPainting += OnCellPainting;

			panel.Controls.Add(_datagridview);
			this.Controls.Add(panel);
			this.AutoScroll = true;

			LoadData();
		}

		private void LoadData()
		{
			_querynumber++;
			switch (_querynumber)
			{
				case 1:
					_expectedcells = 8;
					SqlQuery("SELECT CategoryName FROM Categories");
					break;
				case 2:
					_expectedcells = 180;
					SqlQuery("SELECT CustomerID, CompanyName, ContactName, ContactTitle, City, Country FROM Customers LIMIT 55,30");
					break;
				case 3:
					_expectedcells = 45;
					SqlQuery("SELECT LastName, FirstName, Title, City, Region, Country FROM Employees");
					break;
				case 4:
					_expectedcells = 55;
					SqlQuery("SELECT ProductID, ProductName, QuantityPerUnit, UnitPrice, UnitsInStock FROM Products WHERE ProductName LIKE 'G%'");
					break;
				case 5:
					_expectedcells = 174;
					SqlQuery("SELECT ContactTitle, ContactName, CompanyName, Address, City, Country FROM Suppliers");
					break;
				case 6:
					_parent.ExperimentDone();
					break;
			}
		}

		private void SqlQuery(string sql)
		{
			_connection.Open();
			SQLiteCommand cmd = new SQLiteCommand(sql, _connection);
			SQLiteDataAdapter da = new SQLiteDataAdapter(cmd);
			DataSet ds = new DataSet();
			try
			{
				da.Fill(ds);
				DataTable dt = ds.Tables[0];
				_datagridview.DataSource = dt;

			}
			catch (SQLiteException ex)
			{
				MessageBox.Show(_connectionstring + "\n" + ex.Message + "\n" + ex.InnerException);
			}
			_connection.Close();
		}

		private void OnCellPainting(object sender, DataGridViewCellPaintingEventArgs e)
		{
			_paintedcells++;
			if (_paintedcells == _expectedcells)
			{
				_paintedcells = 0;
				LoadData();
			}
		}
	}
}
