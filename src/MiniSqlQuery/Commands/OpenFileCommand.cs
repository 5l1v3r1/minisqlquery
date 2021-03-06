﻿#region License

// Copyright 2005-2019 Paul Kohler (https://github.com/paulkohler/minisqlquery). All rights reserved.
// This source code is made available under the terms of the GNU Lesser General Public License v3.0
// https://github.com/paulkohler/minisqlquery/blob/master/LICENSE
#endregion

using System;
using System.Windows.Forms;
using MiniSqlQuery.Core;
using MiniSqlQuery.Core.Commands;
using WeifenLuo.WinFormsUI.Docking;

namespace MiniSqlQuery.Commands
{
    /// <summary>The open file command.</summary>
    public class OpenFileCommand
        : CommandBase
    {
        /// <summary>Initializes a new instance of the <see cref="OpenFileCommand"/> class.</summary>
        public OpenFileCommand()
            : base("&Open File")
        {
            ShortcutKeys = Keys.Control | Keys.O;
            SmallImage = ImageResource.folder_page;
        }

        /// <summary>Execute the command.</summary>
        public override void Execute()
        {
            OpenFileDialog openFileDialog = new OpenFileDialog();
            openFileDialog.InitialDirectory = Environment.GetFolderPath(Environment.SpecialFolder.MyComputer);
            openFileDialog.Filter = Settings.DefaultFileFilter;
            openFileDialog.CheckFileExists = true;
            if (openFileDialog.ShowDialog(HostWindow.Instance) == DialogResult.OK)
            {
                // todo: check for file exist file in open windows;
                IFileEditorResolver resolver = Services.Resolve<IFileEditorResolver>();
                var fileName = openFileDialog.FileName;
                IEditor editor = resolver.ResolveEditorInstance(fileName);
                editor.FileName = fileName;
                editor.LoadFile();
                HostWindow.DisplayDockedForm(editor as DockContent);

                Services.Resolve<IMostRecentFilesService>().Register(fileName);
            }
        }
    }
}