using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;
using Photon.Realtime;
using Photon.Voice.Unity;

namespace DevBlinkMod.Scripts
{
    public class FaceManager : MonoBehaviourPunCallbacks
    {
        public VRRig rig;
        public Renderer faceRenderer;
        public Material _faceMaterial;
        public Texture talkTexture;
        public Texture faceSheetTexture;

        private float offsetX = 0f;
        private float offsetY = 0f;
        private Vector2 mainTextureOffset;

        private float faceTime;
        private float FaceDelay => Time.time + Random.Range(2f, 7.5f);

        private const float faceCooldownClosed = 0.22f;
        private const float faceCooldown = 0.10f;

        public FaceState currentFaceState = FaceState.Idle;
        private FaceState lastFaceState = FaceState.Idle;
        private FaceState stateBeforeTalking = FaceState.Idle;

        private Recorder recorder;
        private PhotonView photonView;

        public enum FaceState
        {
            Idle,
            IdleHalf,
            BlinkClosed,
            BlinkHalf,
            Talking
        }

        private void OnEnable()
        {
            photonView = GetComponent<PhotonView>();
            rig = GetComponent<VRRig>();
            if (rig == null)
            {
                Debug.LogWarning("VRRig not found on faceManager GameObject.");
                return;
            }

            faceRenderer = rig.headMesh.transform.Find("gorillaface")?.GetComponent<Renderer>();
            if (faceRenderer == null)
            {
                Debug.LogWarning("Face Renderer not found.");
                return;
            }

            if (_faceMaterial != null)
            {
                _faceMaterial.mainTexture = faceSheetTexture;
                _faceMaterial.mainTextureScale = new Vector2(1 / 3f, 1f);
                _faceMaterial.mainTextureOffset = Vector2.zero;
                faceRenderer.material = _faceMaterial;
            }

            if (photonView.IsMine)
            {
                recorder = GameObject.Find("Photon Manager")?.GetComponent<Recorder>();
                if (recorder == null)
                    Debug.LogWarning("Photon Recorder not found on local player.");
            }

            faceTime = FaceDelay;
        }

        private void LateUpdate()
        {
            if (rig == null || faceRenderer == null || _faceMaterial == null)
                return;

            if (!photonView.IsMine)
                return;

            bool isTalking = recorder != null && recorder.IsCurrentlyTransmitting;

            if (isTalking)
            {
                if (currentFaceState != FaceState.Talking)
                {
                    stateBeforeTalking = currentFaceState;
                    currentFaceState = FaceState.Talking;
                    faceTime = Time.time + faceCooldown;

                    photonView.RPC(nameof(RPC_SetTalkingFace), RpcTarget.AllBuffered);
                }
                return;
            }

            if (currentFaceState == FaceState.Talking)
            {
                currentFaceState = stateBeforeTalking;
                faceTime = Time.time + faceCooldown;

                photonView.RPC(nameof(RPC_SetFaceState), RpcTarget.AllBuffered, (int)currentFaceState);
            }

            if (Time.time >= faceTime)
            {
                switch (currentFaceState)
                {
                    case FaceState.Idle:
                        currentFaceState = FaceState.IdleHalf;
                        offsetX = 1 / 3f;
                        faceTime = Time.time + faceCooldown * 0.7f;
                        break;

                    case FaceState.IdleHalf:
                        currentFaceState = FaceState.BlinkClosed;
                        offsetX = 1 / 3f * 2;
                        faceTime = Time.time + faceCooldown;
                        break;

                    case FaceState.BlinkClosed:
                        currentFaceState = FaceState.BlinkHalf;
                        offsetX = 1 / 3f * 3;
                        faceTime = Time.time + faceCooldown;
                        break;

                    case FaceState.BlinkHalf:
                        currentFaceState = FaceState.Idle;
                        offsetX = 0f;
                        faceTime = FaceDelay;
                        break;
                }

                photonView.RPC(nameof(RPC_SetFaceState), RpcTarget.AllBuffered, (int)currentFaceState);
            }
        }

        [PunRPC]
        public void RPC_SetTalkingFace()
        {
            _faceMaterial.mainTexture = talkTexture;
            _faceMaterial.mainTextureScale = new Vector2(1f, 1f);
            _faceMaterial.mainTextureOffset = Vector2.zero;
            faceRenderer.material = _faceMaterial;
        }

        [PunRPC]
        public void RPC_SetFaceState(int state)
        {
            currentFaceState = (FaceState)state;

            if (_faceMaterial.mainTexture != faceSheetTexture)
            {
                _faceMaterial.mainTexture = faceSheetTexture;
                _faceMaterial.mainTextureScale = new Vector2(1 / 3f, 1f);
            }

            switch (currentFaceState)
            {
                case FaceState.Idle: offsetX = 0f; break;
                case FaceState.IdleHalf: offsetX = 1 / 3f; break;
                case FaceState.BlinkClosed: offsetX = 1 / 3f * 2; break;
                case FaceState.BlinkHalf: offsetX = 1 / 3f * 3; break;
            }

            mainTextureOffset = new Vector2(offsetX, offsetY);
            _faceMaterial.mainTextureOffset = mainTextureOffset;
            faceRenderer.material = _faceMaterial;
        }
    }
}