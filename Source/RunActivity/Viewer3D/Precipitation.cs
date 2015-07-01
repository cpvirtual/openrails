﻿// COPYRIGHT 2010, 2011, 2012, 2013, 2014 by the Open Rails project.
// 
// This file is part of Open Rails.
// 
// Open Rails is free software: you can redistribute it and/or modify
// it under the terms of the GNU General Public License as published by
// the Free Software Foundation, either version 3 of the License, or
// (at your option) any later version.
// 
// Open Rails is distributed in the hope that it will be useful,
// but WITHOUT ANY WARRANTY; without even the implied warranty of
// MERCHANTABILITY or FITNESS FOR A PARTICULAR PURPOSE.  See the
// GNU General Public License for more details.
// 
// You should have received a copy of the GNU General Public License
// along with Open Rails.  If not, see <http://www.gnu.org/licenses/>.

// This file is the responsibility of the 3D & Environment Team. 

using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Runtime.InteropServices;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Graphics.PackedVector;
using ORTS.Common;

namespace ORTS.Viewer3D
{
    public class PrecipitationViewer
    {
        public const float MinIntensityPPSPM2 = 0;
        // 16 bit version.
        public const float MaxIntensityPPSPM2_16 = 0.010f;
        // Default 32 bit version.
        public const float MaxIntensityPPSPM2 = 0.020f;
        // 32 bit version for systems unable to take advantage of the LAA option.
        public const float MaxIntensityPPSPM2_Limited = 0.015f;
        
        readonly Viewer Viewer;
        readonly WeatherControl Weather;

        readonly Material Material;
        readonly PrecipitationPrimitive Pricipitation;

        Vector3 Wind;

        public PrecipitationViewer(Viewer viewer, WeatherControl weather)
        {
            Viewer = viewer;
            Weather = weather;

            Material = viewer.MaterialManager.Load("Precipitation");
            Pricipitation = new PrecipitationPrimitive(Viewer.GraphicsDevice);

            Wind = new Vector3(0, 0, 0);
            Reset();
        }

        public void PrepareFrame(RenderFrame frame, ElapsedTime elapsedTime)
        {
            var gameTime = (float)Viewer.Simulator.GameTime;
            Pricipitation.DynamicUpdate(Weather, Viewer, ref Wind);
            Pricipitation.Update(gameTime, elapsedTime, Weather.pricipitationIntensityPPSPM2, Viewer);

            // Note: This is quite a hack. We ideally should be able to pass this through RenderItem somehow.
            var XNAWorldLocation = Matrix.Identity;
            XNAWorldLocation.M11 = gameTime;
            XNAWorldLocation.M21 = Viewer.Camera.TileX;
            XNAWorldLocation.M22 = Viewer.Camera.TileZ;

            frame.AddPrimitive(Material, Pricipitation, RenderPrimitiveGroup.Precipitation, ref XNAWorldLocation);
        }

        public void Reset()
        {
            // This procedure is only called once at the start of an activity.
            // Added random Wind.X value for rain and snow.
            Random randWind = new Random();
            // Max value used by randWind.Next is max value - 1.
            Wind.X = Viewer.Simulator.Weather == Orts.Formats.Msts.WeatherType.Snow ? randWind.Next(2, 6) : randWind.Next(15, 21);
                                    
            var gameTime = (float)Viewer.Simulator.GameTime;
            Pricipitation.Initialize(Viewer.Simulator.Weather, Wind);
            // Camera is null during first initialisation.
            if (Viewer.Camera != null) Pricipitation.Update(gameTime, null, Weather.pricipitationIntensityPPSPM2, Viewer);
        }

        [CallOnThread("Loader")]
        internal void Mark()
        {
            Material.Mark();
        }
    }

    public class PrecipitationPrimitive : RenderPrimitive
    {
        // http://www-das.uwyo.edu/~geerts/cwx/notes/chap09/hydrometeor.html
        // "Rain  1.8 - 2.2mm  6.1 - 6.9m/s"
        const float RainVelocityMpS = 6.9f;
        // "Snow flakes of any size falls at about 1 m/s"
        const float SnowVelocityMpS = 1.0f;
        // This is a fiddle factor because the above values feel too slow. Alternative suggestions welcome.
        const float ParticleVelocityFactor = 10.0f;

