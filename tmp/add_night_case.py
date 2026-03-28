import os

file_path = r"c:\Unity\LATEST\Minionsparadise\Assets\Scripts\Assembly-CSharp\Kampai\Game\ConfigurationDefinition.cs"

with open(file_path, 'r') as f:
    lines = f.readlines()

new_case = """			case "NIGHT":
				reader.Read();
				night = global::System.Convert.ToBoolean(reader.Value);
				break;
"""

def add_case_to_switch(lines, search_text):
    indices = [i for i, s in enumerate(lines) if search_text in s]
    # We want to add it AFTER the 'break;' of the search_text case.
    for index in reversed(indices):
        # Find the 'break;' line
        for j in range(index, len(lines)):
            if 'break;' in lines[j]:
                lines.insert(j + 1, new_case)
                break
    return lines

lines = add_case_to_switch(lines, 'case "RATEAPPAFTER":')

with open(file_path, 'w') as f:
    f.writelines(lines)
