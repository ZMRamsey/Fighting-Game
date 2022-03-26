using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AIBrain : MonoBehaviour
{
    LayerMask _netDetection;
    LayerMask _stageDetection;
    float _tickRate = 0.05f;
    float _xMovementOffset = 1f;
    float _tick;

    bool _wantToJump;

    InputHandler _handler;
    ShuttleCock _shuttle;

    RaycastHit _vision;
    Vector3 _verticalTarget;
    Vector3 _horizontalTarget;

    FighterController _controller;
    FighterController _enemy;

    EsmeFighter _esme;
    RacketFighter _raket;
    RayAndTekaFighter _ray;

    void Start() {
        _netDetection = LayerMask.GetMask("Net");
        _stageDetection = LayerMask.GetMask("Stage");
        _shuttle = GameManager.Get().GetShuttle();
        _handler = GetComponent<InputHandler>();
        _controller = GetComponent<FighterController>();
        _esme = GetComponent<EsmeFighter>();
        _raket = GetComponent<RacketFighter>();
        _ray = GetComponent<RayAndTekaFighter>();
        _enemy = GameManager.Get().GetFighterOne();
        if (_controller == _enemy) {
            _enemy = GameManager.Get().GetFighterTwo();
        }
    }

    void Update() {
        if (Time.timeScale <= 0 || !GameManager.Get().IsGameActive()) {
            return;
        }

        _tick += Time.deltaTime;

        if (_tick > _tickRate) {
            _verticalTarget = _shuttle.transform.position + _shuttle.GetVelocity();
            _horizontalTarget = _verticalTarget;
            _verticalTarget.y = 0;
            _horizontalTarget.y = Mathf.Clamp(_horizontalTarget.y, 0, _shuttle.transform.position.y);

            if (_controller.GetFilter() == FighterFilter.one) {
                _verticalTarget.x = Mathf.Clamp(_verticalTarget.x, _shuttle.transform.position.x, Mathf.Infinity);
            }
            else {
                _verticalTarget.x = Mathf.Clamp(_verticalTarget.x, -Mathf.Infinity, _shuttle.transform.position.x);
            }

            var grabbed = _shuttle.IsGrabbed(_controller);

            if (grabbed) {
                _verticalTarget.x = _enemy.transform.position.x;
            }

            ClampMovementTarget();

            MovementSuperAvoidance();

            ProcessMovement();

            if(_raket != null) {
                ProcessRaket();
            }

            if (grabbed) {
                _handler._jumpHeld = true;
                _handler._jumpInput = true;

                if (transform.position.y > 4) {
                    grabbed = false;
                }
            }

            Vector3 hitOffset = Vector3.right * 0.5f;

            if(_controller.GetFilter() == FighterFilter.one) {
                hitOffset = -Vector3.right * 0.5f;
            }


            if (!grabbed && Vector3.Distance(transform.position + hitOffset, _shuttle.transform.position) < Mathf.Clamp(Mathf.Sqrt(_shuttle.GetVelocity().magnitude), 2, 20)) {
                ProcessHit();
            }

            _tick = 0.0f;
        }

        //bool moveAwayOverride = false;
        //bool inRangeOfSubWoofer = false;
        //bool inHittingRangeOfSubWoofer = false;
        //bool isWooferOnMySide = false;
        //bool isTimeToMove = false;

        //tick += Time.deltaTime;
        //float dist = Vector3.Distance(transform.position, _shuttle.transform.position);
        //float time = dist / _controller.GetSpeed();
        //targetPosition = _shuttle.transform.position + _shuttle.GetVelocity() / 4;
        //targetPosition.y = Mathf.Clamp(targetPosition.y, 1, 10);

        //float shuttleXDist = Vector3.Distance(new Vector3(transform.position.x, transform.position.y, 0), new Vector3(_shuttle.transform.position.x, transform.position.y, 0));

        //if (_controller.GetFilter() == FighterFilter.one) {
        //    targetPosition.x += 0.5f;
        //}
        //else {
        //    targetPosition.x -= 0.5f;
        //}

        //if (tick > 0.05f && GameManager.Get().IsGameActive() && !GameManager.Get().IsInKO() && _shuttle.GetVelocity().magnitude > 1) {
        //    _handler._inputX = 0;
        //    RaycastHit netDetection;

        //    Collider[] colliders = Physics.OverlapSphere(transform.position, 4);
        //    foreach (Collider hit in colliders) {
        //        SubWoofer woofer = hit.GetComponent<SubWoofer>();


        //        if (woofer != null) {
        //            if (IsOnMySide(woofer.transform)) {
        //                isWooferOnMySide = true;
        //            }

        //            isTimeToMove = woofer.GetTimer() > 3.5f;

        //            inRangeOfSubWoofer = Vector3.Distance(transform.position, woofer.transform.position) < 6f;
        //            inHittingRangeOfSubWoofer = Vector3.Distance(transform.position, woofer.transform.position) < 1f;

        //            if (!isTimeToMove && IsOnMySide(woofer.transform) && !IsOnMySide()) {
        //                if (woofer.transform.position.x < transform.position.x) {
        //                    _handler._inputX = -1;
        //                }

        //                if (woofer.transform.position.x > transform.position.x) {
        //                    _handler._inputX = 1;
        //                }
        //            }
        //            else {
        //                if (inRangeOfSubWoofer && IsOnMySide(woofer.transform) && _controller.GetFilter() == FighterFilter.one) {
        //                    _handler._inputX = -1;
        //                    if (Physics.Raycast(transform.position, new Vector3(-1, 0, 0), out netDetection, 2, _netDetection)) {
        //                        _handler._jumpHeld = true;
        //                        _handler._jumpInput = true;
        //                    }
        //                }

        //                if (inRangeOfSubWoofer && !IsOnMySide(woofer.transform) && _controller.GetFilter() == FighterFilter.one) {
        //                    _handler._inputX = 1;
        //                    if (Physics.Raycast(transform.position, new Vector3(1, 0, 0), out netDetection, 2, _netDetection)) {
        //                        _handler._jumpHeld = true;
        //                        _handler._jumpInput = true;
        //                    }
        //                }


        //                if (inRangeOfSubWoofer && IsOnMySide(woofer.transform) && _controller.GetFilter() == FighterFilter.two) {
        //                    _handler._inputX = 1;
        //                    if (Physics.Raycast(transform.position, new Vector3(1, 0, 0), out netDetection, 2, _netDetection)) {
        //                        _handler._jumpHeld = true;
        //                        _handler._jumpInput = true;
        //                    }
        //                }

        //                if (inRangeOfSubWoofer && !IsOnMySide(woofer.transform) && _controller.GetFilter() == FighterFilter.two) {
        //                    _handler._inputX = -1;
        //                    if (Physics.Raycast(transform.position, new Vector3(-1, 0, 0), out netDetection, 2, _netDetection)) {
        //                        _handler._jumpHeld = true;
        //                        _handler._jumpInput = true;
        //                    }
        //                }
        //            }

        //            moveAwayOverride = true;
        //        }
        //    }


        //    if (!moveAwayOverride) {
        //        if (IsOnMySide() || HeadingMyDirection()) {
        //            _handler._jumpHeld = IsBallAbovePlayer() && transform.position.y < _shuttle.transform.position.y;
        //            _handler._jumpInput = IsBallAbovePlayer();
        //        }

        //        if (IsOnMySide() || HeadingMyDirection() || _controller.InSuper()) {

        //            if (IsBallOnRight()) {
        //                _handler._inputX = 1;
        //            }

        //            if (IsBallOnLeft()) {
        //                _handler._inputX = -1;
        //            }

        //        }

        //        if (!IsOnMySide() && !IsOnMySide(transform) && !isWooferOnMySide && Vector3.Distance(transform.position, _shuttle.transform.position) > 2) {

        //            if (_controller.GetFilter() == FighterFilter.one) {
        //                _handler._inputX = 1;
        //                if (Physics.Raycast(transform.position, new Vector3(2, 0, 0), out netDetection, 2, _netDetection)) {
        //                    _handler._jumpHeld = true;
        //                    _handler._jumpInput = true;
        //                }
        //            }
        //            else {
        //                _handler._inputX = -1;
        //                if (Physics.Raycast(transform.position, new Vector3(-2, 0, 0), out netDetection, 2, _netDetection)) {
        //                    _handler._jumpHeld = true;
        //                    _handler._jumpInput = true;
        //                }
        //            }
        //        }
        //    }

        //    if (_esme != null) {
        //        _handler._crouchInput = false;
        //        if (_esme.GetComponent<Rigidbody>().velocity.y < 0 && IsBallAbovePlayer()) {
        //            _handler._jumpExtraInput = true;
        //        }

        //        if (_esme.CanGhostShot() && _controller.GetGrounded() && Vector3.Distance(transform.position, targetPosition) > 4f) {
        //            _handler._crouchInput = true;
        //            ProcessHit();
        //        }

        //        if (!IsOnMySide() && _esme.GetMeter() >= 1) {
        //            _handler._specialInput = true;
        //        }
        //    }

        //    if (_raket != null) {
        //        _handler._crouchInput = false;

        //        if (_raket.GetBuildMeter() < 1f && !IsOnMySide() && !HeadingMyDirection() && !isTimeToMove && !inRangeOfSubWoofer) {
        //            _handler._crouchInput = true;
        //        }

        //        if (_raket.GetBuildMeter() >= 1) {
        //            _handler._crouchInput = true;
        //            _handler._chipInput = true;
        //        }

        //        if (!IsOnMySide() && _raket.GetMeter() >= 1) {
        //            _handler._jumpHeld = true;
        //            _handler._jumpInput = true;
        //            if (transform.position.y > 6) {
        //                _handler._specialInput = true;
        //            }
        //        }
        //    }


        //    if (_ray != null) {
        //        _handler._specialInput = false;
        //        if (HeadingMyDirection() || IsOnMySide()) {
        //            if (_shuttle.transform.position.y < transform.position.y) {
        //                _handler._crouchInput = true;
        //            }
        //            else {
        //                _handler._crouchInput = false;

        //            }
        //        }

        //        if (IsOnMySide() && _ray.GetMeter() >= 1 && ((_shuttle.GetComponent<Rigidbody>().velocity.y > 6 && HeadingMyDirection()) || Vector3.Distance(_shuttle.transform.position, _ray.GetRayPos()) < 3)) {
        //            _handler._specialInput = true;
        //        }
        //    }

        //    bool canShoot = true;

        //    if (_shuttle.IsGrabbed(_controller)) {
        //        _handler._jumpInput = true;
        //        _handler._jumpHeld = true;

        //        if(transform.position.y < 5) {
        //            canShoot = false;
        //        }
        //    }

        //    if (canShoot && (!isTimeToMove && inHittingRangeOfSubWoofer) || Vector3.Distance(transform.position, targetPosition) < 1.5f || (Vector3.Distance(transform.position, _shuttle.transform.position) < 1.5f && shuttleXDist < 0.6f)) {
        //        ProcessHit();
        //    }

        //    tick = 0.0f;
        //}

        //if (GameManager.Get().IsInKO()) {
        //    _handler._inputX = 0;
        //}
    }

    void MovementSuperAvoidance() {
        var distanceExplosion = 5;

        Collider[] subWooferDetection = Physics.OverlapSphere(_verticalTarget, distanceExplosion);
        foreach (Collider hit in subWooferDetection) {

            SubWoofer woofer = hit.GetComponent<SubWoofer>();

            if (woofer != null) {
                if (Vector3.Distance(_verticalTarget, woofer.transform.position) < distanceExplosion) {
                    float xDir = (_verticalTarget - woofer.transform.position).normalized.x;

                    if (xDir > 0) {
                        xDir = 1;
                    }
                    else xDir = -1;

                    Vector3 movementInfluence = new Vector3();
                    bool isNextToRightWall = Physics.Raycast(_verticalTarget, new Vector3(1, 2, 0), distanceExplosion, _stageDetection);
                    bool isNextToLeftWall = Physics.Raycast(_verticalTarget, new Vector3(-1, 2, 0), distanceExplosion, _stageDetection);

                    if (isNextToRightWall) {
                        movementInfluence.x = distanceExplosion;
                    }
                    else if (isNextToLeftWall) {
                        movementInfluence.x = -distanceExplosion;
                    }
                    else {
                        movementInfluence.x = distanceExplosion * xDir;
                    }

                    _verticalTarget += movementInfluence;
                }
            }
        }
    }

    void ClampMovementTarget() {
        if (_controller.GetFilter() == FighterFilter.one) {
            _verticalTarget.x = Mathf.Clamp(_verticalTarget.x, 0.5f, 11);
        }
        else {
            _verticalTarget.x = Mathf.Clamp(_verticalTarget.x, -11, -0.5f);
        }
    }

    void ProcessMovement() {
        bool isNetOnLeft = Physics.Raycast(transform.position, new Vector3(2, 0, 0), out _vision, 2, _netDetection);
        bool isNetOnRight = Physics.Raycast(transform.position, new Vector3(-2, 0, 0), out _vision, 2, _netDetection);

        _handler._jumpInput = false;
        _handler._jumpHeld = false;
        _handler._dashInput = false;

        if (_verticalTarget.x > transform.position.x + _xMovementOffset) {
            if (isNetOnLeft) {
                _handler._jumpInput = true;
                _handler._jumpHeld = true;
            }

            _handler._inputX = 1;
        }
        else if (_verticalTarget.x < transform.position.x - _xMovementOffset) {
            if (isNetOnRight) {
                _handler._jumpInput = true;
                _handler._jumpHeld = true;
            }

            _handler._inputX = -1;
        }
        else {
            _handler._inputX = 0;
        }


        if (IsTargetOnMySide() && HeadingMyDirection() && _horizontalTarget.y >= transform.position.y + 1) {
            _handler._jumpInput = true;
            _handler._jumpHeld = true;
        }
    }

    void ProcessHit() {
        int chipShot = Random.Range(0, 5);
        bool _heavy = true;
        _handler._chipHeld = false;

        if (chipShot == 2 && _shuttle.transform.position.y > 1.2f && _shuttle.GetSpeedPercent() > 0.5f) {
            _handler._chipInput = true;
        }
        else {
            var enemyRay = _enemy.GetComponent<RayAndTekaFighter>();
            var enemyAngle = (transform.position - _enemy.transform.position).normalized;

            if (enemyRay != null) {
                enemyAngle = (transform.position - enemyRay.GetRayPos()).normalized;
            }

            var shootDirection = _controller.GetLiftMove().GetHitDirection().normalized;

            if (_controller.GetFilter() == FighterFilter.one) {
                shootDirection.x *= -1;
            }

            var testingAngle = Vector3.Angle(shootDirection, enemyAngle);
            var currentAngle = testingAngle;
            var angle = ShotType.lift;

            shootDirection = _controller.GetSmashMove().GetHitDirection().normalized;

            if (_controller.GetFilter() == FighterFilter.one) {
                shootDirection.x *= -1;
            }

            testingAngle = Vector3.Angle(shootDirection, enemyAngle);
            if (testingAngle > currentAngle) {
                currentAngle = testingAngle;
                angle = ShotType.smash;
            }

            shootDirection = _controller.GetDriveMove().GetHitDirection().normalized;

            if (_controller.GetFilter() == FighterFilter.one) {
                shootDirection.x *= -1;
            }

            testingAngle = Vector3.Angle(shootDirection, enemyAngle);
            if (testingAngle > currentAngle) {
                currentAngle = testingAngle;
                angle = ShotType.drive;
            }

            if (angle == ShotType.drive) {
                if (Physics.Raycast(transform.position, shootDirection, _netDetection)) {
                    _heavy = false;

                    if (_controller.CanGrab() && !_shuttle.IsGrabbed(_controller)) {
                        angle = ShotType.chip;
                    }
                    else {
                        angle = ShotType.lift;
                        _heavy = true;
                    }
                }
            }

            if (angle == ShotType.chip) {
                if (Physics.Raycast(transform.position, shootDirection, _netDetection)) {
                    if (_controller.CanGrab() && !_shuttle.IsGrabbed(_controller)) {
                        _handler._chipHeld = true;

                    }
                }
            }

            _handler._chargeInput = _heavy;

            if (angle == ShotType.lift) {
                _handler._dropInput = true;
            }
            if (angle == ShotType.smash) {
                _handler._smashInput = true;
            }
            if (angle == ShotType.drive) {
                _handler._driveInput = true;
            }
            if (angle == ShotType.chip) {
                _handler._chipInput = true;
            }
        }
    }

    void ProcessRaket() {
        if (!IsOnMySide() && _raket.GetMeter() >= 1) {
            _handler._jumpHeld = true;
            _handler._jumpInput = true;
            if (transform.position.y > 6) {
                _handler._specialInput = true;
            }
        }
    }

    bool HeadingMyDirection() {
        if (_controller.GetFilter() == FighterFilter.one) {
            return _shuttle.GetComponent<Rigidbody>().velocity.x > 0;
        }
        return _shuttle.GetComponent<Rigidbody>().velocity.x < 0;
    }

    bool IsOnMySide() {
        if (_controller.GetFilter() == FighterFilter.one) {
            return _shuttle.transform.position.x > 0f;
        }
        else {
            return _shuttle.transform.position.x < 0f;
        }
    }

    bool IsTargetOnMySide() {
        if (_controller.GetFilter() == FighterFilter.one) {
            return _verticalTarget.x > 0f;
        }
        else {
            return _verticalTarget.x < 0f;
        }
    }

    bool IsOnMySide(Transform transform) {
        if (_controller.GetFilter() == FighterFilter.one) {
            return transform.position.x > 0.1f;
        }
        else {
            return transform.position.x < -0.1f;
        }
    }

    bool IsBallOnLeft() {
        return transform.position.x > _verticalTarget.x;
    }

    bool IsBallOnRight() {
        return transform.position.x < _verticalTarget.x;
    }

    bool IsBallAbovePlayer() {
        return _shuttle.transform.position.y > transform.position.y + 4;
    }

    bool InHittingRange() {
        if (Vector3.Distance(transform.position, _verticalTarget) < 1f + (_shuttle.GetSpeedPercent() * _shuttle.GetVelocity().magnitude)) {
            return true;
        }

        return Vector3.Distance(transform.position, _shuttle.transform.position) < 2f;
    }

    public void OnDrawGizmos() {
        Gizmos.color = Color.red;
        Gizmos.DrawSphere(_verticalTarget, 1);
        Gizmos.color = Color.yellow;
        Gizmos.DrawSphere(_horizontalTarget, 1);
    }
}
