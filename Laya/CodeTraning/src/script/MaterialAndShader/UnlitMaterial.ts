import RES, { RESNormal } from "../RES";

export class UnlitMaterial extends Laya.BaseMaterial
{
    static hasInit = false;
    static shaderName = "UnlitShader";
    static ALBEDOTEXTURE = Laya.Shader3D.propertyNameToID('u_AlbedoTexture');
    static ALBEDOCOLOR = Laya.Shader3D.propertyNameToID('u_AlbedoColor');

    static RENDERMODE_OPAQUE = 0;
    static RENDERMODE_CUTOUT = 1;
    static RENDERMODE_TRANSPARENT = 2;
    static RENDERMODE_ADDTIVE = 3;
    static SHADERDEFINE_ALBEDOTEXTURE = 0;
    static SHADERDEFINE_TILINGOFFSET = 0;
    static SHADERDEFINE_ENABLEVERTEXCOLOR = 0;

    private _albedoIntensity = 1.0;
    private _enableVertexColor = false;
    private _albedoColor: Laya.Vector4 = new Laya.Vector4(1, 1, 1, 1);
    private _alphaTest: boolean = true;

    public async compile()
    {
        await UnlitMaterial.initShader();
        this.setShaderName(UnlitMaterial.shaderName);
        this._shaderValues.setVector(UnlitMaterial.ALBEDOCOLOR, new Laya.Vector4(1, 1, 1, 1));
    }

    public set albedoColor(value: Laya.Vector4)
    {
        this._albedoColor = value;
        this._shaderValues.setVector(UnlitMaterial.ALBEDOCOLOR, value);
    }

    public get albedoColor()
    {
        return this._albedoColor;
    }

    public set albedoTexture(value: Laya.BaseTexture)
    {
        if (value)
        {
            this._defineDatas.add(UnlitMaterial.SHADERDEFINE_ALBEDOTEXTURE);
        }
        else
        {
            this._defineDatas.remove(UnlitMaterial.SHADERDEFINE_ALBEDOTEXTURE);
        }
        this._shaderValues.setTexture(UnlitMaterial.ALBEDOTEXTURE, value);
    }

    public get albedoTexture()
    {
        return this._shaderValues.getTexture(UnlitMaterial.ALBEDOTEXTURE);
    }

    static async initShader()
    {
        if (UnlitMaterial.hasInit)
        {
            return;
        }
        UnlitMaterial.hasInit = true;

        UnlitMaterial.SHADERDEFINE_ALBEDOTEXTURE = UnlitMaterial.shaderDefines.registerDefine("ALBEDOTEXTURE");
        UnlitMaterial.SHADERDEFINE_TILINGOFFSET = UnlitMaterial.shaderDefines.registerDefine("TILINGOFFSET");
        UnlitMaterial.SHADERDEFINE_ENABLEVERTEXCOLOR = UnlitMaterial.shaderDefines.registerDefine("ENABLEVERTEXCOLOR");

        let attributeMap = {
            'a_Position':/*laya.d3.graphics.Vertex.VertexMesh.MESH_POSITION0*/0,
            'a_Color':/*laya.d3.graphics.Vertex.VertexMesh.MESH_COLOR0*/1,
            'a_Texcoord0':/*laya.d3.graphics.Vertex.VertexMesh.MESH_TEXTURECOORDINATE0*/2,
            'a_BoneWeights':/*laya.d3.graphics.Vertex.VertexMesh.MESH_BLENDWEIGHT0*/7,
            'a_BoneIndices':/*laya.d3.graphics.Vertex.VertexMesh.MESH_BLENDINDICES0*/6
        };
        let uniformMap = {
            'u_Bones':/*laya.d3.shader.Shader3D.PERIOD_CUSTOM*/0,
            'u_AlbedoTexture':/*laya.d3.shader.Shader3D.PERIOD_MATERIAL*/1,
            'u_AlbedoColor':/*laya.d3.shader.Shader3D.PERIOD_MATERIAL*/1,
            'u_TilingOffset':/*laya.d3.shader.Shader3D.PERIOD_MATERIAL*/1,
            'u_AlphaTestValue':/*laya.d3.shader.Shader3D.PERIOD_MATERIAL*/1,
            'u_MvpMatrix':/*laya.d3.shader.Shader3D.PERIOD_SPRITE*/2,
            'u_FogStart':/*laya.d3.shader.Shader3D.PERIOD_SCENE*/4,
            'u_FogRange':/*laya.d3.shader.Shader3D.PERIOD_SCENE*/4,
            'u_FogColor':/*laya.d3.shader.Shader3D.PERIOD_SCENE*/4
        };

        let vs = await RESNormal.loadRes("UnlitShader/vs.glsl");
        let ps = await RESNormal.loadRes("UnlitShader/ps.glsl");

        let shader = Laya.Shader3D.add(UnlitMaterial.shaderName);
        let subShader = new Laya.SubShader(attributeMap, uniformMap, Laya.SkinnedMeshSprite3D.shaderDefines, UnlitMaterial.shaderDefines);
        shader.addSubShader(subShader);
        subShader.addShaderPass(vs, ps);
    }
}