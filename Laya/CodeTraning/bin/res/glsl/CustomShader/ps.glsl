#ifdef FSHIGHPRECISION
    precision highp float;
#else
    precision mediump float;
#endif


uniform vec4 u_Color;
uniform sampler2D u_MainTex;
uniform sampler2D u_MainTex2;

varying vec2 v_Texcoord;

const highp vec3 W = vec3(0.2125, 0.7154, 0.0721);

void main()
{
    // 灰度
    // vec4 c1 = texture2D(u_MainTex, v_Texcoord);
    // float gray = c1.x * 0.3 + c1.y * 0.59 + c1.z * 0.11;
    // float gray = (c1.x + c1.y + c1.z) * 0.3;
    // float gray = dot(c1.rgb, W);
    // gl_FragColor = vec4(vec3(gray), 1.0);

    // 颠倒
    // vec4 c1 = texture2D(u_MainTex, vec2(v_Texcoord.x, 1.0 - v_Texcoord.y));
    // gl_FragColor = c1;

    // 旋涡
    // vec2 uv = v_Texcoord - vec2(0.5,0.5);
    // float dist = length(uv);

    // float radius = 200.0;
    // float percent = (radius - dist) / radius;
    // float angle = 100.0;
    // if(percent < 1.0 && percent > 0.0)
    // {
    //     float theta = percent * percent * angle * 8.0;
    //     float s = sin(theta);
    //     float c = cos(theta);

    //     uv = vec2(uv.x * c - uv.y * s, uv.x * s + uv.y * c);
    // }
    // uv += vec2(0.5,0.5);
    // gl_FragColor = texture2D(u_MainTex, uv);

    // ivec2 ires = ivec2(512, 512);
    // float Res = float(ires.s);
    
    // vec2 st = v_Texcoord;
    // float Radius = Res * uR;

    // vec2 xy = Res * st;
    
    // vec2 dxy = xy - vec2(Res/2., Res/2.);
    // float r = length(dxy);
    
    // float beta = atan(dxy.y, dxy.x) + radians(uD) * 2.0 * (-(r/Radius)*(r/Radius) + 1.0);//(1.0 - r/Radius);
    
    // vec2 xy1 = xy;
    // if(r<=Radius)
    // {
    //     xy1 = Res/2. + r*vec2(cos(beta), sin(beta));
    // }
    
    // st = xy1/Res;
    
    // vec3 irgb = texture2D(u_MainTex, st).rgb;
    
    // gl_FragColor = vec4( irgb, 1.0 );

    

    // 加法
    // vec4 color1 = texture2D(u_MainTex, v_Texcoord);
    // vec4 color2 = texture2D(u_MainTex2, v_Texcoord);
    // float alpha = 0.6;
    // gl_FragColor = vec4(vec3(color1 * (1.0-alpha) + color2 * alpha), 1.0);

    // 减法
    // vec4 c1 = texture2D(u_MainTex, v_Texcoord);
    // vec4 c2 = texture2D(u_MainTex2, v_Texcoord);
    // gl_FragColor = vec4(vec3(c1 - c2), 1.0);

    // 乘法
    // vec4 c1 = texture2D(u_MainTex, v_Texcoord);
    // vec4 c2 = texture2D(u_MainTex2, v_Texcoord);
    // gl_FragColor = vec4(vec3(1.5 * c1 * c2), 1.0);

    // 除法
    // vec4 c1 = texture2D(u_MainTex, v_Texcoord);
    // vec4 c2 = texture2D(u_MainTex2, v_Texcoord);
    // gl_FragColor = vec4(vec3(c2 / c1), 1.0);

    // 非
    // vec4 c1 = texture2D(u_MainTex, v_Texcoord);
    // vec3 temp = vec3(255);
    // gl_FragColor = vec4(temp - vec3(c1), 1.0);

    // 与 X
    // vec4 c1 = texture2D(u_MainTex, v_Texcoord);
    // vec4 c2 = texture2D(u_MainTex2, v_Texcoord);
    // gl_FragColor = vec4(vec3(c1.x & c2.x,c1.y & c2.y,c1.z & c2.z), 1.0);

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
    // float block = 100.0;
    // float delta = 1.0/block;
    // vec4 maxColor = vec4(1.0);
    
    // for (int i = -1; i <= 1 ; i++) {
    //     for (int j = -1; j <= 1; j++) {
    //         float x = v_Texcoord.x + float(i) * delta;
    //         float y = v_Texcoord.y + float(i) * delta;
    //         maxColor = min(texture2D(u_MainTex, vec2(x, y)), maxColor);
    //     }
    // }
    
    // gl_FragColor = maxColor;

    // 高斯模糊
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