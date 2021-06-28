﻿using System;
using System.Collections.Generic;
using System.Text;
using System.Linq;
using Microsoft.EntityFrameworkCore.Infrastructure;

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

    public class BookingModel
    {
        public DateTime StartDateTime { get; set; }

        public DateTime EndDateTime { get; set; }

        public string RoomCode { get; set; }

        public string RoomNo { get; set; }

        public string CustomerUserName { get; set; }

        public string CustomerName { get; set; }

        public string Phone { get; set; }

        public decimal PackagePrice { get; set; }

        public decimal PricePaid { get; set; }

        public int? OrderId { get; set; }

        public decimal Discount { get; set; }

        public string RoomType { get; set; }

        public string Floor { get; set; }

    }

    public class BookingServiceProvider
    {
        LuxylovedbContext _entities;

        public BookingServiceProvider()
        {
            _entities = new LuxylovedbContext();

        }


        public List<BookingModel> GetActiveBookings()
        {
            var bookings = _entities.LuxyBookings.Where(p => p.IsVoid == false && p.IsOpen == true);

            List<BookingModel> activeBookings = new List<BookingModel>();



            foreach (var x in bookings)
            {
                activeBookings.Add(new BookingModel() { EndDateTime = x.EndDateTime, StartDateTime = x.StartDateTime, CustomerUserName = x.Username, RoomCode = x.RoomCode, RoomNo = x.RoomNo, CustomerName = x.CustomerName, Phone = x.PhoneNumber, PackagePrice = x.Price, PricePaid = x.Price - x.DiscountPrice.Value, OrderId = x.OrderId });
            }

            return activeBookings;
        }

        public List<BookingModel> GetActiveBookingByRoomCode(string roomCode)
        {
            var bookings = _entities.LuxyBookings.Where(p => p.IsVoid == false && p.IsOpen == true && p.RoomCode == roomCode).ToList();

            var room = _entities.LuxyRooms.Where(p => p.RoomCode == roomCode).FirstOrDefault();

            bool isConvertable = _entities.LuxyRooms.Where(p => p.RoomNo == room.RoomNo).Count() > 1;

            bool isShared = room.RoomType == "DeluxeTwinBed" ? true : false;


            if (isConvertable)
            {              

                if (isShared)
                {
                    var nonSharebookings = _entities.LuxyBookings.Where(p => p.IsVoid == false && p.IsOpen == true && p.IsShared == false && p.RoomNo == room.RoomNo).ToList();

                    bookings.AddRange(nonSharebookings);
                }
                else
                {
                    //Non shared, customer occupied the whole room
                    bookings = _entities.LuxyBookings.Where(p => p.IsVoid == false && p.IsOpen == true && p.RoomNo == room.RoomNo).ToList();
                }
            }

            List<BookingModel> activeBookings = new List<BookingModel>();



            foreach (var x in bookings)
            {
                activeBookings.Add(new BookingModel() { EndDateTime = x.EndDateTime, StartDateTime = x.StartDateTime, CustomerUserName = x.Username, RoomCode = x.RoomCode, RoomNo = x.RoomNo, CustomerName = x.CustomerName, Phone = x.PhoneNumber, PackagePrice = x.Price, PricePaid = x.Price - x.DiscountPrice.Value, OrderId = x.OrderId });
            }

            return activeBookings;
        }


        public List<BookingModel> GetActiveBookingByRoomCodeAvailable(string roomCode, int days)
        {
            List<BookingModel> activeBookings = new List<BookingModel>();

            var bookings = _entities.LuxyBookings.Where(p => p.IsVoid == false && p.IsOpen == true && p.RoomCode == roomCode).OrderBy(p=>p.StartDateTime).ToList();


            var room = _entities.LuxyRooms.Where(p => p.RoomCode == roomCode).FirstOrDefault();

            bool isConvertable = _entities.LuxyRooms.Where(p => p.RoomNo == room.RoomNo).Count() > 1;

            bool isShared = room.RoomType == "DeluxeTwinBed" ? true : false;


            if (isConvertable)
            {              


                if (isShared)
                {
                    var nonSharebookings = _entities.LuxyBookings.Where(p => p.IsVoid == false && p.IsOpen == true && p.IsShared == false && p.RoomNo == room.RoomNo).ToList();

                    bookings.AddRange(nonSharebookings);

                    bookings = bookings.OrderBy(p => p.StartDateTime).ToList();
                }
                else
                {
                    //Non shared, customer occupied the whole room
                    bookings = _entities.LuxyBookings.Where(p => p.IsVoid == false && p.IsOpen == true && p.RoomNo == room.RoomNo).OrderBy(p => p.StartDateTime).ToList();
                }

            }


            if (bookings.Count() == 0)
            {
                BookingModel defaultbookingModel = new BookingModel();
                defaultbookingModel.StartDateTime = DateTime.Today;
                defaultbookingModel.EndDateTime = DateTime.Today.AddYears(2);

                activeBookings.Add(defaultbookingModel);
                return activeBookings;
            }

           
            for (int i = 0; i < bookings.Count()-1; i++)
            {
                if (bookings[i + 1].StartDateTime.Subtract(bookings[i].EndDateTime).Days > days)
                {

                    BookingModel bookingModel = new BookingModel();
                    bookingModel.StartDateTime = bookings[i].EndDateTime;
                    bookingModel.EndDateTime = bookings[i + 1].StartDateTime;

                    activeBookings.Add(bookingModel);

                }
            }


            if (bookings[0].StartDateTime.Subtract(DateTime.Today).Days > days)
            {

                BookingModel bookingModel = new BookingModel();
                bookingModel.StartDateTime = DateTime.Now;
                bookingModel.EndDateTime = bookings[0].StartDateTime;

                activeBookings.Add(bookingModel);

            }



            //add another 2 years active
            BookingModel endbookingModel = new BookingModel();
            endbookingModel.StartDateTime = bookings[bookings.Count - 1].EndDateTime;
            endbookingModel.EndDateTime = endbookingModel.StartDateTime.AddYears(2);
            activeBookings.Add(endbookingModel);
          


            return activeBookings;
        }

        public bool CanBook(int orderId)
        {
            List<string> roomSkus = new List<string>(new string[] { "A1", "A2", "B1", "B2", "B2A", "B2B", "B3", "B3A", "B3B", "C1", "C2", "C3" });

            var orderItems = _entities.OrderItems.Where(p => p.OrderId == orderId && roomSkus.Contains(p.Product.Sku));

            return orderItems.Any();
        }


        public bool IsRoomProduct(string sku)
        {
            List<string> roomSkus = new List<string>(new string[] { "A1", "A2", "B1", "B2", "B2A", "B2B", "B3", "B3A", "B3B", "C1", "C2", "C3" });

            if (roomSkus.Contains(sku))
            {
                return true;
            }

            return false;
        }

        public Guid? GetcancellationBookingGuid(int orderId)
        {
            List<string> roomSkus = new List<string>(new string[] { "A1", "A2", "B1", "B2", "B2A", "B2B", "B3", "B3A", "B3B", "C1", "C2", "C3" });

            var orderItems = _entities.OrderItems.Where(p => p.OrderId == orderId && roomSkus.Contains(p.Product.Sku));

            if (orderItems.Any())
            {
                Guid orderItemGuid =  orderItems.FirstOrDefault().OrderItemGuid;
                var booking = _entities.LuxyBookings.Where(p => p.BookingGuid == orderItemGuid && p.IsVoid == false).FirstOrDefault();

                if (booking != null)
                {
                    return booking.BookingGuid;
                }


            }

            return null;


        }

        public void VoidBooking(Guid bookingGuid)
        {
          var booking =  _entities.LuxyBookings.Where(p => p.BookingGuid == bookingGuid).FirstOrDefault();
            if(booking != null)
            {
                booking.IsVoid = true;
                booking.IsOpen = false;
                _entities.SaveChanges();

            }
    
        }


        public BookingModel? Book(int orderId)
        {

            BookingModel bookingModel = null;

            List<string> roomSkus = new List<string>(new string[] { "A1", "A2", "B1", "B2", "B2A", "B2B", "B3", "B3A", "B3B", "C1", "C2", "C3" });


            //No booking if one order consist of more than 1 room
            if (_entities.OrderItems.Where(p => p.OrderId == orderId && roomSkus.Contains(p.Product.Sku)).Count() > 1)
            {
                return null;
            }


            var orderItem = _entities.OrderItems.Where(p => p.OrderId == orderId && roomSkus.Contains(p.Product.Sku)).FirstOrDefault();


            string[] productCustom = orderItem.AttributeDescription.Split(@"<br />", StringSplitOptions.RemoveEmptyEntries);


            //no booking if no product attributes
            if (productCustom.Count() < 2)
            {
                return null;
            }


            DateTime checkInTime = DateTime.Parse(productCustom[0].Replace("Check In Date: ", ""));

            int days = int.Parse(productCustom[1].Replace("Days: ", "").Substring(0,2));

     

            PackageDay packageDay = PackageDay.Standard;

            switch (days)
            {
                case 7:
                    packageDay = PackageDay.Basic;
                    break;
                case 28:
                    packageDay = PackageDay.Standard;
                    break;
                case 42:
                    packageDay = PackageDay.Premium;
                    break;
                default:
                    packageDay = PackageDay.Standard;
                    break;

            }



            Guid? guid2 = Book(orderItem.Product.Sku, checkInTime.AddHours(15), packageDay, orderItem.Order.Customer.Username, orderId, orderItem.OrderItemGuid, orderItem.Order.BillingAddress.FirstName + " " + orderItem.Order.BillingAddress.LastName, orderItem.Order.Customer.BillingAddress.PhoneNumber);


            var roomBooking = _entities.LuxyBookings.Where(p => p.BookingGuid == guid2).FirstOrDefault();

            if (roomBooking != null)
            {
                bookingModel = new BookingModel();

                bookingModel.CustomerName = roomBooking.CustomerName;
                bookingModel.CustomerUserName = roomBooking.Username;
                bookingModel.EndDateTime = roomBooking.EndDateTime;
                bookingModel.StartDateTime = roomBooking.StartDateTime;
                bookingModel.OrderId = roomBooking.OrderId;
                bookingModel.PackagePrice = roomBooking.Price;
                bookingModel.Phone = roomBooking.PhoneNumber;
                bookingModel.RoomCode = roomBooking.RoomCode;
                bookingModel.RoomNo = roomBooking.RoomNo;
                bookingModel.Discount = roomBooking.DiscountPrice.Value;
                bookingModel.Floor = roomBooking.Floor;
                bookingModel.RoomType = roomBooking.RoomType;


            }



            return bookingModel;

        }

        public Guid? Book(string roomCode, DateTime startDateTime, PackageDay packageDay, string username, int? orderId = null, Guid? newBookingGuid = null, string customerfullname = null, string phoneNo = null)
        {

            var customer = _entities.Customers.Where(p => p.Username == username);


            if (!customer.Any())
            {
                throw new UsernameNotExistException("Username not exists");
            }



            var room = _entities.LuxyRooms.Where(p => p.RoomCode == roomCode).FirstOrDefault();

            if (room != null)
            {

                bool isShared = room.RoomType == "DeluxeTwinBed" ? true : false;

                bool isConvertable = _entities.LuxyRooms.Where(p => p.RoomNo == room.RoomNo).Count() > 1;

                DateTime endDateTime = startDateTime.AddDays((int)packageDay);


                //shared booking
                if (isShared)
                {
                    var sharedBooking = _entities.LuxyBookings.Where(p => p.RoomNo == room.RoomNo && p.IsVoid ==false).Where(x => (startDateTime >= x.StartDateTime && startDateTime <= x.EndDateTime) || (endDateTime >= x.StartDateTime && endDateTime <= x.EndDateTime));

                    //add another booking if still a bed left for shared bedroom
                    if (sharedBooking.GroupBy(p=>p.RoomCode).Count() < 2)
                    {

                        //convertable room exist
                        if (sharedBooking.Where(p => p.IsShared == false).Count() == 1)
                        {
                            return newBookingGuid;
                        }


                        if (newBookingGuid == null)
                        {
                            newBookingGuid = Guid.NewGuid();

                        }

                        _entities.LuxyBookings.Add(new LuxyBooking
                        {
                            OrderId = orderId,
                            CustomerName = customerfullname,
                            DiscountPrice = 0,
                            PhoneNumber = phoneNo,
                            BookingGuid = newBookingGuid.Value,
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
                        var sharedBooking = _entities.LuxyBookings.Where(p => p.IsShared && p.RoomNo == room.RoomNo && p.IsVoid == false).Where(x => (startDateTime >= x.StartDateTime && startDateTime <= x.EndDateTime) || (endDateTime >= x.StartDateTime && endDateTime <= x.EndDateTime));

                        if (sharedBooking.Count() >= 1)
                        {
                            return newBookingGuid;

                        }
                    }


                    //not shared
                    var booking = _entities.LuxyBookings.Where(p => p.RoomCode == roomCode && p.IsVoid == false).Where(x => (startDateTime >= x.StartDateTime && startDateTime <= x.EndDateTime) || (endDateTime >= x.StartDateTime && endDateTime <= x.EndDateTime)).FirstOrDefault();

                    if (booking == null)
                    {

                        if (newBookingGuid == null)
                        {
                            newBookingGuid = Guid.NewGuid();

                        }


                        _entities.LuxyBookings.Add(new LuxyBooking
                        {
                            OrderId = orderId,
                            CustomerName = customerfullname,
                            DiscountPrice = 0,
                            PhoneNumber = phoneNo,
                            BookingGuid = newBookingGuid.Value,
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