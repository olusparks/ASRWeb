using ASRWebMVC.Interface;
using ASRWebMVC.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using static ASRWebMVC.Helper.HelperMtds;

namespace ASRWebMVC.Service
{
	public class UserService : IUserService
	{
		private ASRDbfContext _context;

		public UserService(ASRDbfContext context)
		{
			_context = context;
		}
		public Task<bool> AddUser(SignUpModel user)
		{
			if (_context.User.Any(x => x.Email == user.Email))
				Task.FromResult(false);

			User usr = new User();
			if (user.Email.After("@").Contains("student"))
			{
				usr.UserId = IDGenerator("2");
				usr.Name = user.Name;
				usr.Email = user.Email;
			}
			else
			{
				usr.UserId = IDGenerator("e");
				usr.Name = user.Name;
				usr.Email = user.Email;
			}

			_context.User.Add(usr);
			_context.SaveChangesAsync();
			return Task.FromResult(true);
		}

		public Task<bool> ValidateCredentials(string email, out User user)
		{
			user = null;
			if (string.IsNullOrEmpty(email))
				return Task.FromResult(false);

			var usr = _context.User.SingleOrDefault(x => x.Email == email);
			if (usr == null)
				return Task.FromResult(false);

			user = usr;
			return Task.FromResult(true);
		}

		public User GetUserByEmail(string email)
		{
			var user = _context.User.Where(x => x.Email == email).SingleOrDefault();
			if (user == null)
				return null;

			return user;
		}

		public List<User> GetUsers()
		{
			return _context.User.Where(x => x.UserId.StartsWith("e")).ToList();
		}
	}
}
