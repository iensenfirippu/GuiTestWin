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
using GuiTestLib;

namespace Experiment3_Win
{

	public partial class Form1 : Form
	{
		private const bool AUTOCLOSE = true;
		private const int COLUMNS =	2;
		private const int ROWS = 10;
		private const int LABELWIDTH = 300;
		private const int TEXTWIDTH = 300;
		private const int TEXTHEIGHT = 50;

		private int _controlspainted = 0;
		private int _repetitions = 0;
		private int _next = 0;
		private bool _writing = false;
		private List<Label> _labels = new List<Label>();
		private List<TextBox> _textboxes = new List<TextBox>();
		private string _filedirectory = Directory.GetCurrentDirectory().ToString() + "\\Files\\";

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
			Panel panel = new Panel();
			panel.Padding = new Padding(5);
			panel.Dock = DockStyle.Fill;
			panel.AutoScroll = true;

			TableLayoutPanel table = new TableLayoutPanel();
			table.RowCount = ROWS + 1;
			table.ColumnCount = COLUMNS + 1;
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
						_labels.Add(label);
					}
					else
					{
						TextBox text = new TextBox();
						text.Multiline = true;
						text.Dock = DockStyle.Fill;
						text.Width = TEXTWIDTH;
						text.Height = TEXTHEIGHT;
						text.TextChanged += OnTextChanged;
						table.Controls.Add(text, x, y);
						_textboxes.Add(text);
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

			if (_repetitions == 10)
			{
				_repetitions++;
				_gt.Usage.TakeSnapshot("draw end");
				_gt.Stop();

				if (AUTOCLOSE) { this.Close(); }
				else
				{
					Console.WriteLine("finished");
					foreach (TextBox text in _textboxes)
					{
						text.TextChanged -= OnTextChanged;
					}
				}
			}
			else if (_controlspainted == 1)
			{
				_gt.Usage.TakeSnapshot("draw start");
			}
			else if (_controlspainted > 10)
			{
				Console.WriteLine(_repetitions + ":" + _controlspainted);
				if (!_writing)
				{
					_textboxes[_next].Text = _gt.Random.LongString;
				}
			}
		}

		public void OnTextChanged(object sender, EventArgs args)
		{
			_writing = true;
			TextBox Sender = (TextBox)sender;
			string oldcontent = string.Empty;

			if (Sender.Text != string.Empty)
			{
				int index = _textboxes.IndexOf(Sender);

				string filepath = _filedirectory + "file" + (index + 1).ToString("00") + ".txt";
				_labels[_next].Text = File.ReadAllText(filepath);

				File.Delete(filepath);
				File.WriteAllText(filepath, Sender.Text);

				if (index == 9)
				{
					_next = 0;
					_repetitions++;
				}
				else
				{
					_next++;
				}
			}
			_writing = false;
		}
	}
}
