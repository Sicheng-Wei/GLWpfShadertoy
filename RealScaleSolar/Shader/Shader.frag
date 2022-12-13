#version 460 core

// WPF-Shadertoy Transfer Macros
# define mainImage main

uniform vec2    iResolution;
uniform float   iTime;
uniform vec3    iMouse;             // TODO; Add Mouse
out vec4 fragColor;
vec2 fragCoord = gl_FragCoord.xy;

// CONSTANTS
const float PI = acos(-1.);

// VIEW TRANSFORMATION UNIFORMS
uniform mat4 viewMatrix;

// VERTEX SHADER INTERFACE
in vec2 texCoord;

// TEXTURES
uniform sampler2D[1] iJupitex;

// Sphere Intersect [Shadertoy Link: https://www.shadertoy.com/view/XdBGzd]
float genSphere( in vec3 ro, in vec3 rd, in vec4 sph )
{
	vec3 oc = ro - sph.xyz;
	float b = dot( oc, rd );
	float c = dot( oc, oc ) - sph.w * sph.w;
	float h = b * b - c;
	if( h < 0. ) return -1.;
	return -b - sqrt( h );
}

vec3 mapSphere( in vec3 p )
{
    float lat = - 90. + acos( p.y / length( p ) ) * 180. / PI;
    float lon = atan( p.x, p.z ) * 180. / PI;
    vec2 uv = vec2( lon / 360., lat / 180. ) + 0.5;
    return texture( iJupitex[0], uv ).rgb;
}

// TODO: Ring Intersect
// TODO: Lighting
// TODO: Integrate Planet Rendering
// TODO: Ray-Marching for Gas Planet


void mainImage( )
{
    // Screen Normalization
    vec2 iScreenNorm = ( 2. * fragCoord.xy - iResolution.xy ) / iResolution.x;
    
    // View Normalized Vector
    mat3 viewMatrix3d = mat3( viewMatrix );
    vec3 camSite = viewMatrix[3].xyz;
    vec3 camDir = normalize( viewMatrix3d * vec3( 20., iScreenNorm.y, iScreenNorm.x ) );

    // Sphere Params
    vec4 sphParams = vec4( 50., 0., 0., 1. );

    float dist = genSphere( camSite, camDir, sphParams );

    fragColor = vec4( 0 );
    if ( dist >= 0. ) {
      vec3 q = camSite - sphParams.xyz + camDir * dist;
      fragColor = vec4( mapSphere( q ), 1. );
    }
}