using ASRWebMVC.Interface;
using ASRWebMVC.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ASRWebMVC.Service
{
	public class RoomService : IRoomService
	{
		private ASRDbfContext _context;

		public RoomService(ASRDbfContext context)
		{
			_context = context;
		}

		public List<RoomModel> GetAllRooms()
		{
			var rooms = _context.Room.ToList();
			if (rooms == null)
				return null;

			List<RoomModel> roomModels = new List<RoomModel>();
			foreach (var item in rooms)
			{
				RoomModel roomModel = new RoomModel
				{
					RoomId = item.RoomId
				};
				roomModels.Add(roomModel);
			}
			return roomModels;
		}

		public Task<bool> CheckRoomAvailablitiy(RoomModel model)
		{
			var room = _context.Room.Find(model.RoomId);
			if (room == null)
				return Task.FromResult(false);

			var booked = _context.Slot.Where(s => s.RoomId == model.RoomId &&
									//s.StartTime == model.StartTime &&
									string.IsNullOrEmpty(s.BookedInStudentId))
									.SingleOrDefault();
			//var booked = _context.Slot.Where(s => s.StartTime == model.StartTime && s.BookedInStudentId == string.Empty).SingleOrDefault();
			if (booked == null)
				return Task.FromResult(false);

			return Task.FromResult(true);
		}
	}
}
