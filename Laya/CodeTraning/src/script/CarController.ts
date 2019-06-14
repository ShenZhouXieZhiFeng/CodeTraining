enum CarStateType
{
    Invalid = -1,
    Move,
    DriftLeft,
    DriftRight,
    Fly,
    Die,
    Finish
}
let Vector3 = Laya.Vector3;
export class CarController extends Laya.Script
{
    ownerSprited: Laya.Sprite3D;
    ownerTransform: Laya.Transform3D;
    body: Laya.Sprite3D;
    bodyTransform: Laya.Transform3D;
    moveSpeed = 10;
    moveDir = new Vector3(0, 0, 0);
    carState: CarStateType = CarStateType.Invalid;

    onAwake()
    {
        window['car'] = this;
        this.ownerSprited = this.owner as Laya.Sprite3D;
        this.ownerTransform = this.ownerSprited.transform;
        this.body = this.ownerSprited.getChildByName("carBody") as Laya.Sprite3D;
        this.bodyTransform = this.body.transform;
        Laya.stage.on(Laya.Event.MOUSE_DOWN, this, this.onMouseDown);
        Laya.stage.on(Laya.Event.MOUSE_UP, this, this.onMouseUp);
    }

    onEnable()
    {
        let forward = this.ownerTransform.forward;
        this.moveDir = new Vector3(forward.x, forward.y, -forward.z);

        this.changeCarState(CarStateType.Move);
    }

    onUpdate()
    {
        if (!this.canMove)
        {
            return;
        }
        let deltaTime = Laya.timer.delta / 1000;
        this.simulateState(deltaTime);
        this.updatePosition(deltaTime);
    }

    get canMove(): boolean
    {
        let res = false;
        switch (this.carState)
        {
            case CarStateType.Move:
            case CarStateType.DriftLeft:
            case CarStateType.DriftRight:
            case CarStateType.Fly:
            case CarStateType.Finish:
                res = true;
        }
        return res;
    }

    onMouseDown()
    {
        this.changeCarState(CarStateType.DriftRight);
    }

    onMouseUp()
    {
        this.changeCarState(CarStateType.DriftLeft);
    }

    changeCarState(newState: CarStateType)
    {
        if (this.carState == newState || this.carState == CarStateType.Finish)
        {
            return;
        }
        this.carState = newState;
        switch (this.carState)
        {
            case CarStateType.Invalid:
                this.EnterInvalidState();
                break;
            case CarStateType.Move:
                this.EnterMoveState();
                break;
            case CarStateType.DriftLeft:
                this.EnterDriftLeftState();
                break;
            case CarStateType.DriftRight:
                this.EnterDriftRightState();
                break;
            case CarStateType.Fly:
                this.EnterFlyState();
                break;
            case CarStateType.Die:
                this.EnterDieState();
                break;
            case CarStateType.Finish:
                this.EnterFinishState();
                break;
        }
    }

    EnterInvalidState()
    {
    }

    EnterMoveState()
    {
        // this.CorrectRotationY();
    }

    EnterDriftLeftState()
    {
        this.StartTurnFwd(180);
    }

    EnterDriftRightState()
    {
        this.StartTurnFwd(90);
    }

    EnterFlyState()
    {

    }

    EnterDieState()
    {

    }

    EnterFinishState()
    {

    }

    /***************UPDATE****************/

    simulateState(deltaTime)
    {
        let state = this.carState;
        switch (state)
        {
            case CarStateType.DriftLeft:
                break;
            case CarStateType.DriftRight:
                break;
            case CarStateType.Fly:
                break;
            case CarStateType.Die:
                break;
            case CarStateType.Finish:
                break;
        }
    }

    updatePosition(deltaTime)
    {
        let deltaDir = new Laya.Vector3();
        Laya.Vector3.scale(this.moveDir, this.moveSpeed * deltaTime, deltaDir);
        this.ownerTransform.translate(deltaDir, true);
    }

    /******************func*******************/

    backDir = new Vector3(0, 0, -1);
    upDir = new Vector3(0, 1, 0);
    leftDir = new Vector3(1, 0, 0)

    CorrectRotationY()
    {
        let m_Trans = this.bodyTransform;
        let num = Vector3.dot(m_Trans.forward, this.backDir);
        let num2 = Vector3.dot(m_Trans.forward, this.leftDir);
        let eulerAngles = m_Trans.rotationEuler;
        if (num < num2)
        {
            eulerAngles.y = 270;
        }
        else
        {
            eulerAngles.y = 180;
        }
        m_Trans.rotationEuler = eulerAngles;
    }

    tweenData = { val: 0 };
    rotateSpeed = 0.25;
    StartTurnFwd(rotateY)
    {
        this.StopTurnFwd();
        let curY = this.bodyTransform.rotationEuler.y;
        let data = this.tweenData;
        data.val = curY;
        let diff = Math.abs(curY - rotateY);
        let rotateTime = diff / this.rotateSpeed;
        if (rotateTime == 0)
        {
            return;
        }
        Laya.Tween.to(data, { val: rotateY }, rotateTime).update =
            Laya.Handler.create(this, () =>
            {
                let ang = this.bodyTransform.rotationEuler;
                ang.y = data.val;
                this.bodyTransform.rotationEuler = ang;
            }, null, false);
    }

    StopTurnFwd()
    {
        Laya.Tween.clearAll(this.tweenData);
    }
}
