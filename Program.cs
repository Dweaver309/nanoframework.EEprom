using System;

namespace EEprom.driver
{
    public class Program
    {
        public static void Main()
        {

            String str = "This is an EEprom test for ESP32.";
           
            EEprom24LC256.Write(EEprom24LC256.Address.SecondString, str);
            
           str = EEprom24LC256.Read(EEprom24LC256.Address.SecondString);

            Console.WriteLine(" Returned " + str);

        }
    }
}
