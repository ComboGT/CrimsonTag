using System.Collections;
using System.Collections.Generic;
using GorillaLocomotion;
using GorillaNetworking;
using Photon.Pun;
using Photon.Realtime;
using Photon.Voice.Unity;
using Steamworks;
using UnityEngine;
using UnityEngine.XR;
using UnityEngine.XR.Management;

public class GorillaTagger : MonoBehaviour
{
	public enum StatusEffect
	{
		None = 0,
		Frozen = 1,
		Slowed = 2,
		Dead = 3,
		Infected = 4,
		It = 5
	}

	private static GorillaTagger _instance;

	public static bool hasInstance;

	public bool inCosmeticsRoom;

	public SphereCollider headCollider;

	public CapsuleCollider bodyCollider;

	private Vector3 lastLeftHandPositionForTag;

	private Vector3 lastRightHandPositionForTag;

	private Vector3 lastBodyPositionForTag;

	private Vector3 lastHeadPositionForTag;

	public Transform rightHandTransform;

	public Transform leftHandTransform;

	public float hapticWaitSeconds = 0.05f;

	public float handTapVolume = 0.1f;

	public float tapCoolDown = 0.15f;

	public float lastLeftTap;

	public float lastRightTap;

	public float tapHapticDuration = 0.05f;

	public float tapHapticStrength = 0.5f;

	public float tagHapticDuration = 0.15f;

	public float tagHapticStrength = 1f;

	public float taggedHapticDuration = 0.35f;

	public float taggedHapticStrength = 1f;

	private bool leftHandTouching;

	private bool rightHandTouching;

	public float taggedTime;

	public float tagCooldown;

	public float slowCooldown = 3f;

	public VRRig myVRRig;

	public VRRig offlineVRRig;

	public GameObject thirdPersonCamera;

	public GameObject mainCamera;

	public bool testTutorial;

	public bool disableTutorial;

	public GameObject leftHandTriggerCollider;

	public GameObject rightHandTriggerCollider;

	public Camera mirrorCamera;

	public AudioSource leftHandSlideSource;

	public AudioSource rightHandSlideSource;

	public bool overrideNotInFocus;

	private Vector3 leftRaycastSweep;

	private Vector3 leftHeadRaycastSweep;

	private Vector3 rightRaycastSweep;

	private Vector3 rightHeadRaycastSweep;

	private Vector3 headRaycastSweep;

	private Vector3 bodyRaycastSweep;

	private InputDevice rightDevice;

	private InputDevice leftDevice;

	private bool primaryButtonPressRight;

	private bool secondaryButtonPressRight;

	private bool primaryButtonPressLeft;

	private bool secondaryButtonPressLeft;

	private RaycastHit hitInfo;

	public Photon.Realtime.Player otherPlayer;

	private Photon.Realtime.Player tryPlayer;

	private Vector3 topVector;

	private Vector3 bottomVector;

	private Vector3 bodyVector;

	private int tempInt;

	private InputDevice inputDevice;

	private bool wasInOverlay;

	private PhotonView tempView;

	public StatusEffect currentStatus;

	public float statusStartTime;

	public float statusEndTime;

	private float refreshRate;

	private float baseSlideControl;

	private int gorillaTagColliderLayerMask;

	private RaycastHit[] nonAllocRaycastHits = new RaycastHit[30];

	private int nonAllocHits;

	private Recorder myRecorder;

	private Callback<GameOverlayActivated_t> gameOverlayActivatedCb;

	private bool isGameOverlayActive;

	// Cached XR loader type (replaces deprecated XRSettings.loadedDeviceName checks)
	private bool isOculusLoader;
	private bool isOpenVRLoader;

	// Cached layer mask and camera for ShowCosmeticParticles
	private int cosmeticParticleLayerMask;
	private Camera mainCameraComponent;

	// Fixed physics constants (anti-cheat: prevents refresh rate manipulation exploits)
	private const float FixedPhysicsRate = 90f;
	private const float FixedDeltaTime = 1f / FixedPhysicsRate;
	private const int FixedVelocityHistorySize = 7;  // 90Hz / 12 = 7.5, floored to 7

