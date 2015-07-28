using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using System.Threading;

namespace HotelBookingSystem
{
    class TravelAgency
    {
        int[] hotel = { 1, 2};
        int[] price = { 90, 90, 90 };
        int tID = 0;

        public TravelAgency(int i)                       
        {
            tID = i;
        }
        /*
         * Generate and place orders from TravelAgency Randomly
         */
        public void agency()                            
        {
            for (int i = 0; i < 2; i++)
            {
                Random random = new Random();
                int r = random.Next(0, 2);

                OrderClass order = new OrderClass();
                order.setCreditCardNumber(TransactionAgency.validCreditCard[random.Next(0, 2)]);
                order.setRoomPrice(price[r]);
                order.setNumberOfRooms(random.Next(1, 5));
                order.setSenderID(tID);
                order.setReceiverID(r);
                string value = EncoderDecoder.getEncodedValue(order);
                lock (this)
                {
                    Monitor.Enter(MainSystem.mcb);
                    try
                    {
                        value = value +"?"+ DateTime.Now.Ticks; //append timestamp to order object
                        MainSystem.mcb.setOneCell(value);
                        Console.WriteLine("\n-----------------------------------------------------------------------");
                        Console.WriteLine("TravelAgency {0}=>\nOrder Details: \n\tRoomPrice:{1} \n\tNumberOfRooms:{2}", order.getSenderID(), order.getRoomPrice(), order.getNumberOfRooms());
                        Console.WriteLine("Order is initiated successfully for Hotel: " + order.getReceiverID());
                        Console.WriteLine("-----------------------------------------------------------------------");
                        MainSystem.placed++;
                    }
                    finally
                    {
                        Monitor.Exit(MainSystem.mcb);
                    }
                }
                Thread.Sleep(600);
            }
        }
        /*
         * This event handler governs change in hotel room price
         */
        public void changeInHotelRoomPrice(int pr, int hID)      
        {
            if (tID == 0)
                Console.WriteLine("\nRoom Price changed at Hotel {0} : {1}", hID, pr);
            price[hID] = pr;
        }
        /*
         * This event handler checks the order status
         */
        public void RoomStatus(Boolean success, OrderClass oObject, String timeStamp)            
        {
            DateTime dt1 = new DateTime(Convert.ToInt64(timeStamp));
            DateTime dt2 = DateTime.Now;
            if (oObject.getSenderID() == tID && success)
            {
                Console.WriteLine("\n++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++");
                Console.WriteLine("Room Booking is done successfully at HotelID:{0}", oObject.getReceiverID());
                Console.WriteLine("Order Details: \n\tTravelAgency:{0} \n\tRoomPrice:{1} \n\tNumberOfRooms:{2}", oObject.getSenderID(), oObject.getRoomPrice(), oObject.getNumberOfRooms());
                Console.WriteLine("Total Cost: $" + (oObject.getRoomPrice() * oObject.getNumberOfRooms() + 0.12 * (oObject.getRoomPrice() * oObject.getNumberOfRooms())));
                Console.WriteLine("Time span for completion of the order:" + dt2.Subtract(dt1));
                Console.WriteLine("++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++");
                MainSystem.notified++;
            }
            if (oObject.getSenderID() == tID && !success)
            {
                Console.WriteLine("\n\t\tRoom not booked at Hotel: " + oObject.getReceiverID());
                Console.WriteLine("\t\tTravelAgency:{0}, UnitPrice:{1}, NumberOfUnit:{2}\n", oObject.getSenderID(), oObject.getRoomPrice(), oObject.getNumberOfRooms());
                MainSystem.notified++;
            }
        }
    }
}
