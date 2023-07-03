using UnityEngine;

public class CurrencyDriveInfo : ScriptableObject
{
	public const int SECONDS_IN_HOUR = 3600;

	[Header("Mora")]
	[SerializeField] int _moraPerHour = 1000;
	[SerializeField] int _levelsToMoraPerHourIncrease = 1;
	[SerializeField] int _moraPerHourIncrease = 1;
	[SerializeField] int _baseMoraStorage = 1000;
	[SerializeField] int _moraStorageExpandCount = 500;
	[SerializeField] int _levelsToMoraStorageExpand = 10;
	public int MoraPerHour => _moraPerHour;
	public int LevelsToMoraPerHourIncrease => _levelsToMoraPerHourIncrease;
	public int MoraPerHourIncrease => _moraPerHourIncrease;
	public int BaseMoraStorage => _baseMoraStorage;
	public int MoraStorageExpandCount => _moraStorageExpandCount;
	public int LevelsToMoraStorageExpand => _levelsToMoraStorageExpand;
	public float MoraPerSecond => (float)MoraPerHour / SECONDS_IN_HOUR;


	[Header("Gems")]
	[SerializeField] int _gemsPerHour = 400;
	[SerializeField] int _gemsPerHourIncreaseCount = 1;
	[SerializeField] int _levelsToGemsPerHourIncrease = 5;
	[SerializeField] int _gemsPerLevel = 60;
	[SerializeField] int _baseGemsStorage = 700;
	[SerializeField] int _levelsToExpandGemsStorage = 10;
	[SerializeField] int _gemsStorageExpandCount = 500;
	public int GemsPerHour => _gemsPerHour;
	public int GemsPerLevel => _gemsPerLevel;
	public int GemsPerHourIncreaseCount => _gemsPerHourIncreaseCount;
	public int LevelsToGemsPerHourIncrease => _levelsToGemsPerHourIncrease;
	public int BaseGemsStorage => _baseGemsStorage;
	public int LevelsToGemsStorageExpand => _levelsToExpandGemsStorage;
	public int GemsStorageExpandCount => _gemsStorageExpandCount;
	public float GemsPerSecond => (float)_gemsPerHour / SECONDS_IN_HOUR;
}
