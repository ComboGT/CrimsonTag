using Photon.Pun;
using UnityEngine;

public class GorillaFlag : GorillaTrigger
{
	public bool isRedFlag;

	public override void OnTriggered()
	{
		base.OnTriggered();
		var ctfManager = GorillaGameManager.instance as GorillaCTFManager;
		if (ctfManager != null)
		{
			PhotonView.Get(ctfManager).RPC("TagFlag", RpcTarget.MasterClient, isRedFlag);
		}
	}
}