	public static GorillaTagger Instance => _instance;

	public float sphereCastRadius => 0.03f;

	protected void Awake()
	{
		if (_instance != null && _instance != this)
		{
			Object.Destroy(base.gameObject);
		}
		else
		{
			_instance = this;
			hasInstance = true;
		}
		if (!disableTutorial && (testTutorial || (PlayerPrefs.GetString("tutorial") != "done" && PhotonNetworkController.Instance.gameVersion != "dev")))
		{
			base.transform.parent.position = new Vector3(-140f, 28f, -102f);
			base.transform.parent.eulerAngles = new Vector3(0f, 180f, 0f);
			GorillaLocomotion.Player.Instance.InitializeValues();
			PlayerPrefs.SetFloat("redValue", Random.value);
			PlayerPrefs.SetFloat("greenValue", Random.value);
			PlayerPrefs.SetFloat("blueValue", Random.value);
			PlayerPrefs.Save();
			UpdateColor(PlayerPrefs.GetFloat("redValue", 0f), PlayerPrefs.GetFloat("greenValue", 0f), PlayerPrefs.GetFloat("blueValue", 0f));
		}
		thirdPersonCamera.SetActive(Application.platform != RuntimePlatform.Android);
		inputDevice = InputDevices.GetDeviceAtXRNode(XRNode.RightHand);
		wasInOverlay = false;
		baseSlideControl = GorillaLocomotion.Player.Instance.slideControl;
		gorillaTagColliderLayerMask = LayerMask.GetMask("Gorilla Tag Collider");

		// Cache layer mask and camera component for ShowCosmeticParticles
		cosmeticParticleLayerMask = 1 << LayerMask.NameToLayer("GorillaCosmeticParticle");
		mainCameraComponent = mainCamera.GetComponent<Camera>();
	}

	protected void OnDestroy()
	{
		// Clean up Steamworks callback to prevent memory leak
		if (gameOverlayActivatedCb != null)
		{
			gameOverlayActivatedCb.Dispose();
			gameOverlayActivatedCb = null;
		}

		if (_instance == this)
		{
			_instance = null;
			hasInstance = false;
		}
	}

	protected void Start()
	{
		// Cache XR loader type (replaces deprecated XRSettings.loadedDeviceName)
		var activeLoader = XRGeneralSettings.Instance?.Manager?.activeLoader;
		string loaderName = activeLoader?.GetType().Name ?? "";
		isOpenVRLoader = loaderName.Contains("OpenVR");
		isOculusLoader = loaderName.Contains("Oculus");

		if (isOpenVRLoader)
		{
			GorillaLocomotion.Player.Instance.leftHandOffset = new Vector3(-0.02f, 0f, -0.07f);
			GorillaLocomotion.Player.Instance.rightHandOffset = new Vector3(0.02f, 0f, -0.07f);
		}
		bodyVector = new Vector3(0f, bodyCollider.height / 2f - bodyCollider.radius, 0f);
		if (SteamManager.Initialized)
		{
			gameOverlayActivatedCb = Callback<GameOverlayActivated_t>.Create(OnGameOverlayActivated);
		}

		// Initialize fixed physics (anti-cheat: all players use same physics regardless of display refresh rate)
		InitializeFixedPhysics();
	}

	/// <summary>
	/// Sets fixed physics values for all players to prevent refresh rate manipulation exploits.
	/// All clients run at the same physics rate regardless of their display's refresh rate.
	/// </summary>
	private void InitializeFixedPhysics()
	{
		Time.fixedDeltaTime = FixedDeltaTime;

		var player = GorillaLocomotion.Player.Instance;
		player.velocityHistorySize = FixedVelocityHistorySize;

		// Calculate fixed slide control based on 90Hz physics
		// Formula: 1 - pow(pow(1 - baseSlideControl, 120), 1/targetFPS)
		float fixedSlideControl = 1f - CalcSlideControl(FixedPhysicsRate);
		player.slideControl = fixedSlideControl;

		player.InitializeValues();

		Debug.Log($"[Anti-Cheat] Fixed physics initialized: {FixedPhysicsRate}Hz, velocityHistorySize={FixedVelocityHistorySize}, slideControl={fixedSlideControl:F6}");
	}

