using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace archiver
{
    
    public class IVTarchiver
    {
        public static void main()
        {
            BinaryReader ar = new BinaryReader(new FileStream("archive.ivt", FileMode.Open));
            char b = ar.ReadChar();
            char v = ar.ReadChar();
            char t = ar.ReadChar();

            if (!(b == 'i') && (v == 'v') && (t == 't'))
            {
                Console.WriteLine("неправильный формат файла");
            }

            byte codeLength = ar.ReadByte();
            Dictionary<string, char> decodes = new Dictionary<string, char>();

            Console.WriteLine("Всего символов в дереве: " + codeLength.ToString());

            for (int i = 0; i < codeLength; i++)
            {
                char symb = ar.ReadChar();
                byte codeLen = ar.ReadByte();
                string code = "";
                for (int j = 0; j < codeLen; j++)
                {
                    code += ar.ReadChar();
                }
                decodes.Add(code, symb);
            }

            foreach (KeyValuePair<string, char> kv in decodes)
            {
                Console.WriteLine("Символ = " + kv.Value + " code " + kv.Key);
            }

            StreamWriter sw = new StreamWriter("decoder.txt");
            string buffer = "";

            while (ar.BaseStream.Position < ar.BaseStream.Length)
            {
                char c = ar.ReadChar();
                buffer += c;
                if (decodes.ContainsKey(buffer))
                {
                    if (decodes[buffer] == '~')
                        sw.WriteLine();
                    else
                        sw.Write(decodes[buffer]);
                    buffer = "";
                }
            }

            sw.Close();
        }
    }
}
