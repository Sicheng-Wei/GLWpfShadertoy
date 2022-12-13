using System.IO;
using OpenTK.Graphics.OpenGL4;

namespace GLWpfShadertoy
{
    public class GLSLPort
    {
        private static int shaderProgram;   // Program ID
        private static int vertexShader;    // Vertex Shader ID
        private static int fragmentShader;  // Fragment Shader ID
        private static int planetTexture;   // Texture IDs

        // Screen Parameter Allocate
        private static readonly float[] fullScreen =
        {
            -1.0f, -1.0f,
             1.0f, -1.0f,
             1.0f,  1.0f,
            -1.0f,  1.0f
        };

        private static int VAO;
        private static int VBO;
        
        private static void GLInit(byte[] GLTexMap, int[] stats)
        {
            // VBO Binding & vertices to GPU Buffer
            VBO = GL.GenBuffer();
            GL.BindBuffer(BufferTarget.ArrayBuffer, VBO);
            GL.BufferData(BufferTarget.ArrayBuffer, sizeof(float) * fullScreen.Length, fullScreen, BufferUsageHint.StaticDraw);
            
            // VAO Binding
            VAO = GL.GenVertexArray();
            GL.BindVertexArray(VAO);
            GL.VertexAttribPointer(0, 2, VertexAttribPointerType.Float, false, 2 * sizeof(float), 0);

            // Canvas Vertex Attributed
            GL.EnableVertexAttribArray(0);

            // Vertex Shader
            string vertexShaderSource = File.ReadAllText("./Shader/Shader.vert");
            vertexShader = GL.CreateShader(ShaderType.VertexShader);
            GL.ShaderSource(vertexShader, vertexShaderSource);

            // Fragment Shader
            string fragmentShaderSource = File.ReadAllText("./Shader/Shader.frag");
            fragmentShader = GL.CreateShader(ShaderType.FragmentShader);
            GL.ShaderSource(fragmentShader, fragmentShaderSource);

            // Attach Shader to Program
            shaderProgram = GL.CreateProgram();

            // Texture
            planetTexture = GL.GenTexture();
            GL.ActiveTexture(TextureUnit.Texture0 + planetTexture);
            GL.BindTexture(TextureTarget.Texture2D, planetTexture);
            GL.TexImage2D(TextureTarget.Texture2D, 0, PixelInternalFormat.Rgba, stats[0], stats[1], 0, PixelFormat.Rgba, PixelType.UnsignedByte, GLTexMap);
            GL.GenerateMipmap(GenerateMipmapTarget.Texture2D);
            GL.TexParameter(TextureTarget.Texture2D, TextureParameterName.TextureMinFilter, (int)TextureMinFilter.Nearest);
            GL.TexParameter(TextureTarget.Texture2D, TextureParameterName.TextureMagFilter, (int)TextureMagFilter.Nearest);
        }

        public static void Render(byte[] GLTexMap, int[] stats)
        {
            // Clear Canvas
            GL.Clear(ClearBufferMask.ColorBufferBit | ClearBufferMask.DepthBufferBit);

            if (MainWindow.CurrentGLState.iInitialize == false)
            {
                GLInit(GLTexMap, stats);
            }
            
            // Draw Canvas
            GL.DrawArrays(PrimitiveType.TriangleFan, 0, 4);

            // Shader Program Load
            GL.AttachShader(shaderProgram, vertexShader);
            GL.AttachShader(shaderProgram, fragmentShader);
            GL.LinkProgram(shaderProgram);
            GL.UseProgram(shaderProgram);

            // Uniform Data Interaction
            GL.Uniform2(GL.GetUniformLocation(shaderProgram, "iResolution"),    
                        MainWindow.CurrentGLState.iResolution[0],
                        MainWindow.CurrentGLState.iResolution[1]);                  // Pass GLScreenSize as "uniform vec2 iResolution"            

            GL.Uniform1(GL.GetUniformLocation(shaderProgram, "iTime"),
                        MainWindow.CurrentGLState.iTime);                           // Pass TimeSpan as     "uniform float iTime"  
            
            GL.UniformMatrix4(GL.GetUniformLocation(shaderProgram, "viewMatrix"),
                              false, ref MainWindow.CurrentGLState.viewMatrix);     // Pass ViewMatrix as   "uniform mat4 viewMatrix"

            GL.Uniform1(GL.GetUniformLocation(shaderProgram, "iJupitex"),
                        planetTexture);

        }

        public static void Destroy()
        {
            // Clear Buffers
            GL.DeleteVertexArray(VAO);
            GL.DeleteBuffer(VBO);
            GL.DeleteTexture(planetTexture);
            GL.DeleteProgram(shaderProgram);
        }
    }
}
