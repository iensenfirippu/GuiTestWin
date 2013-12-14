using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Drawing.Imaging;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using GuiTestLib;

namespace Experiment2_Win
{
	public partial class Form1 : Form
	{
		private const bool AUTOCLOSE = true;
		private const int COLUMNS = 10;
		private const int ROWS = 20;
		private const int LABELWIDTH = 100;
		private const int BUTTONWIDTH = 200;

		private int _controlspainted = 0;

		private GuiTracker _gt;

		public Form1(GuiTracker gt)
		{
			InitializeComponent();
			_gt = gt;

			_gt.Usage.TakeSnapshot("init start");
			DoStuff();
			_gt.Usage.TakeSnapshot("init end");
		}

		private void DoStuff()
		{
			string imagepath = Directory.GetCurrentDirectory().ToString() + "\\Resources\\";

			Panel panel = new Panel();
			panel.Padding = new Padding(5);
			panel.Dock = DockStyle.Fill;
			panel.AutoScroll = true;

			TableLayoutPanel table = new TableLayoutPanel();
			table.RowCount = ROWS + 1;
			table.ColumnCount = COLUMNS + 1;
			table.Dock = DockStyle.Fill;
			table.AutoScroll = true;

			int i = 0;
			for (int y = 0; y < ROWS; y++)
			{
				for (int x = 0; x < COLUMNS; x++)
				{
					if (x % 2 == 0)
					{
						Label label = new Label();
						label.Dock = DockStyle.Fill;
						label.Text = _gt.Random.ShortString;
						label.Width = LABELWIDTH;
						label.TextAlign = ContentAlignment.MiddleRight;
						label.Paint += OnPaint;
						table.Controls.Add(label, x, y);
					}
					else
					{
						Button button = new Button();
						button.Dock = DockStyle.Fill;

						i++;
						button.Image = Image.FromFile(imagepath + i.ToString("000") + ".png");

						button.ImageAlign = ContentAlignment.MiddleLeft;
						button.Text = _gt.Random.ShortString;
						button.Width = BUTTONWIDTH;
						button.Height = 42;
						//button.Paint += OnPaint; // The paint event is never fired on buttons
						table.Controls.Add(button, x, y);
					}
				}
			}

			panel.Controls.Add(table);
			this.Controls.Add(panel);
			this.AutoScroll = true;
			this.WindowState = FormWindowState.Maximized;
		}

		public void OnPaint(object sender, PaintEventArgs args)
		{
			_controlspainted++;
			if (_controlspainted == 1)
			{
				_gt.Usage.TakeSnapshot("draw start");
			}	
			else if (_controlspainted >= 100)
			{
				_gt.Usage.TakeSnapshot("draw end");
				_gt.Stop();

				if (AUTOCLOSE) { this.Close(); }
			}
			//Console.WriteLine(_controlspainted.ToString());
		}
	}
}
