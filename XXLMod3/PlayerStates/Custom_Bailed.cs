using System;
using System.Collections.Generic;
using FSMHelper;
using RootMotion.Dynamics;
using UnityEngine;
using XXLModCV.Controller;
using XXLModCV.Core;

namespace XXLModCV.PlayerStates
{
    public class Custom_Bailed : PlayerState_OffBoard
    {
        private bool _isExiting;
        private bool _isGrounded;
        private int count = 0;

        private float _fallBlend;
        private float _fallTarget;
        private float _hipForwardAngle;
        private float _hipForwardLerp;
        private float _hipUp;
        private float _hipUpLerp;
        private float _hipXAngle;
        private float _hipYAngle;
        private float _hipXLerp;
        private float _hipYLerp;

        private int _frames;

        private bool run = false;

        private RaycastHit[] _rayCasts = new RaycastHit[1];
        private Rigidbody _hips;

        public override void SetupDefinition(ref FSMStateType stateType, ref List<Type> children)
        {
            stateType = FSMStateType.Type_OR;
        }

        List<GameObject> list = new List<GameObject>();
        List<Collider> list2 = new List<Collider>();
        public void createPrimitive(Collider collider)
        {
            GameObject go = GameObject.CreatePrimitive(PrimitiveType.Cube);
            go.GetComponent<MeshRenderer>().material.shader = Shader.Find("HDRP/Lit");
            go.GetComponent<Collider>().enabled = false;
            go.transform.position = collider.transform.position;
            go.transform.localScale = new Vector3(.1f, .1f, .1f);
            go.transform.rotation = collider.transform.rotation;
            // Logger.Log(collider.bounds.size.x.ToString());
            go.AddComponent<ObjectTracker>();
            go.gameObject.AddComponent<ObjectTracker>();
            list.Add(go);
            list2.Add(collider);
        }

        public void updatePrimitives()
        {
            int count = 0;
            foreach(GameObject go in list)
            {
                Collider col = list2[count];
                go.transform.position = col.transform.position;
                go.transform.rotation = col.transform.rotation;
                count++;
            }
        }