        readonly float ParticleBoxLengthM;
        readonly float ParticleBoxWidthM;
        readonly float ParticleBoxHeightM;

        // 32bitbit Box Parameters
        const float ParticleBoxLengthM_32 = 2000;
        const float ParticleBoxWidthM_32 = 1000;
        const float ParticleBoxHeightM_32 = 240;

        // 32bitbit Box Parameters for systems that are unable to take advantage of LAA
        const float ParticleBoxLengthM_Limited_32 = 1400;
        const float ParticleBoxWidthM_Limited_32 = 870;
        const float ParticleBoxHeightM_Limited_32 = 175;

        // 16bit Box Parameters
        const float ParticleBoxLengthM_16 = 500;
        const float ParticleBoxWidthM_16 = 500;
        const float ParticleBoxHeightM_16 = 43;

        const int IndiciesPerParticle = 6;
        const int VerticiesPerParticle = 4;
        const int PrimitivesPerParticle = 2;

        readonly int MaxParticles;
        readonly ParticleVertex[] Vertices;
        readonly VertexDeclaration VertexDeclaration;
        readonly int VertexStride;
        readonly DynamicVertexBuffer VertexBuffer;
        readonly IndexBuffer IndexBuffer;

        struct ParticleVertex
        {
            public Vector4 StartPosition_StartTime;
            public Vector4 EndPosition_EndTime;
            public Short4 TileXZ_Vertex;

            public static readonly VertexElement[] VertexElements =
            {
                new VertexElement(0, 0, VertexElementFormat.Vector4, VertexElementMethod.Default, VertexElementUsage.Position, 0),
                new VertexElement(0, 16, VertexElementFormat.Vector4, VertexElementMethod.Default, VertexElementUsage.Position, 1),
                new VertexElement(0, 16 + 16, VertexElementFormat.Short4, VertexElementMethod.Default, VertexElementUsage.Position, 2),
            };
        }

        float ParticleDuration;
        Vector3 ParticleDirection;
        HeightCache Heights;

        // Particle buffer goes like this:
        //   +--active>-----new>--+
        //   |                    |
        //   +--<retired---<free--+

        int FirstActiveParticle;
        int FirstNewParticle;
        int FirstFreeParticle;
        int FirstRetiredParticle;

        float ParticlesToEmit;
        float TimeParticlesLastEmitted;
        int DrawCounter;

