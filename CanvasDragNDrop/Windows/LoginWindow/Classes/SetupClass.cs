using CanvasDragNDrop.UtilityClasses;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CanvasDragNDrop.Windows.LoginWindow.Classes
{
    public static class SetupClass
    {
        /// <summary> Адрес сервера </summary>
        public static string MockAdress
        {
            get { return _mocAdress; }
            set { _mocAdress = value; }
        }
        private static string _mocAdress;

        /// <summary> Порт сервера </summary>
        public static int Port
        {
            get { return _port; }
            set { _port = value; }
        }
        private static int _port;

        /// <summary> логин юзера </summary>
        public static string Login
        {
            get { return _login; }
            set { _login = value; }
        }
        private static string _login;

        /// <summary> Пароль юзера </summary>
        public static string Pass
        {
            get { return _pass; }
            set { _pass = value; }
        }
        private static string _pass;

        /// <summary> вер </summary>
        public static bool Verif
        {
            get { return _verif; }
            set { _verif = value; }
        }
        private static bool _verif;
    }
}
