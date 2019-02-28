using System;
using UnityEngine;

namespace game
{
	public static class UnityHelper
	{
		public static float Min(float a, float b, float c)
		{
			return Mathf.Min(Math.Min(a, b), c);
		}

		public static float Max(float a, float b, float c)
		{
			return Mathf.Max(Math.Max(a, b), c);
		}

		public static int Min(int a, int b, int c)
		{
			return Mathf.Min(Math.Min(a, b), c);
		}

		public static int Max(int a, int b, int c)
		{
			return Mathf.Max(Math.Max(a, b), c);
		}

		public static int Clamp(int value, int min, int max)
		{
			if (value < min) return min;
			if (value > max) return max;
			return value;
		}

		public static int ModInt(int n, int m)
		{
			return ((n %= m) < 0) ? n + m : n;
		}

		// https://answers.unity.com/questions/380035/c-modulus-is-wrong-1.html
		public static float Modf(float a, float b)
		{
			return a - b * Mathf.Floor(a / b);
		}

		// https://codereview.stackexchange.com/questions/57923/index-into-array-as-if-it-is-circular
		public static int WrapAroundIndex(int index, int length)
		{
			return ((index % length) + length) % length;
		}

		public static float NormalizeAngle(float angleDegrees)
		{
			return NormalizeValue(angleDegrees, 0f, 360f);
		}

		public static float AngleFromDirection(Vector2 direction)
		{
			return Quaternion.FromToRotation(Vector3.up, direction).eulerAngles.z;
		}

		// Normalizes any number to an arbitrary range by assuming the range wraps around when going below min or above max
		public static float NormalizeValue(float value, float start, float end)
		{
			float width = end - start;
			float offset = value - start;
			return (offset - (Mathf.Floor(offset / width) * width)) + start;
		}

		public static int NormalizeValue(int value, int start, int end)
		{
			int width = end - start;
			int offset = value - start;
			return (offset - ((offset / width) * width)) + start;
		}

		// Returns the angle between vectors from and to [-180,180]
		public static float VectorAngle180(Vector2 from, Vector2 to)
		{
			float angle = Vector3.Angle(from, to);
			Vector3 cross = Vector3.Cross(from, to);
			if (cross.z > 0)
			{
				angle = -angle;
			}
			return angle;
		}

		// Returns the angle between vectors from and to [0,360]
		public static float VectorAngle360(Vector2 from, Vector2 to)
		{
			float angle = Vector3.Angle(from, to);
			Vector3 cross = Vector3.Cross(from, to);
			if (cross.z > 0)
			{
				angle = 360 - angle;
			}
			return angle;
		}

		public static Vector2 ConvertWorldToScreenPoint(Vector3 worldPosition, Camera camera)
		{
			Vector2 screenPoint = camera.WorldToScreenPoint(worldPosition);
			screenPoint.y = Screen.height - screenPoint.y;
			return screenPoint;
		}

		public static Vector3 ConvertScreenToWorldPoint(Vector2 screenPoint, Camera camera)
		{
			Vector3 position = screenPoint;
			position.z = Mathf.Abs(camera.transform.position.z);
			return camera.ScreenToWorldPoint(position);
		}

		// http://answers.unity3d.com/questions/799616/unity-46-beta-19-how-to-convert-from-world-space-t.html
		public static Vector2 WorldToCanvasPosition(RectTransform canvas, Camera camera, Vector3 position)
		{
			Vector2 temp = camera.WorldToViewportPoint(position);
			temp.x *= canvas.sizeDelta.x;
			temp.y *= canvas.sizeDelta.y;
			temp.x -= canvas.sizeDelta.x * canvas.pivot.x;
			temp.y -= canvas.sizeDelta.y * canvas.pivot.y;
			return temp;
		}

		static private readonly float In2Cm = 1f / 2.54f;
		public static float GetScreenPixelsPerCentimeter()
		{
			float dpi = Screen.dpi;
			if (dpi == 0)
			{
		#if UNITY_ANDROID
				dpi = 160f;
		#elif UNITY_IOS
				dpi = 326f;
		#else
				dpi = 72f;
		#endif
			}
			return dpi * In2Cm;
		}

