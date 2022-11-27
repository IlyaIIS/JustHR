#if OPENGL
	#define SV_POSITION POSITION
	#define VS_SHADERMODEL vs_3_0
	#define PS_SHADERMODEL ps_3_0
#else
	#define VS_SHADERMODEL vs_4_0_level_9_1
	#define PS_SHADERMODEL ps_4_0_level_9_1
#endif

extern float ElapsedTime;
extern float ElapsedWeight;

extern Texture2D mainTexture;

sampler2D Character01Sampler = sampler_state
{
    Texture = <mainTexture>;
};


struct VertexShaderOutput
{
	float4 Position : SV_POSITION;
	float4 Color : COLOR0;
	float2 TextureCoordinates : TEXCOORD0;
};


float4 MainPS(VertexShaderOutput input) : COLOR
{
    float4 color = tex2D(Character01Sampler, input.TextureCoordinates + float2((sin(ElapsedTime + input.TextureCoordinates.y * 10) / 20) * ElapsedWeight, 0));

	return color;
}

technique SpriteBlending
{
	pass P0
	{
		PixelShader = compile PS_SHADERMODEL MainPS();
	}
};