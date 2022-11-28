using System;
using System.Windows.Forms;

namespace JustHR
{
    public static class Program
    {
        [STAThread]
        static void Main()
        {


            using (var game = new Game1())
            {
                /*Form MyGameForm = (Form)Form.FromHandle(game.Window.Handle);
                MyGameForm.Closing += ClosingFunction;
                void ClosingFunction(object sender, System.ComponentModel.CancelEventArgs e)
                {
                    e.Cancel = true; // Cancel the closing event
                    return;
                }*/ //отменяе выход
                game.Run();
            }
        }
    }
}
