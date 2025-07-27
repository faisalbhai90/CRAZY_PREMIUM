using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;
using System.Windows.Forms;

public class SNIPER_SCOPE
{
    public static List<long> addrs = new List<long>();

    public static string orig = "FF FF FF FF 08 00 00 00 00 00 60 40 CD CC 8C 3F 8F C2 F5 3C CD CC CC 3D 06 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 80 3F 33 33 13 40 00 00 B0 3F";
    public static string patch = "FF FF FF FF 08 00 00 00 00 00 60 40 CD CC 8C 3F 8F C2 F5 3C CD CC CC 3D 06 00 00 00 00 00 00 3F 00 00 00 00 00 00 00 00 00 00 00 00 00 00 80 3F 33 33 13 40";
    public static string[] proc = { "HD-Player" };

    public static async Task Run(dynamic mem, dynamic PID)
    {
        try
        {
            PID.Text = "🔍 Loading Sniper Scope...";
            var isSet = mem.SetProcess(proc);
            if (!isSet)
            {
                PID.Text = "❌ Process Not Found";
                MessageBox.Show("Sniper Scope Process Not Found!", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }

            var raw = await mem.AoBScan(orig);
            addrs = ((IEnumerable<long>)raw).ToList();

            if (addrs.Count > 0)
            {
                PID.Text = "✅ Sniper Scope Load Success";
                Console.Beep(800, 200);
            }
            else
            {
                PID.Text = "❌ Sniper Scope Failed";
                MessageBox.Show("Sniper Scope Failed X", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }
        catch (Exception ex)
        {
            PID.Text = $"❌ Patch Failed: {ex.Message}";
        }
    }

    public static async Task Toggle(dynamic mem)
    {
        if (addrs.Count == 0) return;
        Scope(mem, true);
        await Task.Delay(74);
        Scope(mem, false);
    }

    public static void Scope(dynamic mem, bool on)
    {
        foreach (var addr in addrs)
            mem.AobReplace(addr, on ? patch : orig);
    }
}
