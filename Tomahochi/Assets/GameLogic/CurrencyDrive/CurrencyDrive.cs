using UnityEngine;

public abstract class CurrencyDrive
{
	[Header("Currency")]
	public float MoraCount = 0;
	public float GemsCount = 0;

	public virtual bool DriveCondition => true;
	public abstract float MoraPerSecond { get; }
	public abstract float GemsPerSecond { get; }
	public abstract int MoraStorage { get; }
	public abstract int GemsStorage { get; }
	public virtual void EmitLive(float seconds)
	{
		if (DriveCondition)
		{
			MoraCount = Mathf.Clamp(MoraCount + MoraPerSecond * seconds, 0, MoraStorage);
			GemsCount = Mathf.Clamp(GemsCount + GemsPerSecond * seconds, 0, GemsStorage);
		}
	}
}
