using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.IO;

namespace MegaDesk_6_JonesCrossley
{
    public class DeskQuote
    {

        public string CustomerName { get; set; }
        public RushDays RushOrderDays { get; set; }

        public Desk Desk = new Desk();
        private const decimal BASE_PRICE = 200;
        private const decimal PER_DRAWER_PRICE = 50;
        private int[,] rushOrderPrice;


        public decimal QuoteAmount { get; set; }
        public DateTime QuoteDate { get; set; }
        //public string Material { get => Material; set => Material = value; }

        public const int MIN_WIDTH = 24;
        public const int MAX_WIDTH = 96;
        public const int MIN_DEPTH = 12;
        public const int MAX_DEPTH = 48;
        public const int MIN_DRAWERS = 0;
        public const int MAX_DRAWERS = 7;

        public enum RushDays
        {
            None = 0,
            Three = 3,
            Five = 5,
            Seven = 7
        }

        public void CalculateDeskQuote()
        {
            // Get Rush Order prices from file
            string rushPriceLoadResponse = "";
            if (!GetRushOrder(ref rushPriceLoadResponse))
            {
                throw new Exception(rushPriceLoadResponse);
            }

            // Updates the QuoteAmount and QuoteDate
            QuoteAmount = BASE_PRICE + LargeDeskPrice() + DrawerPrice() + (int)Desk.Surface + RushOrderPrice();
            QuoteDate = DateTime.Now;
        }

        private decimal DrawerPrice()
        {
            // Returns cost of all drawers
            return PER_DRAWER_PRICE * Desk.DrawerCount;
        }

        private decimal LargeDeskPrice()
        {
            // Returns cost of large desks > 1000 sq in.
            if (Desk.GetSurfaceArea() > 1000)
            {
                return Desk.GetSurfaceArea() - 1000;
            }
            else
                return 0;
        }

        private bool GetRushOrder(ref string msgResponse)
        {
            // Retrieve rush order amounts from a file into a 3x3 2 dimensional array
            try
            {
                //int[,] rushOrder = new int[,]
                //{
                //    { 60,70,80 }
                //    ,{ 40,50,60 }
                //    ,{ 30,35,40 }
                //};

                int[,] rushOrder = new int[3, 3];
                string[] readPrices = File.ReadAllLines(App.RUSH_ORDER_PRICE_FILE);

                if (!(readPrices.Length == 9))
                {
                    msgResponse = "Rush Order Prices file expected to contain 9 entries, but found " + readPrices.Length.ToString() + ".";
                    return false;
                }

                // Uses nested loops.
                int priceEntry = 0;
                for (int i = 0; i <= 2; i++)
                {
                    for (int j = 0; j <= 2; j++, priceEntry++)
                    {
                        rushOrder[i, j] = Convert.ToInt32(readPrices[priceEntry]);
                    }
                }

                //// Uses one loop through the raw array.
                //int i = 0;
                //int j = 0;
                //foreach (string s in readPrices)
                //{
                //    if (j > 2)
                //    {
                //        // reset for next group of 3 (next row, first column)
                //        i++;
                //        j = 0;
                //    }

                //    if (j <= 2)
                //    {
                //        rushOrder[i, j] = Convert.ToInt32(s);
                //        // advance to next column
                //        j++;
                //    }
                //}

                rushOrderPrice = rushOrder;
                return true;
            }
            catch (Exception e)
            {
                msgResponse = e.Message;
                return false;
            }
        }

        private decimal RushOrderPrice()
        {

            if (RushOrderDays == RushDays.None)
                return 0;

            int row = 0;
            int column = 0;

            // Determine which row of the array to look at for number of days
            switch (RushOrderDays)
            {
                case RushDays.Three:
                    row = 0;
                    break;
                case RushDays.Five:
                    row = 1;
                    break;
                case RushDays.Seven:
                    row = 2;
                    break;
            }

            // Determine which column of the array to look at for the price
            if (Desk.GetSurfaceArea() < 1000)
                column = 0;
            else if (Desk.GetSurfaceArea() >= 1000 && Desk.GetSurfaceArea() <= 2000)
                column = 1;
            else
                column = 2;

            // Using row and column variables, pick out the correct price
            return rushOrderPrice[row, column];
        }

        public static implicit operator List<object>(DeskQuote v)
        {
            throw new NotImplementedException();
        }
    }
}
