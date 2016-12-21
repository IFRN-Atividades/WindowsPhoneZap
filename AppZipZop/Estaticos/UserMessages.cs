using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AppZipZop.Estaticos
{
    public class UserMessages
    {
        private static List<string> mensagens;

        public static void AddMensagem(string m)
        {
            mensagens.Add(m);
        }

        public static List<string> getMensagens()
        {
            return mensagens;
        }
    }
}
