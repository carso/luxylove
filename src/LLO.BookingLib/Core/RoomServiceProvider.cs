using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LLO.BookingLib
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
        public string RoomName { get; set; }

        public RoomTypeEnum? RoomType { get; set; }

        public string RoomCode { get; set; }
        public FloorEnum? Floor { get; set; }

        public string RoomNumber { get; set; }
    }




    public class RoomExistException : Exception
    {
        public RoomExistException(string message) : base(message)
        {

        }
    }
    public class RoomNoProductSKUException : Exception
    {
        public RoomNoProductSKUException(string message) : base(message)
        {

        }
    }

    public class RoomServiceProvider
    {

        public List<RoomModel> GetAllRoom()
        {

            List<RoomModel> roomModels = new List<RoomModel>();

            LuxylovedbContext luxylovedbEntities = new LuxylovedbContext();

            foreach(var room in luxylovedbEntities.LuxyRooms)
            {
                roomModels.Add(new RoomModel() {  RoomCode = room.RoomCode, RoomNumber = room.RoomNo, RoomName = room.Name  });
            }

            return roomModels;
        }

        public List<RoomModel> GetAllRoomByRoomNo(string roomNo)
        {

            List<RoomModel> roomModels = new List<RoomModel>();

            LuxylovedbContext luxylovedbEntities = new LuxylovedbContext();

            foreach (var room in luxylovedbEntities.LuxyRooms.Where(p=>p.RoomNo == roomNo))
            {
                roomModels.Add(new RoomModel() { RoomCode = room.RoomCode, RoomNumber = room.RoomNo, RoomName =   room.Name });
            }

            return roomModels;
        }


        public List<RoomModel> GetAllRoomNo()
        {

            List<RoomModel> roomModels = new List<RoomModel>();

            LuxylovedbContext luxylovedbEntities = new LuxylovedbContext();

            foreach (var room in luxylovedbEntities.LuxyRooms.GroupBy(p=>p.RoomNo).Select(p=>p.Key))
            {
                roomModels.Add(new RoomModel() { RoomNumber = room });
            }
           
            return roomModels;
        }

        public void AddRoom(RoomModel roomModel)
        {
          LuxylovedbContext luxylovedbEntities = new LuxylovedbContext();

            var products = luxylovedbEntities.Products.Where(p => p.Sku == roomModel.RoomCode && p.Published == true).OrderByDescending(p=>p.CreatedOnUtc);

            if (!products.Any())
            {
                throw new RoomNoProductSKUException("No SKU product found!");        
            }


            var room = luxylovedbEntities.LuxyRooms.Where(p => p.RoomCode == roomModel.RoomCode);

            if (room.Any())
            {
                throw new RoomExistException("Room Exists!");
            }

            var product = products.FirstOrDefault();
                       

            luxylovedbEntities.LuxyRooms.Add(new LuxyRoom()
            {
                Floor = roomModel.Floor.ToString(),
                IsActive = true,
                Name = string.Format("{0} ({1})", roomModel.RoomCode, roomModel.RoomType.ToString()), 
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
