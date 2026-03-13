namespace Kampai.Common
{
	public interface ICoppaService
	{
		bool IsBirthdateKnown();

		bool GetBirthdate(out global::System.DateTime birthdate);

		bool Restricted();

		void SetUserBirthdate(global::System.DateTime birthdate);

		int GetAge();
	}
}
