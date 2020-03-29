#ifndef BREAKEDIT_GRID_GENERATOR_HLSL
#define BREAKEDIT_GRID_GENERATOR_HLSL

void GenerateGridXZ_float(in float3 WorldSpace, in float Threshold, out float AlphaMap)
{
	// Create grid
	WorldSpace = abs(WorldSpace);
	WorldSpace %= 1;
	WorldSpace -= 0.5f;
	WorldSpace = abs(WorldSpace);
	
	WorldSpace *= 2;
	//WorldSpace = 1.0f - WorldSpace;
	
	WorldSpace -= 1.0f - Threshold;
	WorldSpace = clamp(WorldSpace, 0.0f, 1.0f);
	float alpha = WorldSpace.x + WorldSpace.z;
	AlphaMap = alpha;
}

#endif