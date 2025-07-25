using System;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Collections.Generic;

public class M82B_Location
{
    public static async Task Run(dynamic mem, dynamic PID)
    {
        try
        {
            Stopwatch stopwatch = new Stopwatch();
            stopwatch.Start();
            Console.Beep(100, 200);
            PID.Text = "ᴀᴘᴘʟʏɪɴɢ ᴍ82ʙ ʟᴏᴄᴀᴛꞮᴏɴ";

            Int32 proc = Process.GetProcessesByName("HD-Player")[0].Id;
            mem.OpenProcess(proc);

            var rawResult = await mem.AoBScan("19 00 00 00 69 00 6e 00 67 00 61 00 6d 00 65 00 2f 00 70 00 69 00 63 00 6b 00 75 00 70 00 2f 00 70 00 69 00 63 00 6b 00 75 00 70 00 5f 00 62 00 6d 00 39 00 34 00 00 00");
            var result = ((IEnumerable<long>)rawResult).ToList();  // ✅ Type casting & List conversion

            if (result.Count != 0 && result.Count < 2)
            {
                foreach (long num in result)
                {
                    mem.WriteMemory(num.ToString("X"), "bytes",
                        "19 00 00 00 65 00 66 00 66 00 65 00 63 00 74 00 73 00 2f 00 76 00 66 00 78 00 5f 00 69 00 6e 00 67 00 61 00 6d 00 65 00 5f 00 6c 00 61 00 73 00 65 00 72 00 00 00 00 00",
                        string.Empty, null);
                }

                stopwatch.Stop();
                double elapsedSeconds = stopwatch.Elapsed.TotalSeconds;
                Console.Beep(200, 300);
                PID.Text = $"ᴍ82ʙ-ʟᴏᴄᴀᴛꞮᴏɴ=ᴏɴ,ᴛꞮᴍᴇ: {elapsedSeconds:F2} Seconds";
            }
            else
            {
                PID.Text = "❌ M82B value not found or too many results.";

                if (result.Count > 2)
                {
                    MessageBox.Show("ᴛʜꞮꜱ ᴄᴏᴅᴇ ɴᴏᴛ ꜱᴀꜰᴇ.", "ᴇƦƦᴏƦ", MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
            }
        }
        catch (Exception ex)
        {
            PID.Text = $"Patch Failed: {ex.Message}";
        }
    }
}
await M82B_Location.Run(mem, PID);

