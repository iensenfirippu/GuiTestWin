﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Windows.Forms;
using GuiTestLib;

namespace Experiment5_Win
{
	static class Program
	{
		/// <summary>
		/// The main entry point for the application.
		/// </summary>
		[STAThread]
		static void Main()
		{
			Application.EnableVisualStyles();
			Application.SetCompatibleTextRenderingDefault(false);
			Application.Run(new Form1(new GuiTracker("Experiment5", GuiTracker.Framework.Dnet, GuiTracker.Toolkit.Win)));
		}
	}
}
