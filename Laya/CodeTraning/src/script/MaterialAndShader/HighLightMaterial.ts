import { RESNormal } from "../RES";

/**瞬间高亮 */
export class HighLightMaterial extends Laya.BaseMaterial
{
    static hasInit = false;
    static SHADER_NAME = "HighLightShader";
    static SHADERDEFINE_CULLOFF_ON;
    static ALBEDOTEXTURE: number = Laya.Shader3D.propertyNameToID("u_MainTex");
    static COLOR: number = Laya.Shader3D.propertyNameToID("u_Color");
    static HIDETEX: number = Laya.Shader3D.propertyNameToID("u_HideTex");

    private _color: Laya.Vector4;
    private _hideTex: boolean;

    public async compile()
    {
        await HighLightMaterial.initShader();
        this.setOpaqueRender();
        this.setShaderName(HighLightMaterial.SHADER_NAME);
        this.albedoColor = new Laya.Vector4(1, 1, 1, 1);
    }

    public get hideTex()
    {
        return this._hideTex;
    }

    public set hideTex(value: boolean)
    {
        this._hideTex = value;
        this._shaderValues.setBool(HighLightMaterial.HIDETEX, value);
    }

    public get albedoTexture(): Laya.BaseTexture
    {
        return this._shaderValues.getTexture(HighLightMaterial.ALBEDOTEXTURE);
    }

    public set albedoTexture(value)
    {
        this._shaderValues.setTexture(HighLightMaterial.ALBEDOTEXTURE, value);
    }

    public get albdoColor(): Laya.Vector4
    {
        return this._color;
    }

    public set albedoColor(value: Laya.Vector4)
    {
        this._color = value;
        this._shaderValues.setVector(HighLightMaterial.COLOR, value);
    }

    public setOpaqueRender()
    {
        this.renderQueue = Laya.BaseMaterial.RENDERQUEUE_OPAQUE;
    }

    static async initShader()
    {
        if (HighLightMaterial.hasInit)
        {
            return;
        }
        HighLightMaterial.hasInit = true;

        HighLightMaterial.SHADERDEFINE_CULLOFF_ON = HighLightMaterial.shaderDefines.registerDefine("CULLOFF_ON");

        let attributeMap =
            {
                'a_Position': Laya.VertexMesh.MESH_POSITION0,
                'a_Texcoord0': Laya.VertexMesh.MESH_TEXTURECOORDINATE0
            };
        let uniformMap =
            {
                'u_MvpMatrix': Laya.Shader3D.PERIOD_SPRITE,
                'u_MainTex': Laya.Shader3D.PERIOD_MATERIAL,
                'u_Color': Laya.Shader3D.PERIOD_MATERIAL,
                'u_HideTex': Laya.Shader3D.PERIOD_MATERIAL
            };

        let vs = await RESNormal.loadRes("HighLightShader/vs.glsl");
        let ps = await RESNormal.loadRes("HighLightShader/ps.glsl");

        let spShader: Laya.Shader3D = Laya.Shader3D.add(HighLightMaterial.SHADER_NAME);
        let subShader: Laya.SubShader = new Laya.SubShader(attributeMap, uniformMap, HighLightMaterial.shaderDefines);
        spShader.addSubShader(subShader);
        subShader.addShaderPass(vs, ps);
    }
}