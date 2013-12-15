using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Data;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using GuiTestLib;

namespace Experiment5_Win
{
	public partial class Experiment3 : UserControl
	{
		private const bool AUTOCLOSE = true;
		private const int COLUMNS = 2;
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

		private Form1 _parent;
		private GuiTracker _gt;

		public Experiment3(Form1 parent, GuiTracker tracker)
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
		}

		private void OnPaint(object sender, PaintEventArgs args)
		{
			if (_repetitions <= 10)
			{
				_controlspainted++;

				if (_repetitions == 10)
				{
					_repetitions++;
					_parent.ExperimentDone();
				}
				else if (_controlspainted == 1)
				{
					_gt.Usage.TakeSnapshot("draw start");
				}
				else if (_controlspainted >= 10)
				{
					if (!_writing)
					{
						_textboxes[_next].Text = _gt.Random.LongString;
					}
				}
			}
		}

		private void OnTextChanged(object sender, EventArgs args)
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
