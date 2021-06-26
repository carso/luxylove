using System;
using Xunit;
using LLO.BookingLib.Core;
using LLO.BookingLib;

namespace LLO.Booking.Test
{
    [TestCaseOrderer("LLO.Booking.Test.PriorityOrderer", "LLO.Booking.Test")]
    public class RoomServiceProviderTest : IDisposable
    {
        private RoomServiceProvider _roomService;

        public RoomServiceProviderTest()
        {
           

            _roomService = new RoomServiceProvider();

            //Ground floor
            _roomService.AddRoom(new RoomModel()
            {
                Floor = FloorEnum.Ground,
                RoomCode = "A2",
                RoomType = RoomTypeEnum.PremiumQueen,
                RoomNumber = "G1"
            });

    
        }




        public void Dispose()
        {
            LuxylovedbContext context = new LuxylovedbContext();
            context.LuxyBookings.Clear();
            context.LuxyRooms.Clear();
            context.SaveChanges();

        }


        [Fact, TestPriority(1)]
        public void AddRoom_RoomNoProductSKUException()
        {

            Assert.Throws<RoomNoProductSKUException>(() =>

              _roomService.AddRoom(new RoomModel()
              {
                  Floor = FloorEnum.Ground,
                  RoomCode = "A2T",
                  RoomType = RoomTypeEnum.PremiumQueen,
                  RoomNumber = "G1"
              })

           );

        }

        [Fact, TestPriority(2)]
        public void AddRoom_RoomExistException()
        {

             Assert.Throws<RoomExistException>(()=>

               _roomService.AddRoom(new RoomModel()
                {
                    Floor = FloorEnum.Ground,
                    RoomCode = "A2",
                    RoomType = RoomTypeEnum.PremiumQueen,
                    RoomNumber = "G1"
                })
            
            );

        }

     
    }
}
