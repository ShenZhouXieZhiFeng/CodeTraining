let la = Laya;

////普通资源类型////
const RES_PATH_ATLAS: string = "res/atlas";		//图集
const RES_PATH_IMAGE: string = "res/images";	//图片
const RES_PATH_CONFIG: string = "res/configs";	//配置文件
const RES_PATH_AUDIO: string = "res/sounds";   	//音频,不能预先加载
const RES_PATH_VIEW: string = "res/view";       //导出的UI文件
const RES_PATH_ANIMATION: string = "res/animation"; // 动画文件
////3D类型资源////
const RES_PATH_D3_SCENES: string = "res/d3/scenes";	//场景
const RES_PATH_D3_MODELS: string = "res/d3/models";	//模型
const RES_PATH_D3_OTHERS: string = "res/d3/others";	//其他

/**资源类型 */
export enum RES_TYPE
{
    normal,	//普通类型，图集，图片，配置文件等
    d3		//3d资源，场景，模型等,3d资源用RES3D加载
}

/**资源封装类 */
export default class RES
{
    /**
     * 根据资源民称后缀拼接资源真实路径，资源需要按照规定位置存放
     * 参数如: 1.jpg
     */
    public static joinResPath(resName: string): string
    {
        // 如果已经是全路径，就不做凭拼接
        if(resName.indexOf("res/") >= 0)
        {
            return resName;
        }
        let path = "";
        let suffix = la.Utils.getFileExtension(resName);
        switch (suffix)
        {
            case "atlas":
                path = RES_PATH_ATLAS;
                // 对序列帧动画做专门处理
                if(resName.indexOf("anim") >= 0)
                {
                    path = RES_PATH_ANIMATION;
                }
                break;
            case "png":
            case "jpg":
            case "jpeg":
                path = RES_PATH_IMAGE;
                break;
            case "json":
            case "xml":
                path = RES_PATH_CONFIG;
                break;
            case "mp3":
            case "wav":
                path = RES_PATH_AUDIO;
                break;
            case "sk":
                path = RES_PATH_ANIMATION;
                break;
            case "scene":
                path = RES_PATH_VIEW;
                break;
            case "ls":
                path = RES_PATH_D3_SCENES;
                break;
            case "lh":
                path = RES_PATH_D3_MODELS;
                break;
            default:
                break;
        }
        let res = `${path}/${resName}`;
        return res;
    }

    /**
     * 获取资源的完整路径
     * @param _resName 资源局部路径
     */
    public static getFullResPath(_resName: string): string
    {
        return this.joinResPath(_resName);
    }

    /**
     * 获取资源，资源需要已经加载过，否则返回空
     * @param resName 资源局部路径，例如: scene1/scene1.ls
     */
    public static getRes(resName: string): any
    {
        let resFullName = this.joinResPath(resName);
        return this.getResByFullName(resFullName);
    }

    /**
     * 获取资源，资源需要已经加载过，否则返回空
     * @param resName 资源完整路径
     */
    public static getResByFullName(resFullName: string)
    {
        let res = la.loader.getRes(resFullName);
        return res;
    }

    /**
     * 完全清除一个资源，注意：3d资源通过这个只能清除一层壳，3d资源的管理只能通过AssetRecord文件来做
     * @param resName 资源局部路径
     */
    public static clearRes(resName: string)
    {
        let resFullName = this.joinResPath(resName);
        this.clearResByFullName(resFullName);
    }

    /**
     * 完全清除一个资源，注意：3d资源通过这个只能清除一层壳，3d资源的管理只能通过AssetRecord文件来做
     * @param resName 资源完整路径
     */
    public static clearResByFullName(resFullName: string)
    {
        // lg.info(TG_RES, "clearResByFullName %o", resFullName);
        Laya.Loader.clearRes(resFullName);
    }

}

/**普通资源加载封装 */
export class RESNormal
{
    /**
     * 获得一张图集中的图片,图集需要已加载过，否则回空
     * @param atlasName 图集名称
     * @param imgName 图片名称
     */
    public static getImageFromAtlas(atlasName: string, imgName: string): laya.resource.Texture
    {
        let fullPath = `${atlasName}/${imgName}`;
        return RES.getResByFullName(fullPath) as laya.resource.Texture;
    }

    /**
     * 加载单个资源,图集、图片、配置表等
     * @param _resName 资源局部路径，例如: bg.png
     * @param _needMgr 是否归于场景管理
     */
    public static async loadRes(_resName: string, _prograssFunc = null): Promise<any>
    {
        let real_path = RES.joinResPath(_resName);
        if (!real_path) return null;
        return this.loadResByFullName(real_path);
    }

    /**
     * 加载资源组
     * @param _resArr 资源组数组，每个资源都是局部路径
     * @param _prograssFunc 进度回调，带一个参数（0-1）
     */
    public static async loadGroupAsync(_resArr: string[], _prograssFunc = null): Promise<boolean>
    {
        if (!_resArr || _resArr.length <= 0)
        {
            return false;
        }
        let real_res_arr = [];
        for (let res_name of _resArr)
        {
            let real_name = RES.joinResPath(res_name);
            real_res_arr.push(real_name);
        }
        return this.loadResByFullName(real_res_arr, _prograssFunc);
    }