	private void OnGameOverlayActivated(GameOverlayActivated_t pCallback)
	{
		isGameOverlayActive = pCallback.m_bActive != 0;
	}

	protected void LateUpdate()
	{
		HandleOculusOverlay();
		ProcessTagDetection();
		ProcessHandFeedback();
		CheckEndStatusEffect();
		UpdateTrackingPositions();
		ProcessVoiceChat();
	}

	/// <summary>
	/// Handles Oculus input focus and overlay state changes.
	/// Disables hand colliders when system UI is open.
	/// </summary>
	private void HandleOculusOverlay()
	{
		if (isOculusLoader)
		{
			if (OVRManager.hasInputFocus && !overrideNotInFocus)
			{
				if (!leftHandTriggerCollider.activeSelf)
				{
					leftHandTriggerCollider.SetActive(value: true);
					rightHandTriggerCollider.SetActive(value: true);
				}
				GorillaLocomotion.Player.Instance.inOverlay = false;
				if (wasInOverlay && CosmeticsController.instance != null)
				{
					CosmeticsController.instance.LeaveSystemMenu();
				}
				wasInOverlay = false;
			}
			else
			{
				if (leftHandTriggerCollider.activeSelf)
				{
					leftHandTriggerCollider.SetActive(value: false);
					rightHandTriggerCollider.SetActive(value: true);
				}
				GorillaLocomotion.Player.Instance.inOverlay = true;
				wasInOverlay = true;
			}
		}
		else if (Application.platform != RuntimePlatform.Android && OVRManager.instance != null && OVRManager.OVRManagerinitialized && OVRManager.instance.gameObject != null && OVRManager.instance.gameObject.activeSelf)
		{
			Object.Destroy(OVRManager.instance.gameObject);
		}
	}

