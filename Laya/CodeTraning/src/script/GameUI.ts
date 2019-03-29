import { ui } from "./../ui/layaMaxUI";
import RES, { RESD3 } from "./RES";
import { CubeController } from "./CubeController";
import { CustomMaterial } from "./MaterialAndShader/CustomMaterial";
import { UnlitMaterial } from "./MaterialAndShader/UnlitMaterial";
import { HighLightMaterial } from "./MaterialAndShader/HighLightMaterial";

export default class GameUI
{
    constructor()
    {
        window['gu'] = this;

        Laya.Shader3D.debugMode = true;

        //添加3D场景
        let scene: Laya.Scene3D = Laya.stage.addChild(new Laya.Scene3D()) as Laya.Scene3D;

        //添加照相机
        let camera: Laya.Camera = (scene.addChild(new Laya.Camera(0, 0.1, 100))) as Laya.Camera;
        camera.transform.translate(new Laya.Vector3(0, 3, 3));
        camera.transform.rotate(new Laya.Vector3(-30, 0, 0), true, false);

        //添加方向光
        let directionLight: Laya.DirectionLight = scene.addChild(new Laya.DirectionLight()) as Laya.DirectionLight;
        directionLight.color = new Laya.Vector3(0.6, 0.6, 0.6);
        directionLight.transform.worldMatrix.setForward(new Laya.Vector3(1, -1, 0));

        //添加自定义模型
        let box: Laya.MeshSprite3D = scene.addChild(new Laya.MeshSprite3D(Laya.PrimitiveMesh.createBox(1, 1, 1))) as Laya.MeshSprite3D;
        box.transform.rotate(new Laya.Vector3(0, 45, 0), false, false);
        box.addComponent(CubeController);

        // var customMaterial: CustomMaterial = new CustomMaterial();
        // box.meshRenderer.sharedMaterial = customMaterial;

        // let spMat: Laya.UnlitMaterial = new Laya.UnlitMaterial();
        // Laya.Texture2D.load("res/layabox.png", Laya.Handler.create(null, (tex) =>
        // {
        //     spMat.albedoTexture = tex;
        // }));
        // box.meshRenderer.material = spMat;

        this.setMat(box);

        // let material: Laya.BlinnPhongMaterial = new Laya.BlinnPhongMaterial();
        // Laya.Texture2D.load("res/layabox.png", Laya.Handler.create(null, function (tex: Laya.Texture2D)
        // {
        //     material.albedoTexture = tex;
        // }));
        // box.meshRenderer.material = material;
    }

    private async setMat(box)
    {
        // let spMat: UnlitMaterial = new UnlitMaterial();
        // await spMat.compile();
        // Laya.Texture2D.load("res/layabox.png", Laya.Handler.create(null, (tex) =>
        // {
        //     spMat.albedoTexture = tex;
        // }));
        // box.meshRenderer.material = spMat;

        let spMat: HighLightMaterial = new HighLightMaterial();
        await spMat.compile();
        Laya.Texture2D.load("res/layabox.png", Laya.Handler.create(null, (tex) =>
        {
            spMat.albedoTexture = tex;
        }));
        // spMat.albedoColor = new Laya.Vector4(1, 0, 0, 1);
        box.meshRenderer.material = spMat;


        window['spMat'] = spMat;
    }
}