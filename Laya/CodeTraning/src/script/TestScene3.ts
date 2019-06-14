import RES, { RESD3 } from "./RES";
import { CarController } from "./CarController";

export default class TestScene3
{
    // sceneUrl = "scene03/Conventional/scene03.ls";
    sceneUrl = "scene01/Conventional/test1.ls";
    scene: Laya.Scene3D;

    constructor()
    {
        window['test'] = this;
        this.loadScene();
    }

    async loadScene()
    {
        let scene = new Laya.Scene3D();
        Laya.stage.addChildAt(scene, 0);
        this.scene = scene;
        // await RESD3.loadRes(this.sceneUrl);
        // let scene = RES.getRes(this.sceneUrl) as Laya.Scene3D;
        // Laya.stage.addChild(scene);
        // this.scene = scene;

        // let car = scene.getChildByName("car") as Laya.Sprite3D;
        // let carCtrl = car.addComponent(CarController);

        // //添加照相机
        let camera: Laya.Camera = (scene.addChild(new Laya.Camera(0, 0.1, 100))) as Laya.Camera;
        camera.transform.translate(new Laya.Vector3(0, 3, 3));
        camera.transform.rotate(new Laya.Vector3(-30, 0, 0), true, false);
    }

    async destoryScene()
    {
        // this.scene.removeSelf();
        // this.scene.destroy(true);
        // this.scene = null;
        // RES.clearRes(this.sceneUrl);
    }

    url1 = "res/bg.jpg";
    url2 = "res/bg_create.png";

    img1: Laya.Image;
    loadImg(url)
    {
        let img = new Laya.Image();
        img.loadImage(url);
        Laya.stage.addChild(img);
        this.img1 = img;
    }

    clearImg()
    {
        this.img1.texture.destroy(true);
        this.img1.destroy(true);
        this.img1 = null;
    }

    baseUrl = "res/d3/models/Arch_AA00/"
    assetUrl = "AssetsRecord.json";
    modelUrl = "Arch_AA00.lh";

    // baseUrl = "res/d3/models/womanBody01/"
    // assetUrl = "AssetsRecord.json";
    // modelUrl = "womanBody01.lh";
    model: Laya.Sprite3D;

    async loadModel()
    {
        await RESD3.loadRes(this.baseUrl + this.modelUrl);
        let model = RES.getRes(this.baseUrl + this.modelUrl);
        this.scene.addChild(model);

        this.model = model;
    }

    destoryModel()
    {
        this.scene.removeChild(this.model);
        this.model.removeSelf();
        this.model.destroy(true);
        this.model = null;
        this.assetsDispose();
    }

    private assetsDispose()
    {
        //加载资源释放表
        Laya.loader.load(this.baseUrl + this.assetUrl, Laya.Handler.create(this, this.onAssetOK));
    }

    //加载资源释放表完成后
    private onAssetOK(): void
    {
        //获取加载的数据（Json数据转化成数组）
        var arr: any = Laya.Loader.getRes(this.baseUrl + this.assetUrl);
        for (var i: number = arr.length - 1; i > -1; i--)
        {
            //根据资源路径获取资源
            let url = this.baseUrl + `${arr[i].url}`;
            var resource: Laya.Resource = Laya.loader.getRes(url) as Laya.Resource;
            console.error(url, resource);
            if (resource)
            {
                //资源释放
                resource.destroy();
            }
            else
            {
                console.log(url);
            }
        }
    }

}