        public PrecipitationPrimitive(GraphicsDevice graphicsDevice)
        {
            // Snow is the slower particle, hence longer duration, hence more particles in total.
            // Setting the precipitaton box size based on GraphicsDeviceCapabilities.
            if (graphicsDevice.GraphicsDeviceCapabilities.MaxVertexIndex > 0xFFFF) // As an integer, 0xFFFF is 65535.
            {
                // Testing if UseLargeAddressAware has been selected.
                if(Program.Simulator.Settings.UseLargeAddressAware)
                {
                    ParticleBoxLengthM = ParticleBoxLengthM_32;
                    ParticleBoxWidthM = ParticleBoxWidthM_32;
                    ParticleBoxHeightM = ParticleBoxHeightM_32;
                }
                // if UseLargeAddressAware is not selected, use box size with smaller values.
                else
                {
                    ParticleBoxLengthM = ParticleBoxLengthM_Limited_32;
                    ParticleBoxWidthM = ParticleBoxWidthM_Limited_32;
                    ParticleBoxHeightM = ParticleBoxHeightM_Limited_32;
                }
            }
            else
            {
                ParticleBoxLengthM = ParticleBoxLengthM_16;
                ParticleBoxWidthM = ParticleBoxWidthM_16;
                ParticleBoxHeightM = ParticleBoxHeightM_16;
            }
            if (graphicsDevice.GraphicsDeviceCapabilities.MaxVertexIndex > 0xFFFF) // As an integer, 0xFFFF is 65535.
            {
                // Testing if UseLargeAddressAware has been selected.
                if(Program.Simulator.Settings.UseLargeAddressAware)
                    MaxParticles = (int)(PrecipitationViewer.MaxIntensityPPSPM2 * ParticleBoxLengthM * ParticleBoxWidthM * ParticleBoxHeightM / SnowVelocityMpS / ParticleVelocityFactor);
                else
                    MaxParticles = (int)(PrecipitationViewer.MaxIntensityPPSPM2_Limited * ParticleBoxLengthM * ParticleBoxWidthM * ParticleBoxHeightM / SnowVelocityMpS / ParticleVelocityFactor);
            }
            // Processing 16bit device
            else
                MaxParticles = (int)(PrecipitationViewer.MaxIntensityPPSPM2_16 * ParticleBoxLengthM * ParticleBoxWidthM * ParticleBoxHeightM / SnowVelocityMpS / ParticleVelocityFactor);
            // Checking if graphics device is 16bit.
            if (graphicsDevice.GraphicsDeviceCapabilities.MaxVertexIndex == 0xFFFF)
                Debug.Assert(MaxParticles * VerticiesPerParticle < ushort.MaxValue, "The maximum number of precipitation verticies must be able to fit in a ushort (16bit unsigned) index buffer.");
            Vertices = new ParticleVertex[MaxParticles * VerticiesPerParticle];
            VertexDeclaration = new VertexDeclaration(graphicsDevice, ParticleVertex.VertexElements);
            VertexStride = Marshal.SizeOf(typeof(ParticleVertex));
            VertexBuffer = new DynamicVertexBuffer(graphicsDevice, typeof(ParticleVertex), MaxParticles * VerticiesPerParticle, BufferUsage.WriteOnly);
            VertexBuffer.ContentLost += VertexBuffer_ContentLost;
            // Processing either 32bit or 16bit InitIndexBuffer depending on GraphicsDeviceCapabilities.
           if (graphicsDevice.GraphicsDeviceCapabilities.MaxVertexIndex > 0xFFFF) // As an integer, 0xFFFF is 65535.
               IndexBuffer = InitIndexBuffer(graphicsDevice, MaxParticles * IndiciesPerParticle);
           else
               IndexBuffer = InitIndexBuffer16(graphicsDevice, MaxParticles * IndiciesPerParticle);
            Heights = new HeightCache(8);
            // This Trace command is used to show how much memory is used.
            //Trace.TraceInformation(String.Format("Allocation for {0:N0} particles:\n\n  {1,13:N0} B RAM vertex data\n  {2,13:N0} B RAM index data (temporary)\n  {1,13:N0} B VRAM DynamicVertexBuffer\n  {2,13:N0} B VRAM IndexBuffer", MaxParticles, Marshal.SizeOf(typeof(ParticleVertex)) * MaxParticles * VerticiesPerParticle, (graphicsDevice.GraphicsDeviceCapabilities.MaxVertexIndex > 0xFFFF ? sizeof(uint) : sizeof(ushort)) * MaxParticles * IndiciesPerParticle));
        }

        void VertexBuffer_ContentLost(object sender, EventArgs e)
        {
            VertexBuffer.SetData(0, Vertices, 0, Vertices.Length, VertexStride, SetDataOptions.NoOverwrite);
        }
        // IndexBuffer for 32bit process.
        static IndexBuffer InitIndexBuffer(GraphicsDevice graphicsDevice, int numIndicies)
        {
            var indices = new uint[numIndicies];
            var index = 0;
            for (var i = 0; i < numIndicies; i += IndiciesPerParticle)
            {
                indices[i] = (uint)index;
                indices[i + 1] = (uint)(index + 1);
                indices[i + 2] = (uint)(index + 2);

                indices[i + 3] = (uint)(index + 2);
                indices[i + 4] = (uint)(index + 3);
                indices[i + 5] = (uint)(index);

                index += VerticiesPerParticle;
            }
            var indexBuffer = new IndexBuffer(graphicsDevice, sizeof(uint) * numIndicies, BufferUsage.WriteOnly, IndexElementSize.ThirtyTwoBits);
            indexBuffer.SetData(indices);
            return indexBuffer;
        }
        // IndexBuffer for computers that still use 16bit graphics.
        static IndexBuffer InitIndexBuffer16(GraphicsDevice graphicsDevice, int numIndicies)
        {
            var indices = new ushort[numIndicies];
            var index = 0;
            for (var i = 0; i < numIndicies; i += IndiciesPerParticle)
            {
                indices[i] = (ushort)index;
                indices[i + 1] = (ushort)(index + 1);
                indices[i + 2] = (ushort)(index + 2);

                indices[i + 3] = (ushort)(index + 2);
                indices[i + 4] = (ushort)(index + 3);
                indices[i + 5] = (ushort)(index);

                index += VerticiesPerParticle;
            }
            var indexBuffer = new IndexBuffer(graphicsDevice, sizeof(ushort) * numIndicies, BufferUsage.WriteOnly, IndexElementSize.SixteenBits);
            indexBuffer.SetData(indices);
            return indexBuffer;
        }

