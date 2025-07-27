using System;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;
using System.Windows.Forms;

public class M24_Location
{
    public static async Task Run(dynamic mem, dynamic PID)
    {
        try
        {
            Stopwatch stopwatch = new Stopwatch();
            stopwatch.Start();
            Console.Beep(100, 200);
            PID.Text = "ᴀᴘᴘʟʏɪɴɢ ᴍ24 ʟᴏᴄᴀᴛꞮᴏɴ";

            Int32 proc = Process.GetProcessesByName("HD-Player")[0].Id;
            mem.OpenProcess(proc);

            var result = await mem.AoBScan("18 00 00 00 69 00 6E 00 67 00 61 00 6D 00 65 00 2F 00 70 00 69 00 63 00 6B 00 75 00 70 00 2F 00 70 00 69 00 63 00 6B 00 75 00 70 00 5F 00 6D 00 32 00 34 00 00 00 00 00 80 AD 33 92");

            if (result.Count() != 0 && result.Count() < 2)
            {
                foreach (long num in result)
                {
                    mem.WriteMemory(num.ToString("X"), "bytes",
                        "00 00 00 65 00 66 00 66 00 65 00 63 00 74 00 73 00 2F 00 76 00 66 00 78 00 5F 00 69 00 6E 00 67 00 61 00 6D 00 65 00 5F 00 6C 00 61 00 73 00 65 00 72 00 5F 00 72 00 65 00 64 00",
                        string.Empty, null);
                }

                stopwatch.Stop();
                double elapsedSeconds = stopwatch.Elapsed.TotalSeconds;
                Console.Beep(200, 300);
                PID.Text = $"ᴍ24-ʟᴏᴄᴀᴛꞮᴏɴ=ᴏɴ,ᴛꞮᴍᴇ: {elapsedSeconds:F2} Seconds";
            }
            else
            {
                PID.Text = "❌ M24 value not found or too many results.";
                if (result.Count() > 2)
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
