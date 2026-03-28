import os

file_path = r"c:\Unity\LATEST\Minionsparadise\Assets\Scripts\Assembly-CSharp\Kampai\Game\ConfigurationDefinition.cs"

with open(file_path, 'r') as f:
    lines = f.readlines()

# Re-inserting missing properties
# We lost properties around line 15-20 of the current file.
# Current file around line 13:
# 13: 		[global::Newtonsoft.Json.JsonProperty(NullValueHandling = global::Newtonsoft.Json.NullValueHandling.Ignore)]
# 14: 		public bool serverPushNotifications { get; set; }
# 15: 
# 16: 		[global::Newtonsoft.Json.JsonProperty(NullValueHandling = global::Newtonsoft.Json.NullValueHandling.Ignore)]
# 17: 		public int msHeartbeat { get; set; }

# Original:
# 14: 		public bool serverPushNotifications { get; set; }
# 15: 
# 16: 		[global::Newtonsoft.Json.JsonProperty(NullValueHandling = global::Newtonsoft.Json.NullValueHandling.Ignore)]
# 17: 		public bool AprilsFool { get; set; }
# 18: 
# 19: 		[global::Newtonsoft.Json.JsonProperty(NullValueHandling = global::Newtonsoft.Json.NullValueHandling.Ignore)]
# 20: 		public float minimumVersion { get; set; }
# 21: 
# 22: 		[global::Newtonsoft.Json.JsonProperty(NullValueHandling = global::Newtonsoft.Json.NullValueHandling.Ignore)]
# 23: 		[global::Kampai.Util.Deserializer("ReaderUtil.ReadRateAppTriggerConfig")]
# 24: 		public global::System.Collections.Generic.Dictionary<global::Kampai.Game.ConfigurationDefinition.RateAppAfterEvent, bool> rateAppAfter { get; set; }
# 25: 
# 26: 		[global::Newtonsoft.Json.JsonProperty(NullValueHandling = global::Newtonsoft.Json.NullValueHandling.Ignore)]
# 27: 		public bool? night { get; set; }    <-- New
# 28: 
# 29: 		[global::Newtonsoft.Json.JsonProperty(NullValueHandling = global::Newtonsoft.Json.NullValueHandling.Ignore)]
# 30: 		public int maxRPS { get; set; }

new_properties = """		public bool AprilsFool { get; set; }

		[global::Newtonsoft.Json.JsonProperty(NullValueHandling = global::Newtonsoft.Json.NullValueHandling.Ignore)]
		public float minimumVersion { get; set; }

		[global::Newtonsoft.Json.JsonProperty(NullValueHandling = global::Newtonsoft.Json.NullValueHandling.Ignore)]
		[global::Kampai.Util.Deserializer("ReaderUtil.ReadRateAppTriggerConfig")]
		public global::System.Collections.Generic.Dictionary<global::Kampai.Game.ConfigurationDefinition.RateAppAfterEvent, bool> rateAppAfter { get; set; }

		[global::Newtonsoft.Json.JsonProperty(NullValueHandling = global::Newtonsoft.Json.NullValueHandling.Ignore)]
		public bool? night { get; set; }

		[global::Newtonsoft.Json.JsonProperty(NullValueHandling = global::Newtonsoft.Json.NullValueHandling.Ignore)]
		public int maxRPS { get; set; }

		[global::Newtonsoft.Json.JsonProperty(NullValueHandling = global::Newtonsoft.Json.NullValueHandling.Ignore)]
		public global::System.Collections.Generic.List<global::Kampai.Game.KillSwitch> killSwitches { get; set; }

"""

# Find where to insert. Find 'serverPushNotifications { get; set; }'
insertion_point = -1
for i, line in enumerate(lines):
    if 'public bool serverPushNotifications { get; set; }' in line:
        insertion_point = i + 2 # Skip property and empty line
        break

if insertion_point != -1:
    # Remove everything until 'msHeartbeat'
    end_point = -1
    for i in range(insertion_point, len(lines)):
        if 'public int msHeartbeat { get; set; }' in lines[i]:
            end_point = i - 1 # Keep the JsonProperty attribute if possible, but actually we should just replace everything between.
            # Wait, currently the JsonProperty is right before msHeartbeat.
            break
    
    if end_point != -1:
        del lines[insertion_point:end_point]
        lines.insert(insertion_point, new_properties)

with open(file_path, 'w') as f:
    f.writelines(lines)
