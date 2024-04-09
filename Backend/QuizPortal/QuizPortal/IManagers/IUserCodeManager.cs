using System;
using QuizPortal.DBModels;

namespace QuizPortal.IManagers
{
	public interface IUserCodeManager
	{
		public Task Add(UserCode userCode);

		public Task UpdateIsActive(UserCode updatedUserCode);

		public Task<UserCode> GetUserCodeByCode(string code);
	}
}
