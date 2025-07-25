using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;
using System.Windows.Forms;

public class AWM_Y_Patch
{
    public static async Task Run(dynamic S1K, Label PID)
    {
        if (Process.GetProcessesByName("HD-Player").Length == 0)
        {
            PID.Text = "Emulator not found";
            Console.Beep(200, 100);
            return;
        }

        Stopwatch stopwatch = new Stopwatch();
        stopwatch.Start();
        PID.Text = "Applying...";
        Console.Beep(200, 300);

        string search = "FF FF FF FF FF FF FF FF FF FF FF FF 01 00 00 00 68 00 00 00 94 01 00 00 02 00 00 00 03 00 00 00 CD CC 8C 3F CD CC 4C 3F CD CC 4C 3F 00 00 00 00 00 00 80 3F 00 00 80 3F ?? ?? ?? ?? 00 00 00 00 ?? ?? ?? ?? 20 ?? ?? ?? 00 00 00 00 FF FF FF FF 00 00 00 00 ?? ?? ?? ?? C8 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 05 00 00 00 01 00 00 00 B4 C8 D6 3F 01 00 00 00 B4 C8 D6 3F 00 00 00 00 B4 C8 D6 3F 00 00 80 3F 00 00 80 3F 0A D7 A3 3D 00 00 00 00 00 00 5C 43 00 00 90 42 00 00 B4 42 96 00 00 00 00 00 00 00 00 00 00 3F 00 00 80 3E 00 00 00 00 04 00 00 00 00 00 80 3F 00 00 20 41 00 00 34 42 01 00 00 00 01 00 00 00 00 00 00 00 00 00 00 00 00 00 80 3F 8F C2 35 3F 9A 99 99 3F 00 00 80 3F 00 00 00 00 00 00 80 3F 00 00 80 3F 00 00 80 3F 00 00 00 00 00 00 00 00 00 00 00 3F 00 00 00 00 00 00 00 00 00 00 00 00 00 00 80 3F 00 00 80 3F 00 00 80 3F";
        string replace = "FF FF FF FF FF FF FF FF FF FF FF FF 01 00 00 00 68 00 00 00 94 01 00 00 02 00 00 00 03 00 00 00 CD CC 8C 3F CD CC 4C 3F CD CC 4C 3F 00 00 00 00 00 00 80 3F 00 00 80 3F ?? ?? ?? ?? 00 00 00 00 ?? ?? ?? ?? 20 ?? ?? ?? 00 00 00 00 FF FF FF FF 00 00 00 00 ?? ?? ?? ?? C8 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 05 00 00 00 01 00 00 00 B4 C8 D6 3F 01 00 00 00 B4 C8 D6 3F 00 00 00 00 B4 C8 D6 3F 00 00 80 3F 00 00 80 3F 0A D7 A3 3D 00 00 00 00 00 00 5C 43 00 00 90 42 00 00 B4 42 96 00 00 00 00 00 00 00 EC 51 B8 3D 8F C2 F5 3C 00 00 00 00 04 00 00 00 00 00 80 3F 00 00 20 41 00 00 34 42 01 00 00 00 01 00 00 00 00 00 00 00 00 00 00 00 00 00 80 3F 8F C2 35 3F 9A 99 99 3F 00 00 80 3F 00 00 00 00 00 00 80 3F 00 00 80 3F 00 00 80 3F 00 00 00 00 00 00 00 00 00 00 00 3F 00 00 00 00 00 00 00 00 00 00 00 00 00 00 80 3F 00 00 80 3F 00 00 80 3F";

        bool success = false;
        S1K.OpenProcess("HD-Player");

        IEnumerable<long> results = await S1K.AoBScan(search); // ✅ Writable বাদ দেওয়া হয়েছে
        string[] replaceParts = replace.Split(' ');

        if (results.Any())
        {
            int patchLength = replaceParts.Length;

            foreach (long addr in results)
            {
                byte[] originalMemory = S1K.ReadBytes(addr.ToString("X"), patchLength);
                byte[] finalPatch = ReplaceWithOriginalIfNeeded(replaceParts, originalMemory);
                S1K.WriteBytes(addr.ToString("X"), finalPatch);
            }

            success = true;
        }

        stopwatch.Stop();
        double elapsedSeconds = stopwatch.Elapsed.TotalSeconds;
        Console.Beep(200, 300);
        PID.Text = $"AWM Y Switch Done,ᴛꞮᴍᴇ: {elapsedSeconds:F2} Seconds";
    }

    private static byte[] ReplaceWithOriginalIfNeeded(string[] replaceParts, byte[] original)
    {
        List<byte> result = new List<byte>();
        for (int i = 0; i < replaceParts.Length; i++)
        {
            if (replaceParts[i] == "??")
                result.Add(original[i]);
            else
                result.Add(Convert.ToByte(replaceParts[i], 16));
        }
        return result.ToArray();
    }
}

