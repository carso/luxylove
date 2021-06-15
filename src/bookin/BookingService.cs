using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace bookin
{

    public enum PackageDay
    {
        Basic = 7,  Standard = 28, Premium = 42 
    }

    public class BookingService
    {
        luxylovedbEntities _entities;


        private string _username;

        public BookingService(string username)
        {

            _entities = new luxylovedbEntities();
           
            if(!_entities.Customers.Where(p => p.Username == username).Any())
            {
                throw new Exception("Username not exists");
            }
           

            _username = username;
        }


        public void Book(string roomCode, DateTime startDateTime, PackageDay packageDay)
        {

            var room = _entities.Luxy_Room.Where(p => p.RoomCode == roomCode).FirstOrDefault();

            if (room != null)
            {

                DateTime endDateTime = startDateTime.AddDays((int)packageDay).AddHours(12);

                var booking = _entities.Luxy_Booking.Where(p => p.RoomCode == roomCode).Where(x => (startDateTime >= x.StartDateTime && startDateTime <= x.EndDateTime) || (endDateTime >= x.StartDateTime && endDateTime <= x.EndDateTime)).FirstOrDefault();


                if (booking == null)
                {

                    _entities.Luxy_Booking.Add(new Luxy_Booking
                    {
                        BookingGuid = Guid.NewGuid(),
                        CreatedDate = DateTime.Now,
                        IsVoid = false,
                        Username = _username,
                        RoomCode = roomCode,
                        StartDateTime = startDateTime,
                        EndDateTime = endDateTime,
                        Floor = room.Floor,
                        Price = room.Price.Value,
                        ProductId = room.ProductId.Value,
                        RoomNo = room.RoomNo,
                        RoomType = room.RoomType

                    });

                    _entities.SaveChanges();

                }
            }
        }

    }
}
