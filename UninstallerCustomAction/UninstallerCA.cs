ï»¿using System;
using System.Collections;
using System.Collections.Generic;
using System.ComponentModel;
using System.Configuration.Install;
using System.Linq;
using System.IO;


namespace UninstallerCustomAction
{
    [RunInstaller(true)]
    public partial class UninstallerCA : Installer
    {
        public UninstallerCA()
        {
            InitializeComponent();
        }

        public override void Uninstall(IDictionary savedState)
        {
            base.Uninstall(savedState);
            //string output = this.Context.Parameters["assemblypath"];
            //string target_dir = Path.GetDirectoryName(output);
            //System.Diagnostics.Debugger.Break();
            //foreach (string file in Directory.GetFiles(target_dir))
            //{
            //    if (File.Exists(file))
            //    {
            //        File.Delete(file);
            //    }
            //}
            //Directory.Delete(target_dir);
        }
   
    }
}