        public override void Enter()
        {
            count = 0;
            PlayerController.Instance.currentStateEnum = PlayerController.CurrentState.Bailed;
            XXLController.CurrentState = CurrentState.Bailed;
            XXLController.Instance.FlipDetected = false;

            // PlayerController.Instance.AnimRelease(true);
            /*Vector3 force = Vector3.up * 160f;
            PlayerController.Instance.respawn.behaviourPuppet.puppetMaster.muscles[0].rigidbody.AddRelativeForce(force.x, force.y, force.z, ForceMode.Impulse);*/
            // PlayerController.Instance.respawn.behaviourPuppet.puppetMaster.muscles[1].rigidbody.AddRelativeForce(30f, 20f, 0f, ForceMode.Impulse);
            // PlayerController.Instance.respawn.behaviourPuppet.puppetMaster.muscles[2].rigidbody.AddRelativeForce(-250f, -60f, 0f, ForceMode.Impulse);
            // PlayerController.Instance.respawn.behaviourPuppet.puppetMaster.muscles[3].rigidbody.AddRelativeForce(0f, 60f, 90f, ForceMode.Impulse);

            /*PlayerController.Instance.respawn.behaviourPuppet.puppetMaster.muscles[12].rigidbody.AddRelativeForce(80f, -40f, 30f, ForceMode.Impulse);
            PlayerController.Instance.respawn.behaviourPuppet.puppetMaster.muscles[15].rigidbody.AddRelativeForce(-80f, -40f, 30f, ForceMode.Impulse);*/

            PlayerController.Instance.respawn.behaviourPuppet.puppetMaster.muscles[12].rigidbody.mass = 10f;
            PlayerController.Instance.respawn.behaviourPuppet.puppetMaster.muscles[15].rigidbody.mass = 10f;

            PlayerController.Instance.respawn.behaviourPuppet.puppetMaster.muscles[3].transform.LookAt(PlayerController.Instance.boardController.transform.position);

            setMotionType(PlayerController.Instance.respawn.behaviourPuppet.puppetMaster.muscles[3], ConfigurableJointMotion.Free);
            setMotionType(PlayerController.Instance.respawn.behaviourPuppet.puppetMaster.muscles[4], ConfigurableJointMotion.Limited);
            setMotionType(PlayerController.Instance.respawn.behaviourPuppet.puppetMaster.muscles[5], ConfigurableJointMotion.Limited);
            setMotionType(PlayerController.Instance.respawn.behaviourPuppet.puppetMaster.muscles[6], ConfigurableJointMotion.Limited);
            setMotionType(PlayerController.Instance.respawn.behaviourPuppet.puppetMaster.muscles[7], ConfigurableJointMotion.Limited);
            setMotionType(PlayerController.Instance.respawn.behaviourPuppet.puppetMaster.muscles[8], ConfigurableJointMotion.Limited);
            setMotionType(PlayerController.Instance.respawn.behaviourPuppet.puppetMaster.muscles[9], ConfigurableJointMotion.Limited);
            setMotionType(PlayerController.Instance.respawn.behaviourPuppet.puppetMaster.muscles[10], ConfigurableJointMotion.Limited);
            setMotionType(PlayerController.Instance.respawn.behaviourPuppet.puppetMaster.muscles[11], ConfigurableJointMotion.Limited);
            setMotionType(PlayerController.Instance.respawn.behaviourPuppet.puppetMaster.muscles[12], ConfigurableJointMotion.Limited);
            setMotionType(PlayerController.Instance.respawn.behaviourPuppet.puppetMaster.muscles[13], ConfigurableJointMotion.Limited);
            setMotionType(PlayerController.Instance.respawn.behaviourPuppet.puppetMaster.muscles[14], ConfigurableJointMotion.Limited);
            setMotionType(PlayerController.Instance.respawn.behaviourPuppet.puppetMaster.muscles[15], ConfigurableJointMotion.Limited);

            for (int i = 0; i < PlayerController.Instance.respawn.behaviourPuppet.puppetMaster.muscles.Length; i++)
            {
                /*Logger.Log(i.ToString());*/
                Logger.Log(PlayerController.Instance.respawn.behaviourPuppet.puppetMaster.muscles[i].joint.name);
                //Logger.Log(PlayerController.Instance.respawn.behaviourPuppet.puppetMaster.muscles[i].joint.xMotion.ToString());
                /*Logger.Log(PlayerController.Instance.respawn.behaviourPuppet.puppetMaster.muscles[i].joint.yMotion.ToString());
                Logger.Log(PlayerController.Instance.respawn.behaviourPuppet.puppetMaster.muscles[i].joint.zMotion.ToString());
                Logger.Log(PlayerController.Instance.respawn.behaviourPuppet.puppetMaster.muscles[i].joint.linearLimit.ToString());*/
                PlayerController.Instance.respawn.behaviourPuppet.puppetMaster.muscles[i].rigidbody.solverIterations = 20;
                PlayerController.Instance.respawn.behaviourPuppet.puppetMaster.muscles[i].rigidbody.mass = 3f;
            }

            if (Main.settings.BailRespawnAt)
            {
                Utils.InvokeMethod(PlayerController.Instance.respawn, "SetSpawnPos");
            }
            if (Main.settings.BetterBails)
            {
                XXLController.Instance.SetMuscleWeight(3f);
            }

            PlayerController.Instance.respawn.puppetMaster.DisableTeleport();
            _hips = PlayerController.Instance.respawn.puppetMaster.muscles[0].rigidbody;

            EventManager.Instance.OnBail();
            PlayerController.Instance.respawn.puppetMaster.Kill();
            PlayerController.Instance.ToggleFlipColliders(false);

            GroundLogic();
            PlayerController.Instance.SetBoardPhysicsMaterial(PlayerController.FrictionType.Bail);
            PlayerController.Instance.RagdollLayerChange(true);

            _fallTarget = 1f;
            _fallBlend = .7f;
            PlayerController.Instance.animationController.SetValue("FallBlend", _fallBlend);
            PlayerController.Instance.skaterController.rightFootCollider.isTrigger = false;
            PlayerController.Instance.skaterController.leftFootCollider.isTrigger = false;
            PlayerController.Instance.SetKneeIKTargetWeight(1000f);
            PlayerController.Instance.Invoke("DoBail", 6f);
            PlayerController.Instance.skaterController.skaterRigidbody.useGravity = true;
            PlayerController.Instance.respawn.behaviourPuppet.defaults.minMappingWeight = 1f;
            PlayerController.Instance.respawn.behaviourPuppet.masterProps.normalMode = BehaviourPuppet.NormalMode.Active;
            InitializeBailAnimInfo();

            PlayerController.Instance.animationController.skaterAnim.CrossFade("Falling", _fallBlend);

            XXLController.Instance.ActivateSlowMotion(Main.settings.SlowMotionBails, Main.settings.SlowMotionBailSpeed);
        }

