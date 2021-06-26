using System;
using LLO.BookingLib.Core;
using LLO.BookingLib;
using System.Linq;
using System.Data.Entity;

namespace LLO.Booking.WinService
{
    class Program
    {
        static void Main(string[] args)
        {
            LuxylovedbContext luxylovedbContext = new LuxylovedbContext();
           var x = luxylovedbContext.OrderItems.Where(p => p.OrderId == 82).ToList();


            foreach(var v in x)
            {
                var ddd =  v.Product;
            }


            luxylovedbContext.LuxyRooms.Where(p => p.RoomCode == "B3");


            Console.WriteLine("Hello World!");
        }
    }
}