	/// <summary>
	/// Performs raycast sweeps to detect tag collisions with other players.
	/// Checks hands, head, and body for potential tags.
	/// </summary>
	private void ProcessTagDetection()
	{
		// Calculate sweep vectors from last frame positions
		leftRaycastSweep = leftHandTransform.position - lastLeftHandPositionForTag;
		leftHeadRaycastSweep = leftHandTransform.position - headCollider.transform.position;
		rightRaycastSweep = rightHandTransform.position - lastRightHandPositionForTag;
		rightHeadRaycastSweep = rightHandTransform.position - headCollider.transform.position;
		headRaycastSweep = headCollider.transform.position - lastHeadPositionForTag;
		bodyRaycastSweep = bodyCollider.transform.position - lastBodyPositionForTag;

		otherPlayer = null;
		float castRadius = sphereCastRadius * GorillaLocomotion.Player.Instance.scale;

		// Left hand sweep
		nonAllocHits = Physics.SphereCastNonAlloc(lastLeftHandPositionForTag, castRadius, leftRaycastSweep.normalized, nonAllocRaycastHits, Mathf.Max(leftRaycastSweep.magnitude, castRadius), gorillaTagColliderLayerMask, QueryTriggerInteraction.Collide);
		if (nonAllocHits > 0 && TryToTag(nonAllocRaycastHits[0], isBodyTag: false, out tryPlayer))
		{
			otherPlayer = tryPlayer;
		}

		// Left hand to head sweep
		nonAllocHits = Physics.SphereCastNonAlloc(headCollider.transform.position, castRadius, leftHeadRaycastSweep.normalized, nonAllocRaycastHits, Mathf.Max(leftHeadRaycastSweep.magnitude, castRadius), gorillaTagColliderLayerMask, QueryTriggerInteraction.Collide);
		if (nonAllocHits > 0 && TryToTag(nonAllocRaycastHits[0], isBodyTag: false, out tryPlayer))
		{
			otherPlayer = tryPlayer;
		}

		// Right hand sweep
		nonAllocHits = Physics.SphereCastNonAlloc(lastRightHandPositionForTag, castRadius, rightRaycastSweep.normalized, nonAllocRaycastHits, Mathf.Max(rightRaycastSweep.magnitude, castRadius), gorillaTagColliderLayerMask, QueryTriggerInteraction.Collide);
		if (nonAllocHits > 0 && TryToTag(nonAllocRaycastHits[0], isBodyTag: false, out tryPlayer))
		{
			otherPlayer = tryPlayer;
		}

		// Right hand to head sweep
		nonAllocHits = Physics.SphereCastNonAlloc(headCollider.transform.position, castRadius, rightHeadRaycastSweep.normalized, nonAllocRaycastHits, Mathf.Max(rightHeadRaycastSweep.magnitude, castRadius), gorillaTagColliderLayerMask, QueryTriggerInteraction.Collide);
		if (nonAllocHits > 0 && TryToTag(nonAllocRaycastHits[0], isBodyTag: false, out tryPlayer))
		{
			otherPlayer = tryPlayer;
		}

		// Head sweep
		float headRadius = headCollider.radius * headCollider.transform.localScale.x * GorillaLocomotion.Player.Instance.scale;
		nonAllocHits = Physics.SphereCastNonAlloc(headCollider.transform.position, headRadius, headRaycastSweep.normalized, nonAllocRaycastHits, Mathf.Max(headRaycastSweep.magnitude, castRadius), gorillaTagColliderLayerMask, QueryTriggerInteraction.Collide);
		if (nonAllocHits > 0 && TryToTag(nonAllocRaycastHits[0], isBodyTag: true, out tryPlayer))
		{
			otherPlayer = tryPlayer;
		}

		// Body capsule sweep
		topVector = lastBodyPositionForTag + bodyVector;
		bottomVector = lastBodyPositionForTag - bodyVector;
		float bodyRadius = bodyCollider.radius * 2f * GorillaLocomotion.Player.Instance.scale;
		nonAllocHits = Physics.CapsuleCastNonAlloc(topVector, bottomVector, bodyRadius, bodyRaycastSweep.normalized, nonAllocRaycastHits, Mathf.Max(bodyRaycastSweep.magnitude, castRadius), gorillaTagColliderLayerMask, QueryTriggerInteraction.Collide);
		if (nonAllocHits > 0 && TryToTag(nonAllocRaycastHits[0], isBodyTag: true, out tryPlayer))
		{
			otherPlayer = tryPlayer;
		}

		// Report tag to master client
		if (otherPlayer != null && GorillaGameManager.instance != null)
		{
			Debug.Log("tagging someone yeet");
			PhotonView.Get(GorillaGameManager.instance).RPC("ReportTagRPC", RpcTarget.MasterClient, otherPlayer);
		}

		// Find local VRRig if not set
		if (myVRRig == null && PhotonNetwork.InRoom)
		{
			foreach (VRRig vrrig in GorillaParent.instance.vrrigs)
			{
				if (vrrig != null && !vrrig.isOfflineVRRig && vrrig.photonView != null && vrrig.photonView.IsMine)
				{
					myVRRig = vrrig;
					break;
				}
			}
		}
	}

	/// <summary>
	/// Processes hand tap and slide feedback including haptics and audio.
	/// </summary>
	private void ProcessHandFeedback()
	{
		var player = GorillaLocomotion.Player.Instance;

		// Left hand feedback
		ProcessSingleHandFeedback(
			isLeftHand: true,
			ref leftHandTouching,
			ref lastLeftTap,
			leftHandSlideSource,
			player.leftHandSurfaceOverride,
			player.leftHandMaterialTouchIndex
		);

		// Right hand feedback
		ProcessSingleHandFeedback(
			isLeftHand: false,
			ref rightHandTouching,
			ref lastRightTap,
			rightHandSlideSource,
			player.rightHandSurfaceOverride,
			player.rightHandMaterialTouchIndex
		);
	}