        private void CustomEnableArmPhysics()
        {
            float _lerp = 0f;
            while (_lerp < 2f)
            {
                _lerp += Time.deltaTime * 2f;
                float multi = _lerp * 10f;
                PlayerController.Instance.respawn.behaviourPuppet.groupOverrides[0].props.minMappingWeight = -multi;
                PlayerController.Instance.respawn.behaviourPuppet.groupOverrides[0].props.maxMappingWeight = multi;
                PlayerController.Instance.respawn.behaviourPuppet.groupOverrides[1].props.minMappingWeight = multi;
                PlayerController.Instance.respawn.behaviourPuppet.groupOverrides[1].props.maxMappingWeight = multi;
                PlayerController.Instance.respawn.behaviourPuppet.groupOverrides[2].props.minMappingWeight = -multi;
                PlayerController.Instance.respawn.behaviourPuppet.groupOverrides[2].props.maxMappingWeight = multi;
                PlayerController.Instance.respawn.behaviourPuppet.groupOverrides[3].props.minMappingWeight = -multi;
                PlayerController.Instance.respawn.behaviourPuppet.groupOverrides[3].props.maxMappingWeight = multi;
                PlayerController.Instance.respawn.behaviourPuppet.groupOverrides[4].props.minMappingWeight = -multi;
                PlayerController.Instance.respawn.behaviourPuppet.groupOverrides[4].props.maxMappingWeight = -multi;
                PlayerController.Instance.respawn.behaviourPuppet.groupOverrides[5].props.minMappingWeight = multi;
                PlayerController.Instance.respawn.behaviourPuppet.groupOverrides[5].props.maxMappingWeight = multi;
            }
        }

        private void setMotionType(Muscle muscle, ConfigurableJointMotion joint)
        {
            muscle.joint.xMotion = joint;
            muscle.joint.yMotion = joint;
            muscle.joint.zMotion = joint;
            SoftJointLimit limit = new UnityEngine.SoftJointLimit();
            limit.bounciness = 1;
            /*limit.contactDistance = 0;*/
            /*limit.limit = 45f;*/
            muscle.joint.linearLimit = limit;
            /*muscle.joint.enableCollision = true;*/
        }

        public override void Exit()
        {
            XXLController.Instance.ResetTime(Main.settings.SlowMotionBails);
            _isExiting = true;
            if (Main.settings.BetterBails)
            {
                XXLController.Instance.ResetMuscleWeight();
            }
            PlayerController.Instance.RagdollLayerChange(true);
            PlayerController.Instance.SetKneeBendWeightManually(0f);
            PlayerController.Instance.respawn.puppetMaster.pinWeight = 1.75f;
            PlayerController.Instance.respawn.puppetMaster.muscleWeight = 1.75f;
            PlayerController.Instance.respawn.behaviourPuppet.defaults.minMappingWeight = 1f;
            PlayerController.Instance.respawn.behaviourPuppet.masterProps.normalMode = BehaviourPuppet.NormalMode.Unmapped;
            PlayerController.Instance.SetBoardPhysicsMaterial(PlayerController.FrictionType.Default);

            setMotionType(PlayerController.Instance.respawn.behaviourPuppet.puppetMaster.muscles[3], ConfigurableJointMotion.Locked);
            setMotionType(PlayerController.Instance.respawn.behaviourPuppet.puppetMaster.muscles[10], ConfigurableJointMotion.Locked);
            setMotionType(PlayerController.Instance.respawn.behaviourPuppet.puppetMaster.muscles[11], ConfigurableJointMotion.Locked);
            setMotionType(PlayerController.Instance.respawn.behaviourPuppet.puppetMaster.muscles[12], ConfigurableJointMotion.Locked);
            setMotionType(PlayerController.Instance.respawn.behaviourPuppet.puppetMaster.muscles[13], ConfigurableJointMotion.Locked);
            setMotionType(PlayerController.Instance.respawn.behaviourPuppet.puppetMaster.muscles[14], ConfigurableJointMotion.Locked);
            setMotionType(PlayerController.Instance.respawn.behaviourPuppet.puppetMaster.muscles[15], ConfigurableJointMotion.Locked);
        }

