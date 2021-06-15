using System;
using System.Collections.Generic;
using System.Text;
using System.Linq;

namespace LLO.BookingLib.Core
{
    public enum PackageDay
    {
        Basic = 7, Standard = 28, Premium = 42
    }

    public class UsernameNotExistException : Exception
    {
        public UsernameNotExistException(string message) : base(message)
        {

        }
    }

    public class BookingServiceProvider
    {
        LuxylovedbContext _entities;

        public BookingServiceProvider()
        {
            _entities = new LuxylovedbContext();

        }


        public Guid Book(string roomCode, DateTime startDateTime, PackageDay packageDay, string username)
        {



            if (!_entities.Customers.Where(p => p.Username == username).Any())
            {
                throw new UsernameNotExistException("Username not exists");
            }

            Guid newBookingGuid = new Guid();

            var room = _entities.LuxyRooms.Where(p => p.RoomCode == roomCode).FirstOrDefault();

            if (room != null)
            {

                bool isShared = room.RoomType == "DeluxeTwinBed" ? true : false;

                bool isConvertable = _entities.LuxyRooms.Where(p => p.RoomNo == room.RoomNo).Count() > 1;

                DateTime endDateTime = startDateTime.AddDays((int)packageDay).AddHours(12);


                //shared booking
                if (isShared)
                {
                    var sharedBooking = _entities.LuxyBookings.Where(p => p.RoomNo == room.RoomNo).Where(x => (startDateTime >= x.StartDateTime && startDateTime <= x.EndDateTime) || (endDateTime >= x.StartDateTime && endDateTime <= x.EndDateTime));



                    //add another booking if still a bed left for shared bedroom
                    if (sharedBooking.Count() < 2)
                    {

                        //convertable room exist
                        if (sharedBooking.Where(p => p.IsShared == false).Count() == 1)
                        {
                            return newBookingGuid;
                        }



                        newBookingGuid = Guid.NewGuid();
                        _entities.LuxyBookings.Add(new LuxyBooking
                        {
                            BookingGuid = newBookingGuid,
                            CreatedDate = DateTime.Now,
                            IsVoid = false,
                            Username = username,
                            RoomCode = roomCode,
                            StartDateTime = startDateTime,
                            EndDateTime = endDateTime,
                            Floor = room.Floor,
                            Price = room.Price.Value,
                            ProductId = room.ProductId.Value,
                            RoomNo = room.RoomNo,
                            RoomType = room.RoomType,
                            IsShared = isShared,
                            IsOpen = true //room can be used by default

                        });

                        _entities.SaveChanges();
                    }
                }
                else
                {


                    if (isConvertable)
                    {
                        var sharedBooking = _entities.LuxyBookings.Where(p => p.IsShared && p.RoomNo == room.RoomNo).Where(x => (startDateTime >= x.StartDateTime && startDateTime <= x.EndDateTime) || (endDateTime >= x.StartDateTime && endDateTime <= x.EndDateTime));

                        if (sharedBooking.Count() >= 1)
                        {
                            return newBookingGuid;

                        }
                    }


                    //not shared
                    var booking = _entities.LuxyBookings.Where(p => p.RoomCode == roomCode).Where(x => (startDateTime >= x.StartDateTime && startDateTime <= x.EndDateTime) || (endDateTime >= x.StartDateTime && endDateTime <= x.EndDateTime)).FirstOrDefault();

                    if (booking == null)
                    {
                        newBookingGuid = Guid.NewGuid();
                        _entities.LuxyBookings.Add(new LuxyBooking
                        {
                            BookingGuid = newBookingGuid,
                            CreatedDate = DateTime.Now,
                            IsVoid = false,
                            Username = username,
                            RoomCode = roomCode,
                            StartDateTime = startDateTime,
                            EndDateTime = endDateTime,
                            Floor = room.Floor,
                            Price = room.Price.Value,
                            ProductId = room.ProductId.Value,
                            RoomNo = room.RoomNo,
                            RoomType = room.RoomType,
                            IsShared = isShared,
                            IsOpen = true //room can be used by default

                        });

                        _entities.SaveChanges();

                    }
                }

            }

            return newBookingGuid;
        }


    }
}