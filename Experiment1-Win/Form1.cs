using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using GuiTestLib;

namespace Experiment1_Win
{
	public partial class Form1 : Form
	{
		private const int COLUMNS = 10;
		private const int ROWS = 20;
		private const int LABELWIDTH = 100;
		private const int ENTRYWIDTH = 100;

		private int _controlspainted = 0;
		
		private GuiTracker _gt;

		public Form1(GuiTracker gt)
		{
			InitializeComponent();
			_gt = gt;

			DoStuff();
		}
	
		private void DoStuff()
		{
			Panel panel = new Panel();
			panel.Padding = new Padding(5);
			panel.Dock = DockStyle.Fill;
			panel.AutoScroll = true;

			TableLayoutPanel table = new TableLayoutPanel();
			table.RowCount = ROWS +1;
			table.ColumnCount = COLUMNS +1;
			table.Dock = DockStyle.Fill;
			table.AutoScroll = true;
		
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
						TextBox text = new TextBox();
						text.Dock = DockStyle.Fill;
						text.Text = _gt.Random.ShortString;
						text.Width = ENTRYWIDTH;
						//text.Paint += OnPaint;
						table.Controls.Add(text, x, y);
					}
				}
			}

			panel.Controls.Add(table);
			this.Controls.Add(panel);
			this.AutoScroll = true;
			this.WindowState = FormWindowState.Maximized;
		
			//Console.WriteLine("Executed in: {0}", _gt.ExecutionTime.ToString());
			//Console.WriteLine("CPU Usage: {0}", (_gt.Usage.Cpu / System.Environment.ProcessorCount).ToString());
			//Console.WriteLine("RAM Usage: {0}", _gt.Usage.Ram.ToString());
		}

		public void OnPaint(object sender, PaintEventArgs args)
		{
			_controlspainted++;
			if (_controlspainted > 100)
			{
				_gt.Stop();
				this.Close();
			}
			Console.WriteLine(_controlspainted.ToString());
		}
	}
}