        public override void Update()
        {
            if (_isExiting)
            {
                return;
            }
            base.Update();
            if (Main.settings.BailControls)
            {
                if (InputController.Instance.player.GetButton("RB"))
                {
                    PlayerController.Instance.respawn.behaviourPuppet.puppetMaster.muscles[2].rigidbody.AddForce(0, Main.settings.BailUpForce, 0f, ForceMode.Impulse);
                }
                if (InputController.Instance.player.GetButton("LB"))
                {
                    PlayerController.Instance.respawn.behaviourPuppet.puppetMaster.muscles[12].rigidbody.AddForce(0, -Main.settings.BailDownForce, 0f, ForceMode.Impulse);
                    PlayerController.Instance.respawn.behaviourPuppet.puppetMaster.muscles[15].rigidbody.AddForce(0, -Main.settings.BailDownForce, 0f, ForceMode.Impulse);
                }
                if(InputController.Instance.player.GetAxis("LT") > 0.1f)
                {
                    PlayerController.Instance.respawn.behaviourPuppet.puppetMaster.muscles[6].rigidbody.AddForce(0, Main.settings.BailArmForce, 0f, ForceMode.Impulse);
                }
                if (InputController.Instance.player.GetAxis("RT") > 0.1f)
                {
                    PlayerController.Instance.respawn.behaviourPuppet.puppetMaster.muscles[9].rigidbody.AddForce(0, Main.settings.BailArmForce, 0f, ForceMode.Impulse);
                }
                if (InputController.Instance.player.GetButton("Left Stick Button"))
                {
                    PlayerController.Instance.respawn.behaviourPuppet.puppetMaster.muscles[12].rigidbody.AddForce(0, Main.settings.BailLegForce, 0f, ForceMode.Impulse);
                }
                if (InputController.Instance.player.GetButton("Right Stick Button"))
                {
                    PlayerController.Instance.respawn.behaviourPuppet.puppetMaster.muscles[15].rigidbody.AddForce(0, Main.settings.BailLegForce, 0f, ForceMode.Impulse);
                }
            }
            PlayerController.Instance.LerpKneeIkWeight();
            UpdateBailAnimInfo();
            if (Physics.RaycastNonAlloc(_hips.position, -Vector3.up, _rayCasts, 1f, 8) > 0)
            {
                _fallTarget = 0f;
                PlayerController.Instance.respawn.puppetMaster.pinWeight = 0.01f;
                PlayerController.Instance.respawn.puppetMaster.muscleWeight = 0.1f;
            }
            else
            {
                _fallTarget = 1f;
            }
            _fallBlend = Mathf.Lerp(_fallBlend, _fallTarget, Time.fixedDeltaTime * Main.settings.Gravity);
            PlayerController.Instance.animationController.SetValue("FallBlend", _fallBlend);

            if(count % 16 == 0 && count <= (16 * 12))
            {
                PlayerController.Instance.animationController.skaterAnim.CrossFade("Falling", .166f);
            }
            count++;
        }

        private void InitializeBailAnimInfo()
        {
            calculateVelocity();
            _hipUpLerp = _hipUp;
            _hipForwardLerp = _hipForwardAngle;
            _hipXLerp = _hipXAngle;
            _hipYLerp = _hipYAngle;
            setAnimationValues(_hipXAngle, _hipYAngle, _hipUp, _hipForwardAngle, Vector3.ProjectOnPlane(_hips.velocity, Vector3.up).magnitude);
        }

        private void UpdateBailAnimInfo()
        {
            calculateVelocity();            
            _hipUpLerp = Mathf.MoveTowards(_hipUpLerp, _hipUp, Time.deltaTime * 50f);
            _hipForwardLerp = Mathf.MoveTowards(_hipForwardLerp, _hipForwardAngle, Time.deltaTime * 50f);
            _hipXLerp = Mathf.MoveTowards(_hipXLerp, _hipXAngle, Time.deltaTime * 50f);
            _hipYLerp = Mathf.MoveTowards(_hipYLerp, _hipYAngle, Time.deltaTime * 50f);
            setAnimationValues(_hipXLerp, _hipYLerp, _hipUpLerp, _hipForwardAngle, Vector3.ProjectOnPlane(_hips.velocity, Vector3.up).magnitude);
        }

