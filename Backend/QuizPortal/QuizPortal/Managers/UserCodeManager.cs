using System;
using QuizPortal.IManagers;
using Microsoft.EntityFrameworkCore;
using QuizPortal.DBModels;

namespace QuizPortal.Managers
{
	public class UserCodeManager: IUserCodeManager
	{
		private readonly AppDBContext _context;

		public UserCodeManager(AppDBContext context)
		{
			_context = context;
		}

		public async Task Add(UserCode userCode)
		{
			await _context.UserCodes.AddAsync(userCode);
			await _context.SaveChangesAsync();
		}

        public async Task UpdateIsActive(UserCode updatedUserCode)
        {
			UserCode? userCode = await _context.UserCodes.FindAsync(updatedUserCode.Id);
			if(userCode == null)
			{
				return;
			}

			userCode.IsActive = updatedUserCode.IsActive;

			await _context.SaveChangesAsync();
        }

        public async Task<UserCode> GetUserCodeByCode(string code)
        {
			UserCode? userCode = await _context.UserCodes
				.Include(uc => uc.User)
				.FirstOrDefaultAsync(uc => uc.Code == code && uc.IsActive);
			return userCode;
        }
    }
}