		public static float DistancePointLine(Vector2 p, Vector2 a, Vector2 b)
		{
			Vector2 n = b - a;
			Vector2 pa = a - p;
			Vector2 c = n * (Vector2.Dot(pa, n) / Vector2.Dot(n, n));
			Vector2 d = pa - c;
			return Mathf.Sqrt(Vector2.Dot(d, d));
		}

		public static bool IntersectRayAABB(Ray2 ray, AABB2 aabb, ref float tmin, ref Vector2 q)
		{
			tmin = 0f;
			float tmax = float.MaxValue;

			for (int i = 0; i < 2; ++i)
			{
				if (Mathf.Abs(ray.direction[i]) < Mathf.Epsilon)
				{
					if (ray.origin[i] < aabb.min[i] || ray.origin[i] > aabb.max[i])
					{
						return false;
					}
				}
				else
				{
					float ood = 1f / ray.direction[i];
					float t1 = (aabb.min[i] - ray.origin[i]) * ood;
					float t2 = (aabb.max[i] - ray.origin[i]) * ood;
					if (t1 > t2)
					{
						float t = t1; t1 = t2; t2 = t;
					}

					tmin = Mathf.Max(tmin, t1);
					tmax = Mathf.Min(tmax, t2);
					if (tmin > tmax)
					{
						return false;
					}
				}
			}

			q = ray.origin + ray.direction * tmin;
			return true;
		}

		public static float TweenCosine(float from, float to, float value)
		{
			float t = (1f - Mathf.Cos(value * Mathf.PI)) / 2f;
			return (from * (1f - t) + to * t);
		}

		// https://stackoverflow.com/questions/29643352/converting-hex-to-rgb-value-in-python
		static private readonly float ItoF = 1f / 255f;
		public static Color ConvertHEXtoRGB(string hex, Color defaultColor)
		{
			if (string.IsNullOrEmpty(hex))
			{
				return defaultColor;
			}

			hex = hex.Trim();

			if (hex.IndexOf('#') == 0)
			{
				hex = hex.Remove(0, 1);
			}

			if (hex.Length != 6)
			{
				return defaultColor;
			}

			try
			{
				int r = Convert.ToInt32(hex.Substring(0, 2), 16);
				int g = Convert.ToInt32(hex.Substring(2, 2), 16);
				int b = Convert.ToInt32(hex.Substring(4, 2), 16);
				return new Color(r * ItoF, g * ItoF, b * ItoF);
			}
			catch (Exception)
			{
				return defaultColor;
			}
		}

		public static string ConvertRGBtoHEX(Color color)
		{
			int r = (int)Mathf.Round(color.r * 255f);
			int g = (int)Mathf.Round(color.g * 255f);
			int b = (int)Mathf.Round(color.b * 255f);
			return string.Format("{0:X2}{1:X2}{2:X2}", r, g, b);
		}

