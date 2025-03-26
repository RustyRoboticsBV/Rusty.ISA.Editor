using Godot;
using System.IO;

namespace Rusty.ISA.Editor
{
    public static class FileDialogMaker
    {
        /* Public methods. */
        public static FileDialog GetSave(string title, string folderPath = "res://", string fileName = "", string extension = "")
        {
            FileDialog dialog = Get(title, folderPath, fileName, extension);
            dialog.FileMode = FileDialog.FileModeEnum.SaveFile;
            return dialog;
        }

        public static FileDialog GetOpen(string title, string folderPath = "res://", string fileName = "", string extension = "")
        {
            FileDialog dialog = Get(title, folderPath, fileName, extension);
            dialog.FileMode = FileDialog.FileModeEnum.OpenFile;
            return dialog;
        }

        /* Private methods. */
        private static FileDialog Get(string title, string folderPath, string fileName, string extension)
        {
            // Make sure the root folder exists.
            if (!Directory.Exists(folderPath))
                Directory.CreateDirectory(folderPath);
            GD.Print(folderPath);

            // Create dialog.
            FileDialog dialog = new();
            dialog.Title = title;
            dialog.Mode = Window.ModeEnum.Maximized;
            dialog.InitialPosition = Window.WindowInitialPosition.CenterPrimaryScreen;
            dialog.Size = new Vector2I(1000, 500);
            dialog.Access = FileDialog.AccessEnum.Filesystem;
            dialog.CurrentDir = folderPath;
            dialog.CurrentFile = fileName;
            if (extension != "")
                dialog.AddFilter("*." + extension);
            return dialog;
        }
    }
}
