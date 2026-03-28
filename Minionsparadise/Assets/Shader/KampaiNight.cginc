#ifndef KAMPAI_NIGHT_INCLUDED
#define KAMPAI_NIGHT_INCLUDED

// Global parameters set by C# script
uniform half3 _GlobalNightTint;      // RGB tint for night (e.g., 0.2, 0.3, 0.6)
uniform half _GlobalNightFactor;    // 0 = Day, 1 = Night
uniform half _GlobalNightSaturation; // 0 = BW, 1 = Normal

// Applied to specific materials
// _NightGlow is defined in the shader's Properties and used here.

half3 ApplyKampaiNight(half3 color, half glow) {
    // 1. Calculate darkened color
    half3 nightColor = color * _GlobalNightTint;
    
    // 2. Linear interpolation between day and night based on GlobalNightFactor
    half3 finalColor = lerp(color, nightColor, _GlobalNightFactor);
    
    // 3. Compensation for materials that should glow
    // If glow is 1, we want (color) even at night.
    // So we lerp back towards 'color' based on glow * factor.
    finalColor = lerp(finalColor, color, glow * _GlobalNightFactor);
    
    return finalColor;
}

#endif