		public static void DumpSystemInfo()
		{
			Debug.Log("BatteryLevel: " + SystemInfo.batteryLevel);
			Debug.Log("BatteryStatus: " + SystemInfo.batteryStatus);
			Debug.Log("CopyTextureSupport: " + SystemInfo.copyTextureSupport);
			Debug.Log("DeviceModel: " + SystemInfo.deviceModel);
			Debug.Log("DeviceName: " + SystemInfo.deviceName);
			Debug.Log("DeviceType: " + SystemInfo.deviceType);
			Debug.Log("DeviceUniqueIdentifier: " + SystemInfo.deviceUniqueIdentifier);
			Debug.Log("GraphicsDeviceID: " + SystemInfo.graphicsDeviceID);
			Debug.Log("GraphicsDeviceName: " + SystemInfo.graphicsDeviceName);
			Debug.Log("GraphicsDeviceType: " + SystemInfo.graphicsDeviceType);
			Debug.Log("GraphicsDeviceVendor: " + SystemInfo.graphicsDeviceVendor);
			Debug.Log("GraphicsDeviceVendorID: " + SystemInfo.graphicsDeviceVendorID);
			Debug.Log("GraphicsDeviceVersion: " + SystemInfo.graphicsDeviceVersion);
			Debug.Log("GraphicsMemorySize: " + SystemInfo.graphicsMemorySize);
			Debug.Log("GraphicsMultiThreaded: " + SystemInfo.graphicsMultiThreaded);
			Debug.Log("GraphicsShaderLevel: " + SystemInfo.graphicsShaderLevel);
			Debug.Log("GraphicsUVStartsAtTop: " + SystemInfo.graphicsUVStartsAtTop);
			Debug.Log("HasDynamicUniformArrayIndexingInFragmentShaders: " + SystemInfo.hasDynamicUniformArrayIndexingInFragmentShaders);
			Debug.Log("HasHiddenSurfaceRemovalOnGPU: " + SystemInfo.hasHiddenSurfaceRemovalOnGPU);
			Debug.Log("MaxCubemapSize: " + SystemInfo.maxCubemapSize);
			Debug.Log("MaxTextureSize: " + SystemInfo.maxTextureSize);
			Debug.Log("NpotSupport: " + SystemInfo.npotSupport);
			Debug.Log("OperatingSystem: " + SystemInfo.operatingSystem);
			Debug.Log("OperatingSystemFamily: " + SystemInfo.operatingSystemFamily);
			Debug.Log("ProcessorCount: " + SystemInfo.processorCount);
			Debug.Log("ProcessorFrequency: " + SystemInfo.processorFrequency);
			Debug.Log("ProcessorType: " + SystemInfo.processorType);
			Debug.Log("SupportedRenderTargetCount: " + SystemInfo.supportedRenderTargetCount);
			Debug.Log("Supports2DArrayTextures: " + SystemInfo.supports2DArrayTextures);
			Debug.Log("Supports32bitsIndexBuffer: " + SystemInfo.supports32bitsIndexBuffer);
			Debug.Log("Supports3DRenderTextures: " + SystemInfo.supports3DRenderTextures);
			Debug.Log("Supports3DTextures: " + SystemInfo.supports3DTextures);
			Debug.Log("SupportsAccelerometer: " + SystemInfo.supportsAccelerometer);
			Debug.Log("SupportsAsyncCompute: " + SystemInfo.supportsAsyncCompute);
			Debug.Log("SupportsAsyncGPUReadback: " + SystemInfo.supportsAsyncGPUReadback);
			Debug.Log("SupportsAudio: " + SystemInfo.supportsAudio);
			Debug.Log("SupportsComputeShaders: " + SystemInfo.supportsComputeShaders);
			Debug.Log("SupportsCubemapArrayTextures: " + SystemInfo.supportsCubemapArrayTextures);
			Debug.Log("SupportsGPUFence: " + SystemInfo.supportsGPUFence);
			Debug.Log("SupportsGyroscope: " + SystemInfo.supportsGyroscope);
			Debug.Log("SupportsHardwareQuadTopology: " + SystemInfo.supportsHardwareQuadTopology);
			Debug.Log("SupportsImageEffects: " + SystemInfo.supportsImageEffects);
			Debug.Log("SupportsInstancing: " + SystemInfo.supportsInstancing);
			Debug.Log("SupportsLocationService: " + SystemInfo.supportsLocationService);
			Debug.Log("SupportsMipStreaming: " + SystemInfo.supportsMipStreaming);
			Debug.Log("SupportsMotionVectors: " + SystemInfo.supportsMotionVectors);
			Debug.Log("SupportsMultisampleAutoResolve: " + SystemInfo.supportsMultisampleAutoResolve);
			Debug.Log("SupportsMultisampledTextures: " + SystemInfo.supportsMultisampledTextures);
			Debug.Log("SupportsRawShadowDepthSampling: " + SystemInfo.supportsRawShadowDepthSampling);
			Debug.Log("SupportsRenderToCubemap: " + SystemInfo.supportsRenderToCubemap);
			Debug.Log("SupportsSeparatedRenderTargetsBlend: " + SystemInfo.supportsSeparatedRenderTargetsBlend);
			Debug.Log("SupportsShadows: " + SystemInfo.supportsShadows);
			Debug.Log("SupportsSparseTextures: " + SystemInfo.supportsSparseTextures);
			Debug.Log("SupportsTextureWrapMirrorOnce: " + SystemInfo.supportsTextureWrapMirrorOnce);
			Debug.Log("SupportsVibration: " + SystemInfo.supportsVibration);
			Debug.Log("SystemMemorySize: " + SystemInfo.systemMemorySize);
			Debug.Log("UnsupportedIdentifier: " + SystemInfo.unsupportedIdentifier);
			Debug.Log("UsesReversedZBuffer: " + SystemInfo.usesReversedZBuffer);
		}
	}
}
