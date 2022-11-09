using System.IO;
using OpenTK.Graphics.OpenGL4;

namespace RealScaleSolar
{
    public class GLSLPort
    {
        public static void Render(float[] GLScreenSize, float GLiTime)
        {
            GL.Clear(ClearBufferMask.ColorBufferBit | ClearBufferMask.DepthBufferBit);

            float[] fullScreen =
            {
                -1.0f, -1.0f,
                 1.0f, -1.0f,
                 1.0f,  1.0f,
                -1.0f,  1.0f
            };

            int VAO = GL.GenVertexArray();
            int VBO = GL.GenBuffer();

            // VAO Binding
            GL.BindVertexArray(VAO);

            // VBO Binding & vertices to GPU Buffer
            GL.BindBuffer(BufferTarget.ArrayBuffer, VBO);
            GL.BufferData(BufferTarget.ArrayBuffer, sizeof(float) * fullScreen.Length, fullScreen, BufferUsageHint.StaticDraw);

            // VBO Binding & vertices to GPU Buffer
            GL.VertexAttribPointer(0, 2, VertexAttribPointerType.Float, false, 2 * sizeof(float), 0);

            // Canvas Vertex Attributed
            GL.EnableVertexAttribArray(0);

            // Vertex Shader
            string vertexShaderSource = File.ReadAllText("./Shader/Shader.vert");
            int vertexShader = GL.CreateShader(ShaderType.VertexShader);
            GL.ShaderSource(vertexShader, vertexShaderSource);

            // Fragment Shader
            string fragmentShaderSource = File.ReadAllText("./Shader/Shader.frag");
            int fragmentShader = GL.CreateShader(ShaderType.FragmentShader);
            GL.ShaderSource(fragmentShader, fragmentShaderSource);

            // Attach Shader to Program
            int shaderProgram = GL.CreateProgram();

            // Shader Program Load
            GL.AttachShader(shaderProgram, vertexShader);
            GL.AttachShader(shaderProgram, fragmentShader);
            GL.LinkProgram(shaderProgram);
            GL.UseProgram(shaderProgram);

            // Uniform Data Interaction
            GL.Uniform2(GL.GetUniformLocation(shaderProgram, "iResolution"), GLScreenSize[0], GLScreenSize[1]);
            GL.Uniform1(GL.GetUniformLocation(shaderProgram, "iTime"), GLiTime);

            // Draw Canvas
            GL.DrawArrays(PrimitiveType.TriangleFan, 0, 4);
            
            // Clear Buffers
            GL.DeleteVertexArray(VAO);
            GL.DeleteBuffer(VBO);
            GL.DeleteProgram(shaderProgram);
        }
    }
}
