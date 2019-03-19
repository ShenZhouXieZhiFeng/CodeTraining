import { ui } from "./../ui/layaMaxUI";
import RES, { RESD3 } from "./RES";
/**
 * 本示例采用非脚本的方式实现，而使用继承页面基类，实现页面逻辑。在IDE里面设置场景的Runtime属性即可和场景进行关联
 * 相比脚本方式，继承式页面类，可以直接使用页面定义的属性（通过IDE内var属性定义），比如this.tipLbll，this.scoreLbl，具有代码提示效果
 * 建议：如果是页面级的逻辑，需要频繁访问页面内多个元素，使用继承式写法，如果是独立小模块，功能单一，建议用脚本方式实现，比如子弹脚本。
 */
export default class GameUI
{
    constructor()
    {
        this._showScene();

        //添加3D场景
        // var scene: Laya.Scene3D = Laya.stage.addChild(new Laya.Scene3D()) as Laya.Scene3D;

        // //添加照相机
        // var camera: Laya.Camera = (scene.addChild(new Laya.Camera(0, 0.1, 100))) as Laya.Camera;
        // camera.transform.translate(new Laya.Vector3(0, 3, 3));
        // camera.transform.rotate(new Laya.Vector3(-30, 0, 0), true, false);

        // //添加方向光
        // var directionLight: Laya.DirectionLight = scene.addChild(new Laya.DirectionLight()) as Laya.DirectionLight;
        // directionLight.color = new Laya.Vector3(0.6, 0.6, 0.6);
        // directionLight.transform.worldMatrix.setForward(new Laya.Vector3(1, -1, 0));

        // //添加自定义模型
        // var box: Laya.MeshSprite3D = scene.addChild(new Laya.MeshSprite3D(Laya.PrimitiveMesh.createBox(1, 1, 1))) as Laya.MeshSprite3D;
        // box.transform.rotate(new Laya.Vector3(0, 45, 0), false, false);
        // var material: Laya.BlinnPhongMaterial = new Laya.BlinnPhongMaterial();
        // Laya.Texture2D.load("res/layabox.png", Laya.Handler.create(null, function(tex:Laya.Texture2D) {
        // 		material.albedoTexture = tex;
        // }));
        // box.meshRenderer.material = material;

        window['gu'] = this;
    }

    private cube: Laya.Sprite3D;

    private async _showScene()
    {
        let sceneUrl = "res/d3/scenes/scene01/Conventional/SampleScene.ls";
        await RESD3.loadResByFullName(sceneUrl);
        let scene: Laya.Scene3D = RES.getResByFullName(sceneUrl);
        Laya.stage.addChild(scene);

        let cube: Laya.MeshSprite3D = scene.getChildByName("Cube") as Laya.MeshSprite3D;

        let mat: Laya.PBRStandardMaterial = new Laya.PBRStandardMaterial();

        // Laya.Texture2D.load("res/layabox.png", Laya.Handler.create(this, (tex: Laya.Texture2D) =>
        // {
        //     mat.albedoTexture = tex;
        //     cube.meshRenderer.material = mat;
        // }))

        this.cube = cube;
    }

    private rotate(x, y, z)
    {
        this.cube.transform.rotate(new Laya.Vector3(x, y, z));
    }

    public getPYO()
    {
        let v3 = new Laya.Vector3();
        this.cube.transform.rotation.getYawPitchRoll(v3);
        console.error(v3.x * Math.PI, v3.y * Math.PI, v3.z * Math.PI);
    }

    /**度转弧度 */
    public deg2rad(deg: number)
    {
        let res = 0;
        res = deg * Math.PI / 180;
        return res;
    }
}