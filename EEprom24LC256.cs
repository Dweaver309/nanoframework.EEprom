
//**************
// I2C1 
//Pin 18 Data 
//Pin 19 Clock

// 12C2
//Pin 25 Data
//Pin 26 Clock
//*************

using System;
using Windows.Devices.I2c;
using System.Threading;

public class EEprom24LC256
{
    private static I2cDevice EEprom = I2cDevice.FromId("I2C1", new I2cConnectionSettings(0x54) { BusSpeed = I2cBusSpeed.StandardMode });
   
    /// <summary>
    /// Structure to put addresses to write to and read from
    /// </summary>
    public struct Address
    {
        public static int FirstString = 64;
        public static int SecondString = 128;
        public static int ThirdString = 192;
    }
 
    /// <summary>
    /// Write string to address
    /// </summary>
    public static void Write(int address, string str)
    {
        try
        {
            Thread.Sleep(5);
                  
            string strlength = String.Empty;

        if (str.Length < 10)
        { 
            strlength = "0" + str.Length.ToString();
         }
        else
        {
            strlength = str.Length.ToString();
        }
        
            // The addess in buffer 0 and 1
            // Length in buffer 3 and 4
            str = "00" + strlength + str;

            Console.WriteLine("String to encode: " + str);

            byte[] buffer = System.Text.Encoding.UTF8.GetBytes(str);

            // Set Write address as first 2 bytes, High + low 
            buffer[0] = (Byte)(address >> 8);

            buffer[1] = (Byte)(address & 0xFF);
          
            EEprom.Write(buffer);

            // Give thread time to write 
            Thread.Sleep(20);

        }

        catch (Exception)
        {

            Console.WriteLine("Error writing to eeprom");
            
        }

    }

    /// <summary>
    /// Read data lengh bytes  
    /// Return the saved string
    // </summary>
    public static String Read(int address, int datalength = 64)
    {
        try
        { 

            var Data = new byte[datalength];
          
            // Clear the write buffer
            EEprom.Write(null);
            
            EEprom.Write(new[] { (Byte)(address >> 8), (Byte)(address & 0xFF) });

            EEprom.Read(Data);

            //Get the first char in the two byte length
            char flb = Convert.ToChar(Data[0]);

            Console.WriteLine("First length char " + flb.ToString());

            // Get the second char in length
            char slb = Convert.ToChar(Data[1]);

            Console.WriteLine("Second length char " + slb.ToString());

            //make the string
            string sl = flb.ToString() + slb.ToString();

            //Convert to integer
            int length = Convert.ToInt32(sl);


            // Bad read avoid exception
        if (length > datalength)
        {
             length = datalength - 2;
        }
       
            Console.WriteLine("Length " + length);

            string rs = string.Empty;

        //Start reading after the two byte saved length
        for (int i = 2; i < length + 2; i++)
        {

            char c = Convert.ToChar(Data[i]);

            rs = rs + c.ToString();

            Console.WriteLine("Read Adddress  " + address + " Char Read " + c.ToString());

            address += 1;

        }

        return rs;

        }

        catch (Exception)
        {

            return "Error reading eeprom";

        }

    }

    }

