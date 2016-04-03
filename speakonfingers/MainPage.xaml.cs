using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices.WindowsRuntime;
using Windows.Foundation;
using Windows.Foundation.Collections;
using Windows.Media.SpeechRecognition;
using Windows.Media.SpeechSynthesis;
using Windows.Storage;
using Windows.Storage.Streams;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Controls.Primitives;
using Windows.UI.Xaml.Data;
using Windows.UI.Xaml.Input;
using Windows.UI.Xaml.Media;
using Windows.UI.Xaml.Navigation;


 // The Blank Page item template is documented at http://go.microsoft.com/fwlink/?LinkId=391641

namespace speakonfingers
{
    /// <summary>
    /// An empty page that can be used on its own or navigated to within a Frame.
    /// </summary>
    public sealed partial class MainPage : Page
    {
        public MainPage()
        {
            this.InitializeComponent();

            this.NavigationCacheMode = NavigationCacheMode.Required;
        }

        /// <summary>
        /// Invoked when this page is about to be displayed in a Frame.
        /// </summary>
        /// <param name="e">Event data that describes how this page was reached.
        /// This parameter is typically used to configure the page.</param>
        protected override void OnNavigatedTo(NavigationEventArgs e)
        {
            // TODO: Prepare page for display here.

            // TODO: If your application contains multiple pages, ensure that you are
            // handling the hardware Back button by registering for the
            // Windows.Phone.UI.Input.HardwareButtons.BackPressed event.
            // If you are using the NavigationHelper provided by some templates,
            // this event is handled for you.
        }
        List<string> lstVoices = new List<string>();
        private void btnShowVoices_Click(object sender, RoutedEventArgs e)
        {
            //Get all the Voices
            foreach (var voice in SpeechSynthesizer.AllVoices)
            {
                lstVoices.Add(voice.DisplayName);
            }
            lstvwVoices.ItemsSource = lstVoices;
        }
        private void btnTTS_Click(object sender, RoutedEventArgs e)
        {
            SpeakText(audioPlayer, txtInfo.Text);
        }

        private async void SpeakText(MediaElement audioPlayer, string TTS)
        {
            SpeechSynthesizer ttssynthesizer = new SpeechSynthesizer();

            //Set the Voice/Speaker
            using (var Speaker = new SpeechSynthesizer())
            {
                Speaker.Voice = (SpeechSynthesizer.DefaultVoice);
                lol.Text = Speaker.Voice.Description.ToString();
              //  Speaker.Voice = (SpeechSynthesizer.AllVoices.First(x => x.Gender == VoiceGender.Female));
                ttssynthesizer.Voice = Speaker.Voice;
            }

            SpeechSynthesisStream ttsStream = await ttssynthesizer.SynthesizeTextToStreamAsync(TTS);

            audioPlayer.SetSource(ttsStream, "");
           // saveAudio(ttsStream);
        }
        public async void saveAudio(SpeechSynthesisStream stream)
        {
            // get inputstream and size of stream
            ulong size = stream.Size;
            IInputStream inputStream = stream.GetInputStreamAt(0);

            stream.Dispose();

            DataReader dataReader = new DataReader(inputStream);
            await dataReader.LoadAsync((uint)size);
            byte[] buffer = new byte[(int)size];
            dataReader.ReadBytes(buffer);
            inputStream.Dispose();
            dataReader.Dispose();

            // open folder and file
            IStorageFolder folder = await ApplicationData.Current.LocalFolder.CreateFolderAsync("Audio", CreationCollisionOption.OpenIfExists);
            IStorageFile file = await folder.CreateFileAsync("audio.wav", Windows.Storage.CreationCollisionOption.ReplaceExisting);

            // write file
            await Windows.Storage.FileIO.WriteBytesAsync(file, buffer);
            System.Diagnostics.Debug.WriteLine("save complete");
        }
        //private async void btnSTT_Click(object sender, RoutedEventArgs e)
        //{
        //    // Compile the dictation grammar
        //    await speechRecog.CompileConstraintsAsync();

        //    // Start Recognition
        //    SpeechRecognitionResult speechRecognitionResult = await this.speechRecog.RecognizeWithUIAsync();

        //    // Show Output
        //    var sttDialog = new Windows.UI.Popups.MessageDialog(speechRecognitionResult.Text, "Heard You said...");
        //    await sttDialog.ShowAsync();
        //}

        private void likhbc_SelectionChanged(object sender, RoutedEventArgs e)
        {
            
        }
    }
}
