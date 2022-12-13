#version 460 core

// CONSTANTS
const float PI = 3.1415926535;

// WPF MAINWINDOW MACRO
uniform vec2 iResolution;
uniform float iTime;
uniform vec3 iMouse;
out vec4 fragColor;
vec2 fragCoord = gl_FragCoord.xy;

// VIEW TRANSFORMATION UNIFORMS
uniform mat4 viewMatrix;

// VERTEX SHADER INTERFACE
in vec2 texCoord;

// TEXTURES
uniform sampler2D iJupitex;

// Sphere Intersect [Shadertoy Link (MIT License): https://www.shadertoy.com/view/XdBGzd]
float iSphere( in vec3 ro, in vec3 rd, in vec4 sph )
{
	vec3 oc = ro - sph.xyz;
	float b = dot( oc, rd );
	float c = dot( oc, oc ) - sph.w * sph.w;
	float h = b * b - c;
	if( h < 0.0 ) return -1.0;
	return -b - sqrt( h );
}

vec3 map(vec3 p)
{
    float lat = - 90. + acos(p.y / length(p)) * 180./ PI;
    float lon = + atan(p.x, p.z) * 180. / PI;
    vec2 uv = vec2(lon / 360., lat / 180.) + 0.5;
    return texture(iJupitex, uv).rgb;
}

void texPlanet(vec3 planetSite, sampler2D planetTex)
{
    
}

void main()
{
    // Screen Normalization
    vec2 iScreenNorm = (2. * fragCoord.xy - iResolution.xy) / iResolution.x;
    
    // View Normalized Vector
    mat3 iViewMatrix = mat3(viewMatrix);
    vec3 camSite = viewMatrix[3].xyz;
    vec3 camDir = normalize(iViewMatrix * vec3(20.0, iScreenNorm.y, iScreenNorm.x));

    float dist = iSphere(camSite, camDir, vec4(50.0, 0.0, 0, 1.0));

    fragColor = vec4(0);
    if (dist >= 0.) {
        vec3 q = camSite - vec3(50.0, 0.0, 0) + camDir * dist;
        fragColor = vec4(map(q), 1.0);
    }
}