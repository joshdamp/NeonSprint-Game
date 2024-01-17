using System;
using System.Collections.Generic;
using System.Windows.Forms;
using System.Drawing;

namespace NeonSprint
{
    internal class Program
    {
        //go to loading screen
        static void Main(string[] args)
        {
            LoadingScreen loading = new LoadingScreen();
            Application.Run(loading);
        }
    }
}
