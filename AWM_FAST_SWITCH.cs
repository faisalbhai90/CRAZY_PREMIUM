using System;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace RemoteMethods
{
    public static class AWM_FAST_SWITCH
    {
        public static async Task Run(object memObject, Label pidLabel, bool isMuted)
        {
            // Casting the memory object to your actual memory class
            dynamic mem = memObject;

            try
            {
                Stopwatch stopwatch = new Stopwatch();
                stopwatch.Start();

                if (!isMuted)
                    Console.Beep(100, 200);

                pidLabel.Text = "ᴀᴘᴘʟʏɪɴɢ AWM SWITCH";

                int procId = Process.GetProcessesByName("HD-Player")[0].Id;
                mem.OpenProcess(procId);

                var result = await mem.AoBScan("0A D7 A3 3D 00 00 00 00 00 00 5C 43 00 00 90 42 00 00 B4 42 96 00 00 00 00 00 00 00 00 00 00 3F 00 00 80 3E 00 00 00 00 04 00 00 00 00 00 80 3F 00 00 20 41 00 00 34 42 01 00 00 00 01 00 00 00 00 00 00 00 00 00 00 00 00 00 80 3F 0A D7 23 3F 9A 99 99 3F 00 00 80 3F 00 00 00 00 00 00 80 3F 00 00 80 3F 00 00 80 3F 00 00 00 00 00 00 00 00 00 00 00 3F 00 00 00 00 00 00 00 00 00 00 00 00 00 00 80 3F 00 00 80 3F 00 00 80 3F 00 00");

                if (result.Count == 0)
                {
                    pidLabel.Text = "No Address Found";
                    return;
                }

                string newBytes = "0A D7 A3 3D 00 00 00 00 00 00 5C 43 00 00 90 42 00 00 B4 42 96 00 00 00 00 00 00 00 EC 51 B8 3D 8F C2 F5 3C 00 00 00 00 04 00 00 00 00 00 80 3F 00 00 20 41 00 00 34 42 01 00 00 00 01 00 00 00 00 00 00 00 00 00 00 00 00 00 80 3F 0A D7 23 3F 9A 99 99 3F 00 00 80 3F 00 00 00 00 00 00 80 3F 00 00 80 3F 00 00 80 3F 00 00 00 00 00 00 00 00 00 00 00 3F 00 00 00 00 00 00 00 00 00 00 00 00 00 00 80 3F 00 00 80 3F 00 00 80 3F 00 00";

                foreach (long addr in result)
                {
                    if (result.Count < 2)
                    {
                        mem.WriteMemory(addr.ToString("X"), "bytes", newBytes, string.Empty, null);
                    }
                }

                stopwatch.Stop();
                double elapsedSeconds = stopwatch.Elapsed.TotalSeconds;

                if (!isMuted)
                    Console.Beep(200, 300);

                pidLabel.Text = $"AWM Switch=ᴏɴ,ᴛꞮᴍᴇ: {elapsedSeconds:F2} Seconds";

                if (result.Count > 2)
                {
                    MessageBox.Show("ᴛʜꞮꜱ ᴄᴏᴅᴇ ɴᴏᴛ ꜱᴀꜰᴇ.", "ᴇƦƦᴏƦ", MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Error in AWM_FAST_SWITCH:\n{ex.Message}");
            }
        }
    }
}

