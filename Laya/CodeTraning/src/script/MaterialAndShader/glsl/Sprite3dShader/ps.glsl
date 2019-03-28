#ifdef FSHIGHPRECISION
    precision highp float;
#else
    precision mediump float;
#endif

varying vec2 v_Texcoord;
uniform sampler2D u_MainTex;
uniform vec4 u_Color;

void main()
{
    vec4 albedo = texture2D(u_MainTex,v_Texcoord);
	gl_FragColor = albedo * u_Color;

	#ifdef CULLOFF_ON
		if(gl_FragColor.a < 0.5)
			discard;
	#endif
}