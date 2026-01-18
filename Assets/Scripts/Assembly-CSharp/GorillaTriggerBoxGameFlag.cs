using Photon.Pun;
using UnityEngine;

public class GorillaTriggerBoxGameFlag : GorillaTriggerBox
{
	public string functionName;

	public override void OnBoxTriggered()
	{
		base.OnBoxTriggered();
		if (GorillaGameManager.instance != null)
		{
			PhotonView.Get(GorillaGameManager.instance).RPC(functionName, RpcTarget.MasterClient, null);
		}
	}
}
