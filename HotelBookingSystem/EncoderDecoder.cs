using System;
using System.Security.Cryptography;
using System.Text;
using System.Threading;
namespace HotelBookingSystem
{
    class EncoderDecoder
    {

        public static string getEncodedValue(OrderClass OrderClass)
        {
            // Create the concatenated string i.e. encoded string
            string encodedString = OrderClass.getCreditCardNumber() + "#" + OrderClass.getRoomPrice() + "#" + OrderClass.getNumberOfRooms() + "#" + OrderClass.getSenderID() + "#" + OrderClass.getReceiverID();

            return encodedString ;
        }
        public static string Encrypt(String input, String key)
        {
            byte[] inputArray = UTF8Encoding.UTF8.GetBytes(input);
            String result = "";
            try
            {
                TripleDESCryptoServiceProvider tripleDES = new TripleDESCryptoServiceProvider();
                tripleDES.Key = UTF8Encoding.UTF8.GetBytes(key);
                tripleDES.Mode = CipherMode.ECB;
                tripleDES.Padding = PaddingMode.PKCS7;
                ICryptoTransform cTransform = tripleDES.CreateEncryptor();
                byte[] resultArray = cTransform.TransformFinalBlock(inputArray, 0, inputArray.Length);
                tripleDES.Clear();
                result = Convert.ToBase64String(resultArray, 0, resultArray.Length);
            }
            catch (CryptographicException e)
            {
                Console.WriteLine(e.ToString());
            }
            return result;
        }

        public static OrderClass getDecodedValue(string encodedString)
        {
            string mergedString = encodedString;//Decrypt(encodedString, "ABCDEFGHIJKLMNOP");
            string[] tokens = mergedString.Split('#');
            OrderClass OrderClass = new OrderClass();

            if (tokens.Length != 5)    // String is not communicated properly
            {
                Console.WriteLine("Parameter missmatch");
                OrderClass.setCreditCardNumber("-1");
                OrderClass.setRoomPrice(-1);
                OrderClass.setNumberOfRooms(-1);
                OrderClass.setSenderID(-1);
                OrderClass.setReceiverID(-1);
            }
            else
            {
                OrderClass.setCreditCardNumber(tokens[0]);
                OrderClass.setRoomPrice(Int32.Parse(tokens[1]));
                OrderClass.setNumberOfRooms(Int32.Parse(tokens[2]));
                OrderClass.setSenderID(Int32.Parse(tokens[3]));
                OrderClass.setReceiverID(Int32.Parse(tokens[4]));
            }

            return OrderClass;
        }
        public static String Decrypt(String input, string key)
        {
            Byte[] inputArray = Convert.FromBase64String(input);
            TripleDESCryptoServiceProvider tripleDES = new TripleDESCryptoServiceProvider();
            tripleDES.Key = UTF8Encoding.UTF8.GetBytes(key);
            tripleDES.Mode = CipherMode.ECB;
            tripleDES.Padding = PaddingMode.PKCS7;
            ICryptoTransform cTransform = tripleDES.CreateDecryptor();
            Byte[] resultArray = cTransform.TransformFinalBlock(inputArray, 0, inputArray.Length);
            tripleDES.Clear();
            return UTF8Encoding.UTF8.GetString(resultArray);
        }
    }
}
