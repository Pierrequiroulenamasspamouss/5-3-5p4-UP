using Elevation.Logging;
using Kampai.Util;

namespace Kampai.Common
{
    public class SimpleLogTelemetrySender : ITelemetrySender
    {
        private IKampaiLogger logger = LogManager.GetClassLogger("SimpleLogTelemetrySender") as IKampaiLogger;

        public void SendEvent(TelemetryEvent gameEvent)
        {
            string parameters = "";
            if (gameEvent.Parameters != null)
            {
                foreach (var p in gameEvent.Parameters)
                {
                    parameters += string.Format("\n  {0}: {1}", p.name, p.value);
                }
            }
            logger.Info("TELEMETRY EVENT: {0} {1}", gameEvent.Type, parameters);
        }

        public void COPPACompliance()
        {
            logger.Info("TELEMETRY: COPPA Compliance triggered");
        }

        public void SharingUsage(bool enabled)
        {
            logger.Info("TELEMETRY: Sharing Usage: {0}", enabled);
        }
    }
}