        void RetireActiveParticles(float currentTime)
        {
            while (FirstActiveParticle != FirstNewParticle)
            {
                var vertex = FirstActiveParticle * VerticiesPerParticle;
                var expiry = Vertices[vertex].EndPosition_EndTime.W;

                // Stop as soon as we find the first particle which hasn't expired.
                if (expiry > currentTime)
                    break;

                // Expire particle.
                Vertices[vertex].StartPosition_StartTime.W = (float)DrawCounter;
                FirstActiveParticle = (FirstActiveParticle + 1) % MaxParticles;
            }
        }

        void FreeRetiredParticles()
        {
            while (FirstRetiredParticle != FirstActiveParticle)
            {
                var vertex = FirstRetiredParticle * VerticiesPerParticle;
                var age = DrawCounter - (int)Vertices[vertex].StartPosition_StartTime.W;

                // Stop as soon as we find the first expired particle which hasn't been expired for at least 2 'ticks'.
                if (age < 2)
                    break;

                FirstRetiredParticle = (FirstRetiredParticle + 1) % MaxParticles;
            }
        }

        int GetCountFreeParticles()
        {
            var nextFree = (FirstFreeParticle + 1) % MaxParticles;

            if (nextFree <= FirstRetiredParticle)
                return FirstRetiredParticle - nextFree;

            return (MaxParticles - nextFree) + FirstRetiredParticle;
        }

        public void Initialize(Orts.Formats.Msts.WeatherType weather, Vector3 wind)
        {
            ParticleDuration = ParticleBoxHeightM / (weather == Orts.Formats.Msts.WeatherType.Snow ? SnowVelocityMpS : RainVelocityMpS) / ParticleVelocityFactor;
            ParticleDirection = wind;
            FirstActiveParticle = FirstNewParticle = FirstFreeParticle = FirstRetiredParticle = 0;
            ParticlesToEmit = TimeParticlesLastEmitted = 0;
            DrawCounter = 0;
        }

        public void DynamicUpdate(WeatherControl weatherControl, Viewer viewer, ref Vector3 wind)
        {
            if (!weatherControl.weatherChangeOn || weatherControl.dynamicWeather.precipitationLiquidityTimer <= 0) return;
            ParticleDuration = ParticleBoxHeightM / ((RainVelocityMpS-SnowVelocityMpS) *  weatherControl.precipitationLiquidity + SnowVelocityMpS)/ ParticleVelocityFactor;
            wind.X = 18 * weatherControl.precipitationLiquidity + 2;
            ParticleDirection = wind;
        }

