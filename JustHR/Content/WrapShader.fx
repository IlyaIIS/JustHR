#if OPENGL
	#define SV_POSITION POSITION
	#define VS_SHADERMODEL vs_3_0
	#define PS_SHADERMODEL ps_3_0
#else
	#define VS_SHADERMODEL vs_4_0_level_9_1
	#define PS_SHADERMODEL ps_4_0_level_9_1
#endif

extern float Distance = 1;

extern Texture2D Character;

sampler2D CharacterSampler = sampler_state
{
	Texture = <Character>;
};


struct VertexShaderOutput
{
	float4 Position : SV_POSITION;
	float4 Color : COLOR0;
	float2 TextureCoordinates : TEXCOORD0;
};


float4 MainPS(VertexShaderOutput input) : COLOR
{
    float4 rColor = tex2D(CharacterSampler, input.TextureCoordinates.xy + float2(Distance, 0));
    float4 tColor = tex2D(CharacterSampler, input.TextureCoordinates.xy - float2(0, Distance));
    float4 lColor = tex2D(CharacterSampler, input.TextureCoordinates.xy - float2(Distance, 0));
    float4 dColor = tex2D(CharacterSampler, input.TextureCoordinates.xy + float2(0, Distance));
    
    float4 color = tex2D(CharacterSampler, input.TextureCoordinates.xy);
    if (color.a == 0)
    {
        if (rColor.a != 0 || tColor.a != 0 || lColor.a != 0 || dColor.a != 0)
            color = float4(250/255, 220/255, 120/255, 1);
    } else if (color.a < 0.95f)
    {
        if (rColor.a != 0 || tColor.a != 0 || lColor.a != 0 || dColor.a != 0)
            color = float4(lerp(float3(1, 0, 0), color.rgb, color.a), 1);
    }
			
    return color;
}


technique SpriteBlending
{
	pass P0
	{
		PixelShader = compile PS_SHADERMODEL MainPS();
	}
};