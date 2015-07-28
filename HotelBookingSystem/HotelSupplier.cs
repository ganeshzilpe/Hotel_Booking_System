using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace HotelBookingSystem
{
    public delegate void priceChangeEvent(int price, int ID);                               // Delegate for price change event
    public delegate void bookingStatus(Boolean success, OrderClass orderObject, String timeStamp);      // Delegate to inform Travel agency the status of placed order

    class HotelSupplier
    {
        public static CrypticService.ServiceClient client;
        int hotelID;
        public int iterations = 0;
        public HotelSupplier(int ID)    // Constructor
        {
            hotelID = ID;
            client = new CrypticService.ServiceClient();
        }

        OrderClass orderObject = new OrderClass();
        Random random = new Random();

        public static event priceChangeEvent priceChange;
        public static event bookingStatus bStatus;
        public int hotelPrice = 100;                            // Default hotel room price

        public void hotelFunction()                                 // Function to check buffer repeatedly for entries
        {
            do
            {
                Monitor.Enter(MainSystem.mcb);
                try
                {
                    for (int i = 0; i < 2; i++)                 // Check all 2 entries
                    {
                        string encodedString = MainSystem.mcb.getOneCell(i, hotelID);
                        string[] stringTokens = null;
                        if(encodedString != null)
                            stringTokens = encodedString.Split('?');
                        if (stringTokens != null && stringTokens[0] != null)                        // If entry found
                        {
                            orderObject = EncoderDecoder.getDecodedValue(stringTokens[0]);
                            orderObject.setCreditCardNumber((client.Encrypt(orderObject.getCreditCardNumber().ToString())));
                            var t = new Thread(() => orderProcessing(orderObject, orderObject.getRoomPrice(), stringTokens[1]));      // Start processing order
                            Console.WriteLine("\n======================================================================");
                            Console.WriteLine("Order Received for the Hotel :"+  orderObject.getReceiverID());
                            Console.WriteLine("TravelAgency:{0}, RoomPrice:{1}, NumberOfRooms:{2}", orderObject.getSenderID(), orderObject.getRoomPrice(), orderObject.getNumberOfRooms());
                            Console.WriteLine("======================================================================");
                            t.Start();
                            t.Join();
                        }
                    }
                }
                finally
                {
                    Monitor.Exit(MainSystem.mcb);
                }
                Thread.Sleep(1000);
            } while (iterations != 100);
        }


        public void priceFunction()                              
        {
            for (int i = 0; i < 10; i++)                    
            {
                Thread.Sleep(500);
                int newPrice = random.Next(75, 125);           // randomly fluctuate prices within 75 to 125
                hotelPrice = newPrice;
                if (priceChange != null)
                    priceChange(newPrice, hotelID);          
            }
            iterations = 100;
        }

        public static void orderProcessing(OrderClass order, int price, String timeStamp)
        {
            string decryptedCreditCard = HotelSupplier.client.Decrypt(order.getCreditCardNumber().ToString());
            if (TransactionAgency.checkForValidity(decryptedCreditCard))       
            {
                        if (bStatus != null)
                            bStatus(true, order, timeStamp);
                        return;
            }
            else
                Console.WriteLine("Card is not valid");

            bStatus(false, order, timeStamp);
            return;
        }
    }
}
