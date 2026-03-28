import os

file_path = r"c:\Unity\LATEST\Minionsparadise\Assets\Scripts\Assembly-CSharp\Kampai\Main\MainCompleteCommand.cs"

with open(file_path, 'r') as f:
    lines = f.readlines()

new_block = [
    '\t\t\tlogger.EventStart("MainCompleteCommand.LoadUI");\n',
    '\t\t\tautoSavePlayerSignal.Dispatch();\n',
    '\t\t\treloadConfigs.Dispatch();\n',
    '\t\t\t// --- Day/Night Cycle Initialization ---\n',
    '\t\t\tglobal::UnityEngine.GameObject dnManagerGo = new global::UnityEngine.GameObject("DayNightCycleManager");\n',
    '\t\t\tglobal::Kampai.Game.DayNightCycleManager dnManager = dnManagerGo.AddComponent<global::Kampai.Game.DayNightCycleManager>();\n',
    '\t\t\tbase.injectionBinder.injector.Inject(dnManager);\n',
    '\t\t\tglobal::UnityEngine.Object.DontDestroyOnLoad(dnManagerGo);\n'
]

# Find the insertion point: search for 'logger.EventStart("MainCompleteCommand.LoadUI")'
# Actually, it might have been deleted, so let's find 'global::Kampai.Util.TimeProfiler.EndSection("loading game scene");'
insertion_point = -1
for i, line in enumerate(lines):
    if 'global::Kampai.Util.TimeProfiler.EndSection("loading game scene")' in line:
        insertion_point = i + 1
        break

if insertion_point != -1:
    # Check if 'LoadUI' or 'autoSavePlayerSignal' already exist further down
    # In case I already partially modified it.
    # Current state shows 'clientHealthService.MarkMeterEvent("AppFlow.AppStart");' follows.
    end_point = -1
    for i in range(insertion_point, len(lines)):
        if 'clientHealthService.MarkMeterEvent("AppFlow.AppStart")' in lines[i]:
            end_point = i
            break
    
    if end_point != -1:
        del lines[insertion_point:end_point]
        for line in reversed(new_block):
            lines.insert(insertion_point, line)

with open(file_path, 'w') as f:
    f.writelines(lines)
