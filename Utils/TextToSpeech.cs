using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Speech.Synthesis;
using System.Globalization;

namespace SRWords
{
    public static class TextToSpeech
    {
        public static void Say(string table, string text)
        {
            SpeechSynthesizer synth = new SpeechSynthesizer();

            synth.Volume = 100;
            synth.Rate = 0;

            if (table == "rus")
                //synth.SelectVoice("Microsoft Pavel");
                synth.SelectVoice("Microsoft Irina Desktop");
            else
                synth.SelectVoice("Microsoft Matej");

            synth.SpeakAsync(text);


            //synth.SetOutputToDefaultAudioDevice();

            //synth.SelectVoice("Microsoft Irina Desktop");
            //synth.SelectVoiceByHints(VoiceGender.Male, VoiceAge.Senior);

            //synth.SelectVoice(voices[0].VoiceInfo.Name);

            ///*
            //synth.Volume = 100;
            //synth.Rate = 0;
            //synth.Speak("Если устанавливаемые голоса не работают, вам может помочь руководство");
            //synth.SpeakAsync("Если устанавливаемые голоса не работают, вам может помочь руководство");
            //*/

            //synth.SelectVoice("Microsoft Matej");
            //synth.SpeakAsync("Pri kraju Prvoga svjetskog rata, 1918 godine, Hrvatska raskida veze s Austro-Ugarskom i sudjeluje u osnivanju Države");

            //System.Threading.Thread.Sleep(2000);

            //synth.SelectVoice("Microsoft Pavel");
            //synth.SpeakAsync("Если устанавливаемые голоса не работают, вам может помочь руководство");

        }

        public static void ShowVoices()
        {
            SpeechSynthesizer synth = new SpeechSynthesizer();
            string t = "";
            foreach (var voice in synth.GetInstalledVoices())
            {
                var info = voice.VoiceInfo;
                t += String.Format("Id: {0} | Name: {1} | Age: {2} | Gender: {3} | Culture: {4}", info.Id, info.Name, info.Age, info.Gender, info.Culture) + Environment.NewLine;
            }
            System.Windows.Forms.MessageBox.Show(t);

            // Для того, чтобы узнать какие русские голоса установлены выполните код
            // var voices = synth.GetInstalledVoices(new CultureInfo("ru-RU"));
        }

        /*
         * https://stackoverflow.com/questions/34776593/speechsynthesizer-selectvoice-fails-with-no-matching-voice-is-installed-or-th
        The problem is that some of the voices are not registered for all applications. There is a nice article about it here: 
        https://www.ghacks.net/2018/08/11/unlock-all-windows-10-tts-voices-system-wide-to-get-more-of-them/

        But for those who will find this answer when the above link doesn't work:

        There are two registry keys which are involved.

        Computer\HKEY_LOCAL_MACHINE\SOFTWARE\Microsoft\Speech\Voices\Tokens
        Computer\HKEY_LOCAL_MACHINE\SOFTWARE\Microsoft\Speech_OneCore\Voices\Tokens
        The first one is used when querying with GetInstalledVoices() API call. The second one is used by the Windows Settings App.

        To make the unlisted voices available for GetInstalledVoices() you need to copy the data of the desired voices from Speech_OneCore to the Speech node 
        (and its x86 counterpart if needed).

        Step 1: Open the Windows Registry Editor (regedit.exe)
        Step 2: Open the list of available voices at Computer\HKEY_LOCAL_MACHINE\SOFTWARE\Microsoft\Speech_OneCore\Voices\Tokens
        Step 3: Export the key of the voice you need
        Step 4: Modify the exported Registry file and replace
        HKEY_LOCAL_MACHINE\SOFTWARE\Microsoft\Speech_OneCore\Voices\Tokens\ with
        HKEY_LOCAL_MACHINE\SOFTWARE\Microsoft\Speech\Voices\Tokens\
        Step 5: Also add both entries for 32 bit apps (if you need them) by duplicating the entries and replace the
        HKEY_LOCAL_MACHINE\SOFTWARE\Microsoft\Speech_OneCore\Voices\Tokens\ with
        HKEY_LOCAL_MACHINE\SOFTWARE\WOW6432Node\Microsoft\Speech\Voices\Tokens\
        Step 6: Import the modified file to the registry.
        Now it should work (a restart might be needed)
         */

    }
}