        public void Update(float currentTime, ElapsedTime elapsedTime, float particlesPerSecondPerM2, Viewer viewer)
        {
            var tiles = viewer.Tiles;
            var scenery = viewer.World.Scenery;
            var worldLocation = viewer.Camera.CameraWorldLocation;
                        
            if (TimeParticlesLastEmitted == 0)
            {
                TimeParticlesLastEmitted = currentTime - ParticleDuration;
                ParticlesToEmit += ParticleDuration * particlesPerSecondPerM2 * ParticleBoxLengthM * ParticleBoxWidthM;
            }
            else
            {
                RetireActiveParticles(currentTime);
                FreeRetiredParticles();

                ParticlesToEmit += elapsedTime.ClockSeconds * particlesPerSecondPerM2 * ParticleBoxLengthM * ParticleBoxWidthM;
            }

            var numParticlesAdded = 0;
            var numToBeEmitted = (int)ParticlesToEmit;
            var numCanBeEmitted = GetCountFreeParticles();
            var numToEmit = Math.Min(numToBeEmitted, numCanBeEmitted);

            for (var i = 0; i < numToEmit; i++)
            {
                var temp = new WorldLocation(worldLocation.TileX, worldLocation.TileZ, worldLocation.Location.X + (float)((Program.Random.NextDouble() - 0.5) * ParticleBoxLengthM), 0, worldLocation.Location.Z + (float)((Program.Random.NextDouble() - 0.5) * ParticleBoxWidthM));
                temp.Location.Y = Heights.GetHeight(temp, tiles, scenery);
                var position = new WorldPosition(temp);

                var time = MathHelper.Lerp(TimeParticlesLastEmitted, currentTime, (float)i / numToEmit);
                var particle = (FirstFreeParticle + 1) % MaxParticles;
                var vertex = particle * VerticiesPerParticle;

                for (var j = 0; j < VerticiesPerParticle; j++)
                {
                    Vertices[vertex + j].StartPosition_StartTime = new Vector4(position.XNAMatrix.Translation - ParticleDirection * ParticleDuration, time);
                    Vertices[vertex + j].StartPosition_StartTime.Y += ParticleBoxHeightM;
                    Vertices[vertex + j].EndPosition_EndTime = new Vector4(position.XNAMatrix.Translation, time + ParticleDuration);
                    Vertices[vertex + j].TileXZ_Vertex = new Short4(position.TileX, position.TileZ, j, 0);
                }

                FirstFreeParticle = particle;
                ParticlesToEmit--;
                numParticlesAdded++;
            }

            if (numParticlesAdded > 0)
                TimeParticlesLastEmitted = currentTime;

            ParticlesToEmit = ParticlesToEmit - (int)ParticlesToEmit;
        }

        void AddNewParticlesToVertexBuffer()
        {
            if (FirstNewParticle < FirstFreeParticle)
            {
                var numParticlesToAdd = FirstFreeParticle - FirstNewParticle;
                VertexBuffer.SetData(FirstNewParticle * VertexStride * VerticiesPerParticle, Vertices, FirstNewParticle * VerticiesPerParticle, numParticlesToAdd * VerticiesPerParticle, VertexStride, SetDataOptions.NoOverwrite);
            }
            else
            {
                var numParticlesToAddAtEnd = MaxParticles - FirstNewParticle;
                VertexBuffer.SetData(FirstNewParticle * VertexStride * VerticiesPerParticle, Vertices, FirstNewParticle * VerticiesPerParticle, numParticlesToAddAtEnd * VerticiesPerParticle, VertexStride, SetDataOptions.NoOverwrite);
                if (FirstFreeParticle > 0)
                    VertexBuffer.SetData(0, Vertices, 0, FirstFreeParticle * VerticiesPerParticle, VertexStride, SetDataOptions.NoOverwrite);
            }

            FirstNewParticle = FirstFreeParticle;
        }

        public bool HasParticlesToRender()
        {
            return FirstActiveParticle != FirstFreeParticle;
        }

        public override void Draw(GraphicsDevice graphicsDevice)
        {
            if (FirstNewParticle != FirstFreeParticle)
                AddNewParticlesToVertexBuffer();

            if (HasParticlesToRender())
            {
                graphicsDevice.Indices = IndexBuffer;
                graphicsDevice.VertexDeclaration = VertexDeclaration;
                graphicsDevice.Vertices[0].SetSource(VertexBuffer, 0, VertexStride);

                if (FirstActiveParticle < FirstFreeParticle)
                {
                    var numParticles = FirstFreeParticle - FirstActiveParticle;
                    graphicsDevice.DrawIndexedPrimitives(PrimitiveType.TriangleList, 0, FirstActiveParticle * VerticiesPerParticle, numParticles * VerticiesPerParticle, FirstActiveParticle * IndiciesPerParticle, numParticles * PrimitivesPerParticle);
                }
                else
                {
                    var numParticlesAtEnd = MaxParticles - FirstActiveParticle;
                    if (numParticlesAtEnd > 0)
                        graphicsDevice.DrawIndexedPrimitives(PrimitiveType.TriangleList, 0, FirstActiveParticle * VerticiesPerParticle, numParticlesAtEnd * VerticiesPerParticle, FirstActiveParticle * IndiciesPerParticle, numParticlesAtEnd * PrimitivesPerParticle);
                    if (FirstFreeParticle > 0)
                        graphicsDevice.DrawIndexedPrimitives(PrimitiveType.TriangleList, 0, 0, FirstFreeParticle * VerticiesPerParticle, 0, FirstFreeParticle * PrimitivesPerParticle);
                }
            }

            DrawCounter++;
        }

