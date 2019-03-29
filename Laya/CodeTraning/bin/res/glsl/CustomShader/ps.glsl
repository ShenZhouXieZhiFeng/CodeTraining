#ifdef FSHIGHPRECISION
    precision highp float;
#else
    precision mediump float;
#endif

uniform vec4 u_Color;
uniform sampler2D u_MainTex;

varying vec2 v_Texcoord;

void main()
{

    // https://www.jianshu.com/p/e831e0061158
    //膨胀
    // float block = 100.0;
    // float delta = 1.0/block;
    // vec4 maxColor = vec4(-1.0);
    
    // for (int i = -1; i <= 1 ; i++) {
    //     for (int j = -1; j <= 1; j++) {
    //         float x = v_Texcoord.x + float(i) * delta;
    //         float y = v_Texcoord.y + float(i) * delta;
    //         maxColor = max(texture2D(u_MainTex, vec2(x, y)), maxColor);
    //     }
    // }
    
    // gl_FragColor = maxColor;

    //腐蚀
    float block = 100.0;
    float delta = 1.0/block;
    vec4 maxColor = vec4(1.0);
    
    for (int i = -1; i <= 1 ; i++) {
        for (int j = -1; j <= 1; j++) {
            float x = v_Texcoord.x + float(i) * delta;
            float y = v_Texcoord.y + float(i) * delta;
            maxColor = min(texture2D(u_MainTex, vec2(x, y)), maxColor);
        }
    }
    
    gl_FragColor = maxColor;

    // 平均模糊
    // float block = 150.0;
    // float delta = 1.0/block;
    // vec4 color = vec4(0.0);
    
    // float factor[9];
    // factor[0] = 0.0947416; factor[1] = 0.118318; factor[2] = 0.0947416;
    // factor[3] = 0.118318; factor[4] = 0.147761; factor[5] = 0.118318;
    // factor[6] = 0.0947416; factor[7] = 0.118318; factor[8] = 0.0947416;
    
    // for (int i = -1; i <= 1; i++) {
    //     for (int j = -1; j <= 1; j++) {
    //         float x = max(0.0, v_Texcoord.x + float(i) * delta);
    //         float y = max(0.0, v_Texcoord.y + float(i) * delta);
    //         color += texture2D(u_MainTex, vec2(x, y)) * factor[(i+1)*3+(j+1)];
    //     }
    // }
    
    // gl_FragColor = vec4(vec3(color), 1.0);
}