	private void ProcessSingleHandFeedback(bool isLeftHand, ref bool handTouching, ref float lastTap, AudioSource slideSource, GorillaSurfaceOverride surfaceOverride, int materialTouchIndex)
	{
		var player = GorillaLocomotion.Player.Instance;
		bool isSliding = player.IsHandSliding(forLeftHand: isLeftHand);
		bool isTouching = player.IsHandTouching(forLeftHand: isLeftHand);

		if (!isSliding && isTouching && !handTouching && Time.time > lastTap + tapCoolDown && !player.inOverlay)
		{
			StartVibration(forLeftController: isLeftHand, tapHapticStrength, tapHapticDuration);
			tempInt = (surfaceOverride != null) ? surfaceOverride.overrideIndex : materialTouchIndex;
			if (PhotonNetwork.InRoom && myVRRig != null)
			{
				PhotonView.Get(myVRRig).RPC("PlayHandTap", RpcTarget.Others, tempInt, isLeftHand, handTapVolume);
			}
			offlineVRRig.PlayHandTapLocal(tempInt, isLeftHand: isLeftHand, handTapVolume);
			lastTap = Time.time;
		}
		else if (isSliding && !player.inOverlay)
		{
			StartVibration(forLeftController: isLeftHand, tapHapticStrength / 5f, Time.fixedDeltaTime);
			if (!slideSource.isPlaying)
			{
				slideSource.Play();
			}
		}

		if (!isSliding)
		{
			slideSource.Stop();
		}

		handTouching = isTouching;
	}

	/// <summary>
	/// Updates tracking positions for next frame's sweep calculations.
	/// </summary>
	private void UpdateTrackingPositions()
	{
		lastLeftHandPositionForTag = leftHandTransform.position;
		lastRightHandPositionForTag = rightHandTransform.position;
		lastBodyPositionForTag = bodyCollider.transform.position;
		lastHeadPositionForTag = headCollider.transform.position;
	}

	/// <summary>
	/// Handles voice chat push-to-talk and push-to-mute functionality.
	/// </summary>
	private void ProcessVoiceChat()
	{
		if (GorillaComputer.instance.voiceChatOn == "TRUE")
		{
			if (myRecorder == null)
			{
				myRecorder = PhotonNetworkController.Instance.GetComponent<Recorder>();
			}
			if (myRecorder == null) return;

			if (GorillaComputer.instance.pttType == "ALL CHAT")
			{
				if (!myRecorder.TransmitEnabled)
				{
					myRecorder.TransmitEnabled = true;
				}
				return;
			}

			// Check for any PTT button press
			bool anyButtonPressed = ControllerInputPoller.PrimaryButtonPress(XRNode.RightHand)
				|| ControllerInputPoller.SecondaryButtonPress(XRNode.RightHand)
				|| ControllerInputPoller.PrimaryButtonPress(XRNode.LeftHand)
				|| ControllerInputPoller.SecondaryButtonPress(XRNode.LeftHand);

			if (GorillaComputer.instance.pttType == "PUSH TO MUTE")
			{
				myRecorder.TransmitEnabled = !anyButtonPressed;
			}
			else if (GorillaComputer.instance.pttType == "PUSH TO TALK")
			{
				myRecorder.TransmitEnabled = anyButtonPressed;
			}
		}
		else if (myRecorder != null && myRecorder.TransmitEnabled)
		{
			myRecorder.TransmitEnabled = false;
		}
	}

	private bool TryToTag(RaycastHit hitInfo, bool isBodyTag, out Photon.Realtime.Player taggedPlayer)
	{
		if (PhotonNetwork.InRoom)
		{
			tempView = hitInfo.collider.GetComponentInParent<PhotonView>();
			if (tempView != null && PhotonNetwork.LocalPlayer != tempView.Owner && GorillaGameManager.instance != null && GorillaGameManager.instance.LocalCanTag(PhotonNetwork.LocalPlayer, tempView.Owner) && Time.time > taggedTime + tagCooldown)
			{
				if (!isBodyTag)
				{
					StartVibration(((leftHandTransform.position - hitInfo.collider.transform.position).magnitude < (rightHandTransform.position - hitInfo.collider.transform.position).magnitude) ? true : false, tagHapticStrength, tagHapticDuration);
				}
				else
				{
					StartVibration(forLeftController: true, tagHapticStrength, tagHapticDuration);
					StartVibration(forLeftController: false, tagHapticStrength, tagHapticDuration);
				}
				taggedPlayer = tempView.Owner;
				return true;
			}
		}
		taggedPlayer = null;
		return false;
	}

