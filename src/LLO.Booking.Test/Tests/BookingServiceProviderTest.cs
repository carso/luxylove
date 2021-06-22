using System;
using Xunit;
using LLO.BookingLib;
using LLO.BookingLib.Core;

namespace LLO.Booking.Test
{
    [TestCaseOrderer("LLO.Booking.Test.PriorityOrderer", "LLO.Booking.Test")]
    public class BookingServiceProviderTest : IDisposable
    {
        private BookingServiceProvider _bookingService;
        private RoomServiceProvider _roomService;
        public BookingServiceProviderTest()
        {

            LuxylovedbContext context = new LuxylovedbContext();
            context.LuxyBookings.Clear();
            context.LuxyRooms.Clear();
            context.SaveChanges();

            _bookingService = new BookingServiceProvider();

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



        public void Dispose()
        {
            LuxylovedbContext context = new LuxylovedbContext();
            context.LuxyBookings.Clear();
            context.LuxyRooms.Clear();
            context.SaveChanges();
        }


        [Fact, TestPriority(-1)]
        public void AddRoom_UsernameNotExistException()
        {

            Assert.Throws<UsernameNotExistException>(() =>

_bookingService.Book("C3", new DateTime(2021, 9, 9).AddHours(15), PackageDay.Premium, "carso.leong2@outlook.com")

);
        }

     

        [Fact, TestPriority(1)]
        public void MakeBooking_BookingAdded()
        {
            Guid? guid = _bookingService.Book("C3", new DateTime(2021, 9, 9).AddHours(15), PackageDay.Premium, "carso.leong@outlook.com");
            Assert.True(guid != new Guid());
        }

        [Fact, TestPriority(-1)]
        public void MakeBookingDiffRoom_BookingAdded()
        {
            Guid? guid = _bookingService.Book("C3", new DateTime(2021, 9, 9).AddHours(15), PackageDay.Premium, "carso.leong@outlook.com");
            Guid? guid2 = _bookingService.Book("A2", new DateTime(2021, 9, 9).AddHours(15), PackageDay.Premium, "carso.leong@outlook.com");
            Assert.True(guid2 != new Guid());
        }


        [Fact, TestPriority(2)]
        public void MakeDoubleBooking_NoBookingAdded()
        {
            Guid? guid = _bookingService.Book("C3", new DateTime(2021, 9, 9).AddHours(15), PackageDay.Premium, "carso.leong@outlook.com");
            Guid? guid2 = _bookingService.Book("C3", new DateTime(2021, 9, 11).AddHours(15), PackageDay.Premium, "carso.leong@outlook.com");
            Assert.Null(guid2);
        }


        [Fact, TestPriority(3)]
        public void MakeNextDayBooking_BookingAdded()
        {
            Guid? guid = _bookingService.Book("C3", new DateTime(2021, 9, 9).AddHours(15), PackageDay.Premium, "carso.leong@outlook.com");
            Guid? guid2 = _bookingService.Book("C3", new DateTime(2021, 10, 22).AddHours(15), PackageDay.Premium, "carso.leong@outlook.com");
            Assert.True(guid2 != new Guid());
        }

        [Fact, TestPriority(4)]
        public void MakeNextDayBookingOtherUser_BookingAdded()
        {
            Guid? guid = _bookingService.Book("C3", new DateTime(2021, 9, 9).AddHours(15), PackageDay.Premium, "carso.leong@outlook.com");
            Guid? guid2 = _bookingService.Book("C3", new DateTime(2021, 10, 22).AddHours(15), PackageDay.Premium, "carso.leong1@outlook.com");
            Assert.True(guid2 != new Guid());
        }


        [Fact, TestPriority(5)]
        public void MakeDoubleBookingOtherUser_NoBookingAdded()
        {
            Guid? guid = _bookingService.Book("C3", new DateTime(2021, 9, 9).AddHours(15), PackageDay.Premium, "carso.leong@outlook.com");
            Guid? guid2 = _bookingService.Book("C3", new DateTime(2021, 9, 12).AddHours(15), PackageDay.Premium, "carso.leong1@outlook.com");
                       
            Assert.Null(guid2);
        }

        [Fact, TestPriority(6)]
        public void MakeFirstSharedBookingSameRoom_BookingAdded()
        {
            Guid? guid = _bookingService.Book("B2A", new DateTime(2021, 9, 9).AddHours(15), PackageDay.Premium, "carso.leong@outlook.com");

            Assert.True(guid != new Guid());
        }

        [Fact, TestPriority(7)]
        public void MakeSecondSharedBookingSameRoom_BookingAdded()
        {
            Guid? guid = _bookingService.Book("B2A", new DateTime(2021, 9, 9).AddHours(15), PackageDay.Premium, "carso.leong@outlook.com");
            Guid? guid2 = _bookingService.Book("B2B", new DateTime(2021, 9, 12).AddHours(15), PackageDay.Premium, "carso.leong@outlook.com");

            Assert.True(guid2 != new Guid());
        }

        [Fact, TestPriority(8)]
        public void MakeThirdSharedBookingSameRoom_NoBookingAdded()
        {
            Guid? guid = _bookingService.Book("B2A", new DateTime(2021, 9, 9).AddHours(15), PackageDay.Premium, "carso.leong@outlook.com");
            Guid? guid2 = _bookingService.Book("B2B", new DateTime(2021, 9, 9).AddHours(15), PackageDay.Premium, "carso.leong@outlook.com");
            Guid? guid3 = _bookingService.Book("B2A", new DateTime(2021, 9, 9).AddHours(15), PackageDay.Premium, "carso.leong@outlook.com");

            Assert.Null(guid3);
        }

        [Fact, TestPriority(9)]
        public void MakeThirdSharedBookingDiffDate_BookingAdded()
        {
            Guid? guid = _bookingService.Book("B2A", new DateTime(2021, 9, 9).AddHours(15), PackageDay.Premium, "carso.leong@outlook.com");
            Guid? guid2 = _bookingService.Book("B2B", new DateTime(2021, 9, 9).AddHours(15), PackageDay.Premium, "carso.leong@outlook.com");
            Guid? guid3 = _bookingService.Book("B2A", new DateTime(2021, 10, 22).AddHours(15), PackageDay.Premium, "carso.leong@outlook.com");

            Assert.True(guid3 != new Guid());
        }

        [Fact, TestPriority(10)]
        public void MakeAllSharedBookingDiffRoom_BookingAdded()
        {
            Guid? guid = _bookingService.Book("B2A", new DateTime(2021, 9, 9).AddHours(15), PackageDay.Premium, "carso.leong@outlook.com");
            Guid? guid2 = _bookingService.Book("B2B", new DateTime(2021, 9, 9).AddHours(15), PackageDay.Premium, "carso.leong@outlook.com");
            Guid? guid3 = _bookingService.Book("B3A", new DateTime(2021, 9, 9).AddHours(15), PackageDay.Premium, "carso.leong@outlook.com");
            Guid? guid4 = _bookingService.Book("B3A", new DateTime(2021, 9, 9).AddHours(15), PackageDay.Premium, "carso.leong@outlook.com");

            Assert.True(guid4 != new Guid());
        }

        [Fact, TestPriority(11)]
        public void MakeConvertableBookingInSharedRoom_NoBookingAdded()
        {
            Guid? guid = _bookingService.Book("B2A", new DateTime(2021, 9, 9).AddHours(15), PackageDay.Premium, "carso.leong@outlook.com");
            Guid? guid2 = _bookingService.Book("B2", new DateTime(2021, 9, 9).AddHours(15), PackageDay.Premium, "carso.leong@outlook.com");

            Assert.Null(guid2);
        }

        [Fact, TestPriority(12)]
        public void MakeConvertableBooking_BookingAdded()
        {
            Guid? guid = _bookingService.Book("B2", new DateTime(2021, 9, 9).AddHours(15), PackageDay.Premium, "carso.leong@outlook.com");

            Assert.True(guid != new Guid());
        }

        [Fact, TestPriority(13)]
        public void MakeSharedRoomBookingInConvertableRoom_NoBookingAdded()
        {
            Guid? guid = _bookingService.Book("B2", new DateTime(2021, 9, 9).AddHours(15), PackageDay.Premium, "carso.leong@outlook.com");
            Guid? guid2 = _bookingService.Book("B2A", new DateTime(2021, 9, 9).AddHours(15), PackageDay.Premium, "carso.leong@outlook.com");

            Assert.Null(guid2);
        }

        [Fact, TestPriority(14)]
        public void MakeSharedRoomBookingInConvertableRoomDiffDate_BookingAdded()
        {
            Guid? guid = _bookingService.Book("B2", new DateTime(2021, 9, 9).AddHours(15), PackageDay.Premium, "carso.leong@outlook.com");
            Guid? guid2 = _bookingService.Book("B2A", new DateTime(2021, 10, 22).AddHours(15), PackageDay.Premium, "carso.leong@outlook.com");

            Assert.True(guid2 != new Guid());
        }

        [Fact, TestPriority(15)]
        public void MakeConvertableBookingInSharedRoomDiffDate_BookingAdded()
        {
            Guid? guid = _bookingService.Book("B2A", new DateTime(2021, 9, 9).AddHours(15), PackageDay.Premium, "carso.leong@outlook.com");
            Guid? guid2 = _bookingService.Book("B2", new DateTime(2021, 10, 22).AddHours(15), PackageDay.Premium, "carso.leong@outlook.com");

            Assert.True(guid2 != new Guid());
        }

        [Fact, TestPriority(16)]
        public void MakeTwoConvertableBooking_BookingAdded()
        {
            Guid? guid = _bookingService.Book("B2", new DateTime(2021, 9, 9).AddHours(15), PackageDay.Premium, "carso.leong@outlook.com");
            Guid? guid2 = _bookingService.Book("B3", new DateTime(2021, 9, 9).AddHours(15), PackageDay.Premium, "carso.leong@outlook.com");

            Assert.True(guid2 != new Guid());
        }

        [Theory]
        [InlineData("carso.leong@outlook.com")]
        [InlineData("carso.leong1@outlook.com")]
        public void Simulation1(string username)
        {
            Guid? guid = _bookingService.Book("B2", new DateTime(2021, 9, 9).AddHours(15), PackageDay.Premium, username);
            Guid? guid2 = _bookingService.Book("B3", new DateTime(2021, 9, 9).AddHours(15), PackageDay.Premium, username);
            Guid? guid3 = _bookingService.Book("B2A", new DateTime(2021, 9, 9).AddHours(15), PackageDay.Premium, username);
            Guid? guid4 = _bookingService.Book("B3A", new DateTime(2021, 9, 9).AddHours(15), PackageDay.Premium, username);
            Guid? guid5 = _bookingService.Book("B2B", new DateTime(2021, 9, 9).AddHours(15), PackageDay.Premium, username);
            Guid? guid6 = _bookingService.Book("B3B", new DateTime(2021, 9, 9).AddHours(15), PackageDay.Premium, username);
            Guid? guid7 = _bookingService.Book("B3B", new DateTime(2021, 10, 22).AddHours(15), PackageDay.Premium, username);

            Assert.NotNull(guid); //added
            Assert.NotNull(guid2); //added
            Assert.Null(guid3); //Not added
            Assert.Null(guid4);  //Not added
            Assert.Null(guid5); //Not added
            Assert.Null(guid6); //Not added
            Assert.NotNull(guid7); //added


        }

    }
}
