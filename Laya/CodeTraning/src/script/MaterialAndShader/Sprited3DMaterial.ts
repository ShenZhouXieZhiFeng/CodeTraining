export class Sprited3DMaterial extends Laya.BaseMaterial
{
    static hasInit = false;

    public static ALBEDOTEXTURE: number = 1;
    public static COLOR: number = 2;

    public static SHADER_NAME = "Sprited3DShader";

    public static shaderDefines: laya.d3.shader.ShaderDefines;

    constructor()
    {
        super();
        Sprited3DMaterial._initShader();
        this.setShaderName(Sprited3DMaterial.SHADER_NAME);
        let sv = this._shaderValues;
        sv.setVector(Sprited3DMaterial.COLOR, new Laya.Vector4(1,1,1,1));
    }

    public get albedoTexture(): Laya.BaseTexture
    {
        return this._shaderValues.getTexture(Sprited3DMaterial.ALBEDOTEXTURE);
    }

    public set albedoTexture(value)
    {
        this._shaderValues.setTexture(Sprited3DMaterial.ALBEDOTEXTURE, value);
    }

    // public set color(value: Laya.Vector4)
    // {
    //     this._shaderValues.setVector(Sprited3DMaterial.COLOR, value);
    // }

    static _initShader()
    {
        if (Sprited3DMaterial.hasInit)
        {
            return;
        }
        Sprited3DMaterial.hasInit = true;

        Sprited3DMaterial.shaderDefines = new laya.d3.shader.ShaderDefines();
        // Sprited3DMaterial.shaderDefines.registerDefine()

        let attributeMap =
            {
                'a_Position': Laya.VertexMesh.MESH_POSITION0,
                'a_Texcoord0': Laya.VertexMesh.MESH_TEXTURECOORDINATE0
            };
        let uniformMap =
            {
                'u_MvpMatrix': Laya.Shader3D.PERIOD_SPRITE,
                'u_MainTex': Laya.Shader3D.PERIOD_MATERIAL,
                'u_Color': Laya.Shader3D.PERIOD_MATERIAL
            };
        let vs: string = 'attribute vec4 a_Position; \n uniform mat4 u_MvpMatrix; \n attribute vec2 a_Texcoord0; \n varying vec2 v_Texcoord; \n  \n void main(){ \n     gl_Position = u_MvpMatrix * a_Position; \n     v_Texcoord = a_Texcoord0; \n } \n';
        let ps: string = '#ifdef FSHIGHPRECISION \n     precision highp float; \n #else \n     precision mediump float; \n #endif \n  \n varying vec2 v_Texcoord; \n uniform sampler2D u_MainTex; \n uniform vec4 u_Color; \n  \n void main() \n { \n     vec4 albedo = texture2D(u_MainTex,v_Texcoord); \n       gl_FragColor = albedo * u_Color; \n  \n         #ifdef CULLOFF_ON \n            if(gl_FragColor.a < 0.5) \n                     discard; \n     #endif \n } \n';

        let spShader: Laya.Shader3D = Laya.Shader3D.add(Sprited3DMaterial.SHADER_NAME);
        let subShader: Laya.SubShader = new Laya.SubShader(attributeMap, uniformMap, Sprited3DMaterial.shaderDefines);
        spShader.addSubShader(subShader);
        subShader.addShaderPass(vs, ps);
    }
}