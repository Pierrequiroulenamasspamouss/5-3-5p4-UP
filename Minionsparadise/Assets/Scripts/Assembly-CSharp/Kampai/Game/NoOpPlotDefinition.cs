namespace Kampai.Game
{
	internal sealed class NoOpPlotDefinition : global::Kampai.Game.PlotDefinition, global::Kampai.Util.IBuilder<global::Kampai.Game.Instance>
	{
		public override int TypeCode
		{
			get
			{
				return 1127;
			}
		}

		public global::Kampai.Game.Instance Build()
		{
			return new global::Kampai.Game.NoOpPlot(this);
		}

		public override global::Kampai.Game.Plot Instantiate()
		{
			return new global::Kampai.Game.NoOpPlot(this);
		}
	}
}
