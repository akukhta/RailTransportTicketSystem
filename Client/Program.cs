using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace Client
{
    static class Program
    {
        /// <summary>
        /// Главная точка входа для приложения.
        /// </summary>
        [STAThread]
        static void Main()
        {
            Application.EnableVisualStyles();
            Application.SetCompatibleTextRenderingDefault(false);
            ClientConnection client = new ClientConnection();
            Form2 loginForm = new Form2(ref client);
            User user;
            Application.Run(loginForm);
            user = loginForm.user;

            if (user.isValid)
                Application.Run(new Form1(ref client));
        }
    }
}