        class HeightCache
        {
            const int TileCount = 10;

            readonly int BlockSize;
            readonly int Divisions;
            readonly List<Tile> Tiles = new List<Tile>();

            public HeightCache(int blockSize)
            {
                BlockSize = blockSize;
                Divisions = (int)Math.Round(2048f / blockSize);
            }

            public float GetHeight(WorldLocation location, TileManager tiles, SceneryDrawer scenery)
            {
                location.Normalize();

                // First, ensure we have the tile in question cached.
                var tile = Tiles.FirstOrDefault(t => t.TileX == location.TileX && t.TileZ == location.TileZ);
                if (tile == null)
                    Tiles.Add(tile = new Tile(location.TileX, location.TileZ, Divisions));

                // Remove excess entries.
                if (Tiles.Count > TileCount)
                    Tiles.RemoveAt(0);

                // Now calculate division to query.
                var x = (int)((location.Location.X + 1024) / BlockSize);
                var z = (int)((location.Location.Z + 1024) / BlockSize);

                // If we don't have it cached, load it.
                if (tile.Height[x, z] == float.MinValue)
                {
                    var position = new WorldLocation(location.TileX, location.TileZ, (x + 0.5f) * BlockSize - 1024, 0, (z + 0.5f) * BlockSize - 1024);
                    tile.Height[x, z] = Math.Max(tiles.GetElevation(position), scenery.GetBoundingBoxTop(position, BlockSize));
                    tile.Used++;
                }

                return tile.Height[x, z];
            }

            [DebuggerDisplay("Tile = {TileX},{TileZ} Used = {Used}")]
            class Tile
            {
                public readonly int TileX;
                public readonly int TileZ;
                public readonly float[,] Height;
                public int Used;

                public Tile(int tileX, int tileZ, int divisions)
                {
                    TileX = tileX;
                    TileZ = tileZ;
                    Height = new float[divisions, divisions];
                    for (var x = 0; x < divisions; x++)
                        for (var z = 0; z < divisions; z++)
                            Height[x, z] = float.MinValue;
                }
            }
        }
    }

    public class PrecipitationMaterial : Material
    {
        Texture2D RainTexture;
        Texture2D SnowTexture;
        Texture2D[] DynamicPrecipitationTexture = new Texture2D[12];
        IEnumerator<EffectPass> ShaderPasses;

        public PrecipitationMaterial(Viewer viewer)
            : base(viewer, null)
        {
            // TODO: This should happen on the loader thread.
            RainTexture = SharedTextureManager.Get(Viewer.RenderProcess.GraphicsDevice, System.IO.Path.Combine(Viewer.ContentPath, "Raindrop.png"));
            SnowTexture = SharedTextureManager.Get(Viewer.RenderProcess.GraphicsDevice, System.IO.Path.Combine(Viewer.ContentPath, "Snowflake.png"));
            DynamicPrecipitationTexture[0] = SnowTexture;
            DynamicPrecipitationTexture[11] = RainTexture;
            for (int i = 1; i<=10; i++)
            {
                var path = "Raindrop" + i.ToString() + ".png";
                DynamicPrecipitationTexture[11 - i] = SharedTextureManager.Get(Viewer.RenderProcess.GraphicsDevice, System.IO.Path.Combine(Viewer.ContentPath, path));
            }
        }

