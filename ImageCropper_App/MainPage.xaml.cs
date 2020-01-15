using Microsoft.Toolkit.Uwp.UI.Controls;
using Microsoft.Toolkit.Uwp.UI.Extensions;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices.WindowsRuntime;
using System.Threading.Tasks;
using Windows.Foundation;
using Windows.Foundation.Collections;
using Windows.Storage;
using Windows.Storage.Pickers;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Controls.Primitives;
using Windows.UI.Xaml.Data;
using Windows.UI.Xaml.Input;
using Windows.UI.Xaml.Media;
using Windows.UI.Xaml.Navigation;

// The Blank Page item template is documented at https://go.microsoft.com/fwlink/?LinkId=402352&clcid=0x409

namespace ImageCropper_App
{
    /// <summary>
    /// An empty page that can be used on its own or navigated to within a Frame.
    /// </summary>
    public sealed partial class MainPage : Page
    {

        private ImageCropper _imageCropper;
        public MainPage()
        {
            this.InitializeComponent();
            OnXamlRendered(BigPanel);
        }

        public async void OnXamlRendered(FrameworkElement control)
        {
            _imageCropper = control.FindChildByName("ImageCropper") as ImageCropper;
            if (_imageCropper != null)
            {
                var file = await StorageFile.GetFileFromApplicationUriAsync(new Uri("ms-appx:///Assets/StoreLogo.png"));
                await _imageCropper.LoadImageFromFile(file);
            }
        }

        private async Task PickImage()
        {
            var filePicker = new FileOpenPicker
            {
                ViewMode = PickerViewMode.Thumbnail,
                SuggestedStartLocation = PickerLocationId.PicturesLibrary,
                FileTypeFilter =
                {
                    ".png", ".jpg", ".jpeg"
                }
            };
            var file = await filePicker.PickSingleFileAsync();
            if (file != null && _imageCropper != null)
            {
                await _imageCropper.LoadImageFromFile(file);
            }
        }

        private async Task SaveCroppedImage()
        {
            var savePicker = new FileSavePicker
            {
                SuggestedStartLocation = PickerLocationId.PicturesLibrary,
                SuggestedFileName = "Cropped_Image",
                FileTypeChoices =
                {
                    { "PNG Picture", new List<string> { ".png" } },
                    { "JPEG Picture", new List<string> { ".jpg" } }
                }
            };
            var imageFile = await savePicker.PickSaveFileAsync();
            if (imageFile != null)
            {
                BitmapFileFormat bitmapFileFormat;
                switch (imageFile.FileType.ToLower())
                {
                    case ".png":
                        bitmapFileFormat = BitmapFileFormat.Png;
                        break;
                    case ".jpg":
                        bitmapFileFormat = BitmapFileFormat.Jpeg;
                        break;
                    default:
                        bitmapFileFormat = BitmapFileFormat.Png;
                        break;
                }

                using (var fileStream = await imageFile.OpenAsync(FileAccessMode.ReadWrite, StorageOpenOptions.None))
                {
                    await _imageCropper.SaveAsync(fileStream, bitmapFileFormat);
                }
            }
        }

        private async void Button_Click(object sender, RoutedEventArgs e)
        {
           await PickImage();
        }

        private async void Button_Click_1(object sender, RoutedEventArgs e)
        {
            await SaveCroppedImage();
        }
    }
}

