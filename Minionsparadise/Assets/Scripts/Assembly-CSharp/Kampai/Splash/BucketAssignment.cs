namespace Kampai.Splash
{
	public class BucketAssignment
	{
		public int BucketId { get; set; }

		public float Time { get; set; }

		public BucketAssignment()
		{
		}

		public BucketAssignment(int bucketId, float time)
		{
			BucketId = bucketId;
			Time = time;
		}
	}
}
