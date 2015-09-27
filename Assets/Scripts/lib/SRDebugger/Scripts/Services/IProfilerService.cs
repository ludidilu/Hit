using CircularBuffer;

namespace SRDebugger.Services
{

	public struct ProfilerFrame
	{

		public double FrameTime;

		public double UpdateTime;
		public double RenderTime;
		public double OtherTime;

	}

	public interface IProfilerService
	{

		float AverageFrameTime { get; }
		float LastFrameTime { get; }

		CircularBuffer<ProfilerFrame> FrameBuffer { get; } 

	}

}