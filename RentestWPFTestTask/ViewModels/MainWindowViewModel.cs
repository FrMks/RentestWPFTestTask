using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;
using Microsoft.Win32;
using RentestWPFTestTask.Infrastructure.Commands;
using RentestWPFTestTask.ViewModels.Baze;

namespace RentestWPFTestTask.ViewModels
{
    internal class MainWindowViewModel : ViewModel
    {
        public ICommand OpenImageCommand { get; }
        private bool CanOpenImageCommandExecute(object p) => true;
        private void OnOpenImageCommandExecute(object p)
        {
            var openFileDialog = new OpenFileDialog
            {
                Filter = "Image files (*.png;*.jpeg;*.jpg;*.bmp)|*.png;*.jpeg;*.jpg;*.bmp|All files (*.*)|*.*",
                InitialDirectory = Environment.GetFolderPath(Environment.SpecialFolder.MyPictures)
            };
            if (openFileDialog.ShowDialog()  == true)
            {
                string selectedImagePath = openFileDialog.FileName;
            }
        }

        public MainWindowViewModel()
        {
            OpenImageCommand = new LambdaCommand(OnOpenImageCommandExecute, CanOpenImageCommandExecute);
        }
    }
}
