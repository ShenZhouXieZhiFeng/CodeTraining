
export class CustomMaterial extends Laya.BaseMaterial
{
    static hasInit = false;

    constructor()
    {
        super();

        
            CustomMaterial._initShader();
        this.setShaderName("CustomShader");
    }

    static _initShader()
    {
        if (CustomMaterial.hasInit)
        {
            return;
        }
        CustomMaterial.hasInit = true;

        let attributeMap: Object = {
            'a_Position': Laya.VertexMesh.MESH_POSITION0,
            'a_Normal': Laya.VertexMesh.MESH_NORMAL0
        };
        let uniformMap: Object = {
            'u_MvpMatrix': Laya.Shader3D.PERIOD_SPRITE,
            'u_WorldMat': Laya.Shader3D.PERIOD_SPRITE
        };
        let vs: string = '#include "Lighting.glsl";\n' +
            "attribute vec4 a_Position;\n" +
            "uniform mat4 u_MvpMatrix;\n" +
            "uniform mat4 u_WorldMat;\n" +
            "attribute vec3 a_Normal;\n" +
            "varying vec3 v_Normal;\n" +
            "void main()\n" +
            "{\n" +
            "gl_Position = u_MvpMatrix * a_Position;\n" +
            "mat3 worldMat=mat3(u_WorldMat);\n" +
            "v_Normal=worldMat*a_Normal;\n" +
            "gl_Position=remapGLPositionZ(gl_Position);\n" +
            "}";
        let ps: string = "#ifdef FSHIGHPRECISION\n" +
            "precision highp float;\n" +
            "#else\n" +
            "precision mediump float;\n" +
            "#endif\n" +
            "varying vec3 v_Normal;\n" +
            "void main()\n" +
            "{\n" +
            "gl_FragColor=vec4(v_Normal,1.0);\n" +
            "}";

        let customShader: Laya.Shader3D = Laya.Shader3D.add("CustomShader");
        let subShader: Laya.SubShader = new Laya.SubShader(attributeMap, uniformMap);
        customShader.addSubShader(subShader);
        subShader.addShaderPass(vs, ps);
    }

}