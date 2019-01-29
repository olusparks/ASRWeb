using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;
using ASRWebMVC.Interface;
using ASRWebMVC.Models;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;

namespace ASRWebMVC.Controllers
{
    public class SlotController : Controller
    {
		private ISlotService _slotService;
		private IUserService _userService;
		private IRoomService _roomService;

		public SlotController(ISlotService slotService, IUserService userService, IRoomService roomService)
		{
			_slotService = slotService;
			_userService = userService;
			_roomService = roomService;
		}
        public IActionResult Index()
        {
			var userSlots = _slotService.GetUnBookedSlot(GetUserId());
			return View(userSlots);
        }

		[HttpGet]
		public IActionResult Create()
		{
			var allRooms = _roomService.GetAllRooms();
			ViewBag.AllRooms = allRooms;
			return View();
		}

		[HttpPost]
		[ValidateAntiForgeryToken]
		public async Task<IActionResult> Create(SlotModel model)
		{
			if (ModelState.IsValid)
			{
				model.StaffId = GetUserId();
				if(await _slotService.Create(model))
				{
					return RedirectToAction("Index", "Slot");
				}
				ModelState.AddModelError("CreateError", "Maximum slots have been reached..");
			}
			return View();
		}


		[HttpGet]
		[HttpPost]
		[ValidateAntiForgeryToken]
		public async Task<IActionResult> Book(string id)
		{
			var stdId = GetUserId();
			var stTime = HttpContext.Request.Form.SingleOrDefault(x => x.Key == "stTime").Value;
			var staffId = HttpContext.Request.Form.SingleOrDefault(x => x.Key == "staffId").Value;
			var roomId = HttpContext.Request.Form.SingleOrDefault(x => x.Key == "roomId").Value;

			if (stdId.StartsWith("e"))
			{
				if (await _slotService.Book(stTime, stdId, roomId))
				{
					return RedirectToAction("Index", "Slot");
				}
				ViewBag.Error = "Maximum book limit reached!";
			}
			else
			{
				ViewBag.Error = "A staff cannot book..";
				ModelState.AddModelError("BookingModel", "A staff cannot book..");
			}
			
			var slot = _slotService.GetSlotDetails(roomId);
			return View(slot);
		}

		[HttpGet]
		[HttpPost, ActionName("Booked")]
		public IActionResult Booked(string staffId)
		{
			var user = _userService.GetUsers();
			ViewData["StaffId"] = new SelectList(user, "UserId", "Name");
			List<SlotModel> bookedSlots;
			 
			if (!string.IsNullOrEmpty(staffId))
			{
				bookedSlots = _slotService.GetBookedSlotByStaffId(staffId);
				return View(bookedSlots);
			}

			bookedSlots = _slotService.GetBookedSlot();
			return View(bookedSlots);
		}

		[HttpPost]
		[HttpGet]
		[ValidateAntiForgeryToken]
		public async Task<IActionResult> CancelBook()
		{
			var stdId = GetUserId();
			var stTime = HttpContext.Request.Form.SingleOrDefault(x => x.Key == "stTime").Value;

			if (await _slotService.CancelBooking(stTime, stdId))
			{
				return RedirectToAction("Booked", "Slot");
			}
			return View();
		}

		[HttpGet]
		[HttpPost]
		[ValidateAntiForgeryToken]
		public async Task<IActionResult> Delete()
		{
			var stTime = HttpContext.Request.Form.SingleOrDefault(x => x.Key == "stTime").Value;
			if (await _slotService.Delete(stTime))
			{
				return RedirectToAction("Index");
			}
			return View();
		}
		//public IActionResult Search()
		//{
		//	var staffId = HttpContext.Request.Form.SingleOrDefault(x => x.Key == "staffId").Value;
		//	var bookedSlots = _slotService.GetBookedSlotByStaffId(staffId);
		//	return View();
		//}

		#region Helper
		public string GetUserId()
		{
			Claim cp = User.FindFirst("name");
			string userEmail = cp?.Value;
			var user = _userService.GetUserByEmail(userEmail);
			return user.UserId;
		}
		#endregion

	}
}