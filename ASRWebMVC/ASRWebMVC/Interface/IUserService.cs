using ASRWebMVC.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ASRWebMVC.Interface
{
	public interface IUserService
	{
		Task<bool> ValidateCredentials(string email, out User user);
		Task<bool> AddUser(SignUpModel user);
		User GetUserByEmail(string email);
		List<User> GetUsers();
	}
}
