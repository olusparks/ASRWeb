using ASRWebMVC.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ASRWebMVC.Interface
{
	public interface ISlotService
	{
		Task<bool> Create(SlotModel model);
		Task<bool> Delete(string startTime);
		List<SlotModel> GetBookedSlotByStaffId(string staffId);
		List<SlotModel> GetBookedSlot();
		List<SlotModel> GetUnBookedSlot(string staffId);
		Task<bool> Book(string startTime, string studentId, string roomId);
		Task<bool> CancelBooking(string startTime, string studentId);
		SlotModel GetSlotDetails(string roomId);
		int? MaximumSlotPerDay(string staffId);
		int? MaximumBookPerDay(string studentId);
	}
}
