using ASRWebMVC.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ASRWebMVC.Interface
{
	public interface IRoomService
	{
		Task<bool> CheckRoomAvailablitiy(RoomModel model);
		List<RoomModel> GetAllRooms();
	}
}
