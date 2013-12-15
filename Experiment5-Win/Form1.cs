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
using System.Threading;

namespace Experiment5_Win
{
	public partial class Form1 : Form
	{
		private const bool AUTOCLOSE = true;
		private const int COLUMNS = 2;
		private const int ROWS = 10;
		private const int LABELWIDTH = 100;
		private const int TEXTWIDTH = 150;

		string _imagepath = Directory.GetCurrentDirectory().ToString() + "\\Resources\\";

		private List<Label> _labels = new List<Label>();
		private List<TextBox> _textboxes = new List<TextBox>();
		private List<Button> _buttons = new List<Button>();
		private TabControl _tabbox = new TabControl();

		private int _tabnumber = 0;
		private int _controlspainted = 0;
		private int _imageindex = 0;

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
			TableLayoutPanel tabletext = new TableLayoutPanel();
			tabletext.RowCount = ROWS +1;
			tabletext.ColumnCount = COLUMNS +1;
			tabletext.Dock = DockStyle.Left;
			tabletext.Width = 270;

			_tabbox.Dock = DockStyle.Fill;

			TableLayoutPanel tablebutton = new TableLayoutPanel();
			tablebutton.RowCount = ROWS + 1;
			tablebutton.ColumnCount = COLUMNS +1;
			tablebutton.Dock = DockStyle.Right;
			tablebutton.Width = 270;

			foreach (ToolStripMenuItem item in _menu.Items)
			{
				item.Text = _gt.Random.ShortString;

				for (int x = 0; x < 5; x++)
				{
					_imageindex++;
					if (_imageindex > 100) { _imageindex = 1; }
					Image image = Image.FromFile(_imagepath + _imageindex.ToString("000") + ".png");

					ToolStripMenuItem subitem = new ToolStripMenuItem();
					subitem.Image = image;
					subitem.Text = _gt.Random.ShortString;
					subitem.Paint += OnPaint;

					item.DropDownItems.Add(subitem);
				}
			}

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
						tabletext.Controls.Add(label, x, y);
						_labels.Add(label);

						Label label2 = new Label();
						label2.Dock = DockStyle.Fill;
						label2.Text = _gt.Random.ShortString;
						label2.Width = LABELWIDTH;
						label2.TextAlign = ContentAlignment.MiddleRight;
						label2.Paint += OnPaint;
						tablebutton.Controls.Add(label2, x+1, y);
						_labels.Add(label2);
					}
					else
					{
						TextBox text = new TextBox();
						text.Dock = DockStyle.Fill;
						text.Text = _gt.Random.ShortString;
						text.Width = TEXTWIDTH;
						//text.Paint += OnPaint; // The paint event is never fired on textboxes
						tabletext.Controls.Add(text, x, y);
						_textboxes.Add(text);

						Button button = new Button();
						button.Dock = DockStyle.Fill;
						_imageindex++;
						if (_imageindex > 100) { _imageindex = 1; }
						button.Image = Image.FromFile(_imagepath + _imageindex.ToString("000") + ".png");
						button.ImageAlign = ContentAlignment.MiddleLeft;
						button.Text = _gt.Random.ShortString;
						button.Width = TEXTWIDTH;
						button.Height = 42;
						//button.Paint += OnPaint; // The paint event is never fired on buttons
						tablebutton.Controls.Add(button, x+1, y);
						_buttons.Add(button);
					}
				}
			}

			for (int y = 0; y < 4; y++)
			{
				_tabbox.Controls.Add(new TabPage(_gt.Random.ShortString));
			}

			_mainpanel.Controls.Add(_tabbox);
			_mainpanel.Controls.Add(tabletext);
			_mainpanel.Controls.Add(tablebutton);
			this.AutoScroll = true;
			this.WindowState = FormWindowState.Maximized;
		}

		public void LoadTab()
		{
			_tabnumber++;
			switch (_tabnumber)
			{
				case 1:
					Experiment1 exp1 = new Experiment1(this, _gt);
					exp1.Dock = DockStyle.Fill;
					_tabbox.Controls[0].Controls.Add(exp1);
					_tabbox.SelectedIndex = 0;
					break;
				case 2:
					Experiment2 exp2 = new Experiment2(this, _gt);
					exp2.Dock = DockStyle.Fill;
					_tabbox.Controls[1].Controls.Add(exp2);
					_tabbox.SelectedIndex = 1;
					break;
				case 3:
					Experiment3 exp3 = new Experiment3(this, _gt);
					exp3.Dock = DockStyle.Fill;
					_tabbox.Controls[2].Controls.Add(exp3);
					_tabbox.SelectedIndex = 2;
					break;
				case 4:
					Experiment4 exp4 = new Experiment4(this, _gt);
					exp4.Dock = DockStyle.Fill;
					_tabbox.Controls[3].Controls.Add(exp4);
					_tabbox.SelectedIndex = 3;
					break;
				case 5:
					_gt.Usage.TakeSnapshot("draw end");
					_gt.Stop();

					if (AUTOCLOSE) { this.Close(); }
					else { Console.WriteLine("finished"); }
					break;
			}

		}

		public void NewValues()
		{
			foreach (Label label in _labels)
			{
				label.Text = _gt.Random.ShortString;
			}
			foreach (TextBox text in _textboxes)
			{
				text.Text = _gt.Random.ShortString;
			}
			foreach (Button button in _buttons)
			{
				_imageindex++;
				if (_imageindex > 100) { _imageindex = 1; }
				button.Image = Image.FromFile(_imagepath + _imageindex.ToString("000") + ".png");
				button.Text = _gt.Random.ShortString;
			}
		}

		public void FocusMenu()
		{
			foreach (ToolStripMenuItem item in _menu.Items)
			{
				item.ShowDropDown();
				foreach (ToolStripMenuItem subitem in item.DropDownItems)
				{
					subitem.Select();
				}
				item.HideDropDown();
			}
		}

		public void ExperimentDone()
		{
			NewValues();
		}

		public void OnPaint(object sender, PaintEventArgs args)
		{
			_controlspainted++;
			if (_tabnumber == 0 && _controlspainted == 1)
			{
				_gt.Usage.TakeSnapshot("draw start");
			}
			else if (_controlspainted == 20 || _controlspainted == 185 || _controlspainted == 345 || _controlspainted == 510)
			{
				FocusMenu();
			}
			else if (_controlspainted == 165)
			{
				toolStripStatusLabel1.Text = _gt.Random.LongString;
				toolStripProgressBar1.ProgressBar.Value = 5;
				LoadTab();
			}
			else if (_controlspainted == 325)
			{
				toolStripStatusLabel1.Text = _gt.Random.LongString;
				toolStripProgressBar1.ProgressBar.Value = 25;
				LoadTab();
			}
			else if (_controlspainted == 490)
			{
				toolStripStatusLabel1.Text = _gt.Random.LongString;
				toolStripProgressBar1.ProgressBar.Value = 50;
				LoadTab();
			}
			else if (_controlspainted == 655)
			{
				toolStripStatusLabel1.Text = _gt.Random.LongString;
				toolStripProgressBar1.ProgressBar.Value = 75;
				LoadTab();
			}
			else if (_controlspainted == 675)
			{
				toolStripStatusLabel1.Text = _gt.Random.LongString;
				toolStripProgressBar1.ProgressBar.Value = 100;
				LoadTab();
			}
			/*else
			{
				Console.WriteLine(_controlspainted + ": " + sender.GetType());
			}*/
		}
	}
}
