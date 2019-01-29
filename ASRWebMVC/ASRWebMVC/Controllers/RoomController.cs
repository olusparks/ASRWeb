using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using ASRWebMVC.Interface;
using ASRWebMVC.Models;
using Microsoft.AspNetCore.Mvc;

namespace ASRWebMVC.Controllers
{
    public class RoomController : Controller
    {
		private IRoomService _roomService;

		public RoomController(IRoomService roomService)
		{
			_roomService = roomService;
		}
        public IActionResult Index()
        {
			//var allRooms = _roomService.GetAllRooms();
			//return View(allRooms);
			return View();
        }

		[HttpGet]
		public IActionResult CheckRoom()
		{
			var allRooms = _roomService.GetAllRooms();
			ViewBag.AllRooms = allRooms;
			return View();
		}


		[HttpPost]
		[ValidateAntiForgeryToken]
		public async Task<IActionResult> CheckRoom(RoomModel model)
		{
			if (ModelState.IsValid)
			{
				if (await _roomService.CheckRoomAvailablitiy(model))
				{
					ViewBag.msg = "Room is available";
					return PartialView("_message",  ViewBag.msg);
				}
			}
			ModelState.AddModelError("Error", "Could not find Room");
			return View(model);
		}
    }
}