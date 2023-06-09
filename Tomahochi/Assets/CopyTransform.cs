using UnityEngine;

public class CopyTransform : MonoBehaviour
{
    public Transform Target;
    public Vector3 PositionOffset;
    public bool
        CopyPositionX = true,
        CopyPositionY = true,
        CopyPositionZ = true;
    public bool
        CopyRotateX = false,
        CopyRotateY = false,
        CopyRotateZ = false;

    public bool CopyPosition 
    {
        set
        {
            CopyPositionX = value;
            CopyPositionY = value;
            CopyPositionZ = value;
        }
    }

    public bool CopyRotate
    {
        set
        {
            CopyRotateX = value;
            CopyRotateY = value;
            CopyRotateZ = value;
        }
    }


	protected void Awake()
	{
        transform.parent = null;
	}

	protected void Update()
    {
        if (Target == null)
        {
            Destroy(this.gameObject);
            return;
        }

        Vector3 resultPosition = new Vector3();
        resultPosition.x = CopyPositionX ? Target.position.x + PositionOffset.x : transform.position.x;
        resultPosition.y = CopyPositionY ? Target.position.y + PositionOffset.y : transform.position.y;
        resultPosition.z = CopyPositionZ ? Target.position.z + PositionOffset.z : transform.position.z;
        transform.position = resultPosition;
        Vector3 anglesResult = new();
        anglesResult.x = CopyRotateX ? Target.eulerAngles.x : transform.eulerAngles.x;
        anglesResult.y = CopyRotateY ? Target.eulerAngles.y : transform.eulerAngles.y;
        anglesResult.z = CopyRotateZ ? Target.eulerAngles.z : transform.eulerAngles.z;
        transform.eulerAngles = anglesResult;
    }

	private void OnValidate()
	{
		if (Application.isPlaying == false && Target != null)
        {
            PositionOffset = Target.position - transform.position;
        }
	}
}
