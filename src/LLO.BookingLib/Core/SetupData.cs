using System;
using System.Collections.Generic;
using System.Text;

namespace LLO.BookingLib.Core
{
    public class SetupData
    {
        private RoomServiceProvider _roomService;
        public SetupData()
        {


        }


        public void Initialize()
        {
            _roomService = new RoomServiceProvider();

            

            //Ground floor
            _roomService.AddRoom(new RoomModel()
            {
                Floor = FloorEnum.Ground,
                RoomCode = "A2",
                RoomType = RoomTypeEnum.PremiumQueen,
                RoomNumber = "0G1"
            });

            _roomService.AddRoom(new RoomModel()
            {
                Floor = FloorEnum.Ground,
                RoomCode = "A1",
                RoomType = RoomTypeEnum.DeluxeRoom,
                RoomNumber = "0G2"
            });


            //1st Floor
            _roomService.AddRoom(new RoomModel()
            {
                Floor = FloorEnum.First,
                RoomCode = "B1",
                RoomType = RoomTypeEnum.VIPSuite,
                RoomNumber = "1F1"
            });


            _roomService.AddRoom(new RoomModel()
            {
                Floor = FloorEnum.First,
                RoomCode = "B2",
                RoomType = RoomTypeEnum.PremiumKing,
                RoomNumber = "1F2"
            });

            _roomService.AddRoom(new RoomModel()
            {
                Floor = FloorEnum.First,
                RoomCode = "B2A",
                RoomType = RoomTypeEnum.DeluxeTwinBed,
                RoomNumber = "1F2"
            });


            _roomService.AddRoom(new RoomModel()
            {
                Floor = FloorEnum.First,
                RoomCode = "B2B",
                RoomType = RoomTypeEnum.DeluxeTwinBed,
                RoomNumber = "1F2"
            });


            _roomService.AddRoom(new RoomModel()
            {
                Floor = FloorEnum.First,
                RoomCode = "B3",
                RoomType = RoomTypeEnum.PremiumKing,
                RoomNumber = "1F3"
            });

            _roomService.AddRoom(new RoomModel()
            {
                Floor = FloorEnum.First,
                RoomCode = "B3A",
                RoomType = RoomTypeEnum.DeluxeTwinBed,
                RoomNumber = "1F3"
            });

            _roomService.AddRoom(new RoomModel()
            {
                Floor = FloorEnum.First,
                RoomCode = "B3B",
                RoomType = RoomTypeEnum.DeluxeTwinBed,
                RoomNumber = "1F3"
            });


            //2nd Floor
            _roomService.AddRoom(new RoomModel()
            {
                Floor = FloorEnum.Second,
                RoomCode = "C1",
                RoomType = RoomTypeEnum.VIPSuite,
                RoomNumber = "2F1"
            });

            _roomService.AddRoom(new RoomModel()
            {
                Floor = FloorEnum.Second,
                RoomCode = "C2",
                RoomType = RoomTypeEnum.PremiumQueen,
                RoomNumber = "2F2"
            });


            _roomService.AddRoom(new RoomModel()
            {
                Floor = FloorEnum.Second,
                RoomCode = "C3",
                RoomType = RoomTypeEnum.PremiumQueen,
                RoomNumber = "2F3"
            });
        }

    }
}