        public override void SetState(GraphicsDevice graphicsDevice, Material previousMaterial)
        {
            var shader = Viewer.MaterialManager.PrecipitationShader;
            shader.CurrentTechnique = shader.Techniques["Pricipitation"];
            if (ShaderPasses == null) ShaderPasses = shader.Techniques["Pricipitation"].Passes.GetEnumerator();

            shader.LightVector.SetValue(Viewer.Settings.UseMSTSEnv ? Viewer.World.MSTSSky.mstsskysolarDirection : Viewer.World.Sky.solarDirection);
            shader.particleSize.SetValue(1);
            if (!Viewer.World.WeatherControl.weatherChangeOn)
            shader.precipitation_Tex.SetValue(Viewer.Simulator.Weather == Orts.Formats.Msts.WeatherType.Snow ? SnowTexture : RainTexture);
            else
            {
                var precipitation_TexIndex = (int)(Viewer.World.WeatherControl.precipitationLiquidity * 11);
                shader.precipitation_Tex.SetValue(DynamicPrecipitationTexture[precipitation_TexIndex]);
            }

            var rs = graphicsDevice.RenderState;
            rs.AlphaBlendEnable = true;
            rs.DepthBufferWriteEnable = false;
            rs.DestinationBlend = Blend.InverseSourceAlpha;
            rs.SourceBlend = Blend.SourceAlpha;
        }

        public override void Render(GraphicsDevice graphicsDevice, IEnumerable<RenderItem> renderItems, ref Matrix XNAViewMatrix, ref Matrix XNAProjectionMatrix)
        {
            var shader = Viewer.MaterialManager.PrecipitationShader;

            shader.Begin();
            ShaderPasses.Reset();
            while (ShaderPasses.MoveNext())
            {
                ShaderPasses.Current.Begin();
                foreach (var item in renderItems)
                {
                    // Note: This is quite a hack. We ideally should be able to pass this through RenderItem somehow.
                    shader.cameraTileXZ.SetValue(new Vector2(item.XNAMatrix.M21, item.XNAMatrix.M22));
                    shader.currentTime.SetValue(item.XNAMatrix.M11);

                    shader.SetMatrix(Matrix.Identity, ref XNAViewMatrix, ref XNAProjectionMatrix);
                    shader.CommitChanges();
                    item.RenderPrimitive.Draw(graphicsDevice);
                }
                ShaderPasses.Current.End();
            }
            shader.End();
        }

        public override void ResetState(GraphicsDevice graphicsDevice)
        {
            var rs = graphicsDevice.RenderState;
            rs.AlphaBlendEnable = false;
            rs.DepthBufferWriteEnable = true;
            rs.DestinationBlend = Blend.Zero;
            rs.SourceBlend = Blend.One;
        }

        public override bool GetBlending()
        {
            return true;
        }

        public override void Mark()
        {
            Viewer.TextureManager.Mark(RainTexture);
            Viewer.TextureManager.Mark(SnowTexture);
            for (int i = 1; i <= 10; i++)
                Viewer.TextureManager.Mark(DynamicPrecipitationTexture[i]);
            base.Mark();
        }
    }

    [CallOnThread("Render")]
    public class PrecipitationShader : Shader
    {
        internal readonly EffectParameter worldViewProjection;
        internal readonly EffectParameter invView;
        internal readonly EffectParameter LightVector;
        internal readonly EffectParameter particleSize;
        internal readonly EffectParameter cameraTileXZ;
        internal readonly EffectParameter currentTime;
        internal readonly EffectParameter precipitation_Tex;

        public PrecipitationShader(GraphicsDevice graphicsDevice)
            : base(graphicsDevice, "PrecipitationShader")
        {
            worldViewProjection = Parameters["worldViewProjection"];
            invView = Parameters["invView"];
            LightVector = Parameters["LightVector"];
            particleSize = Parameters["particleSize"];
            cameraTileXZ = Parameters["cameraTileXZ"];
            currentTime = Parameters["currentTime"];
            precipitation_Tex = Parameters["precipitation_Tex"];
        }

        public void SetMatrix(Matrix world, ref Matrix view, ref Matrix projection)
        {
            worldViewProjection.SetValue(world * view * projection);
            invView.SetValue(Matrix.Invert(view));
        }
    }
}
