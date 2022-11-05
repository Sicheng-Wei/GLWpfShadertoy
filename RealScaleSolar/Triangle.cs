using OpenTK;
using OpenTK.Graphics;
using OpenTK.Graphics.OpenGL4;
using System.Threading.Tasks;

namespace RealScaleSolar
{
    public class Triangle
    {
        public static async Task Render()
        {
            GL.Clear(ClearBufferMask.ColorBufferBit | ClearBufferMask.DepthBufferBit);
            float[] vertices = {
                -0.5f, -0.5f, 0.0f,
                 0.5f, -0.5f, 0.0f,
                 0.0f,  0.5f, 0.0f
            };  

            int VAO = GL.GenVertexArray();
            int VBO = GL.GenBuffer();

            // VAO Binding
            GL.BindVertexArray(VAO);

            // VBO Binding & vertices to GPU Buffer
            GL.BindBuffer(BufferTarget.ArrayBuffer, VBO);
            GL.BufferData(BufferTarget.ArrayBuffer, sizeof(float) * 9, vertices, BufferUsageHint.StaticDraw);

            // VBO Binding & vertices to GPU Buffer
            GL.VertexAttribPointer(0, 3, VertexAttribPointerType.Float, false, 3 * sizeof(float), 0);
            GL.EnableVertexAttribArray(0);

            // Vertex Shader
            string vertexShaderSource = "#version 330 core\nlayout (location = 0) in vec3 aPos;\n\nvoid main()\n{\n    gl_Position = vec4(aPos.x, aPos.y, aPos.z, 1.0);\n}";
            int vertexShader = GL.CreateShader(ShaderType.VertexShader);
            GL.ShaderSource(vertexShader, vertexShaderSource);

            // Fragment Shader
            string fragmentShaderSource = "#version 330 core\nout vec4 FragColor;\n\nvoid main()\n{\n    FragColor = vec4(1.0f, 0.8f, 0.6f, 1.0f);\n} ";
            int fragmentShader = GL.CreateShader(ShaderType.FragmentShader);
            GL.ShaderSource(fragmentShader, fragmentShaderSource);

            // Attach Shader to Program
            int shaderProgram = GL.CreateProgram();
            GL.AttachShader(shaderProgram, vertexShader);
            GL.AttachShader(shaderProgram, fragmentShader);
            GL.LinkProgram(shaderProgram);
            GL.UseProgram(shaderProgram);

            // Draw Triangle
            GL.DrawArrays(PrimitiveType.Triangles, 0, 3);

            await Task.Delay(0);
        }

    }
}
