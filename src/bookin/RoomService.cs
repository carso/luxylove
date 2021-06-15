using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace bookin
{
    public enum RoomTypeEnum
    {
        DeluxeRoom, DeluxeTwinBed, PremiumQueen, PremiumKing, VIPSuite 
    }

    public enum FloorEnum
    {
        Ground, First, Second, Third
    }

    public class RoomModel
    {
        public RoomTypeEnum RoomType { get; set; }

        public string RoomCode { get; set; }
        public FloorEnum Floor { get; set; }

        public string RoomNumber { get; set; }
    }



    public class RoomService
    {
        public void AddRoom(RoomModel roomModel)
        {
            luxylovedbEntities luxylovedbEntities = new luxylovedbEntities();

            var products = luxylovedbEntities.Products.Where(p => p.Sku == roomModel.RoomCode && p.Published == true).OrderByDescending(p=>p.CreatedOnUtc);

            if (!products.Any())
            {
                throw new Exception("No SKU product found!");        
            }


            var room = luxylovedbEntities.Luxy_Room.Where(p => p.RoomCode == roomModel.RoomCode);

            if (room.Any())
            {
                throw new Exception("Room Exists!");
            }

            var product = products.FirstOrDefault();

            

            luxylovedbEntities.Luxy_Room.Add(new Luxy_Room()
            {
                Floor = roomModel.Floor.ToString(),
                IsActive = true,
                Name =  roomModel.RoomType.ToString() + " " + roomModel.RoomCode,
                Price = product.Price,
                RoomCode = roomModel.RoomCode,
                RoomType = roomModel.RoomType.ToString(),
                Product = product, 
                RoomNo = roomModel.RoomNumber
            });


            luxylovedbEntities.SaveChanges();


        }
        
    }
}
