using ASRWebMVC.Interface;
using ASRWebMVC.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ASRWebMVC.Service
{
	public class SlotService : ISlotService
	{
		private ASRDbfContext _context;

		public SlotService(ASRDbfContext context)
		{
			_context = context;
		}
		public Task<bool> Create(SlotModel model)
		{
			int? maxSlot = MaximumSlotPerDay(model.StaffId);
			if (maxSlot == null)
				return Task.FromResult(false);

			if (maxSlot > 3)
				return Task.FromResult(false);

			Slot slot = new Slot
			{
				RoomId = model.RoomId,
				StartTime = model.StartTime,
				StaffId = model.StaffId
			};

			_context.Slot.Add(slot);
			_context.SaveChangesAsync();
			return Task.FromResult(true);
		}

		public Task<bool> Delete(string startTime)
		{
			DateTime stTime = Convert.ToDateTime(startTime);
			var slot = _context.Slot.Where(x => x.StartTime == stTime && string.IsNullOrEmpty(x.BookedInStudentId)).SingleOrDefault();
			if (slot == null)
				return Task.FromResult(false);

			_context.Slot.Remove(slot);
			_context.SaveChangesAsync();
			return Task.FromResult(true);
		}

		public List<SlotModel> GetBookedSlotByStaffId(string staffId)
		{
			List<SlotModel> slotModels = new List<SlotModel>();
			var slots = _context.Slot.Where(x => x.StaffId == staffId && !string.IsNullOrEmpty(x.BookedInStudentId)).ToList();
			foreach (var item in slots)
			{
				SlotModel slotModel = new SlotModel()
				{
					RoomId = item.RoomId,
					StartTime = item.StartTime,
					StaffId = item.StaffId,
					BookedInStudentId = item.BookedInStudentId
				};
				slotModels.Add(slotModel);
			}
			return slotModels;
		}

		public List<SlotModel> GetBookedSlot()
		{
			List<SlotModel> slotModels = new List<SlotModel>();
			var slots = _context.Slot.Where(x => !string.IsNullOrEmpty(x.BookedInStudentId)).ToList();
			foreach (var item in slots)
			{
				SlotModel slotModel = new SlotModel()
				{
					RoomId = item.RoomId,
					StartTime = item.StartTime,
					StaffId = item.StaffId,
					BookedInStudentId = item.BookedInStudentId
				};
				slotModels.Add(slotModel);
			}
			return slotModels;
		}

		public List<SlotModel> GetUnBookedSlot(string staffId)
		{
			List<SlotModel> slotModels = new List<SlotModel>();
			var slots = _context.Slot.Where(x => x.StaffId == staffId && string.IsNullOrEmpty(x.BookedInStudentId)).ToList();
			foreach (var item in slots)
			{
				SlotModel slotModel = new SlotModel()
				{
					RoomId = item.RoomId,
					StartTime = item.StartTime,
					StaffId = item.StaffId,
					BookedInStudentId = item.BookedInStudentId
				};
				slotModels.Add(slotModel);
			}
			return slotModels;
		}

		public Task<bool> Book(string startTime, string studentId, string roomId)
		{
			if (string.IsNullOrEmpty(studentId) || string.IsNullOrEmpty(roomId) || string.IsNullOrEmpty(startTime))
				return Task.FromResult(false);

			var bookNo = MaximumBookPerDay(studentId);
			if (bookNo == null)
				return Task.FromResult(false);

			if (bookNo > 0)
				return Task.FromResult(false);

			DateTime stTime = Convert.ToDateTime(startTime);
			var slot = _context.Slot.Where(x => x.StartTime == stTime && x.RoomId == roomId).SingleOrDefault();
			if (slot == null)
				return Task.FromResult(false);
			
			if(!string.IsNullOrEmpty(slot.BookedInStudentId))
				return Task.FromResult(false);

			slot.BookedInStudentId = studentId;
			_context.Update(slot);
			_context.SaveChanges();
			return Task.FromResult(true);
		}

		public Task<bool> CancelBooking(string startTime, string studentId)
		{
			if (string.IsNullOrEmpty(studentId) ||  string.IsNullOrEmpty(startTime))
				return Task.FromResult(false);

			DateTime stTime = Convert.ToDateTime(startTime);
			var slot = _context.Slot.Where(x => x.StartTime == stTime && x.BookedInStudentId == studentId).SingleOrDefault();
			if (slot == null)
				return Task.FromResult(false);

			if (string.IsNullOrEmpty(slot.BookedInStudentId))
				return Task.FromResult(false);

			slot.BookedInStudentId = null;
			_context.Update(slot);
			_context.SaveChanges();
			return Task.FromResult(true);
		}

		public SlotModel GetSlotDetails(string roomId)
		{
			//if (string.IsNullOrEmpty(startTime))
			//	return null;

			if (string.IsNullOrEmpty(roomId))
				return null;

			//DateTime stTime = Convert.ToDateTime(startTime);
			var slot = _context.Slot.Where(x => x.RoomId == roomId).SingleOrDefault();
			var slotModel = new SlotModel
			{
				RoomId = slot.RoomId,
				StaffId = slot.StaffId,
				StartTime = slot.StartTime
			};

			return slotModel;
		}

		public int? MaximumSlotPerDay(string staffId)
		{
			if (string.IsNullOrEmpty(staffId))
				return null;

			var stTime = DateTime.Now.Date;
			int slotNo =_context.Slot.Where(x => x.StartTime == stTime && x.StaffId == staffId).Count();
			return slotNo;
		}

		public int? MaximumBookPerDay(string studentId)
		{
			if (string.IsNullOrEmpty(studentId))
				return null;

			var stTime = DateTime.Now.Date;
			int bookNo = _context.Slot.Where(x => x.StartTime == stTime && x.BookedInStudentId == studentId).Count();
			return bookNo;
		}
	}
}
