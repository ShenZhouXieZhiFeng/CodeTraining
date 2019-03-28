export class CubeController extends Laya.Script
{
    private _owner: Laya.Sprite3D;

    onAwake()
    {
        this._owner = this.owner as Laya.Sprite3D;
    }

    private _rotateV3 = new Laya.Vector3(0, -0.01, 0);
    onUpdate()
    {
        this._owner.transform.rotate(this._rotateV3, true);
    }
}