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

    constructor()
    {
        super();
        UnlitMaterial.initShader();
        this.setShaderName(UnlitMaterial.shaderName);
        this._shaderValues.setVector(UnlitMaterial.ALBEDOCOLOR, new Laya.Vector4(1, 1, 1, 1));
    }

    public set alphaTest(value: boolean)
    {
        this._alphaTest = value;
    }

    public get alphaTest(): boolean
    {
        return this._alphaTest;
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

    static initShader()
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

        let vs = "#include \"Lighting.glsl\";\n\nattribute vec4 a_Position;\n\nattribute vec2 a_Texcoord0;\n\nuniform mat4 u_MvpMatrix;\n\nattribute vec4 a_Color;\nvarying vec4 v_Color;\nvarying vec2 v_Texcoord0;\n\n#ifdef TILINGOFFSET\n	uniform vec4 u_TilingOffset;\n#endif\n\n#ifdef BONE\n	const int c_MaxBoneCount = 24;\n	attribute vec4 a_BoneIndices;\n	attribute vec4 a_BoneWeights;\n	uniform mat4 u_Bones[c_MaxBoneCount];\n#endif\n\nvoid main() {\n	#ifdef BONE\n		mat4 skinTransform = mat4(0.0);\n		skinTransform += u_Bones[int(a_BoneIndices.x)] * a_BoneWeights.x;\n		skinTransform += u_Bones[int(a_BoneIndices.y)] * a_BoneWeights.y;\n		skinTransform += u_Bones[int(a_BoneIndices.z)] * a_BoneWeights.z;\n		skinTransform += u_Bones[int(a_BoneIndices.w)] * a_BoneWeights.w;\n		vec4 position = skinTransform * a_Position;\n		gl_Position = u_MvpMatrix * position;\n	#else\n		gl_Position = u_MvpMatrix * a_Position;\n	#endif\n\n	v_Texcoord0 = a_Texcoord0;\n	#ifdef TILINGOFFSET\n		v_Texcoord0=TransformUV(v_Texcoord0,u_TilingOffset);\n	#endif\n\n	#if defined(COLOR)&&defined(ENABLEVERTEXCOLOR)\n		v_Color = a_Color;\n	#endif\n	gl_Position=remapGLPositionZ(gl_Position);\n}";
        let ps = "#ifdef FSHIGHPRECISION\n	precision highp float;\n#else\n	precision mediump float;\n#endif\n\n#if defined(COLOR)&&defined(ENABLEVERTEXCOLOR)\n	varying vec4 v_Color;\n#endif\n\n#ifdef ALBEDOTEXTURE\n	uniform sampler2D u_AlbedoTexture;\n	varying vec2 v_Texcoord0;\n#endif\n\nuniform vec4 u_AlbedoColor;\n\n#ifdef ALPHATEST\n	uniform float u_AlphaTestValue;\n#endif\n\n#ifdef FOG\n	uniform float u_FogStart;\n	uniform float u_FogRange;\n	#ifdef ADDTIVEFOG\n	#else\n		uniform vec3 u_FogColor;\n	#endif\n#endif\n\nvoid main()\n{\n	vec4 color =  u_AlbedoColor;\n	#ifdef ALBEDOTEXTURE\n		color *= texture2D(u_AlbedoTexture, v_Texcoord0);\n	#endif\n	#if defined(COLOR)&&defined(ENABLEVERTEXCOLOR)\n		color *= v_Color;\n	#endif\n	\n	#ifdef ALPHATEST\n		if(color.a < u_AlphaTestValue)\n			discard;\n	#endif\n	\n	gl_FragColor = color;\n	\n	#ifdef FOG\n		float lerpFact = clamp((1.0 / gl_FragCoord.w - u_FogStart) / u_FogRange, 0.0, 1.0);\n		#ifdef ADDTIVEFOG\n			gl_FragColor.rgb = mix(gl_FragColor.rgb, vec3(0.0), lerpFact);\n		#else\n			gl_FragColor.rgb = mix(gl_FragColor.rgb, u_FogColor, lerpFact);\n		#endif\n	#endif\n	\n}\n\n";
        let shader = Laya.Shader3D.add(UnlitMaterial.shaderName);
        let subShader = new Laya.SubShader(attributeMap, uniformMap, Laya.SkinnedMeshSprite3D.shaderDefines, UnlitMaterial.shaderDefines);
        shader.addSubShader(subShader);
        subShader.addShaderPass(vs, ps);
    }
}