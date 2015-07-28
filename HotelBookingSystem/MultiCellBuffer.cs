using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Threading;

namespace HotelBookingSystem
{
    class MultiCellBuffer
    {
        //buffer for orders placed for hotels
        public static string[] bufferCells = new string[2]; 
        //tracking of slot in buffer for each hotel
        static bool[] isSlotWritable = new bool[2];

        public void init_bufferCells()                                
        {
            for (int i = 0; i < 2; i++)
            {
                bufferCells[i] = null;
                isSlotWritable[i] = true;
            }
        }

        //add order in buffer
        public void setOneCell(string orderStr)                 
        {
            int i = 0;
            lock (this)
            {
                while (!isSlotWritable[0] && !isSlotWritable[1])    
                {
                    Monitor.Wait(this, 500);
                }
                while (i < 2 && !isSlotWritable[i])                   
                {
                    i++;
                }
                if (i < 2)                                      
                {
                    bufferCells[i] = orderStr;
                    isSlotWritable[i] = false;
                }
                try
                {
                    Monitor.Pulse(this);
                }
                catch
                {
                    Console.WriteLine("Error in monitor.pulse");
                }
            }
        }

        public string getOneCell(int j, int rID)                       
        {
            string retString;
            lock (this)
            {
                while (isSlotWritable[j])
                {
                    try
                    {
                        if (MainSystem.hotelSupplier[rID].iterations == 100)        
                            return null;
                        Monitor.Wait(this, 500);
                    }
                    catch
                    {
                        Console.WriteLine("error in monitor.wait in get one cell");
                    }
                }
                if (bufferCells[j] != null && EncoderDecoder.getDecodedValue(bufferCells[j].Split('?')[0]).getReceiverID() == rID)  // Got the required entry
                {
                    //Console.WriteLine("Deleting from cell {0}", j);
                    retString = bufferCells[j];
                    isSlotWritable[j] = true;
                    try
                    {
                        Monitor.Pulse(this);
                    }
                    catch
                    {
                        Console.WriteLine("error in monitor.pulse");
                    }
                    return retString;
                }
                else                        
                {
                    try
                    {
                        Monitor.Pulse(this);
                    }
                    catch
                    {
                        Console.WriteLine("error in monitor.pulse");
                    }
                    return null;
                }
            }
        }
    }
}
