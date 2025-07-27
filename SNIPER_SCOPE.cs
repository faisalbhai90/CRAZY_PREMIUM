using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;
using System.Windows.Forms;
using Gma.System.MouseKeyHook;

public class SNIPER_SCOPE
{
    public static meowSniper memo = new meowSniper();
    public static List<long> addrs = new List<long>();
    public static string orig = "FF FF FF FF 08 00 00 00 00 00 60 40 CD CC 8C 3F 8F C2 F5 3C CD CC CC 3D 06 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 80 3F 33 33 13 40 00 00 B0 3F";
    public static string patch = "FF FF FF FF 08 00 00 00 00 00 60 40 CD CC 8C 3F 8F C2 F5 3C CD CC CC 3D 06 00 00 00 00 00 00 3F 00 00 00 00 00 00 00 00 00 00 00 00 00 00 80 3F 33 33 13 40";
    public static string[] proc = { "HD-Player" };
    public static IKeyboardMouseEvents hook;
    public static Keys key = Keys.None;
    public static MouseButtons btn = MouseButtons.None;
    public static int wheel = 0;
    public static DateTime last = DateTime.MinValue;
    public static bool listen = false;
    public const int delay = 74, cooldown = 200;

    public static async Task Run(dynamic mem, dynamic PID, dynamic ButtonLabel)
    {
        hook = Hook.GlobalEvents();

        hook.KeyDown += async (a, b) =>
        {
            if (listen)
            {
                key = b.KeyCode;
                btn = MouseButtons.None;
                wheel = 0;
                PID.Text = $"Key: {key}";
                ButtonLabel.Text = key.ToString();
                listen = false;
            }
            else if (b.KeyCode == key)
            {
                await Toggle(mem);
            }
        };

        hook.MouseDown += async (a, b) =>
        {
            if (listen)
            {
                btn = b.Button;
                key = Keys.None;
                wheel = 0;
                PID.Text = $"Mouse: {btn}";
                ButtonLabel.Text = btn.ToString();
                listen = false;
            }
            else if (b.Button == btn)
            {
                await Toggle(mem);
            }
        };

        hook.MouseWheel += async (a, b) =>
        {
            if (listen)
            {
                wheel = b.Delta > 0 ? 1 : -1;
                key = Keys.None;
                btn = MouseButtons.None;
                PID.Text = wheel == 1 ? "Wheel UP" : "Wheel DOWN";
                ButtonLabel.Text = PID.Text;
                listen = false;
            }
            else if ((wheel == 1 && b.Delta > 0) || (wheel == -1 && b.Delta < 0))
            {
                await Toggle(mem);
            }
        };

        if (!memo.SetProcess(proc)) return;
        var r = await memo.AoBScan(orig);
        addrs = ((IEnumerable<long>)r).ToList();
        PID.Text = addrs.Count > 0 ? "Sniper Scope Load Success" : "Sniper Scope Failed";
        if (addrs.Count == 0)
        {
            MessageBox.Show("Sniper Scope Failed X", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
        }
    }

    public static void Scope(dynamic mem, bool on)
    {
        foreach (var a in addrs)
        {
            memo.AobReplace(a, on ? patch : orig);
        }
    }

    public static async Task Toggle(dynamic mem)
    {
        if ((DateTime.Now - last).TotalMilliseconds < cooldown || addrs.Count == 0) return;
        last = DateTime.Now;
        Scope(mem, true);
        await Task.Delay(delay);
        Scope(mem, false);
    }

    public static void Listen(dynamic ButtonLabel)
    {
        listen = true;
        ButtonLabel.Text = "Press a Key...";
    }
}