	public void StartVibration(bool forLeftController, float amplitude, float duration)
	{
		StartCoroutine(HapticPulses(forLeftController, amplitude, duration));
	}

	private IEnumerator HapticPulses(bool forLeftController, float amplitude, float duration)
	{
		float startTime = Time.time;
		uint channel = 0u;
		InputDevice device = ((!forLeftController) ? InputDevices.GetDeviceAtXRNode(XRNode.RightHand) : InputDevices.GetDeviceAtXRNode(XRNode.LeftHand));
		while (Time.time < startTime + duration)
		{
			device.SendHapticImpulse(channel, amplitude, hapticWaitSeconds);
			yield return new WaitForSeconds(hapticWaitSeconds * 0.9f);
		}
	}

	public void UpdateColor(float red, float green, float blue)
	{
		if (GorillaComputer.instance != null)
		{
			offlineVRRig.InitializeNoobMaterialLocal(red, green, blue, GorillaComputer.instance.leftHanded);
		}
		else
		{
			offlineVRRig.InitializeNoobMaterialLocal(red, green, blue, leftHanded: false);
		}
		offlineVRRig.mainSkin.material = offlineVRRig.materialsToChangeTo[0];
	}

	protected void OnTriggerEnter(Collider other)
	{
		if (other == null || other.gameObject == null) return;

		// Check for direct GorillaTriggerBox component (layer 15)
		if (PhotonNetwork.InRoom && other.gameObject.layer == 15)
		{
			var directTriggerBox = other.gameObject.GetComponent<GorillaTriggerBox>();
			if (directTriggerBox != null)
			{
				directTriggerBox.OnBoxTriggered();
			}
		}

		// Check for child GorillaTriggerBox
		var childTriggerBox = other.GetComponentInChildren<GorillaTriggerBox>();
		if (childTriggerBox != null)
		{
			childTriggerBox.OnBoxTriggered();
		}
		else
		{
			// Check for parent GorillaTrigger
			var parentTrigger = other.GetComponentInParent<GorillaTrigger>();
			if (parentTrigger != null)
			{
				parentTrigger.OnTriggered();
			}
		}
	}

	public void ShowCosmeticParticles(bool showParticles)
	{
		if (showParticles)
		{
			mainCameraComponent.cullingMask |= cosmeticParticleLayerMask;
			mirrorCamera.cullingMask |= cosmeticParticleLayerMask;
		}
		else
		{
			mainCameraComponent.cullingMask &= ~cosmeticParticleLayerMask;
			mirrorCamera.cullingMask &= ~cosmeticParticleLayerMask;
		}
	}

	public void ApplyStatusEffect(StatusEffect newStatus, float duration)
	{
		EndStatusEffect(currentStatus);
		currentStatus = newStatus;
		statusEndTime = Time.time + duration;
		switch (newStatus)
		{
		case StatusEffect.Frozen:
			GorillaLocomotion.Player.Instance.disableMovement = true;
			break;
		case StatusEffect.None:
		case StatusEffect.Slowed:
			break;
		}
	}

	private void CheckEndStatusEffect()
	{
		if (Time.time > statusEndTime)
		{
			EndStatusEffect(currentStatus);
		}
	}

	private void EndStatusEffect(StatusEffect effectToEnd)
	{
		switch (effectToEnd)
		{
		case StatusEffect.Frozen:
			GorillaLocomotion.Player.Instance.disableMovement = false;
			currentStatus = StatusEffect.None;
			break;
		case StatusEffect.Slowed:
			currentStatus = StatusEffect.None;
			break;
		case StatusEffect.None:
			break;
		}
	}

	private float CalcSlideControl(float fps)
	{
		return Mathf.Pow(Mathf.Pow(1f - baseSlideControl, 120f), 1f / fps);
	}
}
