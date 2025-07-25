using System;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Collections.Generic; // IEnumerable etc.

public static class M82B_Patch
{
    public static async Task Run(dynamic mem, dynamic PID, bool isMuted = false)
    {
        Stopwatch stopwatch = new Stopwatch();
        stopwatch.Start();

        if (!isMuted)
            Console.Beep(100, 200);

        PID.Text = "ᴀᴘᴘʟʏɪɴɢ";

        Int32 proc = Process.GetProcessesByName("HD-Player")[0].Id;
        mem.OpenProcess(proc);

        string searchPattern = "99 3E 00 00 00 00 00 00 5C 43 00 00 28 42 00 00 B4 42 78 00 00 00 00 00 00 00 9A 99 19 3F 00 00 80 3E 00 00 00 00 04 00 00 00 00 00 80 3F 00 00 20 41 00 00 34 42 01 00 00 00 01 00 00 00 00 00 00 00 00 00 00 00 00 00 80 3F 9A 99 19 3F CD CC 8C 3F 00 00 80 3F 00 00 00 00 66 66 66 3F 00 00 80 3F 00 00 80 3F 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 80 3F 00 00 80 3F 00 00 80 3F 00 00 00 00 01 00 00";

        // Run AoBScan and get result as IEnumerable<long>
        var result = await mem.AoBScan(searchPattern);

        // Check if result is enumerable and has matches
        if (result is IEnumerable<long> matches && matches.Any() && matches.Count() < 2)
        {
            foreach (var CurrentAddress in matches)
            {
                Int64 address10thByte = CurrentAddress + 0L;
                Int64 address11thByte = CurrentAddress + 4L;

                mem.WriteMemory(address10thByte.ToString("X"), "bytes",
                    "99 3E 00 00 00 00 00 00 5C 43 00 00 28 42 00 00 B4 42 78 00 00 00 00 00 00 00 9A 99 19 1B 00 00 80 10 00 00 00 00 04 00 00 00 00 00 80 3F 00 00 20 41 00 00 34 42 01 00 00 00 01 00 00 00 00 00 00 00 00 00 00 00 00 00 80 3F 9A 99 19 3F CD CC 8C 3F 00 00 80 3F 00 00 00 00 66 66 66 3F 00 00 80 3F 00 00 80 3F 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 80 3F 00 00 80 3F 00 00 80 3F 00 00 00 00 01 00 00");
            }

            stopwatch.Stop();
            double elapsedSeconds = stopwatch.Elapsed.TotalSeconds;

            if (!isMuted)
                Console.Beep(200, 300);

            PID.Text = $"M82B ꜰᴀꜱᴛ-ꜱᴡɪᴛᴄʜ=ᴏɴ, ᴛɪᴍᴇ: {elapsedSeconds:F2} Seconds";
        }
        else if (result is IEnumerable<long> matches2 && matches2.Count() >= 2)
        {
            MessageBox.Show("THIS CODE IS PATCHED.", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
        }
        else
        {
            PID.Text = "ERROR";

            if (!isMuted)
            {
                Console.Beep(2000, 400);
            }
        }
    }
}
