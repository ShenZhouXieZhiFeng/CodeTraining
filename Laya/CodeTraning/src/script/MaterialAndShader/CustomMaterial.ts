import { RESNormal } from "../RES";

export class CustomMaterial extends Laya.BaseMaterial
{
    static hasInit = false;
    static shaderName = "CustomShader";

    static TEXTURE = Laya.Shader3D.propertyNameToID("u_MainTex");
    static COLOR = Laya.Shader3D.propertyNameToID("u_Color");

    constructor()
    {
        super();
        this.setShaderName(CustomMaterial.shaderName);
        this.color = new Laya.Vector4(1, 1, 1, 1);
    }

    set color(color: Laya.Vector4)
    {
        this._shaderValues.setVector(CustomMaterial.COLOR, color);
    }

    set texture(value)
    {
        this._shaderValues.setTexture(CustomMaterial.TEXTURE, value);
    }

    public static async compile()
    {
        if (CustomMaterial.hasInit)
        {
            return;
        }
        CustomMaterial.hasInit = true;

        let attributeMap: Object = {
            'a_Position': Laya.VertexMesh.MESH_POSITION0,
            'a_Texcoord0': Laya.VertexMesh.MESH_TEXTURECOORDINATE0
        };
        let uniformMap: Object = {
            'u_MainTex': Laya.Shader3D.PERIOD_MATERIAL,
            'u_Color': Laya.Shader3D.PERIOD_MATERIAL,
            'u_MvpMatrix': Laya.Shader3D.PERIOD_SPRITE
        };

        let vs = await RESNormal.loadRes("CustomShader/vs.glsl");
        let ps = await RESNormal.loadRes("CustomShader/ps.glsl");

        let customShader: Laya.Shader3D = Laya.Shader3D.add(CustomMaterial.shaderName);
        let subShader: Laya.SubShader = new Laya.SubShader(attributeMap, uniformMap);
        customShader.addSubShader(subShader);
        subShader.addShaderPass(vs, ps);
    }

}