using System.Reflection.Metadata.Ecma335;
using NAudio;
using NAudio.Wave.SampleProviders;
using NAudio.Wave;
using System.Runtime.InteropServices;
using System.IO.Enumeration;

Console.Clear();
// Main loop that runs until user sets running to false
bool running = true;
while (running == true)
{
    Console.WriteLine("Press 1 to Begin \nPress 0 to Exit");
    string introAns = Console.ReadLine();
    {
        if (introAns == "1")
        {
            Console.Clear();
            App();
        }
        else if (introAns == "0")
        {
            running = false;
        }
        else
        {
            Console.Clear();
            ErrorMessage();
        }
    }
}


// generic error message for invalid input

static void ErrorMessage()
{
    Console.WriteLine("Invalid input! \nPress Anything to Continue...");
    Console.ReadKey();
    Console.Clear();
}


// main app

static void App()
{
    //error check loop for error handling
    bool errorCheck = true;
    while (errorCheck == true)
    {
        //create array from files in the uploads folder
        string[] fileArray = Directory.GetFiles(@".\Input");

        if (fileArray.Length == 0)
        {
            Console.WriteLine("There Are No Files In The Input Folder! \nPress Anything To Continue...");
            Console.ReadKey();
            Console.Clear();
        }
        else
        {
            int fileSelection = 1;
            // create and display list of all files in array so user can select which ones to convert
            foreach (string file in fileArray)
            {
                Console.WriteLine(fileSelection + ") "+ file);
                fileSelection++;
            }
            // user selection
            Console.WriteLine("A or a) All Files\nSelect Which File(s) to Nightcorify:");
            string fileSelectionAns = Console.ReadLine();
            int fileSelectionAnsInt;
            // check if input is a valid number, the letter A or a, or an invalid input
            if (int.TryParse(fileSelectionAns, out fileSelectionAnsInt))
            {
                //create filename by manipulating the file path string
                string fileName = (fileArray[fileSelectionAnsInt - 1].Remove(0, 9).Remove(fileArray[fileSelectionAnsInt - 1].Length - 13));
                //call the pitch modifier
                Pitcher(fileArray[fileSelectionAnsInt - 1], fileName);
                Console.WriteLine("Done! \nPress Anything To Continue...");
                Console.ReadKey();
                Console.Clear();
                errorCheck = false;
            }
            else if (fileSelectionAns == "a" || fileSelectionAns == "A")
            {
                // foreach loop that creates names for the files by manipulating the file path string then calls the pitch modifier on each file
                foreach (string file in fileArray)
                {
                    string fileName = (file.Remove(0, 9).Remove(file.Length - 13));
                    Pitcher(file, fileName);
                }
                Console.WriteLine("Done! \nPress Anything To Continue...");
                Console.ReadKey();
                Console.Clear();
                errorCheck = false;
            }
            else
            {
                Console.Clear();
                Console.WriteLine("Invalid Input \nPress Anything to Continue...");
                Console.ReadKey();
                Console.Clear();
            }


        }
    }

}

// pitch modifier
static void Pitcher(string fileSelector, string fileOutput)
{
    //get file to pitch and set an output directory
    var inPath = fileSelector;
    var outfile = @"./Output/" + Convert.ToString(fileOutput) + ".wav";
    using (var reader = new MediaFoundationReader(inPath))
    {
        //set up the audio library
        var pitch = new SmbPitchShiftingSampleProvider(reader.ToSampleProvider());
        using (var device = new WaveOutEvent())
        {
            Console.Clear();
            Console.WriteLine("Converting. Please Wait.");
            // pitch the audio
            pitch.PitchFactor = (float)1.12246204831;
            //output the pitched audio to a wave file
            WaveFileWriter.CreateWaveFile(outfile, pitch.ToWaveProvider());
            Console.Clear();
        }
    }
}
