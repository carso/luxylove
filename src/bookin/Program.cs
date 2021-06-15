using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace bookin
{
    class Program
    {
        static void Main(string[] args)
        {
            InitializeRoomData();
            BookingService bookingService = new BookingService("carso.leong@outlook.com");
            bookingService.Book("A2", new DateTime(2021, 9, 9).AddHours(15), PackageDay.Premium);

       
            Console.WriteLine("Hello World");
            Console.ReadLine();
        }

        private static void InitializeRoomData()
        {
            RoomService roomService = new RoomService();

            //Ground floor
            roomService.AddRoom(new RoomModel()
            {
                Floor = FloorEnum.Ground,
                RoomCode = "A2",
                RoomType = RoomTypeEnum.PremiumQueen,
                RoomNumber = "G1"
            });

            roomService.AddRoom(new RoomModel()
            {
                Floor = FloorEnum.Ground,
                RoomCode = "A1",
                RoomType = RoomTypeEnum.DeluxeRoom,
                RoomNumber = "G2"
            });


            //1st Floor
            roomService.AddRoom(new RoomModel()
            {
                Floor = FloorEnum.First,
                RoomCode = "B1",
                RoomType = RoomTypeEnum.VIPSuite,
                RoomNumber = "1F1"
            });


            roomService.AddRoom(new RoomModel()
            {
                Floor = FloorEnum.First,
                RoomCode = "B2",
                RoomType = RoomTypeEnum.PremiumKing,
                RoomNumber = "1F2"
            });

            roomService.AddRoom(new RoomModel()
            {
                Floor = FloorEnum.First,
                RoomCode = "B2A",
                RoomType = RoomTypeEnum.DeluxeTwinBed,
                RoomNumber = "1F2"
            });


            roomService.AddRoom(new RoomModel()
            {
                Floor = FloorEnum.First,
                RoomCode = "B2B",
                RoomType = RoomTypeEnum.DeluxeTwinBed,
                RoomNumber = "1F2"
            });


            roomService.AddRoom(new RoomModel()
            {
                Floor = FloorEnum.First,
                RoomCode = "B3",
                RoomType = RoomTypeEnum.PremiumKing,
                RoomNumber = "1F3"
            });

            roomService.AddRoom(new RoomModel()
            {
                Floor = FloorEnum.First,
                RoomCode = "B3A",
                RoomType = RoomTypeEnum.DeluxeTwinBed,
                RoomNumber = "1F3"
            });

            roomService.AddRoom(new RoomModel()
            {
                Floor = FloorEnum.First,
                RoomCode = "B3B",
                RoomType = RoomTypeEnum.DeluxeTwinBed,
                RoomNumber = "1F3"
            });


            //2nd Floor
            roomService.AddRoom(new RoomModel()
            {
                Floor = FloorEnum.Second,
                RoomCode = "C1",
                RoomType = RoomTypeEnum.VIPSuite,
                RoomNumber = "2F1"
            });

            roomService.AddRoom(new RoomModel()
            {
                Floor = FloorEnum.Second,
                RoomCode = "C2",
                RoomType = RoomTypeEnum.PremiumQueen,
                RoomNumber = "2F2"
            });


            roomService.AddRoom(new RoomModel()
            {
                Floor = FloorEnum.Second,
                RoomCode = "C3",
                RoomType = RoomTypeEnum.PremiumQueen,
                RoomNumber = "2F3"
            });


        }

    }
}
