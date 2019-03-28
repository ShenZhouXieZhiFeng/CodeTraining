#include "Lighting.glsl";
attribute vec4 a_Position;
attribute vec3 a_Normal;

uniform mat4 u_MvpMatrix;
uniform mat4 u_WorldMat;

varying vec3 v_Normal;
void main()
{
    gl_Position = u_MvpMatrix * a_Position;
    mat3 worldMat=mat3(u_WorldMat);
    v_Normal=worldMat*a_Normal;
    gl_Position=remapGLPositionZ(gl_Position);
}