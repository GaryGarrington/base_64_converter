using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.IO;

namespace base64_converter_pokus4
{
    class Program
    {
        static char[] input = new char[4];
        static byte[] input_byte = new byte[4];
        static byte[] output = new byte[3];

        static byte Char_to_byte(char znak)
        {
            List<char> znaky = new List<char>
            {
                'A', 'B', 'C', 'D', 'E', 'F', 'G', 'H', 'I', 'J', 'K', 'L', 'M', 'N', 'O', 'P', 'Q', 'R', 'S', 'T', 'U', 'V', 'W', 'X', 'Y', 'Z',
                'a', 'b', 'c', 'd', 'e', 'f', 'g', 'h', 'i', 'j', 'k', 'l', 'm', 'n', 'o', 'p', 'q', 'r', 's', 't', 'u', 'v', 'w', 'x', 'y', 'z',
                '0', '1', '2', '3', '4', '5', '6', '7', '8', '9', '+', '/'
            };

                for (int i = 0; i < znaky.Count; i++)
                {
                    if (znak == znaky[i])
                    {
                        return (byte)i;
                    }
                }
            return (byte)128;
        }

        static void Prevod()
        {
            for (int i = 0; i < input.Length; i++)
            {
                input_byte[i] = Char_to_byte(input[i]);
            }

            byte zapis;
            zapis = (byte)(((input_byte[0] & 0b00111111) << 2) + ((input_byte[1] & 0b00110000) >> 4));
            output[0] = zapis;
            zapis = (byte)(((input_byte[1] & 0b00001111) << 4) + ((input_byte[2] & 0b00111100) >> 2));
            output[1] = zapis;
            zapis = (byte)(((input_byte[2] & 0b00000011) << 6) + (input_byte[3] & 0b00111111));
            output[2] = zapis;
        }

        static void Main(string[] args)
        {


            FileStream RStream = new FileStream("Obrazek.b64", FileMode.Open, FileAccess.Read);
            FileStream WStream = new FileStream("Vystup.jpg", FileMode.Create, FileAccess.Write);
            BinaryReader Reader = new BinaryReader(RStream);
            BinaryWriter Writer = new BinaryWriter(WStream);

            Console.WriteLine("Decoding...");
            while(RStream.Position < RStream.Length)
            {
                for(int i = 0; i < 4; i++)
                {
                    if (RStream.Position < RStream.Length)
                    {
                        input[i] = Reader.ReadChar();
                        if (Char_to_byte(input[i]) == 128) i--;
                    }
                    else if (i != 3) input[3] = '*';
                }
                if (input[3] != '*') Prevod();
                else break;
                for (int i = 0; i < output.Length; i++)
                {
                    Writer.Write(output[i]);
                }
            }
            Reader.Close();
            Writer.Close();
            Console.WriteLine("Done");
            Console.ReadKey();
        }
    }
}
