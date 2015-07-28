using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HotelBookingSystem
{
    public class OrderClass
    {
        private int senderID;
        private string cardNo;
        private int roomPrice;
        private int numberOfRooms;
        private int receiverID;

        public OrderClass()
        {
        }

        public void setCreditCardNumber(string temp) 
        {
            cardNo = temp; 
        }
        public string getCreditCardNumber() 
        { 
            return cardNo; 
        }

        public void setRoomPrice(int temp) 
        {
            roomPrice = temp; 
        }
        public int getRoomPrice() 
        { 
            return roomPrice; 
        }

        public void setNumberOfRooms(int temp) 
        {
            numberOfRooms = temp;
        }
        public int getNumberOfRooms() 
        { 
            return numberOfRooms; 
        }

        public void setSenderID(int temp) 
        {
            senderID = temp;
        }
        public int getSenderID() 
        { 
            return senderID; 
        }

        public void setReceiverID(int temp) 
        {
            receiverID = temp; 
        }
        public int getReceiverID() 
        { 
            return receiverID; 
        }

    }
}

