using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Threading;

namespace HotelBookingSystem
{
    class MainSystem
    {
        public static MultiCellBuffer mcb = new MultiCellBuffer();
        public static int placed = 0, notified = 0;                 // To count total placed and executed orders
        public static HotelSupplier[] hotelSupplier = new HotelSupplier[2];
        static void Main(string[] args)
        {
            mcb.init_bufferCells();

            //2 hotels
            hotelSupplier[0] = new HotelSupplier(0);
            hotelSupplier[1] = new HotelSupplier(1);

            //5 travel agencies
            TravelAgency[] travelAgency = new TravelAgency[5];
            for (int i = 0; i < 5; i++)
            {
                travelAgency[i] = new TravelAgency(i);
            }

            HotelSupplier.priceChange += new priceChangeEvent(travelAgency[0].changeInHotelRoomPrice);
            HotelSupplier.priceChange += new priceChangeEvent(travelAgency[1].changeInHotelRoomPrice);
            HotelSupplier.priceChange += new priceChangeEvent(travelAgency[2].changeInHotelRoomPrice);
            HotelSupplier.priceChange += new priceChangeEvent(travelAgency[3].changeInHotelRoomPrice);
            HotelSupplier.priceChange += new priceChangeEvent(travelAgency[4].changeInHotelRoomPrice);

            HotelSupplier.bStatus += new bookingStatus(travelAgency[0].RoomStatus);
            HotelSupplier.bStatus += new bookingStatus(travelAgency[1].RoomStatus);
            HotelSupplier.bStatus += new bookingStatus(travelAgency[2].RoomStatus);
            HotelSupplier.bStatus += new bookingStatus(travelAgency[3].RoomStatus);
            HotelSupplier.bStatus += new bookingStatus(travelAgency[4].RoomStatus);

            Thread hotelSupplier1T1 = new Thread(hotelSupplier[0].hotelFunction); hotelSupplier1T1.Start();
            Thread hotelSupplier1T2 = new Thread(hotelSupplier[0].priceFunction); hotelSupplier1T2.Start();

            Thread hotelSupplier2T1 = new Thread(hotelSupplier[1].hotelFunction); hotelSupplier2T1.Start();
            Thread hotelSupplier2T2 = new Thread(hotelSupplier[1].priceFunction); hotelSupplier2T2.Start();

            Thread[] t = new Thread[5];
            for (int i = 0; i < 5; i++)
            {
                t[i] = new Thread(travelAgency[i].agency);
                t[i].Start();
            }

            // Wait for every thread to finish
            hotelSupplier1T1.Join();
            hotelSupplier2T1.Join();

            hotelSupplier1T2.Join();
            hotelSupplier2T2.Join();

            for (int i = 0; i < 5; i++)
            {
                t[i].Join();
            }
            Console.WriteLine("-------------------------------------------------------------------------");
            Console.WriteLine("Result:");
            Console.WriteLine("Total Orders placed by Travel Agencies: {0}\nTotal Orders processed succesfully by Hotel Suppliers: {1}", placed, notified);
            Console.WriteLine("-------------------------------------------------------------------------");
            Console.ReadKey();
        }
    }
}