    /**
     * 加载资源,使用完整路径
     * @param _resName 完整资源路径或资源组名
     * @param prograssFunc 进度回调，带一个参数（0-1）
     */
    public static async loadResByFullName(resFullName: any, prograssFunc = null): Promise<any>
    {
        if (!resFullName) return;
        let prograssHandler = null;
        // lg.info(TG_RES, "loadResFull normal %o", resFullName);
        let progress_handler = null;
        if (prograssFunc)
        {
            progress_handler = la.Handler.create(this, prograssFunc, null, false);
        }
        return new Promise((resolve) =>
        {
            la.loader.load
                (
                resFullName,
                la.Handler.create(this, (_res) =>
                {
                    // lg.info(TG_RES, "loadResFull normal over %o", resFullName);
                    resolve(_res);
                }),
                progress_handler
                );
        });
    }

}

/**3d资源加载封装 */
export class RESD3
{

    /**
     * 根据lh、ls资源名称，获取对应的AssetsRecord文件路径
     * @param resName 场景或模型资源的局部路径
     */
    public static getAssetsRecord(resName: string): string
    {
        let resFullName = RES.joinResPath(resName);
        return this.getAssetsRecordByResFullName(resFullName);
    }

    /**
     * 根据lh、ls资源名称，获取对应的AssetsRecord文件路径
     * @param resFullName 场景或模型资源的完整路径
     */
    public static getAssetsRecordByResFullName(resFullName: string): string
    {
        let num = resFullName.lastIndexOf('/');
        let parentFolder = resFullName.substr(0, num);
        let assetsRecordUrl = `${parentFolder}/AssetsRecord.json`;
        return assetsRecordUrl;
    }

    /**
     * 获取某个已加载过的3d物体的克隆
     * @param _resName 局部路径
     */
    public static getSprited3DClone(resName): Laya.Sprite3D
    {
        let spriteModel: Laya.Sprite3D = RES.getRes(resName) as Laya.Sprite3D;
        return spriteModel.clone();
    }

    /**
     * 加载3D资源,使用完整路径
     * @param resFullName 资源局部路径
     * @param prograssFunc loading函数
     */
    public static async loadRes(resName: string, prograssFunc: Function = null): Promise<any>
    {
        let resFullName = RES.getFullResPath(resName);
        return this.loadResByFullName(resFullName, prograssFunc);
    }

    /**
     * 加载3D资源组
     * @param _resArr 资源组数组，所有资源都是局部路径
     * @param _prograssFunc 进度回调，带一个参数（0-1）
     */
    public static async loadGroupAsync(_resArr: string[], _prograssFunc = null): Promise<any>
    {
        if (!_resArr || _resArr.length <= 0)
        {
            return false;
        }
        let real_res_arr = [];
        for (let res_name of _resArr)
        {
            let real_name = RES.joinResPath(res_name);
            real_res_arr.push(real_name);
        }
        return this.loadResByFullName(real_res_arr, _prograssFunc);
    }

    /**
     * 加载3D资源组
     * @param _resArr 资源组数组，所有资源都是完整路径
     * @param _prograssFunc 进度回调，带一个参数（0-1）
     */
    public static async loadGroupAsyncByFullName(_resArr: string[], _prograssFunc = null): Promise<any>
    {
        if (!_resArr || _resArr.length <= 0)
        {
            return false;
        }
        return this.loadResByFullName(_resArr, _prograssFunc);
    }

    /**
     * 加载3D资源,使用完整路径
     * @param resFullName 资源完整路径
     * @param prograssFunc loading函数
     */
    public static async loadResByFullName(resFullName: any, prograssFunc: Function = null)
    {
        if (!resFullName) return;
        let prograssHandler = null;
        // lg.info(TG_RES, "loadResByFullName 3d begin %o", resFullName);
        let progress_handler: Laya.Handler = null;
        if (prograssFunc)
        {
            progress_handler = la.Handler.create(this, prograssFunc, null, false);
        }
        return new Promise((resolve) =>
        {
            la.loader.create
                (
                resFullName,
                la.Handler.create(this, () =>
                {
                    // lg.info(TG_RES, "loadResByFullName 3d over %o", resFullName);
                    resolve();
                }),
                progress_handler
                )
        });
    }

    /**
     * 加载3D资源,基于AssetsRecord加载,进度条更精细
     * @param resFullName 资源完整路径
     * @param prograssFunc loading函数
     */
    public static async loadResByAssetsRecord(resFullName: string, prograssFunc: Function = null)
    {
        // . 加载AssetsRecord文件，较小，不计入进度值
        let arRes = this.getAssetsRecordByResFullName(resFullName);
        await this.loadResByFullName(arRes);
        let arConfig = RES.getResByFullName(arRes);
        if (!arConfig || arConfig.length <= 0)
        {
            return;
        }
        // . 组织要加载的资源文件
        let resArr = [];
        let num = resFullName.lastIndexOf('/');
        let parentFolder = resFullName.substr(0, num) + '/';
        for (let res of arConfig)
        {
            resArr.push(parentFolder + res.url);
        }
        await this.loadResByFullName(resArr, prograssFunc);
        // 最后再加载并创建自身（ls、lh），其内部资源已经在上方加载完了
        await this.loadResByFullName(resFullName);
    }

}