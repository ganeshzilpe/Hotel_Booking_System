using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HotelBookingSystem
{
    class TransactionAgency
    {
        // Available credit card information 
        public static string[] validCreditCard = { "123456789", "987654321", "675648398" };
        // Length of credit card number
        public static int validLength = 9;

        public static bool checkForValidity(string cardNumber)
        {
            if (cardNumber.Length != validLength)
            {
                return false;
            }
            for (int i = 0; i < validCreditCard.Length; i++)
            {
                if (validCreditCard[i].Equals(cardNumber))
                {
                    return true;
                }
            }
            return false;
        }

      
    }
}