        private void calculateVelocity()
        {
            Vector3 velocity = _hips.velocity / 1.4f;
            /*velocity.y = 0f;*/
            Vector3 vector = Quaternion.AngleAxis(90f, Vector3.up) * velocity;
            Vector3 from = Vector3.ProjectOnPlane(_hips.transform.up + -_hips.transform.right, Vector3.up);
            Vector3 from2 = Vector3.ProjectOnPlane(_hips.transform.up, vector);
            _hipYAngle = Vector3.SignedAngle(from, velocity, Vector3.up);
            _hipXAngle = Vector3.SignedAngle(from2, velocity, vector);
            _hipUp = Vector3.Angle(-_hips.transform.right, Vector3.up);
            _hipForwardAngle = Vector3.SignedAngle(_hips.transform.up, Vector3.up, -_hips.transform.right);
        }

        private void setAnimationValues(float hipX, float hipY, float hipUp, float hipForward, float bailVel)
        {
            PlayerController.Instance.animationController.SetValue("hipX", hipX);
            PlayerController.Instance.animationController.SetValue("hipY", hipY);
            PlayerController.Instance.animationController.SetValue("hipUp", hipUp);
            PlayerController.Instance.animationController.SetValue("hipForward", hipForward);
            PlayerController.Instance.animationController.SetValue("bailVel", bailVel);
        }

        public override void FixedUpdate()
        {
            if (_isExiting)
            {
                return;
            }
            base.FixedUpdate();
            GroundLogic();
            CustomEnableArmPhysics();

            /*if(run == false)
            {
                for (int i = 0; i < PlayerController.Instance.respawn.behaviourPuppet.puppetMaster.muscles.Length; i++)
                {
                    for (int y = 0; y < PlayerController.Instance.respawn.behaviourPuppet.puppetMaster.muscles[i].colliders.Length; y++)
                    {
                        Logger.Log(PlayerController.Instance.respawn.behaviourPuppet.puppetMaster.muscles[i].colliders[y].bounds.size.ToString());
                        // PlayerController.Instance.respawn.behaviourPuppet.puppetMaster.muscles[i].colliders[y].transform.localScale = new Vector3(1f, 1f, 1f);
                        createPrimitive(PlayerController.Instance.respawn.behaviourPuppet.puppetMaster.muscles[i].colliders[y]);
                    }
                }
                run = true;
            }
            else
            {
                updatePrimitives();
            }*/
        }

        private void GroundLogic()
        {
            if (PlayerController.Instance.IsGrounded())
            {
                if (!_isGrounded)
                {
                    _isGrounded = true;
                    PlayerController.Instance.SetBoardPhysicsMaterial(PlayerController.FrictionType.Default);
                }
                Vector3 vector = PlayerController.Instance.boardController.boardTransform.InverseTransformDirection(PlayerController.Instance.boardController.boardRigidbody.angularVelocity);
                vector.y = Mathf.Lerp(vector.y, 0f, Time.deltaTime * (Main.settings.Gravity * -1f));
                PlayerController.Instance.boardController.boardRigidbody.angularVelocity = PlayerController.Instance.boardController.boardTransform.TransformDirection(vector);
                PlayerController.Instance.ApplyFriction();
                SoundManager.Instance.SetRollingVolumeFromRPS(PlayerController.Instance.GetSurfaceTag(PlayerController.Instance.boardController.GetSurfaceTagString()), PlayerController.Instance.boardController.boardRigidbody.velocity.magnitude);
                return;
            }
            if (_isGrounded)
            {
                _isGrounded = false;
                PlayerController.Instance.SetBoardPhysicsMaterial(PlayerController.FrictionType.Bail);
            }
            SoundManager.Instance.SetRollingVolumeFromRPS(PlayerController.Instance.GetSurfaceTag(PlayerController.Instance.boardController.GetSurfaceTagString()), PlayerController.Instance.boardController._rollSoundSpeed);
        }

        public override void OnCollisionEnterEvent(Vector3 _impulse, bool _isBoard, Collision c)
        {
            SoundManager.Instance.PlayBoardHit(_impulse.magnitude);
        }

        public override bool IsInBailState()
        {
            return true;
        }

        public override void OnRespawn()
        {
            base.OnRespawn();
        }
    }
}